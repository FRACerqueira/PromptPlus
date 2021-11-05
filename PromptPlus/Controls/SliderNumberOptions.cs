// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusControls.ValueObjects;

namespace PromptPlusControls.Controls
{
    internal class SliderNumberOptions : BaseOptions
    {
        public int Witdth { get; private set; } = PromptPlus.SliderWitdth;
        public SliderNumberType Type { get; set; } = SliderNumberType.LeftRight;
        public int FracionalDig { get; set; } = 0;
        public double Value { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double ShortStep { get; set; }
        public double? LargeStep { get; set; }

    }
}
