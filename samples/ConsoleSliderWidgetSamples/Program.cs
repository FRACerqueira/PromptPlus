// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleSliderWidgetSamples
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

            PromptPlus.Widgets.DoubleDash("Sample basic slider with 70", extraLines: 1);

            PromptPlus.Widgets.Slider(70)
                .Show();
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample slider(30) with hide elements", extraLines: 1);

            PromptPlus.Widgets.Slider(30)
                .HideElements(HideSlider.Delimit | HideSlider.Range)
                .Show();
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample slider(-20) with custom range", extraLines: 1);

            PromptPlus.Widgets.Slider(-20,-50,50)
                .Show();
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample slider(60) with ChangeColor", extraLines: 1);

            PromptPlus.Widgets.Slider(60)
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
                .Show();
            PromptPlus.Console.WriteLine("");


            PromptPlus.Widgets.DoubleDash("Sample slider(85) with Gradiente color", extraLines: 1);

            PromptPlus.Widgets.Slider(85)
                .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                .Show();
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample slider(50) with all types fill", extraLines: 1);

            var typelayout = Enum.GetValues<SliderBarType>();
            foreach (var type in typelayout)
            {
                PromptPlus.Widgets.Slider(50)
                    .Fill(type)
                    .Show();
                PromptPlus.Console.WriteLine("");
            }
        }
    }
}
