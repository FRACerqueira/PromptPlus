// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus
{
    /// <summary>
    /// Contains extension methods for <see cref="Style"/>.
    /// </summary>
    public static class StyleExtensions
    {
        /// <summary>
        /// Create a new style from the specified one with the specified foreground color.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="color">The foreground color.</param>
        /// <returns>The new <see cref="Style"/></returns>
        public static Style Foreground(this Style style, Color color)
        {
            return new Style(
                foreground: color,
                background: style.Background,
                overflowStrategy: style.OverflowStrategy);
        }

        /// <summary>
        /// Create a new style from the specified one with the specified overfow strategy
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="overflow">The <see cref="PPlus.Overflow"/> overflow strategy</param>
        /// <returns>The new <see cref="Style"/></returns>
        public static Style Overflow(this Style style, Overflow overflow)
        {
            return new Style(
                foreground: style.Foreground,
                background: style.Background,
                overflowStrategy: overflow);
        }

        /// <summary>
        /// Create a new style from the specified one with
        /// the specified background color.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="color">The background color.</param>
        /// <returns>The new <see cref="Style"/></returns>
        public static Style Background(this Style style, Color color)
        {
            return new Style(
                foreground: style.Foreground,
                background: color,
                overflowStrategy: style.OverflowStrategy);
        }
    }
}
