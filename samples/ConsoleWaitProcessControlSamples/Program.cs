// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleWaitProcessControlSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;


            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample Wait Process", extraLines: 1);

            var result = PromptPlus.Controls.WaitProcess("Wait Process: ")
                .Spinner(SpinnersType.Ascii)
                .ShowElapsedTime()
                .AddTask(TaskMode.Sequential, "id1", (_, token) => { token.WaitHandle.WaitOne(5000); }, "label 1")
                .AddTask(TaskMode.Sequential, "id2", (_, token) => { token.WaitHandle.WaitOne(3000); })
                .AddTask(TaskMode.Parallel, "id3", (_, token) => { token.WaitHandle.WaitOne(1000); }, "label 3")
                .AddTask(TaskMode.Parallel, "id4", (_, token) => { token.WaitHandle.WaitOne(2000); }, "label 4")
                .AddTask(TaskMode.Parallel, "id5", (_, token) => { token.WaitHandle.WaitOne(3000); }, "label 5")
                .AddTask(TaskMode.Parallel, "id6", (_, token) => { token.WaitHandle.WaitOne(4000); }, "label 6")
                .AddTask(TaskMode.Parallel, "id7", (_, token) => { token.WaitHandle.WaitOne(5000); })
                .AddTask(TaskMode.Parallel, "id8", (_, token) => { token.WaitHandle.WaitOne(6000); })
                .AddTask(TaskMode.Parallel, "id9", (_, token) => { token.WaitHandle.WaitOne(7000); })
                .AddTask(TaskMode.Sequential, "id10", (_, token) => { token.WaitHandle.WaitOne(4000); })
                .Run();


            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");
            foreach (var item in result.Content)
            {
                PromptPlus.Console.WriteLine($"ID: {item.Id}, Status: {item.Status} {item.ElapsedTime}");

            }

            PromptPlus.Widgets.DoubleDash("Sample Wait Process with custom Finish", extraLines: 1);

            result = PromptPlus.Controls.WaitProcess("Wait Process: ")
                .Spinner(SpinnersType.Ascii)
                .Finish((_) => "End process")
                .AddTask(TaskMode.Sequential, "id1", (_, token) => { token.WaitHandle.WaitOne(5000); }, "label 1")
                .Run();


            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.Length}");
            foreach (var item in result.Content)
            {
                PromptPlus.Console.WriteLine($"ID: {item.Id}, Status: {item.Status} {item.ElapsedTime}");

            }
        }
    }
}
