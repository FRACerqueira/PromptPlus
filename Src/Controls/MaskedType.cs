// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    internal enum ControlMaskedType 
    {
        Generic,
        DateOnly,
        TimeOnly,
        DateTime,
        Number,
        Currency
    }

    /// <summary>
    /// Represents Masked Type input
    /// </summary>
    public enum MaskedType
    {
        /// <summary>
        /// <para>Date only.</para> 
        /// </summary>
        DateOnly,
        /// <summary>
        /// <para>Time only.</para> 
        /// </summary>
        TimeOnly,
        /// <summary>
        /// <para>Date and Time.</para> 
        /// </summary>
        DateTime,
        /// <summary>
        /// <para>Only Number.</para> 
        /// </summary>
        Number,
        /// <summary>
        /// <para>Only Currency.</para> 
        /// </summary>
        Currency
    }

    /// <summary>
    /// Represents Format Year input
    /// </summary>
    public enum FormatYear
    {
        /// <summary>
        /// Long format 4 dig.
        /// </summary>
        Long,
        /// <summary>
        /// Short format 2 dig.
        /// </summary>
        Short,
    }

    /// <summary>
    /// Represents Format Year to show
    /// </summary>
    public enum FormatWeek
    {
        /// <summary>
        /// None, not show
        /// </summary>
        None,
        /// <summary>
        /// Short format
        /// </summary>
        Short,
        /// <summary>
        /// Long format
        /// </summary>
        Long
    }

    /// <summary>
    /// Represents Format Time input
    /// </summary>
    public enum FormatTime
    {
        /// <summary>
        /// Hours:Minutes:Seconds
        /// </summary>
        HMS,
        /// <summary>
        /// Hours:Minutes:00
        /// </summary>
        OnlyHM,
        /// <summary>
        /// Hours:00:00
        /// </summary>
        OnlyH
    }
}
