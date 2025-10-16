// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Interface for managing file history operations.
    /// </summary>
    public interface IHistory
    {
        /// <summary>
        /// Adds a new entry to the file history.
        /// </summary>
        /// <param name="value">The value to add to the history.</param>
        /// <param name="timeout">Optional timeout for the history entry.</param>
        IHistory AddHistory(string value, TimeSpan? timeout = null);

        /// <summary>
        /// Saves the current history to persistent storage.
        /// </summary>
        void Save();

        /// <summary>
        /// Remove history to persistent storage.
        /// </summary>
        void Remove();
    }
}
