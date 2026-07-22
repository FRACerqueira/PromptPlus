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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.Tree
{
    /// <inheritdoc/>
    internal sealed class TreeControl<T> : BaseControlPrompt<T?>, ITreeControl<T>
    {
        /// <summary>
        /// Rows reserved around the tree list (prompt+answer, description, tooltip, pagination footer).
        /// </summary>
        private const int ReservedTemplateLines = 7;

        /// <summary>
        /// User-visible node of the tree. Owns its children (never released) so the user hierarchy
        /// stays intact between renders. The visible/flat projection lives in <see cref="_nodes"/>.
        /// </summary>
        private sealed class TreeNode(T value, TreeNode? parent) : ITreeNode<T>
        {
            public T Value { get; } = value;
            public TreeNode? ParentNode { get; } = parent;
            public List<TreeNode> Children { get; } = [];

            ITreeNode<T>? ITreeNode<T>.Parent => ParentNode;
            T ITreeNode<T>.Value => Value;

            public ITreeNode<T> AddLast(T v)
            {
                var n = new TreeNode(v, this);
                Children.Add(n);
                return n;
            }

            public ITreeNode<T> AddFirst(T v)
            {
                var n = new TreeNode(v, this);
                Children.Insert(0, n);
                return n;
            }
        }

        /// <summary>
        /// Rendering node in the flat visible list. Materialized on expand, released on collapse.
        /// </summary>
        private sealed class VNode(string uniqueId, TreeNode source, int depth, bool isLast)
        {
            public string UniqueId { get; } = uniqueId;
            public TreeNode Source { get; } = source;
            public int Depth { get; } = depth;
            public bool IsLast { get; set; } = isLast;
            public bool IsExpanded { get; set; }
            public bool IsRoot { get; init; }
            public bool HasChildren => Source.Children.Count > 0;
            public T Value => Source.Value;
            public long AncestorMask;
        }

        private readonly Dictionary<TreeStyles, Style> _optStyles;
        private readonly List<VNode> _nodes = [];
        private Paginator<VNode>? _localpaginator;

        // Filter state ---------------------------------------------------------------------------
        private enum ModeView { Select, Filter }
        private ModeView _modeView = ModeView.Select;
        private FilterMode _filterType = FilterMode.Disabled;
        private EmacsConsoleBuffer? _filterBuffer;
        private EmacsConsoleBuffer? _answerBuffer;
        private bool _updatePosAnswerBuffer;
        private string _lastinput = string.Empty;
        // Flat projection of every user node, built once when the user first enters filter mode.
        // Each VNode here carries a synthesized "display" (its full path) so the paginator can
        // filter by it and rendering can show the caller which branch each match belongs to.
        private List<VNode>? _flatAll;
        private Dictionary<string, string>? _flatDisplayCache; // UniqueId -> full path text

        private TreeNode? _root;
        private Func<T, string>? _textSelector;
        private Func<T, string?>? _extraInfoSelector;
        private Func<T, Task<string?>>? _extraInfoSelectorAsync;
        private Func<T, T, bool>? _equals;
        private Func<T, string>? _changeDescription;
        private Func<T, Task<string>>? _changeDescriptionAsync;
        private char _pathSep = '/';
        private bool _selectLeafOnly;
        private bool _showFullPath;
        private bool _viewOnly;
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private Func<T, Task<(bool, string?)>>? _predicatevalidselectAsync;
        private byte _pageSize;
        private int _effectivePageSize;
        private int _sequence;
        private T? _defaultValue;
        private bool _hasDefault;
        private bool _useDefaultHistory = true;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;

        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Select, [] },
            { ModeView.Filter, [] }
        };
        private int _indexTooptip;

        public TreeControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions)
            : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<TreeStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
        }

        #region ITreeControl<T>

        /// <inheritdoc/>
        public ITreeControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> Styles(TreeStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> Root(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _root = new TreeNode(value, parent: null);
            return this;
        }

        private TreeNode RequireRoot()
        {
            if (_root == null)
            {
                throw new InvalidOperationException("Root must be set before adding nodes.");
            }
            return _root;
        }

        private static TreeNode Unwrap(ITreeNode<T> node)
        {
            ArgumentNullException.ThrowIfNull(node);
            return node as TreeNode
                ?? throw new InvalidOperationException("The provided node does not belong to this tree.");
        }

        private bool BelongsToTree(TreeNode node)
        {
            TreeNode? cursor = node;
            while (cursor != null)
            {
                if (ReferenceEquals(cursor, _root))
                {
                    return true;
                }
                cursor = cursor.ParentNode;
            }
            return false;
        }

        /// <inheritdoc/>
        public ITreeNode<T> AddLast(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return RequireRoot().AddLast(value);
        }

        /// <inheritdoc/>
        public ITreeNode<T> AddFirst(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return RequireRoot().AddFirst(value);
        }

        /// <inheritdoc/>
        public ITreeNode<T> AddAfter(ITreeNode<T> node, T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            TreeNode target = Unwrap(node);
            if (!BelongsToTree(target) || target.ParentNode is null)
            {
                throw new InvalidOperationException("The provided node does not belong to this tree or is the root.");
            }
            TreeNode parent = target.ParentNode;
            int idx = parent.Children.IndexOf(target);
            var created = new TreeNode(value, parent);
            parent.Children.Insert(idx + 1, created);
            return created;
        }

        /// <inheritdoc/>
        public ITreeNode<T> AddBefore(ITreeNode<T> node, T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            TreeNode target = Unwrap(node);
            if (!BelongsToTree(target) || target.ParentNode is null)
            {
                throw new InvalidOperationException("The provided node does not belong to this tree or is the root.");
            }
            TreeNode parent = target.ParentNode;
            int idx = parent.Children.IndexOf(target);
            var created = new TreeNode(value, parent);
            parent.Children.Insert(idx, created);
            return created;
        }

        /// <inheritdoc/>
        public ITreeControl<T> TextSelector(Func<T, string> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);
            _textSelector = selector;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfoSelector = extraInfoNode;
            _extraInfoSelectorAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfoSelectorAsync = extraInfoNode;
            _extraInfoSelector = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> PathSeparator(char value)
        {
            _pathSep = value;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> PageSize(byte value)
        {
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> SelectLeafOnly(bool value = true)
        {
            _selectLeafOnly = value;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> ShowFullPath(bool value = true)
        {
            _showFullPath = value;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);
            _equals = comparer;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> Default(T value, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(value);
            _defaultValue = value;
            _hasDefault = true;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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

        /// <inheritdoc/>
        public ITreeControl<T> ViewOnly(bool value = true)
        {
            _viewOnly = value;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> ChangeDescription(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> Filter(FilterMode value)
        {
            _filterType = value;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) => (validselect(input), (string?)null);
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidselect = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = validselect;
            _predicatevalidselect = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITreeControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T1 item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        /// <inheritdoc/>
        public ITreeControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITreeControl<T>, Task> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T1 item in items)
            {
                interactionAction.Invoke(item, this).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_root is null)
            {
                throw new InvalidOperationException("Tree control requires a Root value.");
            }
            if (_textSelector is null)
            {
                throw new InvalidOperationException("Tree control requires a TextSelector.");
            }
            if (_equals is null)
            {
                throw new InvalidOperationException("Tree control requires DefaultMatchBy.");
            }

            _answerBuffer = new(true, CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
            _filterBuffer = new(false, CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
            _updatePosAnswerBuffer = true;
            _modeView = ModeView.Select;
            _lastinput = string.Empty;
            _flatAll = null;
            _flatDisplayCache = null;

            _nodes.Clear();
            _sequence = 0;

            if (_viewOnly)
            {
                _historyOptions = null;
            }

            // Resolve target: history (if enabled) can override the explicit Default.
            T? target = _hasDefault ? _defaultValue : default;
            bool hasTarget = _hasDefault;

            if (_historyOptions != null)
            {
                ValidateHistorySerializable();
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0
                    && TryDeserializeHistoryValue(_itemHistories[0].History, out T? historyValue)
                    && historyValue is not null)
                {
                    target = historyValue;
                    hasTarget = true;
                }
            }

            VNode rootV = new(NextId(), _root, depth: 0, isLast: true) { IsRoot = true, IsExpanded = true };
            _nodes.Add(rootV);
            InsertChildren(0);

            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            RebuildPaginator(selectFirst: true);

            if (hasTarget && target is not null)
            {
                ExpandToTarget(target);
            }

            LoadTooltipToggle();
        }

        /// <summary>
        /// Walks the user tree to find the ancestor path to <paramref name="target"/> (using
        /// <see cref="_equals"/>), then expands each ancestor from the root down and selects the
        /// matching visible row.
        /// </summary>
        private void ExpandToTarget(T target)
        {
            List<TreeNode>? path = FindPath(_root!, target);
            if (path is null || path.Count == 0)
            {
                return;
            }

            // Expand each ancestor in _nodes (they will be visible one after the other as we expand).
            for (int i = 0; i < path.Count; i++)
            {
                TreeNode step = path[i];
                int idx = IndexOfSource(step);
                if (idx < 0)
                {
                    return;
                }
                if (i < path.Count - 1)
                {
                    VNode v = _nodes[idx];
                    if (v.HasChildren && !v.IsExpanded)
                    {
                        Expand(idx);
                        RebuildPaginator(selectFirst: false);
                    }
                }
                else
                {
                    _localpaginator!.EnsureVisibleIndex(idx);
                }
            }
        }

        /// <summary>
        /// DFS on the user tree returning the chain of nodes from the root (inclusive) down to the
        /// node whose value equals <paramref name="target"/>. Returns <c>null</c> if not found.
        /// </summary>
        private List<TreeNode>? FindPath(TreeNode from, T target)
        {
            if (_equals!(from.Value, target))
            {
                return [from];
            }
            foreach (TreeNode child in from.Children)
            {
                List<TreeNode>? sub = FindPath(child, target);
                if (sub is not null)
                {
                    sub.Insert(0, from);
                    return sub;
                }
            }
            return null;
        }

        private int IndexOfSource(TreeNode source)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (ReferenceEquals(_nodes[i].Source, source))
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
                string name = _textSelector!(_nodes[i].Value) ?? string.Empty;
                if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    _updatePosAnswerBuffer = true;

                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<T?>(default, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;
                    VNode? selected = _localpaginator!.SelectedItem;

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T?>(default, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && selected != null)
                    {
                        _indexTooptip = 0;
                        if (_viewOnly)
                        {
                            ResultCtrl = new ResultPrompt<T?>(_hasDefault ? _defaultValue : default, false);
                            break;
                        }
                        if (_selectLeafOnly && selected.HasChildren)
                        {
                            SetError(PromptPlusResources.SelectionDisabled);
                            break;
                        }
                        (bool ok, string? message) = _predicatevalidselectAsync is not null
                            ? _predicatevalidselectAsync.Invoke(selected.Value).ConfigureAwait(false).GetAwaiter().GetResult()
                            : _predicatevalidselect?.Invoke(selected.Value) ?? (true, (string?)null);
                        if (!ok)
                        {
                            SetError(string.IsNullOrEmpty(message) ? PromptPlusResources.SelectionDisabled : message!);
                            break;
                        }
                        ResultCtrl = new ResultPrompt<T?>(selected.Value, false);
                        SaveHistory(selected.Value);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips[_modeView].Length)
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
                        _showFullPath = !_showFullPath;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressExpandKey()
                             && _modeView == ModeView.Select
                             && selected is { IsExpanded: false } && selected.HasChildren)
                    {
                        Expand(_localpaginator.CurrentIndex);
                        RebuildPaginator(selectFirst: false);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCollapseKey()
                             && _modeView == ModeView.Select
                             && selected is { IsExpanded: true } && selected.HasChildren)
                    {
                        Collapse(_localpaginator.CurrentIndex);
                        RebuildPaginator(selectFirst: false);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressTabKey() && _modeView == ModeView.Select && selected != null)
                    {
                        int current = _localpaginator.CurrentIndex;
                        if (selected.HasChildren)
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
                    else if (keyinfo.IsPressShiftTabKey() && _modeView == ModeView.Select && selected != null)
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
                        _updatePosAnswerBuffer = false;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter
                             && _filterBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        UpdateFilterFromBuffer();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Select
                             && _answerBuffer!.IsPrintable(keyinfo.KeyChar))
                    {
                        var keifilter = keyinfo;
                        if (keifilter.IsPressFilterActivationKey())
                        {
                            keifilter = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
                        }
                        if (_filterBuffer!.TryAcceptedReadlineConsoleKey(keifilter))
                        {
                            _modeView = ModeView.Filter;
                            UpdateFilterFromBuffer();
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType == FilterMode.Disabled
                             && selected != null && !char.IsControl(keyinfo.KeyChar) && keyinfo.KeyChar != '\0')
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
                _lastinput = _filterBuffer!.ToString();
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

            WritePrompt(screenBuffer, _optStyles[TreeStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteDescription(screenBuffer);
            WriteTreeList(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[TreeStyles.Error]);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[TreeStyles.Prompt]);
            string answer;
            if (ResultCtrl!.Value.IsAborted)
            {
                answer = OptionsControl.ShowMessageAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
            }
            else
            {
                T? value = ResultCtrl.Value.Content;
                answer = value is null ? string.Empty : FormatAnswerForValue(value);
            }
            screenBuffer.WriteLine(answer, _optStyles[TreeStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
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
            VNode node = _nodes[index];
            if (!node.HasChildren || node.IsExpanded)
            {
                return;
            }
            node.IsExpanded = true;
            InsertChildren(index);
        }

        private void Collapse(int index)
        {
            VNode node = _nodes[index];
            if (!node.HasChildren || !node.IsExpanded)
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
            VNode parent = _nodes[parentIndex];
            List<TreeNode> src = parent.Source.Children;
            if (src.Count == 0)
            {
                return;
            }
            var children = new List<VNode>(src.Count);
            for (int i = 0; i < src.Count; i++)
            {
                children.Add(new VNode(NextId(), src[i], parent.Depth + 1, isLast: i == src.Count - 1));
            }
            _nodes.InsertRange(parentIndex + 1, children);
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

            if (_modeView == ModeView.Filter)
            {
                // In filter mode we search across the whole tree; the paginator gets the flat
                // projection and matches against each node's full path text.
                EnsureFlatAllBuilt();
                // StartsWith matches against the node's own name (leaf segment) so that typing
                // the beginning of a node name works naturally.  Contains matches against the
                // full ancestor path so the user can narrow results by any path segment.
                Func<VNode, string> filterKey = _filterType == FilterMode.StartsWith
                    ? (item) => _textSelector!(item.Value) ?? string.Empty
                    : (item) => _flatDisplayCache![item.UniqueId];

                _localpaginator = new Paginator<VNode>(
                    _filterType,
                    _flatAll!,
                    _effectivePageSize,
                    Optional<VNode>.Empty(),
                    (a, b) => a.UniqueId == b.UniqueId,
                    filterKey);
            }
            else
            {
                _localpaginator = new Paginator<VNode>(
                    FilterMode.Disabled,
                    _nodes,
                    _effectivePageSize,
                    Optional<VNode>.Empty(),
                    (a, b) => a.UniqueId == b.UniqueId,
                    (item) => _textSelector!(item.Value) ?? string.Empty);
            }

            if (keepIndex >= 0 && keepIndex < (_modeView == ModeView.Filter ? _flatAll!.Count : _nodes.Count))
            {
                _localpaginator.EnsureVisibleIndex(keepIndex);
            }
            else
            {
                _localpaginator.FirstItem();
            }
        }

        /// <summary>
        /// Builds a flat projection of every node in the user tree (root + all descendants) and
        /// caches its full-path display text so the paginator can filter by path. Built lazily the
        /// first time the user enters filter mode; the same VNode identities are reused so that
        /// the paginator can restore selection correctly when navigating back.
        /// </summary>
        private void EnsureFlatAllBuilt()
        {
            if (_flatAll is not null)
            {
                return;
            }
            _flatAll = [];
            _flatDisplayCache = [];
            // Reuse the root VNode already at _nodes[0] so identity survives across modes.
            var rootV = _nodes[0];
            AddFlat(rootV);

            void AddFlat(VNode v)
            {
                _flatAll.Add(v);
                _flatDisplayCache[v.UniqueId] = BuildFullPath(v.Source);
                for (int i = 0; i < v.Source.Children.Count; i++)
                {
                    // Materialize a synthetic VNode when we haven't visited this branch yet;
                    // depth/isLast are only used for tree rendering and are irrelevant in filter
                    // view (we render each match as a plain full-path line).
                    var child = new VNode(NextId(), v.Source.Children[i], v.Depth + 1, isLast: i == v.Source.Children.Count - 1);
                    AddFlat(child);
                }
            }
        }

        private void RecomputeAncestorMasks()
        {
            long chainMask = 0;
            for (int i = 0; i < _nodes.Count; i++)
            {
                VNode n = _nodes[i];
                int depth = n.Depth;
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

        private void SaveHistory(T value)
        {
            if (_historyOptions == null)
            {
                return;
            }
            string serializedValue = JsonSerializer.Serialize(value);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;
        }

        /// <summary>
        /// Applies the current text held by <see cref="_filterBuffer"/> to the paginator. When the
        /// buffer becomes empty the control transitions back to select mode and rebuilds the lazy
        /// tree view (preserving the previously expanded branches).
        /// </summary>
        private void UpdateFilterFromBuffer()
        {
            string filter = _filterBuffer!.ToString();
            if (string.IsNullOrEmpty(filter))
            {
                _modeView = ModeView.Select;
                _lastinput = string.Empty;
                RebuildPaginator(selectFirst: false);
                return;
            }

            if (!filter.Equals(_lastinput, StringComparison.OrdinalIgnoreCase) || _localpaginator is null)
            {
                RebuildPaginator(selectFirst: true);
                _localpaginator!.UpdateFilter(filter);
            }
            _lastinput = filter;
        }

        private static bool TryDeserializeHistoryValue(string value, out T? result)
        {
            result = default;
            try
            {
                result = JsonSerializer.Deserialize<T>(value);
                return result is not null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validates that <typeparamref name="T"/> can reasonably be round-tripped through
        /// <see cref="JsonSerializer"/>. Only invoked when history is enabled.
        /// </summary>
        private static void ValidateHistorySerializable()
        {
            Type t = typeof(T);
            if (t.IsPrimitive || t.IsEnum || t == typeof(string)) return;
            if (t == typeof(decimal) || t == typeof(DateTime) || t == typeof(DateTimeOffset)
                || t == typeof(DateOnly) || t == typeof(TimeOnly) || t == typeof(TimeSpan)
                || t == typeof(Guid)) return;

            bool hasKnownAttr = t.GetCustomAttributes(inherit: true).Any(a =>
            {
                string? fn = a.GetType().FullName;
                return a is SerializableAttribute
                    || fn == "System.Runtime.Serialization.DataContractAttribute"
                    || fn == "System.Text.Json.Serialization.JsonSerializableAttribute";
            });
            if (hasKnownAttr) return;

            // Record types expose a compiler-generated <Clone>$ method.
            if (t.GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null) return;

            // Fallback: a public parameterless constructor is enough for STJ default behavior.
            if (t.GetConstructor(Type.EmptyTypes) != null) return;

            throw new InvalidOperationException(
                $"Type '{t.FullName}' cannot be safely serialized to history. " +
                "Decorate it with [Serializable]/[DataContract] or provide a public parameterless constructor.");
        }

        #endregion

        #region rendering

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Filter)
            {
                WriteAnswerFilter(screenBuffer);
                return;
            }

            VNode? selected = _localpaginator?.SelectedItem;
            string text = selected is null ? string.Empty : FormatAnswer(selected);
            if (_updatePosAnswerBuffer)
            {
                _answerBuffer!.LoadPrintable(text);
                _answerBuffer.ToHome();
            }
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_answerBuffer!, promptWidth);
            screenBuffer.Write(visibleLeft, _optStyles[TreeStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine(visibleRight, _optStyles[TreeStyles.Answer]);
        }

        private void WriteAnswerFilter(BufferScreen screenBuffer)
        {
            Style found = _localpaginator!.TotalCount == 0 ? _optStyles[TreeStyles.Error] : _optStyles[TreeStyles.Answer];
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_filterBuffer!, promptWidth);
            screenBuffer.Write(visibleLeft, found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(visibleRight, found);
            screenBuffer.WriteLine($" ({PromptPlusResources.Filter})", _optStyles[TreeStyles.ChildsCount]);
        }

        private string FormatAnswer(VNode node)
        {
            if (_showFullPath)
            {
                return BuildFullPath(node.Source);
            }
            if (node.IsRoot)
            {
                return _textSelector!(node.Value) ?? string.Empty;
            }
            TreeNode? parent = node.Source.ParentNode;
            string name = _textSelector!(node.Value) ?? string.Empty;
            if (parent is null)
            {
                return name;
            }
            string parentName = _textSelector!(parent.Value) ?? string.Empty;
            return $"{parentName}{_pathSep}{name}";
        }

        private string FormatAnswerForValue(T value)
        {
            // Locate the source node so we can build the path if needed.
            List<TreeNode>? path = _root is null ? null : FindPath(_root, value);
            if (path is null || path.Count == 0)
            {
                return _textSelector!(value) ?? string.Empty;
            }
            TreeNode leaf = path[^1];
            if (_showFullPath)
            {
                return BuildFullPath(leaf);
            }
            if (path.Count == 1)
            {
                return _textSelector!(leaf.Value) ?? string.Empty;
            }
            TreeNode parent = path[^2];
            return $"{_textSelector!(parent.Value)}{_pathSep}{_textSelector!(leaf.Value)}";
        }

        private string BuildFullPath(TreeNode node)
        {
            var parts = new List<string>();
            TreeNode? cursor = node;
            while (cursor != null)
            {
                parts.Add(_textSelector!(cursor.Value) ?? string.Empty);
                cursor = cursor.ParentNode;
            }
            parts.Reverse();
            return string.Join(_pathSep, parts);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            VNode? selected = _localpaginator?.SelectedItem;
            if (selected is not null)
            {
                if (_changeDescriptionAsync is not null)
                {
                    desc = _changeDescriptionAsync.Invoke(selected.Value)
                        .ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else if (_changeDescription is not null)
                {
                    desc = _changeDescription.Invoke(selected.Value);
                }
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[TreeStyles.Description]);
            }
        }

        private void WriteTreeList(BufferScreen screenBuffer)
        {
            ArraySegment<VNode> subset = _localpaginator!.GetPageData();
            VNode? selectedItem = _localpaginator.SelectedIndex >= 0 ? _localpaginator.SelectedItem : null;

            foreach (VNode node in subset)
            {
                bool isSelected = selectedItem != null && node.UniqueId == selectedItem.UniqueId;
                Style lineStyle = isSelected ? _optStyles[TreeStyles.Selected] : _optStyles[TreeStyles.UnSelected];

                screenBuffer.Write(isSelected ? GetSymbol(SymbolType.Selector) : " ", ConsoleHandler.CurrentStyle);

                if (_modeView == ModeView.Filter)
                {
                    // Filter view: render each match as a flat "full path" line, no tree glyphs.
                    string pathText = _flatDisplayCache is not null && _flatDisplayCache.TryGetValue(node.UniqueId, out string? p)
                        ? p
                        : (_textSelector!(node.Value) ?? string.Empty);
                    Style nameStyle = isSelected ? lineStyle
                        : node.IsRoot ? _optStyles[TreeStyles.Root] : _optStyles[TreeStyles.Node];
                    screenBuffer.Write($" {pathText}", nameStyle);

                    if (_extraInfoSelector != null || _extraInfoSelectorAsync != null)
                    {
                        string? extra = GetExtraInfoText(node.Value);
                        if (!string.IsNullOrEmpty(extra))
                        {
                            screenBuffer.Write($"  {extra}", _optStyles[TreeStyles.ChildsCount]);
                        }
                    }
                    screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
                    continue;
                }

                if (node.Depth > 0)
                {
                    for (int d = 1; d < node.Depth; d++)
                    {
                        screenBuffer.Write((node.AncestorMask & (1L << d)) != 0
                            ? GetSymbol(SymbolType.TreeLinevertical)
                            : GetSymbol(SymbolType.TreeLinespace), _optStyles[TreeStyles.Lines]);
                    }
                    screenBuffer.Write(node.IsLast
                        ? GetSymbol(SymbolType.TreeLinecorner)
                        : GetSymbol(SymbolType.TreeLinecross), _optStyles[TreeStyles.Lines]);
                }

                if (node.HasChildren)
                {
                    screenBuffer.Write(node.IsExpanded
                        ? GetSymbol(SymbolType.Expanded)
                        : GetSymbol(SymbolType.Collapsed), _optStyles[TreeStyles.ExpandSymbol]);
                    screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                }

                Style nameStyleSelect = node.IsRoot
                    ? _optStyles[TreeStyles.Root]
                    : node.HasChildren ? _optStyles[TreeStyles.Node] : _optStyles[TreeStyles.Node];
                if (isSelected)
                {
                    nameStyleSelect = lineStyle;
                }
                screenBuffer.Write(_textSelector!(node.Value) ?? string.Empty, nameStyleSelect);

                if (_extraInfoSelector != null || _extraInfoSelectorAsync != null)
                {
                    string? extra = GetExtraInfoText(node.Value);
                    if (!string.IsNullOrEmpty(extra))
                    {
                        screenBuffer.Write($"  {extra}", _optStyles[TreeStyles.ChildsCount]);
                    }
                }

                screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
            }

            if (_localpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount)!;
                screenBuffer.WriteLine(template, _optStyles[TreeStyles.Pagination]);
            }
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
            screenBuffer.WriteLine(tooltip, _optStyles[TreeStyles.Tooltips]);
        }

        private string? GetExtraInfoText(T value)
        {
            if (_extraInfoSelectorAsync is not null)
            {
                return _extraInfoSelectorAsync.Invoke(value)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            if (_extraInfoSelector is not null)
            {
                return _extraInfoSelector.Invoke(value);
            }
            return null;
        }

        private string GetTooltipToggle()
        {
            string[] entries = _toggerTooptips[_modeView];
            if (_indexTooptip >=  entries.Length)
            {
                _indexTooptip = 0;
            }
            return entries.Length == 0 ? string.Empty : entries[_indexTooptip];
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips = [GetTooltipNavigate(mode)];

                // Paging always applies.
                lsttooltips.Add(PromptPlusResources.TooltipPages);

                if (mode == ModeView.Select)
                {
                    // Expand/collapse and Tab-driven descent only apply while the tree is visible.
                    lsttooltips.Add(PromptPlusResources.TooltipExpandCollapse);

                    if (_filterType != FilterMode.Disabled && !_viewOnly)
                    {
                        // Any printable key jumps into filter mode.
                        lsttooltips.Add(PromptPlusResources.TooltipFilter);
                    }
                    else if (!_viewOnly)
                    {
                        // No filter: the printable-key gesture is the jump-by-initial helper.
                        lsttooltips.Add(PromptPlusResources.TooltipJump);
                    }

                    // Full-path toggle only affects the tree/answer, not the filter box.
                    lsttooltips.Add($"{ConfigPrompt.HotKeyToggleFullPath}:{PromptPlusResources.TooltipToggleFullPath}");
                }

                if (OptionsControl.EnabledAbortKeyValue)
                {
                    lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
                }
                lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");

                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        /// <summary>
        /// First tooltip entry: composes the "how to navigate/finish" hints for the current mode.
        /// In select mode we mention Enter (when selection is allowed), the base arrow navigation
        /// and the tab-driven descent. In filter mode we drop tab and expand/collapse hints (they
        /// are not available) and keep only Enter (when allowed) plus arrow navigation.
        /// </summary>
        private string GetTooltipNavigate(ModeView mode)
        {
            StringBuilder tooltip = new();
            if (!_viewOnly)
            {
                tooltip.Append(PromptPlusResources.TooltipEnterFinish);
                tooltip.Append('.');
            }
            if (mode == ModeView.Select)
            {
                tooltip.Append(PromptPlusResources.TooltipTreeTab);
                tooltip.Append('.');
            }
            tooltip.Append(PromptPlusResources.TooltipBaseNavegate);
            tooltip.Append('.');
            return tooltip.ToString();
        }

        #endregion
    }
}
