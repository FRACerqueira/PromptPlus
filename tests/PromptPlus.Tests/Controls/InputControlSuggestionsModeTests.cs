using ConsolePlusLibrary.Testing;
using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Core;
using System;
using System.Threading;
using Xunit;

namespace PromptPlus.Tests.Controls
{
    // Camada 2 (render + estado via VirtualTerminal) — piloto Fase 1, controle Input, "Sugestions".
    // Duas sub-suítes por comportamento (SuggestionHandler(autocomplete: bool)), não por ModeView:
    // autocomplete=true NUNCA sai de ModeView.Input (cicla inline); autocomplete=false entra em
    // ModeView.Sugestions (dropdown com paginação própria). Globais e modo `Input` básico estão em
    // InputControlTests.cs; modo `History` está em InputControlHistoryModeTests.cs.
    public class InputControlSuggestionsModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        private static readonly string[] Suggestions = ["Alpha", "Beta", "Gamma"];

        private static IInputControl MakeAutocompleteInput(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Input("Name")
                .SuggestionHandler(_ => Suggestions); // autocomplete: true (default) — stays ModeView.Input

        private static IInputControl MakeDropdownInput(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Input("Name")
                .SuggestionHandler(_ => Suggestions, autocomplete: false); // enters ModeView.Sugestions

        // ---- Autocomplete inline (stays ModeView.Input — no cancel-revert concern, unlike History/dropdown) ----

        [Fact]
        public void Tab_cycles_forward_through_suggestions_inline()
        {
            var vt = MakeTerminal();
            var control = MakeAutocompleteInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Name: Alpha");
        }

        [Fact]
        public void Tab_twice_cycles_to_the_second_suggestion()
        {
            var vt = MakeTerminal();
            var control = MakeAutocompleteInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 10).Should().Be("Name: Beta");
        }

        [Fact]
        public void Tab_wraps_around_after_the_last_suggestion()
        {
            var vt = MakeTerminal();
            var control = MakeAutocompleteInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 11).Should().Be("Name: Alpha");
        }

        [Fact]
        public void ShiftTab_has_no_effect_in_inline_autocomplete_mode()
        {
            // The outer guard (InputControl.cs:650) only routes Shift+Tab into the suggestions
            // branch when autocomplete is OFF: `keyinfo.IsPressTabKey() || (IsPressShiftTabKey() &&
            // !_autocompleteSuggestions)`. With autocomplete ON (this test), Shift+Tab falls through
            // to the generic key handler instead — backward-cycling is dropdown-only, not a general
            // "Tab forward / Shift+Tab backward" pair like the dropdown's own Tab/Shift+Tab.
            var vt = MakeTerminal();
            var control = MakeAutocompleteInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 6).Should().Be("Name: ");
        }

        [Fact]
        public void Tab_does_nothing_below_the_minimum_suggestion_length()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name")
                .SuggestionHandler(_ => Suggestions).MinimumSuggestionLength(3);
            _ = vt.Keys.Type("ab").Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 9).Should().Be("Name: ab ");
        }

        [Fact]
        public void Tab_does_nothing_when_the_handler_returns_no_suggestions()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Input("Name")
                .SuggestionHandler(_ => Array.Empty<string>());
            _ = vt.Keys.Type("x").Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 7).Should().Be("Name: x");
        }

        // ---- Dropdown (enters ModeView.Sugestions — confirm via Tab/Enter/Shift+Tab, never via
        // cancellation-timeout: the same InputControl cancel-revert already found for History mode
        // applies here too, restoring pre-dropdown text the moment Run() ends outside ModeView.Input) ----

        // No standalone "Tab opens the dropdown and renders the list" test: like History mode, a
        // cancellation-only snapshot right after opening the dropdown cannot be asserted (same
        // revert-on-cancel behavior found there — confirmed empirically here too), and every
        // functional aspect of "the dropdown opened with the right items" is already covered by the
        // Enter/Tab/DownArrow tests below, which observe it through a real confirm instead.

        [Fact]
        public void Tab_inside_the_dropdown_accepts_the_selected_suggestion()
        {
            var vt = MakeTerminal();
            var control = MakeDropdownInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Alpha");
        }

        [Fact]
        public void DownArrow_inside_the_dropdown_moves_to_the_next_suggestion_then_Tab_accepts_it()
        {
            var vt = MakeTerminal();
            var control = MakeDropdownInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("Beta");
        }

        [Fact]
        public void Enter_inside_the_dropdown_confirms_the_selected_suggestion_as_the_final_result()
        {
            var vt = MakeTerminal();
            var control = MakeDropdownInput(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be("Alpha");
        }

        [Fact]
        public void ShiftTab_inside_the_dropdown_cancels_and_restores_the_text_typed_before_it()
        {
            var vt = MakeTerminal();
            var control = MakeDropdownInput(vt);
            _ = vt.Keys.Type("dr").Enqueue(ConsoleKey.Tab).Enqueue(ConsoleKey.Tab, shift: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("dr");
        }

        [Fact]
        public void Editing_inside_the_dropdown_exits_back_to_input_mode_keeping_the_edit()
        {
            // Opening the dropdown does NOT load the highlighted suggestion into the edit buffer —
            // only accepting it (Tab/Enter) does (InputControl.cs:459/717). While just browsing,
            // typing continues from whatever text was there BEFORE Tab was pressed, not from
            // "Alpha" — confirmed empirically (a first version of this test expected "Alpha!" and
            // got "!").
            var vt = MakeTerminal();
            var control = MakeDropdownInput(vt);
            _ = vt.Keys.Type("dr").Enqueue(ConsoleKey.Tab).Type("!").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be("dr!");
        }
    }
}
