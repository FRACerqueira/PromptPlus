// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading.Tasks;

namespace PPlus.Controls
{
    /// <summary>
    /// Process state
    /// </summary>
    public readonly struct StateProcess
    {
        public StateProcess()
        {
            throw new PromptPlusException("StateProcess CTOR NotImplemented");
        }

        internal StateProcess(string? id, string? description, TaskStatus status,TimeSpan elapsedtime, StepMode stepMode)
        {
            Id = id;
            Description = description;
            Status = status;
            StepMode = stepMode;
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
        /// Step Mode of Task <see cref="StepMode"/>
        /// </summary>
        public StepMode StepMode { get; }

        /// <summary>
        /// Elapsed Time of Task
        /// </summary>
        public TimeSpan ElapsedTime { get; }
    }
}
