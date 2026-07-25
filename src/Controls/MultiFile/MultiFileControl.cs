// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.MultiFile
{
    /// <inheritdoc/>
    internal sealed class MultiFileControl : BaseControlPrompt<FileItem[]>, IMultiFileControl
    {
        // Swappable so tests can run against a MockFileSystem instead of the real disk — see
        // FileControl.FileSystem (FileExec/FileControl.cs) for the same pattern and rationale.
        internal static IFileSystem FileSystem { get; set; } = new FileSystem();

        /// <summary>
        /// Total rows the control template reserves around the tree list: prompt+answer line,
        /// optional description line, tooltip line and an extra row for the pagination footer.
        /// </summary>
        private const int ReservedTemplateLines = 7;

        // Cached composite format strings for improved performance.
        private static readonly System.Text.CompositeFormat s_minSelectionFormat = CompositeFormat.Parse(PromptPlusResources.MultiSelectMinSelection);
        private static readonly System.Text.CompositeFormat s_maxSelectionFormat = CompositeFormat.Parse(PromptPlusResources.MultiSelectMaxSelection);
        private static readonly System.Text.CompositeFormat s_countCheckFormat = CompositeFormat.Parse(PromptPlusResources.TooltipCountCheck);

        // Byte-size unit suffixes (static so FormatSize does not allocate a new array per call/frame).
        private static readonly string[] s_sizeUnits = ["KB", "MB", "GB", "TB", "PB"];

        /// <summary>
        /// A single visible node of the tree. Children are materialized lazily (only when the node
        /// is expanded) and released on collapse, so memory stays proportional to what is visible.
        /// The checked state is NOT stored here (it would be lost on collapse); it is tracked in
        /// <see cref="_checkedItems"/> keyed by full path so it survives expand/collapse cycles.
        /// </summary>
        private sealed class Node(string uniqueId, string fullPath, string name, bool isDirectory, int depth, bool isLast)
        {
            public string UniqueId { get; } = uniqueId;
            public string FullPath { get; } = fullPath;
            public string Name { get; } = name;
            public bool IsDirectory { get; } = isDirectory;
            public int Depth { get; } = depth;
            public bool IsLast { get; set; } = isLast;
            public bool IsExpanded { get; set; }
            public long Length { get; set; }
            public DateTime LastWriteTime { get; set; }
            public bool IsRoot { get; init; }
            public long AncestorMask;
        }

        private readonly Dictionary<MultiFileStyles, Style> _optStyles;
        // Flat list of currently visible nodes (memory-conscious: only the visible/expanded slice).
        private readonly List<Node> _nodes = [];
        // Fast path -> visible-node-index map, rebuilt whenever the visible slice changes (in
        // RecomputeAncestorMasks). Replaces repeated O(n) List.IndexOf scans with O(1) lookups.
        private readonly Dictionary<string, int> _indexByPath = new(PathComparer);
        // Cached folder tri-state (0=none,1=all,2=partial) keyed by full path. Populated lazily on
        // render and invalidated whenever the checked set or the visible node slice changes, so the
        // hot render path never recomputes the descendant scan for unchanged folders.
        private readonly Dictionary<string, int> _folderStateCache = new(PathComparer);
        // Checked entries keyed by full path (case-insensitive). Persists across collapse/expand.
        private readonly Dictionary<string, FileItem> _checkedItems = new(PathComparer);
        private Paginator<Node>? _localpaginator;

        private string _root = FileSystem.Directory.GetCurrentDirectory();
        private string _searchPattern = "*";
        private bool _onlyFolders;
        private bool _showHidden;
        private bool _showSystem;
        private bool _hideSize;
        private bool _selectFilesOnly;
        private bool _showFullPath;
        private bool _cascadeCheck = true;
        private bool _recursiveMarkWithCtrlSpace;
        private byte _pageSize;
        private int _effectivePageSize;
        private int _sequence;
        private IEnumerable<string> _defaultFullPaths = [];
        private bool _useDefaultHistory = true;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;
        private int _maxSelect = int.MaxValue;
        private int _minSelect;

        // Optional predicate that decides whether a FileItem may be checked. Only one of the sync/
        // async variants is active at a time (setting one clears the other). Individual toggles surface
        // the returned message as an error; mass selections (recursive/wildcard/all) silently skip
        // rejected items (Option C).
        private Func<FileItem, (bool, string?)>? _predicatevalidcheck;
        private Func<FileItem, Task<(bool, string?)>>? _predicatevalidcheckAsync;

        private string[] _toggerTooptips = [];
        private int _indexTooptip;

        // "Filter only selected" view (toggled by ConfigPrompt.HotKeyFilterAllSelected). When active,
        // the tree is temporarily replaced by a flat list of the currently-checked items. The real
        // tree slice is snapshotted in _savedNodes and restored untouched when the filter is turned
        // off, so this feature never mutates the normal browsing state.
        private bool _filterOnlySelected;
        // Snapshot of the checked count/filter-only-selected flag used the last time the tooltip
        // strings were built. When either transitions, the "filter all selected" hint must be
        // added/removed, so we rebuild the tooltip cache lazily on the next frame.
        private int _lastCountCheckedTooltip = -1;
        private bool _lastFilterOnlySelectedTooltip;
        private List<Node>? _savedNodes;
        private int _savedIndex;

        // Option B: background (cancelable) recursive wildcard selection over a folder subtree.
        // The expensive disk enumeration runs off the UI thread; the model is only mutated when
        // the task finishes, so the live tree stays untouched (and race-free) while it runs.
        // Multiple operations can run concurrently — one per folder — and each is tracked and
        // cancelled independently (keyed by the target folder's full path).
        private CancellationToken _lifetimeToken;
        private readonly Dictionary<string, BackgroundOp> _bgOps = new(PathComparer);

        /// <summary>
        /// A single independently-managed background wildcard operation over one folder subtree.
        /// The expensive disk enumeration runs off the UI thread and fills <see cref="Result"/>;
        /// the model is only mutated (on the UI thread) once <see cref="Completed"/> flips true.
        /// </summary>
        private sealed class BackgroundOp
        {
            public required string TargetPath { get; init; }
            public required bool Check { get; init; }
            public required CancellationTokenSource Cts { get; init; }
            public Task? Task;
            public volatile bool Completed;
            // Written by the background task, read by the UI thread. Accessed via Volatile.Read/
            // Volatile.Write so the completed enumeration snapshot is guaranteed to be visible to
            // the UI thread once Completed is observed as true.
            private List<FileItem>? _result;
            public List<FileItem>? Result
            {
                get => Volatile.Read(ref _result);
                set => Volatile.Write(ref _result, value);
            }
        }

        // Internal wake-up signals (keychar 1 / ConsoleKey.None) used to repaint / resume TryResult
        // while / after the background task runs, mirroring the pattern used by TaskControl.
        // Finished => Shift+Control; Tick => Control only (distinct so they can be told apart).
        private static readonly ConsoleKeyInfo s_finishedWakeUp = new((char)1, ConsoleKey.None, true, false, true);
        private static readonly ConsoleKeyInfo s_tickWakeUp = new((char)1, ConsoleKey.None, false, false, true);

        public MultiFileControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<MultiFileStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
        }

        #region IMultiFileControl

        /// <inheritdoc/>
        public IMultiFileControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl Styles(MultiFileStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl Root(string path)
        {
            ArgumentNullException.ThrowIfNull(path);
            _root = path;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl SearchPattern(string pattern)
        {
            ArgumentNullException.ThrowIfNull(pattern);
            _searchPattern = string.IsNullOrWhiteSpace(pattern) ? "*" : pattern;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl OnlyFolders(bool value = true)
        {
            _onlyFolders = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl ShowHidden(bool value = true)
        {
            _showHidden = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl ShowSystem(bool value = true)
        {
            _showSystem = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl HideSize(bool value = true)
        {
            _hideSize = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl PageSize(byte value)
        {
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl SelectFilesOnly(bool value = true)
        {
            _selectFilesOnly = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl ShowFullPath(bool value = true)
        {
            _showFullPath = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl CascadeCheck(bool value = true)
        {
            _cascadeCheck = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl RecursiveMarkWithCtrlSpace(bool value = true)
        {
            _recursiveMarkWithCtrlSpace = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl Range(int minvalue, int? maxvalue = null)
        {
            if (minvalue > (maxvalue ?? int.MaxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(minvalue), $"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minSelect = minvalue;
            _maxSelect = maxvalue ?? int.MaxValue;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl PredicateChecked(Func<FileItem, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = validselect;
            _predicatevalidcheckAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl PredicateChecked(Func<FileItem, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = (input) => (validselect(input), (string?)null);
            _predicatevalidcheckAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl PredicateCheckedAsync(Func<FileItem, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = validselect;
            _predicatevalidcheck = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl PredicateCheckedAsync(Func<FileItem, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidcheck = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl Default(IEnumerable<string> fullPaths, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(fullPaths);
            _defaultFullPaths = fullPaths;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        /// <inheritdoc/>
        public IMultiFileControl EnableHistory(string filename, Action<IHistoryOptions>? options = null)
        {
            ArgumentNullException.ThrowIfNull(filename);
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Filename cannot be empty or whitespace.", nameof(filename));
            }
            _historyOptions = new HistoryOptions(filename);
            options?.Invoke(_historyOptions);
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (!FileSystem.Directory.Exists(_root))
            {
                throw new DirectoryNotFoundException($"Root directory not found: {_root}");
            }
            _root = FileSystem.Path.GetFullPath(_root);

            _lifetimeToken = cancellationToken;

            _nodes.Clear();
            _checkedItems.Clear();
            _sequence = 0;
            _filterOnlySelected = false;
            _savedNodes = null;
            _savedIndex = 0;

            // Resolve the checked targets: history values (if enabled) can override the explicit Default.
            IEnumerable<string> targets = _defaultFullPaths;
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0
                    && TryDeserializeHistoryValue(_itemHistories[0].History, out string[] historyValues)
                    && historyValues.Length > 0)
                {
                    targets = historyValues;
                }
            }

            // Pre-check each valid target (files or directories under the root). The selection
            // predicate (if any) is honored here too: rejected targets are silently skipped so the
            // initial checked set never contains items the predicate would forbid.
            string? firstTarget = null;
            foreach (string target in targets)
            {
                if (TryResolveCheckable(target, out string full, out bool isDirectory))
                {
                    FileItem candidate = BuildFileItem(full, isDirectory);
                    if (!TryValidatePredicate(candidate, out _))
                    {
                        continue;
                    }
                    firstTarget ??= full;
                    _checkedItems[full] = candidate;
                }
            }

            // The root node starts expanded so its immediate children are visible.
            Node rootNode = new(NextId(), _root, _root, isDirectory: true, depth: 0, isLast: true) { IsRoot = true, IsExpanded = true };
            _nodes.Add(rootNode);
            InsertChildren(0);

            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            RebuildPaginator(selectFirst: true);

            // Expand the tree down to the first checked target (if any) and select it.
            ExpandToTarget(firstTarget);

            LoadTooltipToggle();
        }

        /// <summary>
        /// Resolves a candidate path to a checkable entry: it must exist (file or directory) and lie
        /// under the root. Files-only mode rejects directories.
        /// </summary>
        private bool TryResolveCheckable(string? target, out string full, out bool isDirectory)
        {
            full = string.Empty;
            isDirectory = false;
            if (string.IsNullOrEmpty(target))
            {
                return false;
            }
            try
            {
                full = FileSystem.Path.GetFullPath(target);
            }
            catch
            {
                return false;
            }
            bool dir = FileSystem.Directory.Exists(full);
            bool file = FileSystem.File.Exists(full);
            if (!dir && !file)
            {
                return false;
            }
            if (!IsPathUnderRoot(full, _root)
                && !PathEquals(full, _root))
            {
                return false;
            }
            if (_selectFilesOnly && dir)
            {
                return false;
            }
            isDirectory = dir;
            return true;
        }

        /// <summary>
        /// Expands each ancestor folder from the root down to <paramref name="target"/> (lazily, one
        /// level at a time) and selects the matching node. Only paths under the root are honored.
        /// </summary>
        private void ExpandToTarget(string? target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return;
            }

            string full;
            try
            {
                full = FileSystem.Path.GetFullPath(target);
            }
            catch
            {
                return;
            }
            if ((!FileSystem.File.Exists(full) && !FileSystem.Directory.Exists(full))
                || !IsPathUnderRoot(full, _root))
            {
                return;
            }

            int maxSteps = CountSeparators(full) + 1;
            for (int step = 0; step <= maxSteps; step++)
            {
                int matchIndex = IndexOfPath(full);
                if (matchIndex >= 0)
                {
                    _localpaginator!.EnsureVisibleIndex(matchIndex);
                    return;
                }

                int ancestorIndex = DeepestCollapsedAncestor(full);
                if (ancestorIndex < 0)
                {
                    return;
                }
                Expand(ancestorIndex);
                RebuildPaginator(selectFirst: false);
            }
        }

        private int IndexOfPath(string full)
            => _indexByPath.TryGetValue(full, out int index) && index < _nodes.Count
               && PathEquals(_nodes[index].FullPath, full)
                ? index
                : -1;

        /// <summary>
        /// Resolves the visible index of <paramref name="node"/> via the path -> index map (O(1)),
        /// falling back to a linear scan only if the map is momentarily out of sync.
        /// </summary>
        private int NodeIndex(Node node)
        {
            if (_indexByPath.TryGetValue(node.FullPath, out int index)
                && index < _nodes.Count
                && ReferenceEquals(_nodes[index], node))
            {
                return index;
            }
            return _nodes.IndexOf(node);
        }

        private int FindNodeIndexStartingWith(string prefix, int startIndex)
        {
            for (int i = Math.Max(0, startIndex); i < _nodes.Count; i++)
            {
                if (_nodes[i].Name.StartsWith(prefix, NameComparison))
                {
                    return i;
                }
            }
            return -1;
        }

        private int DeepestCollapsedAncestor(string full)
        {
            int bestIndex = -1;
            int bestLen = -1;
            for (int i = 0; i < _nodes.Count; i++)
            {
                Node n = _nodes[i];
                if (n.IsDirectory && !n.IsExpanded
                    && IsPathUnderRoot(full, n.FullPath)
                    && n.FullPath.Length > bestLen)
                {
                    bestIndex = i;
                    bestLen = n.FullPath.Length;
                }
            }
            return bestIndex;
        }

        private static int CountSeparators(string path)
        {
            int count = 0;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == Path.DirectorySeparatorChar || path[i] == Path.AltDirectorySeparatorChar)
                {
                    count++;
                }
            }
            return count;
        }

        private static string EnsureTrailingSeparator(string path)
            => path.EndsWith(Path.DirectorySeparatorChar)
               || path.EndsWith(Path.AltDirectorySeparatorChar)
                ? path
                : path + Path.DirectorySeparatorChar;

        private static StringComparison PathComparison
            => OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        private static StringComparer PathComparer
            => OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        private static StringComparison NameComparison
            => OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        private static bool PathEquals(string left, string right)
            => string.Equals(
                left.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                right.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                PathComparison);

        private static bool IsPathUnderRoot(string fullPath, string rootPath)
            => EnsureTrailingSeparator(fullPath)
                .StartsWith(EnsureTrailingSeparator(rootPath), PathComparison);

        private static bool IsUnixLike
            => OperatingSystem.IsLinux() || OperatingSystem.IsMacOS();

        private static bool IsUnixHiddenByName(string path)
        {
            string name = Path.GetFileName(path);
            return !string.IsNullOrEmpty(name) && name[0] == '.';
        }

        private bool ShouldSkipHiddenEntry(string path)
            => !_showHidden && IsUnixLike && IsUnixHiddenByName(path);

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<FileItem[]>([], true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;
                    Node? selected = _localpaginator!.SelectedItem;

                    // While one or more background wildcard operations run, the UI stays fully
                    // interactive: the user can keep navigating/expanding/checking and even launch
                    // more background operations on other folders. Each op is tracked and cancelled
                    // independently. Only a few events are handled specially here; everything else
                    // falls through to the normal key handling below.
                    if (IsBackgroundActive)
                    {
                        // 1) Any finished operation(s): apply their results to the model on the UI
                        //    thread (woken by s_finishedWakeUp from WaitKeypress, or detected here).
                        if (HasCompletedBackgroundOp() || IsFinishedWakeUp(keyinfo))
                        {
                            ApplyCompletedBackgroundOps();
                            SetRangeValidationErrorIfNeeded();
                            _indexTooptip = 0;
                            break;
                        }
                        // 2) Re-pressing the recursive-mark key (space, or Ctrl+Space when the
                        //    recursive action is bound to it) on a folder that has a running
                        //    operation cancels just that operation and leaves the model unchanged.
                        if ((_recursiveMarkWithCtrlSpace ? keyinfo.IsPressCtrlSpaceKey() : keyinfo.IsPressSpaceKey())
                            && selected is { IsDirectory: true }
                            && _bgOps.ContainsKey(selected.FullPath))
                        {
                            CancelBackgroundWildcard(selected.FullPath);
                            _indexTooptip = 0;
                            break;
                        }
                        // 3) Internal repaint tick (no real key was pending): throttle a little and
                        //    repaint so the wait glyph animates without a busy spin. A real key
                        //    interrupts the throttle immediately (handled above / below).
                        if (IsTickWakeUp(keyinfo))
                        {
                            cancellationToken.WaitHandle.WaitOne(80);
                            break;
                        }
                        // Any other real key: DO NOT swallow it. Let it flow into the normal
                        // handling below so navigation/checking stays responsive (UI is not blocked).
                    }

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<FileItem[]>([], true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        if (_checkedItems.Count < _minSelect)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_minSelectionFormat, _minSelect));
                            break;
                        }
                        if (_checkedItems.Count > _maxSelect)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_maxSelectionFormat, _maxSelect));
                            break;
                        }
                        ResultCtrl = new ResultPrompt<FileItem[]>(BuildResult(), false);
                        SaveHistory();
                        break;
                    }
                    else if (keyinfo.IsPressSpaceKey() && selected != null && !selected.IsRoot)
                    {
                        // Space toggles the checked state of the selected file/folder.
                        _indexTooptip = 0;
                        if (_selectFilesOnly && selected.IsDirectory)
                        {
                            SetError(PromptPlusResources.SelectionDisabled);
                            break;
                        }
                        // For a folder, space performs a recursive selection when CascadeCheck is enabled.
                        // When RecursiveMarkWithCtrlSpace is also enabled, plain space only toggles the
                        // selected entry itself (folder included) without recursing into its subtree.
                        // When CascadeCheck is disabled, always toggle only the single item.
                        if (!_cascadeCheck || _recursiveMarkWithCtrlSpace)
                        {
                            // Toggle single item only (no cascade).
                            ToggleCheckedWithPredicate(selected);
                            RefreshAncestorStates(selected);
                            SetRangeValidationErrorIfNeeded();
                        }
                        else if (selected.IsDirectory)
                        {
                            // Cascade is enabled and not moved to Ctrl+Space, so Space does recursive.
                            ToggleWildcardOnFolder(selected);
                        }
                        else
                        {
                            ToggleAndReconcile(selected);
                            SetRangeValidationErrorIfNeeded();
                        }
                        break;
                    }
                    else if (_cascadeCheck && _recursiveMarkWithCtrlSpace && keyinfo.IsPressCtrlSpaceKey()
                             && selected is { IsRoot: false })
                    {
                        // Ctrl+Space performs the recursive folder selection (background select /
                        // fast unselect) when cascade is enabled and that action was moved off plain space.
                        // For a file it simply toggles its checked state (no recursion applies).
                        _indexTooptip = 0;
                        if (_selectFilesOnly && selected.IsDirectory)
                        {
                            SetError(PromptPlusResources.SelectionDisabled);
                            break;
                        }
                        if (selected.IsDirectory)
                        {
                            ToggleWildcardOnFolder(selected);
                        }
                        else
                        {
                            ToggleCheckedWithPredicate(selected);
                            RefreshAncestorStates(selected);
                            SetRangeValidationErrorIfNeeded();
                        }
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    else if (ConfigPrompt.HotKeyToggleAll.Equals(keyinfo))
                    {
                        // Toggle the checked state of every currently visible checkable node.
                        ToggleAllVisible();
                        SetRangeValidationErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (ConfigPrompt.HotKeyFilterAllSelected.Equals(keyinfo))
                    {
                        // Toggle the "filter only selected" flat view. It is a no-op (kept silent)
                        // when trying to enable it with nothing checked, so nothing else is affected.
                        // This stays available while background wildcard operations run: the overlay
                        // is a view-layer snapshot and ApplyBackgroundOp is overlay-aware, so it is
                        // rebuilt from the updated checked set when a background op completes.
                        ToggleFilterOnlySelected();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (ConfigPrompt.HotKeyToggleFullPath.Equals(keyinfo))
                    {
                        _showFullPath = !_showFullPath;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressExpandKey()
                             && selected is { IsDirectory: true, IsExpanded: false })
                    {
                        Expand(_localpaginator.CurrentIndex);
                        RebuildPaginator(selectFirst: false);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCollapseKey()
                             && selected is { IsDirectory: true, IsExpanded: true })
                    {
                        Collapse(_localpaginator.CurrentIndex);
                        RebuildPaginator(selectFirst: false);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressTabKey() && selected != null)
                    {
                        int current = _localpaginator.CurrentIndex;
                        if (selected.IsDirectory)
                        {
                            if (!selected.IsExpanded)
                            {
                                Expand(current);
                                RebuildPaginator(selectFirst: false);
                            }
                            if (current + 1 < _nodes.Count && _nodes[current + 1].Depth > selected.Depth)
                            {
                                _localpaginator.EnsureVisibleIndex(current + 1);
                            }
                        }
                        else
                        {
                            if (_localpaginator.IsLastPageItem)
                            {
                                _localpaginator.NextPage(IndexOption.FirstItem);
                            }
                            else
                            {
                                _localpaginator.NextItem();
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressShiftTabKey() && selected != null)
                    {
                        int current = _localpaginator.CurrentIndex;
                        int parentIndex = ParentIndex(current);
                        bool isFirstChild = parentIndex >= 0 && current == parentIndex + 1;

                        if (isFirstChild)
                        {
                            _localpaginator.EnsureVisibleIndex(parentIndex);
                            if (_nodes[parentIndex].IsExpanded)
                            {
                                Collapse(parentIndex);
                                RebuildPaginator(selectFirst: false);
                            }
                        }
                        else
                        {
                            if (_localpaginator.IsFirstPageItem)
                            {
                                _localpaginator.PreviousPage(IndexOption.LastItem);
                            }
                            else
                            {
                                _localpaginator.PreviousItem();
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressDownArrowKey())
                    {
                        if (_localpaginator!.IsLastPageItem)
                        {
                            _localpaginator.NextPage(IndexOption.FirstItem);
                        }
                        else
                        {
                            _localpaginator.NextItem();
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem)
                        {
                            _localpaginator.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _localpaginator.PreviousItem();
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        _localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        _localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlHomeKey())
                    {
                        _localpaginator!.Home();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        _localpaginator!.End();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (TryAnswerViewportNavigation(keyinfo))
                    {
                        break;
                    }
                    else if (selected != null && !char.IsControl(keyinfo.KeyChar) && keyinfo.KeyChar != '\0')
                    {
                        string keyChar = keyinfo.KeyChar.ToString();
                        int start = _localpaginator!.CurrentIndex;
                        int index = FindNodeIndexStartingWith(keyChar, start + 1);
                        if (index < 0 && start >= 0)
                        {
                            index = FindNodeIndexStartingWith(keyChar, 0);
                        }
                        if (index >= 0)
                        {
                            _localpaginator.EnsureVisibleIndex(index);
                            _indexTooptip = 0;
                            break;
                        }
                    }
                }
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            int targetPageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            if (targetPageSize != _effectivePageSize)
            {
                _effectivePageSize = targetPageSize;
                _localpaginator?.UpdatePageSize(_effectivePageSize);
            }

            WritePrompt(screenBuffer, _optStyles[MultiFileStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteDescription(screenBuffer);
            WriteTreeList(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[MultiFileStyles.Error]);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[MultiFileStyles.Prompt]);
            string answer;
            if (ResultCtrl!.Value.IsAborted)
            {
                answer = OptionsControl.ShowMessageAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
            }
            else
            {
                answer = BuildCheckedItemsText();
            }
            screenBuffer.WriteLine(answer, _optStyles[MultiFileStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            CancelAllBackgroundWildcards();
            _nodes.Clear();
            _checkedItems.Clear();
        }

        #region checked state

        private void ToggleChecked(Node node)
        {
            if (_checkedItems.ContainsKey(node.FullPath))
            {
                _checkedItems.Remove(node.FullPath);
            }
            else
            {
                _checkedItems[node.FullPath] = ToFileItem(node);
            }
            InvalidateFolderStateChain(node.FullPath);
        }

        /// <summary>
        /// Toggles the checked state of any non-root <paramref name="node"/> (file or folder) and then
        /// reconciles ancestor folders so their tri-state stays coherent. When the node is a folder its
        /// whole subtree is expanded and set to the new state so the folder itself no longer renders as
        /// partial after the toggle.
        /// </summary>
        private void ToggleAndReconcile(Node node)
        {
            if (node.IsDirectory)
            {
                bool check = !_checkedItems.ContainsKey(node.FullPath) || GetFolderSelectionState(node) != 1;
                int index = NodeIndex(node);
                if (index < 0)
                {
                    return;
                }
                ExpandSubtree(index);
                RebuildPaginator(selectFirst: false);
                index = NodeIndex(node);
                ApplyCheckToSubtree(index, check);
            }
            else
            {
                ToggleCheckedWithPredicate(node);
            }
            // When a non-root item changes state, reconcile every ancestor folder so its stored
            // (fully-selected) membership matches the new descendant states.
            RefreshAncestorStates(node);
        }

        private void ToggleAllVisible()
        {
            var candidates = _nodes.Where(n => !n.IsRoot && (!_selectFilesOnly || !n.IsDirectory)).ToList();
            if (candidates.Count == 0)
            {
                return;
            }
            // If any candidate is unchecked, check them all; otherwise uncheck them all.
            bool check = candidates.Any(n => !_checkedItems.ContainsKey(n.FullPath));
            foreach (Node n in candidates)
            {
                if (check)
                {
                    // Mass selection: silently skip nodes rejected by the predicate (Option C).
                    FileItem candidate = ToFileItem(n);
                    if (!TryValidatePredicate(candidate, out _))
                    {
                        continue;
                    }
                    _checkedItems[n.FullPath] = candidate;
                }
                else
                {
                    _checkedItems.Remove(n.FullPath);
                }
            }
            // Every visible checkable node (folders included, unless files-only) now shares the same
            // state, so every visible folder is already fully-selected/none — ancestor reconciliation
            // would only recompute the same result. In files-only mode folders are not candidates, so
            // reconcile their tri-state once (single pass) to keep them coherent.
            if (_selectFilesOnly)
            {
                foreach (Node n in candidates)
                {
                    RefreshAncestorStates(n);
                }
            }
            _folderStateCache.Clear();
        }

        /// <summary>
        /// Toggles the "filter only selected" view. When turning it on, the current tree slice is
        /// snapshotted and the visible list is replaced by a flat, read-only list of the currently
        /// checked items; when turning it off, the original tree slice is restored exactly. Returns
        /// false (no-op) when trying to enable it with nothing checked, so existing behavior is never
        /// affected.
        /// </summary>
        private bool ToggleFilterOnlySelected()
        {
            if (!_filterOnlySelected)
            {
                if (_checkedItems.Count == 0)
                {
                    return false;
                }
                // Snapshot the live tree slice and current position so they can be restored intact.
                _savedNodes = [.. _nodes];
                _savedIndex = _localpaginator?.CurrentIndex ?? 0;

                LoadFilteredSelectedNodes();
                _filterOnlySelected = true;
                RebuildPaginator(selectFirst: true);
            }
            else
            {
                // Restore the original tree slice untouched.
                _nodes.Clear();
                if (_savedNodes != null)
                {
                    _nodes.AddRange(_savedNodes);
                }
                _savedNodes = null;
                _filterOnlySelected = false;
                RebuildPaginator(selectFirst: false);
                if (_savedIndex >= 0 && _savedIndex < _nodes.Count)
                {
                    _localpaginator!.EnsureVisibleIndex(_savedIndex);
                }
            }
            return true;
        }

        /// <summary>
        /// Rebuilds the visible node list as a flat, read-only list of the currently checked items
        /// (ordered by full path). Used by the "filter only selected" overlay both when it is first
        /// enabled and when it must be refreshed after a background operation mutates the checked set
        /// while the overlay is active.
        /// </summary>
        private void LoadFilteredSelectedNodes()
        {
            _nodes.Clear();
            foreach (FileItem item in _checkedItems.Values.OrderBy(x => x.FullPath, PathComparer))
            {
                _nodes.Add(new Node(NextId(), item.FullPath, item.Name, item.IsDirectory, depth: 0, isLast: true)
                {
                    Length = item.Length,
                    LastWriteTime = item.LastWriteTime
                });
            }
        }

        private void SetRangeValidationErrorIfNeeded()
        {
            if (_checkedItems.Count < _minSelect)
            {
                SetError(string.Format(CultureInfo.CurrentCulture, s_minSelectionFormat, _minSelect));
                return;
            }
            if (_checkedItems.Count > _maxSelect)
            {
                SetError(string.Format(CultureInfo.CurrentCulture, s_maxSelectionFormat, _maxSelect));
            }
        }

        /// <summary>
        /// Evaluates the optional selection predicate for <paramref name="item"/>. Returns <c>true</c>
        /// (with a <c>null</c> message) when no predicate is configured. The async variant is invoked
        /// synchronously (the control's key loop is synchronous).
        /// </summary>
        private bool TryValidatePredicate(FileItem item, out string? message)
        {
            if (_predicatevalidcheck == null && _predicatevalidcheckAsync == null)
            {
                message = null;
                return true;
            }
            (bool ok, string? validationMessage) = _predicatevalidcheckAsync != null
                ? _predicatevalidcheckAsync.Invoke(item).ConfigureAwait(false).GetAwaiter().GetResult()
                : (_predicatevalidcheck?.Invoke(item) ?? (true, null));
            message = validationMessage;
            return ok;
        }

        /// <summary>
        /// Individual toggle of a single node's checked state, honoring the selection predicate: when
        /// the node is about to be checked and the predicate rejects it, an error is surfaced and the
        /// state is left unchanged (Option C — individual toggles report, mass selections skip silently).
        /// Returns <c>true</c> when the toggle was applied.
        /// </summary>
        private bool ToggleCheckedWithPredicate(Node node)
        {
            bool willCheck = !_checkedItems.ContainsKey(node.FullPath);
            if (willCheck && !TryValidatePredicate(ToFileItem(node), out string? message))
            {
                SetError(string.IsNullOrEmpty(message) ? PromptPlusResources.PredicateSelectInvalid : message);
                return false;
            }
            ToggleChecked(node);
            return true;
        }

        /// <summary>
        /// Runs the recursive wildcard action on a folder <paramref name="folder"/>: if the whole
        /// subtree is already fully checked it is unselected in-memory (fast, no disk I/O); otherwise
        /// the recursive disk walk that discovers every descendant runs on a cancelable background
        /// task. Shared by the wildcard hotkey and the space key so both behave identically on folders.
        /// </summary>
        private void ToggleWildcardOnFolder(Node folder)
        {
            bool check = GetFolderSelectionState(folder) != 1;
            if (check)
            {
                // Selecting needs the recursive disk walk to discover every
                // descendant, so it runs on a cancelable background task.
                StartBackgroundWildcard(folder, check);
            }
            else
            {
                // Unselecting does NOT need any disk I/O: the checked set already
                // knows exactly what is checked, so we just drop every checked entry
                // under this folder in-memory (instant, no background task).
                RemoveSubtreeFast(folder);
                SetRangeValidationErrorIfNeeded();
            }
        }

        /// <summary>
        /// Fast (synchronous, no disk I/O) recursive unselect of a folder subtree. Because the checked
        /// set already stores exactly which paths are checked, unselecting just removes the folder
        /// itself plus every checked entry located under it (prefix match) directly from
        /// <see cref="_checkedItems"/>. The visible subtree is then collapsed back to the folder and
        /// ancestor folders are reconciled — mirroring the background unselect result without the cost
        /// of enumerating the disk.
        /// </summary>
        private void RemoveSubtreeFast(Node folder)
        {
            string prefix = EnsureTrailingSeparator(folder.FullPath);

            // Drop the folder itself and every checked descendant (path-keyed, independent of the
            // visible slice). Snapshot the keys first so we can mutate the dictionary while removing.
            List<string> toRemove = [];
            foreach (string path in _checkedItems.Keys)
            {
                if (PathEquals(path, folder.FullPath)
                    || path.StartsWith(prefix, PathComparison))
                {
                    toRemove.Add(path);
                }
            }
            if (toRemove.Count == 0)
            {
                return;
            }
            foreach (string path in toRemove)
            {
                _checkedItems.Remove(path);
            }
            _folderStateCache.Clear();

            // Keep the visible tree coherent: collapse the subtree back to the folder and reconcile
            // ancestor folders so their tri-state reflects the removal.
            int index = NodeIndex(folder);
            if (index >= 0)
            {
                Collapse(index);
                RebuildPaginator(selectFirst: false);
                int reindex = NodeIndex(folder);
                if (reindex >= 0)
                {
                    _localpaginator!.EnsureVisibleIndex(reindex);
                    RefreshAncestorStates(_nodes[reindex]);
                }
            }
        }

        #region background (cancelable) wildcard selection

        /// <summary>
        /// True while at least one background wildcard recursive operation is active (from the moment
        /// it starts until its result has been applied or it was cancelled). Stays true even after a
        /// task finished computing, so its wait glyph persists until the model is actually updated.
        /// </summary>
        private bool IsBackgroundActive => _bgOps.Count > 0;

        /// <summary>
        /// True when at least one background operation has finished computing and is waiting to have
        /// its result applied to the model on the UI thread.
        /// </summary>
        private bool HasCompletedBackgroundOp()
        {
            foreach (BackgroundOp op in _bgOps.Values)
            {
                if (op.Completed)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Waits for a key press. While any background wildcard operation is running, it returns
        /// immediately (without blocking) so the render loop keeps spinning: this frees the UI right
        /// after the operation starts, repaints the wait glyph(s), and stays responsive to keys.
        /// On completion it returns a finished wake-up so <see cref="TryResult"/> can apply results.
        /// When idle, defers to the base implementation. Honors resize/cancellation.
        /// </summary>
        public override ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken token)
        {
            if (_bgOps.Count == 0)
            {
                return base.WaitKeypress(intercept, token);
            }

            // A real key press (e.g. the cancel key) always takes priority.
            if (ConsoleHandler.KeyAvailable && !token.IsCancellationRequested)
            {
                return ConsoleHandler.ReadKey(intercept);
            }
            if (token.IsCancellationRequested)
            {
                return default;
            }
            if (IsPendingResize)
            {
                return default;
            }
            // Any operation finished: wake TryResult so it applies the computed result(s).
            if (HasCompletedBackgroundOp())
            {
                return s_finishedWakeUp;
            }
            // Still running: return a tick wake-up immediately so the loop repaints the glyph(s) and
            // remains free/responsive. TryResult throttles these ticks to avoid a busy spin.
            return s_tickWakeUp;
        }

        /// <summary>
        /// Starts the recursive wildcard select/unselect for a folder on a background task, tracked
        /// independently by the folder's full path. Multiple folders can run concurrently. Only the
        /// expensive disk enumeration runs off the UI thread; the produced <see cref="FileItem"/> list
        /// is applied to the model on the UI thread in <see cref="ApplyCompletedBackgroundOps"/>.
        /// </summary>
        private void StartBackgroundWildcard(Node folder, bool check)
        {
            if (_bgOps.ContainsKey(folder.FullPath))
            {
                // An operation is already running for this folder; ignore duplicate starts.
                return;
            }

            string rootPath = folder.FullPath;
            bool onlyFolders = _onlyFolders;
            bool selectFilesOnly = _selectFilesOnly;
            bool showHidden = _showHidden;
            string searchPattern = _searchPattern;
            FileAttributes skip = BuildAttributesToSkip();

            // Capture the predicate delegates locally so the background task never reads instance
            // fields from another thread. Only relevant when selecting (check == true); unselect
            // never consults the predicate.
            Func<FileItem, (bool, string?)>? predicate = _predicatevalidcheck;
            Func<FileItem, Task<(bool, string?)>>? predicateAsync = _predicatevalidcheckAsync;

            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeToken);
            BackgroundOp op = new()
            {
                TargetPath = rootPath,
                Check = check,
                Cts = cts
            };
            _bgOps[rootPath] = op;

            CancellationToken token = cts.Token;
            // Runs on the thread pool (I/O-bound with short CPU bursts — no dedicated thread needed).
            // The selection predicate (if any) is evaluated HERE, on the background thread, so its cost
            // (including an async predicate) never blocks the UI thread when the result is applied.
            // Any failure is captured so the operation always completes and never leaves a stuck wait
            // glyph: on cancellation or any error Result stays null so nothing is applied to the model.
            op.Task = Task.Run(() =>
            {
                try
                {
                    List<FileItem> collected = [];
                    EnumerateSubtree(rootPath, onlyFolders, selectFilesOnly, showHidden, searchPattern, skip, collected, token);

                    // On select, pre-filter the enumerated snapshot with the predicate off the UI
                    // thread. Rejected items are silently dropped (Option C — mass selection). The
                    // UI-side apply then becomes a trivial dictionary fill and never blocks.
                    if (check && (predicate != null || predicateAsync != null))
                    {
                        List<FileItem> approved = new(collected.Count);
                        foreach (FileItem item in collected)
                        {
                            token.ThrowIfCancellationRequested();
                            (bool ok, _) = predicateAsync != null
                                ? predicateAsync.Invoke(item).ConfigureAwait(false).GetAwaiter().GetResult()
                                : (predicate?.Invoke(item) ?? (true, null));
                            if (ok)
                            {
                                approved.Add(item);
                            }
                        }
                        collected = approved;
                    }

                    op.Result = collected;
                }
                catch (OperationCanceledException)
                {
                    // Cancelled via the wildcard/abort key: discard any partial work.
                    op.Result = null;
                }
                catch
                {
                    // Any unexpected failure (e.g. PathTooLong, Security): discard partial work and
                    // let the operation complete so its wait glyph is cleared instead of hanging.
                    op.Result = null;
                }
                finally
                {
                    op.Completed = true;
                }
            }, token);
        }

        /// <summary>
        /// Recursively enumerates every checkable descendant (and the folder itself, unless files-only)
        /// under <paramref name="rootPath"/> into <paramref name="collected"/>. Pure I/O; safe to run
        /// off the UI thread because it never touches the live tree model.
        /// </summary>
        private static void EnumerateSubtree(string rootPath, bool onlyFolders, bool selectFilesOnly, bool showHidden, string searchPattern, FileAttributes skip, List<FileItem> collected, CancellationToken token)
        {
            if (!selectFilesOnly)
            {
                collected.Add(BuildFileItem(rootPath, isDirectory: true));
            }

            var options = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = false,
                AttributesToSkip = skip
            };

            var stack = new Stack<string>();
            stack.Push(rootPath);
            while (stack.Count > 0)
            {
                token.ThrowIfCancellationRequested();
                string current = stack.Pop();
                try
                {
                    foreach (string dir in FileSystem.Directory.EnumerateDirectories(current, "*", options))
                    {
                        token.ThrowIfCancellationRequested();
                        if (!showHidden && IsUnixLike && IsUnixHiddenByName(dir))
                        {
                            continue;
                        }
                        if (!selectFilesOnly)
                        {
                            collected.Add(BuildFileItem(dir, isDirectory: true));
                        }
                        stack.Push(dir);
                    }
                    if (!onlyFolders)
                    {
                        foreach (string file in FileSystem.Directory.EnumerateFiles(current, searchPattern, options))
                        {
                            token.ThrowIfCancellationRequested();
                            if (!showHidden && IsUnixLike && IsUnixHiddenByName(file))
                            {
                                continue;
                            }
                            collected.Add(BuildFileItem(file, isDirectory: false));
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException)
                {
                }
            }
        }

        /// <summary>
        /// Applies every background operation that has finished computing to the live model: expands
        /// each target subtree, checks/unchecks the enumerated items, collapses on unselect, and
        /// reconciles ancestors. Runs on the UI thread once a task signals completion. Operations that
        /// are still running are left untouched.
        /// </summary>
        private void ApplyCompletedBackgroundOps()
        {
            // Snapshot the finished ops so we can safely mutate the dictionary while iterating.
            List<BackgroundOp> finished = [];
            foreach (BackgroundOp op in _bgOps.Values)
            {
                if (op.Completed)
                {
                    finished.Add(op);
                }
            }

            foreach (BackgroundOp op in finished)
            {
                _bgOps.Remove(op.TargetPath);
                op.Cts.Dispose();
                ApplyBackgroundOp(op);
            }
        }

        /// <summary>
        /// Applies a single finished operation's enumerated snapshot to the model and keeps the
        /// visible tree coherent (expand on select / collapse on unselect + ancestor reconcile).
        /// </summary>
        private void ApplyBackgroundOp(BackgroundOp op)
        {
            List<FileItem>? result = op.Result;
            bool check = op.Check;

            if (result == null)
            {
                // Cancelled or faulted before producing a result; nothing to apply.
                return;
            }

            // Apply the checked-state changes from the enumerated snapshot (path-keyed, so it does
            // not depend on the visible node slice). On select, the snapshot was already filtered by
            // the predicate on the background thread, so no predicate is consulted here (the UI apply
            // stays a trivial dictionary fill and never blocks — even with an async predicate).
            foreach (FileItem item in result)
            {
                if (selectFilesOnlyBlocks(item))
                {
                    continue;
                }
                if (check)
                {
                    _checkedItems[item.FullPath] = item;
                }
                else
                {
                    _checkedItems.Remove(item.FullPath);
                }
            }
            _folderStateCache.Clear();

            // When the "filter only selected" overlay is active, the visible slice (_nodes) is the
            // flat checked-item list and the real tree is parked in _savedNodes. Reconcile the real
            // tree (expand/collapse + ancestor states) off-screen against _savedNodes, then rebuild
            // the overlay from the now-updated checked set so it stays coherent and never corrupts
            // the snapshot that is restored when the overlay is turned off.
            if (_filterOnlySelected)
            {
                ApplyBackgroundOpToSavedTree(op, check);
                LoadFilteredSelectedNodes();
                RebuildPaginator(selectFirst: false);
                return;
            }

            // Keep the visible tree coherent: expand (on select) or collapse (on unselect) the
            // target subtree and reconcile ancestor folders. The target node may no longer be in the
            // visible slice (collapsed elsewhere); locate it by path when possible.
            int index = IndexOfPath(op.TargetPath);
            if (index >= 0)
            {
                if (check)
                {
                    ExpandSubtree(index);
                }
                else
                {
                    Collapse(index);
                }
                RebuildPaginator(selectFirst: false);
                int reindex = IndexOfPath(op.TargetPath);
                if (reindex >= 0)
                {
                    _localpaginator!.EnsureVisibleIndex(reindex);
                    RefreshAncestorStates(_nodes[reindex]);
                }
            }

            bool selectFilesOnlyBlocks(FileItem item) => _selectFilesOnly && item.IsDirectory;
        }

        /// <summary>
        /// Reconciles the parked real tree (<see cref="_savedNodes"/>) for a completed background
        /// operation while the "filter only selected" overlay is active. It temporarily swaps the
        /// saved tree back into <see cref="_nodes"/>, runs the normal expand/collapse + ancestor
        /// reconciliation, and then re-parks the updated tree — all without touching the visible
        /// overlay, so the snapshot restored when the overlay closes reflects the background result.
        /// </summary>
        private void ApplyBackgroundOpToSavedTree(BackgroundOp op, bool check)
        {
            if (_savedNodes == null)
            {
                return;
            }

            // Swap the parked real tree into _nodes so the existing index/expand/collapse helpers
            // (which all operate on _nodes) reconcile the real tree instead of the flat overlay.
            List<Node> overlay = [.. _nodes];
            _nodes.Clear();
            _nodes.AddRange(_savedNodes);
            RecomputeAncestorMasks();

            int index = IndexOfPath(op.TargetPath);
            if (index >= 0)
            {
                if (check)
                {
                    ExpandSubtree(index);
                }
                else
                {
                    Collapse(index);
                }
                int reindex = IndexOfPath(op.TargetPath);
                if (reindex >= 0)
                {
                    RefreshAncestorStates(_nodes[reindex]);
                }
            }

            // Re-park the updated real tree and restore the visible overlay slice.
            _savedNodes = [.. _nodes];
            _nodes.Clear();
            _nodes.AddRange(overlay);
        }

        /// <summary>
        /// Requests cancellation of the background wildcard operation for <paramref name="targetPath"/>
        /// (if any) and discards its partial result. The live model is left unchanged. Other running
        /// operations are unaffected.
        /// </summary>
        private void CancelBackgroundWildcard(string targetPath)
        {
            if (!_bgOps.TryGetValue(targetPath, out BackgroundOp? op))
            {
                return;
            }
            _bgOps.Remove(targetPath);
            CancelAndDispose(op);
        }

        /// <summary>
        /// Cancels and disposes every running background operation (used on finalize). The live model
        /// is left unchanged.
        /// </summary>
        private void CancelAllBackgroundWildcards()
        {
            if (_bgOps.Count == 0)
            {
                return;
            }
            List<BackgroundOp> ops = [.. _bgOps.Values];
            _bgOps.Clear();
            foreach (BackgroundOp op in ops)
            {
                CancelAndDispose(op);
            }
        }

        private static void CancelAndDispose(BackgroundOp op)
        {
            op.Cts.Cancel();
            try
            {
                op.Task?.Wait(TimeSpan.FromSeconds(2));
            }
            catch
            {
                // ignore cancellation/aggregation exceptions
            }
            op.Cts.Dispose();
        }

        private static bool IsFinishedWakeUp(ConsoleKeyInfo keyInfo)
            => keyInfo.KeyChar == (char)1 && keyInfo.Key == ConsoleKey.None
               && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
               && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control);

        private static bool IsTickWakeUp(ConsoleKeyInfo keyInfo)
            => keyInfo.KeyChar == (char)1 && keyInfo.Key == ConsoleKey.None
               && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control)
               && !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift);

        #endregion

        /// <summary>
        /// Recursively expands <paramref name="index"/> and every descendant directory so the entire
        /// subtree is materialized in <see cref="_nodes"/>.
        /// </summary>
        private void ExpandSubtree(int index)
        {
            Node node = _nodes[index];
            if (!node.IsDirectory)
            {
                return;
            }
            int rootDepth = node.Depth;
            int i = index;
            while (i < _nodes.Count && (i == index || _nodes[i].Depth > rootDepth))
            {
                Node current = _nodes[i];
                if (current.IsDirectory && !current.IsExpanded)
                {
                    Expand(i);
                }
                i++;
            }
        }

        /// <summary>
        /// Applies the checked state to every descendant of the directory at <paramref name="index"/>.
        /// The directory itself is included unless files-only mode is enabled.
        /// </summary>
        private void ApplyCheckToSubtree(int index, bool check)
        {
            if (index < 0)
            {
                return;
            }
            Node parent = _nodes[index];
            if (!_selectFilesOnly)
            {
                SetChecked(parent, check);
            }
            for (int i = index + 1; i < _nodes.Count && _nodes[i].Depth > parent.Depth; i++)
            {
                Node n = _nodes[i];
                if (_selectFilesOnly && n.IsDirectory)
                {
                    continue;
                }
                SetChecked(n, check);
            }
        }

        private void SetChecked(Node node, bool check)
        {
            if (check)
            {
                // Mass selection: silently skip nodes rejected by the predicate (Option C).
                FileItem candidate = ToFileItem(node);
                if (!TryValidatePredicate(candidate, out _))
                {
                    return;
                }
                _checkedItems[node.FullPath] = candidate;
            }
            else
            {
                _checkedItems.Remove(node.FullPath);
            }
            InvalidateFolderStateChain(node.FullPath);
        }

        /// <summary>
        /// Selectively drops the cached tri-state of <paramref name="path"/> and every ancestor folder
        /// up to (and including) the root — the only folders whose selection state can change when a
        /// single entry under <paramref name="path"/> is checked/unchecked. Leaves unrelated folders'
        /// cached states intact (cheaper than clearing the whole cache on every mutation).
        /// </summary>
        private void InvalidateFolderStateChain(string path)
        {
            if (_folderStateCache.Count == 0)
            {
                return;
            }
            string? current = path;
            while (!string.IsNullOrEmpty(current))
            {
                _folderStateCache.Remove(current);
                if (PathEquals(current, _root))
                {
                    break;
                }
                current = Path.GetDirectoryName(current);
            }
        }

        /// <summary>
        /// After a child toggle, walks every ancestor folder of <paramref name="node"/> (excluding the
        /// root) and reconciles its stored membership in <see cref="_checkedItems"/> with the tri-state
        /// of its descendants: a folder is only kept "checked" when ALL its descendants are checked;
        /// otherwise it is removed so it renders as partial/none.
        /// </summary>
        private void RefreshAncestorStates(Node node)
        {
            int index = NodeIndex(node);
            if (index < 0)
            {
                return;
            }
            int childDepth = node.Depth;
            for (int i = index - 1; i >= 0; i--)
            {
                Node ancestor = _nodes[i];
                if (ancestor.Depth >= childDepth)
                {
                    continue;
                }
                childDepth = ancestor.Depth;

                if (ancestor.IsRoot || !ancestor.IsDirectory || _selectFilesOnly)
                {
                    if (ancestor.IsRoot)
                    {
                        break;
                    }
                    continue;
                }

                // Fully selected => keep checked; otherwise drop so it shows partial/none.
                if (GetFolderSelectionState(ancestor) == 1)
                {
                    _checkedItems[ancestor.FullPath] = ToFileItem(ancestor);
                }
                else
                {
                    _checkedItems.Remove(ancestor.FullPath);
                }

                if (ancestor.Depth == 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Computes the tri-state selection status of a directory based on its visible descendants:
        /// <c>0</c> = none selected, <c>1</c> = all selected, <c>2</c> = partially selected.
        /// </summary>
        private int GetFolderSelectionState(Node folder)
        {
            if (_folderStateCache.TryGetValue(folder.FullPath, out int cached))
            {
                return cached;
            }

            int state = ComputeFolderSelectionState(folder);
            _folderStateCache[folder.FullPath] = state;
            return state;
        }

        private int ComputeFolderSelectionState(Node folder)
        {
            int index = NodeIndex(folder);
            if (index < 0)
            {
                return _checkedItems.ContainsKey(folder.FullPath) ? 1 : 0;
            }

            // Tri-state derived purely from the folder's descendants (its own membership only
            // matters when the folder has no checkable descendants).
            bool anyChecked = false;
            bool anyUnchecked = false;
            bool hasDescendant = false;

            for (int i = index + 1; i < _nodes.Count && _nodes[i].Depth > folder.Depth; i++)
            {
                Node n = _nodes[i];
                if (_selectFilesOnly && n.IsDirectory)
                {
                    continue;
                }
                hasDescendant = true;
                if (_checkedItems.ContainsKey(n.FullPath))
                {
                    anyChecked = true;
                }
                else
                {
                    anyUnchecked = true;
                }
            }

            if (!hasDescendant)
            {
                return _checkedItems.ContainsKey(folder.FullPath) ? 1 : 0;
            }
            if (anyChecked && anyUnchecked)
            {
                return 2;
            }
            return anyChecked ? 1 : 0;
        }

        private FileItem[] BuildResult()
            => [.. _checkedItems.Values.OrderBy(x => x.FullPath, PathComparer)];

        private string BuildCheckedItemsText()
            => string.Join(',', _checkedItems.Values
                .OrderBy(x => x.FullPath, PathComparer)
                .Select(x => FormatAnswer(x.FullPath, x.Name, IsRootPath(x.FullPath))));

        #endregion

        #region tree model (lazy expand / collapse)

        private string NextId()
        {
            _sequence++;
            return _sequence.ToString(CultureInfo.CurrentCulture);
        }

        private void Expand(int index)
        {
            Node node = _nodes[index];
            if (!node.IsDirectory || node.IsExpanded)
            {
                return;
            }
            node.IsExpanded = true;
            InsertChildren(index);
        }

        private void Collapse(int index)
        {
            Node node = _nodes[index];
            if (!node.IsDirectory || !node.IsExpanded)
            {
                return;
            }
            node.IsExpanded = false;
            int removeFrom = index + 1;
            int removeCount = 0;
            for (int i = removeFrom; i < _nodes.Count && _nodes[i].Depth > node.Depth; i++)
            {
                removeCount++;
            }
            if (removeCount > 0)
            {
                _nodes.RemoveRange(removeFrom, removeCount);
            }
        }

        private void InsertChildren(int parentIndex)
        {
            Node parent = _nodes[parentIndex];
            List<Node> children = LoadChildren(parent);
            if (children.Count == 0)
            {
                return;
            }
            _nodes.InsertRange(parentIndex + 1, children);
        }

        private List<Node> LoadChildren(Node parent)
        {
            var dirs = new List<Node>();
            var files = new List<Node>();
            try
            {
                var options = new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = false,
                    AttributesToSkip = BuildAttributesToSkip()
                };

                foreach (string dir in FileSystem.Directory.EnumerateDirectories(parent.FullPath, "*", options))
                {
                    if (ShouldSkipHiddenEntry(dir))
                    {
                        continue;
                    }
                    IDirectoryInfo di = FileSystem.DirectoryInfo.New(dir);
                    dirs.Add(new Node(NextId(), di.FullName, di.Name, isDirectory: true, parent.Depth + 1, isLast: false)
                    {
                        LastWriteTime = SafeLastWrite(di)
                    });
                }

                if (!_onlyFolders)
                {
                    foreach (string file in FileSystem.Directory.EnumerateFiles(parent.FullPath, _searchPattern, options))
                    {
                        if (ShouldSkipHiddenEntry(file))
                        {
                            continue;
                        }
                        IFileInfo fi = FileSystem.FileInfo.New(file);
                        files.Add(new Node(NextId(), fi.FullName, fi.Name, isDirectory: false, parent.Depth + 1, isLast: false)
                        {
                            Length = SafeLength(fi),
                            LastWriteTime = SafeLastWrite(fi)
                        });
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }

            dirs.Sort(static (a, b) => string.Compare(a.Name, b.Name, NameComparison));
            files.Sort(static (a, b) => string.Compare(a.Name, b.Name, NameComparison));

            var result = new List<Node>(dirs.Count + files.Count);
            result.AddRange(dirs);
            result.AddRange(files);
            if (result.Count > 0)
            {
                result[^1].IsLast = true;
            }
            return result;
        }

        private FileAttributes BuildAttributesToSkip()
        {
            FileAttributes skip = 0;
            if (!_showHidden)
            {
                skip |= FileAttributes.Hidden;
            }
            if (!_showSystem && OperatingSystem.IsWindows())
            {
                skip |= FileAttributes.System;
            }
            return skip;
        }

        private int ParentIndex(int index)
        {
            int depth = _nodes[index].Depth;
            for (int i = index - 1; i >= 0; i--)
            {
                if (_nodes[i].Depth < depth)
                {
                    return i;
                }
            }
            return -1;
        }

        private void RebuildPaginator(bool selectFirst)
        {
            RecomputeAncestorMasks();

            int keepIndex = selectFirst ? -1 : (_localpaginator?.CurrentIndex ?? -1);
            _localpaginator = new Paginator<Node>(
                FilterMode.Disabled,
                _nodes,
                _effectivePageSize,
                Optional<Node>.Empty(),
                (a, b) => a.UniqueId == b.UniqueId,
                (item) => item.Name);

            if (keepIndex >= 0 && keepIndex < _nodes.Count)
            {
                _localpaginator.EnsureVisibleIndex(keepIndex);
            }
            else
            {
                _localpaginator.FirstItem();
            }
        }

        private void RecomputeAncestorMasks()
        {
            // The visible slice changed: rebuild the path -> index map and drop the folder tri-state
            // cache (both are keyed off the current visible node layout).
            _indexByPath.Clear();
            _folderStateCache.Clear();

            long chainMask = 0;
            for (int i = 0; i < _nodes.Count; i++)
            {
                Node n = _nodes[i];
                int depth = n.Depth;

                _indexByPath[n.FullPath] = i;

                long ancestorBits = depth <= 1 ? 0 : (((1L << depth) - 1) & ~1L);
                n.AncestorMask = chainMask & ancestorBits;

                if (depth < 63)
                {
                    if (n.IsLast)
                    {
                        chainMask &= ~(1L << depth);
                    }
                    else
                    {
                        chainMask |= (1L << depth);
                    }
                    chainMask &= (1L << (depth + 1)) - 1;
                }
            }
        }

        private static FileItem ToFileItem(Node node)
            => new(node.FullPath, node.Name, node.IsDirectory, node.Length, node.LastWriteTime);

        private static FileItem BuildFileItem(string full, bool isDirectory)
        {
            long length = 0;
            DateTime lastWrite = DateTime.MinValue;
            try
            {
                if (isDirectory)
                {
                    lastWrite = FileSystem.DirectoryInfo.New(full).LastWriteTime;
                }
                else
                {
                    IFileInfo fi = FileSystem.FileInfo.New(full);
                    length = SafeLength(fi);
                    lastWrite = SafeLastWrite(fi);
                }
            }
            catch
            {
                // ignore; use defaults
            }
            return new FileItem(full, Path.GetFileName(full), isDirectory, length, lastWrite);
        }

        private void SaveHistory()
        {
            if (_historyOptions == null)
            {
                return;
            }
            string[] paths = [.. _checkedItems.Values.Select(x => x.FullPath)];
            string serializedValue = JsonSerializer.Serialize(paths);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;
        }

        private static bool TryDeserializeHistoryValue(string value, out string[] result)
        {
            result = [];
            try
            {
                string[]? deserialized = JsonSerializer.Deserialize<string[]>(value);
                if (deserialized is not null)
                {
                    result = deserialized;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static long SafeLength(IFileInfo fi)
        {
            try { return fi.Length; }
            catch { return 0; }
        }

        private static DateTime SafeLastWrite(IFileSystemInfo info)
        {
            try { return info.LastWriteTime; }
            catch { return DateTime.MinValue; }
        }

        #endregion

        #region rendering

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            Node? selected = _localpaginator?.SelectedItem;
            string text = selected is null ? string.Empty : FormatAnswer(selected.FullPath, selected.Name, selected.IsRoot);

            // Read-only, horizontally-scrollable answer handled by the base: it owns the emacs
            // buffer and only reloads it when the text changes, so horizontal navigation and the
            // left/right ellipsis behave correctly for long paths.
            WriteAnswerViewport(screenBuffer, text, _optStyles[MultiFileStyles.Answer]);
        }

        private string FormatAnswer(string fullPath, string name, bool isRoot)
        {
            if (_showFullPath)
            {
                return fullPath;
            }

            // Root: show only the folder name (fall back to the full path for a drive root like "C:\").
            if (isRoot)
            {
                string rootName = Path.GetFileName(fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                return string.IsNullOrEmpty(rootName) ? fullPath : rootName;
            }

            string? parentDir = Path.GetDirectoryName(fullPath);
            if (string.IsNullOrEmpty(parentDir))
            {
                return name;
            }

            string parentName = Path.GetFileName(parentDir);
            if (string.IsNullOrEmpty(parentName))
            {
                parentName = parentDir;
            }
            return $"{parentName}{Path.DirectorySeparatorChar}{name}";
        }

        /// <summary>
        /// Determines whether <paramref name="fullPath"/> refers to the browse root.
        /// </summary>
        private bool IsRootPath(string fullPath)
            => PathEquals(fullPath, _root);

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[MultiFileStyles.Description]);
            }
        }

        private void WriteTreeList(BufferScreen screenBuffer)
        {
            ArraySegment<Node> subset = _localpaginator!.GetPageData();
            Node? selectedItem = _localpaginator.SelectedIndex >= 0 ? _localpaginator.SelectedItem : null;

            foreach (Node node in subset)
            {
                bool isSelected = selectedItem != null && node.UniqueId == selectedItem.UniqueId;
                bool isChecked = _checkedItems.ContainsKey(node.FullPath);

                Style lineStyle = isSelected ? _optStyles[MultiFileStyles.Selected] : _optStyles[MultiFileStyles.UnSelected];

                // Selector marker.
                screenBuffer.Write(isSelected ? GetSymbol(SymbolType.Selector) : " ", ConsoleHandler.CurrentStyle);

                // Indentation using tree-line symbols (depth-based).
                if (node.Depth > 0)
                {
                    for (int d = 1; d < node.Depth; d++)
                    {
                        screenBuffer.Write((node.AncestorMask & (1L << d)) != 0
                            ? GetSymbol(SymbolType.TreeLinevertical)
                            : GetSymbol(SymbolType.TreeLinespace), _optStyles[MultiFileStyles.Lines]);
                    }

                    screenBuffer.Write(node.IsLast
                        ? GetSymbol(SymbolType.TreeLinecorner)
                        : GetSymbol(SymbolType.TreeLinecross), _optStyles[MultiFileStyles.Lines]);
                }

                // Check indicator (root cannot be checked).
                if (!node.IsRoot && (!_selectFilesOnly || !node.IsDirectory))
                {
                    // While a background wildcard operation targets this folder, show a wait glyph
                    // (wrapped in brackets) in place of the check indicator so the user knows the
                    // recursive work is running. It persists across page changes / list refreshes
                    // until the background operation completes or is cancelled. Multiple folders can
                    // display the glyph independently when several operations run concurrently.
                    bool isBackgroundTarget = _bgOps.ContainsKey(node.FullPath);

                    if (isBackgroundTarget)
                    {
                        screenBuffer.Write(GetSymbol(SymbolType.WaitProcess), lineStyle);
                        screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                    }
                    else
                    {
                        SymbolType checkSymbol;
                        if (node.IsDirectory)
                        {
                            // Tri-state for folders: all / none / partially selected.
                            checkSymbol = GetFolderSelectionState(node) switch
                            {
                                1 => SymbolType.Selected,
                                2 => SymbolType.PartialSelect,
                                _ => SymbolType.NotSelect
                            };
                        }
                        else
                        {
                            checkSymbol = isChecked ? SymbolType.Selected : SymbolType.NotSelect;
                        }
                        screenBuffer.Write(GetSymbol(checkSymbol), lineStyle);
                        screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                    }
                }

                // Expand/collapse indicator for directories.
                if (node.IsDirectory)
                {
                    screenBuffer.Write(node.IsExpanded
                        ? GetSymbol(SymbolType.Expanded)
                        : GetSymbol(SymbolType.Collapsed), _optStyles[MultiFileStyles.ExpandSymbol]);
                    screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                }

                // Entry name.
                Style nameStyle = node.IsRoot
                    ? _optStyles[MultiFileStyles.FileRoot]
                    : node.IsDirectory ? _optStyles[MultiFileStyles.FileTypeFolder] : _optStyles[MultiFileStyles.FileTypeFile];
                if (isSelected)
                {
                    nameStyle = lineStyle;
                }
                screenBuffer.Write(node.IsRoot ? FormatAnswer(node.FullPath, node.Name, isRoot: true) : node.Name, nameStyle);

                // File size.
                if (!node.IsDirectory && !_hideSize)
                {
                    screenBuffer.Write($"  {FormatSize(node.Length)}", _optStyles[MultiFileStyles.FileSize]);
                }

                screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
            }

            if (_localpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount)!;
                template = $"{template} {string.Format(CultureInfo.CurrentCulture, s_countCheckFormat, _checkedItems.Count)}";
                screenBuffer.Write(template, _optStyles[MultiFileStyles.Pagination]);

                // Only while at least one background wildcard operation is running, append a tagged
                // "(Running Background)" note (from resources) styled with TaggedInfo.
                if (IsBackgroundActive)
                {
                    screenBuffer.Write($" {PromptPlusResources.MultiFileRunningBackground}", _optStyles[MultiFileStyles.TaggedInfo]);
                }

                // While the "filter only selected" flat view is active, append a tagged
                // "(only selected)" note (from resources) styled with TaggedInfo.
                if (_filterOnlySelected)
                {
                    screenBuffer.Write($" {PromptPlusResources.MultiFileOnlySelected}", _optStyles[MultiFileStyles.TaggedInfo]);
                }

                screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
            }
        }

        private static string FormatSize(long bytes)
        {
            if (bytes < 1024)
            {
                return $"{bytes} B";
            }
            string[] units = s_sizeUnits;
            double size = bytes;
            int unit = -1;
            do
            {
                size /= 1024;
                unit++;
            }
            while (size >= 1024 && unit < units.Length - 1);
            return string.Format(CultureInfo.CurrentCulture, "{0:0.##} {1}", size, units[unit]);
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            // Reload tooltip cache when the "checked" state crosses the 0 boundary or when the
            // "only selected" view toggles: both change whether the FilterAllSelected hint applies.
            bool hasChecked = _checkedItems.Count > 0;
            bool hadChecked = _lastCountCheckedTooltip > 0;
            if (hasChecked != hadChecked || _lastFilterOnlySelectedTooltip != _filterOnlySelected)
            {
                LoadTooltipToggle();
                _lastCountCheckedTooltip = _checkedItems.Count;
                _lastFilterOnlySelectedTooltip = _filterOnlySelected;
            }
            string tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[MultiFileStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            if (_indexTooptip >= _toggerTooptips.Length)
            {
                _indexTooptip = 0;
            }
            return _toggerTooptips[_indexTooptip];
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                GetTooltipNavigate(),
                PromptPlusResources.TooltipExpandCollapse,
                $"{ConfigPrompt.HotKeyToggleAll}:{PromptPlusResources.TooltipCheckAll}"
            ];
            // Only advertise the "filter all selected" hotkey when it actually does something:
            // either there is at least one checked item (to enter the view) or we are already
            // inside the "only selected" view (to leave it).
            if (_checkedItems.Count > 0 || _filterOnlySelected)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyFilterAllSelected}:{PromptPlusResources.TooltipFilterAllSelected}");
            }
            lsttooltips.Add(PromptPlusResources.TooltipPages);
            lsttooltips.Add(PromptPlusResources.TooltipJump);
            lsttooltips.Add($"{ConfigPrompt.HotKeyToggleFullPath}:{PromptPlusResources.TooltipToggleFullPath}");
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
        }

        private string GetTooltipNavigate()
        {
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            tooltip.Append(PromptPlusResources.TooltipCheckItem);
            tooltip.Append('.');
            if (_recursiveMarkWithCtrlSpace)
            {
                tooltip.Append(PromptPlusResources.TooltipRecursiveMark);
                tooltip.Append('.');
            }
            tooltip.Append(PromptPlusResources.TooltipTreeTab);
            tooltip.Append('.');
            tooltip.Append(PromptPlusResources.TooltipBaseNavegate);
            tooltip.Append('.');
            return tooltip.ToString();
        }

        #endregion
    }
}
