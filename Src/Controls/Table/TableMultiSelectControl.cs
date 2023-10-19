// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading;
using PPlus.Controls.Objects;


namespace PPlus.Controls.Table
{
    internal class TableMultiSelectControl<T> : BaseControl<IEnumerable<T>>, IControlTableMultiSelect<T> where T : class
    {
        private readonly TableOptions<T> _options;
        private readonly List<ItemTableRow<T>> _selectedItems = new();
        private Optional<IList<T>> _defaultHistoric = Optional<IList<T>>.Create(null);
        private Paginator<ItemTableRow<T>> _localpaginator;
        private readonly EmacsBuffer _filterBuffer = new(CaseOptions.Uppercase, modefilter: true);
        private bool ShowingFilter => _filterBuffer.Length > 0;
        private int _currentrow = -1;
        private int _currentcol = -1;
        private int _totalTableLenWidth;
        private int[] _maxlencontentcols;
        private int? _oldBufferWidth;
        private (int startWrite, int endwrite) _tableviewport;
        private MoveViewport _moveviewport = MoveViewport.None;
        private bool _hasprompt;
        private enum MoveViewport
        { 
            None,
            Left,
            Right
        }


        public TableMultiSelectControl(IConsoleControl console, TableOptions<T> options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            //Validate has columns
            if (_options.Columns.Count == 0)
            {
                throw new PromptPlusException("Not found columns definition");
            }

            //Validate layout SupportsAnsi
            if (!ConsolePlus.SupportsAnsi)
            {
                switch (_options.Layout)
                {
                    case TableLayout.SingleGridFull:
                        _options.Layout = TableLayout.AsciiSingleGridFull;
                        break;
                    case TableLayout.SingleGridSoft:
                        _options.Layout = TableLayout.AsciiSingleGridSoft;
                        break;
                    case TableLayout.DoubleGridFull:
                        _options.Layout = TableLayout.AsciiDoubleGridFull;
                        break;
                    case TableLayout.DoubleGridSoft:
                        _options.Layout = TableLayout.AsciiDoubleGridSoft;
                        break;
                }
            }

            //re-calc column width when IsColumnsNavigation
            if (_options.IsColumnsNavigation)
            {
                for (int i = 0; i < _options.Columns.Count; i++)
                {
                    var w = (ushort)(!_options.Columns[i].TitleReplacesWidth ?
                        _options.Columns[i].Width
                        : (_options.Columns[i].Width < _options.Columns[i].Title.Length) ?
                            _options.Columns[i].Title.Length
                            : _options.Columns[i].Width);

                    _options.Columns[i].Width = w;
                    _options.Columns[i].OriginalWidth = w;
                }
            }
            //validate/re-calc FilterColumns
            if (_options.FilterColumns.Length > 1)
            {
                for (int i = 0; i < _options.FilterColumns.Length; i++)
                {
                    _options.FilterColumns[i]++;
                }
                for (int i = 0; i < _options.FilterColumns.Length; i++)
                {
                    if (_options.FilterColumns[i] > _options.Columns.Count)
                    {
                        throw new PromptPlusException($"FilterColumns {i} Not found in columns definition");
                    }
                }
            }

            _options.Columns.Insert(0, new ItemItemColumn<T> { Field = (_) => $"", Title = $"{_options.Symbol(SymbolType.Selected)}", AlignTitle= Alignment.Center, Width = 3, OriginalWidth = 3 });

            if (_options.IsColumnsNavigation)
            {
                _currentcol = 1;
            }

            if (_options.AutoFill)
            {
                //Calculate max content Length
                _maxlencontentcols = new int[_options.Columns.Count];

                foreach (var item in _options.Items.Select(x => x.Value))
                {
                    for (int i = 0; i < _options.Columns.Count; i++)
                    {
                        var text = GetTextColumn(item, _options.Columns[i].Field, _options.Columns[i].Format);
                        var lentext = text.NormalizeNewLines().Split(Environment.NewLine).Max(x => x.Length);
                        if (_options.MaxColWidth.HasValue && lentext > _options.MaxColWidth.Value)
                        {
                            lentext = _options.MaxColWidth.Value;
                            if (_options.Columns[i].OriginalWidth < lentext)
                            {
                                _options.Columns[i].OriginalWidth = lentext;
                                _options.Columns[i].Width = lentext;
                            }
                        }
                        else
                        {
                            if (_options.Columns[i].OriginalWidth < lentext)
                            {
                                _options.Columns[i].OriginalWidth = lentext;
                                _options.Columns[i].Width = lentext;
                            }
                        }
                        if (_maxlencontentcols[i] < lentext)
                        {
                            _maxlencontentcols[i] = lentext;
                        }
                    }
                }
            }

            _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.OriginalWidth);

            _tableviewport = (0, _totalTableLenWidth);

            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                LoadHistory();
            }
            _options.EqualItems ??= (item1, item2) => item1.Equals(item2);

            foreach (var item in _options.RemoveItems)
            {
                int index;
                do
                {
                    index = _options.Items.FindIndex(x => _options.EqualItems(x.Value, item));
                    if (index >= 0)
                    {
                        _options.Items.RemoveAt(index);
                    }
                }
                while (index >= 0);
            }

            foreach (var item in _options.DisableItems)
            {
                List<ItemTableRow<T>> founds;
                founds = _options.Items.FindAll(x =>_options.EqualItems(x.Value, item));
                if (founds.Any())
                {
                    foreach (var itemfound in founds)
                    {
                        itemfound.Disabled = true;
                    }
                }
            }

            Optional<ItemTableRow<T>> defvaluepage = Optional<ItemTableRow<T>>.s_empty;
            Optional<IList<T>> defvalue = _options.DefaultValues;
            if (_defaultHistoric.HasValue)
            {
                defvalue = _defaultHistoric;
            }
            else
            {
                if (!defvalue.HasValue)
                {
                    defvalue = Optional<IList<T>>.Create(null);
                }
            }

            if (defvalue.HasValue)
            {
                foreach (var item in defvalue.Value)
                {
                    IEnumerable<ItemTableRow<T>> foundmark;
                    foundmark = _options.Items.Where(x => _options.EqualItems(x.Value, item));
                    foreach (var itemmark in foundmark)
                    {
                        if (_selectedItems.Count <= _options.Maximum)
                        {
                            itemmark.IsCheck = true;
                        }
                    }
                }
            }
            var qtdsel = 0;
            for (int i = 0; i < _options.Items.Count; i++)
            {
                if (_options.Items[i].IsCheck)
                {
                    qtdsel++;
                    if (qtdsel > _options.Maximum)
                    {
                        _options.Items[i].IsCheck = false;
                    }
                }
            }

            if (_options.OrderBy != null)
            {
                if (_options.IsOrderDescending)
                {
                    _options.Items = _options.Items.OrderByDescending(x => _options.OrderBy.Invoke(x.Value)).ToList();
                }
                else
                {
                    _options.Items = _options.Items.OrderBy(x => _options.OrderBy.Invoke(x.Value)).ToList();
                }
            }

            _selectedItems.AddRange(_options.Items.Where(x => x.IsCheck));


            var skip = 1;

            if (_options.FilterType == FilterMode.Disabled)
            {
                _localpaginator = new Paginator<ItemTableRow<T>>(
                    _options.FilterType,
                    _options.Items,
                    _options.PageSize,
                    defvaluepage,
                    (item1, item2) => item1.UniqueId == item2.UniqueId,
                    null,
                    IsEnnabled);
                _currentrow = _localpaginator.CurrentIndex;
            }
            else
            {
                _localpaginator = new Paginator<ItemTableRow<T>>(
                    _options.FilterType,
                    _options.Items,
                    _options.PageSize,
                    defvaluepage,
                    (item1, item2) => item1.UniqueId == item2.UniqueId,
                    (item) => SearchContent(item, skip),
                    IsEnnabled);
                _currentrow = _localpaginator.CurrentIndex;
            }

            return string.Empty;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_options.AutoFill && (!_options.MinColWidth.HasValue || !_options.MaxColWidth.HasValue) && (_oldBufferWidth??0) != ConsolePlus.BufferWidth)
            {
                for (ushort i = 0; i < _options.Columns.Count; i++)
                {
                    _options.Columns[i].Width = _options.Columns[i].OriginalWidth;
                }
                _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.OriginalWidth);
                var diff = ConsolePlus.BufferWidth - 1 - _totalTableLenWidth;
                do
                {
                    if (diff > 0)
                    {
                        var haschange = false;
                        for (ushort i = 0; i < _options.Columns.Count; i++)
                        {
                            if (_options.Columns[i].Width+1 <= _maxlencontentcols[i])
                            {
                                haschange = true;
                                _options.Columns[i].Width++;
                                diff--;
                            }
                            if (diff == 0)
                            {
                                break;
                            }
                        }
                        if (!haschange)
                        {
                            diff = 0;
                        }
                    }
                } while (diff > 0);
                _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.Width);
                if (_oldBufferWidth == null)
                {
                    _tableviewport = (0, _totalTableLenWidth);
                }
            }
            else
            {
                //Calculate Table Length again when set autofit
                if (_options.HasAutoFit)
                {
                    for (ushort i = 0; i < _options.Columns.Count; i++)
                    {
                        _options.Columns[i].Width = _options.Columns[i].OriginalWidth;
                    }
                    _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.OriginalWidth);
                    var diff = ConsolePlus.BufferWidth - 1 - _totalTableLenWidth;
                    do
                    {
                        if (diff > 0)
                        {
                            for (ushort i = 0; i < _options.Columns.Count; i++)
                            {
                                ushort index = i;
                                if (i == 0)
                                {
                                    continue;
                                }
                                index--;
                                if (_options.AutoFitColumns.Length == 0 || _options.AutoFitColumns.Contains(index))
                                {
                                    _options.Columns[i].Width++;
                                    diff--;
                                }
                                if (diff == 0)
                                {
                                    break;
                                }
                            }
                        }
                    } while (diff > 0);
                    _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.Width);
                }
            }

            //ajust _tableviewport
            var columnspos = GetColumnsPosition();

            if (!_options.IsColumnsNavigation || ((_oldBufferWidth ?? 0) != ConsolePlus.BufferWidth && (_tableviewport.endwrite - _tableviewport.startWrite) > ConsolePlus.BufferWidth - 1))
            {
                switch (_moveviewport)
                {
                    case MoveViewport.None:
                        {
                            var end = _tableviewport.startWrite + (ConsolePlus.BufferWidth - 1);
                            if (end > _totalTableLenWidth)
                            {
                                end = _totalTableLenWidth;
                            }
                            _tableviewport = (_tableviewport.startWrite, end);
                        }
                        break;
                    case MoveViewport.Left when _tableviewport.startWrite != 0:
                        {
                            var start = _tableviewport.startWrite - ConsolePlus.BufferWidth;
                            if (start < 0)
                            {
                                start = 0;
                            }
                            var end = start + (ConsolePlus.BufferWidth - 1);
                            if (end > _totalTableLenWidth)
                            {
                                end = _totalTableLenWidth;
                            }
                            _tableviewport = (start, end);
                        }
                        break;
                    case MoveViewport.Right when _tableviewport.endwrite != _totalTableLenWidth:
                        {
                            var end = _tableviewport.endwrite + ConsolePlus.BufferWidth;
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
                var poscol = columnspos[_currentcol];
                if ( (_oldBufferWidth ?? 0) != ConsolePlus.BufferWidth || _tableviewport.startWrite > poscol || _tableviewport.endwrite < poscol || poscol + _options.Columns[_currentcol].Width > _tableviewport.endwrite)
                {
                    var start = poscol;
                    if (start <= 2) 
                    {
                        start = 0;
                    }
                    var end = start + (ConsolePlus.BufferWidth - 1);
                    if (end > _totalTableLenWidth)
                    {
                        end = _totalTableLenWidth;
                    }
                    _tableviewport = (start, end);
                }
            }

            _oldBufferWidth = ConsolePlus.BufferWidth;


            screenBuffer.WritePrompt(_options, "");

            _hasprompt = !string.IsNullOrEmpty(_options.OptPrompt) && !_options.OptMinimalRender;
            string answer = null;
            if (_options.MultiSelectedTemplate != null && !_options.OptMinimalRender)
            {
                if (_localpaginator.Count > 0)
                {
                    answer = _options.MultiSelectedTemplate.Invoke(_selectedItems.Select(x => x.Value));
                }
            }
            if (!string.IsNullOrEmpty(answer))
            {
                screenBuffer.AddBuffer(answer, _options.OptStyleSchema.Answer());
                _hasprompt = true;
                screenBuffer.SaveCursor();
            }

            var hasdesc = screenBuffer.WriteLineDescriptionTable(_localpaginator.SelectedItem?.Value, _currentrow, _currentcol, _options);
            if (hasdesc)
            {
                _hasprompt = true;
            }

            if (ShowingFilter)
            {
                if (_hasprompt)
                {
                    screenBuffer.NewLine();
                }
                if (!_options.OptMinimalRender)
                {
                    screenBuffer.WriteTaggedInfo(_options, $"{Messages.Filter}: ");
                }
                screenBuffer.WriteFilterTable(_options, _filterBuffer.ToString(), _filterBuffer);
                screenBuffer.SaveCursor();
                _hasprompt = true;
            }

            WriteTable(screenBuffer, _hasprompt);

            var haspag = false;
            if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
            {
                haspag = true;
                screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage(_options.OptPaginationTemplate));
                screenBuffer.SaveCursor();
            }
            if (_tableviewport.startWrite != 0 || _tableviewport.endwrite != _totalTableLenWidth)
            {
                var spc = " ";
                if (!haspag)
                {
                    screenBuffer.NewLine();
                    spc = "";
                }
                var viewstartcol = -1;
                var viewendcol = -1;
                var tot = _options.Columns.Count-1;
                for (int i = 0; i < columnspos.Count; i++)
                {
                    if (columnspos[i] + _options.Columns[i].Width < _tableviewport.startWrite)
                    {
                        continue;
                    }
                    if (viewstartcol < 0)
                    {
                        viewstartcol = i;
                    }
                    if (columnspos[i] <= _tableviewport.endwrite)
                    {
                        viewendcol = i;
                    }
                    else
                    {
                        break;
                    }
                }
                screenBuffer.AddBuffer($"{spc}Cols: {viewstartcol}~{viewendcol} of {tot}", _options.OptStyleSchema.Pagination(), true);
            }
            if (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1)
            {
                screenBuffer.WriteLineTooltipsTableMultSelect(_options, ShowingFilter, _options.IsColumnsNavigation || _tableviewport.startWrite != 0 || _tableviewport.endwrite != _totalTableLenWidth, _selectedItems.Count);
            }
            else
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer($"{_options.Symbol(SymbolType.Selected)}: {_selectedItems.Count}", _options.OptStyleSchema.TaggedInfo(), true);
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, IEnumerable<T> result, bool aborted)
        {
            if (!aborted)
            {
                SaveHistory(result);
            }
 
            string answer = string.Empty;
            if (!aborted && _options.MultiFinishTemplate != null)
            {
                answer = _options.MultiFinishTemplate.Invoke(result);
            }
            if (aborted && !_options.OptMinimalRender)
            {
                screenBuffer.AddBuffer(Messages.CanceledKey, _options.OptStyleSchema.Answer(), true);
                screenBuffer.NewLine();
            }
            else if (!string.IsNullOrEmpty(answer))
            {
                screenBuffer.AddBuffer(answer, _options.OptStyleSchema.Answer(), false);
                screenBuffer.NewLine();
            }
        }

        public override ResultPrompt<IEnumerable<T>> TryResult(CancellationToken cancellationToken)
        {
            var endinput = false;
            var abort = false;
            var isvalidkey = false;
            _moveviewport = MoveViewport.None;
            do
            {
                var keyInfo = WaitKeypress(cancellationToken);
                if (!keyInfo.HasValue)
                {
                    endinput = true;
                    abort = true;
                }
                else if (CheckAbortKey(keyInfo.Value))
                {
                    abort = true;
                    endinput = true;
                }
                else if (CheckTooltipKeyPress(keyInfo.Value))
                {
                    isvalidkey = true;
                }
                else if (IskeyPageNavegator(keyInfo.Value, _localpaginator))
                {
                    isvalidkey = true;
                }
                else if (_options.SelectAllPress.Equals(keyInfo.Value))
                {
                    _filterBuffer.Clear();
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    var qtd = _options.Items.Count(x => !x.Disabled);
                    if (qtd <= _options.Maximum)
                    {
                        foreach (var item in _options.Items)
                        {
                            if (!item.Disabled)
                            {
                                item.IsCheck = true;
                            }
                            if (!_selectedItems.Contains(item))
                            {
                                if (!item.Disabled)
                                {
                                    _selectedItems.Add(item);
                                }
                            }
                        }
                        isvalidkey = true;
                    }
                    else
                    {
                        SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                    }
                }
                else if (_options.InvertSelectedPress.Equals(keyInfo.Value))
                {
                    _filterBuffer.Clear();
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    var qtd = _options.Items.Count(x =>!x.Disabled);
                    var qtdsel = _selectedItems.Count;
                    var diff = qtd - qtdsel;
                    if (diff <= _options.Maximum)
                    {
                        foreach (var item in _options.Items)
                        {
                            if (!item.Disabled)
                            {
                                item.IsCheck = !item.IsCheck;
                            }
                            if (!item.IsCheck)
                            {
                                _selectedItems.Remove(item);
                            }
                            else
                            {
                                if (!_selectedItems.Select(x => x.Value).Any(x => x.Equals(item.Value)))
                                {
                                    _selectedItems.Add(item);
                                }
                            }
                        }
                        isvalidkey = true;
                    }
                    else
                    {
                        SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                    }
                }
                else if (keyInfo.Value.IsPressSpaceKey())
                {
                    _localpaginator.TryGetSelected(out var currentItem);
                    _filterBuffer.Clear();
                    _localpaginator.UpdateFilter(_filterBuffer.ToString(), Optional<ItemTableRow<T>>.Create(currentItem));
                    if (currentItem != null)
                    {
                        var index = _selectedItems.FindIndex(x => x.Value.Equals(currentItem.Value));
                        if (index >= 0)
                        {
                            _selectedItems.RemoveAt(index);
                            currentItem.IsCheck = false;
                            isvalidkey = true;
                        }
                        else
                        {
                            if (_selectedItems.Count >= _options.Maximum)
                            {
                                SetError(string.Format(Messages.MultiSelectMaxSelection, _options.Maximum));
                            }
                            else
                            {
                                _selectedItems.Add(currentItem);
                                var auxsel = _options.Items.Where(x => _selectedItems.Select(x => x.Value).Contains(x.Value)).ToArray();
                                _selectedItems.Clear();
                                _selectedItems.AddRange(auxsel);
                                currentItem.IsCheck = true;
                                isvalidkey = true;
                            }
                        }
                    }
                }
                else if (keyInfo.Value.IsPressLeftArrowKey(true) && !ShowingFilter)
                {
                    _moveviewport = MoveViewport.Left;
                    if (_options.IsColumnsNavigation)
                    {
                        var minpos = 1;
                        if (_currentcol > minpos)
                        {
                            _currentcol--;
                        }
                        else
                        {
                            _currentcol = _options.Columns.Count - 1;
                        }
                    }
                    isvalidkey = true;
                }
                else if (keyInfo.Value.IsPressRightArrowKey(true) && !ShowingFilter)
                {
                    _moveviewport = MoveViewport.Right;
                    if (_options.IsColumnsNavigation)
                    {
                        if (_currentcol < _options.Columns.Count - 1)
                        {
                            _currentcol++;
                        }
                        else
                        {
                            var minpos = 1;
                            _currentcol = minpos;
                        }
                    }
                    isvalidkey = true;
                }
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    endinput = true;
                    isvalidkey = true;
                }
                else if (_options.FilterType != FilterMode.Disabled && _filterBuffer.TryAcceptedReadlineConsoleKey(keyInfo.Value))
                {
                    _localpaginator.UpdateFilter(_filterBuffer.ToString());
                    isvalidkey = true;
                    if (_localpaginator.Count == 1 && !_localpaginator.IsUnSelected && _options.AutoSelect)
                    {
                        endinput = true;
                    }
                }
                else
                {
                    if (ConsolePlus.Provider == "Memory")
                    {
                        if (!KeyAvailable)
                        {
                            break;
                        }
                    }
                }
                if (isvalidkey)
                {
                    break;
                }
            }
            while (!endinput);
            if (endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }

            _currentrow = _localpaginator.CurrentIndex;

            return new ResultPrompt<IEnumerable<T>>(
                _selectedItems.Select(x => x.Value), 
                abort, !endinput, notrender);
        }

        #region IControlTableSelct


        public IControlTableMultiSelect<T> Range(int minvalue, int? maxvalue = null)
        {
            if (!maxvalue.HasValue)
            {
                maxvalue = _options.Maximum;
            }
            if (minvalue < 0)
            {
                throw new PromptPlusException($"Ranger invalid. minvalue({minvalue})");
            }
            if (maxvalue < 0)
            {
                throw new PromptPlusException($"Ranger invalid. maxvalue({maxvalue})");
            }
            if (minvalue > maxvalue)
            {
                throw new PromptPlusException($"Ranger invalid. minvalue({minvalue}) > maxvalue({maxvalue})");
            }
            _options.Minimum = minvalue;
            _options.Maximum = maxvalue.Value;
            return this;
        }

        public IControlTableMultiSelect<T> AddDefault(IEnumerable<T> value)
        {
            foreach (var item in value)
            {
                if (_options.DefaultValues.Value == null)
                {
                    _options.DefaultValues = Optional<IList<T>>.Create(new List<T>());
                }
                _options.DefaultValues.Value.Add(Optional<T>.Create(item));
            }
            return this;
        }

        public IControlTableMultiSelect<T> AddDefault(params T[] value)
        {
            foreach (var item in value)
            {
                if (_options.DefaultValues.Value == null)
                {
                    _options.DefaultValues = Optional<IList<T>>.Create(new List<T>());
                }
                _options.DefaultValues.Value.Add(Optional<T>.Create(item));
            }
            return this;
        }


        public IControlTableMultiSelect<T> AutoFill(params ushort?[] minmaxwidth)
        {
            if (_options.Columns.Count > 0 || _options.HasAutoFit)
            {
                throw new PromptPlusException($"AutoFill cannot be used with AddColumn and/or AutoFit");
            }
            if (minmaxwidth.Length > 2)
            {
                throw new PromptPlusException($"the 'minmawidth' parameter can have a maximum of 2 positions(min and max)");
            }
            ushort? min = null;
            ushort? max = null;
            if (minmaxwidth.Length == 2)
            {
                min = minmaxwidth[0];
                max = minmaxwidth[1];
            }
            if (min.HasValue && max.HasValue && min.Value > max.Value)
            {
                throw new PromptPlusException($"AutoFill: The minimum is greater than the maximum");
            }
            if (minmaxwidth.Length == 1)
            {
                min = minmaxwidth[0];
            }

            _options.MinColWidth = min;
            _options.MaxColWidth = max;
            _options.AutoFill = true;
            var colpropInfo = typeof(T).GetProperties();
            for (int i = 0; i < colpropInfo.Length; i++)
            {
                var tc = Type.GetTypeCode(colpropInfo[i].PropertyType);
                var isvalid = true;
                if (isvalid && tc == TypeCode.Object)
                {
                    isvalid = !colpropInfo[i].PropertyType.IsClass;
                    if (isvalid && Nullable.GetUnderlyingType(colpropInfo[i].PropertyType) != null)
                    {
                        var aux = Type.GetTypeCode(Nullable.GetUnderlyingType(colpropInfo[i].PropertyType));
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
                    var tit = $"  {(colpropInfo[i].Name ?? string.Empty).Trim()}";
                    if (tit.Length > byte.MaxValue)
                    {
                        tit = tit[..byte.MaxValue];
                    }
                    var w = tit.Trim().Length;
                    if (tit.Trim().Length < (_options.MinColWidth ?? tit.Trim().Length))
                    {
                        w = _options.MinColWidth?? tit.Trim().Length;
                    }
                    _options.Columns.Add(new ItemItemColumn<T>()
                    {
                        AlignCol = Alignment.Left,
                        Field = GenerateLambdaField(colpropInfo[i].Name).Compile(),
                        Format = null,
                        Width = w,
                        OriginalWidth = w,
                        TextCrop = false,
                        Title = tit,
                        AlignTitle = Alignment.Center,
                        MaxSlidingLines = null,
                        TitleReplacesWidth = true
                    });
                }
            }

            return this;
        }

        public IControlTableMultiSelect<T> AddColumn(Expression<Func<T, object>> field, ushort width, Func<object, string> format = null, Alignment alignment = Alignment.Left, string? title = null, Alignment titlealignment = Alignment.Center, bool titlereplaceswidth = true, bool textcrop = false, ushort? maxslidinglines = null)
        {
            if (_options.AutoFill)
            {
                throw new PromptPlusException($"AddColumn cannot be used with AutoFill");
            }

            if (maxslidinglines.HasValue)
            {
                if (maxslidinglines.Value <= 0)
                {
                    maxslidinglines = null;
                }
            }
            var tit = $"  {(title ?? string.Empty).Trim()}";
            if (tit.Length > byte.MaxValue)
            { 
                tit = tit[..byte.MaxValue];
            }
            try
            {
                var fieldname = GetNameUnaryExpression(field);
                if (string.IsNullOrEmpty(tit.Trim()))
                {
                    tit = $"  {fieldname}";
                }
            }
            catch (Exception ex)
            {
                throw new PromptPlusException($"Expression field must be UnaryExpression", ex);
            }
            var w = (!titlereplaceswidth ? width : (width < tit.Trim().Length) ? tit.Trim().Length : width);

            _options.Columns.Add(new ItemItemColumn<T>() 
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
            return this;
        }

        public IControlTableMultiSelect<T> AddFormatType<T1>(Func<object,string> funcfomatType)
        {
            var type = typeof(T1);
            if (_options.FormatTypes.ContainsKey(type))
            {
                _options.FormatTypes[type] = funcfomatType;
            }
            else
            {
                _options.FormatTypes.Add(type, funcfomatType);
            }
            return this;
        }

        public IControlTableMultiSelect<T> AddItem(T value, bool disable = false,bool selected = false)
        {
            _options.Items.Add(new ItemTableRow<T>() { Value = value, Disabled = disable, IsCheck = selected});
            return this;
        }

        public IControlTableMultiSelect<T> AddItems(IEnumerable<T> values, bool disable = false, bool selected = false)
        {
            foreach (var item in values)
            {
                AddItem(item, disable,selected);
            }
            return this;
        }

        public IControlTableMultiSelect<T> AddItemsTo(AdderScope scope, params T[] values)
        {
            foreach (var item in values)
            {
                switch (scope)
                {
                    case AdderScope.Disable:
                        {
                            _options.DisableItems.Add(item);
                        }
                        break;
                    case AdderScope.Remove:
                        {
                            _options.RemoveItems.Add(item);
                        }
                        break;
                    default:
                        throw new PromptPlusException($"AdderScope : {scope} Not Implemented");
                }
            }
            return this;
        }

        public IControlTableMultiSelect<T> AutoFit(params ushort[] indexColumn)
        {
            if (_options.AutoFill)
            {
                throw new PromptPlusException($"AutoFit cannot be used with AutoFill");
            }
            _options.HasAutoFit = true;
            _options.AutoFitColumns = indexColumn;
            return this;
        }

        public IControlTableMultiSelect<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlTableMultiSelect<T> HideHeaders(bool value = true)
        {
            _options.HideHeaders = value;
            return this;
        }

        public IControlTableMultiSelect<T> ColumnsNavigation(bool value = true)
        {
            _options.IsColumnsNavigation = value;
            return this;
        }

        public IControlTableMultiSelect<T> Templates(Func<IEnumerable<T>, string> selectedTemplate = null, Func<IEnumerable<T>, string> finishTemplate = null)
        {
            _options.MultiSelectedTemplate = selectedTemplate;
            _options.MultiFinishTemplate = finishTemplate;
            return this;
        }

        public IControlTableMultiSelect<T> ChangeDescription(Func<T, int, int, string> func = null)
        {
            _options.ChangeDescription = func;
            return this;
        }
        public IControlTableMultiSelect<T> EqualItems(Func<T, T, bool> comparer)
        {
            _options.EqualItems = comparer;
            return this;
        }

        public IControlTableMultiSelect<T> FilterByColumns(FilterMode filter, params ushort[] indexColumn)
        {
            _options.FilterType = filter;
            if (filter == FilterMode.Disabled)
            {
                _options.FilterColumns = null;
            }
            else
            {
                _options.FilterColumns = indexColumn;
            }
            return this;
        }

        public IControlTableMultiSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlTableMultiSelect<T>, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }

        public IControlTableMultiSelect<T> Layout(TableLayout value)
        {
            _options.Layout = value;
            return this;
        }

        public IControlTableMultiSelect<T> OrderBy(Expression<Func<T, object>> value)
        {
            _options.IsOrderDescending = false;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlTableMultiSelect<T> OrderByDescending(Expression<Func<T, object>> value)
        {
            _options.IsOrderDescending = true;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlTableMultiSelect<T> OverwriteDefaultFrom(string value, TimeSpan? timeout = null)
        {
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                if (timeout.Value.TotalMilliseconds == 0)
                {
                    throw new PromptPlusException("timeout must be greater than 0");
                }
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlTableMultiSelect<T> PageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("PageSize must be greater than or equal to 1");
            }
            _options.PageSize = value;
            return this;
        }

        public IControlTableMultiSelect<T> SeparatorRows(bool value = true)
        {
            _options.SeparatorRows = value;
            return this;
        }

        public IControlTableMultiSelect<T> Styles(TableStyle styletype, Style value)
        {
            switch (styletype)
            {
                case TableStyle.Grid:
                    _options.GridStyle = value;
                    break;
                case TableStyle.Title:
                    _options.TitleStyle = value;
                    break;
                case TableStyle.Header:
                    _options.HeaderStyle = value;
                    break;
                case TableStyle.SelectedHeader:
                    _options.SelectedHeaderStyle = value;
                    break;
                case TableStyle.Content:
                    _options.ContentStyle = value;
                    break;
                case TableStyle.DisabledContent:
                    _options.DisabledContentStyle = value;
                    break;
                case TableStyle.SelectedContent:
                    _options.SelectedContentStyle = value;
                    break;
                default:
                    throw new PromptPlusException($"TableStyle: {styletype} Not Implemented");
            }
            return this;
        }

        public IControlTableMultiSelect<T> Title(string value, Alignment alignment = Alignment.Center, TableTitleMode titleMode = TableTitleMode.InLine)
        {
            _options.Title = value;
            _options.TitleAlignment = alignment;
            _options.TitleMode = titleMode;
            return this;
        }

        #endregion

        private List<int> GetColumnsPosition()
        {
            var skp = 1;
            var start = 2;
            var result = new List<int>
            {
                0
            };
            foreach (var item in _options.Columns.Skip(skp))
            {
                result.Add(start);
                start += item.Width + 1;
            }
            return result;
        }

        private static Expression<Func<T, object>> GenerateLambdaField(string property_name)
        {
            var parameter = Expression.Parameter(typeof(T));

            return Expression.Lambda<Func<T, object>>(
                Expression.Convert(Expression.Property(parameter, property_name), typeof(object)), parameter);
        }

        private static string GetNameUnaryExpression(Expression<Func<T, object>> exp)
        {
            if (exp.Body is not MemberExpression body)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }
            return body.Member.Name;
        }

        private static string AlignmentText(string value, Alignment alignment, int maxlenght)
        {
            switch (alignment)
            {
                case Alignment.Left:
                    value = value.PadRight(maxlenght);
                    break;
                case Alignment.Right:
                    value = value.PadLeft(maxlenght);
                    break;
                case Alignment.Center:
                    {
                        if (value.Length < maxlenght)
                        {
                            var spc = new string(' ', (maxlenght - value.Length) / 2);
                            value = $"{spc}{value}{spc} ".PadRight(maxlenght - 2);
                        }
                    }
                    break;
                default:
                    throw new PromptPlusException($"Alignment {alignment} Not implemented");
            }
            if (value.Length > maxlenght)
            {
                value = value[..(maxlenght)];
            }
            return value;
        }

        private void BuildLineColumn(ScreenBuffer screenBuffer,char startln,char sepln, char endln, char contentln)
        {
            var stl = _options.GridStyle;
            if ( _options.Layout == TableLayout.HideGrid)
            {
                stl = Style.Default;
            }
            screenBuffer.AddBuffer($"{startln}{new string(contentln, _options.Columns[0].Width)}", stl);
            foreach (var item in _options.Columns.Skip(1))
            {
                screenBuffer.AddBuffer($"{sepln}{new string(contentln, item.Width)}", stl);
            }
            screenBuffer.AddBuffer(endln, stl);
        }

        private object GetValueColumn(int column, ItemTableRow<T> item)
        {
            if (column < 0 || column > _options.Columns.Count)
            {
                return null;
            }
            return _options.Columns[column].Field.Invoke(item.Value);
        }

        private List<string[]> GetTextColumns(T value, out int lines)
        {
            var cols = new List<string>();
            foreach (var defcol in _options.Columns)
            {
                cols.Add(GetTextColumn(value, defcol.Field, defcol.Format));
            }
            var result = new List<string[]>();
            for (int i = 0; i < cols.Count; i++)
            {
                if (_options.Columns[i].TextCrop)
                {
                    var auxcol = cols[i].NormalizeNewLines().Replace(Environment.NewLine,"");
                    if (auxcol.Length > _options.Columns[i].Width)
                    {
                        result.Add(new[] { auxcol[.._options.Columns[i].Width] });
                    }
                    else
                    {
                        result.Add(new[] { auxcol });
                    }
                }
                else
                {
                    var auxcol = cols[i].NormalizeNewLines().Split(Environment.NewLine);
                    if (auxcol.Length == 1)
                    {
                        result.Add(SplitIntoChunks(auxcol[0], _options.Columns[i].Width, _options.Columns[i].MaxSlidingLines));
                    }
                    else
                    {
                        var auxlines = new List<string>();
                        foreach (var item in auxcol)
                        {
                            auxlines.AddRange(SplitIntoChunks(item, _options.Columns[i].Width, _options.Columns[i].MaxSlidingLines));
                        }
                        result.Add(auxlines.ToArray());
                    }
                }
            }
            lines = result.Max(x => x.Length);
            return result;
        }

        private string SearchContent(ItemTableRow<T> row,int skip)
        {
            var content = new StringBuilder();
            if (_options.FilterColumns.Length == 0)
            {
                for (var i = 0; i < _options.Columns.Count; i++)
                {
                    if (i < skip)
                    {
                        continue;
                    }
                    content.Append(GetTextColumn(row.Value, _options.Columns[i].Field, _options.Columns[i].Format));
                }
            }
            else
            {
                for (ushort i = 0; i < _options.Columns.Count; i++)
                {
                    if (i < skip || !_options.FilterColumns.Contains(i))
                    {
                        continue;
                    }
                    content.Append(GetTextColumn(row.Value, _options.Columns[i].Field, _options.Columns[i].Format));
                }
            }
            return content.ToString();
        }

        private static string[] SplitIntoChunks(string value, int chunkSize, int? maxsplit)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[] { "" };
            }
            if (chunkSize < 1) throw new PromptPlusException("SplitIntoChunks: The chunk size should be equal or greater than one.");

            int divResult = Math.DivRem(value.Length, chunkSize, out int remainder);

            int numberOfChunks = remainder > 0 ? divResult + 1 : divResult;
            var result = new string[numberOfChunks];

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

        private string GetTextColumn(T value, Func<T, object> objcol, Func<object, string> objftm)
        {
            var obj = objcol(value);
            if (obj == null) 
            {
                return "";
            }
            string col;
            if (objftm != null)
            {
                col = objftm(obj);
            }
            else if (_options.FormatTypes.ContainsKey(obj.GetType()))
            {
                col = _options.FormatTypes[obj.GetType()](obj);
            }
            else
            {
                col = obj.ToString();
            }
            return col;
        }

        private void WriteTable(ScreenBuffer screenBuffer,bool startnewline)
        {
            var sb = new ScreenBuffer();
            WriteTableTitle(sb,startnewline);
            WriteTableHeader(sb);
            WriteTableRows(sb);
            WriteTableFooter(sb);
            var pos = (int)ConsolePlus.PadLeft;
            foreach (var item in sb.Buffer)
            {
                if (item.Text == Environment.NewLine)
                {
                    pos = ConsolePlus.PadLeft;
                    screenBuffer.AddBuffer(item.Text, item.Style, true,true);
                    continue;
                }
                if (pos >= _tableviewport.endwrite)
                {
                    continue;
                }
                if (pos + item.Width < _tableviewport.startWrite)
                {
                    pos += item.Width;
                }
                else
                {
                    var strb = new StringBuilder();
                    foreach (var chartext in item.Text)
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
                    screenBuffer.AddBuffer(strb.ToString(), item.Style, true, false);
                }
            }
        }

        private void WriteTableTitle(ScreenBuffer screenBuffer, bool startnewline)
        {
            var tit = _options.Title;
            if (_options.TitleMode == TableTitleMode.InLine)
            {
                if (string.IsNullOrEmpty(tit))
                {
                    if (startnewline)
                    {
                        screenBuffer.NewLine();
                    }
                    switch (_options.Layout)
                    {
                        case TableLayout.HideGrid:
                            BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                            break;
                        case TableLayout.SingleGridFull:
                        case TableLayout.SingleGridSoft:
                            if (_options.HideHeaders)
                            {
                                if (_options.Layout == TableLayout.SingleGridFull)
                                {
                                    BuildLineColumn(screenBuffer, '┌', '┬', '┐', '─');
                                }
                                else
                                {
                                    BuildLineColumn(screenBuffer, '┌', '─', '┐', '─');
                                }
                            }
                            else
                            {
                                BuildLineColumn(screenBuffer, '┌', '┬', '┐', '─');
                            }
                            break;
                        case TableLayout.DoubleGridFull:
                        case TableLayout.DoubleGridSoft:
                            if (_options.HideHeaders)
                            {
                                if (_options.Layout == TableLayout.DoubleGridFull)
                                {
                                    BuildLineColumn(screenBuffer, '╔', '╦', '╗', '═');
                                }
                                else
                                {
                                    BuildLineColumn(screenBuffer, '╔', '═', '╗', '═');
                                }
                            }
                            else
                            {
                                BuildLineColumn(screenBuffer, '╔', '╦', '╗', '═');
                            }
                            break;
                        case TableLayout.AsciiSingleGridFull:
                        case TableLayout.AsciiSingleGridSoft:
                            if (_options.HideHeaders)
                            {
                                if (_options.Layout == TableLayout.AsciiSingleGridFull)
                                {
                                    BuildLineColumn(screenBuffer, '+', '+', '+', '-');
                                }
                                else
                                {
                                    BuildLineColumn(screenBuffer, '+', '-', '+', '-');
                                }
                            }
                            else
                            {
                                BuildLineColumn(screenBuffer, '+', '+', '+', '-');
                            }
                            break;
                        case TableLayout.AsciiDoubleGridFull:
                        case TableLayout.AsciiDoubleGridSoft:
                            if (_options.HideHeaders)
                            {
                                if (_options.Layout == TableLayout.AsciiDoubleGridFull)
                                {
                                    BuildLineColumn(screenBuffer, '+', '+', '+', '=');
                                }
                                else
                                {
                                    BuildLineColumn(screenBuffer, '+', '=', '+', '=');
                                }
                            }
                            else
                            {
                                BuildLineColumn(screenBuffer, '+', '+', '+', '=');
                            }
                            break;
                        default:
                            throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
                    }
                    return;
                }
                screenBuffer.NewLine();
                tit = AlignmentText(tit, _options.TitleAlignment, _totalTableLenWidth);
                screenBuffer.AddBuffer(tit, _options.TitleStyle);
                tit = string.Empty;
                startnewline = true;
            }
            WriteTopTable(screenBuffer, startnewline);
            if (!string.IsNullOrEmpty(tit))
            {
                WriteTitleInRow(tit, screenBuffer);
            }
        }

        private void WriteTitleInRow(string tit, ScreenBuffer screenBuffer)
        {
            screenBuffer.NewLine();
            tit = AlignmentText(tit, _options.TitleAlignment, _totalTableLenWidth - 2);
            var sep = ' ';
            switch (_options.Layout)
            {
                case TableLayout.HideGrid:
                    break;
                case TableLayout.SingleGridSoft:
                case TableLayout.SingleGridFull:
                    sep = '│';
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    sep = '║';
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    sep = '|';
                    break;
                default:
                    throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
            }
            screenBuffer.AddBuffer(sep, _options.GridStyle);
            screenBuffer.AddBuffer(tit, _options.TitleStyle);
            screenBuffer.AddBuffer(sep, _options.GridStyle);

            screenBuffer.NewLine();
            switch (_options.Layout)
            {
                case TableLayout.HideGrid:
                    BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                    break;
                case TableLayout.SingleGridFull:
                case TableLayout.SingleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.SingleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '├', '┬', '┤', '─');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '├', '─', '┤', '─');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '├', '┬', '┤', '─');
                    }
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.DoubleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '╠', '╦', '╣', '═');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '╠', '═', '╣', '═');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '╠', '╦', '╣', '═');
                    }
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.AsciiSingleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '|', '+', '|', '-');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '|', '-', '|', '-');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '|', '+', '|', '-');
                    }
                    break;
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.AsciiDoubleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '|', '+', '|', '=');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '|', '=', '|', '=');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '|', '+', '|', '=');
                    }
                    break;
            }
        }

        private void WriteTopTable(ScreenBuffer screenBuffer, bool startnewline)
        {
            if (startnewline)
            {
                screenBuffer.NewLine();
            }
            if (_options.TitleMode == TableTitleMode.InRow && !string.IsNullOrEmpty(_options.Title))
            {
                switch (_options.Layout)
                {
                    case TableLayout.HideGrid:
                        BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                        break;
                    case TableLayout.SingleGridFull:
                    case TableLayout.SingleGridSoft:
                        BuildLineColumn(screenBuffer, '┌', '─', '┐', '─');
                        break;
                    case TableLayout.DoubleGridFull:
                    case TableLayout.DoubleGridSoft:
                        BuildLineColumn(screenBuffer, '╔', '═', '╗', '═');
                        break;
                    case TableLayout.AsciiSingleGridFull:
                    case TableLayout.AsciiSingleGridSoft:
                        BuildLineColumn(screenBuffer, '+', '-', '+', '-');
                        break;
                    case TableLayout.AsciiDoubleGridFull:
                    case TableLayout.AsciiDoubleGridSoft:
                        BuildLineColumn(screenBuffer, '+', '=', '+', '=');
                        break;
                    default:
                        throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
                }
                return;
            }
            switch (_options.Layout)
            {
                case TableLayout.HideGrid:
                    BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                    break;
                case TableLayout.SingleGridFull:
                case TableLayout.SingleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.SingleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '┌', '┬', '┐', '─');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '┌', '─', '┐', '─');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '┌', '┬', '┐', '─');
                    }
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.DoubleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '╔', '╦', '╗', '═');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '╔', '═', '╗', '═');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '╔', '╦', '╗', '═');
                    }
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.AsciiSingleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '+', '+', '+', '-');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '+', '-', '+', '-');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '+', '+', '+', '-');
                    }
                    break;
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    if (_options.HideHeaders)
                    {
                        if (_options.Layout == TableLayout.AsciiDoubleGridFull)
                        {
                            BuildLineColumn(screenBuffer, '+', '+', '+', '=');
                        }
                        else
                        {
                            BuildLineColumn(screenBuffer, '+', '=', '+', '=');
                        }
                    }
                    else
                    {
                        BuildLineColumn(screenBuffer, '+', '+', '+', '=');
                    }
                    break;
                default:
                    throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
            }
        }

        private void WriteTableHeader(ScreenBuffer screenBuffer)
        {
            if (_options.HideHeaders)
            {
                return;
            }
            screenBuffer.NewLine();
            var col = -1;
            var sepstart = " ";
            var sepend = " ";
            var stl = _options.GridStyle;
            switch (_options.Layout)
            {
                case TableLayout.HideGrid:
                    stl = Style.Default;
                    break;
                case TableLayout.SingleGridFull:
                case TableLayout.SingleGridSoft:
                    sepstart = "│";
                    sepend = "│";
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    sepstart = "║";
                    sepend = "║";
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    sepstart = "|";
                    sepend = "|";
                    break;
            }
            foreach (var item in _options.Columns)
            {
                col++;
                screenBuffer.AddBuffer(sepstart, stl);
                if (_options.IsColumnsNavigation && col == _currentcol)
                {
                    var h = AlignmentText($"{_options.Symbol(SymbolType.Selector)} {item.Title.Trim()}", item.AlignTitle, item.Width);
                    screenBuffer.AddBuffer(h, _options.SelectedHeaderStyle);
                }
                else
                {
                    var h = AlignmentText(item.Title.Trim(), item.AlignTitle, item.Width);
                    screenBuffer.AddBuffer(h, _options.HeaderStyle);
                }
            }
            screenBuffer.AddBuffer(sepend, stl);
            screenBuffer.NewLine();
            switch (_options.Layout)
            {
                case TableLayout.HideGrid:
                    BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                    break;
                case TableLayout.SingleGridFull:
                    BuildLineColumn(screenBuffer, '├', '┼', '┤', '─');
                    break;
                case TableLayout.SingleGridSoft:
                    BuildLineColumn(screenBuffer, '├', '┴', '┤', '─');
                    break;
                case TableLayout.DoubleGridFull:
                    BuildLineColumn(screenBuffer, '╠', '╬', '╣', '═');
                    break;
                case TableLayout.DoubleGridSoft:
                    BuildLineColumn(screenBuffer, '╠', '╩', '╣', '═');
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                    BuildLineColumn(screenBuffer, '|', '+', '|', '-');
                    break;
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    BuildLineColumn(screenBuffer, '|', '+', '|', '=');
                    break;
            }
        }

        private void WriteTableRows(ScreenBuffer screenBuffer)
        {

            ArraySegment<ItemTableRow<T>> subset;
            subset = _localpaginator.GetPageData();
            var pos = 0;
            foreach (var item in subset)
            {
                pos++;
                screenBuffer.NewLine();
                var isseleted = false;
                var isdisabled = false;
                var ischeck = item.IsCheck;
                if (_localpaginator.TryGetSelected(out var selectedItem) && EqualityComparer<ItemTableRow<T>>.Default.Equals(item, selectedItem))
                {
                    isseleted = true;
                }
                else
                {
                    isdisabled = IsDisabled(item);
                }

                var cols = GetTextColumns(item.Value, out var lines);

                var sep = " ";
                var sepcol = " ";
                var sepend = " ";
                var stl = _options.GridStyle;
                switch (_options.Layout)
                {
                    case TableLayout.HideGrid:
                        stl = Style.Default;
                        break;
                    case TableLayout.SingleGridFull:
                        sep = "│";
                        sepend = "│";
                        sepcol = "│";
                        break;
                    case TableLayout.SingleGridSoft:
                        sep = "│";
                        sepend = "│";
                        sepcol = " ";
                        break;
                    case TableLayout.DoubleGridFull:
                        sep = "║";
                        sepend = "║";
                        sepcol = "║";
                        break;
                    case TableLayout.DoubleGridSoft:
                        sep = "║";
                        sepend = "║";
                        sepcol = " ";
                        break;
                    case TableLayout.AsciiSingleGridFull:
                    case TableLayout.AsciiSingleGridSoft:
                    case TableLayout.AsciiDoubleGridFull:
                        sep = "|";
                        sepend = "|";
                        sepcol = "|";
                        break;
                    case TableLayout.AsciiDoubleGridSoft:
                        sep = "|";
                        sepend = "|";
                        sepcol = " ";
                        break;
                }

                for (int i = 0; i < lines; i++)
                {
                    for (int itemcol = 0; itemcol < cols.Count; itemcol++)
                    {
                        if (itemcol == 0)
                        {
                            screenBuffer.AddBuffer(sep, stl);
                        }
                        else
                        { 
                            screenBuffer.AddBuffer(sepcol, stl);
                        }
                        string col = null;
                        if (cols[itemcol].Length > i)
                        {
                            col = AlignmentText(cols[itemcol][i], _options.Columns[itemcol].AlignCol, _options.Columns[itemcol].Width);
                        }
                        else
                        {
                            col = new string(' ', _options.Columns[itemcol].Width);
                        }
                        if (itemcol == 0 && !isseleted)
                        {
                            if (i == 0)
                            {
                                if (ischeck)
                                {
                                    col = $" {_options.Symbol(SymbolType.Selected)} ";
                                }
                                else
                                {
                                    col = $" {_options.Symbol(SymbolType.NotSelect)} ";
                                }
                            }
                            else
                            {
                                col = "   ";
                            }
                        }
                        else if (itemcol == 0 && isseleted)
                        {
                            if (i == 0)
                            {
                                if (ischeck)
                                {
                                    col = $"{_options.Symbol(SymbolType.Selector)}{_options.Symbol(SymbolType.Selected)} ";
                                }
                                else
                                {
                                    col = $"{_options.Symbol(SymbolType.Selector)}{_options.Symbol(SymbolType.NotSelect)} ";
                                }
                            }
                            else
                            {
                                col = "   ";
                            }
                        }
                        if (isseleted)
                        {
                            if (_options.IsColumnsNavigation)
                            {
                                if (itemcol == _currentcol || itemcol == 0)
                                {
                                    screenBuffer.AddBuffer(col, _options.SelectedContentStyle, true);
                                }
                                else
                                {
                                    screenBuffer.AddBuffer(col, _options.ContentStyle, true);
                                }
                            }
                            else
                            {
                                screenBuffer.AddBuffer(col, _options.SelectedContentStyle, true);
                            }
                        }
                        else
                        {
                            var stld = _options.ContentStyle;
                            if (isdisabled)
                            {
                                stld = _options.DisabledContentStyle;
                            }
                            screenBuffer.AddBuffer(col, stld, true);
                        }
                    }
                    screenBuffer.AddBuffer(sepend, stl);
                    if (lines > 1 && i != lines - 1)
                    {
                        screenBuffer.NewLine();
                    }
                }

                if (_options.SeparatorRows && pos < subset.Count)
                {
                    screenBuffer.NewLine();
                    switch (_options.Layout)
                    {
                        case TableLayout.HideGrid:
                            BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                            break;
                        case TableLayout.SingleGridFull:
                            BuildLineColumn(screenBuffer, '├', '┼', '┤', '─');
                            break;
                        case TableLayout.SingleGridSoft:
                            BuildLineColumn(screenBuffer, '├', '─', '┤', '─');
                            break;
                        case TableLayout.DoubleGridFull:
                            BuildLineColumn(screenBuffer, '╠', '╬', '╣', '═');
                            break;
                        case TableLayout.DoubleGridSoft:
                            BuildLineColumn(screenBuffer, '╠', '═', '╣', '═');
                            break;
                        case TableLayout.AsciiSingleGridFull:
                            BuildLineColumn(screenBuffer, '|', '+', '|', '-');
                            break;
                        case TableLayout.AsciiSingleGridSoft:
                            BuildLineColumn(screenBuffer, '|', '-', '|', '-');
                            break;
                        case TableLayout.AsciiDoubleGridFull:
                            BuildLineColumn(screenBuffer, '|', '+', '|', '=');
                            break;
                        case TableLayout.AsciiDoubleGridSoft:
                            BuildLineColumn(screenBuffer, '|', '=', '|', '=');
                            break;
                    }
                }
            }
        }

        private void WriteTableFooter(ScreenBuffer screenBuffer)
        {
            screenBuffer.NewLine();
            switch (_options.Layout)
            {
                case TableLayout.HideGrid:
                    BuildLineColumn(screenBuffer, ' ', ' ', ' ', ' ');
                    break;
                case TableLayout.SingleGridFull:
                    BuildLineColumn(screenBuffer, '└', '┴', '┘', '─');
                    break;
                case TableLayout.SingleGridSoft:
                    BuildLineColumn(screenBuffer, '└', '─', '┘', '─');
                    break;
                case TableLayout.DoubleGridFull:
                    BuildLineColumn(screenBuffer, '╚', '╩', '╝', '═');
                    break;
                case TableLayout.DoubleGridSoft:
                    BuildLineColumn(screenBuffer, '╚', '═', '╝', '═');
                    break;
                case TableLayout.AsciiSingleGridFull:
                    BuildLineColumn(screenBuffer, '+', '+', '+', '-');
                    break;
                case TableLayout.AsciiSingleGridSoft:
                    BuildLineColumn(screenBuffer, '+', '-', '+', '-');
                    break;
                case TableLayout.AsciiDoubleGridFull:
                    BuildLineColumn(screenBuffer, '+', '+', '+', '=');
                    break;
                case TableLayout.AsciiDoubleGridSoft:
                    BuildLineColumn(screenBuffer, '+', '=', '+', '=');
                    break;
                default:
                    throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
            }
        }

        private bool IsEnnabled(ItemTableRow<T> item)
        {
            return !IsDisabled(item);
        }

        private bool IsDisabled(ItemTableRow<T> item)
        {
            return _options.Items.Any(x => x.UniqueId == item.UniqueId && x.Disabled);
        }

        private void LoadHistory()
        {
            _defaultHistoric = Optional<IList<T>>.Create(null);
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom, 1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = Optional<IList<T>>.Create(JsonSerializer.Deserialize<IList<T>>(aux[0].History));
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void SaveHistory(IEnumerable<T> value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = JsonSerializer.Serialize<IEnumerable<T>>(value);
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist = FileHistory.AddHistory(aux, _options.TimeoutOverwriteDefault, null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }
    }
}
