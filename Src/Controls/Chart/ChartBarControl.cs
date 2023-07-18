// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal class ChartBarControl : BaseControl<bool>, IControlChartBar
    {
        private readonly ChartBarOptions _options;
        private int _startpos;
        private int _indexLabel;
        private (int id,int page)[] _paginginfo;

        private double _totalvalue;

        public ChartBarControl(IConsoleControl console, ChartBarOptions options) : base(console, options)
        {
            _options = options;
        }

        public override string InitControl(CancellationToken cancellationToken)
        {
            if (!_options.Labels.Any())
            {
                return string.Empty;
            }

            _startpos = 0;

            _options.CurrentCulture ??= PromptPlus.Config.AppCulture;

            if (_options.CurrentChartType == ChartType.StackBar)
            {
                _options.ShowLegend = true;
            }

            _totalvalue = _options.Labels.Sum(x => x.Value);

            //order items
            ChangeOrder();

            //auto-color
            var indexcolor = 15;
            foreach (var item in _options.Labels)
            {
                if (!item.ColorBar.HasValue)
                {
                    if (Color.FromInt32(indexcolor) == Color.FromConsoleColor(ConsolePlus.BackgroundColor))
                    {
                        indexcolor--;
                        if (indexcolor < 0)
                        {
                            indexcolor = 15;
                        }
                    }
                    item.ColorBar = Color.FromInt32(indexcolor);
                    indexcolor--;
                }
            }

            return string.Empty;
        }

        #region IControlChartBar


        public IControlChartBar Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlChartBar Type(ChartType value)
        {
            _options.StartChartType = value;
            _options.CurrentChartType = value;
            return this;
        }

        public IControlChartBar BarType(ChartBarType value)
        {
            _options.BarType = value;
            return this;
        }

        public IControlChartBar Culture(CultureInfo value)
        {
            _options.CurrentCulture = value;
            return this;
        }

        public IControlChartBar Culture(string value)
        {
            _options.CurrentCulture = new CultureInfo(value);
            return this;
        }

        public IControlChartBar Width(int value)
        {
            _options.Witdth = value;
            return this;
        }

        public IControlChartBar TitleAlignment(Alignment value = Alignment.Left)
        {
            _options.TitleAligment = value;
            return this; ;
        }

        public IControlChartBar Interaction<T1>(IEnumerable<T1> values, Action<IControlChartBar, T1> action)
        {
            foreach (var item in values)
            {
                action.Invoke(this, item);
            }
            return this;
        }

        public IControlChartBar Styles(StyleChart styletype, Style value)
        {
            switch (styletype)
            {
                case StyleChart.Order:
                    _options.OrderStyle = value;
                    break;
                case StyleChart.Title:
                    _options.TitleStyle = value;
                    break;
                case StyleChart.Label:
                    _options.LabelStyle = value;
                    break;
                case StyleChart.Percent:
                    _options.PercentStyle = value;
                    break;
                case StyleChart.Value:
                    _options.ValueStyle = value;
                    break;
                default:
                    throw new PromptPlusException($"StyleChart: {styletype} Not Implemented");
            }
            return this;
        }

        public IControlChartBar AddItem(string label, double value, Color? colorbar = null)
        {
            _options.Labels.Add(new ItemChartBar(++_indexLabel) { Label = label, Value = value, ColorBar = colorbar });
            return this;
        }

        public IControlChartBar FracionalDig(int value)
        {
            if (value < 0)
            {
                value = 0;
            }
            _options.FracionalDig = value;
            return this;
        }

        public IControlChartBar OrderBy(ChartOrder chartOrder)
        {
            _options.CurrentOrder = chartOrder;
            _options.Order = chartOrder;
            return this;
        }

        public IControlChartBar HideOrdination()
        {
            _options.HideInfoOrder= true;
            return this;
        }

        public IControlChartBar HidePercent()
        {
            _options.HidePercentBar = true;
            return this;
        }

        public IControlChartBar HideValue()
        {
            _options.HideValueBar = true;
            return this;
        }

        public IControlChartBar PadLeft(byte value)
        {
            _options.PadLeft = value;
            return this;
        }

        public IControlChartBar ShowLegends(bool withvalue = true, bool withPercent = true)
        {
            _options.CurrentShowLegend = true;
            _options.ShowLegend = true;
            _options.ShowLegendValue = withvalue;
            _options.ShowLegendPercent = withPercent;
            return this;
        }

        public IControlChartBar EnabledInteractionUser(bool switchType = true, bool switchLegend = true, bool switchorder = true, int? pagesize = null)
        {
            _options.EnabledInteractionUser = true;
            _options.EnabledSwitchType = switchType;
            _options.EnabledSwitchLegend = switchLegend;
            _options.EnabledSwitchOrder = switchorder;
            if (pagesize.HasValue)
            {
                if (pagesize.Value < 1)
                {
                    _options.Pagesize = 1;
                }
                else
                {
                    _options.Pagesize = pagesize.Value;
                }
            }
            return this;
        }

        public IControlChartBar HotKeySwitchType(HotKey value)
        {
            _options.SwitchType = value;
            return this;
        }

        public IControlChartBar HotKeySwitchLegend(HotKey value)
        {
            _options.SwitchLegend = value;
            return this;
        }

        public IControlChartBar HotKeySwitchOrder(HotKey value)
        {
            _options.SwitchOrder = value;
            return this;
        }

        #endregion

        public override void FinalizeControl(CancellationToken cancellationToken)
        {
            //none
        }

        public override void InputTemplate(ScreenBuffer screenBuffer)
        {
            if (!_options.EnabledInteractionUser)
            {
                return;
            }
            WriteTitle(screenBuffer);
            switch (_options.CurrentChartType)
            {
                case ChartType.StandBar:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / _options.Labels.Max(x => x.Value);
                        WriteStandBar(screenBuffer, _options.BarType, _startpos, _totalvalue, ticketStep);
                    }
                    break;
                case ChartType.StackBar:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / _totalvalue;
                        WriteStackBar(screenBuffer, _options.BarType, ticketStep);
                    }
                    break;
                default:
                    throw new PromptPlusException($"Show ChartType {_options.CurrentChartType} Not implemented");
            }
            if (_options.CurrentShowLegend)
            {
                WriteLegends(screenBuffer, _startpos, _totalvalue);
            }
            WritePageInfo(screenBuffer);
            if (!_options.HideInfoOrder)
            {
                screenBuffer.AddBuffer(' ', Style.Plain, true);
                screenBuffer.AddBuffer(string.Format(Messages.TooltipOrder, TextOrder(_options.CurrentOrder)), _options.OrderStyle);
            }
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result, bool aborted)
        {
            _options.CurrentChartType = _options.StartChartType;
            if (_options.CurrentOrder != _options.Order)
            {
                _options.CurrentOrder = _options.Order;
                ChangeOrder();
            }
            ShowInitialChart(screenBuffer);
        }


        public override ResultPrompt<bool> TryResult(CancellationToken cancellationToken)
        {
            if (!_options.EnabledInteractionUser)
            {
                return new ResultPrompt<bool>(true, false, false);                         
            }

            var inipos = _startpos;
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
                else if (_options.SwitchType.Equals(keyInfo.Value))
                {
                    if (_options.CurrentChartType == ChartType.StandBar)
                    {
                        _options.CurrentChartType = ChartType.StackBar;
                    }
                    else
                    {
                        _options.CurrentChartType = ChartType.StandBar;
                    }
                    isvalidkey = true;
                }
                else if (_options.SwitchLegend.Equals(keyInfo.Value))
                {
                    if (_options.CurrentChartType == ChartType.StandBar)
                    {
                        _options.CurrentShowLegend = !_options.CurrentShowLegend;
                        isvalidkey = true;
                    }
                }
                else if (_options.SwitchOrder.Equals(keyInfo.Value))
                {
                    var intorder = (int)_options.CurrentOrder;
                    var id = _options.Labels[inipos].Id;
                    intorder++;
                    if (!Enum.IsDefined(typeof(ChartOrder), intorder))
                    {
                        intorder = 0;
                    }
                    _options.CurrentOrder = (ChartOrder)intorder;
                    ChangeOrder();
                    inipos = _options.Labels.FindIndex(x => x.Id == id);
                    isvalidkey = true;
                }
                else if (keyInfo.Value.IsPressPageUpKey(true))
                {
                    if (inipos - _options.Pagesize >= 0)
                    {
                        inipos -= _options.Pagesize;
                        isvalidkey = true;
                    }
                    else
                    {
                        inipos = 0;
                        isvalidkey = true;
                    }
                }
                else if (keyInfo.Value.IsPressPageDownKey(true))
                {
                    if (inipos + _options.Pagesize < _options.Labels.Count)
                    {
                        inipos += _options.Pagesize;
                        isvalidkey = true;
                    }
                    else
                    {
                        inipos = _options.Labels.Count - 1;
                        isvalidkey = true;
                    }
                }
                else if (keyInfo.Value.IsPressUpArrowKey(true))
                {
                    if (inipos - 1 >= 0)
                    {
                        inipos--;
                        isvalidkey = true;
                    }
                }
                else if (keyInfo.Value.IsPressDownArrowKey(true))
                {
                    if (inipos + 1 < _options.Labels.Count)
                    {
                        inipos++;
                        isvalidkey = true;
                    }
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
            if (!abort && !endinput)
            {
                _startpos = inipos;
            }
            return new ResultPrompt<bool>(true,abort,!endinput);
        }

        private string TextOrder(ChartOrder value)
        {
            return value switch
            {
                ChartOrder.None => Messages.OrderStandard,
                ChartOrder.Highest => Messages.OrderHighest,
                ChartOrder.Smallest => Messages.OrderSmallest,
                ChartOrder.LabelAsc => Messages.OrderLabelAsc,
                ChartOrder.LabelDec => Messages.OrderLabelDec,
                _ => throw new PromptPlusException($"ChartOrder {value} Not implemented"),
            };
        }

        private void ShowInitialChart(ScreenBuffer screenBuffer)
        {
            WriteTitle(screenBuffer);
            double totalvalue = _options.Labels.Sum(x => x.Value);
            switch (_options.StartChartType)
            {
                case ChartType.StandBar:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / _options.Labels.Max(x => x.Value);
                        WriteStandBar(screenBuffer, _options.BarType, 0, totalvalue, ticketStep);
                    }
                    break;
                case ChartType.StackBar:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / totalvalue;
                        WriteStackBar(screenBuffer, _options.BarType, ticketStep);
                    }
                    break;
                default:
                    throw new PromptPlusException($"Show ChartType {_options.CurrentChartType} Not implemented");
            }
            if (_options.ShowLegend)
            {
                WriteLegends(screenBuffer, 0, totalvalue);
            }
            if (!_options.HideInfoOrder)
            {
                if (_options.PadLeft > 0)
                {
                    screenBuffer.AddBuffer(new string(' ', _options.PadLeft), Style.Plain, true);
                }
                screenBuffer.AddBuffer(string.Format(Messages.TooltipOrder, TextOrder(_options.CurrentOrder)), _options.OrderStyle);
                screenBuffer.NewLine();
            }
        }

        private void WriteTitle(ScreenBuffer screenBuffer)
        {
            if (string.IsNullOrEmpty(_options.OptPrompt) && string.IsNullOrEmpty(_options.OptDescription))
            {
                return;
            }
            if (!string.IsNullOrEmpty(_options.OptPrompt))
            {
                switch (_options.TitleAligment)
                {
                    case Alignment.Left:
                        screenBuffer.AddBuffer(_options.OptPrompt.PadLeft(_options.OptPrompt.Length + _options.PadLeft), _options.TitleStyle);
                        break;
                    case Alignment.Right:
                        {
                            var aux = _options.OptPrompt;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', _options.Witdth - _options.OptPrompt.Length - _options.PadLeft) + _options.OptPrompt;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length + _options.PadLeft), _options.TitleStyle);
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptPrompt.PadLeft(_options.OptPrompt.Length + _options.PadLeft), _options.TitleStyle);
                            }
                        }
                        break;
                    case Alignment.Center:
                        {
                            var aux = _options.OptPrompt;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', (_options.Witdth - _options.OptPrompt.Length - _options.PadLeft) / 2) + _options.OptPrompt;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length + _options.PadLeft), _options.TitleStyle);
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptPrompt.PadLeft(_options.OptPrompt.Length + _options.PadLeft), _options.TitleStyle);
                            }
                        }
                        break;
                    default:
                        throw new PromptPlusException($"Alignment {_options.TitleAligment} Not implemented");
                }
                screenBuffer.NewLine();
            }
            if (!string.IsNullOrEmpty(_options.OptDescription))
            {
                switch (_options.TitleAligment)
                {
                    case Alignment.Left:
                        screenBuffer.AddBuffer(_options.OptDescription.PadLeft(_options.OptDescription.Length + _options.PadLeft), _options.OptStyleSchema.Description());
                        break;
                    case Alignment.Right:
                        {
                            var aux = _options.OptDescription;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', _options.Witdth - _options.OptDescription.Length - _options.PadLeft) + _options.OptDescription;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length + _options.PadLeft), _options.OptStyleSchema.Description());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptDescription.PadLeft(_options.OptDescription.Length + _options.PadLeft), _options.OptStyleSchema.Description());
                            }
                        }
                        break;
                    case Alignment.Center:
                        {
                            var aux = _options.OptDescription;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', (_options.Witdth - _options.OptDescription.Length - _options.PadLeft) / 2) + _options.OptDescription;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length + _options.PadLeft), _options.OptStyleSchema.Description());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptDescription.PadLeft(_options.OptDescription.Length + _options.PadLeft), _options.OptStyleSchema.Description());
                            }
                        }
                        break;
                    default:
                        throw new PromptPlusException($"Alignment {_options.TitleAligment} Not implemented");
                }
                screenBuffer.NewLine();
            }
        }

        private void WriteStandBar(ScreenBuffer screenBuffer,ChartBarType barType,int inipos,double totalvalue, double ticketStep)
        {
            var pagesize = _options.Pagesize;
            if (!_options.EnabledInteractionUser)
            {
                inipos = 0;
                pagesize = int.MaxValue;
            }
            else
            {
                if (_options.Pagesize >= _options.Labels.Count)
                {
                    inipos = 0;
                }
            }
            var maxlenghtlabel = _options.Labels.Max(x => x.Label.Length);
            char charbarOn = ' ';
            switch (barType)
            {
                case ChartBarType.Fill:
                    {
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = _options.CharBar;
                        }
                    }
                    break;
                case ChartBarType.Light:
                    {
                        charbarOn = '─';
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = '-';
                        }
                    }
                    break;
                case ChartBarType.Heavy:
                    {
                        charbarOn = '━';
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = '=';
                        }
                    }
                    break;
                case ChartBarType.Square:
                    {
                        charbarOn = '■';
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = _options.CharBar;
                        }
                    }
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {barType}");
            }
            foreach (var item in _options.Labels.Skip(inipos).Take(pagesize))
            {
                var OnStyle = Style.Plain.Foreground(item.ColorBar.Value);
                if (barType == ChartBarType.Fill)
                {
                    OnStyle = Style.Plain.Background(item.ColorBar.Value);
                }
                screenBuffer.NewLine();
                if (_options.PadLeft > 0)
                {
                    screenBuffer.AddBuffer(new string(' ',_options.PadLeft), Style.Plain,false,false);
                }
                var tkt = (int)(ticketStep * item.Value);
                if (tkt == 0)
                {
                    tkt = 1;
                }
                if (_options.CurrentShowLegend)
                {
                    screenBuffer.AddBuffer(new string(charbarOn, tkt), OnStyle, false, true);
                }
                else
                {
                    screenBuffer.AddBuffer(item.Label.PadRight(maxlenghtlabel) , _options.LabelStyle);
                    screenBuffer.AddBuffer(": ",Style.Plain, false, false);
                    screenBuffer.AddBuffer(new string(charbarOn, tkt), OnStyle, false, true);
                }
                if (!_options.HideValueBar)
                {
                    screenBuffer.AddBuffer(' ', Style.Plain, false, false);
                    screenBuffer.AddBuffer(ValueToString(item.Value), _options.ValueStyle, false, false);
                }
                if (!_options.HidePercentBar)
                {
                    screenBuffer.AddBuffer(' ', Style.Plain, false, false);
                    if (!_options.HideValueBar)
                    {
                        screenBuffer.AddBuffer($"({ValueToString((100 * item.Value) / totalvalue)}%)", _options.PercentStyle, false, false);
                    }
                    else
                    {
                        screenBuffer.AddBuffer($"{ValueToString((100 * item.Value) / totalvalue)}%", _options.PercentStyle, false, false);
                    }
                }
            }
            screenBuffer.NewLine();
        }

        private void WriteStackBar(ScreenBuffer screenBuffer, ChartBarType barType, double ticketStep)
        {
            char charbarOn = ' ';
            screenBuffer.NewLine();
            if (_options.PadLeft > 0)
            {
                screenBuffer.AddBuffer(new string(' ', _options.PadLeft), Style.Plain, false, false);
            }
            switch (barType)
            {
                case ChartBarType.Fill:
                    {
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = _options.CharBar;
                        }
                    }
                    break;
                case ChartBarType.Light:
                    {
                        charbarOn = '─';
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = '-';
                        }
                    }
                    break;
                case ChartBarType.Heavy:
                    {
                        charbarOn = '━';
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = '=';
                        }
                    }
                    break;
                case ChartBarType.Square:
                    {
                        charbarOn = '■';
                        if (!ConsolePlus.IsUnicodeSupported)
                        {
                            charbarOn = _options.CharBar;
                        }
                    }
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {barType}");
            }

            foreach (var item in _options.Labels)
            {
                var OnStyle = Style.Plain.Foreground(item.ColorBar.Value);
                if (barType == ChartBarType.Fill)
                {
                    OnStyle = Style.Plain.Background(item.ColorBar.Value);
                }
                int tkt;
                if (double.IsInfinity(ticketStep))
                {
                    tkt = _options.Witdth / _options.Labels.Count;
                }
                else
                {
                    tkt = (int)(ticketStep * item.Value);
                    if (tkt == 0)
                    {
                        tkt = 1;
                    }
                }
                screenBuffer.AddBuffer(new string(charbarOn, tkt), OnStyle, false, true);
            }
            screenBuffer.NewLine();
        }

        private void WriteLegends(ScreenBuffer screenBuffer,int inipos, double totalvalue)
        {
            var pagesize = _options.Pagesize;
            if (!_options.EnabledInteractionUser)
            {
                inipos = 0;
                pagesize = int.MaxValue;
            }
            else
            {
                if (_options.Pagesize >= _options.Labels.Count)
                {
                    inipos = 0;
                }
            }

            var maxlenghtlabel = _options.Labels.Max(x => x.Label.Length);

            foreach (var item in _options.Labels.Skip(inipos).Take(pagesize))
            {
                screenBuffer.NewLine();
                if (_options.PadLeft > 0)
                {
                    screenBuffer.AddBuffer(new string(' ', _options.PadLeft),Style.Plain, false, false);
                }
                screenBuffer.AddBuffer("■ ", Style.Plain.Foreground(item.ColorBar.Value), false, false);
                screenBuffer.AddBuffer($"{item.Label.PadRight(maxlenghtlabel)}", _options.LabelStyle);
                if (_options.ShowLegendValue || _options.ShowLegendPercent)
                {
                    screenBuffer.AddBuffer(": ", Style.Plain, false, false);
                }
                if (_options.ShowLegendValue)
                {
                    screenBuffer.AddBuffer(ValueToString(item.Value), _options.ValueStyle);
                }
                if (_options.ShowLegendPercent)
                {
                    screenBuffer.AddBuffer(' ', Style.Plain, false, false);
                    if (_options.ShowLegendValue)
                    {
                        if (totalvalue == 0)
                        {
                            screenBuffer.AddBuffer("(0%)", _options.PercentStyle, false, false);
                        }
                        else
                        {
                            screenBuffer.AddBuffer($"({ValueToString((100 * item.Value) / totalvalue)}%)", _options.PercentStyle, false, false);
                        }
                    }
                    else
                    {
                        if (totalvalue == 0)
                        {
                            screenBuffer.AddBuffer("0%", _options.PercentStyle, false, false);
                        }
                        screenBuffer.AddBuffer($"({ValueToString((100 * item.Value) / totalvalue)}%)", _options.PercentStyle, false, false);
                    }
                }
            }
            screenBuffer.NewLine();
        }

        private void ChangeOrder()
        {
            //order items
            switch (_options.CurrentOrder)
            {
                case ChartOrder.None:
                    _options.Labels = _options.Labels.OrderBy(x => x.Id).ToList();
                    break;
                case ChartOrder.Highest:
                    _options.Labels = _options.Labels.OrderByDescending(x => x.Value).ToList();
                    break;
                case ChartOrder.Smallest:
                    _options.Labels = _options.Labels.OrderBy(x => x.Value).ToList();
                    break;
                case ChartOrder.LabelAsc:
                    _options.Labels = _options.Labels.OrderBy(x => x.Label).ToList();
                    break;
                case ChartOrder.LabelDec:
                    _options.Labels = _options.Labels.OrderByDescending(x => x.Label).ToList();
                    break;
                default:
                    throw new PromptPlusException($"ChartOrder {_options.Order} Not implemented");
            }
            var auxpaginginfo = new List<(int id, int page)>();
            var page = 0;
            var index = 0;
            foreach (var item in _options.Labels)
            {
                index++;
                if (index > _options.Pagesize)
                {
                    index = 1;
                    page++;
                }
                auxpaginginfo.Add(new (item.Id, page));
            }
            _paginginfo = auxpaginginfo.ToArray();
        }

        private void WritePageInfo(ScreenBuffer screenBuffer)
        {
            var defaultcharttip = string.Empty;
            if (_options.Pagesize < _options.Labels.Count)
            {
                defaultcharttip =Messages.TooltipChart;
            }
            if (_options.OptShowTooltip)
            {
                if (_options.OptEnabledAbortKey)
                {
                    if (_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == ChartType.StandBar))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder,_options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }

                        }
                    }
                    else if (_options.EnabledSwitchType && (!_options.EnabledSwitchLegend || _options.CurrentChartType == ChartType.StackBar))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                    }
                    else if (!_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == ChartType.StandBar))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),    
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter), _options.OptStyleSchema.Tooltips());
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else 
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip), _options.OptStyleSchema.Tooltips());
                            }
                        }
                    }
                }
                else
                {
                    if (_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == ChartType.StandBar))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                    }
                    else if (_options.EnabledSwitchType && (!_options.EnabledSwitchLegend || _options.CurrentChartType == ChartType.StackBar))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                    }
                    else if (!_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == ChartType.StandBar))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.OptStyleSchema.Tooltips());
                            }
                            else 
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.InputFisnishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.OptStyleSchema.Tooltips());
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            screenBuffer.AddBuffer(string.Format("{0}, {1}",
                                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                Messages.InputFisnishEnter), _options.OptStyleSchema.Tooltips());
                        }
                        else
                        {
                            screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                Messages.InputFisnishEnter,
                                defaultcharttip), _options.OptStyleSchema.Tooltips());
                        }
                    }
                }
            }
            if (_options.Pagesize >= _options.Labels.Count)
            {
                return;
            }
            var selectedPage = _paginginfo.First(x => x.id == _options.Labels[_startpos].Id).page;
            var pagecount = (_options.Labels.Count / _options.Pagesize) + 1;
            if (_options.OptShowTooltip)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer(string.Format(Messages.PaginationTemplate, _options.Labels.Count, selectedPage + 1, pagecount), _options.OptStyleSchema.Pagination());
        }

        private string ValueToString(double value)
        {
            var tmp = Math.Round(value, _options.FracionalDig).ToString(_options.CurrentCulture);
            var decsep = _options.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var index = tmp.IndexOf(decsep);
            if (index >= 0)
            {
                var dec = tmp[(index + 1)..];
                if (dec.Length > _options.FracionalDig)
                {
                    dec = dec[.._options.FracionalDig];
                }
                if (dec.Length < _options.FracionalDig)
                {
                    dec += new string('0', _options.FracionalDig - dec.Length);
                }
                if (_options.FracionalDig > 0)
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
                if (_options.FracionalDig > 0)
                {
                    tmp += decsep + new string('0', _options.FracionalDig);
                }
            }
            return tmp;
        }
    }
}
