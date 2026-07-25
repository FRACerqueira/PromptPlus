// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Defines horizontal navigation behavior for table columns.
    /// </summary>
    public enum HorizontalScrollMode
    {
        /// <summary>
        /// Moves the visible viewport as a full column window.
        /// </summary>
        Full,
        /// <summary>
        /// Scrolls by focusing columns one by one.
        /// </summary>
        Column
    }
}
