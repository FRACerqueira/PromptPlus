// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferMaskEditList
    {
        public static void WriteLineDescriptionMaskEditList(this ScreenBuffer screenBuffer, MaskEditListOptions options, string input)
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

        public static void WriteLineTooltipsMaskEditList(this ScreenBuffer screenBuffer, MaskEditListOptions options, bool isInAutoCompleteMode, bool isEditMode, bool hasselected)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var smk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipMaskEditList(options, isInAutoCompleteMode, isEditMode, hasselected);
                    smk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), smk);
                }
            }
        }

        private static string DefaultToolTipMaskEditList(MaskEditListOptions baseOptions, bool isInAutoCompleteMode, bool isEditMode, bool hasselected)
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
                        if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                        {
                            var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                            if (baseOptions.Type == ControlMaskedType.DateTime)
                            {
                                aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                            }
                            return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                Messages.TooltipSuggestionToggle,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipEnterPressList,
                                string.Format(Messages.MaskEditJump, aux));
                        }
                        else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                        {
                            string aux;
                            if (baseOptions.Type == ControlMaskedType.Number)
                            {
                                aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                            }
                            else
                            {
                                aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                            }
                            return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                Messages.TooltipSuggestionToggle,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipEnterPressList,
                                string.Format(Messages.MaskEditJump, aux));
                        }
                        else
                        {
                            return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                Messages.TooltipSuggestionToggle,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipEnterPressList);
                        }
                    }
                    else
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipSuggestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipSuggestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                        if (baseOptions.Type == ControlMaskedType.DateTime)
                                        {
                                            aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                        }
                                        return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                            Messages.TooltipSuggestionToggle,
                                            string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                            string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                            Messages.TooltipPages,
                                            string.Format(Messages.MaskEditJump, aux),
                                            Messages.TooltipEnterPressList);
                                    }
                                    else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                    {
                                        string aux;
                                        if (baseOptions.Type == ControlMaskedType.Number)
                                        {
                                            aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                        }
                                        else
                                        {
                                            aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                        }
                                        return string.Format("{0}, {1}, {2}, {3}, {4}\n{5}, {6}, {7}",
                                            string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                            string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                            Messages.TooltipSuggestionToggle,
                                            string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                            string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                            Messages.TooltipPages,
                                            string.Format(Messages.MaskEditJump, aux),
                                            Messages.TooltipEnterPressList);
                                    }
                                    else
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
                                }
                            }
                            else
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}, {5}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                         Messages.TooltipSuggestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
                                         Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2},\n{3}, {4}, {5}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                         Messages.TooltipSuggestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
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
                        }
                        else
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipSuggestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipSuggestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2}, {3}\n{4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipSuggestionToggle,
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
                                }
                            }
                            else
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         Messages.TooltipSuggestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
                                         Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                         string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                         Messages.TooltipSuggestionToggle,
                                         Messages.TooltipPages,
                                         string.Format(Messages.MaskEditJump, aux),
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
                }
                else
                {
                    if (isEditMode)
                    {
                        if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                        {
                            var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                            if (baseOptions.Type == ControlMaskedType.DateTime)
                            {
                                aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                            }
                            return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                string.Format(Messages.MaskEditJump, aux),
                                Messages.TooltipEnterPressList);
                        }
                        else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                        {
                            string aux;
                            if (baseOptions.Type == ControlMaskedType.Number)
                            {
                                aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                            }
                            else
                            {
                                aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                            }
                            return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                string.Format(Messages.MaskEditJump, aux),
                                Messages.TooltipEnterPressList);
                        }
                        else
                        {
                            return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                Messages.TooltipAbortEdit,
                                string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                Messages.TooltipPages,
                                Messages.TooltipEnterPressList);
                        }
                    }
                    else
                    {
                        if (baseOptions.OptEnabledAbortKey)
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1} {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1} {2}, {3}\n{4}, {5}, {6}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1} {2}, {3}\n{4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
                                }
                            }
                            else
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
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
                        }
                        else
                        {
                            if (hasselected)
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}, {2}\n{3}, {4}, {5}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else
                                {
                                    return string.Format("{0}, {1}, {2}\n{3}, {4}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        string.Format(Messages.TooltipEditItem, baseOptions.EditItemPress),
                                        string.Format(Messages.TooltipRemoveItem, baseOptions.RemoveItemPress),
                                        Messages.TooltipPages,
                                        Messages.TooltipEnterPressList);
                                }
                            }
                            else
                            {
                                if (baseOptions.Type == ControlMaskedType.DateOnly || baseOptions.Type == ControlMaskedType.DateTime)
                                {
                                    var aux = $"'{baseOptions.CurrentCulture.DateTimeFormat.DateSeparator}'";
                                    if (baseOptions.Type == ControlMaskedType.DateTime)
                                    {
                                        aux = aux + " or " + $"'{baseOptions.CurrentCulture.DateTimeFormat.TimeSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
                                        Messages.TooltipEnterPressList);
                                }
                                else if (baseOptions.Type == ControlMaskedType.Number || baseOptions.Type == ControlMaskedType.Currency)
                                {
                                    string aux;
                                    if (baseOptions.Type == ControlMaskedType.Number)
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.NumberDecimalSeparator}'";
                                    }
                                    else
                                    {
                                        aux = $"'{baseOptions.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}'";
                                    }
                                    return string.Format("{0}, {1}\n{2}, {3}",
                                        string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                                        Messages.TooltipPages,
                                        string.Format(Messages.MaskEditJump, aux),
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
}
