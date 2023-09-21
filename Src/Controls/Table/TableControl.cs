// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
        private int _currentrow;
        private byte _currentcol;
        private int _TotalTableLenWidth;

        public TableControl(IConsoleControl console, TableOptions<T> options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            _options.CurrentCulture ??= _options.Config.AppCulture;

            //Validate has columns
            if (_options.Columns.Count == 0)
            {
                throw new PromptPlusException("Not found columns definition");
            }

            //Validate Merge Headers
            if (_options.MergeHeaders.Count > 0) 
            {
                foreach (var item in _options.MergeHeaders)
                {
                    if (item.StartColumn > _options.Columns.Count)
                    {
                        throw new PromptPlusException($"Invalid Merge Headers.StartColumn {item.StartColumn} not found");
                    }
                    if (item.EndColumn > _options.Columns.Count)
                    {
                        throw new PromptPlusException($"Invalid Merge Headers.EndColumn {item.EndColumn} not found");
                    }
                }
            }

            //Calculate Table Length
            _TotalTableLenWidth = _options.Columns.Count + 1 + _options.Columns.Sum(x => x.Width);
  
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

            _localpaginator = new Paginator<ItemTableRow<T>>(
                _options.FilterType,
                _options.Items,
                _options.PageSize,
                defvaluepage,
                (item1, item2) => item1.UniqueId == item2.UniqueId,
                null,
                IsEnnabled);

            return string.Empty;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_options.IsInteraction)
            {
                screenBuffer.WritePrompt(_options, "");
                string answer = null;
                if (_options.SelectedTemplate != null)
                {
                    answer = _options.SelectedTemplate.Invoke(_localpaginator.SelectedItem.Value, _currentcol);
                }
                if (ShowingFilter)
                {
                    screenBuffer.WriteTaggedInfo(_options, $"{Messages.Filter}: ");
                    screenBuffer.WriteFilterTable(_options, _filterBuffer.ToString(), _filterBuffer);
                    screenBuffer.NewLine();
                }
                else
                {
                    if (!string.IsNullOrEmpty(answer))
                    {
                        screenBuffer.AddBuffer(answer, _options.OptStyleSchema.Answer());
                    }
                    screenBuffer.SaveCursor();
                    if (!string.IsNullOrEmpty(answer))
                    {
                        screenBuffer.NewLine();
                    }
                }
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
                answer = _options.FinishTemplate.Invoke(_localpaginator.SelectedItem.Value, _currentcol);
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
                else if (keyInfo.Value.IsPressEnterKey())
                {
                    endinput = true;
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
            return new ResultPrompt<ResultTable<T>>(
                new ResultTable<T>(_currentrow, _currentcol,_localpaginator.SelectedItem.Value, GetValueColumn(_currentcol, _localpaginator.SelectedItem)), 
                abort, !endinput, notrender);
        }

        #region IControlTable

        public static string GetNameUnaryExpression(Expression<Func<T, object>> exp)
        {
            if (exp.Body is not MemberExpression body)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }
            return body.Member.Name;
        }

        public IControlTable<T> AddColumn(Expression<Func<T, object>> field, byte width, Func<object, string> format = null, Alignment alignment = Alignment.Left, string? title = null, Alignment titlealignment = Alignment.Center, bool titlereplaceswidth = true, bool textcrop = false)
        {
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

            _options.Columns.Add(new ItemItemColumn<T>() 
            { 
                AlignCol = alignment, 
                Field = field.Compile(), 
                Format = format, 
                Width = (byte)(!titlereplaceswidth?width:(width < tit.Length)?tit.Length:width), 
                TextCrop = textcrop, 
                Title = tit, 
                AlignTitle = titlealignment,
            });
            return this;
        }

        public IControlTable<T> AddFormatType<T1>(Func<object, string> funcfomatType)
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

        public IControlTable<T> AutoFit(params byte[] indexColumn)
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

        public IControlTable<T> HideHeaders()
        {
            _options.HideHeaders = true;
            return this;
        }

        public IControlTable<T> Culture(CultureInfo value)
        {
            _options.CurrentCulture = value;
            return this;
        }

        public IControlTable<T> Culture(string value)
        {
            _options.CurrentCulture = new CultureInfo(value);
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

        public IControlTable<T> EnabledInteractionUser(Func<T, byte, string> selectedTemplate = null, Func<T, byte, string> finishTemplate = null, bool removetable = true)
        {
            _options.IsInteraction = true;
            _options.SelectedTemplate = selectedTemplate;
            _options.FinishTemplate = finishTemplate;
            _options.RemoveTableAtFinish = removetable;
            return this;
        }

        public IControlTable<T> EqualItems(Func<T, T, bool> comparer)
        {
            _options.EqualItems = comparer;
            return this;
        }

        public IControlTable<T> FilterByColumns(params byte[] indexColumn)
        {
            _options.FilterColumns = indexColumn;
            return this;
        }

        public IControlTable<T> FilterType(FilterMode value = FilterMode.Disabled)
        {
            _options.FilterType = value;
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

        public IControlTable<T> MergeColumns(string value, byte startColumn, byte endcolumn, Alignment alignment = Alignment.Center)
        {
            if (startColumn >= endcolumn)
            {
                throw new PromptPlusException($"Invalid StartColumn({startColumn}) and/or EndColumn {endcolumn}");
            }
            if (_options.MergeHeaders.Any(x => x.StartColumn == startColumn || x.EndColumn == endcolumn))
            {
                throw new PromptPlusException($"there is already a range of columns being used for these values: {startColumn}~{endcolumn}");
            }
            _options.MergeHeaders.Add(new ItemTableMergeHeader(value, alignment, startColumn, endcolumn));
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

        private object GetValueColumn(byte column, ItemTableRow<T> item)
        {
            return _options.Columns[column].Field.Invoke(item.Value);
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
                switch (_options.TitleAlignment)
                {
                    case Alignment.Left:
                        break;
                    case Alignment.Right:
                        tit = tit.PadLeft(_TotalTableLenWidth);
                        screenBuffer.AddBuffer(tit, _options.TitleStyle.Overflow(Overflow.Crop));
                        break;
                    case Alignment.Center:
                        {
                            if (tit.Length < _TotalTableLenWidth)
                            {
                                tit = new string(' ', (_TotalTableLenWidth - tit.Length) / 2) + tit;
                            }
                        }
                        break;
                    default:
                        throw new PromptPlusException($"Alignment {_options.TitleAlignment} Not implemented");
                }
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
            switch (_options.TitleAlignment)
            {
                case Alignment.Left:
                    break;
                case Alignment.Right:
                    tit = tit.PadLeft(_TotalTableLenWidth-2);
                    break;
                case Alignment.Center:
                    {
                        if (tit.Length < _TotalTableLenWidth -2)
                        {
                            var spc = new string(' ', (_TotalTableLenWidth -2 - tit.Length ) / 2);
                            tit = $"{spc}{tit}{spc}".PadRight(_TotalTableLenWidth - 2);
                        }
                    }
                    break;
                default:
                    throw new PromptPlusException($"Alignment {_options.TitleAlignment} Not implemented");
            }
            if (tit.Length > _TotalTableLenWidth - 2)
            {
                tit = tit[..(_TotalTableLenWidth - 2)];
            }
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
                case TableLayout.SingleGridSoft:
                    screenBuffer.AddBuffer($"├{new string('─', _TotalTableLenWidth - 2)}┤", _options.GridStyle.Overflow(Overflow.Crop));
                    break;
                case TableLayout.DoubleGridSoft:
                    break;
                case TableLayout.AsciiSingleGridSoft:
                    break;
                case TableLayout.AsciiDoubleGridSoft:
                    break;
                case TableLayout.HeavyGridSoft:
                    break;
                case TableLayout.SingleGridFull:
                    screenBuffer.AddBuffer($"├{new string('─', _options.Columns[0].Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    foreach (var item in _options.Columns.Skip(1))
                    {
                        screenBuffer.AddBuffer($"┬{new string('─', item.Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    }
                    screenBuffer.AddBuffer("┤", _options.GridStyle.Overflow(Overflow.Crop));
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
                        screenBuffer.AddBuffer($"┌{new string('─', _TotalTableLenWidth - 2)}┐", _options.GridStyle.Overflow(Overflow.Crop));
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
                case TableLayout.SingleGridFull:
                    screenBuffer.AddBuffer($"┌{new string('─', _options.Columns[0].Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    foreach (var item in _options.Columns.Skip(1))
                    {
                        screenBuffer.AddBuffer($"┬{ new string('─', item.Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    }
                    screenBuffer.AddBuffer("┐", _options.GridStyle.Overflow(Overflow.Crop));
                    break;
                case TableLayout.SingleGridSoft:
                    screenBuffer.AddBuffer($"┌{new string('─', _TotalTableLenWidth-2)}┐", _options.GridStyle.Overflow(Overflow.Crop));
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
            foreach (var item in _options.Columns)
            {
                screenBuffer.AddBuffer('│', _options.GridStyle.Overflow(Overflow.Crop));
                var h = item.Title;
                switch (item.AlignTitle)
                {
                    case Alignment.Left:
                        if (h.Length < item.Width)
                        { 
                            h = h.PadRight(item.Width);
                        }
                        break;
                    case Alignment.Right:
                        if (h.Length < item.Width)
                        {
                            h = h.PadLeft(item.Width);
                        }
                        break;
                    case Alignment.Center:
                        {
                            if (h.Length < item.Width)
                            {
                                var spc = new string(' ', (item.Width - h.Length) / 2);
                                h = $"{spc}{h}{spc}".PadRight(item.Width);
                            }
                        }
                        break;
                    default:
                        throw new PromptPlusException($"Alignment {_options.TitleAlignment} Not implemented");
                }
                if (h.Length > item.Width)
                {
                    h = h[..(item.Width)];
                }
                screenBuffer.AddBuffer(h, _options.HeaderStyle.Overflow(Overflow.Crop));
            }
            screenBuffer.AddBuffer('│', _options.GridStyle.Overflow(Overflow.Crop));
            screenBuffer.NewLine();
            switch (_options.Layout)
            {
                case TableLayout.SingleGridFull:
                    screenBuffer.AddBuffer($"├{new string('─', _options.Columns[0].Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    foreach (var item in _options.Columns.Skip(1))
                    {
                        screenBuffer.AddBuffer($"┼{new string('─', item.Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    }
                    screenBuffer.AddBuffer("┤", _options.GridStyle.Overflow(Overflow.Crop));
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
                    screenBuffer.AddBuffer($"├{new string('─', _TotalTableLenWidth - 2)}┤", _options.GridStyle.Overflow(Overflow.Crop));
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
                switch (_options.Layout)
                {
                    case TableLayout.SingleGridFull:
                    case TableLayout.SingleGridSoft:
                        {
                            screenBuffer.AddBuffer("│", _options.GridStyle.Overflow(Overflow.Crop));
                            var objcol = _options.Columns[0].Field(item.Value);
                            string col;
                            if (_options.Columns[0].Format != null)
                            {
                                col = _options.Columns[0].Format(objcol);
                            }
                            else if (_options.FormatTypes.ContainsKey(objcol.GetType()))
                            {
                                col = _options.FormatTypes[objcol.GetType()](objcol);
                            }
                            else
                            {
                                col = objcol.ToString();
                            }
                            switch (_options.Columns[0].AlignCol)
                            {
                                case Alignment.Left:
                                    if (col.Length < _options.Columns[0].Width)
                                    {
                                        col = col.PadRight(_options.Columns[0].Width);
                                    }
                                    break;
                                case Alignment.Right:
                                    if (col.Length < _options.Columns[0].Width)
                                    {
                                        col = col.PadLeft(_options.Columns[0].Width);
                                    }
                                    break;
                                case Alignment.Center:
                                    {
                                        if (col.Length < _options.Columns[0].Width)
                                        {
                                            var spc = new string(' ', (_options.Columns[0].Width - col.Length) / 2);
                                            col = $"{spc}{col}{spc}".PadRight(_options.Columns[0].Width);
                                        }
                                    }
                                    break;
                                default:
                                    throw new PromptPlusException($"Alignment {_options.Columns[0].AlignCol} Not implemented");
                            }
                            if (col.Length > _options.Columns[0].Width)
                            {
                                col = col[..(_options.Columns[0].Width)];
                            }
                            screenBuffer.AddBuffer(col, _options.ContentStyle.Overflow(Overflow.Crop));
                            foreach (var defcol in _options.Columns.Skip(1))
                            {
                                objcol = defcol.Field(item.Value);
                                if (defcol.Format != null)
                                {
                                    col = defcol.Format(objcol);
                                }
                                else if (_options.FormatTypes.ContainsKey(objcol.GetType()))
                                {
                                    col = _options.FormatTypes[objcol.GetType()](objcol);
                                }
                                else
                                {
                                    col = objcol.ToString();
                                }
                                switch (defcol.AlignCol)
                                {
                                    case Alignment.Left:
                                        if (col.Length < defcol.Width)
                                        {
                                            col = col.PadRight(defcol.Width);
                                        }
                                        break;
                                    case Alignment.Right:
                                        if (col.Length < defcol.Width)
                                        {
                                            col = col.PadLeft(defcol.Width);
                                        }
                                        break;
                                    case Alignment.Center:
                                        {
                                            if (col.Length < defcol.Width)
                                            {
                                                var spc = new string(' ', (defcol.Width - col.Length) / 2);
                                                col = $"{spc}{col}{spc}".PadRight(defcol.Width);
                                            }
                                        }
                                        break;
                                    default:
                                        throw new PromptPlusException($"Alignment {defcol.AlignCol} Not implemented");
                                }
                                if (col.Length > defcol.Width)
                                {
                                    col = col[..(defcol.Width)];
                                }
                                var sep = "│";
                                if (_options.Layout == TableLayout.SingleGridSoft)
                                {
                                    sep = " ";
                                }
                                screenBuffer.AddBuffer(sep, _options.GridStyle.Overflow(Overflow.Crop));
                                screenBuffer.AddBuffer(col, _options.ContentStyle.Overflow(Overflow.Crop));
                            }
                            screenBuffer.AddBuffer("│", _options.GridStyle.Overflow(Overflow.Crop));
                            if (_options.WithSeparatorRows && pos < subset.Count)
                            {
                                screenBuffer.NewLine();
                                screenBuffer.AddBuffer($"├{new string('─', _options.Columns[0].Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                                foreach (var itemcol in _options.Columns.Skip(1))
                                {
                                    screenBuffer.AddBuffer($"┼{new string('─', itemcol.Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                                }
                                screenBuffer.AddBuffer("┤", _options.GridStyle.Overflow(Overflow.Crop));
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
                    screenBuffer.AddBuffer($"└{new string('─', _options.Columns[0].Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    foreach (var item in _options.Columns.Skip(1))
                    {
                        screenBuffer.AddBuffer($"┴{new string('─', item.Width)}", _options.GridStyle.Overflow(Overflow.Crop));
                    }
                    screenBuffer.AddBuffer("┘", _options.GridStyle.Overflow(Overflow.Crop));
                    break;
                case TableLayout.SingleGridSoft:
                    screenBuffer.AddBuffer($"└{new string('─', _TotalTableLenWidth - 2)}┘", _options.GridStyle.Overflow(Overflow.Crop));
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
