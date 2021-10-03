// ********************************************************************************************
// MIT LICENCE
// This project is based on a fork of the Sharprompt project on github.
// The maintenance and evolution is maintained by the PromptPlus project under same MIT license
// ********************************************************************************************

using PromptPlus.Internal;

namespace PromptPlus.Options
{
    public class SliderSwitcheOptions : BaseOptions
    {
        public bool Value { get; set; }
        public string OffValue { get; set; } = Messages.OffValue;
        public string OnValue { get; set; } = Messages.OnValue;
    }
}
