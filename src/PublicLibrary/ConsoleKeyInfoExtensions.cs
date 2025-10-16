// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents KeyInfo Extensions
    /// </summary>
    public static class ConsoleKeyInfoExtensions
    {
        /// <summary>
        /// Check ConsoleKeyInfo is Special Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="key"><see cref="ConsoleKey"/> to compare</param>
        /// <param name="modifier"><see cref="ConsoleModifiers"/> to compare</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
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

        /// <summary>
        /// Check ConsoleKeyInfo is Enter Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">If <c>true</c> accept 'CTRL+J' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressEnterKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return keyinfo.Key == ConsoleKey.Enter || (emacskeys && keyinfo.Key == ConsoleKey.J && keyinfo.Modifiers == ConsoleModifiers.Control);
            }
            return (keyinfo.KeyChar == 13 && keyinfo.Modifiers == 0) || (keyinfo.KeyChar == 10 && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.J && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        private static bool IsEqualChar(this ConsoleKeyInfo consoleKeyInfo, char charB)
        {
            return char.ToLower(consoleKeyInfo.KeyChar) == char.ToLower(charB) && !consoleKeyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt | ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Yes key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsYesResponseKey(this ConsoleKeyInfo keyinfo)
        {
            if (!PromptPlus.Config.YesChar.HasValue)
            {
                return false;
            }
            return keyinfo.IsEqualChar(PromptPlus.Config.YesChar.Value);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Yes key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsNoResponseKey(this ConsoleKeyInfo keyinfo)
        {
            if (!PromptPlus.Config.NoChar.HasValue)
            {
                return false;
            }
            return keyinfo.IsEqualChar(PromptPlus.Config.NoChar.Value);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Abort key Control
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsAbortKeyPress(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Equals(PromptPlus.Config.HotKeyAbortKeyPress.KeyInfo);
        }



        /// <summary>
        /// Check ConsoleKeyInfo is Lowers Current Word Emacs Key 
        /// <br>Alt+L = Lowers the case of every character from the cursor's position to the end of the current words</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsLowersCurrentWord(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.L && keyinfo.Modifiers == ConsoleModifiers.Alt;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Clear Before Cursor Emacs Key 
        /// <br>Ctrl+U = Clears the line content before the cursor</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsClearBeforeCursor(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.U && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Clear After Cursor Emacs Key 
        /// <br>Ctrl+K = Clears the line content after the cursor</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsClearAfterCursor(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.K && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Clear Word Before Cursor Emacs Key 
        /// <br>Ctrl+W = Clears the word before the cursor</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsClearWordBeforeCursor(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.W && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Clear Word After Cursor Emacs Key 
        /// <br>Ctrl+D = Clears the word after the cursor</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsClearWordAfterCursor(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.D && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Capitalize Over Cursor Emacs Key 
        /// <br>Alt+C = Capitalizes the character under the cursor and moves to the end of the word</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsCapitalizeOverCursor(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.C && keyinfo.Modifiers == ConsoleModifiers.Alt;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Forward Word Emacs Key 
        /// <br>Alt+F = Moves the cursor forward one word.</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsForwardWord(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.F && keyinfo.Modifiers == ConsoleModifiers.Alt;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Backward Word Emacs Key 
        /// <br>Alt+B = Moves the cursor backward one word.</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsBackwardWord(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.B && keyinfo.Modifiers == ConsoleModifiers.Alt;
        }


        /// <summary>
        /// Check ConsoleKeyInfo is Lowers Current Word Emacs Key 
        /// <br>Alt+U = Upper the case of every character from the cursor's position to the end of the current word</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsUppersCurrentWord(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.U && keyinfo.Modifiers == ConsoleModifiers.Alt;
        }


        /// <summary>
        /// Check ConsoleKeyInfo is Transpose Previous Emacs Key 
        /// <br>Ctrl+T = Transpose the previous two characters</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsTransposePrevious(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.T && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Clear Emacs Key 
        /// <br>Ctrl+L = Clears the content</br>
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsClearContent(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.L && keyinfo.Modifiers == ConsoleModifiers.Control;
        }


        /// <summary>
        /// Check ConsoleKeyInfo is Tab Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressTabKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Tab && keyinfo.Modifiers == 0;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Shift + Tab Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressShiftTabKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Tab && keyinfo.Modifiers == ConsoleModifiers.Shift;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is End Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+E' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressEndKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.End && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.E && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is End Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+A' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressHomeKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.Home && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.A && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is End Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+H' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressBackspaceKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.Backspace && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.H && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Delete Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+D' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressDeleteKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.Delete && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.D && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        internal static bool IsPressCtrlDeleteKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Delete && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Left Arrow Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+B' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressLeftArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.LeftArrow && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.B && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Space Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressSpaceKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Spacebar && keyinfo.Modifiers == 0;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Space Key + Ctrl Modifier
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressCtrlSpaceKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Spacebar && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Right Arrow Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+F' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressRightArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.RightArrow && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.F && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Up Arrow Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+P' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressUpArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.UpArrow && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.P && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Down Arrow Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'CTRL+N' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressDownArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.DownArrow && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.N && keyinfo.Modifiers == ConsoleModifiers.Control);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is PageUp Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'Alt+P' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressPageUpKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.PageUp && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.P && keyinfo.Modifiers == ConsoleModifiers.Alt);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is PageDown Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <param name="emacskeys">if <c>true</c> accept 'Alt+N' </param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressPageDownKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
        {
            return (keyinfo.Key == ConsoleKey.PageDown && keyinfo.Modifiers == 0) || (emacskeys && keyinfo.Key == ConsoleKey.N && keyinfo.Modifiers == ConsoleModifiers.Alt);
        }

        /// <summary>
        /// Check ConsoleKeyInfo is Esc Key
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to check</param>
        /// <returns><c>true</c> if equal otherwise <c>false</c>.</returns>
        public static bool IsPressEscKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.Escape && keyinfo.Modifiers == 0;
        }

        internal static bool IsPressCtrlCKey(this ConsoleKeyInfo keyinfo)
        {
            return keyinfo.Key == ConsoleKey.C && keyinfo.Modifiers == ConsoleModifiers.Control;
        }

        /// <summary>
        /// Convert <see cref="ConsoleKeyInfo"/> KeyChar to Uppercase / Lowercase
        /// </summary>
        /// <param name="keyinfo"><see cref="ConsoleKeyInfo"/> to convert</param>
        /// <param name="value">The <see cref="CaseOptions"/></param>
        /// <returns><see cref="ConsoleKeyInfo"/> converted</returns>
        public static ConsoleKeyInfo ToCase(this ConsoleKeyInfo keyinfo, CaseOptions value)
        {
            return value switch
            {
                CaseOptions.Uppercase => new ConsoleKeyInfo(char.ToUpper(keyinfo.KeyChar), keyinfo.Key, keyinfo.Modifiers.HasFlag(ConsoleModifiers.Shift), keyinfo.Modifiers.HasFlag(ConsoleModifiers.Alt), keyinfo.Modifiers.HasFlag(ConsoleModifiers.Control)),
                CaseOptions.Lowercase => new ConsoleKeyInfo(char.ToLower(keyinfo.KeyChar), keyinfo.Key, keyinfo.Modifiers.HasFlag(ConsoleModifiers.Shift), keyinfo.Modifiers.HasFlag(ConsoleModifiers.Alt), keyinfo.Modifiers.HasFlag(ConsoleModifiers.Control)),
                _ => keyinfo,
            };
        }

    }
}
