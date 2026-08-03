using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // MultiTasksControl, the third "Live" control covered here. Same family as
    // ProgressBar/TaskExec: each task's handler (Action(Async)) runs
    // on a background Task, and `_completed` (volatile) signals when the whole run (all tasks) is
    // done — so completion is deterministic in tests without Duration/Sleep guessing. See
    // [[promptplus-live-controls-strategy]] for the ManualResetEventSlim handshake technique used
    // for mid-run assertions below.
    //
    // Tasks execute strictly in list order; consecutive tasks resolving to MultiTasksMode.Parallel
    // form a sub-set that runs concurrently (throttled by MaxDegreeOfParallelism), and the run only
    // advances to the next task/sub-set once every item of the current one has finished. A failed
    // task never counts as the overall run being "Aborted" — StateMultiTasks.Aborted only reflects
    // Escape/external cancellation; per-task outcomes are in Results/AnyFailed/AllSucceeded.
    //
    // 1 real bug found and fixed this session (confirmed via probe, then redesigned per the user's
    // explicit request for a clearer 3-way breakdown instead of a fraction):
    // - WriteSummary (running) showed "{done}/{total}" (done = success+failed) while
    //   WriteFinishSummary (final frame) showed "{success}/{total}" for the SAME position in the
    //   string — the visible numerator's meaning silently changed on the very last frame (e.g.
    //   "2/2" while running could become "1/2 (1 failed)" on finish for the identical 2 tasks).
    //   Replaced entirely with an explicit "{success} ok, {failed} failed, {waiting} wait" format
    //   used identically by both WriteSummary and WriteFinishSummary — added 2 new resource keys
    //   (MultiTasksSuccessCount, MultiTasksWaitingCount) and repurposed MultiTasksFailed (dropped
    //   its baked-in parentheses/trailing space) across all 11 locales.
    //
    // ASCII glyphs (SupportsUnicode=false): Waiting=" ", Running=">", Success="v", Failed="x".
    [Collection(SerializedGlobalStateCollection.Name)]
    public class MultiTasksControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IMultiTasksControl MakeControl(VirtualTerminal vt) =>
            new PromptPlusControls(vt, new PromptConfig()).MultiTasks("Working");

        [Fact]
        public void Default_render_finishes_with_every_task_counted_as_ok()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .AddTask("Step 1", ct => { })
                .AddTask("Step 2", ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.AllSucceeded.Should().BeTrue();
            _ = vt.TextAt(0, 0, 40).TrimEnd().Should().StartWith("Working: 2 ok, 0 failed, 0 wait");
        }

        [Fact]
        public void Mid_run_shows_running_and_waiting_tasks_with_their_own_glyphs()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .AddTask("Step 1", ct => { ready.Set(); proceed.Wait(ct); })
                .AddTask("Step 2", ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            string summary = vt.TextAt(0, 0, 40).TrimEnd();
            string row1 = vt.TextAt(1, 0, 13);
            string row2 = vt.TextAt(2, 0, 13);
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = summary.Should().StartWith("Working: 0 ok, 0 failed, 2 wait");
            _ = row1.Should().Be("> [>] Step 1 ");
            _ = row2.Should().Be("  [ ] Step 2 ");
        }

        [Fact]
        public void Failed_task_does_not_stop_the_others_or_count_as_aborted()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .AddTask("Step 1", ct => throw new InvalidOperationException("boom"))
                .AddTask("Step 2", ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.Aborted.Should().BeFalse();
            _ = result.Content.AnyFailed.Should().BeTrue();
            _ = result.Content.AllSucceeded.Should().BeFalse();
            _ = result.Content.Results.Should().Contain(r => r.Title == "Step 2" && r.State == MultiTaskState.Success);
            var failedResult = result.Content.Results.Single(r => r.Title == "Step 1");
            _ = failedResult.State.Should().Be(MultiTaskState.Failed);
            _ = failedResult.Exception.Should().BeOfType<InvalidOperationException>();
            _ = vt.TextAt(0, 0, 40).TrimEnd().Should().StartWith("Working: 1 ok, 1 failed, 0 wait");
        }

        [Fact]
        public void StopOnError_in_sequential_mode_stops_the_remaining_tasks()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .StopOnError()
                .AddTask("Step 1", ct => throw new InvalidOperationException("boom"))
                .AddTask("Step 2", ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.Results.Single(r => r.Title == "Step 1").State.Should().Be(MultiTaskState.Failed);
            _ = result.Content.Results.Single(r => r.Title == "Step 2").State.Should().Be(MultiTaskState.Waiting);
            _ = vt.TextAt(0, 0, 40).TrimEnd().Should().StartWith("Working: 0 ok, 1 failed, 1 wait");
        }

        [Fact]
        public void StopOnError_is_ignored_in_parallel_mode()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .StopOnError()
                .Mode(MultiTasksMode.Parallel)
                .AddTask("Step 1", ct => throw new InvalidOperationException("boom"))
                .AddTask("Step 2", ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.Results.Single(r => r.Title == "Step 2").State.Should().Be(MultiTaskState.Success);
        }

        [Fact]
        public void Parallel_mode_runs_consecutive_tasks_concurrently()
        {
            var vt = MakeTerminal();
            var ready1 = new ManualResetEventSlim();
            var ready2 = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .Mode(MultiTasksMode.Parallel)
                .AddTask("A", ct => { ready1.Set(); proceed.Wait(ct); })
                .AddTask("B", ct => { ready2.Set(); proceed.Wait(ct); });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            bool bothReady = ready1.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken) && ready2.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            proceed.Set();
            var result = runTask.GetAwaiter().GetResult();

            _ = bothReady.Should().BeTrue();
            _ = result.Content.AllSucceeded.Should().BeTrue();
        }

        [Fact]
        public void MaxDegreeOfParallelism_throttles_concurrent_execution()
        {
            var vt = MakeTerminal();
            int concurrent = 0;
            int maxObservedConcurrent = 0;
            var control = MakeControl(vt)
                .Mode(MultiTasksMode.Parallel)
                .MaxDegreeOfParallelism(1)
                .AddTask("A", ct =>
                {
                    int c = Interlocked.Increment(ref concurrent);
                    maxObservedConcurrent = Math.Max(maxObservedConcurrent, c);
                    _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(80);
                    _ = Interlocked.Decrement(ref concurrent);
                })
                .AddTask("B", ct =>
                {
                    int c = Interlocked.Increment(ref concurrent);
                    maxObservedConcurrent = Math.Max(maxObservedConcurrent, c);
                    _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(80);
                    _ = Interlocked.Decrement(ref concurrent);
                });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = control.Run(cts.Token);

            _ = maxObservedConcurrent.Should().Be(1);
        }

        [Fact]
        public void Escape_aborts_and_captures_a_snapshot_of_current_task_states()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .AddTask("Step 1", ct => { ready.Set(); while (!ct.IsCancellationRequested) { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5); } })
                .AddTask("Step 2", ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Aborted.Should().BeTrue();
            _ = result.Content.Results.Single(r => r.Title == "Step 1").State.Should().Be(MultiTaskState.Running);
            _ = result.Content.Results.Single(r => r.Title == "Step 2").State.Should().Be(MultiTaskState.Waiting);
        }

        [Fact]
        public void External_cancellation_populates_content_but_blanks_the_screen()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .AddTask("Step 1", ct => { while (!ct.IsCancellationRequested) { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5); } });

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Snapshot().Trim().Should().BeEmpty();
        }

        [Fact]
        public void Context_round_trips_per_task()
        {
            var vt = MakeTerminal();
            var input = new Dictionary<string, object?> { ["x"] = 42 };
            var control = MakeControl(vt)
                .AddTask("Step 1", (ctx, ct) =>
                {
                    _ = ctx.TryGetValue("x", out var v);
                    return new Dictionary<string, object?> { ["echo"] = v };
                }, input);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.Results[0].GetOutput<int>("echo", out bool found).Should().Be(42);
            _ = found.Should().BeTrue();
        }

        [Fact]
        public void AddTaskAsync_overloads_drive_completion_like_the_sync_overload()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .AddTaskAsync("Step 1", async (ctx, ct) =>
                {
                    await Task.Delay(1, ct);
                    return null;
                })
                .AddTaskAsync("Step 2", async ct => await Task.Delay(1, ct));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.AllSucceeded.Should().BeTrue();
        }

        [Fact]
        public void Interaction_helper_adds_one_task_per_item_preserving_order()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .Interaction(["a", "b", "c"], (item, c) => c.AddTask(item, ct => { }));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.Results.Select(r => r.Title).Should().Equal("a", "b", "c");
        }

        [Fact]
        public void Missing_tasks_throws_before_rendering_anything()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            Action act = () => control.Run(cts.Token);

            _ = act.Should().Throw<InvalidOperationException>().WithMessage("*AddTask*");
        }

        [Fact]
        public void PageSize_limits_the_visible_task_rows_per_page()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .PageSize(2)
                .AddTask("Step 1", ct => { ready.Set(); proceed.Wait(ct); })
                .AddTask("Step 2", ct => { })
                .AddTask("Step 3", ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            var pagination = vt.Find("1 of 2 pages");
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = pagination.Should().NotBeNull();
        }

        [Fact]
        public void F1_cycles_through_navigation_abort_and_showhide_tooltips()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .AddTask("Step 1", ct => { ready.Set(); proceed.Wait(ct); });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            var navigationTooltip = vt.Find("PgUp/PgDown:Move");

            _ = vt.Keys.Enqueue(ConsoleKey.F1);
            WaitForTextChange(vt, "PgUp/PgDown:Move");
            var abortTooltip = vt.Find("Esc:Abort");

            _ = vt.Keys.Enqueue(ConsoleKey.F1);
            WaitForText(vt, "Ctrl F1");
            var showHideTooltip = vt.Find("Ctrl F1");

            proceed.Set();
            cts.Cancel();
            try { _ = runTask.GetAwaiter().GetResult(); } catch (OperationCanceledException) { }

            _ = navigationTooltip.Should().NotBeNull();
            _ = abortTooltip.Should().NotBeNull();
            _ = showHideTooltip.Should().NotBeNull();
        }

        private static void WaitForText(VirtualTerminal vt, string text)
        {
            var deadline = DateTime.UtcNow.AddSeconds(2);
            while (vt.Find(text) is null && DateTime.UtcNow < deadline)
            {
                _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(20);
            }
        }

        private static void WaitForTextChange(VirtualTerminal vt, string previousText)
        {
            var deadline = DateTime.UtcNow.AddSeconds(2);
            while (vt.Find(previousText) is not null && DateTime.UtcNow < deadline)
            {
                _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(20);
            }
        }
    }
}
