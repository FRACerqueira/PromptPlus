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

namespace PromptPlusLibrary.Controls.Time
{
    /// <inheritdoc/>
    internal sealed class TimeControl : BaseControlPrompt<TimeSpan>, ITimeControl
    {
        /// <summary>
        /// The Time control renders automatically (countdown ticking) by firing simulated key
        /// events, so it must opt into the base "Live" resize handling. This makes the main
        /// render loop detect terminal size changes even before the SizeChanged event arrives
        /// and route them through the full relayout that clears the previous footprint,
        /// preventing leftover artifacts on height shrink/grow.
        /// </summary>
        protected override bool IsLiveAutoRenderControl => true;

        private const int WaitLoopIntervalMs = 16;
        private const int ResizeStabilizationWindowMs = 150;

        /// <summary>
        /// Repaint throttle interval, in milliseconds. Set to the smallest interval among all
        /// registered spinners so the fastest spinner still animates smoothly, while never going
        /// below the base render-loop polling interval (<see cref="WaitLoopIntervalMs"/>) to keep
        /// CPU usage low.
        /// </summary>
        private const int TickIntervalMs = 17;

        private readonly Dictionary<TimeStyles, Style> _optStyles;
        private readonly Stopwatch _stopwatch = new();
        private readonly Stopwatch _spinnerTimer = new();
        private CultureInfo _culture;
        private TimeSpan _duration = TimeSpan.Zero;
        private string _format = @"hh\:mm\:ss";
        private TimeDisplayMode _displayMode = TimeDisplayMode.Countdown;
        private string? _finishText;
        private SpinnerBase? _spinner;
        private Func<TimeSpan, string>? _changeDescription;
        private Func<TimeSpan, Task<string>>? _changeDescriptionAsync;
        private string[] _toggerTooptips = [];
        private int _indexTooptip;
        private int _lastObservedWidth;
        private int _lastObservedHeight;
        private long _suppressRenderUntilTick;

        public TimeControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<TimeStyles>(console.CurrentStyle);
            _culture = ConfigPrompt.DefaultCulture;
        }

        #region ITimeControl

        /// <inheritdoc/>
        public ITimeControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl Styles(TimeStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl Duration(TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be greater than zero.");
            }
            _duration = duration;
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl Duration(int seconds)
        {
            if (seconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), "Duration must be greater than zero.");
            }
            _duration = TimeSpan.FromSeconds(seconds);
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl Format(string format)
        {
            ArgumentNullException.ThrowIfNull(format);
            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException("Format cannot be empty or whitespace.", nameof(format));
            }
            _format = format;
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl Culture(CultureInfo culture)
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
        public ITimeControl Finish(string finishtext)
        {
            _finishText = finishtext;
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl ChangeDescription(Func<TimeSpan, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl ChangeDescriptionAsync(Func<TimeSpan, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl DisplayMode(TimeDisplayMode mode)
        {
            _displayMode = mode;
            return this;
        }

        /// <inheritdoc/>
        public ITimeControl Spinner(SpinnersType spinnersType)
        {
            if (!ConsoleHandler.SupportsUnicode)
            {
                _spinner = SpinnerBase.Known.Ascii;
                return this;
            }
            _spinner = SpinnerBase.Known.FromType(spinnersType);
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            _lastObservedWidth = ConsoleHandler.Width;
            _lastObservedHeight = ConsoleHandler.Height;
            _suppressRenderUntilTick = 0;

            LoadTooltipToggle();

            _stopwatch.Restart();
            _spinnerTimer.Restart();
        }

        /// <inheritdoc/>
        public override ConsoleKeyInfo WaitKeypress(bool intercept, CancellationToken token)
        {
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
                    // a full relayout and return default so the TryResult loop breaks and the main
                    // render loop performs it. This prevents leftover artifacts on resize.
                    if (Environment.TickCount64 >= _suppressRenderUntilTick)
                    {
                        RequestResizeRelayout();
                        return default;
                    }
                    token.WaitHandle.WaitOne(WaitLoopIntervalMs);
                    continue;
                }

                // Countdown finished: wake up so TryResult can complete the control.
                if (_stopwatch.Elapsed >= _duration)
                {
                    return CreateWakeUpKeyInfo(finished: true);
                }

                // Advance the spinner frame when its interval elapses so it animates at its own pace.
                if (_spinner != null && _spinnerTimer.Elapsed >= _spinner.Interval)
                {
                    _spinner.NextFrame();
                    _spinnerTimer.Restart();
                }

                // Regular tick: wake up to repaint the remaining time.
                return CreateWakeUpKeyInfo(finished: false);
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
                            ResultCtrl = new ResultPrompt<TimeSpan>(_stopwatch.Elapsed, true);
                        }
                        // On resize, break so the main render loop performs the full relayout.
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    // Countdown finished (internal wake-up signaled completion).
                    if (IsFinishedWakeUp(keyinfo) || _stopwatch.Elapsed >= _duration)
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<TimeSpan>(_duration, false);
                        break;
                    }

                    // Internal tick wake-up: repaint the remaining time.
                    if (IsTickWakeUp(keyinfo))
                    {
                        // Throttle the repaint frequency to the tick interval.
                        TimeControl.TokenWaitTick(cancellationToken);
                        break;
                    }

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        ResultCtrl = new ResultPrompt<TimeSpan>(_stopwatch.Elapsed, true);
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

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[TimeStyles.Prompt]);

            WriteAnswer(screenBuffer);

            WriteDescription(screenBuffer);

            WriteTooltip(screenBuffer);

            // Keep an explicit cursor anchor inside the current frame so the base resize
            // logic can accurately recover the frame top after terminal reflow/scroll.
            screenBuffer.SavePromptCursor();
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _stopwatch.Stop();

            WritePrompt(screenBuffer, _optStyles[TimeStyles.Prompt]);

            string answer;
            Style styleanswer = _optStyles[TimeStyles.Answer];
            if (ResultCtrl!.Value.IsAborted)
            {
                answer = OptionsControl.EnabledAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
            }
            else if (!string.IsNullOrEmpty(_finishText))
            {
                answer = _finishText!;
            }
            else
            {
                answer = FormatTime(DisplayValue());
            }

            screenBuffer.WriteLine(answer, styleanswer);
            return true;
        }

        public override void FinalizeControl()
        {
            // none
        }

        private TimeSpan Remaining()
        {
            TimeSpan remaining = _duration - _stopwatch.Elapsed;
            return remaining < TimeSpan.Zero ? TimeSpan.Zero : remaining;
        }

        private TimeSpan Elapsed()
        {
            TimeSpan elapsed = _stopwatch.Elapsed;
            return elapsed > _duration ? _duration : elapsed;
        }

        /// <summary>
        /// Gets the time value to display according to the configured <see cref="TimeDisplayMode"/>.
        /// </summary>
        private TimeSpan DisplayValue()
            => _displayMode == TimeDisplayMode.Elapsed ? Elapsed() : Remaining();

        private string FormatTime(TimeSpan value) => value.ToString(_format, _culture);

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            string answer = FormatTime(DisplayValue());
            screenBuffer.Write(answer, _optStyles[TimeStyles.Answer]);
            if (_spinner != null)
            {
                screenBuffer.Write($" {_spinner.CurrentFrame}", _optStyles[TimeStyles.Spinner]);
            }
            screenBuffer.SavePromptCursor();
            screenBuffer.WriteLine("", _optStyles[TimeStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc;
            if (_changeDescriptionAsync is not null)
            {
                desc = _changeDescriptionAsync.Invoke(DisplayValue())
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                desc = _changeDescription?.Invoke(DisplayValue()) ?? OptionsControl.DescriptionValue;
            }

            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[TimeStyles.Description]);
            }
        }

        private void WriteTooltip(BufferScreen screenBuffer)
        {
            if (!IsShowTooltip)
            {
                return;
            }
            string tooltip = GetTooltipToggle();
            tooltip = $"{ConfigPrompt.HotKeyTooltip}:{PromptPlusResources.TooltipBase}.{tooltip}";
            if (!tooltip.EndsWith('.'))
            {
                tooltip = $"{tooltip}.";
            }
            screenBuffer.WriteLine(tooltip, _optStyles[TimeStyles.Tooltips]);
        }

        private string GetTooltipToggle()
        {
            if (_indexTooptip >= _toggerTooptips.Length)
            {
                _indexTooptip = 0;
            }
            return _toggerTooptips[_indexTooptip];
        }

        private void LoadTooltipToggle()
        {
            List<string> lsttooltips = [];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
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

        private static void TokenWaitTick(CancellationToken token)
        {
            // Sleep a tick between repaints so the countdown updates at a steady rate
            // instead of spinning the render loop.
            token.WaitHandle.WaitOne(TickIntervalMs);
        }

        // keychar(1)/ConsoleKey.None + modifiers are used as internal wake-up signaling for TryResult.
        private static ConsoleKeyInfo CreateWakeUpKeyInfo(bool finished)
            => finished
                ? new ConsoleKeyInfo((char)1, ConsoleKey.None, true, false, true)
                : new ConsoleKeyInfo((char)1, ConsoleKey.None, false, false, true);

        private static bool IsInternalWakeUp(ConsoleKeyInfo keyInfo)
            => keyInfo.KeyChar == (char)1 && keyInfo.Key == ConsoleKey.None;

        private static bool IsFinishedWakeUp(ConsoleKeyInfo keyInfo)
            => IsInternalWakeUp(keyInfo)
               && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift)
               && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control);

        private static bool IsTickWakeUp(ConsoleKeyInfo keyInfo)
            => IsInternalWakeUp(keyInfo)
               && keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control)
               && !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift);
    }
}
