// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the Styles MaskEdit Input Control
    /// This enum defines various regions or components of the MaskEdit Input Control.
    /// </summary>
    public enum MaskEditStyles
    {
        /// <summary>
        /// Prompt Region
        /// </summary>
        Prompt,
        /// <summary>
        /// Answer Region
        /// </summary>
        Answer,
        /// <summary>
        /// Description Region
        /// </summary>
        Description,
        /// <summary>
        /// Error Region
        /// </summary>
        Error,
        /// <summary>
        /// TaggedInfo Region
        /// </summary>
        TaggedInfo,
        /// <summary>
        /// Tooltips Region
        /// </summary>
        Tooltips,
        /// <summary>
        /// Mask Negative Region
        /// </summary>
        NegativeValue,
        /// <summary>
        /// Mask Positive Region
        /// </summary>
        PositiveValue,
    }
}
