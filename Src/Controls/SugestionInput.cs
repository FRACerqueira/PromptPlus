// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Suggestion Input struct.
    /// </summary>
    public readonly struct SuggestionInput
    {
        /// <summary>
        /// Create a SuggestionInput
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public SuggestionInput()
        {
            throw new PromptPlusException("SuggestionInput CTOR NotImplemented");
        }

        internal SuggestionInput(string input, object context)
        {
            Text = input;
            Context = context;
        }

        /// <summary>
        /// Get Suggestion Text input
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Get generic context parameter 
        /// </summary>
        public object Context { get; }
    }
}
