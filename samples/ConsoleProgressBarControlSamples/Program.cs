// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleProgressBarControlSamples
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

            PromptPlus.Widgets.DoubleDash("Sample basic ProgressBar", extraLines: 1);

            var result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(MyProgress)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FinishedValue!}% {result.Content.ElapsedTime}");


            PromptPlus.Widgets.DoubleDash("Sample ProgressBar with spinner, custom Ranger and text finish", extraLines: 1);

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(MyProgress)
                .Spinner(SpinnersType.Dots)
                .Range(-30,30)
                .Finish("End progress")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FinishedValue!}% {result.Content.ElapsedTime}");

            PromptPlus.Widgets.DoubleDash("Sample ProgressBar with hide elements", extraLines: 1);

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(MyProgress)
                .HideElements(HideProgressBar.PromptAnswer | HideProgressBar.Range | HideProgressBar.Delimit | HideProgressBar.ProgressbarAtFinish)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FinishedValue!}% {result.Content.ElapsedTime}");

            PromptPlus.Widgets.DoubleDash("Sample ProgressBar with Gradient color", extraLines: 1);

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(MyProgress)
                .HideElements(HideProgressBar.ElapsedTime)
                .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FinishedValue!}% {result.Content.ElapsedTime}");


            PromptPlus.Widgets.DoubleDash("Sample ProgressBar with ChangeColor", extraLines: 1);

            result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
                .UpdateHandler(MyProgress)
                .ChangeColor((value) =>
                {
                    if (value <= 30)
                    {
                        return new Style(Color.Red,Color.Red);
                    }
                    if (value <= 70)
                    {
                        return new Style(Color.Blue, Color.Blue);
                    }
                    return new Style(Color.DarkGoldenrod, Color.DarkGoldenrod);
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FinishedValue!}% {result.Content.ElapsedTime}");

            PromptPlus.Widgets.DoubleDash("Sample ProgressBar with all types fill", extraLines: 1);

            var typelayout = Enum.GetValues<ProgressBarType>();
            foreach (var type in typelayout)
            {
                result = PromptPlus.Controls.ProgressBar("Wait Progress: ", type.ToString())
                    .UpdateHandler(MyProgress)
                    .Fill(type)
                    .Run();
                PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content.FinishedValue!}% {result.Content.ElapsedTime}");

            }
        }

        private static void MyProgress(HandlerProgressBar bar, CancellationToken token)
        {
            while (!token.IsCancellationRequested || !bar.Finish)
            {
                token.WaitHandle.WaitOne(250);
                bar.Update(bar.Value + 1);
            }
        }
    }
}
