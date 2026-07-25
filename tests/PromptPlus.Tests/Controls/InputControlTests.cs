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
    // Camada 2 (render + estado via VirtualTerminal) — piloto Fase 1, controle Input, modo `Input`
    // (globais + edição básica). Modo `History` está em InputControlHistoryModeTests.cs, modo
    // `Sugestions` em InputControlSuggestionsModeTests.cs.
    // Mesmas regras de SelectControlTests (tecla terminal + CancellationToken de seguranca).
    public class InputControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static IInputControl MakeInput(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Input("Name");

        [Fact]
        public void Typed_characters_are_echoed_next_to_the_prompt()
        {
            var vt = MakeTerminal();
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Joe");

            // No terminal key queued: WaitKeypress spins until the token cancels, leaving the last
            // typed frame on the grid uncleared (same technique as SelectControlTests).
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 9).Should().Be("Name: Joe");
        }

        [Fact]
        public void Backspace_removes_the_last_typed_character()
        {
            var vt = MakeTerminal();
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Backspace).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Jo");
        }

        [Fact]
        public void Enter_confirms_the_typed_text_and_renders_the_final_answer()
        {
            var vt = MakeTerminal();
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Joe");
            _ = vt.TextAt(0, 0, 9).Should().Be("Name: Joe");
            _ = vt.GetCursorPosition().Should().Be((0, 1));
        }

        [Fact]
        public void Escape_aborts_and_keeps_the_text_typed_so_far()
        {
            var vt = MakeTerminal();
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("Joe");
            _ = vt.Find("Canceled").Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_sync_predicate_shows_an_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name").PredicateValid(_ => false);
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Enter);

            // Ended by the safety-net timeout, not a real Escape — same technique/caveat as
            // SelectControlTests (see that class's remarks).
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("Joe");
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_a_failing_predicate_shows_the_custom_message()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name")
                .PredicateValid(_ => (false, "Custom rejection"));
            _ = vt.Keys.Type("Joe").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().Be("Joe");
            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void Cancellation_while_a_validation_error_is_showing_does_not_erase_it()
        {
            // Regression test for a real bug (found and fixed 2026-07-23):
            // InputControl.TryResult's cancellation branch didn't set ResultCtrl (unlike
            // SelectControl's), so it returned false even on a genuine cancellation. That made
            // BaseControlPrompt.Run's outer loop think one more render pass was needed before
            // stopping — that extra pass rebuilt the template with WriteError's one-shot error
            // state already cleared from the PREVIOUS pass, silently wiping the error off the
            // screen right before the control actually exited. Confirmed with a mid-flight snapshot
            // (taken on a background thread, before cancelling) showing the error WAS rendered, vs.
            // a final snapshot (after cancellation) showing it gone. Fixed by setting ResultCtrl in
            // that branch, mirroring SelectControl's.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name").PredicateValid(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Enter_on_empty_input_with_DefaultIfEmpty_confirms_the_default_value()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name").DefaultIfEmpty("Anonymous");
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Anonymous");
        }

        [Fact]
        public void CtrlAltTab_and_the_filter_activation_key_are_ignored()
        {
            var vt = MakeTerminal();
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Jo").Enqueue(ConsoleKey.Tab, alt: true, ctrl: true).Type("e").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Joe");
        }

        // Correction (2026-07-23): an earlier version of this comment claimed
        // `ConsoleHandler.EnabledEmacs` (InputControl.cs:357) read a static global singleton, and
        // skipped these tests to avoid mutating shared state across parallel test classes. That was
        // wrong — verified by reading BaseControlPrompt.cs:149: `ConsoleHandler` is an INSTANCE
        // property (`public IConsole ConsoleHandler => console;`), so `ConsoleHandler.EnabledEmacs`
        // resolves to the injected VirtualTerminal's own `EnabledEmacs` (a plain per-instance
        // property, VirtualTerminal.cs:90) — nothing global or shared between tests.

        [Fact]
        public void CtrlA_then_CtrlK_through_the_control_jumps_home_and_kills_to_the_end()
        {
            // Routes through TryAcceptedReadlineConsoleKey into the already-unit-tested
            // EmacsConsoleBuffer (see EmacsConsoleBufferTests.cs) — this only proves InputControl's
            // TryResult actually dispatches these keys to it, which the buffer's own unit tests can't.
            var vt = MakeTerminal();
            vt.EnabledEmacs = true;
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Hello").Enqueue(ConsoleKey.A, ctrl: true).Enqueue(ConsoleKey.K, ctrl: true).Type("Hi").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Hi");
        }

        [Fact]
        public void CtrlE_through_the_control_jumps_to_the_end()
        {
            var vt = MakeTerminal();
            vt.EnabledEmacs = true;
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Hello").Enqueue(ConsoleKey.A, ctrl: true).Enqueue(ConsoleKey.E, ctrl: true).Type("!").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Hello!");
        }

        [Fact]
        public void CtrlA_then_CtrlK_is_a_no_op_when_emacs_bindings_are_disabled_on_the_console()
        {
            // EnabledEmacs defaults to false on VirtualTerminal (same as a real console that hasn't
            // opted in) — Ctrl+A/Ctrl+K fall through to the generic key handler and do nothing, so
            // typing continues from wherever the cursor already was (end of "Hello").
            var vt = MakeTerminal();
            var control = MakeInput(vt);
            _ = vt.Keys.Type("Hello").Enqueue(ConsoleKey.A, ctrl: true).Enqueue(ConsoleKey.K, ctrl: true).Type("Hi").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("HelloHi");
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeInput(vt);

            using var cts0 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts0.Token);
            _ = vt.Find("Enter:Finish").Should().NotBeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeInput(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1);
            using var cts1 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts1.Token);
            _ = vt2.Find("Moves the cursor within the prompt").Should().NotBeNull();
            _ = vt2.Find("Enter:Finish").Should().BeNull();
        }

        [Fact]
        public void CtrlF1_hides_and_then_shows_the_tooltip_again()
        {
            var vt = MakeTerminal();
            var control = MakeInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1, ctrl: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Tips.").Should().BeNull();

            var vt2 = MakeTerminal();
            var control2 = MakeInput(vt2);
            _ = vt2.Keys.Enqueue(ConsoleKey.F1, ctrl: true).Enqueue(ConsoleKey.F1, ctrl: true);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.Find("Tips.").Should().NotBeNull();
        }
    }
}
