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
    // SelectControl, `Select` mode (global behavior + navigation/editing outside of filter),
    // covering render + state via VirtualTerminal. `Filter` mode scenarios are in
    // SelectControlFilterModeTests.cs.
    //
    // Mandatory rule: every key sequence ends in Enter/Escape, and every Run() receives a
    // CancellationToken with a short timeout as a safety net — WaitKeypress only returns when a
    // key is available or the token cancels, with no exception if the queue drains before that.
    // VirtualTerminal uses the default dimensions (Width=80, Height=24): values below
    // MinSafeRenderWidth(80)/MinSafeRenderHeight(10) trap Run() in the "terminal too small"
    // warning (RenderBuffer, BaseControlPrompt.cs:1523-1530), which only exits via
    // console.CancelToken — not the token passed to Run() — and VirtualTerminal.CancelToken never
    // cancels.
    //
    // Scenarios that end in a validation error / disabled item never reach an Enter->confirm:
    // TryResult just calls SetError and returns false, so Run()'s loop goes back to waiting for
    // another key. Since those tests don't want to send a real key after the error, they reuse the
    // SAME safety net (a short CancellationTokenSource) used by the initial-render tests: the
    // cancellation (not a real Escape) is what ends Run(), and the final result comes back
    // Aborted because of that — not the app "aborting on its own", but a deliberate artifact of
    // the test harness.
    public class SelectControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static ISelectControl<string> MakeSelect(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose").AddItems(["A", "B", "C"]);

        private static ISelectControl<string> MakePagedSelect(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose").AddItems(["A", "B", "C", "D", "E"]).PageSize(2);

        private static ISelectControl<string> MakeSelectWithDisabledItem(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose")
                .AddItem("A").AddItem("B", disable: true).AddItem("C");

        [Fact]
        public void Initial_render_shows_the_list_with_the_first_item_selected()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);

            // No keys queued: WaitKeypress spins until the token cancels, leaving the FIRST
            // rendered frame on the grid (Select is not a "Live" control, so BaseControlPrompt.Run's
            // cancel-cleanup branch does not touch/clear it).
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 9).Should().Be("Choose: A");
            _ = vt.TextAt(1, 0, 3).Should().Be("> A");
            _ = vt.TextAt(2, 0, 3).Should().Be("  B");
            _ = vt.TextAt(3, 0, 3).Should().Be("  C");
            _ = vt.GetCursorPosition().Should().Be((8, 0));

            // Selected item's style must differ from an unselected item's — checked relatively
            // rather than against a hardcoded color, since the exact default theme is not part of
            // this contract.
            _ = vt.StyleAt(1, 2).Foreground.Should().NotBe(vt.StyleAt(2, 2).Foreground);
        }

        [Fact]
        public void DownArrow_moves_the_selection_to_the_next_item()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 9).Should().Be("Choose: B");
            _ = vt.TextAt(1, 0, 3).Should().Be("  A");
            _ = vt.TextAt(2, 0, 3).Should().Be("> B");
            _ = vt.TextAt(3, 0, 3).Should().Be("  C");
        }

        [Fact]
        public void Enter_confirms_the_selected_item_and_renders_the_final_answer()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("B");
            _ = vt.TextAt(0, 0, 9).Should().Be("Choose: B");
            _ = vt.GetCursorPosition().Should().Be((0, 1));
        }

        [Fact]
        public void ExtraInfo_is_appended_to_the_live_answer()
        {
            // The live answer line can scroll horizontally (ViewportSlice) when text overflows the
            // console width, unlike a list row — so it's a reliable place to surface ExtraInfo.
            // No key sent: the safety-net timeout ends Run() leaving this live frame on screen
            // (Select never reaches FinishTemplate on that path — see the class-level comment).
            var vt = MakeTerminal();
            var control = MakeSelect(vt).ExtraInfo(x => $"Length: {x.Length}");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: A (Length: 1)").Should().NotBeNull();
        }

        [Fact]
        public void ExtraInfo_overflowing_the_console_width_is_hidden_up_front()
        {
            // Motivating scenario for showing ExtraInfo in the live answer: WriteListSelect has no
            // viewport, so a long ExtraInfo can get cut off in the list row with no way back to it.
            // First, confirm the tail really is off-screen before scrolling.
            var vt = MakeTerminal();
            string longExtra = new string('X', 100) + "TAIL";
            var control = MakeSelect(vt).ExtraInfo(_ => longExtra);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("TAIL").Should().BeNull();
        }

        [Fact]
        public void ExtraInfo_overflow_is_reachable_by_scrolling_the_live_answer_with_End()
        {
            // ...and now confirm the answer line's horizontal scroll (Home/End/Left/Right) makes
            // that same tail reachable — the whole point of surfacing ExtraInfo here instead of
            // only in the list row.
            var vt = MakeTerminal();
            string longExtra = new string('X', 100) + "TAIL";
            var control = MakeSelect(vt).ExtraInfo(_ => longExtra);
            _ = vt.Keys.Enqueue(ConsoleKey.End);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("TAIL").Should().NotBeNull();
        }

        [Fact]
        public void ExtraInfo_is_not_appended_to_the_final_answer()
        {
            // The final answer (after Enter) intentionally stays plain text.
            var vt = MakeTerminal();
            var control = MakeSelect(vt).ExtraInfo(x => $"Length: {x.Length}");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("A");
            _ = vt.Find("(Length: 1)").Should().BeNull();
            _ = vt.TextAt(0, 0, 9).Should().Be("Choose: A");
        }

        [Fact]
        public void ExtraInfoAsync_is_appended_to_the_live_answer_but_not_to_the_final_one()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt).ExtraInfoAsync(x => System.Threading.Tasks.Task.FromResult<string?>($"Length: {x.Length}"));
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Choose: B (Length: 1)").Should().NotBeNull();
        }

        [Fact]
        public void Escape_aborts_and_keeps_the_item_selected_at_the_time_of_cancel()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("A");
            _ = vt.Find("Canceled").Should().NotBeNull();
        }

        [Fact]
        public void UpArrow_moves_the_selection_to_the_previous_item()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.UpArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("B");
        }

        [Fact]
        public void DownArrow_on_the_last_item_wraps_to_the_first_page()
        {
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.End, ctrl: true).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("A");
        }

        [Fact]
        public void UpArrow_on_the_first_item_wraps_to_the_last_page()
        {
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.UpArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("E");
        }

        [Fact]
        public void PageDown_moves_to_the_first_item_of_the_next_page()
        {
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("C");
        }

        [Fact]
        public void PageDown_wraps_around_from_the_last_page_back_to_the_first()
        {
            // A,B | C,D | E — Paginator.NextPage advances the page index modulo PageCount
            // (Paginator.cs:220: SelectedPage = (SelectedPage + 1) % PageCount), so paging is
            // cyclic, not clamped: a third PageDown from the last page wraps back to page 0.
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("A");
        }

        [Fact]
        public void PageUp_moves_to_the_last_item_of_the_previous_page()
        {
            // PageUp uses IndexOption.LastItemWhenHasPages (SelectControl.cs:623), which lands on
            // the LAST item of the previous page (not the first) — mirrors "scrolling up" UX.
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageDown).Enqueue(ConsoleKey.PageUp).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("D");
        }

        [Fact]
        public void PageUp_wraps_around_from_the_first_page_to_the_last()
        {
            // Same modulo wraparound as PageDown (Paginator.cs:239), the other direction.
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageUp).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("E");
        }

        [Fact]
        public void CtrlEnd_moves_directly_to_the_last_item()
        {
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.End, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("E");
        }

        [Fact]
        public void CtrlHome_moves_directly_back_to_the_first_item()
        {
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.End, ctrl: true).Enqueue(ConsoleKey.Home, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("A");
        }

        [Fact]
        public void CtrlHome_when_already_on_the_first_item_is_a_no_op()
        {
            var vt = MakeTerminal();
            var control = MakePagedSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Home, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("A");
        }

        [Fact]
        public void Navigating_onto_a_disabled_item_shows_an_error_but_still_selects_it()
        {
            var vt = MakeTerminal();
            var control = MakeSelectWithDisabledItem(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow);

            // Safety-net cancellation (see class remarks) — no terminal key needed, the error frame
            // is what we're asserting against.
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 3).Should().Be("> B");
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void Enter_on_a_disabled_item_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeSelectWithDisabledItem(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            // Ended by the safety-net timeout, not a real Escape — see class remarks. Content is
            // NOT asserted here: which of two different cancellation branches wins (BaseControlPrompt
            // .Run's own post-render check, which always uses default(T), vs TryResult's ReadNextKey
            // cancellation branch, which preserves SelectedItem) depends on exact render timing, not
            // on control behavior — asserting it would test a race, not a contract.
            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.SelectionDisabled).Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_sync_predicate_shows_the_default_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt).PredicateSelected(_ => false);
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
            var control = MakeSelect(vt).PredicateSelected(_ => (false, "Custom rejection"));
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
            var control = MakeSelect(vt).PredicateSelectedAsync(_ => System.Threading.Tasks.Task.FromResult(false));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void ViewOnly_Enter_confirms_without_running_validation()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt).ViewOnly().PredicateSelected(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("A");
        }

        [Fact]
        public void Tab_key_is_ignored_while_an_item_is_selected()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("A");
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_item_starting_with_it()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose")
                .AddItems(["Apple", "Banana", "Berry", "Cherry"]);
            _ = vt.Keys.Type("b").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Banana");
        }

        [Fact]
        public void Typing_a_letter_wraps_around_to_the_beginning_when_no_match_follows_the_current_item()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose")
                .AddItems(["Apple", "Banana", "Cherry"]);
            _ = vt.Keys.Enqueue(ConsoleKey.End, ctrl: true).Type("a").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Apple");
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);

            using var cts0 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts0.Token);
            _ = vt.Find("Enter:Finish").Should().NotBeNull();
            _ = vt.Find("Arrows:Navigate").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeSelect(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1);
            using var cts1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts1.Token);
            _ = vt2.Find("PgUp/PgDown:Move").Should().NotBeNull();
            _ = vt2.Find("Enter:Finish").Should().BeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeSelect(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeSelect(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }

        [Fact]
        public void AddSeparator_line_spans_the_display_width_of_a_wide_cjk_item_not_its_character_count()
        {
            // Regression: _lengthSeparationline used to be computed from item.Text.Length (character
            // count). "가나다" is 3 characters but 6 display columns, so the old bug sized the
            // separator line to 3 dashes instead of 6, leaving it shorter than the item it spans.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Select<string>("Choose")
                .AddItems(["가나다", "B"]).AddSeparator(SeparatorLine.UserChar, '-');

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(new string('-', 6)).Should().NotBeNull();
            _ = vt.Find(new string('-', 7)).Should().BeNull();
        }
    }
}
