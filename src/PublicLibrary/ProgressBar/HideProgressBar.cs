// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the elements that can be hidden on a ProgressBar.
    /// </summary>
    [Flags]
    public enum HideProgressBar
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

        /// <summary>
        /// Hides the Prompt and Answer.
        /// </summary>        
        PromptAnswer = 4,

        /// <summary>
        /// Hides the elapsed time display.
        /// </summary>
        ElapsedTime = 8,

        /// <summary>
        /// Hides Progressbar at finish
        /// </summary> 
        ProgressbarAtFinish = 16
    }
}
