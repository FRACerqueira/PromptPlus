// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;
using PPlus.Drivers;

namespace PPlus.Controls.Objects
{
    internal static class ConsoleKeyInfoExtensions
    {

        public static bool IskeyPageNavagator<Tkey>(this ConsoleKeyInfo consoleKeyInfo, Paginator<Tkey> paginator)
        {
            if (consoleKeyInfo.IsPressPageUpKey() && paginator.PageCount > 1)
            {
                return paginator.PreviousPage(IndexOption.LastItemWhenHasPages);
            }
            else if (consoleKeyInfo.IsPressPageDownKey() && paginator.PageCount > 1)
            {
                return paginator.NextPage(IndexOption.FirstItemWhenHasPages);
            }
            else if (consoleKeyInfo.IsPressUpArrowKey() && paginator.Count > 0)
            {
                if (paginator.IsFistPageItem)
                {
                    return paginator.PreviousPage(IndexOption.LastItem);
                }
                return paginator.PreviousItem();
            }
            else if (consoleKeyInfo.IsPressDownArrowKey() && paginator.Count > 0)
            {
                if (paginator.IsLastPageItem)
                {
                    return paginator.NextPage(IndexOption.FirstItem);
                }
                return paginator.NextItem();
            }
            return false;
        }

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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return (keyinfo.Key == ConsoleKey.Enter) || (keyinfo.Key == ConsoleKey.J && keyinfo.Modifiers == ConsoleModifiers.Control);
            }
            return (keyinfo.KeyChar == 13 && keyinfo.Modifiers == 0) || (keyinfo.KeyChar == 10 && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.J && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressTabKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Tab && keyinfo.Modifiers == 0;
        }

        public static bool IsPressShiftTabKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Tab && keyinfo.Modifiers == ConsoleModifiers.Shift;
        }

        public static bool IsPressEndKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.End && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.E && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressHomeKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.Home && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.A && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressBackspaceKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.Backspace && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.H && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressDeleteKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.Delete && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.D && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressCtrlDeleteKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.Delete && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressLeftArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.LeftArrow && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.B && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressSpaceKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.Spacebar && keyinfo.Modifiers == 0);
        }

        public static bool IsPressRightArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.RightArrow && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.F && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressUpArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.UpArrow && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.P && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressDownArrowKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.DownArrow && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.N && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        public static bool IsPressPageUpKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.PageUp && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.P && keyinfo.Modifiers == ConsoleModifiers.Alt);
        }

        public static bool IsPressPageDownKey(this ConsoleKeyInfo keyinfo)
        {
            return (keyinfo.Key == ConsoleKey.PageDown && keyinfo.Modifiers == 0) || (keyinfo.Key == ConsoleKey.N && keyinfo.Modifiers == ConsoleModifiers.Alt);
        }

        public static bool IsPressEscKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Escape && keyinfo.Modifiers == 0;
        }

        public static bool IsPressCtrlCKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.C && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        public static ConsoleKeyInfo ToCase(this ConsoleKeyInfo keyinfo, CaseOptions value)
        {
            return value switch
            {
                CaseOptions.Uppercase => new ConsoleKeyInfo(char.ToUpperInvariant(keyinfo.KeyChar), keyinfo.Key, keyinfo.Modifiers == ConsoleModifiers.Shift, keyinfo.Modifiers == ConsoleModifiers.Alt, keyinfo.Modifiers == ConsoleModifiers.Control),
                CaseOptions.Lowercase => new ConsoleKeyInfo(char.ToLowerInvariant(keyinfo.KeyChar), keyinfo.Key, keyinfo.Modifiers == ConsoleModifiers.Shift, keyinfo.Modifiers == ConsoleModifiers.Alt, keyinfo.Modifiers == ConsoleModifiers.Control),
                _ => keyinfo,
            };
        }

    }
}
