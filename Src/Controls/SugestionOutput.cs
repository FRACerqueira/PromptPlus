// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PPlus.Controls
{
    /// <summary>
    /// Represents a Sugestion output struct.
    /// </summary>
    public struct SugestionOutput
    {
        private readonly List<string> _items;
        public SugestionOutput()
        {
            _items = new();
        }

        internal SugestionOutput(IList<string> items)
        {
            _items.AddRange(items);
            Sugestions = new ReadOnlyCollection<string>(_items);
        }

        /// <summary>
        /// Add sugestion
        /// </summary>
        /// <param name="value">text sugestion</param>
        public void Add(string value)
        {
            _items.Add(value);
            Sugestions = new ReadOnlyCollection<string>(_items);
        }

        /// <summary>
        /// Add Enumerable sugestions
        /// </summary>
        /// <param name="items">Enumerable text sugestions</param>
        public void AddRange(IEnumerable<string> items)
        {
            _items.AddRange(items);
            Sugestions = new ReadOnlyCollection<string>(_items);
        }

        internal ReadOnlyCollection<string> Sugestions { get; private set; }
    }
}
