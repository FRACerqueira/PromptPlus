// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace MaskEditDateTimeControlSamples
{
    /// <summary>
    /// Demonstrates every method of <see cref="IMaskEditDateTimeControl{T}"/>.
    /// The date/time MaskEdit control edits DateTime / DateOnly / TimeOnly values.
    ///
    /// Factory helpers used here:
    ///   MaskDate(...)      -> IMaskEditDateTimeControl&lt;DateTime&gt; (date only mask d/M/y)
    ///   MaskDateTime(...)  -> IMaskEditDateTimeControl&lt;DateTime&gt; (date + time d/M/y h:m:s)
    ///   MaskDateOnly(...)  -> IMaskEditDateTimeControl&lt;DateOnly&gt;
    ///   MaskTime(...)      -> IMaskEditDateTimeControl&lt;DateTime&gt; (time only h:m:s)
    ///   MaskTimeOnly(...)  -> IMaskEditDateTimeControl&lt;TimeOnly&gt;
    ///
    /// The mask/order of the date parts follows the active Culture.
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            // ----------------------------------------------------------------------------
            // MaskDate - basic date. The separators/order come from the culture.
            // ----------------------------------------------------------------------------
            ShowSection("1) MaskDate - simple date (culture ordered)");
            var result = PromptPlus.Controls.MaskDate("Birth date")
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // PromptMask(value) - character shown on empty positions.
            // ----------------------------------------------------------------------------
            ShowSection("2) PromptMask - use '_' explicitly (default) shown as '#'");
            result = PromptPlus.Controls.MaskDate("Date")
                .PromptMask('#')
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // FixedValues(part, value) - locks a date/time part to a constant.
            // Here the year is fixed to the current year (-1 = "now").
            // ----------------------------------------------------------------------------
            ShowSection("3) FixedValues - fix Year to current, Month to 12");
            result = PromptPlus.Controls.MaskDate("Day only")
                .FixedValues(DateTimePart.Year, -1)
                .FixedValues(DateTimePart.Month, 12)
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // InputMode(EditCursorFreely) - cursor moves freely across the mask.
            // ----------------------------------------------------------------------------
            ShowSection("4) InputMode - EditCursorFreely");
            result = PromptPlus.Controls.MaskDateTime("Timestamp")
                .InputMode(InputBehavior.EditCursorFreely)
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // HideTipInputType() - hides the input type hint line.
            // ----------------------------------------------------------------------------
            ShowSection("5) HideTipInputType - hide the input type hint");
            result = PromptPlus.Controls.MaskDate("Date")
                .HideTipInputType()
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // WeekTypeMode(value) - shows the week day next to the answer once the
            // date is complete (WeekShort = "Mon", WeekLong = "Monday").
            // ----------------------------------------------------------------------------
            ShowSection("6) WeekTypeMode - show long week day next to the value");
            result = PromptPlus.Controls.MaskDate("Pick a date")
                .WeekTypeMode(WeekType.WeekLong)
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // Default(value) - pre-fills with an initial date/time.
            // ----------------------------------------------------------------------------
            ShowSection("7) Default - pre-filled with today");
            result = PromptPlus.Controls.MaskDate("Date")
                .Default(DateTime.Today)
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // DefaultIfEmpty(value) - value returned when submitting empty input.
            // ----------------------------------------------------------------------------
            ShowSection("8) DefaultIfEmpty - press ENTER empty to return 2000-01-01");
            result = PromptPlus.Controls.MaskDate("Date")
                .DefaultIfEmpty(new DateTime(2000, 1, 1))
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // Culture(CultureInfo) - drives the date part order and separators.
            // pt-BR uses dd/MM/yyyy.
            // ----------------------------------------------------------------------------
            ShowSection("9) Culture(CultureInfo) - pt-BR (dd/MM/yyyy)");
            result = PromptPlus.Controls.MaskDate("Data")
                .Culture(new CultureInfo("pt-BR"))
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // Culture(string) - same, using a culture name (US: MM/dd/yyyy).
            // ----------------------------------------------------------------------------
            ShowSection("10) Culture(string) - en-US (MM/dd/yyyy)");
            result = PromptPlus.Controls.MaskDate("Date")
                .Culture("en-US")
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,bool>) - simple validation.
            // ----------------------------------------------------------------------------
            ShowSection("11) PredicateSelected(bool) - date must be in the past");
            result = PromptPlus.Controls.MaskDate("Past date")
                .PredicateSelected(value => value < DateTime.Today)
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // PredicateSelected(Func<T,(bool,string?)>) - validation + custom message.
            // ----------------------------------------------------------------------------
            ShowSection("12) PredicateSelected(tuple) - must be a weekday, with message");
            result = PromptPlus.Controls.MaskDate("Working day")
                .PredicateSelected(value =>
                    value.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday)
                        ? (true, null)
                        : (false, "Please choose a weekday (Mon-Fri)."))
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // Styles(styleType, style) - customize regions.
            // ----------------------------------------------------------------------------
            ShowSection("13) Styles - custom prompt, answer and tagged-info");
            result = PromptPlus.Controls.MaskDate("Styled date")
                .WeekTypeMode(WeekType.WeekShort)
                .Styles(MaskEditStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(MaskEditStyles.Answer, new Style(Color.Green, Color.Black))
                .Styles(MaskEditStyles.TaggedInfo, new Style(Color.Cyan, Color.Black))
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // Options(action) - shared control options.
            // ----------------------------------------------------------------------------
            ShowSection("14) Options - description and abort key");
            result = PromptPlus.Controls.MaskDateTime("Appointment")
                .Options(opt =>
                {
                    opt.Description("Enter date and time");
                    opt.EnabledAbortKey(true);
                    opt.ShowMessageAbortKey(true);
                    opt.HideAfterFinish(false);
                })
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // MaskTime - time-only (returns a DateTime whose date part is unused).
            // ----------------------------------------------------------------------------
            ShowSection("15) MaskTime - time only (h:m:s)");
            result = PromptPlus.Controls.MaskTime("Alarm")
                .Run();
            PrintDateResult(result);

            // ----------------------------------------------------------------------------
            // MaskDateOnly - DateOnly value.
            // ----------------------------------------------------------------------------
            ShowSection("16) MaskDateOnly - DateOnly value");
            var dateOnly = PromptPlus.Controls.MaskDateOnly("Release date")
                .Default(DateOnly.FromDateTime(DateTime.Today))
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {dateOnly.IsAborted}, Value: {dateOnly.Content}");
            PromptPlus.Console.WriteLine(string.Empty);

            // ----------------------------------------------------------------------------
            // MaskTimeOnly - TimeOnly value.
            // ----------------------------------------------------------------------------
            ShowSection("17) MaskTimeOnly - TimeOnly value");
            var timeOnly = PromptPlus.Controls.MaskTimeOnly("Start time")
                .Default(new TimeOnly(8, 30, 0))
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {timeOnly.IsAborted}, Value: {timeOnly.Content}");
            PromptPlus.Console.WriteLine(string.Empty);

            // ----------------------------------------------------------------------------
            // Run(token) - cancelable execution.
            // ----------------------------------------------------------------------------
            ShowSection("18) Run(token) - auto-cancels after 5 seconds");
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
            {
                result = PromptPlus.Controls.MaskDate("Type before timeout")
                    .Run(cts.Token);
                PrintDateResult(result);
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

        private static void PrintDateResult(ResultPrompt<DateTime> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
