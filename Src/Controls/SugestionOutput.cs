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

        /// <summary>
        /// Create a new instance of SuggestionOutput
        /// </summary>
        /// <returns><see cref="SuggestionOutput"/></returns>
        public static SuggestionOutput Create()
        { 
            return new SuggestionOutput();
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
        /// <returns><see cref="SuggestionOutput"/></returns>
        public SuggestionOutput Add(string value)
        {
            _items.Add(value);
            Suggestions = new ReadOnlyCollection<string>(_items);
            return this;
        }

        /// <summary>
        /// Add Enumerable suggestions
        /// </summary>
        /// <param name="items">Enumerable text suggestions</param>
        /// <returns><see cref="SuggestionOutput"/></returns>
        public SuggestionOutput AddRange(IEnumerable<string> items)
        {
            _items.AddRange(items);
            Suggestions = new ReadOnlyCollection<string>(_items);
            return this;
        }

        internal ReadOnlyCollection<string> Suggestions { get; private set; }
    }
}
