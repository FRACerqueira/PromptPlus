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
    // Fase 2, Grupo 2 (FASE2-CONTROLS-PLAN.md) — TableControl, modo `Select` (globais + navegação de
    // linha/coluna/ViewOnly/disabled/predicado fora de filtro). Cenários do modo `Filter`
    // (Answer/ColumnFilters) estão em TableControlFilterModeTests.cs. Checklist levantado lendo
    // TryResult/BufferTemplate/FinishTemplate reais (TableControl.cs) + sondas de render.
    //
    // Diferenças confirmadas por sonda vs. o piloto Select/MultiSelect:
    // - Cancelamento por Escape REAL preserva linha/coluna atuais (igual Select); cancelamento por
    //   timeout (sem tecla) sempre devolve TableResult<T> default (Value nulo, Row/Col=0).
    // - A "linha de resposta" (WriteAnswer/FinishTemplate) usa o valor da CÉLULA DA COLUNA ATUAL
    //   (GetAnswerText), não `value.ToString()` como a doc do TextSelector sugere — Tab muda qual
    //   célula aparece como resposta, tanto durante a navegação quanto no Enter final.
    [Collection(SerializedGlobalStateCollection.Name)]
    public class TableControlTests : IDisposable
    {
        private const string HistoryFile = "table-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public TableControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private sealed record Person(string Name, int Age);

        private static ITableControl<Person> MakeTable(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Ann", 30), new Person("Bob", 25), new Person("Cid", 40)]);

        [Fact]
        public void Initial_render_shows_headers_borders_and_the_first_row_selected()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
            _ = vt.TextAt(1, 0, 19).Should().Be("  +--------+-------");
            _ = vt.TextAt(2, 0, 19).Should().Be("  |> Name  |  Age  ");
            _ = vt.TextAt(4, 0, 19).Should().Be("> |Ann     |30     ");
            _ = vt.TextAt(6, 0, 19).Should().Be("  |Bob     |25     ");
            _ = vt.TextAt(8, 0, 19).Should().Be("  |Cid     |40     ");
            _ = vt.Find("Qty:3 items").Should().NotBeNull();
            _ = vt.Find("Col: 1/2").Should().NotBeNull();
        }

        [Fact]
        public void DownArrow_moves_the_selection_to_the_next_row()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(4, 0, 19).Should().Be("  |Ann     |30     ");
            _ = vt.TextAt(6, 0, 19).Should().Be("> |Bob     |25     ");
        }

        [Fact]
        public void Tab_moves_the_column_cursor_forward_and_wraps()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 19).Should().Be("  |  Name  |> Age  ");
            _ = vt.Find("Col: 2/2").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeTable(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.TextAt(2, 0, 19).Should().Be("  |> Name  |  Age  ");
            _ = vt2.Find("Col: 1/2").Should().NotBeNull();
        }

        [Fact]
        public void ShiftTab_moves_the_column_cursor_backward_and_wraps()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 19).Should().Be("  |  Name  |> Age  ");
            _ = vt.Find("Col: 2/2").Should().NotBeNull();
        }

        [Fact]
        public void Enter_confirms_the_selected_row_with_its_row_and_column_coordinates()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Value.Should().Be(new Person("Bob", 25));
            _ = result.Content.RowIndex.Should().Be(1);
            _ = result.Content.ColumnIndex.Should().Be(0);
            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Bob");
        }

        [Fact]
        public void The_answer_text_reflects_the_current_columns_cell_not_a_fixed_column()
        {
            // GetAnswerText falls back to the cell of whatever column is currently focused when no
            // TextSelector is configured — Tab changes which cell is shown as the confirmed answer.
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Value.Should().Be(new Person("Bob", 25));
            _ = result.Content.ColumnIndex.Should().Be(1);
            _ = vt.TextAt(0, 0, 10).Should().Be("Choose: 25");
        }

        [Fact]
        public void Escape_real_key_aborts_and_preserves_the_row_and_column_at_time_of_cancel()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Value.Should().Be(new Person("Bob", 25));
            _ = result.Content.RowIndex.Should().Be(1);
            _ = vt.Find("Canceled").Should().NotBeNull();
        }

        [Fact]
        public void Cancellation_without_a_real_keypress_returns_a_default_result()
        {
            // Different from a real Escape: the safety-net timeout cancellation (press.IsCancelled)
            // always returns TableResult<T> default, it does not preserve wherever the cursor was.
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Value.Should().BeNull();
            _ = result.Content.RowIndex.Should().Be(0);
        }

        [Fact]
        public void ViewOnly_Enter_after_navigating_returns_the_initial_item_and_its_own_coordinates()
        {
            // Regression: Value and RowIndex/ColumnIndex used to describe two different rows (Value
            // from the initial item, coordinates from wherever the cursor had browsed to). Fixed so
            // both always describe the initial item.
            var vt = MakeTerminal();
            var control = MakeTable(vt).ViewOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Value.Should().Be(new Person("Ann", 30));
            _ = result.Content.RowIndex.Should().Be(0);
            _ = result.Content.ColumnIndex.Should().Be(0);
            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
        }

        [Fact]
        public void ViewOnly_Escape_after_navigating_also_returns_the_initial_items_coordinates()
        {
            // Same rule applied to abort: the display (FinishTemplate) always shows the initial
            // item's text in view-only mode, so the returned result must match it instead of
            // returning wherever the cursor had browsed to.
            var vt = MakeTerminal();
            var control = MakeTable(vt).ViewOnly();
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Value.Should().Be(new Person("Ann", 30));
            _ = result.Content.RowIndex.Should().Be(0);
            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: Ann");
        }

        [Fact]
        public void Navigating_onto_a_disabled_row_shows_an_error_but_still_selects_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItem(new Person("Ann", 30))
                .AddItem(new Person("Bob", 25), disable: true)
                .AddItem(new Person("Cid", 40));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void Enter_on_a_disabled_row_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItem(new Person("Ann", 30))
                .AddItem(new Person("Bob", 25), disable: true)
                .AddItem(new Person("Cid", 40));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_sync_predicate_shows_the_default_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).PredicateSelected(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_sync_predicate_shows_the_custom_message()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).PredicateSelected(_ => (false, "Custom rejection"));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_async_predicate_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).PredicateSelectedAsync(_ => System.Threading.Tasks.Task.FromResult(false));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void PageDown_wraps_around_from_the_last_page_back_to_the_first()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItems([new("A", 1), new("B", 2), new("C", 3), new("D", 4), new("E", 5)])
                .PageSize(2);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Value.Name.Should().Be("A");
        }

        [Fact]
        public void PageUp_lands_on_the_last_item_of_the_previous_page()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItems([new("A", 1), new("B", 2), new("C", 3), new("D", 4), new("E", 5)])
                .PageSize(2);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageUp).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Value.Name.Should().Be("D");
        }

        [Fact]
        public void CtrlEnd_then_CtrlHome_moves_to_the_last_then_back_to_the_first_row()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name)
                .AddItems([new("A", 1), new("B", 2), new("C", 3), new("D", 4), new("E", 5)])
                .PageSize(2);
            _ = vt.Keys.Enqueue(ConsoleKey.End, ctrl: true).Enqueue(ConsoleKey.Home, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Value.Name.Should().Be("A");
        }

        [Fact]
        public void Typing_a_letter_does_nothing_when_no_column_is_filterable()
        {
            // Discovered gap: jump-by-letter relies on item.FilterableText, which is only populated
            // from columns marked isFilterable — without one, typing silently does not move the cursor,
            // even though the "Jump" tooltip used to be advertised regardless (fixed: see the
            // filterable-gated tooltip tests below).
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Type("c").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Value.Should().Be(new Person("Ann", 30));
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_row_when_a_column_is_filterable()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name, isFilterable: true)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Ann", 30), new Person("Bob", 25), new Person("Cid", 40)]);
            _ = vt.Keys.Type("c").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Value.Should().Be(new Person("Cid", 40));
        }

        [Fact]
        public void Jump_tooltip_is_hidden_when_no_column_is_filterable()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1).Enqueue(ConsoleKey.F1).Enqueue(ConsoleKey.F1).Enqueue(ConsoleKey.F1).Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipTableJump).Should().BeNull();
        }

        [Fact]
        public void Jump_tooltip_is_shown_when_a_column_is_filterable()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Table<Person>("Choose")
                .AddColumn("Name", p => p.Name, isFilterable: true)
                .AddColumn("Age", p => p.Age)
                .AddItems([new Person("Ann", 30), new Person("Bob", 25), new Person("Cid", 40)]);
            _ = vt.Keys.Enqueue(ConsoleKey.F1).Enqueue(ConsoleKey.F1).Enqueue(ConsoleKey.F1);

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
        public void EnableHistory_persists_the_confirmed_row_and_reloads_it_as_the_default()
        {
            var vt = MakeTerminal();
            var control = MakeTable(vt).EnableHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            var control2 = MakeTable(vt2).EnableHistory(HistoryFile).UseDefaultHistory();
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Value.Should().Be(new Person("Bob", 25));
        }
    }
}
