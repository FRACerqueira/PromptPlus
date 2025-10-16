// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the Style of color and overflow strategy.
    /// </summary>
    /// <remarks>
    /// Create a new instance of <see cref="Style"/> with foreground/background color and overflow strategy.
    /// </remarks>
    /// <param name="foreground"><see cref="Color"/> foreground</param>
    /// <param name="background"><see cref="Color"/> background</param>
    /// <param name="overflowStrategy"><see cref="Overflow"/> Strategy</param>
    public readonly struct Style(Color foreground, Color background, Overflow overflowStrategy = Overflow.None) : IEquatable<Style>
    {
        internal const char UnicodeEllipsis = '…';
        internal const char AsciiEllipsis = '.';

        /// <summary>
        /// Create a new style with default value from Console.
        /// </summary>
        /// <returns>The new <see cref="Style"/></returns>
        public static Style Default() => new(
            foreground: PromptPlus.Console.ForegroundColor,
            background: PromptPlus.Console.BackgroundColor);

        /// <summary>
        /// Create a new style with default value from Console.
        /// </summary>
        /// <returns>The new <see cref="Style"/></returns>
        public static Style Colors(Color forecolor, Color? backcolor = null) => new(
            foreground: forecolor,
            background: backcolor ?? PromptPlus.Console.BackgroundColor);

        /// <summary>
        /// Gets the foreground Color.
        /// </summary>
        public Color Foreground { get; } = foreground;

        /// <summary>
        /// Gets the background Color.
        /// </summary>
        public Color Background { get; } = background;

        /// <summary>
        /// Gets the Overflow strategy.
        /// </summary>
        public Overflow OverflowStrategy { get; } = overflowStrategy;

        /// <summary>
        /// Combines this style with another one.
        /// </summary>
        /// <param name="other">The item to combine with this.</param>
        /// <returns>A new style representing a combination of this and the other one.</returns>
        public Style Combine(Style other) => new(
            other.Foreground,
            other.Background,
            other.OverflowStrategy == Overflow.None ? OverflowStrategy : other.OverflowStrategy);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Foreground, Background, OverflowStrategy);

        /// <summary>
        /// Checks if two <see cref="Style"/> instances are equal.
        /// </summary>
        /// <param name="other">The Style instance to compare.</param>
        /// <returns><c>true</c> if the two Style are equal, otherwise <c>false</c>.</returns>
        public bool Equals(Style other) => Foreground.Equals(other.Foreground) &&
                                           Background.Equals(other.Background) &&
                                           OverflowStrategy.Equals(other.OverflowStrategy);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Style style && Equals(style);

        /// <summary>
        /// Checks if two <see cref="Style"/> instances are equal.
        /// </summary>
        /// <param name="left">The first Style instance to compare.</param>
        /// <param name="right">The second Style instance to compare.</param>
        /// <returns><c>true</c> if the two Style are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(Style left, Style right) => left.Equals(right);

        /// <summary>
        /// Checks if two <see cref="Style"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first Style instance to compare.</param>
        /// <param name="right">The second Style instance to compare.</param>
        /// <returns><c>true</c> if the two Style are not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(Style left, Style right) => !(left == right);

        /// <summary>
        /// Converts a <see cref="ConsoleColor"/> to a <see cref="Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="ConsoleColor"/> to convert.</param>

        public static implicit operator Style(Color color)
        {
            return Default().ForeGround(color);
        }
    }
}
