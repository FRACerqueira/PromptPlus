// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusControls.Internal;

namespace PromptPlusControls.Controls
{
    internal class SliderSwitcheOptions : BaseOptions
    {
        public bool Value { get; set; }
        public string OffValue { get; set; } = Messages.OffValue;
        public string OnValue { get; set; } = Messages.OnValue;
    }
}
