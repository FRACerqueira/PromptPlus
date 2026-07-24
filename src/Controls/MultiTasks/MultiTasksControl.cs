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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary.Controls.MultiTasks
{
    /// <inheritdoc/>
    internal sealed class MultiTasksControl : BaseControlPrompt<StateMultiTasks>, IMultiTasksControl, IDisposable
    {
        /// <summary>
        /// The MultiTasks control renders automatically (elapsed time / status ticking) by firing
        /// simulated key events, so it must opt into the base "Live" resize handling. This makes
        /// the main render loop detect terminal size changes even before the SizeChanged event
        /// arrives and route them through the full relayout that clears the previous footprint,
        /// preventing leftover artifacts on height shrink/grow.
        /// </summary>
        protected override bool IsLiveAutoRenderControl => true;

        /// <summary>
        /// Total rows the control template reserves around the tasks list: prompt+answer line,
        /// optional description line, tooltip line and an extra row for the pagination footer.
        /// </summary>
        private const int ReservedTemplateLines = 7;

        private const int WaitLoopIntervalMs = 16;
        private const int ResizeStabilizationWindowMs = 150;

        // Cached composite format strings for improved performance (CA1863).
        private static readonly CompositeFormat s_multiTasksSuccessFormat = CompositeFormat.Parse(PromptPlusResources.MultiTasksSuccessCount);
        private static readonly CompositeFormat s_multiTasksFailedFormat = CompositeFormat.Parse(PromptPlusResources.MultiTasksFailed);
        private static readonly CompositeFormat s_multiTasksWaitingFormat = CompositeFormat.Parse(PromptPlusResources.MultiTasksWaitingCount);

        private sealed class TaskItem(string uniqueId, string title, Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler, IDictionary<string, object?>? inputContext, MultiTasksMode mode)
        {
            public string UniqueId { get; } = uniqueId;
            public string Title { get; } = title;
            public Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> Handler { get; } = handler;
            public IDictionary<string, object?>? InputContext { get; } = inputContext;
            public MultiTasksMode Mode { get; } = mode;
            public volatile MultiTaskState State = MultiTaskState.Waiting;
            public readonly Stopwatch Timer = new();
            public Dictionary<string, object?> Output = [];
            public Exception? Error;
        }

        private readonly Dictionary<MultiTasksStyles, Style> _optStyles;
        private readonly Stopwatch _stopwatch = new();
        private readonly Stopwatch _spinnerTimer = new();
        private readonly List<TaskItem> _tasks = [];
        private CultureInfo _culture;
        private SpinnerBase? _spinner;
        private MultiTasksMode _defaultMode = MultiTasksMode.Sequential;
        private bool _showElapsedTime = true;
        private string _elapsedFormat = @"hh\:mm\:ss";
        private bool _stopOnError;
        private byte _pageSize;
        private int _effectivePageSize;
        private int _sequence;
        private int _maxDegreeOfParallelism = DefaultMaxParallelism();

        private Paginator<TaskItem>? _localpaginator;
        private string[] _toggerTooptips = [];
        private int _indexTooptip;
        private int _lastObservedWidth;
        private int _lastObservedHeight;
        private long _suppressRenderUntilTick;

        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _executionTask;
        private volatile bool _completed;
        private bool _disposed;

        public MultiTasksControl(IConsole console, PromptConfig promptConfig, BaseControlOptions baseControlOptions) : base(false, console, promptConfig, baseControlOptions)
        {
            _optStyles = OptionsControl.LoadStyle<MultiTasksStyles>(console.CurrentStyle);
            _culture = ConfigPrompt.DefaultCulture;
            _pageSize = ConfigPrompt.PageSize;
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

        #region IMultiTasksControl

        /// <inheritdoc/>
        public IMultiTasksControl Options(Action<IControlOptions> options)
        {
            ArgumentNullException.ThrowIfNull(options);
            options.Invoke(OptionsControl);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl Styles(MultiTasksStyles styleType, Style style)
        {
            _optStyles[styleType] = style;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl Culture(CultureInfo culture)
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
        public IMultiTasksControl Mode(MultiTasksMode mode)
        {
            _defaultMode = mode;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl ShowElapsedTime(bool value = true, string? format = null)
        {
            _showElapsedTime = value;
            if (!string.IsNullOrWhiteSpace(format))
            {
                _elapsedFormat = format!;
            }
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl Spinner(SpinnersType spinnersType)
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
        public IMultiTasksControl StopOnError(bool value = true)
        {
            _stopOnError = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl PageSize(byte value)
        {
            _pageSize = value;
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl MaxDegreeOfParallelism(int value)
        {
            _maxDegreeOfParallelism = value <= 0 ? DefaultMaxParallelism() : ClampParallelism(value);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl AddTask(string title, Func<IReadOnlyDictionary<string, object?>, CancellationToken, IDictionary<string, object?>?> handler, IDictionary<string, object?>? context = null, MultiTasksMode? mode = null)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(handler);
            AddTaskCore(title, (input, token) => Task.FromResult(handler(input, token)), context, mode);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl AddTask(string title, Action<CancellationToken> handler, MultiTasksMode? mode = null)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(handler);
            AddTaskCore(title, (_, token) =>
            {
                handler(token);
                return Task.FromResult<IDictionary<string, object?>?>(null);
            }, null, mode);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl AddTaskAsync(string title, Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler, IDictionary<string, object?>? context = null, MultiTasksMode? mode = null)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(handler);
            AddTaskCore(title, handler, context, mode);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl AddTaskAsync(string title, Func<CancellationToken, Task> handler, MultiTasksMode? mode = null)
        {
            ArgumentNullException.ThrowIfNull(title);
            ArgumentNullException.ThrowIfNull(handler);
            AddTaskCore(title, async (_, token) =>
            {
                await handler(token).ConfigureAwait(false);
                return null;
            }, null, mode);
            return this;
        }

        /// <inheritdoc/>
        public IMultiTasksControl Interaction<T>(IEnumerable<T> items, Action<T, IMultiTasksControl> interactionAction)
        {
            ArgumentNullException.ThrowIfNull(items);
            ArgumentNullException.ThrowIfNull(interactionAction);
            foreach (T item in items)
            {
                interactionAction.Invoke(item, this);
            }
            return this;
        }

        #endregion

        private void AddTaskCore(string title, Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler, IDictionary<string, object?>? context, MultiTasksMode? mode)
        {
            _sequence++;
            // Copy to isolate the per-task input context from external mutation.
            IDictionary<string, object?>? isolated = context is null ? null : new Dictionary<string, object?>(context);
            _tasks.Add(new TaskItem(_sequence.ToString(CultureInfo.CurrentCulture), title, handler, isolated, mode ?? _defaultMode));
        }

        public override void InitControl(CancellationToken cancellationToken)
        {
            if (_tasks.Count == 0)
            {
                throw new InvalidOperationException("At least one task must be added with AddTask/AddTaskAsync.");
            }

            _lastObservedWidth = ConsoleHandler.Width;
            _lastObservedHeight = ConsoleHandler.Height;
            _suppressRenderUntilTick = 0;
            _completed = false;

            _effectivePageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);

            _localpaginator = new Paginator<TaskItem>(
                FilterMode.Disabled,
                _tasks,
                _effectivePageSize,
                Optional<TaskItem>.Empty(),
                (a, b) => a.UniqueId == b.UniqueId,
                (item) => item.Title);
            _localpaginator.FirstItem();

            LoadTooltipToggle();

            _stopwatch.Restart();
            _spinnerTimer.Restart();

            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _executionTask = Task.Factory
                .StartNew(() => ExecuteAllAsync(_cancellationTokenSource.Token),
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
                    if (Environment.TickCount64 >= _suppressRenderUntilTick)
                    {
                        RequestResizeRelayout();
                        return default;
                    }
                    token.WaitHandle.WaitOne(WaitLoopIntervalMs);
                    continue;
                }

                // All tasks finished: wake up so TryResult can complete the control.
                if (_completed)
                {
                    return CreateWakeUpKeyInfo(finished: true);
                }

                // Advance the spinner frame when its interval elapses (used in the summary line
                // only while there is at least one running task).
                if (_spinner != null && _spinnerTimer.Elapsed >= _spinner.Interval)
                {
                    _spinner.NextFrame();
                    _spinnerTimer.Restart();
                }

                // Regular tick: wake up to repaint the list (elapsed time / status updates).
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
                            SetResultAndCancel(aborted: true);
                        }
                        break;
                    }

                    ConsoleKeyInfo keyinfo = press.Key;

                    // All tasks finished (internal wake-up signaled completion).
                    if (IsFinishedWakeUp(keyinfo) || _completed)
                    {
                        _indexTooptip = 0;
                        SetResultAndCancel(aborted: false);
                        break;
                    }

                    // Internal tick wake-up: repaint. Navigation keys let the user scroll the list.
                    if (IsTickWakeUp(keyinfo))
                    {
                        break;
                    }

                    if (IsAbortKeyPress(keyinfo))
                    {
                        _indexTooptip = 0;
                        SetResultAndCancel(aborted: true);
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

                    if (keyinfo.IsPressDownArrowKey())
                    {
                        if (_localpaginator!.IsLastPageItem)
                        {
                            _localpaginator.NextPage(IndexOption.FirstItem);
                        }
                        else
                        {
                            _localpaginator.NextItem();
                        }
                        break;
                    }

                    if (keyinfo.IsPressUpArrowKey())
                    {
                        if (_localpaginator!.IsFirstPageItem)
                        {
                            _localpaginator.PreviousPage(IndexOption.LastItem);
                        }
                        else
                        {
                            _localpaginator.PreviousItem();
                        }
                        break;
                    }

                    if (keyinfo.IsPressPageDownKey())
                    {
                        _localpaginator!.NextPage(IndexOption.FirstItemWhenHasPages);
                        break;
                    }

                    if (keyinfo.IsPressPageUpKey())
                    {
                        _localpaginator!.PreviousPage(IndexOption.LastItemWhenHasPages);
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
            int targetPageSize = ComputeEffectivePageSize(ReservedTemplateLines, _pageSize);
            if (targetPageSize != _effectivePageSize)
            {
                _effectivePageSize = targetPageSize;
                _localpaginator?.UpdatePageSize(_effectivePageSize);
            }

            WritePrompt(screenBuffer, _optStyles[MultiTasksStyles.Prompt]);
            WriteSummary(screenBuffer);
            WriteDescription(screenBuffer);
            WriteTasksList(screenBuffer);
            WriteTooltip(screenBuffer);

            screenBuffer.SavePromptCursor();
        }

        public override bool FinishTemplate(BufferScreen screenBuffer)
        {
            _stopwatch.Stop();

            // Compact finish frame (like other Live controls): collapse to a single summary line
            // instead of repainting the whole paginated list. Keeping the full list here would
            // leave its footprint on screen and interfere with terminal auto-scroll.
            WritePrompt(screenBuffer, _optStyles[MultiTasksStyles.Prompt]);
            WriteFinishSummary(screenBuffer);
            return true;
        }

        private void WriteFinishSummary(BufferScreen screenBuffer)
        {
            // Same three explicit counts as WriteSummary (success/failed/waiting), so the meaning
            // never changes between the running and finished frames.
            (int total, int done, int success, int failed, _) = CountStates();

            var sb = new StringBuilder();
            AppendCounts(sb, total, done, success, failed);

            Style style = failed > 0 ? _optStyles[MultiTasksStyles.FailedTask] : _optStyles[MultiTasksStyles.SuccessTask];

            if (_showElapsedTime)
            {
                sb.Append(' ');
                sb.Append(FormatElapsed(_stopwatch.Elapsed));
            }

            screenBuffer.WriteLine(sb.ToString().TrimEnd(), style);
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
            catch
            {
                // Individual task errors are recorded per-task; ignore aggregate here.
            }

            _cancellationTokenSource?.Dispose();
        }

        private async Task ExecuteAllAsync(CancellationToken token)
        {
            try
            {
                // Execute strictly in list order. Walk the list and, whenever we find consecutive
                // tasks whose mode resolves to Parallel, run them together as a sub-set; a
                // Sequential task runs alone. The run only advances to the next task/sub-set once
                // every item of the current one has finished. Order is never changed and modes are
                // never grouped globally — only CONSECUTIVE same-mode items form a sub-set.
                int i = 0;
                while (i < _tasks.Count && !token.IsCancellationRequested)
                {
                    TaskItem first = _tasks[i];

                    if (first.Mode == MultiTasksMode.Parallel)
                    {
                        // Collect the consecutive run of Parallel tasks starting at i.
                        int start = i;
                        while (i < _tasks.Count && _tasks[i].Mode == MultiTasksMode.Parallel)
                        {
                            i++;
                        }
                        await RunParallelSubsetAsync(start, i - start, token).ConfigureAwait(false);
                    }
                    else
                    {
                        // Single sequential task.
                        await RunTaskAsync(first, token).ConfigureAwait(false);
                        i++;

                        // StopOnError only affects sequential steps: abort the whole remaining run.
                        if (_stopOnError && first.State == MultiTaskState.Failed)
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                _completed = true;
            }
        }

        private async Task RunParallelSubsetAsync(int start, int count, CancellationToken token)
        {
            // Throttle concurrency to the configured (CPU-aware) limit so a large sub-set does not
            // oversubscribe the CPU or the thread pool. A failing task NEVER stops the others in the
            // same sub-set: each task's exception is captured inside RunTaskAsync and the per-task
            // wrapper is fully isolated so a failure cannot break Task.WhenAll.
            int limit = Math.Max(1, Math.Min(_maxDegreeOfParallelism, count));
            using var gate = new SemaphoreSlim(limit, limit);
            var running = new List<Task>(count);
            for (int idx = start; idx < start + count; idx++)
            {
                TaskItem task = _tasks[idx];
                try
                {
                    await gate.WaitAsync(token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // The whole run was cancelled (Esc/Ctrl+C/token): stop scheduling more tasks
                    // but keep awaiting the ones already started below.
                    break;
                }

                running.Add(Task.Run(async () =>
                {
                    try
                    {
                        await RunTaskAsync(task, token).ConfigureAwait(false);
                    }
                    catch
                    {
                        // Defensive: RunTaskAsync already records per-task failures.
                    }
                    finally
                    {
                        gate.Release();
                    }
                }, CancellationToken.None));
            }
            // Only advance to the next sub-set once every item of this one has finished.
            await Task.WhenAll(running).ConfigureAwait(false);
        }

        private static async Task RunTaskAsync(TaskItem task, CancellationToken token)
        {
            task.State = MultiTaskState.Running;
            task.Timer.Restart();
            try
            {
                IReadOnlyDictionary<string, object?> input =
                    new ReadOnlyDictionary<string, object?>(task.InputContext ?? new Dictionary<string, object?>());
                IDictionary<string, object?>? output = await task.Handler(input, token).ConfigureAwait(false);
                task.Output = output is null ? [] : new Dictionary<string, object?>(output);
                task.State = MultiTaskState.Success;
            }
            catch (OperationCanceledException)
            {
                task.State = MultiTaskState.Failed;
                task.Error = new OperationCanceledException("Task canceled.");
            }
            catch (Exception ex)
            {
                task.Error = ex;
                task.State = MultiTaskState.Failed;
            }
            finally
            {
                task.Timer.Stop();
            }
        }

        private void SetResultAndCancel(bool aborted)
        {
            // Pre-size the result list to avoid intermediate growth reallocations.
            List<MultiTaskResult> results = new(_tasks.Count);
            for (int i = 0; i < _tasks.Count; i++)
            {
                TaskItem t = _tasks[i];
                results.Add(new MultiTaskResult(
                    t.Title,
                    t.State,
                    t.Timer.Elapsed,
                    new ReadOnlyDictionary<string, object?>(t.Output),
                    t.Error));
            }

            var state = new StateMultiTasks(_stopwatch.Elapsed, results, aborted);
            ResultCtrl = new ResultPrompt<StateMultiTasks>(state, aborted);
            _cancellationTokenSource?.Cancel();
        }

        private string FormatElapsed(TimeSpan value) => value.ToString(_elapsedFormat, _culture);

        /// <summary>
        /// Counts task states in a single pass. Called every render tick, so it avoids the multiple
        /// LINQ enumerations (Count/Any) that would otherwise walk the list several times per frame.
        /// </summary>
        private (int total, int done, int success, int failed, bool anyRunning) CountStates()
        {
            int done = 0, success = 0, failed = 0;
            bool anyRunning = false;
            // Index-based loop avoids the enumerator allocation of foreach on List<T> hot paths.
            for (int i = 0; i < _tasks.Count; i++)
            {
                switch (_tasks[i].State)
                {
                    case MultiTaskState.Success:
                        success++;
                        done++;
                        break;
                    case MultiTaskState.Failed:
                        failed++;
                        done++;
                        break;
                    case MultiTaskState.Running:
                        anyRunning = true;
                        break;
                }
            }
            return (_tasks.Count, done, success, failed, anyRunning);
        }

        private void WriteSummary(BufferScreen screenBuffer)
        {
            (int total, int done, int success, int failed, bool anyRunning) = CountStates();

            var sb = new StringBuilder();
            AppendCounts(sb, total, done, success, failed);
            if (_showElapsedTime)
            {
                sb.Append(' ');
                sb.Append(FormatElapsed(_stopwatch.Elapsed));
            }
            screenBuffer.Write(sb.ToString(), _optStyles[MultiTasksStyles.Answer]);

            // Show the spinner in the summary line only while it is pertinent: at least one task
            // is currently running.
            if (_spinner != null && anyRunning)
            {
                screenBuffer.Write($" {_spinner.CurrentFrame}", _optStyles[MultiTasksStyles.Spinner]);
            }

            screenBuffer.WriteLine("", _optStyles[MultiTasksStyles.Answer]);
        }

        /// <summary>
        /// Appends the explicit "{success} ok, {failed} failed, {waiting} wait" breakdown shared by
        /// the running and finished summary lines, so the meaning of each count never changes
        /// between frames (see FASE2-CONTROLS-PLAN.md, Grupo 6, for why the previous done/total
        /// fraction was replaced).
        /// </summary>
        private static void AppendCounts(StringBuilder sb, int total, int done, int success, int failed)
        {
            sb.Append(string.Format(CultureInfo.CurrentCulture, s_multiTasksSuccessFormat, success));
            sb.Append(", ");
            sb.Append(string.Format(CultureInfo.CurrentCulture, s_multiTasksFailedFormat, failed));
            sb.Append(", ");
            sb.Append(string.Format(CultureInfo.CurrentCulture, s_multiTasksWaitingFormat, total - done));
        }

        private void WriteDescription(BufferScreen screenBuffer)
        {
            string? desc = OptionsControl.DescriptionValue;
            if (!string.IsNullOrEmpty(desc))
            {
                screenBuffer.WriteLine(desc, _optStyles[MultiTasksStyles.Description]);
            }
        }

        private void WriteTasksList(BufferScreen screenBuffer)
        {
            ArraySegment<TaskItem> subset = _localpaginator!.GetPageData();
            foreach (TaskItem item in subset)
            {
                bool isSelected = _localpaginator.SelectedIndex >= 0
                    && _localpaginator.SelectedItem != null
                    && item.UniqueId == _localpaginator.SelectedItem.UniqueId;

                screenBuffer.Write(isSelected ? GetSymbol(SymbolType.Selector) : " ", ConsoleHandler.CurrentStyle);

                // Per-item status marker, similar to MultiSelect's [x]/[ ] but representing the
                // task execution state instead of a selection: [ ] waiting, [spinner] running,
                // [√] success, [!] failed.
                (string glyph, Style style) = GetStateVisual(item);
                screenBuffer.Write(" [", ConsoleHandler.CurrentStyle);
                screenBuffer.Write(glyph, style);
                screenBuffer.Write("] ", ConsoleHandler.CurrentStyle);
                screenBuffer.Write(item.Title, isSelected ? style : ConsoleHandler.CurrentStyle);

                if (_showElapsedTime && item.State != MultiTaskState.Waiting)
                {
                    screenBuffer.Write($"  {FormatElapsed(item.Timer.Elapsed)}", _optStyles[MultiTasksStyles.ElapsedTime]);
                }

                if (item.State == MultiTaskState.Failed && item.Error is not null)
                {
                    screenBuffer.Write($"  {item.Error.Message}", _optStyles[MultiTasksStyles.FailedTask]);
                }

                screenBuffer.WriteLine("", ConsoleHandler.CurrentStyle);
            }

            if (_localpaginator.PageCount > 0)
            {
                string template = ConfigPrompt.PaginationTemplateValue(
                    _localpaginator.TotalCountValid,
                    _localpaginator.SelectedPage + 1,
                    _localpaginator.PageCount)!;
                screenBuffer.WriteLine(template, _optStyles[MultiTasksStyles.Pagination]);
            }
        }

        private (string glyph, Style style) GetStateVisual(TaskItem item)
        {
            // GetSymbol resolves the Unicode/ASCII variant automatically based on the terminal's
            // capabilities (and output encoding), so no manual capability check is needed here.
            return item.State switch
            {
                MultiTaskState.Running => (GetSymbol(SymbolType.TaskRunning), _optStyles[MultiTasksStyles.RunningTask]),
                MultiTaskState.Success => (GetSymbol(SymbolType.TaskSuccess), _optStyles[MultiTasksStyles.SuccessTask]),
                MultiTaskState.Failed => (GetSymbol(SymbolType.TaskFailed), _optStyles[MultiTasksStyles.FailedTask]),
                _ => (GetSymbol(SymbolType.TaskWaiting), _optStyles[MultiTasksStyles.WaitingTask]),
            };
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
            screenBuffer.WriteLine(tooltip, _optStyles[MultiTasksStyles.Tooltips]);
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
            // First tooltip entry follows the list-navigation pattern (like MultiSelect), then the
            // remaining entries are cycled with the tooltip-toggle hotkey (F1) and the whole tooltip
            // block is shown/hidden with the show-hide hotkey (Ctrl+F1).
            List<string> lsttooltips =
            [
                GetTooltipNavigation()
            ];
            if (OptionsControl.EnabledAbortKeyValue)
            {
                lsttooltips.Add($"{ConfigPrompt.HotKeyAbortKeyPress}:{PromptPlusResources.Abort}");
            }
            lsttooltips.Add($"{ConfigPrompt.HotKeyTooltipShowHide}:{PromptPlusResources.TooltipShowHide}");
            _toggerTooptips = [.. lsttooltips];
        }

        private static string GetTooltipNavigation()
        {
            // Detailed list navigation instructions, matching the list-based controls.
            string nav = PromptPlusResources.TooltipPages;
            return nav.EndsWith('.') ? nav : $"{nav}.";
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

        /// <summary>
        /// Default maximum degree of parallelism derived from the number of CPU cores. Capped to a
        /// small ceiling so console rendering stays responsive even on high-core machines.
        /// </summary>
        private static int DefaultMaxParallelism()
            => ClampParallelism(Environment.ProcessorCount);

        /// <summary>
        /// Clamps a requested parallelism value to a sensible range (1..min(ProcessorCount*2, 16)).
        /// </summary>
        private static int ClampParallelism(int value)
        {
            int ceiling = Math.Min(Environment.ProcessorCount * 2, 16);
            if (ceiling < 1)
            {
                ceiling = 1;
            }
            return Math.Clamp(value, 1, ceiling);
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
