// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the text string with style
    /// </summary>
    public struct StringStyle :IEquatable<StringStyle>
    {
        /// <summary>
        /// Create empty StringStyle 
        /// </summary>
        public StringStyle()
        {
            Text = string.Empty;
            Style = Style.Default;
            Width = 0;

        }

        /// <summary>
        /// Create StringStyle with text
        /// </summary>
        ///<param name="text">The text</param>
        public StringStyle(string text)
        {
            Text = text.NormalizeNewLines();
            Style = Style.Default;
            Width = text.GetLengthWidthEastAsian();
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="StringStyle"/> with style default.
        /// </summary>
        /// <param name="value">The string to convert.</param>

        public static implicit operator StringStyle(string value)
        {
            return new StringStyle(value);
        }

        /// <summary>
        /// Create a new instance of String-Style
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="style"><see cref="Style"/> text</param>
        public StringStyle(string text, Style style)
        {
            Text = text.NormalizeNewLines();
            Style = style;
            Width = text.GetLengthWidthEastAsian();
            SkipMarkup = false;
            ClearRestOfLine = true;
        }


        internal StringStyle(string text, Style style, bool skipmarkupparse, bool clearrestofline = true)
        {
            Text = text.NormalizeNewLines();
            Style = style;
            Width = text.GetLengthWidthEastAsian();
            SkipMarkup = skipmarkupparse;
            ClearRestOfLine = clearrestofline;
        }

        /// <summary>
        /// Get/Set Text
        /// </summary>
        public string Text { get;  set; }

        /// <summary>
        /// Get/Set Style/>
        /// </summary>
        public Style Style { get;  set; }

        internal int Width { get; }
        internal bool SaveCursor { get; set; }
        internal bool SkipMarkup { get; set; }
        internal bool ClearRestOfLine { get; set; }

        /// <summary>
        /// Checks if two <see cref="StringStyle"/> instances are equal.
        /// </summary>
        /// <param name="other">The StringStyle to compare.</param>
        /// <returns><c>true</c> if the two StringStyle are equal, otherwise <c>false</c>.</returns>
        public readonly bool Equals(StringStyle other)
        {
            return Text == other.Text && Style == other.Style;
        }

        /// <summary>
        /// Checks if two <see cref="StringStyle"/> instances are equal.
        /// </summary>
        /// <param name="left">The first StringStyle instance to compare.</param>
        /// <param name="right">The second StringStyle instance to compare.</param>
        /// <returns><c>true</c> if the two StringStyle are equal, otherwise <c>false</c>.</returns>
        public static bool operator ==(StringStyle left, StringStyle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two <see cref="StringStyle"/> instances are different.
        /// </summary>
        /// <param name="left">The first StringStyle instance to compare.</param>
        /// <param name="right">The second StringStyle instance to compare.</param>
        /// <returns><c>true</c> if the two StringStyle are different, otherwise <c>false</c>.</returns>
        public static bool operator !=(StringStyle left, StringStyle right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override readonly bool Equals(object obj)
        {
            return obj is StringStyle prompt && Equals(prompt);
        }

        /// <inheritdoc/>
        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Text, Style);
        }
    }
}
