using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // MaskEditControl<string> (`IMaskEditStringControl`).
    // MaskEdit has no `ModeView`; the single `MaskEditControl<T>` class dispatches behavior by
    // runtime type (T=string here). Suites split by TYPE instead of mode — see
    // MaskEditNumberControlTests.cs / MaskEditCurrencyControlTests.cs / MaskEditDateTimeControlTests.cs
    // for int/long, decimal/double, and DateTime/DateOnly/TimeOnly respectively. Behaviors shared
    // across all 4 types (Enter/Escape, tooltip cycle, Default/DefaultIfEmpty, predicate,
    // HideTipInputType) are exercised once here (the most representative type) and only smoke-
    // tested elsewhere to avoid duplicating the same assertions 4x.
    //
    // 4 real bugs found and fixed this session:
    // - `U[...]`/`{U[...]}` custom uppercase-letter masks wrongly accepted lowercase input
    //   (`Validchars` was set to `CharLowerLetters` instead of `CharUpperLetters` in both the
    //   single-char and group custom-bracket branches of `NormalizeStringMask`).
    // - F1 tooltip cycle unconditionally advertised 7 Emacs shortcuts (`MaskEditBuffer.
    //   GetEmacsTooltips()`) regardless of `ConsoleHandler.EnabledEmacs` — confirmed the
    //   underlying key handling was already correctly gated (predates this session), only the
    //   tooltip was out of sync. Fixed by threading `enabledEmacs` through, matching every other
    //   control's `GetEmacsTooltips(bool)` convention.
    // - Negative sign silently dropped from `MaskInteger`/`MaskLong` results (see
    //   MaskEditNumberControlTests.cs for the regression test — string masks aren't affected).
    // - 2 resource typos in the neutral (English) `.resx`: `TooltipJumpdelimiter` said
    //   "Tab/ShitTab" (missing the 'f', also wrong separator vs. every other locale's "Tab\
    //   ShiftTab"), and `MaskEditPosLetterLower` said "Letra (a-z)" (leftover Portuguese word,
    //   should be "Letter (a-z)" like its sibling keys).
    // Also confirmed as an eager-validation improvement (not a bug, a fail-fast enhancement):
    // `NumberFormat(...)`'s digit-count limits (10/19/28/15 for int/long/decimal/double) now
    // throw immediately when called, instead of lazily at Run() time.
    public class MaskEditStringControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IMaskEditStringControl<string> MakeMask(VirtualTerminal vt, string mask = "999-LLL")
            => new PromptPlusControls(vt, new PromptConfig()).MaskEdit("Choose").Mask(mask);

        [Fact]
        public void Initial_render_shows_the_prompt_placeholders_and_type_hint()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: ___-___");
            _ = vt.Find(PromptPlusResources.MaskEditPosNumeric).Should().NotBeNull();
        }

        [Fact]
        public void Typing_fills_positions_left_to_right_and_Enter_confirms_the_unmasked_value()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("123abc");
        }

        [Fact]
        public void ReturnWithMask_true_includes_the_literal_characters_in_the_result()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskEdit("Choose")
                .Mask("999-LLL", returnWithMask: true);
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("123-abc");
        }

        [Fact]
        public void Escape_aborts_with_a_null_result_and_the_canceled_answer()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);
            _ = vt.Keys.Type("123").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
            _ = vt.Find(PromptPlusResources.CanceledKey).Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_unfilled_positions_shows_the_pending_input_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);
            _ = vt.Keys.Type("12").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.MaskeditInputPending).Should().NotBeNull();
        }

        [Fact]
        public void An_out_of_alphabet_character_is_silently_rejected_and_does_not_advance_the_cursor()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);
            // '#' is not a valid digit for the leading '9' position.
            _ = vt.Keys.Type("#1");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: 1__-___");
        }

        [Fact]
        public void Backspace_clears_the_previous_position_and_moves_the_cursor_back()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);
            _ = vt.Keys.Type("123").Enqueue(ConsoleKey.Backspace);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: 12_-___");
        }

        [Fact]
        public void Custom_uppercase_bracket_rejects_lowercase_but_accepts_the_listed_letters()
        {
            // Regression for the bug fixed this session: Validchars was CharLowerLetters instead
            // of CharUpperLetters for the U[...] custom-bracket branch.
            var vt = MakeTerminal();
            var control = MakeMask(vt, "U[AB]");
            _ = vt.Keys.Type("zA").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("A");
        }

        [Fact]
        public void Custom_uppercase_group_rejects_lowercase_but_accepts_the_listed_letters()
        {
            // Same regression, group form: {UU}[AB].
            var vt = MakeTerminal();
            var control = MakeMask(vt, "{UU}[AB]");
            _ = vt.Keys.Type("zAAB").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("AA");
        }

        [Fact]
        public void Lowercase_custom_bracket_rejects_uppercase()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt, "L[ab]");
            _ = vt.Keys.Type("Za").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("a");
        }

        [Fact]
        public void Numeric_group_with_constant_renders_as_fixed_uneditable_digits()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt, "{999}(007)");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Choose: 007");
            _ = vt.Find(string.Format(System.Globalization.CultureInfo.InvariantCulture, PromptPlusResources.MaskEditPosConstant, '0')).Should().NotBeNull();
        }

        [Fact]
        public void Escaped_character_renders_as_a_fixed_literal_not_an_input_position()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt, "99\\9L");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 12).Should().Be("Choose: __9_");
        }

        [Fact]
        public void Default_prefills_the_buffer_shown_on_the_first_render()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt).Default("123abc");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: 123-abc");
        }

        [Fact]
        public void DefaultIfEmpty_is_returned_when_Enter_is_pressed_without_typing_anything()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt).DefaultIfEmpty("000aaa");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("000aaa");
        }

        [Fact]
        public void PredicateSelected_sync_rejecting_the_value_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt).PredicateSelected(_ => false);
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void PredicateSelected_with_a_custom_message_shows_it_instead_of_the_default()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt).PredicateSelected(_ => (false, "Custom rejection"));
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void PredicateSelectedAsync_rejecting_the_value_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt).PredicateSelectedAsync(_ => Task.FromResult(false));
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void PredicateSelected_accepting_the_value_confirms_normally()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt).PredicateSelected(v => v == "123abc");
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("123abc");
        }

        [Fact]
        public void HideTipInputType_suppresses_the_type_hint_line()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt).HideTipInputType();

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.MaskEditPosNumeric).Should().BeNull();
        }

        [Fact]
        public void Tab_and_ShiftTab_are_inert_for_string_masks()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);
            _ = vt.Keys.Type("1").Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab, shift: true).Type("2");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Neither Tab press moved the cursor away from the normal left-to-right fill order.
            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: 12_-___");
            _ = vt.Find(PromptPlusResources.TooltipJumpdelimiter).Should().BeNull();
        }

        [Fact]
        public void F1_cycles_the_tooltip_without_advertising_emacs_shortcuts_when_disabled()
        {
            var vt = MakeTerminal();
            // vt.EnabledEmacs defaults to false.
            var control = MakeMask(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1).Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.Emac_ctrl_l).Should().BeNull();
        }

        [Fact]
        public void F1_cycle_reaches_the_emacs_shortcuts_when_enabled()
        {
            var vt = MakeTerminal();
            vt.EnabledEmacs = true;
            var control = MakeMask(vt);
            for (int i = 0; i < 9; i++) _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.Emac_ctrl_d).Should().NotBeNull();
        }

        [Fact]
        public void EmacsCtrlL_clears_the_field_when_emacs_is_enabled()
        {
            var vt = MakeTerminal();
            vt.EnabledEmacs = true;
            var control = MakeMask(vt);
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.L, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: ___-___");
        }

        [Fact]
        public void EmacsCtrlL_does_nothing_when_emacs_is_disabled()
        {
            var vt = MakeTerminal();
            var control = MakeMask(vt);
            _ = vt.Keys.Type("123abc").Enqueue(ConsoleKey.L, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: 123-abc");
        }
    }
}
