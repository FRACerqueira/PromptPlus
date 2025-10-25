// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents border options when writing a banner.
    /// </summary>
    public enum BannerDashOptions
    {
        /// <summary>
        /// No border.
        /// </summary>
        None,

        /// <summary>
        /// Border '-' after banner.
        /// </summary>
        AsciiSingleBorderDown,

        /// <summary>
        /// Border '=' after banner. If not unicode supported, writes '='.
        /// </summary>
        AsciiDoubleBorderDown,

        /// <summary>
        /// Border '─' after banner. If not unicode supported, writes '-'.
        /// </summary>
        SingleBorderDown,

        /// <summary>
        /// Border '═' after banner. If not unicode supported, writes '='.
        /// </summary>
        DoubleBorderDown,

        /// <summary>
        /// Border '━' after the banner. If not Unicode supported, writes '*'.
        /// </summary>
        HeavyBorderDown,

        /// <summary>
        /// Border '-' before and after banner.
        /// </summary>
        AsciiSingleBorderUpDown,

        /// <summary>
        /// Border '=' before and after banner. If not unicode supported, writes '='.
        /// </summary>
        AsciiDoubleBorderUpDown,

        /// <summary>
        /// Border '─' before and after banner. If not unicode supported, writes '-'.
        /// </summary>
        SingleBorderUpDown,

        /// <summary>
        /// Border '═' before and after banner. If not unicode supported, writes '='.
        /// </summary>
        DoubleBorderUpDown,

        /// <summary>
        /// Border '━' before and after banner. If not unicode supported, writes '*'.
        /// </summary>
        HeavyBorderUpDown
    }
}
