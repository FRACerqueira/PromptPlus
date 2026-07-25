// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace ProgressBarControlSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Basic ProgressBar");

            var result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateLinear)
                .Run();
            PrintProgressResult(result);


            ShowSection("2) Spinner + custom range + finish text");

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateLinear)
                .Spinner(SpinnersType.Dots)
                .Range(-30, 30)
                .Default(-30)
                .FractionalDigits(1)
                .Finish("End progress")
                .Run();
            PrintProgressResult(result);

            ShowSection("3) Hide selected elements");

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateLinear)
                .HideElements(HideProgressBar.PromptAnswer | HideProgressBar.Range | HideProgressBar.Delimit | HideProgressBar.ProgressbarAtFinish)
                .Run();
            PrintProgressResult(result);

            ShowSection("4) Gradient color");

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateLinear)
                .HideElements(HideProgressBar.ElapsedTime)
                .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                .Run();
            PrintProgressResult(result);


            ShowSection("5) Dynamic color by value");

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateLinear)
                .ChangeColor((value) =>
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
            PrintProgressResult(result);

            ShowSection("6) Dynamic description (sync)");
            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateLinear)
                .ChangeDescription(value => $"Processed: {value:0}%")
                .Run();
            PrintProgressResult(result);

            ShowSection("7) Dynamic description (async)");
            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateLinear)
                .ChangeDescriptionAsync(async value =>
                {
                    await Task.Delay(10);
                    return $"Processed (async): {value:0}%";
                })
                .Run();
            PrintProgressResult(result);

            ShowSection("8) UpdateHandlerAsync");
            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandlerAsync(UpdateLinearAsync)
                .Run();
            PrintProgressResult(result);

            ShowSection("9) Context input/output in handler");
            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(UpdateWithContext, new Dictionary<string, object?>
                {
                    ["step"] = 4,
                    ["tag"] = "context-sample"
                })
                .Run();
            PrintProgressResult(result);

            ShowSection("10) Culture + width + fractional digits");
            result = PromptPlus.Controls.ProgressBar("Wait Progress: ", "Culture: pt-BR")
                .Culture("pt-BR")
                .Width(30)
                .FractionalDigits(2)
                .Range(0, 10)
                .Default(0)
                .UpdateHandler((bar, token) =>
                {
                    while (!token.IsCancellationRequested && !bar.Finish)
                    {
                        token.WaitHandle.WaitOne(120);
                        bar.Update(bar.Value + 0.75);
                    }
                })
                .Run();
            PrintProgressResult(result);

            ShowSection("11) Abort by cancellation token");
            using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(900)))
            {
                result = PromptPlus.Controls.ProgressBar("Wait Progress: ", "Token cancels before finish")
                    .UpdateHandler(UpdateLinear)
                    .Run(cts.Token);
            }
            PrintProgressResult(result);

            ShowSection("12) Abort by handler error");
            result = PromptPlus.Controls.ProgressBar("Wait Progress: ", "Handler emits ErrorAndAbort")
                .UpdateHandler(UpdateWithError)
                .Run();
            PrintProgressResult(result);

            ShowSection("13) All fill styles");

            var typelayout = Enum.GetValues<ProgressBarType>();
            foreach (var type in typelayout)
            {
                result = PromptPlus.Controls.ProgressBar("Wait Progress: ", type.ToString())
                    .UpdateHandler(UpdateLinear)
                    .Fill(type)
                    .Run();
                PrintProgressResult(result);
            }
        }

        private static void UpdateLinear(ProgressBarEvent bar, CancellationToken token)
        {
            while (!token.IsCancellationRequested && !bar.Finish)
            {
                token.WaitHandle.WaitOne(80);
                bar.Update(bar.Value + 2);
            }
        }

        private static async Task UpdateLinearAsync(ProgressBarEvent bar, CancellationToken token)
        {
            while (!token.IsCancellationRequested && !bar.Finish)
            {
                await Task.Delay(80, token).ConfigureAwait(false);
                bar.Update(bar.Value + 2);
            }
        }

        private static void UpdateWithContext(ProgressBarEvent bar, CancellationToken token)
        {
            int step = bar.InputParam<int>("step", out bool hasStep);
            if (!hasStep)
            {
                step = 1;
            }

            string tag = bar.InputParam<string>("tag", out bool hasTag);
            if (!hasTag)
            {
                tag = "n/a";
            }

            while (!token.IsCancellationRequested && !bar.Finish)
            {
                token.WaitHandle.WaitOne(90);
                bar.Update(bar.Value + step);
                bar.AddOutputContext("LastTag", tag);
                bar.AddOutputContext("LastStep", step);
            }
            bar.AddOutputContext("FinishedAt", DateTimeOffset.UtcNow.ToString("O"));
        }

        private static void UpdateWithError(ProgressBarEvent bar, CancellationToken token)
        {
            while (!token.IsCancellationRequested && !bar.Finish)
            {
                token.WaitHandle.WaitOne(100);
                if (bar.Value >= 40)
                {
                    bar.ErrorAndAbort(new InvalidOperationException("Simulated failure after 40%."));
                    return;
                }
                bar.Update(bar.Value + 5);
            }
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void PrintProgressResult(ResultPrompt<StateProgress> result)
        {
            PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Value: {result.Content.FinishedValue}, Elapsed: {result.Content.ElapsedTime}");
            if (!string.IsNullOrWhiteSpace(result.Content.FinishedText))
            {
                PromptPlus.Console.WriteLine($"FinishedText: {result.Content.FinishedText}");
            }
            if (result.Content.ExceptionProgress is not null)
            {
                PromptPlus.Console.WriteLine($"Error: {result.Content.ExceptionProgress.Message}");
            }
            if (result.Content.OutputContext is not null && result.Content.OutputContext.Count > 0)
            {
                PromptPlus.Console.WriteLine("OutputContext:");
                foreach (var item in result.Content.OutputContext)
                {
                    PromptPlus.Console.WriteLine($"  - {item.Key}: {item.Value}");
                }
            }
            PromptPlus.Console.WriteLine("");
        }
    }
}
