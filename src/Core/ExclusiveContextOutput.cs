// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Core
{
    internal sealed class ExclusiveContextOutput : IDisposable
    {
        private bool _disposed;
        private readonly IConsoleExtend _consoleExtend;
        private readonly bool _skipRelease = true;
        public ExclusiveContextOutput(IConsoleExtend console, bool skipexplusive = true)
        {
            _consoleExtend = console;
            if (skipexplusive)
            {
                if (_consoleExtend.ExclusiveContext.CurrentCount == 1)
                {
                    _consoleExtend.ExclusiveContext.Wait();
                    _skipRelease = false;
                }
            }
            else
            {
                _consoleExtend.ExclusiveContext.Wait();
                _skipRelease = false;
            }
        }

        #region IDisposable

        public void Dispose()
        {
            if (!_disposed)
            {
                if (!_skipRelease)
                {
                    _consoleExtend.ExclusiveContext.Release();
                }
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
