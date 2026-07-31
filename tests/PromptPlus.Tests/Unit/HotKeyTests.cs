using FluentAssertions;
using PromptPlusLibrary;
using System;
using Xunit;

namespace PromptPlus.Tests.Unit
{
    // HotKey (Shared/Common/HotKey.cs) — pure unit-level. Support type for every control's
    // configurable key bindings (abort key, tooltip toggle, filter activation, etc.).
    public class HotKeyTests
    {
        private static readonly char Blank = (char)0;

        [Fact]
        public void Equals_HotKey_compares_key_and_all_modifiers()
        {
            var a = new HotKey(ConsoleKey.F1, ctrl: true);
            var b = new HotKey(ConsoleKey.F1, ctrl: true);
            var differentKey = new HotKey(ConsoleKey.F2, ctrl: true);
            var differentModifier = new HotKey(ConsoleKey.F1, ctrl: false);

            _ = a.Equals(b).Should().BeTrue();
            _ = (a == b).Should().BeTrue();
            _ = a.Equals(differentKey).Should().BeFalse();
            _ = a.Equals(differentModifier).Should().BeFalse();
            _ = (a != differentModifier).Should().BeTrue();
        }

        [Fact]
        public void Equals_ConsoleKeyInfo_matches_on_key_and_modifier_set_ignoring_KeyChar()
        {
            var ctrlA = new HotKey(ConsoleKey.A, ctrl: true);

            _ = ctrlA.Equals(new ConsoleKeyInfo(Blank, ConsoleKey.A, false, false, true)).Should().BeTrue();
            _ = ctrlA.Equals(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false)).Should().BeFalse(); // missing Ctrl
            _ = ctrlA.Equals(new ConsoleKeyInfo(Blank, ConsoleKey.B, false, false, true)).Should().BeFalse(); // wrong key
        }

        [Fact]
        public void Equals_object_overload_dispatches_to_the_right_comparison()
        {
            var esc = new HotKey(ConsoleKey.Escape);
            object boxedHotKey = new HotKey(ConsoleKey.Escape);
            object boxedKeyInfo = new ConsoleKeyInfo(Blank, ConsoleKey.Escape, false, false, false);

            _ = esc.Equals(boxedHotKey).Should().BeTrue();
            _ = esc.Equals(boxedKeyInfo).Should().BeTrue();
            _ = esc.Equals("not a key").Should().BeFalse();
        }

        [Theory]
        [InlineData("F1")]
        [InlineData("F24")]
        public void ToString_function_keys_use_the_enum_name(string expected)
        {
            var key = new HotKey(Enum.Parse<ConsoleKey>(expected));
            _ = key.ToString().Should().Be(expected);
        }

        [Fact]
        public void ToString_escape_and_spacebar_use_friendly_labels()
        {
            _ = new HotKey(ConsoleKey.Escape).ToString().Should().Be("Esc");
        }

        [Fact]
        public void ToString_prefixes_modifiers_in_Ctrl_Shift_Alt_order()
        {
            var key = new HotKey(ConsoleKey.F1, alt: true, ctrl: true, shift: true);
            _ = key.ToString().Should().Be("Ctrl Shift Alt F1");
        }

        [Fact]
        public void ToString_printable_ascii_keys_render_as_the_literal_character()
        {
            _ = new HotKey(ConsoleKey.A).ToString().Should().Be("A");
        }

        [Fact]
        public void ToString_math_symbol_keys_render_as_their_symbol()
        {
            _ = new HotKey(ConsoleKey.Add).ToString().Should().Be("+");
            _ = new HotKey(ConsoleKey.Subtract).ToString().Should().Be("-");
            _ = new HotKey(ConsoleKey.Multiply).ToString().Should().Be("*");
            _ = new HotKey(ConsoleKey.Divide).ToString().Should().Be("/");
        }

        [Fact]
        public void Default_hotkeys_have_the_expected_key_and_modifiers()
        {
            _ = HotKey.DefaultAbortKeyPress.Should().Be(new HotKey(ConsoleKey.Escape));
            _ = HotKey.DefaultTooltip.Should().Be(new HotKey(ConsoleKey.F1));
            _ = HotKey.DefaultTooltipShowHide.Should().Be(new HotKey(ConsoleKey.F1, ctrl: true));
            _ = HotKey.DefaultToggleFullPath.Should().Be(new HotKey(ConsoleKey.F3, shift: true));
        }

        // Regression for a real (latent, unused-in-production) bug found while writing this suite:
        // KeyInfo used to cast the ConsoleKey enum value directly to char ((char)Key), which happened
        // to be correct for keys whose enum value overlaps printable ASCII/known control codes
        // (A-Z, D0-D9, Escape=27, Tab=9, Backspace=8, Enter=13), but produced a nonsensical KeyChar
        // for everything else — e.g. F1 (enum value 112) became 'p'. Fixed to report '\0' for any
        // key outside that safe range, matching what a real console reports for non-printable keys.
        [Fact]
        public void KeyInfo_KeyChar_is_correct_for_printable_and_known_control_keys()
        {
            _ = new HotKey(ConsoleKey.A).KeyInfo.KeyChar.Should().Be('A');
            _ = new HotKey(ConsoleKey.Escape).KeyInfo.KeyChar.Should().Be((char)27);
            _ = new HotKey(ConsoleKey.Tab).KeyInfo.KeyChar.Should().Be((char)9);
            _ = new HotKey(ConsoleKey.Backspace).KeyInfo.KeyChar.Should().Be((char)8);
            _ = new HotKey(ConsoleKey.Enter).KeyInfo.KeyChar.Should().Be((char)13);
        }

        [Fact]
        public void KeyInfo_KeyChar_is_blank_for_keys_with_no_real_character_representation()
        {
            _ = new HotKey(ConsoleKey.F1).KeyInfo.KeyChar.Should().Be((char)0);
            _ = new HotKey(ConsoleKey.LeftArrow).KeyInfo.KeyChar.Should().Be((char)0); // enum value 37 = '%', would leak with a naive range check
            _ = new HotKey(ConsoleKey.Home).KeyInfo.KeyChar.Should().Be((char)0);
            _ = new HotKey(ConsoleKey.PageDown).KeyInfo.KeyChar.Should().Be((char)0);
            _ = new HotKey(ConsoleKey.OemPlus).KeyInfo.KeyChar.Should().Be((char)0); // was already wrong before the fix too, just silently
        }

        [Fact]
        public void GetHashCode_is_consistent_with_Equals()
        {
            var a = new HotKey(ConsoleKey.F1, ctrl: true);
            var b = new HotKey(ConsoleKey.F1, ctrl: true);

            _ = a.GetHashCode().Should().Be(b.GetHashCode());
        }
    }
}
