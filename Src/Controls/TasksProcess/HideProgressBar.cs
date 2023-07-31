// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the elememt to hide on ProgressBar
    /// </summary>
    [Flags]
    public enum HideProgressBar
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Percent
        /// </summary>
        Percent = 1,
        /// <summary>
        /// Delimit
        /// </summary>
        Delimit = 2,
        /// <summary>
        /// Range
        /// </summary>
        Ranger = 4
    }
}
