// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Controls
{
    internal sealed class ItemSelect<T>(string uniqueId, T value, bool disabled, bool valuechecked = false)
    {
        public string UniqueId => uniqueId;
        public T Value => value;
        public bool Disabled => disabled;
        public string? Text { get; set; }
        public string? ExtraText { get; set; }
        public string? Group { get; set; }
        public bool IsFirstItemGroup { get; set; }
        public bool IsLastItemGroup { get; set; }
        public char? CharSeparation { get; set; }
        public bool ValueChecked { get; set; } = valuechecked;
    }
}
