// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus.Controls.Objects;
using System;
using System.Collections.Generic;

namespace PPlus.Controls
{
    internal static class ScreenBufferSliderNumber
    {

        public static void WriteLineWidgetsSliderNumber(this ScreenBuffer screenBuffer, SliderNumberOptions options,int valuestep, double input)
        {
            screenBuffer.NewLine();
            screenBuffer.AddBuffer($"{options.ValueToString(options.Minvalue)} ", options.OptStyleSchema.UnSelected(),true);
            if (options.ChangeColor != null)
            {
                var color = options.ChangeColor(input);
                screenBuffer.AddBuffer(new string(' ', valuestep), Style.Plain.Foreground(color).Background(color),true,false);
            }
            else if (options.Gradient != null)
            {
                var txt = new string(' ', options.Witdth);
                var aux = Gradient(txt,options.Gradient);
                for (int i = 0; i < aux.Length; i++)
                {
                    if (i <= valuestep && valuestep > 0)
                    {
                        screenBuffer.AddBuffer(aux[i].Text, aux[i].Style, true, false);
                    }
                }            
            }
            else
            {
                screenBuffer.AddBuffer(new string(' ', valuestep), options.OptStyleSchema.Slider().Background(options.OptStyleSchema.Slider().Foreground), true, false);
            }
            screenBuffer.AddBuffer(new string(' ', options.Witdth - valuestep), options.OptStyleSchema.Slider(), true, false);
            screenBuffer.AddBuffer($" {options.ValueToString(options.Maxvalue)}", options.OptStyleSchema.UnSelected(),true,false);
        }

        public static void WritePromptSliderNumber(this ScreenBuffer screenBuffer, SliderNumberOptions options)
        {
            screenBuffer.AddBuffer($"{options.OptPrompt}: ", options.OptStyleSchema.Prompt());
            if (options.MoveKeyPress == SliderNumberType.UpDown)
            {
                screenBuffer.AddBuffer($"[{options.Minvalue},{options.Maxvalue}] ",options.OptStyleSchema.Sugestion(),true,false);
            }
        }

        public static void WriteLineDescriptionSliderNumber(this ScreenBuffer screenBuffer, SliderNumberOptions options, double input)
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

        public static void WriteLineTooltipsSliderNumber(this ScreenBuffer screenBuffer, SliderNumberOptions options)
        {
            if (options.OptShowTooltip)
            {
                var tp = options.OptToolTip;
                var swm = false;
                if (string.IsNullOrEmpty(tp))
                {
                    tp = DefaultToolTipSliderNumber(options);
                    swm = true;
                }
                if (!string.IsNullOrEmpty(tp))
                {
                    screenBuffer.NewLine();
                    screenBuffer.AddBuffer(tp, options.OptStyleSchema.Tooltips(), swm);
                }
            }
        }

        private static string DefaultToolTipSliderNumber(SliderNumberOptions baseOptions)
        {
            var msgnav = Messages.SliderNumberLeftRightKeyNavigator;
            if (baseOptions.MoveKeyPress != SliderNumberType.LeftRight)
            {
                msgnav = Messages.SliderNumberUpDownKeyNavigator;
            }
            if (baseOptions.OptEnabledAbortKey)
            {
                return string.Format("{0}, {1}\n{2}",
                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                string.Format(Messages.TooltipCancelEsc, PromptPlus.Config.AbortKeyPress),
                msgnav);
            }
            else
            {
                return string.Format("{0},\n{1}",
                string.Format(Messages.TooltipToggle, PromptPlus.Config.TooltipKeyPress),
                msgnav);
            }
        }

        private static StringStyle[] Gradient(string text, params Color[] colors)
        {
            var result = new List<StringStyle>();
            for (int i = 0; i < text.Length; i++)
            {
                float percentage = (colors.Length - 1) * ((float)i / text.Length);
                int colorPrevIndex = (int)percentage;
                int colorNextIndex = (int)Math.Ceiling(percentage);
                Color colorPrev = colors[colorPrevIndex];
                Color colorNext = colors[colorNextIndex];
                float ltrOffset = percentage - colorPrevIndex;
                float rtlOffset = 1 - ltrOffset;

                byte r = (byte)(rtlOffset * colorPrev.R + ltrOffset * colorNext.R);
                byte g = (byte)(rtlOffset * colorPrev.G + ltrOffset * colorNext.G);
                byte b = (byte)(rtlOffset * colorPrev.B + ltrOffset * colorNext.B);

                var color = new Color(r, g, b);
                result.Add(new StringStyle(text[i].ToString(), new Style(color,color)));
            }
            return result.ToArray();
        }
    }
}
