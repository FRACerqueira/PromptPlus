// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Represents an exception thrown by PromptPlus for Press Ctrl+C or Ctrl+Break
    /// </summary>
    internal sealed class PromptPlusException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the PromptPlusException class.
        /// </summary>
        public PromptPlusException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PromptPlusException class with message.
        /// </summary>
        /// <param name="message"></param>
        public PromptPlusException(string message) : base(message)
        {
        }
    }
}
