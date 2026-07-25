// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary.Controls.Calendar;
using PromptPlusLibrary.Controls.ChartBar;
using PromptPlusLibrary.Controls.Common;
using PromptPlusLibrary.Controls.Slider;
using PromptPlusLibrary.Controls.Switch;
using System;
using System.IO;

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Internal implementation of the IControls interface, providing factory methods for creating interactive controls with fluent configuration options.
    /// </summary>
    /// <param name="console">The console interface used for input/output operations.</param>
    /// <param name="promptConfig"></param>

    internal sealed class PromptPlusWidgets(IConsole console, PromptConfig promptConfig) : IWidgets
    {
        public ISliderWidget Slider(double value, double minvalue = 0, double maxvalue = 100, byte fractionalDigits = 2)
        {
            SliderControl ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            ctrl.FractionalDigits(fractionalDigits);
            ctrl.Range(minvalue, maxvalue);
            ctrl.Default(value);
            return ctrl;
        }

        public ICalendarWidget Calendar(DateTime dateref)
        {
            CalendarControl ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            ctrl.Default(dateref);
            return ctrl;
        }

        public ISwitchWidget Switch(bool value)
        {
            SwitchContrrol ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            ctrl.Default(value);
            return ctrl;
        }

        /// <inheritdoc/>
        public void Banner(string? value, Style? style = null, DashOptions dashOptions = DashOptions.None)
        {
            console.Banner(value, style, dashOptions);
        }

        /// <inheritdoc/>
        public void Banner(string? value, string pathfontFiglet, Style? style = null, DashOptions dashOptions = DashOptions.None)
        {
            console.Banner(value, pathfontFiglet, style, dashOptions);
        }

        /// <inheritdoc/>
        public void Banner(string? value, Stream streamFontFiglet, Style? style = null, DashOptions dashOptions = DashOptions.None)
        {
            console.Banner(value, streamFontFiglet, style, dashOptions);
        }

        public void Dash(string? value, Style? style = null, DashOptions dashOptions = DashOptions.SingleBorder, int extralines = 0, bool applycolorbackground = false)
        {
            console.Dash(value,dashOptions, style ?? console.CurrentStyle, extralines, applycolorbackground);
        }

        /// <inheritdoc/>
        public void SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null, bool applycolorbackground = false)
        {
            ConsolePlus.RunAtomic(() =>
            {
                var localdashOptions = dashOptions switch
                {
                    DashOptions.AsciiSingleBorder => DashOptions.AsciiSingleBorder,
                    DashOptions.AsciiDoubleBorder => DashOptions.AsciiDoubleBorder,
                    DashOptions.SingleBorder => DashOptions.SingleBorder,
                    DashOptions.DoubleBorder => DashOptions.SingleBorder,
                    DashOptions.None => DashOptions.None,
                    DashOptions.HeavyBorder => DashOptions.HeavyBorder,
                    DashOptions.AsciiSingleBorderUpDown => DashOptions.AsciiSingleBorder,
                    DashOptions.AsciiDoubleBorderUpDown => DashOptions.AsciiDoubleBorder,
                    DashOptions.SingleBorderUpDown => DashOptions.SingleBorder,
                    DashOptions.DoubleBorderUpDown => DashOptions.SingleBorder,
                    DashOptions.HeavyBorderUpDown => DashOptions.HeavyBorder,
                    _ => throw new AbortException($"dashOptions : {dashOptions} Not Implemented")
                };
                console.Dash(value, localdashOptions, style);
                if (style.HasValue && applycolorbackground)
                {
                    console.ForegroundColor = style.Value.Foreground;
                    console.BackgroundColor = style.Value.Background;
                }
                if (extralines > 0)
                {
                    console.WriteLines(extralines);
                }
            });
        }

        /// <inheritdoc/>
        public void DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null, bool applycolorbackground = false)
        {
            ConsolePlus.RunAtomic(() =>
            {
                var localdashOptions = dashOptions switch
                {
                    DashOptions.AsciiSingleBorder => DashOptions.AsciiSingleBorderUpDown,
                    DashOptions.AsciiDoubleBorder => DashOptions.AsciiDoubleBorderUpDown,
                    DashOptions.SingleBorder => DashOptions.SingleBorderUpDown,
                    DashOptions.DoubleBorder => DashOptions.DoubleBorderUpDown,
                    DashOptions.None => DashOptions.None,
                    DashOptions.HeavyBorder => DashOptions.HeavyBorderUpDown,
                    DashOptions.AsciiSingleBorderUpDown => DashOptions.AsciiSingleBorderUpDown,
                    DashOptions.AsciiDoubleBorderUpDown => DashOptions.AsciiDoubleBorderUpDown,
                    DashOptions.SingleBorderUpDown => DashOptions.SingleBorderUpDown,
                    DashOptions.DoubleBorderUpDown => DashOptions.DoubleBorderUpDown,
                    DashOptions.HeavyBorderUpDown => DashOptions.HeavyBorderUpDown,
                    _ => throw new AbortException($"dashOptions : {dashOptions} Not Implemented")
                }; console.Dash(value, localdashOptions, style);
                if (style.HasValue && applycolorbackground)
                {
                    console.ForegroundColor = style.Value.Foreground;
                    console.BackgroundColor = style.Value.Background;
                }
                if (extralines > 0)
                {
                    console.WriteLines(extralines);
                }
            });
        }

        public IChartBarWidget ChartBar()
        {
            ChartBarControl ctrl = new(true, console, promptConfig, new BaseControlOptions(promptConfig));
            return ctrl;
        }

    }
}
