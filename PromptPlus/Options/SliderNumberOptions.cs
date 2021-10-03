// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using System;

using PromptPlus.Internal;

namespace PromptPlus.Options
{
    public class SliderNumberOptions<T> : BaseOptions where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        internal int Witdth { get; private set; } = PPlus.SliderWitdth;

        internal SliderNumberType Type { get; set; } = SliderNumberType.LeftRight;

        public int FracionalDig { get; set; } = 0;
        public T Value { get; set; }
        public T Min { get; set; }
        public T Max { get; set; }
        public T ShortStep { get; set; }
        public T? LargeStep { get; set; }

    }
}
