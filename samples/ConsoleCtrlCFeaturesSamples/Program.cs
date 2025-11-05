// ***************************************************************************************
// MIT LICENCE
// Copyright 2020 Patrik Svensson, Phil Scott, Nils Andresen.
// https://spectreconsole.net
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using PromptPlusLibrary;

namespace ConsoleCtrlCFeaturesSamples
{
    internal class Program
    {
        static void Main()
        {

            PromptPlus.Console.ResetColor();
            PromptPlus.Console.Clear();

            PromptPlus.Widgets.DoubleDash("Sample features for Ctrl+C not abort application and AbortCurrentControl");

            PromptPlus.Console.CancelKeyPress(AfterCancelKeyPress.AbortCurrentControl, (o, e) =>
            {
                //not abort apppliation
                e.Cancel = true;
            });


            var result = PromptPlus.Controls.Input("Input1 : ", "Please, press Ctrl+C").Run();
            PromptPlus.Console.WriteLine($"Input1 => IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            result = PromptPlus.Controls.Input("Input2 : ").Run();
            PromptPlus.Console.WriteLine($"Input2 => IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Console.WriteLine($"Ctrl+C Press : {PromptPlus.Console.UserPressKeyAborted}");

            PromptPlus.Widgets.DoubleDash("Sample features for Ctrl+C not abort application and AbortAllControl");
            PromptPlus.Console.WriteLine($"[yellow]Please, press Ctrl+C[/]");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Console.CancelKeyPress(AfterCancelKeyPress.AbortAllControl, (o, e) =>
            {
                //not abort apppliation
                e.Cancel = true;
            });

            result = PromptPlus.Controls.Input("Input1 : ", "Please, press Ctrl+C").Run();
            PromptPlus.Console.WriteLine($"Input1 => IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            result = PromptPlus.Controls.Input("Input2 : ").Run();
            PromptPlus.Console.WriteLine($"Input2 => IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Console.WriteLine($"Ctrl+C Press : {PromptPlus.Console.UserPressKeyAborted}");

            PromptPlus.Widgets.DoubleDash("Restored default behavior the current cancel key press (Ctrl+C/Break)");

            PromptPlus.Console.RemoveCancelKeyPress();

            PromptPlus.Console.WriteLine($"Ctrl+C Press : {PromptPlus.Console.UserPressKeyAborted}");

            result = PromptPlus.Controls.Input("InputX : ","Please Ctrl+C").Run();
            PromptPlus.Console.WriteLine($"InputX => IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            result = PromptPlus.Controls.Input("InputY : ").Run();
            PromptPlus.Console.WriteLine($"InputY =>  IsAborted : {result.IsAborted}, Value: {result.Content}");
            PromptPlus.Console.WriteLine("");

            PromptPlus.Widgets.DoubleDash("Now press Ctrl+C to abort application");
            PromptPlus.Console.WriteLine("");
            PromptPlus.Console.WriteLine($"[yellow]Show file log at {Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/PromptPlus.Log [/]");

            if (!PromptPlus.Console.UserPressKeyAborted)
            {
                while (true)
                {
                    Console.WriteLine("Wait press Ctrl+C to abort application...");
                    Thread.Sleep(500);
                }
            }
        }
    }
}
