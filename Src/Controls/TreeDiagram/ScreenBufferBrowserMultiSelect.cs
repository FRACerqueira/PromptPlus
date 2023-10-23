// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;

namespace PPlus.Controls
{
    internal static class ScreenBufferBrowserMultiSelect
    {
        public static void WriteFilterBrowserMultiSelect(this ScreenBuffer screenBuffer, BrowserOptions options, string input, EmacsBuffer filter)
        {
            if (options.FilterType == FilterMode.StartsWith)
            {
                if (input.StartsWith(filter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    screenBuffer.WriteFilterMatch(options, input.Substring(0, filter.Length));
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteFilterUnMatch(options, input.Substring(filter.Length));
                }
                else
                {
                    screenBuffer.WriteEmptyFilter(options, filter.ToBackward());
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteEmptyFilter(options, filter.ToForward());
                }
            }
            else
            {
                var parts = input.ToUpperInvariant().Split(filter.ToString().ToUpperInvariant());
                var first = true;
                var pos = 0;
                foreach (var itempart in parts)
                {
                    pos++;
                    screenBuffer.WriteFilterMatch(options, itempart);
                    if (first)
                    {
                        first = false;
                        screenBuffer.SaveCursor();
                    }
                    if (pos < parts.Length)
                    {
                        screenBuffer.WriteFilterUnMatch(options, filter.ToString());
                    }
                }
            }
        }

        public static void WriteLineTooltipsMultiSelectBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var swm = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipMultiSelectBrowser(options, data);
                    swm = true;
                }
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(tp, options.StyleContent(StyleControls.Tooltips),swm);
            }
        }

        private static string DefaultToolTipMultiSelectBrowser(BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            if (options.OptEnabledAbortKey)
            {
                if (data.Value.IsFolder)
                {
                    return string.Format("{0}, {1}, {2}, {3} \n{4}, {5}, {6}\n{7}, {8}, {9}",
                        string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                        Messages.MultiSelectFinishEnter,
                        Messages.TooltipPages,
                        Messages.TooltipSelectFilter,
                        Messages.TooltipPressSpace,
                        string.Format(Messages.TooltipFullPath, options.HotKeyTooltipFullPath),
                        string.Format(Messages.TooltipToggleExpandPress, options.HotKeyToggleExpandPress),
                        string.Format(Messages.TooltipToggleExpandAllPress, options.HotKeyToggleExpandAllPress),
                        Messages.PaginatorHome);
                }
                return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}, {7}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                    Messages.MultiSelectFinishEnter,
                    Messages.TooltipPressSpace,
                    string.Format(Messages.TooltipFullPath, options.HotKeyTooltipFullPath),
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter,
                    Messages.PaginatorHome);
            }
            else
            {
                if (data.Value.IsFolder)
                {
                    return string.Format("{0}, {1}, {2} {3}\n{4}, {5}, {6}, {7}",
                        string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                        Messages.MultiSelectFinishEnter,
                        Messages.TooltipPages,
                        string.Format(Messages.TooltipFullPath, options.HotKeyTooltipFullPath),
                        Messages.TooltipSelectFilter,
                        string.Format(Messages.TooltipToggleExpandPress, options.HotKeyToggleExpandPress),
                        string.Format(Messages.TooltipToggleExpandAllPress, options.HotKeyToggleExpandAllPress),
                        Messages.PaginatorHome);

                }
                return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    Messages.MultiSelectFinishEnter,
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter,
                    string.Format(Messages.TooltipFullPath, options.HotKeyTooltipFullPath),
                    Messages.PaginatorHome);
            }
        }

        internal static string DefaultToolTipMultiSelectLoadRoot(BrowserOptions options)
        {
            if (options.OptEnabledAbortKey)
            {
                return string.Format("{0}, {1}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress));
            }
            else
            {
                return string.Format("{0}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress));
            }

        }

        public static void WriteLineMultiSelectorBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(options.Symbol(SymbolType.Selector), options.StyleContent(StyleControls.Selected), true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.StyleContent(StyleControls.Selected), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.Selected), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.Selected).Overflow(Overflow.Crop), true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.StyleContent(StyleControls.Selected).Overflow(Overflow.Crop), true);
        }

        public static void WriteLineNotDisabledMultiSelectorBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(options.Symbol(SymbolType.Selector), options.StyleContent(StyleControls.Selected), true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.StyleContent(StyleControls.Disabled), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.Disabled), true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.Disabled).Overflow(Overflow.Crop), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.StyleContent(StyleControls.Disabled).Overflow(Overflow.Crop), true,false);
        }

        public static void WriteLineNotMultiSelectorBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.StyleContent(StyleControls.UnSelected), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.TreeViewExpand), true,false);
            if (data.IsRoot)
            {
                screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.TreeViewRoot).Overflow(Overflow.Crop), true,false);
            }
            else
            {
                if (data.Value.IsFolder)
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.BrowserFolder).Overflow(Overflow.Crop), true,false);
                }
                else
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.BrowserFile).Overflow(Overflow.Crop), true,false);
                }
            }
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.StyleContent(StyleControls.BrowserSize).Overflow(Overflow.Crop), true,false);
        }

        public static void WriteLineDisabledNotMultiSelectorBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.StyleContent(StyleControls.Disabled), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.Disabled), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.Disabled).Overflow(Overflow.Crop), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.StyleContent(StyleControls.Disabled).Overflow(Overflow.Crop), true,false);
        }

    }
}
