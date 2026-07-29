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
    // MaskEditControl<int|long> (`IMaskEditNumberControl`).
    // Globals shared with every MaskEdit type (Enter/Escape/tooltip/Default/predicate) are
    // exercised once in MaskEditStringControlTests.cs; this file focuses on what's specific to
    // integer masks: NumberFormat-driven mask construction, the shift-left digit entry model,
    // sign/group-separator placement, and culture-driven separator characters.
    //
    // Culture gotcha confirmed by probe: `new PromptConfig()` defaults `DefaultCulture` to
    // `CultureInfo.CurrentCulture` (the OS/test-runner locale), NOT invariant — so every test that
    // asserts on rendered separators/signs pins `.Culture("en-US")` explicitly, otherwise the
    // decimal/group separator characters (and therefore exact rendered strings) depend on
    // whatever machine runs the suite.
    public class MaskEditNumberControlTests
    {
        private static VirtualTerminal MakeTerminal() => VirtualTerminal.Create(o => { o.SupportsUnicode = false; });

        [Fact]
        public void Initial_render_shows_group_separators_and_the_type_hint()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: __,___.");
        }

        [Fact]
        public void NumberFormat_without_separator_group_omits_the_comma()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5, withseparatorgroup: false);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 14).Should().Be("Choose: _____.");
        }

        [Fact]
        public void Typing_digits_fills_from_the_right_shift_left_and_Enter_confirms()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5);
            _ = vt.Keys.Type("123").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: __,123.");
            _ = result.Content.Should().Be(123);
        }

        [Fact]
        public void Typing_past_a_full_integer_part_rejects_further_digits()
        {
            // Shiftleft only has somewhere to shift INTO when at least one digit position is
            // still empty (MaskEditBuffer.Shiftleft returns false outright otherwise) - once the
            // 3-digit slot is full from "123", a 4th digit is simply rejected, not shifted in
            // to drop the oldest one.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(3, withseparatorgroup: false);
            _ = vt.Keys.Type("1234").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(123);
        }

        [Fact]
        public void Withsignal_places_the_sign_at_the_front_with_a_trailing_space()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(3, withsignal: true, withseparatorgroup: false);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 14).Should().Be("Choose: + ___.");
        }

        [Fact]
        public void Bugfix_typing_a_negative_sign_is_reflected_in_the_confirmed_value()
        {
            // Regression: MaskEditBuffer.GetWithoutMask() for integers never looked at the
            // SignSymbol element, so a visibly negative input ("- __5.") was silently returned
            // as +5. Fixed to mirror the decimal/double branch's IsNegative/IsPositive handling.
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(3, withsignal: true, withseparatorgroup: false);
            _ = vt.Keys.Type("-5").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(-5);
        }

        [Fact]
        public void Bugfix_explicit_positive_sign_still_confirms_as_positive()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskLong("Choose")
                .Culture("en-US")
                .NumberFormat(3, withsignal: true, withseparatorgroup: false);
            _ = vt.Keys.Type("+5").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(5L);
        }

        [Fact]
        public void Bugfix_negative_long_is_also_reflected_in_the_confirmed_value()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskLong("Choose")
                .Culture("en-US")
                .NumberFormat(3, withsignal: true, withseparatorgroup: false);
            _ = vt.Keys.Type("-5").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(-5L);
        }

        [Fact]
        public void NumberFormat_too_many_digits_for_int_throws_immediately_not_lazily_on_Run()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose");

            Action act = () => control.NumberFormat(11);

            _ = act.Should().Throw<FormatException>();
        }

        [Fact]
        public void NumberFormat_too_many_digits_for_long_throws_immediately()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskLong("Choose");

            Action act = () => control.NumberFormat(20);

            _ = act.Should().Throw<FormatException>();
        }

        [Fact]
        public void NumberFormat_zero_integer_part_throws()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose");

            Action act = () => control.NumberFormat(0);

            _ = act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Backspace_at_the_decimal_position_removes_the_last_typed_digit()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(3, withseparatorgroup: false);
            _ = vt.Keys.Type("12").Enqueue(ConsoleKey.Backspace);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 12).Should().Be("Choose: __1.");
        }

        [Fact]
        public void Tab_is_inert_for_number_masks()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.TooltipJumpdelimiter).Should().BeNull();
        }

        [Fact]
        public void Default_prefills_the_typed_value_on_first_render()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5)
                .Default(42);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.TextAt(0, 0, 15).Should().Be("Choose: __,_42.");
        }

        [Fact]
        public void DefaultIfEmpty_is_returned_on_Enter_without_typing_anything()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5)
                .DefaultIfEmpty(7);
            _ = vt.Keys.Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.Content.Should().Be(7);
        }

        [Fact]
        public void PredicateSelected_rejecting_the_value_shows_the_default_error()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5)
                .PredicateSelected(_ => false);
            _ = vt.Keys.Type("123").Enqueue(ConsoleKey.Enter);

            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            _ = control.Run(cts.Token);

            _ = vt.Find(PromptPlusResources.PredicateSelectInvalid).Should().NotBeNull();
        }

        [Fact]
        public void Escape_aborts_with_a_default_value_result()
        {
            var vt = MakeTerminal();
            var control = new PromptPlusControls(vt, new PromptConfig()).MaskInteger("Choose")
                .Culture("en-US")
                .NumberFormat(5);
            _ = vt.Keys.Type("123").Enqueue(ConsoleKey.Escape);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            var result = control.Run(cts.Token);

            _ = result.IsAborted.Should().BeTrue();
        }
    }
}
