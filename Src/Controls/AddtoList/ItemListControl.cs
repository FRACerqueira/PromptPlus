// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal readonly struct ItemListControl
    {
        private readonly string _uniqueId;
        public ItemListControl()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }

        public ItemListControl(string text, bool immutable = false)
        {
            Text = text;
            Immutable = immutable;
        }
        public string Text { get; }
        public bool Immutable { get; }
        internal string UniqueId => _uniqueId;

    }
}
