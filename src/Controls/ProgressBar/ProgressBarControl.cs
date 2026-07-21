// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.ProgressBar
{
    /// <inheritdoc/>
    internal sealed class ProgressBarControl : BaseControlPrompt<StateProgress>, IProgressBarControl, IDisposable
    {
        /// <summary>
        /// ProgressBar renders automatically (spinner/progress) by firing simulated key
        /// events, so it must opt into the base "Live" resize handling. This makes the main
        /// render loop detect terminal size changes even before the SizeChanged event arrives
        /// and route them through the full relayout that clears the previous footprint,
        /// preventing leftover artifacts on height shrink/grow.
        /// </summary>
        protected override bool IsLiveAutoRenderControl => true;

        private readonly Dictionary<ProgressBarStyles, Style> _optStyles;
        private string[] _toggerTooptips = [];
        private int _indexTooptip;
        private Func<double, Style>? _changeColor;
        private Color[]? _changeGradient;
        private Func<double, string>? _changeDescription;
        private Func<double, Task<string>>? _changeDescriptionAsync;
        private CultureInfo _culture = CultureInfo.CurrentCulture;
        private double? _defaultValue;
        private double _maxValue = 100;
        private double _minValue;
        private byte _width;
        private ProgressBarType _progressBarType = ProgressBarType.Fill;
        private SpinnerBase? _spinner;
        private string? _finishText;
        private byte _fracionalDig;
        private HideProgressBar _hideProgressBar = HideProgressBar.None;
        private Func<ProgressBarEvent, CancellationToken, Task>? _actionProgressBarAsync;
        private IDictionary<string, object?>? _paramcontext;
        private ProgressBarEvent? _progressbarEvent;
        private double _range;
        private double _precision;
        private int _factor;
        private readonly Stopwatch _stopwatch = new();
        private char _barOn = ' ';
        private char _barOff = ' ';
        private string _valueFormat = "F0";
        private Color[]? _gradientColors;
        private bool _disposed;
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _actionProgressBarTask;
        private const int WaitLoopIntervalMs = 16;
        private const int ResizeStabilizationWindowMs = 150;
        private int _lastObservedWidth;
        private int _lastObservedHeight;
        private long _suppressRenderUntilTick;

        public ProgressBarControl(bool isWidget, IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(isWidget, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<ProgressBarStyles>(console.CurrentStyle);
            _culture = ConfigPrompt.DefaultCulture;
            _width = ConfigPrompt.ProgressBarWidth;

        }

        #region IDisposable

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                FinalizeControl();
            }
        }

        #endregion

        #region IProgressBarControl

        /// <inheritdoc/>
        public IProgressBarControl ChangeColor(Func<double, Style> value)
        {
            _changeColor = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            _changeGradient = null;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl ChangeDescription(Func<double, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl ChangeDescriptionAsync(Func<double, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl ChangeGradient(params Color[] colors)
        {
            _changeGradient = colors ?? throw new ArgumentNullException(nameof(colors), "The value cannot be null.");
            _changeColor = null;
            return this;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IProgressBarControl Default(double value)
        {
            _defaultValue = value;
            return this;
        }


        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IProgressBarControl Fill(ProgressBarType type)
        {
            _progressBarType = type;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl Finish(string finishtext)
        {
            _finishText = finishtext;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl FractionalDigits(byte value)
        {
            _fracionalDig = value;
            return _fracionalDig > 5 ? throw new ArgumentOutOfRangeException(nameof(value), "FractionalDigits must be less than 5") : this;
        }

        /// <inheritdoc/>
        public IProgressBarControl HideElements(HideProgressBar value)
        {
            _hideProgressBar = value;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl Spinner(SpinnersType spinnersType)
        {
            if (!ConsoleHandler.SupportsUnicode)
            {
                _spinner = SpinnerBase.Known.Ascii;
                return this;
            }
            _spinner = SpinnerBase.Known.FromType(spinnersType);
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl Styles(ProgressBarStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl UpdateHandler(Action<ProgressBarEvent, CancellationToken> value, IDictionary<string, object?>? paramcontext = null)
        {
            ArgumentNullException.ThrowIfNull(value);
            _actionProgressBarAsync = (progressBarEvent, cancellationToken) =>
            {
                value(progressBarEvent, cancellationToken);
                return Task.CompletedTask;
            };
            _paramcontext = paramcontext;
            return this;
        }

        /// <inheritdoc/>
        public IProgressBarControl UpdateHandlerAsync(Func<ProgressBarEvent, CancellationToken, Task> value, IDictionary<string, object?>? context = null)
        {
            _actionProgressBarAsync = value ?? throw new ArgumentNullException(nameof(value), "The value cannot be null.");
            _paramcontext = context;
            return this;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
            if (_actionProgressBarAsync is null)
            {
                throw new InvalidOperationException("The UpdateHandler cannot be null.");
            }
            _progressbarEvent = new ProgressBarEvent(_defaultValue.Value, _minValue, _maxValue, _paramcontext);

            _lastObservedWidth = ConsoleHandler.Width;
            _lastObservedHeight = ConsoleHandler.Height;
            _suppressRenderUntilTick = 0;

            _range = _maxValue - _minValue;
            if (_range < 0)
            {
                _range *= -1;
            }
            _factor = 100;
            for (int i = 0; i < _fracionalDig; i++)
            {
                _factor *= 10;
            }
            _valueFormat = $"F{_fracionalDig}";
            _precision = _range / _factor;

            SetupCharBar();

            // Precompute the per-column gradient once. It only depends on the (now fixed)
            // width and colors, so it never changes during the render loop.
            _gradientColors = _changeGradient is null ? null : BuildGradient(_width, _changeGradient);

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cancellationTokenSource.Token.Register(() =>
            {
                _progressbarEvent.Abort();
            });

            LoadTooltipToggle();

            _stopwatch.Restart();

            _actionProgressBarTask = Task.Factory
                .StartNew(() => ExecuteProgressHandlerAsync(_cancellationTokenSource.Token),
                    _cancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default)
                .Unwrap();
        }

        /// <inheritdoc/>
        public override ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken token)
        {
            var elaptimer = new Stopwatch();
            elaptimer.Start();
            while (!ConsoleHandler.KeyAvailable && !token.IsCancellationRequested)
            {
                ObserveResizeAndDebounce();

                bool sizeChangedButEventPending = ConsoleHandler.Width != _lastObservedWidth
                    || ConsoleHandler.Height != _lastObservedHeight;

                bool resizeInProgress = IsPendingResize
                    || sizeChangedButEventPending
                    || Environment.TickCount64 < _suppressRenderUntilTick;

                if (resizeInProgress)
                {
                    // While the terminal is actively changing size, suppress all automatic
                    // wake-up renders. Once dimensions settle past the debounce window, request
                    // a full relayout (clears the previous footprint) and return default so the
                    // TryResult loop breaks and the main render loop performs it. This prevents
                    // leftover artifacts on height shrink/grow for Live auto-render controls.
                    if (Environment.TickCount64 >= _suppressRenderUntilTick)
                    {
                        RequestResizeRelayout();
                        return default;
                    }
                    token.WaitHandle.WaitOne(WaitLoopIntervalMs);
                    continue;
                }

                bool isUpdatedSpinner = _spinner != null && elaptimer.Elapsed >= _spinner.Interval;
                bool hasUpdatedProgress = _progressbarEvent?.HasChange() ?? false;
                bool hasError = _progressbarEvent!.Error is not null;

                if ((isUpdatedSpinner || hasUpdatedProgress || hasError) && !IsPendingResize)
                {
                    var wakeUpCondition = GetWakeUpCondition(hasError, hasUpdatedProgress, isUpdatedSpinner);
                    if (wakeUpCondition is WaitWakeUpCondition.SpinnerOnly or WaitWakeUpCondition.SpinnerAndProgress)
                    {
                        //reset elaptimer to wait next frame so that spinner
                        _spinner?.NextFrame();
                        elaptimer.Restart();
                    }

                    ConsoleKeyInfo resut = CreateWakeUpKeyInfo(wakeUpCondition);
                    return resut;
                }
                token.WaitHandle.WaitOne(WaitLoopIntervalMs);
            }
            return ConsoleHandler.KeyAvailable && !token.IsCancellationRequested ? ConsoleHandler.ReadKey(intercept) : default;
        }

        /// <inheritdoc/>
        public override bool TryResult(CancellationToken cancellationToken)
        {
            bool oldcursor = ConsoleHandler.CursorVisible;
            ConsoleHandler.CursorVisible = false;
            try
            {
                ResultCtrl = null;
                while (!cancellationToken.IsCancellationRequested)
                {
                    KeyPressResult press = ReadNextKey(true, cancellationToken);

                    if (press.IsResize || press.IsCancelled)
                    {
                        if (!press.IsResize)
                        {
                            _indexTooptip = 0;
                            ResultCtrl = new ResultPrompt<StateProgress>(default!, true);

                        }
                        // On resize, break so the main render loop performs the full relayout
                        // (clears the old footprint) instead of rendering at the stale anchor.
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;
                    var wakeUpCondition = GetWakeUpCondition(keyinfo);


                    if (_progressbarEvent!.Finish)
                    {
                        SetResultAndCancel(isAborted: false);
                        break;
                    }

                    if (IsRenderWakeUpCondition(wakeUpCondition))
                    {
                        break;
                    }

                    if (IsAbortKeyPress(keyinfo) || wakeUpCondition is WaitWakeUpCondition.Error || _progressbarEvent!.Error is not null)
                    {
                        _indexTooptip = 0;
                        SetResultAndCancel(isAborted: true);
                        break;
                    }

                    if (!IsPendingResize && IsTooltipToggerKeyPress(keyinfo))
                    {
                        _indexTooptip++;
                        if (_indexTooptip > _toggerTooptips.Length)
                        {
                            _indexTooptip = 0;
                        }
                        break;
                    }

                    if (!IsPendingResize && CheckTooltipShowHideKeyPress(keyinfo))
                    {
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

        private void ObserveResizeAndDebounce()
        {
            int currentWidth = ConsoleHandler.Width;
            int currentHeight = ConsoleHandler.Height;
            if (currentWidth != _lastObservedWidth || currentHeight != _lastObservedHeight)
            {
                _lastObservedWidth = currentWidth;
                _lastObservedHeight = currentHeight;
                _suppressRenderUntilTick = Environment.TickCount64 + ResizeStabilizationWindowMs;
            }
        }

        private static bool IsRenderWakeUpCondition(WaitWakeUpCondition condition)
        {
            return condition is WaitWakeUpCondition.ProgressOnly
                or WaitWakeUpCondition.SpinnerOnly
                or WaitWakeUpCondition.SpinnerAndProgress;
        }

        private void SetResultAndCancel(bool isAborted)
        {
            ResultCtrl = new ResultPrompt<StateProgress>(CreateCurrentStateProgress(), isAborted);
            _cancellationTokenSource?.Cancel();
        }

        private StateProgress CreateCurrentStateProgress()
        {
            return new StateProgress(
                _progressbarEvent!.Value,
                _finishText,
                _minValue,
                _maxValue,
                _stopwatch.Elapsed,
                _progressbarEvent.OutputContext,
                _progressbarEvent.Error);
        }

        /// <inheritdoc/>
        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            if (!_hideProgressBar.HasFlag(HideProgressBar.PromptAnswer))
            {
                WritePrompt(screenBuffer, _optStyles[ProgressBarStyles.Prompt]);

                WriteAnswer(screenBuffer);
            }

            WriteDescription(screenBuffer);

            WriteProgressBar(screenBuffer);

            WriteTooltip(screenBuffer);

            WriteError(screenBuffer, _optStyles[ProgressBarStyles.Prompt]);

            // Keep an explicit cursor anchor inside the current frame so the base resize
            // logic can accurately recover the frame top after terminal reflow/scroll.
            screenBuffer.SavePromptCursor();
        }

        /// <inheritdoc/>
        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _stopwatch.Stop();
            var hasoutput = false;
            if (!_hideProgressBar.HasFlag(HideProgressBar.PromptAnswer))
            {
                hasoutput = true;
                var aux = (ResultCtrl!.Value.Content.FinishedValue!.Value - _progressbarEvent!.Minvalue) / (_progressbarEvent!.Maxvalue - _progressbarEvent!.Minvalue) * 100;
                string answer = $"{ValueToString(aux)}% - {ResultCtrl!.Value.Content.ElapsedTime:hh\\:mm\\:ss\\:ff}"; ;
                Style styleanswer = _optStyles[ProgressBarStyles.Answer];
                if (ResultCtrl!.Value.IsAborted)
                {
                    if (OptionsControl.ShowMessageAbortKeyValue)
                    {
                        answer = PromptPlusResources.CanceledKey;
                    }
                    if (ResultCtrl!.Value.Content.ExceptionProgress is not null)
                    {
                        answer = PromptPlusResources.Error;
                        styleanswer = _optStyles[ProgressBarStyles.Error];
                    }
                }
                else if (!string.IsNullOrEmpty(_finishText))
                {
                    answer = _finishText;
                }
                if (!string.IsNullOrEmpty(OptionsControl.PromptValue))
                {
                    screenBuffer.Write(OptionsControl.PromptValue, _optStyles[ProgressBarStyles.Prompt]);
                }
                screenBuffer.WriteLine(answer, styleanswer);
            }
            if (!_hideProgressBar.HasFlag(HideProgressBar.ProgressbarAtFinish))
            {
                hasoutput = true;
                WriteProgressBar(screenBuffer);
            }
            if (!hasoutput)
            {
                return false;
            }
            return true;
        }

        /// <inheritdoc/>
        public override void FinalizeControl()
        {
            if (!_disposed)
            {
                _disposed = true;
                if (_cancellationTokenSource is { IsCancellationRequested: false })
                {
                    _cancellationTokenSource.Cancel();
                }

                try
                {
                    _actionProgressBarTask?.GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    _progressbarEvent?.ErrorAndAbort(ex);
                }

                _cancellationTokenSource?.Dispose();
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string? tooltip = GetTooltipToggle();
            var renderTooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!renderTooltip.EndsWith('.'))
            {
                renderTooltip = $"{renderTooltip}.";
            }
            screenBuffer.WriteLine(renderTooltip, _optStyles[ProgressBarStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            if (_indexTooptip >= _toggerTooptips.Length)
            {
                _indexTooptip = 0;
            }
            return _toggerTooptips[_indexTooptip];
        }

        private void WriteProgressBar(BufferScreen screenBuffer)
        {
            double currentvalue = _progressbarEvent!.Value;
            if (!_hideProgressBar.HasFlag(HideProgressBar.Range))
            {
                screenBuffer.Write($"{ValueToString(_progressbarEvent.Minvalue)} ", _optStyles[ProgressBarStyles.Ranger]);
            }

            string delimitbar = GetSymbol(SymbolType.GridSingleDividerY);
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
                Color[] gradient = _gradientColors ??= BuildGradient(_width, _changeGradient);
                string bar = _barOn.ToString();
                bool isSpaceBar = _barOn == ' ';
                int count = Math.Min(valuestep, gradient.Length);
                for (int i = 0; i < count; i++)
                {
                    Color color = gradient[i];
                    screenBuffer.Write(bar, isSpaceBar ? OnStyle.Background(color) : OnStyle.ForeGround(color));
                }
            }
            if (offlength > 0)
            {
                screenBuffer.Write(new string(_barOff, offlength), ConsoleHandler.CurrentStyle.ForeGround(_optStyles[ProgressBarStyles.Slider].Background));
            }
            if (!_hideProgressBar.HasFlag(HideProgressBar.Delimit))
            {
                screenBuffer.Write(delimitbar, _optStyles[ProgressBarStyles.Ranger]);
            }
            if (!_hideProgressBar.HasFlag(HideProgressBar.Range))
            {
                screenBuffer.Write($" {ValueToString(_progressbarEvent.Maxvalue)}", _optStyles[ProgressBarStyles.Ranger]);
            }
            screenBuffer.WriteLine("", _optStyles[ProgressBarStyles.Prompt]);
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
            // could reach 100 * 10^fracionalDig iterations per frame).
            double qtd = _precision > 0 ? Math.Ceiling((value - _minValue) / _precision) : 0;
            double perc = qtd / _factor;
            return (int)Math.Round(_width * perc, _fracionalDig);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            if (_changeDescriptionAsync is not null)
            {
                desc = _changeDescriptionAsync.Invoke(_progressbarEvent!.Value)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                desc = _changeDescription?.Invoke(_progressbarEvent!.Value) ?? OptionsControl.DescriptionValue;
            }
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[ProgressBarStyles.Description]);
            }
        }

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            var aux = (_progressbarEvent!.Value - _progressbarEvent!.Minvalue) / (_progressbarEvent!.Maxvalue - _progressbarEvent!.Minvalue) * 100;
            string answer = ValueToString(aux);
            screenBuffer.Write($"{answer} %", _optStyles[ProgressBarStyles.Answer]);
            if (!_hideProgressBar.HasFlag(HideProgressBar.ElapsedTime))
            {
                screenBuffer.Write($" - {_stopwatch.Elapsed:hh\\:mm\\:ss\\:ff}", _optStyles[ProgressBarStyles.Answer]);
            }
            if (_spinner != null)
            {
                screenBuffer.Write($" {_spinner.CurrentFrame} ", _optStyles[ProgressBarStyles.Spinner]);
            }
            screenBuffer.WriteLine("", _optStyles[ProgressBarStyles.Answer]);
        }

        private string ValueToString(double value)
        {
            // Fixed-point format (no group separators). The format string is cached in InitControl
            // to avoid re-allocating it on every render.
            return Math.Round(value, _fracionalDig).ToString(_valueFormat, _culture);
        }



        private enum WaitWakeUpCondition
        {
            None,
            Error,
            ProgressOnly,
            SpinnerOnly,
            SpinnerAndProgress
        }

        private static WaitWakeUpCondition GetWakeUpCondition(bool hasError, bool hasProgressUpdate, bool hasSpinnerUpdate)
        {
            if (hasError)
            {
                return WaitWakeUpCondition.Error;
            }

            if (hasProgressUpdate && hasSpinnerUpdate)
            {
                return WaitWakeUpCondition.SpinnerAndProgress;
            }

            if (hasProgressUpdate)
            {
                return WaitWakeUpCondition.ProgressOnly;
            }

            if (hasSpinnerUpdate)
            {
                return WaitWakeUpCondition.SpinnerOnly;
            }

            return WaitWakeUpCondition.None;
        }

        private static ConsoleKeyInfo CreateWakeUpKeyInfo(WaitWakeUpCondition condition)
        {
            // keychar(1)/ConsoleKey.None + modifiers are used as internal wake-up signaling for TryResult
            return condition switch
            {
                WaitWakeUpCondition.Error => new ConsoleKeyInfo((char)1, ConsoleKey.None, true, false, true),
                WaitWakeUpCondition.ProgressOnly => new ConsoleKeyInfo((char)1, ConsoleKey.None, false, false, true),
                WaitWakeUpCondition.SpinnerOnly => new ConsoleKeyInfo((char)1, ConsoleKey.None, false, true, false),
                WaitWakeUpCondition.SpinnerAndProgress => new ConsoleKeyInfo((char)1, ConsoleKey.None, false, true, true),
                _ => default
            };
        }

        private static WaitWakeUpCondition GetWakeUpCondition(ConsoleKeyInfo keyInfo)
        {
            // Internal wake-up key generated by WaitKeypress
            if (keyInfo.KeyChar != (char)1 || keyInfo.Key != ConsoleKey.None)
            {
                return WaitWakeUpCondition.None;
            }

            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift) && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                return WaitWakeUpCondition.Error;
            }

            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt) && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                return WaitWakeUpCondition.SpinnerAndProgress;
            }

            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                return WaitWakeUpCondition.SpinnerOnly;
            }

            if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                return WaitWakeUpCondition.ProgressOnly;
            }

            return WaitWakeUpCondition.None;
        }

        private async Task ExecuteProgressHandlerAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _actionProgressBarAsync!(_progressbarEvent!, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                //nothing to do, just exit the method
            }
            catch (Exception ex)
            {
                _progressbarEvent?.ErrorAndAbort(ex);
            }
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips =[];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
        }

        private void SetupCharBar()
        {
            switch (_progressBarType)
            {
                case ProgressBarType.Fill:
                    _barOff = _barOn;
                    break;
                case ProgressBarType.Light:
                    _barOn = GetSymbol(SymbolType.ProgressBarLight)[0];
                    break;
                case ProgressBarType.DoubleLight:
                    _barOn = GetSymbol(SymbolType.ProgressBarDoubleLight)[0];
                    break;
                case ProgressBarType.Square:
                    _barOn = GetSymbol(SymbolType.ProgressBarSquare)[0];
                    break;
                case ProgressBarType.Bar:
                    _barOn = GetSymbol(SymbolType.ProgressBarBar)[0];
                    break;
                case ProgressBarType.Dot:
                    _barOn = GetSymbol(SymbolType.ProgressBarDot)[0];
                    break;
                default:
                    throw new NotImplementedException($"BarType: {_progressBarType} Not Implemented");
            }
        }

    }
}
