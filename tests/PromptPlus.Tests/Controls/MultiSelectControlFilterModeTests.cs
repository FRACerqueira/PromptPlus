using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Fase 2, Grupo 2 (FASE2-CONTROLS-PLAN.md) — MultiSelectControl, modo `Filter` (FilterMode !=
    // Disabled, digitação real). Globais e modo `Select` (incluindo a visão "só selecionados", que
    // NÃO troca o ModeView) estão em MultiSelectControlTests.cs.
    public class MultiSelectControlFilterModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IMultiSelectControl<string> MakeFilterable(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItems(["Apple", "Banana", "Berry", "Cherry"])
                .Filter(FilterMode.StartsWith);

        [Fact]
        public void Typing_a_character_enters_filter_mode_and_narrows_the_list()
        {
            var vt = MakeTerminal();
            var control = MakeFilterable(vt);
            _ = vt.Keys.Type("b");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: b (Filter)").Should().NotBeNull();
            _ = vt.TextAt(1, 0, 11).Should().Be(">[ ] Banana");
            _ = vt.TextAt(2, 0, 10).Should().Be(" [ ] Berry");
            _ = vt.Find("Apple").Should().BeNull();
            _ = vt.Find("Cherry").Should().BeNull();
        }

        [Fact]
        public void Backspacing_the_filter_to_empty_returns_to_the_unfiltered_list()
        {
            var vt = MakeTerminal();
            var control = MakeFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Backspace);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Apple").Should().NotBeNull();
            _ = vt.Find("Cherry").Should().NotBeNull();
            _ = vt.Find("Qty:4 items").Should().NotBeNull();
        }

        [Fact]
        public void Space_inside_filter_mode_checks_the_highlighted_item()
        {
            var vt = MakeTerminal();
            var control = MakeFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: b (Filter)").Should().NotBeNull();
            _ = vt.TextAt(1, 0, 11).Should().Be(">[x] Banana");
            _ = vt.Find("1 selected").Should().NotBeNull();
        }

        [Fact]
        public void F2_toggle_all_only_affects_the_filtered_subset()
        {
            var vt = MakeTerminal();
            var control = MakeFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Both filtered items (Banana, Berry) get checked; Apple/Cherry are outside the
            // filter and were never touched by this F2 press.
            _ = vt.TextAt(1, 0, 11).Should().Be(">[x] Banana");
            _ = vt.TextAt(2, 0, 10).Should().Be(" [x] Berry");
            _ = vt.Find("2 selected").Should().NotBeNull();
        }

        [Fact]
        public void F2_toggle_all_within_the_filter_again_unchecks_only_the_filtered_subset()
        {
            var vt = MakeTerminal();
            var control = MakeFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 11).Should().Be(">[ ] Banana");
            _ = vt.TextAt(2, 0, 10).Should().Be(" [ ] Berry");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void F2_unchecking_the_filtered_subset_ignores_the_predicate()
        {
            // Same rule as the global F2 in ModeView.Select: the predicate only gates checking,
            // never unchecking — including when the toggle-all is scoped to the filtered subset.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItem("Apple").AddItem("Banana", ischecked: true).AddItem("Berry", ischecked: true).AddItem("Cherry")
                .Filter(FilterMode.StartsWith)
                .PredicateChecked(_ => false);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 11).Should().Be(">[ ] Banana");
            _ = vt.TextAt(2, 0, 10).Should().Be(" [ ] Berry");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void Arrow_navigation_works_while_the_filter_is_active()
        {
            var vt = MakeTerminal();
            var control = MakeFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Berry"]);
        }

        [Fact]
        public void Escape_while_filtering_aborts_and_returns_an_empty_array()
        {
            var vt = MakeTerminal();
            var control = MakeFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeEmpty();
        }
    }
}
