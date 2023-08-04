// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;

namespace PPlus.Controls
{
    internal class SliderSwitchOptions : BaseOptions
    {
        private const int _defaultSliderWitdth = 6;

        private SliderSwitchOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("SliderSwitchOptions CTOR NotImplemented");
        }
        internal SliderSwitchOptions(StyleSchema styleSchema, Config config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            StyleStateOn = styleSchema.Slider();
            StyleStateOff = styleSchema.Slider();
            TimeoutOverwriteDefault = config.HistoryTimeout;
        }

        public Style StyleStateOn { get; set; }
        public Style StyleStateOff { get; set; }
        public bool DefaultValue { get; set; }
        public int Witdth { get; set; } = _defaultSliderWitdth;
        public string OffValue { get; set; } = null;
        public string OnValue { get; set; } = null;
        public Func<bool, string> ChangeDescription { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; }
    }
}
