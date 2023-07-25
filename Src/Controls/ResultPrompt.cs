// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Result <typeparamref name="T"/> to Controls
    /// </summary>
    /// <typeparam name="T">Typeof return</typeparam>
    public struct ResultPrompt<T>
    {
        /// <summary>
        /// Create a ResultPrompt
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public ResultPrompt()
        {
            throw new PromptPlusException("ResultPrompt CTOR NotImplemented");
        }

        internal static ResultPrompt<T> NullResult()
        {
            return new ResultPrompt<T>(default, false, true, false);
        }

        internal ResultPrompt(T value, bool aborted, bool isrunning = false, bool notrender = false, bool clearLastRender = true)
        {
            Value = value;
            IsAborted = aborted;
            IsRunning = isrunning;
            NotRender = notrender;
            ClearLastRender = clearLastRender;
        }

        internal bool IsRunning { get; }
        internal bool NotRender { get; }
        internal bool ClearLastRender { get; }


        /// <summary>
        /// <typeparamref name="T"/> Value result
        /// </summary>
        public T Value { get; }


        /// <summary>
        /// Control is Aborted. True to aborted; otherwise, false.
        /// </summary>
        public bool IsAborted { get; }

    }
}
