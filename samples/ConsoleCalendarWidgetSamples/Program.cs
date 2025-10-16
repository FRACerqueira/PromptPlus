// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleCalendarWidgetSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture =  new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.ProfileConfig("Myprofile", (cfg) =>
            {
                cfg.PadLeft = 2;
            });
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash($"Set default Culture to {cult.Name}", DashOptions.DoubleBorder, style: Color.Yellow);

            var typelayout = Enum.GetValues<CalendarLayout>();
            foreach (var type in typelayout)
            {

                PromptPlus.Widgets
                   .DoubleDash($"Widget:Calendar ({cult.Name}) - layout {type}");

                PromptPlus.Widgets
                    .Calendar(DateTime.Now)
                    .Layout(type)
                    .Show();

            }


            PromptPlus.Widgets.DoubleDash($"Widget:Calendar DateTime - with overwrite culture:pt-br.");

            PromptPlus.Widgets
                .Calendar(DateTime.Now)
                .Layout(CalendarLayout.DoubleGrid)
                .Culture("pt-BR")
                .Show();

            PromptPlus.Widgets.DoubleDash($"Widget:Calendar DateTime - with FirstDayOfWeek");

            PromptPlus.Widgets
                .Calendar(DateTime.Now)
                .Layout(CalendarLayout.SingleGrid)
                .FirstDayOfWeek(DayOfWeek.Monday)
                .Show();

            PromptPlus.Widgets.DoubleDash($"Widget:Calendar DateTime - with Styles");

            PromptPlus.Widgets
                .Calendar(DateTime.Now)
                .Styles(CalendarStyles.Lines, Color.Blue)
                .Styles(CalendarStyles.Selected, Color.Green)
                .Styles(CalendarStyles.CalendarDay, Color.Yellow)
                .Styles(CalendarStyles.CalendarHighlight, Color.Blue)
                .Styles(CalendarStyles.CalendarMonth, Color.Green)
                .Styles(CalendarStyles.CalendarWeekDay, Color.Aqua)
                .Styles(CalendarStyles.CalendarYear, Color.Violet)
                .Show();
        }
    }
}
