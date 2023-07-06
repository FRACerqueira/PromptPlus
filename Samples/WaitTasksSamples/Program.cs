using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace WaitTasksSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            var steps1 = new List<int>
            {
                1, 2, 3, 4, 5 
            };

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.KeyPress("Press any key to start", cfg => cfg.ShowTooltip(false))
                .Run();

            PromptPlus.DoubleDash($"Control:WaitProcess - normal usage sequencial mode");
            var wt1 = PromptPlus.WaitProcess("wait process", "main desc")
                .Finish($"end wait all process")
                .Interaction(steps1, (ctrl, item) =>
                {
                    ctrl.AddStep(StepMode.Sequential, $"id{item}",null,
                            (cts) =>
                            {
                                cts.WaitHandle.WaitOne(TimeSpan.FromSeconds(item));
                            });
                })
                .ShowElapsedTime()
                .AddStep(StepMode.Sequential, "id5-10", "Desc 5 and 10",
                    (cts) =>
                    {
                        cts.WaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                    },
                    (cts) =>
                    {
                        cts.WaitHandle.WaitOne(TimeSpan.FromSeconds(10));
                    })
                .Run();

            if (!wt1.IsAborted)
            {
                foreach (var item in wt1.Value)
                {
                    PromptPlus.WriteLine($"You task {item.Id} - {item.Description}, {item.Status}, {item.ElapsedTime}, {item.StepMode}");
                }
            }

            PromptPlus.KeyPress("Press any key to continue", cfg => cfg.ShowTooltip(false))
                .Run();

            var steps2 = new List<int>
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11, 12, 13
            };


            PromptPlus.DoubleDash($"Control:WaitProcess - normal usage Parallel mode");
            wt1 = PromptPlus.WaitProcess("wait process", "main desc")
                .Finish($"end wait all process")
                .TaskTitle("MyProcess")
                .Interaction(steps2, (ctrl, item) =>
                {
                    ctrl.AddStep(StepMode.Parallel, $"id{item}",null,
                            (cts) =>
                            {
                                cts.WaitHandle.WaitOne(TimeSpan.FromSeconds(item));
                            });
                })
                .ShowElapsedTime()
                .AddStep(StepMode.Parallel, "id5-10", "Desc 5 and 10",
                    (cts) =>
                    {
                        cts.WaitHandle.WaitOne(TimeSpan.FromSeconds(5));
                    },
                    (cts) =>
                    {
                        cts.WaitHandle.WaitOne(TimeSpan.FromSeconds(10));
                    })
                .Run();

            if (!wt1.IsAborted)
            {
                foreach (var item in wt1.Value)
                {
                    PromptPlus.WriteLine($"You task {item.Id} - {item.Description}, {item.Status}, {item.ElapsedTime}, {item.StepMode}");
                }
            }

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }
    }
}