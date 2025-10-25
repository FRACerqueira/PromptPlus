// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleSliderControlSamples
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

            PromptPlus.Widgets.DoubleDash("Sample basic slider", extraLines: 1);

            var result = PromptPlus.Controls.Slider("Select value: ")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content??-1}");


            PromptPlus.Widgets.DoubleDash("Sample basic slider UpDown layout", extraLines: 1);

            result = PromptPlus.Controls.Slider("Select value: ")
                .Layout(SliderLayout.UpDown)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content ?? -1}");

            PromptPlus.Widgets.DoubleDash("Sample slider with hide elements LeftRight layout", extraLines: 1);

            result = PromptPlus.Controls.Slider("Select value: ")
                .HideElements(HideSlider.Delimit)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content ?? -1}");

            PromptPlus.Widgets.DoubleDash("Sample slider with custom range and steps", extraLines: 1);

            result = PromptPlus.Controls.Slider("Select value: ","Custom range")
                .Range(-50, 50)
                .FracionalDig(1)
                .Step(0.5)
                .LargeStep(5)
                .Default(0)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content ?? -1}");

            PromptPlus.Widgets.DoubleDash("Sample slider with ChangeColor", extraLines: 1);

            result = PromptPlus.Controls.Slider("Select value: ")
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
                    return new Style(Color.DarkGoldenrod, Color.DarkGoldenrod);
                })
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content ?? -1}");


            PromptPlus.Widgets.DoubleDash("Sample slider with Gradiente color", extraLines: 1);

            result = PromptPlus.Controls.Slider("Select value: ")
                .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content ?? -1}");

            PromptPlus.Widgets.DoubleDash("Sample slider with History (F3)", extraLines: 1);

            //this code for sample or pre-load History, the control internally carries out this management.
            PromptPlus.Controls.History("TestSliderSample")
                .AddHistory(double.Parse("150").ToString(cult))
                .AddHistory(double.Parse("10.05").ToString(cult))
                .AddHistory(double.Parse("20").ToString(cult))
                .AddHistory(double.Parse("5").ToString(cult))
                .AddHistory(double.Parse("15").ToString(cult))
                .AddHistory(double.Parse("55").ToString(cult))
                .Save();

            result = PromptPlus.Controls.Slider("Select value: ", "Try History")
                .Default(0,true) //Default value is 0, but the history is used to set the default value.
                .FracionalDig(2)
                .Step(0.5)
                .LargeStep(5)
                .EnabledHistory("TestSliderSample")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content ?? -1}");

            //this code for sample. Remove history to persistent storage.
            PromptPlus.Controls.History("TestSliderSample")
                .Remove();

            PromptPlus.Widgets.DoubleDash("Sample slider with all types fill", extraLines: 1);

            var typelayout = Enum.GetValues<SliderBarType>();
            foreach (var type in typelayout)
            {
                result = PromptPlus.Controls.Slider("Select value: ",type.ToString())
                    .Fill(type)
                    .Run();
                PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content ?? -1}");
            }
        }
    }
}
