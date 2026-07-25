// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Controls.Table;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.MultiTable
{
    /// <inheritdoc/>
    internal sealed class MultiTableControl<T> : BaseControlPrompt<T[]>, IMultiTableControl<T>
    {
        /// <summary>
        /// Total rows the control template reserves around the items list:
        /// prompt+answer, optional description, tooltip, error, and pagination footer.
        /// </summary>
        private const int ReservedTemplateLines = 8;

        private const int HeaderSelectionPrefixWidth = 2;
        private const int HeaderSelectionSuffixWidth = 2;

        // Cached composite format strings
        private static readonly CompositeFormat s_minSelectionFormat = CompositeFormat.Parse(PromptPlusResources.MultiSelectMinSelection);
        private static readonly CompositeFormat s_maxSelectionFormat = CompositeFormat.Parse(PromptPlusResources.MultiSelectMaxSelection);
        private static readonly CompositeFormat s_tooltipCountCheckFormat = CompositeFormat.Parse(PromptPlusResources.TooltipCountCheck);

        private readonly Dictionary<(char Ch, int Count), string> _lineCache = [];
        private bool _allColumnsVisible;
        private readonly Dictionary<MultiTableStyles, Style> _optStyles;
        private readonly List<ColumnDefinition<T>> _columns = [];
        private readonly List<ItemTable<T>> _items = [];
        private Func<T, (bool, string?)>? _predicatevalidcheck;
        private Func<T, Task<(bool, string?)>>? _predicatevalidcheckAsync;
        private Func<T, T, bool> _DefaultMatchBy = EqualityComparer<T>.Default.Equals;
        private IEnumerable<T> _defaultValues = [];
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;
        private Func<T, string>? _changeDescription;
        private Func<T, Task<string>>? _changeDescriptionAsync;
        private Func<T, string>? _textSelector;
        private Func<T, Task<string>>? _textSelectorAsync;
        private FilterMode _filterType = FilterMode.Disabled;
        private FilterTableMode _filterBy = FilterTableMode.Answer;
        private byte _pageSize;
        private bool _viewOnly;
        private TableLayoutMode _layoutMode = TableLayoutMode.SingleBox;
        private HideTable _layoutBorders = HideTable.None;
        private HorizontalScrollMode _horizontalScrollMode = HorizontalScrollMode.Full;
        private int _currentColumnIndex;
        private int _columnScrollOffset;
        private int _visibleStartCol;
        private int _visibleEndCol;
        private int _previewColumnIndex = -1;
        private readonly EmacsConsoleBuffer _filterBuffer;
        private readonly EmacsConsoleBuffer _answerBuffer;
        private Paginator<ItemTable<T>>? _localpaginator;
        private int _indexTooptip;
        private int _effectivePageSize;
        private bool _updatePosAnswerBuffer;
        private string _lastinput = string.Empty;

        // Multi-select state
        private int _maxSelect = int.MaxValue;
        private int _minSelect;
        private int _countChecked;
        private bool _onfilterOnlySelected;
        private int _lastCountCheckedTooltip = -1;
        private bool _lastFilterOnlySelectedTooltip;

        // Cached sum of all column CalculatedWidths; updated in CalculateColumnWidths().
        private int _totalColumnsWidth;

        // Width of the selector prefix area written before the table border (selector symbol + one space).
        private int _selectorPrefixWidth;
        // Width (chars) of the fixed checkbox column inside the table (first column, always visible).
        private int _checkboxColWidth;

        // Per-frame border string cache. Strings include the selector prefix before the table border.
        // Invalidated when the visible column window changes.
        private int    _cachedBorderStartCol   = -1;
        private int    _cachedBorderEndCol     = -1;
        private int    _cachedBorderPreviewCol = -2;
        private string? _cachedTopMain;
        private string  _cachedTopPreview        = string.Empty;
        private string  _cachedHeaderSepMain     = string.Empty;
        private string  _cachedHeaderSepPreview  = string.Empty;
        private string  _cachedRowSepMain        = string.Empty;
        private string  _cachedRowSepPreview     = string.Empty;
        private string? _cachedBottomMain;
        private string  _cachedBottomPreview     = string.Empty;
        private string  _cachedBorderPrefix      = string.Empty;

        // Pre-resolved symbol strings for the WriteHeader hot path.
        private string _cachedSelectorMarker   = "  ";
        private string _cachedFilterableSuffix = "  ";

        private enum ModeView { Select, Filter }

        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Select, [] },
            { ModeView.Filter, [] }
        };
        private ModeView _modeView = ModeView.Select;

        public MultiTableControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions)
            : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<MultiTableStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
            _filterBuffer = new(false, CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
            _answerBuffer  = new(true,  CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
        }

        #region IMultiTableControl

        /// <inheritdoc/>
        public IMultiTableControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> Styles(MultiTableStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> LayoutMode(TableLayoutMode mode)
        {
            _layoutMode = mode;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> HideElements(HideTable borders)
        {
            _layoutBorders = borders;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> HorizontalScroll(HorizontalScrollMode mode)
        {
            _horizontalScrollMode = mode;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> AddColumn(string header, Func<T, object> selector, Func<object, string>? formatter = null, int? width = null, ColumnAlignment alignment = ColumnAlignment.Left, bool isFilterable = false)
        {
            ArgumentNullException.ThrowIfNull(header);
            ArgumentNullException.ThrowIfNull(selector);
            if (string.IsNullOrWhiteSpace(header))
                throw new ArgumentException("Header cannot be empty or whitespace.", nameof(header));
            if (width.HasValue && width.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero when specified.");

            _columns.Add(new ColumnDefinition<T>(header, selector, formatter, width, alignment, isFilterable)
            {
                CalculatedWidth = width ?? 0
            });
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> PageSize(byte value)
        {
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> Filter(FilterMode value, FilterTableMode filterby = FilterTableMode.Answer)
        {
            _filterType = value;
            _filterBy = filterby;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> AddItem(T value, bool ischecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(value);
            _items.Add(new ItemTable<T>(Guid.NewGuid().ToString(), value, disable)
            {
                ValueChecked = ischecked
            });
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> AddItems(IEnumerable<T> values, bool ischecked = false, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values);
            foreach (T item in values)
                AddItem(item, ischecked, disable);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> Default(IEnumerable<T> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            _defaultValues = values;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> Range(int minvalue, int? maxvalue = null)
        {
            if (minvalue < 0)
                throw new ArgumentOutOfRangeException(nameof(minvalue), "Minimum must be >= 0.");
            _minSelect = minvalue;
            _maxSelect = maxvalue.HasValue
                ? (maxvalue.Value < minvalue
                    ? throw new ArgumentOutOfRangeException(nameof(maxvalue), "Maximum must be >= minimum.")
                    : maxvalue.Value)
                : int.MaxValue;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> PredicateChecked(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = (input) => (validselect(input), (string?)null);
            _predicatevalidcheckAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidcheck = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> PredicateChecked(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheck = validselect;
            _predicatevalidcheckAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidcheckAsync = validselect;
            _predicatevalidcheck = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);
            _DefaultMatchBy = comparer;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
        {
            ArgumentNullException.ThrowIfNull(filename);
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename cannot be empty or whitespace.", nameof(filename));
            _historyOptions = new HistoryOptions(filename);
            options?.Invoke(_historyOptions);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> UseDefaultHistory()
        {
            _defaultValues = [];
            _useDefaultHistory = true;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> ChangeDescription(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> TextSelector(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _textSelector = value;
            _textSelectorAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> TextSelectorAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _textSelectorAsync = value;
            _textSelector = null;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> ViewOnly(bool value = true)
        {
            _viewOnly = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, IMultiTableControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T1 item in items)
                interactionAction.Invoke(item, this);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTableControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, IMultiTableControl<T>, Task> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T1 item in items)
                interactionAction.Invoke(item, this).ConfigureAwait(false).GetAwaiter().GetResult();
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_columns.Count == 0)
                throw new ValidationException("At least one column must be defined for MultiTable control.");
            if (_items.Count == 0)
                throw new ValidationException("At least one item must be added to MultiTable control.");

            _updatePosAnswerBuffer = true;
            _currentColumnIndex = 0;
            _columnScrollOffset = 0;

            BuildItemsCache();
            CalculateColumnWidths();

            // Compute selector prefix width (outside the border) and checkbox column width (inside the table).
            string selectorSym   = GetSymbol(SymbolType.Selector);
            string checkedSym0   = GetSymbol(SymbolType.Selected);
            string uncheckedSym0 = GetSymbol(SymbolType.NotSelect);
            int    checkboxSymLen = Math.Max(checkedSym0.Length, uncheckedSym0.Length);

            _selectorPrefixWidth = selectorSym.Length + 1;  // selector + one space (written before table border)
            _checkboxColWidth    = checkboxSymLen;           // fixed checkbox column inside the table
            _cachedBorderPrefix  = new string(' ', _selectorPrefixWidth);

            _cachedSelectorMarker   = $"{selectorSym[0]} ";
            _cachedFilterableSuffix = $" {GetSymbol(SymbolType.FilterableStatus)[0]} ";

            if (_viewOnly)
                _historyOptions = null;

            bool loadedDefaultsFromHistory = false;
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    if (TryDeserializeHistoryValue(_itemHistories[0].History, out T[] histvalues) && histvalues.Length > 0)
                    {
                        foreach (var hv in histvalues)
                        {
                            int idx = _items.FindIndex(x => _DefaultMatchBy.Invoke(x.Value!, hv));
                            if (idx >= 0 && (_items[idx].Disabled || TryValidateCheckPredicate(_items[idx].Value, out _)))
                                _items[idx].ValueChecked = true;
                        }
                        loadedDefaultsFromHistory = true;
                    }
                }
            }

            // Apply Default() values (pre-checks + cursor position).
            // Disabled items matching the list are also marked (read-only visual).
            var defaultList = _defaultValues.ToList();
            if (defaultList.Count > 0 && !loadedDefaultsFromHistory)
            {
                foreach (var dv in defaultList)
                {
                    int idx = _items.FindIndex(x => _DefaultMatchBy.Invoke(x.Value!, dv));
                    if (idx >= 0 && (_items[idx].Disabled || TryValidateCheckPredicate(_items[idx].Value, out _)))
                        _items[idx].ValueChecked = true;
                }
            }
            _defaultValues = [];

            _countChecked = _items.Count(x => x.ValueChecked);

            // Position cursor on first Default match (non-disabled preferred, but disabled accepted).
            Optional<ItemTable<T>> defvaluepage = Optional<ItemTable<T>>.Empty();
            if (defaultList.Count > 0)
            {
                ItemTable<T>? found = _items.FirstOrDefault(x => !x.Disabled && _DefaultMatchBy.Invoke(x.Value!, defaultList[0]));
                found ??= _items.FirstOrDefault(x => _DefaultMatchBy.Invoke(x.Value!, defaultList[0]));
                if (found != null)
                    defvaluepage = Optional<ItemTable<T>>.Set(found);
            }

            _effectivePageSize = ComputeTableEffectivePageSize();
            _localpaginator = new Paginator<ItemTable<T>>(
                _filterType,
                _items,
                _effectivePageSize,
                defvaluepage,
                (a, b) => a.UniqueId == b.UniqueId,
                GetFilterText);

            if (_localpaginator.SelectedItem == null)
                _localpaginator.FirstItem();

            if (!_viewOnly && _localpaginator!.SelectedIndex >= 0 && _localpaginator.SelectedItem!.Disabled)
                SetError(PromptPlusResources.SelectionDisabled);

            RefreshAnswerBuffer();
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
                    bool updatePosAnswerBufferBeforeThisKey = _updatePosAnswerBuffer;
                    _updatePosAnswerBuffer = true;

                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        // Restore the flag's pre-iteration value instead of leaving it force-set to
                        // `true` above — same fix as Select/MultiSelect/Table: a resize must not
                        // silently undo a scroll the user had just navigated to on the answer
                        // preview.
                        _updatePosAnswerBuffer = updatePosAnswerBufferBeforeThisKey;
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<T[]>([], true);
                        }
                        break;
                    }
                    ConsoleKeyInfo keyinfo = press.Key;

                    #region Abort / Tooltip

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<T[]>([], true);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip >= _toggerTooptips[_modeView].Length)
                            _indexTooptip = 0;
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }

                    #endregion

                    #region Enter (confirm)

                    else if (keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null)
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        if (_viewOnly)
                        {
                            // ViewOnly: return all pre-checked items unchanged.
                            ResultCtrl = new ResultPrompt<T[]>([.. _items.Where(x => x.ValueChecked).Select(x => x.Value)], false);
                            break;
                        }
                        if (_countChecked < _minSelect)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_minSelectionFormat, _minSelect));
                            break;
                        }
                        if (_countChecked > _maxSelect)
                        {
                            SetError(string.Format(CultureInfo.CurrentCulture, s_maxSelectionFormat, _maxSelect));
                            break;
                        }
                        SaveHistory();
                        ResultCtrl = new ResultPrompt<T[]>([.. _items.Where(x => x.ValueChecked).Select(x => x.Value)], false);
                        break;
                    }

                    #endregion

                    #region Filter activation

                    else if (keyinfo.IsPressFilterActivationKey() && _localpaginator!.SelectedItem != null)
                    {
                        _indexTooptip = 0;
                        _updatePosAnswerBuffer = false;
                        continue;
                    }

                    #endregion

                    #region Column navigation (Tab / Shift+Tab)

                    else if (keyinfo.IsPressTabKey())
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.Filter)
                            ExitFilterMode();
                        _currentColumnIndex++;
                        if (_currentColumnIndex >= _columns.Count)
                            _currentColumnIndex = 0;
                        break;
                    }
                    else if (keyinfo.IsPressShiftTabKey())
                    {
                        _indexTooptip = 0;
                        if (_modeView == ModeView.Filter)
                            ExitFilterMode();
                        _currentColumnIndex--;
                        if (_currentColumnIndex < 0)
                            _currentColumnIndex = _columns.Count - 1;
                        break;
                    }

                    #endregion

                    #region F3 – filter only selected (toggle)

                    else if (_onfilterOnlySelected && _modeView == ModeView.Select && ConfigPrompt.HotKeyFilterAllSelected.Equals(keyinfo))
                    {
                        // Exit "only selected" view: restore full list.
                        _onfilterOnlySelected = false;
                        _localpaginator!.UpdateCollection(_items);
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        RebuildTooltipIfNeeded();
                        break;
                    }
                    else if (!_onfilterOnlySelected && _countChecked > 0 && _modeView == ModeView.Select && ConfigPrompt.HotKeyFilterAllSelected.Equals(keyinfo))
                    {
                        // Enter "only selected" view: show only checked items.
                        _onfilterOnlySelected = true;
                        _localpaginator!.UpdateCollection(_items.Where(x => x.ValueChecked));
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }

                    #endregion

                    #region Row navigation

                    else if (keyinfo.IsPressDownArrowKey())
                    {
                        if (_localpaginator!.IsLastPageItem)
                            _localpaginator.NextPage(IndexOption.FirstItem);
                        else
                            _localpaginator.NextItem();
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem)
                            _localpaginator!.PreviousPage(IndexOption.LastItem);
                        else
                            _localpaginator!.PreviousItem();
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            SetSelectionDisabledErrorIfNeeded();
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            SetSelectionDisabledErrorIfNeeded();
                            _indexTooptip = 0;
                            break;
                        }
                    }
                    else if (keyinfo.IsPressCtrlHomeKey())
                    {
                        if (!_localpaginator!.Home()) { continue; }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        if (!_localpaginator!.End()) { continue; }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }

                    #endregion

                    #region F2 – toggle all

                    else if (!_viewOnly && ConfigPrompt.HotKeyToggleAll.Equals(keyinfo) && _onfilterOnlySelected)
                    {
                        // Uncheck all + exit filter-only-selected mode.
                        foreach (var item in _localpaginator!.AllItems().Where(x => !x.Disabled))
                        {
                            if (item.ValueChecked)
                            {
                                item.ValueChecked = false;
                                _countChecked--;
                            }
                        }
                        _onfilterOnlySelected = false;
                        _localpaginator!.UpdateCollection(_items);
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        RefreshAnswerBuffer();
                        RebuildTooltipIfNeeded();
                        break;
                    }
                    else if (!_viewOnly && _modeView == ModeView.Filter && ConfigPrompt.HotKeyToggleAll.Equals(keyinfo) && !_onfilterOnlySelected)
                    {
                        // Toggle all in current filtered view.
                        int filteredCount = _localpaginator!.AllItems().Count(x => !x.Disabled);
                        bool targetChecked = _localpaginator.AllItems().Count(x => x.ValueChecked && !x.Disabled) != filteredCount;
                        foreach (var item in _localpaginator.AllItems().Where(x => !x.Disabled))
                        {
                            if (item.ValueChecked != targetChecked)
                            {
                                if (targetChecked && !TryValidateCheckPredicate(item.Value, out _))
                                    continue;
                                item.ValueChecked = targetChecked;
                                _countChecked += targetChecked ? 1 : -1;
                            }
                        }
                        if (_countChecked == 0) _onfilterOnlySelected = false;
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        RefreshAnswerBuffer();
                        RebuildTooltipIfNeeded();
                        break;
                    }
                    else if (!_viewOnly && _modeView == ModeView.Select && ConfigPrompt.HotKeyToggleAll.Equals(keyinfo) && !_onfilterOnlySelected)
                    {
                        // Toggle all in full list.
                        int allCount   = _items.Count(x => !x.Disabled);
                        bool targetChecked = _items.Count(x => x.ValueChecked && !x.Disabled) != allCount;
                        foreach (var item in _items.Where(x => !x.Disabled))
                        {
                            if (item.ValueChecked != targetChecked)
                            {
                                if (targetChecked && !TryValidateCheckPredicate(item.Value, out _))
                                    continue;
                                item.ValueChecked = targetChecked;
                                _countChecked += targetChecked ? 1 : -1;
                            }
                        }
                        if (_countChecked == 0) _onfilterOnlySelected = false;
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        RefreshAnswerBuffer();
                        RebuildTooltipIfNeeded();
                        break;
                    }

                    #endregion

                    #region Space – toggle current row

                    else if (!_viewOnly && keyinfo.IsPressSpaceKey() && _localpaginator!.SelectedItem != null && !_localpaginator.SelectedItem.Disabled)
                    {
                        if (_localpaginator!.SelectedItem.ValueChecked)
                        {
                            // Unchecking never needs the predicate — it only gates checking a row.
                            _localpaginator.SelectedItem.ValueChecked = false;
                            _countChecked--;
                        }
                        else
                        {
                            if (!TryValidateCheckPredicate(_localpaginator!.SelectedItem.Value, out string? message))
                            {
                                SetError(string.IsNullOrEmpty(message) ? PromptPlusResources.PredicateSelectInvalid : message);
                                break;
                            }
                            _localpaginator.SelectedItem.ValueChecked = true;
                            _countChecked++;
                        }
                        if (_countChecked == 0) _onfilterOnlySelected = false;
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        RefreshAnswerBuffer();
                        RebuildTooltipIfNeeded();
                        break;
                    }

                    #endregion

                    #region Filter input / quick search

                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter && _filterBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        UpdateFilterFromBuffer();
                        SetSelectionDisabledErrorIfNeeded(ignoreViewOnly: true);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Select && _answerBuffer.IsPrintable(keyinfo.KeyChar))
                    {
                        var keifilter = keyinfo;
                        if (keifilter.IsPressFilterActivationKey())
                            keifilter = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
                        if (_filterBuffer.TryAcceptedReadlineConsoleKey(keifilter))
                        {
                            _modeView = ModeView.Filter;
                            UpdateFilterFromBuffer();
                            SetSelectionDisabledErrorIfNeeded(ignoreViewOnly: true);
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_answerBuffer.IsPrintable(keyinfo.KeyChar) && _answerBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _updatePosAnswerBuffer = false;
                        break;
                    }
                    else if (_modeView == ModeView.Select && !_onfilterOnlySelected && _localpaginator!.SelectedItem != null && _answerBuffer.IsPrintable(keyinfo.KeyChar))
                    {
                        string keyChar = keyinfo.KeyChar.ToString();
                        int start = _localpaginator.CurrentIndex;
                        int index = _items.FindIndex(start + 1, x => x.FilterableText.StartsWith(keyChar, StringComparison.OrdinalIgnoreCase));
                        if (index < 0 && start >= 0)
                            index = _items.FindIndex(0, x => x.FilterableText.StartsWith(keyChar, StringComparison.OrdinalIgnoreCase));
                        if (index >= 0)
                        {
                            _localpaginator.EnsureVisibleIndex(index);
                            _indexTooptip = 0;
                            break;
                        }
                    }

                    #endregion
                }
                _lastinput = _filterBuffer.ToString();
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            int targetPageSize = ComputeTableEffectivePageSize();
            if (targetPageSize != _effectivePageSize)
            {
                _effectivePageSize = targetPageSize;
                _localpaginator?.UpdatePageSize(_effectivePageSize);
            }

            WritePrompt(screenBuffer, _optStyles[MultiTableStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteDescription(screenBuffer);
            WriteTable(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[MultiTableStyles.Error]);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _modeView = ModeView.Select;
            WritePrompt(screenBuffer, _optStyles[MultiTableStyles.Prompt]);
            if (ResultCtrl!.Value.IsAborted && OptionsControl.ShowMessageAbortKeyValue)
            {
                screenBuffer.WriteLine(PromptPlusResources.CanceledKey, _optStyles[MultiTableStyles.Answer]);
            }
            else
            {
                // Show comma-joined text of checked items (like MultiSelect).
                string answer = string.Join(',', _items.Where(x => x.ValueChecked).Select(GetAnswerText));
                screenBuffer.WriteLine(answer, _optStyles[MultiTableStyles.Answer]);
            }
            return true;
        }

        public override void FinalizeControl()
        {
            // none
        }

        // ── Private helpers ───────────────────────────────────────────────────────

        private void RefreshAnswerBuffer()
        {
            string text = string.Empty;
            if (_countChecked > 0)
            {
                text = string.Join(',', _items.Where(x => x.ValueChecked).Select(GetAnswerText));
            }
            _answerBuffer.LoadPrintable(text);
            _answerBuffer.ToHome();
        }

        private void RebuildTooltipIfNeeded()
        {
            bool hasChecked = _countChecked > 0;
            if (hasChecked != (_lastCountCheckedTooltip > 0) || _onfilterOnlySelected != _lastFilterOnlySelectedTooltip)
            {
                LoadTooltipToggle();
                _lastCountCheckedTooltip = _countChecked;
                _lastFilterOnlySelectedTooltip = _onfilterOnlySelected;
            }
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips = [GetTooltipSelect()];
                lsttooltips.Add(PromptPlusResources.TooltipPages);
                if (!_viewOnly)
                    lsttooltips.Add($"{ConfigPrompt.HotKeyToggleAll}:{PromptPlusResources.TooltipCheckAll}");
                if (mode == ModeView.Select)
                {
                    // Only advertise the "filter all selected" hotkey when it actually does
                    // something: either there is at least one checked item (to enter the view) or
                    // we are already inside the "only selected" view (to leave it).
                    if (_countChecked > 0 || _onfilterOnlySelected)
                        lsttooltips.Add($"{ConfigPrompt.HotKeyFilterAllSelected}:{PromptPlusResources.TooltipFilterAllSelected}");
                    if (!_viewOnly && _filterType != FilterMode.Disabled)
                        lsttooltips.Add(PromptPlusResources.TooltipFilter);
                    if (!_viewOnly)
                        lsttooltips.Add(PromptPlusResources.TooltipNavegateTextPrompt);
                    // Jump-by-first-char is only reachable when filter is disabled (otherwise any
                    // printable key transitions the control into filter mode instead of jumping).
                    // It also only ever finds a match via item.FilterableText, which is only
                    // populated from columns marked isFilterable — advertise it only when at
                    // least one column qualifies, otherwise the tooltip promises a key that
                    // silently does nothing (same fix as TableControl).
                    if (!_viewOnly && _filterType == FilterMode.Disabled && _columns.Exists(c => c.IsFilterable))
                        lsttooltips.Add(PromptPlusResources.TooltipTableJump);
                }
                if (OptionsControl.EnabledAbortKeyValue)
                    lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
                lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
                lsttooltips.AddRange(GetEmacsTooltips(_viewOnly));
                _toggerTooptips[mode] = [.. lsttooltips];
            }
            _lastCountCheckedTooltip = _countChecked;
            _lastFilterOnlySelectedTooltip = _onfilterOnlySelected;
        }

        private string GetTooltipSelect()
        {
            var sb = new StringBuilder();
            if (!_viewOnly)
            {
                sb.Append(PromptPlusResources.TooltipEnterFinish);
                sb.Append('.');
                sb.Append(PromptPlusResources.TooltipCheckItem);
                sb.Append('.');
            }
            sb.Append(PromptPlusResources.TooltipBaseNavegate);
            sb.Append('.');
            // Tab/ShiftTab navigate between columns — unique table feature.
            sb.Append(PromptPlusResources.TooltipTableColumnNav);
            sb.Append('.');
            return sb.ToString();
        }

        private string GetTooltipToggle()
        {
            string[] entries = _toggerTooptips[_modeView];
            if (_indexTooptip >= entries.Length)
            {
                _indexTooptip = 0;
            }
            return entries.Length == 0 ? string.Empty : entries[_indexTooptip];
        }

        private void UpdateFilterFromBuffer()
        {
            string filter = _filterBuffer.ToString();
            if (!filter.Equals(_lastinput, StringComparison.OrdinalIgnoreCase))
                _localpaginator!.UpdateFilter(filter);
            _lastinput = filter;
            if (string.IsNullOrEmpty(filter))
                _modeView = ModeView.Select;
        }

        // A filter term is only ever valid on the column it was typed against (Answer falls back
        // to the current column's cell text when no TextSelector is set; ColumnFilters always
        // targets the current column). Changing column mid-filter exits it entirely instead of
        // silently re-targeting the search or, for ColumnFilters, matching nothing at all.
        private void ExitFilterMode()
        {
            _modeView = ModeView.Select;
            _filterBuffer.Clear();
            _localpaginator!.UpdateFilter(string.Empty);
            _lastinput = string.Empty;
        }

        private void SetSelectionDisabledErrorIfNeeded(bool ignoreViewOnly = false)
        {
            if ((!_viewOnly || ignoreViewOnly) && _localpaginator?.SelectedItem?.Disabled == true)
                SetError(PromptPlusResources.SelectionDisabled);
        }

        private bool TryValidateCheckPredicate(T value, out string? message)
        {
            if (_predicatevalidcheck == null && _predicatevalidcheckAsync == null)
            {
                message = null;
                return true;
            }
            bool ok;
            (ok, message) = _predicatevalidcheckAsync != null
                ? _predicatevalidcheckAsync.Invoke(value).ConfigureAwait(false).GetAwaiter().GetResult()
                : (_predicatevalidcheck?.Invoke(value) ?? (true, (string?)null));
            return ok;
        }

        private void SaveHistory()
        {
            if (_historyOptions == null) return;
            T[] checkedValues = [.. _items.Where(x => x.ValueChecked).Select(x => x.Value)];
            string serialized = JsonSerializer.Serialize(checkedValues);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serialized, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;
        }

        private int ComputeTableEffectivePageSize()
        {
            int structural = _layoutBorders.HasFlag(HideTable.OuterBorder) ? 0 : 1;
            structural    += _layoutBorders.HasFlag(HideTable.Header)      ? 0 : 2;
            structural    += _layoutBorders.HasFlag(HideTable.OuterBorder) ? 0 : 1;
            int itemHeight = _layoutBorders.HasFlag(HideTable.RowSeparator) ? 1 : 2;
            int available  = Math.Max(1, ConsoleHandler.Height - ReservedTemplateLines - structural);
            int computed   = Math.Max(1, available / itemHeight);
            return _pageSize == 0 ? computed : Math.Min(_pageSize, computed);
        }

        // ── Rendering ─────────────────────────────────────────────────────────────

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Select)
            {
                if (_updatePosAnswerBuffer)
                    RefreshAnswerBuffer();
                int promptWidth = GetPromptDisplayWidth();
                (string visibleLeft, string visibleRight) = ViewportSlice(_answerBuffer, promptWidth);
                screenBuffer.Write(visibleLeft, _optStyles[MultiTableStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.WriteLine(visibleRight, _optStyles[MultiTableStyles.Answer]);
            }
            else
            {
                WriteAnswerFilter(screenBuffer);
            }
        }

        private void WriteAnswerFilter(BufferScreen screenBuffer)
        {
            Style found = _localpaginator!.TotalCount == 0
                ? _optStyles[MultiTableStyles.Error]
                : _optStyles[MultiTableStyles.TaggedInfo];
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_filterBuffer, promptWidth);
            screenBuffer.Write(visibleLeft, found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(visibleRight, found);
            screenBuffer.WriteLine($" ({PromptPlusResources.Filter})", _optStyles[MultiTableStyles.TaggedInfo]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = GetChangeDescriptionValue();
            if (!string.IsNullOrEmpty(desc))
                screenBuffer.WriteLine(desc, _optStyles[MultiTableStyles.Description]);
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip) return;
            RebuildTooltipIfNeeded();
            string tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
                tooltip = $"{tooltip}.";
            screenBuffer.WriteLine(tooltip, _optStyles[MultiTableStyles.Tooltips]);
        }

        private void WriteTable(BufferScreen screenBuffer)
        {
            (_visibleStartCol, _visibleEndCol) = ComputeVisibleColumnRange();
            _allColumnsVisible = _columns.Count == 0 || (_visibleStartCol == 0 && _visibleEndCol == _columns.Count - 1);

            _previewColumnIndex = (_horizontalScrollMode == HorizontalScrollMode.Full && _visibleEndCol < _columns.Count - 1)
                ? _visibleEndCol + 1
                : -1;

            if (_visibleStartCol != _cachedBorderStartCol
                || _visibleEndCol  != _cachedBorderEndCol
                || _previewColumnIndex != _cachedBorderPreviewCol)
            {
                RebuildBorderCache();
                _cachedBorderStartCol   = _visibleStartCol;
                _cachedBorderEndCol     = _visibleEndCol;
                _cachedBorderPreviewCol = _previewColumnIndex;
            }

            if (_cachedTopMain is not null)
            {
                screenBuffer.Write(_cachedTopMain,    _optStyles[MultiTableStyles.BorderLines]);
                screenBuffer.WriteLine(_cachedTopPreview, _optStyles[MultiTableStyles.DisabledRow]);
            }

            if (!_layoutBorders.HasFlag(HideTable.Header))
            {
                WriteHeader(screenBuffer);
                if (!string.IsNullOrEmpty(_cachedHeaderSepMain))
                {
                    screenBuffer.Write(_cachedHeaderSepMain,    _optStyles[MultiTableStyles.BorderLines]);
                    screenBuffer.WriteLine(_cachedHeaderSepPreview, _optStyles[MultiTableStyles.DisabledRow]);
                }
            }

            WriteDataRows(screenBuffer);

            if (_cachedBottomMain is not null)
            {
                screenBuffer.Write(_cachedBottomMain,    _optStyles[MultiTableStyles.BorderLines]);
                screenBuffer.WriteLine(_cachedBottomPreview, _optStyles[MultiTableStyles.DisabledRow]);
            }

            if (_localpaginator!.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount)!;
                if (_countChecked > 0)
                    template = $"{template} {string.Format(CultureInfo.CurrentCulture, s_tooltipCountCheckFormat, _countChecked)}";
                string colInfo = string.Concat($"{PromptPlusResources.Col_Info}: ", (_currentColumnIndex + 1).ToString(CultureInfo.InvariantCulture), "/", _columns.Count.ToString(CultureInfo.InvariantCulture), ".");
                screenBuffer.WriteLine(string.Concat(template, colInfo), _optStyles[MultiTableStyles.Pagination]);
            }
        }

        private void RebuildBorderCache()
        {
            var (topMain, topPreview) = BuildTopBorder();
            _cachedTopMain    = topMain is not null ? string.Concat(_cachedBorderPrefix, topMain) : null;
            _cachedTopPreview = topPreview;

            var (hSepMain, hSepPreview) = BuildHeaderSeparator();
            _cachedHeaderSepMain    = !string.IsNullOrEmpty(hSepMain) ? string.Concat(_cachedBorderPrefix, hSepMain) : string.Empty;
            _cachedHeaderSepPreview = hSepPreview;

            var (rSepMain, rSepPreview) = BuildRowSeparator();
            _cachedRowSepMain    = !string.IsNullOrEmpty(rSepMain) ? string.Concat(_cachedBorderPrefix, rSepMain) : string.Empty;
            _cachedRowSepPreview = rSepPreview;

            var (botMain, botPreview) = BuildBottomBorder();
            _cachedBottomMain    = botMain is not null ? string.Concat(_cachedBorderPrefix, botMain) : null;
            _cachedBottomPreview = botPreview;
        }

        private void WriteHeader(BufferScreen screenBuffer)
        {
            char separator = !_layoutBorders.HasFlag(HideTable.ColumnSeparator)
                ? GetLayoutSymbol(SymbolType.GridSingleDividerY, SymbolType.GridDoubleDividerY)[0]
                : ' ';

            // Write selector prefix as spaces (no selector symbol in header).
            screenBuffer.Write(_cachedBorderPrefix, _optStyles[MultiTableStyles.BorderLines]);

            if (_layoutMode != TableLayoutMode.None)
            {
                char borderLeft = _layoutBorders.HasFlag(HideTable.OuterBorder)
                    ? ' '
                    : GetLayoutSymbol(SymbolType.GridSingleBorderLeft, SymbolType.GridDoubleBorderLeft)[0];
                screenBuffer.Write(borderLeft, _optStyles[MultiTableStyles.BorderLines]);
            }

            // Fixed checkbox column header: symbol conveying it's the selection column
            string checkboxHeaderSym = GetSymbol(SymbolType.ChartLabel);
            string checkboxHeaderCell = DisplayWidthHelpers.AlignCell(DisplayWidthHelpers.Truncate(checkboxHeaderSym, _checkboxColWidth), _checkboxColWidth, ColumnAlignment.Center);
            screenBuffer.Write(checkboxHeaderCell, _optStyles[MultiTableStyles.HeaderText]);
            // Separator between checkbox column and first user column
            screenBuffer.Write(separator, _optStyles[MultiTableStyles.BorderLines]);

            for (int i = _visibleStartCol; i <= _visibleEndCol; i++)
            {
                ColumnDefinition<T> col = _columns[i];
                bool isSelectedColumn = i == _currentColumnIndex;
                if (i > _visibleStartCol)
                    screenBuffer.Write(separator, _optStyles[MultiTableStyles.BorderLines]);

                bool showSelection = isSelectedColumn && _columns.Count > 1;
                string marker = showSelection ? _cachedSelectorMarker : "  ";
                string suffix = col.IsFilterable ? _cachedFilterableSuffix : "  ";

                int colWidth = col.CalculatedWidth;
                int headerDisplayWidth = col.Header.GetDisplayLength() is { Length: > 0 } hd ? hd[0] : 0;
                int availableAfterMarker = Math.Max(0, colWidth - marker.Length);
                int headerVisibleLength = Math.Min(headerDisplayWidth, availableAfterMarker);
                int suffixLength = Math.Min(suffix.Length, Math.Max(0, availableAfterMarker - headerVisibleLength));
                int textWidth = Math.Max(0, availableAfterMarker - suffixLength);
                string headerText = textWidth == 0 ? string.Empty : DisplayWidthHelpers.AlignCell(col.Header, textWidth, ColumnAlignment.Left);
                string cell = DisplayWidthHelpers.Truncate(marker + headerText + suffix, colWidth);

                screenBuffer.Write(cell, showSelection ? _optStyles[MultiTableStyles.SelectedCell] : _optStyles[MultiTableStyles.HeaderText]);
            }

            if (_previewColumnIndex >= 0)
            {
                ColumnDefinition<T> previewCol = _columns[_previewColumnIndex];
                screenBuffer.Write(separator, _optStyles[MultiTableStyles.DisabledRow]);
                int previewWidth = previewCol.CalculatedWidth;
                int previewAvailable = Math.Max(0, previewWidth - HeaderSelectionPrefixWidth);
                string previewText = DisplayWidthHelpers.AlignCell(DisplayWidthHelpers.Truncate(previewCol.Header, previewAvailable), previewWidth - HeaderSelectionPrefixWidth, ColumnAlignment.Left);
                string previewCell = DisplayWidthHelpers.Truncate("  " + previewText, previewWidth);
                screenBuffer.Write(previewCell, _optStyles[MultiTableStyles.DisabledRow]);
            }

            if (_layoutMode != TableLayoutMode.None)
            {
                char borderRight = _layoutBorders.HasFlag(HideTable.OuterBorder)
                    ? ' '
                    : GetLayoutSymbol(SymbolType.GridSingleBorderRight, SymbolType.GridDoubleBorderRight)[0];
                screenBuffer.Write(borderRight, _optStyles[MultiTableStyles.BorderLines]);
            }

            screenBuffer.WriteLine(string.Empty, ConsoleHandler.CurrentStyle);
        }

        private void WriteDataRows(BufferScreen screenBuffer)
        {
            if (_localpaginator!.TotalCountValid == 0) return;

            char separator = !_layoutBorders.HasFlag(HideTable.ColumnSeparator)
                ? GetLayoutSymbol(SymbolType.GridSingleDividerY, SymbolType.GridDoubleDividerY)[0]
                : ' ';

            ArraySegment<ItemTable<T>> subset = _localpaginator!.GetPageData();
            bool hasRowSep = !string.IsNullOrEmpty(_cachedRowSepMain);
            int index = 0;

            string selectorSym    = GetSymbol(SymbolType.Selector);
            string checkedSym     = GetSymbol(SymbolType.Selected);
            string notCheckedSym  = GetSymbol(SymbolType.NotSelect);
            int    selectorLen    = selectorSym.Length;

            foreach (ItemTable<T> item in subset)
            {
                bool isSelectedRow = item.UniqueId == (_localpaginator.SelectedItem?.UniqueId ?? string.Empty);
                Style rowStyle = item.Disabled
                    ? _optStyles[MultiTableStyles.DisabledRow]
                    : (isSelectedRow ? _optStyles[MultiTableStyles.SelectedCell] : _optStyles[MultiTableStyles.UnselectedCell]);

                // ── Selector prefix (outside the table border) ─────────────────
                if (isSelectedRow)
                    screenBuffer.Write(selectorSym, _optStyles[MultiTableStyles.SelectedCell]);
                else
                    screenBuffer.Write(new string(' ', selectorLen), ConsoleHandler.CurrentStyle);
                screenBuffer.Write(" ", ConsoleHandler.CurrentStyle);
                // ─────────────────────────────────────────────────────────────────

                if (_layoutMode != TableLayoutMode.None)
                {
                    char borderLeft = _layoutBorders.HasFlag(HideTable.OuterBorder)
                        ? ' '
                        : GetLayoutSymbol(SymbolType.GridSingleBorderLeft, SymbolType.GridDoubleBorderLeft)[0];
                    screenBuffer.Write(borderLeft, _optStyles[MultiTableStyles.BorderLines]);
                }

                // Fixed checkbox column (first column inside the table, always visible)
                string checkSym = item.ValueChecked ? checkedSym : notCheckedSym;
                string checkCell = DisplayWidthHelpers.AlignCell(checkSym, _checkboxColWidth, ColumnAlignment.Center);
                screenBuffer.Write(checkCell, rowStyle);
                // Separator between checkbox column and first user column
                screenBuffer.Write(separator, _optStyles[MultiTableStyles.BorderLines]);

                for (int i = _visibleStartCol; i <= _visibleEndCol; i++)
                {
                    if (i > _visibleStartCol)
                        screenBuffer.Write(separator, _optStyles[MultiTableStyles.BorderLines]);
                    string cellText = BuildCell(item.CachedCellValues[i]!, _columns[i]);
                    screenBuffer.Write(cellText, rowStyle);
                }

                if (_previewColumnIndex >= 0)
                {
                    screenBuffer.Write(separator, _optStyles[MultiTableStyles.DisabledRow]);
                    string previewCell = BuildCell(item.CachedCellValues[_previewColumnIndex]!, _columns[_previewColumnIndex]);
                    screenBuffer.Write(previewCell, _optStyles[MultiTableStyles.DisabledRow]);
                }

                if (_layoutMode != TableLayoutMode.None)
                {
                    char borderRight = _layoutBorders.HasFlag(HideTable.OuterBorder)
                        ? ' '
                        : GetLayoutSymbol(SymbolType.GridSingleBorderRight, SymbolType.GridDoubleBorderRight)[0];
                    screenBuffer.Write(borderRight, _optStyles[MultiTableStyles.BorderLines]);
                }
                screenBuffer.WriteLine(string.Empty, ConsoleHandler.CurrentStyle);

                index++;
                if (index < subset.Count && hasRowSep)
                {
                    screenBuffer.Write(_cachedRowSepMain,    _optStyles[MultiTableStyles.BorderLines]);
                    screenBuffer.WriteLine(_cachedRowSepPreview, _optStyles[MultiTableStyles.DisabledRow]);
                }
            }
        }

        // ── Column width / visibility ─────────────────────────────────────────────

        private void CalculateColumnWidths()
        {
            for (int colIndex = 0; colIndex < _columns.Count; colIndex++)
            {
                ColumnDefinition<T> column = _columns[colIndex];
                int suffixWidth = column.IsFilterable
                    ? HeaderSelectionSuffixWidth + 1
                    : HeaderSelectionSuffixWidth;

                int headerDisplayWidth = column.Header.GetDisplayLength() is { Length: > 0 } hd ? hd[0] : 0;

                if (column.Width.HasValue)
                {
                    int minForHeader = headerDisplayWidth + HeaderSelectionPrefixWidth + suffixWidth;
                    column.CalculatedWidth = Math.Max(column.Width.Value, minForHeader);
                    continue;
                }
                int autoWidth = headerDisplayWidth + HeaderSelectionPrefixWidth + suffixWidth;
                foreach (ItemTable<T> item in _items)
                {
                    if (colIndex < item.CachedCellValues.Length)
                    {
                        int cellLen = item.CachedCellValues[colIndex]?.GetDisplayLength() is { Length: > 0 } cd ? cd[0] : 0;
                        if (cellLen > autoWidth) autoWidth = cellLen;
                    }
                }
                column.CalculatedWidth = autoWidth;
            }
            _totalColumnsWidth = 0;
            for (int i = 0; i < _columns.Count; i++)
                _totalColumnsWidth += _columns[i].CalculatedWidth;
        }

        private void BuildItemsCache()
        {
            int colCount = _columns.Count;
            var sbFilter = new StringBuilder();
            foreach (ItemTable<T> item in _items)
            {
                string[] cellValues = new string[colCount];
                sbFilter.Clear();
                bool firstFilterable = true;
                for (int ci = 0; ci < colCount; ci++)
                {
                    ColumnDefinition<T> col = _columns[ci];
                    object? raw = col.Selector(item.Value);
                    string text = col.Formatter?.Invoke(raw ?? string.Empty) ?? raw?.ToString() ?? string.Empty;
                    cellValues[ci] = text;
                    if (col.IsFilterable && !string.IsNullOrEmpty(text))
                    {
                        if (!firstFilterable) sbFilter.Append((char)1);
                        sbFilter.Append(text);
                        firstFilterable = false;
                    }
                }
                item.CachedCellValues   = cellValues;
                item.CachedTextSelector = _textSelector is not null
                    ? _textSelector.Invoke(item.Value)
                    : (_textSelectorAsync?.Invoke(item.Value).ConfigureAwait(false).GetAwaiter().GetResult());
                item.FilterableText = sbFilter.Length > 0 ? sbFilter.ToString() : (item.CachedTextSelector ?? (cellValues.Length > 0 ? cellValues[0] : string.Empty));
            }
        }

        private int ComputeEndColumnFromStart(int startCol, int available, bool hasSep)
        {
            int endCol = startCol;
            int used = _columns[startCol].CalculatedWidth;
            for (int i = startCol + 1; i < _columns.Count; i++)
            {
                int add = _columns[i].CalculatedWidth + (hasSep ? 1 : 0);
                if (used + add > available) break;
                used += add;
                endCol = i;
            }
            return endCol;
        }

        private int ComputeStartColumnFromEnd(int endCol, int available, bool hasSep)
        {
            int startCol = endCol;
            int used = _columns[endCol].CalculatedWidth;
            for (int i = endCol - 1; i >= 0; i--)
            {
                int add = _columns[i].CalculatedWidth + (hasSep ? 1 : 0);
                if (used + add > available) break;
                used += add;
                startCol = i;
            }
            return startCol;
        }

        private (int startCol, int endCol) ComputeVisibleColumnRange()
        {
            if (_columns.Count == 0) return (0, -1);

            bool hasOuterBorder = _layoutMode != TableLayoutMode.None;
            bool hasSep = _layoutMode != TableLayoutMode.None;

            // available width for user columns: total minus selector prefix, outer borders,
            // fixed checkbox column, and the separator between it and the first user column
            int available = ConsoleHandler.Width
                - _selectorPrefixWidth
                - (hasOuterBorder ? 2 : 0)
                - _checkboxColWidth
                - (hasSep ? 1 : 0);

            int totalWidth = _totalColumnsWidth + (hasSep ? Math.Max(0, _columns.Count - 1) : 0);
            if (totalWidth <= available)
            {
                _columnScrollOffset = 0;
                return (0, _columns.Count - 1);
            }

            _columnScrollOffset = Math.Clamp(_columnScrollOffset, 0, _columns.Count - 1);
            int endCol = ComputeEndColumnFromStart(_columnScrollOffset, available, hasSep);
            bool needsEndColRecalc = false;

            if (_currentColumnIndex < _columnScrollOffset)
            {
                _columnScrollOffset = _horizontalScrollMode == HorizontalScrollMode.Full
                    ? ComputeStartColumnFromEnd(_currentColumnIndex, available, hasSep)
                    : _currentColumnIndex;
                needsEndColRecalc = true;
            }
            else if (_currentColumnIndex > endCol)
            {
                _columnScrollOffset = ComputeStartColumnFromEnd(_currentColumnIndex, available, hasSep);
                needsEndColRecalc = true;
            }

            if (needsEndColRecalc)
                endCol = ComputeEndColumnFromStart(_columnScrollOffset, available, hasSep);

            return (_columnScrollOffset, endCol);
        }

        // ── Border builders ───────────────────────────────────────────────────────

        private (string? main, string preview) BuildTopBorder()
        {
            if (_layoutBorders.HasFlag(HideTable.OuterBorder) || _layoutMode == TableLayoutMode.None)
                return (null, string.Empty);
            return BuildHorizontalGridLine(SymbolType.GridSingleTopLeft, SymbolType.GridSingleTopCenter, SymbolType.GridSingleTopRight, SymbolType.GridSingleBorderTop);
        }

        private (string main, string preview) BuildHeaderSeparator()
        {
            if (_layoutBorders.HasFlag(HideTable.Header) || _layoutMode == TableLayoutMode.None)
                return (string.Empty, string.Empty);
            return BuildHorizontalGridLine(SymbolType.GridSingleMiddleLeft, SymbolType.GridSingleMiddleCenter, SymbolType.GridSingleMiddleRight, SymbolType.GridSingleDividerX);
        }

        private (string main, string preview) BuildRowSeparator()
        {
            if (_layoutBorders.HasFlag(HideTable.RowSeparator) || _layoutMode == TableLayoutMode.None)
                return (string.Empty, string.Empty);
            return BuildHorizontalGridLine(SymbolType.GridSingleMiddleLeft, SymbolType.GridSingleMiddleCenter, SymbolType.GridSingleMiddleRight, SymbolType.GridSingleDividerX);
        }

        private (string? main, string preview) BuildBottomBorder()
        {
            if (_layoutBorders.HasFlag(HideTable.OuterBorder) || _layoutMode == TableLayoutMode.None)
                return (null, string.Empty);
            return BuildHorizontalGridLine(SymbolType.GridSingleBottomLeft, SymbolType.GridSingleBottomCenter, SymbolType.GridSingleBottomRight, SymbolType.GridSingleBorderBottom);
        }

        private string GetLayoutSymbol(SymbolType singleSymbol, SymbolType doubleSymbol)
        {
            bool useUnicode = _layoutMode is not (TableLayoutMode.SingleASCII or TableLayoutMode.DoubleASCII);
            SymbolType type = _layoutMode is TableLayoutMode.DoubleBox or TableLayoutMode.DoubleASCII
                ? doubleSymbol
                : singleSymbol;
            return GetSymbol(type, useUnicode);
        }

        private static SymbolType ToDoubleSymbol(SymbolType single) => single switch
        {
            SymbolType.GridSingleTopLeft      => SymbolType.GridDoubleTopLeft,
            SymbolType.GridSingleTopCenter    => SymbolType.GridDoubleTopCenter,
            SymbolType.GridSingleTopRight     => SymbolType.GridDoubleTopRight,
            SymbolType.GridSingleMiddleLeft   => SymbolType.GridDoubleMiddleLeft,
            SymbolType.GridSingleMiddleCenter => SymbolType.GridDoubleMiddleCenter,
            SymbolType.GridSingleMiddleRight  => SymbolType.GridDoubleMiddleRight,
            SymbolType.GridSingleBottomLeft   => SymbolType.GridDoubleBottomLeft,
            SymbolType.GridSingleBottomCenter => SymbolType.GridDoubleBottomCenter,
            SymbolType.GridSingleBottomRight  => SymbolType.GridDoubleBottomRight,
            SymbolType.GridSingleBorderLeft   => SymbolType.GridDoubleBorderLeft,
            SymbolType.GridSingleBorderRight  => SymbolType.GridDoubleBorderRight,
            SymbolType.GridSingleBorderTop    => SymbolType.GridDoubleBorderTop,
            SymbolType.GridSingleBorderBottom => SymbolType.GridDoubleBorderBottom,
            SymbolType.GridSingleDividerY     => SymbolType.GridDoubleDividerY,
            SymbolType.GridSingleDividerX     => SymbolType.GridDoubleDividerX,
            _                                 => single
        };

        private (string main, string preview) BuildHorizontalGridLine(
            SymbolType leftSymbol, SymbolType centerSymbol, SymbolType rightSymbol, SymbolType horizontalSymbol)
        {
            char left       = GetLayoutSymbol(leftSymbol,       ToDoubleSymbol(leftSymbol))[0];
            char center     = GetLayoutSymbol(centerSymbol,     ToDoubleSymbol(centerSymbol))[0];
            char horizontal = GetLayoutSymbol(horizontalSymbol, ToDoubleSymbol(horizontalSymbol))[0];
            char right      = GetLayoutSymbol(rightSymbol,      ToDoubleSymbol(rightSymbol))[0];

            bool hideOuter  = _layoutBorders.HasFlag(HideTable.OuterBorder) || _layoutMode == TableLayoutMode.None;
            char effectiveLeft  = hideOuter ? horizontal : left;
            char effectiveRight = hideOuter ? horizontal : right;

            StringBuilder main = new();
            main.Append(effectiveLeft);

            // Fixed checkbox column segment in horizontal border line
            main.Append(GetRepeated(horizontal, _checkboxColWidth));
            if (_columns.Count > 0)
                main.Append(_layoutBorders.HasFlag(HideTable.ColumnSeparator) ? horizontal : center);

            for (int i = _visibleStartCol; i <= _visibleEndCol; i++)
            {
                main.Append(GetRepeated(horizontal, _columns[i].CalculatedWidth));
                if (i < _visibleEndCol)
                    main.Append(_layoutBorders.HasFlag(HideTable.ColumnSeparator) ? horizontal : center);
            }

            if (_previewColumnIndex < 0)
            {
                main.Append(effectiveRight);
                return (main.ToString(), string.Empty);
            }

            StringBuilder preview = new();
            preview.Append(_layoutBorders.HasFlag(HideTable.ColumnSeparator) ? horizontal : center);
            preview.Append(GetRepeated(horizontal, _columns[_previewColumnIndex].CalculatedWidth));
            preview.Append(effectiveRight);
            return (main.ToString(), preview.ToString());
        }

        // ── Cell helpers ──────────────────────────────────────────────────────────

        private static string BuildCell(string value, ColumnDefinition<T> column)
            => DisplayWidthHelpers.AlignCell(DisplayWidthHelpers.Truncate(value, column.CalculatedWidth), column.CalculatedWidth, column.Alignment);

        private string GetRepeated(char ch, int count)
        {
            if (count <= 0) return string.Empty;
            if (!_lineCache.TryGetValue((ch, count), out var result))
            {
                result = new string(ch, count);
                _lineCache[(ch, count)] = result;
            }
            return result;
        }

        private string GetFilterText(ItemTable<T> item)
        {
            return _filterBy switch
            {
                FilterTableMode.Answer        => GetAnswerText(item),
                FilterTableMode.ColumnFilters => GetColumnFilterText(item),
                _                             => string.Empty,
            };
        }

        private string GetAnswerText(ItemTable<T> item)
        {
            if (item.CachedTextSelector is not null)
            {
                return item.CachedTextSelector;
            }
            int col = Math.Max(0, Math.Min(_currentColumnIndex, item.CachedCellValues.Length - 1));
            return item.CachedCellValues.Length > 0 ? item.CachedCellValues[col] ?? string.Empty : string.Empty;
        }

        private string GetColumnFilterText(ItemTable<T> item)
        {
            int col = Math.Max(0, Math.Min(_currentColumnIndex, item.CachedCellValues.Length - 1));
            if (!_columns[col].IsFilterable)
            {
                return string.Empty;
            }
            return item.CachedCellValues.Length > 0 ? item.CachedCellValues[col] ?? string.Empty : string.Empty;
        }

        private string? GetChangeDescriptionValue()
        {
            if (_localpaginator!.SelectedItem is null)
                return OptionsControl.DescriptionValue;
            if (_changeDescriptionAsync is not null)
                return _changeDescriptionAsync(_localpaginator.SelectedItem.Value).ConfigureAwait(false).GetAwaiter().GetResult();
            if (_changeDescription is not null)
                return _changeDescription(_localpaginator.SelectedItem.Value);
            return OptionsControl.DescriptionValue;
        }
    }
}
