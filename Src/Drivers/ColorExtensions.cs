// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// ***************************************************************************************

namespace PPlus
{
    /// <summary>
    /// Contains extension methods for <see cref="Color"/>.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Get Inverted color by Luminance for best contrast 
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>
        /// <see cref="Color"/> White or Black
        /// </returns>
        public static Color GetInvertedColor(this Color value)
        {
            return value.GetLuminance() < 140 ? Color.White : Color.Black;
        }

        private static float GetLuminance(this Color color)
        {
            return (float)(0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B);
        }
    }
}
