﻿// ***************************************************************************************
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

            _options.CurrentCulture ??= _options.Config.AppCulture;

            if (_options.CurrentChartType == LayoutChart.Stacked)
            {
                _options.ShowLegend = true;
            }

            _totalvalue = _options.Labels.Sum(x => Math.Round(x.Value, _options.FracionalDig));

            //order items
            ChangeOrder();

            //auto-color
            var indexcolor = 15;
            foreach (var item in _options.Labels.Where(x => !x.ColorBar.HasValue))
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

            return string.Empty;
        }

        #region IControlChartBar


        public IControlChartBar Config(Action<IPromptConfig> context)
        {
            context?.Invoke(_options);
            return this;
        }

        public IControlChartBar Layout(LayoutChart value)
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
            if (value < 10)
            {
                throw new PromptPlusException("Width must be greater than or equal to 10");
            }
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

        public IControlChartBar Styles(ChartStyles content, Style value)
        {
            _options.StyleControl(content, value);
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
                throw new PromptPlusException("FracionalDig must be greater than or equal to 0");
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

        public IControlChartBar HideOrdination(bool value = true)
        {
            _options.HideInfoOrder= value;
            return this;
        }

        public IControlChartBar HidePercent(bool value = true)
        {   
            _options.HidePercentBar = value;
            return this;
        }

        public IControlChartBar HideValue(bool value = true)
        {
            _options.HideValueBar = value;
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

        public IControlChartBar EnabledInteractionUser(bool switchType = true, bool switchLegend = true, bool switchorder = true)
        {
            _options.EnabledInteractionUser = true;
            _options.EnabledSwitchType = switchType;
            _options.EnabledSwitchLegend = switchLegend;
            _options.EnabledSwitchOrder = switchorder;
            return this;
        }
        public IControlChartBar PageSize(int value)
        {
            if (value < 1)
            {
                throw new PromptPlusException("PageSize must be greater than or equal to 1");
            }
            _options.PageSize = value;
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
                case LayoutChart.Standard:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / _options.Labels.Max(x => x.Value);
                        WriteStandBar(screenBuffer, _options.BarType, _startpos, ticketStep);
                    }
                    break;
                case LayoutChart.Stacked:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / _totalvalue;
                        WriteStackBar(screenBuffer, _options.BarType, ticketStep);
                    }
                    break;
                default:
                    throw new PromptPlusException($"Show ChartType {_options.CurrentChartType} Not implemented");
            }
            if (!_options.HideInfoOrder && !_options.OptMinimalRender)
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(string.Format(Messages.TooltipOrder, TextOrder(_options.CurrentOrder)), _options.StyleContent(StyleControls.ChartOrder));
            }
            if (_options.CurrentShowLegend)
            {
                WriteLegends(screenBuffer, _startpos);
            }
            WritePageInfo(screenBuffer);
        }

        public override void FinishTemplate(ScreenBuffer screenBuffer, bool result, bool aborted)
        {
            if (_options.OptMinimalRender && _options.EnabledInteractionUser)
            {
                return;
            }
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
                    if (_options.CurrentChartType == LayoutChart.Standard)
                    {
                        _options.CurrentChartType = LayoutChart.Stacked;
                    }
                    else
                    {
                        _options.CurrentChartType = LayoutChart.Standard;
                    }
                    isvalidkey = true;
                }
                else if (_options.SwitchLegend.Equals(keyInfo.Value))
                {
                    if (_options.CurrentChartType == LayoutChart.Standard)
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
                    if (inipos - _options.PageSize >= 0)
                    {
                        inipos -= _options.PageSize;
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
                    if (inipos + _options.PageSize < _options.Labels.Count)
                    {
                        inipos += _options.PageSize;
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
            if (!string.IsNullOrEmpty(ValidateError) || endinput)
            {
                ClearBuffer();
            }
            var notrender = false;
            if (KeyAvailable)
            {
                notrender = true;
            }
            return new ResultPrompt<bool>(true,abort,!endinput,notrender);
        }

        private static string TextOrder(ChartOrder value)
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
            switch (_options.StartChartType)
            {
                case LayoutChart.Standard:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / _options.Labels.Max(x => x.Value);
                        WriteStandBar(screenBuffer, _options.BarType, 0, ticketStep);
                    }
                    break;
                case LayoutChart.Stacked:
                    {
                        var ticketStep = double.Parse(_options.Witdth.ToString()) / _totalvalue;
                        WriteStackBar(screenBuffer, _options.BarType, ticketStep);
                    }
                    break;
                default:
                    throw new PromptPlusException($"Show ChartType {_options.CurrentChartType} Not implemented");
            }
            if (!_options.HideInfoOrder)
            {
                screenBuffer.NewLine();
                 screenBuffer.AddBuffer(string.Format(Messages.TooltipOrder, TextOrder(_options.CurrentOrder)), _options.StyleContent(StyleControls.ChartOrder));
            }
            if (_options.ShowLegend)
            {
                WriteLegends(screenBuffer, 0);
            }
            screenBuffer.NewLine();
        }

        private void WriteTitle(ScreenBuffer screenBuffer)
        {
            if (_options.OptMinimalRender)
            {
                return;
            }
            if (!string.IsNullOrEmpty(_options.OptPrompt))
            {
                switch (_options.TitleAligment)
                {
                    case Alignment.Left:
                        screenBuffer.AddBuffer(_options.OptPrompt.PadLeft(_options.OptPrompt.Length), _options.StyleContent(StyleControls.ChartTitle));
                        break;
                    case Alignment.Right:
                        {
                            var aux = _options.OptPrompt;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', _options.Witdth - _options.OptPrompt.Length) + _options.OptPrompt;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length), _options.StyleContent(StyleControls.ChartTitle));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptPrompt.PadLeft(_options.OptPrompt.Length), _options.StyleContent(StyleControls.ChartTitle));
                            }
                        }
                        break;
                    case Alignment.Center:
                        {
                            var aux = _options.OptPrompt;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', (_options.Witdth - _options.OptPrompt.Length) / 2) + _options.OptPrompt;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length), _options.StyleContent(StyleControls.ChartTitle));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptPrompt.PadLeft(_options.OptPrompt.Length), _options.StyleContent(StyleControls.ChartTitle));
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
                        screenBuffer.AddBuffer(_options.OptDescription.PadLeft(_options.OptDescription.Length), _options.StyleContent(StyleControls.Description));
                        break;
                    case Alignment.Right:
                        {
                            var aux = _options.OptDescription;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', _options.Witdth - _options.OptDescription.Length) + _options.OptDescription;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length), _options.StyleContent(StyleControls.Description));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptDescription.PadLeft(_options.OptDescription.Length), _options.StyleContent(StyleControls.Description));
                            }
                        }
                        break;
                    case Alignment.Center:
                        {
                            var aux = _options.OptDescription;
                            if (aux.Length < _options.Witdth)
                            {
                                aux = new string(' ', (_options.Witdth - _options.OptDescription.Length) / 2) + _options.OptDescription;
                                screenBuffer.AddBuffer(aux.PadLeft(aux.Length), _options.StyleContent(StyleControls.Description));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(_options.OptDescription.PadLeft(_options.OptDescription.Length), _options.StyleContent(StyleControls.Description));
                            }
                        }
                        break;
                    default:
                        throw new PromptPlusException($"Alignment {_options.TitleAligment} Not implemented");
                }
            }
        }

        private void WriteStandBar(ScreenBuffer screenBuffer,ChartBarType barType,int inipos,double ticketStep)
        {
            var pagesize = _options.PageSize;
            if (!_options.EnabledInteractionUser)
            {
                inipos = 0;
                pagesize = int.MaxValue;
            }
            else
            {
                if (_options.PageSize >= _options.Labels.Count)
                {
                    inipos = 0;
                }
            }
            var maxlengthlabel = _options.Labels.Max(x => x.Label.Length);
            char barOn = ' ';
            switch (barType)
            {
                case ChartBarType.Fill:
                    break;
                case ChartBarType.Light:
                    barOn = _options.Symbol(SymbolType.ChartLight)[0];
                    break;
                case ChartBarType.Heavy:
                    barOn = _options.Symbol(SymbolType.ChartHeavy)[0];
                    break;
                case ChartBarType.Square:
                    barOn = _options.Symbol(SymbolType.ChartSquare)[0];
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {barType}");
            }
            var first = true;
            foreach (var item in _options.Labels.Skip(inipos).Take(pagesize))
            {
                var OnStyle = Style.Default.Foreground(item.ColorBar.Value);
                if (barType == ChartBarType.Fill)
                {
                    OnStyle = Style.Default.Background(item.ColorBar.Value);
                }
                if (!_options.OptMinimalRender)
                {
                    if (!(string.IsNullOrEmpty(_options.OptPrompt) && string.IsNullOrEmpty(_options.OptDescription)))
                    {
                        screenBuffer.NewLine();
                    }
                }
                else
                {
                    if (!first)
                    {
                        screenBuffer.NewLine();
                    }
                    else
                    {
                        first = false;
                    }
                }
                var tkt = (int)(ticketStep * item.Value);
                if (tkt == 0)
                {
                    tkt = 1;
                }
                if (_options.CurrentShowLegend)
                {
                    screenBuffer.AddBuffer(new string(barOn, tkt), OnStyle, false, true);
                }
                else
                {
                    screenBuffer.AddBuffer(item.Label.PadRight(maxlengthlabel) , _options.StyleContent(ChartStyles.ChartLabel));
                    screenBuffer.AddBuffer(": ",Style.Default, false, false);
                    screenBuffer.AddBuffer(new string(barOn, tkt), OnStyle, false, true);
                }
                if (!_options.HideValueBar)
                {
                    screenBuffer.AddBuffer(' ', Style.Default, false, false);
                    screenBuffer.AddBuffer(ValueToString(item.Value), _options.StyleContent(ChartStyles.ChartValue), false, false);
                }
                if (!_options.HidePercentBar)
                {
                    screenBuffer.AddBuffer(' ', Style.Default, false, false);
                    if (!_options.HideValueBar)
                    {
                        screenBuffer.AddBuffer($"({ValueToString((100 * item.Value) / _totalvalue)}%)", _options.StyleContent(ChartStyles.ChartPercent), false, false);
                    }
                    else
                    {
                        screenBuffer.AddBuffer($"{ValueToString((100 * item.Value) / _totalvalue)}%", _options.StyleContent(ChartStyles.ChartPercent), false, false);
                    }
                }
            }
        }

        private void WriteStackBar(ScreenBuffer screenBuffer, ChartBarType barType, double ticketStep)
        {
            char barOn = ' ';
            if (!_options.OptMinimalRender)
            {
                if (!(string.IsNullOrEmpty(_options.OptPrompt) && string.IsNullOrEmpty(_options.OptDescription)))
                {
                    screenBuffer.NewLine();
                }
            }
            else
            {
                screenBuffer.NewLine();
            }
            switch (barType)
            {
                case ChartBarType.Fill:
                    break;
                case ChartBarType.Light:
                    barOn = _options.Symbol(SymbolType.ChartLight)[0];
                    break;
                case ChartBarType.Heavy:
                    barOn = _options.Symbol(SymbolType.ChartHeavy)[0];
                    break;
                case ChartBarType.Square:
                    barOn = _options.Symbol(SymbolType.ChartSquare)[0];
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {barType}");
            }

            foreach (var item in _options.Labels)
            {
                var OnStyle = Style.Default.Foreground(item.ColorBar.Value);
                if (barType == ChartBarType.Fill)
                {
                    OnStyle = Style.Default.Background(item.ColorBar.Value);
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
                screenBuffer.AddBuffer(new string(barOn, tkt), OnStyle, false, true);
            }
        }

        private void WriteLegends(ScreenBuffer screenBuffer,int inipos)
        {
            var pagesize = _options.PageSize;
            if (!_options.EnabledInteractionUser)
            {
                inipos = 0;
                pagesize = int.MaxValue;
            }
            else
            {
                if (_options.PageSize >= _options.Labels.Count)
                {
                    inipos = 0;
                }
            }

            var maxlengthlabel = _options.Labels.Max(x => x.Label.Length);

            foreach (var item in _options.Labels.Skip(inipos).Take(pagesize))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(_options.Symbol(SymbolType.ChartLabel), _options.StyleContent(StyleControls.ChartLabel).Foreground(item.ColorBar.Value), false, false);
                screenBuffer.AddBuffer(" ", _options.StyleContent(StyleControls.ChartLabel), false, false);
                screenBuffer.AddBuffer($"{item.Label.PadRight(maxlengthlabel)}", _options.StyleContent(StyleControls.ChartLabel));
                if (_options.ShowLegendValue || _options.ShowLegendPercent)
                {
                    screenBuffer.AddBuffer(": ", _options.StyleContent(StyleControls.ChartLabel), false, false);
                }
                if (_options.ShowLegendValue)
                {
                    screenBuffer.AddBuffer(ValueToString(item.Value), _options.StyleContent(StyleControls.ChartValue));
                }
                if (_options.ShowLegendPercent)
                {
                    screenBuffer.AddBuffer(' ', _options.StyleContent(StyleControls.ChartPercent), false, false);
                    if (_options.ShowLegendValue)
                    {
                        if (_totalvalue == 0)
                        {
                            screenBuffer.AddBuffer("(0%)", _options.StyleContent(StyleControls.ChartPercent), false, false);
                        }
                        else
                        {
                            screenBuffer.AddBuffer($"({ValueToString((100 * item.Value) / _totalvalue)}%)", _options.StyleContent(StyleControls.ChartPercent), false, false);
                        }
                    }
                    else
                    {
                        if (_totalvalue == 0)
                        {
                            screenBuffer.AddBuffer("0%", _options.StyleContent(StyleControls.ChartPercent), false, false);
                        }
                        screenBuffer.AddBuffer($"({ValueToString((100 * item.Value) / _totalvalue)}%)", _options.StyleContent(StyleControls.ChartPercent), false, false);
                    }
                }
            }
        }

        private void ChangeOrder()
        {
            //order items
            _options.Labels = _options.CurrentOrder switch
            {
                ChartOrder.None => _options.Labels.OrderBy(x => x.Id).ToList(),
                ChartOrder.Highest => _options.Labels.OrderByDescending(x => x.Value).ToList(),
                ChartOrder.Smallest => _options.Labels.OrderBy(x => x.Value).ToList(),
                ChartOrder.LabelAsc => _options.Labels.OrderBy(x => x.Label).ToList(),
                ChartOrder.LabelDec => _options.Labels.OrderByDescending(x => x.Label).ToList(),
                _ => throw new PromptPlusException($"ChartOrder {_options.Order} Not implemented"),
            };
            var auxpaginginfo = new List<(int id, int page)>();
            var page = 0;
            var index = 0;
            foreach (var item in _options.Labels)
            {
                index++;
                if (index > _options.PageSize)
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
            if (_options.PageSize < _options.Labels.Count)
            {
                var selectedPage = _paginginfo.First(x => x.id == _options.Labels[_startpos].Id).page;
                var pagecount = (_options.Labels.Count / _options.PageSize) + 1;
                if (_options.OptShowTooltip)
                {
                    screenBuffer.NewLine();
                }
                if (_options.OptMinimalRender)
                {
                    screenBuffer.AddBuffer(_options.OptPaginationTemplate(_options.Labels.Count, selectedPage + 1, pagecount), _options.StyleContent(StyleControls.Pagination));
                }
                else
                {
                    if (_options.OptPaginationTemplate != null)
                    {
                        screenBuffer.AddBuffer(_options.OptPaginationTemplate(_options.Labels.Count, selectedPage + 1, pagecount), _options.StyleContent(StyleControls.Pagination));
                    }
                    else
                    {
                        screenBuffer.AddBuffer(string.Format(Messages.PaginationTemplate, _options.Labels.Count, selectedPage + 1, pagecount), _options.StyleContent(StyleControls.Pagination));
                    }
                }
            }
            var defaultcharttip = string.Empty;
            if (_options.PageSize < _options.Labels.Count)
            {
                defaultcharttip =Messages.TooltipChart;
            }
            if (_options.OptShowTooltip)
            {
                screenBuffer.NewLine();
                if (_options.OptEnabledAbortKey)
                {
                    if (_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == LayoutChart.Standard))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder,_options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
                            }

                        }
                    }
                    else if (_options.EnabledSwitchType && (!_options.EnabledSwitchLegend || _options.CurrentChartType == LayoutChart.Stacked))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                    }
                    else if (!_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == LayoutChart.Standard))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),    
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
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
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else 
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, _options.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                    }
                }
                else
                {
                    if (_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == LayoutChart.Standard))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                    }
                    else if (_options.EnabledSwitchType && (!_options.EnabledSwitchLegend || _options.CurrentChartType == LayoutChart.Stacked))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchType, _options.SwitchType)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                    }
                    else if (!_options.EnabledSwitchType && (_options.EnabledSwitchLegend && _options.CurrentChartType == LayoutChart.Standard))
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                        else
                        {
                            if (_options.EnabledSwitchOrder)
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend),
                                    string.Format(Messages.TooltipChartSwitchOrder, _options.SwitchOrder)), _options.StyleContent(StyleControls.Tooltips));
                            }
                            else 
                            {
                                screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    defaultcharttip,
                                    string.Format(Messages.TooltipChartSwitchLegend, _options.SwitchLegend)), _options.StyleContent(StyleControls.Tooltips));
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(defaultcharttip))
                        {
                            screenBuffer.AddBuffer(string.Format("{0}, {1}",
                                string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                Messages.InputFinishEnter), _options.StyleContent(StyleControls.Tooltips));
                        }
                        else
                        {
                            screenBuffer.AddBuffer(string.Format("{0}, {1}, {2}",
                                string.Format(Messages.TooltipToggle, _options.Config.TooltipKeyPress),
                                Messages.InputFinishEnter,
                                defaultcharttip), _options.StyleContent(StyleControls.Tooltips));
                        }
                    }
                }
            }
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
