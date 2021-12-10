using System;

namespace PPlus.Objects
{
    public static class ReadLineBufferExtension
    {
        public static bool IsPressSpecialKey(this ConsoleKeyInfo keyinfo, ConsoleKey key, ConsoleModifiers modifier)
        {
            if (keyinfo.Key == key)
            {
                if (keyinfo.Modifiers == modifier)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPressEnterKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Enter || (keyinfo.Key == ConsoleKey.J && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressTabKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Tab;
        }

        public static bool IsPressEndKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.End || (keyinfo.Key == ConsoleKey.E && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressHomeKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Home || (keyinfo.Key == ConsoleKey.A && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressBackspaceKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Backspace || (keyinfo.Key == ConsoleKey.H && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressDeleteKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Delete || (keyinfo.Key == ConsoleKey.D && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressLeftArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.LeftArrow || (keyinfo.Key == ConsoleKey.B && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressRightArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.RightArrow || (keyinfo.Key == ConsoleKey.F && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressUpArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.UpArrow || (keyinfo.Key == ConsoleKey.P && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressDownArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.DownArrow || (keyinfo.Key == ConsoleKey.N && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressPageUpKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.PageUp || (keyinfo.Key == ConsoleKey.P && keyinfo.Modifiers == ConsoleModifiers.Alt);
        }

        public static bool IsPressPageDownKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.PageDown || (keyinfo.Key == ConsoleKey.N && keyinfo.Modifiers == ConsoleModifiers.Alt);
        }

        public static bool IsPressEscKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Escape;
        }

        public static bool IsPressCtrlCKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.C && keyinfo.Modifiers == ConsoleModifiers.Control;
        }
    }
}
