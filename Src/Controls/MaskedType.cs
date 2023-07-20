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
    /// Represents Masked type input
    /// </summary>
    public enum MaskedType
    {
        /// <summary>
        /// Date only. 
        /// </summary>
        DateOnly,
        /// <summary>
        /// Time only. 
        /// </summary>
        TimeOnly,
        /// <summary>
        /// Date and Time. 
        /// </summary>
        DateTime,
        /// <summary>
        /// Only Number. 
        /// </summary>
        Number,
        /// <summary>
        /// Only Currency. 
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
