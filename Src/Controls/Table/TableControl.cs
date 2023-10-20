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
using System.Threading;
using PPlus.Controls.Objects;


namespace PPlus.Controls.Table
{
    internal class TableControl<T> : BaseControl<bool>, IControlTable<T> where T : class
    {
        private readonly TableOptions<T> _options;
        private int _totalTableLenWidth;
        private int[] _maxlencontentcols;
        private (int startWrite, int endwrite) _tableviewport;
        private bool _hasprompt;

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

            _options.HideSelectorRow = true;

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
                        if (_options.MinColWidth.HasValue && lentext < _options.MinColWidth.Value)
                        {
                            if (i == 0)
                            {
                                lentext = _options.MinColWidth.Value;
                                _options.Columns[i].OriginalWidth = lentext;
                                _options.Columns[i].Width = lentext;
                            }
                        }

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

            _options.EqualItems ??= (item1, item2) => item1.Equals(item2);

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
            return string.Empty;
        }

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (_options.AutoFill && (!_options.MinColWidth.HasValue || !_options.MaxColWidth.HasValue))
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

            screenBuffer.WritePrompt(_options, "");

            _hasprompt = !string.IsNullOrEmpty(_options.OptPrompt) && !_options.OptMinimalRender;

            var hasdesc = screenBuffer.WriteLineDescriptionTable(null, -1, -1, _options);
            if (hasdesc)
            {
                _hasprompt = true;
            }
            return;
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result, bool aborted)
        {
            WriteTable(screenBuffer, _hasprompt);
            screenBuffer.NewLine();
        }

        public override ResultPrompt<bool> TryResult(CancellationToken cancellationToken)
        {
            return new ResultPrompt<bool>(true,false,false,false);
        }

        #region IControlTable


        public IControlTable<T> AutoFill(params ushort?[] minmaxwidth)
        {
            //AutoFill cannot be used with AddColumn and/or AutoFit

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

        public IControlTable<T> AddColumn(Expression<Func<T, object>> field, ushort width, Func<object, string> format = null, Alignment alignment = Alignment.Left, string? title = null, Alignment titlealignment = Alignment.Center, bool titlereplaceswidth = true, bool textcrop = false, ushort? maxslidinglines = null)
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

        public IControlTable<T> AddItem(T value)
        {
            _options.Items.Add(new ItemTableRow<T>() { Value = value, Disabled = false});
            return this;
        }

        public IControlTable<T> AddItems(IEnumerable<T> values)
        {
            foreach (var item in values)
            {
                AddItem(item);
            }
            return this;
        }

        public IControlTable<T> AutoFit(params ushort[] indexColumn)
        {
            if (_options.AutoFill)
            {
                throw new PromptPlusException($"AutoFit cannot be used with AutoFill");
            }
            _options.HasAutoFit = true;
            _options.AutoFitColumns = indexColumn;
            return this;
        }

        public IControlTable<T> Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlTable<T> HideHeaders(bool value = true)
        {
            _options.HideHeaders = value;
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

        public IControlTable<T> SeparatorRows(bool value = true)
        {
            _options.SeparatorRows = value;
            return this;
        }

        public IControlTable<T> Styles(TableStyle styletype, Style value)
        {
            switch (styletype)
            {
                case TableStyle.Title:
                    _options.TitleStyle = value;
                    break;
                case TableStyle.Header:
                    _options.HeaderStyle = value;
                    break;
                case TableStyle.Content:
                    _options.ContentStyle = value;
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
            var stl = _options.OptStyleSchema.Lines();
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
                tit = TableControl<T>.AlignmentText(tit, _options.TitleAlignment, _totalTableLenWidth);
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
            tit = TableControl<T>.AlignmentText(tit, _options.TitleAlignment, _totalTableLenWidth - 2);
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
            screenBuffer.AddBuffer(sep, _options.OptStyleSchema.Lines());
            screenBuffer.AddBuffer(tit, _options.TitleStyle);
            screenBuffer.AddBuffer(sep, _options.OptStyleSchema.Lines());

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
            var stl = _options.OptStyleSchema.Lines();
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
                var h = TableControl<T>.AlignmentText(item.Title.Trim(), item.AlignTitle, item.Width);
                screenBuffer.AddBuffer(h, _options.HeaderStyle);
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
            subset = new ArraySegment<ItemTableRow<T>>(_options.Items.ToArray(), 0, _options.Items.Count);
            var pos = 0;
            foreach (var item in subset)
            {
                pos++;
                screenBuffer.NewLine();
                var isseleted = false;
                var isdisabled = false;

                var cols = GetTextColumns(item.Value, out var lines);

                var sep = " ";
                var sepcol = " ";
                var sepend = " ";
                var stl = _options.OptStyleSchema.Lines();
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
                        if (isseleted)
                        {
                            if (_options.IsColumnsNavigation)
                            {
                                if (itemcol == 0)
                                {
                                    screenBuffer.AddBuffer(col, _options.OptStyleSchema.Selected(), true);
                                }
                                else
                                {
                                    screenBuffer.AddBuffer(col, _options.ContentStyle, true);
                                }
                            }
                            else
                            {
                                screenBuffer.AddBuffer(col, _options.OptStyleSchema.Selected(), true);
                            }
                        }
                        else
                        {
                            var stld = _options.ContentStyle;
                            if (isdisabled)
                            {
                                stld = _options.OptStyleSchema.Disabled();
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
    }
}
