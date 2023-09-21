// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal struct ItemItemColumn<T>
    {
        public Func<T, object> Field { get; set; }
        public Alignment AlignCol { get; set; }
        public byte Width { get; set; }
        public bool TextCrop { get; set; }
        public Func<object, string>? Format { get; set; }
        public string Title { get; set; }
        public Alignment AlignTitle { get; set; }
    }
}
