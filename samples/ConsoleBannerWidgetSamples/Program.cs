// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;

namespace ConsoleBannerWidgetSamples
{
    internal class Program
    {
        static void Main()
        {
            //default Banner
            PromptPlus.Widgets
                .Banner("PromptPlus")
                .Show();

            //with Forecolor
            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets
                .Banner("PromptPlus", Style.Colors(Color.Black, Color.White))
                .Show();

            //Load external font
            PromptPlus.Console.WriteLine("");
            PromptPlus.Widgets.Banner("PromptPlus", Color.Blue)
                .FromFont("starwars.flf")
                .Show();

            var aux = Enum.GetValues<BannerDashOptions>();
            foreach (var item in aux)
            {
                PromptPlus.Console.WriteLine("");
                //with border
                PromptPlus.Widgets.Banner("PromptPlus", Color.Yellow)
                    .Border(item)
                    .Show();
            }

        }
    }
}
