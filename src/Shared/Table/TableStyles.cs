// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents style regions for the Table control.
    /// </summary>
    public enum TableStyles
    {
        /// <summary>
        /// Prompt region.
        /// </summary>
        Prompt,
        /// <summary>
        /// Answer region.
        /// </summary>
        Answer,
        /// <summary>
        /// Description region.
        /// </summary>
        Description,
        /// <summary>
        /// Header text region.
        /// </summary>
        HeaderText,
        /// <summary>
        /// Borders region (outer borders, column separators and header separator).
        /// </summary>
        BorderLines,
        /// <summary>
        /// Selected cell region.
        /// </summary>
        SelectedCell,
        /// <summary>
        /// Unselected cell region.
        /// </summary>
        UnselectedCell,
        /// <summary>
        /// Disabled row region.
        /// </summary>
        DisabledRow,
        /// <summary>
        /// Pagination region.
        /// </summary>
        Pagination,
        /// <summary>
        /// Error region.
        /// </summary>
        Error,
        /// <summary>
        /// Tooltips region.
        /// </summary>
        Tooltips,
        /// <summary>
        /// Tagged information region.
        /// </summary>
        TaggedInfo
    }
}
