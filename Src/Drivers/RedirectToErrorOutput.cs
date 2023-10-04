using System;

namespace PPlus.Drivers
{
    internal class RedirectToErrorOutput : IDisposable
    {
        private bool _disposed = false;
        public RedirectToErrorOutput()
        {
            PromptPlus._consoledrive.WriteToErroOutput = true;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    PromptPlus._consoledrive.WriteToErroOutput = false;
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
