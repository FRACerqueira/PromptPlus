// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace MultiTasksControlSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Sequential execution");
            var result = PromptPlus.Controls.MultiTasks("Running setup steps")
                .Mode(MultiTasksMode.Sequential)
                .ShowElapsedTime()
                .Spinner(SpinnersType.Dots)
                .AddTaskAsync("Load configuration", async token => await Task.Delay(1200, token))
                .AddTaskAsync("Connect to database", async token => await Task.Delay(1500, token))
                .AddTaskAsync("Warm up cache", async token => await Task.Delay(1000, token))
                .Run();
            PrintResult(result);

            ShowSection("2) Parallel execution (MaxDegreeOfParallelism = 2)");
            result = PromptPlus.Controls.MultiTasks("Downloading files in parallel")
                .Mode(MultiTasksMode.Parallel)
                .MaxDegreeOfParallelism(2)
                .ShowElapsedTime()
                .Spinner(SpinnersType.Dots)
                .AddTaskAsync("file-1.zip", async token => await Task.Delay(2000, token))
                .AddTaskAsync("file-2.zip", async token => await Task.Delay(1200, token))
                .AddTaskAsync("file-3.zip", async token => await Task.Delay(2600, token))
                .AddTaskAsync("file-4.zip", async token => await Task.Delay(800, token))
                .Run();
            PrintResult(result);

            ShowSection("3) Sequential with a failing task (StopOnError)");
            result = PromptPlus.Controls.MultiTasks("Deploy pipeline")
                .Mode(MultiTasksMode.Sequential)
                .StopOnError()
                .AddTaskAsync("Build", async token => await Task.Delay(1200, token))
                .AddTaskAsync("Test", async token =>
                {
                    await Task.Delay(1000, token);
                    throw new InvalidOperationException("2 tests failed");
                })
                .AddTaskAsync("Publish", async token => await Task.Delay(1000, token))
                .Run();
            PrintResult(result);

            ShowSection("4) Parallel with mixed success/failure");
            result = PromptPlus.Controls.MultiTasks("Health checks")
                .Mode(MultiTasksMode.Parallel)
                .AddTaskAsync("api", async token => await Task.Delay(1500, token))
                .AddTaskAsync("worker", async token =>
                {
                    await Task.Delay(1000, token);
                    throw new TimeoutException("worker not responding");
                })
                .AddTaskAsync("storage", async token => await Task.Delay(2000, token))
                .Run();
            PrintResult(result);

            ShowSection("5) Input/Output contexts per task");
            var ctxA = new Dictionary<string, object?> { ["factor"] = 3 };
            var ctxB = new Dictionary<string, object?> { ["factor"] = 5 };
            result = PromptPlus.Controls.MultiTasks("Computing values")
                .Mode(MultiTasksMode.Parallel)
                .AddTaskAsync("compute A", async (input, token) =>
                {
                    await Task.Delay(1200, token);
                    int f = input.TryGetValue("factor", out var v) && v is int n ? n : 1;
                    return new Dictionary<string, object?> { ["value"] = f * 10 };
                }, ctxA)
                .AddTaskAsync("compute B", async (input, token) =>
                {
                    await Task.Delay(1500, token);
                    int f = input.TryGetValue("factor", out var v) && v is int n ? n : 1;
                    return new Dictionary<string, object?> { ["value"] = f * 10 };
                }, ctxB)
                .Run();
            PrintResult(result);
            foreach (var r in result.Content.Results)
            {
                int value = r.GetOutput<int>("value", out bool found);
                PromptPlus.Console.WriteLine($"  {r.Title} => state: {r.State}, value: {(found ? value : -1)}");
            }
            PromptPlus.Console.WriteLine(string.Empty);

            ShowSection("6) Paginated list - many tasks (CPU-aware parallelism)");
            var multi = PromptPlus.Controls.MultiTasks("Processing batch", "Use Up/Down and PageUp/PageDown to scroll")
                .Mode(MultiTasksMode.Parallel)
                .MaxDegreeOfParallelism(0) // 0 = auto based on CPU cores
                .Spinner(SpinnersType.Dots)
                .PageSize(6);
            for (int i = 1; i <= 20; i++)
            {
                int delay = 500 + (i % 5) * 400;
                multi.AddTaskAsync($"job-{i:00}", async token => await Task.Delay(delay, token));
            }
            result = multi.Run();
            PrintResult(result);

            ShowSection("7) Run(token) - cancelable after 2 seconds");
            using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
            {
                result = PromptPlus.Controls.MultiTasks("Long running batch")
                    .Mode(MultiTasksMode.Parallel)
                    .AddTaskAsync("task-1", async token => await Task.Delay(TimeSpan.FromSeconds(10), token))
                    .AddTaskAsync("task-2", async token => await Task.Delay(TimeSpan.FromSeconds(10), token))
                    .Run(sw.Token);
                PrintResult(result);
            }

            ShowSection("8) Per-task mode - ordered sub-sets (seq, parallel block, seq)");
            result = PromptPlus.Controls.MultiTasks("Mixed pipeline", "Order preserved; parallel block runs together")
                .Mode(MultiTasksMode.Sequential) // default
                .Spinner(SpinnersType.Dots)
                // 1) sequential
                .AddTaskAsync("Prepare", async token => await Task.Delay(1000, token))
                // 2-4) consecutive parallel sub-set (run together, wait all)
                .AddTaskAsync("Download A", async token => await Task.Delay(1800, token), mode: MultiTasksMode.Parallel)
                .AddTaskAsync("Download B", async token => await Task.Delay(1200, token), mode: MultiTasksMode.Parallel)
                .AddTaskAsync("Download C", async token => await Task.Delay(2200, token), mode: MultiTasksMode.Parallel)
                // 5) sequential (only starts after the parallel block finished)
                .AddTaskAsync("Finalize", async token => await Task.Delay(1000, token))
                .Run();
            PrintResult(result);

            ShowSection("9) AddTasks + Interaction helpers");
            var services = new[] { "auth", "billing", "notifications" };
            result = PromptPlus.Controls.MultiTasks("Bootstrapping services")
                .Mode(MultiTasksMode.Parallel)
                .Spinner(SpinnersType.Dots)
                // add several parallel tasks at once
                .Interaction(new (string, Func<CancellationToken, Task>)[]
                {
                    ("warmup 1", async t => await Task.Delay(1000, t)),
                    ("warmup 2", async t => await Task.Delay(1400, t)),
                    ("warmup 3", async t => await Task.Delay(900, t)),
                }, (item, ctrl) => ctrl.AddTaskAsync(item.Item1, item.Item2, mode: MultiTasksMode.Parallel))
                // register one sequential task per item using Interaction
                .Interaction(services, (svc, ctrl) =>
                    ctrl.AddTaskAsync($"start {svc}", async t => await Task.Delay(800, t), mode: MultiTasksMode.Sequential))
                .Run();
            PrintResult(result);
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

        private static void PrintResult(ResultPrompt<StateMultiTasks> result)
        {
            var s = result.Content;
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Elapsed: {s.ElapsedTime}, AllSucceeded: {s.AllSucceeded}, AnyFailed: {s.AnyFailed}");
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
