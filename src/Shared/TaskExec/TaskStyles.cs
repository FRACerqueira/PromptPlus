// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the styles of the Task control.
    /// This enum defines various regions or components of the Task control.
    /// </summary>
    public enum TaskStyles
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
        /// Tooltips Region
        /// </summary>
        Tooltips,
        /// <summary>
        /// Spinner Region
        /// </summary>
        Spinner,
        /// <summary>
        /// Elapsed time Region
        /// </summary>
        ElapsedTime,
        /// <summary>
        /// Error Region
        /// </summary>
        Error
    }
}
