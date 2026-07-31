// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace SwitchControlSamples
{
    internal class Program
    {
        private const string historyDefaultKey = "SampleSwitch.DefaultHistory";

        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Controls.History(historyDefaultKey).Remove();
            try
            {
                ShowSection("1) Basic - default switch");
                var result = PromptPlus.Controls.Switch("Enable feature?")
                    .Run();
                PrintSelectionResult(result);

                ShowSection("2) Default(true)");
                result = PromptPlus.Controls.Switch("Enable feature?")
                    .Default(true)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("3) Custom labels (On/Off)");
                result = PromptPlus.Controls.Switch("Environment")
                    .OnValue("Production")
                    .OffValue("Development")
                    .Default(false)
                    .Run();
                PrintSelectionResult(result);

                ShowSection("4) Emoji labels with fallback");
                result = PromptPlus.Controls.Switch("Power")
                    .OnValue(EmojiName.GreenCircle, "ON")
                    .OffValue(EmojiName.RedCircle, "OFF")
                    .Run();
                PrintSelectionResult(result);

                ShowSection("5) ChangeDescription (sync)");
                result = PromptPlus.Controls.Switch("Notifications")
                    .ChangeDescription(current => current ? "Notifications are enabled" : "Notifications are disabled")
                    .Run();
                PrintSelectionResult(result);

                ShowSection("6) ChangeDescriptionAsync");
                result = PromptPlus.Controls.Switch("Telemetry")
                    .ChangeDescriptionAsync(async current =>
                    {
                        await Task.Delay(1).ConfigureAwait(false);
                        return current ? "Telemetry will be sent" : "Telemetry will stay local";
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("7) Options - hide tooltip and keep result visible");
                result = PromptPlus.Controls.Switch("Verbose mode")
                    .Options(opt =>
                    {
                        opt.Description("Toggle using Left/Right arrows, Tab/Shift+Tab, or Space");
                        opt.ShowTooltip(false);
                        opt.ShowMessageAbortKey(true);
                        opt.EnabledAbortKey(true);
                        opt.HideAfterFinish(false);
                        opt.HideOnAbort(false);
                    })
                    .Run();
                PrintSelectionResult(result);

                ShowSection("8) Styles - custom switch-on/switch-off regions");
                result = PromptPlus.Controls.Switch("Use cache")
                    // Other style regions available on this control (not exercised here):
                    //.Styles(SwitchStyles.Prompt, new Style(Color.Yellow, Color.Black))
                    //.Styles(SwitchStyles.Answer, new Style(Color.Green, Color.Black))
                    //.Styles(SwitchStyles.Marker, new Style(Color.White, Color.Darkgray))
                    .Styles(SwitchStyles.SwitchOn, new Style(Color.Black, Color.Darkgreen))
                    .Styles(SwitchStyles.SwitchOff, new Style(Color.Black, Color.Darkred))
                    //.Styles(SwitchStyles.Ranger, new Style(Color.Cyan, Color.Black))
                    .Run();
                PrintSelectionResult(result);

                ShowSection("9) Run(token) - cancelable prompt after 5 seconds");
                using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                {
                    result = PromptPlus.Controls.Switch("Cancelable switch", "Runs with a CancellationToken")
                        .Run(sw.Token);
                    PrintSelectionResult(result);
                }

                ShowSection("10) EnableHistory + Default from history");
                PromptPlus.Controls.History(historyDefaultKey)
                    .AddHistory("true")
                    .Save();

                result = PromptPlus.Controls.Switch("Use default from history?")
                    .Default(false, true)
                    .EnableHistory(historyDefaultKey)
                    .Run();
                PrintSelectionResult(result);
            }
            finally
            {
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

        private static void PrintSelectionResult(ResultPrompt<bool?> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
