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
    /// Represents a text rendering style consisting of a foreground color, background color and an overflow strategy.
    /// </summary>
    /// <remarks>
    /// Use the primary constructor to specify explicit colors and an <see cref="Overflow"/> strategy, or the
    /// helper factory methods (<see cref="Default"/> / <see cref="Colors(Color, Color?)"/>) to derive styles from current console settings.
    /// </remarks>
    /// <param name="foreground">Foreground <see cref="Color"/> used when writing content.</param>
    /// <param name="background">Background <see cref="Color"/> used behind the content.</param>
    /// <param name="overflowStrategy">Overflow handling strategy applied when content exceeds the target width.</param>
    public readonly struct Style(Color foreground, Color background, Overflow overflowStrategy = Overflow.None) : IEquatable<Style>
    {
        internal const char UnicodeEllipsis = '…';
        internal const char AsciiEllipsis = '.';

        /// <summary>
        /// Creates a style using the current console foreground and background colors.
        /// </summary>
        /// <returns>A new <see cref="Style"/> reflecting current console colors.</returns>
        public static Style Default() => new(
            foreground: PromptPlus.Console.ForegroundColor,
            background: PromptPlus.Console.BackgroundColor);

        /// <summary>
        /// Creates a style with an explicit foreground color and an optional background color.
        /// If no background color is provided, the current console background color is used.
        /// </summary>
        /// <param name="forecolor">The foreground <see cref="Color"/>.</param>
        /// <param name="backcolor">Optional background <see cref="Color"/>; if <c>null</c>, console background is used.</param>
        /// <returns>A new <see cref="Style"/> with the specified colors.</returns>
        public static Style Colors(Color forecolor, Color? backcolor = null) => new(
            foreground: forecolor,
            background: backcolor ?? PromptPlus.Console.BackgroundColor);

        /// <summary>
        /// Gets the foreground <see cref="Color"/>.
        /// </summary>
        public Color Foreground { get; } = foreground;

        /// <summary>
        /// Gets the background <see cref="Color"/>.
        /// </summary>
        public Color Background { get; } = background;

        /// <summary>
        /// Gets the <see cref="Overflow"/> strategy applied when content exceeds the available width.
        /// </summary>
        public Overflow OverflowStrategy { get; } = overflowStrategy;

        /// <summary>
        /// Combines this style with another style, taking the other style's colors and (if not <see cref="Overflow.None"/>) its overflow strategy.
        /// </summary>
        /// <param name="other">The style to merge with this instance.</param>
        /// <returns>A new <see cref="Style"/> representing the merged result.</returns>
        public Style Combine(Style other) => new(
            other.Foreground,
            other.Background,
            other.OverflowStrategy == Overflow.None ? OverflowStrategy : other.OverflowStrategy);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Foreground, Background, OverflowStrategy);

        /// <summary>
        /// Determines whether this instance is equal to another <see cref="Style"/>.
        /// </summary>
        /// <param name="other">The style to compare.</param>
        /// <returns><c>true</c> if all components match; otherwise, <c>false</c>.</returns>
        public bool Equals(Style other) => Foreground.Equals(other.Foreground) &&
                                           Background.Equals(other.Background) &&
                                           OverflowStrategy.Equals(other.OverflowStrategy);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Style style && Equals(style);

        /// <summary>
        /// Determines whether two <see cref="Style"/> instances are equal.
        /// </summary>
        /// <param name="left">The first style.</param>
        /// <param name="right">The second style.</param>
        /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Style left, Style right) => left.Equals(right);

        /// <summary>
        /// Determines whether two <see cref="Style"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first style.</param>
        /// <param name="right">The second style.</param>
        /// <returns><c>true</c> if not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Style left, Style right) => !(left == right);

        /// <summary>
        /// Creates a new <see cref="Style"/> from the default console colors replacing only the foreground with the specified color.
        /// </summary>
        /// <param name="color">The foreground <see cref="Color"/> to apply.</param>
        /// <returns>A new <see cref="Style"/> using the specified foreground and current console background.</returns>
        public static implicit operator Style(Color color)
        {
            return Default().ForeGround(color);
        }
    }
}
