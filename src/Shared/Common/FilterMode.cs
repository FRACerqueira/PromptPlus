// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Filter strategy for filter items in colletion
    /// </summary>
    public enum FilterMode
    {
        /// <summary>
        /// Filter with Contains text
        /// </summary>
        Contains,
        /// <summary>
        /// Filter with start with text
        /// </summary>
        StartsWith,
        /// <summary>
        /// Disabled Filter feature
        /// </summary>
        Disabled
    }
}
