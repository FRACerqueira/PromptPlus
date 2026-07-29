using FluentAssertions;
using PromptPlusLibrary;
using PromptPlusLibrary.Controls.MaskEdit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // MaskEditBuffer<T>/MaskElement (Controls/MaskEdit/{MaskEditBuffer,MaskElement}.cs) — pure
    // unit-level. State machine backing the whole MaskEdit*Control family (string/number/currency/
    // date-time). Fixtures are built via the REAL private NormalizeStringMask/NormalizeNumberMask
    // factory methods on MaskEditControl<T> (invoked via reflection, since they're private, not
    // internal — IVT does not relax `private`), so the element layout matches production exactly
    // instead of a hand-built approximation. Exact expected values for the numeric shift-left entry,
    // backspace-at-decimal and sign handling were confirmed with a throwaway probe.
    public class MaskEditBufferTests
    {
        private static Dictionary<int, MaskElement> NormalizeStringMask(string mask, char promptchar)
        {
            var method = typeof(MaskEditControl<string>).GetMethod("NormalizeStringMask", BindingFlags.NonPublic | BindingFlags.Static)!;
            return (Dictionary<int, MaskElement>)method.Invoke(null, [mask, promptchar])!;
        }

        private static Dictionary<int, MaskElement> NormalizeNumberMask<T>(string mask, char promptchar, CultureInfo culture)
        {
            var method = typeof(MaskEditControl<T>).GetMethod("NormalizeNumberMask", BindingFlags.NonPublic | BindingFlags.Static)!;
            return (Dictionary<int, MaskElement>)method.Invoke(null, [mask, promptchar, culture])!;
        }

        private static ConsoleKeyInfo Ch(char c) => new(c, ConsoleKey.A, false, false, false);
        private static ConsoleKeyInfo Ctrl(ConsoleKey k) => new((char)0, k, false, false, true);
        private static ConsoleKeyInfo Plain(ConsoleKey k) => new((char)0, k, false, false, false);

        private static MaskEditBuffer<string> StringBuffer(string mask = "AAA", char prompt = '_')
            => new(NormalizeStringMask(mask, prompt), prompt, InputBehavior.EditSkipToInput);

        private static MaskEditBuffer<decimal> DecimalBuffer(string mask = "99.99", char prompt = '_')
            => new(NormalizeNumberMask<decimal>(mask, prompt, CultureInfo.InvariantCulture), prompt, InputBehavior.EditSkipToInput);

        // ---- string mask ----

        [Fact]
        public void String_mask_starts_at_the_first_input_position()
        {
            var b = StringBuffer();
            _ = b.CursorPosition.Should().Be(0);
            _ = b.MaskOut.Should().Be("___");
        }

        [Fact]
        public void String_mask_accepts_a_valid_letter_and_advances_the_cursor()
        {
            var b = StringBuffer();

            bool accepted = b.TryAcceptedReadlineConsoleKey(Ch('a'), false);

            _ = accepted.Should().BeTrue();
            _ = b.MaskOut.Should().Be("a__");
            _ = b.CursorPosition.Should().Be(1);
        }

        [Fact]
        public void String_mask_rejects_a_character_outside_the_valid_set()
        {
            var b = StringBuffer(); // "AAA" only accepts letters

            bool accepted = b.TryAcceptedReadlineConsoleKey(Ch('5'), false);

            _ = accepted.Should().BeFalse();
            _ = b.MaskOut.Should().Be("___");
        }

        [Fact]
        public void WithoutMask_is_empty_while_input_is_still_pending_and_matches_MaskOut_once_full()
        {
            var b = StringBuffer();
            _ = b.WithoutMask.Should().BeEmpty();

            _ = b.TryAcceptedReadlineConsoleKey(Ch('a'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Ch('b'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Ch('c'), false);

            _ = b.HasInputPending.Should().BeFalse();
            _ = b.WithoutMask.Should().Be("abc");
        }

        [Fact]
        public void Backspace_clears_the_previous_character_without_shifting_the_rest()
        {
            var b = StringBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('a'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Ch('b'), false);

            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.Backspace), false);

            _ = b.MaskOut.Should().Be("a__");
            _ = b.CursorPosition.Should().Be(1);
        }

        [Fact]
        public void Delete_clears_the_character_under_the_cursor_and_advances()
        {
            var b = StringBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('a'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Ch('b'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.Home), false);

            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.Delete), false);

            _ = b.MaskOut.Should().Be("_b_");
        }

        [Fact]
        public void Home_and_End_move_to_the_first_and_last_input_positions()
        {
            var b = StringBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.End), false);
            _ = b.CursorPosition.Should().Be(2);

            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.Home), false);
            _ = b.CursorPosition.Should().Be(0);
        }

        [Fact]
        public void Clear_resets_every_input_position_and_returns_to_the_start()
        {
            var b = StringBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('a'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Ch('b'), false);

            _ = b.TryAcceptedReadlineConsoleKey(Ctrl(ConsoleKey.L), true); // Ctrl+L requires enabledemacks=true

            _ = b.MaskOut.Should().Be("___");
            _ = b.CursorPosition.Should().Be(0);
        }

        [Fact]
        public void Enter_and_Escape_are_always_rejected_without_changing_the_buffer()
        {
            var b = StringBuffer();

            _ = b.TryAcceptedReadlineConsoleKey(new ConsoleKeyInfo('\r', ConsoleKey.Enter, false, false, false), false).Should().BeFalse();
            _ = b.TryAcceptedReadlineConsoleKey(new ConsoleKeyInfo((char)27, ConsoleKey.Escape, false, false, false), false).Should().BeFalse();
            _ = b.MaskOut.Should().Be("___");
        }

        [Fact]
        public void Emacs_shortcuts_are_ignored_when_not_enabled_but_physical_keys_still_work()
        {
            var b = StringBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('a'), false);

            _ = b.TryAcceptedReadlineConsoleKey(Ctrl(ConsoleKey.H), false).Should().BeFalse(); // Ctrl+H needs enabledemacks=true
            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.Backspace), false).Should().BeTrue(); // physical key always works
        }

        [Fact]
        public void Tab_delimiter_jump_is_a_no_op_for_string_masks()
        {
            var b = StringBuffer();

            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.Tab), false).Should().BeFalse();
        }

        // ---- numeric (decimal) mask ----

        [Fact]
        public void Numeric_mask_starts_with_the_cursor_parked_at_the_decimal_separator()
        {
            var b = DecimalBuffer();
            _ = b.CursorPosition.Should().Be(2);
            _ = b.MaskOut.Should().Be("__.__");
        }

        [Fact]
        public void Typing_integer_digits_shifts_them_in_from_the_right_of_the_decimal_point()
        {
            var b = DecimalBuffer();

            _ = b.TryAcceptedReadlineConsoleKey(Ch('5'), false);
            _ = b.MaskOut.Should().Be("_5.__");

            _ = b.TryAcceptedReadlineConsoleKey(Ch('3'), false);
            _ = b.MaskOut.Should().Be("53.__");
        }

        [Fact]
        public void Extra_integer_digits_are_rejected_once_the_integer_part_is_full()
        {
            var b = DecimalBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('5'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Ch('3'), false);

            bool accepted = b.TryAcceptedReadlineConsoleKey(Ch('7'), false);

            _ = accepted.Should().BeFalse();
            _ = b.MaskOut.Should().Be("53.__");
        }

        [Fact]
        public void Backspace_at_the_decimal_point_removes_the_most_recently_shifted_digit()
        {
            var b = DecimalBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('5'), false);
            _ = b.TryAcceptedReadlineConsoleKey(Ch('3'), false); // "53.__"

            _ = b.TryAcceptedReadlineConsoleKey(Plain(ConsoleKey.Backspace), false);

            _ = b.MaskOut.Should().Be("_5.__");
        }

        [Fact]
        public void Typing_the_decimal_separator_moves_the_cursor_into_the_decimal_digits()
        {
            var b = DecimalBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('5'), false);

            bool accepted = b.TryAcceptedReadlineConsoleKey(new ConsoleKeyInfo('.', ConsoleKey.OemPeriod, false, false, false), false);

            _ = accepted.Should().BeTrue();
            _ = b.CursorPosition.Should().Be(3);
        }

        [Fact]
        public void Decimal_digits_fill_left_to_right_after_the_separator_normally_not_shifted()
        {
            var b = DecimalBuffer();
            _ = b.TryAcceptedReadlineConsoleKey(Ch('5'), false);
            _ = b.TryAcceptedReadlineConsoleKey(new ConsoleKeyInfo('.', ConsoleKey.OemPeriod, false, false, false), false);

            _ = b.TryAcceptedReadlineConsoleKey(Ch('7'), false);
            _ = b.MaskOut.Should().Be("_5.7_");

            _ = b.TryAcceptedReadlineConsoleKey(Ch('9'), false);
            _ = b.MaskOut.Should().Be("_5.79");
            _ = b.WithoutMask.Should().Be("5.79");
        }

        [Fact]
        public void Signed_mask_defaults_to_a_plus_sign_and_accepts_minus_to_flip_it()
        {
            var elements = NormalizeNumberMask<decimal>("*99.99", '_', CultureInfo.InvariantCulture);
            var b = new MaskEditBuffer<decimal>(elements, '_', InputBehavior.EditSkipToInput);
            _ = b.IsPositive.Should().BeTrue();

            bool accepted = b.TryAcceptedReadlineConsoleKey(new ConsoleKeyInfo('-', ConsoleKey.OemMinus, false, false, false), false);

            _ = accepted.Should().BeTrue();
            _ = b.IsNegative.Should().BeTrue();
        }

        // ---- construction guard (regression) ----

        // Regression for a real (latent) bug found while writing this suite: every numeric code path
        // (ToStart, TryAcceptedReadlineConsoleKey, Backspace, Delete, ...) indexes
        // _charElements[_decimalposition] with no bounds check. A numeric mask lacking a decimal
        // separator element left _decimalposition at -1, which left CursorPosition at -1 after
        // construction (no error yet) and threw KeyNotFoundException on the very first keystroke.
        // Unreachable via the public API today — MaskEditControl<T>.SetNumberFormat always appends a
        // '.' even for pure integers (decimalpart=0) — but fixed with a constructor guard so this
        // fails fast and clearly instead of relying on that invariant holding forever.
        [Fact]
        public void Numeric_mask_without_a_decimal_separator_throws_at_construction_instead_of_on_first_keystroke()
        {
            var elements = NormalizeStringMask("999", '_'); // wrong factory on purpose: no DecimalSeparator element
            // Re-tag as InputMask-only dict with no decimal separator, mirroring a hypothetical
            // malformed numeric mask (NormalizeStringMask happens to produce a compatible shape here:
            // three InputMask elements, no DecimalSeparator).

            Action act = () => new MaskEditBuffer<decimal>(elements, '_', InputBehavior.EditSkipToInput);

            _ = act.Should().Throw<ArgumentException>();
        }
    }
}
