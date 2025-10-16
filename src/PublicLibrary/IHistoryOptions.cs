// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Defines a set of options for managing history in the PromptPlus library.
    /// </summary>
    public interface IHistoryOptions
    {
        /// <summary>
        /// Sets the minimum prefix length required for history matching. Defaut value is 3.
        /// Default value is 1.
        /// </summary>
        /// <param name="value">The minimum prefix length.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>

        IHistoryOptions MinPrefixLength(byte value);

        /// <summary>
        /// Sets the maximum number of items to retain in the history. Default value is 255 items.
        /// </summary>
        /// <param name="value">The maximum number of history items.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IHistoryOptions MaxItems(byte value);

        /// <summary>
        /// Sets the expiration time for history items.Default value is 365 days.
        /// </summary>
        /// <param name="value">The duration after which history items expire.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1 second.</exception>
        IHistoryOptions ExpirationTime(TimeSpan value);

        /// <summary>
        /// Sets the number of items to display per page when viewing history.Default value is 5.
        /// </summary>
        /// <param name="value">The number of items per page.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IHistoryOptions PageSize(byte value);
    }
}
