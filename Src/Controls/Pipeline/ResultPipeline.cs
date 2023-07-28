// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Result to Pipeline Controls
    /// </summary>
    /// <typeparam name="T">Typeof return</typeparam>
    public readonly struct ResultPipeline<T>
    {
        /// <summary>
        /// Create a ResultPipeline
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public ResultPipeline()
        {
            throw new PromptPlusException("ResultPipeline CTOR NotImplemented");
        }

        /// <summary>
        /// Create a ResultPipeline.Purpose only for unit testing
        /// </summary>
        /// <param name="conext">The value context</param>
        /// <param name="pipes">The status pipes</param>
        public ResultPipeline(T conext, PipeRunningStatus[] pipes)
        {
            Context = conext;
            Pipes = pipes;
        }

        /// <summary>
        /// Get conext value
        /// </summary>
        public T Context { get; }

        /// <summary>
        /// Get running status of pipeline
        /// </summary>
        public PipeRunningStatus[] Pipes { get; }
    }
}
