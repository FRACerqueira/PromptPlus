// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Controls.MultiSelect
{
    internal sealed class ItemMultiSelect<T>(int uniqueId, T value, bool valuechecked, bool disabled)
    {
        public int UniqueId => uniqueId;
        public T Value => value;
        public bool Disabled => disabled;
        public bool ValueChecked { get; set; } = valuechecked;
        public string? Text { get; set; }
        public string? Group { get; set; }
        public bool IsFirstItemGroup { get; set; }
        public bool IsLastItemGroup { get; set; }
        public char? CharSeparation { get; set; }
    }
}
