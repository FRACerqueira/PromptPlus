// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus
{
    /// <summary>
    /// Represents the Style : Colors and overflow strategy.
    /// </summary>
    public readonly struct Style : IEquatable<Style>
    {
        internal const string UnicodeEllipsis = "…";
        internal const string AsciiEllipsis = "...";

        /// <summary>
        /// create a new <see cref="Style"/> with default foreground/background colors and none overflow strategy.
        /// </summary>
        public Style()
        {
            Foreground = Color.DefaultForecolor;
            Background = Color.DefaultBackcolor;
            OverflowStrategy = Overflow.None;
        }

        /// <summary>
        /// Create a new instance of <see cref="Style"/> with foreground, default backgroundcolors and overflow strategy.
        /// </summary>
        /// <param name="foreground"><see cref="Color"/> foreground</param>
        /// <param name="overflowStrategy"><see cref="Overflow"/> Strategy</param>
        public Style(Color foreground, Overflow overflowStrategy = Overflow.None)
        {
            Foreground = foreground;
            Background = Default.Background;
            OverflowStrategy = overflowStrategy;
        }

        /// <summary>
        /// Create a new instance of <see cref="Style"/> with foreground/background colors and overflow strategy.
        /// </summary>
        /// <param name="foreground"><see cref="Color"/> foreground</param>
        /// <param name="background"><see cref="Color"/> background</param>
        /// <param name="overflowStrategy"><see cref="Overflow"/> Strategy</param>
        public Style(Color foreground, Color background, Overflow overflowStrategy = Overflow.None)
        {
            Foreground = foreground;
            Background = background;
            OverflowStrategy = overflowStrategy;
        }

        /// <summary>
        /// Gets a <see cref="Style"/> with the default colors and overflow.None
        /// </summary>
        public static Style Default => new(Color.DefaultForecolor, Color.DefaultBackcolor, Overflow.None);

        /// <summary>
        /// Gets a <see cref="Style"/> with the default colors and overflow Crop.
        /// </summary>
        public static Style OverflowCrop => Default.Overflow(Overflow.Crop);

        /// <summary>
        /// Gets a <see cref="Style"/> with the default colors and overflow Ellipsis.
        /// </summary>
        public static Style OverflowEllipsis => Default.Overflow(Overflow.Ellipsis);

        /// <summary>
        /// Gets the foreground color.
        /// </summary>
        public Color Foreground { get; }

        /// <summary>
        /// Gets the background color.
        /// </summary>
        public Color Background { get; }

        /// <summary>
        /// Gets the Overflow strategy.
        /// </summary>
        public Overflow OverflowStrategy { get; }

        /// <summary>
        /// Combines this style with another one.
        /// </summary>
        /// <param name="other">The item to combine with this.</param>
        /// <returns>A new style representing a combination of this and the other one.</returns>
        public Style Combine(Style other)
        {
            var foreground = other.Foreground;
            var background = other.Background;
            return new Style(foreground, background,
                other.OverflowStrategy == Overflow.None ? OverflowStrategy : other.OverflowStrategy);
        }

        private static string Ellipsis(bool unicodesupoorted)
        {
            return unicodesupoorted ? UnicodeEllipsis : AsciiEllipsis;
        }

        internal static string ApplyOverflowStrategy(int offset, int bufferWidth, Overflow overflow, string? value, bool unicodesupoorted)
        {
            if (value is null)
            {
                return null;
            }
            if (value.Length == 0)
            {
                return "";
            }
            switch (overflow)
            {
                case Overflow.None:
                    {
                        return value;
                    }
                case Overflow.Crop:
                    {
                        var max = bufferWidth-1 - (offset + value.Length);
                        if (max >= 0)
                        {
                            return value;
                        }
                        if (value == "\n" || value == Environment.NewLine)
                        {
                            return value;
                        }
                        if (max < 0)
                        {
                            max *= -1;
                        }
                        var aux = value[..(value.Length - max)];
                        return aux;
                    }
                case Overflow.Ellipsis:
                    {
                        var max = bufferWidth-1 - (offset + value.Length);
                        if (max >= 0)
                        {
                            return value;
                        }
                        if (value == "\n" || value == Environment.NewLine)
                        {
                            return value;
                        }
                        if (max < 0)
                        {
                            max *= -1;
                        }
                        if (value.Length <= Ellipsis(unicodesupoorted).Length || value.Length - max - Ellipsis(unicodesupoorted).Length <= 0)
                        {
                            return Ellipsis(unicodesupoorted);
                        }
                        var aux = value.Substring(0, value.Length - max - Ellipsis(unicodesupoorted).Length);
                        return $"{aux}{Ellipsis(unicodesupoorted)}";
                    }
            }
            throw new PromptPlusException($"Overflow : {overflow} Not Implemented");
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Foreground, Background, OverflowStrategy);
        }

        /// <summary>
        /// Checks if two <see cref="Style"/> instances are not equal.
        /// </summary>
        /// <param name="other">The Style instance to compare.</param>
        /// <returns><c>true</c> if the two Style are not equal, otherwise <c>false</c>.</returns>
        public bool Equals(Style other)
        {
            return Foreground.Equals(other.Foreground) &&
                Background.Equals(other.Background) &&
                OverflowStrategy.Equals(other.OverflowStrategy);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Style style && Equals(style);
        }

        /// <summary>
        /// Checks if two <see cref="Style"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first Style instance to compare.</param>
        /// <param name="right">The second Style instance to compare.</param>
        /// <returns><c>true</c> if the two Style are not equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(Style left, Style right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two <see cref="Style"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first Style instance to compare.</param>
        /// <param name="right">The second Style instance to compare.</param>
        /// <returns><c>true</c> if the two Style are not equal, otherwise <c>false</c>.</returns>
        /// <returns><c>true</c> if the two colors are not equal, otherwise <c>false</c>.</returns>
        public static bool operator !=(Style left, Style right)
        {
            return !(left == right);
        }
    }
}
