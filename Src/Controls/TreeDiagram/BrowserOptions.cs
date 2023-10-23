// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal class BrowserOptions : BaseOptions
    {

        private BrowserOptions() : base(null, null, null, true)
        {
            throw new PromptPlusException("BrowserOptions CTOR NotImplemented");
        }

        internal BrowserOptions(StyleSchema styleSchema, ConfigControls config, IConsoleControl console, bool showcursor) : base(styleSchema, config, console, showcursor)
        {
            FixedSelected = new();
            PageSize = config.PageSize;
            HotKeyTooltipFullPath = config.FullPathPress;
            HotKeyToggleExpandPress = config.ToggleExpandPress;
            HotKeyToggleExpandAllPress = config.ToggleExpandAllPress;
        }

        public bool DisabledRecursiveExpand { get; set; }
        public FilterMode FilterType { get; set; } = FilterMode.Contains;
        public List<string> FixedSelected { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; } = int.MaxValue;
        public Func<ItemBrowser, bool> ExpressionSelected { get; set; }
        public Func<ItemBrowser, bool> ExpressionDisabled { get; set; }
        public Spinners? Spinner { get; set; }
        public bool ExpandAll { get; set; }
        public bool SelectAll { get; set; }
        public Func<ItemBrowser, bool> SelectAllExpression { get; set; }
        public bool ShowLines { get; set; } = true;
        public bool ShowExpand { get; set; } = true;
        public bool OnlyFolders { get; set; }
        public bool ShowSize { get; set; } = true;
        public bool AcceptHiddenAttributes { get; set; }
        public bool AcceptSystemAttributes { get; set; }
        public string SearchFolderPattern { get; set; } = "*";
        public string SearchFilePattern { get; set; } = "*";
        public int PageSize { get; set; }
        public string RootFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public string DefautPath { get; set; } = null;
        public bool ShowCurrentFolder { get; set; } = true;
        public bool ShowCurrentFullPath { get; set; }
        public HotKey HotKeyTooltipFullPath { get; set; }
        public HotKey HotKeyToggleExpandPress { get; set; }
        public HotKey HotKeyToggleExpandAllPress { get; set; }
        public Action<ItemBrowser> BeforeExpanded { get; set; }
        public Action<ItemBrowser> AfterExpanded { get; set; }
        public Action<ItemBrowser> BeforeCollapsed { get; set; }
        public Action<ItemBrowser> AfterCollapsed { get; set; }
    }
}
