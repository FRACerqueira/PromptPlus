// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the styles for the MultiFile control.
    /// This enum defines various regions or components of the MultiFile control.
    /// </summary>
    public enum MultiFileStyles
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
        /// TaggedInfo Region
        /// </summary>
        TaggedInfo,
        /// <summary>
        /// Tooltips Region
        /// </summary>
        Tooltips,
        /// <summary>
        /// Tree lines Region
        /// </summary>
        Lines,
        /// <summary>
        /// Expand/Collapse symbol Region
        /// </summary>
        ExpandSymbol,
        /// <summary>
        /// Root folder Region
        /// </summary>
        FileRoot,
        /// <summary>
        /// Folder entry Region
        /// </summary>
        FileTypeFolder,
        /// <summary>
        /// File entry Region
        /// </summary>
        FileTypeFile,
        /// <summary>
        /// File size Region
        /// </summary>
        FileSize
    }
}
