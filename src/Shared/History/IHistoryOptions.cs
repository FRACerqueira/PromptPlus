// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring persisted history behavior, including filtering, limits, expiration, and paging.
    /// </summary>
    public interface IHistoryOptions
    {
        /// <summary>
        /// Sets the minimum number of typed characters required before history suggestions are shown.
        /// The default value is 0.
        /// </summary>
        /// <param name="value">The minimum prefix length. Must be greater than or equal to 1.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for chaining.</returns>
        IHistoryOptions MinPrefixLength(byte value);

        /// <summary>
        /// Sets the maximum number of entries retained in history.
        /// The default value is 255.
        /// </summary>
        /// <param name="value">The maximum number of items. Must be greater than or equal to 1.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> less than 1.</exception>
        IHistoryOptions MaxItems(byte value);

        /// <summary>
        /// Sets the expiration duration applied to newly added history entries.
        /// The default value is 365 days.
        /// </summary>
        /// <param name="value">A positive <see cref="TimeSpan"/> that defines when entries expire.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than one second.</exception>
        IHistoryOptions ExpirationTime(TimeSpan value);

        /// <summary>
        /// Sets the filtering strategy for history suggestions.
        /// Default is <see cref="FilterMode.Contains"/>, which matches entries containing the typed prefix.
        /// </summary>
        /// <param name="value">The filtering strategy to apply.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for chaining.</returns>
        IHistoryOptions FilterType(FilterMode value);

        /// <summary>
        /// Sets the number of history entries displayed per page during history navigation.
        /// The default value is 5.
        /// </summary>
        /// <param name="value">The page size. Must be greater than or equal to 1.</param>
        /// <returns>The current <see cref="IHistoryOptions"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> less than 1.</exception>
        IHistoryOptions PageSize(byte value);
    }
}
