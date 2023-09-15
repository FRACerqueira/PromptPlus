// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferList
    {
        public static void WriteLineDescriptionList(this ScreenBuffer screenBuffer, ListOptions options, string _)
        {
            var result = options.OptDescription;
            if (!string.IsNullOrEmpty(result))
            {
                screenBuffer.NewLine();
                screenBuffer.AddBuffer(result, options.OptStyleSchema.Description());
            }
        }

        public static void WriteLineTooltipsList(this ScreenBuffer screenBuffer, ListOptions options, bool isInAutoCompleteMode, bool isEditMode, bool hasselected)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var smk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipList(options, isInAutoCompleteMode, isEditMode, hasselected);
                    smk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), smk);
                }
            }
        }

        private static string DefaultToolTipList(ListOptions baseOptions, bool isInAutoCompleteMode, bool isEditMode, bool hasselected)
        {
            if (isInAutoCompleteMode)
            {
                return string.Format("{0}, {1}\n{2}, {3}",
                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                Messages.TooltipSuggestionEsc,
                Messages.TooltipSuggestionToggle,
                Messages.TooltipEnterPressList);
            }
            else
            {
                if (baseOptions.SuggestionHandler != null)
                {
                    if (isEditMode)
                    {
                        return string.Format("{0}, {1}, {2}\n{3}, {4}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            Messages.TooltipAbortEdit,
                            Messages.TooltipSuggestionToggle,
                            string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                            Messages.TooltipEnterPressList);
                    }
                    else
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            if (hasselected)
                            {
                                return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    Messages.TooltipSuggestionToggle,
                                    string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                    string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                            else 
                            {
                                return string.Format("{0}, {1}, {2},\n{3}, {4}",
                                     string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                     string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                     Messages.TooltipSuggestionToggle,
                                     Messages.TooltipPages,
                                     Messages.TooltipEnterPressList);
                            }
                        }
                        else
                        {
                            if (hasselected)
                            {
                                return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    Messages.TooltipSuggestionToggle,
                                    string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                    string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                            else
                            {
                                return string.Format("{0}, {1}\n{2}, {3}",
                                     string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                     Messages.TooltipSuggestionToggle,
                                     Messages.TooltipPages,
                                     Messages.TooltipEnterPressList);
                            }
                        }
                    }
                }
                else
                {
                    if (isEditMode)
                    {
                        return string.Format("{0}, {1}, {2}\n{3}, {4}",
                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                            Messages.TooltipAbortEdit,
                            string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                            Messages.TooltipPages,
                            Messages.TooltipEnterPressList);
                    }
                    else
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            if (hasselected)
                            {
                                return string.Format("{0}, {1} {2}, {3}\n{4}, {5}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                    string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                            else
                            {
                                return string.Format("{0}, {1}\n{2}, {3}",
                                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                        }
                        else
                        {
                            if (hasselected)
                            {
                                return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                Messages.TooltipEnterPressList);
                            }
                            else
                            {
                                return string.Format("{0}\n{1}, {2}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipPages,
                                Messages.TooltipEnterPressList);
                            }
                        }
                    }
                }
            }
        }
    }
}
