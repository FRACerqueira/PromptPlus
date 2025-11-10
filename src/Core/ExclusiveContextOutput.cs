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
            if (_consoleExtend.EnabledExclusiveContext)
            {
                if (skipexplusive)
                {
                    if (_consoleExtend.ExclusiveContext.CurrentCount == 1)
                    {
                        try
                        {
                            _consoleExtend.ExclusiveContext.Wait(console.TokenCancelPress);
                        }
                        catch (OperationCanceledException)
                        {
                            //none
                        }
                        _skipRelease = false;
                    }
                }
                else
                {
                    try
                    {
                        _consoleExtend.ExclusiveContext.Wait(console.TokenCancelPress);
                    }
                    catch (OperationCanceledException)
                    {
                        //none
                    }
                    _skipRelease = false;
                }
            }
            else
            {
                _skipRelease = true;
            }
        }
        #region IDisposable

        public void Dispose()
        {
            if (!_disposed)
            {
                if (!_skipRelease)
                {
                    try
                    {
                        if (_consoleExtend.ExclusiveContext.CurrentCount == 0)
                        {
                            _consoleExtend.ExclusiveContext.Release();
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        //skip when exception
                    }
                }
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
