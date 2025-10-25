// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides a fluent API for configuring persisted input history behavior (size, filtering, expiration and paging).
    /// </summary>
    public interface IHistoryOptions
    {
        /// <summary>
        /// Sets the minimum number of input characters required before history suggestions are eligible.
        /// Default value is 3.
        /// </summary>
        /// <param name="value">The minimum prefix length (must be &gt;= 1).</param>
        /// <returns>The same <see cref="IHistoryOptions"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> &lt; 1.</exception>
        IHistoryOptions MinPrefixLength(byte value);

        /// <summary>
        /// Sets the maximum number of entries retained in the history store.
        /// Default value is 255.
        /// </summary>
        /// <param name="value">The maximum item count (must be &gt;= 1).</param>
        /// <returns>The same <see cref="IHistoryOptions"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> &lt; 1.</exception>
        IHistoryOptions MaxItems(byte value);

        /// <summary>
        /// Sets the expiration duration applied to newly added history entries.
        /// Default value is 365 days.
        /// </summary>
        /// <param name="value">A positive <see cref="TimeSpan"/> after which items expire.</param>
        /// <returns>The same <see cref="IHistoryOptions"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than one second.</exception>
        IHistoryOptions ExpirationTime(TimeSpan value);

        /// <summary>
        /// Sets the number of history entries displayed per page in history navigation.
        /// Default value is 5.
        /// </summary>
        /// <param name="value">The page size (must be &gt;= 1).</param>
        /// <returns>The same <see cref="IHistoryOptions"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> &lt; 1.</exception>
        IHistoryOptions PageSize(byte value);
    }
}
