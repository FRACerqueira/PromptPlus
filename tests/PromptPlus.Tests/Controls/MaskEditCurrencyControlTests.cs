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
    // Fase 2, Grupo 3 (FASE2-CONTROLS-PLAN.md) — MaskEditControl<decimal|double> (`IMaskEditCurrencyControl`).
    // Globals shared with every MaskEdit type are exercised once in MaskEditStringControlTests.cs.
    // This file focuses on what's specific to decimal/double masks: NumberFormat with a decimal
    // part, the currency symbol (`MaskDecimalCurrency`/`MaskDoubleCurrency` vs. plain
    // `MaskDecimal`/`MaskDouble`), and sign placement (currency puts the sign at the END with a
    // leading space, unlike plain Number masks which put it at the front).
    //
    // Non-obvious behavior confirmed by probe, wrong on the first pass of writing these tests:
    // typing digits WITHOUT navigating only ever shifts into the INTEGER part
    // (`MaskEditBuffer.Shiftleft` scans `_charElements` and `break`s the moment it hits the
    // `DecimalSeparator` element, so the decimal digit positions are never part of the shift
    // chain at all) — "5" on a `NumberFormat(3, 2)` mask yields the WHOLE number 5, not 0.5 or
    // 0.05. The cursor also never auto-advances past the decimal separator on its own (the
    // `CursorPosition == _decimalposition` branch in `TryAcceptedReadlineConsoleKey` returns
    // immediately after `Shiftleft`, without calling `GetNextPos()`). To fill the decimal part,
    // the user must explicitly move the cursor right (arrow key or Ctrl+F) past the separator
    // first, then type directly into the decimal digit positions (plain left-to-right fill,
    // like string/datetime masks) — this is the intended two-step workflow: type the whole part
    // (auto-shifting), move right, type the fractional part.
    //
    // Confirmed by probe: unlike MaskInteger/MaskLong (bug fixed this session — see
    // MaskEditNumberControlTests.cs), MaskEditBuffer.GetWithoutMask()'s decimal branch already
    // correctly applied IsNegative/IsPositive to the parsed value, so no regression test needed
    // here for sign propagation, only for placement/rendering.
    public class MaskEditCurrencyControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        [Fact]
        public void MaskDecimalCurrency_renders_the_currency_symbol_and_a_trailing_space()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimalCurrency("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 16).Should().Be("Choose: $ ___.__");
        }

        [Fact]
        public void MaskDecimal_plain_has_no_currency_symbol()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 14).Should().Be("Choose: ___.__");
        }

        [Fact]
        public void Withsignal_on_currency_places_the_sign_at_the_end_with_a_leading_space()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimalCurrency("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withsignal: true, withseparatorgroup: false);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 18).Should().Be("Choose: $ ___.__ +");
        }

        [Fact]
        public void Typing_digits_without_navigating_only_fills_the_integer_part()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false);
            _ = vt.Keys.Type("5").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(5m);
        }

        [Fact]
        public void Moving_right_past_the_decimal_separator_allows_typing_the_fractional_part()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false);
            _ = vt.Keys.Enqueue(ConsoleKey.RightArrow).Type("5").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(0.5m);
        }

        [Fact]
        public void Typing_the_whole_part_then_moving_right_then_the_fractional_part_combines_both()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false);
            _ = vt.Keys.Type("123").Enqueue(ConsoleKey.RightArrow).Type("45").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(123.45m);
        }

        [Fact]
        public void Negative_decimal_value_is_returned_correctly()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withsignal: true, withseparatorgroup: false);
            _ = vt.Keys.Type("-123").Enqueue(ConsoleKey.RightArrow).Type("45").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(-123.45m);
        }

        [Fact]
        public void MaskDoubleCurrency_returns_a_double_value()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDoubleCurrency("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false);
            _ = vt.Keys.Type("123").Enqueue(ConsoleKey.RightArrow).Type("45").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(123.45d);
        }

        [Fact]
        public void NumberFormat_too_many_integer_digits_for_decimal_throws_immediately()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose");

            Action act = () => control.NumberFormat(29, 2);

            _ = act.Should().Throw<FormatException>();
        }

        [Fact]
        public void NumberFormat_too_many_decimal_digits_for_double_throws_immediately()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDouble("Choose");

            Action act = () => control.NumberFormat(5, 16);

            _ = act.Should().Throw<FormatException>();
        }

        [Fact]
        public void NumberFormat_zero_integer_and_decimal_parts_throws()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose");

            Action act = () => control.NumberFormat(0, 0);

            _ = act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Default_prefills_the_value_on_first_render()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false)
                .Default(1.5m);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 14).Should().Be("Choose: __1.5_");
        }

        [Fact]
        public void DefaultIfEmpty_is_returned_on_Enter_without_typing_anything()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false)
                .DefaultIfEmpty(9.9m);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(9.9m);
        }

        [Fact]
        public void PredicateSelected_rejecting_the_value_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskDecimal("Choose")
                .Culture("en-US")
                .NumberFormat(3, 2, withseparatorgroup: false)
                .PredicateSelected(_ => false);
            _ = vt.Keys.Type("100").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }
    }
}
