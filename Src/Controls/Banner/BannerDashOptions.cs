// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents a boder when write Banner.
    /// </summary>
    public enum BannerDashOptions
    {
        /// <summary>
        /// Nome, No border.
        /// </summary>
        None,
        /// <summary>
        /// Ascii Single Border '-' after banner
        /// </summary>
        AsciiSingleBorderDown,
        /// <summary>
        /// Ascii Double Border '=' after banner. If not unicode supported write '='
        /// </summary>
        AsciiDoubleBorderDown,
        /// <summary>
        /// Single Border '─' after banner. If not unicode supported write '-'
        /// </summary>
        SingleBorderDown,
        /// <summary>
        /// Single Border '═' after banner. If not unicode supported write '='
        /// </summary>
        DoubleBorderDown,
        /// <summary>
        /// Single Border '━' after banner. If not unicode supported write '*'
        /// </summary>
        HeavyBorderDown,
        /// <summary>
        /// Ascii single Border '=' before and after banner
        /// </summary>
        AsciiSingleBorderUpDown,
        /// <summary>
        /// Ascii Double Border '=' before and after banner
        /// </summary>
        AsciiDoubleBorderUpDown,
        /// <summary>
        /// Single Border '─' after banner and after banner. If not unicode supported write '-'
        /// </summary>
        SingleBorderUpDown,
        /// <summary>
        /// Single Border '═' after banner and after banner. If not unicode supported write '='
        /// </summary>
        DoubleBorderUpDown,
        /// <summary>
        /// Single Border '━' after banner. If not unicode supported write '*'
        /// </summary>
        HeavyBorderUpDown
    }
}
