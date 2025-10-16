// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents The Style Wait Process Control
    /// This enum defines various regions or components of the Wait Process Control.
    /// </summary>
    public enum WaitProcessStyles
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
        /// TaggedInfo Region
        /// </summary>
        TaggedInfo,
        /// <summary>
        /// Tooltips Region
        /// </summary>
        Tooltips,
        /// <summary>
        /// Spinner Region
        /// </summary>
        Spinner,
        /// <summary>
        /// ElapsedTime Region
        /// </summary>
        ElapsedTime,
        /// <summary>
        ///  Task Waiting Region
        /// </summary>
        UnSelected,
        /// <summary>
        ///  Task Running Region
        /// </summary>
        Selected,
        /// <summary>
        /// Error Task Region
        /// </summary>
        Error,
    }
}
