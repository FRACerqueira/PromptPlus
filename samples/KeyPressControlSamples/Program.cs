// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace KeyPressControlSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = originalCulture;


            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Basic KeyPress (accept any key)");

            var result = PromptPlus.Controls.KeyPress()
                .Run();
            PrintResult(result);

            ShowSection("2) Confirm with prompt messsage (en-US)");

            result = PromptPlus.Controls.Confirm("Confirm Test")
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .Run();
            PrintResult(result);

            ShowSection("3) Confirm without prompt message (en-US)");

            result = PromptPlus.Controls.Confirm()
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .Run();
            PrintResult(result);

            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");

            ShowSection("4) Confirm without prompt message (pt-BR)");

            result = PromptPlus.Controls.Confirm()
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .Run();
            PrintResult(result);

            PromptPlus.Config.DefaultCulture = originalCulture;

            ShowSection("5) Valid keys + custom invalid message (ShowMessage)");

            result = PromptPlus.Controls.KeyPress("Press a valid key", "A, Ctrl+B, N(Off), Y(On)")
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .AddValidKey(ConsoleKey.A)
                .AddValidKey(ConsoleKey.B, ConsoleModifiers.Control)
                .AddValidKey(ConsoleKey.N, null, "Off")
                .AddValidKey(ConsoleKey.Y, null, "On")
                .ShowMessage((key) => $"Invalid key '{key.Key}'. Try A, Ctrl+B, N or Y.")
                .Run();
            PrintResult(result);

            ShowSection("6) Valid keys + async invalid message (ShowMessageAsync)");

            result = PromptPlus.Controls.KeyPress("Choose mode", "Only D (Debug) or R (Release)")
                .AddValidKey(ConsoleKey.D, null, "Debug")
                .AddValidKey(ConsoleKey.R, null, "Release")
                .ShowMessageAsync(async (key, cancellationToken) =>
                {
                    await Task.Delay(80, cancellationToken);
                    return $"'{key.Key}' is not valid. Use D or R.";
                })
                .Run();
            PrintResult(result);

            ShowSection("7) Styles customization (Styles)");

            result = PromptPlus.Controls.KeyPress("Styled sample", "Press 1 or 2")
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .AddValidKey(ConsoleKey.D1, null, "Option 1")
                .AddValidKey(ConsoleKey.D2, null, "Option 2")
                .Styles(KeyPressStyles.Prompt, Color.Yellow)
                .Styles(KeyPressStyles.Description, Color.Cyan)
                .Styles(KeyPressStyles.Answer, Color.Green)
                .Styles(KeyPressStyles.Tooltips, Color.Magenta)
                .Styles(KeyPressStyles.Error, Color.Red)
                .Run();
            PrintResult(result);

            ShowSection("8) KeyPress with showresult=false (factory overload)");

            result = PromptPlus.Controls.KeyPress("Hidden after finish", "Result line is hidden by control", showresult: false)
                .Run();
            PrintResult(result);

            ShowSection("9) Confirm with prompt + description (all parameters)");

            result = PromptPlus.Controls.Confirm("Apply changes", "Press the culture-specific Yes/No key")
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .Run();
            PrintResult(result);
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorder, 1);
        }

        private static void PrintResult(ResultPrompt<ConsoleKeyInfo?> result)
        {
            var key = result.Content.HasValue ? result.Content.Value.Key.ToString() : string.Empty;
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {key}");
            PromptPlus.Console.WriteLine("");
        }
    }
}
