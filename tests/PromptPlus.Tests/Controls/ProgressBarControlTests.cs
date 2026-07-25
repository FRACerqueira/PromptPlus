using ConsolePlusLibrary;
using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Fase 2, Grupo 6 (FASE2-CONTROLS-PLAN.md) — ProgressBarControl, the first "Live"
    // (IsLiveAutoRenderControl) control covered in this rollout.
    //
    // Unlike every control tested so far, ProgressBar is NOT driven by anything internal
    // (Stopwatch, real elapsed time) — it REQUIRES a caller-supplied UpdateHandler(Async) callback
    // that runs on its own background Task and drives the value via ProgressBarEvent.Update(...).
    // The control finishes when ProgressBarEvent.Finish becomes true (aborted, or value >= max).
    // This makes most scenarios fully deterministic and race-free WITHOUT any TestContext.Current.CancellationToken.WaitHandle.WaitOne/Duration
    // guessing: a handler that calls e.Update(max) and returns finishes the control almost
    // instantly (bounded only by normal thread-pool scheduling, not a fixed wait), and control.Run(
    // cts.Token) can be called synchronously on the test thread with a generous CTS as a safety net.
    //
    // The one deterministic technique needed for a genuinely "still running" mid-progress frame
    // (spinner glyph, partial fill) is real synchronization (ManualResetEventSlim), not a timing
    // guess: the handler sets a value, signals "ready", and blocks on "proceed" until released by
    // the test. The test waits on "ready" (a real signal, not a sleep), gives the render loop a
    // short fixed margin to notice the change and repaint (~16ms tick), reads the snapshot, then
    // releases "proceed". This is a new pattern for this rollout — see FASE2-CONTROLS-PLAN.md's
    // Grupo 6 section for the fuller rationale and how it differs from Time/TaskExec/MultiTasks.
    //
    // 3 real bugs were found and fixed this session (all confirmed via probe before/after):
    // - A handler exception was reported as IsAborted=false (looked successful) because TryResult
    //   checked the generic ProgressBarEvent.Finish (true for BOTH success and error/abort) BEFORE
    //   the specific error-check branch. Fixed by reordering the checks so error/abort is checked
    //   first — see the regression test below asserting IsAborted=true + "Error!" on the final frame.
    // - FinishTemplate wrote OptionsControl.PromptValue directly, skipping the ": " suffix that
    //   WritePrompt (used everywhere else, including this control's own BufferTemplate) applies.
    //   Fixed by calling WritePrompt instead of reimplementing it.
    // - HideElements(HideProgressBar.ElapsedTime) was honored in WriteAnswer (while running) but not
    //   in FinishTemplate (the final frame still showed the elapsed time). Fixed to match.
    //
    // ProgressBarType.Fill (the default) renders both the "on" and "off" bar segments as plain
    // spaces (only the background style differs) — tests that need a visible fill character use
    // ProgressBarType.Square instead. Default Range is 0..100, default Width is 40 (ConfigPrompt.
    // ProgressBarWidth), default abort key is Escape (shown as "Esc" in the tooltip).
    //
    // [Collection(BackgroundTimingCollection.Name)]: see BackgroundTimingCollection.cs — every test
    // here spawns a real background Task and relies on real wall-clock margins, which flaked under
    // full-suite parallel load until serialized against the other background-task-heavy classes.
    [Collection(BackgroundTimingCollection.Name)]
    public class ProgressBarControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IProgressBarControl MakeControl(VirtualTerminal vt) =>
            new PromptPlusControls(vt, new PromptConfig()).ProgressBar("Working");

        [Fact]
        public void Handler_reaching_max_completes_with_the_expected_final_frame()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).UpdateHandler((e, ct) => e.Update(100));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.FinishedValue.Should().Be(100);
            _ = result.Content.ExceptionProgress.Should().BeNull();
            _ = vt.TextAt(0, 0, 12).Should().Be("Working: 100");
            _ = vt.TextAt(1, 0, 48).Should().Be("0 |                                        | 100");
        }

        [Fact]
        public void Handler_exception_is_reported_as_aborted_with_the_error_text_and_style()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).UpdateHandler((e, ct) => throw new InvalidOperationException("boom"));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.ExceptionProgress.Should().BeOfType<InvalidOperationException>();
            _ = vt.TextAt(0, 0, 15).Should().Be($"Working: {PromptPlusResources.Error}");
            _ = vt.StyleAt(0, 9).Foreground.Should().NotBe(vt.StyleAt(0, 0).Foreground);
        }

        [Fact]
        public void Escape_aborts_and_shows_the_canceled_text()
        {
            var vt = MakeTerminal();
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            var control = MakeControl(vt).UpdateHandler((e, ct) =>
            {
                while (!ct.IsCancellationRequested) { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5); }
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.ExceptionProgress.Should().BeNull();
            _ = vt.TextAt(0, 0, 17).Should().Be($"Working: {PromptPlusResources.CanceledKey}");
        }

        [Fact]
        public void Finish_text_overrides_the_default_completion_text()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Finish("Done!").UpdateHandler((e, ct) => e.Update(100));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 14).Should().Be("Working: Done!");
        }

        [Fact]
        public void HideElements_ElapsedTime_is_honored_on_both_the_running_and_final_frame()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt).HideElements(HideProgressBar.ElapsedTime)
                .UpdateHandler((e, ct) =>
                {
                    e.Update(50);
                    ready.Set();
                    proceed.Wait(ct);
                    e.Update(100);
                });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            string midLine = vt.TextAt(0, 0, 13);
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = midLine.Should().Be("Working: 50 %");
            _ = vt.TextAt(0, 0, 13).Should().Be("Working: 100%");
        }

        [Fact]
        public void HideElements_Range_and_Delimit_remove_only_the_labels_and_bar_edges()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Fill(ProgressBarType.Square)
                .HideElements(HideProgressBar.Range | HideProgressBar.Delimit)
                .UpdateHandler((e, ct) => e.Update(100));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 40).Should().Be(new string('#', 40));
        }

        [Fact]
        public void HideElements_PromptAnswer_and_ProgressbarAtFinish_leave_the_final_frame_blank()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .HideElements(HideProgressBar.PromptAnswer | HideProgressBar.ProgressbarAtFinish)
                .UpdateHandler((e, ct) => e.Update(100));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = control.Run(cts.Token);

            _ = vt.Snapshot().Trim().Should().BeEmpty();
        }

        [Fact]
        public void Square_type_renders_a_visible_fill_character_at_the_configured_width()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Fill(ProgressBarType.Square).Range(0, 10).Width(20)
                .UpdateHandler((e, ct) => e.Update(10));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 27).Should().Be("0 |####################| 10");
        }

        [Fact]
        public void Mid_progress_can_be_observed_deterministically_via_a_real_handshake()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt).UpdateHandler((e, ct) =>
            {
                e.Update(50);
                ready.Set();
                proceed.Wait(ct);
                e.Update(100);
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            string midAnswer = vt.TextAt(0, 0, 13);
            proceed.Set();
            var result = runTask.GetAwaiter().GetResult();

            _ = midAnswer.Should().Be("Working: 50 %");
            _ = result.Content.FinishedValue.Should().Be(100);
        }

        [Fact]
        public void Re_pressing_escape_cancels_a_running_operation_before_completion()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).UpdateHandler((e, ct) =>
            {
                while (!ct.IsCancellationRequested) { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5); }
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(20);
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            var result = runTask.GetAwaiter().GetResult();

            _ = result.IsAborted.Should().BeTrue();
        }

        [Fact]
        public void Spinner_and_gradient_render_without_crashing_and_do_not_survive_to_the_final_frame()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .Spinner(SpinnersType.Ascii)
                .ChangeGradient(new Color(255, 0, 0), new Color(0, 255, 0))
                .UpdateHandler((e, ct) => e.Update(100));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = vt.Find(PromptPlusResources.Error).Should().BeNull();
        }

        [Fact]
        public void Gradient_colors_the_filled_columns_differently_across_the_bar()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .Fill(ProgressBarType.Square)
                .ChangeGradient(new Color(255, 0, 0), new Color(0, 255, 0))
                .UpdateHandler((e, ct) =>
                {
                    e.Update(100);
                    ready.Set();
                    proceed.Wait(ct);
                });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            Style first = vt.StyleAt(1, 3);
            Style last = vt.StyleAt(1, 3 + 39);
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = first.Foreground.Should().NotBe(last.Foreground);
        }

        [Fact]
        public void ChangeDescription_reflects_the_current_value()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .ChangeDescription(v => $"at {v}")
                .UpdateHandler((e, ct) =>
                {
                    e.Update(50);
                    ready.Set();
                    proceed.Wait(ct);
                    e.Update(100);
                });

            // WriteDescription is only rendered by BufferTemplate (while running) — FinishTemplate
            // never calls it, so this must be observed mid-flight, not on the final frame.
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            var found = vt.Find("at 50");
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = found.Should().NotBeNull();
        }

        [Fact]
        public void ChangeDescriptionAsync_is_awaited_and_reflects_the_current_value()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .ChangeDescriptionAsync(v => Task.FromResult($"async at {v}"))
                .UpdateHandler((e, ct) =>
                {
                    e.Update(50);
                    ready.Set();
                    proceed.Wait(ct);
                    e.Update(100);
                });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            var found = vt.Find("async at 50");
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = found.Should().NotBeNull();
        }

        [Fact]
        public void UpdateHandlerAsync_drives_completion_the_same_way_as_the_sync_overload()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).UpdateHandlerAsync(async (e, ct) =>
            {
                await Task.Delay(1, ct);
                e.Update(100);
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.FinishedValue.Should().Be(100);
        }

        [Fact]
        public void Output_and_input_context_round_trip_through_the_handler()
        {
            var vt = MakeTerminal();
            var context = new Dictionary<string, object?> { ["in"] = 42 };
            var control = MakeControl(vt).UpdateHandler((e, ct) =>
            {
                int input = e.InputParam<int>("in", out bool found);
                e.AddOutputContext("found", found);
                e.AddOutputContext("echo", input);
                e.Update(100);
            }, context);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.GetOutput<bool>("found", out bool foundOk).Should().BeTrue();
            _ = foundOk.Should().BeTrue();
            _ = result.Content.GetOutput<int>("echo", out bool echoOk).Should().Be(42);
            _ = echoOk.Should().BeTrue();
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            _ = vt.Keys.Enqueue(ConsoleKey.F1);
            var control = MakeControl(vt).UpdateHandler((e, ct) =>
            {
                while (!ct.IsCancellationRequested) { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5); }
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            // Poll instead of a single fixed sleep: F1 is processed on the very first render cycle
            // (WaitKeypress always prioritizes an already-queued real key over a tick), but under
            // heavy parallel test-suite load the background Run() task itself can be slow to get
            // scheduled, so a short blind sleep occasionally reads the screen before that first
            // cycle completed at all (observed as an empty tooltip line).
            string tooltip = "";
            var deadline = DateTime.UtcNow.AddSeconds(2);
            while (string.IsNullOrEmpty(tooltip) && DateTime.UtcNow < deadline)
            {
                _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(20);
                tooltip = vt.TextAt(2, 0, 40).TrimEnd();
            }
            cts.Cancel();
            try { _ = runTask.GetAwaiter().GetResult(); } catch (OperationCanceledException) { }

            _ = tooltip.Should().Be($"F1:{PromptPlusResources.TooltipBase}.Ctrl F1:{PromptPlusResources.TooltipShowHide}.");
        }

        [Fact]
        public void Missing_UpdateHandler_throws_before_rendering_anything()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            Action act = () => control.Run(cts.Token);

            _ = act.Should().Throw<InvalidOperationException>().WithMessage("*UpdateHandler*");
        }

        [Fact]
        public void Default_outside_the_configured_range_throws_on_run()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Range(0, 10).Default(20).UpdateHandler((e, ct) => e.Update(10));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            Action act = () => control.Run(cts.Token);

            _ = act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Range_with_minvalue_greater_than_maxvalue_throws_immediately()
        {
            Action act = () => new PromptPlusControls(MakeTerminal(), new PromptConfig())
                .ProgressBar("Working").Range(10, 0);

            _ = act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Width_below_ten_throws_immediately()
        {
            Action act = () => new PromptPlusControls(MakeTerminal(), new PromptConfig())
                .ProgressBar("Working").Width(9);

            _ = act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void FractionalDigits_above_five_throws_immediately()
        {
            Action act = () => new PromptPlusControls(MakeTerminal(), new PromptConfig())
                .ProgressBar("Working").FractionalDigits(6);

            _ = act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
