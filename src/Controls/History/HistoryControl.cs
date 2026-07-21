
// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace PromptPlusLibrary.Controls.History
{
    /// <inheritdoc/>
    internal sealed class HistoryControl : IHistory
    {
        private readonly List<ItemHistory> _items = [];
        private readonly string _filename;

        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryControl"/> class.
        /// </summary>
        /// <param name="filename">The file name where the history will be saved.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null or empty.</exception>
        public HistoryControl(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename), "Filename cannot be null or empty.");
            }
            _filename = filename;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Remove()
        {
            FileHistory.ClearHistory(_filename);
        }

        /// <inheritdoc/>
        public void Save()
        {
            FileHistory.SaveHistory(_filename, _items);
        }
    }
}
