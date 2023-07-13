// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/WenceyWang/FIGlet.Net
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PPlus.Controls
{
    internal class ChartControl : IControlChart
    {
        private readonly IConsoleBase _console;
        private readonly ChartType _chartType;
        private char _charBar = '#';
        private List<(string label, double value, Color? color)> _items = new();
        private CultureInfo _cultureInfo = null;
        private int _fracionalDig = 0;
        private ChartOrder _chartOrder = ChartOrder.None;
        private string _title = null;
        private Style _styletitle = Style.Plain;
        private TitleAligment _titlealigment = TitleAligment.Left;
        private int _width = 80;
        private int _padleft = 0;
        private int _pagesize = 10;
        private bool _showPercent = true;
        private bool _showvalue = true;
        private bool _showlegends = false;
        private bool _showlegendsperc = false;
        private bool _showlegendvalue = false;
        private Style _styleLabelChart = Style.Plain;
        private Style _stylePercentChart = Style.Plain;
        private Style _styleValueChart = Style.Plain;

        private ChartControl()
        {
            throw new PromptPlusException("ChartControl CTOR NotImplemented");
        }

        public ChartControl(IConsoleBase console, ChartType chartType)
        {
            _console = console;
            _chartType = chartType;
            switch (_chartType)
            {
                case ChartType.StandBar:
                    break;
                case ChartType.StackBar:
                    {
                        _showPercent = false;
                        _showvalue = false;
                        _showlegends = true;
                        _showlegendsperc = true;
                        _showlegendvalue=true;
                    }
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {_chartType}");
            }
        }

        public IControlChart AddItem(string label, double value, Color? colorbar)
        {
            _items.Add(new (label??string.Empty.Trim(), value, colorbar));
            return this;
        }

        public IControlChart PageSize(int value)
        {
            if (value < 1)
            {
                value = 1;
            }
            _pagesize = value;
            return this;
        }

        public IControlChart Styles(StyleChart styletype, Style value)
        {
            switch (styletype)
            {
                case StyleChart.Label:
                    _styleLabelChart = value;
                    break;
                case StyleChart.Percent:
                    _stylePercentChart = value;
                    break;
                case StyleChart.Value:
                    _styleValueChart = value;
                    break;
                default:
                    throw new PromptPlusException($"StyleChart: {styletype} Not Implemented");
            }
            return this;
        }

        public IControlChart CharBar(char value)
        {
            _charBar = value;
            return this;
        }

        public IControlChart Culture(CultureInfo value)
        {
            _cultureInfo = value;
            return this;
        }

        public IControlChart Culture(string value)
        {
            _cultureInfo = new CultureInfo(value);
            return this;
        }

        public IControlChart FracionalDig(int value)
        {
            _fracionalDig = value;
            return this;
        }

        public IControlChart Interaction<T1>(IEnumerable<T1> values, Action<IControlChart, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }

        public IControlChart OrderBy(ChartOrder chartOrder)
        {
            _chartOrder = chartOrder;
            return this;
        }

        public IControlChart Title(string value, TitleAligment titlealigment = TitleAligment.Left, Style? style = null)
        {
            _title = value.Trim();
            _titlealigment = titlealigment;
            if (style.HasValue) 
            {
                _styletitle = style.Value;
            }
            return this;
        }

        public IControlChart Width(int value)
        {
            if (value <= 0)
            {
                value = 1;
            }
            _width = value;
            return this;
        }

        public IControlChart ChartPadLeft(int value = 1)
        {
            _padleft = value;
            return this;
        }

        public IControlChart HidePercent()
        {
            if (_chartType != ChartType.StandBar)
            {
                return this;
            }
            _showPercent = false;
            return this;
        }

        public IControlChart HideValue()
        {
            if (_chartType != ChartType.StandBar)
            {
                return this;
            }
            _showvalue = false;
            return this;
        }

        public IControlChart ShowLegends(bool withvalue = true, bool withPercent = true)
        {
            if (_chartType == ChartType.StandBar)
            {
                _showlegends = true;
            }
            _showlegendvalue = withvalue;
            _showlegendsperc = withPercent;
            return this;
        }


        public void Run(ChartBarType? barType = ChartBarType.Fill, BannerDashOptions bannerDash = BannerDashOptions.None, Color? colorDash = null)
        {
            if (!_items.Any())
            {
                return;
            }

            //order items
            switch (_chartOrder)
            {
                case ChartOrder.None:
                    break;
                case ChartOrder.Highest:
                    _items = _items.OrderByDescending(x => x.value).ToList();
                    break;
                case ChartOrder.Smallest:
                    _items = _items.OrderBy(x => x.value).ToList();
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {_chartOrder}");
            }

            var indexcolor = 15;
            for (int i = 0; i < _items.Count; i++)
            {
                if (!_items[i].color.HasValue)
                {
                    if (Color.FromInt32(indexcolor) == Color.FromConsoleColor(_console.BackgroundColor))
                    {
                        indexcolor--;
                        if (indexcolor < 0)
                        { 
                            indexcolor = 15;
                        }
                    }
                    _items[i] = new(_items[i].label, _items[i].value, Color.FromInt32(indexcolor));
                    indexcolor--;
                }
            }

            _cultureInfo ??= PromptPlus.Config.AppCulture;

            double totalvalues = _items.Sum(x => x.value);

            Color localcorlor = _console.ForegroundColor;
            if (colorDash != null)
            {
                localcorlor = colorDash.Value;
            }

            //show BannerDashOptions
            if (bannerDash != BannerDashOptions.None)
            {
                char? dach = null;
                switch (bannerDash)
                {
                    case BannerDashOptions.AsciiSingleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.SingleBorder).value[0];
                        break;
                    case BannerDashOptions.AsciiDoubleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.DoubleBorder).value[0];
                        break;
                    case BannerDashOptions.SingleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.SingleBorder).unicode[0];
                        break;
                    case BannerDashOptions.DoubleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.DoubleBorder).unicode[0];
                        break;
                    case BannerDashOptions.HeavyBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.HeavyBorder).unicode[0];
                        break;
                    default:
                        break;
                }
                if (!_console.IsUnicodeSupported && dach.HasValue)
                {
                    switch (bannerDash)
                    {
                        case BannerDashOptions.SingleBorderUpDown:
                            dach = PromptPlus.Config.Symbols(SymbolType.SingleBorder).value[0];
                            break;
                        case BannerDashOptions.DoubleBorderUpDown:
                            dach = PromptPlus.Config.Symbols(SymbolType.DoubleBorder).value[0];
                            break;
                        case BannerDashOptions.HeavyBorderUpDown:
                            dach = PromptPlus.Config.Symbols(SymbolType.HeavyBorder).value[0];
                            break;
                        default:
                            break;
                    }
                }
                if (dach.HasValue)
                {
                    _console.WriteLine(new string(dach.Value, _width), Style.Plain.Foreground(localcorlor).Overflow(Overflow.Crop));
                }
            }

            //title
            if (!string.IsNullOrEmpty(_title)) 
            {
                switch (_titlealigment)
                {
                    case TitleAligment.Left:
                        _console.WriteLine(_title.PadLeft(_title.Length+_padleft), _styletitle);
                        break;
                    case TitleAligment.Right:
                        {
                            var aux = _title;
                            if (aux.Length < _width)
                            {
                                aux = new string(' ', _width - _title.Length) + _title;
                                _console.WriteLine(aux, _styletitle);
                            }
                            else
                            {
                                _console.WriteLine(_title.PadLeft(_title.Length + _padleft), _styletitle);
                            }
                        }
                        break;
                    case TitleAligment.Center:
                        {
                            var aux = _title;
                            if (aux.Length < _width)
                            {
                                aux = new string(' ', (_width - _title.Length) / 2) + _title;
                                _console.WriteLine(aux, _styletitle);
                            }
                            else
                            {
                                _console.WriteLine(_title.PadLeft(_title.Length + _padleft), _styletitle);
                            }
                        }
                        break;
                    default:
                        throw new PromptPlusException($"Not implemented {_titlealigment}");
                }
            }

            //show  type chart
            ShowChart(totalvalues, barType.Value);

            //show BannerDashOptions
            if (bannerDash != BannerDashOptions.None)
            {
                char? dach = null;
                switch (bannerDash)
                {
                    case BannerDashOptions.AsciiSingleBorderDown:
                    case BannerDashOptions.AsciiSingleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.SingleBorder).value[0];
                        break;
                    case BannerDashOptions.AsciiDoubleBorderDown:
                    case BannerDashOptions.AsciiDoubleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.DoubleBorder).value[0];
                        break;
                    case BannerDashOptions.SingleBorderDown:
                    case BannerDashOptions.SingleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.SingleBorder).unicode[0];
                        break;
                    case BannerDashOptions.DoubleBorderDown:
                    case BannerDashOptions.DoubleBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.DoubleBorder).unicode[0];
                        break;
                    case BannerDashOptions.HeavyBorderDown:
                    case BannerDashOptions.HeavyBorderUpDown:
                        dach = PromptPlus.Config.Symbols(SymbolType.HeavyBorder).unicode[0];
                        break;
                    default:
                        break;
                }
                if (!_console.IsUnicodeSupported && dach.HasValue)
                {
                    switch (bannerDash)
                    {
                        case BannerDashOptions.SingleBorderDown:
                        case BannerDashOptions.SingleBorderUpDown:
                            dach = PromptPlus.Config.Symbols(SymbolType.SingleBorder).value[0];
                            break;
                        case BannerDashOptions.DoubleBorderDown:
                        case BannerDashOptions.DoubleBorderUpDown:
                            dach = PromptPlus.Config.Symbols(SymbolType.DoubleBorder).value[0];
                            break;
                        case BannerDashOptions.HeavyBorderDown:
                        case BannerDashOptions.HeavyBorderUpDown:
                            dach = PromptPlus.Config.Symbols(SymbolType.HeavyBorder).value[0];
                            break;
                        default:
                            break;
                    }
                }
                if (dach.HasValue)
                {
                    _console.Write(new string(dach.Value, _width), Style.Plain.Foreground(localcorlor).Overflow(Overflow.Crop));
                    _console.WriteLine();
                }
            }
        }

        private int WriteStackBar(ChartBarType barType, int inipos, double ticketStep, double totalvalue, int selectedPage, int pagecount, bool isend)
        {
            char charbarOn = '▓';
            var stylepages = new StyleSchema().Tooltips();
            var qtdlines = _console.WriteLine();
            if (_padleft > 0)
            {
                qtdlines += _console.Write(new string(' ', _padleft));
            }
            Style OnStyle = Style.Plain;

            switch (barType)
            {
                case ChartBarType.Fill:
                    {
                        OnStyle.Background(OnStyle.Foreground);
                        if (!_console.IsUnicodeSupported)
                        {
                            charbarOn = _charBar;
                        }
                    }
                    break;
                case ChartBarType.Light:
                    {
                        charbarOn = '─';
                        if (!_console.IsUnicodeSupported)
                        {
                            charbarOn = '-';
                        }
                    }
                    break;
                case ChartBarType.Heavy:
                    {
                        charbarOn = '━';
                        if (!_console.IsUnicodeSupported)
                        {
                            charbarOn = '=';
                        }
                    }
                    break;
                case ChartBarType.Square:
                    {
                        charbarOn = '■';
                        if (!_console.IsUnicodeSupported)
                        {
                            charbarOn = '#';
                        }
                    }
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {barType}");
            }

            foreach (var (_, value, color) in _items)
            {
                OnStyle = Style.Plain.Foreground(color.Value);
                int tkt;
                if (double.IsInfinity(ticketStep))
                {
                    tkt = _width / _items.Count;
                }
                else
                {
                    tkt = (int)(ticketStep * value);
                    if (tkt == 0)
                    {
                        tkt = 1;
                    }
                }
                qtdlines += _console.Write(new string(charbarOn, tkt), OnStyle);
            }

            qtdlines += _console.WriteLine();

            qtdlines += WriteLegends(inipos, totalvalue);

            if (_items.Count > _pagesize && !isend)
            {
                qtdlines += _console.Write(string.Format(Messages.PaginationTemplate, _items.Count, selectedPage + 1, pagecount), stylepages);
                qtdlines += _console.WriteLine();
                qtdlines += _console.Write(DefaultToolTip(), stylepages);
                qtdlines += _console.WriteLine();
            }
            return qtdlines;
        }

        private int WriteStandBar(ChartBarType barType, int inipos, double ticketStep, double totalvalue, int selectedPage,int pagecount, bool isend)
        {
            var stylepages = new StyleSchema().Tooltips();
            var maxlenghtlabel = _items.Max(x => x.label.Length);
            char charbarOn = '▓';
            var qtdlines = _console.WriteLine();
            foreach (var (label, value, color) in _items.Skip(inipos).Take(_pagesize))
            {
                Style OnStyle = Style.Plain.Foreground(color.Value);
                switch (barType)
                {
                    case ChartBarType.Fill:
                        {
                            OnStyle.Background(OnStyle.Foreground);
                            if (!_console.IsUnicodeSupported)
                            {
                                charbarOn = _charBar;
                            }
                        }
                        break;
                    case ChartBarType.Light:
                        {
                            charbarOn = '─';
                            if (!_console.IsUnicodeSupported)
                            {
                                charbarOn = '-';
                            }
                        }
                        break;
                    case ChartBarType.Heavy:
                        {
                            charbarOn = '━';
                            if (!_console.IsUnicodeSupported)
                            {
                                charbarOn = '=';
                            }
                        }
                        break;
                    case ChartBarType.Square:
                        {
                            charbarOn = '■';
                            if (!_console.IsUnicodeSupported)
                            {
                                charbarOn = '#';
                            }
                        }
                        break;
                    default:
                        throw new PromptPlusException($"Not implemented {barType}");
                }

                qtdlines += _console.WriteLine();
                if (_padleft > 0)
                {
                    qtdlines += _console.Write(new string(' ', _padleft));
                }
                var tkt = (int)(ticketStep * value);
                if (tkt == 0)
                {
                    tkt = 1;
                }
                if (_showlegends)
                {
                    qtdlines += _console.Write(new string(charbarOn, tkt), OnStyle);
                    if (_showvalue)
                    {
                        qtdlines += _console.Write(" ");
                        qtdlines += _console.Write($"{ValueToString(value)}", _styleValueChart);
                    }
                    if (_showPercent)
                    {
                        qtdlines += _console.Write(" ");
                        if (_showvalue)
                        {
                            qtdlines += _console.Write($"({ValueToString((100 * value) / totalvalue)}%)", _stylePercentChart);
                        }
                        else
                        {
                            qtdlines += _console.Write($"{ValueToString((100 * value) / totalvalue)}%", _stylePercentChart);
                        }
                    }
                }
                else
                {
                    qtdlines += _console.Write($"{label.PadRight(maxlenghtlabel)}", _styleLabelChart);
                    qtdlines += _console.Write(": ");
                    qtdlines += _console.Write(new string(charbarOn, tkt), OnStyle);
                    if (_showvalue)
                    {
                        qtdlines += _console.Write(" ");
                        qtdlines += _console.Write($"{ValueToString(value)}", _styleValueChart);
                    }
                    if (_showPercent)
                    {
                        qtdlines += _console.Write(" ");
                        if (_showvalue)
                        {
                            qtdlines += _console.Write($"({ValueToString((100 * value) / totalvalue)}%)", _stylePercentChart);
                        }
                        else
                        {
                            qtdlines += _console.Write($"{ValueToString((100 * value) / totalvalue)}%", _stylePercentChart);
                        }
                    }
                }
            }
            if (_showlegends)
            {
                qtdlines += _console.WriteLine();
                qtdlines += WriteLegends(inipos, totalvalue);
            }
            else
            {
                qtdlines += _console.WriteLine();
            }
            if (_items.Count > _pagesize && !isend)
            {
                qtdlines += _console.WriteLine();
                qtdlines += _console.Write(string.Format(Messages.PaginationTemplate, _items.Count, selectedPage + 1, pagecount), stylepages);
                qtdlines += _console.WriteLine();
                qtdlines += _console.Write(DefaultToolTip(), stylepages);
                qtdlines += _console.WriteLine();
            }
            return qtdlines;
        }

        private void ShowChart(double totalvalue, ChartBarType barType)
        {
            var ticketStep = double.Parse(_width.ToString()) / totalvalue;

            if (_chartType == ChartType.StackBar)
            {
                var qtdzerolength = 0;
                foreach (var (label, value, color) in _items)
                {
                    if ((int)(ticketStep * value) == 0)
                    {
                        qtdzerolength++;
                        totalvalue -= value;
                    }
                }
                if (_width < qtdzerolength)
                {
                    _width = qtdzerolength + 1;
                }
                ticketStep = double.Parse((_width - qtdzerolength).ToString()) / totalvalue;
            }

            _console.CursorVisible = false;

            var inipos = 0;
            var seletedpage = 0;
            var pagecount = (_items.Count / _pagesize) + 1;
            var end = _items.Count <= _pagesize;
            var isvalidkey = true;
            var inicursor = _console.CursorTop;
            var endcursor = _console.CursorTop;

            do
            {
                if (isvalidkey)
                {
                    inicursor = _console.CursorTop;
                    var qtdlines = _chartType switch
                    {
                        ChartType.StackBar => WriteStackBar(barType, inipos, ticketStep, totalvalue, seletedpage, pagecount, false),
                        ChartType.StandBar => WriteStandBar(barType, inipos, ticketStep, totalvalue, seletedpage, pagecount, false),
                        _ => throw new PromptPlusException($"Not implemented {_chartType}"),
                    };
                    endcursor = _console.CursorTop;
                    if (_console.IsTerminal && inicursor + qtdlines >= _console.BufferHeight)
                    {
                        var dif = inicursor + qtdlines - _console.BufferHeight;
                        inicursor -= dif;
                    }
                    isvalidkey = false;
                }
                ConsoleKeyInfo keyInfo = new();
                if (_items.Count > _pagesize)
                {
                    keyInfo = _console.ReadKey(true);
                }
                if (keyInfo.IsPressPageUpKey(true))
                {
                    if (inipos - _pagesize >= 0)
                    {
                        inipos -= _pagesize;
                        isvalidkey = true;
                    }
                }
                else if (keyInfo.IsPressPageDownKey(true))
                {
                    if (inipos + _pagesize < _items.Count)
                    {
                        inipos += _pagesize;
                        isvalidkey = true;
                    }
                }
                else if (keyInfo.IsPressUpArrowKey(true))
                {
                    if (inipos - 1 >= 0)
                    {
                        inipos--;
                        isvalidkey = true;
                    }
                }
                else if (keyInfo.IsPressDownArrowKey(true))
                {
                    if (inipos + 1 < _items.Count)
                    {
                        inipos++;
                        isvalidkey = true;
                    }
                }
                else if (keyInfo.IsPressEscKey())
                {
                    end = true;
                    isvalidkey=true;
                }
                if (isvalidkey)
                {
                    _console.SetCursorPosition(0, inicursor);
                    _console.WriteLines(endcursor - inicursor);
                    _console.SetCursorPosition(0, inicursor);
                    seletedpage = inipos / _pagesize;
                    if (end)
                    {
                        switch (_chartType)
                        {
                            case ChartType.StackBar:
                                WriteStackBar(barType, inipos, ticketStep, totalvalue, seletedpage, pagecount, true);
                                break;
                            case ChartType.StandBar:
                                WriteStandBar(barType, inipos, ticketStep, totalvalue, seletedpage, pagecount, true);
                                break;
                            default:
                                throw new PromptPlusException($"Not implemented {_chartType}");
                        }
                    }
                }
            }
            while (!end);
        }

        private int WriteLegends(int inipos, double totalvalue)
        {
            var qtdlines = 0;
            var maxlenghtlabel = _items.Max(x => x.label.Length);

            foreach (var (label, value, color) in _items.Skip(inipos).Take(_pagesize))
            {
                qtdlines += _console.WriteLine();
                if (_padleft > 0)
                {
                    qtdlines += _console.Write(new string(' ', _padleft));
                }
                qtdlines += _console.Write("■ ", Style.Plain.Foreground(color.Value));
                qtdlines += _console.Write($"{label.PadRight(maxlenghtlabel)}", _styleLabelChart);
                if (_showlegendvalue || _showPercent)
                {
                    qtdlines += _console.Write(": ");
                }
                if (_showlegendvalue)
                {
                    qtdlines += _console.Write($"{ValueToString(value)}", _styleValueChart);
                }
                if (_showlegendsperc)
                {
                    qtdlines += _console.Write(" ");
                    if (_showlegendvalue)
                    {
                        if (totalvalue == 0)
                        {
                            qtdlines += _console.Write("(0%)", _stylePercentChart);
                        }
                        else
                        {
                            qtdlines += _console.Write($"({ValueToString((100 * value) / totalvalue)}%)", _stylePercentChart);
                        }
                    }
                    else
                    {
                        if (totalvalue == 0)
                        {
                            qtdlines += _console.Write("0%", _stylePercentChart);
                        }
                        qtdlines += _console.Write($"{ValueToString((100 * value) / totalvalue)}%", _stylePercentChart);
                    }
                }
            }
            qtdlines += _console.WriteLine();
            return qtdlines;
        }


        private string ValueToString(double value)
        {
            var tmp = Math.Round(value, _fracionalDig).ToString(_cultureInfo);
            var decsep = _cultureInfo.NumberFormat.NumberDecimalSeparator;
            var index = tmp.IndexOf(decsep);
            if (index >= 0)
            {
                var dec = tmp[(index + 1)..];
                if (dec.Length > _fracionalDig)
                {
                    dec = dec[.._fracionalDig];
                }
                if (dec.Length < _fracionalDig)
                {
                    dec += new string('0', _fracionalDig - dec.Length);
                }
                if (_fracionalDig > 0)
                {
                    tmp = tmp[..index] + decsep + dec;
                }
                else
                {
                    tmp = tmp[..index];
                }
            }
            else
            {
                if (_fracionalDig > 0)
                {
                    tmp += decsep + new string('0', _fracionalDig);
                }
            }
            return tmp;
        }

        private static string DefaultToolTip()
        {
            return Messages.TooltipChart;
        }

    }
}
