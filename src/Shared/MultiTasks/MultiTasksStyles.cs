// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the styles of the MultiTasks control.
    /// This enum defines various regions or components of the MultiTasks control.
    /// </summary>
    public enum MultiTasksStyles
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
        /// Pagination Region
        /// </summary>
        Pagination,
        /// <summary>
        /// Spinner Region (shown in the summary while any task is running)
        /// </summary>
        Spinner,
        /// <summary>
        /// Elapsed time Region
        /// </summary>
        ElapsedTime,
        /// <summary>
        /// Task waiting-to-run state Region
        /// </summary>
        WaitingTask,
        /// <summary>
        /// Task running state Region
        /// </summary>
        RunningTask,
        /// <summary>
        /// Task completed-with-success state Region
        /// </summary>
        SuccessTask,
        /// <summary>
        /// Task completed-with-failure state Region
        /// </summary>
        FailedTask,
        /// <summary>
        /// Error Region
        /// </summary>
        Error
    }
}
