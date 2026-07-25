using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.History;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Fase 2, Grupo 2 (FASE2-CONTROLS-PLAN.md) — MultiSelectControl, modo `Select` (globais +
    // check/uncheck/grupos/toggle-all fora de filtro). Cenários do modo `Filter` estão em
    // MultiSelectControlFilterModeTests.cs. Checklist levantado lendo TryResult/BufferTemplate reais
    // (MultiSelectControl.cs) + sondas de render (mesma técnica do piloto Select/Input).
    //
    // Diferença confirmada por sonda vs. o piloto Select: no cancelamento (Escape ou timeout de
    // segurança), MultiSelect sempre retorna Content=[] (array vazio) — não preserva os itens
    // marcados até aquele momento, diferente do Select (que preserva o item destacado). Ver
    // MultiSelectControl.cs:573/585 (`ResultCtrl = new ResultPrompt<T[]>([], true)`).
    [Collection(FileHistoryCollection.Name)]
    public class MultiSelectControlTests : IDisposable
    {
        private const string HistoryFile = "multiselect-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public MultiSelectControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IMultiSelectControl<string> MakeMultiSelect(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose").AddItems(["A", "B", "C"]);

        private static IMultiSelectControl<string> MakeMultiSelectWithDisabledItem(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItem("A").AddItem("B", disable: true).AddItem("C");

        [Fact]
        public void Initial_render_shows_the_list_with_all_items_unchecked()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.TextAt(1, 0, 6).Should().Be(">[ ] A");
            _ = vt.TextAt(2, 0, 6).Should().Be(" [ ] B");
            _ = vt.TextAt(3, 0, 6).Should().Be(" [ ] C");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void DownArrow_moves_the_selection_to_the_next_item()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 6).Should().Be(" [ ] A");
            _ = vt.TextAt(2, 0, 6).Should().Be(">[ ] B");
        }

        [Fact]
        public void Space_checks_the_selected_item_and_updates_the_answer()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 9).Should().Be("Choose: A");
            _ = vt.TextAt(1, 0, 6).Should().Be(">[x] A");
            _ = vt.Find("1 selected").Should().NotBeNull();
        }

        [Fact]
        public void Space_again_unchecks_the_item_and_clears_the_answer()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.TextAt(1, 0, 6).Should().Be(">[ ] A");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void Enter_confirms_with_all_checked_items()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo(["A", "B"]);
        }

        [Fact]
        public void Escape_aborts_and_returns_an_empty_array()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeEmpty();
            _ = vt.Find("Canceled").Should().NotBeNull();
        }

        [Fact]
        public void Space_on_an_item_rejected_by_the_sync_predicate_shows_the_default_error_and_does_not_check_it()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt).PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
            _ = vt.TextAt(1, 0, 6).Should().Be(">[ ] A");
        }

        [Fact]
        public void Space_on_an_item_rejected_by_the_sync_predicate_with_custom_message_shows_it()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt).PredicateChecked(_ => (false, "Custom rejection"));
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void Space_on_an_item_rejected_by_the_async_predicate_shows_an_error_and_does_not_check_it()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt).PredicateCheckedAsync(_ => System.Threading.Tasks.Task.FromResult(false));
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
            _ = vt.TextAt(1, 0, 6).Should().Be(">[ ] A");
        }

        [Fact]
        public void Space_unchecking_an_already_checked_item_ignores_the_predicate()
        {
            // The predicate only gates checking an item — unchecking a disabled-by-predicate item
            // must still be allowed, otherwise it could never be removed from the checked set again.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItem("A", ischecked: true).AddItem("B")
                .PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.TextAt(1, 0, 6).Should().Be(">[ ] A");
            _ = vt.Find("0 selected").Should().NotBeNull();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().BeNull();
        }

        [Fact]
        public void Space_on_a_group_header_toggles_every_item_in_the_group()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddGroupedItem("G1", "A").AddGroupedItem("G1", "B").AddGroupedItem("G2", "C");
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: A,B").Should().NotBeNull();
            _ = vt.Find("2 selected").Should().NotBeNull();
            // G2's own item stays untouched — only G1's children are affected.
            _ = vt.Find("[ ] C").Should().NotBeNull();
        }

        [Fact]
        public void Space_on_a_group_header_again_unchecks_the_whole_group()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddGroupedItem("G1", "A").AddGroupedItem("G1", "B");
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void Space_on_a_group_header_unchecking_the_whole_group_ignores_the_predicate()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddGroupedItem("G1", "A", ischecked: true).AddGroupedItem("G1", "B", ischecked: true)
                .PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void F2_checks_every_item_when_none_are_fully_checked()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 13).Should().Be("Choose: A,B,C");
            _ = vt.Find("3 selected").Should().NotBeNull();
            _ = vt.TextAt(1, 0, 6).Should().Be(">[x] A");
            _ = vt.TextAt(2, 0, 6).Should().Be(" [x] B");
            _ = vt.TextAt(3, 0, 6).Should().Be(" [x] C");
        }

        [Fact]
        public void F2_checking_all_still_skips_only_the_items_the_predicate_rejects()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt).PredicateChecked(x => x != "B");
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 6).Should().Be(">[x] A");
            _ = vt.TextAt(2, 0, 6).Should().Be(" [ ] B");
            _ = vt.TextAt(3, 0, 6).Should().Be(" [x] C");
            _ = vt.Find("2 selected").Should().NotBeNull();
        }

        [Fact]
        public void F2_unchecking_all_ignores_the_predicate()
        {
            // Once every item is checked, the same F2 press flips to the "uncheck all" direction —
            // which must not be gated by the predicate (only checking is gated).
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItem("A", ischecked: true).AddItem("B", ischecked: true)
                .PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void F2_again_unchecks_every_item_once_all_are_checked()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.Find("0 selected").Should().NotBeNull();
        }

        [Fact]
        public void F3_enters_and_then_exits_the_only_selected_view()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Only-selected view: the list narrows to the single checked item.
            _ = vt.TextAt(1, 0, 6).Should().Be(">[x] A");
            _ = vt.Find("[ ] B").Should().BeNull();
            _ = vt.Find("Qty:1 items").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeMultiSelect(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.F3);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            // Second F3 exits the view: the full list is restored, selection is preserved.
            _ = vt2.Find("[ ] B").Should().NotBeNull();
            _ = vt2.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void F2_from_within_the_only_selected_view_unchecks_everything_and_restores_the_full_list()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.Find("0 selected").Should().NotBeNull();
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void F2_from_within_the_only_selected_view_keeps_the_answer_line_in_sync_when_a_disabled_item_survives()
        {
            // Bug fix: the "uncheck all" branch of F2 skips disabled items (they cannot be
            // unchecked), so a disabled+checked item can survive it. The answer line used to be
            // unconditionally cleared regardless, so it displayed "nothing selected" while the
            // control still had 1 item checked (and would still confirm it on Enter). Fixed to
            // rebuild the answer text from the real checked set, like every other branch does.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItem("A", ischecked: true, disable: true)
                .AddItem("B", ischecked: true)
                .AddItem("C");
            _ = vt.Keys.Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            // Confirmed via the final answer line (FinishTemplate re-renders it from the same
            // _answerBuffer) and the returned Content — both must agree that A is still checked.
            _ = vt.TextAt(0, 0, 9).Should().Be("Choose: A");
            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo(["A"]);
        }

        [Fact]
        public void Enter_below_the_minimum_range_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt).Range(2);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("Minimum selection of 2 items is required").Should().NotBeNull();
        }

        [Fact]
        public void Enter_above_the_maximum_range_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt).Range(0, 1);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("Maximum item selection(1) has been exceeded").Should().NotBeNull();
        }

        [Fact]
        public void ViewOnly_ignores_space_and_Enter_confirms_the_initial_checked_items()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItem("A", ischecked: true).AddItem("B").ViewOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo(["A"]);
        }

        [Fact]
        public void Navigating_onto_a_disabled_item_shows_an_error_but_still_selects_it()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelectWithDisabledItem(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 6).Should().Be(">[ ] B");
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void Tab_key_is_ignored_while_an_item_is_selected()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_item_starting_with_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItems(["Apple", "Banana", "Berry", "Cherry"]);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo(["Banana"]);
        }

        [Fact]
        public void Separator_items_are_excluded_from_the_pagination_count_and_rendered_as_a_line()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItem("A").AddSeparator().AddItem("B");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 6).Should().Be(" -----");
            _ = vt.Find("Qty:2 items").Should().NotBeNull();
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);

            using var cts0 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts0.Token);
            _ = vt.Find("Enter:Finish").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeMultiSelect(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1);
            using var cts1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts1.Token);
            _ = vt2.Find("Enter:Finish").Should().BeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeMultiSelect(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }

        [Fact]
        public void Group_description_tip_line_shows_the_current_items_group()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddGroupedItem("G1", "A");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(1, 0, 2).Should().Be("G1");
        }

        [Fact]
        public void EnabledHistory_persists_the_checked_items_and_reloads_them_as_defaults()
        {
            var vt = MakeTerminal();
            var control = MakeMultiSelect(vt).EnabledHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            var control2 = new PromptPlusControls(vt2, new PromptConfig()).MultiSelect<string>("Choose").AddItems(["A", "B", "C"])
                .EnabledHistory(HistoryFile).UseDefaultHistory();
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Should().BeEquivalentTo(["A", "B"]);
        }

        [Fact]
        public void AddSeparator_line_spans_the_display_width_of_a_wide_cjk_item_not_its_character_count()
        {
            // Regression: _lengthSeparationline used to be computed from item.Text.Length (character
            // count). "가나다" is 3 characters but 6 display columns, so the old bug sized the
            // separator line 3 columns short of the item it spans. Baseline (ASCII-only items "A"/"B",
            // same shape as the existing "Separator_items_are_excluded..." test) renders 5 dashes
            // (1 + selected-symbol width + 1); swapping in a 6-column-wide item must grow the line by
            // exactly the extra 5 columns (3 chars -> 6 columns), to 10 dashes.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MultiSelect<string>("Choose")
                .AddItems(["가나다", "B"]).AddSeparator(SeparatorLine.UserChar, '-');

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(new string('-', 10)).Should().NotBeNull();
            _ = vt.Find(new string('-', 11)).Should().BeNull();
        }
    }
}
