// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Result to masked Controls
    /// </summary>
    public readonly struct ResultMasked
    {
        /// <summary>
        /// Create a ResultMasked
        /// </summary>
        /// <remarks>
        /// Do not use this constructor!
        /// </remarks>
        public ResultMasked()
        {
            throw new PromptPlusException("ResultMasked CTOR NotImplemented");
        }

        /// <summary>
        /// Create a ResultMasked. Purpose only for unit testing
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueMask"></param>
        public ResultMasked(string value, string valueMask)
        {
            Input = value;
            Masked = valueMask;
        }

        /// <summary>
        /// Get Text without mask
        /// </summary>
        public string Input { get; }

        /// <summary>
        /// Get Text with mask
        /// </summary>
        public string Masked { get; }
    }
}
