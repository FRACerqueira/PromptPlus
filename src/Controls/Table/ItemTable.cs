
// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Controls.Table
{
    internal sealed class ItemTable<T>(string uniqueId, T value, bool disabled)
    {
        public string UniqueId => uniqueId;
        public T Value => value;
        public bool Disabled => disabled;
        public string?[] CachedCellValues { get; set; } = [];
        public string? CachedTextSelector { get; set; }
        public string FilterableText { get; set; } = string.Empty;
        public bool ValueChecked { get; set; }
    }
}
