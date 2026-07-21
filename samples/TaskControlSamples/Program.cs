// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace TaskControlSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Basic - synchronous action");
            var result = PromptPlus.Controls.Task("Processing")
                .Action(token =>
                {
                    Thread.Sleep(2000);
                })
                .Run();
            PrintResult(result);

            ShowSection("2) Async function with elapsed time and spinner");
            result = PromptPlus.Controls.Task("Downloading")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Dots)
                .ActionAsync(async token =>
                {
                    await Task.Delay(3000, token).ConfigureAwait(false);
                })
                .Run();
            PrintResult(result);

            ShowSection("3) Input/Output isolated contexts");
            var context = new Dictionary<string, object?>
            {
                ["name"] = "PromptPlus",
                ["count"] = 10
            };
            result = PromptPlus.Controls.Task("Computing")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .Context(context)
                .ActionAsync(async (input, token) =>
                {
                    await Task.Delay(1500, token).ConfigureAwait(false);
                    int count = input.TryGetValue("count", out var raw) && raw is int c ? c : 0;
                    return new Dictionary<string, object?>
                    {
                        ["result"] = count * 2,
                        ["message"] = $"Processed {input["name"]}"
                    };
                })
                .Run();
            PrintResult(result);
            int doubled = result.Content.GetOutput<int>("result", out bool foundResult);
            string message = result.Content.GetOutput<string>("message", out _) ?? string.Empty;
            PromptPlus.Console.WriteLine($"Output => result: {(foundResult ? doubled : -1)}, message: {message}");
            PromptPlus.Console.WriteLine(string.Empty);

            ShowSection("3a) Input context only - read and use inside the action");
            var inputOnly = new Dictionary<string, object?>
            {
                ["user"] = "Alice",
                ["retries"] = 3
            };
            result = PromptPlus.Controls.Task("Authenticating")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .Context(inputOnly)
                .ChangeDescription(_ => $"Signing in user '{inputOnly["user"]}'...")
                .ActionAsync(async (input, token) =>
                {
                    string user = input.TryGetValue("user", out var u) && u is string s ? s : "unknown";
                    int retries = input.TryGetValue("retries", out var r) && r is int n ? n : 1;
                    for (int i = 1; i <= retries; i++)
                    {
                        await Task.Delay(600, token).ConfigureAwait(false);
                    }
                    PromptPlus.Console.WriteLine($"[action] Authenticated '{user}' after {retries} attempt(s)");
                    return null; // no output context in this sample
                })
                .Run();
            PrintResult(result);

            ShowSection("3b) Output context only - produce a result and read it back");
            result = PromptPlus.Controls.Task("Generating token")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .ActionAsync(async token =>
                {
                    await Task.Delay(1500, token).ConfigureAwait(false);
                    return new Dictionary<string, object?>
                    {
                        ["token"] = Guid.NewGuid().ToString("N"),
                        ["expiresInSeconds"] = 3600
                    };
                })
                .Run();
            PrintResult(result);
            string token = result.Content.GetOutput<string>("token", out bool foundToken) ?? string.Empty;
            int expires = result.Content.GetOutput<int>("expiresInSeconds", out _);
            PromptPlus.Console.WriteLine($"Output => token: {(foundToken ? token : "<none>")}, expiresInSeconds: {expires}");
            PromptPlus.Console.WriteLine(string.Empty);

            ShowSection("4) Finish text");
            result = PromptPlus.Controls.Task("Saving")
                .Spinner(SpinnersType.Default)
                .Finish("Saved!")
                .Action(token =>
                {
                    Thread.Sleep(2000);
                })
                .Run();
            PrintResult(result);

            ShowSection("5) ChangeDescription (sync)");
            result = PromptPlus.Controls.Task("Working")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .ChangeDescription(elapsed => $"Running for {elapsed.TotalSeconds:0} second(s)...")
                .ActionAsync(async token =>
                {
                    await Task.Delay(3000, token).ConfigureAwait(false);
                })
                .Run();
            PrintResult(result);

            ShowSection("6) ChangeDescriptionAsync");
            result = PromptPlus.Controls.Task("Working")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .ChangeDescriptionAsync(async elapsed =>
                {
                    await Task.Delay(1).ConfigureAwait(false);
                    return $"Async status at {elapsed:ss} s";
                })
                .ActionAsync(async token =>
                {
                    await Task.Delay(3000, token).ConfigureAwait(false);
                })
                .Run();
            PrintResult(result);

            ShowSection("7) Error handling - task throws (Finish error text)");
            result = PromptPlus.Controls.Task("Risky operation")
                .Spinner(SpinnersType.Default)
                .Finish("Done!", "Operation failed!")
                .Action(token =>
                {
                    Thread.Sleep(1500);
                    throw new InvalidOperationException("Something went wrong");
                })
                .Run();
            PrintResult(result);
            if (result.Content.Exception is not null)
            {
                PromptPlus.Console.WriteLine($"Exception => {result.Content.Exception.Message}");
                PromptPlus.Console.WriteLine(string.Empty);
            }

            ShowSection("8) Styles");
            result = PromptPlus.Controls.Task("Please wait")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .Styles(TaskStyles.Prompt, new Style(Color.Yellow, Color.Black))
                .Styles(TaskStyles.ElapsedTime, new Style(Color.Cyan, Color.Black))
                .Styles(TaskStyles.Spinner, new Style(Color.Green, Color.Black))
                .ActionAsync(async token =>
                {
                    await Task.Delay(2000, token).ConfigureAwait(false);
                })
                .Run();
            PrintResult(result);

            ShowSection("9) Run(token) - cancelable task after 2 seconds");
            using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                result = PromptPlus.Controls.Task("Long task", "Runs with a CancellationToken")
                    .ShowElapsedTime()
                    .Spinner(SpinnersType.Default)
                    .ActionAsync(async token =>
                    {
                        await Task.Delay(TimeSpan.FromSeconds(10), token).ConfigureAwait(false);
                    })
                    .Run(sw.Token);
                PrintResult(result);
            }

            ShowSection("10) Async error - exception thrown inside ActionAsync");
            result = PromptPlus.Controls.Task("Fetching data")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .Finish("Fetched!", "Fetch failed!")
                .ActionAsync(async token =>
                {
                    await Task.Delay(1500, token).ConfigureAwait(false);
                    throw new TimeoutException("Remote server did not respond");
                })
                .Run();
            PrintResult(result);
            if (result.Content.Exception is not null)
            {
                PromptPlus.Console.WriteLine($"Exception => {result.Content.Exception.GetType().Name}: {result.Content.Exception.Message}");
                PromptPlus.Console.WriteLine(string.Empty);
            }

            ShowSection("11) Error with input/output context - partial output before failure");
            var errContext = new Dictionary<string, object?> { ["id"] = 42 };
            result = PromptPlus.Controls.Task("Processing record")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .Finish("Processed!", "Processing failed!")
                .Context(errContext)
                .ActionAsync(async (input, token) =>
                {
                    await Task.Delay(1200, token).ConfigureAwait(false);
                    int id = input.TryGetValue("id", out var raw) && raw is int v ? v : -1;
                    // Validate input and fail when invalid.
                    if (id < 100)
                    {
                        throw new ArgumentOutOfRangeException(nameof(id), $"Id {id} is below the required minimum (100).");
                    }
                    return new Dictionary<string, object?> { ["ok"] = true };
                })
                .Run();
            PrintResult(result);
            if (result.Content.Exception is not null)
            {
                PromptPlus.Console.WriteLine($"Exception => {result.Content.Exception.Message}");
                PromptPlus.Console.WriteLine(string.Empty);
            }

            ShowSection("12) Default error text - no custom error message on Finish");
            result = PromptPlus.Controls.Task("Running")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Default)
                .Action(token =>
                {
                    Thread.Sleep(1000);
                    throw new InvalidOperationException("Unhandled failure");
                })
                .Run();
            PrintResult(result);
            if (result.Content.Exception is not null)
            {
                PromptPlus.Console.WriteLine($"Exception => {result.Content.Exception.Message}");
                PromptPlus.Console.WriteLine(string.Empty);
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

        private static void PrintResult(ResultPrompt<StateTask> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Elapsed: {result.Content.ElapsedTime}, HasError: {result.Content.Exception is not null}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
