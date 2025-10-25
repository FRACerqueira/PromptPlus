// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Controls
{
    /// <summary>
    /// Provides a base implementation of <see cref="IHistoryOptions"/> for managing history options.
    /// </summary>
    internal sealed class HistoryOptions(string filename) : IHistoryOptions
    {
        private TimeSpan _expirationTime = TimeSpan.FromDays(365);
        private byte _maxItems = byte.MaxValue;
        private byte _pagesize = 5;
        private byte _minPrefixLength = 3;

        /// <summary>
        /// Gets the minimum prefix length required for history matching. 
        /// </summary>
        public byte MinPrefixLengthValue => _minPrefixLength;

        /// <summary>
        /// Gets the expiration time for history items.
        /// </summary>
        public TimeSpan ExpirationTimeValue => _expirationTime;

        /// <summary>
        /// Gets the file name where the history will be stored.
        /// </summary>
        public string FileNameValue => filename;

        /// <summary>
        /// Gets the maximum number of items to retain in the history.
        /// </summary>
        public byte MaxItemsValue => _maxItems;

        /// <summary>
        /// Gets the number of items to display per page when viewing history.
        /// </summary>
        public byte PageSizeValue => _pagesize;

        /// <inheritdoc />
        public IHistoryOptions ExpirationTime(TimeSpan value)
        {
            if (value.TotalSeconds < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Expiration time must be greater or equal than 1 second.");
            }
            _expirationTime = value;
            return this;
        }

        /// <inheritdoc />
        public IHistoryOptions MaxItems(byte value)
        {
            if (value == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Max items must be greater than zero.");
            }

            _maxItems = value;
            return this;
        }

        /// <inheritdoc />
        public IHistoryOptions MinPrefixLength(byte value)
        {
            if (value == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "MinPrefixLength must be greater than zero.");
            }

            _minPrefixLength = value;
            return this;
        }

        /// <inheritdoc />
        public IHistoryOptions PageSize(byte value)
        {
            if (value == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Page size must be greater than zero.");
            }
            _pagesize = value;
            return this;
        }
    }
}
