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

        private ChartBarOptions(): base(null,null, null, true)
        {
            throw new PromptPlusException("ChartBarOptions CTOR NotImplemented");
        }

        internal ChartBarOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        { 
            Labels = new();
            PageSize = config.PageSize;
            SwitchType = config.ChartBarSwitchTypePress;
            SwitchLegend = config.ChartBarSwitchLegendPress;
            SwitchOrder = config.ChartBarSwitchOrderPress;
        }

        public LayoutChart StartChartType { get; set; } = LayoutChart.Standard;
        public LayoutChart CurrentChartType { get; set; } = LayoutChart.Standard;
        public char CharBar => '#';
        public ChartBarType BarType { get; set; } = ChartBarType.Fill;
        public int Witdth { get; set; } = _defaultSliderWitdth;
        public int FracionalDig { get; set; }
        public CultureInfo CurrentCulture { get; set; } = null;
        public Alignment TitleAligment { get; set; } = Alignment.Left;
        public List<ItemChartBar> Labels { get; set; }
        public ChartOrder CurrentOrder { get; set; } = ChartOrder.None;
        public ChartOrder Order { get; set; } = ChartOrder.None;
        public bool HideInfoOrder { get; set; }
        public bool HidePercentBar { get; set; }
        public bool HideValueBar { get; set; }
        public bool CurrentShowLegend { get; set; }
        public bool ShowLegend { get; set; }
        public bool ShowLegendPercent { get; set; } = true;
        public bool ShowLegendValue { get; set; } = true;
        public bool EnabledInteractionUser { get; set; }
        public bool EnabledSwitchType { get; set; } = true;
        public bool EnabledSwitchLegend { get; set; } = true;
        public bool EnabledSwitchOrder { get; set; } = true;
        public int PageSize { get; set; }
        public HotKey SwitchType { get; set; }
        public HotKey SwitchLegend { get; set; }
        public HotKey SwitchOrder { get; set; }
    }

}