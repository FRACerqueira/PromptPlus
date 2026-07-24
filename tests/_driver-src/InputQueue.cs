// ***************************************************************************************
// MIT LICENCE
// Headless test driver shared by ConsolePlus.Tests and PromptPlus.Tests (linked source, see tests/TEST-PLAN.md)
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace ConsolePlusLibrary.Testing
{
    /// <summary>
    /// Own key queue that feeds <see cref="VirtualTerminal"/>'s <c>KeyAvailable</c>/<c>ReadKey</c>/<c>ReadKeyAsync</c>,
    /// since <c>Console.KeyAvailable</c>/<c>Console.ReadKey</c> cannot be redirected via <c>SetIn</c>.
    /// </summary>
    public sealed class InputQueue
    {
        private readonly Queue<ConsoleKeyInfo> _keys = new();

        public bool HasNext => _keys.Count > 0;

        public InputQueue Enqueue(ConsoleKeyInfo key)
        {
            _keys.Enqueue(key);
            return this;
        }

        public InputQueue Enqueue(ConsoleKey key, bool shift = false, bool alt = false, bool ctrl = false)
            => Enqueue(new ConsoleKeyInfo(DefaultCharFor(key), key, shift, alt, ctrl));

        /// <summary>
        /// Char a real console reports alongside this key, for keys whose confirmation logic checks
        /// <c>KeyChar</c> instead of <c>Key</c>. On non-Windows, <c>IsPressEnterKey</c> compares
        /// <c>KeyChar</c> (13/10), not <c>Key</c> — see ConsolePlus/src/Shared/ConsoleKeyInfoExtensions.cs:42-48 —
        /// so <c>Enqueue(ConsoleKey.Enter)</c> would silently fail to confirm anything outside Windows
        /// without this mapping. Navigation keys (arrows, Home/End, Escape) are matched by <c>Key</c>
        /// only, so they correctly keep KeyChar '\0'.
        /// </summary>
        private static char DefaultCharFor(ConsoleKey key) => key switch
        {
            ConsoleKey.Enter => '\r',
            ConsoleKey.Tab => '\t',
            ConsoleKey.Backspace => '\b',
            ConsoleKey.Escape => (char)27,
            _ => '\0',
        };

        public InputQueue Type(string text)
        {
            foreach (char ch in text)
            {
                _keys.Enqueue(new ConsoleKeyInfo(ch, CharToKey(ch), shift: char.IsUpper(ch), alt: false, control: false));
            }
            return this;
        }

        public ConsoleKeyInfo Next() => _keys.Dequeue();

        private static ConsoleKey CharToKey(char ch)
        {
            if (char.IsAsciiLetter(ch))
            {
                return Enum.Parse<ConsoleKey>(char.ToUpperInvariant(ch).ToString());
            }
            if (char.IsAsciiDigit(ch))
            {
                return (ConsoleKey)('0' + (ch - '0'));
            }
            return ch switch
            {
                ' ' => ConsoleKey.Spacebar,
                '-' => ConsoleKey.OemMinus,
                '.' => ConsoleKey.OemPeriod,
                ',' => ConsoleKey.OemComma,
                _ => ConsoleKey.Oem1, // extend as pilot tests need specific punctuation
            };
        }
    }
}
