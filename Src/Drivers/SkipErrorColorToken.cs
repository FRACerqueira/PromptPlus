using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPlus.Drivers
{
    internal class SkipErrorColorToken : IDisposable
    {
        private bool _disposed = false;
        private bool _globalIgnoreMalformedColorToken;

        public SkipErrorColorToken()
        {
            _globalIgnoreMalformedColorToken = PromptPlus.IgnoreErrorColorTokens;
            PromptPlus.IgnoreErrorColorTokens = true;
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
                    PromptPlus.IgnoreErrorColorTokens = _globalIgnoreMalformedColorToken;
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
