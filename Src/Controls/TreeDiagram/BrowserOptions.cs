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

        private BrowserOptions()
        {
            throw new PromptPlusException("BrowserOptions CTOR NotImplemented");
        }

        internal BrowserOptions(bool showcursor) : base(showcursor)
        {
            FixedSelected = new();
        }

        public bool DisabledRecursiveExpand { get; set; }
        public FilterMode FilterType { get; set; } = FilterMode.Contains;
        public List<string> FixedSelected { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; } = int.MaxValue;
        public Func<ItemBrowser, bool> ExpressionSeleted { get; set; } = null;
        public Func<ItemBrowser, bool> ExpressionDisabled { get; set; } = null;
        public Spinners? Spinner { get; set; } = null;
        public Style SpinnerStyle { get; set; } = PromptPlus.StyleSchema.Prompt().Overflow(Overflow.Crop);
        public Style CurrentFolderStyle { get; set; } = PromptPlus.StyleSchema.TaggedInfo().Overflow(Overflow.Crop);
        public Style RootStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style SelectedRootStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public Style LineStyle { get; set; } = PromptPlus.StyleSchema.Prompt().Overflow(Overflow.Crop);
        public Style SizeStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style ExpandStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style FolderStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style FileStyle { get; set; } = PromptPlus.StyleSchema.UnSelected().Overflow(Overflow.Crop);
        public Style SelectedFolderStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public Style SelectedFileStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public Style SelectedSizeStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public Style SelectedExpandStyle { get; set; } = PromptPlus.StyleSchema.Selected().Overflow(Overflow.Crop);
        public bool ExpandAll { get; set; } = false;
        public bool SelectAll { get; set; } = false;
        public Func<ItemBrowser, bool> SelectAllExpression { get; set; } = null;
        public bool ShowLines { get; set; } = true;
        public bool ShowExpand { get; set; } = true;
        public bool OnlyFolders { get; set; } = false;
        public bool ShowSize { get; set; } = true;
        public bool AcceptHiddenAttributes { get; set; } = false;
        public bool AcceptSystemAttributes { get; set; } = false;
        public string SearchFolderPattern { get; set; } = "*";
        public string SearchFilePattern { get; set; } = "*";
        public int PageSize { get; set; } = PromptPlus.Config.PageSize;
        public string RootFolder { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public string DefautPath { get; set; } = null;
        public bool ShowCurrentFolder { get; set; } = true;
        public bool ShowCurrentFullPath { get; set; } = false;
        public HotKey HotKeyTooltipFullPath { get; set; } = PromptPlus.Config.FullPathPress;
        public HotKey HotKeyToggleExpandPress { get; set; } = PromptPlus.Config.ToggleExpandPress;
        public HotKey HotKeyToggleExpandAllPress { get; set; } = PromptPlus.Config.ToggleExpandAllPress;
        public Action<ItemBrowser> BeforeExpanded { get; set; } = null;
        public Action<ItemBrowser> AfterExpanded { get; set; } = null;
        public Action<ItemBrowser> BeforeCollapsed { get; set; } = null;
        public Action<ItemBrowser> AfterCollapsed { get; set; } = null;
    }
}
