// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Result to WaitProcess Controls
    /// </summary>
    /// <typeparam name="T">Typeof return</typeparam>
    public readonly struct ResultWaitProcess<T>
    {
        /// <summary>
        /// Create a ResultPipeline
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public ResultWaitProcess()
        {
            throw new PromptPlusException("ResultWaitProcess CTOR NotImplemented");
        }

        /// <summary>
        /// Create a ResultPipeline.Purpose only for unit testing
        /// </summary>
        /// <param name="conext">The value context</param>
        /// <param name="stateprocess">The state of process</param>
        public ResultWaitProcess(T conext, StateProcess[] stateprocess)
        {
            Context = conext;
            States = stateprocess;
        }

        /// <summary>
        /// Get conext value
        /// </summary>
        public T Context { get; }

        /// <summary>
        /// Get State of process
        /// </summary>
        public StateProcess[] States { get; }
    }
}
