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
    /// Defines ProgressBar UI elements that can be hidden.
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
        /// Hides the prompt and answer.
        /// </summary>
        PromptAnswer = 4,

        /// <summary>
        /// Hides the elapsed time display.
        /// </summary>
        ElapsedTime = 8,

        /// <summary>
        /// Hides the ProgressBar when it finishes.
        /// </summary>
        ProgressbarAtFinish = 16
    }
}
