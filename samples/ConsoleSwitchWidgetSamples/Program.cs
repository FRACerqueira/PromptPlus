// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleSwitchWidgetSamples
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

            PromptPlus.Widgets.Switch(true)
                .Show();

            PromptPlus.Widgets.DoubleDash("Sample basic Switch custom values", extraLines: 1);

            PromptPlus.Widgets.Switch(false, "My On", "My Off")
                .Show();

            PromptPlus.Widgets.DoubleDash("Sample basic Switch custom color and width", extraLines: 1);

            PromptPlus.Widgets.Switch(true)
                       .Width(10)
                       .Styles(SwitchStyles.SliderOff, new Style(Color.Red,ConsoleColor.DarkGray ))
                       .Styles(SwitchStyles.SliderOn, new Style(Color.Green, ConsoleColor.DarkGray))
                       .Show();
        }
    }
}
