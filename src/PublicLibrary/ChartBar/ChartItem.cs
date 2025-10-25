// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a chart item.
    /// </summary>
    public sealed class ChartItem(string id, string label, double value, Color? color = null)
    {
        /// <summary>
        /// Gets the Id of the chart item.
        /// </summary>
        public string Id { get; } = id;

        /// <summary>
        /// Gets the label of the chart item.
        /// </summary>
        public string Label { get; } = label;

        /// <summary>
        /// Gets the value of the chart item.
        /// </summary>
        public double Value { get; } = value;

        /// <summary>
        /// Gets the percent value of the chart item.
        /// </summary>
        public double Percent { get; internal set; }

        /// <summary>
        /// Gets the color of the chart item.
        /// </summary>
        public Color? Color { get; internal set; } = color;

        internal Style? StyleBar { get; set; }
    }
}
