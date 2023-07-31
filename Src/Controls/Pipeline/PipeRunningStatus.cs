// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

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

        /// <summary>
        /// Create a PipeRunningStatus. Purpose only for unit testing
        /// </summary>
        /// <param name="pipe">The pipe id</param>
        /// <param name="status">The status</param>
        /// <param name="elapsedtime">The elapsedtime</param>
        public PipeRunningStatus(string pipe, PipeStatus status,TimeSpan elapsedtime)
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
