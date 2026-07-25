using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Camada 2 (render + estado via VirtualTerminal) — piloto Fase 1, controle Select, modo `Filter`
    // (FilterMode != Disabled). Globais e modo `Select` estão em SelectControlTests.cs.
    public class SelectControlFilterModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static ISelectControl<string> MakeFilterableSelect(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose")
                .AddItems(["Apple", "Banana", "Berry", "Cherry"])
                .Filter(FilterMode.StartsWith);

        [Fact]
        public void Typing_a_character_enters_filter_mode_and_narrows_the_list()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableSelect(vt);
            _ = vt.Keys.Type("b");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 6).Should().Be("> Bana");
            _ = vt.TextAt(2, 0, 6).Should().Be("  Berr");
            _ = vt.Find("Apple").Should().BeNull();
            _ = vt.Find("Cherry").Should().BeNull();
        }

        [Fact]
        public void Typing_more_characters_refines_the_filter()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableSelect(vt);
            _ = vt.Keys.Type("be");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Banana").Should().BeNull();
            _ = vt.TextAt(1, 0, 6).Should().Be("> Berr");
        }

        [Fact]
        public void Backspacing_the_filter_to_empty_returns_to_the_unfiltered_list()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableSelect(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Backspace);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Apple").Should().NotBeNull();
            _ = vt.Find("Cherry").Should().NotBeNull();
        }

        [Fact]
        public void Filtering_down_to_a_single_item_with_AutoSelect_confirms_automatically()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableSelect(vt).AutoSelect();
            _ = vt.Keys.Type("ch");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Cherry");
        }

        [Fact]
        public void Filtering_down_to_a_single_item_without_AutoSelect_does_not_confirm_automatically()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableSelect(vt);
            _ = vt.Keys.Type("ch");

            // Ended by the safety-net timeout, not a real Enter/Escape — see SelectControlTests
            // remarks. Only the still-filtering screen state is asserted here (not IsAborted/Content),
            // since the exact result depends on which of two cancellation branches wins the race.
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Cherry").Should().NotBeNull();
        }

        [Fact]
        public void Arrow_navigation_works_while_the_filter_is_active()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableSelect(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Berry");
        }

        [Fact]
        public void Escape_while_filtering_aborts_and_keeps_the_item_selected_at_the_time_of_cancel()
        {
            var vt = MakeTerminal();
            var control = MakeFilterableSelect(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("Banana");
        }
    }
}
