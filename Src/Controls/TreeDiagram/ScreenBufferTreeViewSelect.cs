// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;

namespace PPlus.Controls
{
    internal static class ScreenBufferTreeViewSelect
    {
        public static void WriteFilterTreeViewSelect<T>(this ScreenBuffer screenBuffer, TreeViewOptions<T> options, string input, EmacsBuffer filter)
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

        public static void WriteLineTooltipsTreeViewSelect<T>(this ScreenBuffer screenBuffer, TreeViewOptions<T> options, bool isparent)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var swm = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipDefaultToolTipTreeViewSelect<T>(options, isparent);
                    swm = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.StyleContent(StyleControls.Tooltips),swm);
                }
            }
        }

        private static string DefaultToolTipDefaultToolTipTreeViewSelect<T>(TreeViewOptions<T> options, bool isparent)
        {
            if (options.OptEnabledAbortKey)
            {
                if (isparent)
                {
                    return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}, {7}",
                        string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                        Messages.SelectFinishEnter,
                        Messages.TooltipPages,
                        Messages.TooltipSelectFilter,
                        string.Format(Messages.TooltipFullPath, options.HotKeyFullPathNodePress),
                        string.Format(Messages.TooltipToggleExpandPress, options.HotKeyToggleExpandPress),
                        string.Format(Messages.TooltipToggleExpandAllPress, options.HotKeyToggleExpandAllPress));
                }
                return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                    Messages.SelectFinishEnter,
                    string.Format(Messages.TooltipFullPath, options.HotKeyFullPathNodePress),
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter);
            }
            else
            {
                if (isparent)
                {
                    return string.Format("{0}, {1}, {2} {3}\n{4}, {5}, {6}",
                        string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                        Messages.SelectFinishEnter,
                        Messages.TooltipPages,
                        string.Format(Messages.TooltipFullPath, options.HotKeyFullPathNodePress),
                        Messages.TooltipSelectFilter,
                        string.Format(Messages.TooltipToggleExpandPress, options.HotKeyToggleExpandPress),
                        string.Format(Messages.TooltipToggleExpandAllPress, options.HotKeyToggleExpandAllPress));

                }
                return string.Format("{0}, {1}, {2}\n{3}, {4}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    Messages.SelectFinishEnter,
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter,
                    string.Format(Messages.TooltipFullPath, options.HotKeyFullPathNodePress));
            }
        }

        public static void WriteLineSelectorTreeViewSelect<T>(this ScreenBuffer screenBuffer, TreeViewOptions<T> options, ItemTreeViewFlatNode<T> data, bool newline)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer(options.Symbol(SymbolType.Selector), options.StyleContent(StyleControls.Selected), true);
            screenBuffer.AddBuffer(' ', Style.Default, true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.Selected), true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.Selected).Overflow(Overflow.Crop), true, false);
        }

        public static void WriteLineDisabledSelectorTreeViewSelect<T>(this ScreenBuffer screenBuffer, TreeViewOptions<T> options, ItemTreeViewFlatNode<T> data,bool newline)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer(options.Symbol(SymbolType.Selector), options.StyleContent(StyleControls.Selected), true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.Disabled), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.Disabled).Overflow(Overflow.Crop), true, false);
        }

        public static void WriteLineNotSelectorTreeViewSelect<T>(this ScreenBuffer screenBuffer, TreeViewOptions<T> options, ItemTreeViewFlatNode<T> data, bool isparent, bool newline)
        {
            if (newline) 
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.TreeViewExpand), true, false);
            if (data.IsRoot)
            {
                screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.TreeViewRoot).Overflow(Overflow.Crop), true, false);
            }
            else
            {
                if (isparent)
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.TreeViewParent).Overflow(Overflow.Crop), true, false);
                }
                else
                {
                    screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.TreeViewChild).Overflow(Overflow.Crop), true, false);
                }
            }
        }

        public static void WriteLineDisabledNotSelectorTreeViewSelect<T>(this ScreenBuffer screenBuffer, TreeViewOptions<T> options, ItemTreeViewFlatNode<T> data, bool newline)
        {
            if (newline) 
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer(' ', Style.Default, true);
            screenBuffer.AddBuffer(' ', Style.Default, true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextLines, options.StyleContent(StyleControls.Lines), true, false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextExpand, options.StyleContent(StyleControls.Disabled), true,false);
            screenBuffer.AddBuffer(data.MessagesNodes.TextItem, options.StyleContent(StyleControls.Disabled).Overflow(Overflow.Crop), true, false);
        }
    }
}
