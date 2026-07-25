// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Flags enumeration specifying which chart elements should be hidden.
    /// </summary>
    [Flags]
    public enum HideChart
    {
        /// <summary>
        /// Show all chart elements
        /// </summary>
        None = 0,

        /// <summary>
        /// Hide the chart title
        /// </summary>
        Title = 1,

        /// <summary>
        /// Hide numeric values in chart bars
        /// </summary>
        Values = 2,

        /// <summary>
        /// Hide percentage values in chart bars
        /// </summary>
        Percentage = 4
    }
}
