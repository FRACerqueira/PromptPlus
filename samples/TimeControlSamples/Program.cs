// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace TimeControlSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Duration(int seconds) - 3 seconds");
            var result = PromptPlus.Controls.Time("Starting in")
                .Duration(3)
                .Run();
            PrintSelectionResult(result);

            ShowSection("3) Duration(TimeSpan) - 10 seconds");
            result = PromptPlus.Controls.Time("Cooling down")
                .Duration(TimeSpan.FromSeconds(20))
                .Run();
            PrintSelectionResult(result);

            ShowSection("3) Format - mm:ss:fff");
            result = PromptPlus.Controls.Time("Countdown")
                .Duration(5)
                .Format(@"mm\:ss\:fff")
                .Run();
            PrintSelectionResult(result);

            ShowSection("4) Finish text");
            result = PromptPlus.Controls.Time("Please wait")
                .Duration(3)
                .Finish("Done!")
                .Run();
            PrintSelectionResult(result);

            ShowSection("5) Culture - pt-BR formatting");
            result = PromptPlus.Controls.Time("Aguarde")
                .Duration(4)
                .Culture(new CultureInfo("pt-BR"))
                .Run();
            PrintSelectionResult(result);

            ShowSection("6) ChangeDescription (sync)");
            result = PromptPlus.Controls.Time("Please wait")
                .Duration(5)
                .ChangeDescription(remaining => $"Remaining: {remaining.TotalSeconds:0} second(s)")
                .Run();
            PrintSelectionResult(result);

            ShowSection("7) ChangeDescriptionAsync");
            result = PromptPlus.Controls.Time("Please wait")
                .Duration(5)
                .ChangeDescriptionAsync(async remaining =>
                {
                    await Task.Delay(1).ConfigureAwait(false);
                    return $"Async remaining: {remaining:ss} s";
                })
                .Run();
            PrintSelectionResult(result);

            ShowSection("8) Styles - custom prompt and answer regions");
            result = PromptPlus.Controls.Time("Please wait")
                .Duration(5)
                .Styles(TimeStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(TimeStyles.Answer, new Style(Color.Green, Color.Black))
                .Run();
            PrintSelectionResult(result);

            ShowSection("9) Options - hide tooltip and keep result visible");
            result = PromptPlus.Controls.Time("Please wait")
                .Duration(4)
                .Options(opt =>
                {
                    opt.Description("Press ESC to abort the countdown");
                    opt.ShowTooltip(false);
                    opt.ShowMessageAbortKey(true);
                    opt.EnabledAbortKey(true);
                    opt.HideAfterFinish(false);
                    opt.HideOnAbort(false);
                })
                .Run();
            PrintSelectionResult(result);

            ShowSection("10) Run(token) - cancelable countdown after 2 seconds");
            using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                result = PromptPlus.Controls.Time("Cancelable countdown", "Runs with a CancellationToken")
                    .Duration(10)
                    .Run(sw.Token);
                PrintSelectionResult(result);
            }

            ShowSection("11) DisplayMode - Countdown (default)");
            result = PromptPlus.Controls.Time("Please wait")
                .Duration(5)
                .DisplayMode(TimeDisplayMode.Countdown)
                .Run();
            PrintSelectionResult(result);

            ShowSection("12) DisplayMode - Elapsed (count up)");
            var totalDuration = TimeSpan.FromSeconds(5);
            result = PromptPlus.Controls.Time("Running")
                .Duration(totalDuration)
                .DisplayMode(TimeDisplayMode.Elapsed)
                .ChangeDescription(elapsed =>
                {
                    var remaining = totalDuration - elapsed;
                    if (remaining < TimeSpan.Zero)
                    {
                        remaining = TimeSpan.Zero;
                    }
                    return $"Remaining: {remaining.TotalSeconds:0} second(s)";
                })
                .Run();
            PrintSelectionResult(result);

            ShowSection("13) No prompt and no tooltips");
            result = PromptPlus.Controls.Time()
                .Duration(5)
                .Options(opt => opt.ShowTooltip(false))
                .Run();
            PrintSelectionResult(result);

            ShowSection("14) Spinner at the end of the answer");
            result = PromptPlus.Controls.Time("Please wait")
                .Duration(5)
                .Spinner(SpinnersType.Default)
                .Run();
            PrintSelectionResult(result);
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

        private static void PrintSelectionResult(ResultPrompt<TimeSpan> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Elapsed: {result.Content}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
