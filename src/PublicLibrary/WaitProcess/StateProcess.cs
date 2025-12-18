// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading.Tasks;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents The Process state
    /// </summary>
    /// <remarks>
    /// Create The Process state.
    /// </remarks>
    public sealed class StateProcess(TaskMode mode, string id, string? label, TaskStatus status, Exception? exceptionProcess, TimeSpan elapsedtime)
    {
        /// <summary>
        /// Unique ID of Process
        /// </summary>
        public string Id { get; } = id;

        /// <summary>
        /// Name of Process
        /// </summary>
        public string? Label { get; } = label;

        /// <summary>
        /// Dynamic information of Process
        /// </summary>
        internal string? DynamicInfo { get; set; }


        /// <summary>
        /// Task Mode <see cref="TaskMode"/>
        /// </summary>
        public TaskMode RunMode { get; internal set; } = mode;


        /// <summary>
        /// Status of Task <see cref="TaskStatus"/>
        /// </summary>
        public TaskStatus Status { get; internal set; } = status;

        /// <summary>
        /// Exception of Task
        /// </summary>
        public Exception? ExceptionProcess { get; internal set; } = exceptionProcess;

        /// <summary>
        /// Elapsed Time of Task
        /// </summary>
        public TimeSpan ElapsedTime { get; internal set; } = elapsedtime;
    }
}
