// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides a fluent API for recording and managing persisted history entries.
    /// </summary>
    public interface IHistory
    {
        /// <summary>
        /// Adds a value to the history.
        /// </summary>
        /// <param name="value">The text/value to store. Ignored if null or empty (implementation dependent).</param>
        /// <param name="timeout">
        /// Optional lifetime for the entry. After the timeout elapses the entry may be pruned
        /// (exact behavior depends on implementation). If <c>null</c>, the entry is durable.
        /// </param>
        /// <returns>The same <see cref="IHistory"/> instance for fluent chaining.</returns>
        IHistory AddHistory(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Persists the in‑memory history set to durable storage.
        /// </summary>
        void Save();

        /// <summary>
        /// Removes the persisted history (e.g. deletes the backing store or clears all entries).
        /// </summary>
        void Remove();
    }
}
