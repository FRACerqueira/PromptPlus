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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.Table
{
    /// <inheritdoc/>
    internal sealed class TableControl<T> : BaseControlPrompt<TableResult<T>>, ITableControl<T>
    {
        /// <summary>
        /// Total rows the control template reserves around the items list:
        /// prompt+answer line, optional error/group line, optional description line,
        /// tooltip line and an extra row for the pagination footer when active.
        /// Used to derive the maximum visible page size from the available console height.
        /// </summary>
        private const int ReservedTemplateLines = 8;

        private const int HeaderSelectionPrefixWidth = 2;
        private const int HeaderSelectionSuffixWidth = 2;

        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;

        private readonly Dictionary<(char Ch, int Count), string> _lineCache = [];
        private readonly Dictionary<TableStyles, Style> _optStyles;
        private readonly List<ColumnDefinition<T>> _columns = [];
        private readonly List<ItemTable<T>> _items = [];
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private Func<T, Task<(bool, string?)>>? _predicatevalidselectAsync;
        private Func<T, T, bool> _DefaultMatchBy = EqualityComparer<T>.Default.Equals;
        private Optional<T> _defaultValue = Optional<T>.Empty();
        private bool _useDefaultHistory;
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
        private readonly EmacsConsoleBuffer? _answerBuffer;
        private Paginator<ItemTable<T>>? _localpaginator;
        private int _indexTooptip;
        private int _effectivePageSize;
        private bool _updatePosAnswerBuffer;
        private string _lastinput = string.Empty;
        // When _viewOnly is true this field captures the initial selection at InitControl so that
        // Enter always returns the original default/initial value, not the row the user browsed to.
        private ItemTable<T>? _initialItem;

        // ached sum of all column CalculatedWidths (without separators); updated in CalculateColumnWidths().
        private int _totalColumnsWidth;

        // per-frame border string cache. Main strings already include the "  " row-prefix so
        // callers can Write them directly without an extra allocation per frame.
        // null or empty = skip rendering. Invalidated when the visible column window changes.
        private int    _cachedBorderStartCol   = -1;
        private int    _cachedBorderEndCol     = -1;
        private int    _cachedBorderPreviewCol = -2; // -2 = uninitialised (distinct from -1 = no preview)
        private string? _cachedTopMain;
        private string  _cachedTopPreview        = string.Empty;
        private string  _cachedHeaderSepMain     = string.Empty;
        private string  _cachedHeaderSepPreview  = string.Empty;
        private string  _cachedRowSepMain        = string.Empty;
        private string  _cachedRowSepPreview     = string.Empty;
        private string? _cachedBottomMain;
        private string  _cachedBottomPreview     = string.Empty;

        // pre-resolved symbol strings for the WriteHeader hot path.
        // Computed once in InitControl(); constant for the lifetime of the control.
        private string _cachedSelectorMarker   = "  "; // 2 chars: "{SelectorChar} "
        private string _cachedFilterableSuffix = "  "; // 3 chars: " {FilterableStatusChar} "

        private enum ModeView
        {
            Select,
            Filter
        }
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Select,[] },
            { ModeView.Filter,[] }
        };
        private ModeView _modeView = ModeView.Select;

        public TableControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions)
            : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<TableStyles>(console.CurrentStyle);
            _pageSize = ConfigPrompt.PageSize;
            _filterBuffer = new(false, CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
            _answerBuffer = new(true, CaseOptions.Any, ConsoleHandler.EnabledEmacs, (_) => true);
        }

        #region ITableControl

        /// <inheritdoc/>
        public ITableControl<T> Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> Styles(TableStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> LayoutMode(TableLayoutMode mode)
        {
            _layoutMode = mode;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> HideElements(HideTable borders)
        {
            _layoutBorders = borders;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> HorizontalScroll(HorizontalScrollMode mode)
        {
            _horizontalScrollMode = mode;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> AddColumn(string header, Func<T, object> selector, Func<object, string>? formatter = null, int? width = null, ColumnAlignment alignment = ColumnAlignment.Left, bool isFilterable = false)
        {
            ArgumentNullException.ThrowIfNull(header);
            ArgumentNullException.ThrowIfNull(selector);

            if (string.IsNullOrWhiteSpace(header))
            {
                throw new ArgumentException("Header cannot be empty or whitespace.", nameof(header));
            }

            if (width.HasValue && width.Value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero when specified.");
            }

            ColumnDefinition<T> column = new(
                header,
                selector,
                formatter,
                width,
                alignment,
                isFilterable)
            {
                CalculatedWidth = width ?? 0  // auto-width: computed in CalculateColumnWidths() at InitControl
            };
            _columns.Add(column);
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> PageSize(byte value)
        {
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> Filter(FilterMode value, FilterTableMode filterby = FilterTableMode.Answer)
        {
            _filterType = value;
            _filterBy = filterby;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) => (validselect(input), (string?)null);
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = async (input) => ((await validselect(input).ConfigureAwait(false)), (string?)null);
            _predicatevalidselect = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            _predicatevalidselectAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselectAsync = validselect;
            _predicatevalidselect = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);
            _DefaultMatchBy = comparer;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> AddItem(T value, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(value);
            _items.Add(new ItemTable<T>(
                Guid.NewGuid().ToString(),
                value,
                disable));
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> AddItems(IEnumerable<T> values, bool disable = false)
        {
            ArgumentNullException.ThrowIfNull(values);
            foreach (T item in values)
            {
                AddItem(item, disable);
            }
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> Default(T value, bool useDefaultHistory = true)
        {
            ArgumentNullException.ThrowIfNull(value);
            _defaultValue = Optional<T>.Set(value);
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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
        public ITableControl<T> UseDefaultHistory()
        {
            _defaultValue = Optional<T>.Empty();
            _useDefaultHistory = true;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> ChangeDescription(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITableControl<T>> interactionAction)
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
        public ITableControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITableControl<T>, Task> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T1 item in items)
            {
                interactionAction.Invoke(item, this)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> TextSelector(Func<T, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _textSelector = value;
            _textSelectorAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> TextSelectorAsync(Func<T, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _textSelectorAsync = value;
            _textSelector = null;
            return this;
        }

        /// <inheritdoc/>
        public ITableControl<T> ViewOnly(bool value = true)
        {
            _viewOnly = value;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_columns.Count == 0)
            {
                throw new ValidationException("At least one column must be defined for table control.");
            }

            if (_items.Count == 0)
            {
                throw new ValidationException("At least one item must be added to table control.");
            }
            _updatePosAnswerBuffer = true;
            _currentColumnIndex = 0;
            _columnScrollOffset = 0;

            BuildItemsCache();// rebuild the cache for all items

            CalculateColumnWidths(); // calculate the column widths based on header and content

            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    if (TryDeserializeHistoryValue(_itemHistories[0].History, out T historyValue))
                    {
                        _defaultValue = Optional<T>.Set(historyValue);
                    }
                }
            }
            Optional<ItemTable<T>> defvaluepage = Optional<ItemTable<T>>.Empty();
            if (_defaultValue.HasValue)
            {
                ItemTable<T>? found = _items.FirstOrDefault(x => !x.Disabled && _DefaultMatchBy.Invoke(x.Value!, _defaultValue.Value));
                // Honor the selection predicate: a default/history value rejected by the predicate
                // does not position the cursor on it.
                if (found != null && (_viewOnly || TrySelectionPredicate(found.Value)))
                {
                    defvaluepage = Optional<ItemTable<T>>.Set(found);
                }
            }
            _effectivePageSize = ComputeTableEffectivePageSize();

            _localpaginator = new Paginator<ItemTable<T>>(
                _filterType,
                _items,
                _effectivePageSize,
                defvaluepage,
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                (item) => GetFilterText(item));

            if (_localpaginator.SelectedItem == null)
            {
                _localpaginator.FirstItem();
            }
            if (!_viewOnly && _localpaginator!.SelectedIndex >= 0 && _localpaginator.SelectedItem!.Disabled)
            {
                SetError(PromptPlusResources.SelectionDisabled);
            }
            // Snapshot the initial item so that in view-only mode Enter always returns
            // the original default/initial value regardless of where the user navigated.
            _initialItem = _localpaginator!.SelectedItem;
            // D5: resolve symbol chars once so WriteHeader can use pre-built strings on every frame.
            _cachedSelectorMarker   = $"{GetSymbol(SymbolType.Selector)[0]} ";
            _cachedFilterableSuffix = $" {GetSymbol(SymbolType.FilterableStatus)[0]} ";
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
                    _updatePosAnswerBuffer = true;

                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<TableResult<T>>(default!, true);
                        }
                        break;
                    }
                    ConsoleKeyInfo keyinfo = press.Key;

                    #region default Press to Finish and tooltip

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
                        ResultCtrl = _localpaginator!.SelectedItem != null
                            ? new ResultPrompt<TableResult<T>>(new TableResult<T>(_localpaginator.SelectedItem.Value!, _localpaginator.CurrentIndex, _currentColumnIndex), true)
                            : new ResultPrompt<TableResult<T>>(default!, true);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip >= _toggerTooptips[_modeView].Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null)
                    {
                        _indexTooptip = 0;
                        if (_viewOnly)
                        {
                            _modeView = ModeView.Select;
                            // In view-only mode return the original initial item, not the browsed cursor.
                            var viewItem = _initialItem ?? _localpaginator!.SelectedItem;
                            ResultCtrl = new ResultPrompt<TableResult<T>>(new TableResult<T>(viewItem.Value, _localpaginator.CurrentIndex, _currentColumnIndex), false);
                            break;
                        }
                        if (_localpaginator.SelectedItem.Disabled)
                        {
                            SetError(PromptPlusResources.SelectionDisabled);
                            break;
                        }
                        var (ok, message) = _predicatevalidselectAsync is not null
                            ? _predicatevalidselectAsync.Invoke(_localpaginator!.SelectedItem.Value).ConfigureAwait(false).GetAwaiter().GetResult()
                            : _predicatevalidselect?.Invoke(_localpaginator!.SelectedItem.Value) ?? (true, null);
                        if (!ok)
                        {
                            if (string.IsNullOrEmpty(message))
                            {
                                SetError(PromptPlusResources.PredicateSelectInvalid);
                            }
                            else
                            {
                                SetError(message);
                            }
                            break;
                        }
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<TableResult<T>>(new TableResult<T>(_localpaginator!.SelectedItem.Value, _localpaginator.CurrentIndex, _currentColumnIndex), false);
                        SaveHistory(_localpaginator!.SelectedItem.Value);
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                    else if ((keyinfo.IsPressFilterActivationKey() && _localpaginator!.SelectedItem != null))
                    {
                        _indexTooptip = 0;
                        _updatePosAnswerBuffer = false;
                        continue;
                    }

                    #endregion

                    else if (keyinfo.IsPressTabKey())
                    {
                        _indexTooptip = 0;
                        _currentColumnIndex++;
                        if (_currentColumnIndex >= _columns.Count)
                        {
                            _currentColumnIndex = 0;
                        }
                        break;
                    }
                    else if (keyinfo.IsPressShiftTabKey())
                    {
                        _indexTooptip = 0;
                        _currentColumnIndex--;
                        if (_currentColumnIndex < 0)
                        {
                            _currentColumnIndex = _columns.Count - 1;
                        }
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
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem)
                        {
                            _localpaginator!.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _localpaginator!.PreviousItem();
                        }
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
                        if (!_localpaginator!.Home())
                        {
                            continue;
                        }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        if (!_localpaginator!.End())
                        {
                            continue;
                        }
                        SetSelectionDisabledErrorIfNeeded();
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter && _filterBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        UpdateFilterFromBuffer();
                        SetSelectionDisabledErrorIfNeeded(ignoreViewOnly: true);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Select && _answerBuffer!.IsPrintable(keyinfo.KeyChar))
                    {
                        var keifilter = keyinfo;
                        if (keifilter.IsPressFilterActivationKey())
                        {
                            keifilter = new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
                        }
                        if (_filterBuffer.TryAcceptedReadlineConsoleKey(keifilter))
                        {
                            _modeView = ModeView.Filter;
                            UpdateFilterFromBuffer();
                            SetSelectionDisabledErrorIfNeeded(ignoreViewOnly: true);
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_answerBuffer!.IsPrintable(keyinfo.KeyChar) && _answerBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _updatePosAnswerBuffer = false;
                        break;
                    }
                    else if (_modeView == ModeView.Select && _localpaginator!.SelectedItem != null && _answerBuffer!.IsPrintable(keyinfo.KeyChar))
                    {
                        string keyChar = keyinfo.KeyChar.ToString();
                        int start = _localpaginator.CurrentIndex;
                        // Use the cached item text instead of re-invoking the (possibly async) text
                        // selector for every item on each keystroke.
                        int index = _items.FindIndex(start + 1, x => x.FilterableText.StartsWith(keyChar, StringComparison.OrdinalIgnoreCase));
                        if (index < 0 && start >= 0)
                        {
                            index = _items.FindIndex(0, x => x.FilterableText.StartsWith(keyChar, StringComparison.OrdinalIgnoreCase));
                        }
                        if (index >= 0)
                        {
                            _localpaginator.EnsureVisibleIndex(index);
                            _indexTooptip = 0;
                            break;
                        }
                    }
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
            // Re-evaluate the effective page size every frame so the visible item count
            // stays in sync with the current console height (after any terminal resize).
            // ComputeTableEffectivePageSize accounts for structural table lines (borders,
            // header, row separators) so the paginator never over- or under-fills the screen.
            int targetPageSize = ComputeTableEffectivePageSize();
            if (targetPageSize != _effectivePageSize)
            {
                _effectivePageSize = targetPageSize;
                _localpaginator?.UpdatePageSize(_effectivePageSize);
            }
            WritePrompt(screenBuffer, _optStyles[TableStyles.Prompt]);
            WriteAnswer(screenBuffer);
            WriteDescription(screenBuffer);
            WriteTable(screenBuffer);
            WriteTooltip(screenBuffer);
            WriteError(screenBuffer, _optStyles[TableStyles.Error]);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _modeView = ModeView.Select;
            _updatePosAnswerBuffer = false;
            string answer = string.Empty;
            if (!ResultCtrl!.Value.IsAborted && _localpaginator!.SelectedItem is not null)
            {
                answer = GetAnswerText(_localpaginator.SelectedItem);
            }
            else if (ResultCtrl!.Value.IsAborted && OptionsControl.ShowMessageAbortKeyValue)
            {
                answer = PromptPlusResources.CanceledKey;
            }
            WritePrompt(screenBuffer, _optStyles[TableStyles.Prompt]);
            if (!_viewOnly)
            {
                screenBuffer.WriteLine(answer, _optStyles[TableStyles.Answer]);
            }
            else
            {
                if (_defaultValue.HasValue)
                {
                    var found = _items.FirstOrDefault(x => !x.Disabled && _DefaultMatchBy.Invoke(x.Value!, _defaultValue.Value));
                    if (found is not null)
                        {
                            screenBuffer.WriteLine(GetAnswerText(found), _optStyles[TableStyles.Answer]);
                        }
                }
                else
                {
                    if (_initialItem is not null)
                        {
                            screenBuffer.WriteLine(GetAnswerText(_initialItem), _optStyles[TableStyles.Answer]);
                        }
                }
            }
            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    GetTooltipSelect()
                ];
                lsttooltips.Add(PromptPlusResources.TooltipPages);
                if (mode == ModeView.Select)
                {
                    if (!_viewOnly && _filterType != FilterMode.Disabled)
                    {
                        lsttooltips.Add(PromptPlusResources.TooltipFilter);
                    }
                    if (!_viewOnly)
                    {
                        lsttooltips.Add(PromptPlusResources.TooltipNavegateTextPrompt);
                    }
                    // Jump-by-first-char is only reachable when filter is disabled (otherwise any
                    // printable key transitions the control into filter mode instead of jumping).
                    if (!_viewOnly && _filterType == FilterMode.Disabled)
                    {
                        lsttooltips.Add(PromptPlusResources.TooltipJump);
                    }
                }
                if (OptionsControl.EnabledAbortKeyValue)
                {
                    lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
                }
                lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
                lsttooltips.AddRange(GetEmacsTooltips(_viewOnly));
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetTooltipSelect()
        {
            StringBuilder tooltip = new();
            if (!_viewOnly)
            {
                tooltip.Append(PromptPlusResources.TooltipEnterFinish);
                tooltip.Append('.');
            }
            tooltip.Append(PromptPlusResources.TooltipBaseNavegate);
            tooltip.Append('.');
            // Tab/ShiftTab navigate between columns — unique table feature.
            tooltip.Append(PromptPlusResources.TooltipTableColumnNav);
            tooltip.Append('.');
            return tooltip.ToString();
        }

        private void UpdateFilterFromBuffer()
        {
            string filter = _filterBuffer.ToString();
            if (!filter.Equals(_lastinput, StringComparison.OrdinalIgnoreCase)) 
            {
                _localpaginator!.UpdateFilter(filter);
            }

            _lastinput = filter;
            if (string.IsNullOrEmpty(filter))
            {
                _modeView = ModeView.Select;
            }
        }

        private void SetSelectionDisabledErrorIfNeeded(bool ignoreViewOnly = false)
        {
            if ((!_viewOnly || ignoreViewOnly) && _localpaginator?.SelectedItem?.Disabled == true)
            {
                SetError(PromptPlusResources.SelectionDisabled);
            }
        }

        /// <summary>
        /// Evaluates the optional selection predicate for <paramref name="value"/>, returning
        /// <c>true</c> when no predicate is configured or when it accepts the value. Used to decide
        /// whether a default/history value may position the cursor (rejected values are not honored).
        /// </summary>
        private bool TrySelectionPredicate(T value)
        {
            if (_predicatevalidselect == null && _predicatevalidselectAsync == null)
            {
                return true;
            }
            (bool ok, _) = _predicatevalidselectAsync != null
                ? _predicatevalidselectAsync.Invoke(value).ConfigureAwait(false).GetAwaiter().GetResult()
                : (_predicatevalidselect?.Invoke(value) ?? (true, (string?)null));
            return ok;

        }

        private void SaveHistory(T selectedValue)
        {
            if (_historyOptions == null)
            {
                return;
            }
            string serializedValue = JsonSerializer.Serialize(selectedValue);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear();
            hist = FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;

        }

        /// <summary>
        /// Computes the effective page size for the table control.
        /// Unlike the base <c>ComputeEffectivePageSize</c>, this method also subtracts
        /// the structural lines that the table frame itself occupies (borders, header,
        /// header/data separator) and divides by the per-item render height, which is
        /// 2 when row separators are visible and 1 when they are hidden.
        /// <list type="bullet">
        ///   <item><description>Structural lines: top border (1), header row + header/data separator (2), bottom border (1) � each suppressed by the matching <see cref="HideTable"/> flag.</description></item>
        ///   <item><description>Per-item height: 1 when <see cref="HideTable.RowSeparator"/> is set, 2 otherwise.</description></item>
        /// </list>
        /// </summary>
        private int ComputeTableEffectivePageSize()
        {
            // Fixed structural lines owned by the table frame
            int structural = _layoutBorders.HasFlag(HideTable.OuterBorder) ? 0 : 1;  // top border
            structural    += _layoutBorders.HasFlag(HideTable.Header)      ? 0 : 2;  // header row + header/data separator
            structural    += _layoutBorders.HasFlag(HideTable.OuterBorder) ? 0 : 1;  // bottom border

            // Each data row costs 1 line; visible row separators add 1 per gap => height = 2
            int itemHeight = _layoutBorders.HasFlag(HideTable.RowSeparator) ? 1 : 2;

            int available = Math.Max(1, ConsoleHandler.Height - ReservedTemplateLines - structural);
            int computed  = Math.Max(1, available / itemHeight);

            return _pageSize == 0 ? computed : Math.Min(_pageSize, computed);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = GetChangeDescriptionValue();
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[TableStyles.Description]);
            }
        }

        private void WriteTable(BufferScreen screenBuffer)
        {
            (_visibleStartCol, _visibleEndCol) = ComputeVisibleColumnRange();

            _previewColumnIndex = (_horizontalScrollMode == HorizontalScrollMode.Full && _visibleEndCol < _columns.Count - 1)
                ? _visibleEndCol + 1
                : -1;

            // rebuild border strings only when the visible column window changes.
            // The cached main strings already include the "  " row-prefix (D4), eliminating
            // per-frame PrefixNonSelectedLine() allocations.
            if (_visibleStartCol != _cachedBorderStartCol
                || _visibleEndCol != _cachedBorderEndCol
                || _previewColumnIndex != _cachedBorderPreviewCol)
            {
                RebuildBorderCache();
                _cachedBorderStartCol   = _visibleStartCol;
                _cachedBorderEndCol     = _visibleEndCol;
                _cachedBorderPreviewCol = _previewColumnIndex;
            }

            if (_cachedTopMain is not null)
            {
                screenBuffer.Write(_cachedTopMain, _optStyles[TableStyles.BorderLines]);
                screenBuffer.WriteLine(_cachedTopPreview, _optStyles[TableStyles.DisabledRow]);
            }

            if (!_layoutBorders.HasFlag(HideTable.Header))
            {
                WriteHeader(screenBuffer);

                if (!string.IsNullOrEmpty(_cachedHeaderSepMain))
                {
                    screenBuffer.Write(_cachedHeaderSepMain, _optStyles[TableStyles.BorderLines]);
                    screenBuffer.WriteLine(_cachedHeaderSepPreview, _optStyles[TableStyles.DisabledRow]);
                }
            }

            WriteDataRows(screenBuffer);

            if (_cachedBottomMain is not null)
            {
                screenBuffer.Write(_cachedBottomMain, _optStyles[TableStyles.BorderLines]);
                screenBuffer.WriteLine(_cachedBottomPreview, _optStyles[TableStyles.DisabledRow]);
            }

            if (_localpaginator!.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                // D8: string.Concat avoids the interpolation overhead; colInfo is the common empty case.
                string colInfo = string.Concat($"{PromptPlusResources.Col_Info}: ", (_currentColumnIndex + 1).ToString(CultureInfo.InvariantCulture), "/", _columns.Count.ToString(CultureInfo.InvariantCulture), ".");
                screenBuffer.WriteLine(string.Concat(template, colInfo), _optStyles[TableStyles.Pagination]);
            }
        }

        /// <summary>
        /// Rebuilds all cached border strings whenever the visible column window
        /// (<see cref="_visibleStartCol"/>, <see cref="_visibleEndCol"/>, <see cref="_previewColumnIndex"/>)
        /// changes. Each main string already includes the two-space row prefix (D4) so
        /// <see cref="WriteTable"/> and <see cref="WriteDataRows"/> can Write them directly
        /// without a per-frame <c>PrefixNonSelectedLine</c> allocation.
        /// </summary>
        private void RebuildBorderCache()
        {
            var (topMain, topPreview) = BuildTopBorder();
            _cachedTopMain    = topMain is not null ? string.Concat("  ", topMain) : null;
            _cachedTopPreview = topPreview;

            var (hSepMain, hSepPreview) = BuildHeaderSeparator();
            _cachedHeaderSepMain    = !string.IsNullOrEmpty(hSepMain) ? string.Concat("  ", hSepMain) : string.Empty;
            _cachedHeaderSepPreview = hSepPreview;

            var (rSepMain, rSepPreview) = BuildRowSeparator();
            _cachedRowSepMain    = !string.IsNullOrEmpty(rSepMain) ? string.Concat("  ", rSepMain) : string.Empty;
            _cachedRowSepPreview = rSepPreview;

            var (botMain, botPreview) = BuildBottomBorder();
            _cachedBottomMain    = botMain is not null ? string.Concat("  ", botMain) : null;
            _cachedBottomPreview = botPreview;
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Select)
            {
                string text = string.Empty;
                if (_localpaginator!.SelectedIndex >= 0)
                {
                    text = GetAnswerText(_localpaginator!.SelectedItem);
                }
                if (_updatePosAnswerBuffer)
                {
                    _answerBuffer!.LoadPrintable(text);
                    _answerBuffer.ToHome();
                }
                int promptWidth = GetPromptDisplayWidth();
                (string visibleLeft, string visibleRight) = ViewportSlice(_answerBuffer!, promptWidth);
                screenBuffer.Write(visibleLeft, _optStyles[TableStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.WriteLine(visibleRight, _optStyles[TableStyles.Answer]);
            }
            else if (_modeView == ModeView.Filter)
            {
                WriteAnswerFilter(screenBuffer);
            }
            else
            {
                throw new NotImplementedException($"ModeView {_modeView} not implemented.");
            }
        }

        private void WriteAnswerFilter(BufferScreen screenBuffer)
        {
            Style found = _optStyles[TableStyles.TaggedInfo];
            if (_localpaginator!.TotalCount == 0)
            {
                found = _optStyles[TableStyles.Error];
            }
            int promptWidth = GetPromptDisplayWidth();
            (string visibleLeft, string visibleRight) = ViewportSlice(_filterBuffer, promptWidth);
            screenBuffer.Write(visibleLeft, found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(visibleRight, found);
            screenBuffer.WriteLine($" ({PromptPlusResources.Filter})", _optStyles[TableStyles.TaggedInfo]);
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
            screenBuffer.WriteLine(tooltip, _optStyles[TableStyles.Tooltips]);
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

        private void CalculateColumnWidths()
        {
            for (int colIndex = 0; colIndex < _columns.Count; colIndex++)
            {
                ColumnDefinition<T> column = _columns[colIndex];

                // Suffix width: filterable columns need an extra leading space before the filter symbol.
                int suffixWidth = column.IsFilterable
                    ? HeaderSelectionSuffixWidth + 1
                    : HeaderSelectionSuffixWidth;

                if (column.Width.HasValue)
                {
                    // The declared width is a minimum. Expand it when it is too narrow
                    // to render the header text together with the marker + suffix decorations
                    // (which always occupy HeaderSelectionPrefixWidth + suffixWidth chars).
                    int minForHeader = column.Header.Length + HeaderSelectionPrefixWidth + suffixWidth;
                    column.CalculatedWidth = Math.Max(column.Width.Value, minForHeader);
                    continue;
                }

                // Auto-width: start from the header+decoration minimum, then expand to
                // the widest formatted cell value across all items.
                int autoWidth = column.Header.Length + HeaderSelectionPrefixWidth + suffixWidth;

                foreach (ItemTable<T> item in _items)
                {
                    if (colIndex < item.CachedCellValues.Length)
                    {
                        int cellLen = item.CachedCellValues[colIndex]?.Length ?? 0;
                        if (cellLen > autoWidth)
                        {
                            autoWidth = cellLen;
                        }
                    }
                }

                column.CalculatedWidth = autoWidth;
            }

            // keep a running total so ComputeVisibleColumnRange() can use O(1) width check
            // instead of a LINQ .Sum() with delegate allocation on every render frame.
            _totalColumnsWidth = 0;
            for (int i = 0; i < _columns.Count; i++)
            {
                _totalColumnsWidth += _columns[i].CalculatedWidth;
            }
        }

        private void BuildItemsCache()
        {
            int colCount = _columns.Count;
            // one shared StringBuilder reused across all items avoids a per-item allocation;
            // the pre-sized string[] replaces the intermediate List<(string,bool)> + LINQ spread;
            // the filterable-text loop replaces the .Where().Select() LINQ chain.
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
                item.FilterableText = sbFilter.Length > 0 ? sbFilter.ToString() : string.Empty;
            }
        }

        /// <summary>
        /// Computes the last visible column index when fitting columns from <paramref name="startCol"/>
        /// within <paramref name="available"/> width.
        /// </summary>
        private int ComputeEndColumnFromStart(int startCol, int available, bool hasSep)
        {
            int endCol = startCol;
            int used = _columns[startCol].CalculatedWidth;
            for (int i = startCol + 1; i < _columns.Count; i++)
            {
                int add = _columns[i].CalculatedWidth + (hasSep ? 1 : 0);
                if (used + add > available)
                {
                    break;
                }
                used += add;
                endCol = i;
            }
            return endCol;
        }

        /// <summary>
        /// Computes the first visible column index when the window ends at <paramref name="endCol"/>.
        /// </summary>
        private int ComputeStartColumnFromEnd(int endCol, int available, bool hasSep)
        {
            int startCol = endCol;
            int used = _columns[endCol].CalculatedWidth;
            for (int i = endCol - 1; i >= 0; i--)
            {
                int add = _columns[i].CalculatedWidth + (hasSep ? 1 : 0);
                if (used + add > available)
                {
                    break;
                }
                used += add;
                startCol = i;
            }
            return startCol;
        }

        /// <summary>
        /// Computes the visible column range [startCol, endCol] that fits within the console width,
        /// ensuring <see cref="_currentColumnIndex"/> is always within the visible window.
        /// Updates <see cref="_columnScrollOffset"/> as needed.
        /// </summary>
        private (int startCol, int endCol) ComputeVisibleColumnRange()
        {
            if (_columns.Count == 0)
            {
                return (0, -1);
            }

            // Hidden separators are replaced by a space so they still occupy 1 char;
            // only TableLayoutMode.None removes them entirely.
            bool hasOuterBorder = _layoutMode != TableLayoutMode.None;
            bool hasSep = _layoutMode != TableLayoutMode.None;

            // available width = console width - row prefix (2) - outer borders (2 if present)
            int available = ConsoleHandler.Width
                - 2
                - (hasOuterBorder ? 2 : 0);

            // If everything fits, no scrolling needed (D1: use cached sum — O(1) instead of O(columns) LINQ).
            int totalWidth = _totalColumnsWidth
                + (hasSep ? Math.Max(0, _columns.Count - 1) : 0);
            if (totalWidth <= available)
            {
                _columnScrollOffset = 0;
                return (0, _columns.Count - 1);
            }

            // Clamp scroll offset to valid range
            _columnScrollOffset = Math.Clamp(_columnScrollOffset, 0, _columns.Count - 1);

            // Find end column reachable from current scroll offset
            int endCol = ComputeEndColumnFromStart(_columnScrollOffset, available, hasSep);

            bool needsEndColRecalc = false;
            if (_currentColumnIndex < _columnScrollOffset)
            {
                // Selected column is to the left — shift the window left.
                // Full mode: re-anchor so the selected column becomes the right edge.
                // Column mode: minimum shift — selected column becomes the left edge.
                _columnScrollOffset = _horizontalScrollMode == HorizontalScrollMode.Full
                    ? ComputeStartColumnFromEnd(_currentColumnIndex, available, hasSep)
                    : _currentColumnIndex;
                needsEndColRecalc = true;
            }
            else if (_currentColumnIndex > endCol)
            {
                // Selected column is to the right — shift the window right.
                // Both modes anchor the selected column as the right edge of the new window.
                _columnScrollOffset = ComputeStartColumnFromEnd(_currentColumnIndex, available, hasSep);
                needsEndColRecalc = true;
            }

            if (needsEndColRecalc)
            {
                endCol = ComputeEndColumnFromStart(_columnScrollOffset, available, hasSep);
            }

            return (_columnScrollOffset, endCol);
        }

        private (string? main, string preview) BuildTopBorder()
        {
            if (_layoutBorders.HasFlag(HideTable.OuterBorder) || _layoutMode == TableLayoutMode.None)
            {
                return (null, string.Empty);
            }

            var (main, preview) = BuildHorizontalGridLine(
                SymbolType.GridSingleTopLeft,
                SymbolType.GridSingleTopCenter,
                SymbolType.GridSingleTopRight,
                SymbolType.GridSingleBorderTop);
            return (main, preview);
        }

        private (string main, string preview) BuildHeaderSeparator()
        {
            if (_layoutBorders.HasFlag(HideTable.Header) || _layoutMode == TableLayoutMode.None)
            {
                return (string.Empty, string.Empty);
            }

            return BuildHorizontalGridLine(
                SymbolType.GridSingleMiddleLeft,
                SymbolType.GridSingleMiddleCenter,
                SymbolType.GridSingleMiddleRight,
                SymbolType.GridSingleDividerX);
        }

        private (string main, string preview) BuildRowSeparator()
        {
            if (_layoutBorders.HasFlag(HideTable.RowSeparator) || _layoutMode == TableLayoutMode.None)
            {
                return (string.Empty, string.Empty);
            }

            return BuildHorizontalGridLine(
                SymbolType.GridSingleMiddleLeft,
                SymbolType.GridSingleMiddleCenter,
                SymbolType.GridSingleMiddleRight,
                SymbolType.GridSingleDividerX);
        }

        private (string? main, string preview) BuildBottomBorder()
        {
            if (_layoutBorders.HasFlag(HideTable.OuterBorder) || _layoutMode == TableLayoutMode.None)
            {
                return (null, string.Empty);
            }

            var (main, preview) = BuildHorizontalGridLine(
                SymbolType.GridSingleBottomLeft,
                SymbolType.GridSingleBottomCenter,
                SymbolType.GridSingleBottomRight,
                SymbolType.GridSingleBorderBottom);
            return (main, preview);
        }

        /// <summary>
        /// Returns the character string for a symbol, selecting the double-line variant
        /// and/or forcing ASCII output based on the current <see cref="_layoutMode"/>.
        /// </summary>
        private string GetLayoutSymbol(SymbolType singleSymbol, SymbolType doubleSymbol)
        {
            bool useUnicode = _layoutMode is not (TableLayoutMode.SingleASCII or TableLayoutMode.DoubleASCII);
            SymbolType type = _layoutMode is TableLayoutMode.DoubleBox or TableLayoutMode.DoubleASCII
                ? doubleSymbol
                : singleSymbol;
            return GetSymbol(type, useUnicode);
        }

        /// <summary>
        /// Maps a <c>GridSingle*</c> symbol to its <c>GridDouble*</c> counterpart.
        /// </summary>
        private static SymbolType ToDoubleSymbol(SymbolType single) => single switch
        {
            SymbolType.GridSingleTopLeft       => SymbolType.GridDoubleTopLeft,
            SymbolType.GridSingleTopCenter     => SymbolType.GridDoubleTopCenter,
            SymbolType.GridSingleTopRight      => SymbolType.GridDoubleTopRight,
            SymbolType.GridSingleMiddleLeft    => SymbolType.GridDoubleMiddleLeft,
            SymbolType.GridSingleMiddleCenter  => SymbolType.GridDoubleMiddleCenter,
            SymbolType.GridSingleMiddleRight   => SymbolType.GridDoubleMiddleRight,
            SymbolType.GridSingleBottomLeft    => SymbolType.GridDoubleBottomLeft,
            SymbolType.GridSingleBottomCenter  => SymbolType.GridDoubleBottomCenter,
            SymbolType.GridSingleBottomRight   => SymbolType.GridDoubleBottomRight,
            SymbolType.GridSingleBorderLeft    => SymbolType.GridDoubleBorderLeft,
            SymbolType.GridSingleBorderRight   => SymbolType.GridDoubleBorderRight,
            SymbolType.GridSingleBorderTop     => SymbolType.GridDoubleBorderTop,
            SymbolType.GridSingleBorderBottom  => SymbolType.GridDoubleBorderBottom,
            SymbolType.GridSingleDividerY      => SymbolType.GridDoubleDividerY,
            SymbolType.GridSingleDividerX      => SymbolType.GridDoubleDividerX,
            _ => single
        };

        private (string main, string preview) BuildHorizontalGridLine(
            SymbolType leftSymbol,
            SymbolType centerSymbol,
            SymbolType rightSymbol,
            SymbolType horizontalSymbol)
        {
            char left       = GetLayoutSymbol(leftSymbol,       ToDoubleSymbol(leftSymbol))[0];
            char center     = GetLayoutSymbol(centerSymbol,     ToDoubleSymbol(centerSymbol))[0];
            char horizontal = GetLayoutSymbol(horizontalSymbol, ToDoubleSymbol(horizontalSymbol))[0];
            char right      = GetLayoutSymbol(rightSymbol,      ToDoubleSymbol(rightSymbol))[0];

            // When outer border is hidden replace the outer edge chars with the fill char
            // so header/row separator lines remain visually consistent.
            bool hideOuter = _layoutBorders.HasFlag(HideTable.OuterBorder) || _layoutMode == TableLayoutMode.None;
            char effectiveLeft  = hideOuter ? horizontal : left;
            char effectiveRight = hideOuter ? horizontal : right;

            StringBuilder main = new();
            main.Append(effectiveLeft);
            for (int i = _visibleStartCol; i <= _visibleEndCol; i++)
            {
                main.Append(GetRepeated(horizontal, _columns[i].CalculatedWidth));
                if (i < _visibleEndCol)
                {
                    main.Append(_layoutBorders.HasFlag(HideTable.ColumnSeparator) ? horizontal : center);
                }
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

        private static string BuildCell(string value, ColumnDefinition<T> column)
        {
            int displayWidth = column.CalculatedWidth;
            return AlignCell(Truncate(value, displayWidth), displayWidth, column.Alignment);
        }

        private void WriteHeader(BufferScreen screenBuffer)
        {
            char separator = !_layoutBorders.HasFlag(HideTable.ColumnSeparator)
                ? GetLayoutSymbol(SymbolType.GridSingleDividerY, SymbolType.GridDoubleDividerY)[0]
                : ' ';

            screenBuffer.Write("  ", _optStyles[TableStyles.BorderLines]);

            if (_layoutMode != TableLayoutMode.None)
            {
                char borderLeft = _layoutBorders.HasFlag(HideTable.OuterBorder)
                    ? ' '
                    : GetLayoutSymbol(SymbolType.GridSingleBorderLeft, SymbolType.GridDoubleBorderLeft)[0];
                screenBuffer.Write(borderLeft, _optStyles[TableStyles.BorderLines]);
            }

            for (int i = _visibleStartCol; i <= _visibleEndCol; i++)
            {
                ColumnDefinition<T> col = _columns[i];
                bool isSelectedColumn = i == _currentColumnIndex;

                if (i > _visibleStartCol)
                {
                    screenBuffer.Write(separator, _optStyles[TableStyles.BorderLines]);
                }

                bool showSelection = isSelectedColumn;

                // use pre-cached symbol strings instead of per-frame interpolation allocations.
                string marker = showSelection ? _cachedSelectorMarker : "  ";

                // Leading space before filter symbol ensures visual separation from header text.
                string suffix = col.IsFilterable ? _cachedFilterableSuffix : "  ";

                string header = col.Header;
                int colWidth = col.CalculatedWidth;
                int availableAfterMarker = Math.Max(0, colWidth - marker.Length);
                int headerVisibleLength = Math.Min(header.Length, availableAfterMarker);
                int suffixLength = Math.Min(
                    suffix.Length,
                    Math.Max(0, availableAfterMarker - headerVisibleLength));

                int textWidth = Math.Max(0, availableAfterMarker - suffixLength);
                string headerText = textWidth == 0
                    ? string.Empty
                    : AlignCell(header, textWidth, ColumnAlignment.Left);
                string cell = Truncate(marker + headerText + suffix, colWidth);

                screenBuffer.Write(cell, showSelection ? _optStyles[TableStyles.SelectedCell] : _optStyles[TableStyles.HeaderText]);
            }

            if (_previewColumnIndex >= 0)
            {
                ColumnDefinition<T> previewCol = _columns[_previewColumnIndex];
                screenBuffer.Write(separator, _optStyles[TableStyles.DisabledRow]);
                string previewHeader = previewCol.Header;
                int previewWidth = previewCol.CalculatedWidth;
                int previewAvailable = Math.Max(0, previewWidth - HeaderSelectionPrefixWidth);
                string previewText = AlignCell(Truncate(previewHeader, previewAvailable), previewWidth - HeaderSelectionPrefixWidth, ColumnAlignment.Left);
                string previewCell = Truncate("  " + previewText, previewWidth);
                screenBuffer.Write(previewCell, _optStyles[TableStyles.DisabledRow]);
            }

            if (_layoutMode != TableLayoutMode.None)
            {
                char borderRight = _layoutBorders.HasFlag(HideTable.OuterBorder)
                    ? ' '
                    : GetLayoutSymbol(SymbolType.GridSingleBorderRight, SymbolType.GridDoubleBorderRight)[0];
                screenBuffer.Write(borderRight, _optStyles[TableStyles.BorderLines]);
            }

            screenBuffer.WriteLine(string.Empty, ConsoleHandler.CurrentStyle);
        }

        private void WriteDataRows(BufferScreen screenBuffer)
        {
            if (_localpaginator!.TotalCountValid == 0)
            {
                return;
            }
            char separator = (!_layoutBorders.HasFlag(HideTable.ColumnSeparator)
                ? GetLayoutSymbol(SymbolType.GridSingleDividerY, SymbolType.GridDoubleDividerY)[0]
                : ' ');
            ArraySegment<ItemTable<T>> subset = _localpaginator!.GetPageData();
            // row separator is identical for every row — evaluate the cached flag once per frame.
            bool hasRowSep = !string.IsNullOrEmpty(_cachedRowSepMain);
            int index = 0;
            foreach (ItemTable<T> item in subset)
            {
                bool isSelectedRow = item.UniqueId == (_localpaginator.SelectedItem?.UniqueId ?? string.Empty);
                Style rowTextStyle = (item.Disabled
                    ? _optStyles[TableStyles.DisabledRow]
                    : (isSelectedRow ? _optStyles[TableStyles.SelectedCell] : _optStyles[TableStyles.UnselectedCell]));

                WriteRowPrefix(screenBuffer, isSelectedRow);

                if (_layoutMode != TableLayoutMode.None)
                {
                    char borderLeft = _layoutBorders.HasFlag(HideTable.OuterBorder)
                        ? ' '
                        : GetLayoutSymbol(SymbolType.GridSingleBorderLeft, SymbolType.GridDoubleBorderLeft)[0];
                    screenBuffer.Write(borderLeft, _optStyles[TableStyles.BorderLines]);
                }

                for (int i = _visibleStartCol; i <= _visibleEndCol; i++)
                {
                    if (i > _visibleStartCol)
                    {
                        screenBuffer.Write(separator, _optStyles[TableStyles.BorderLines]);
                    }

                    string cellText = BuildCell(item.CachedCellValues[i]!, _columns[i]);
                    screenBuffer.Write(cellText, rowTextStyle);
                }

                if (_previewColumnIndex >= 0)
                {
                    screenBuffer.Write(separator, _optStyles[TableStyles.DisabledRow]);
                    string previewCellText = BuildCell(item.CachedCellValues[_previewColumnIndex]!, _columns[_previewColumnIndex]);
                    screenBuffer.Write(previewCellText, _optStyles[TableStyles.DisabledRow]);
                }

                if (_layoutMode != TableLayoutMode.None)
                {
                    char borderRight = _layoutBorders.HasFlag(HideTable.OuterBorder)
                        ? ' '
                        : GetLayoutSymbol(SymbolType.GridSingleBorderRight, SymbolType.GridDoubleBorderRight)[0];
                    screenBuffer.Write(borderRight, _optStyles[TableStyles.BorderLines]);
                }
                screenBuffer.WriteLine(string.Empty, ConsoleHandler.CurrentStyle);

                index++;
                if (index < subset.Count && hasRowSep)
                {
                    screenBuffer.Write(_cachedRowSepMain, _optStyles[TableStyles.BorderLines]);
                    screenBuffer.WriteLine(_cachedRowSepPreview, _optStyles[TableStyles.DisabledRow]);
                }
            }
        }

        private void WriteRowPrefix(BufferScreen screenBuffer, bool isSelectedRow)
        {
            if (isSelectedRow)
            {
                screenBuffer.Write(GetSymbol(SymbolType.Selector)[0], _optStyles[TableStyles.SelectedCell]);
                screenBuffer.Write(" ", _optStyles[TableStyles.BorderLines]);
            }
            else
            {
                screenBuffer.Write("  ", _optStyles[TableStyles.BorderLines]);
            }
        }

        private static string Truncate(string value, int width)
        {
            if (width <= 0)
            {
                return string.Empty;
            }

            return value.Length <= width ? value : value[..width];
        }

        private static string AlignCell(string value, int width, ColumnAlignment alignment)
        {
            string normalized = value.Length > width ? value[..width] : value;
            int missing = width - normalized.Length;
            if (missing <= 0)
            {
                return normalized;
            }

            return alignment switch
            {
                ColumnAlignment.Right => new string(' ', missing) + normalized,
                ColumnAlignment.Center => new string(' ', missing / 2) + normalized + new string(' ', missing - (missing / 2)),
                _ => normalized + new string(' ', missing)
            };
        }

       private string? GetChangeDescriptionValue()
        {
            if (_localpaginator!.SelectedItem is null)
            {
                return OptionsControl.DescriptionValue;
            }

            if (_changeDescriptionAsync is not null)
            {
                return _changeDescriptionAsync(_localpaginator.SelectedItem.Value)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }

            if (_changeDescription is not null)
            {
                return _changeDescription(_localpaginator.SelectedItem.Value);
            }

            return OptionsControl.DescriptionValue;
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

        private string GetColumnFilterText(ItemTable<T> item)
        {
            int col = Math.Max(0, Math.Min(_currentColumnIndex, item.CachedCellValues.Length - 1));
            if (!_columns[col].IsFilterable)
            {
                return string.Empty;
            }
            return item.CachedCellValues.Length > 0 ? item.CachedCellValues[col] ?? string.Empty : string.Empty;
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

        private string GetRepeated(char ch, int count)
        {
            if (count <= 0)
            {
                return string.Empty;
            }

            if (!_lineCache.TryGetValue((ch, count), out var result))
            {
                result = new string(ch, count);
                _lineCache[(ch, count)] = result;
            }

            return result;
        }
    }
}
