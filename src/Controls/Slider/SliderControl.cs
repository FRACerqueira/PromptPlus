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

namespace PromptPlusLibrary.Controls.Slider
{
    internal sealed class SliderControl : BaseControlPrompt<double?>, ISliderControl, ISliderWidget
    {
        private readonly Dictionary<SliderStyles, Style> _optStyles = BaseControlOptions.LoadStyle<SliderStyles>();
        private Func<double, Style>? _changeColor;
        private Color[]? _changeGradient;
        private Func<double, string>? _changeDescription;
        private CultureInfo _culture;
        private double? _defaultValue;
        private bool _useDefaultHistory;
        private HistoryOptions? _historyOptions;
        private List<ItemHistory>? _itemHistories;
        private Paginator<ItemHistory>? _localpaginator;
        private double _maxValue = 100;
        private double _minValue;
        private byte _width;
        private HideSlider _hideslide = HideSlider.None;
        private SliderLayout _sliderLayout = SliderLayout.LeftRight;
        private byte _fractionalDigits;
        private SliderBarType _slideBarType = SliderBarType.Fill;
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private string _tooltipModeHistory = string.Empty;
        private char _slidebar = ' ';
        private double _precision;
        private int _fator;
        private double _ranger;
        private double? _largeStep;
        private double? _smallStep;
        private double _currentValue;
        private double? _savedinput;
        private bool _abortedKeyPress;

        private ModeView _modeView = ModeView.Input;
        private readonly Dictionary<ModeView, string[]> _toggerTooptips = new()
        {
            { ModeView.Input,[] },
            { ModeView.History,[] }
        };

        public void InternalRange(double minvalue, double maxvalue)
        {
            if (minvalue > maxvalue)
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minValue = minvalue;
            _maxValue = maxvalue;
        }

        public void InternalDefault(double value)
        {
            _defaultValue = value;
            _useDefaultHistory = false;
        }

        public void InternalFracionalDig(byte value)
        {
            _fractionalDigits = value;
            if (_fractionalDigits > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "FracionalDig must be less than 5");
            }
        }

        public SliderControl(bool iswidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(iswidget, console, promptConfig, baseControlOptions)
        {
            _culture = ConfigPlus.DefaultCulture;
            _width = ConfigPlus.SliderWidth;
        }

        #region ISliderControl

        ISliderWidget ISliderWidget.Fill(SliderBarType type)
        {
            _slideBarType = type;
            return this;
        }

        ISliderWidget ISliderWidget.Styles(SliderStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        ISliderWidget ISliderWidget.Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        ISliderWidget ISliderWidget.Width(byte value)
        {
            if (value < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater or equal than 10");
            }
            _width = value;
            return this;
        }

        ISliderWidget ISliderWidget.ChangeColor(Func<double, Style> value)
        {
            _changeColor = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            _changeGradient = null;
            return this;
        }

        ISliderWidget ISliderWidget.ChangeGradient(params Color[] colors)
        {
            _changeGradient = colors ?? throw new ArgumentNullException(nameof(colors), "The colors cannot be null.");
            _changeColor = null;
            return this;
        }

        ISliderWidget ISliderWidget.HideElements(HideSlider value)
        {
            _hideslide = value;
            return this;
        }

        public ISliderControl ChangeColor(Func<double, Style> value)
        {
            _changeColor = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            _changeGradient = null;
            return this;
        }

        public ISliderControl ChangeDescription(Func<double, string> value)
        {
            _changeDescription = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            return this;
        }

        public ISliderControl ChangeGradient(params Color[] colors)
        {
            _changeGradient = colors ?? throw new ArgumentNullException(nameof(colors), "The colors cannot be null.");
            _changeColor = null;
            return this;
        }

        public ISliderControl Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        public ISliderControl Default(double value, bool usedefaultHistory = true)
        {
            _defaultValue = value;
            _useDefaultHistory = usedefaultHistory;
            return this;
        }

        public ISliderControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
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

        public ISliderControl Fill(SliderBarType type)
        {
            _slideBarType = type;
            return this;
        }

        public ISliderControl FracionalDig(byte value)
        {
            _fractionalDigits = value;
            if (_fractionalDigits > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "FracionalDig must be less than 5");
            }
            return this;
        }

        public ISliderControl HideElements(HideSlider value)
        {
            _hideslide = value;
            return this;
        }

        public ISliderControl Width(byte value)
        {
            if (value < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater or equal than 10");
            }
            _width = value;
            return this;
        }

        public ISliderControl Layout(SliderLayout value)
        {
            _sliderLayout = value;
            return this;
        }

        public ISliderControl LargeStep(double value)
        {
            _largeStep = value;
            return this;
        }

        public ISliderControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public ISliderControl Range(double minvalue, double maxvalue)
        {
            if (minvalue > maxvalue)
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minValue = minvalue;
            _maxValue = maxvalue;
            return this;
        }

        public ISliderControl Step(double value)
        {
            _smallStep = value;
            return this;
        }

        public ISliderControl Styles(SliderStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            int odd = _width % 2;
            if (odd != 0)
            {
                _width += 1;
            }
            if (_historyOptions != null)
            {
                try
                {
                    _itemHistories = [.. FileHistory
                        .LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue)
                        .Select(x => (Item: x, Parsed: double.TryParse(x.History, out double val) ? val : (double?)null))
                        .Where(x => x.Parsed.HasValue && x.Parsed.Value >= _minValue && x.Parsed.Value <= _maxValue)
                        .Select(x => x.Item)];
                }
                catch
                {
                    _itemHistories = [];
                }
                FileHistory.SaveHistory(_historyOptions.FileNameValue, _itemHistories, _historyOptions.MaxItemsValue);
                if (_useDefaultHistory && _itemHistories.Count > 0)
                {
                    if (double.TryParse(_itemHistories[0].History, out double auxdefault))
                    {
                        _defaultValue = auxdefault;
                    }
                }
            }

            if (!_defaultValue.HasValue)
            {
                _defaultValue = _minValue;
            }
            _currentValue = _defaultValue.Value;

            if (_minValue == _maxValue)
            {
                throw new InvalidOperationException($"Range Minvalue to Maxvalue must be greater than 0");
            }
            if (_currentValue > _maxValue)
            {
                throw new InvalidOperationException($"Default({_currentValue}) >  Maxvalue({_maxValue})");
            }
            if (_currentValue < _minValue)
            {
                throw new InvalidOperationException($"Default({_currentValue}) < Minvalue({_minValue})");
            }

            _ranger = _maxValue - _minValue;
            if (_ranger < 0)
            {
                _ranger *= -1;
            }
            string fra = "1";
            for (int i = 0; i < _fractionalDigits; i++)
            {
                fra += "0";
            }

            _fator = 100 * (int.Parse(fra));

            if (!_smallStep.HasValue)
            {
                _smallStep = _ranger / 100;
            }
            if (!_largeStep.HasValue)
            {
                _largeStep = _ranger / 10;
            }
            _precision = _ranger / _fator;

            SetSlideBarType();

            if (!IsWidgetControl)
            {
                LoadTooltipToggle();
                _tooltipModeInput = GetTooltipModeInput();
                _tooltipModeHistory = GetTooltipModeHistory();
            }
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!IsWidgetControl)
            {
                WritePrompt(screenBuffer);

                WriteAnswer(screenBuffer);

                WriteError(screenBuffer);

                WriteDescription(screenBuffer);
            }

            WriteSlider(screenBuffer);

            if (!IsWidgetControl)
            {
                WriteHistory(screenBuffer);

                WriteTooltip(screenBuffer);
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
                    ConsoleKeyInfo keyinfo = WaitKeypress(true, cancellationToken);

                    #region default Press to Finish and tooltip
                    if (cancellationToken.IsCancellationRequested)
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _savedinput = null;
                            _localpaginator = null;
                            _modeView = ModeView.Input;
                            break;
                        }
                        _abortedKeyPress = true;
                        ResultCtrl = new ResultPrompt<double?>(_currentValue, true);
                        break;
                    }
                    else if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _savedinput = null;
                            _localpaginator = null;
                            _modeView = ModeView.Input;
                            break;
                        }
                        _abortedKeyPress = true;
                        ResultCtrl = new ResultPrompt<double?>(_currentValue, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        if (_modeView != ModeView.Input)
                        {
                            _currentValue = _savedinput!.Value;
                            _savedinput = null;
                            _localpaginator = null;
                            _modeView = ModeView.Input;
                        }
                        ResultCtrl = new ResultPrompt<double?>(_currentValue, false);
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

                    #region Histories
                    else if (_modeView == ModeView.Input && ConfigPlus.HotKeyShowHistory.Equals(keyinfo) && (_itemHistories?.Count ?? 0) > 0)
                    {
                        IEnumerable<ItemHistory> subhist = _itemHistories!.Where(x =>
                             DateTime.Now < new DateTime(x.TimeOutTicks));
                        if (!subhist.Any())
                        {
                            SetError(Messages.HistoryNotFound);
                            break;
                        }
                        _savedinput = _currentValue;
                        _localpaginator = new Paginator<ItemHistory>(
                            FilterMode.Disabled,
                            subhist,
                            _historyOptions!.PageSizeValue,
                            Optional<ItemHistory>.Empty(),
                            (item1, item2) => item1.History == item2.History,
                            (item) => item.History);
                        _modeView = ModeView.History;
                        _currentValue = double.Parse(_localpaginator.SelectedItem.History);
                        _indexTooptip = 0;
                        break;
                    }

                    else if (_modeView == ModeView.History)
                    {
                        if (keyinfo.IsPressCtrlDeleteKey())
                        {
                            _indexTooptip = 0;
                            FileHistory.ClearHistory(_historyOptions!.FileNameValue);
                            _itemHistories!.Clear();
                            _currentValue = _savedinput!.Value;
                            _indexTooptip = 0;
                            _localpaginator = null;
                            _savedinput = null;
                            _modeView = ModeView.Input;
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
                            _currentValue = double.Parse(_localpaginator.SelectedItem.History);
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
                            _currentValue = double.Parse(_localpaginator.SelectedItem.History);
                            _indexTooptip = 0;
                            break;
                        }
                        else if (keyinfo.IsPressPageDownKey())
                        {
                            if (_localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages))
                            {
                                _currentValue = double.Parse(_localpaginator.SelectedItem.History);
                                _indexTooptip = 0;
                                break;
                            }
                        }
                        else if (keyinfo.IsPressPageUpKey())
                        {
                            if (_localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages))
                            {
                                _currentValue = double.Parse(_localpaginator.SelectedItem.History);
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
                    }
                    #endregion

                    else if ((keyinfo.IsPressDownArrowKey() && _sliderLayout == SliderLayout.UpDown) ||
                        (keyinfo.IsPressLeftArrowKey() && _sliderLayout == SliderLayout.LeftRight))
                    {
                        if (_currentValue.CompareTo(_minValue) == 0)
                        {
                            continue;
                        }
                        double aux = _currentValue - _smallStep!.Value;
                        if (aux.CompareTo(_minValue) < 0)
                        {
                            aux = _minValue;
                        }
                        _currentValue = Math.Round(aux, _fractionalDigits);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressShiftTabKey())
                    {
                        if (_currentValue.CompareTo(_minValue) == 0)
                        {
                            continue;
                        }
                        double aux = _currentValue - _largeStep!.Value;
                        if (aux.CompareTo(_minValue) < 0)
                        {
                            aux = _minValue;
                        }
                        _currentValue = Math.Round(aux, _fractionalDigits);
                        _indexTooptip = 0;
                        break;
                    }
                    else if ((keyinfo.IsPressUpArrowKey() && _sliderLayout == SliderLayout.UpDown) ||
                             (keyinfo.IsPressRightArrowKey() && _sliderLayout == SliderLayout.LeftRight))
                    {
                        if (_currentValue.CompareTo(_maxValue) == 0)
                        {
                            continue;
                        }
                        double aux = _currentValue + _smallStep!.Value;
                        if (aux.CompareTo(_maxValue) > 0)
                        {
                            aux = _maxValue;
                        }
                        _currentValue = Math.Round(aux, _fractionalDigits);
                        _indexTooptip = 0;
                        break;
                    }
                    else if (keyinfo.IsPressTabKey())
                    {
                        if (_currentValue.CompareTo(_maxValue) == 0)
                        {
                            continue;
                        }
                        double aux = _currentValue + _largeStep!.Value;
                        if (aux.CompareTo(_maxValue) > 0)
                        {
                            aux = _maxValue;
                        }
                        _currentValue = Math.Round(aux, _fractionalDigits);
                        _indexTooptip = 0;
                        break;
                    }
                }
            }
            finally
            {
                ConsolePlus.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[SliderStyles.Prompt]);
            }
            string answer;
            if (ResultCtrl!.Value.IsAborted)
            {
                if (GeneralOptions.ShowMesssageAbortKeyValue)
                {
                    answer = Messages.CanceledKey;
                }
                else
                {
                    answer = string.Empty;
                }
            }
            else
            {
                answer = ValueToString(_currentValue);
            }
            screenBuffer.WriteLine(answer, _optStyles[SliderStyles.Answer]);

            return true;
        }

        public override void FinalizeControl()
        {
            if (_historyOptions != null && !_abortedKeyPress)
            {
                FileHistory.AddHistory(_currentValue.ToString(_culture), _historyOptions.ExpirationTimeValue, _itemHistories);
                FileHistory.SaveHistory(_historyOptions.FileNameValue, _itemHistories!, _historyOptions.MaxItemsValue);
            }
        }

        private string GetTooltipModeHistory()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            tooltip.Append(Messages.TooltipPages);
            return tooltip.ToString();
        }

        private string GetTooltipModeInput()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            tooltip.Append(", ");
            if (_sliderLayout == SliderLayout.LeftRight && _modeView == ModeView.Input)
            {
                tooltip.Append(Messages.SliderNumberLeftRightKeyNavigator);
            }
            if (_sliderLayout == SliderLayout.UpDown && _modeView == ModeView.Input)
            {
                tooltip.Append(Messages.SliderNumberUpDownKeyNavigator);
            }
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

                if (mode == ModeView.Input && GeneralOptions.EnabledAbortKeyValue)
                {
                    lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
                }
                if (mode == ModeView.History)
                {
                    lsttooltips[0] += $", {Messages.TooltipHistoryEsc}";
                }
                if (_itemHistories != null && _itemHistories.Count > 0 && mode == ModeView.Input)
                {
                    lsttooltips.Add(string.Format(Messages.TooltipHistoryKey, ConfigPlus.HotKeyShowHistory));
                }
                _toggerTooptips[mode] = [.. lsttooltips];
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(ValidateError))
            {
                screenBuffer.WriteLine(ValidateError, _optStyles[SliderStyles.Error]);
                ClearError();
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            else if (_modeView == ModeView.Input)
            {
                tooltip = _tooltipModeInput;
            }
            else if (_modeView == ModeView.History)
            {
                tooltip = _tooltipModeHistory;
            }
            else
            {
                throw new NotImplementedException($"ModeView {_modeView} not implemented.");
            }
            screenBuffer.Write(tooltip, _optStyles[SliderStyles.Tooltips]);

        }

        private string GetTooltipToggle()
        {
            return _modeView switch
            {
                ModeView.Input => _toggerTooptips[ModeView.Input][_indexTooptip - 1],
                ModeView.History => _toggerTooptips[ModeView.History][_indexTooptip - 1],
                _ => throw new NotImplementedException($"ModeView {_modeView} not implemented.")
            };

        }

        private void WriteHistory(BufferScreen screenBuffer)
        {
            if (_modeView != ModeView.History)
            {
                return;
            }

            ArraySegment<ItemHistory> subset = _localpaginator!.GetPageData(); // Cache the page data
            screenBuffer.WriteLine(Messages.EntryHistory, _optStyles[SliderStyles.Selected]);
            foreach (ItemHistory item in subset)
            {
                string value = item.History;
                if (_localpaginator.TryGetSelected(out ItemHistory selectedItem) && EqualityComparer<ItemHistory>.Default.Equals(item, selectedItem))
                {
                    screenBuffer.Write($"{ConfigPlus.GetSymbol(SymbolType.Selector)}", _optStyles[SliderStyles.Selected]);
                    screenBuffer.WriteLine($" {value}", _optStyles[SliderStyles.Selected]);
                }
                else
                {
                    screenBuffer.Write(" ", _optStyles[SliderStyles.UnSelected]);
                    screenBuffer.WriteLine($" {value}", _optStyles[SliderStyles.UnSelected]);
                }
            }

            if (_localpaginator.PageCount > 1)
            {
                string template = ConfigPlus.PaginationTemplate.Invoke(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount
                )!;
                screenBuffer.WriteLine(template, _optStyles[SliderStyles.Pagination]);
            }
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_currentValue) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[SliderStyles.Description]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_sliderLayout == SliderLayout.UpDown)
            {
                if (!_hideslide.HasFlag(HideSlider.Range))
                {
                    screenBuffer.Write($"[{ValueToString(_minValue)},{ValueToString(_maxValue)}] ", _optStyles[SliderStyles.Ranger]);
                }
            }
            string answer = ValueToString(_currentValue);
            screenBuffer.Write(answer, _optStyles[SliderStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[SliderStyles.Answer]);
        }

        private void WriteSlider(BufferScreen screenBuffer)
        {
            if (_sliderLayout != SliderLayout.LeftRight)
            {
                return;
            }
            if (!_hideslide.HasFlag(HideSlider.Range))
            {
                screenBuffer.Write($"{ValueToString(_minValue)} ", _optStyles[SliderStyles.Ranger]);
            }

            string delimitbar = "│";
            if (!ConsolePlus.IsUnicodeSupported)
            {
                delimitbar = "|";
            }
            if (!_hideslide.HasFlag(HideSlider.Delimit))
            {
                screenBuffer.Write(delimitbar, _optStyles[SliderStyles.Ranger]);
            }

            Style OnStyle = _optStyles[SliderStyles.Slider].Background(_optStyles[SliderStyles.Slider].Foreground);
            if (_slideBarType != SliderBarType.Fill)
            {
                OnStyle = _optStyles[SliderStyles.Slider];
            }
            int valuestep = CurrentValueStep(_currentValue);

            int offlength = _width - valuestep;

            if (_changeGradient is null)
            {
                if (_changeColor != null)
                {
                    OnStyle = _changeColor(_currentValue);
                }
                screenBuffer.Write(new string(_slidebar, valuestep), OnStyle);
            }
            else
            {
                List<(string Text, Style StyeText)> aux = Gradient(new string(_slidebar, _width), _changeGradient);
                for (int i = 0; i < aux.Count; i++)
                {
                    if (i < valuestep && valuestep > 0)
                    {
                        if (aux[i].Text[0].Equals(' '))
                        {
                            screenBuffer.Write(aux[i].Text[0].ToString(), OnStyle.Background(aux[i].StyeText.Foreground));
                        }
                        else
                        {
                            screenBuffer.Write(aux[i].Text[0].ToString(), OnStyle.ForeGround(aux[i].StyeText.Foreground));
                        }
                    }
                }
            }
            if (offlength > 0)
            {
                if (_slideBarType == SliderBarType.Fill)
                {
                    screenBuffer.Write(new string(' ', _width - valuestep), _optStyles[SliderStyles.Slider]);
                }
                else
                {
                    screenBuffer.Write(new string(_slidebar, _width - valuestep), _optStyles[SliderStyles.Slider].Background);
                }
            }
            if (!_hideslide.HasFlag(HideSlider.Delimit))
            {
                screenBuffer.Write(delimitbar, _optStyles[SliderStyles.Ranger]);
            }
            if (!_hideslide.HasFlag(HideSlider.Range))
            {
                screenBuffer.Write($" {ValueToString(_maxValue)}", _optStyles[SliderStyles.Ranger]);
            }
            screenBuffer.WriteLine("", _optStyles[SliderStyles.Prompt]);
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[SliderStyles.Prompt]);
            }
        }

        private string ValueToString(double value)
        {
            // Use "N" for number format with group separators, or "F" for fixed-point (no group separators)
            // "F" is typically preferred for progress bar values
            return Math.Round(value, _fractionalDigits).ToString($"F{_fractionalDigits}", _culture);
        }

        private int CurrentValueStep(double value)
        {
            if (value < _minValue)
            {
                value = _minValue;
            }
            if (value > _maxValue)
            {
                value = _maxValue;
            }
            double min = _minValue;
            double qtd = 0;
            while (min < value)
            {
                min += _precision;
                qtd++;
            }
            double perc = qtd / _fator;
            return (int)Math.Round(_width * perc, _fractionalDigits);
        }

        private static List<(string Text, Style StyeText)> Gradient(string text, params Color[] colors)
        {
            List<(string Text, Style StyeText)> result = [];
            for (int i = 0; i < text.Length; i++)
            {
                float percentage = (colors.Length - 1) * ((float)i / text.Length);
                int colorPrevIndex = (int)percentage;
                int colorNextIndex = (int)Math.Ceiling(percentage);
                Color colorPrev = colors[colorPrevIndex];
                Color colorNext = colors[colorNextIndex];
                float ltrOffset = percentage - colorPrevIndex;
                float rtlOffset = 1 - ltrOffset;

                byte r = (byte)(rtlOffset * colorPrev.R + ltrOffset * colorNext.R);
                byte g = (byte)(rtlOffset * colorPrev.G + ltrOffset * colorNext.G);
                byte b = (byte)(rtlOffset * colorPrev.B + ltrOffset * colorNext.B);

                Color color = new(r, g, b);
                result.Add((text[i].ToString(), new Style(color, color)));
            }
            return result;
        }

        private void SetSlideBarType()
        {
            switch (_slideBarType)
            {
                case SliderBarType.Fill:
                    break;
                case SliderBarType.Light:
                    _slidebar = ConfigPlus.GetSymbol(SymbolType.SliderBarLight)[0];
                    break;
                case SliderBarType.DoubleLight:
                    _slidebar = ConfigPlus.GetSymbol(SymbolType.SliderBarDoubleLight)[0];
                    break;
                case SliderBarType.Square:
                    _slidebar = ConfigPlus.GetSymbol(SymbolType.SliderBarSquare)[0];
                    break;
                default:
                    throw new NotImplementedException($"BarType: {_slideBarType} Not Implemented");
            }
        }

        private enum ModeView
        {
            Input,
            History
        }
    }
}
