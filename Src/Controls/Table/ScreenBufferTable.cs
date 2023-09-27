// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferTable
    {
        public static void WriteFilterTable<T>(this ScreenBuffer screenBuffer, TableOptions<T> options, string input, EmacsBuffer filter)
        {
            if (options.FilterType == FilterMode.StartsWith)
            {
                if (input.StartsWith(filter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    screenBuffer.WriteAnswer(options, input[..filter.Length]);
                    screenBuffer.SaveCursor();
                    screenBuffer.WriteSuggestion(options, input[filter.Length..]);
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
                if (parts.Length == 1 && string.IsNullOrEmpty(parts[0]))
                {
                    screenBuffer.WriteEmptyFilter(options, filter.ToString());
                    screenBuffer.SaveCursor();
                    return;
                }
                var first = true;
                var pos = 0;
                foreach (var itempart in parts)
                {
                    pos++;
                    screenBuffer.WriteSuggestion(options, itempart);
                    if (pos < parts.Length)
                    {
                        screenBuffer.WriteAnswer(options, filter.ToString());
                    }
                    if (first)
                    {
                        first = false;
                        screenBuffer.SaveCursor();
                    }
                }
            }
        }

        public static void WriteLineDescriptionTable<T>(this ScreenBuffer screenBuffer,T input,int rowpos, int colpos, TableOptions<T> options)
        {
            var result = options.OptDescription;
            if (input != null)
            {
                if (options.ChangeDescription != null)
                {
                    result = options.ChangeDescription.Invoke(input,rowpos,colpos);
                }
            }
            if (!string.IsNullOrEmpty(result))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(result, options.OptStyleSchema.Description());
            }
        }

        public static void WriteLineTooltipsTable<T>(this ScreenBuffer screenBuffer, TableOptions<T> options, bool isfiltering, bool ismovecol)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var swm = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipSelect(options, isfiltering, ismovecol);
                    swm = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), swm);
                }
            }
        }

        private static string DefaultToolTipSelect<T>(TableOptions<T> options, bool isfiltering, bool ismovecol)
        {
            if (options.OptEnabledAbortKey)
            {
                if (!isfiltering)
                {
                    if (ismovecol)
                    {
                        return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages,
                            Messages.TooltipSelectFilter,
                            Messages.TableMoveCols);
                    }
                    else
                    {
                        return string.Format("{0}, {1}, {2}, {3}\n{4}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages,
                            Messages.TooltipSelectFilter);
                    }
                }
                else
                {
                    if (options.FilterType != FilterMode.Disabled)
                    {
                        return string.Format("{0}, {1}, {2}, {3}\n{4}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages,
                            Messages.TooltipSelectFilter);
                    }
                    else
                    {
                        return string.Format("{0}, {1}, {2}, {3}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            string.Format(Messages.TooltipCancelEsc, options.Config.AbortKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages);
                    }
                }
            }
            else
            {
                if (!isfiltering)
                {
                    if (ismovecol)
                    {
                        return string.Format("{0}, {1}, {2}\n{3}, {4}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages,
                            Messages.TooltipSelectFilter,
                            Messages.TableMoveCols);
                    }
                    else
                    {
                        return string.Format("{0}, {1}\n{2}, {3}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages,
                            Messages.TooltipSelectFilter);
                    }
                }
                else
                {
                    if (options.FilterType != FilterMode.Disabled)
                    {
                        return string.Format("{0}, {1}\n{2}, {3}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages,
                            Messages.TooltipSelectFilter);
                    }
                    else
                    {
                        return string.Format("{0}, {1}, {2}",
                            string.Format(Messages.TooltipToggle, options.Config.TooltipKeyPress),
                            Messages.SelectFinishEnter,
                            Messages.TooltipPages);
                    }
                }
            }
        }
    }
}
