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
    // TableMultiSelectControl, `Select` mode (global navigation + check/uncheck/columns/F2/F3/Range/
    // ViewOnly/disabled/predicate outside of filter). Combines the two already-validated
    // playbooks: MultiSelect (check/uncheck semantics) + Table (columns, Tab/ShiftTab,
    // GetAnswerText by current column). `Filter` mode scenarios are in
    // TableMultiSelectControlFilterModeTests.cs. Checklist derived from reading the real
    // TryResult/InitControl plus render probes.
    //
    // Fixes applied this session (same patterns already fixed in MultiSelect/Table — see
    // promptplus-multi-predicate-rule and promptplus-naming-audit-checklist):
    // - `PredicateSelected`/`PredicateSelectedAsync` -> `PredicateChecked`/`PredicateCheckedAsync`.
    // - The predicate only validates on CHECK (Space) — unchecking never called the predicate on
    //   the mass toggle (F2), but individual Space called it unconditionally; fixed.
    // - `Tab`/`Shift+Tab` now exit filter mode (`ExitFilterMode()`) before changing column, in
    //   both `FilterTableMode` values — without this, `ColumnFilters` emptied the list when
    //   changing column mid-filter (same bug as Table).
    // - The jump tooltip now uses `TooltipTableJump` and only appears with at least one
    //   `isFilterable` column (same fix as Table).
    [Collection(SerializedGlobalStateCollection.Name)]
    public class TableMultiSelectControlTests : IDisposable
    {
        private const string HistoryFile = "multitable-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public TableMultiSelectControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private sealed record Person(string Name, int Age);

        private static ITableMultiSelectControl<Person> MakeTable(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Ann", 30), new Person("Bob", 25), new Person("Cid", 40)]);

        [Fact]
        public void Initial_render_shows_headers_borders_checkboxes_and_the_first_row_selected()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
            _ = vt.TextAt(1, 0, 23).Should().Be("  +---+--------+-------");
            _ = vt.TextAt(2, 0, 23).Should().Be("  | # |> Name  |  Age  ");
            _ = vt.TextAt(4, 0, 23).Should().Be("> |[ ]|Ann     |30     ");
            _ = vt.TextAt(6, 0, 23).Should().Be("  |[ ]|Bob     |25     ");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Space_checks_the_row_and_the_answer_reflects_the_focused_row_and_column()
        {
            // The answer follows the cursor (row + currently Tab-focused column), not a running
            // summary of checked rows — here it happens to be the same row that was just checked,
            // but that's incidental (cursor never moved off row 1; only the column changed).
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 10).Should().Be("Choose: 30");
            _ = vt.TextAt(2, 0, 23).Should().Be("  | # |  Name  |> Age  ");
            _ = vt.TextAt(4, 0, 23).Should().Be("> |[x]|Ann     |30     ");
            _ = vt.Find("1 selected").Should().NotBeNull();
        }

        [Fact]
        public void Space_again_unchecks_the_row()
        {
            // Cursor stays parked on row 1 ("Ann") throughout — Space never moves it.
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
            // Unlike MultiSelect, the "N selected" suffix is only appended when N > 0 — with
            // nothing checked, it's omitted entirely rather than showing "0 selected".
            _ = vt.Find("selected").Should().BeNull();
        }

        [Fact]
        public void Enter_confirms_all_checked_rows()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo([new Person("Ann", 30), new Person("Bob", 25)]);
            // FinishTemplate must still show the checked-items summary (not the cursor's last
            // position, which was row 2/"Bob") — the discriminating check for the whole design:
            // live answer follows the cursor, final answer summarizes the result.
            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: Ann,Bob");
        }

        [Fact]
        public void Escape_aborts_and_returns_an_empty_array()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeEmpty();
        }

        [Fact]
        public void Space_unchecking_an_already_checked_row_ignores_the_predicate()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItem(new Person("Ann", 30), ischecked: true)
                .AddItem(new Person("Bob", 25))
                .PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(4, 0, 7).Should().Be("> |[ ]|");
            // Unlike MultiSelect, the "N selected" suffix is only appended when N > 0 — with
            // nothing checked, it's omitted entirely rather than showing "0 selected".
            _ = vt.Find("selected").Should().BeNull();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().BeNull();
        }

        [Fact]
        public void Space_checking_a_row_rejected_by_the_sync_predicate_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
            // Unlike MultiSelect, the "N selected" suffix is only appended when N > 0 — with
            // nothing checked, it's omitted entirely rather than showing "0 selected".
            _ = vt.Find("selected").Should().BeNull();
        }

        [Fact]
        public void Space_checking_a_row_rejected_with_a_custom_message_shows_it()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).PredicateChecked(_ => (false, "Custom rejection"));
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void Space_checking_a_row_rejected_by_the_async_predicate_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).PredicateCheckedAsync(_ => System.Threading.Tasks.Task.FromResult(false));
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void F2_checks_every_row_when_none_are_fully_checked()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // The answer follows the cursor (still parked on row 1, "Ann") — F2 doesn't navigate —
            // not a running summary of every checked row.
            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
            _ = vt.Find("3 selected").Should().NotBeNull();
        }

        [Fact]
        public void F2_again_unchecks_every_row_once_all_are_checked()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
            // Unlike MultiSelect, the "N selected" suffix is only appended when N > 0 — with
            // nothing checked, it's omitted entirely rather than showing "0 selected".
            _ = vt.Find("selected").Should().BeNull();
        }

        [Fact]
        public void F2_checking_all_skips_only_the_rows_the_predicate_rejects()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).PredicateChecked(p => p.Name != "Bob");
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("2 selected").Should().NotBeNull();
            _ = vt.TextAt(6, 0, 7).Should().Be("  |[ ]|");
        }

        [Fact]
        public void F2_unchecking_all_ignores_the_predicate()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItem(new Person("Ann", 30), ischecked: true)
                .AddItem(new Person("Bob", 25), ischecked: true)
                .PredicateChecked(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Unlike MultiSelect, the "N selected" suffix is only appended when N > 0 — with
            // nothing checked, it's omitted entirely rather than showing "0 selected".
            _ = vt.Find("selected").Should().BeNull();
        }

        [Fact]
        public void F3_enters_and_then_exits_the_only_selected_view()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Bob").Should().BeNull();
            _ = vt.Find("Qty:1 items").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeTable(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.F3);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Bob").Should().NotBeNull();
            _ = vt2.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void F2_from_within_the_only_selected_view_unchecks_everything_and_restores_the_full_list()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.F3).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Exiting the view resets the cursor to the first row ("Ann") via UpdateCollection's
            // default FirstItem() fallback.
            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
            // Unlike MultiSelect, the "N selected" suffix is only appended when N > 0 — with
            // nothing checked, it's omitted entirely rather than showing "0 selected".
            _ = vt.Find("selected").Should().BeNull();
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
        }

        [Fact]
        public void Enter_below_the_minimum_range_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).Range(2);
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
            var control = MakeTable(vt).Range(0, 1);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("Maximum item selection(1) has been exceeded").Should().NotBeNull();
        }

        [Fact]
        public void Navigating_onto_a_disabled_row_shows_an_error_but_still_selects_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItem(new Person("Ann", 30))
                .AddItem(new Person("Bob", 25), disable: true);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
            _ = vt.TextAt(6, 0, 1).Should().Be(">");
        }

        [Fact]
        public void Space_on_a_disabled_row_shows_an_error_and_does_not_toggle_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItem(new Person("Ann", 30))
                .AddItem(new Person("Bob", 25), disable: true);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
            _ = vt.TextAt(6, 0, 7).Should().Be("> |[ ]|");
        }

        [Fact]
        public void ViewOnly_ignores_space_and_Enter_confirms_the_initial_checked_rows()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItem(new Person("Ann", 30), ischecked: true)
                .AddItem(new Person("Bob", 25))
                .ViewOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().BeEquivalentTo([new Person("Ann", 30)]);
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_row_starting_with_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItems([new Person("Apple", 1), new Person("Banana", 2), new Person("Berry", 3)]);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo([new Person("Banana", 2)]);
        }

        [Fact]
        public void Jump_tooltip_is_hidden_when_no_column_is_filterable()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            for (int i = 0; i < 8; i++) _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipTableJump).Should().BeNull();
        }

        [Fact]
        public void Jump_tooltip_is_shown_when_a_column_is_filterable()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).TableMultiSelect<Person>("Choose")
                .AddColumn("Name", p => p.Name, isFilterable: true)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Ann", 30), new Person("Bob", 25), new Person("Cid", 40)]);
            for (int i = 0; i < 4; i++) _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipTableJump).Should().NotBeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeTable(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }

        [Fact]
        public void EnableHistory_persists_the_checked_rows_and_reloads_them_as_defaults()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).EnableHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            var control2 = MakeTable(vt2).EnableHistory(HistoryFile).UseDefaultHistory();
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Should().BeEquivalentTo([new Person("Ann", 30), new Person("Bob", 25)]);
        }
    }
}
