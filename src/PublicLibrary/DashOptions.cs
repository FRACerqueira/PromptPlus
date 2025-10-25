// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a boder when write line with SingleDash/DoubleDash.
    /// </summary>
    public enum DashOptions
    {
        /// <summary>
        /// Ascii Single Border '-'
        /// </summary>
        AsciiSingleBorder,
        /// <summary>
        /// Ascii Single Border '='
        /// </summary>
        AsciiDoubleBorder,
        /// <summary>
        /// Single Border unicode '─' 
        /// When not supported unicode : '-'
        /// </summary>
        SingleBorder,
        /// <summary>
        /// Double Border unicode '═' 
        /// When not supported unicode : '='
        /// </summary>
        DoubleBorder
    }
}
