// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

using PromptPlusControls.Resources;

namespace PromptPlusObjects
{
    public struct HotKey : IEquatable<ConsoleKeyInfo>
    {
        public HotKey(ConsoleKey key, bool alt, bool ctrl, bool shift)
        {
            Key = key;
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
            if (!IsValidHotKey())
            {
                throw new ArgumentException(Exceptions.Ex_InvalidHotKey);
            }
        }

        public ConsoleKeyInfo KeyInfo => new(char.MinValue, Key, Shift, Alt, Ctrl);
        public ConsoleKey Key { get; private set; }
        public bool Alt { get; private set; }
        public bool Ctrl { get; private set; }
        public bool Shift { get; private set; }

        public override string ToString()
        {
            var modifiers = string.Empty;
            if (KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                modifiers += "Crtl+";
            }
            if (KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                modifiers += "Shift+";
            }
            if (KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                modifiers += "Alt+";
            }
            if (Key == ConsoleKey.Escape)
            {
                return $"{modifiers}Esc";
            }
            return $"{modifiers}{Key}";
        }

        public bool Equals(ConsoleKeyInfo other) => Key == other.Key && KeyInfo.Modifiers == other.Modifiers;

        public bool IsValidHotKey()
        {
            var localinfo = KeyInfo;
            if (localinfo.Key == ConsoleKey.Enter)
            {
                return false;
            }
            if (localinfo.Key == ConsoleKey.Delete)
            {
                return false;
            }

            if (localinfo.Key == ConsoleKey.LeftArrow)
            {
                return false;
            }
            if (localinfo.Key == ConsoleKey.RightArrow)
            {
                return false;
            }
            if (localinfo.Key == ConsoleKey.UpArrow)
            {
                return false;
            }
            if (localinfo.Key == ConsoleKey.DownArrow)
            {
                return false;
            }
            if (localinfo.Key == ConsoleKey.PageUp)
            {
                return false;
            }
            if (localinfo.Key == ConsoleKey.PageDown)
            {
                return false;
            }
            if (localinfo.Key == ConsoleKey.Backspace)
            {
                return false;
            }
            return true;
        }
    }
}
