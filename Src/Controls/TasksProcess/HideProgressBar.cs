using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Ranger
        /// </summary>
        Ranger = 4
    }
}
