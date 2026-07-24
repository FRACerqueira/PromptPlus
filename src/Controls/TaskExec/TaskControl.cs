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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.TaskExec
{
    /// <inheritdoc/>
    internal sealed class TaskControl : BaseControlPrompt<StateTask>, ITaskControl, IDisposable
    {
        /// <summary>
        /// The Task control renders automatically (elapsed time / spinner ticking) by firing
        /// simulated key events, so it must opt into the base "Live" resize handling. This makes
        /// the main render loop detect terminal size changes even before the SizeChanged event
        /// arrives and route them through the full relayout that clears the previous footprint,
        /// preventing leftover artifacts on height shrink/grow.
        /// </summary>
        protected override bool IsLiveAutoRenderControl => true;

        private const int WaitLoopIntervalMs = 16;
        private const int ResizeStabilizationWindowMs = 150;
        private const int TickIntervalMs = 100;

        private readonly Dictionary<TaskStyles, Style> _optStyles;
        private readonly Stopwatch _stopwatch = new();
        private readonly Stopwatch _spinnerTimer = new();
        private CultureInfo _culture;
        private SpinnerBase? _spinner;
        private bool _showElapsedTime;
        private string _elapsedFormat = @"hh\:mm\:ss";
        private string? _finishText;
        private string? _finishErrorText;
        private Func<TimeSpan, string>? _changeDescription;
        private Func<TimeSpan, Task<string>>? _changeDescriptionAsync;
        private IDictionary<string, object?>? _inputContext;
        private Dictionary<string, object?> _outputContext = [];
        private Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>>? _actionAsync;

        private string[] _toggerTooptips = [];
        private int _indexTooptip;
        private int _lastObservedWidth;
        private int _lastObservedHeight;
        private long _suppressRenderUntilTick;

        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _executionTask;
        private volatile bool _completed;
        private volatile bool _cancelledByHandler;
        private Exception? _error;
        private bool _disposed;

        public TaskControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<TaskStyles>(console.CurrentStyle);
            _culture = ConfigPrompt.DefaultCulture;
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

        #region ITaskControl

        /// <inheritdoc/>
        public ITaskControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl Styles(TaskStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl Culture(CultureInfo culture)
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
        public ITaskControl ShowElapsedTime(bool value = true, string? format = null)
        {
            _showElapsedTime = value;
            if (!string.IsNullOrWhiteSpace(format))
            {
                _elapsedFormat = format!;
            }
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl Spinner(SpinnersType spinnersType)
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
        public ITaskControl Finish(string finishtext, string? errortext = null)
        {
            _finishText = finishtext;
            _finishErrorText = errortext;
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl ChangeDescription(Func<TimeSpan, string> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescription = value;
            _changeDescriptionAsync = null;
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl ChangeDescriptionAsync(Func<TimeSpan, Task<string>> value)
        {
            ArgumentNullException.ThrowIfNull(value);
            _changeDescriptionAsync = value;
            _changeDescription = null;
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl Context(IDictionary<string, object?> context)
        {
            ArgumentNullException.ThrowIfNull(context);
            // Copy to isolate the input context from external mutation during execution.
            _inputContext = new Dictionary<string, object?>(context);
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl Action(Func<IReadOnlyDictionary<string, object?>, CancellationToken, IDictionary<string, object?>?> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _actionAsync = (input, token) => Task.FromResult(handler(input, token));
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl Action(Func<CancellationToken, IDictionary<string, object?>?> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _actionAsync = (_, token) => Task.FromResult(handler(token));
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl Action(Action<CancellationToken> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _actionAsync = (_, token) =>
            {
                handler(token);
                return Task.FromResult<IDictionary<string, object?>?>(null);
            };
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl ActionAsync(Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _actionAsync = handler;
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl ActionAsync(Func<CancellationToken, Task<IDictionary<string, object?>?>> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _actionAsync = (_, token) => handler(token);
            return this;
        }

        /// <inheritdoc/>
        public ITaskControl ActionAsync(Func<CancellationToken, Task> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);
            _actionAsync = async (_, token) =>
            {
                await handler(token).ConfigureAwait(false);
                return null;
            };
            return this;
        }

        #endregion

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_actionAsync is null)
            {
                throw new InvalidOperationException("A task Action or ActionAsync must be provided.");
            }

            _lastObservedWidth = ConsoleHandler.Width;
            _lastObservedHeight = ConsoleHandler.Height;
            _suppressRenderUntilTick = 0;
            _completed = false;
            _cancelledByHandler = false;
            _error = null;

            LoadTooltipToggle();

            _stopwatch.Restart();
            _spinnerTimer.Restart();

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            IReadOnlyDictionary<string, object?> input =
                new ReadOnlyDictionary<string, object?>(_inputContext ?? new Dictionary<string, object?>());

            _executionTask = Task.Factory
                .StartNew(() => ExecuteHandlerAsync(input, _cancellationTokenSource.Token),
                    _cancellationTokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default)
                .Unwrap();
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

                // Task finished (or errored): wake up so TryResult can complete the control.
                if (_completed)
                {
                    return CreateWakeUpKeyInfo(finished: true);
                }

                // Advance the spinner frame when its interval elapses so it animates at its own pace.
                if (_spinner != null && _spinnerTimer.Elapsed >= _spinner.Interval)
                {
                    _spinner.NextFrame();
                    _spinnerTimer.Restart();
                }

                // Regular tick: wake up to repaint the elapsed time / spinner.
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
                            SetResultAndCancel(isAborted: true);
                        }
                        // On resize, break so the main render loop performs the full relayout.
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    // Task finished (internal wake-up signaled completion).
                    if (IsFinishedWakeUp(keyinfo) || _completed)
                    {
                        _indexTooptip = 0;
                        SetResultAndCancel(isAborted: _error is not null || _cancelledByHandler);
                        break;
                    }

                    // Internal tick wake-up: repaint elapsed time / spinner.
                    if (IsTickWakeUp(keyinfo))
                    {
                        // Throttle repaints; the wait is interrupted immediately by Esc/Ctrl+C
                        // (cancellationToken) so cancellation stays fully responsive.
                        cancellationToken.WaitHandle.WaitOne(TickIntervalMs);
                        break;
                    }

                    if (IsAbortKeyPress(keyinfo))
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

        public override void BufferTemplate(BufferScreen screenBuffer)
        {
            WritePrompt(screenBuffer, _optStyles[TaskStyles.Prompt]);

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

            WritePrompt(screenBuffer, _optStyles[TaskStyles.Prompt]);

            string answer;
            Style styleanswer = _optStyles[TaskStyles.Answer];
            if (ResultCtrl!.Value.IsAborted)
            {
                if (_error is not null)
                {
                    answer = !string.IsNullOrEmpty(_finishErrorText) ? _finishErrorText! : PromptPlusResources.Error;
                    styleanswer = _optStyles[TaskStyles.Error];
                }
                else
                {
                    answer = OptionsControl.ShowMessageAbortKeyValue ? PromptPlusResources.CanceledKey : string.Empty;
                }
            }
            else if (!string.IsNullOrEmpty(_finishText))
            {
                answer = _finishText!;
            }
            else
            {
                answer = FormatElapsed();
            }

            screenBuffer.WriteLine(answer, styleanswer);
            return true;
        }

        public override void FinalizeControl()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;

            if (_cancellationTokenSource is { IsCancellationRequested: false })
            {
                _cancellationTokenSource.Cancel();
            }

            try
            {
                _executionTask?.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _error ??= ex;
            }

            _cancellationTokenSource?.Dispose();
        }

        private async Task ExecuteHandlerAsync(IReadOnlyDictionary<string, object?> input, CancellationToken token)
        {
            try
            {
                IDictionary<string, object?>? output = await _actionAsync!(input, token).ConfigureAwait(false);
                // Isolate the output context from external mutation after execution.
                _outputContext = output is null
                    ? []
                    : new Dictionary<string, object?>(output);
            }
            catch (OperationCanceledException)
            {
                // Cancellation is not treated as an error, but it IS an abort — the run must not
                // be reported as a successful completion (IsAborted=false) just because no
                // exception was recorded.
                _cancelledByHandler = true;
            }
            catch (Exception ex)
            {
                _error = ex;
            }
            finally
            {
                _completed = true;
            }
        }

        private void SetResultAndCancel(bool isAborted)
        {
            var state = new StateTask(
                _stopwatch.Elapsed,
                new ReadOnlyDictionary<string, object?>(_outputContext),
                _error);
            ResultCtrl = new ResultPrompt<StateTask>(state, isAborted);
            _cancellationTokenSource?.Cancel();
        }

        private string FormatElapsed() => _stopwatch.Elapsed.ToString(_elapsedFormat, _culture);

        private void WriteAnswer(BufferScreen screenBuffer)
        {
            bool hasContent = false;
            if (_showElapsedTime)
            {
                screenBuffer.Write(FormatElapsed(), _optStyles[TaskStyles.ElapsedTime]);
                hasContent = true;
            }
            if (_spinner != null)
            {
                screenBuffer.Write(hasContent ? $" {_spinner.CurrentFrame}" : _spinner.CurrentFrame, _optStyles[TaskStyles.Spinner]);
                hasContent = true;
            }
            screenBuffer.SavePromptCursor();
            // Always terminate the line, even with no elapsed time/spinner to show — otherwise the
            // cursor stays on the prompt's row and WriteDescription/WriteTooltip get appended to it
            // instead of starting their own rows.
            screenBuffer.WriteLine("", _optStyles[TaskStyles.Answer]);
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc;
            if (_changeDescriptionAsync is not null)
            {
                desc = _changeDescriptionAsync.Invoke(_stopwatch.Elapsed)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            }
            else
            {
                desc = _changeDescription?.Invoke(_stopwatch.Elapsed) ?? OptionsControl.DescriptionValue;
            }

            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[TaskStyles.Description]);
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
            screenBuffer.WriteLine(tooltip, _optStyles[TaskStyles.Tooltips]);
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
            // Entries are cycled with the tooltip toggle hotkey (F1) and the whole block is
            // shown/hidden with the show-hide hotkey (Ctrl+F1).
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
