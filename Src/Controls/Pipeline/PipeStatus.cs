// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents the status of the tube
    /// </summary>
    public enum PipeStatus
    {
        /// <summary>
        /// Pipe has executed
        /// </summary>
        Executed,
        /// <summary>
        /// Pipe has Jumped, Not valid condition.
        /// </summary>
        Jumped,
        /// <summary>
        /// Pipe has canceled
        /// </summary>
        Canceled,
        /// <summary>
        /// Pipe not executed
        /// </summary>
        Waiting
    }
}
