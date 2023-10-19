// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PPlus;
using PPlus.Controls;

namespace AlternateScreenSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {

            PromptPlus.DoubleDash($"PromptPlus AlternateScreen");
            PromptPlus.SetCursorPosition(0,PromptPlus.CursorTop+1);
            PromptPlus.KeyPress("Press any key to Switch secondary screen", cfg =>
            {
                cfg.ShowTooltip(false);
            })
            .Run();


            var hassecondscreen = PromptPlus.RunOnBuffer(TargetBuffer.Secondary,
                (cts) =>
                {
                    PromptPlus.WriteLine("This text run in secondary screen");
                    PromptPlus.WriteLines(2);
                    PromptPlus.KeyPress("Press any key to Switch primary screen", cfg =>
                    {
                        cfg.ShowTooltip(false);
                    })
                    .Run(cts);
                },
                ConsoleColor.White,
                ConsoleColor.Red);
            if (!hassecondscreen)
                //faça algo

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg =>
            {
                cfg.ShowTooltip(false);
                cfg.ApplyStyle(StyleControls.Tooltips, Style.Default.Foreground(Style.Default.Background.GetInvertedColor()));
            })
            .Run();

        }
    }
}