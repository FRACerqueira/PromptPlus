// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents a single item in a chart bar visualization with label, value, color, and calculated properties.
    /// </summary>
    /// <param name="id">Unique identifier for the item.</param>
    /// <param name="label">Display label for the item.</param>
    /// <param name="value">Numeric value of the item.</param>
    /// <param name="color">Optional color for the bar representation.</param>
    public sealed class ChartItem(string id, string label, double value, Color? color)
    {
        /// <summary>
        /// Gets the unique identifier for this chart item.
        /// </summary>
        public string Id { get; } = id;

        /// <summary>
        /// Gets the display label for this chart item.
        /// </summary>
        public string Label { get; } = label;

        /// <summary>
        /// Gets the numeric value associated with this chart item.
        /// </summary>
        public double Value { get; } = value;

        /// <summary>
        /// Gets or sets the color used to render the bar for this item.
        /// </summary>
        public Color? Color { get; set; } = color;

        /// <summary>
        /// Gets or sets the calculated percentage this item represents of the total.
        /// </summary>
        public double Percent { get; set; }

        /// <summary>
        /// Gets or sets the style to use when rendering the bar for this item.
        /// </summary>
        public Style? StyleBar { get; set; }
    }
}
