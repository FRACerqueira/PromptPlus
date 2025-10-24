// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Linq;

namespace PromptPlusLibrary.Core.Ansi
{
    internal static class AnsiSequences
    {
        /// <summary>
        /// The ASCII escape character (decimal 27).
        /// </summary>
        public const string ESC = "\u001b";

        /// <summary>
        /// Introduces a control sequence that uses 8-bit characters.
        /// </summary>
        public const string CSI = ESC + "[";

        /// <summary>
        /// Text cursor enable.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/DECRQM.html#T5-8"/>.
        /// </remarks>
        public const int DECTCEM = 25;

        /// <summary>
        /// This control function selects one or more character attributes at the same time.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/SGR.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string SGR(params byte[] codes)
        {
            string joinedCodes = string.Join(";", codes.Select(c => c.ToString()));
            return $"{CSI}{joinedCodes}m";
        }

        /// <summary>
        /// This control function erases characters from part or all of the display.
        /// When you erase complete lines, they become single-height, single-width lines,
        /// with all visual character attributes cleared.
        /// ED works inside or outside the scrolling margins.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/ED.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string ED(int code)
        {
            return $"{CSI}{code}J";
        }

        /// <summary>
        /// Hides the cursor.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/RM.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string RM(int code = DECTCEM)
        {
            return $"{CSI}?{code}l";
        }

        /// <summary>
        /// Shows the cursor.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/SM.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string SM(int code = DECTCEM)
        {
            return $"{CSI}?{code}h";
        }

        /// <summary>
        /// This control function erases characters on the line that has the cursor.
        /// EL clears all character attributes from erased character positions.
        /// EL works inside or outside the scrolling margins.
        /// </summary>
        /// <remarks>
        /// See <see href="https://vt100.net/docs/vt510-rm/EL.html"/>.
        /// </remarks>
        /// <returns>The ANSI escape code.</returns>
        public static string EL(int code)
        {
            return $"{CSI}{code}K";
        }
    }
}
