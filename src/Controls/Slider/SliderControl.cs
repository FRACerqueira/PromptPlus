// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.Slider
{
    internal sealed class SliderControl : BaseControlPrompt<double?>, ISliderControl, ISliderWidget
    {
        private readonly Dictionary<SliderStyles, Style> _optStyles;
        private Func<double, Style>? _changeColor;
        private Color[]? _changeGradient;
        private Func<double, string>? _changeDescription;
        private Func<double, Task<string>>? _changeDescriptionAsync;
        private CultureInfo _culture;
        private double _maxValue = 100;
        private double _minValue;
        private byte _width;
        private SliderBarType _sliderBarType = SliderBarType.Fill;
        private byte _fracionalDig;
        private HideSlider _hideSlider = HideSlider.None;
        private SliderLayout _layout = SliderLayout.LeftRight;
        private double? _step;
        private double? _largeStep;
        private double? _defaultValue;
        private bool _useDefaultHistory = true;
        private HistoryOptions? _historyOptions;
        private IList<ItemHistory>? _itemHistories;
        private double _currentValue;
        private double _ranger;
        private int _fator;
        private double _precision;
        private char _slidebar = ' ';
        private string[] _toggerTooptips = [];
        private int _indexTooptip;
        private string _valueFormat = "F0";
        private Color[]? _gradientColors;

        public SliderControl(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<SliderStyles>(console.CurrentStyle);
            _culture = ConfigPrompt.DefaultCulture;
            _width = ConfigPrompt.SliderWidth;
        }

        #region ISliderControl, ISliderWidget

        public ISliderControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        public ISliderControl Fill(SliderBarType type)
        {
            _sliderBarType = type;
            return this;
        }

        public ISliderControl Styles(SliderStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
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

        public ISliderControl Range(double minvalue, double maxvalue)
        {
            if (minvalue >= maxvalue)
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) must be less than Maxvalue({maxvalue})");
            }
            _minValue = minvalue;
            _maxValue = maxvalue;
            return this;
        }

        public ISliderControl Width(byte value)
        {
            if (value < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater or equal than 10");
            }
            else if (value > 100) 
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be less or equal than 100");
            }
            _width = value;
            return this;
        }

        public ISliderControl Default(double value, bool useDefaultHistory = true)
        {
            _defaultValue = value;
            _useDefaultHistory = useDefaultHistory;
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

        public ISliderControl FractionalDigits(byte value)
        {
            _fracionalDig = value;
            return _fracionalDig > 5 ? throw new ArgumentOutOfRangeException(nameof(value), "FractionalDigits must be less than 5") : this;
        }

        public ISliderControl Layout(SliderLayout value)
        {
            _layout = value;
            return this;
        }

        public ISliderControl Step(double value)
        {
            _step = value;
            return this;
        }

        public ISliderControl LargeStep(double value)
        {
            _largeStep = value;
            return this;
        }

        public ISliderControl ChangeColor(Func<double, Style> value)
        {
            _changeColor = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            _changeGradient = null;
            return this;
        }

        public ISliderControl ChangeGradient(params Color[] colors)
        {
            ArgumentNullException.ThrowIfNull(colors);
            if (colors.Length == 0)
            {
                throw new ArgumentNullException(nameof(colors), "The value cannot be empty.");
            }
            _changeGradient = colors;
            _changeColor = null;
            return this;
        }

        public ISliderControl ChangeDescription(Func<double, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        public ISliderControl ChangeDescriptionAsync(Func<double, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        public ISliderControl HideElements(HideSlider value)
        {
            _hideSlider = value;
            return this;
        }

        /// <inheritdoc/>
        ISliderWidget ISliderWidget.Fill(SliderBarType type)
        {
            Fill(type);
            return this;
        }

        /// <inheritdoc/>
        ISliderWidget ISliderWidget.Styles(SliderStyles styleType, Style style)
        {
            Styles(styleType, style);
            return this;
        }

        /// <inheritdoc/>
        ISliderWidget ISliderWidget.Culture(CultureInfo culture)
        {
            Culture(culture);
            return this;
        }

        /// <inheritdoc/>
        ISliderWidget ISliderWidget.Width(byte value)
        {
            Width(value);
            return this;
        }

        /// <inheritdoc/>
        ISliderWidget ISliderWidget.ChangeColor(Func<double, Style> value)
        {
            ChangeColor(value);
            return this;
        }

        /// <inheritdoc/>
        ISliderWidget ISliderWidget.ChangeGradient(params Color[] colors)
        {
            ChangeGradient(colors);
            return this;
        }

        /// <inheritdoc/>
        ISliderWidget ISliderWidget.HideElements(HideSlider value)
        {
            HideElements(value);
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
            _fator = 100;
            for (int i = 0; i < _fracionalDig; i++)
            {
                _fator *= 10;
            }
            _valueFormat = $"F{_fracionalDig}";

            if (!_step.HasValue)
            {
                _step = _ranger / 100;
            }
            if (!_largeStep.HasValue)
            {
                _largeStep = _ranger / 10;
            }
            _precision = _ranger / _fator;

            SetSlideBarType();

            // Precompute the per-column gradient once. It only depends on the (now fixed)
            // width and colors, so it never changes during the render loop.
            _gradientColors = _changeGradient is null ? null : BuildGradient(_width, _changeGradient);

            if (!IsWidget)
            {
                LoadTooltipToggle();
            }
        }

        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = true;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    KeyPressResult press = ReadNextKey(true, cancellationToken);
                    if (press.IsResize || press.IsCancelled)
                    {
                        if (press.IsCancelled)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<double?>(default!, true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    #region default Press to Finish and tooltip

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<double?>(_currentValue, true);
                        break;
                    }
                    else if (keyinfo.IsPressEnterKey())
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<double?>(_currentValue, false);
                        SaveHistory();
                        break;
                    }
                    else if (IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Length)
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

                    else if ((keyinfo.IsPressDownArrowKey() && _layout == SliderLayout.UpDown) ||
                        (keyinfo.IsPressLeftArrowKey() && _layout == SliderLayout.LeftRight))
                    {
                        if (_currentValue.CompareTo(_minValue) == 0)
                        {
                            continue;
                        }
                        double aux = _currentValue - _step!.Value;
                        if (aux.CompareTo(_minValue) < 0)
                        {
                            aux = _minValue;
                        }
                        _currentValue = Math.Round(aux, _fracionalDig);
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
                        _currentValue = Math.Round(aux, _fracionalDig);
                        _indexTooptip = 0;
                        break;
                    }
                    else if ((keyinfo.IsPressUpArrowKey() && _layout == SliderLayout.UpDown) ||
                             (keyinfo.IsPressRightArrowKey() && _layout == SliderLayout.LeftRight))
                    {
                        if (_currentValue.CompareTo(_maxValue) == 0)
                        {
                            continue;
                        }
                        double aux = _currentValue + _step!.Value;
                        if (aux.CompareTo(_maxValue) > 0)
                        {
                            aux = _maxValue;
                        }
                        _currentValue = Math.Round(aux, _fracionalDig);
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
                        _currentValue = Math.Round(aux, _fracionalDig);
                        _indexTooptip = 0;
                        break;
                    }

                }
            }
            finally
            {
                ConsoleHandler.CursorVisible = oldcursor;
            }
            return ResultCtrl != null;
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!IsWidget)
            {
                WritePrompt(screenBuffer, _optStyles[SliderStyles.Prompt]);

                WriteAnswer(screenBuffer);

                WriteDescription(screenBuffer);

            }
            WriteSlider(screenBuffer);

            if (!IsWidget)
            {
                WriteTooltip(screenBuffer);

                WriteError(screenBuffer, _optStyles[SliderStyles.Error]);

            }

        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[SliderStyles.Prompt]);

            string answer = ResultCtrl!.Value.IsAborted
                ? OptionsControl.EnabledAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty
                : ValueToString(_currentValue);

            screenBuffer.WriteLine(answer, _optStyles[SliderStyles.Answer]);

            return true;
        }

        public override void FinalizeControl()
        {
            //NONE
        }

        private string ValueToString(double value)
        {
            // Fixed-point format (no group separators). The format string is cached in InitControl
            // to avoid re-allocating it on every render.
            return Math.Round(value, _fracionalDig).ToString(_valueFormat, _culture);
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
            // Number of precision steps required to reach 'value'. This is the closed-form
            // equivalent of the previous incremental loop (O(1) instead of O(qtd), which
            // could reach 100 * 10^fracionalDig iterations per render).
            double qtd = _precision > 0 ? Math.Ceiling((value - _minValue) / _precision) : 0;
            double perc = qtd / _fator;
            return (int)Math.Round(_width * perc, _fracionalDig);
        }

        private void WriteSlider(BufferScreen screenBuffer)
        {
            if (_layout != SliderLayout.LeftRight)
            {
                return;
            }
            if (!_hideSlider.HasFlag(HideSlider.Range))
            {
                screenBuffer.Write($"{ValueToString(_minValue)} ", _optStyles[SliderStyles.Ranger]);
            }

            string delimitbar = GetSymbol(SymbolType.GridSingleDividerY);
            if (!_hideSlider.HasFlag(HideSlider.Delimit))
            {
                screenBuffer.Write(delimitbar, _optStyles[SliderStyles.Ranger]);
            }

            Style OnStyle = _optStyles[SliderStyles.Slider].Background(_optStyles[SliderStyles.Slider].Foreground);
            if (_sliderBarType != SliderBarType.Fill)
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
                Color[] gradient = _gradientColors ??= BuildGradient(_width, _changeGradient);
                string bar = _slidebar.ToString();
                bool isSpaceBar = _slidebar == ' ';
                int count = Math.Min(valuestep, gradient.Length);
                for (int i = 0; i < count; i++)
                {
                    Color color = gradient[i];
                    screenBuffer.Write(bar, isSpaceBar ? OnStyle.Background(color) : OnStyle.ForeGround(color));
                }
            }
            if (offlength > 0)
            {
                if (_sliderBarType == SliderBarType.Fill)
                {
                    screenBuffer.Write(new string(' ', _width - valuestep), _optStyles[SliderStyles.Slider]);
                }
                else
                {
                    screenBuffer.Write(new string(_slidebar, _width - valuestep), _optStyles[SliderStyles.Slider].Background);
                }
            }
            if (!_hideSlider.HasFlag(HideSlider.Delimit))
            {
                screenBuffer.Write(delimitbar, _optStyles[SliderStyles.Ranger]);
            }
            if (!_hideSlider.HasFlag(HideSlider.Range))
            {
                screenBuffer.Write($" {ValueToString(_maxValue)}", _optStyles[SliderStyles.Ranger]);
            }
            screenBuffer.WriteLine("", _optStyles[SliderStyles.Prompt]);
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[SliderStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            if (_indexTooptip >= _toggerTooptips.Length)
            {
                _indexTooptip = 0;
            }
            return _toggerTooptips[_indexTooptip];
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            if (_changeDescriptionAsync is not null)
            {
                desc = _changeDescriptionAsync.Invoke(_currentValue)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                desc = _changeDescription?.Invoke(_currentValue) ?? OptionsControl.DescriptionValue;
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[SliderStyles.Description]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            if (_layout == SliderLayout.UpDown)
            {
                if (!_hideSlider.HasFlag(HideSlider.Range))
                {
                    screenBuffer.Write($"[{ValueToString(_minValue)},{ValueToString(_maxValue)}] ", _optStyles[SliderStyles.Ranger]);
                }
            }
            string answer = ValueToString(_currentValue);
            screenBuffer.Write(answer, _optStyles[SliderStyles.Answer]);
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[SliderStyles.Answer]);
        }

        private static Color[] BuildGradient(int width, params Color[] colors)
        {
            Color[] result = new Color[width];
            for (int i = 0; i < width; i++)
            {
                float percentage = (colors.Length - 1) * ((float)i / width);
                int colorPrevIndex = (int)percentage;
                int colorNextIndex = (int)Math.Ceiling(percentage);
                Color colorPrev = colors[colorPrevIndex];
                Color colorNext = colors[colorNextIndex];
                float ltrOffset = percentage - colorPrevIndex;
                float rtlOffset = 1 - ltrOffset;

                byte r = (byte)(rtlOffset * colorPrev.R + ltrOffset * colorNext.R);
                byte g = (byte)(rtlOffset * colorPrev.G + ltrOffset * colorNext.G);
                byte b = (byte)(rtlOffset * colorPrev.B + ltrOffset * colorNext.B);

                result[i] = new Color(r, g, b);
            }
            return result;
        }


        private void SetSlideBarType()
        {
            switch (_sliderBarType)
            {
                case SliderBarType.Fill:
                    break;
                case SliderBarType.Light:
                    _slidebar = GetSymbol(SymbolType.SliderBarLight)[0];
                    break;
                case SliderBarType.DoubleLight:
                    _slidebar = GetSymbol(SymbolType.SliderBarDoubleLight)[0];
                    break;
                case SliderBarType.Square:
                    _slidebar = GetSymbol(SymbolType.SliderBarSquare)[0];
                    break;
                case SliderBarType.Dot:
                    _slidebar = GetSymbol(SymbolType.SliderBarDot)[0];
                    break;
                default:
                    throw new NotImplementedException($"BarType: {_sliderBarType} Not Implemented");
            }
        }

        private string GetTooltipMain()
        {
            StringBuilder tooltip = new();
            tooltip.Append(PromptPlusResources.TooltipEnterFinish);
            tooltip.Append('.');
            if (_layout == SliderLayout.LeftRight)
            {
                tooltip.Append(PromptPlusResources.TooltipSliderLeftRight);
                tooltip.Append('.');
            }
            else if (_layout == SliderLayout.UpDown)
            {
                tooltip.Append(PromptPlusResources.TooltipSliderUpDown);
                tooltip.Append('.');
            }
            return tooltip.ToString();
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                GetTooltipMain()
            ];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
        }

        private void SaveHistory()
        {
            if (_historyOptions == null)
            {
                return;
            }
            string serializedValue = JsonSerializer.Serialize(_currentValue);
            IList<ItemHistory> hist = FileHistory.LoadHistory(_historyOptions.FileNameValue, _historyOptions.MaxItemsValue);
            hist.Clear(); 
            FileHistory.AddHistory(serializedValue, _historyOptions.ExpirationTimeValue, hist);
            FileHistory.SaveHistory(_historyOptions.FileNameValue, hist, _historyOptions.MaxItemsValue);
            _itemHistories = hist;

        }
    }
}
