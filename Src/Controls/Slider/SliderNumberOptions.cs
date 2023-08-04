// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;

namespace PPlus.Controls
{
    internal class SliderNumberOptions : BaseOptions
    {
        private const int _defaultSliderWitdth = 40;

        private SliderNumberOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("SliderNumberOptions CTOR NotImplemented");
        }

        internal SliderNumberOptions(StyleSchema styleSchema, Config config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            TimeoutOverwriteDefault = config.HistoryTimeout;
        }
        public SliderBarType BarType { get; set; } = SliderBarType.Fill;
        public Color[] Gradient { get; set; } 
        public Func<double, Color> ChangeColor { get; set; }
        public CultureInfo CurrentCulture { get; set; } = null;
        public double DefaultValue { get; set; }
        public int Witdth { get; set; } = _defaultSliderWitdth;
        public LayoutSliderNumber MoveKeyPress { get; set; } = LayoutSliderNumber.LeftRight;
        public int FracionalDig { get; set; } 
        public double Value { get; set; }
        public double Maxvalue { get; set; }
        public double Minvalue { get; set; }
        public double? ShortStep { get; set; }
        public double? LargeStep { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; }
        public Func<double, string> ChangeDescription { get; set; }

        public string ValueToString(double value)
        {
            var tmp = value.ToString(CurrentCulture);
            var decsep = CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var index = tmp.IndexOf(decsep);
            if (index >= 0)
            {
                var dec = tmp[(index + 1)..];
                if (dec.Length > FracionalDig)
                {
                    dec = dec[..FracionalDig];
                }
                if (dec.Length < FracionalDig)
                {
                    dec += new string('0', FracionalDig - dec.Length);
                }
                if (FracionalDig > 0)
                {
                    tmp = tmp[..index] + decsep + dec;
                }
                else
                {
                    tmp = tmp[..index];
                }
            }
            else
            {
                if (FracionalDig > 0)
                {
                    tmp += decsep + new string('0', FracionalDig);
                }
            }
            return tmp;
        }


    }
}
