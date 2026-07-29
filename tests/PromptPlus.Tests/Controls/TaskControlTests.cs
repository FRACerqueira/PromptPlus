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
    // TaskControl, the second "Live" control in this suite. Same family as ProgressBarControl:
    // driven by a caller-supplied Action(Async) callback running on its own background Task
    // (`_completed`/`_error` volatile fields signal completion), not by an internal Stopwatch —
    // so completion is deterministic in tests without any Duration/Sleep guessing. See
    // ProgressBarControlTests.cs for the general rationale and the ManualResetEventSlim handshake
    // technique used for mid-run assertions below.
    //
    // 3 real bugs found and fixed this session (all confirmed via probe before/after):
    // - WriteAnswer only terminated its line (WriteLine) when ShowElapsedTime/Spinner had content —
    //   with neither configured (the DEFAULT), the cursor stayed on the prompt's row and
    //   WriteDescription/WriteTooltip got appended to it instead of starting their own rows
    //   ("Working: F1:Tips.Esc:Abort." on one line instead of two). Fixed by always terminating
    //   the line.
    // - FinishTemplate checked OptionsControl.EnabledAbortKeyValue (whether the abort key is
    //   enabled at all) instead of ShowMessageAbortKeyValue (whether to show a cancel message) —
    //   provably dead code as written (Escape is the only way to reach that branch, and it already
    //   requires EnabledAbortKeyValue=true to register), but inconsistent with ProgressBar's
    //   equivalent check for the same concept. Fixed to match ProgressBar.
    // - A handler that throws OperationCanceledException on its own initiative (not necessarily
    //   because the token was cancelled) was reported as a SUCCESSFUL run (IsAborted=false) just
    //   because no Exception was recorded — conflating "not an error" with "completed
    //   successfully". Fixed with a new _cancelledByHandler flag so that case now reports
    //   IsAborted=true (shown as "Canceled", not "Error!", since it's still not an error).
    //
    // Confirmed by probe, NOT bugs (documented Live-control convention already established for
    // ProgressBar): external CancellationToken cancellation (not Escape) always blanks the
    // control's screen area and skips FinishTemplate entirely, even though TryResult's own
    // press.IsCancelled branch DOES populate ResultCtrl with real state (ElapsedTime, etc.) first —
    // so result.Content is meaningful even though nothing is left on screen.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class TaskControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static ITaskControl MakeControl(VirtualTerminal vt) =>
            new PromptPlusControls(vt, new PromptConfig()).Task("Working");

        [Fact]
        public void Default_render_finishes_with_the_elapsed_time_on_a_single_answer_line()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Action(ct => { });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = vt.TextAt(0, 0, 17).Should().Be("Working: 00:00:00");
        }

        [Fact]
        public void Default_render_keeps_the_tooltip_on_its_own_row_while_running()
        {
            // Regression test: with neither ShowElapsedTime nor Spinner configured (the default),
            // WriteAnswer used to skip its line terminator entirely, so the tooltip line got
            // appended directly after "Working: " instead of starting its own row.
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt).Action(ct => { ready.Set(); proceed.Wait(ct); });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            string row0 = vt.TextAt(0, 0, 9);
            string row1 = vt.TextAt(1, 0, 8);
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = row0.Should().Be("Working: ");
            _ = row1.Should().Be("F1:Tips.");
        }

        [Fact]
        public void ShowElapsedTime_and_spinner_are_observable_while_running_on_the_same_row()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt).ShowElapsedTime().Spinner(SpinnersType.Ascii)
                .Action(ct => { ready.Set(); proceed.Wait(ct); });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            string row0 = vt.TextAt(0, 9, 12);
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = row0.Should().StartWith("00:00:00 ");
        }

        [Fact]
        public void Error_in_action_is_reported_as_aborted_with_error_text_and_style()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Action(ct => throw new InvalidOperationException("boom"));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Exception.Should().BeOfType<InvalidOperationException>();
            _ = vt.TextAt(0, 0, 15).Should().Be($"Working: {PromptPlusResources.Error}");
            _ = vt.StyleAt(0, 9).Foreground.Should().NotBe(vt.StyleAt(0, 0).Foreground);
        }

        [Fact]
        public void Finish_success_and_error_text_are_shown_on_the_matching_outcome()
        {
            var vtOk = MakeTerminal();
            var okControl = MakeControl(vtOk).Finish("Done!", "Oops!").Action(ct => { });
            using var ctsOk = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = okControl.Run(ctsOk.Token);
            _ = vtOk.TextAt(0, 0, 14).Should().Be("Working: Done!");

            var vtErr = MakeTerminal();
            var errControl = MakeControl(vtErr).Finish("Done!", "Oops!")
                .Action(ct => throw new InvalidOperationException("boom"));
            using var ctsErr = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = errControl.Run(ctsErr.Token);
            _ = vtErr.TextAt(0, 0, 14).Should().Be("Working: Oops!");
        }

        [Fact]
        public void Escape_aborts_and_shows_the_canceled_text()
        {
            var vt = MakeTerminal();
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            var control = MakeControl(vt).Action(ct =>
            {
                while (!ct.IsCancellationRequested) { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5); }
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Exception.Should().BeNull();
            _ = vt.TextAt(0, 0, 17).Should().Be($"Working: {PromptPlusResources.CanceledKey}");
        }

        [Fact]
        public void Handler_throwing_OperationCanceledException_on_its_own_is_reported_as_aborted_not_success()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Action(ct => throw new OperationCanceledException());

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Exception.Should().BeNull();
            _ = vt.TextAt(0, 0, 17).Should().Be($"Working: {PromptPlusResources.CanceledKey}");
        }

        [Fact]
        public void External_cancellation_populates_content_but_blanks_the_screen()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Action(ct =>
            {
                while (!ct.IsCancellationRequested) { _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(5); }
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(300));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Snapshot().Trim().Should().BeEmpty();
        }

        [Fact]
        public void Context_round_trips_through_the_synchronous_Action_overload()
        {
            var vt = MakeTerminal();
            var input = new Dictionary<string, object?> { ["x"] = 42 };
            var control = MakeControl(vt).Context(input).Action((ctx, ct) =>
            {
                _ = ctx.TryGetValue("x", out var v);
                return new Dictionary<string, object?> { ["echo"] = v };
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.GetOutput<int>("echo", out bool found).Should().Be(42);
            _ = found.Should().BeTrue();
        }

        [Fact]
        public void ActionAsync_with_context_drives_completion_the_same_way_as_the_sync_overload()
        {
            var vt = MakeTerminal();
            var input = new Dictionary<string, object?> { ["x"] = 42 };
            var control = MakeControl(vt).Context(input).ActionAsync(async (ctx, ct) =>
            {
                await Task.Delay(1, ct);
                _ = ctx.TryGetValue("x", out var v);
                return new Dictionary<string, object?> { ["echo"] = v };
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.GetOutput<int>("echo", out bool found).Should().Be(42);
            _ = found.Should().BeTrue();
        }

        [Fact]
        public void ActionAsync_simple_overload_with_no_context_completes_normally()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).ActionAsync(async ct => await Task.Delay(1, ct));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
        }

        [Fact]
        public void ChangeDescription_and_ChangeDescriptionAsync_reflect_state_while_running()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .ChangeDescription(t => "sync desc")
                .Action(ct => { ready.Set(); proceed.Wait(ct); });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            var found = vt.Find("sync desc");
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = found.Should().NotBeNull();
        }

        [Fact]
        public void ChangeDescriptionAsync_is_awaited_while_running()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            var control = MakeControl(vt)
                .ChangeDescriptionAsync(t => Task.FromResult("async desc"))
                .Action(ct => { ready.Set(); proceed.Wait(ct); });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            _ = ready.Wait(TimeSpan.FromSeconds(2), TestContext.Current.CancellationToken);
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(50);
            var found = vt.Find("async desc");
            proceed.Set();
            _ = runTask.GetAwaiter().GetResult();

            _ = found.Should().NotBeNull();
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var ready = new ManualResetEventSlim();
            var proceed = new ManualResetEventSlim();
            _ = vt.Keys.Enqueue(ConsoleKey.F1);
            var control = MakeControl(vt).Action(ct => { ready.Set(); proceed.Wait(ct); });

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            string tooltip = "";
            var deadline = DateTime.UtcNow.AddSeconds(2);
            while (string.IsNullOrEmpty(tooltip) && DateTime.UtcNow < deadline)
            {
                _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(20);
                tooltip = vt.TextAt(1, 0, 40).TrimEnd();
            }
            proceed.Set();
            cts.Cancel();
            try { _ = runTask.GetAwaiter().GetResult(); } catch (OperationCanceledException) { }

            _ = ready.IsSet.Should().BeTrue();
            _ = tooltip.Should().Be($"F1:{PromptPlusResources.TooltipBase}.Ctrl F1:{PromptPlusResources.TooltipShowHide}.");
        }

        [Fact]
        public void Missing_Action_throws_before_rendering_anything()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            Action act = () => control.Run(cts.Token);

            _ = act.Should().Throw<InvalidOperationException>().WithMessage("*Action*");
        }
    }
}
