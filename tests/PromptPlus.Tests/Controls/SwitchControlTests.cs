using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // SwitchControl, no ModeView. Confirmed by reading the source: cancellation already sets
    // ResultCtrl correctly (line 220-224) — not the same as InputControl's bug #8.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class SwitchControlTests : IDisposable
    {
        private const string HistoryFile = "switch-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public SwitchControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static ISwitchControl MakeSwitch(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Switch("Enabled");

        [Fact]
        public void Enter_confirms_the_default_value_off()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be(false);
        }

        [Fact]
        public void RightArrow_turns_it_on()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(true);
        }

        [Fact]
        public void LeftArrow_turns_it_off_again()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(false);
        }

        [Fact]
        public void Spacebar_toggles_the_current_value()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(true);
        }

        [Fact]
        public void Escape_aborts_and_keeps_the_value_set_at_the_time_of_cancel()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be(true);
        }

        [Fact]
        public void Cancellation_with_no_key_returns_a_null_aborted_result()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
        }

        [Fact]
        public void Default_sets_the_initial_value()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt).Default(true);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(true);
        }

        [Fact]
        public void OnValue_and_OffValue_change_the_rendered_answer_text()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt).OnValue("Yes").OffValue("No");
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            _ = vt.Find("Yes").Should().NotBeNull();
        }

        [Fact]
        public void EnableHistory_persists_the_confirmed_value_and_reloads_it_as_the_default()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt).EnableHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            var control2 = new PromptPlusControls(vt2, new PromptConfig()).Switch("Enabled").EnableHistory(HistoryFile);
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Should().Be(true);
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);

            using var cts0 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts0.Token);
            _ = vt.Find("Enter:Finish").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeSwitch(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1);
            using var cts1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts1.Token);
            _ = vt2.Find("Abort").Should().NotBeNull();
            _ = vt2.Find("Enter:Finish").Should().BeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeSwitch(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeSwitch(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }
    }
}
