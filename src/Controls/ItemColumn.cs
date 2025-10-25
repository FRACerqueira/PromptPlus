// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PromptPlusLibrary.Controls
{
    internal sealed class ItemColumn<T>
    {
        public required Func<T, object> Field { get; set; }
        public TextAlignment AlignCol { get; set; }
        public int Width { get; set; }
        public int OriginalWidth { get; set; }
        public bool TextCrop { get; set; }
        public int? MaxSlidingLines { get; set; }
        public Func<object, string>? Format { get; set; }
        public string? Title { get; set; }
        public TextAlignment AlignTitle { get; set; }
        public bool TitleReplacesWidth { get; set; }

    }
}
