// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;

namespace PPlus.Controls
{
    internal static class ScreenBufferMultiSelectBrowser
    {
        public static void WriteFilterBrowserMultiSelect(this ScreenBuffer screenBuffer, BrowserOptions options, string input, EmacsBuffer filter)
        {
            if (options.FilterType == FilterMode.StartsWith)
            {
                if (input.StartsWith(filter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    screenBuffer.WriteAnswer(options, input.Substring(0, filter.Length));
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteSuggestion(options, input.Substring(filter.Length));
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
                    screenBuffer.WriteSuggestion(options, itempart);
                    if (first)
                    {
                        first = false;
                        screenBuffer.SaveCursor();
                    }
                    if (pos < parts.Length)
                    {
                        screenBuffer.WriteAnswer(options, filter.ToString());
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
                screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(),swm);
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
            screenBuffer.AddBuffer(options.Symbol(SymbolType.Selector), options.OptStyleSchema.Selected(), true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.LineStyle, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.SelectedExpandStyle, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.SelectedExpandStyle, true,false);
            if (data.IsRoot)
            {
                screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.SelectedRootStyle, true,false);
            }
            else
            {
                if (data.Value.IsFolder)
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.SelectedFolderStyle, true,false);
                }
                else
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.SelectedFileStyle, true,false);
                }
            }
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.SelectedSizeStyle, true);
        }

        public static void WriteLineNotDisabledMultiSelectorBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(options.Symbol(SymbolType.Selector), options.OptStyleSchema.Selected(), true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.LineStyle, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.OptStyleSchema.Disabled(), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.OptStyleSchema.Disabled(), true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.OptStyleSchema.Disabled(), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.OptStyleSchema.Disabled(), true,false);
        }

        public static void WriteLineNotMultiSelectorBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.LineStyle, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.ExpandStyle, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.ExpandStyle, true,false);
            if (data.IsRoot)
            {
                screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.RootStyle, true,false);
            }
            else
            {
                if (data.Value.IsFolder)
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.FolderStyle, true,false);
                }
                else
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.FileStyle, true,false);
                }
            }
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.SizeStyle, true,false);
        }

        public static void WriteLineDisabledNotMultiSelectorBrowser(this ScreenBuffer screenBuffer, BrowserOptions options, ItemTreeViewFlatNode<ItemBrowser> data)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.LineStyle, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSelected, options.OptStyleSchema.Disabled(), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.OptStyleSchema.Disabled(), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.OptStyleSchema.Disabled(), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextSize, options.OptStyleSchema.Disabled(), true,false);
        }

    }
}
