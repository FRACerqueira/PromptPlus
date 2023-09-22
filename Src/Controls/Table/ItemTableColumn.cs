// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal class ItemItemColumn<T>
    {
        public Func<T, object> Field { get; set; }
        public Alignment AlignCol { get; set; }
        public ushort Width { get; set; }
        public ushort OriginalWidth { get; set; }
        public bool TextCrop { get; set; }
        public int? MaxSlidingLines { get; set; }
        public Func<object, string>? Format { get; set; }
        public string Title { get; set; }
        public Alignment AlignTitle { get; set; }
    }
}
