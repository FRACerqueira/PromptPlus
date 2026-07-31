// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using PromptPlusLibrary;
using System.Globalization;

namespace SwitchWidgetSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Basic - Switch(true)");
            PromptPlus.Widgets.Switch(true).Show();
            Pause();

            ShowSection("2) Off state - Switch(false)");
            PromptPlus.Widgets.Switch(false).Show();
            Pause();

            ShowSection("3) Custom labels (On/Off)");
            PromptPlus.Widgets.Switch(true)
                .OnValue("Enabled")
                .OffValue("Disabled")
                .Show();
            Pause();

            ShowSection("4) Emoji labels with fallback");
            PromptPlus.Widgets.Switch(false)
                .OnValue(EmojiName.GreenCircle, "ON")
                .OffValue(EmojiName.RedCircle, "OFF")
                .Show();
            Pause();

            ShowSection("5) Styles - custom marker and labels");
            PromptPlus.Widgets.Switch(true)
                .Styles(SwitchStyles.Answer, new Style(Color.Green, Color.Black))
                .Styles(SwitchStyles.Marker, new Style(Color.White, Color.Darkgray))
                .Styles(SwitchStyles.SwitchOn, new Style(Color.Black, Color.Darkgreen))
                .Styles(SwitchStyles.SwitchOff, new Style(Color.Black, Color.Darkred))
                .Show();
            Pause("Press any key to end");
        }

        private static void ConfigureCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.SufixAfterPrompt = string.Empty;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void Pause(string message = "[Yellow]Press any key to continue[/]")
        {
            PromptPlus.Console.WriteLine(string.Empty);
            PromptPlus.Console.WriteLine(message);
            PromptPlus.Console.ReadKey();
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
