// **************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary.Controls;
using PromptPlusLibrary.Controls.Calendar;
using PromptPlusLibrary.Controls.ChartBar;
using PromptPlusLibrary.Controls.Slider;
using PromptPlusLibrary.Controls.Switch;
using PromptPlusLibrary.Controls.TableSelect;
using PromptPlusLibrary.Core;
using PromptPlusLibrary.Widgets.Banner;
using System;

namespace PromptPlusLibrary.Widgets
{
    internal sealed class PromptPlusWidgets(IConsoleExtend console, PromptConfig promptConfig) : IWidgets
    {
        public ISwitchWidget Switch(bool value, string? onValue = null, string? offValue = null)
        {
            SwitchControl ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            ctrl.InternalDefault(value);
            if (!string.IsNullOrEmpty(offValue))
            {
                ctrl.InternalOffValue(offValue);
            }
            if (!string.IsNullOrEmpty(onValue))
            {
                ctrl.InternalOnValue(onValue);
            }
            return ctrl;
        }

        public ISliderWidget Slider(double value, double minvalue = 0, double maxvalue = 100, byte fracionaldig = 2)
        {
            SliderControl ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            ctrl.InternalFracionalDig(fracionaldig);
            ctrl.InternalRange(minvalue, maxvalue);
            ctrl.InternalDefault(value);
            return ctrl;
        }

        public ITableWidget<T> Table<T>() where T : class
        {
            return new TableSelectControl<T>(true, console, promptConfig, new BaseControlOptions(promptConfig));
        }

        public IChartBarWidget ChartBar(string title, TextAlignment titleAlignment = TextAlignment.Center, bool showlegends = false)
        {
            ChartBarControl ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            ctrl.InternalTitle(title, titleAlignment);
            ctrl.InternalShowLegends(showlegends);
            return ctrl;
        }

        public ICalendarWidget Calendar(DateTime dateref)
        {
            CalendarControl ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            ctrl.InternalDefault(dateref);
            return ctrl;
        }

        public IBanner Banner(string value, Style? style = null)
        {
            return new BannerWidget(console, promptConfig, value, style ?? Style.Default());
        }

        public void SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null, bool applycolorbackground = false)
        {
            using (console.InternalExclusiveContext())
            {
                SingleDoubleDash((IConsole)console, promptConfig, false, value, dashOptions, extralines, style, applycolorbackground);
            }
        }

        public void DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null, bool applycolorbackground = false)
        {
            using (console.InternalExclusiveContext())
            {
                SingleDoubleDash((IConsole)console, promptConfig, true, value, dashOptions, extralines, style, applycolorbackground);
            }
        }

        private static void SingleDoubleDash(IConsole console, PromptConfig config, bool doubleDash, string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null, bool applycolorbackground = false)
        {
            Style originalstyle = new(console.ForegroundColor, console.BackgroundColor);
            Style localstyle = style ?? new Style(console.ForegroundColor, console.BackgroundColor);

            char wrapperChar = dashOptions switch
            {
                DashOptions.AsciiSingleBorder => config.GetSymbol(SymbolType.SingleBorder, false)[0],
                DashOptions.AsciiDoubleBorder => config.GetSymbol(SymbolType.DoubleBorder, false)[0],
                DashOptions.SingleBorder => config.GetSymbol(SymbolType.SingleBorder, console.IsUnicodeSupported)[0],
                DashOptions.DoubleBorder => config.GetSymbol(SymbolType.DoubleBorder, console.IsUnicodeSupported)[0],
                _ => throw new NotImplementedException($"dashOptions : {dashOptions} Not Implemented")
            };
            int maxlength = 0;
            string[] parts = value.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (part.Length > maxlength)
                {
                    bool tokencolor = value.ToSegment(localstyle, console) != null;
                    maxlength = tokencolor ? part.LengthTokenColor() : part.Length;
                }
            }
            if (doubleDash)
            {
                if (!applycolorbackground)
                {
                    console.Write("", originalstyle, true);
                    console.WriteLine(new string(wrapperChar, maxlength), localstyle, false);
                }
                else
                {
                    console.WriteLine(new string(wrapperChar, maxlength), localstyle);
                }
            }
            if (!applycolorbackground)
            {
                console.Write("", originalstyle, true);
                console.WriteLine(value, localstyle, false);
                console.Write("", originalstyle, true);
                console.WriteLine(new string(wrapperChar, maxlength), localstyle, false);
            }
            else
            {
                console.WriteLine(new string(wrapperChar, maxlength), localstyle);
                console.WriteLine(value, localstyle);
            }
            if (originalstyle != localstyle)
            {
                console.Write("", originalstyle);
            }
            if (extralines > 0)
            {
                for (int i = 0; i < extralines; i++)
                {
                    console.WriteLine("");
                }
            }
        }
    }
}
