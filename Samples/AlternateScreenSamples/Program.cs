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

            PromptPlus.AlternateScreen()
                .ForegroundColor(ConsoleColor.White)
                .BackgroundColor(ConsoleColor.Red)
                .CustomAction((cts) =>
                {
                    PromptPlus.WriteLine("This text run in secondary screen");
                    PromptPlus.WriteLines(2);
                    PromptPlus.KeyPress("Press any key to Swith primary screen", cfg =>
                    {
                        cfg.ShowTooltip(false);
                    })
                    .Run();
                 })
                .Run();

            PromptPlus.WriteLines(2);
            PromptPlus.KeyPress("End Sample!, Press any key", cfg =>
            {
                cfg.ShowTooltip(false);
                cfg.ApplyStyle(StyleControls.Tooltips, Style.Plain.Foreground(Style.Plain.Background.GetInvertedColor()));
            })
            .Run();

        }
    }
}