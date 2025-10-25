// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents The Result <typeparamref name="T"/> to Controls
    /// </summary>
    /// <typeparam name="T">Type of return</typeparam>
    /// <param name="value">The content value.</param>
    /// <param name="aborted">If aborted.</param>
    public readonly struct ResultPrompt<T>(T value, bool aborted)
    {
        /// <summary>
        /// <typeparamref name="T"/> Content result
        /// </summary>
        public T Content => value;

        /// <summary>
        /// Control is Aborted. True to aborted; otherwise, false.
        /// </summary>
        public bool IsAborted => aborted;

        /// <summary>
        /// Deconstructs the <see cref="ResultPrompt{T}"/> into its components.
        /// </summary>
        /// <param name="ContentValue">The value.</param>
        /// <param name="Aborted">If aborted.</param>
        public void Deconstruct(out T ContentValue, out bool Aborted)
        {
            ContentValue = Content;
            Aborted = IsAborted;
        }
    }
}
