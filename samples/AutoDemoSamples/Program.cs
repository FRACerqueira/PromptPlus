// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace AutoDemoSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;

            // Gives time to select the recording area/window before the demo starts.
            Console.ReadKey();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            // Demo mode is opt-in and additive: while disabled, every control behaves exactly
            // as with real keyboard input. Enqueue keys immediately before each .Run() call —
            // Run() only returns after consuming its own Enter, so ordering across controls
            // stays correct without any extra synchronization.
            PromptPlus.Console.DemoModeEnabled = true;
            PromptPlus.Console.ScriptedDelayMs = 180;

            ShowSection("1) Input");
            PromptPlus.Console.EnqueueText("Fulano", delayMs: 500);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter, delayMs: 500);
            var nome = PromptPlus.Controls.Input("Nome").Run();
            PromptPlus.Console.WriteLine($"IsAborted: {nome.IsAborted}, Value: {nome.Content}");
            PromptPlus.Console.WriteLine("");

            ShowSection("2) Select");
            PromptPlus.Console.EnqueueKey(ConsoleKey.DownArrow,delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.DownArrow,delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter, delayMs: 800);
            var cor = PromptPlus.Controls.Select<string>("Cor").AddItems(["Vermelho", "Verde", "Azul"]).Run();
            PromptPlus.Console.WriteLine($"IsAborted: {cor.IsAborted}, Value: {cor.Content}");
            PromptPlus.Console.WriteLine("");

            ShowSection("3) MultiSelect");
            PromptPlus.Console.EnqueueKey(ConsoleKey.Spacebar,delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.DownArrow,delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Spacebar,delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter,delayMs: 800);
            var tags = PromptPlus.Controls.MultiSelect<string>("Tags").AddItems(["A", "B", "C"]).Run();
            PromptPlus.Console.WriteLine($"IsAborted: {tags.IsAborted}, Value: {string.Join(", ", tags.Content ?? [])}");
            PromptPlus.Console.WriteLine("");

            ShowSection("4) MaskEdit (date)");
            // Field order follows the current culture's short date pattern (en-US: Month/Day/Year).
            PromptPlus.Console.EnqueueText("12312025", delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter,delayMs: 800);
            var data = PromptPlus.Controls.MaskDate("Data").Run();
            PromptPlus.Console.WriteLine($"IsAborted: {data.IsAborted}, Value: {data.Content:d}");
            PromptPlus.Console.WriteLine("");

            ShowSection("5) Input with suggestions (auto-complete: \"dev\", \"test\", \"staging\", \"prod\", \"sandbox\")");
            PromptPlus.Console.EnqueueText("s", delayMs: 300);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Tab, delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Tab, delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter, delayMs: 800);
            var envAuto = PromptPlus.Controls.Input("Environment", "TAB accepts the first matching suggestion")
                .SuggestionHandler(input =>
                {
                    var values = new[] { "dev", "test", "staging", "prod", "sandbox" };
                    return string.IsNullOrWhiteSpace(input)
                        ? values
                        : [.. values.Where(x => x.StartsWith(input, StringComparison.OrdinalIgnoreCase))];
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {envAuto.IsAborted}, Value: {envAuto.Content}");
            PromptPlus.Console.WriteLine("");

            ShowSection("6) Input with suggestions (not auto-complete)");
            PromptPlus.Console.EnqueueText("s");
            PromptPlus.Console.EnqueueKey(ConsoleKey.Tab, delayMs: 1500);
            PromptPlus.Console.EnqueueKey(ConsoleKey.DownArrow, delayMs: 800);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter, delayMs: 800);
            var envManual = PromptPlus.Controls.Input("Environment", "TAB only rotates the suggestion, does not autofill")
                .SuggestionHandler(input =>
                {
                    var values = new[] { "dev", "test", "staging", "prod", "sandbox" };
                    return string.IsNullOrWhiteSpace(input)
                        ? values
                        : [.. values.Where(x => x.StartsWith(input, StringComparison.OrdinalIgnoreCase))];
                }, false)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {envManual.IsAborted}, Value: {envManual.Content}");
            PromptPlus.Console.WriteLine("");

            ShowSection("7) MultiTasks");
            // A "live" control: it completes on its own tasks finishing, no key presses needed —
            // it already runs under redirected input today, demo mode or not.
            var tasks = PromptPlus.Controls.MultiTasks("Running setup steps")
                .Mode(MultiTasksMode.Sequential)
                .ShowElapsedTime()
                .Spinner(SpinnersType.Dots)
                .AddTaskAsync("Load configuration", async token => await Task.Delay(2000, token), MultiTasksMode.Parallel)
                .AddTaskAsync("Connect to database", async token => await Task.Delay(5000, token), MultiTasksMode.Parallel)
                .AddTaskAsync("Warm up cache", async token => await Task.Delay(7000, token), MultiTasksMode.Parallel)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {tasks.IsAborted}, AllSucceeded: {tasks.Content.AllSucceeded}");
            PromptPlus.Console.WriteLine("");

            ShowSection("8) ProgressBar with gradient");
            // Also a "live" control: it completes when the update handler sets Finish, no key
            // presses needed — runs under redirected input today, demo mode or not.
            var progress = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                .UpdateHandler((bar, token) =>
                {
                    while (!token.IsCancellationRequested && !bar.Finish)
                    {
                        token.WaitHandle.WaitOne(100);
                        bar.Update(bar.Value + 5);
                    }
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {progress.IsAborted}, Value: {progress.Content.FinishedValue}");
            PromptPlus.Console.WriteLine("");

            ShowSection("9) Slider with gradient");
            PromptPlus.Console.EnqueueKeys(100,
                [.. Enumerable.Repeat(new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false), 70)]);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter, delayMs: 800);
            var slider = PromptPlus.Controls.Slider("Select value")
                .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {slider.IsAborted}, Value: {slider.Content ?? -1}");
            PromptPlus.Console.WriteLine("");

            ShowSection("10) ChartBar with labels");
            PromptPlus.Console.EnqueueKey(ConsoleKey.DownArrow, delayMs: 1500);
            PromptPlus.Console.EnqueueKey(ConsoleKey.DownArrow, delayMs: 1500);
            PromptPlus.Console.EnqueueKey(ConsoleKey.Enter, delayMs: 1500);
            var chart = PromptPlus.Controls.ChartBar("Select item")
                .Title("Sales by Region", TextAlignment.Center)
                .AddItem("North", 120)
                .AddItem("South", 80)
                .AddItem("East", 95)
                .ShowLegends(true)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted: {chart.IsAborted}, Value: {chart.Content?.Label}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Console.DemoModeEnabled = false;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Console.WriteLine();
            PromptPlus.Console.Dash(title);
        }
    }
}
