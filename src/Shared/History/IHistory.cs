// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for adding, reading, and managing persisted history entries.
    /// </summary>
    public interface IHistory
    {
        /// <summary>
        /// Adds an entry to history.
        /// </summary>
        /// <param name="value">The value to store in history.</param>
        /// <param name="timeout">
        /// Optional lifetime for the entry. After the timeout expires, the entry may be removed
        /// depending on the implementation. If <c>null</c>, no expiration is applied.
        /// </param>
        /// <returns>The current <see cref="IHistory"/> instance for fluent chaining.</returns>
        IHistory AddHistory(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Reads all history entries and deserializes them to the specified type.
        /// </summary>
        /// <typeparam name="T">The type used to deserialize history entries.</typeparam>
        /// <returns>A list of deserialized history entries.</returns>
        IList<T> ReadHistory<T>();

        /// <summary>
        /// Persists in-memory history entries to durable storage.
        /// </summary>
        void Save();

        /// <summary>
        /// Removes persisted history, such as deleting the backing store or clearing all entries.
        /// </summary>
        void Remove();
    }
}
