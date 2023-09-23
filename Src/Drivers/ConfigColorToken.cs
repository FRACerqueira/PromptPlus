using System;

namespace PPlus.Drivers
{
    internal class ConfigColorToken : IDisposable
    {
        private bool _disposed = false;
        private readonly bool _globalIgnoreMalformedColorToken;

        private ConfigColorToken()
        {
        }

        public ConfigColorToken(bool IgnoreColorTokens)
        {
            _globalIgnoreMalformedColorToken = PromptPlus.IgnoreColorTokens;
            PromptPlus.IgnoreColorTokens = IgnoreColorTokens;
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
                    PromptPlus.IgnoreColorTokens = _globalIgnoreMalformedColorToken;
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
