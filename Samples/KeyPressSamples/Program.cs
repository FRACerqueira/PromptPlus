// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;
using PPlus.Controls;

PromptPlus.WriteLine("Hello, World!");
//Ensure ValueResult Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

PromptPlus.DoubleDash("Control:Keypress - anykey");
var kp1 = PromptPlus
    .KeyPress()
    .Run();

if (!kp1.IsAborted)
{
    PromptPlus.WriteLine($"You Pressed {kp1.Value.Key}");
}

PromptPlus.DoubleDash("Control:Keypress - anykey - Ignore ESC");
PromptPlus
    .KeyPress()
    .Config((cfg) => cfg.EnabledAbortKey(false))
    .Run();

PromptPlus.DoubleDash("Control keypress - with sppiner");
PromptPlus
    .KeyPress()
    .Spinner(SpinnersType.DotsScrolling)
    .Run();

PromptPlus.DoubleDash("Control:Keypress - HideAfter Finish");
PromptPlus
    .KeyPress()
    .Config(cfg => cfg.HideAfterFinish(true))
    .Run();

PromptPlus.DoubleDash("Control:Keypress - with color");
PromptPlus
    .KeyPress("Custom [Blue]Prompt[/] and [yellow]description[/]","[Red]My[/] Description")
    .Run();

PromptPlus.DoubleDash("Control:Keypress - valid keys");
PromptPlus
    .KeyPress()
    .AddKeyValid(ConsoleKey.A)
    .AddKeyValid(ConsoleKey.B, ConsoleModifiers.Control)
    .Run();

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();