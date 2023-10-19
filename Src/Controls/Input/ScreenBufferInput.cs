// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferInput
    {
        public static void WriteLineDescriptionInput(this ScreenBuffer screenBuffer, InputOptions options, string input)
        {
            string result = string.Empty;
            if (!options.OptMinimalRender)
            {
                result = options.OptDescription;
            }
            if (options.ChangeDescription != null)
            {
                result = options.ChangeDescription.Invoke(input);
            }
            if (!string.IsNullOrEmpty(result))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(result, options.OptStyleSchema.Description());
            }
        }

        public static void WriteLineTooltipsInput(this ScreenBuffer screenBuffer, InputOptions options, bool isInAutoCompleteMode)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var smk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipInput(options, isInAutoCompleteMode);
                    smk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(),smk);
                }
            }
        }

        private static string DefaultToolTipInput(InputOptions baseOptions, bool isInAutoCompleteMode)
        {
            if (baseOptions.IsSecret)
            {
                if (baseOptions.EnabledViewSecret)
                {
                    if (baseOptions.OptEnabledAbortKey)
                    {
                        return string.Format("{0}, {1}, {2}, {3}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            string.Format(Messages.TooltipViewPassword, baseOptions.SwitchView),
                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                            Messages.InputFinishEnter);
                    }
                    else
                    {
                        return string.Format("{0}, {1}, {2}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            string.Format(Messages.TooltipViewPassword, baseOptions.SwitchView),
                            Messages.InputFinishEnter);
                    }
                }
                else
                {
                    if (baseOptions.OptEnabledAbortKey)
                    {
                        return string.Format("{0}, {1}, {2}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                            Messages.InputFinishEnter);
                    }
                    else
                    {
                        return string.Format("{0}, {1}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            Messages.InputFinishEnter);
                    }
                }
            }
            else
            {
                if (baseOptions.HistoryEnabled)
                {
                    if (baseOptions.ShowingHistory)
                    {
                        return string.Format("{0}, {1}, {2}\n{3}, {4}",
                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                        Messages.TooltipHistoryEsc,
                        Messages.SelectFinishEnter,
                        Messages.TooltipPages,
                        Messages.TooltipHistoryClear);
                    }
                    else
                    {
                        if (isInAutoCompleteMode)
                        {
                            return string.Format("{0}, {1}, {2}\n{3}, {4}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            Messages.TooltipSuggestionEsc,
                            Messages.InputFinishEnter,
                            string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                            Messages.TooltipSuggestionToggle);
                        }
                        else
                        {
                            if (baseOptions.SuggestionHandler != null)
                            {
                                if (baseOptions.OptEnabledAbortKey)
                                {
                                    return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSuggestionToggle);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2}, {3}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter,
                                    string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength),
                                    Messages.TooltipSuggestionToggle);
                                }
                            }
                            else
                            {
                                if (baseOptions.OptEnabledAbortKey)
                                {
                                    return string.Format("{0}, {1}, {2}, {3}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.InputFinishEnter,
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength));
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.InputFinishEnter,
                                        string.Format(Messages.TooltipHistoryToggle, baseOptions.HistoryMinimumPrefixLength));
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (isInAutoCompleteMode)
                    {
                        return string.Format("{0}, {1}, {2}, {3}",
                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                        Messages.TooltipSuggestionEsc,
                        Messages.InputFinishEnter,
                        Messages.TooltipSuggestionToggle);
                    }
                    else
                    {
                        if (baseOptions.SuggestionHandler != null)
                        {
                            if (baseOptions.OptEnabledAbortKey)
                            {
                                return string.Format("{0}, {1}, {2}, {3}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipSuggestionToggle,
                                Messages.InputFinishEnter,
                                string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress));
                            }
                            else
                            {
                                return string.Format("{0}, {1}, {2}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.InputFinishEnter,
                                Messages.TooltipSuggestionToggle);
                            }
                        }
                        else
                        {
                            if (baseOptions.OptEnabledAbortKey)
                            {
                                return string.Format("{0}, {1}, {2}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    Messages.InputFinishEnter);
                            }
                            else
                            {
                                return string.Format("{0}, {1}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    Messages.InputFinishEnter);
                            }
                        }
                    }
                }
            }
        }
    }
}
