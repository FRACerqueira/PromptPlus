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
    // Fase 2, Grupo 4 (FASE2-CONTROLS-PLAN.md) — CalendarControl, modo `ShowNotes` (F2, só quando
    // o dia atual tem ao menos uma nota). Globais e modo `Input` estão em CalendarControlTests.cs.
    //
    // Achado confirmado por sonda, não é bug: o buffer de texto (`EmacsConsoleBuffer`) usado pra
    // exibir a nota selecionada é somente-leitura de fato — digitar uma letra imprimível NUNCA
    // edita o texto da nota; em vez disso, pula (jump-by-letter, com wraparound) pra próxima nota
    // da lista cujo texto comece com essa letra. Só teclas de navegação (setas/Home/End, via
    // `TryAcceptedReadlineConsoleKey`) movem o cursor dentro do viewport da nota exibida.
    //
    // Achado confirmado por sonda: `Enter` dentro do modo ShowNotes primeiro reseta de volta pro
    // modo `Input` e ENTÃO continua o fluxo normal de confirmação (usa `_selectedDate`, que nunca
    // mudou enquanto via notas) — ou seja, `Enter` a partir das notas fecha a visão E confirma o
    // calendário no mesmo pressionar de tecla, não é só um "fechar notas".
    public class CalendarControlNotesModeTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        // March 15 2024 is a Friday.
        private static ICalendarControl MakeCalendarWithNote(VirtualTerminal vt, string note = "Meeting")
            => new PromptPlusControls(vt, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15))
                .AddNote(new DateTime(2024, 3, 15), note);

        [Fact]
        public void F2_switches_to_notes_mode_and_shows_the_note_list()
        {
            var vt = MakeTerminal();
            var control = MakeCalendarWithNote(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(string.Format(System.Globalization.CultureInfo.InvariantCulture, PromptPlusResources.ShowingCalendarNotes, "3/15/2024"))
                .Should().NotBeNull();
            _ = vt.Find("Meeting").Should().NotBeNull();
            _ = vt.Find("Qty:1 items").Should().NotBeNull();
        }

        [Fact]
        public void F2_is_a_noop_when_the_current_day_has_no_note()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15));
            _ = vt.Keys.Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Still showing the calendar grid, not the notes view.
            _ = vt.TextAt(2, 0, 37).Should().Be("| March                        2024 |");
        }

        [Fact]
        public void F2_again_exits_notes_mode_back_to_the_calendar_grid()
        {
            var vt = MakeTerminal();
            var control = MakeCalendarWithNote(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.F2);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(2, 0, 37).Should().Be("| March                        2024 |");
        }

        [Fact]
        public void Typing_a_letter_jumps_to_the_next_note_starting_with_it_instead_of_editing()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15))
                .AddNote(new DateTime(2024, 3, 15), "Apple meeting")
                .AddNote(new DateTime(2024, 3, 15), "Banana call");
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Type("b");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("> Banana call").Should().NotBeNull();
        }

        [Fact]
        public void DownArrow_moves_the_selection_to_the_next_note()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15))
                .AddNote(new DateTime(2024, 3, 15), "First note")
                .AddNote(new DateTime(2024, 3, 15), "Second note");
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.DownArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("> Second note").Should().NotBeNull();
        }

        [Fact]
        public void Tab_is_inert_inside_notes_mode()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15))
                .AddNote(new DateTime(2024, 3, 15), "First note")
                .AddNote(new DateTime(2024, 3, 15), "Second note");
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Still on the first note - Tab did not move the selection or leave notes mode.
            _ = vt.Find("> First note").Should().NotBeNull();
        }

        [Fact]
        public void Enter_from_notes_mode_exits_and_confirms_the_calendars_selected_date()
        {
            var vt = MakeTerminal();
            var control = MakeCalendarWithNote(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be(new DateTime(2024, 3, 15));
        }

        [Fact]
        public void Escape_from_notes_mode_aborts_the_whole_control()
        {
            var vt = MakeTerminal();
            var control = MakeCalendarWithNote(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
        }

        [Fact]
        public void Tooltip_in_notes_mode_shows_the_jump_and_prompt_navigation_hints()
        {
            var vt = MakeTerminal();
            var control = MakeCalendarWithNote(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2).Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipPages).Should().NotBeNull();
        }

        [Fact]
        public void Emacs_readonly_tooltip_is_hidden_in_notes_mode_when_disabled()
        {
            var vt = MakeTerminal();
            var control = MakeCalendarWithNote(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2);
            for (int i = 0; i < 5; i++) _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Full Emac_ctrl_b text is longer than the terminal width and gets clipped in
            // rendering, so match on a short prefix instead of the whole resource string.
            _ = vt.Find("Ctrl+B:Moves the cursor back").Should().BeNull();
        }

        [Fact]
        public void Emacs_readonly_tooltip_appears_in_notes_mode_when_enabled()
        {
            var vt = MakeTerminal();
            vt.EnabledEmacs = true;
            var control = MakeCalendarWithNote(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F2);
            for (int i = 0; i < 5; i++) _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Ctrl+B:Moves the cursor back").Should().NotBeNull();
        }
    }
}
