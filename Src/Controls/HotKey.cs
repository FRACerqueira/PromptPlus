// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the HotKey to control
    /// </summary>
    public struct HotKey : IEquatable<ConsoleKeyInfo>
    {
        /// <summary>
        /// Create a HotKey
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public HotKey() 
        {
            throw new PromptPlusException("HotKey CTOR NotImplemented");
        }

        /// <summary>
        /// Create a HotKey
        /// </summary>
        /// <param name="key"><see cref="UserHotKey"/> to create </param>
        /// <param name="alt">With Alt key</param>
        /// <param name="ctrl">With Ctrl key</param>
        /// <param name="shift">With Shift key</param>
        public HotKey(UserHotKey key, bool shift = false, bool alt = false, bool ctrl = false)
        {
            Key = Enum.Parse<ConsoleKey>(key.ToString(), true);
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
        }

        /// <summary>
        /// Get HotKey default for Tooltip 'F1'
        /// </summary>
        public static HotKey TooltipDefault => new(ConsoleKey.F1);

        /// <summary>
        /// Get HotKey default for PasswordView 'F2'
        /// </summary>
        public static HotKey PasswordViewDefault => new(ConsoleKey.F2);

        /// <summary>
        /// Get HotKey default for Tooltip FullPath 'F2'
        /// </summary>
        public static HotKey TooltipFullPathDefault => new(ConsoleKey.F2);


        /// <summary>
        /// Get HotKey default for expand node 'F3'
        /// </summary>
        public static HotKey ToggleExpandNodeDefault => new (ConsoleKey.F3);


        /// <summary>
        /// Get HotKey default for expand node 'F4'
        /// </summary>
        public static HotKey ToggleExpandAllNodeDefault => new(ConsoleKey.F4);

        /// <summary>
        /// Get HotKey default for Select All 'F2'
        /// </summary>
        public static HotKey SelectAllDefault => new(ConsoleKey.F2);

        /// <summary>
        /// Get HotKey default for Edit Item 'F2'
        /// </summary>
        public static HotKey EditItemDefault => new(ConsoleKey.F2);

        /// <summary>
        /// Get HotKey default for Invert Selected 'F3'
        /// </summary>
        public static HotKey InvertSelectedDefault => new(ConsoleKey.F3);

        /// <summary>
        /// Get HotKey default for Remove item 'F3'
        /// </summary>
        public static HotKey RemoveItemDefault => new(ConsoleKey.F3);

        /// <summary>
        /// Create a HotKey
        /// </summary>
        /// <param name="key"><see cref="ConsoleKey"/> to create </param>
        /// <param name="alt">With Alt key</param>
        /// <param name="ctrl">With Ctrl key</param>
        /// <param name="shift">With Shift key</param>
        internal HotKey(ConsoleKey key, bool alt = false, bool ctrl = false, bool shift = false)
        {
            Key = key;
            Alt = alt;
            Ctrl = ctrl;
            Shift = shift;
        }

        /// <summary>
        /// Get <see cref="ConsoleKeyInfo"/> to HotKey
        /// </summary>
        public readonly ConsoleKeyInfo KeyInfo => new((char)Key, Key, Shift, Alt, Ctrl);

        internal ConsoleKey Key { get; private set; }
        internal bool Alt { get; private set; }
        internal bool Ctrl { get; private set; }
        internal bool Shift { get; private set; }

        /// <inheritdoc/>
        public override readonly string ToString()
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

        /// <summary>
        /// Checks Hotkey instances are equal the <see cref="ConsoleKeyInfo"/>.
        /// </summary>
        /// <param name="other">The ConsoleKeyInfo to compare.</param>
        /// <returns><c>true</c> if the Hotkey are equal, otherwise <c>false</c>.</returns>
        public readonly bool Equals(ConsoleKeyInfo other) => Key == other.Key && KeyInfo.Modifiers == other.Modifiers;

    }
}
