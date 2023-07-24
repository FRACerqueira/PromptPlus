// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Result to ProgessBar Controls
    /// </summary>
    /// <typeparam name="T">Typeof return</typeparam>
    public readonly struct ResultProgessBar<T>
    {
        /// <summary>
        /// Create a ResultProgessBar
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public ResultProgessBar()
        {
            throw new PromptPlusException("ResultProgessBar CTOR NotImplemented");
        }

        internal ResultProgessBar(T conext ,double lastvalue)
        {
            Context = conext;
            Lastvalue = lastvalue;
        }

        /// <summary>
        /// Get conext Result
        /// </summary>
        public T Context { get; }

        /// <summary>
        /// Get last value progress
        /// </summary>
        public double Lastvalue { get; }
    }
}
