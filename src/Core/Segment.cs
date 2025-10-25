// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Represents a renderable part text.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="Segment"/> class.
    /// </remarks>
    /// <param name="text">The segment text.</param>
    /// <param name="style">The <see cref="Style"/> text.</param>
    internal sealed class Segment(string text, Style style)
    {
        /// <summary>
        /// Gets the segment style.
        /// </summary>
        public Style Style { get; } = style;

        /// <summary>
        /// Gets the segment text.
        /// </summary>
        public string Text { get; } = text.NormalizeNewLines();
    }
}
