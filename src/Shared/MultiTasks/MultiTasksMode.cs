// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Defines how a set of MultiTasks are executed.
    /// </summary>
    public enum MultiTasksMode
    {
        /// <summary>
        /// Tasks are executed one after another, in the order they were added.
        /// </summary>
        Sequential,
        /// <summary>
        /// Tasks are executed concurrently (in parallel).
        /// </summary>
        Parallel
    }

    /// <summary>
    /// Represents the execution state of a single task in the MultiTasks control.
    /// </summary>
    public enum MultiTaskState
    {
        /// <summary>
        /// The task is waiting to be executed.
        /// </summary>
        Waiting,
        /// <summary>
        /// The task is currently running.
        /// </summary>
        Running,
        /// <summary>
        /// The task finished successfully.
        /// </summary>
        Success,
        /// <summary>
        /// The task finished with an error.
        /// </summary>
        Failed
    }
}
