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
        public Alignment Align { get; set; }
        public byte MinWidth { get; set; }
        public byte MaxWidth { get; set; }
        public bool TextCrop { get; set; }
        public Func<object, string>? Format { get; set; }
    }
}
