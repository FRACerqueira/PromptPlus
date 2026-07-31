// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Filter strategy for filter items in table.
    /// </summary>
    public enum FilterTableMode
    {
        /// <summary>
        /// Filter by the answer text (result of <c>TextSelector</c>).
        /// </summary>
        Answer,
        /// <summary>
        /// Filter by the concatenated text of all filterable columns
        /// (columns declared with <c>isFilterable: true</c>).
        /// </summary>
        ColumnFilters,
    }
}
