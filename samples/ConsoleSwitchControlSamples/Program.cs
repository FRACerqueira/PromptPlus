// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleSwitchControlSamples
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

            PromptPlus.Widgets.DoubleDash("Sample basic Switch", extraLines: 1);

            var result = PromptPlus.Controls.Switch("Select value: ")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");

            PromptPlus.Widgets.DoubleDash("Sample basic slider custom values", extraLines: 1);

            result = PromptPlus.Controls.Switch("Select value: ")
                       .OffValue("My Off")
                       .OnValue("My On")
                       .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");

            PromptPlus.Widgets.DoubleDash("Sample basic slider custom color, width and default", extraLines: 1);

            result = PromptPlus.Controls.Switch("Select value: ")
                       .Default(true)
                       .Width(10)
                       .Styles(SwitchStyles.SliderOff, new Style(Color.Red,ConsoleColor.DarkGray ))
                       .Styles(SwitchStyles.SliderOn, new Style(Color.Green, ConsoleColor.DarkGray))
                       .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {result.Content}");
        }
    }
}
