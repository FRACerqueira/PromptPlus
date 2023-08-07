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
            PromptPlus.KeyPress("Press any key to Swith secondary screen", cfg =>
            {
                cfg.ShowTooltip(false);
            })
            .Run();


            PromptPlus.RunOnBuffer(TargetBuffer.Secondary,
                (cts) =>
                {
                    PromptPlus.WriteLine("This text run in secondary screen");
                    PromptPlus.WriteLines(2);
                    PromptPlus.KeyPress("Press any key to Swith primary screen", cfg =>
                    {
                        cfg.ShowTooltip(false);
                    })
                    .Run(cts);
                },
                ConsoleColor.White,
                ConsoleColor.Red);

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