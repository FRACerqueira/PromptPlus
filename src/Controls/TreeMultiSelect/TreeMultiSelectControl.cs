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

namespace PromptPlusLibrary.Controls.TreeMultiSelect
{
    /// <inheritdoc/>
    internal sealed class TreeMultiSelectControl<T> : BaseControlPrompt<T[]>, ITreeMultiSelectControl<T>
    {
        private const int ReservedTemplateLines = 7;

        private static readonly CompositeFormat s_countCheckFormat =
            CompositeFormat.Parse(PromptPlusResources.TooltipCountCheck);
        private static readonly CompositeFormat s_minSelectionFormat =
            CompositeFormat.Parse(PromptPlusResources.MultiSelectMinSelection);
        private static readonly CompositeFormat s_maxSelectionFormat =
            CompositeFormat.Parse(PromptPlusResources.MultiSelectMaxSelection);

        // ─── user tree ──────────────────────────────────────────────────────────────
        private sealed class TreeNode(T value, TreeNode? parent, bool disabled = false, bool check = false) : ITreeMultiSelectNode<T>
        {
            public T Value { get; } = value;
            public TreeNode? ParentNode { get; } = parent;
            public List<TreeNode> Children { get; } = [];
            public bool Disabled { get; } = disabled;
            // Construction-time pre-check flag, applied once in InitControl (ApplyConstructionTimeChecks)
            // via the same SetCheckedOnSource(force:true) path used by Default — additive with it.
            public bool Checked { get; } = check;

            ITreeNode<T>? ITreeNode<T>.Parent => ParentNode;
            T ITreeNode<T>.Value => Value;

            // ITreeNode<T> (shared with TreeSelectControl) has no `check` concept — forward with check:false.
            ITreeNode<T> ITreeNode<T>.AddLast(T v, bool disable) => AddLast(v, disable, check: false);
            ITreeNode<T> ITreeNode<T>.AddFirst(T v, bool disable) => AddFirst(v, disable, check: false);

            public ITreeMultiSelectNode<T> AddLast(T v, bool disable = false, bool check = false)
            {
                var n = new TreeNode(v, this, disable, check);
                Children.Add(n);
                return n;
            }

            public ITreeMultiSelectNode<T> AddFirst(T v, bool disable = false, bool check = false)
            {
                var n = new TreeNode(v, this, disable, check);
                Children.Insert(0, n);
                return n;
            }
        }

        // ─── check state ────────────────────────────────────────────────────────────
        /// <summary>Tri-state for each visible node.</summary>
        private enum CheckState { Unchecked, Checked, Indeterminate }

        // ─── visible node ───────────────────────────────────────────────────────────
        private sealed class VNode(string uniqueId, TreeNode source, int depth, bool isLast)
        {
            public string UniqueId { get; } = uniqueId;
            public TreeNode Source { get; } = source;
            public int Depth { get; } = depth;
            public bool IsLast { get; set; } = isLast;
            public bool IsExpanded { get; set; }
            public bool IsRoot { get; init; }
            public bool HasChildren => Source.Children.Count > 0;
            public bool Disabled => Source.Disabled;
            public T Value => Source.Value;
            public long AncestorMask;
            public CheckState Check { get; set; } = CheckState.Unchecked;
        }

        // ─── fields ─────────────────────────────────────────────────────────────────
        private readonly Dictionary<TreeMultiSelectStyles, Style> _optStyles;
        private readonly List<VNode> _nodes = [];
        private Paginator<VNode>? _localpaginator;

        private enum ModeView { Select, Filter }
        private ModeView _modeView = ModeView.Select;
        private FilterMode _filterType = FilterMode.Disabled;
        private EmacsConsoleBuffer? _filterBuffer;
        private string _lastinput = string.Empty;
        private List<VNode>? _flatAll;
        private Dictionary<string, string>? _flatDisplayCache;

        // Checked state is stored by UniqueId so it survives collapse/expand.
        // For the flat projection VNodes we also register here so filter-mode marks
        // are always reflected back into the tree-mode VNodes (same Source reference).
        private readonly HashSet<string> _checkedSourceIds = [];   // keyed on Source identity hash

        private TreeNode? _root;
        private Func<T, string>? _textSelector;
        private Func<T, string?>? _extraInfoSelector;
        private Func<T, Task<string?>>? _extraInfoSelectorAsync;
        private Func<T, T, bool>? _equals;
        private Func<T, string>? _changeDescription;
        private Func<T, Task<string>>? _changeDescriptionAsync;
        private char _pathSep = '/';
        private bool _checkLeafOnly;
        private bool _showFullPath;
        private bool _cascadeCheck = true;
        private bool _recursiveMarkWithCtrlSpace;
        private bool _viewOnly;
        private Func<T, (bool, string?)>? _predicatevalidcheck;
        private Func<T, Task<(bool, string?)>>? _predicatevalidcheckAsync;
        private byte _pageSize;
        private int _effectivePageSize;
        private int _sequence;
        private IEnumerable<T>? _defaultValues;
        private bool _hasDefault;
        private bool _useDefaultHistory = true;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;
        private int _minRange;
        private int? _maxRange;

        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Select, [] },
            { ModeView.Filter, [] }
        };
        private int _indexTooptip;

        public TreeMultiSelectControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions)
            : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<TreeMultiSelectStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
        }

        #region ITreeMultiSelectControl<T>

        public ITreeMultiSelectControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        public ITreeMultiSelectControl<T> Styles(TreeMultiSelectStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public ITreeMultiSelectControl<T> Root(T value, bool disable = false, bool check = false)
        {
            ArgumentNullException.ThrowIfNull(value);
            _root = new TreeNode(value, parent: null, disable, check);
            return this;
        }

        private TreeNode RequireRoot()
        {
            if (_root == null) throw new InvalidOperationException("Root must be set before adding nodes.");
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
            while (cursor != null) { if (ReferenceEquals(cursor, _root)) return true; cursor = cursor.ParentNode; }
            return false;
        }

        public ITreeMultiSelectNode<T> AddLast(T value, bool disable = false, bool check = false)
        {
            ArgumentNullException.ThrowIfNull(value);
            return RequireRoot().AddLast(value, disable, check);
        }

        public ITreeMultiSelectNode<T> AddFirst(T value, bool disable = false, bool check = false)
        {
            ArgumentNullException.ThrowIfNull(value);
            return RequireRoot().AddFirst(value, disable, check);
        }

        public ITreeMultiSelectNode<T> AddAfter(ITreeNode<T> node, T value, bool disable = false, bool check = false)
        {
            ArgumentNullException.ThrowIfNull(value);
            TreeNode target = Unwrap(node);
            if (!BelongsToTree(target) || target.ParentNode is null)
                throw new InvalidOperationException("The provided node does not belong to this tree or is the root.");
            TreeNode parent = target.ParentNode;
            int idx = parent.Children.IndexOf(target);
            var created = new TreeNode(value, parent, disable, check);
            parent.Children.Insert(idx + 1, created);
            return created;
        }

        public ITreeMultiSelectNode<T> AddBefore(ITreeNode<T> node, T value, bool disable = false, bool check = false)
        {
            ArgumentNullException.ThrowIfNull(value);
            TreeNode target = Unwrap(node);
            if (!BelongsToTree(target) || target.ParentNode is null)
                throw new InvalidOperationException("The provided node does not belong to this tree or is the root.");
            TreeNode parent = target.ParentNode;
            int idx = parent.Children.IndexOf(target);
            var created = new TreeNode(value, parent, disable, check);
            parent.Children.Insert(idx, created);
            return created;
        }

        public ITreeMultiSelectControl<T> TextSelector(Func<T, string> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);
            _textSelector = selector;
            return this;
        }

        /// <inheritdoc/>
        public ITreeMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfoSelector = extraInfoNode;
            _extraInfoSelectorAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITreeMultiSelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
        {
            ArgumentNullException.ThrowIfNull(extraInfoNode);
            _extraInfoSelectorAsync = extraInfoNode;
            _extraInfoSelector = null;
            return this;
        }

        public ITreeMultiSelectControl<T> PathSeparator(char value) { _pathSep = value; return this; }

        public ITreeMultiSelectControl<T> PageSize(byte value) { _pageSize = value; return this; }

        public ITreeMultiSelectControl<T> CheckLeafOnly(bool value = true) { _checkLeafOnly = value; return this; }

        public ITreeMultiSelectControl<T> ShowFullPath(bool value = true) { _showFullPath = value; return this; }

        public ITreeMultiSelectControl<T> CascadeCheck(bool value = true) { _cascadeCheck = value; return this; }

        public ITreeMultiSelectControl<T> RecursiveMarkWithCtrlSpace(bool value = true) { _recursiveMarkWithCtrlSpace = value; return this; }

        public ITreeMultiSelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);
            _equals = comparer;
            return this;
        }

        public ITreeMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(values);
            _defaultValues = values;
            _hasDefault = true;
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        public ITreeMultiSelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null)
        {
            ArgumentNullException.ThrowIfNull(filename);
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename cannot be empty or whitespace.", nameof(filename));
            _historyOptions = new HistoryOptions(filename);
            options?.Invoke(_historyOptions);
            return this;
        }

        public ITreeMultiSelectControl<T> ViewOnly(bool value = true) { _viewOnly = value; return this; }

        public ITreeMultiSelectControl<T> ChangeDescription(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        public ITreeMultiSelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        public ITreeMultiSelectControl<T> Filter(FilterMode value)
        {
            _filterType = value;
            return this;
        }

        public ITreeMultiSelectControl<T> PredicateChecked(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = (input) => (validselect(input), (string?)null);
            _predicatevalidcheckAsync = null;
            return this;
        }

        public ITreeMultiSelectControl<T> PredicateChecked(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = validselect;
            _predicatevalidcheckAsync = null;
            return this;
        }

        public ITreeMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidcheck = null;
            return this;
        }

        public ITreeMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = validselect;
            _predicatevalidcheck = null;
            return this;
        }

        public ITreeMultiSelectControl<T> Range(int minvalue, int? maxvalue = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(minvalue);
            if (maxvalue.HasValue && maxvalue.Value < minvalue) throw new ArgumentOutOfRangeException(nameof(maxvalue));
            _minRange = minvalue;
            _maxRange = maxvalue;
            return this;
        }

        public ITreeMultiSelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITreeMultiSelectControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T1 item in items) interactionAction.Invoke(item, this);
            return this;
        }

        public ITreeMultiSelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITreeMultiSelectControl<T>, Task> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T1 item in items) interactionAction.Invoke(item, this).ConfigureAwait(false).GetAwaiter().GetResult();
            return this;
        }

        #endregion

        // ─── lifecycle ──────────────────────────────────────────────────────────────

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_root is null) throw new InvalidOperationException("TreeMultiSelect control requires a Root value.");
            if (_textSelector is null) throw new InvalidOperationException("TreeMultiSelect control requires a TextSelector.");
            if (_equals is null) throw new InvalidOperationException("TreeMultiSelect control requires DefaultMatchBy.");

            _filterBuffer = new(false, CaseOptions.Any, ConsoleHandler.EnabledEmacs , (_) => true);
            _modeView = ModeView.Select;
            _lastinput = string.Empty;
            _flatAll = null;
            _flatDisplayCache = null;
            _checkedSourceIds.Clear();
            // Construction-time `check: true` nodes, applied before Default/history so both
            // layers are purely additive (whichever marks a node checked, it stays checked).
            ApplyConstructionTimeChecks(_root);

            _nodes.Clear();
            _sequence = 0;

            if (_viewOnly) _historyOptions = null;

            // Resolve defaults (history overrides explicit default when enabled).
            IEnumerable<T>? targets = _hasDefault ? _defaultValues : null;

            if (_historyOptions != null)
            {
                ValidateHistorySerializable();
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0
                    && TryDeserializeHistoryValues(_itemHistories[0].History, out T[]? historyValues)
                    && historyValues is { Length: > 0 })
                {
                    targets = historyValues;
                }
            }

            VNode rootV = new(NextId(), _root, depth: 0, isLast: true) { IsRoot = true, IsExpanded = true };
            _nodes.Add(rootV);
            InsertChildren(0);
            // InsertChildren already computes each inserted child's Check via ComputeCheck, but
            // rootV itself is created above with the CheckState.Unchecked default — recompute
            // once so a construction-time check on the root (or its descendants) renders
            // correctly on the very first frame, even with no Default/history targets.
            RefreshNodeChecks();

            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            RebuildPaginator(selectFirst: true);

            if (targets != null)
            {
                foreach (T target in targets)
                {
                    ExpandAndCheckTarget(target);
                }
            }

            // Same immediate-error convention as ITreeSelectControl: if the cursor rests on a
            // disabled node once init is done, surface it right away rather than waiting for
            // the first navigation key.
            if (!_viewOnly && _localpaginator!.SelectedItem?.Disabled == true)
            {
                SetError(PromptPlusResources.SelectionDisabled);
            }

            LoadTooltipToggle();
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
                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<T[]>([], true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;
                    VNode? selected = _localpaginator!.SelectedItem;

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<T[]>([], true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && selected != null)
                    {
                        _indexTooptip = 0;
                        if (_viewOnly)
                        {
                            // In ViewOnly mode return the pre-checked defaults (no user changes allowed).
                            ResultCtrl = new ResultPrompt<T[]>(CollectChecked(), false);
                            break;
                        }
                        T[] checkedValues = CollectChecked();
                        if (checkedValues.Length < _minRange)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_minSelectionFormat, _minRange));
                            break;
                        }
                        if (_maxRange.HasValue && checkedValues.Length > _maxRange.Value)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_maxSelectionFormat, _maxRange.Value));
                            break;
                        }
                        ResultCtrl = new ResultPrompt<T[]>(checkedValues, false);
                        SaveHistory(checkedValues);
                        break;
                    }
                    else if (keyinfo.IsPressSpaceKey() && selected != null && !_viewOnly)
                    {
                        // Space = toggle check on the focused node.
                        // When RecursiveMarkWithCtrlSpace is enabled, Space only toggles the single node;
                        // otherwise it uses cascade behavior (if CascadeCheck is also enabled).
                        _indexTooptip = 0;
                        if (_recursiveMarkWithCtrlSpace)
                        {
                            ToggleCheckSingleNode(selected);
                        }
                        else
                        {
                            ToggleCheck(selected);
                        }
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (_recursiveMarkWithCtrlSpace && keyinfo.IsPressCtrlSpaceKey() && selected != null && !_viewOnly)
                    {
                        // Ctrl+Space performs the recursive toggle when that action was moved off plain space.
                        _indexTooptip = 0;
                        ToggleCheck(selected);
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (ConfigPrompt.HotKeyToggleAll.Equals(keyinfo) && !_viewOnly)
                    {
                        _indexTooptip = 0;
                        ToggleAllVisible();
                        SetRangeValidationErrorIfNeeded();
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips[_modeView].Length) _indexTooptip = 0;
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
                    else if (keyinfo.IsPressExpandKey() && _modeView == ModeView.Select
                             && selected is { IsExpanded: false } && selected.HasChildren)
                    {
                        Expand(_localpaginator.CurrentIndex);
                        RebuildPaginator(selectFirst: false);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCollapseKey() && _modeView == ModeView.Select
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
                            if (!selected.IsExpanded) { Expand(current); RebuildPaginator(selectFirst: false); }
                            if (current + 1 < _nodes.Count && _nodes[current + 1].Depth > selected.Depth)
                                _localpaginator.EnsureVisibleIndex(current + 1);
                        }
                        else
                        {
                            if (_localpaginator.IsLastPageItem) _localpaginator.NextPage(IndexOption.FirstItem);
                            else _localpaginator.NextItem();
                        }
                        SetSelectionDisabledErrorIfNeeded();
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
                            if (_nodes[parentIndex].IsExpanded) { Collapse(parentIndex); RebuildPaginator(selectFirst: false); }
                        }
                        else
                        {
                            if (_localpaginator.IsFirstPageItem) _localpaginator.PreviousPage(IndexOption.LastItem);
                            else _localpaginator.PreviousItem();
                        }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressDownArrowKey())
                    {
                        if (_localpaginator!.IsLastPageItem) _localpaginator.NextPage(IndexOption.FirstItem);
                        else _localpaginator.NextItem();
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem) _localpaginator.PreviousPage(IndexOption.LastItem);
                        else _localpaginator.PreviousItem();
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        _localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages);
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        _localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages);
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlHomeKey())
                    {
                        _localpaginator!.Home();
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        _localpaginator!.End();
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (TryAnswerViewportNavigation(keyinfo))
                    {
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
                             && !keyinfo.IsPressSpaceKey()
                             && _filterBuffer!.IsPrintable(keyinfo.KeyChar))
                    {
                        var keifilter = keyinfo;
                        if (keifilter.IsPressFilterActivationKey())
                            keifilter = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
                        if (_filterBuffer!.TryAcceptedReadlineConsoleKey(keifilter))
                        {
                            _modeView = ModeView.Filter;
                            UpdateFilterFromBuffer();
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType == FilterMode.Disabled && selected != null
                             && !char.IsControl(keyinfo.KeyChar) && keyinfo.KeyChar != '\0')
                    {
                        string keyChar = keyinfo.KeyChar.ToString();
                        int start = _localpaginator!.CurrentIndex;
                        int index = FindNodeIndexStartingWith(keyChar, start + 1);
                        if (index < 0 && start >= 0) index = FindNodeIndexStartingWith(keyChar, 0);
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
            WritePrompt(screenBuffer, _optStyles[TreeMultiSelectStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteDescription(screenBuffer);
            WriteTreeList(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[TreeMultiSelectStyles.Error]);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[TreeMultiSelectStyles.Prompt]);
            string answer;
            if (ResultCtrl!.Value.IsAborted)
            {
                answer = OptionsControl.ShowMessageAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
            }
            else
            {
                T[] values = ResultCtrl.Value.Content ?? [];
                answer = values.Length == 0
                    ? string.Empty
                    : string.Join(',', values.Select(v => FormatAnswerForValue(v)));
            }
            screenBuffer.WriteLine(answer, _optStyles[TreeMultiSelectStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            _nodes.Clear();
            _checkedSourceIds.Clear();
            _flatAll = null;
            _flatDisplayCache = null;
        }

        // ─── check logic ────────────────────────────────────────────────────────────

        /// <summary>
        /// Toggles the checked state of every currently visible checkable node. If any visible
        /// node is unchecked, all are checked; otherwise all are unchecked.
        /// When CascadeCheck is on, container nodes cascade to all descendants (including
        /// collapsed subtrees), matching the behaviour of ToggleCheck.
        /// </summary>
        private void ToggleAllVisible()
        {
            // Collect visible candidates (skip root; respect CheckLeafOnly).
            var candidates = _nodes
                .Where(n => !n.IsRoot && (!_checkLeafOnly || !n.HasChildren))
                .ToList();
            if (candidates.Count == 0) return;

            // If any candidate (or its cascaded descendants) is unchecked, check all; otherwise uncheck all.
            bool check = candidates.Any(n => n.Check != CheckState.Checked);

            foreach (VNode v in candidates)
            {
                // Disabled nodes are never touched directly by the mass toggle in either
                // direction: checking silently skips them (like the predicate), and unchecking
                // leaves them exactly as they are (e.g. force-checked via Default) — same rule as
                // MultiSelect/MultiTable's F2.
                if (v.Disabled) continue;

                if (check)
                {
                    // Mass check: silently skip nodes rejected by the predicate.
                    if (_predicatevalidcheck != null || _predicatevalidcheckAsync != null)
                    {
                        (bool ok, _) = _predicatevalidcheckAsync is not null
                            ? _predicatevalidcheckAsync.Invoke(v.Value).ConfigureAwait(false).GetAwaiter().GetResult()
                            : _predicatevalidcheck!.Invoke(v.Value);
                        if (!ok) continue;
                    }
                }
                // When CascadeCheck is on, delegate to SetCheckedOnSource so all descendants
                // (including those in collapsed subtrees) are marked/unmarked consistently with
                // the behaviour of ToggleCheck. When off, only touch the visible node itself.
                if (_cascadeCheck)
                {
                    SetCheckedOnSource(v.Source, check);
                }
                else
                {
                    string id = RuntimeId(v.Source);
                    if (check) _checkedSourceIds.Add(id);
                    else _checkedSourceIds.Remove(id);
                }
            }
            RefreshNodeChecks();
        }

        /// <summary>
        /// Toggles the check state of <paramref name="node"/>. Validates via predicate if set.
        /// When cascade is on, propagates to all descendants. Then recalculates ancestors.
        /// </summary>
        private void ToggleCheck(VNode node)
        {
            if (node.Disabled)
            {
                SetError(PromptPlusResources.SelectionDisabled);
                return;
            }
            if (_checkLeafOnly && node.HasChildren)
            {
                SetError(PromptPlusResources.SelectionDisabled);
                return;
            }

            // Determine new state first: Indeterminate and Unchecked both → Checked; Checked →
            // Unchecked. The predicate only gates checking — unchecking never needs it.
            bool newChecked = node.Check != CheckState.Checked;

            if (newChecked && (_predicatevalidcheck != null || _predicatevalidcheckAsync != null))
            {
                (bool ok, string? message) = _predicatevalidcheckAsync is not null
                    ? _predicatevalidcheckAsync.Invoke(node.Value).ConfigureAwait(false).GetAwaiter().GetResult()
                    : _predicatevalidcheck!.Invoke(node.Value);
                if (!ok)
                {
                    SetError(string.IsNullOrEmpty(message) ? PromptPlusResources.PredicateSelectInvalid : message!);
                    return;
                }
            }

            SetCheckedOnSource(node.Source, newChecked);

            // When cascade is active and the node is a container, auto-expand the subtree on check
            // so the user can see all newly-marked descendants, and auto-collapse on uncheck to keep
            // the tree tidy — mirroring the MultiFileControl behavior on folder toggle.
            if (_cascadeCheck && node.HasChildren && _modeView == ModeView.Select)
            {
                int idx = _localpaginator!.CurrentIndex;
                if (newChecked)
                {
                    ExpandSubtree(idx);
                }
                else if (node.IsExpanded)
                {
                    Collapse(idx);
                }
                RebuildPaginator(selectFirst: false);
                // Re-position the cursor on the toggled node after the paginator is rebuilt.
                int reIdx = IndexOfSource(node.Source);
                if (reIdx >= 0) _localpaginator!.EnsureVisibleIndex(reIdx);
            }

            RefreshNodeChecks();
        }

        /// <summary>
        /// Toggles the checked state of a single node without cascading to descendants, regardless
        /// of the <see cref="_cascadeCheck"/> setting. Used when <see cref="_recursiveMarkWithCtrlSpace"/>
        /// is enabled and the user presses plain Space (non-recursive toggle).
        /// </summary>
        private void ToggleCheckSingleNode(VNode node)
        {
            if (node.Disabled)
            {
                SetError(PromptPlusResources.SelectionDisabled);
                return;
            }
            if (_checkLeafOnly && node.HasChildren)
            {
                SetError(PromptPlusResources.SelectionDisabled);
                return;
            }

            // Toggle just this node (never cascade). Determine direction first — the
            // predicate only gates checking, never unchecking.
            bool newChecked = node.Check != CheckState.Checked;

            if (newChecked && (_predicatevalidcheck != null || _predicatevalidcheckAsync != null))
            {
                (bool ok, string? message) = _predicatevalidcheckAsync is not null
                    ? _predicatevalidcheckAsync.Invoke(node.Value).ConfigureAwait(false).GetAwaiter().GetResult()
                    : _predicatevalidcheck!.Invoke(node.Value);
                if (!ok)
                {
                    SetError(string.IsNullOrEmpty(message) ? PromptPlusResources.PredicateSelectInvalid : message!);
                    return;
                }
            }

            string id = RuntimeId(node.Source);
            if (newChecked)
                _checkedSourceIds.Add(id);
            else
                _checkedSourceIds.Remove(id);

            RefreshNodeChecks();
        }

        /// <summary>
        /// Walks the construction-time tree and force-checks every node created with
        /// <c>check: true</c> (<see cref="Root"/>/<see cref="AddLast"/>/<see cref="AddFirst"/>/
        /// <see cref="AddAfter"/>/<see cref="AddBefore"/>). Additive with <see cref="Default"/>/
        /// history — called before them in <see cref="InitControl"/> so neither layer clears
        /// the other; whichever mechanism marks a node checked, it stays checked.
        /// </summary>
        private void ApplyConstructionTimeChecks(TreeNode node)
        {
            if (node.Checked)
                SetCheckedOnSource(node, true, force: true);
            foreach (TreeNode child in node.Children)
                ApplyConstructionTimeChecks(child);
        }

        /// <summary>
        /// Sets the checked flag on <paramref name="source"/> (and optionally all descendants
        /// when <see cref="_cascadeCheck"/> is <c>true</c>) using the source-identity set.
        /// </summary>
        // A disabled node never has its own flag touched by an interactive/cascading toggle
        // (the cascade still passes through it to reach enabled descendants), unless <paramref
        // name="force"/> is set — used by Default/history pre-checking and by construction-time
        // `check: true` nodes (ApplyConstructionTimeChecks), both of which force-mark a disabled
        // node the same way IMultiSelectControl does.
        private void SetCheckedOnSource(TreeNode source, bool checkedState, bool force = false)
        {
            if (force || !source.Disabled)
            {
                string id = RuntimeId(source);
                if (checkedState) _checkedSourceIds.Add(id);
                else _checkedSourceIds.Remove(id);
            }

            if (_cascadeCheck)
            {
                foreach (TreeNode child in source.Children)
                    SetCheckedOnSource(child, checkedState);
            }
        }

        /// <summary>
        /// Recalculates <see cref="VNode.Check"/> for every visible node bottom-up and also
        /// updates the flat projection if it has been built.
        /// </summary>
        private void SetRangeValidationErrorIfNeeded()
        {
            int count = CollectChecked().Length;
            if (count < _minRange)
            {
                SetError(string.Format(CultureInfo.CurrentCulture, s_minSelectionFormat, _minRange));
                return;
            }
            if (_maxRange.HasValue && count > _maxRange.Value)
            {
                SetError(string.Format(CultureInfo.CurrentCulture, s_maxSelectionFormat, _maxRange.Value));
            }
        }

        private void RefreshNodeChecks()
        {
            foreach (VNode v in _nodes)
                v.Check = ComputeCheck(v.Source);

            if (_flatAll != null)
            {
                foreach (VNode v in _flatAll)
                    v.Check = ComputeCheck(v.Source);
            }
        }

        private CheckState ComputeCheck(TreeNode source)
        {
            if (source.Children.Count == 0)
                return _checkedSourceIds.Contains(RuntimeId(source))
                    ? CheckState.Checked
                    : CheckState.Unchecked;

            // When CascadeCheck is false, parent nodes can be checked independently
            // of their children, so check _checkedSourceIds directly.
            if (!_cascadeCheck)
            {
                return _checkedSourceIds.Contains(RuntimeId(source))
                    ? CheckState.Checked
                    : CheckState.Unchecked;
            }

            int checkedCount = 0, total = 0;
            CountDescendantLeaves(source, ref checkedCount, ref total);
            CheckState aggregate = total == 0 ? CheckState.Unchecked
                : checkedCount == total ? CheckState.Checked
                : checkedCount == 0 ? CheckState.Unchecked
                : CheckState.Indeterminate;

            if (source.Disabled)
            {
                // A disabled container's own flag can only ever be set via a Default/history
                // force-check — interactive toggles refuse to touch it (ToggleCheck/
                // ToggleCheckSingleNode/SetCheckedOnSource's non-force path all skip it). So if
                // it's set here, it's a deliberate force-check and wins outright. Otherwise the
                // container must never report as fully Checked just because a cascade happened
                // to pass through it and its descendants ended up all checked — it was passed
                // through, never actually confirmed itself.
                return _checkedSourceIds.Contains(RuntimeId(source))
                    ? CheckState.Checked
                    : aggregate == CheckState.Checked ? CheckState.Indeterminate : aggregate;
            }

            return aggregate;
        }

        private void CountDescendantLeaves(TreeNode node, ref int checkedCount, ref int total)
        {
            if (node.Children.Count == 0)
            {
                total++;
                if (_checkedSourceIds.Contains(RuntimeId(node))) checkedCount++;
                return;
            }
            foreach (TreeNode child in node.Children)
                CountDescendantLeaves(child, ref checkedCount, ref total);
        }

        /// <summary>Uses object identity as a stable string key.</summary>
        private static string RuntimeId(TreeNode source)
            => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(source).ToString(CultureInfo.InvariantCulture);

        /// <summary>Collects all leaf nodes (or all nodes when not leaf-only) that are checked.</summary>
        private T[] CollectChecked()
        {
            var result = new List<T>();
            CollectCheckedFrom(_root!, result);
            return [.. result];
        }

        // Uses ComputeCheck (the same rule the checkbox rendering uses) instead of reading
        // _checkedSourceIds directly: a cascaded check stamps every descendant's own id, so a
        // container's raw flag can go stale (e.g. one child gets individually unchecked later)
        // while nobody clears it. Deriving from ComputeCheck keeps the returned values in sync
        // with whatever the checkbox actually showed the user.
        private void CollectCheckedFrom(TreeNode node, List<T> result)
        {
            if (ComputeCheck(node) == CheckState.Checked && (!_checkLeafOnly || node.Children.Count == 0))
                result.Add(node.Value);
            foreach (TreeNode child in node.Children)
                CollectCheckedFrom(child, result);
        }

        // ─── tree model ─────────────────────────────────────────────────────────────

        private void ExpandSubtree(int index)
        {
            VNode node = _nodes[index];
            if (!node.HasChildren) return;
            int rootDepth = node.Depth;
            int i = index;
            while (i < _nodes.Count && (i == index || _nodes[i].Depth > rootDepth))
            {
                VNode current = _nodes[i];
                if (current.HasChildren && !current.IsExpanded)
                {
                    Expand(i);
                }
                i++;
            }
        }

        private string NextId() => (++_sequence).ToString(CultureInfo.CurrentCulture);

        private void Expand(int index)
        {
            VNode node = _nodes[index];
            if (!node.HasChildren || node.IsExpanded) return;
            node.IsExpanded = true;
            InsertChildren(index);
        }

        private void Collapse(int index)
        {
            VNode node = _nodes[index];
            if (!node.HasChildren || !node.IsExpanded) return;
            node.IsExpanded = false;
            int removeFrom = index + 1, removeCount = 0;
            for (int i = removeFrom; i < _nodes.Count && _nodes[i].Depth > node.Depth; i++) removeCount++;
            if (removeCount > 0) _nodes.RemoveRange(removeFrom, removeCount);
        }

        private void InsertChildren(int parentIndex)
        {
            VNode parent = _nodes[parentIndex];
            List<TreeNode> src = parent.Source.Children;
            if (src.Count == 0) return;
            var children = new List<VNode>(src.Count);
            for (int i = 0; i < src.Count; i++)
            {
                var v = new VNode(NextId(), src[i], parent.Depth + 1, isLast: i == src.Count - 1)
                {
                    Check = ComputeCheck(src[i])
                };
                children.Add(v);
            }
            _nodes.InsertRange(parentIndex + 1, children);
        }

        private int ParentIndex(int index)
        {
            int depth = _nodes[index].Depth;
            for (int i = index - 1; i >= 0; i--)
                if (_nodes[i].Depth < depth) return i;
            return -1;
        }

        private int FindNodeIndexStartingWith(string prefix, int startIndex)
        {
            for (int i = Math.Max(0, startIndex); i < _nodes.Count; i++)
            {
                string name = _textSelector!(_nodes[i].Value) ?? string.Empty;
                if (name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) return i;
            }
            return -1;
        }

        // View-only ignores Disabled entirely (same exemption as CheckLeafOnly/PredicateChecked
        // in the toggle helpers), so this is only ever called from non-view-only navigation.
        private void SetSelectionDisabledErrorIfNeeded()
        {
            if (!_viewOnly && _localpaginator?.SelectedItem?.Disabled == true)
            {
                SetError(PromptPlusResources.SelectionDisabled);
            }
        }

        private void ExpandAndCheckTarget(T target)
        {
            List<TreeNode>? path = FindPath(_root!, target);
            if (path is null || path.Count == 0) return;

            // Mark the leaf as checked. Force-bypass Disabled: Default/history pre-checking
            // force-marks a disabled node, same as IMultiSelectControl.
            SetCheckedOnSource(path[^1], true, force: true);

            // Expand ancestors so the checked node is visible.
            for (int i = 0; i < path.Count; i++)
            {
                int idx = IndexOfSource(path[i]);
                if (idx < 0) return;
                if (i < path.Count - 1)
                {
                    VNode v = _nodes[idx];
                    if (v.HasChildren && !v.IsExpanded) { Expand(idx); RebuildPaginator(selectFirst: false); }
                }
                else
                {
                    _localpaginator!.EnsureVisibleIndex(idx);
                }
            }
            RefreshNodeChecks();
        }

        private List<TreeNode>? FindPath(TreeNode from, T target)
        {
            if (_equals!(from.Value, target)) return [from];
            foreach (TreeNode child in from.Children)
            {
                List<TreeNode>? sub = FindPath(child, target);
                if (sub is not null) { sub.Insert(0, from); return sub; }
            }
            return null;
        }

        private int IndexOfSource(TreeNode source)
        {
            for (int i = 0; i < _nodes.Count; i++)
                if (ReferenceEquals(_nodes[i].Source, source)) return i;
            return -1;
        }

        // ─── paginator ──────────────────────────────────────────────────────────────

        private void RebuildPaginator(bool selectFirst)
        {
            RecomputeAncestorMasks();
            int keepIndex = selectFirst ? -1 : (_localpaginator?.CurrentIndex ?? -1);

            if (_modeView == ModeView.Filter)
            {
                EnsureFlatAllBuilt();
                Func<VNode, string> filterKey = _filterType == FilterMode.StartsWith
                    ? (item) => _textSelector!(item.Value) ?? string.Empty
                    : (item) => _flatDisplayCache![item.UniqueId];

                _localpaginator = new Paginator<VNode>(
                    _filterType, _flatAll!, _effectivePageSize, Optional<VNode>.Empty(),
                    (a, b) => a.UniqueId == b.UniqueId, filterKey);
            }
            else
            {
                _localpaginator = new Paginator<VNode>(
                    FilterMode.Disabled, _nodes, _effectivePageSize, Optional<VNode>.Empty(),
                    (a, b) => a.UniqueId == b.UniqueId,
                    (item) => _textSelector!(item.Value) ?? string.Empty);
            }

            if (keepIndex >= 0 && keepIndex < (_modeView == ModeView.Filter ? _flatAll!.Count : _nodes.Count))
                _localpaginator.EnsureVisibleIndex(keepIndex);
            else
                _localpaginator.FirstItem();
        }

        private void EnsureFlatAllBuilt()
        {
            if (_flatAll is not null) return;
            _flatAll = [];
            _flatDisplayCache = [];
            var rootV = _nodes[0];
            AddFlat(rootV);

            void AddFlat(VNode v)
            {
                _flatAll.Add(v);
                _flatDisplayCache[v.UniqueId] = BuildFullPath(v.Source);
                for (int i = 0; i < v.Source.Children.Count; i++)
                {
                    var child = new VNode(NextId(), v.Source.Children[i], v.Depth + 1, isLast: i == v.Source.Children.Count - 1)
                    {
                        Check = ComputeCheck(v.Source.Children[i])
                    };
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
                    if (n.IsLast) chainMask &= ~(1L << depth);
                    else chainMask |= (1L << depth);
                    chainMask &= (1L << (depth + 1)) - 1;
                }
            }
        }

        // ─── history ────────────────────────────────────────────────────────────────

        private void SaveHistory(T[] values)
        {
            if (_historyOptions == null) return;
            string serializedValue = JsonSerializer.Serialize(values);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;
        }

        private static bool TryDeserializeHistoryValues(string value, out T[]? result)
        {
            result = null;
            try { result = JsonSerializer.Deserialize<T[]>(value); return result is { Length: > 0 }; }
            catch { return false; }
        }

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
            if (t.GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null) return;
            if (t.GetConstructor(Type.EmptyTypes) != null) return;
            throw new InvalidOperationException(
                $"Type '{t.FullName}' cannot be safely serialized to history. " +
                "Decorate it with [Serializable]/[DataContract] or provide a public parameterless constructor.");
        }

        // ─── filter ─────────────────────────────────────────────────────────────────

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

        // ─── rendering ──────────────────────────────────────────────────────────────

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Filter)
            {
                WriteAnswerFilter(screenBuffer);
                return;
            }

            // Show the node currently under the cursor — mirroring MultiFileControl.WriteAnswer
            // which displays the selected item's formatted name/path in the answer line.
            // The checked-item count is shown in the pagination footer instead.
            VNode? selected = _localpaginator?.SelectedItem;
            string text = selected is null ? string.Empty : FormatAnswerForNode(selected);
            // Shown live only, same two-space convention as the list row (no Prefix/Suffix
            // config here, unlike Select/MultiSelect). The final answer (the checked-values join
            // via FormatAnswerForValue) intentionally stays plain.
            if (selected is not null && (_extraInfoSelector is not null || _extraInfoSelectorAsync is not null))
            {
                string? extra = GetExtraInfoText(selected.Value);
                if (!string.IsNullOrEmpty(extra))
                {
                    text += $"  {extra}";
                }
            }
            WriteAnswerViewport(screenBuffer, text, _optStyles[TreeMultiSelectStyles.Answer]);
        }

        private void WriteAnswerFilter(BufferScreen screenBuffer)
        {
            Style found = _localpaginator!.TotalCount == 0
                ? _optStyles[TreeMultiSelectStyles.Error]
                : _optStyles[TreeMultiSelectStyles.Answer];
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_filterBuffer!, promptWidth);
            screenBuffer.Write(visibleLeft, found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(visibleRight, found);
            screenBuffer.WriteLine($" ({PromptPlusResources.Filter})", _optStyles[TreeMultiSelectStyles.TaggedInfo]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            VNode? selected = _localpaginator?.SelectedItem;
            if (selected is not null)
            {
                if (_changeDescriptionAsync is not null)
                    desc = _changeDescriptionAsync.Invoke(selected.Value).ConfigureAwait(false).GetAwaiter().GetResult();
                else if (_changeDescription is not null)
                    desc = _changeDescription.Invoke(selected.Value);
            }
            if (!string.IsNullOrEmpty(desc))
                screenBuffer.WriteLine(desc, _optStyles[TreeMultiSelectStyles.Description]);
        }

        private void WriteTreeList(BufferScreen screenBuffer)
        {
            ArraySegment<VNode> subset = _localpaginator!.GetPageData();
            VNode? selectedItem = _localpaginator.SelectedIndex >= 0 ? _localpaginator.SelectedItem : null;

            foreach (VNode node in subset)
            {
                bool isSelected = selectedItem != null && node.UniqueId == selectedItem.UniqueId;
                Style lineStyle = isSelected ? _optStyles[TreeMultiSelectStyles.Selected] : _optStyles[TreeMultiSelectStyles.UnSelected];

                screenBuffer.Write(isSelected ? GetSymbol(SymbolType.Selector) : " ", ConsoleHandler.CurrentStyle);

                // Checkbox symbol (tri-state).
                string checkSymbol = node.Check switch
                {
                    CheckState.Checked => GetSymbol(SymbolType.Selected),
                    CheckState.Indeterminate => GetSymbol(SymbolType.PartialSelect),
                    _ => GetSymbol(SymbolType.NotSelect)
                };
                Style checkStyle = node.Disabled ? _optStyles[TreeMultiSelectStyles.Disabled]
                    : isSelected ? lineStyle : _optStyles[TreeMultiSelectStyles.UnSelected];
                screenBuffer.Write(checkSymbol, checkStyle);

                if (_modeView == ModeView.Filter)
                {
                    string pathText = _flatDisplayCache is not null && _flatDisplayCache.TryGetValue(node.UniqueId, out string? p)
                        ? p
                        : (_textSelector!(node.Value) ?? string.Empty);
                    Style nameStyle = node.Disabled ? _optStyles[TreeMultiSelectStyles.Disabled]
                        : isSelected ? lineStyle
                        : node.IsRoot ? _optStyles[TreeMultiSelectStyles.Root] : _optStyles[TreeMultiSelectStyles.Node];
                    screenBuffer.Write($" {pathText}", nameStyle);
                    WriteExtraInfo(screenBuffer, node);
                    screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
                    continue;
                }

                if (node.Depth > 0)
                {
                    for (int d = 1; d < node.Depth; d++)
                    {
                        screenBuffer.Write((node.AncestorMask & (1L << d)) != 0
                            ? GetSymbol(SymbolType.TreeLinevertical)
                            : GetSymbol(SymbolType.TreeLinespace), _optStyles[TreeMultiSelectStyles.Lines]);
                    }
                    screenBuffer.Write(node.IsLast
                        ? GetSymbol(SymbolType.TreeLinecorner)
                        : GetSymbol(SymbolType.TreeLinecross), _optStyles[TreeMultiSelectStyles.Lines]);
                }

                if (node.HasChildren)
                {
                    screenBuffer.Write(node.IsExpanded
                        ? GetSymbol(SymbolType.Expanded)
                        : GetSymbol(SymbolType.Collapsed), _optStyles[TreeMultiSelectStyles.ExpandSymbol]);
                    screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                }

                Style nameStyleSelect = node.IsRoot
                    ? _optStyles[TreeMultiSelectStyles.Root]
                    : _optStyles[TreeMultiSelectStyles.Node];
                if (isSelected) nameStyleSelect = lineStyle;
                if (node.Disabled) nameStyleSelect = _optStyles[TreeMultiSelectStyles.Disabled];
                screenBuffer.Write(_textSelector!(node.Value) ?? string.Empty, nameStyleSelect);

                WriteExtraInfo(screenBuffer, node);
                screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
            }

            if (_localpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount)!;
                // Reconciled count (CollectChecked), not the raw _checkedSourceIds.Count: a
                // cascaded check stamps every descendant's own id, so after an individual child
                // is later unchecked the container's stale id would otherwise inflate this badge
                // beyond what Enter actually returns.
                template = $"{template} {string.Format(CultureInfo.CurrentCulture, s_countCheckFormat, CollectChecked().Length)}";
                screenBuffer.WriteLine(template, _optStyles[TreeMultiSelectStyles.Pagination]);
            }
        }

        private void WriteExtraInfo(BufferScreen screenBuffer, VNode node)
        {
            if (_extraInfoSelector == null && _extraInfoSelectorAsync == null) return;
            string? extra = GetExtraInfoText(node.Value);
            if (!string.IsNullOrEmpty(extra))
                screenBuffer.Write($"  {extra}", _optStyles[TreeMultiSelectStyles.ChildsCount]);
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

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip) return;
            string tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.')) tooltip = $"{tooltip}.";
            screenBuffer.WriteLine(tooltip, _optStyles[TreeMultiSelectStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            string[] entries = _toggerTooptips[_modeView];
            if (_indexTooptip >= entries.Length) _indexTooptip = 0;
            return entries.Length == 0 ? string.Empty : entries[_indexTooptip];
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips = [GetTooltipNavigate(mode)];
                lsttooltips.Add(PromptPlusResources.TooltipPages);

                if (mode == ModeView.Select)
                {
                    lsttooltips.Add(PromptPlusResources.TooltipExpandCollapse);
                    if (!_viewOnly)
                    {
                        lsttooltips.Add(PromptPlusResources.TooltipCheckItem);
                        if (_recursiveMarkWithCtrlSpace && _cascadeCheck)
                        {
                            lsttooltips.Add(PromptPlusResources.TooltipRecursiveMark);
                        }
                        lsttooltips.Add($"{ConfigPrompt.HotKeyToggleAll}:{PromptPlusResources.TooltipCheckAll}");
                    }

                    if (_filterType != FilterMode.Disabled && !_viewOnly)
                        lsttooltips.Add(PromptPlusResources.TooltipFilter);
                    else if (!_viewOnly)
                        lsttooltips.Add(PromptPlusResources.TooltipJump);

                    lsttooltips.Add($"{ConfigPrompt.HotKeyToggleFullPath}:{PromptPlusResources.TooltipToggleFullPath}");
                }

                if (OptionsControl.EnabledAbortKeyValue)
                    lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
                lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");

                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

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

        // ─── formatting helpers ─────────────────────────────────────────────────────

        private string FormatAnswerForNode(VNode node)
        {
            if (_showFullPath) return BuildFullPath(node.Source);
            if (node.IsRoot) return _textSelector!(node.Value) ?? string.Empty;
            TreeNode? parent = node.Source.ParentNode;
            string name = _textSelector!(node.Value) ?? string.Empty;
            if (parent is null) return name;
            return $"{_textSelector!(parent.Value)}{_pathSep}{name}";
        }

        private string FormatAnswerForValue(T value)
        {
            List<TreeNode>? path = _root is null ? null : FindPath(_root, value);
            if (path is null || path.Count == 0) return _textSelector!(value) ?? string.Empty;
            TreeNode leaf = path[^1];
            if (_showFullPath) return BuildFullPath(leaf);
            if (path.Count == 1) return _textSelector!(leaf.Value) ?? string.Empty;
            TreeNode parent = path[^2];
            return $"{_textSelector!(parent.Value)}{_pathSep}{_textSelector!(leaf.Value)}";
        }

        private string BuildFullPath(TreeNode node)
        {
            var parts = new List<string>();
            TreeNode? cursor = node;
            while (cursor != null) { parts.Add(_textSelector!(cursor.Value) ?? string.Empty); cursor = cursor.ParentNode; }
            parts.Reverse();
            return string.Join(_pathSep, parts);
        }
    }
}
