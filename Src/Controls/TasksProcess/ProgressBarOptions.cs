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

        private ProgressBarOptions()
        {
            throw new PromptPlusException("ProgressBarOptions CTOR NotImplemented");
        }

        internal ProgressBarOptions(bool showcursor) : base(showcursor)
        {
        }

        public bool ShowPercent { get; set; } = true;
        public bool ShowDelimit { get; set; } = true;
        public bool ShowRanger { get; set; } = true;
        public char CharBar { get; set; } = '#';
        public string Finish { get; set; }
        public Style SpinnerStyle { get; set; } = PromptPlus.StyleSchema.Prompt();
        public Spinners? Spinner { get; set; } = null;
        public Color[] Gradient { get; set; } = null;
        public Func<double, Style> ChangeColor { get; set; } = null;
        public double StartWith { get; set; } = 0;
        public T ValueResult { get; set; } = default;
        public int Witdth { get; set; } = _defaultSliderWitdth;
        public int FracionalDig { get; set; } = 0;
        public double Maxvalue => 100;
        public double Minvalue => 0;
        public CultureInfo CurrentCulture { get; set; } = null;
        public Action<UpdateProgressBar<T>, CancellationToken> UpdateHandler { get; set; }
        public ProgressBarType BarType { get; set; } = ProgressBarType.Fill;
    }
}
