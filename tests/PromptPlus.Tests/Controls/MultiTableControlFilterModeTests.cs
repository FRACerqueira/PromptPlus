using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // MultiTableControl, `Filter` mode (FilterMode != Disabled), covering both FilterTableMode
    // values: `Answer` (no dependency on isFilterable) and `ColumnFilters` (depends on the
    // current column — which is why Tab/ShiftTab exit the filter, see ExitFilterMode). Globals
    // and `Select` mode are in MultiTableControlTests.cs.
    public class MultiTableControlFilterModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private sealed record Person(string Name, int Age);

        private static IMultiTableControl<Person> MakeAnswerFilterable(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).MultiTable<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Apple", 1), new Person("Banana", 2), new Person("Berry", 3), new Person("Cherry", 4)])
                .Filter(FilterMode.StartsWith); // FilterTableMode defaults to Answer

        private static IMultiTableControl<Person> MakeColumnFiltersTable(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).MultiTable<Person>("Choose")
                .AddColumn("Name", p => p.Name, isFilterable: true)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Apple", 1), new Person("Banana", 2), new Person("Berry", 3), new Person("Cherry", 4)])
                .Filter(FilterMode.StartsWith, FilterTableMode.ColumnFilters);

        [Fact]
        public void Typing_a_character_enters_filter_mode_and_narrows_the_list()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("b (Filter)").Should().NotBeNull();
            _ = vt.Find("Apple").Should().BeNull();
            _ = vt.Find("Cherry").Should().BeNull();
            _ = vt.Find("Banana").Should().NotBeNull();
            _ = vt.Find("Berry").Should().NotBeNull();
            _ = vt.Find("Qty:2 items").Should().NotBeNull();
        }

        [Fact]
        public void Answer_mode_filters_even_when_no_column_is_marked_filterable()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt); // no column marked isFilterable
            _ = vt.Keys.Type("ban");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            _ = vt.Find("Banana").Should().NotBeNull();
        }

        [Fact]
        public void Space_checks_the_highlighted_row_while_filtering()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Spacebar);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("1 selected").Should().NotBeNull();
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
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().BeEquivalentTo([new Person("Berry", 3)]);
        }

        [Fact]
        public void Escape_while_filtering_aborts_and_returns_an_empty_array()
        {
            var vt = MakeTerminal();
            var control = MakeAnswerFilterable(vt);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Spacebar).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeEmpty();
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
        public void Tab_exits_filter_mode_in_columnfilters_mode_instead_of_matching_nothing()
        {
            // Bug found and fixed (same as TableControl): Tab used to change the current column
            // mid-filter without resetting anything. Since GetColumnFilterText only reads the
            // current column, moving to a non-filterable one made every row's filter text empty,
            // and the active search term stopped matching anything. Now Tab exits the filter
            // entirely (back to the full, unfiltered list) instead of carrying a stale term over.
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
