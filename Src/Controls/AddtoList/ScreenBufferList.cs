// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferList
    {
        public static void WriteLineDescriptionList(this ScreenBuffer screenBuffer, ListOptions options, string input)
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
                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                Messages.TooltipSugestionEsc,
                Messages.TooltipSugestionToggle,
                Messages.TooltipEnterPressList);
            }
            else
            {
                if (baseOptions.SuggestionHandler != null)
                {
                    if (isEditMode)
                    {
                        return string.Format("{0}, {1}, {2}\n{3}, {4}",
                            string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                            Messages.TooltipAbortEdit,
                            Messages.TooltipSugestionToggle,
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
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.TooltipSugestionToggle,
                                    string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                    string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                            else 
                            {
                                return string.Format("{0}, {1}, {2},\n{3}, {4}",
                                     string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                     string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                     Messages.TooltipSugestionToggle,
                                     Messages.TooltipPages,
                                     Messages.TooltipEnterPressList);
                            }
                        }
                        else
                        {
                            if (hasselected)
                            {
                                return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    Messages.TooltipSugestionToggle,
                                    string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                    string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                            else
                            {
                                return string.Format("{0}, {1}\n{2}, {3}",
                                     string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                     Messages.TooltipSugestionToggle,
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
                            string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
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
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                    string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                            else
                            {
                                return string.Format("{0}, {1}\n{2}, {3}",
                                    string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                    string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                                    Messages.TooltipPages,
                                    Messages.TooltipEnterPressList);
                            }
                        }
                        else
                        {
                            if (hasselected)
                            {
                                return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                                string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                Messages.TooltipEnterPressList);
                            }
                            else
                            {
                                return string.Format("{0}\n{1}, {2}",
                                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
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
