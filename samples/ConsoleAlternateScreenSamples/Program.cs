// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************


using PromptPlusLibrary;

namespace ConsoleAlternateScreenSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample features for Alternate Screen");

            if (PromptPlus.Console.IsEnabledSwapScreen)
            {
                PromptPlus.Console.WriteLine("This console Is Enableded to SwapScreen",Style.Default().ForeGround(Color.LightGreen));
            }
            else
            {
                PromptPlus.Console.WriteLine("This console NOT Enableded to SwapScreen", Style.Default().ForeGround(Color.Red1));
                PromptPlus.Controls.KeyPress("Press any key to end")
                    .Options(x => x.ShowTooltip(false).HideAfterFinish(true))
                    .Run();
                Environment.Exit(0);
            }
            PromptPlus.Controls.KeyPress("Press any key to Switch secondary screen")
                .Options(x => x.ShowTooltip(false).HideAfterFinish(true))
                .Run();

            PromptPlus.Console.OnBuffer(TargetScreen.Secondary,
                (cts) =>
                {
                    PromptPlus.Console.WriteLine("This text run in secondary screen");
                    PromptPlus.Console.WriteLines(2);
                    PromptPlus.Controls.KeyPress("Press any key to Switch Primary screen")
                        .Options(x => x.ShowTooltip(false).HideAfterFinish(true))
                        .Run();
                });

            PromptPlus.Controls.KeyPress("Press any key to end")
               .Options(x => x.ShowTooltip(false).HideAfterFinish(true))
               .Run();
        }
    }
}