// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Extensions for the Color class.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Converts a Color to a ConsoleColor.
        /// </summary>
        /// <param name="colorRGB"></param>
        /// <returns><see cref="ConsoleColor"/></returns>
        public static ConsoleColor ToConsoleColor(this Color colorRGB)
        {
            return colorRGB.ToConsoleColor();
        }
    }
}
