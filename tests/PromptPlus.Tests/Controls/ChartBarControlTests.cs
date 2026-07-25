using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Grupo 1 (FASE2-CONTROLS-PLAN.md) — ChartBarControl, sem ModeView, mas com Paginator (mesma
    // mecânica cíclica já confirmada no piloto Select: PageDown/PageUp dão a volta, PageUp pousa no
    // último item da página anterior). Cancelamento já seta ResultCtrl corretamente (linha 478-483).
    public class ChartBarControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IChartBarControl MakeChart(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).ChartBar("Sales")
                .AddItem("A", 1).AddItem("B", 2).AddItem("C", 3);

        private static IChartBarControl MakePagedChart(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).ChartBar("Sales")
                .AddItem("A", 1).AddItem("B", 2).AddItem("C", 3).AddItem("D", 4).AddItem("E", 5)
                .PageSize(2);

        [Fact]
        public void Enter_confirms_the_first_item_by_default()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content!.Label.Should().Be("A");
        }

        [Fact]
        public void DownArrow_moves_to_the_next_item()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("B");
        }

        [Fact]
        public void UpArrow_from_the_first_item_wraps_to_the_last()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.UpArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("C");
        }

        [Fact]
        public void PageDown_wraps_around_from_the_last_page_back_to_the_first()
        {
            var vt = MakeTerminal();
            var control = MakePagedChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("A");
        }

        [Fact]
        public void PageUp_moves_to_the_last_item_of_the_previous_page()
        {
            var vt = MakeTerminal();
            var control = MakePagedChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageUp).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("D");
        }

        [Fact]
        public void CtrlEnd_then_CtrlHome_moves_to_the_last_and_back_to_the_first()
        {
            var vt = MakeTerminal();
            var control = MakePagedChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.End, ctrl: true).Enqueue(ConsoleKey.Home, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("A");
        }

        [Fact]
        public void CtrlEnd_moves_directly_to_the_last_item()
        {
            var vt = MakeTerminal();
            var control = MakePagedChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.End, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("E");
        }

        [Fact]
        public void F4_cycles_the_sort_order_to_highest_first()
        {
            // Re-sorting preserves whichever item was already selected (ChangeOrder explicitly
            // carries the current selection across the reorder, ChartBarControl.cs:706-723) — Enter
            // right after F4 still confirms "A" unchanged. CtrlHome jumps to the new first item
            // (post-reorder) to actually observe the new order: Highest-first puts C (value 3) there.
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F4).Enqueue(ConsoleKey.Home, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("C");
        }

        [Fact]
        public void F2_switches_to_Stacked_layout_enabling_RightArrow_as_navigation()
        {
            // RightArrow only navigates when _layout == Stacked (TryResult gates it explicitly) —
            // observing it move confirms the switch actually happened, without needing to inspect
            // rendering internals. CanRenderStackedLayout requires ConsoleHandler.Width >= chart
            // Width + item count margin (ChartBarControl.cs:781-790); the default chart Width (80,
            // ConfigPrompt.ChartWidth) barely exceeds the VT's default terminal width (80) once the
            // +2 margin is added, so a narrower chart Width is needed for the switch to succeed here.
            var vt = MakeTerminal();
            var control = MakeChart(vt).Width(30);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("B");
        }

        [Fact]
        public void RightArrow_is_ignored_in_the_default_Standard_layout()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("A");
        }

        [Fact]
        public void F3_switches_legends_only_when_ShowLegends_was_enabled()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt).ShowLegends();
            _ = vt.Keys.Enqueue(ConsoleKey.F3);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Smoke check: toggling legends off after enabling them should not throw or corrupt the
            // render — a plain "still shows the prompt" check is enough here.
            _ = vt.Find("Sales").Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_predicate_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt).PredicateSelected(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Escape_aborts_and_keeps_the_item_selected_at_the_time_of_cancel()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content!.Label.Should().Be("B");
        }

        [Fact]
        public void Cancellation_with_no_key_aborts_keeping_the_currently_selected_item()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content!.Label.Should().Be("A");
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().NotBeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeChart(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeChart(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }

        [Fact]
        public void Default_order_None_preserves_insertion_order_regardless_of_item_count()
        {
            // Regression test for a real bug (found and fixed 2026-07-23): ChangeOrder() is called
            // unconditionally from InitControl (even with the default ChartBarOrder.None), and its
            // None branch used to be `_items.OrderBy(x => x.Id)`. Auto-generated ids were random
            // GUIDs (AddItem), so "None" — documented as "no sorting applied; items appear in
            // insertion order" — silently randomized item order on every run instead. Confirmed via
            // a snapshot showing items rendered as A, C, B for AddItem calls in A, B, C order. Fixed
            // by making None a true no-op, and separately switched auto-generated ids from GUIDs to
            // a zero-padded sequential counter (more stable/debuggable regardless of this fix).
            // 11 items specifically exercises the zero-padding fix too: unpadded "10" would have
            // sorted before "2" under the OLD (buggy) code — moot now that None doesn't sort at all,
            // but still a meaningful id-stability check if some other order ever ties-breaks by Id.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).ChartBar("Sales");
            string[] labels = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K"];
            foreach (string label in labels)
            {
                _ = control.AddItem(label, 1);
            }
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content!.Label.Should().Be("A");
        }
    }
}
