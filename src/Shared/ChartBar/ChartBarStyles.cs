// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Defines the available style types for chart bar components.
    /// </summary>
    public enum ChartBarStyles
    {
        /// <summary>
        /// Style for the prompt text
        /// </summary>
        Prompt,

        /// <summary>
        /// Style for error messages
        /// </summary>
        Error,

        /// <summary>
        /// Style for the selected/highlighted chart item
        /// </summary>
        Selected,

        /// <summary>
        /// Style for chart item labels
        /// </summary>
        ChartLabel,

        /// <summary>
        /// Style for chart numeric values
        /// </summary>
        ChartValue,

        /// <summary>
        /// Style for chart percentage values
        /// </summary>
        ChartPercent,

        /// <summary>
        /// Style for chart title
        /// </summary>
        ChartTitle,

        /// <summary>
        /// Style for the selected/current answer
        /// </summary>
        Answer,

        /// <summary>
        /// Style for pagination information
        /// </summary>
        Pagination
    }
}
