// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus
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
        /// <br>When not supported unicode : '-'</br>
        /// </summary>
        SingleBorder,
        /// <summary>
        /// Double Border unicode '═'
        /// <br>When not supported unicode : '='</br>
        /// </summary>
        DoubleBorder,
        /// <summary>
        /// Heavy Border unicode '━'
        /// <br>When not supported unicode : '*'</br>
        /// </summary>
        HeavyBorder
    }

}
