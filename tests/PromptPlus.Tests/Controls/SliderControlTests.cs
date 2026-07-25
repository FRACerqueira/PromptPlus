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
    // Grupo 1 (FASE2-CONTROLS-PLAN.md) — SliderControl, sem ModeView. Confirmado por leitura:
    // cancelamento já seta ResultCtrl corretamente (linha 347-351). Default: Range(0,100),
    // Step=ranger/100=1, LargeStep=ranger/10=10, Layout=LeftRight.
    [Collection(FileHistoryCollection.Name)]
    public class SliderControlTests : IDisposable
    {
        private const string HistoryFile = "slider-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public SliderControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static ISliderControl MakeSlider(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Slider("Volume");

        [Fact]
        public void Enter_confirms_the_default_value_the_minimum()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be(0);
        }

        [Fact]
        public void RightArrow_increments_by_the_step()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(1);
        }

        [Fact]
        public void LeftArrow_decrements_back()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(1);
        }

        [Fact]
        public void LeftArrow_at_the_minimum_is_a_no_op()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(0);
        }

        [Fact]
        public void Tab_increments_by_the_large_step()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(10);
        }

        [Fact]
        public void ShiftTab_decrements_by_the_large_step()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab, shift: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(10);
        }

        [Fact]
        public void RightArrow_at_the_maximum_is_a_no_op()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt).Range(0, 5).Default(5);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(5);
        }

        [Fact]
        public void Escape_aborts_and_keeps_the_value_set_at_the_time_of_cancel()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be(1);
        }

        [Fact]
        public void Cancellation_with_no_key_returns_a_null_aborted_result()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
        }

        [Fact]
        public void Default_sets_the_initial_value()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt).Default(42);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(42);
        }

        [Fact]
        public void UpDown_layout_uses_UpArrow_and_DownArrow_instead_of_LeftRight()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt).Layout(SliderLayout.UpDown);
            _ = vt.Keys.Enqueue(ConsoleKey.UpArrow).Enqueue(ConsoleKey.UpArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(1);
        }

        [Fact]
        public void LeftRight_keys_are_ignored_in_UpDown_layout()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt).Layout(SliderLayout.UpDown);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(0);
        }

        [Fact]
        public void EnabledHistory_persists_the_confirmed_value_and_reloads_it_as_the_default()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt).EnabledHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            var control2 = new PromptPlusControls(vt2, new PromptConfig()).Slider("Volume").EnabledHistory(HistoryFile);
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Should().Be(10);
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeSlider(vt);

            using var cts0 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts0.Token);
            _ = vt.Find("Enter:Finish").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeSlider(vt2);
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
            var control = MakeSlider(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeSlider(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }
    }
}
