// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents The Suggestion output struct.
    /// </summary>
    public struct SuggestionOutput
    {
        private readonly List<string> _items;

        /// <summary>
        /// Create a empty SuggestionOutput
        /// </summary>
        public SuggestionOutput()
        {
            _items = new();
        }

        internal SuggestionOutput(IList<string> items)
        {
            _items.AddRange(items);
            Suggestions = new ReadOnlyCollection<string>(_items);
        }

        /// <summary>
        /// Add suggestion
        /// </summary>
        /// <param name="value">text suggestion</param>
        public void Add(string value)
        {
            _items.Add(value);
            Suggestions = new ReadOnlyCollection<string>(_items);
        }

        /// <summary>
        /// Add Enumerable suggestions
        /// </summary>
        /// <param name="items">Enumerable text suggestions</param>
        public void AddRange(IEnumerable<string> items)
        {
            _items.AddRange(items);
            Suggestions = new ReadOnlyCollection<string>(_items);
        }

        internal ReadOnlyCollection<string> Suggestions { get; private set; }
    }
}
