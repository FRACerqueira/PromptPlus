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

        public static void WriteLineWidgetsSliderNumber(this ScreenBuffer screenBuffer, SliderNumberOptions options,int valuestep, double input, bool isunicode, bool newline)
        {
            var bar = ' ';
            switch (options.BarType)
            {
                case SliderBarType.Fill:
                    {
                        if (!isunicode)
                        {
                            bar ='#';
                        }
                    }
                    break;
                case SliderBarType.Light:
                    {
                        bar = '─';
                        if (!isunicode)
                        {
                            bar = '-';
                        }
                    }
                    break;
                case SliderBarType.Heavy:
                    {
                        bar = '━';
                        if (!isunicode)
                        {
                            bar = '=';
                        }
                    }
                    break;
                case SliderBarType.Square:
                    {
                        bar = '■';
                        if (!isunicode)
                        {
                            bar = '#';
                        }
                    }
                    break;
                default:
                    throw new PromptPlusException($"Not implemented {options.BarType}");
            }
            if (newline)
            {
                screenBuffer.NewLine();
            }
            if (!options.HideRanger)
            {
                screenBuffer.AddBuffer($"{options.ValueToString(options.Minvalue)} ", options.OptStyleSchema.Ranger(), true);
            }
            if (options.ChangeColor != null)
            {
                var color = options.ChangeColor(input);
                if (options.BarType == SliderBarType.Fill)
                {
                    screenBuffer.AddBuffer(new string(bar, valuestep), Style.Default.Foreground(color).Background(color), true, false);
                }
                else
                {
                    screenBuffer.AddBuffer(new string(bar, valuestep), Style.Default.Foreground(color), true, false);
                }
            }
            else if (options.Gradient != null)
            {
                var txt = new string(bar, options.Witdth);
                var aux = Gradient(txt,options.Gradient);
                for (int i = 0; i < aux.Length; i++)
                {
                    if (i <= valuestep && valuestep > 0)
                    {
                        if (options.BarType == SliderBarType.Fill)
                        {
                            screenBuffer.AddBuffer(aux[i].Text, aux[i].Style, true, false);
                        }
                        else
                        {
                            screenBuffer.AddBuffer(aux[i].Text, Style.Default.Foreground(aux[i].Style.Foreground), true, false);
                        }
                    }
                }            
            }
            else
            {
                if (options.BarType == SliderBarType.Fill)
                {
                    screenBuffer.AddBuffer(new string(bar, valuestep), options.OptStyleSchema.Slider().Background(options.OptStyleSchema.Slider().Foreground), true, false);
                }
                else
                {
                    screenBuffer.AddBuffer(new string(bar, valuestep), Style.Default.Foreground(options.OptStyleSchema.Slider().Foreground), true, false);
                }
            }
            if (options.BarType == SliderBarType.Fill)
            {
                screenBuffer.AddBuffer(new string(' ', options.Witdth - valuestep), options.OptStyleSchema.Slider(), true, false);
            }
            else
            {
                screenBuffer.AddBuffer(new string(bar, options.Witdth - valuestep), Style.Default.Foreground(options.OptStyleSchema.Slider().Background), true, false);
            }
            if (!options.HideRanger)
            {
                screenBuffer.AddBuffer($" {options.ValueToString(options.Maxvalue)}", options.OptStyleSchema.Ranger(), true, false);
            }
            screenBuffer.AddBuffer($" ({options.ValueToString(input)})", options.OptStyleSchema.Answer(), true, false);
            screenBuffer.SaveCursor();
        }

        public static bool WriteLineDescriptionSliderNumber(this ScreenBuffer screenBuffer, SliderNumberOptions options, double input)
        {
            var result = string.Empty;
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
                return true;
            }
            return false;
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
            if (baseOptions.MoveKeyPress != LayoutSliderNumber.LeftRight)
            {
                msgnav = Messages.SliderNumberUpDownKeyNavigator;
            }
            if (baseOptions.OptEnabledAbortKey)
            {
                return string.Format("{0}, {1}\n{2}",
                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
                string.Format(Messages.TooltipCancelEsc, baseOptions.Config.AbortKeyPress),
                msgnav);
            }
            else
            {
                return string.Format("{0},\n{1}",
                string.Format(Messages.TooltipToggle, baseOptions.Config.TooltipKeyPress),
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
