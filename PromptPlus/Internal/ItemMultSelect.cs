// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusControls.Internal
{
    internal class ItemMultSelect<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }
        public bool Disabled { get; set; }
        public string Group { get; set; }
        public bool IsGroup { get; set; }
        public bool IsLastItem { get; set; }
        public bool IsSelected { get; set; }

    }
}
