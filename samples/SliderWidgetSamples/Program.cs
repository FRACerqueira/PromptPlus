// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using ConsolePlusLibrary;
using PromptPlusLibrary;

namespace SliderWidgetSamples
{
    internal class Program
    {
        static void Main()
        {
            ConfigureCulture();
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            ShowSection("1) Basic - default range (0..100) with value 70");
            PromptPlus.Widgets.Slider(70).Show();
            Pause();

            ShowSection("2) HideElements - hide delimiters and range");
            PromptPlus.Widgets.Slider(30)
                .HideElements(HideSlider.Delimit | HideSlider.Range)
                .Show();
            Pause();

            ShowSection("3) Custom range - slider(-20, -50, 50)");
            PromptPlus.Widgets.Slider(-20, -50, 50).Show();
            Pause();

            ShowSection("4) Width - wider bar (60 chars)");
            PromptPlus.Widgets.Slider(55)
                .Width(60)
                .Show();
            Pause();

            ShowSection("5) Culture(string) - format value using pt-BR");
            PromptPlus.Widgets.Slider(35.5, 0, 100)
                .Culture("pt-BR")
                .Show();
            Pause();

            ShowSection("6) Culture(CultureInfo) - explicit en-US formatting");
            PromptPlus.Widgets.Slider(35.5, 0, 100)
                .Culture(new CultureInfo("en-US"))
                .Show();
            Pause();

            ShowSection("7) ChangeColor - value-based colors (red/blue/gold)");
            PromptPlus.Widgets.Slider(60)
                .ChangeColor(value =>
                {
                    if (value <= 30)
                    {
                        return new Style(Color.Red, Color.Red);
                    }
                    if (value <= 70)
                    {
                        return new Style(Color.Blue, Color.Blue);
                    }
                    return new Style(Color.Darkgoldenrod, Color.Darkgoldenrod);
                })
                .Show();
            Pause();

            ShowSection("8) ChangeGradient - smooth Green -> Yellow -> Red");
            PromptPlus.Widgets.Slider(85)
                .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
                .Show();
            Pause();

            ShowSection("9) Styles - custom color for answer, slider and range");
            PromptPlus.Widgets.Slider(45)
                .Styles(SliderStyles.Answer, new Style(Color.Green, Color.Black))
                .Styles(SliderStyles.Slider, new Style(Color.Blue, Color.Black))
                .Show();
            Pause();

            ShowSection("10) Fill - all available SliderBarType styles");
            foreach (var type in Enum.GetValues<SliderBarType>())
            {
                PromptPlus.Widgets.Slider(50)
                    .BarType(type)
                    .Show();
                Pause();
            }
        }

        private static void ConfigureCulture()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = Thread.CurrentThread.CurrentCulture;
        }

        private static void ShowSection(string title)
        {
            PromptPlus.Widgets.Dash(title, Color.Yellow, DashOptions.AsciiDoubleBorderUpDown, 1);
        }

        private static void Pause(string message = "[Yellow]Press any key to continue[/]")
        {
            WriteSpacer();
            PromptPlus.Console.WriteLine(message);
            PromptPlus.Console.ReadKey();
            WriteSpacer();
        }

        private static void WriteSpacer()
        {
            PromptPlus.Console.WriteLine(string.Empty);
        }
    }
}
