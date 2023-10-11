// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;

namespace PPlus.Controls
{
    internal static class ScreenBufferSliderSwitch
    {
        public static bool WriteLineDescriptionSliderSwitch(this ScreenBuffer screenBuffer, SliderSwitchOptions options, bool input)
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
                return true;
            }
            return false;
        }

        public static void WriteLineWidgetsSliderSwitch(this ScreenBuffer screenBuffer, SliderSwitchOptions options, bool input, bool newline)
        {
            if (newline)
            {
                screenBuffer.NewLine();
            }
            if (!string.IsNullOrEmpty(options.OffValue))
            {
                screenBuffer.AddBuffer($"{options.OffValue} ", options.OptStyleSchema.UnSelected());
            }
            if (input)
            {
                screenBuffer.AddBuffer(new string(' ', options.Witdth / 2), Style.Default.Background(options.StyleStateOn.Background), true, false);
                screenBuffer.AddBuffer(new string(' ', options.Witdth / 2), Style.Default.Background(options.StyleStateOn.Foreground), true, false);
            }
            else
            {
                screenBuffer.AddBuffer(new string(' ', options.Witdth / 2), Style.Default.Background(options.StyleStateOff.Foreground), true, false);
                screenBuffer.AddBuffer(new string(' ', options.Witdth / 2), Style.Default.Background(options.StyleStateOff.Background), true, false);
            }
            if (!string.IsNullOrEmpty(options.OnValue))
            {
                screenBuffer.AddBuffer($" {options.OnValue}", options.OptStyleSchema.UnSelected(), false, false);
            }
        }

        public static void WriteLineTooltipsSliderSwitch(this ScreenBuffer screenBuffer, SliderSwitchOptions options)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var swm = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipSliderSwitch(options);
                    swm = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(),swm);
                }
            }
        }

        private static string DefaultToolTipSliderSwitch(SliderSwitchOptions baseOptions)
        {
            if (baseOptions.OptEnabledAbortKey)
            {
                return string.Format("{0}, {1}, {2}",
                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                    string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                    Messages.SliderSwitchKeyNavigator);
            }
            else
            {
                return string.Format("{0}, {1}",
                    string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                    Messages.SliderSwitchKeyNavigator);
            }
        }
    }
}
