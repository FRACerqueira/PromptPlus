// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents input behavior
    /// </summary>
    public enum InputBehavior
    {
        /// <summary>
        /// Edit mode. Cursor Skip to the next given entry.
        /// </summary>
        EditSkipToInput = 0,
        /// <summary>
        /// Edit mode. Cursor can move freely. 
        /// </summary>
        EditCursorFreely = 1
    }
}
