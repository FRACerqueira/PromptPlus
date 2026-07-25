// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the styles for the generic MultiTree control.
    /// </summary>
    public enum MultiTreeStyles
    {
        /// <summary>Prompt Region.</summary>
        Prompt,
        /// <summary>Answer Region.</summary>
        Answer,
        /// <summary>Description Region.</summary>
        Description,
        /// <summary>Selected cursor state.</summary>
        Selected,
        /// <summary>Unselected cursor state.</summary>
        UnSelected,
        /// <summary>Disabled state.</summary>
        Disabled,
        /// <summary>Error Region.</summary>
        Error,
        /// <summary>Pagination Region.</summary>
        Pagination,
        /// <summary>Checked item count tag.</summary>
        TaggedInfo,
        /// <summary>Tooltips Region.</summary>
        Tooltips,
        /// <summary>Tree lines Region.</summary>
        Lines,
        /// <summary>Expand/Collapse symbol Region.</summary>
        ExpandSymbol,
        /// <summary>Root node Region.</summary>
        Root,
        /// <summary>Node region (container/leaf entries).</summary>
        Node,
        /// <summary>Extra info column rendered next to each node.</summary>
        ChildsCount
    }
}
