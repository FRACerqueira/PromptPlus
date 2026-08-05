// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the Styles Input Control
    /// This enum defines various regions or components of the Input Control.
    /// </summary>
    public enum InputStyles
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
        /// Suggestion Region
        /// </summary>
        Suggestion,
        /// <summary>
        /// Selected state
        /// </summary>
        Selected,
        /// <summary>
        /// UnSelected state
        /// </summary>
        UnSelected,
        /// <summary>
        /// Error Region
        /// </summary>
        Error,
        /// <summary>
        /// Pagination Region
        /// </summary>
        Pagination,
        /// <summary>
        /// Tooltips Region
        /// </summary>
        Tooltips
    }
}
