// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the elements that can be hidden on a slider.
    /// </summary>
    [Flags]
    public enum HideSlider
    {
        /// <summary>
        /// No elements are hidden.
        /// </summary>
        None = 0,

        /// <summary>
        /// Hides the delimiters.
        /// </summary>
        Delimit = 1,

        /// <summary>
        /// Hides the range display.
        /// </summary>
        Range = 2,
    }
}
