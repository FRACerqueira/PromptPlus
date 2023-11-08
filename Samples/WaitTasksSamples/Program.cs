// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace WaitTasksSamples
{
    internal class Program
    {
        private class MyClass
        {
            public int MyProperty { get; set; }
        }

        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            var steps1 = new List<int>
            {
                1, 2, 3, 3, 3
            };

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.KeyPress("Press any key to start", cfg => cfg.ShowTooltip(false))
                .Run();

            PromptPlus.DoubleDash($"Control:WaitProcess - normal usage sequencial mode");
            var seq = 0;
            var wt1 = PromptPlus.WaitProcess<object>("wait process", "main desc")
                .Finish($"end wait all process")
                .Interaction(steps1, (ctrl, item) =>
                {
                    ctrl.AddStep(StepMode.Sequential, $"id{item}_{seq}",null,
                            (eventw, cts) =>
                            {
                                Task.Delay(TimeSpan.FromSeconds(2), cts).Wait(cts);
                            });
                    seq++;
                })
                .ShowElapsedTime()
                .AddStep(StepMode.Sequential, "id2-4", "Desc 4 and 2",
                    (eventw, cts) =>
                    {
                        Task.Delay(TimeSpan.FromSeconds(2), cts).Wait(cts);
                    },
                    (eventw, cts) =>
                    {
                        Task.Delay(TimeSpan.FromSeconds(4), cts).Wait(cts);
                    })
                .Run();

            foreach (var item in wt1.Value.States)
            {
                PromptPlus.WriteLine($"You task {item.Id} - {item.Description}, {item.Status}, {item.ElapsedTime}, {item.StepMode}");
            }

            PromptPlus.KeyPress("Press any key to continue", cfg => cfg.ShowTooltip(false))
                .Run();

            var steps2 = new List<int>
            {
                1, 2, 3, 4, 5, 6, 7
            };

            PromptPlus.DoubleDash($"Control:WaitProcess - Custom Color");

            var mycontext = new MyClass();
            var wt2 = PromptPlus.WaitProcess<MyClass>("wait process", "main desc")
                .Context(mycontext)
                .Finish($"end wait all process")
                .TaskTitle("MyProcess")
                .MaxDegreeProcess(4)
                .Interaction(steps2.Take(5), (ctrl, item) =>
                {
                    ctrl.AddStep(StepMode.Parallel, $"id{item}", null,
                            (eventw, cts) =>
                            {
                                eventw.ChangeContext((context) => 
                                {
                                    context.MyProperty++;
                                });
                                Task.Delay(TimeSpan.FromSeconds(item), cts).Wait(cts);
                            });
                })
                .ShowElapsedTime()
                .Styles(WaitStyles.Lines, Style.Default.Foreground(Color.Red))
                .Styles(WaitStyles.TaskTitle, Style.Default.Foreground(Color.Blue))
                .Styles(WaitStyles.TaskElapsedTime, Style.Default.Foreground(Color.Green))
                .Run();


            PromptPlus.WriteLine($"You context values is {mycontext.MyProperty}");
            foreach (var item in wt2.Value.States)
            {
                PromptPlus.WriteLine($"You task {item.Id} - {item.Description}, {item.Status}, {item.ElapsedTime}, {item.StepMode}");
            }


            PromptPlus.DoubleDash($"Control:WaitProcess - normal usage Parallel mode");
            wt1 = PromptPlus.WaitProcess<object>("wait process", "main desc")
                .Finish($"end wait all process")
                .TaskTitle("MyProcess")
                .MaxDegreeProcess(4)
                .Interaction(steps2, (ctrl, item) =>
                {
                    ctrl.AddStep(StepMode.Parallel, $"id{item}",null,
                            (eventw, cts) =>
                            {
                                Task.Delay(TimeSpan.FromSeconds(item), cts).Wait(cts);
                            });
                })
                .ShowElapsedTime()
                .AddStep(StepMode.Parallel, "id2-5", "Desc 2 and 5",
                    (eventw, cts) =>
                    {
                        Task.Delay(TimeSpan.FromSeconds(2), cts).Wait(cts);
                    },
                    (eventw, cts) =>
                    {
                        Task.Delay(TimeSpan.FromSeconds(5), cts).Wait(cts);
                    })
                .Run();

            if (!wt1.IsAborted)
            {
                foreach (var item in wt1.Value.States)
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