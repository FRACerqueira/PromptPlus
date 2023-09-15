// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

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
        internal PromptPlusException(string message) : base(message)
        {
            Platform = RuntimeInformation.OSDescription;
            Framework = RuntimeInformation.FrameworkDescription;
            Version = typeof(PromptPlusException).Assembly.GetName().Version.ToString();
        }

        /// <summary>
        /// Represents an exception thrown by PromptPlus
        /// </summary>
        /// <param name="message">The message exception</param>
        /// <param name="innerexception">The inner exception</param>
        internal PromptPlusException(string message, Exception innerexception) : base(message, innerexception)
        {
            Platform = RuntimeInformation.OSDescription;
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
        /// The Platform running 
        /// </summary>
        public string Platform { get; }

        /// <summary>
        /// Write string exception
        /// </summary>
        /// <returns>The string exception</returns>
        public override string ToString()
        {
            if (PromptPlus.ExtraExceptionInfo)
            {
                return $"{Environment.NewLine} Console/Terminal Inf.: PromptPlus Version:{Version},Framework: {Framework}, Platform{Platform}, IsLegacy: {PromptPlus.IsLegacy}, IsTerminal: {PromptPlus.IsTerminal}, IsUnicodeSupported: {PromptPlus.IsUnicodeSupported}, SupportsAnsi: {PromptPlus.SupportsAnsi},Buffers(Width/Height): {PromptPlus.BufferWidth}/{PromptPlus.BufferHeight},OutputEncoding: {PromptPlus.OutputEncoding.EncodingName},CodePage : {PromptPlus.CodePage}, PadScreen(Left/Right): {PromptPlus.PadLeft}/{PromptPlus.PadRight},Current Buffer: {PromptPlus.CurrentTargetBuffer}, ColorDepth: {PromptPlus.ColorDepth}{Environment.NewLine}{base.ToString()}";
            }
            return base.ToString();
        }
    }
}
