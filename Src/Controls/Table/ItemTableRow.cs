// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal class ItemTableRow<T>
    {
        private readonly string _uniqueId;
        public ItemTableRow()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }
        public string UniqueId => _uniqueId;
        public T Value { get; set; }
        public bool Disabled { get; set; }
        public bool IsCheck { get; set; }
    }
}
