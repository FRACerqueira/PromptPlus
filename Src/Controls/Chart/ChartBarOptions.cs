// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Collections.Generic;
using System.Globalization;

namespace PPlus.Controls
{
    internal class ChartBarOptions : BaseOptions
    {
        private const int _defaultSliderWitdth = 80;

        private ChartBarOptions()
        {
            throw new PromptPlusException("ChartBarOptions CTOR NotImplemented");
        }

        internal ChartBarOptions(bool showcursor) : base(showcursor)
        {
            Labels = new();
        }

        public ChartType StartChartType { get; set; } = ChartType.StandBar;
        public ChartType CurrentChartType { get; set; } = ChartType.StandBar;
        public char CharBar => '#';
        public ChartBarType BarType { get; set; } = ChartBarType.Fill;
        public int Witdth { get; set; } = _defaultSliderWitdth;
        public int FracionalDig { get; set; } = 0;
        public CultureInfo CurrentCulture { get; set; } = null;
        public Alignment TitleAligment { get; set; } = Alignment.Left;
        public Style DescriptionStyle { get; set; } = PromptPlus.StyleSchema.Description();
        public Style TitleStyle { get; set; } = PromptPlus.StyleSchema.Chart();
        public Style LabelStyle { get; set; } = PromptPlus.StyleSchema.Chart();
        public Style PercentStyle { get; set; } = PromptPlus.StyleSchema.Chart();
        public Style ValueStyle { get; set; } = PromptPlus.StyleSchema.Chart();
        public Style OrderStyle { get; set; } = PromptPlus.StyleSchema.Tooltips();
        public List<ItemChartBar> Labels { get; set; }
        public ChartOrder CurrentOrder { get; set; } = ChartOrder.None;
        public ChartOrder Order { get; set; } = ChartOrder.None;
        public bool HideInfoOrder { get; set; }
        public bool HidePercentBar { get; set; }
        public bool HideValueBar { get; set; }
        public byte PadLeft { get; set; } = 0;
        public bool CurrentShowLegend { get; set; } = false;
        public bool ShowLegend { get; set; } = false;
        public bool ShowLegendPercent { get; set; } = true;
        public bool ShowLegendValue { get; set; } = true;
        public bool EnabledInteractionUser { get; set; }
        public bool EnabledSwitchType { get; set; } = true;
        public bool EnabledSwitchLegend { get; set; } = true;
        public bool EnabledSwitchOrder { get; set; } = true;
        public int Pagesize { get; set; } = 10;
        public HotKey SwitchType { get; set; } = PromptPlus.Config.ChartBarSwitchTypePress;
        public HotKey SwitchLegend { get; set; } = PromptPlus.Config.ChartBarSwitchLegendPress;
        public HotKey SwitchOrder { get; set; } = PromptPlus.Config.ChartBarSwitchOrderPress;
    }

}