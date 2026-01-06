// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using EastAsianWidthDotNet;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace PromptPlusLibrary.Controls.TableSelect
{
    internal sealed class TableSelectControl<T> : BaseControlPrompt<T>, ITableWidget<T>, ITableSelectControl<T> where T : class
    {
        private readonly Dictionary<TableStyles, Style> _optStyles = BaseControlOptions.LoadStyle<TableStyles>();
        private int _pageSize;
        private bool _autoSelect;
        private Func<T, (bool, string?)>? _predicatevalidselect;
        private readonly List<ItemTableRow<T>> _items = [];
        private Func<T, T, bool> _equalItems = (x, y) => x?.Equals(y) ?? false;
        private Func<T, string>? _textSelector;
        private Optional<T> _defaultValue = Optional<T>.Empty();
        private FilterMode _filterType = FilterMode.Disabled;
        private bool _filterCaseinsensitive;
        private int[] _filterColumns = [];
        private TableLayout _layout = TableLayout.SingleGridFull;
        private readonly List<ItemColumn<T>> _columns = [];
        private readonly bool _hideSelectorRow;
        private bool _hideHeaders;
        private bool _separatorRows;
        private bool _autoFill;
        private int? _minColWidth;
        private int? _maxColWidth;
        private readonly Dictionary<Type, Func<object, string>> _formatTypes = [];
        private Func<T, int, int, string>? _changeDescription;
        private Paginator<ItemTableRow<T>>? _localpaginator;
        private readonly EmacsBuffer _filterBuffer;
        private string _lastinput;
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;

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
        private int _indexTooptip;
        private string _tooltipModeSelect = string.Empty;
        private int _currentrow = -1;
        private int _currentcol = -1;
        private int _totalTableLenWidth;
        private int[] _maxlencontentcols = [];
        private (int startWrite, int endwrite) _tableviewport;
        private enum MoveViewport
        {
            None,
            Left,
            Right
        }
        private MoveViewport _moveviewport = MoveViewport.None;
        private byte _maxWidth;
        private EmacsBuffer? _answerBuffer;
        private bool _updatePosAnswerBuffer;

#pragma warning disable IDE0079 
#pragma warning disable IDE0290 // Use primary constructor
        public TableSelectControl(bool isWidget, IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _filterBuffer = new(false, CaseOptions.Any, (_) => true, ConfigPlus.MaxLenghtFilterText);
            _hideSelectorRow = isWidget;
            _lastinput = string.Empty;
            _pageSize = isWidget ? int.MaxValue : ConfigPlus.PageSize;
            _maxWidth = ConfigPlus.MaxWidth;
        }
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning restore IDE0079


        #region  ITable...

        ITableWidget<T> ITableWidget<T>.Interaction(IEnumerable<T> items, Action<T, ITableWidget<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T? item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        ITableWidget<T> ITableWidget<T>.AddItem(T value, bool disable)
        {
            SetAddItem(value, disable);
            return this;
        }

        ITableWidget<T> ITableWidget<T>.AddItems(IEnumerable<T> values, bool disable)
        {
            foreach (T? item in values)
            {
                SetAddItem(item, disable);
            }
            return this;
        }

        ITableWidget<T> ITableWidget<T>.Layout(TableLayout value)
        {
            SetLayout(value);
            return this;
        }

        ITableWidget<T> ITableWidget<T>.Styles(TableStyles styleType, Style style)
        {
            SetStyles(styleType, style);
            return this;
        }

        ITableWidget<T> ITableWidget<T>.AddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format, TextAlignment alignment, string? title, TextAlignment titlealignment, bool titlereplaceswidth, bool textcrop, int? maxslidinglines)
        {
            SetAddColumn(field, width, format, alignment, title, titlealignment, titlereplaceswidth, textcrop, maxslidinglines);
            return this;
        }

        ITableWidget<T> ITableWidget<T>.AutoFill(int? minwidth, int? maxwidth)
        {
            SetAutoFill(minwidth, maxwidth);
            return this;
        }

        ITableWidget<T> ITableWidget<T>.SeparatorRows(bool value)
        {
            _separatorRows = value;
            return this;
        }

        ITableWidget<T> ITableWidget<T>.HideHeaders(bool value)
        {
            _hideHeaders = value;
            return this;
        }

        ITableWidget<T> ITableWidget<T>.AddFormatType<T1>(Func<object, string> funcfomatType)
        {
            SetAddFormatType<T1>(funcfomatType);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.PredicateSelected(Func<T, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.PredicateSelected(Func<T, bool> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = (input) =>
            {
                bool fn = validselect(input);
                if (fn)
                {
                    return (true, null);
                }
                return (false, null);
            };
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.Default(T value, bool useDefaultHistory)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            _defaultValue = Optional<T>.Set(value);
            _useDefaultHistory = useDefaultHistory;
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.EnabledHistory(string filename, Action<IHistoryOptions>? options)
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

        ITableSelectControl<T> ITableSelectControl<T>.Interaction(IEnumerable<T> items, Action<T, ITableSelectControl<T>> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);

            foreach (T? item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.PageSize(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.AddItem(T value, bool disable)
        {
            SetAddItem(value, disable);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.AddItems(IEnumerable<T> values, bool disable)
        {
            foreach (T? item in values)
            {
                SetAddItem(item, disable);
            }
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.EqualItems(Func<T, T, bool> value)
        {
            SetEqualitem(value);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.TextSelector(Func<T, string> value)
        {
            _textSelector = value ?? throw new ArgumentNullException(nameof(value));
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.FilterByColumns(FilterMode filter, bool caseinsensitive, params int[] indexColumn)
        {
            if (indexColumn.Length == 0)
            {
                throw new ArgumentException("At least 1 column must be informed");
            }
            if (indexColumn.Length > 1 && filter == FilterMode.StartsWith)
            {
                throw new ArgumentException("Only 1 column must be entered for the 'StartsWith' filter");
            }
            _filterType = filter;
            _filterCaseinsensitive = caseinsensitive;
            _filterColumns = filter == FilterMode.Disabled ? [] : indexColumn;
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.Layout(TableLayout value)
        {
            SetLayout(value);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.ChangeDescription(Func<T, int, int, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.Styles(TableStyles styleType, Style style)
        {
            SetStyles(styleType, style);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.AddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format, TextAlignment alignment, string? title, TextAlignment titlealignment, bool titlereplaceswidth, bool textcrop, int? maxslidinglines)
        {
            SetAddColumn(field, width, format, alignment, title, titlealignment, titlereplaceswidth, textcrop, maxslidinglines);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.AutoFill(int? minwidth, int? maxwidth)
        {
            SetAutoFill(minwidth, maxwidth);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.SeparatorRows(bool value)
        {
            _separatorRows = value;
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.HideHeaders(bool value)
        {
            _hideHeaders = value;
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.AddFormatType<T1>(Func<object, string> funcfomatType)
        {
            SetAddFormatType<T1>(funcfomatType);
            return this;
        }

        ITableSelectControl<T> ITableSelectControl<T>.AutoSelect(bool value)
        {
            _autoSelect = value;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            //Validate has columns
            if (_columns.Count == 0)
            {
                throw new InvalidOperationException("Not found columns definition");
            }

            _answerBuffer = new(true, CaseOptions.Any, (_) => true, int.MaxValue, _maxWidth);
            _updatePosAnswerBuffer = true;
            _textSelector ??= DefaultTextSeletor();

            //Validate layout UnicodeSupported
            if (!ConsolePlus.IsUnicodeSupported)
            {
                switch (_layout)
                {
                    case TableLayout.SingleGridFull:
                        _layout = TableLayout.AsciiSingleGridFull;
                        break;
                    case TableLayout.SingleGridSoft:
                        _layout = TableLayout.AsciiSingleGridSoft;
                        break;
                    case TableLayout.DoubleGridFull:
                        _layout = TableLayout.AsciiDoubleGridFull;
                        break;
                    case TableLayout.DoubleGridSoft:
                        _layout = TableLayout.AsciiDoubleGridSoft;
                        break;
                }
            }

            //re-calc column width when IsColumnsNavigation
            for (int i = 0; i < _columns.Count; i++)
            {
                int w = !_columns[i].TitleReplacesWidth ?
                    _columns[i].Width
                    : (_columns[i].Width < ((_columns[i].Title?.Length) ?? 0) ?
                        _columns[i].Title!.Length
                        : _columns[i].Width);
                _columns[i].Width = w;
                _columns[i].OriginalWidth = w;
            }

            //add column selector
            if (!_hideSelectorRow)
            {
                _columns.Insert(0, new ItemColumn<T> { Field = ((_) => ConfigPlus.GetSymbol(SymbolType.Selector)), Title = " ", Width = 1, OriginalWidth = 1 });
            }

            _currentcol = 0;
            if (!_hideSelectorRow)
            {
                _currentcol = 1;
            }

            //cache Values text
            if (_autoFill)
            {
                _maxlencontentcols = new int[_columns.Count];
            }
            foreach (ItemTableRow<T> item in _items)
            {
                List<string> cols = [];
                for (int i = 0; i < _columns.Count; i++)
                {
                    cols.Add(GetTextColumn(item.Value, _columns[i].Field, _columns[i].Format));
                }
                item.TextColumns = [.. cols];
                if (_filterType != FilterMode.Disabled)
                {
                    item.SearchContent = _filterCaseinsensitive ? SearchContent(item, _filterColumns).ToUpperInvariant() : SearchContent(item, _filterColumns);
                }
                //Calculate max content Length
                if (_autoFill)
                {
                    for (int i = 0; i < _columns.Count; i++)
                    {
                        string text = item.TextColumns[i];
                        int lentext = text.NormalizeNewLines().Split(Environment.NewLine).Max(x => x.Length);
                        if (_minColWidth.HasValue && lentext < _minColWidth.Value)
                        {
                            if (i == 0 && _hideSelectorRow)
                            {
                                lentext = _minColWidth.Value;
                                _columns[i].OriginalWidth = lentext;
                                _columns[i].Width = lentext;
                            }
                        }

                        if (_maxColWidth.HasValue && lentext > _maxColWidth.Value)
                        {
                            lentext = _maxColWidth.Value;
                            if (_columns[i].OriginalWidth < lentext)
                            {
                                _columns[i].OriginalWidth = lentext;
                                _columns[i].Width = lentext;
                            }
                        }
                        else
                        {
                            if (_columns[i].OriginalWidth < lentext)
                            {
                                _columns[i].OriginalWidth = lentext;
                                _columns[i].Width = lentext;
                            }
                        }
                        if (_maxlencontentcols[i] < lentext)
                        {
                            _maxlencontentcols[i] = lentext;
                        }
                    }
                }
            }

            //Calculate max content Length
            _totalTableLenWidth = _columns.Count + 1 + _columns.Sum(x => x.OriginalWidth);
            _tableviewport = (0, _totalTableLenWidth);

            //Set Default value
            if (_historyOptions != null)
            {
                _itemHistories = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    try
                    {
                        _defaultValue = Optional<T>.Set(JsonSerializer.Deserialize<T>(_itemHistories[0].History!)!);
                    }
                    catch (Exception)
                    {
                        //invalid Deserialize history 
                    }
                }
            }

            Optional<T> defvalue = Optional<T>.Empty();
            Optional<ItemTableRow<T>> defvaluepage = Optional<ItemTableRow<T>>.Empty();
            if (_defaultValue.HasValue)
            {
                defvalue = Optional<T>.Set(_defaultValue.Value);
            }
            if (defvalue.HasValue)
            {
                ItemTableRow<T>? found = _items.FirstOrDefault(x => _equalItems(x.Value, defvalue.Value));
                if (found != null && !found.Disabled)
                {
                    defvaluepage = Optional<ItemTableRow<T>>.Set(found);
                }
            }

            _localpaginator = IsWidgetControl || _filterType == FilterMode.Disabled
                ? new Paginator<ItemTableRow<T>>(
                    FilterMode.Disabled,
                    _items,
                    _pageSize,
                    defvaluepage,
                    (item1, item2) => item1.UniqueId == item2.UniqueId,
                    null,
                    IsEnnabled)
                : new Paginator<ItemTableRow<T>>(
                    _filterType,
                    _items,
                    _pageSize,
                    defvaluepage,
                    (item1, item2) => item1.UniqueId == item2.UniqueId,
                    (item) => item.SearchContent,
                    IsEnnabled);
            _currentrow = _localpaginator.CurrentIndex;

            if (_localpaginator.SelectedItem == null)
            {
                _localpaginator.FirstItem();
            }
            if (_localpaginator.SelectedItem!.Disabled)
            {
                SetError(Messages.SelectionDisabled);
            }
            if (!IsWidgetControl)
            {
                _tooltipModeSelect = GetTooltipModeSelect();
                LoadTooltipToggle();
            }
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    _updatePosAnswerBuffer = true;

                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltip

                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                        ResultCtrl = new ResultPrompt<T>(default, true);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        _modeView = ModeView.Select;
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                        ResultCtrl = new ResultPrompt<T>(default, true);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey() && _localpaginator!.SelectedItem != null)
                    {
                        _indexTooptip = 0;
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(_localpaginator.SelectedItem.Value) ?? (true, null);
                        if (!ok)
                        {
                            if (string.IsNullOrEmpty(message))
                            {
                                SetError(Messages.PredicateSelectInvalid);
                            }
                            else
                            {
                                SetError(message);
                            }
                            break;
                        }
                        if (_localpaginator.SelectedItem.Disabled)
                        {
                            SetError(Messages.SelectionDisabled);
                            break;
                        }
                        _modeView = ModeView.Select;
                        ResultCtrl = new ResultPrompt<T>(_localpaginator!.SelectedItem.Value, false);
                        SaveHistory(_localpaginator!.SelectedItem.Value);
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
                    #endregion

                    else if (_filterType != FilterMode.Disabled && ConfigPlus.HotKeyFilterMode.Equals(keyinfo))
                    {
                        _localpaginator!.UpdateFilter(string.Empty);
                        _filterBuffer.Clear();
                        _modeView = _modeView != ModeView.Filter ? ModeView.Filter : ModeView.Select;
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
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.Disabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
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
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.Disabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                        {
                            if (_localpaginator.SelectedItem != null)
                            {
                                if (_localpaginator.SelectedItem.Disabled)
                                {
                                    SetError(Messages.SelectionDisabled);
                                }
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        continue;
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                        {
                            if (_localpaginator.SelectedItem != null)
                            {
                                if (_localpaginator.SelectedItem.Disabled)
                                {
                                    SetError(Messages.SelectionDisabled);
                                }
                            }
                            _indexTooptip = 0;
                            break;
                        }
                        continue;
                    }
                    else if (keyinfo.IsPressCtrlHomeKey())
                    {
                        if (!_localpaginator!.Home())
                        {
                            continue;
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        if (!_localpaginator!.End())
                        {
                            continue;
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressLeftArrowKey() && _modeView != ModeView.Filter)
                    {
                        _moveviewport = MoveViewport.Left;
                        int newcurcol = _currentcol;
                        int minpos = 0;
                        if (!_hideSelectorRow)
                        {
                            minpos = 1;
                        }
                        if (newcurcol > minpos)
                        {
                            newcurcol--;
                        }
                        else
                        {
                            newcurcol = _columns.Count - 1;
                        }
                        _currentcol = newcurcol;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressRightArrowKey() && _modeView != ModeView.Filter)
                    {
                        _moveviewport = MoveViewport.Right;
                        int newcurcol = _currentcol;
                        if (newcurcol < _columns.Count - 1)
                        {
                            newcurcol++;
                        }
                        else
                        {
                            int minpos = 0;
                            if (!_hideSelectorRow)
                            {
                                minpos = 1;
                            }
                            newcurcol = minpos;
                        }
                        _currentcol = newcurcol;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (_filterType != FilterMode.Disabled && _modeView == ModeView.Filter && _filterBuffer.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        string filter = _filterBuffer.ToString();
                        if (_filterCaseinsensitive)
                        {
                            filter = filter.ToUpperInvariant();
                            _lastinput = _lastinput.ToUpperInvariant();
                        }
                        if (_lastinput != filter)
                        {
                            _localpaginator!.UpdateFilter(filter);
                        }
                        if (_localpaginator!.Count == 1 && _autoSelect && !_localpaginator!.SelectedItem!.Disabled)
                        {
                            _modeView = ModeView.Select;
                            ResultCtrl = new ResultPrompt<T>(_localpaginator!.SelectedItem!.Value, false);
                        }
                        if (_localpaginator.SelectedItem != null)
                        {
                            if (_localpaginator.SelectedItem.Disabled)
                            {
                                SetError(Messages.SelectionDisabled);
                            }
                        }
                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_answerBuffer!.IsPrintable(keyinfo.KeyChar) && _answerBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _updatePosAnswerBuffer = false;
                        _indexTooptip = 0;
                        break;
                    }
                }
                _lastinput = _filterBuffer.ToString();
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            _currentrow = _localpaginator!.CurrentIndex;
            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            //ajust _tableviewport
            List<int> columnspos = GetColumnsPosition();

            if (_tableviewport.endwrite - _tableviewport.startWrite > ConsolePlus.BufferWidth - 1)
            {
                switch (_moveviewport)
                {
                    case MoveViewport.None:
                        {
                            int end = _tableviewport.startWrite + (ConsolePlus.BufferWidth - 1);
                            if (end > _totalTableLenWidth)
                            {
                                end = _totalTableLenWidth;
                            }
                            _tableviewport = (_tableviewport.startWrite, end);
                        }
                        break;
                    case MoveViewport.Left when _tableviewport.startWrite != 0:
                        {
                            int start = _tableviewport.startWrite - ConsolePlus.BufferWidth;
                            if (start < 0)
                            {
                                start = 0;
                            }
                            int end = start + (ConsolePlus.BufferWidth - 1);
                            if (end > _totalTableLenWidth)
                            {
                                end = _totalTableLenWidth;
                            }
                            _tableviewport = (start, end);
                        }
                        break;
                    case MoveViewport.Right when _tableviewport.endwrite != _totalTableLenWidth:
                        {
                            int end = _tableviewport.endwrite + ConsolePlus.BufferWidth;
                            if (end > _totalTableLenWidth)
                            {
                                end = _totalTableLenWidth;
                            }
                            _tableviewport = (_tableviewport.endwrite + 1, end);
                        }
                        break;
                }
            }
            else
            {
                int poscol = columnspos[_currentcol];
                if (_tableviewport.startWrite > poscol || _tableviewport.endwrite < poscol || poscol + _columns[_currentcol].Width > _tableviewport.endwrite)
                {
                    int start = poscol;
                    if (start <= 2)
                    {
                        start = 0;
                    }
                    int end = start + (ConsolePlus.BufferWidth - 1);
                    if (end > _totalTableLenWidth)
                    {
                        end = _totalTableLenWidth;
                    }
                    _tableviewport = (start, end);
                }
            }

            if (!IsWidgetControl)
            {
                WritePrompt(screenBuffer);

                WriteAnswer(screenBuffer);

                WriteError(screenBuffer);

                WriteDescription(screenBuffer);
            }

            WriteTable(screenBuffer);

            if (!IsWidgetControl)
            {
                WriteTooltip(screenBuffer);
            }
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            string answer = string.Empty;
            if (!ResultCtrl!.Value.IsAborted && _localpaginator!.SelectedItem is not null)
            {
                answer = _textSelector!.Invoke(_localpaginator.SelectedItem.Value);
            }
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[TableStyles.Prompt]);
            }
            screenBuffer.WriteLine(answer, _optStyles[TableStyles.Answer]);
            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        #region private functions/methods

        private void SaveHistory(T value)
        {
            if (_historyOptions == null)
            {
                return;
            }
            string aux = JsonSerializer.Serialize<T>(value);
            FileHistory.ClearHistory(_historyOptions!.FileNameValue);
            IList<ItemHistory> hist = FileHistory.AddHistory(aux, _historyOptions!.ExpirationTimeValue, null);
            FileHistory.SaveHistory(_historyOptions!.FileNameValue, hist);

        }

        private string GetTooltipModeSelect()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(string.Format(Messages.TableMoveCols, Messages.TooltipPages));
            return tooltip.ToString();
        }

        private void LoadTooltipToggle()
        {
            foreach (ModeView mode in Enum.GetValues<ModeView>())
            {
                List<string> lsttooltips =
                [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {Messages.InputFinishEnter}"
                ];
                if (GeneralOptions.EnabledAbortKeyValue)
                {
                    lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
                }
                if (_filterType != FilterMode.Disabled)
                {
                    lsttooltips.Add(string.Format(Messages.TooltipFilterMode, ConfigPlus.HotKeyFilterMode));
                }
                if (mode == ModeView.Filter)
                {
                    lsttooltips.AddRange(EmacsBuffer.GetEmacsTooltips());
                }
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private string GetTooltipToggle()
        {
            return _modeView switch
            {
                ModeView.Select => _toggerTooptips[ModeView.Select][_indexTooptip - 1],
                ModeView.Filter => _toggerTooptips[ModeView.Filter][_indexTooptip - 1],
                _ => throw new NotImplementedException($"ModeView {_modeView} not implemented.")
            };
        }

        private void WriteTable(BufferScreen screenBuffer)
        {
            BufferScreen sb = new();
            WriteTableHeader(sb);
            WriteTableRows(sb);
            WriteTableFooter(sb);

            int pos = ConsolePlus.PadLeft;
            //lines
            foreach (LineScreen item in sb.DiffBuffer())
            {
                foreach (Segment seg in item.Content)
                {
                    if (seg.Text == Environment.NewLine)
                    {
                        pos = ConsolePlus.PadLeft;
                        screenBuffer.Write(seg.Text, seg.Style);
                        continue;
                    }
                    if (pos >= _tableviewport.endwrite)
                    {
                        continue;
                    }
                    if (pos + seg.Text.GetWidth() < _tableviewport.startWrite)
                    {
                        pos += seg.Text.GetWidth();
                    }
                    else
                    {
                        StringBuilder strb = new();
                        foreach (char chartext in seg.Text)
                        {
                            pos++;
                            if (pos < _tableviewport.startWrite)
                            {
                                continue;
                            }
                            strb.Append(chartext);
                            if (pos >= _tableviewport.endwrite)
                            {
                                break;
                            }
                        }
                        screenBuffer.Write(strb.ToString(), seg.Style);
                    }
                }
                pos = ConsolePlus.PadLeft;
                screenBuffer.WriteLine(string.Empty, Style.Default());
            }
            if (IsWidgetControl)
            {
                return;
            }
            if (_localpaginator!.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[TableStyles.Pagination]);
            }
        }

        private void WriteTableHeader(BufferScreen screenBuffer)
        {
            switch (_layout)
            {
                case TableLayout.HideGrid:
                    BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                    break;
                case TableLayout.SingleGridFull:
                case TableLayout.SingleGridSoft:
                    if (_hideHeaders)
                    {
                        if (_layout == TableLayout.SingleGridFull)
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopCenter)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop)[0]);
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop)[0]);
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopCenter)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop)[0]);
                    }
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    if (_hideHeaders)
                    {
                        if (_layout == TableLayout.DoubleGridFull)
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopCenter)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop)[0]);
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop)[0]);
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer,
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft)[0],
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleTopCenter)[0],
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight)[0],
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop)[0]);
                    }
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                    if (_hideHeaders)
                    {
                        if (_layout == TableLayout.SingleGridFull)
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopCenter, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop, false)[0]);
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop, false)[0]);
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopCenter, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleTopRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleBorderTop, false)[0]);
                    }
                    break;
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    if (_hideHeaders)
                    {
                        if (_layout == TableLayout.DoubleGridFull)
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopCenter, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop, false)[0]);
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop, false)[0]);
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer,
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleTopLeft, false)[0],
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleTopCenter, false)[0],
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleTopRight, false)[0],
                            ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderTop, false)[0]);
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Layout {_layout} Not implemented");
            }

            if (_hideHeaders)
            {
                return;
            }

            int col = -1;
            char sepstart = ' ';
            char sepend = ' ';
            Style stl = _optStyles[TableStyles.Lines];
            switch (_layout)
            {
                case TableLayout.HideGrid:
                    stl = Style.Default();
                    break;
                case TableLayout.SingleGridFull:
                case TableLayout.SingleGridSoft:
                    sepstart = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft)[0];
                    sepend = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight)[0];
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    sepstart = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft)[0];
                    sepend = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight)[0];
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                    sepstart = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft, false)[0];
                    sepend = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight, false)[0];
                    break;
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    sepstart = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft, false)[0];
                    sepend = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight, false)[0];
                    break;
            }
            foreach (ItemColumn<T> item in _columns)
            {
                col++;
                screenBuffer.Write(sepstart, stl);
                string h = AlignmentText(item.Title!.Trim(), item.AlignTitle, item.Width);
                if (col == _currentcol && !IsWidgetControl)
                {
                    screenBuffer.Write(h, _optStyles[TableStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(h, _optStyles[TableStyles.TableHeader]);
                }
            }
            screenBuffer.WriteLine(sepend, stl);
            switch (_layout)
            {
                case TableLayout.HideGrid:
                    BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                    break;
                case TableLayout.SingleGridFull:
                case TableLayout.SingleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleLeft)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleCenter)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleRight)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX)[0]);
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleLeft)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleCenter)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleRight)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX)[0]);
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleCenter, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX)[0]);
                    break;
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomCenter, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX, false)[0]);
                    break;
            }
        }

        private void WriteTableRows(BufferScreen screenBuffer)
        {
            ArraySegment<ItemTableRow<T>> subset;
            subset = _localpaginator!.GetPageData();
            int pos = 0;
            foreach (ItemTableRow<T> item in subset)
            {
                pos++;
                bool isseleted = false;
                bool isdisabled = false;
                if (!IsWidgetControl)
                {
                    if (_localpaginator.TryGetSelected(out ItemTableRow<T>? selectedItem) && EqualityComparer<ItemTableRow<T>>.Default.Equals(item, selectedItem))
                    {
                        isseleted = true;
                    }
                    else
                    {
                        isdisabled = IsDisabled(item);
                    }
                }
                else
                {
                    isdisabled = IsDisabled(item);
                }

                List<string[]> cols = GetTextColumns(item!, out int lines);

                char sepleft = ' ';
                char sepcol = ' ';
                char sepright = ' ';
                Style stl = _optStyles[TableStyles.Lines];
                switch (_layout)
                {
                    case TableLayout.HideGrid:
                        stl = Style.Default();
                        break;
                    case TableLayout.SingleGridFull:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight)[0];
                        sepcol = ConfigPlus.GetSymbol(SymbolType.GridSingleDividerY)[0];
                        break;
                    case TableLayout.SingleGridSoft:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight)[0];
                        break;
                    case TableLayout.DoubleGridFull:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight)[0];
                        sepcol = ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerY)[0];
                        break;
                    case TableLayout.DoubleGridSoft:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight)[0];
                        break;
                    case TableLayout.AsciiSingleGridFull:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft, false)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight, false)[0];
                        sepcol = ConfigPlus.GetSymbol(SymbolType.GridSingleDividerY, false)[0];
                        break;
                    case TableLayout.AsciiSingleGridSoft:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderLeft, false)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridSingleBorderRight, false)[0];
                        break;
                    case TableLayout.AsciiDoubleGridFull:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft, false)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight, false)[0];
                        sepcol = ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerY, false)[0];
                        break;
                    case TableLayout.AsciiDoubleGridSoft:
                        sepleft = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderLeft, false)[0];
                        sepright = ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderRight, false)[0];
                        break;
                }

                for (int i = 0; i < lines; i++)
                {
                    for (int itemcol = 0; itemcol < cols.Count; itemcol++)
                    {
                        if (itemcol == 0)
                        {
                            screenBuffer.Write(sepleft, stl);
                        }
                        else
                        {
                            screenBuffer.Write(sepcol, stl);
                        }
                        string? col = cols[itemcol].Length > i
                            ? AlignmentText(cols[itemcol][i], _columns[itemcol].AlignCol, _columns[itemcol].Width)
                            : new string(' ', _columns[itemcol].Width);
                        if (!_hideSelectorRow && itemcol == 0 && !isseleted)
                        {
                            col = " ";
                        }
                        if (isseleted)
                        {
                            screenBuffer.Write(col, _optStyles[TableStyles.Selected]);
                        }
                        else
                        {
                            Style stld = _optStyles[TableStyles.TableContent];
                            if (isdisabled)
                            {
                                stld = _optStyles[TableStyles.Disabled];
                            }
                            screenBuffer.Write(col, stld);
                        }
                    }
                    screenBuffer.WriteLine(sepright, stl);
                }

                if (_separatorRows && pos < subset.Count)
                {
                    switch (_layout)
                    {
                        case TableLayout.HideGrid:
                            BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                            break;
                        case TableLayout.SingleGridFull:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleCenter)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX)[0]);
                            break;
                        case TableLayout.SingleGridSoft:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX)[0]);
                            break;
                        case TableLayout.DoubleGridFull:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleCenter)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX)[0]);
                            break;
                        case TableLayout.DoubleGridSoft:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleLeft)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleRight)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX)[0]);
                            break;
                        case TableLayout.AsciiSingleGridFull:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleCenter, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX, false)[0]);
                            break;
                        case TableLayout.AsciiSingleGridSoft:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleMiddleRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridSingleDividerX, false)[0]);
                            break;
                        case TableLayout.AsciiDoubleGridFull:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleCenter, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX, false)[0]);
                            break;
                        case TableLayout.AsciiDoubleGridSoft:
                            BuildLineColumn(screenBuffer,
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleLeft, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleMiddleRight, false)[0],
                                ConfigPlus.GetSymbol(SymbolType.GridDoubleDividerX, false)[0]);
                            break;
                    }
                }
            }
        }

        private void WriteTableFooter(BufferScreen screenBuffer)
        {
            switch (_layout)
            {
                case TableLayout.HideGrid:
                    BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                    break;
                case TableLayout.SingleGridFull:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomLeft)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomCenter)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomRight)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderBottom)[0]);
                    break;
                case TableLayout.SingleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomLeft)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderBottom)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomRight)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderBottom)[0]);
                    break;
                case TableLayout.DoubleGridFull:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomLeft)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomCenter)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomRight)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderBottom)[0]);

                    break;
                case TableLayout.DoubleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomLeft)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderBottom)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomRight)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderBottom)[0]);
                    break;
                case TableLayout.AsciiSingleGridFull:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomCenter, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderBottom, false)[0]);
                    break;
                case TableLayout.AsciiSingleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderBottom, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBottomRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridSingleBorderBottom, false)[0]);
                    break;
                case TableLayout.AsciiDoubleGridFull:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomCenter, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderBottom, false)[0]);
                    break;
                case TableLayout.AsciiDoubleGridSoft:
                    BuildLineColumn(screenBuffer,
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomLeft, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderBottom, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBottomRight, false)[0],
                        ConfigPlus.GetSymbol(SymbolType.GridDoubleBorderBottom, false)[0]);
                    break;
                default:
                    throw new InvalidOperationException($"Layout {_layout} Not implemented");
            }
        }

        private List<string[]> GetTextColumns(ItemTableRow<T> value, out int lines)
        {
            List<string> cols = [.. value.TextColumns];
            List<string[]> result = [];
            for (int i = 0; i < cols.Count; i++)
            {
                if (_columns[i].TextCrop)
                {
                    string auxcol = cols[i].NormalizeNewLines().Replace(Environment.NewLine, "");
                    if (auxcol.Length > _columns[i].Width)
                    {
                        result.Add([auxcol[.._columns[i].Width]]);
                    }
                    else
                    {
                        result.Add([auxcol]);
                    }
                }
                else
                {
                    string[] auxcol = cols[i].NormalizeNewLines().Split(Environment.NewLine);
                    List<string> auxlines = [];
                    foreach (string item in auxcol)
                    {
                        auxlines.AddRange(SplitIntoChunks(item, _columns[i].Width, _columns[i].MaxSlidingLines));
                    }
                    result.Add([.. auxlines]);
                }
            }
            lines = result.Max(x => x.Length);
            return result;
        }

        private static string[] SplitIntoChunks(string value, int chunkSize, int? maxsplit)
        {
            if (string.IsNullOrEmpty(value))
            {
                return [""];
            }
            if (chunkSize < 1)
            {
                throw new InvalidOperationException("SplitIntoChunks: The chunk size should be equal or greater than one.");
            }

            int divResult = Math.DivRem(value.Length, chunkSize, out int remainder);

            int numberOfChunks = remainder > 0 ? divResult + 1 : divResult;
            string[] result = new string[numberOfChunks];

            int i = 0;
            while (i < numberOfChunks - 1)
            {
                result[i] = value.Substring(i * chunkSize, chunkSize);
                i++;
            }

            int lastChunkSize = remainder > 0 ? remainder : chunkSize;
            result[i] = value.Substring(i * chunkSize, lastChunkSize);

            if (maxsplit.HasValue && result.Length > maxsplit.Value)
            {
                Array.Resize(ref result, maxsplit.Value);
            }

            return result;
        }

        private string GetTextColumn(T value, Func<T, object> objcol, Func<object, string>? objftm)
        {
            object obj = objcol(value);
            if (obj == null)
            {
                return "";
            }
            string col = objftm != null ? objftm(obj) : _formatTypes.ContainsKey(obj.GetType()) ? _formatTypes[obj.GetType()](obj) : obj.ToString()!;
            return col;
        }

        private static string AlignmentText(string value, TextAlignment alignment, int maxlenght)
        {
            switch (alignment)
            {
                case TextAlignment.Left:
                    value = value.PadRight(maxlenght);
                    break;
                case TextAlignment.Right:
                    value = value.PadLeft(maxlenght);
                    break;
                case TextAlignment.Center:
                    {
                        if (value.Length < maxlenght)
                        {
                            string spc = new(' ', (maxlenght - value.Length) / 2);
                            value = $"{spc}{value}{spc} ".PadRight(maxlenght - 2);
                        }
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Alignment {alignment} Not implemented");
            }
            if (value.Length > maxlenght)
            {
                value = value[..(maxlenght)];
            }
            return value;
        }

        private void BuildLineColumn(BufferScreen screenBuffer, char startln, char sepln, char endln, char contentln)
        {
            Style stl = _optStyles[TableStyles.Lines];
            if (_layout == TableLayout.HideGrid)
            {
                stl = Style.Default();
            }
            screenBuffer.Write($"{startln}{new string(contentln, _columns[0].Width)}", stl);
            foreach (ItemColumn<T>? item in _columns.Skip(1))
            {
                screenBuffer.Write($"{sepln}{new string(contentln, item.Width)}", stl);
            }
            screenBuffer.WriteLine(endln, stl);
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = _indexTooptip > 0 ? GetTooltipToggle() : _tooltipModeSelect;
            screenBuffer.Write(tooltip, _optStyles[TableStyles.Tooltips]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_localpaginator!.SelectedItem.Value, _currentrow, _currentcol) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[TableStyles.Description]);
            }
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[TableStyles.Prompt]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_modeView == ModeView.Select)
            {
                string answer = _textSelector!(_localpaginator!.SelectedItem.Value!);
                if (_updatePosAnswerBuffer)
                {
                    _answerBuffer!.LoadPrintable(answer);
                    _answerBuffer.ToHome();
                }
                string str = _answerBuffer!.IsHideLeftBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
                screenBuffer.Write(str, _optStyles[TableStyles.Answer]);
                screenBuffer.Write(_answerBuffer!.ToBackward(), _optStyles[TableStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.Write(_answerBuffer!.ToForward(), _optStyles[TableStyles.Answer]);
                str = _answerBuffer.IsHideRightBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                screenBuffer.WriteLine(str, _optStyles[TableStyles.Answer]);
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

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[TableStyles.Error]);
                ClearError();
            }
        }

        private void WriteAnswerFilter(BufferScreen screenBuffer)
        {
            Style found = _optStyles[TableStyles.TaggedInfo];
            if (_localpaginator!.TotalCount == 0)
            {
                found = _optStyles[TableStyles.Error];
            }
            screenBuffer.Write(_filterBuffer.ToBackward(), found);
            screenBuffer.SavePromptCursor();
            screenBuffer.Write(_filterBuffer.ToForward(), found);
            screenBuffer.WriteLine($" ({Messages.Filter})", _optStyles[TableStyles.TaggedInfo]);
        }

        private List<int> GetColumnsPosition()
        {
            int skp = 0;
            int start = 0;
            List<int> result = [];
            if (!_hideSelectorRow)
            {
                skp = 1;
                start = 2;
                result.Add(0);
            }
            foreach (ItemColumn<T>? item in _columns.Skip(skp))
            {
                result.Add(start);
                start += item.Width + 1;
            }
            return result;
        }

        private Func<T, string> DefaultTextSeletor()
        {
            return (_) => string.Format(Messages.TableAnswerRow, _currentrow);
        }

        private static string SearchContent(ItemTableRow<T> row, int[] columns)
        {
            StringBuilder content = new();
            foreach (int item in columns)
            {
                content.Append(row.TextColumns[item]);

            }
            return content.ToString();
        }

        private bool IsEnnabled(ItemTableRow<T> item)
        {
            return !IsDisabled(item);
        }

        private bool IsDisabled(ItemTableRow<T> item)
        {
            return _items.Any(x => x.UniqueId == item.UniqueId && x.Disabled);
        }

        private void SetAddFormatType<T1>(Func<object, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Type type = typeof(T1);
            if (!_formatTypes.TryAdd(type, value))
            {
                _formatTypes[type] = value;
            }
        }

        private static Expression<Func<T, object>> GenerateLambdaField(string property_name)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));

            return Expression.Lambda<Func<T, object>>(
                Expression.Convert(Expression.Property(parameter, property_name), typeof(object)), parameter);
        }

        private void SetAutoFill(int? minwidth, int? maxwidth)
        {
            if (_columns.Count > 0)
            {
                throw new InvalidOperationException($"AutoFill cannot be used with AddColumn");
            }
            if (minwidth.HasValue && maxwidth.HasValue && minwidth.Value > maxwidth.Value)
            {
                throw new InvalidOperationException($"AutoFill: The minimum is greater than the maximum");
            }
            _minColWidth = minwidth;
            _maxColWidth = maxwidth;
            _autoFill = true;
            PropertyInfo[] colpropInfo = typeof(T).GetProperties();
            for (int i = 0; i < colpropInfo.Length; i++)
            {
                TypeCode tc = Type.GetTypeCode(colpropInfo[i].PropertyType);
                bool isvalid = true;
                if (isvalid && tc == TypeCode.Object)
                {
                    isvalid = !colpropInfo[i].PropertyType.IsClass;
                    if (isvalid && Nullable.GetUnderlyingType(colpropInfo[i].PropertyType) != null)
                    {
                        TypeCode aux = Type.GetTypeCode(Nullable.GetUnderlyingType(colpropInfo[i].PropertyType));
                        if (aux == TypeCode.Object || aux == TypeCode.DBNull)
                        {
                            isvalid = false;
                        }
                    }
                }
                else if (tc == TypeCode.DBNull)
                {
                    isvalid = false;
                }
                if (isvalid)
                {
                    string tit = $"  {(colpropInfo[i].Name ?? string.Empty).Trim()}";
                    if (tit.Length > byte.MaxValue)
                    {
                        tit = tit[..byte.MaxValue];
                    }
                    int w = tit.Trim().Length;
                    if (tit.Trim().Length < (_minColWidth ?? tit.Trim().Length))
                    {
                        w = _minColWidth ?? tit.Trim().Length;
                    }
                    _columns.Add(new ItemColumn<T>()
                    {
                        AlignCol = TextAlignment.Left,
                        Field = GenerateLambdaField(colpropInfo[i].Name).Compile(),
                        Format = null,
                        Width = w,
                        OriginalWidth = w,
                        TextCrop = false,
                        Title = tit,
                        AlignTitle = TextAlignment.Center,
                        MaxSlidingLines = null,
                        TitleReplacesWidth = true
                    });
                }
            }
        }

        private void SetAddColumn(Expression<Func<T, object>> field, int width, Func<object, string>? format, TextAlignment alignment, string? title, TextAlignment titlealignment, bool titlereplaceswidth, bool textcrop, int? maxslidinglines)
        {
            ArgumentNullException.ThrowIfNull(field);
            if (width < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than zero.");
            }
            if (_autoFill || _autoFill)
            {
                throw new InvalidOperationException("AddColumn cannot be used with AutoFill/AutoFit");
            }

            if (maxslidinglines.HasValue)
            {
                if (maxslidinglines.Value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxslidinglines), "maxslidinglines must be greater than zero.");
                }
            }
            string tit = $"  {(title ?? string.Empty).Trim()}";
            if (tit.Length > byte.MaxValue)
            {
                tit = tit[..byte.MaxValue];
            }
            try
            {
                string fieldname = GetNameUnaryExpression(field);
                if (string.IsNullOrEmpty(tit.Trim()))
                {
                    tit = $"  {fieldname}";
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Expression field must be UnaryExpression", ex);
            }
            int w = (!titlereplaceswidth ? width : (width < tit.Trim().Length) ? tit.Trim().Length : width);

            _columns.Add(new ItemColumn<T>()
            {
                AlignCol = alignment,
                Field = field.Compile(),
                Format = format,
                Width = w,
                OriginalWidth = w,
                TextCrop = textcrop,
                Title = tit,
                AlignTitle = titlealignment,
                MaxSlidingLines = maxslidinglines,
                TitleReplacesWidth = titlereplaceswidth
            });
        }

        private static string GetNameUnaryExpression(Expression<Func<T, object>> exp)
        {
            if (exp.Body is not MemberExpression body)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                if (ubody.Operand as MemberExpression is null)
                {
                    throw new InvalidOperationException("Expression field must be UnaryExpression with a MemberExpression operand.");
                }
                body = (ubody.Operand as MemberExpression)!;
            }
            return body.Member.Name;
        }

        private void SetStyles(TableStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
        }

        private void SetLayout(TableLayout value)
        {
            _layout = value;
        }

        private void SetEqualitem(Func<T, T, bool> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _equalItems = value;
        }

        private void SetAddItem(T value, bool disable)
        {
            _items.Add(new ItemTableRow<T>()
            {
                Value = value,
                Disabled = disable
            });
        }

        #endregion
    }
}
