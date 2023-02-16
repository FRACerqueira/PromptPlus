 ____                             _   ____  _
|  _ \ _ __  ___  _ __ ___  _ __ | |_|  _ \| |_   _ ___
| |_) | '__|/ _ \| '_ ` _ \| '_ \| __| |_) | | | | | __|
|  __/| |  | (_) | | | | | | |_) | |_|  __/| | |_| |__ \
|_|   |_|   \___/|_| |_| |_| .__/ \__|_|   |_|\__,_|___/
                           |_|
**Welcome to PromptPlus v3.2.2 ** 

Interactive command-line toolkit for **C#** with powerful controls.

PromptPlus was developed in c# with target frameworks:

- netstandard2.1
- .NET 6
- .NET 7

All controls input/filter using **GNU Readline ** https://en.wikipedia.org/wiki/GNU_Readline Emacs keyboard shortcuts.

PromptPlus has **separate pakage** integrate command line parse CommandDotNet(4.3.0/6.0.0):

- PromptPlus.CommandDotNet (V1.0.0.322)!!!

**visit the official pages for complete documentation** :

https://fracerqueira.github.io/PromptPlus
For PromptPlus controls

https://commanddotnet.bilal-fazlani.com/
CommandDotNet is third party applications

**Relase Notes PromptPlus.CommandDotNet (V1.0.0.322)**
-----------------------------------------------------------
- Updated PromptPlus to 3.2.2

**Relase Notes PromptPlus.CommandDotNet (V1.0.0.321)**
-----------------------------------------------------------
- Updated PromptPlus to 3.2.1

**PromptPlus.CommandDotNet - Sample Usage**
-------------------------------------------
public class Program
{
    static int Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        PromptPlus.ConsoleDefaultColor(ConsoleColor.White, ConsoleColor.Black);
        PromptPlus.Clear();

        return new AppRunner<Examples>()
            .UseDefaultMiddleware()
            .UsePrompter()
            .UseNameCasing(Case.KebabCase)
            .UsePromptPlusAnsiConsole()
            .UsePromptPlusArgumentPrompter()
            .UsePromptPlusWizard()
            .UsePromptPlusRepl(colorizeSessionInitMessage: (msg) => msg.Yellow().Underline())
            .Run(args);
    }
}

//for usage AppRunner see https://commanddotnet.bilal-fazlani.com/

**Relase Notes  PromptPlus (V.3.2.2)**
-----------------------------------------------------------
Fixed bug - Banner Control - removed push cursor 
Fixed bug - Base Control - When abort control not execute clear render when HideAfterFinish = true

**Relase Notes  PromptPlus (V.3.2.1)**
-----------------------------------------------------------
Fixed bug - Browser Control - Default value
Fixed bug - MultiSelect Control - Invert select/Select when exist default value

**PromptPlus Controls - Sample Usage**
--------------------------------------

//ASCII text banners
PromptPlus.Banner("PromptPlus")
    .Run(ConsoleColor.Green);

//MaskEdit Generic
var mask = PromptPlus.MaskEdit(MaskedType.Generic, "Inventory Number")
    .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
    .Run(_stopApp);

if (mask.IsAborted)
{
    return;
}
if (string.IsNullOrEmpty(mask.Result.Value))
{
    Console.WriteLine($"your input was empty!");
}
else
{
    Console.WriteLine($"your input was {mask.Result.ObjectValue}!");
}

//AnyKey
var key = PromptPlus.KeyPress()
        .Run(_stopApp);

if (key.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, key pressed");


//input
var name = PromptPlus.Input("What's your name?")
    .Default("Peter Parker")
    .Addvalidator(PromptPlusValidators.Required())
    .Addvalidator(PromptPlusValidators.MinLength(3))
    .Run(_stopApp);

if (name.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, {name.Result}!");

**Supported platforms**
-----------------------

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app
