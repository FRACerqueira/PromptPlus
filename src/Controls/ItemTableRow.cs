// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Controls
{
    internal sealed class ItemTableRow<T>
    {
        private readonly string _uniqueId;
        public ItemTableRow()
        {
            _uniqueId = Guid.NewGuid().ToString();
        }
        public string UniqueId => _uniqueId;
        public required T Value { get; set; }
        public bool Disabled { get; set; }
        public bool IsCheck { get; set; }
        public string[] TextColumns { get; set; } = [];
        public string SearchContent { get; set; } = string.Empty;
    }
}
