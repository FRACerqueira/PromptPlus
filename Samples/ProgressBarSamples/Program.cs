using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace ProgressBarSamples
{
    internal class Program
    {
        private static void MyPrgBarHandler(UpdateProgressBar<object> prgbar, CancellationToken token)
        {
            while (!token.IsCancellationRequested && !prgbar.Finish)
            {
                token.WaitHandle.WaitOne(30);
                prgbar.Update(prgbar.Value + 1);
                prgbar.ChangeDescription($"Desc {prgbar.Value}");
                if (prgbar.Value >= prgbar.Maxvalue)
                {
                    break;
                }
            }
            prgbar.Finish = true;
        }

        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.KeyPress("Press any key to start", cfg => cfg.ShowTooltip(false))
                .Run();

            PromptPlus.DoubleDash("Control:ProgressBar - minimal usage");

            var result = PromptPlus.ProgressBar(ProgressBarType.Fill, "ProgressBar", new object(), "My Description")
                .UpdateHandler(MyPrgBarHandler)
                .Run();

            if (!result.IsAborted)
            {
                PromptPlus.WriteLine($"You progress is {result.Value.Lastvalue}");
            }


            PromptPlus.DoubleDash("Control:ProgressBar - Finish usage");

            var obj = new object();
            PromptPlus.ProgressBar(ProgressBarType.Fill, "ProgressBar", new object(), "My Description")
                .UpdateHandler(MyPrgBarHandler)
                .Finish("End process")
                .Run();

            PromptPlus.DoubleDash("Control:ProgressBar - Spinner usage");

            PromptPlus.ProgressBar(ProgressBarType.Fill, "ProgressBar", new object(), "My Description")
                .UpdateHandler(MyPrgBarHandler)
                .Spinner(SpinnersType.BouncingBar)
                .Run();

            PromptPlus.ProgressBar(ProgressBarType.Fill, "ProgressBar", new object(), "My Description")
                 .UpdateHandler(MyPrgBarHandler)
                 .HideElements(HideProgressBar.Percent)
                 .Run();

            PromptPlus.DoubleDash("Control:ProgressBar - HideDelimit usage");

            PromptPlus.ProgressBar(ProgressBarType.Fill, "ProgressBar", new object(), "My Description")
                .UpdateHandler(MyPrgBarHandler)
                .HideElements(HideProgressBar.Delimit)
                .Run();

            PromptPlus.DoubleDash("Control:ProgressBar - HideRanger usage");

            PromptPlus.ProgressBar(ProgressBarType.Fill, "ProgressBar", new object(), "My Description")
                .UpdateHandler(MyPrgBarHandler)
                .HideElements(HideProgressBar.Ranger)
                .Run();

            PromptPlus.DoubleDash("Control:ProgressBar - ChangeColor usage");

            PromptPlus.ProgressBar(ProgressBarType.Fill, "ProgressBar", new object(), "My Description")
                .UpdateHandler(MyPrgBarHandler)
                .ChangeColor((value) => 
                {
                    if (value <= 20)
                    {
                        return Style.Plain.Foreground(Color.Violet);
                    }
                    else if (value <= 60)
                    {
                        return Style.Plain.Foreground(PromptPlus.StyleSchema.Slider().Foreground);
                    }
                    else if (value <= 80)
                    {
                        return Style.Plain.Foreground(Color.Yellow);
                    }
                    return Style.Plain.Foreground(Color.Red);
                })
                .Run();

            var auxb = Enum.GetValues(typeof(ProgressBarType));
            foreach (var item in auxb)
            {
                var bt = (ProgressBarType)Enum.Parse(typeof(ProgressBarType), item.ToString()!);
                PromptPlus.DoubleDash($"Control:ProgressBar - ProgressBarType {bt} and Gradient mode");
                var prb = PromptPlus.ProgressBar(bt, "ProgressBar", new object())
                            .UpdateHandler(MyPrgBarHandler)
                            .CharBar('+')
                            .HideElements(HideProgressBar.Delimit | HideProgressBar.Ranger)
                            .Finish("My Task done")
                            .FracionalDig(2)
                            .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                            .Run();
            }

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}