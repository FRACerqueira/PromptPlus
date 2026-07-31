using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // TimeControl, the last "Live" control in this suite, deliberately saved for last because it
    // is the ONLY one of the four driven purely by
    // a real System.Diagnostics.Stopwatch (WaitKeypress checks `_stopwatch.Elapsed >= _duration`)
    // — there is no caller-supplied callback to hook into like ProgressBar/TaskExec/MultiTasks, so
    // completion timing cannot be made fully deterministic. Per the user's explicit guidance: tests
    // don't need deterministic timing, only that the elapsed time is what's expected GIVEN the
    // configured Duration (>= it on success, near-zero on an immediate abort) — never asserting an
    // exact wall-clock duration a test itself took to run.
    //
    // Confirmed by probe (load-bearing facts, not obvious from the interface alone):
    // - On normal completion, ResultCtrl's returned TimeSpan is always exactly the CONFIGURED
    //   Duration (not the real measured _stopwatch.Elapsed, which could overshoot by a tick or two)
    //   — so successful completion assertions ARE exact, no tolerance needed there.
    // - DisplayMode only affects the RENDERED text (Countdown clamps to zero, Elapsed clamps to the
    //   full Duration) — the returned Content is unaffected by DisplayMode.
    // - Duration defaults to TimeSpan.Zero, so a Time control with no .Duration(...) call completes
    //   immediately (no "missing config" exception, unlike ProgressBar/TaskExec/MultiTasks).
    // - WaitKeypress checks KeyAvailable first, so a pre-enqueued key (Escape, F1) always wins over
    //   a tick — deterministic, no background Task needed for those paths.
    // - WriteDescription/WriteTooltip only exist in BufferTemplate (never FinishTemplate) — same
    //   convention as every other Live control this session; observing them requires peeking at the
    //   screen while Run() is still blocked (background Task + a short real-time margin).
    // - External CancellationToken cancellation (not Escape) results in a blank screen AND a
    //   default/zero Content — the tick-wakeup path's own cancellation check short-circuits before
    //   ever reaching the per-control code that would otherwise populate a real elapsed snapshot,
    //   so (unlike what earlier notes for TaskExec/MultiTasks assumed) Content is NOT a meaningful
    //   "elapsed at cancellation time" value here — just confirmed IsAborted=true and a blank frame.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class TimeControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static ITimeControl MakeControl(VirtualTerminal vt) =>
            new PromptPlusControls(vt, new PromptConfig()).Time("Waiting");

        [Fact]
        public void Default_duration_zero_completes_immediately_with_a_zero_countdown()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be(TimeSpan.Zero);
            _ = vt.TextAt(0, 0, 17).Should().Be("Waiting: 00:00:00");
        }

        [Fact]
        public void Countdown_completes_and_returns_exactly_the_configured_duration()
        {
            var vt = MakeTerminal();
            var duration = TimeSpan.FromMilliseconds(80);
            var control = MakeControl(vt).Duration(duration);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be(duration);
            _ = vt.TextAt(0, 0, 17).Should().Be("Waiting: 00:00:00");
        }

        [Fact]
        public void DisplayMode_Elapsed_shows_the_full_duration_instead_of_a_zero_countdown()
        {
            var vt = MakeTerminal();
            var duration = TimeSpan.FromMilliseconds(1200);
            var control = MakeControl(vt).Duration(duration).DisplayMode(TimeDisplayMode.Elapsed);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(duration);
            _ = vt.TextAt(0, 0, 17).Should().Be("Waiting: 00:00:01");
        }

        [Fact]
        public void Custom_format_and_culture_are_applied_to_the_elapsed_value()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .Duration(TimeSpan.FromMilliseconds(80))
                .DisplayMode(TimeDisplayMode.Elapsed)
                .Format(@"ss\.ff")
                .Culture(new CultureInfo("pt-BR"));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 14).Should().Be("Waiting: 00.08");
        }

        [Fact]
        public void Finish_text_overrides_the_default_completion_text()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Duration(TimeSpan.FromMilliseconds(80)).Finish("Done!");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 14).Should().Be("Waiting: Done!");
        }

        [Fact]
        public void Escape_aborts_before_the_duration_elapses_and_shows_the_canceled_text()
        {
            var vt = MakeTerminal();
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);
            var duration = TimeSpan.FromSeconds(20);
            var control = MakeControl(vt).Duration(duration);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeLessThan(duration);
            _ = vt.TextAt(0, 0, 17).Should().Be($"Waiting: {PromptPlusResources.CanceledKey}");
        }

        [Fact]
        public void External_cancellation_aborts_and_blanks_the_screen()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt).Duration(TimeSpan.FromSeconds(20));

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(150));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Snapshot().Trim().Should().BeEmpty();
        }

        [Fact]
        public void Spinner_and_ChangeDescription_are_observable_while_still_counting_down()
        {
            var vt = MakeTerminal();
            var control = MakeControl(vt)
                .Duration(TimeSpan.FromSeconds(2))
                .Spinner(SpinnersType.Ascii)
                .ChangeDescription(t => $"elapsed so far: {t.TotalMilliseconds >= 0}");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            // The elapsed time shown here is inherently non-deterministic (no external hook to
            // pause on, unlike the callback-driven Live controls) — only presence/format is
            // asserted, per the user's explicit "don't require exact timing" guidance for Time.
            _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(150);
            var descriptionFound = vt.Find("elapsed so far: True");
            cts.Cancel();
            try { _ = runTask.GetAwaiter().GetResult(); } catch (OperationCanceledException) { }

            _ = descriptionFound.Should().NotBeNull();
        }

        [Fact]
        public void F1_cycles_from_the_abort_tooltip_to_the_showhide_tooltip()
        {
            var vt = MakeTerminal();
            _ = vt.Keys.Enqueue(ConsoleKey.F1);
            var control = MakeControl(vt).Duration(TimeSpan.FromSeconds(2));

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            var runTask = Task.Run(() => control.Run(cts.Token));
            string? tooltip = null;
            var deadline = DateTime.UtcNow.AddSeconds(2);
            while (tooltip is null && DateTime.UtcNow < deadline)
            {
                _ = TestContext.Current.CancellationToken.WaitHandle.WaitOne(20);
                var found = vt.Find("Ctrl F1");
                if (found is not null) { tooltip = vt.TextAt(found.Value.Row, 0, 40).TrimEnd(); }
            }
            cts.Cancel();
            try { _ = runTask.GetAwaiter().GetResult(); } catch (OperationCanceledException) { }

            _ = tooltip.Should().Be($"F1:{PromptPlusResources.TooltipBase}.Ctrl F1:{PromptPlusResources.TooltipShowHide}.");
        }
    }
}
