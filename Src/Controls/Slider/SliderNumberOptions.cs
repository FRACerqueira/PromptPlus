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

        private SliderNumberOptions()
        {
            throw new PromptPlusException("SliderNumberOptions CTOR NotImplemented");
        }

        internal SliderNumberOptions(bool showcursor) : base(showcursor)
        {
        }
        public SliderBarType BarType { get; set; } = SliderBarType.Fill;
        public Color[] Gradient { get; set; } = null;
        public Func<double, Color> ChangeColor { get; set; } = null;
        public CultureInfo CurrentCulture { get; set; } = null;
        public double DefaultValue { get; set; }
        public int Witdth { get; set; } = _defaultSliderWitdth;
        public SliderNumberType MoveKeyPress { get; set; } = SliderNumberType.LeftRight;
        public int FracionalDig { get; set; } = 0;
        public double Value { get; set; }
        public double Maxvalue { get; set; }
        public double Minvalue { get; set; }
        public double? ShortStep { get; set; }
        public double? LargeStep { get; set; }
        public string? OverwriteDefaultFrom { get; set; } = null;
        public TimeSpan TimeoutOverwriteDefault { get; set; } = PromptPlus.Config.HistoryTimeout;
        public Func<double, string> ChangeDescription { get; set; }

        public string ValueToString(double value)
        {
            var tmp = value.ToString(CurrentCulture);
            var decsep = CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var index = tmp.IndexOf(decsep);
            if (index >= 0)
            {
                var dec = tmp.Substring(index + 1);
                if (dec.Length > FracionalDig)
                {
                    dec = dec.Substring(0, FracionalDig);
                }
                if (dec.Length < FracionalDig)
                {
                    dec += new string('0', FracionalDig - dec.Length);
                }
                if (FracionalDig > 0)
                {
                    tmp = tmp.Substring(0, index) + decsep + dec;
                }
                else
                {
                    tmp = tmp.Substring(0, index);
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
