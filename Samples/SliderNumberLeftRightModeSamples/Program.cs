using System.Globalization;
using PPlus;

namespace SliderNumberLeftRightModeSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");
            //Ensure ValueResult Culture for all controls

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.DoubleDash($"Your default Culture is {cult.Name}", DashOptions.HeavyBorder, style: Style.Plain.Foreground(Color.Yellow));

            PromptPlus.DoubleDash($"Control:SliderNumber {cult} - auto larger/shot step usage");
            var sdl = PromptPlus
               .SliderNumber("SliderNumber", "Larger = range/10 Short = range/100")
               .Range(0, 10)
               .FracionalDig(1)
               .Run();
            if (!sdl.IsAborted)
            {
                PromptPlus.WriteLine($"You seleted is {sdl.Value}");
            }

            PromptPlus.DoubleDash($"Control:SliderNumber {cult} - normal usage");
            PromptPlus
               .SliderNumber("SliderNumber")
               .Range(-100, 100)
               .Default(0)
               .FracionalDig(1)
               .Step(0.1)
               .LargeStep(1)
               .Run();

            PromptPlus.DoubleDash($"Control:SliderNumber with culture (pt-br) - normal usage");
            PromptPlus
               .SliderNumber("SliderNumber")
               .Range(-100, 100)
               .Culture(new CultureInfo("pt-br"))
               .Default(0)
               .FracionalDig(1)
               .Step(0.1)
               .LargeStep(1)
               .Run();

            PromptPlus.DoubleDash($"Control:SliderNumber with culture (en-us) - change color usage");
            PromptPlus
               .SliderNumber("SliderNumber","-100~0 = red, 0~40 green, 40~70 yellow, 70~100 blue")
               .Culture(new CultureInfo("en-us"))
               .Range(-100, 100)
               .Default(0)
               .Step(1)
               .LargeStep(10)
               .ChangeColor((value) => 
               { 
                   if (value <  0)
                   {
                       return Color.Red;
                   }
                   if (value < 40)
                   {
                       return Color.Green;
                   }
                   if (value < 70)
                   {
                       return Color.Yellow;
                   }
                   return Color.Blue;
               })
               .Run();

            PromptPlus.DoubleDash($"Control:SliderNumber with culture (en-us) - gradient color usage");
            PromptPlus
               .SliderNumber("SliderNumber")
               .Culture(new CultureInfo("en-us"))
               .Range(-100, 100)
               .Default(100)
               .Step(1)
               .LargeStep(10)
               .ChangeGradient(Color.Red,Color.Yellow,Color.Green)
               .Run();

            PromptPlus.DoubleDash("For other basic features below see - input samples (same behaviour)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}