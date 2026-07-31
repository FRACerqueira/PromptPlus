// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System.Text;

namespace PromptPlusLibrary.Controls.Common
{
    /// <summary>
    /// Shared text layout helpers built on <see cref="StringExtensions.GetDisplayLength"/> and
    /// <see cref="StringExtensions.TruncateToDisplayWidth"/> — the canonical primitives for measuring
    /// and cutting text by terminal COLUMNS rather than <c>string.Length</c> (character count), which
    /// undercounts East Asian wide runes (CJK: 1 character occupies 2 columns).
    /// </summary>
    /// <remarks>
    /// Centralized here instead of duplicated per control: <see cref="Truncate"/>/<see cref="AlignCell"/>
    /// used to be near-identical private copies in <c>TableControl</c> and <c>MultiTableControl</c>.
    /// </remarks>
    internal static class DisplayWidthHelpers
    {
        /// <summary>
        /// Returns the longest prefix of <paramref name="value"/> whose display width does not exceed
        /// <paramref name="width"/> columns, without splitting a wide rune in half.
        /// </summary>
        internal static string Truncate(string value, int width)
        {
            if (width <= 0)
            {
                return string.Empty;
            }

            int displayWidth = value.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
            return displayWidth <= width ? value : value.TruncateToDisplayWidth(width);
        }

        /// <summary>
        /// Truncates <paramref name="value"/> to <paramref name="width"/> display columns (see
        /// <see cref="Truncate"/>), then pads it to exactly <paramref name="width"/> columns per
        /// <paramref name="alignment"/>. Used for table/column cell and header rendering.
        /// </summary>
        internal static string AlignCell(string value, int width, ColumnAlignment alignment)
        {
            string normalized = Truncate(value, width);
            int normalizedWidth = normalized.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
            int missing = width - normalizedWidth;
            if (missing <= 0)
            {
                return normalized;
            }

            return alignment switch
            {
                ColumnAlignment.Right => new string(' ', missing) + normalized,
                ColumnAlignment.Center => new string(' ', missing / 2) + normalized + new string(' ', missing - (missing / 2)),
                _ => normalized + new string(' ', missing)
            };
        }

        /// <summary>
        /// Truncates <paramref name="text"/> to <paramref name="maxWidth"/> display columns when it
        /// overflows, then pads it to exactly <paramref name="maxWidth"/> columns per
        /// <paramref name="alignment"/>. Used for single-line text alignment (e.g. a chart title)
        /// against a fixed column budget.
        /// </summary>
        internal static string AlignLine(string text, int maxWidth, TextAlignment alignment)
        {
            int textWidth = text.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
            if (textWidth > maxWidth)
            {
                text = text.TruncateToDisplayWidth(maxWidth);
                textWidth = text.GetDisplayLength() is { Length: > 0 } dt ? dt[0] : 0;
            }
            int missing = System.Math.Max(0, maxWidth - textWidth);

            return alignment switch
            {
                TextAlignment.Right => new string(' ', missing) + text,
                TextAlignment.Center => new string(' ', missing / 2) + text + new string(' ', missing - (missing / 2)),
                _ => text + new string(' ', missing)
            };
        }

        /// <summary>
        /// Counts the number of Unicode runes (symbols) in <paramref name="value"/> — a surrogate
        /// pair (a character outside the Basic Multilingual Plane) counts as 1, unlike
        /// <c>string.Length</c> which counts it as 2. Use when a "number of characters" contract
        /// means "number of symbols", not raw UTF-16 units.
        /// </summary>
        internal static int CountRunes(string value)
        {
            int count = 0;
            foreach (Rune _ in value.EnumerateRunes())
            {
                count++;
            }
            return count;
        }

        /// <summary>
        /// Returns the longest prefix of <paramref name="value"/> containing at most
        /// <paramref name="maxRunes"/> Unicode runes, never splitting a surrogate pair.
        /// </summary>
        internal static string TruncateToRuneCount(string value, int maxRunes)
        {
            if (string.IsNullOrEmpty(value) || maxRunes <= 0)
            {
                return string.Empty;
            }

            int runeCount = 0;
            int endIndex = 0;
            foreach (Rune rune in value.EnumerateRunes())
            {
                if (runeCount >= maxRunes)
                {
                    break;
                }
                runeCount++;
                endIndex += rune.Utf16SequenceLength;
            }

            return endIndex == value.Length ? value : value[..endIndex];
        }

        /// <summary>
        /// Pads <paramref name="text"/> on the right with spaces so its display width reaches
        /// <paramref name="targetWidth"/> columns. Never truncates.
        /// </summary>
        internal static string PadToDisplayWidth(string text, int targetWidth)
        {
            int width = text.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
            return width < targetWidth ? text + new string(' ', targetWidth - width) : text;
        }

        /// <summary>
        /// Fits <paramref name="text"/> to exactly <paramref name="targetWidth"/> display columns:
        /// truncates (without splitting a wide rune) when wider, pads on the left when narrower.
        /// </summary>
        internal static string FitToDisplayWidth(string text, int targetWidth)
        {
            int width = text.GetDisplayLength() is { Length: > 0 } d ? d[0] : 0;
            if (width > targetWidth)
            {
                text = text.TruncateToDisplayWidth(targetWidth);
                width = text.GetDisplayLength() is { Length: > 0 } d2 ? d2[0] : 0;
            }
            return width < targetWidth ? new string(' ', targetWidth - width) + text : text;
        }
    }
}
