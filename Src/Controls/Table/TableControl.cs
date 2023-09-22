﻿// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading;
using PPlus.Controls.Objects;


namespace PPlus.Controls.Table
{
    internal class TableControl<T> : BaseControl<ResultTable<T>>, IControlTable<T> where T : class
    {
        private readonly TableOptions<T> _options;
        private Optional<T> _defaultHistoric = Optional<T>.Create(null);
        private Paginator<ItemTableRow<T>> _localpaginator;
        private readonly EmacsBuffer _filterBuffer = new(CaseOptions.Uppercase, modefilter: true);
        private bool ShowingFilter => _filterBuffer.Length > 0;
        private int _currentrow = -1;
        private int _currentcol = -1;
        private int _totalTableLenWidth;

        public TableControl(IConsoleControl console, TableOptions<T> options) : base(console, options)
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

            if (!_options.HideSelectorRow)
            {
                _options.Columns.Insert(0, new ItemItemColumn<T> { Field = ((_) => _options.Symbol(SymbolType.Selector)), Title = " ", Width = 1, OriginalWidth = 1 });
            }

            if (_options.FilterColumns.Length > 1)
            {
                if (!_options.HideSelectorRow)
                {
                    for (int i = 0; i < _options.FilterColumns.Length; i++)
                    {
                        _options.FilterColumns[i]++;
                    }
                }
                for (int i = 0; i < _options.FilterColumns.Length; i++)
                {
                    if (_options.FilterColumns[i] > _options.Columns.Count)
                    {
                        throw new PromptPlusException($"FilterColumns {i} Not found in columns definition");
                    }
                }
            }

            if (_options.IsColumnsNavigation && _options.IsInteraction)
            {
                _currentcol = 0;
                if (!_options.HideSelectorRow)
                {
                    _currentcol = 1;
                }
            }

            //Calculate Table Length
            _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.OriginalWidth);

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
                    index = _options.Items.FindIndex(x => !x.IsSeparator && _options.EqualItems(x.Value, item));
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
                founds = _options.Items.FindAll(x => !x.IsSeparator && _options.EqualItems(x.Value, item));
                if (founds.Any())
                {
                    foreach (var itemfound in founds)
                    {
                        itemfound.Disabled = true;
                    }
                }
            }

            Optional<T> defvalue = Optional<T>.s_empty;

            Optional<ItemTableRow<T>> defvaluepage = Optional<ItemTableRow<T>>.s_empty;

            if (_options.DefaultValue.HasValue)
            {
                defvalue = Optional<T>.Create(_options.DefaultValue.Value);
            }
            if (_defaultHistoric.HasValue)
            {
                defvalue = Optional<T>.Create(_defaultHistoric.Value);
            }

            if (defvalue.HasValue)
            {
                var found = _options.Items.FirstOrDefault(x => _options.EqualItems(x.Value, defvalue.Value));
                if (found != null && !found.Disabled)
                {
                    defvaluepage = Optional<ItemTableRow<T>>.Create(found);
                }
            }

            if (_options.OrderBy != null)
            {
                if (_options.IsOrderDescending)
                {
                    _options.Items = _options.Items.Where(x => !x.IsSeparator).OrderByDescending(x => _options.OrderBy.Invoke(x.Value)).ToList();
                }
                else
                {
                    _options.Items = _options.Items.Where(x => !x.IsSeparator).OrderBy(x => _options.OrderBy.Invoke(x.Value)).ToList();
                }
            }
            var skip = 0;
            if (!_options.HideSelectorRow)
            {
                skip = 1;
            }

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

            }
            _currentrow = _localpaginator.CurrentIndex;

            return string.Empty;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_options.IsInteraction)
            {
                //Calculate Table Length again when set autofit
                if (_options.HasAutoFit)
                {
                    for (ushort i = 0; i < _options.Columns.Count; i++)
                    {
                        _options.Columns[i].Width = _options.Columns[i].OriginalWidth;
                    }
                    _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.OriginalWidth);
                    if (_totalTableLenWidth < ConsolePlus.BufferWidth - 1)
                    {
                        var diff = ConsolePlus.BufferWidth - 1 - _totalTableLenWidth;
                        do
                        {
                            for (ushort i = 0; i < _options.Columns.Count; i++)
                            {
                                ushort index = i;
                                if (i == 0 && !_options.HideSelectorRow)
                                {
                                    continue;
                                }
                                if (!_options.HideSelectorRow)
                                {
                                    index--;
                                }
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
                        } while (diff > 0);
                    }
                    _totalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.Width);
                }

                screenBuffer.WritePrompt(_options, "");
                bool hasprompt = !string.IsNullOrEmpty(_options.OptPrompt);
                string answer = null;
                if (_options.SelectedTemplate != null)
                {
                    if (_localpaginator.Count > 0)
                    {
                        answer = _options.SelectedTemplate.Invoke(_localpaginator.SelectedItem.Value, _currentrow, _currentcol);
                    }
                }
                if (!string.IsNullOrEmpty(answer))
                {
                    screenBuffer.AddBuffer(answer, _options.OptStyleSchema.Answer());
                    hasprompt = true;
                }
                if (!ShowingFilter)
                {
                    screenBuffer.SaveCursor();
                }
                else
                {
                    if (hasprompt)
                    {
                        screenBuffer.NewLine();
                    }
                    screenBuffer.WriteTaggedInfo(_options, $"{Messages.Filter}: ");
                    screenBuffer.WriteFilterTable(_options, _filterBuffer.ToString(), _filterBuffer);
                    screenBuffer.SaveCursor();
                }
            }
            if (!ShowingFilter)
            {
                screenBuffer.WriteLineDescriptionTable(_options);
            }

            WriteTable(screenBuffer);
            if (_options.IsInteraction && (!_options.OptShowOnlyExistingPagination || _localpaginator.PageCount > 1))
            {
                screenBuffer.WriteLinePagination(_options, _localpaginator.PaginationMessage());
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, ResultTable<T> result, bool aborted)
        {
            if (!_options.IsInteraction)
            {
                return;
            }
            string answer = null;
            if (!aborted && _options.FinishTemplate != null)
            {
                answer = _options.FinishTemplate.Invoke(_localpaginator.SelectedItem.Value, _currentrow, _currentcol);
            }
            if (aborted)
            {
                screenBuffer.AddBuffer(Messages.CanceledKey, _options.OptStyleSchema.Answer(), true);
            }
            else
            {
                screenBuffer.AddBuffer(answer, _options.OptStyleSchema.Answer(), false);
            }
            screenBuffer.NewLine();
            if (!aborted)
            {
                SaveHistory(result.RowValue);
            }
            if (!_options.RemoveTableAtFinish)
            {
                WriteTable(screenBuffer);
            }
        }

        public override ResultPrompt<ResultTable<T>> TryResult(CancellationToken cancellationToken)
        {
            if (!_options.IsInteraction)
            {
                return new ResultPrompt<ResultTable<T>>(ResultTable<T>.NullResult(),false);
            }
            var endinput = false;
            var abort = false;
            var isvalidkey = false;
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
                else if (keyInfo.Value.IsPressLeftArrowKey(true) && !ShowingFilter)
                {
                    var minpos = 0;
                    if (!_options.HideSelectorRow)
                    {
                        minpos = 1;
                    }
                    if (_currentcol > minpos)
                    {
                        _currentcol--;
                    }
                    else
                    { 
                        _currentcol = _options.Columns.Count-1;
                    }
                    isvalidkey = true;
                }
                else if (keyInfo.Value.IsPressRightArrowKey(true) && !ShowingFilter)
                {
                    if (_currentcol < _options.Columns.Count - 1)
                    {
                        _currentcol++;
                    }
                    else
                    {
                        var minpos = 0;
                        if (!_options.HideSelectorRow)
                        {
                            minpos = 1;
                        }
                        _currentcol = minpos;
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
            if (_localpaginator.Count == 0)
            {
                return new ResultPrompt<ResultTable<T>>(ResultTable<T>.NullResult(),abort, !endinput, notrender);
            }
            return new ResultPrompt<ResultTable<T>>(
                new ResultTable<T>(_currentrow, _currentcol,_localpaginator.SelectedItem.Value, GetValueColumn(_currentcol, _localpaginator.SelectedItem)), 
                abort, !endinput, notrender);
        }

        #region IControlTable

        public IControlTable<T> AddColumn(Expression<Func<T, object>> field, ushort width, Func<object, string> format = null, Alignment alignment = Alignment.Left, string? title = null, Alignment titlealignment = Alignment.Center, bool titlereplaceswidth = true, bool textcrop = false, int? maxslidinglines = null)
        {
            if (maxslidinglines.HasValue)
            {
                if (maxslidinglines.Value <= 0)
                {
                    maxslidinglines = null;
                }
            }
            var tit = title ?? string.Empty;
            if (tit.Length > byte.MaxValue)
            { 
                tit = tit[..byte.MaxValue];
            }
            try
            {
                var fieldname = GetNameUnaryExpression(field);
                if (string.IsNullOrEmpty(tit))
                {
                    tit = fieldname;
                }
            }
            catch (Exception ex)
            {
                throw new PromptPlusException($"Expression field must be UnaryExpression", ex);
            }
            var w = (ushort)(!titlereplaceswidth ? width : (width < tit.Length) ? tit.Length : width);

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
                MaxSlidingLines = maxslidinglines
            });
            return this;
        }

        public IControlTable<T> AddFormatType<T1>(Func<object,string> funcfomatType)
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

        public IControlTable<T> AddItem(T value, bool disable = false)
        {
            _options.Items.Add(new ItemTableRow<T>() { Value = value, Disabled = disable, IsSeparator = false });
            return this;
        }

        public IControlTable<T> AddItems(IEnumerable<T> values, bool disable = false)
        {
            foreach (var item in values)
            {
                AddItem(item, disable);
            }
            return this;
        }

        public IControlTable<T> AddItemsTo(AdderScope scope, params T[] values)
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

        public IControlTable<T> AutoFit(params ushort[] indexColumn)
        {
            _options.HasAutoFit = true;
            _options.AutoFitColumns = indexColumn;
            return this;
        }

        public IControlTable<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlTable<T> HideSelectorRow()
        {
            _options.HideSelectorRow = true;
            return this;
        }

        public IControlTable<T> HideHeaders()
        {
            _options.HideHeaders = true;
            return this;
        }

        public IControlTable<T> Default(T value)
        {
            _options.DefaultValue = Optional<T>.Create(value);
            return this;
        }

        public IControlTable<T> EnableColumnsNavigation()
        {
            _options.IsColumnsNavigation = true;
            return this;
        }

        public IControlTable<T> EnabledInteractionUser(Func<T, int, int, string> selectedTemplate = null, Func<T, int, int, string> finishTemplate = null, bool removetable = true)
        {
            _options.IsInteraction = true;
            _options.SelectedTemplate = selectedTemplate;
            _options.FinishTemplate = finishTemplate;
            _options.RemoveTableAtFinish = removetable;
            return this;
        }

        public IControlTable<T> ChangeDescription(Func<T, int, int, string> func = null)
        {
            _options.ChangeDescription = func;
            return this;
        }
        public IControlTable<T> EqualItems(Func<T, T, bool> comparer)
        {
            _options.EqualItems = comparer;
            return this;
        }

        public IControlTable<T> FilterByColumns(FilterMode filter = FilterMode.Contains, params ushort[] indexColumn)
        {
            if (filter == FilterMode.Disabled)
            {
                throw new PromptPlusException($"Invalid filter : {filter}");
            }
            _options.FilterType = filter;
            _options.FilterColumns = indexColumn;
            return this;
        }

        public IControlTable<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlTable<T>, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }

        public IControlTable<T> Layout(TableLayout value)
        {
            _options.Layout = value;
            return this;
        }

        public IControlTable<T> OrderBy(Expression<Func<T, object>> value)
        {
            _options.IsOrderDescending = false;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlTable<T> OrderByDescending(Expression<Func<T, object>> value)
        {
            _options.IsOrderDescending = true;
            _options.OrderBy = value.Compile();
            return this;
        }

        public IControlTable<T> OverwriteDefaultFrom(string value, TimeSpan? timeout = null)
        {
            _options.OverwriteDefaultFrom = value;
            if (timeout != null)
            {
                _options.TimeoutOverwriteDefault = timeout.Value;
            }
            return this;
        }

        public IControlTable<T> PageSize(int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            _options.PageSize = value;
            return this;
        }

        public IControlTable<T> WithSeparatorRows()
        {
            _options.WithSeparatorRows = true;
            return this;
        }

        public IControlTable<T> Styles(TableStyle styletype, Style value)
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
                case TableStyle.SelectedColHeader:
                    _options.SelectedColHeader = value;
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
                case TableStyle.SelectedSContent:
                    _options.SelectedContentStyle = value;
                    break;
                default:
                    throw new PromptPlusException($"TableStyle: {styletype} Not Implemented");
            }
            return this;
        }

        public IControlTable<T> Title(string value, Alignment alignment = Alignment.Center, TableTitleMode titleMode = TableTitleMode.InLine)
        {
            _options.Title = value;
            _options.TitleAlignment = alignment;
            _options.TitleMode = titleMode;
            return this;
        }

        #endregion

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
            screenBuffer.AddBuffer($"{startln}{new string(contentln, _options.Columns[0].Width)}", _options.GridStyle.Overflow(Overflow.Crop));
            foreach (var item in _options.Columns.Skip(1))
            {
                screenBuffer.AddBuffer($"{sepln}{new string(contentln, item.Width)}", _options.GridStyle.Overflow(Overflow.Crop));
            }
            screenBuffer.AddBuffer(endln, _options.GridStyle.Overflow(Overflow.Crop));
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
            string col = GetTextColumn(value, _options.Columns[0].Field, _options.Columns[0].Format);
            cols.Add(col);
            foreach (var defcol in _options.Columns.Skip(1))
            {
                col = GetTextColumn(value, defcol.Field, defcol.Format);
                cols.Add(col);
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
            if (string.IsNullOrEmpty(value)) throw new PromptPlusException("SplitIntoChunks: The string cannot be null.");
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

        private void WriteTable(ScreenBuffer screenBuffer)
        {
            if (!_options.IsInteraction)
            {
                WriteTableAllRows(screenBuffer);
                return;
            }
            WriteTableTitle(screenBuffer);
            WriteTableHeader(screenBuffer);
            WriteTableRows(screenBuffer);
            WriteTableFooter(screenBuffer);
        }

        private void WriteTableTitle(ScreenBuffer screenBuffer)
        {
            var tit = _options.Title;
            if (_options.TitleMode == TableTitleMode.InLine)
            {
                if (string.IsNullOrEmpty(tit))
                {
                    return;
                }
                screenBuffer.NewLine();
                tit = TableControl<T>.AlignmentText(tit, _options.TitleAlignment, _totalTableLenWidth);
                screenBuffer.AddBuffer(tit, _options.TitleStyle.Overflow(Overflow.Crop));
                tit = string.Empty;
            }
            WriteTopTable(screenBuffer);
            if (!string.IsNullOrEmpty(tit))
            {
                WriteTitleInRow(tit, screenBuffer);
            }
        }

        private void WriteTitleInRow(string tit, ScreenBuffer screenBuffer)
        {
            screenBuffer.NewLine();
            tit = TableControl<T>.AlignmentText(tit, _options.TitleAlignment, _totalTableLenWidth - 2);
            switch (_options.Layout)
            {
                case TableLayout.SingleGridSoft:
                case TableLayout.SingleGridFull:
                    screenBuffer.AddBuffer('│', _options.GridStyle.Overflow(Overflow.Crop));
                    screenBuffer.AddBuffer(tit, _options.TitleStyle.Overflow(Overflow.Crop));
                    screenBuffer.AddBuffer('│', _options.GridStyle.Overflow(Overflow.Crop));
                    break;
                case TableLayout.DoubleGridFull:
                case TableLayout.DoubleGridSoft:
                    break;
                case TableLayout.AsciiSingleGridFull:
                case TableLayout.AsciiSingleGridSoft:
                    break;
                case TableLayout.AsciiDoubleGridFull:
                case TableLayout.AsciiDoubleGridSoft:
                    break;
                case TableLayout.HeavyGridFull:
                case TableLayout.HeavyGridSoft:
                    break;
                default:
                    throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
            }
            screenBuffer.NewLine();
            switch (_options.Layout)
            {
                case TableLayout.DoubleGridSoft:
                    break;
                case TableLayout.AsciiSingleGridSoft:
                    break;
                case TableLayout.AsciiDoubleGridSoft:
                    break;
                case TableLayout.HeavyGridSoft:
                    break;
                case TableLayout.SingleGridSoft:
                case TableLayout.SingleGridFull:
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
                    break;
                case TableLayout.AsciiSingleGridFull:
                    break;
                case TableLayout.AsciiDoubleGridFull:
                    break;
                case TableLayout.HeavyGridFull:
                    break;
            }
        }

        private void WriteTopTable(ScreenBuffer screenBuffer)
        {
            screenBuffer.NewLine();
            if (_options.TitleMode == TableTitleMode.InRow && !string.IsNullOrEmpty(_options.Title))
            {
                switch (_options.Layout)
                {
                    case TableLayout.SingleGridFull:
                    case TableLayout.SingleGridSoft:
                        BuildLineColumn(screenBuffer, '┌', '─', '┐', '─');
                        break;
                    case TableLayout.DoubleGridFull:
                    case TableLayout.DoubleGridSoft:
                        break;
                    case TableLayout.AsciiSingleGridFull:
                    case TableLayout.AsciiSingleGridSoft:
                        break;
                    case TableLayout.AsciiDoubleGridFull:
                    case TableLayout.AsciiDoubleGridSoft:
                        break;
                    case TableLayout.HeavyGridFull:
                    case TableLayout.HeavyGridSoft:
                        break;
                    default:
                        throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
                }
                return;
            }
            switch (_options.Layout)
            {
                case TableLayout.SingleGridSoft:
                case TableLayout.SingleGridFull:
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
                    break;
                case TableLayout.DoubleGridSoft:
                    break;
                case TableLayout.AsciiSingleGridFull:
                    break;
                case TableLayout.AsciiSingleGridSoft:
                    break;
                case TableLayout.AsciiDoubleGridFull:
                    break;
                case TableLayout.AsciiDoubleGridSoft:
                    break;
                case TableLayout.HeavyGridFull:
                    break;
                case TableLayout.HeavyGridSoft:
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
            foreach (var item in _options.Columns)
            {
                col++;
                screenBuffer.AddBuffer('│', _options.GridStyle.Overflow(Overflow.Crop));
                var h = TableControl<T>.AlignmentText(item.Title, item.AlignTitle, item.Width);
                if (_options.IsColumnsNavigation && _options.IsInteraction && col == _currentcol)
                {
                    screenBuffer.AddBuffer(h, _options.SelectedColHeader.Overflow(Overflow.Crop));
                }
                else
                {
                    screenBuffer.AddBuffer(h, _options.HeaderStyle.Overflow(Overflow.Crop));
                }
            }
            screenBuffer.AddBuffer('│', _options.GridStyle.Overflow(Overflow.Crop));
            screenBuffer.NewLine();
            switch (_options.Layout)
            {
                case TableLayout.SingleGridFull:
                    BuildLineColumn(screenBuffer, '├', '┼', '┤', '─');
                    break;
                case TableLayout.DoubleGridFull:
                    break;
                case TableLayout.AsciiSingleGridFull:
                    break;
                case TableLayout.AsciiDoubleGridFull:
                    break;
                case TableLayout.HeavyGridFull:
                    break;
                case TableLayout.SingleGridSoft:
                    BuildLineColumn(screenBuffer, '├', '┴', '┤', '─');
                    break;
                case TableLayout.DoubleGridSoft:
                    break;
                case TableLayout.AsciiSingleGridSoft:
                    break;
                case TableLayout.AsciiDoubleGridSoft:
                    break;
                case TableLayout.HeavyGridSoft:
                    break;
            }
        }

        private void WriteTableRows(ScreenBuffer screenBuffer)
        {
            var subset = _localpaginator.ToSubset();
            var pos = 0;
            foreach (var item in subset)
            {
                pos++;
                screenBuffer.NewLine();
                var isseleted = false;
                var isdisabled = false;
                if (_localpaginator.TryGetSelectedItem(out var selectedItem) && EqualityComparer<ItemTableRow<T>>.Default.Equals(item, selectedItem))
                {
                    isseleted = true;
                }
                else
                {
                    isdisabled = IsDisabled(item);
                }

                var cols = GetTextColumns(item.Value, out var lines);

                switch (_options.Layout)
                {
                    case TableLayout.SingleGridFull:
                    case TableLayout.SingleGridSoft:
                        {
                            for (int i = 0; i < lines; i++)
                            {
                                for (int itemcol = 0; itemcol < cols.Count; itemcol++)
                                {
                                    if (itemcol == 0)
                                    {
                                        screenBuffer.AddBuffer("│", _options.GridStyle.Overflow(Overflow.Crop));
                                    }
                                    else if (_options.Layout == TableLayout.SingleGridFull)
                                    {
                                        screenBuffer.AddBuffer("│", _options.GridStyle.Overflow(Overflow.Crop));
                                    }
                                    else if (_options.Layout == TableLayout.SingleGridSoft)
                                    {
                                        screenBuffer.AddBuffer(" ", _options.GridStyle.Overflow(Overflow.Crop));
                                    }
                                    string col = null;
                                    if (cols[itemcol].Length > i)
                                    {
                                        col = TableControl<T>.AlignmentText(cols[itemcol][i], _options.Columns[itemcol].AlignCol, _options.Columns[itemcol].Width);
                                    }
                                    else
                                    {
                                        col = new string(' ', _options.Columns[itemcol].Width);
                                    }
                                    if (!_options.HideSelectorRow && itemcol == 0 && !isseleted)
                                    {
                                        col = " ";
                                    }
                                    if (isseleted)
                                    {
                                        if (_options.IsColumnsNavigation)
                                        {
                                            if (itemcol == _currentcol || !_options.HideSelectorRow && itemcol == 0)
                                            {
                                                screenBuffer.AddBuffer(col, _options.SelectedContentStyle.Overflow(Overflow.Crop), true);
                                            }
                                            else
                                            {
                                                screenBuffer.AddBuffer(col, _options.ContentStyle.Overflow(Overflow.Crop), true);
                                            }
                                        }
                                        else
                                        {
                                            screenBuffer.AddBuffer(col, _options.SelectedContentStyle.Overflow(Overflow.Crop), true);
                                        }
                                    }
                                    else
                                    {
                                        var stl = _options.ContentStyle.Overflow(Overflow.Crop);
                                        if (isdisabled)
                                        {
                                            stl = _options.DisabledContentStyle.Overflow(Overflow.Crop);
                                        }
                                        screenBuffer.AddBuffer(col, stl, true);
                                    }
                                }
                                screenBuffer.AddBuffer("│", _options.GridStyle.Overflow(Overflow.Crop));
                                if (lines > 1 && i != lines-1)
                                {
                                    screenBuffer.NewLine();
                                }
                            }

                            if (_options.WithSeparatorRows && pos < subset.Count)
                            {
                                screenBuffer.NewLine();
                                if (_options.Layout == TableLayout.SingleGridSoft)
                                {
                                    BuildLineColumn(screenBuffer, '├', '─', '┤', '─');
                                }
                                else
                                {
                                    BuildLineColumn(screenBuffer, '├', '┼', '┤', '─');
                                }
                            }
                        }
                        break;
                    case TableLayout.DoubleGridFull:
                    case TableLayout.DoubleGridSoft:
                        break;
                    case TableLayout.AsciiSingleGridFull:
                    case TableLayout.AsciiSingleGridSoft:
                        break;
                    case TableLayout.AsciiDoubleGridFull:
                    case TableLayout.AsciiDoubleGridSoft:
                        break;
                    case TableLayout.HeavyGridFull:
                    case TableLayout.HeavyGridSoft:
                        break;
                    default:
                        break;
                }
            }
        }

        private void WriteTableFooter(ScreenBuffer screenBuffer)
        {
            screenBuffer.NewLine();
            switch (_options.Layout)
            {
                case TableLayout.SingleGridFull:
                    BuildLineColumn(screenBuffer, '└', '┴', '┘', '─');
                    break;
                case TableLayout.SingleGridSoft:
                    BuildLineColumn(screenBuffer, '└', '─', '┘', '─');
                    break;
                case TableLayout.DoubleGridFull:
                    break;
                case TableLayout.DoubleGridSoft:
                    break;
                case TableLayout.AsciiSingleGridFull:
                    break;
                case TableLayout.AsciiSingleGridSoft:
                    break;
                case TableLayout.AsciiDoubleGridFull:
                    break;
                case TableLayout.AsciiDoubleGridSoft:
                    break;
                case TableLayout.HeavyGridFull:
                    break;
                case TableLayout.HeavyGridSoft:
                    break;
                default:
                    throw new PromptPlusException($"Layout {_options.Layout} Not implemented");
            }
        }

        private void WriteTableAllRows(ScreenBuffer screenBuffer)
        {
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
            _defaultHistoric = Optional<T>.Create(null);
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = FileHistory.LoadHistory(_options.OverwriteDefaultFrom, 1);
                if (aux.Count == 1)
                {
                    try
                    {
                        _defaultHistoric = Optional<T>.Create(JsonSerializer.Deserialize<T>(aux[0].History));
                    }
                    catch
                    {
                        //invalid Deserialize history 
                    }
                }
            }
        }

        private void SaveHistory(T value)
        {
            if (!string.IsNullOrEmpty(_options.OverwriteDefaultFrom))
            {
                var aux = JsonSerializer.Serialize<T>(value);
                FileHistory.ClearHistory(_options.OverwriteDefaultFrom);
                var hist = FileHistory.AddHistory(aux, _options.TimeoutOverwriteDefault, null);
                FileHistory.SaveHistory(_options.OverwriteDefaultFrom, hist);
            }
        }
    }
}
