using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Fase 2, Grupo 2 (FASE2-CONTROLS-PLAN.md) — TableControl, modo `Filter` (FilterMode != Disabled),
    // cobrindo os dois FilterTableMode: `Answer` (busca pelo texto de resposta/coluna atual) e
    // `ColumnFilters` (busca só pela coluna atual, exige isFilterable). Globais e modo `Select`
    // estão em TableControlTests.cs.
    public class TableControlFilterModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private sealed record Person(string Name, int Age);

        private static ITableControl<Person> MakeAnswerFilterable(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Apple", 1), new Person("Banana", 2), new Person("Berry", 3), new Person("Cherry", 4)])
                .Filter(FilterMode.StartsWith); // FilterTableMode defaults to Answer

        private static ITableControl<Person> MakeColumnFiltersTable(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name, isFilterable: true)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Apple", 1), new Person("Banana", 2), new Person("Berry", 3), new Person("Cherry", 4)])
                .Filter(FilterMode.StartsWith, FilterTableMode.ColumnFilters);

        [Fact]
        public void Typing_a_character_enters_filter_mode_and_narrows_the_list_answer_mode()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: b (Filter)").Should().NotBeNull();
            _ = vt.Find("Apple").Should().BeNull();
            _ = vt.Find("Cherry").Should().BeNull();
            _ = vt.Find("Banana").Should().NotBeNull();
            _ = vt.Find("Berry").Should().NotBeNull();
            _ = vt.Find("Qty:2 items").Should().NotBeNull();
        }

        [Fact]
        public void Answer_mode_filters_even_when_no_column_is_marked_filterable()
        {
            // FilterTableMode.Answer never depends on isFilterable — confirmed distinct from
            // ColumnFilters, which requires it (see the ColumnFilters tests below).
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt); // no column marked isFilterable
            _ = vt.Keys.Type("ban");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = vt.Find("Banana").Should().NotBeNull();
        }

        [Fact]
        public void Backspacing_the_filter_to_empty_returns_to_the_unfiltered_list()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Backspace);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Apple").Should().NotBeNull();
            _ = vt.Find("Qty:4 items").Should().NotBeNull();
        }

        [Fact]
        public void Arrow_navigation_works_while_the_filter_is_active()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Value.Name.Should().Be("Berry");
        }

        [Fact]
        public void Escape_while_filtering_aborts_and_preserves_the_filtered_selection()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Value.Name.Should().Be("Banana");
        }

        [Fact]
        public void Tab_exits_filter_mode_in_answer_mode()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("(Filter)").Should().BeNull();
            _ = vt.Find("Apple").Should().NotBeNull();
            _ = vt.Find("Qty:4 items").Should().NotBeNull();
        }

        [Fact]
        public void ColumnFilters_mode_filters_by_the_current_filterable_column()
        {
            var vt = MakeTerminal();
            var control = MakeColumnFiltersTable(vt);
            _ = vt.Keys.Type("b");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Banana").Should().NotBeNull();
            _ = vt.Find("Berry").Should().NotBeNull();
            _ = vt.Find("Apple").Should().BeNull();
            _ = vt.Find("Qty:2 items").Should().NotBeNull();
        }

        [Fact]
        public void ColumnFilters_mode_header_marks_the_filterable_column_with_an_asterisk()
        {
            var vt = MakeTerminal();
            var control = MakeColumnFiltersTable(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Name *").Should().NotBeNull();
        }

        [Fact]
        public void Tab_exits_filter_mode_in_columnfilters_mode_instead_of_matching_nothing()
        {
            // Bug found and fixed: Tab used to change the current column mid-filter without
            // resetting anything. Since GetColumnFilterText only reads the current column, moving
            // to a non-filterable one made every row's filter text empty, and the active search
            // term stopped matching anything — the table appeared to go empty. Now Tab exits the
            // filter entirely (back to the full, unfiltered list) instead of carrying a stale term
            // over to a column it was never typed against.
            var vt = MakeTerminal();
            var control = MakeColumnFiltersTable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("(Filter)").Should().BeNull();
            _ = vt.Find("Apple").Should().NotBeNull();
            _ = vt.Find("Banana").Should().NotBeNull();
            _ = vt.Find("Qty:4 items").Should().NotBeNull();
        }

        [Fact]
        public void Typing_again_after_the_tab_that_exited_the_filter_starts_a_fresh_search_on_the_new_column()
        {
            // Not a bug: once the filter was exited by Tab, any further printable key starts a
            // brand new filter scoped to whichever column is now current. Age isn't filterable, so
            // GetColumnFilterText returns empty for every row and the fresh search matches nothing
            // — expected, since there is nothing to search on that column.
            var vt = MakeTerminal();
            var control = MakeColumnFiltersTable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Tab).Type("2");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("(Filter)").Should().NotBeNull();
            _ = vt.Find("Apple").Should().BeNull();
            _ = vt.Find("Banana").Should().BeNull();
            // No pagination footer at all when the filtered set is empty (PageCount == 0).
            _ = vt.Find("Qty:").Should().BeNull();
        }

        [Fact]
        public void ShiftTab_also_exits_filter_mode_in_columnfilters_mode()
        {
            var vt = MakeTerminal();
            var control = MakeColumnFiltersTable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Tab, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("(Filter)").Should().BeNull();
            _ = vt.Find("Qty:4 items").Should().NotBeNull();
        }
    }
}
