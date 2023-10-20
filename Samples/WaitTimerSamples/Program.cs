// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

namespace WaitTimerSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var cult = Thread.CurrentThread.CurrentCulture;
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.KeyPress("Press any key to start", cfg => cfg.ShowTooltip(false))
                .Run();

            PromptPlus.DoubleDash($"Control:WaitTimer - minimal usage");
            PromptPlus.WaitTimer($"wait time", TimeSpan.FromSeconds(2));

            PromptPlus.DoubleDash($"Control:WaitTimer - Countdown usage ");
            PromptPlus.WaitTimer($"wait time", TimeSpan.FromSeconds(2),showCountdown: true);

            PromptPlus.DoubleDash($"Control:WaitTimer - Custom Color usage ");
            PromptPlus.WaitTimer($"wait time", 
                TimeSpan.FromSeconds(2), 
                spinnerStyle: Style.Default.Foreground(Color.Red), 
                elapsedTimeStyle: Style.Default.Foreground(Color.Green),  
                showCountdown: true);


            PromptPlus.DoubleDash($"Control:WaitTimer - with no hide after");
            PromptPlus.WaitTimer($"wait time", TimeSpan.FromSeconds(3), SpinnersType.Ascii, true, (cfg) =>
            {
                cfg.HideAfterFinish(false);
            });

            PromptPlus.DoubleDash($"Control:WaitTimer - SpinnersType usage and option to cancel wait");
            var aux = Enum.GetValues(typeof(SpinnersType));
            foreach (var item in aux)
            {
                if (item.ToString() != SpinnersType.Custom.ToString())
                {
                    var styp = (SpinnersType)Enum.Parse(typeof(SpinnersType), item.ToString()!);
                    PromptPlus.WaitTimer($"wait {item}", TimeSpan.FromSeconds(2), styp, true, (cfg) =>
                    {
                        cfg.EnabledAbortKey(true);
                    });
                }
            }

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
                .Run();
        }
    }
}