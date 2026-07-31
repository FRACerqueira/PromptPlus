// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;
using System.Text.RegularExpressions;

namespace InputControlsSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            const string historyKey = "InputControlSamples.History";

            // Clean and pre-load history used by dedicated samples.
            PromptPlus.Controls.History(historyKey).Remove();
            PromptPlus.Controls.History(historyKey)
                .AddHistory("Test-Alpha")
                .AddHistory("Test-Beta")
                .AddHistory("Prod-Alpha")
                .AddHistory("Prod-Beta")
                .AddHistory("Profile-Local")
                .Save();

            ShowSection("1) Basic input (max length)");
            var result = PromptPlus.Controls.Input("Name", "Enter your name (max 20 chars)")
                .MaxLength(20)
                .Run();
            PrintResult(result);

            ShowSection("2) Defaults + live description + custom style");
            result = PromptPlus.Controls.Input("Display Name", "Press Enter without typing to use DefaultIfEmpty")
                .Styles(InputStyles.Answer, Color.Green)
                .Default("John Doe")
                .DefaultIfEmpty("Name Empty")
                .ChangeDescription(input => $"Current length: {input.Length}")
                .MaxLength(150)
                .Run();
            PrintResult(result);

            ShowSection("3) Case transform (Uppercase)");
            result = PromptPlus.Controls.Input("Code", "Any letters typed are transformed to uppercase")
                .InputToCase(CaseOptions.Uppercase)
                .Run();
            PrintResult(result);

            ShowSection("4) Synchronous suggestions");
            result = PromptPlus.Controls.Input("Environment", "Type: dev, test, prod (TAB/Shift+TAB to rotate)")
                .SuggestionHandler(input =>
                {
                    var values = new[] { "dev", "test", "staging", "prod", "sandbox" };
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        return values;
                    }
                    return [.. values.Where(x => x.StartsWith(input, StringComparison.OrdinalIgnoreCase))];
                })
                .Run();
            PrintResult(result);

            ShowSection("5) Synchronous suggestions - Not auto-complete");
            result = PromptPlus.Controls.Input("Environment", "Type: dev, test, prod (TAB/Shift+TAB to rotate)")
                .SuggestionHandler(input =>
                {
                    var values = new[] { "dev", "test", "staging", "prod", "sandbox" };
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        return values;
                    }
                    return [.. values.Where(x => x.StartsWith(input, StringComparison.OrdinalIgnoreCase))];
                },false)
                .Run();
            PrintResult(result);

            ShowSection("6) Async suggestions based on prefix");
            result = PromptPlus.Controls.Input("Country", "Try typing us, uk, br, de, fr")
                .SuggestionHandlerAsync(async input =>
                {
                    await Task.Delay(1);
                    var values = new[] { "us", "uk", "br", "de", "fr", "es", "it" };
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        return values;
                    }
                    return [.. values.Where(x => x.StartsWith(input, StringComparison.OrdinalIgnoreCase))];
                })
                .Run();
            PrintResult(result);

            ShowSection("7) History + suggestion + advanced history options");
            result = PromptPlus.Controls.Input("Profile", "Type at least 2 chars and press history hotkey")
                .EnableHistory(historyKey, opt => opt
                    .PageSize(3)
                    .MinPrefixLength(2)
                    .FilterType(FilterMode.StartsWith)
                    .MaxItems(10)
                    .ExpirationTime(TimeSpan.FromDays(30)))
                .SuggestionHandler(input => ["Test-Alpha", "Prod-Beta", "Profile-Local"])
                .Run();
            PrintResult(result);

            ShowSection("8) Default value from latest history");
            result = PromptPlus.Controls.Input("Profile", "Last saved history entry is loaded as default")
                .Default(string.Empty, true)
                .EnableHistory(historyKey)
                .Run();
            PrintResult(result);

            ShowSection("9) Input filter (digits only)");
            result = PromptPlus.Controls.Input("PIN", "Only digits are accepted (max 5)")
                .AcceptInput(char.IsDigit)
                .MaxLength(5)
                .Run();
            PrintResult(result);

            ShowSection("10) Validation (tuple with message)");
            result = PromptPlus.Controls.Input("Code", "Only digits and at least 2 chars")
                .AcceptInput(char.IsDigit)
                .PredicateValid(x => x.Length < 2
                    ? (false, "Length must be greater than or equal to 2")
                    : (true, null))
                .Run();
            PrintResult(result);

            ShowSection("11) Validation (bool)");
            result = PromptPlus.Controls.Input("Code", "Only digits and at least 2 chars")
                .AcceptInput(char.IsDigit)
                .PredicateValid(x => x.Length >= 2)
                .Run();
            PrintResult(result);

            ShowSection("12) Async validation (bool)");
            result = PromptPlus.Controls.Input("Code", "Only digits and at least 2 chars (async)")
                .AcceptInput(char.IsDigit)
                .PredicateValidAsync(async x =>
                {
                    await Task.Delay(1);
                    return x.Length >= 2;
                })
                .Run();
            PrintResult(result);

            ShowSection("13) Async validation (message)");
            result = PromptPlus.Controls.Input("Code", "Only digits and at least 2 chars (async message)")
                .AcceptInput(char.IsDigit)
                .PredicateValidAsync(async x =>
                {
                    await Task.Delay(1);
                    return x.Length < 2
                        ? (false, "Length must be greater than or equal to 2")
                        : (true, (string?)null);
                })
                .Run();
            PrintResult(result);

            ShowSection("14) Async description");
            result = PromptPlus.Controls.Input("Name", "Type to update the description asynchronously")
                .ChangeDescriptionAsync(async input =>
                {
                    await Task.Delay(1);
                    return $"Current length: {input.Length}";
                })
                .Run();
            PrintResult(result);

            ShowSection("15) Control options");
            result = PromptPlus.Controls.Input("Name", "Abort key (Esc) disabled and hidden after finish")
                .Options(opt => opt
                    .EnabledAbortKey(false)
                    .ShowTooltip(true)
                    .HideAfterFinish(true))
                .Run();
            PrintResult(result);

            ShowSection("16) Secret input with complexity rule");
            result = PromptPlus.Controls.Secret("Password", "Min 8 chars with upper/lower/digit/special")
                .MaskSecret('#', true)
                .PredicateValid(x =>
                {
                    var validate = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
                    return validate.IsMatch(x);
                })
                .Run();
            PrintResult(result);

            ShowSection("17) Secret PIN with strict filter and message");
            result = PromptPlus.Controls.Secret("PIN", "Only 4 digits are accepted")
                .MaskSecret('*', false)
                .Styles(InputStyles.Answer, Color.Green)
                .AcceptInput(char.IsDigit)
                .MaxLength(4)
                .PredicateValid(x => x.Length == 4
                    ? (true, null)
                    : (false, "PIN must be exactly 4 digits"))
                .Run();
            PrintResult(result);

            ShowSection("18) Secret with lowercase transform (scenario not covered before)");
            result = PromptPlus.Controls.Secret("ApiKey", "Input is transformed to lowercase")
                .MaskSecret('*', false)
                .InputToCase(CaseOptions.Lowercase)
                .Run();
            PrintResult(result);

            ShowSection("19) Secret with sync description + async bool validation");
            result = PromptPlus.Controls.Secret("Password", "Minimum 8 chars")
                .ChangeDescription(input => $"Length: {input.Length}")
                .PredicateValidAsync(async x =>
                {
                    await Task.Delay(1);
                    return x.Length >= 8;
                })
                .Run();
            PrintResult(result);

            ShowSection("20) Secret with async description + async message validation");
            result = PromptPlus.Controls.Secret("Password", "Minimum 8 chars (async)")
                .ChangeDescriptionAsync(async input =>
                {
                    await Task.Delay(1);
                    return $"Length: {input.Length}";
                })
                .PredicateValidAsync(async x =>
                {
                    await Task.Delay(1);
                    return x.Length < 8
                        ? (false, "Password must have at least 8 chars")
                        : (true, (string?)null);
                })
                .Run();
            PrintResult(result);

            ShowSection("21) Secret with options");
            result = PromptPlus.Controls.Secret("Password", "Abort key disabled and hidden after finish")
                .Options(opt => opt
                    .EnabledAbortKey(false)
                    .ShowTooltip(true)
                    .HideAfterFinish(true))
                .Run();
            PrintResult(result);

            // Cleanup history used in this sample.
            PromptPlus.Controls.History(historyKey).Remove();
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorder, 1);
        }

        private static void PrintResult(ResultPrompt<string> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");
        }
    }
}
