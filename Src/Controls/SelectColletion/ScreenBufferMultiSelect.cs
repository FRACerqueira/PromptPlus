// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;

namespace PPlus.Controls
{
    internal static class ScreenBufferMultiSelect
    {
        public static void WriteFilterMultiSelect<T>(this ScreenBuffer screenBuffer, MultiSelectOptions<T> options, string input, EmacsBuffer filter)
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

        public static bool WriteLineDescriptionMultiSelect<T>(this ScreenBuffer screenBuffer, MultiSelectOptions<T> options, ItemMultSelect<T>? input)
        {
            string result = string.Empty;
            if (!options.OptMinimalRender)
            {
                result = options.OptDescription;
            }
            if (input != null)
            {
                if (options.DescriptionSelector != null)
                {
                    result = options.DescriptionSelector.Invoke(input.Value);
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(result, options.OptStyleSchema.Description());
                return true;
            }
            return false;
        }

        public static void WriteLinePaginationMultiSelect(this ScreenBuffer screenBuffer, BaseOptions options,  string message, int tagged)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer($"{options.Symbol(SymbolType.Selected)}: {tagged}, ", options.OptStyleSchema.TaggedInfo(), true);
            screenBuffer.AddBuffer(message, options.OptStyleSchema.Pagination(), true, false);
        }

        public static void WriteLineIndentCheckSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message, bool newline = true)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer($"{options.Symbol(SymbolType.Selector)} {options.Symbol(SymbolType.Selected)} ", options.OptStyleSchema.Selected(), true);
            screenBuffer.AddBuffer(message, options.OptStyleSchema.UnSelected(),true,false);
        }

        public static void WriteLineIndentCheckUnSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message, bool newline = true)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer($"  {options.Symbol(SymbolType.Selected)} ", options.OptStyleSchema.Disabled(), true);
            screenBuffer.AddBuffer(message, options.OptStyleSchema.Disabled(), true, false);
        }

        public static void WriteCheckValueSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                screenBuffer.AddBuffer(message, options.OptStyleSchema.Selected(), true);
            }
        }

        public static void WriteLineIndentCheckNotSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message, bool newline = true)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer($"  {options.Symbol(SymbolType.Selected)} ", options.OptStyleSchema.UnSelected(), true);
            screenBuffer.AddBuffer(message, options.OptStyleSchema.UnSelected(), false, false);
        }

        public static void WriteCheckValueNotSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                screenBuffer.AddBuffer(message, options.OptStyleSchema.UnSelected(), true);
            }
        }

        public static void WriteLineIndentUncheckedDisabled(this ScreenBuffer screenBuffer, BaseOptions options, string message, bool newline = true)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer($"  {options.Symbol(SymbolType.NotSelect)} ", options.OptStyleSchema.Disabled(), true);
            screenBuffer.AddBuffer(message, options.OptStyleSchema.UnSelected(),false,false);
        }

        public static void WriteUncheckedValueDisabled(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                screenBuffer.AddBuffer(message, options.OptStyleSchema.Disabled(), true);
            }
        }

        public static void WriteCheckedValueDisabled(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                screenBuffer.AddBuffer(message, options.OptStyleSchema.Disabled(), true);
            }
        }

        public static void WriteLineIndentUncheckedSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message, bool newline = true)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer($"{options.Symbol(SymbolType.Selector)} {options.Symbol(SymbolType.NotSelect)} ", options.OptStyleSchema.Selected(),true);
            screenBuffer.AddBuffer(message, options.OptStyleSchema.UnSelected(), false, false);
        }
        public static void WriteUncheckedValueSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                screenBuffer.AddBuffer(message, options.OptStyleSchema.Selected(), true);
            }
        }

        public static void WriteLineIndentUncheckedNotSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message, bool newline = true)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            screenBuffer.AddBuffer($"  {options.Symbol(SymbolType.NotSelect)} ", options.OptStyleSchema.UnSelected(),true);
            screenBuffer.AddBuffer(message, options.OptStyleSchema.UnSelected(), false, false);
        }

        public static void WriteUncheckedValueNotSelect(this ScreenBuffer screenBuffer, BaseOptions options, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                screenBuffer.AddBuffer(message, options.OptStyleSchema.UnSelected(), true);
            }
        }

        public static void WriteLineTooltipsMultiSelect<T>(this ScreenBuffer screenBuffer, MultiSelectOptions<T> options)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var swk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipMultiSelect(options);
                    swk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), swk);
                }
            }
        }

        private static string DefaultToolTipMultiSelect<T>(MultiSelectOptions<T> options)
        {
            if (options.OptEnabledAbortKey)
            {
                return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}, {7}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                    Messages.MultiSelectFinishEnter,
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter,
                    Messages.TooltipPressSpace,
                    string.Format(Messages.TooltipSelectAll,options.SelectAllPress),
                    string.Format(Messages.TooltipInvertSelectAll, options.InvertSelectedPress));
            }
            else
            {
                return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}, {6}",
                    string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                    Messages.MultiSelectFinishEnter,
                    Messages.TooltipPages,
                    Messages.TooltipSelectFilter,
                    Messages.TooltipPressSpace,
                    string.Format(Messages.TooltipSelectAll, options.SelectAllPress),
                    string.Format(Messages.TooltipInvertSelectAll, options.InvertSelectedPress));
            }

        }

    }
}
