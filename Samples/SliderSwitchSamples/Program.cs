// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace SliderSwitchSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.WriteLine("Hello, World!");
            //Ensure ValueResult Culture for all controls

            var cult = Thread.CurrentThread.CurrentCulture;

            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");
            PromptPlus.Config.MinimalRender = true;

            PromptPlus.DoubleDash($"Control:SliderSwitch (pt-br) - minimal usage");
            var sdl = PromptPlus
               .SliderSwitch("SliderSwitch")
              .Run();
            if (!sdl.IsAborted)
            {
                PromptPlus.WriteLine($"You Pressed {sdl.Value}");
            }

            PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");
            PromptPlus.DoubleDash($"Control:SliderSwitch (en-us) - minimal usage");
            sdl = PromptPlus
                .SliderSwitch("SliderSwitch")
                .Run();

            if (!sdl.IsAborted)
            {
                PromptPlus.WriteLine($"You Pressed {sdl.Value}");
            }

            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.DoubleDash($"Control:SliderSwitch ({cult.Name}) - Custom OnValue/OffValue");
            PromptPlus
               .SliderSwitch("SliderSwitch")
               .OnValue("custom [green]on[/]")
               .OffValue("custom [red]off[/]")
               .Run();

            PromptPlus.DoubleDash($"Control:SliderSwitch ({cult.Name}) - ChangeColorOn/ChangeColorOff");
            PromptPlus
               .SliderSwitch("SliderSwitch")
               .ChangeColorOn(PromptPlus.StyleSchema.Slider().Foreground(Color.Green))
               .ChangeColorOff(PromptPlus.StyleSchema.Slider().Foreground(Color.Red))
               .Default(true)
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