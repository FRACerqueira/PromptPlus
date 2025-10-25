// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Threading.Timer;

namespace PromptPlusLibrary.Controls.ProgressBar
{
    internal sealed class ProgressBarControl : BaseControlPrompt<StateProgress>, IProgressBarControl, IDisposable
    {
        private readonly Dictionary<ProgressBarStyles, Style> _optStyles = BaseControlOptions.LoadStyle<ProgressBarStyles>();
        private readonly List<string> _toggerTooptips = [];
        private Func<double, Style>? _changeColor;
        private Color[]? _changeGradient;
        private Func<double, string>? _changeDescription;
        private int _intervalShowElapsedTime = 100;
        private CultureInfo _culture;
        private double? _defaultValue;
        private double _maxValue = 100;
        private double _minValue;
        private byte _width = 80;
        private CancellationTokenSource? _cancellationTokenSource;
        private Action<HandlerProgressBar, CancellationToken>? _actionProgressBar;
        private HandlerProgressBar? _handlerProgressBar;
        private HideProgressBar _hideProgressBar = HideProgressBar.None;
        private byte _fracionalDig;
        private string? _finishText;
        private ProgressBarType _progressBarType = ProgressBarType.Fill;
        private Spinners? _spinner;
        private int _indexTooptip;
        private string _tooltipModeInput = string.Empty;
        private string? _currentspinnerFrame;
        private bool _hasupdateProgress;
        private Timer? _timer;
        private bool _disposed;
        private char _barOn = ' ';
        private char _barOff = ' ';
        private readonly Stopwatch _stopwatch = new();
        private double _precision;
        private int _factor;
        private double _range;

        public ProgressBarControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _culture = ConfigPlus.DefaultCulture;
        }

        #region IProgressBarControl

        public IProgressBarControl ChangeColor(Func<double, Style> value)
        {
            _changeColor = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            _changeGradient = null;
            return this;
        }

        public IProgressBarControl IntervalUpdate(int mileseconds = 100)
        {
            if (mileseconds < 100 || mileseconds > 1000)
            {
                throw new ArgumentOutOfRangeException(nameof(mileseconds), "Mileseconds must be between 100 and 1000.");
            }
            _intervalShowElapsedTime = mileseconds;
            return this;
        }

        public IProgressBarControl ChangeDescription(Func<double, string> value)
        {
            _changeDescription = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            return this;
        }

        public IProgressBarControl ChangeGradient(params Color[] colors)
        {
            _changeGradient = colors ?? throw new ArgumentNullException(nameof(colors), "The value cannot be null.");
            _changeColor = null;
            return this;
        }

        public IProgressBarControl Culture(CultureInfo culture)
        {
            ArgumentNullException.ThrowIfNull(culture);
            if (!culture.Name.ExistsCulture())
            {
                throw new CultureNotFoundException(culture.Name);
            }
            _culture = culture;
            return this;
        }

        public IProgressBarControl Default(double value)
        {
            _defaultValue = value;
            return this;
        }


        public IProgressBarControl Range(double minvalue, double maxvalue)
        {
            if (minvalue > maxvalue)
            {
                throw new ArgumentOutOfRangeException($"Range invalid. Minvalue({minvalue}) > Maxvalue({maxvalue})");
            }
            _minValue = minvalue;
            _maxValue = maxvalue;
            return this;
        }

        public IProgressBarControl Fill(ProgressBarType type)
        {
            _progressBarType = type;
            return this;
        }

        public IProgressBarControl Finish(string finishtext)
        {
            _finishText = finishtext;
            return this;
        }

        public IProgressBarControl FracionalDig(byte value)
        {
            _fracionalDig = value;
            if (_fracionalDig > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "FracionalDig must be less than 5");
            }
            return this;
        }

        public IProgressBarControl HideElements(HideProgressBar value)
        {
            _hideProgressBar = value;
            return this;
        }

        public IProgressBarControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(GeneralOptions);
            return this;
        }

        public IProgressBarControl Spinner(SpinnersType spinnersType)
        {
            _spinner = new Spinners(spinnersType);
            return this;
        }

        public IProgressBarControl Styles(ProgressBarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        public IProgressBarControl UpdateHandler(Action<HandlerProgressBar, CancellationToken> value)
        {
            _actionProgressBar = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            return this;
        }

        public IProgressBarControl Width(byte value)
        {
            if (value < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Width must be greater or equal than 10");
            }
            _width = value;
            return this;
        }

        #endregion

        public void Dispose()
        {
            if (!_disposed)
            {
                FinalizeControl();
            }
        }

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (!_defaultValue.HasValue)
            {
                _defaultValue = _minValue;
            }
            if (_defaultValue < _minValue || _defaultValue > _maxValue)
            {
                throw new InvalidOperationException($"Default value invalid.Valid values are : Minvalue({_minValue}) and Maxvalue({_maxValue})");
            }
            if (_actionProgressBar is null)
            {
                throw new InvalidOperationException("The UpdateHandler cannot be null.");
            }
            _handlerProgressBar = new HandlerProgressBar(_defaultValue.Value, _minValue, _maxValue);

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cancellationTokenSource.Token.Register(() =>
            {
                _handlerProgressBar.Abort();
            });

            _range = _maxValue - _minValue;
            if (_range < 0)
            {
                _range *= -1;
            }
            string fra = "1";
            for (int i = 0; i < _fracionalDig; i++)
            {
                fra += "0";
            }

            _factor = 100 * (int.Parse(fra));
            _precision = _range / _factor;

            LoadCharBar();
            LoadTooltipToggle();
            _tooltipModeInput = GetTooltipModeInput();

            _stopwatch.Restart();

            Task.Run(() => _actionProgressBar!(_handlerProgressBar!, _cancellationTokenSource.Token), _cancellationTokenSource.Token)
            .ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    _handlerProgressBar.ErrorAndAbort(t.Exception);
                }
            }, TaskContinuationOptions.LongRunning);

            _timer = new Timer(UpdateElapsedTime, null, 0, _intervalShowElapsedTime);
        }

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!_hideProgressBar.HasFlag(HideProgressBar.PromptAnswer))
            {
                WritePrompt(screenBuffer);

                WriteAnswer(screenBuffer);
            }
            WriteError(screenBuffer);

            WriteDescription(screenBuffer);

            WriteProgressBar(screenBuffer);

            WriteTooltip(screenBuffer);
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _stopwatch.Stop();
            bool result = false;
            if (!_hideProgressBar.HasFlag(HideProgressBar.PromptAnswer))
            {
                result = true;
                string answer = $"{ValueToString(ResultCtrl!.Value.Content.FinishedValue!.Value)}% - {ResultCtrl!.Value.Content.ElapsedTime:hh\\:mm\\:ss\\:ff}"; ;
                Style styleanswer = _optStyles[ProgressBarStyles.Answer];
                if (ResultCtrl!.Value.IsAborted)
                {
                    if (GeneralOptions.ShowMesssageAbortKeyValue)
                    {
                        answer = Messages.CanceledKey;
                    }
                    if (ResultCtrl!.Value.Content.ExceptionProgress is not null)
                    {
                        answer = Messages.Error;
                        styleanswer = _optStyles[ProgressBarStyles.Error];
                    }
                }
                else if (!string.IsNullOrEmpty(_finishText))
                {
                    answer = _finishText;
                }
                if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
                {
                    screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[ProgressBarStyles.Prompt]);
                }
                screenBuffer.WriteLine(answer, styleanswer);
            }
            if (!_hideProgressBar.HasFlag(HideProgressBar.ProgressbarAtFinish))
            {
                result = true;
                WriteProgressBar(screenBuffer);
            }
            if (!result)
            {
                screenBuffer.Write("", _optStyles[ProgressBarStyles.Prompt]);
            }
            return true;
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
                    ConsoleKeyInfo? keyinfo = WaitKeypressTimer(cancellationToken);

                    if (!keyinfo.HasValue)
                    {
                        _cancellationTokenSource!.Cancel();
                        ResultCtrl = new ResultPrompt<StateProgress>(
                            new StateProgress(_handlerProgressBar!.Value, _finishText, _minValue, _maxValue, _stopwatch.Elapsed, _handlerProgressBar!.Error)
                            , true);
                        break;
                    }

                    if (IsAbortKeyPress(keyinfo.Value) || _handlerProgressBar!.Error is not null)
                    {
                        _cancellationTokenSource!.Cancel();
                        ResultCtrl = new ResultPrompt<StateProgress>(
                            new StateProgress(_handlerProgressBar!.Value, _finishText, _minValue, _maxValue, _stopwatch.Elapsed, _handlerProgressBar!.Error)
                            , true);
                        break;
                    }
                    if (_handlerProgressBar!.Finish)
                    {
                        _cancellationTokenSource!.Cancel();
                        ResultCtrl = new ResultPrompt<StateProgress>(
                            new StateProgress(_handlerProgressBar!.Value, _finishText, _minValue, _maxValue, _stopwatch.Elapsed, _handlerProgressBar!.Error)
                            , false);
                        break;
                    }
                    if (keyinfo.Value.Key == ConsoleKey.None && keyinfo.Value.Modifiers == ConsoleModifiers.None)
                    {
                        CheckTooltipShowHideKeyPress(keyinfo.Value);
                        break;
                    }

                    if (IsTooltipToggerKeyPress(keyinfo!.Value))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Count)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }
                    else if (CheckTooltipShowHideKeyPress(keyinfo.Value))
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
            return ResultCtrl != null;
        }

        public override void FinalizeControl()
        {
            if (!_disposed)
            {
                _disposed = true;
                _timer?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
        }

        private void LoadCharBar()
        {
            switch (_progressBarType)
            {
                case ProgressBarType.Fill:
                    _barOff = _barOn;
                    break;
                case ProgressBarType.Light:
                    _barOn = ConfigPlus.GetSymbol(SymbolType.ProgressBarLight)[0];
                    break;
                case ProgressBarType.DoubleLight:
                    _barOn = ConfigPlus.GetSymbol(SymbolType.ProgressBarDoubleLight)[0];
                    break;
                case ProgressBarType.Square:
                    _barOn = ConfigPlus.GetSymbol(SymbolType.ProgressBarSquare)[0];
                    break;
                case ProgressBarType.Bar:
                    _barOn = ConfigPlus.GetSymbol(SymbolType.ProgressBarBar)[0];
                    break;
                case ProgressBarType.Dot:
                    _barOn = ConfigPlus.GetSymbol(SymbolType.ProgressBarDot)[0];
                    break;
                default:
                    throw new NotImplementedException($"BarType: {_progressBarType} Not Implemented");
            }
        }

        private string GetTooltipModeInput()
        {
            StringBuilder tooltip = new();
            tooltip.Append(string.Format(Messages.TooltipToggle, ConfigPlus.HotKeyTooltip));
            return tooltip.ToString();
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =
            [
                    $"{string.Format(Messages.TooltipShowHide, ConfigPlus.HotKeyTooltipShowHide)}"
                ];

            if (GeneralOptions.EnabledAbortKeyValue)
            {
                lsttooltips[0] += $", {string.Format(Messages.TooltipCancelEsc, ConfigPlus.HotKeyAbortKeyPress)}";
            }
            _toggerTooptips.Clear();
            _toggerTooptips.AddRange(lsttooltips);
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
            double perc = qtd / _factor;
            return (int)Math.Round(_width * perc, _fracionalDig);
        }

        private string ValueToString(double value)
        {
            // Use "N" for number format with group separators, or "F" for fixed-point (no group separators)
            // "F" is typically preferred for progress bar values
            return Math.Round(value, _fracionalDig).ToString($"F{_fracionalDig}", _culture);
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

        private void UpdateElapsedTime(object? state)
        {
            if (_cancellationTokenSource!.IsCancellationRequested)
            {
                _hasupdateProgress = false;
                return;
            }
            UpdateStatusTasks();
        }

        private void UpdateStatusTasks()
        {
            bool haschange = _handlerProgressBar!.HasChange();
            if (_spinner?.HasNextFrame(out string? newframe) == true)
            {
                _currentspinnerFrame = newframe;
                haschange = true;
            }
            _hasupdateProgress = haschange;
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string tooltip = _tooltipModeInput;
            if (_indexTooptip > 0)
            {
                tooltip = GetTooltipToggle();
            }
            screenBuffer.WriteLine(tooltip!, _optStyles[ProgressBarStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            return _toggerTooptips[_indexTooptip - 1];
        }

        private void WriteProgressBar(BufferScreen screenBuffer)
        {
            double currentvalue = _handlerProgressBar!.Value;
            if (!_hideProgressBar.HasFlag(HideProgressBar.Range))
            {
                screenBuffer.Write($"{ValueToString(_handlerProgressBar.Minvalue)} ", _optStyles[ProgressBarStyles.Ranger]);
            }

            string delimitbar = "│";
            if (!ConsolePlus.IsUnicodeSupported)
            {
                delimitbar = "|";
            }
            if (!_hideProgressBar.HasFlag(HideProgressBar.Delimit))
            {
                screenBuffer.Write(delimitbar, _optStyles[ProgressBarStyles.Ranger]);
            }

            Style OnStyle = _optStyles[ProgressBarStyles.Slider].Background(_optStyles[ProgressBarStyles.Slider].Foreground);
            if (_progressBarType != ProgressBarType.Fill)
            {
                OnStyle = _optStyles[ProgressBarStyles.Slider];
            }
            int valuestep = CurrentValueStep(currentvalue);

            int offlength = _width - valuestep;

            if (_changeGradient is null)
            {
                if (_changeColor != null)
                {
                    OnStyle = _changeColor(currentvalue);
                }
                screenBuffer.Write(new string(_barOn, valuestep), OnStyle);
            }
            else
            {
                List<(string Text, Style StyeText)> aux = Gradient(new string(_barOn, _width), _changeGradient);
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
                screenBuffer.Write(new string(_barOff, offlength), Style.Default().ForeGround(_optStyles[ProgressBarStyles.Slider].Background));
            }
            if (!_hideProgressBar.HasFlag(HideProgressBar.Delimit))
            {
                screenBuffer.Write(delimitbar, _optStyles[ProgressBarStyles.Ranger]);
            }
            if (!_hideProgressBar.HasFlag(HideProgressBar.Range))
            {
                screenBuffer.Write($" {ValueToString(_handlerProgressBar.Maxvalue)}", _optStyles[ProgressBarStyles.Ranger]);
            }
            screenBuffer.WriteLine("", _optStyles[ProgressBarStyles.Prompt]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = _changeDescription?.Invoke(_handlerProgressBar!.Value) ?? GeneralOptions.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[ProgressBarStyles.Description]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            string answer = ValueToString(_handlerProgressBar!.Value);
            screenBuffer.Write($"{answer} %", _optStyles[ProgressBarStyles.Answer]);
            if (!_hideProgressBar.HasFlag(HideProgressBar.ElapsedTime))
            {
                screenBuffer.Write($" - {_stopwatch.Elapsed:hh\\:mm\\:ss\\:ff}", _optStyles[ProgressBarStyles.Answer]);
            }
            if (_currentspinnerFrame != null)
            {
                screenBuffer.Write($" {_currentspinnerFrame} ", _optStyles[ProgressBarStyles.Spinner]);
            }
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[ProgressBarStyles.Answer]);
        }

        private void WritePrompt(BufferScreen screenBuffer)
        {
            if (!string.IsNullOrEmpty(GeneralOptions.PromptValue))
            {
                screenBuffer.Write(GeneralOptions.PromptValue, _optStyles[ProgressBarStyles.Prompt]);
            }
        }

        private void WriteError(BufferScreen screenBuffer)
        {
            if (_handlerProgressBar!.Error is not null)
            {
                screenBuffer.WriteLine(Messages.Error, _optStyles[ProgressBarStyles.Error]);
                ClearError();
            }
        }

        private ConsoleKeyInfo? WaitKeypressTimer(CancellationToken token)
        {
            while (!ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                if (_hasupdateProgress || _handlerProgressBar!.Error is not null)
                {
                    _hasupdateProgress = false;
                    return new ConsoleKeyInfo(new char(), ConsoleKey.None, false, false, false);
                }
                token.WaitHandle.WaitOne(2);
            }
            if (ConsolePlus.KeyAvailable && !token.IsCancellationRequested)
            {
                return ConsolePlus.ReadKey(true);
            }
            return null;
        }
    }
}
