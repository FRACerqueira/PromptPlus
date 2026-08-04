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
    // CalendarControl, `Input` mode (global navigation across date/month/year/week plus
    // Range/disabled/predicate outside of Notes mode). `ShowNotes` mode scenarios are in
    // CalendarControlNotesModeTests.cs. Checklist derived from reading the real
    // TryResult/InitControl/BufferTemplate plus render probes.
    //
    // All tests pin `.Default(new DateTime(2024, 3, 15))` (a Friday) — the control uses
    // `DateTime.Today` as its anchor when no Default is given, which would make most tests
    // non-deterministic (dependent on the day the suite runs). Only the dedicated Home test
    // (`Home_navigates_to_todays_date`) compares against `DateTime.Today` dynamically.
    //
    // Confirmed by probe, not a bug: navigating (Tab/ShiftTab/PageUp/PageDown/arrows/Home) sets
    // `_selectedDate = _currentDate` whenever the date is valid (`IsValidSelect`), WITHOUT
    // re-evaluating the predicate (`PredicateSelected`) — the predicate is only checked on `Enter`
    // (`ValidateSelection`). This means navigating to a date rejected by the predicate leaves it
    // "selected" (shown with the selection marker), and only `Enter` surfaces the error. The
    // INITIAL selection (`InitControl`), however, respects the predicate from the very first
    // render — if the predicate rejects the default date, `_selectedDate` starts out null and
    // `Enter` shows "Invalid date selected!" (not the predicate's message, which is never
    // evaluated in that case).
    [Collection(SerializedGlobalStateCollection.Name)]
    public class CalendarControlTests : IDisposable
    {
        private const string HistoryFile = "calendar-history-tests";
        private readonly IFileSystem _original = FileHistory.FileSystem;
        private readonly MockFileSystem _mock = new();

        public CalendarControlTests() => FileHistory.FileSystem = _mock;
        public void Dispose() => FileHistory.FileSystem = _original;

        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        // March 15 2024 is a Friday.
        private static ICalendarControl MakeCalendar(VirtualTerminal vt)
            => new PromptPlusControls(vt, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15));

        [Fact]
        public void Initial_render_shows_the_month_grid_with_today_and_the_selected_day_marked()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 17).Should().Be("Choose: 3/15/2024");
            _ = vt.TextAt(2, 0, 37).Should().Be("| March                        2024 |");
            _ = vt.TextAt(4, 0, 37).Should().Be("| Sun  Mon  Tue  Wed  Thu <Fri> Sat |");
            _ = vt.TextAt(8, 0, 37).Should().Be("|  10   11   12   13   14  <15>  16 |");
            _ = vt.Find(PromptPlusResources.TooltipBaseNavegate).Should().NotBeNull();
        }

        [Fact]
        public void Tab_moves_to_the_next_month()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 17).Should().Be("Choose: 4/15/2024");
            _ = vt.TextAt(2, 0, 37).Should().Be("| April                        2024 |");
        }

        [Fact]
        public void ShiftTab_moves_to_the_previous_month()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Tab, shift: true);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 17).Should().Be("Choose: 2/15/2024");
        }

        [Fact]
        public void PageUp_moves_to_the_next_year()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageUp);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 17).Should().Be("Choose: 3/15/2025");
        }

        [Fact]
        public void PageDown_moves_to_the_previous_year()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.PageDown);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 17).Should().Be("Choose: 3/15/2023");
        }

        [Fact]
        public void RightArrow_moves_one_day_forward_and_Enter_confirms_it()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be(new DateTime(2024, 3, 16));
        }

        [Fact]
        public void LeftArrow_moves_one_day_backward()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateTime(2024, 3, 14));
        }

        [Fact]
        public void DownArrow_moves_one_week_forward_same_weekday()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.DownArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateTime(2024, 3, 22));
        }

        [Fact]
        public void UpArrow_moves_one_week_backward()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.UpArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateTime(2024, 3, 8));
        }

        [Fact]
        public void CtrlN_acts_as_the_emacs_equivalent_of_DownArrow_regardless_of_EnabledEmacs()
        {
            var vt = MakeTerminal();
            // vt.EnabledEmacs defaults to false - these directional aliases aren't gated on it.
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.N, ctrl: true).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateTime(2024, 3, 22));
        }

        [Fact]
        public void Home_navigates_to_todays_date()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Home).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(DateTime.Today);
        }

        [Fact]
        public void Range_blocks_navigation_past_the_maximum_date()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).Range(new DateTime(2024, 3, 1), new DateTime(2024, 3, 20));
            _ = vt.Keys.Enqueue(ConsoleKey.Tab); // would move to April, out of range

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 17).Should().Be("Choose: 3/15/2024");
            _ = vt.TextAt(2, 0, 37).Should().Be("| March                        2024 |");
        }

        [Fact]
        public void Range_blocks_navigation_before_the_minimum_date()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).Range(new DateTime(2024, 3, 10), new DateTime(2024, 3, 20));
            _ = vt.Keys.Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.LeftArrow)
              .Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.LeftArrow).Enqueue(ConsoleKey.LeftArrow);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(8, 0, 37).Should().Be("| <10>  11   12   13   14   15   16 |");
        }

        [Fact]
        public void Range_throws_when_min_is_after_max()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);

            Action act = () => control.Range(new DateTime(2024, 3, 20), new DateTime(2024, 3, 10));

            _ = act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void DisabledWeekend_blocks_selection_on_a_weekend_day()
        {
            var vt = MakeTerminal();
            // March 16 2024 is a Saturday.
            var control = new PromptPlusControls(vt, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 16))
                .DisabledWeekend();
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt.Find(PromptPlusResources.InvalidDateSelect).Should().NotBeNull();
        }

        [Fact]
        public void DisableDates_blocks_selection_on_a_specific_date()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).DisableDates(new DateTime(2024, 3, 15));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.InvalidDateSelect).Should().NotBeNull();
        }

        [Fact]
        public void Disabled_date_uses_a_visually_distinct_style()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).DisableDates(new DateTime(2024, 3, 10));

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            // Row 8 = "|  10   11   12   13   14  <15>  16 |" - day 10 (disabled) vs day 11 (enabled).
            _ = vt.StyleAt(8, 3).Foreground.Should().NotBe(vt.StyleAt(8, 8).Foreground);
        }

        [Fact]
        public void Highlights_marks_a_date_with_the_highlight_glyph()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).Highlights(new DateTime(2024, 3, 20));

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(9, 0, 37).Should().Be("|  17   18   19  !20   21   22   23 |");
        }

        [Fact]
        public void AddNote_marks_a_date_with_the_note_glyph_and_enables_the_toggle_hint()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).AddNote(new DateTime(2024, 3, 10), "Meeting");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(8, 0, 37).Should().Be("| *10   11   12   13   14  <15>  16 |");
            _ = vt.Find(PromptPlusResources.TooltipToggleNotes).Should().NotBeNull();
        }

        [Fact]
        public void Toggle_notes_hint_is_hidden_when_no_date_has_a_note()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipToggleNotes).Should().BeNull();
        }

        [Fact]
        public void PredicateSelected_rejecting_the_initial_date_shows_the_generic_invalid_date_error()
        {
            // The predicate is also evaluated at InitControl for the initial date, so a rejected
            // initial date leaves _selectedDate null from the very first render - Enter never
            // reaches ValidateSelection()/PredicateSelectInvalid in this case.
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).PredicateSelected(_ => false);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.InvalidDateSelect).Should().NotBeNull();
            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().BeNull();
        }

        [Fact]
        public void PredicateSelected_rejecting_a_date_reached_by_navigation_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).PredicateSelected(d => d != new DateTime(2024, 3, 16));
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void PredicateSelected_with_a_custom_message_shows_it_instead_of_the_default()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt)
                .PredicateSelected(d => d == new DateTime(2024, 3, 16) ? (false, "Custom rejection") : (true, null));
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("Custom rejection").Should().NotBeNull();
        }

        [Fact]
        public void PredicateSelectedAsync_rejecting_a_date_reached_by_navigation_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt)
                .PredicateSelectedAsync(d => System.Threading.Tasks.Task.FromResult(d != new DateTime(2024, 3, 16)));
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void ChangeDescription_reflects_the_currently_highlighted_date()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).ChangeDescription(d => $"desc:{d:yyyy-MM-dd}");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("desc:2024-03-15").Should().NotBeNull();
        }

        [Fact]
        public void ChangeDescriptionAsync_reflects_the_currently_highlighted_date()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).ChangeDescriptionAsync(async d =>
            {
                await System.Threading.Tasks.Task.Delay(1);
                return $"async:{d:yyyy-MM-dd}";
            });

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("async:2024-03-15").Should().NotBeNull();
        }

        [Fact]
        public void Escape_always_aborts_with_a_null_result()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
            _ = result.Content.Should().BeNull();
            _ = vt.Find(PromptPlusResources.CanceledKey).Should().NotBeNull();
        }

        [Fact]
        public void EnableHistory_alone_does_not_autoreload_without_an_explicit_Default_call()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).EnableHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            // Deliberately NOT calling Default(...) on the second run - unlike TreeSelect/TreeMultiSelect,
            // Calendar follows the Select/Table family convention: _useDefaultHistory starts
            // false and is only flipped by Default(value, useDefaultHistory: true).
            var control2 = new PromptPlusControls(vt2, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .EnableHistory(HistoryFile);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control2.Run(cts2.Token);

            _ = vt2.TextAt(0, 0, 8).Should().Be("Choose: ");
            _ = vt2.Find(DateTime.Today.ToString("d", System.Globalization.CultureInfo.InvariantCulture)).Should().BeNull();
        }

        [Fact]
        public void EnableHistory_reloads_when_Default_is_called_with_useDefaultHistory_true()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt).EnableHistory(HistoryFile);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            _ = control.Run(cts.Token);

            var vt2 = MakeTerminal();
            // useDefaultHistory defaults to true - the explicit Default(...) value (Jan 1 2020)
            // is overridden by the persisted history value from the first run (Mar 15 2024).
            var control2 = new PromptPlusControls(vt2, new PromptConfig()).Calendar("Choose")
                .Culture("en-US")
                .Default(new DateTime(2020, 1, 1))
                .EnableHistory(HistoryFile);
            _ = vt2.Keys.Enqueue(ConsoleKey.Enter);
            using var cts2 = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result2 = control2.Run(cts2.Token);

            _ = result2.Content.Should().Be(new DateTime(2024, 3, 15));
        }

        [Fact]
        public void F1_cycles_the_tooltip_to_the_next_hint()
        {
            var vt = MakeTerminal();
            var control = MakeCalendar(vt);
            _ = vt.Keys.Enqueue(ConsoleKey.F1);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.MoveDays).Should().NotBeNull();
        }
    }
}
