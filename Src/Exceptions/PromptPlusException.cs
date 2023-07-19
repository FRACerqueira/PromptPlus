using System;
using System.Runtime.InteropServices;

namespace PPlus
{
    /// <summary>
    /// Represents an exception thrown by PromptPlus
    /// </summary>
    public class PromptPlusException : Exception
    {
        private PromptPlusException() : base()
        {
            throw new NotImplementedException("PromptPlusException");
        }

        /// <summary>
        /// Represents an exception thrown by PromptPlus
        /// </summary>
        /// <param name="message">The message for exception</param>
        public PromptPlusException(string message) : base(message)
        {
            Plataform = RuntimeInformation.OSDescription;
            Framework = RuntimeInformation.FrameworkDescription;
            Version = typeof(PromptPlusException).Assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// The version of PromptPlus running
        /// </summary>
        public string Version { get; }
        /// <summary>
        /// The version of .net Framework running 
        /// </summary>
        public string Framework { get; }
        /// <summary>
        /// The Plataform running 
        /// </summary>
        public string Plataform { get; }
    }
}
