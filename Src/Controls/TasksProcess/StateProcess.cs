// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Process state
    /// </summary>
    public readonly struct StateProcess
    {
        /// <summary>
        /// Create a StateProcess
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public StateProcess()
        {
            throw new PromptPlusException("StateProcess CTOR NotImplemented");
        }

        internal StateProcess(string? id, string? description, TaskStatus status,Exception exceptionProcess, TimeSpan elapsedtime, StepMode stepMode)
        {
            Id = id;
            Description = description;
            Status = status;
            StepMode = stepMode;
            ExceptionProcess = exceptionProcess;
            ElapsedTime = elapsedtime;
        }

        /// <summary>
        /// TaskTitle of Task
        /// </summary>
        public string? Id { get; }

        /// <summary>
        /// Description of Task
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Status of Task <see cref="TaskStatus"/>
        /// </summary>
        public TaskStatus Status { get; }

        /// <summary>
        /// Exception of Task
        /// </summary>
        public Exception ExceptionProcess { get; }


        /// <summary>
        /// Step Mode of Task <see cref="StepMode"/>
        /// </summary>
        public StepMode StepMode { get; }

        /// <summary>
        /// Elapsed Time of Task
        /// </summary>
        public TimeSpan ElapsedTime { get; }
    }
}
