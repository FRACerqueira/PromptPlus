// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// This code was based on work from https://github.com/spectreconsole/spectre.console
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

        private static readonly Style _initstyle = new(PromptPlus.Console.ForegroundColor, PromptPlus.Console.BackgroundColor,Overflow.None);

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
        /// Gets a <see cref="Style"/> with the default colors and and overflow None.
        /// </summary>
        public static Style Plain { get; internal set; } = new Style(_initstyle.Foreground, _initstyle.Background, _initstyle.OverflowStrategy);

        /// <summary>
        /// Gets a <see cref="Style"/> with the default colors and overflow Crop.
        /// </summary>
        public static Style OverflowCrop => Plain.Overflow(Overflow.Crop);

        /// <summary>
        /// Gets a <see cref="Style"/> with the default colors and overflow Ellipsis.
        /// </summary>
        public static Style OverflowEllipsis => Plain.Overflow(Overflow.Ellipsis);

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
                        var max = bufferWidth - (offset + value.Length);
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
                        var max = bufferWidth - (offset + value.Length);
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
                        if (value.Length <= Ellipsis(unicodesupoorted).Length || value.Length - max - Ellipsis(unicodesupoorted).Length - 1 <= 0)
                        {
                            return Ellipsis(unicodesupoorted);
                        }
                        var aux = value.Substring(0, value.Length - max - Ellipsis(unicodesupoorted).Length -1);
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
