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
            SpinnerStyle = styleSchema.Prompt().Overflow(Overflow.Crop);
            CurrentFolderStyle = styleSchema.TaggedInfo().Overflow(Overflow.Crop);
            RootStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            SelectedRootStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            LineStyle = styleSchema.Prompt().Overflow(Overflow.Crop);
            SizeStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            ExpandStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            FolderStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            FileStyle = styleSchema.UnSelected().Overflow(Overflow.Crop);
            SelectedFolderStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            SelectedFileStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            SelectedSizeStyle = styleSchema.Selected().Overflow(Overflow.Crop);
            SelectedExpandStyle = styleSchema.Selected().Overflow(Overflow.Crop);
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
        public Style SpinnerStyle { get; set; }
        public Style CurrentFolderStyle { get; set; } 
        public Style RootStyle { get; set; }
        public Style SelectedRootStyle { get; set; }
        public Style LineStyle { get; set; } 
        public Style SizeStyle { get; set; } 
        public Style ExpandStyle { get; set; }
        public Style FolderStyle { get; set; }
        public Style FileStyle { get; set; } 
        public Style SelectedFolderStyle { get; set; }
        public Style SelectedFileStyle { get; set; } 
        public Style SelectedSizeStyle { get; set; }
        public Style SelectedExpandStyle { get; set; }
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
