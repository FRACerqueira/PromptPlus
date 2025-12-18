// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary.Controls.ChartBar
{
    internal sealed class ChartBarControl : BaseControlPrompt<ChartItem?>, IChartBarControl, IChartBarWidget
    {
        private CultureInfo _culture;
        private readonly Dictionary<ChartBarStyles, Style> _optStyles = BaseControlOptions.LoadStyle<ChartBarStyles>();
        private List<ChartItem> _items = [];
        private ChartBarType _chartBarType = ChartBarType.Fill;
        private ChartBarLayout _layout = ChartBarLayout.Standard;
        private ChartBarOrder _order = ChartBarOrder.None;
        private Func<ChartItem, string>? _changeDescription;
        private Func<ChartItem, (bool, string?)>? _predicatevalidselect;
        private bool _hasLegends;
        private bool _showLegends;
        private byte _width;
        private byte _fractionalDigits = 2;
        private int _startpage;
        private int _indexitem;
        private (string id, int page)[] _paginginfo = [];
        private double _totalvalue;
        private int _pageSize;
        private HideChart _hideChart = HideChart.None;
        private readonly List<string> _toggerTooptips = [];
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private ChartItem? _currentitem;
        private int _sequence;
        private string? _title;
        private TextAlignment _titleAlignment = TextAlignment.Center;
        private char _barOn = ' ';
        private double _ticketStep;
        private int _maxlengthlabel;
        private int _maxShowlengthlabel;
        private EmacsBuffer? _answerBuffer;
        private int _maxWidth;

        public void InternalTitle(string title, TextAlignment alignment)
        {
            _title = title;
            _titleAlignment = alignment;
        }

        public void InternalShowLegends(bool value)
        {
            _hasLegends = value;
            _showLegends = value;
        }

        public ChartBarControl(bool isWidget, IConsoleExtend console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _culture = ConfigPlus.DefaultCulture;
            _pageSize = ConfigPlus.PageSize;
            _width = ConfigPlus.ChartWidth;
            _maxWidth = ConfigPlus.MaxWidth;
            _maxShowlengthlabel = 20;
        }

        #region IChartBar

        IChartBarControl IChartBarControl.MaxWidth(byte maxWidth)
        {
            if (maxWidth < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxWidth), "MaxWidth must be greater than or equal to 1.");
            }
            _maxWidth = maxWidth;
            return this;
        }

        IChartBarControl IChartBarControl.MaxLengthLabel(byte value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "MaxLengthLabel must be greater than 0");
            }
            _maxShowlengthlabel = value;
            return this;
        }

        IChartBarWidget IChartBarWidget.Layout(ChartBarLayout layout)
        {
            Layout(layout);
            return this;
        }

        IChartBarWidget IChartBarWidget.Culture(CultureInfo culture)
        {
            Culture(culture);
            return this;
        }

        IChartBarWidget IChartBarWidget.BarType(ChartBarType type)
        {
            BarType(type);
            return this;
        }

        IChartBarWidget IChartBarWidget.Width(byte value)
        {
            Width(value);
            return this;
        }

        IChartBarWidget IChartBarWidget.Styles(ChartBarStyles styleType, Style style)
        {
            Styles(styleType, style);
            return this;
        }

        IChartBarWidget IChartBarWidget.AddItem(string label, double value, Color? colorBar, string? id)
        {
            AddItem(label, value, colorBar, id);
            return this;
        }

        IChartBarWidget IChartBarWidget.Interaction<T>(IEnumerable<T> items, Action<T, IChartBarWidget> interactionaction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionaction);

            foreach (T? item in items)
            {
                interactionaction.Invoke(item, this);
            }
            return this;
        }

        IChartBarWidget IChartBarWidget.FractionalDigits(byte value)
        {
            FractionalDigits(value);
            return this;
        }

        IChartBarWidget IChartBarWidget.OrderBy(ChartBarOrder order)
        {
            OrderBy(order);
            return this;
        }

        IChartBarWidget IChartBarWidget.HideElements(HideChart value)
        {
            HideElements(value);
            return this;
        }

        public IChartBarControl Title(string title, TextAlignment alignment = TextAlignment.Center)
        {
            _title = title ?? throw new ArgumentNullException(nameof(title), "Title cannot be null");
            _titleAlignment = alignment;
            return this;
        }

        public IChartBarControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IChartBarControl HideElements(HideChart value)
        {
            _hideChart = value;
            return this;
        }

        public IChartBarControl AddItem(string label, double value, Color? colorBar = null, string? id = null)
        {
            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentException("Label cannot be null or empty", nameof(label));
            }
            _sequence++;
            string idlist = $"{_sequence:D5}|{id ?? string.Empty}";
            _items.Add(new ChartItem(idlist, label, value, colorBar));
            return this;
        }

        public IChartBarControl ChangeDescription(Func<ChartItem, string> value)
        {
            if (IsWidgetControl)
            {
                throw new InvalidOperationException("PageSize not available in WidgetControl");
            }
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            return this;
        }

        public IChartBarControl BarType(ChartBarType type = ChartBarType.Fill)
        {
            _chartBarType = type;
            return this;
        }

        public IChartBarControl Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        public IChartBarControl Interaction<T>(IEnumerable<T> items, Action<T, IChartBarControl> interactionaction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionaction);

            foreach (T? item in items)
            {
                interactionaction.Invoke(item, this);
            }
            return this;
        }

        public IChartBarControl Layout(ChartBarLayout layout = ChartBarLayout.Standard)
        {
            _layout = layout;
            return this;
        }

        public IChartBarControl OrderBy(ChartBarOrder order)
        {
            _order = order;
            return this;
        }

        public IChartBarControl ShowLegends(bool value = true)
        {
            _hasLegends = value;
            _showLegends = value;
            return this;
        }

        public IChartBarControl Styles(ChartBarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IChartBarControl PredicateSelected(Func<ChartItem, bool> validselect)
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

        public IChartBarControl PredicateSelected(Func<ChartItem, (bool, string?)> validselect)
        {
            ArgumentNullException.ThrowIfNull(validselect);
            _predicatevalidselect = validselect;
            return this;
        }

        public IChartBarControl Width(byte value)
        {
            if (value < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater or equal than 10");
            }
            _width = value;
            return this;
        }

        public IChartBarControl FractionalDigits(byte value)
        {
            _fractionalDigits = value;
            return _fractionalDigits > 5 ? throw new ArgumentOutOfRangeException(nameof(value), "FracionalDig must be less than 5") : (IChartBarControl)this;
        }

        public IChartBarControl PageSize(byte value)
        {
            if (IsWidgetControl)
            {
                throw new InvalidOperationException("PageSize not available in WidgetControl");
            }
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "PageSize must be greater or equal than 1");
            }
            _pageSize = value;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_maxWidth > _width)
            {
                _maxWidth = _width;
            }
            _answerBuffer = new(true, CaseOptions.Any, (_) => true,int.MaxValue, _maxWidth);
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("No items to show");
            }
            if (IsWidgetControl)
            {
                _pageSize = int.MaxValue;
            }
            double maxValue = _items.Max(x => x.Value);
            _ticketStep = maxValue == 0 ? 1 : _width / maxValue;
            _maxlengthlabel = _items.Max(x => x.Label.Length);
            if (IsWidgetControl)
            {
                _maxShowlengthlabel = int.MaxValue;
            }
            if (_maxlengthlabel > _maxShowlengthlabel && !IsWidgetControl)
            {
                _maxlengthlabel = _maxShowlengthlabel;
            }
            switch (_chartBarType)
            {
                case ChartBarType.Fill:
                    break;
                case ChartBarType.Light:
                    _barOn = ConfigPlus.GetSymbol(SymbolType.ChartLight)[0];
                    break;
                case ChartBarType.Square:
                    _barOn = ConfigPlus.GetSymbol(SymbolType.ChartSquare)[0];
                    break;
                default:
                    throw new NotImplementedException($"Not implemented {_chartBarType}");
            }
            _totalvalue = _items.Sum(x => Math.Round(x.Value, _fractionalDigits));
            //order items
            ChangeOrder();

            //auto-color
            int indexcolor = 15;
            foreach (ChartItem item in _items)
            {
                if (!item.Color.HasValue)
                {
                    if (Color.FromInt32(indexcolor) == Color.FromConsoleColor(ConsolePlus.BackgroundColor))
                    {
                        indexcolor--;
                        if (indexcolor < 0)
                        {
                            indexcolor = 15;
                        }
                    }
                    item.Color = Color.FromInt32(indexcolor);
                    indexcolor--;
                }
                item.Percent = Math.Round((100 * item.Value) / _totalvalue, _fractionalDigits);
                item.StyleBar = _chartBarType == ChartBarType.Fill
                    ? new Style(item.Color.Value, item.Color.Value)
                    : Style.Default().ForeGround(item.Color.Value);
            }
            _currentitem = _items.FirstOrDefault();
            if (_currentitem != null)
            {
                var answer = _currentitem!.Label;
                if (_layout == ChartBarLayout.Stacked && !_showLegends)
                {
                    answer = $"{_currentitem!.Label}: {ValueToString(_currentitem!.Value)}({ValueToString(_currentitem.Percent)}%)";
                }
                _answerBuffer!.LoadPrintable(answer);
                _answerBuffer.ToHome();
            }
            LoadTooltipToggle();
            _tooltipModeInput = GetTooltipModeInput();
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!IsWidgetControl)
            {
                WritePrompt(screenBuffer);

                WriteAnswer(screenBuffer);

                WriteDescription(screenBuffer);
            }

            WriteTitle(screenBuffer);

            WriteChartBar(screenBuffer);

            WriteLegends(screenBuffer);

            if (!_hideChart.HasFlag(HideChart.Ordering) && !IsWidgetControl)
            {
                screenBuffer.WriteLine(string.Format(Messages.TooltipOrder, TextOrder(_order)), _optStyles[ChartBarStyles.ChartOrder]);
            }

            if (!IsWidgetControl)
            {
                if (_pageSize < _items.Count)
                {
                    string template = ConfigPlus.PaginationTemplate.Invoke(
                        _items.Count,
                        _startpage + 1,
                        _items.Count / _pageSize + 1
                    )!;
                    screenBuffer.WriteLine(template, _optStyles[ChartBarStyles.Pagination]);
                }
                WriteTooltip(screenBuffer);
            }
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsolePlus.CursorVisible;
            ConsolePlus.CursorVisible = true;
            bool updateposanswer = false;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    updateposanswer = false;
                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltip

                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<ChartItem?>(_currentitem, true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<ChartItem?>(_currentitem, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        (bool ok, string? message) = _predicatevalidselect?.Invoke(_currentitem!) ?? (true, null);
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
                        ResultCtrl = new ResultPrompt<ChartItem?>(_currentitem, false);
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Count)
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

                    else if (ConfigPlus.HotKeyTooltipChartBarSwitchLayout.Equals(keyinfo) && !_hideChart.HasFlag(HideChart.Layout))
                    {
                        _layout = _layout == ChartBarLayout.Standard ? ChartBarLayout.Stacked : ChartBarLayout.Standard;
                        _indexitem = 0;
                        _startpage = 0;
                        _currentitem = _items[_indexitem];
                        updateposanswer = true;
                        break;
                    }
                    else if (ConfigPlus.HotKeyTooltipChartBarSwitchLegend.Equals(keyinfo) && _hasLegends)
                    {
                        _showLegends = !_showLegends;
                        break;
                    }
                    else if (ConfigPlus.HotKeyTooltipChartBarSwitchOrder.Equals(keyinfo) && !_hideChart.HasFlag(HideChart.Ordering))
                    {
                        int intorder = (int)_order;
                        intorder++;
                        if (!Enum.IsDefined(typeof(ChartBarOrder), intorder))
                        {
                            intorder = 0;
                        }
                        _order = (ChartBarOrder)intorder;
                        ChangeOrder();
                        _startpage = _paginginfo.First(x => x.id == _currentitem!.Id).page;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlHomeKey())
                    {
                        _indexitem = 0;
                        _currentitem = _items[_indexitem];
                        _startpage = _paginginfo.First(x => x.id == _currentitem!.Id).page;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressCtrlEndKey())
                    {
                        _indexitem = _items.Count - 1;
                        _currentitem = _items[_indexitem];
                        _startpage = _paginginfo.First(x => x.id == _currentitem!.Id).page;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageUpKey())
                    {
                        if (_indexitem == 0)
                        {
                            _indexitem = _items.Count - 1;
                        }
                        else
                        {
                            _indexitem -= _pageSize;
                            if (_indexitem < 0)
                            {
                                _indexitem = 0;
                            }
                        }
                        _currentitem = _items[_indexitem];
                        updateposanswer = true;
                        _startpage = _paginginfo.First(x => x.id == _currentitem!.Id).page;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressPageDownKey())
                    {
                        if (_indexitem == _items.Count - 1)
                        {
                            _indexitem = 0;
                        }
                        else
                        {
                            _indexitem += _pageSize;
                            if (_indexitem > _items.Count - 1)
                            {
                                _indexitem = _items.Count - 1;
                            }
                        }
                        _currentitem = _items[_indexitem];
                        updateposanswer = true;
                        _startpage = _paginginfo.First(x => x.id == _currentitem!.Id).page;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_indexitem - 1 >= 0)
                        {
                            _indexitem--;
                        }
                        else
                        {
                            _indexitem = _items.Count - 1;
                        }
                        _currentitem = _items[_indexitem];
                        updateposanswer = true;
                        _startpage = _paginginfo.First(x => x.id == _currentitem!.Id).page;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressDownArrowKey())
                    {
                        if (_indexitem + 1 < _items.Count)
                        {
                            _indexitem++;
                        }
                        else
                        {
                            _indexitem = 0;
                        }
                        _currentitem = _items[_indexitem];
                        updateposanswer = true;
                        _startpage = _paginginfo.First(x => x.id == _currentitem!.Id).page;
                        _indexTooptip = 0;
                        break;
                    }
                    else if (!_answerBuffer!.IsPrintable(keyinfo.KeyChar) && _answerBuffer!.TryAcceptedReadlineConsoleKey(keyinfo))
                    {
                        _indexTooptip = 0;
                        break;
                    }
                }
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            if (_currentitem != null && updateposanswer)
            {
                var answer = _currentitem!.Label;
                if (_layout == ChartBarLayout.Stacked && !_showLegends)
                {
                    answer = $"{_currentitem!.Label}: {ValueToString(_currentitem!.Value)}({ValueToString(_currentitem.Percent)}%)";
                }
                _answerBuffer!.LoadPrintable(answer);
                _answerBuffer.ToHome();
            }
            return ResultCtrl != null;
        }



        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer);

            WriteAnswer(screenBuffer);

            if (!_hideChart.HasFlag(HideChart.ChartbarAtFinish))
            {
                WriteTitle(screenBuffer);
                WriteChartBar(screenBuffer);
                WriteLegends(screenBuffer);
            }

            return true;
        }

        public override void FinalizeControl()
        {
            //none
        }

        private void ChangeOrder()
        {
            //order items
            _items = _order switch
            {
                ChartBarOrder.None => [.. _items.OrderBy(x => x.Id)],
                ChartBarOrder.Smallest => [.. _items.OrderBy(x => x.Value)],
                ChartBarOrder.Highest => [.. _items.OrderByDescending(x => x.Value)],
                ChartBarOrder.LabelAsc => [.. _items.OrderBy(x => x.Label)],
                ChartBarOrder.LabelDec => [.. _items.OrderByDescending(x => x.Label)],
                _ => throw new NotImplementedException($"ChartOrder {_order} Not implemented"),
            };
            List<(string id, int page)> auxpaginginfo = [];
            int page = 0;
            int index = 0;
            foreach (ChartItem item in _items)
            {
                index++;
                if (index > _pageSize)
                {
                    index = 1;
                    page++;
                }
                auxpaginginfo.Add(new(item.Id, page));
            }
            _paginginfo = [.. auxpaginginfo];
        }

        private string GetTooltipModeInput()
        {
            if (IsWidgetControl)
            {
                return string.Empty;
            }
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.InputFinishEnter);
            return tooltip.ToString();
        }

        private void LoadTooltipToggle()
        {
            if (IsWidgetControl)
            {
                return;
            }
            _toggerTooptips.Clear();
            _toggerTooptips.Add(Messages.TooltipPages);
            if (GeneralOptions.EnabledAbortKeyValue)
            {
                _toggerTooptips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}, {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}");
            }
            else
            {
                _toggerTooptips.Add($"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}");
            }
            if (!_hideChart.HasFlag(HideChart.Ordering))
            {
                _toggerTooptips.Add($"{string.Format(Messages.TooltipChartSwitchOrder, ConfigPlus.HotKeyTooltipChartBarSwitchOrder)}");
            }
            if (_showLegends)
            {
                _toggerTooptips.Add($"{string.Format(Messages.TooltipChartSwitchLegend, ConfigPlus.HotKeyTooltipChartBarSwitchLegend)}");
            }
            if (!_hideChart.HasFlag(HideChart.Layout))
            {
                _toggerTooptips.Add($"{string.Format(Messages.TooltipChartSwitchType, ConfigPlus.HotKeyTooltipChartBarSwitchLayout)}");
            }
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (IsWidgetControl)
            {
                return;
            }
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[ChartBarStyles.Prompt]);
            }
            return;
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            screenBuffer.Write("  ", new Style(_currentitem!.Color!.Value, _currentitem!.Color!.Value));
            screenBuffer.Write(" ", Style.Default());
            if (!IsWidgetControl)
            {
                string str = _answerBuffer!.IsHideLeftBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeftMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterLeft);
                screenBuffer.Write(str, _optStyles[ChartBarStyles.Answer]);
                screenBuffer.Write(_answerBuffer!.ToBackward(), _optStyles[ChartBarStyles.Answer]);
                screenBuffer.SavePromptCursor();
                screenBuffer.Write(_answerBuffer!.ToForward(), _optStyles[ChartBarStyles.Answer]);
                str = _answerBuffer.IsHideRightBuffer
                    ? ConfigPlus.GetSymbol(SymbolType.InputDelimiterRightMost)
                    : ConfigPlus.GetSymbol(SymbolType.InputDelimiterRight);
                screenBuffer.Write(str, _optStyles[ChartBarStyles.Answer]);
            }
            else
            {
                screenBuffer.Write(_answerBuffer!.ToString(), _optStyles[ChartBarStyles.Answer]);
            }
            screenBuffer.WriteLine("", _optStyles[ChartBarStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            if (IsWidgetControl)
            {
                return;
            }
            string? desc = _changeDescription?.Invoke(_currentitem!) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[ChartBarStyles.Description]);
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip || IsWidgetControl)
            {
                return;
            }
            string tooltip = _tooltipModeInput;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            screenBuffer.Write(tooltip!, _optStyles[ChartBarStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips[_indexTooptip - 1];
        }

        private void WriteLegends(BufferScreen screenBuffer)
        {
            if (!_showLegends && !_hideChart.HasFlag(HideChart.ChartBar))
            {
                return;
            }
            int start = (_startpage) * _pageSize;
            screenBuffer.WriteLine("", Style.Default());
            foreach (ChartItem? item in _items.Skip(start).Take(_pageSize))
            {
                if (_layout == ChartBarLayout.Stacked && !IsWidgetControl)
                {
                    if (item.Id == _currentitem!.Id)
                    {
                        screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)[0]} ", _optStyles[ChartBarStyles.Answer]);
                    }
                    else
                    {
                        screenBuffer.Write("  ", Style.Default());
                    }
                }
                else if (!IsWidgetControl)
                {
                    screenBuffer.Write("  ", Style.Default());
                }
                screenBuffer.Write("  ", new Style(item.Color!.Value, item.Color!.Value));
                screenBuffer.Write(" ", Style.Default());
                if (item.Label.Length > _maxShowlengthlabel)
                {
                    screenBuffer.Write(item.Label[.._maxShowlengthlabel].PadRight(_maxlengthlabel), _optStyles[ChartBarStyles.ChartLabel]);
                }
                else
                {
                    screenBuffer.Write(item.Label.PadRight(_maxlengthlabel), _optStyles[ChartBarStyles.ChartLabel]);
                }
                if (!_hideChart.HasFlag(HideChart.Values) || !_hideChart.HasFlag(HideChart.Percentage))
                {
                    screenBuffer.Write(": ", Style.Default());
                }
                if (!_hideChart.HasFlag(HideChart.Values))
                {
                    screenBuffer.Write(ValueToString(item.Value), _optStyles[ChartBarStyles.ChartValue]);
                }
                if (!_hideChart.HasFlag(HideChart.Percentage))
                {
                    screenBuffer.Write($"({ValueToString(item!.Percent)}%)", _optStyles[ChartBarStyles.ChartPercent]);
                }
                screenBuffer.WriteLine("", Style.Default());
            }
        }

        private void WriteChartBar(BufferScreen screenBuffer)
        {
            if (_hideChart.HasFlag(HideChart.ChartBar))
            {
                return;
            }
            switch (_layout)
            {
                case ChartBarLayout.Standard:
                    {
                        WriteStandardBar(screenBuffer);
                    }
                    break;
                case ChartBarLayout.Stacked:
                    {
                        WriteStackedBar(screenBuffer);
                    }
                    break;
                default:
                    throw new NotImplementedException($"Show ChartType {_layout} Not implemented");
            }
        }

        private void WriteStackedBar(BufferScreen screenBuffer)
        {
            double tkt = _width / _totalvalue;

            foreach (ChartItem item in _items)
            {
                int lenght = (int)(tkt * item.Value);
                if (tkt == 0)
                {
                    tkt = 1;
                }
                screenBuffer.Write(new string(_barOn, lenght), item.StyleBar!.Value);
            }
            screenBuffer.WriteLine("", Style.Default());
        }

        private void WriteStandardBar(BufferScreen screenBuffer)
        {
            int start = (_startpage) * _pageSize;
            foreach (ChartItem? item in _items.Skip(start).Take(_pageSize))
            {
                int tkt = (int)(_ticketStep * item.Value);
                if (tkt == 0)
                {
                    tkt = 1;
                }
                if (item.Id == _currentitem!.Id && !IsWidgetControl)
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)[0]} ", _optStyles[ChartBarStyles.Answer]);
                }
                else if (!IsWidgetControl)
                {
                    screenBuffer.Write("  ", Style.Default());
                }
                if (_showLegends)
                {
                    screenBuffer.Write(new string(_barOn, tkt), item.StyleBar!.Value);
                }
                else
                {
                    if (IsWidgetControl)
                    {
                        screenBuffer.Write(item.Label.PadRight(_maxlengthlabel), _optStyles[ChartBarStyles.ChartLabel]);
                    }
                    else
                    {
                        if (item.Label.Length > _maxShowlengthlabel)
                        {
                            screenBuffer.Write(item.Label[.._maxShowlengthlabel].PadRight(_maxlengthlabel), _optStyles[ChartBarStyles.ChartLabel]);
                        }
                        else
                        {
                            screenBuffer.Write(item.Label.PadRight(_maxlengthlabel), _optStyles[ChartBarStyles.ChartLabel]);
                        }
                    }
                    screenBuffer.Write(": ", Style.Default());
                    screenBuffer.Write(new string(_barOn, tkt), item.StyleBar!.Value);
                    if (!_hideChart.HasFlag(HideChart.Values))
                    {
                        screenBuffer.Write(' ', Style.Default());
                        screenBuffer.Write(ValueToString(item.Value), _optStyles[ChartBarStyles.ChartValue]);
                    }
                    if (!_hideChart.HasFlag(HideChart.Percentage))
                    {
                        screenBuffer.Write(' ', Style.Default());
                        screenBuffer.Write($"({ValueToString(item.Percent)}%)", _optStyles[ChartBarStyles.ChartPercent]);

                    }
                }
                screenBuffer.WriteLine("", Style.Default());
            }
        }

        private void WriteTitle(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(_title) && !_hideChart.HasFlag(HideChart.Title))
            {
                switch (_titleAlignment)
                {
                    case TextAlignment.Left:
                        {
                            screenBuffer.WriteLine(_title, _optStyles[ChartBarStyles.ChartTitle]);
                        }
                        break;
                    case TextAlignment.Right:
                        {
                            string aux = _title;
                            if (aux.Length < _width)
                            {
                                aux = new string(' ', _width - aux.Length) + _title;
                            }
                            screenBuffer.WriteLine(aux, _optStyles[ChartBarStyles.ChartTitle]);
                        }
                        break;
                    case TextAlignment.Center:
                        {
                            string aux = _title;
                            if (aux.Length < _width)
                            {
                                aux = new string(' ', (_width - aux.Length) / 2) + _title;
                            }
                            screenBuffer.WriteLine(aux, _optStyles[ChartBarStyles.ChartTitle]);
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Alignment {_titleAlignment} Not implemented");
                }
            }
            else
            {
                if (!IsWidgetControl)
                {
                    screenBuffer.SavePromptCursor();
                }
            }
        }

        private string ValueToString(double value)
        {
            // Use "N" for number format with group separators, or "F" for fixed-point (no group separators)
            // "F" is typically preferred for progress bar values
            return Math.Round(value, _fractionalDigits).ToString($"F{_fractionalDigits}", _culture);
        }

        private static string TextOrder(ChartBarOrder value)
        {
            return value switch
            {
                ChartBarOrder.None => Messages.OrderStandard,
                ChartBarOrder.Highest => Messages.OrderHighest,
                ChartBarOrder.Smallest => Messages.OrderSmallest,
                ChartBarOrder.LabelAsc => Messages.OrderLabelAsc,
                ChartBarOrder.LabelDec => Messages.OrderLabelDec,
                _ => throw new NotImplementedException($"ChartOrder {value} Not implemented"),
            };
        }
    }
}
