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
    /// Masked Type input
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

    public enum FormatYear
    {
        Long,
        Short,
    }

    public enum FormatWeek
    {
        None,
        Short,
        Long
    }

    public enum FormatTime
    {
        HMS,
        OnlyHM,
        OnlyH
    }

    public enum PromptDateTimeKind
    {
        DateOnly,
        TimeOnly,
        DateTime
    }
}
