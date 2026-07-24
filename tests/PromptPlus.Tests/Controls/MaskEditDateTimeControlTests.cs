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
    // Fase 2, Grupo 3 (FASE2-CONTROLS-PLAN.md) — MaskEditControl<DateTime|DateOnly|TimeOnly>
    // (`IMaskEditDateTimeControl`). Globals shared with every MaskEdit type are exercised once in
    // MaskEditStringControlTests.cs. This file focuses on what's specific to date/time masks:
    // the 5 factories (MaskDate/MaskDateOnly/MaskTime/MaskTimeOnly/MaskDateTime), culture-driven
    // field ORDER (day/month/year re-templated from `ShortDatePattern`, always joined with a
    // literal '/' regardless of the culture's own date separator), `FixedValues`, `WeekTypeMode`,
    // and Tab/Shift+Tab jumping between fields (unlike Number/String masks, where Tab is inert).
    public class MaskEditDateTimeControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        [Fact]
        public void MaskDate_en_US_orders_fields_month_day_year()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 18).Should().Be("Choose: __/__/____");
        }

        [Fact]
        public void MaskDate_typing_confirms_the_expected_DateTime_value()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US");
            _ = vt.Keys.Type("12312024").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeFalse();
            _ = result.Content.Should().Be(new DateTime(2024, 12, 31));
        }

        [Fact]
        public void MaskDateOnly_returns_a_DateOnly_value()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDateOnly("Choose")
                .Culture("en-US");
            _ = vt.Keys.Type("12312024").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateOnly(2024, 12, 31));
        }

        [Fact]
        public void MaskTime_confirms_hour_minute_second()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskTime("Choose")
                .Culture("en-US");
            _ = vt.Keys.Type("131415").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            DateTime content = (DateTime)(object)result.Content!;
            _ = content.Hour.Should().Be(13);
            _ = content.Minute.Should().Be(14);
            _ = content.Second.Should().Be(15);
        }

        [Fact]
        public void MaskTimeOnly_returns_a_TimeOnly_value()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskTimeOnly("Choose")
                .Culture("en-US");
            _ = vt.Keys.Type("131415").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new TimeOnly(13, 14, 15));
        }

        [Fact]
        public void MaskDateTime_combines_date_and_time_fields()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDateTime("Choose")
                .Culture("en-US");
            _ = vt.Keys.Type("12312024131415").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateTime(2024, 12, 31, 13, 14, 15));
        }

        [Fact]
        public void Tab_jumps_from_one_field_straight_to_the_next()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US");
            // "12" fills Month; Tab jumps past Day (skipping it) straight into Year.
            _ = vt.Keys.Type("12").Enqueue(ConsoleKey.Tab).Type("31");

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 18).Should().Be("Choose: 12/__/31__");
        }

        [Fact]
        public void FixedValues_locks_a_date_part_to_a_constant_that_cannot_be_edited()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US")
                .FixedValues(DateTimePart.Day, 15);
            _ = vt.Keys.Type("012024").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateTime(2024, 1, 15));
        }

        [Fact]
        public void FixedValues_on_a_part_not_present_in_the_mask_throws()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose");

            Action act = () => control.FixedValues(DateTimePart.Hour, 5);

            _ = act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void FixedValues_out_of_range_throws()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose");

            Action act = () => control.FixedValues(DateTimePart.Day, 32);

            _ = act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void WeekTypeMode_shows_the_short_weekday_next_to_a_complete_date()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US")
                .WeekTypeMode()
                .Default(new DateTime(2024, 3, 15));

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("(Fri)").Should().NotBeNull();
        }

        [Fact]
        public void WeekTypeMode_is_hidden_by_default()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15));

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find("(Fri)").Should().BeNull();
        }

        [Fact]
        public void Default_prefills_the_date_and_autoconfirms_on_Enter()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US")
                .Default(new DateTime(2024, 3, 15));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 18).Should().Be("Choose: 03/15/2024");
            _ = result.Content.Should().Be(new DateTime(2024, 3, 15));
        }

        [Fact]
        public void DefaultIfEmpty_is_returned_on_Enter_without_typing_anything()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US")
                .DefaultIfEmpty(new DateTime(2000, 1, 1));
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(new DateTime(2000, 1, 1));
        }

        [Fact]
        public void PredicateSelected_rejecting_the_value_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US")
                .PredicateSelected(_ => false);
            _ = vt.Keys.Type("12312024").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void An_impossible_date_shows_the_invalid_input_error_and_does_not_confirm()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US");
            // February 30th does not exist.
            _ = vt.Keys.Type("02302024").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.MaskEditInvalidInput).Should().NotBeNull();
        }

        [Fact]
        public void Enter_with_unfilled_positions_shows_the_pending_input_error()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US");
            _ = vt.Keys.Type("12").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.MaskeditInputPending).Should().NotBeNull();
        }

        [Fact]
        public void Escape_aborts_without_confirming()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDate("Choose")
                .Culture("en-US");
            _ = vt.Keys.Type("12").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
        }
    }
}
