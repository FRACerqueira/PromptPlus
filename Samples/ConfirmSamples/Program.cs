// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System.Globalization;
using PPlus;

PromptPlus.WriteLine("Hello, World!");

//Ensure ValueResult Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("pt-br");

PromptPlus.DoubleDash("Control:Confirm with embedding resource 'pr-br'");
var kpptbr = PromptPlus
    .Confirm("Confirm")
    .Run();

if (!kpptbr.IsAborted)
{
    PromptPlus.WriteLine($"You Pressed {kpptbr.Value.Key}");
}

//Ensure ValueResult Culture for all controls
PromptPlus.Config.DefaultCulture = new CultureInfo("en-us");

PromptPlus.DoubleDash("Control:Confirm with embedding resource 'en-us'");
var kpenus = PromptPlus
    .Confirm("Confirm")
    .Run();

if (!kpenus.IsAborted)
{
    PromptPlus.WriteLine($"You Pressed {kpenus.Value.Key}");
}

PromptPlus.DoubleDash("Control:Confirm with fixed \"My-Yes:A\" / \"My-no:B\" char");
PromptPlus
    .Confirm("Confirm", ConsoleKey.A, ConsoleKey.B)
    .Run();

PromptPlus.DoubleDash("Control:Confirm with custom \"yes\"/\"no\"");
PromptPlus
    .Confirm("Confirm [green]1=yes[/], [red]2=no[/]", ConsoleKey.D1, ConsoleKey.D2)
    .TextKeyValid((kp) => 
    {
        if (kp.Key == ConsoleKey.D1)
        {
            return "1";
        }
        else if (kp.Key == ConsoleKey.D2)
        {
            return "2";
        }
        return null;
    })
    .Run();

PromptPlus.WriteLines(2);
PromptPlus.KeyPress("End Sample!, Press any key", cfg => cfg.ShowTooltip(false))
    .Run();

