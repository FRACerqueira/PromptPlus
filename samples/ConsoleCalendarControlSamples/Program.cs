// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleCalendarControlSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.ProfileConfig("Myprofile", (cfg) =>
            {
                cfg.PadLeft = 2;
            });
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash($"Control:Calendar with overwrite culture:pt-br.");

            var result = PromptPlus.Controls.Calendar("Select date: ")
                .Default(DateTime.Now)
                .Layout(CalendarLayout.DoubleGrid)
                .Culture("pt-BR")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"Control:Calendar DateTime - with Disabled Weekends");

            result = PromptPlus.Controls.Calendar("Select date: ")
                .Default(DateTime.Now)
                .DisabledWeekend()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash($"Control:Calendar DateTime - with FirstDayOfWeek");

            result = PromptPlus.Controls.Calendar("Select date: ")
                .Default(DateTime.Now)
                .FirstDayOfWeek(DayOfWeek.Monday)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"Control:Calendar DateTime - with Disabled dates");

            result = PromptPlus.Controls.Calendar("Select date: ")
                .Default(DateTime.Now)
                .DisableDates(
                    DateTime.Now.AddDays(1),
                    DateTime.Now.AddDays(2))
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"Control:Calendar DateTime - with Notes");

            result = PromptPlus.Controls.Calendar("Select date: ")
                .Default(DateTime.Now)
                .DisabledWeekend()
                .PageSize(3)
                .AddNote(DateTime.Now, "note sample")
                .Interaction<string>(
                    ["note1 samples", "note2 samples", "note3 samples", "note4 samples", "note5 samples"],
                    (item, ctrl) => ctrl.AddNote(DateTime.Now.AddDays(1), item))
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash($"Control:Calendar DateTime - with Highlight");

            result = PromptPlus.Controls.Calendar("Select date: ")
                .Default(DateTime.Now)
                .PageSize(3)
                .DisabledWeekend()
                .Highlights(DateTime.Now, DateTime.Now.AddDays(1))
                .Run();

            PromptPlus.Widgets.DoubleDash($"Control:Calendar DateTime - with Styles");

            result = PromptPlus.Controls.Calendar("Select date: ")
                .Default(DateTime.Now)
                .Styles(CalendarStyles.Lines, Color.Blue)
                .Styles(CalendarStyles.Selected, Color.Green)
                .Styles(CalendarStyles.CalendarDay, Color.Yellow)
                .Styles(CalendarStyles.CalendarHighlight, Color.Blue)
                .Styles(CalendarStyles.CalendarMonth, Color.Green)
                .Styles(CalendarStyles.CalendarWeekDay, Color.Aqua)
                .Styles(CalendarStyles.CalendarYear, Color.Violet)
                .Run();
        }
    }
}
