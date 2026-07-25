// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace CalendarControlSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.Clear();

            DateTime now = DateTime.Now;
            DateTime baseDate = now.Date;
            CancellationToken token = CancellationToken.None;
            const int asyncDelayMs = 1;
            const string historyDefaultKey = "SampleCalendar.DefaultHistory";

            // Ensure reproducible history-based scenarios.
            PromptPlus.Controls.History(historyDefaultKey).Remove();
            try
            {
                ShowSection("1) Basic - Layout + Culture(string) + Default + Run(token)");
                var result = PromptPlus.Controls.Calendar("Select date")
                    .Default(now)
                    .Layout(CalendarLayout.AsciiSingleGrid)
                    .Culture("ko-KR")
                    .Run(token);
                PrintSelectionResult(result);

                ShowSection("2) Culture(CultureInfo) + FirstDayOfWeek + Range");
                result = PromptPlus.Controls.Calendar("Select date in current range")
                    .Default(now)
                    .Culture(new CultureInfo("en-US"))
                    .FirstDayOfWeek(DayOfWeek.Monday)
                    .Range(baseDate.AddDays(-3), baseDate.AddDays(3))
                    .Run();
                PrintSelectionResult(result);

                ShowSection("3) DisabledWeekend + DisableDates + Highlights");
                result = PromptPlus.Controls.Calendar("Select business date")
                    .Default(now)
                    .DisabledWeekend()
                    .DisableDates(baseDate.AddDays(1), baseDate.AddDays(2))
                    .Highlights(baseDate, baseDate.AddDays(3))
                    .Run();
                PrintSelectionResult(result);

                ShowSection("4) Notes - AddNote + AddNotes + Interaction");
                result = PromptPlus.Controls.Calendar("Select date with notes","Press [F2] in day 7")
                    .Default(now)
                    .AddNote(baseDate,$"Current day note {new string('x',150)}zk")
                    .AddNotes(
                    [
                        (baseDate.AddDays(1), "Tomorrow note"),
                        (baseDate.AddDays(2), "Day+2 note")
                    ])
                    .Interaction([""], (offset, ctrl) =>
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            ctrl.AddNote(baseDate, $"Generated note {i}");
                        }
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("5) InteractionAsync");
                result = PromptPlus.Controls.Calendar("Select date + PageSize + generated async")
                    .Default(now)
                    .PageSize(3)
                    .InteractionAsync([8, 9], async (offset, ctrl) =>
                    {
                        await Task.Delay(asyncDelayMs).ConfigureAwait(false);
                        ctrl.AddNote(baseDate.AddDays(offset), $"Async note {offset}");
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("6) ChangeDescription + PredicateSelected(bool)");
                result = PromptPlus.Controls.Calendar("Select odd day")
                    .Default(now)
                    .ChangeDescription(date => $"Selected day: {date:yyyy-MM-dd}")
                    .PredicateSelected(date => date.HasValue && date.Value.Day % 2 == 1)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("7) ChangeDescriptionAsync + PredicateSelected(message) '");
                result = PromptPlus.Controls.Calendar("Select day <= 28")
                    .Default(now)
                    .ChangeDescriptionAsync(date => Task.FromResult($"Async description: {date:dddd, dd MMM yyyy}"))
                    .PredicateSelected(date =>
                    {
                        if (!date.HasValue)
                        {
                            return (false, "Date is required");
                        }

                        return date.Value.Day <= 28
                            ? (true, (string?)null)
                            : (false, "Only days up to 28 are allowed in this sample");
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("8) PredicateSelectedAsync(bool)");
                result = PromptPlus.Controls.Calendar("Select future date")
                    .Default(now)
                    .PredicateSelectedAsync(async date =>
                    {
                        await Task.Delay(asyncDelayMs).ConfigureAwait(false);
                        return date.HasValue && date.Value.Date >= baseDate;
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("9) PredicateSelectedAsync(message)");
                result = PromptPlus.Controls.Calendar("Select date not on weekend")
                    .Default(now)
                    .PredicateSelectedAsync(async date =>
                    {
                        await Task.Delay(asyncDelayMs).ConfigureAwait(false);
                        if (!date.HasValue)
                        {
                            return (false, "Date is required");
                        }

                        bool isWeekend = date.Value.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
                        return isWeekend
                            ? (false, "Weekend is blocked by async validation")
                            : (true, (string?)null);
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("10) EnabledHistory + Options + Styles");
                result = PromptPlus.Controls.Calendar("Styled date")
                    .Default(now, useDefaultHistory: true)
                    .EnabledHistory(historyDefaultKey, history =>
                    {
                        history.MaxItems(5);
                    })
                    .Options(opt =>
                    {
                        opt.Description("Custom options sample");
                        opt.ShowTooltip(true);
                        opt.ShowMessageAbortKey(true);
                        opt.EnabledAbortKey(true);
                        opt.HideAfterFinish(false);
                        opt.HideOnAbort(false);
                    })
                    .Styles(CalendarStyles.Lines, Color.Blue)
                    .Styles(CalendarStyles.Selected, Color.Green)
                    .Styles(CalendarStyles.CalendarDay, Color.Yellow)
                    .Styles(CalendarStyles.CalendarHighlight, Color.Blue)
                    .Styles(CalendarStyles.CalendarMonth, Color.Green)
                    .Styles(CalendarStyles.CalendarWeekDay, Color.Aqua)
                    .Styles(CalendarStyles.CalendarYear, Color.Violet)
                    .Run();
                PrintSelectionResult(result);
            }
            finally
            {
                // Cleanup persisted sample history.
                PromptPlus.Controls.History(historyDefaultKey).Remove();
            }
        }

        private static void ConfigureCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void PrintSelectionResult(ResultPrompt<DateTime?> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine(string.Empty);
        }

    }
}
