// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using PPlus.Resources;

namespace PPlus.Objects
{
    public struct HotKey : IEquatable<ConsoleKeyInfo>
    {
        internal HotKey(ConsoleKey key, bool alt = false, bool ctrl = false, bool shift = false)
        {
            Key = key;
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
        }

        public HotKey(UserHotKey key, bool alt = false, bool ctrl = false, bool shift = false)
        {
            Key = Enum.Parse<ConsoleKey>(key.ToString(),true);
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
        }

        public ConsoleKeyInfo KeyInfo => new((char)Key, Key, Shift, Alt, Ctrl);
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

    }
}
