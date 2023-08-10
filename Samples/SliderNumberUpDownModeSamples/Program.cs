using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace SliderNumberUpDownModeSamples
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

            PromptPlus.DoubleDash($"Your default Culture is {cult.Name}", DashOptions.HeavyBorder, style: Style.Default.Foreground(Color.Yellow));

            PromptPlus.DoubleDash($"Control:SliderNumber {cult} - auto larger/shot step usage");
            var sdl = PromptPlus
               .SliderNumber("SliderNumber","Larger = range/10 Short = range/100")
               .Layout(LayoutSliderNumber.UpDown)
               .Range(0, 10)
               .FracionalDig(1)
               .Run();
            if (!sdl.IsAborted)
            {
                PromptPlus.WriteLine($"You selected is {sdl.Value}");
            }

            PromptPlus.DoubleDash($"Control:SliderNumber {cult} - normal usage");
            PromptPlus
               .SliderNumber("SliderNumber")
               .Layout(LayoutSliderNumber.UpDown)
               .Range(-100, 100)
               .Default(0)
               .FracionalDig(1)
               .Step(0.1)
               .LargeStep(1)
               .Run();

            PromptPlus.DoubleDash($"Control:SliderNumber with culture (pt-br) - normal usage");
            PromptPlus
               .SliderNumber("SliderNumber")
               .Layout(LayoutSliderNumber.UpDown)
               .Range(-100, 100)
               .Culture(new CultureInfo("pt-br"))
               .Default(0)
               .FracionalDig(1)
               .Step(0.1)
               .LargeStep(1)
               .Run();

            PromptPlus.DoubleDash("For other basic features below see - input samples (same behavior)");
            PromptPlus.WriteLine(". [yellow]ChangeDescription[/] - InputBasicSamples");
            PromptPlus.WriteLine(". [yellow]OverwriteDefaultFrom[/] - InputOverwriteDefaultFromSamples");
            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();

        }
    }
}