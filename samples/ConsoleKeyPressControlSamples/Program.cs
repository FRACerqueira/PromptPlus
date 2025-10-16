// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PromptPlusLibrary;

namespace ConsoleKeyPressControlSamples
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

            PromptPlus.Widgets.DoubleDash("Sample KeyPress", extraLines: 1);

            var result = PromptPlus.Controls.KeyPress("Press any key: ")
                .Run();
            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue? result.Content.Value.Key.ToString():"")}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample KeyPress with spinner", extraLines: 1);

            result = PromptPlus.Controls.KeyPress("Press any key: ")
              .Spinner(SpinnersType.Bounce)
              .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.Key.ToString() : "")}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Sample KeyPress with (Y)es/(N)o mode", extraLines: 1);

            result = PromptPlus.Controls.Confirm("Confirm key: ")
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .Run();

            PromptPlus.Widgets.DoubleDash("Sample KeyPress with valid keys and not hide", extraLines: 1);

            PromptPlus.Widgets.DoubleDash("Sample KeyPress with (S)im/(N)ão mode by culture(pt-BR)", extraLines: 1);

            PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
            result = PromptPlus.Controls.Confirm("Confirmar : ")
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .Run();
            PromptPlus.Config.DefaultCulture = cult;

            PromptPlus.Widgets.DoubleDash("Sample KeyPress with valid keys and not hide", extraLines: 1);

            result = PromptPlus.Controls.KeyPress("Press any key: ")
                .Options((opt) => opt.HideAfterFinish(false).HideOnAbort(false))
                .AddKeyValid(ConsoleKey.A)
                .AddKeyValid(ConsoleKey.B, ConsoleModifiers.Control)
                .AddKeyValid(ConsoleKey.N, showtext: "Off")
                .AddKeyValid(ConsoleKey.Y, showtext: "On")
                .ShowInvalidKey()
                .Run();

            PromptPlus.Console.WriteLine($"IsAborted : {result.IsAborted}, Value: {(result.Content.HasValue ? result.Content.Value.Key.ToString() : "")}");
            PromptPlus.Console.WriteLine("");

        }
    }
}
