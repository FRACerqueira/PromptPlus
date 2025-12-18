// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the elements that can be hidden on a chart.
    /// </summary>
    [Flags]
    public enum HideChart
    {
        /// <summary>
        /// No elements are hidden.
        /// </summary>
        None = 0,

        /// <summary>
        /// Hides the percentage display.
        /// </summary>
        Percentage = 1,

        /// <summary>
        /// Hides the values display.
        /// </summary>
        Values = 2,

        /// <summary>
        /// Hides the Title.
        /// </summary>        
        Title = 4,

        /// <summary>
        /// Hides ordering options.
        /// </summary>     
        Ordering = 8,

        /// <summary>
        /// Hides layout options.
        /// </summary>     
        Layout = 16,

        /// <summary>
        /// Hides Chartbar at finish
        /// </summary> 
        ChartbarAtFinish = 32,
        /// <summary>
        /// Hides hartbar (only legends)
        /// </summary>
        ChartBar = 64
    }
}
