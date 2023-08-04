// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Globalization;
using System.Threading;

namespace PPlus.Controls
{
    internal class ProgressBarOptions<T> : BaseOptions
    {
        private const int _defaultSliderWitdth = 80;

        private ProgressBarOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("ProgressBarOptions CTOR NotImplemented");
        }

        internal ProgressBarOptions(StyleSchema styleSchema, Config config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            SpinnerStyle = styleSchema.Prompt();
        }

        public bool ShowPercent { get; set; } = true;
        public bool ShowDelimit { get; set; } = true;
        public bool ShowRanger { get; set; } = true;
        public char CharBar { get; set; } = '#';
        public string Finish { get; set; }
        public Style SpinnerStyle { get; set; }
        public Spinners? Spinner { get; set; }
        public Color[] Gradient { get; set; }
        public Func<double, Style> ChangeColor { get; set; }
        public double StartWith { get; set; }
        public T ValueResult { get; set; } = default;
        public int Witdth { get; set; } = _defaultSliderWitdth;
        public int FracionalDig { get; set; }
        public double Maxvalue => 100;
        public double Minvalue => 0;
        public CultureInfo CurrentCulture { get; set; } = null;
        public Action<UpdateProgressBar<T>, CancellationToken> UpdateHandler { get; set; }
        public ProgressBarType BarType { get; set; } = ProgressBarType.Fill;
    }
}
