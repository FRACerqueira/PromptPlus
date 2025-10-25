// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Text;
using System.Text.Json.Serialization;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a configurable hotkey composed of a base <see cref="ConsoleKey"/> and optional modifier flags.
    /// </summary>
    /// <remarks>
    /// This struct is lightweight and immutable after construction (properties are init via the primary constructor).
    /// Use the static members for common built‑in hotkeys.
    /// </remarks>
    /// <param name="key">The primary <see cref="ConsoleKey"/>.</param>
    /// <param name="alt">Indicates whether Alt is part of the hotkey.</param>
    /// <param name="ctrl">Indicates whether Ctrl is part of the hotkey.</param>
    /// <param name="shift">Indicates whether Shift is part of the hotkey.</param>
    public struct HotKey(ConsoleKey key, bool alt = false, bool ctrl = false, bool shift = false) : IEquatable<ConsoleKeyInfo>
    {
        /// <summary>
        /// Gets the <see cref="ConsoleKeyInfo"/> representation of this hotkey.
        /// </summary>
        [JsonIgnore]
        public readonly ConsoleKeyInfo KeyInfo => new((char)Key, Key, Shift, Alt, Ctrl);

        /// <summary>
        /// Gets the base <see cref="ConsoleKey"/> of this hotkey.
        /// </summary>
        public ConsoleKey Key { get; private set; } = key;

        /// <summary>
        /// Gets a value indicating whether Alt is included.
        /// </summary>
        public bool Alt { get; private set; } = alt;

        /// <summary>
        /// Gets a value indicating whether Ctrl is included.
        /// </summary>
        public bool Ctrl { get; private set; } = ctrl;

        /// <summary>
        /// Gets a value indicating whether Shift is included.
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
        /// Determines whether this hotkey matches the provided <see cref="ConsoleKeyInfo"/>.
        /// </summary>
        /// <param name="other">The key info to compare.</param>
        /// <returns><c>true</c> if both the key and modifier set are equal; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(ConsoleKeyInfo other) => Key == other.Key && KeyInfo.Modifiers == other.Modifiers;

        /// <summary>
        /// Gets the default abort hotkey (Esc).
        /// </summary>
        internal static HotKey AbortKeyPress => new(ConsoleKey.Escape);

        /// <summary>
        /// Gets the default tooltip toggle hotkey (F1).
        /// </summary>
        internal static HotKey TooltipToggle => new(ConsoleKey.F1);

        /// <summary>
        /// Gets the default show/hide tooltip hotkey (Ctrl+F1).
        /// </summary>
        internal static HotKey TooltipShowHide => new(ConsoleKey.F1, alt: false, ctrl: true, shift: false);

        /// <summary>
        /// Gets the default calendar notes toggle hotkey (F2).
        /// </summary>
        internal static HotKey CalendarSwitchNotes => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default input password visibility toggle hotkey (F2).
        /// </summary>
        internal static HotKey InputPasswordView => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default history view toggle hotkey (F3).
        /// </summary>
        internal static HotKey InputHistoryView => new(ConsoleKey.F3);

        /// <summary>
        /// Gets the default full path toggle hotkey (F2).
        /// </summary>
        internal static HotKey ToggleFullPath => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default filter mode toggle hotkey (F4).
        /// </summary>
        internal static HotKey ToggleFilterMode => new(ConsoleKey.F4);

        /// <summary>
        /// Gets the default select all toggle hotkey (F2).
        /// </summary>
        internal static HotKey ToggleAll => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default chart bar layout switch hotkey (F2).
        /// </summary>
        internal static HotKey ChartBarSwitchLayout => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default chart bar legend switch hotkey (F3).
        /// </summary>
        internal static HotKey ChartBarSwitchLegend => new(ConsoleKey.F3);

        /// <summary>
        /// Gets the default chart bar order switch hotkey (F4).
        /// </summary>
        internal static HotKey ChartBarSwitchOrder => new(ConsoleKey.F4);
    }
}
