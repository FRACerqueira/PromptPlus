// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PromptPlusLibrary.Controls
{
    /// <summary>
    /// Represents a history manager for storing and saving history entries.
    /// </summary>
    internal sealed class History : IHistory
    {
        private readonly List<ItemHistory> _items = [];
        private readonly string _filename;

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class.
        /// </summary>
        /// <param name="filename">The file name where the history will be saved.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null or empty.</exception>
        public History(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename), "Filename cannot be null or empty.");
            }
            _filename = filename;
        }

        /// <summary>
        /// Adds a new entry to the history.
        /// </summary>
        /// <param name="value">The value to add to the history.</param>
        /// <param name="timeout">Optional timeout for the history entry. Defaults to the default history timeout.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="timeout"/> is negative.</exception>
        /// <returns>The current <see cref="IHistory"/> instance for chaining.</returns>
        public IHistory AddHistory(string value, TimeSpan? timeout = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null or empty.");
            }

            if (timeout.HasValue && timeout.Value < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout cannot be negative.");
            }
            _items.Add(ItemHistory.CreateItemHistory(value, timeout ?? FileHistory.DefaultHistoryTimeout));
            return this;
        }

        public IList<T> ReadHistory<T>()
        {
            var aux = FileHistory.LoadHistory(_filename);
            var result = new List<T>();
            foreach (var item in aux)
            {
                var itemresut = JsonSerializer.Deserialize<T>(item.History!);
                result.Add(itemresut!);
            }
            return result;
        }

        /// <summary>
        /// Removew history to persistent storage.
        /// </summary>
        public void Remove()
        {
            FileHistory.ClearHistory(_filename);
        }

        /// <summary>
        /// Saves the current history to persistent storage.
        /// </summary>
        public void Save()
        {
            FileHistory.SaveHistory(_filename, _items);
        }
    }
}
