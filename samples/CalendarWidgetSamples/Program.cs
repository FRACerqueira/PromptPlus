// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace CalendarWidgetSamples
{
    internal class Program
    {
        static void Main()
        {
            static void Pause(string message = "[Yellow]Press any key to continue[/]")
            {
                PromptPlus.Console.WriteLine("");
                PromptPlus.Console.WriteLine(message);
                PromptPlus.Console.ReadKey();
                PromptPlus.Console.WriteLine();
            }

            ConfigureCulture();

            PromptPlus.Console.Clear();
            DateTime baseDate = DateTime.Today;

            ShowSection($"Default culture: {Thread.CurrentThread.CurrentCulture.Name}");

            ShowSection("1) Layouts");
            foreach (CalendarLayout layout in Enum.GetValues<CalendarLayout>())
            {
                PromptPlus.Console.Dash($"Calendar layout: {layout}");
                PromptPlus.Widgets
                    .Calendar(baseDate)
                    .Layout(layout)
                    .Show();
                Pause();
            }

            ShowSection("2) Culture(string) pt-BR");
            PromptPlus.Widgets
                .Calendar(baseDate)
                .Layout(CalendarLayout.DoubleGrid)
                .Culture("pt-BR")
                .Show();

            Pause();

            ShowSection("3) Culture(CultureInfo) en-US");
            PromptPlus.Widgets
                .Calendar(baseDate)
                .Culture(new CultureInfo("en-US"))
                .Show();

            Pause();

            ShowSection("4) FirstDayOfWeek - Monday");
            PromptPlus.Widgets
                .Calendar(baseDate)
                .Layout(CalendarLayout.SingleGrid)
                .FirstDayOfWeek(DayOfWeek.Monday)
                .Show();

            Pause();

            ShowSection("5) DisableDates + Highlights");
            PromptPlus.Widgets
                .Calendar(baseDate)
                .DisableDates(baseDate.AddDays(1), baseDate.AddDays(2))
                .Highlights(baseDate, baseDate.AddDays(3), baseDate.AddDays(5))
                .Show();
            
            Pause();

            ShowSection("6) Styles");
            PromptPlus.Widgets
                .Calendar(baseDate)
                .Styles(CalendarStyles.Lines, Color.Blue)
                .Styles(CalendarStyles.Selected, Color.Green)
                .Styles(CalendarStyles.Disabled, Color.Gray)
                .Styles(CalendarStyles.CalendarDay, Color.Yellow)
                .Styles(CalendarStyles.CalendarHighlight, Color.Blue)
                .Styles(CalendarStyles.CalendarMonth, Color.Green)
                .Styles(CalendarStyles.CalendarWeekDay, Color.Aqua)
                .Styles(CalendarStyles.CalendarYear, Color.Violet)
                .Show();

            Pause("Press any key to end");
        }

        private static void ConfigureCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.SufixAfterPrompt = string.Empty;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }
    }
}
