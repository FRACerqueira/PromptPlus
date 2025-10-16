// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Core
{
    internal sealed class RedirectToErrorOutput : IDisposable
    {
        private bool _disposed;
        private readonly IConsoleExtend _consoleExtend;
        public RedirectToErrorOutput(IConsoleExtend console)
        {
            _consoleExtend = console;
            _consoleExtend.WriteToErroOutput = true;
        }

        #region IDisposable

        public void Dispose()
        {
            if (!_disposed)
            {
                _consoleExtend.WriteToErroOutput = false;
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
