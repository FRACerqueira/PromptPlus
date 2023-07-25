using System;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the running status of the tube
    /// </summary>
    public readonly struct PipeRunningStatus
    {
        /// <summary>
        /// Create a PipeRunningStatus
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public PipeRunningStatus()
        {
            throw new PromptPlusException("PipeStatus CTOR NotImplemented");
        }

        internal PipeRunningStatus(string pipe, PipeStatus status,TimeSpan elapsedtime)
        {
            Pipe = pipe;
            Status = status;
            Elapsedtime = elapsedtime;
        }

        /// <summary>
        /// Get pipes id
        /// </summary>
        public string Pipe { get; }


        /// <summary>
        /// Get status pipes
        /// </summary>
        public PipeStatus Status { get; }

        /// <summary>
        /// Get Elapsedtime pipe
        /// </summary>
        public TimeSpan Elapsedtime { get; }

    }
}
