using System;
using System.Runtime.InteropServices;

namespace PPlus
{
    internal class PromptPlusException : Exception
    {
        private PromptPlusException() : base()
        {
            throw new NotImplementedException("PromptPlusException");
        }

        public PromptPlusException(string message) : base(message)
        {
            Plataform = RuntimeInformation.OSDescription;
            Framework = RuntimeInformation.FrameworkDescription;
            Version = typeof(PromptPlusException).Assembly.GetName().Version.ToString();
        }
        public string Version { get; }
        public string Framework { get; }
        public string Plataform { get; }
    }
}
