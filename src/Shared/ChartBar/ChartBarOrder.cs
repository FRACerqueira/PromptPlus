// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Specifies the sorting order for chart bar items.
    /// </summary>
    public enum ChartBarOrder
    {
        /// <summary>
        /// No sorting applied; items appear in insertion order
        /// </summary>
        None,

        /// <summary>
        /// Sort by value in descending order (highest first)
        /// </summary>
        Highest,

        /// <summary>
        /// Sort by value in ascending order (smallest first)
        /// </summary>
        Smallest,

        /// <summary>
        /// Sort by label in ascending alphabetical order (A-Z)
        /// </summary>
        LabelAsc,

        /// <summary>
        /// Sort by label in descending alphabetical order (Z-A)
        /// </summary>
        LabelDesc
    }
}
