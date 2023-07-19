// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferAutoComplete
    {
        public static void WriteLineDescriptionAutoComplete(this ScreenBuffer screenBuffer, AutoCompleteOptions options, string input)
        {
            var result = options.OptDescription;
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

        public static void WriteLineTooltipsAutoComplete(this ScreenBuffer screenBuffer, AutoCompleteOptions options, bool isInAutoCompleteMode)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip; 
                var smk = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipAutoComplete(options, isInAutoCompleteMode);
                    smk = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(),smk);
                }
            }
        }

        private static string DefaultToolTipAutoComplete(AutoCompleteOptions baseOptions, bool isInAutoCompleteMode)
        {
            if (isInAutoCompleteMode)
            {
                return string.Format("{0}, {1}, {2}, {3}",
                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                Messages.TooltipSugestionEsc,
                Messages.InputFisnishEnter,
                Messages.TooltipPages);
            }
            else
            {
                if (baseOptions.OptEnabledAbortKey)
                {
                    return string.Format("{0}, {1}, {2}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                        Messages.InputFisnishEnter);
                }
                else
                {
                    return string.Format("{0}, {1}",
                        string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                        Messages.InputFisnishEnter);
                }
            }
        }

    }
}
