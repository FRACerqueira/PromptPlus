// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents style regions for the MultiTable control.
    /// </summary>
    public enum MultiTableStyles
    {
        /// <summary>
        /// Prompt region.
        /// </summary>
        Prompt,
        /// <summary>
        /// Answer region (shows the selected-items count).
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
        /// Selected (cursor) row region — applies to the checkbox mark and all cell text.
        /// </summary>
        SelectedCell,
        /// <summary>
        /// Unselected row region — applies to the checkbox mark and all cell text.
        /// </summary>
        UnselectedCell,
        /// <summary>
        /// Disabled row region — applies to the checkbox mark and all cell text.
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
        /// Tagged information region (e.g. filter label).
        /// </summary>
        TaggedInfo
    }
}
