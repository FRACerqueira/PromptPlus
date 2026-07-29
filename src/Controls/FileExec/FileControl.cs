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
using System.Text;
using System.Text.Json;
using System.Threading;

namespace PromptPlusLibrary.Controls.FileExec
{
    /// <inheritdoc/>
    internal sealed class FileControl : BaseControlPrompt<FileItem?>, IFileControl
    {
        // Swappable so tests can run against a MockFileSystem instead of the real disk (Windows and
        // Linux alike — mirrors the FileHistory.FileSystem pattern). Defaults to the real filesystem
        // in production. Pure path-string helpers (CountSeparators, EnsureTrailingSeparator, etc.)
        // are left on plain System.IO.Path below: they don't touch disk, so MockFileSystem wouldn't
        // behave differently there — only members that read live filesystem state are routed here.
        internal static IFileSystem FileSystem { get; set; } = new FileSystem();

        /// <summary>
        /// Total rows the control template reserves around the tree list: prompt+answer line,
        /// optional description line, tooltip line and an extra row for the pagination footer.
        /// </summary>
        private const int ReservedTemplateLines = 7;

        /// <summary>
        /// A single visible node of the tree. Children are materialized lazily (only when the node
        /// is expanded) and released on collapse, so memory stays proportional to what is visible.
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

            // Bitmask of ancestor depths (1..Depth-1) whose branch still continues below this node,
            // i.e. where a vertical guide line (│) must be drawn. Precomputed on structural changes
            // so rendering does not rescan the list every frame. Bit d set => draw │ at column d.
            public long AncestorMask;
        }

        private readonly Dictionary<FileStyles, Style> _optStyles;
        // Flat list of currently visible nodes (the whole point of the memory-conscious design:
        // we never keep the entire file system in memory, only the visible/expanded slice).
        private readonly List<Node> _nodes = [];
        private Paginator<Node>? _localpaginator;

        private string _root = FileSystem.Directory.GetCurrentDirectory();
        private string _searchPattern = "*";
        private bool _onlyFolders;
        private bool _showHidden;
        private bool _showSystem;
        private bool _hideSize;
        private bool _selectFilesOnly;
        private bool _showFullPath;
        private byte _pageSize;
        private int _effectivePageSize;
        private int _sequence;
        private string? _defaultFullPath;
        private bool _useDefaultHistory = true;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;

        private string[] _toggerTooptips = [];
        private int _indexTooptip;

        public FileControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<FileStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
        }

        #region IFileControl

        /// <inheritdoc/>
        public IFileControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public IFileControl Styles(FileStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl Root(string path)
        {
            ArgumentNullException.ThrowIfNull(path);
            _root = path;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl SearchPattern(string pattern)
        {
            ArgumentNullException.ThrowIfNull(pattern);
            _searchPattern = string.IsNullOrWhiteSpace(pattern) ? "*" : pattern;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl OnlyFolders(bool value = true)
        {
            _onlyFolders = value;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl ShowHidden(bool value = true)
        {
            _showHidden = value;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl ShowSystem(bool value = true)
        {
            _showSystem = value;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl HideSize(bool value = true)
        {
            _hideSize = value;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl PageSize(byte value)
        {
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl SelectFilesOnly(bool value = true)
        {
            _selectFilesOnly = value;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl ShowFullPath(bool value = true)
        {
            _showFullPath = value;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl Default(string fullPath, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(fullPath);
            _defaultFullPath = fullPath;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        /// <inheritdoc/>
        public IFileControl EnableHistory(string filename, Action<IHistoryOptions>? options = null)
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

            _nodes.Clear();
            _sequence = 0;

            // Resolve the default target: history value (if enabled) can override the explicit Default.
            string? target = _defaultFullPath;
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0
                    && TryDeserializeHistoryValue(_itemHistories[0].History, out string? historyValue)
                    && !string.IsNullOrEmpty(historyValue))
                {
                    target = historyValue;
                }
            }

            // The root node starts expanded so its immediate children are visible.
            Node rootNode = new(NextId(), _root, _root, isDirectory: true, depth: 0, isLast: true) { IsRoot = true, IsExpanded = true };
            _nodes.Add(rootNode);
            InsertChildren(0);

            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            RebuildPaginator(selectFirst: true);

            // Expand the tree down to the default target (if it lives under the root) and select it.
            ExpandToTarget(target);

            LoadTooltipToggle();
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

            // Expand ancestor folders one segment at a time: at each step find the deepest visible
            // folder that is an ancestor of the target and expand it, until the target node appears.
            // A bounded loop (max = path depth) prevents any accidental infinite loop.
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
                    return; // no further progress possible (inaccessible / missing)
                }
                Expand(ancestorIndex);
                RebuildPaginator(selectFirst: false);
            }
        }

        private int IndexOfPath(string full)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (PathEquals(_nodes[i].FullPath, full))
                {
                    return i;
                }
            }
            return -1;
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
                            ResultCtrl = new ResultPrompt<FileItem?>(null, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;
                    Node? selected = _localpaginator!.SelectedItem;

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<FileItem?>(null, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && selected != null)
                    {
                        _indexTooptip = 0;
                        if (_selectFilesOnly && selected.IsDirectory)
                        {
                            SetError(PromptPlusResources.SelectionDisabled);
                            break;
                        }
                        ResultCtrl = new ResultPrompt<FileItem?>(ToFileItem(selected), false);
                        SaveHistory(selected.FullPath);
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
                    else if (ConfigPrompt.HotKeyToggleFullPath.Equals(keyinfo))
                    {
                        // Toggle between showing the full path and just the name in the answer line.
                        _showFullPath = !_showFullPath;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressExpandKey()
                             && selected is { IsDirectory: true, IsExpanded: false })
                    {
                        // Expand (only the '+' key): materialize children lazily.
                        Expand(_localpaginator.CurrentIndex);
                        RebuildPaginator(selectFirst: false);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCollapseKey()
                             && selected is { IsDirectory: true, IsExpanded: true })
                    {
                        // Collapse (only the '-' key): release children (frees memory).
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
                            // Tab on a folder: expand (if needed) and move to the first child.
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
                            // Tab on a file: move to the next item.
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
                            // Shift+Tab on the first child of a folder: collapse the parent and
                            // move to it.
                            _localpaginator.EnsureVisibleIndex(parentIndex);
                            if (_nodes[parentIndex].IsExpanded)
                            {
                                Collapse(parentIndex);
                                RebuildPaginator(selectFirst: false);
                            }
                        }
                        else
                        {
                            // Otherwise: move to the previous item.
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
                        // Non-printable readline keys (Home/End/Left/Right/...) scroll the long
                        // answer text horizontally (handled by the base answer viewport).
                        break;
                    }
                    else if (selected != null && !char.IsControl(keyinfo.KeyChar) && keyinfo.KeyChar != '\0')
                    {
                        // Jump: type the initial character to move the selection to the next visible
                        // node whose name starts with that character (wraps around).
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

            WritePrompt(screenBuffer, _optStyles[FileStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteDescription(screenBuffer);
            WriteTreeList(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[FileStyles.Error]);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[FileStyles.Prompt]);
            string answer;
            if (ResultCtrl!.Value.IsAborted)
            {
                answer = OptionsControl.ShowMessageAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
            }
            else
            {
                FileItem? content = ResultCtrl.Value.Content;
                answer = content is null
                    ? string.Empty
                    : FormatAnswer(content.FullPath, content.Name, IsRootPath(content.FullPath));
            }
            screenBuffer.WriteLine(answer, _optStyles[FileStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            // Release all materialized nodes so nothing lingers after the control closes.
            _nodes.Clear();
        }

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
            // Remove every descendant (deeper depth) that follows this node. Releasing them here is
            // what keeps memory bounded to the currently expanded branches.
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

        /// <summary>
        /// Enumerates a directory's immediate children with streaming enumeration (no full
        /// materialization of the whole tree), applying attribute/pattern filters. Directories are
        /// listed first, then files, both sorted by name.
        /// </summary>
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

                // Directories (always, even in OnlyFolders mode).
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
                // Inaccessible folder: show it as empty rather than failing the whole control.
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
            // Structural change: recompute the ancestor guide-line masks once here instead of
            // scanning the list for every rendered row on every frame.
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

        /// <summary>
        /// Computes, for every visible node, the bitmask of ancestor depths whose branch still
        /// continues below it (where a vertical guide line │ must be drawn). Single O(N) forward pass
        /// maintaining the "is-last" state of the current ancestor chain — replaces the previous
        /// per-row O(depth × N) rescans and keeps the resize relayout cheap and correct.
        /// </summary>
        private void RecomputeAncestorMasks()
        {
            // chainMask bit d == 1 means: the current ancestor at depth d is NOT the last of its
            // group, so its branch continues and a vertical guide line must be drawn for descendants.
            long chainMask = 0;
            for (int i = 0; i < _nodes.Count; i++)
            {
                Node n = _nodes[i];
                int depth = n.Depth;

                // Ancestor guide lines for this node are the chain bits at depths 1..depth-1.
                long ancestorBits = depth <= 1 ? 0 : (((1L << depth) - 1) & ~1L); // bits 1..depth-1
                n.AncestorMask = chainMask & ancestorBits;

                // This node becomes the current ancestor at its own depth for the nodes that follow:
                // set the bit when it has following siblings (not last), clear it when it is last.
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
                    // Drop any deeper bits so a previous, deeper branch never leaks into shallower rows.
                    chainMask &= (1L << (depth + 1)) - 1;
                }
            }
        }

        private static FileItem ToFileItem(Node node)
            => new(node.FullPath, node.Name, node.IsDirectory, node.Length, node.LastWriteTime);

        private void SaveHistory(string fullPath)
        {
            if (_historyOptions == null)
            {
                return;
            }
            string serializedValue = JsonSerializer.Serialize(fullPath);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;
        }

        private static bool TryDeserializeHistoryValue(string value, out string? result)
        {
            result = null;
            try
            {
                result = JsonSerializer.Deserialize<string>(value);
                return result is not null;
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
            WriteAnswerViewport(screenBuffer, text, _optStyles[FileStyles.Answer]);
        }

        /// <summary>
        /// Builds the answer text: the full path when <see cref="_showFullPath"/> is <c>true</c>;
        /// otherwise the entry name preceded by its immediate parent directory name (when one exists),
        /// e.g. <c>parent\name</c>. When the entry is the root, only the root folder name is shown.
        /// </summary>
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
            // A drive root (e.g. "C:\") has no file-name segment; fall back to the root itself.
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
                screenBuffer.WriteLine(desc, _optStyles[FileStyles.Description]);
            }
        }

        private void WriteTreeList(BufferScreen screenBuffer)
        {
            ArraySegment<Node> subset = _localpaginator!.GetPageData();
            Node? selectedItem = _localpaginator.SelectedIndex >= 0 ? _localpaginator.SelectedItem : null;

            foreach (Node node in subset)
            {
                bool isSelected = selectedItem != null && node.UniqueId == selectedItem.UniqueId;

                Style lineStyle = isSelected ? _optStyles[FileStyles.Selected] : _optStyles[FileStyles.UnSelected];

                screenBuffer.Write(isSelected ? GetSymbol(SymbolType.Selector) : " ", ConsoleHandler.CurrentStyle);

                // Indentation using tree-line symbols (depth-based). The ancestor guide lines come
                // from the precomputed AncestorMask (see RecomputeAncestorMasks), so rendering is
                // O(depth) per row with no list rescans — independent of the page size.
                if (node.Depth > 0)
                {
                    for (int d = 1; d < node.Depth; d++)
                    {
                        screenBuffer.Write((node.AncestorMask & (1L << d)) != 0
                            ? GetSymbol(SymbolType.TreeLinevertical)
                            : GetSymbol(SymbolType.TreeLinespace), _optStyles[FileStyles.Lines]);
                    }

                    // Connector for this node: "corner" (└─) when it is the last item of its level,
                    // "cross" (├─) otherwise. A folder that is the last child closes correctly with
                    // └─ even when expanded — its children below stay connected via the ancestor
                    // vertical lines above.
                    screenBuffer.Write(node.IsLast
                        ? GetSymbol(SymbolType.TreeLinecorner)
                        : GetSymbol(SymbolType.TreeLinecross), _optStyles[FileStyles.Lines]);
                }

                if (node.IsDirectory)
                {
                    screenBuffer.Write(node.IsExpanded
                        ? GetSymbol(SymbolType.Expanded)
                        : GetSymbol(SymbolType.Collapsed), _optStyles[FileStyles.ExpandSymbol]);
                    screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                }

                Style nameStyle = node.IsRoot
                    ? _optStyles[FileStyles.FileRoot]
                    : node.IsDirectory ? _optStyles[FileStyles.FileTypeFolder] : _optStyles[FileStyles.FileTypeFile];
                if (isSelected)
                {
                    nameStyle = lineStyle;
                }
                screenBuffer.Write(node.IsRoot ? FormatAnswer(node.FullPath, node.Name, isRoot: true) : node.Name, nameStyle);

                if (!node.IsDirectory && !_hideSize)
                {
                    screenBuffer.Write($"  {FormatSize(node.Length)}", _optStyles[FileStyles.FileSize]);
                }

                screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
            }

            if (_localpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount)!;
                screenBuffer.WriteLine(template, _optStyles[FileStyles.Pagination]);
            }
        }

        private static string FormatSize(long bytes)
        {
            if (bytes < 1024)
            {
                return $"{bytes} B";
            }
            string[] units = ["KB", "MB", "GB", "TB", "PB"];
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
            string tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[FileStyles.Tooltips]);
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
            // First entry follows the Select pattern (Enter:Finish + Navigate with arrows) plus the
            // File-specific expand/collapse hint. The remaining entries are cycled with the tooltip
            // toggle hotkey (F1) and detail the paging, jump and full-path toggle actions.
            List<string> lsttooltips =
            [
                GetTooltipNavigate(),
                PromptPlusResources.TooltipExpandCollapse,
                PromptPlusResources.TooltipPages,
                PromptPlusResources.TooltipJump,
                $"{ConfigPrompt.HotKeyToggleFullPath}:{PromptPlusResources.TooltipToggleFullPath}"
            ];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
        }

        private static string GetTooltipNavigate()
        {
            // Same base composition used by the Select control, plus the File-specific
            // expand/collapse hint appended to the first tooltip entry.
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            tooltip.Append(PromptPlusResources.TooltipTreeTab);
            tooltip.Append('.');
            tooltip.Append(PromptPlusResources.TooltipBaseNavegate);
            tooltip.Append('.');
            return tooltip.ToString();
        }

        #endregion
    }
}
