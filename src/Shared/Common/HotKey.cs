// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Core;
using PromptPlusLibrary.Resources;
using System;
using System.Text.Json.Serialization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a configurable hotkey composed of a base <see cref="ConsoleKey"/> and optional modifier flags.
    /// </summary>
    /// <remarks>
    /// This struct is lightweight and immutable after construction (properties are init via the primary constructor).
    /// Use the static members for common built-in hotkeys.
    /// </remarks>
    /// <param name="key">The primary <see cref="ConsoleKey"/>.</param>
    /// <param name="alt">Indicates whether Alt is part of the hotkey.</param>
    /// <param name="ctrl">Indicates whether Ctrl is part of the hotkey.</param>
    /// <param name="shift">Indicates whether Shift is part of the hotkey.</param>
    [method: JsonConstructor]
    [JsonConverter(typeof(HotKeyJsonConverter))]
    public readonly struct HotKey(ConsoleKey key, bool alt = false, bool ctrl = false, bool shift = false)
        : IEquatable<HotKey>, IEquatable<ConsoleKeyInfo>
    {

        /// <summary>
        /// Gets the <see cref="ConsoleKeyInfo"/> representation of this hotkey.
        /// </summary>
        /// <remarks>
        /// <see cref="ConsoleKeyInfo.KeyChar"/> is only set for an explicit allowlist of keys whose
        /// <see cref="ConsoleKey"/> numeric value coincides with a real character code (letters,
        /// digits, space, Backspace/Tab/Enter/Escape). A numeric-range check is not safe here: many
        /// non-printable keys (arrows, Home/End/PageUp/PageDown, function keys, ...) have enum
        /// values that fall inside the printable ASCII band by coincidence. Every key outside the
        /// allowlist reports <c>'\0'</c>, matching what a real console reports for those keys,
        /// instead of casting the enum value into a misleading char.
        /// </remarks>
        [JsonIgnore]
        public readonly ConsoleKeyInfo KeyInfo
        {
            get
            {
                bool hasRealChar = Key is ConsoleKey.Backspace or ConsoleKey.Tab or ConsoleKey.Enter or ConsoleKey.Escape or ConsoleKey.Spacebar
                    or (>= ConsoleKey.D0 and <= ConsoleKey.D9)
                    or (>= ConsoleKey.A and <= ConsoleKey.Z);
                return new(hasRealChar ? (char)Key : (char)0, Key, Shift, Alt, Ctrl);
            }
        }

        /// <summary>
        /// Gets the base <see cref="ConsoleKey"/> of this hotkey.
        /// </summary>
        public ConsoleKey Key { get; } = key;

        /// <summary>
        /// Gets a value indicating whether Alt is included.
        /// </summary>
        public bool Alt { get; } = alt;

        /// <summary>
        /// Gets a value indicating whether Ctrl is included.
        /// </summary>
        public bool Ctrl { get; } = ctrl;

        /// <summary>
        /// Gets a value indicating whether Shift is included.
        /// </summary>
        public bool Shift { get; } = shift;

        /// <inheritdoc/>
        public override readonly string ToString()
        {
            var prefix = string.Concat(
                Ctrl  ? "Ctrl "  : string.Empty,
                Shift ? "Shift " : string.Empty,
                Alt   ? "Alt "   : string.Empty);
            return Key switch
            {
                ConsoleKey.Escape => $"{prefix}Esc",
                ConsoleKey.Spacebar => $"{prefix}{PromptPlusResources.Space}",
                ConsoleKey.Multiply => $"{prefix}*",
                ConsoleKey.Add => $"{prefix}+",
                ConsoleKey.OemPlus => $"{prefix}+",
                ConsoleKey.Subtract => $"{prefix}-",
                ConsoleKey.OemMinus => $"{prefix}-",
                ConsoleKey.Divide => $"{prefix}/",
                >= ConsoleKey.F1 and <= ConsoleKey.F24 => $"{prefix}{Key}",
                _ => (int)Key is > 32 and < 127 ? $"{prefix}{(char)Key}" : $"{prefix}{Key}"
            };
        }

        /// <summary>
        /// Determines whether this hotkey matches the provided <see cref="ConsoleKeyInfo"/>.
        /// </summary>
        /// <param name="other">The key info to compare.</param>
        /// <returns><c>true</c> if both the key and modifier set are equal; otherwise, <c>false</c>.</returns>
        public readonly bool Equals(ConsoleKeyInfo other)
        {
            ConsoleModifiers mods = (Ctrl  ? ConsoleModifiers.Control : default)
                                  | (Shift ? ConsoleModifiers.Shift   : default)
                                  | (Alt   ? ConsoleModifiers.Alt     : default);
            return Key == other.Key && mods == other.Modifiers;
        }

        /// <inheritdoc/>
        public readonly bool Equals(HotKey other) =>
            Key == other.Key && Alt == other.Alt && Ctrl == other.Ctrl && Shift == other.Shift;

        /// <inheritdoc/>
        public override readonly bool Equals(object? obj) => obj switch
        {
            HotKey hk         => Equals(hk),
            ConsoleKeyInfo ki => Equals(ki),
            _                 => false
        };

        /// <inheritdoc/>
        public override readonly int GetHashCode() => HashCode.Combine(Key, Alt, Ctrl, Shift);

        /// <summary>Returns <c>true</c> if two hotkeys are equal.</summary>
        public static bool operator ==(HotKey left, HotKey right) => left.Equals(right);

        /// <summary>Returns <c>true</c> if two hotkeys are not equal.</summary>
        public static bool operator !=(HotKey left, HotKey right) => !left.Equals(right);

        /// <summary>
        /// Gets the default abort hotkey (Esc).
        /// </summary>
        public static HotKey DefaultAbortKeyPress => new(ConsoleKey.Escape);

        /// <summary>
        /// Gets the default tooltip toggle hotkey (F1).
        /// </summary>
        public static HotKey DefaultTooltip => new(ConsoleKey.F1);

        /// <summary>
        /// Gets the default tooltip Filter all selected items (F3).
        /// </summary>
        public static HotKey DefaultFilterAllSelected => new(ConsoleKey.F3);

        /// <summary>
        /// Gets the default show/hide tooltip hotkey (Ctrl+F1).
        /// </summary>
        public static HotKey DefaultTooltipShowHide => new(ConsoleKey.F1, alt: false, ctrl: true, shift: false);

        /// <summary>
        /// Gets the default calendar notes toggle hotkey (F2).
        /// </summary>
        public static HotKey DefaultCalendarSwitchNotes => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default input password visibility toggle hotkey (F2).
        /// </summary>
        public static HotKey DefaultInputPasswordView => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default history view toggle hotkey (F3).
        /// </summary>
        public static HotKey DefaultInputHistoryView => new(ConsoleKey.F3);

        /// <summary>
        /// Gets the default full path toggle hotkey (Shift+F3).
        /// </summary>
        public static HotKey DefaultToggleFullPath => new(ConsoleKey.F3,shift:true);

        /// <summary>
        /// Gets the default select all toggle hotkey (F2).
        /// </summary>
        public static HotKey DefaultToggleAll => new(ConsoleKey.F2);

        /// <summary>
        ///  Gets the default select all childs toggle hotkey (F4).
        /// </summary>
        public static HotKey DefaultToggleWildcard => new(ConsoleKey.F4);

        /// <summary>
        /// Gets the default chart bar layout switch hotkey (F2).
        /// </summary>
        public static HotKey DefaultChartBarSwitchLayout => new(ConsoleKey.F2);

        /// <summary>
        /// Gets the default chart bar legend switch hotkey (F3).
        /// </summary>
        public static HotKey DefaultChartBarSwitchLegend => new(ConsoleKey.F3);

        /// <summary>
        /// Gets the default chart bar order switch hotkey (F4).
        /// </summary>
        public static HotKey DefaultChartBarSwitchOrder => new(ConsoleKey.F4);
    }
}
