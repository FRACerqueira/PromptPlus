// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Text;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the HotKey to control
    /// </summary>
    /// <remarks>
    /// Create a HotKey
    /// </remarks>
    /// <param name="key"><see cref="ConsoleKey"/> to create </param>
    /// <param name="alt">With Alt key</param>
    /// <param name="ctrl">With Ctrl key</param>
    /// <param name="shift">With Shift key</param>
    public struct HotKey(ConsoleKey key, bool alt = false, bool ctrl = false, bool shift = false) : IEquatable<ConsoleKeyInfo>
    {
        /// <summary>
        /// Get <see cref="ConsoleKeyInfo"/> to HotKey
        /// </summary>
        public readonly ConsoleKeyInfo KeyInfo => new((char)Key, Key, Shift, Alt, Ctrl);

        /// <summary>
        /// Get <see cref="ConsoleKey"/> to HotKey
        /// </summary>
        public ConsoleKey Key { get; private set; } = key;

        /// <summary>
        /// Get <see cref="ConsoleModifiers"/> Alt to HotKey
        /// </summary>
        public bool Alt { get; private set; } = alt;

        /// <summary>
        /// Get <see cref="ConsoleModifiers"/> Ctrl to HotKey
        /// </summary>
        public bool Ctrl { get; private set; } = ctrl;

        /// <summary>
        /// Get <see cref="ConsoleModifiers"/> Shift to HotKey
        /// </summary>
        public bool Shift { get; private set; } = shift;

        /// <inheritdoc/>
        public override readonly string ToString()
        {
            StringBuilder modifiers = new();
            if (KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                modifiers.Append("Ctrl+");
            }
            if (KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
            {
                modifiers.Append("Shift+");
            }
            if (KeyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt))
            {
                modifiers.Append("Alt+");
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

        /// <summary>
        /// Get HotKey abort control 'Esc'
        /// </summary>
        internal static HotKey AbortKeyPress => new(ConsoleKey.Escape);

        /// <summary>
        /// Get HotKey default for Togger tooltip 'F1'
        /// </summary>
        internal static HotKey TooltipToggle => new(ConsoleKey.F1);

        /// <summary>
        /// Get HotKey default for show/hide tooltip 'Ctrl+F1'
        /// </summary>
        internal static HotKey TooltipShowHide => new(ConsoleKey.F1, false, true, false);

        /// <summary>
        /// Get HotKey default for show/hide calendar note 'F2'
        /// </summary>
        internal static HotKey CalendarSwitchNotes => new(ConsoleKey.F2);

        /// <summary>
        /// Get HotKey default for show/hide Input Password View 'F2'
        /// </summary>
        internal static HotKey InputPasswordView => new(ConsoleKey.F2);

        /// <summary>
        /// Get HotKey default for switch show History entries 'F3'
        /// </summary>
        internal static HotKey InputHistoryView => new(ConsoleKey.F3);

        /// <summary>
        /// Get HotKey default for Toggle FullPath 'F2'
        /// </summary>
        internal static HotKey ToggleFullPath => new(ConsoleKey.F2);


        /// <summary>
        /// Get HotKey default for Select All 'F4'
        /// </summary>
        internal static HotKey ToggleFilterMode => new(ConsoleKey.F4);

        /// <summary>
        /// Get HotKey default for Select All 'F2'
        /// </summary>
        internal static HotKey ToggleAll => new(ConsoleKey.F2);

        /// <summary>
        /// Get HotKey default for ChartBar Switch Layout 'F2'
        /// </summary>
        internal static HotKey ChartBarSwitchLayout => new(ConsoleKey.F2);

        /// <summary>
        /// Get HotKey default for ChartBar Switch Legend 'F3'
        /// </summary>
        internal static HotKey ChartBarSwitchLegend => new(ConsoleKey.F3);

        /// <summary>
        /// Get HotKey default for ChartBar Switch Order 'F4'
        /// </summary>
        internal static HotKey ChartBarSwitchOrder => new(ConsoleKey.F4);
    }
}
