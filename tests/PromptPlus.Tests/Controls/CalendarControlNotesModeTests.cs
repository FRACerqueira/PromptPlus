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
    // CalendarControl, `ShowNotes` mode (F2, only available when the current day has at least one
    // note). Global behavior and `Input` mode are in CalendarControlTests.cs.
    //
    // Confirmed by probe, not a bug: the text buffer (`EmacsConsoleBuffer`) used to display the
    // selected note is effectively read-only — typing a printable letter NEVER edits the note text;
    // instead it jumps (jump-by-letter, with wraparound) to the next note in the list whose text
    // starts with that letter. Only navigation keys (arrows/Home/End, via
    // `TryAcceptedReadlineConsoleKey`) move the cursor within the displayed note's viewport.
    //
    // Confirmed by probe: `Enter` inside ShowNotes mode first resets back to `Input` mode and THEN
    // continues the normal confirmation flow (using `_selectedDate`, which never changed while
    // viewing notes) — i.e. `Enter` from the notes view closes the view AND confirms the calendar
    // in the same keypress, not just a "close notes" action.
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

            // Match on a short prefix instead of the whole resource string, in case rendering
            // clips it at narrow terminal widths.
            _ = vt.Find("Ctrl+B:Move back").Should().BeNull();
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

            _ = vt.Find("Ctrl+B:Move back").Should().NotBeNull();
        }
    }
}
