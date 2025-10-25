// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides extension methods for creating modified <see cref="Style"/> instances
    /// by selectively changing foreground, background, or overflow strategy.
    /// </summary>
    public static class StyleExtensions
    {
        /// <summary>
        /// Creates a new <see cref="Style"/> based on <paramref name="style"/> with a replaced foreground color.
        /// Background and overflow strategy are preserved.
        /// </summary>
        /// <param name="style">The original style.</param>
        /// <param name="color">The new foreground <see cref="Color"/>.</param>
        /// <returns>A new <see cref="Style"/> with the updated foreground color.</returns>
        public static Style ForeGround(this Style style, Color color)
        {
            return new Style(
                foreground: color,
                background: style.Background,
                overflowStrategy: style.OverflowStrategy);
        }

        /// <summary>
        /// Creates a new <see cref="Style"/> based on <paramref name="style"/> with a replaced overflow strategy.
        /// Foreground and background colors are preserved.
        /// </summary>
        /// <param name="style">The original style.</param>
        /// <param name="overflow">The new <see cref="Overflow"/> strategy.</param>
        /// <returns>A new <see cref="Style"/> with the updated overflow strategy.</returns>
        public static Style Overflow(this Style style, Overflow overflow)
        {
            return new Style(
                foreground: style.Foreground,
                background: style.Background,
                overflowStrategy: overflow);
        }

        /// <summary>
        /// Creates a new <see cref="Style"/> based on <paramref name="style"/> with a replaced background color.
        /// Foreground color and overflow strategy are preserved.
        /// </summary>
        /// <param name="style">The original style.</param>
        /// <param name="color">The new background <see cref="Color"/>.</param>
        /// <returns>A new <see cref="Style"/> with the updated background color.</returns>
        public static Style Background(this Style style, Color color)
        {
            return new Style(
                foreground: style.Foreground,
                background: color,
                overflowStrategy: style.OverflowStrategy);
        }
    }
}
