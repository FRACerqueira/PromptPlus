// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleWaitTimerControlSamples
{
    internal class Program
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture =  new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;


            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Simple WaitTime", extraLines: 1);

            var result = PromptPlus.Controls.WaitTimer(5000, "Wait timer...")
                .Run();


            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.ToString() : "")}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample WaitTime with ElapsedTime", extraLines: 1);

            result = PromptPlus.Controls.WaitTimer(5000, "Wait timer...")
                .ShowElapsedTime()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.ToString() : "")}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample WaitTime with spinner", extraLines: 1);

            result = PromptPlus.Controls.WaitTimer(5000, "Wait timer...")
                .ShowElapsedTime()
                .Spinner(SpinnersType.Ascii)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.ToString() : "")}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample WaitTime no count down", extraLines: 1);

            result = PromptPlus.Controls.WaitTimer(5000, "Wait timer...")
                .ShowElapsedTime()
                .IsCountDown(false)
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.ToString() : "")}");
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash("Sample WaitTime with show answer", extraLines: 1);

            result = PromptPlus.Controls.WaitTimer(5000, "Wait timer...", null, true)
                .ShowElapsedTime()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.ToString() : "")}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample WaitTime with custom Finish", extraLines: 1);

            result = PromptPlus.Controls.WaitTimer(5000, "Wait timer...",null,true)
                .ShowElapsedTime()
                .Finish("End time")
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.ToString() : "")}");
            PromptPlus.Console.WriteLine("");

        }
    }
}
