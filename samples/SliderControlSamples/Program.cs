// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace SliderControlSamples
{
    internal class Program
    {
        // History key used by the persisted-history scenario.
        private const string historyDefaultKey = "SampleSlider.DefaultHistory";

        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            CultureInfo culture = Thread.CurrentThread.CurrentCulture;

            // Ensure reproducible history-based scenarios.
            PromptPlus.Controls.History(historyDefaultKey).Remove();
            try
            {
                ShowSection("1) Basic - default range 0..100 + LeftRight layout");
                var result = PromptPlus.Controls.Slider("Select value")
                    .Run();
                PrintSelectionResult(result);

                ShowSection("2) Layout - UpDown (vertical) slider");
                result = PromptPlus.Controls.Slider("Select value")
                    .Layout(SliderLayout.UpDown)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("3) HideElements - hide the min/max delimiters");
                result = PromptPlus.Controls.Slider("Select value")
                    .HideElements(HideSlider.Delimit)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("4) HideElements - hide both delimiters and range display");
                result = PromptPlus.Controls.Slider("Select value")
                    .HideElements(HideSlider.Delimit | HideSlider.Range)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("5) Range + FractionalDig + Step + LargeStep + Default");
                result = PromptPlus.Controls.Slider("Select value", "Custom range -50..50")
                    .Range(-50, 50)
                    .FractionalDigits(1)
                    .Step(0.5)
                    .LargeStep(5)
                    .Default(0)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("6) Width - wider bar (60 chars)");
                result = PromptPlus.Controls.Slider("Select value", "Bar drawn with 60 characters")
                    .Width(60)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("7) Culture - format value using pt-BR (comma decimal separator)");
                result = PromptPlus.Controls.Slider("Select value", "Value formatted with pt-BR culture")
                    .Culture("pt-BR")
                    .Range(0, 10)
                    .FractionalDigits(2)
                    .Step(0.25)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("8) ChangeColor - color changes by value threshold");
                result = PromptPlus.Controls.Slider("Select value", "Red <= 30, Blue <= 70, Gold > 70")
                    .ChangeColor(value =>
                    {
                        if (value <= 30)
                        {
                            return new Style(Color.Red, Color.Red);
                        }
                        if (value <= 70)
                        {
                            return new Style(Color.Blue, Color.Blue);
                        }
                        return new Style(Color.Darkgoldenrod, Color.Darkgoldenrod);
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("9) ChangeGradient - smooth Green -> Yellow -> Red gradient");
                result = PromptPlus.Controls.Slider("Select value")
                    .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("10) ChangeDescription - description text follows the current value");
                result = PromptPlus.Controls.Slider("Select value")
                    .ChangeDescription(value => $"Current selection: {value:0} %")
                    .Run();
                PrintSelectionResult(result);

                ShowSection("11) ChangeDescriptionAsync - description from an async source");
                result = PromptPlus.Controls.Slider("Select value")
                    .ChangeDescriptionAsync(async value =>
                    {
                        await Task.Delay(1).ConfigureAwait(false);
                        return $"Async description for value {value:0}";
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("12) Options - custom description, tooltip hide and abort behavior");
                result = PromptPlus.Controls.Slider("Select value")
                    .Options(opt =>
                    {
                        opt.Description("Custom options sample");
                        opt.ShowTooltip(false);
                        opt.ShowMessageAbortKey(true);
                        opt.EnabledAbortKey(true);
                        opt.HideAfterFinish(false);
                        opt.HideOnAbort(false);
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("13) Styles - restyle prompt, answer, slider and range regions");
                result = PromptPlus.Controls.Slider("Select value")
                    .Styles(SliderStyles.Prompt, new Style(Color.Aqua, Color.Black))
                    .Styles(SliderStyles.Answer, new Style(Color.Green, Color.Black))
                    .Styles(SliderStyles.Slider, new Style(Color.Blue, Color.Black))
                    .Run();
                PrintSelectionResult(result);

                ShowSection("14) Run(token) - cancelable prompt via CancellationToken after 5 seconds");
                using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    result = PromptPlus.Controls.Slider("Select value", "Runs with a CancellationToken")
                     .Run(sw.Token);
                    PrintSelectionResult(result);
                }

                ShowSection("15) EnabledHistory - recall previous values with 20 overwrite 0 - default");
                // Pre-load history for the sample; the control manages this internally at runtime.
                PromptPlus.Controls.History(historyDefaultKey)
                    .AddHistory(double.Parse("20", culture).ToString(culture))
                    .Save();

                result = PromptPlus.Controls.Slider("Select value")
                    .Default(0, true) // Falls back to the last history value as the default.
                    .FractionalDigits(2)
                    .Step(0.5)
                    .LargeStep(5)
                    .EnabledHistory(historyDefaultKey)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("16) Fill - all bar fill styles (SliderBarType)");
                foreach (var type in Enum.GetValues<SliderBarType>())
                {
                    result = PromptPlus.Controls.Slider("Select value", $"Fill style: {type}")
                        .BarType(type)
                        .Run();
                    PrintSelectionResult(result);
                }
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

        private static void PrintSelectionResult(ResultPrompt<double?> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content ?? -1}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
