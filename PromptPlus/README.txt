 ____                             _   ____  _
|  _ \ _ __  ___  _ __ ___  _ __ | |_|  _ \| |_   _ ___
| |_) | '__|/ _ \| '_ ` _ \| '_ \| __| |_) | | | | | __|
|  __/| |  | (_) | | | | | | |_) | |_|  __/| | |_| |__ \
|_|   |_|   \___/|_| |_| |_| .__/ \__|_|   |_|\__,_|___/
                           |_|
**Welcome to PromptPlus**

Interactive command-line toolkit for **C#** with powerful controls.
PromptPlus was developed in c# with the **netstandard2.1, .NET 5 AND .NET6 ** target frameworks, with compatibility for:

- .NET Core 3.1, 5.X, 6.X

**visit the official page for complete documentation** :

https://fracerqueira.github.io/PromptPlus

**Relase Notes (V.2.2.0)**

- Renamed namespace PromptValidators to PromptPlusValidators (requires refactoring)
- Renamed namespace PromptPlusControls.FIGlet to PromptPlusFIGlet (requires refactoring)
- Renamed namespace PromptPlusControls.ValueObjects to PromptPlusObjects (requires refactoring)

- Added Description parameter to all controls
- Added global hotkey (default value = F3) show/hide Description
- Added color Schema Description (default value = ConsoleColor.Cyan)
- Added Description parameter to all controls
- Added Dynamic Description -  DescriptionSelector Method for Description change on each interaction
    - Input-Control
    - AutoComplete-Control
    - Listmasked-Control
    - List-Control
    - MaskEdit-Control
    - Select-Control
    - MultSelect-Control

**Relase Notes (V.2.1.0)**

- Added Product logo/icon
- AutoComplete-Control : **New Control** Input with sugestions 
- New target frameworks: netstandard2.1, .NET 5 AND .NET6
- Masked-control : bug fixed in create mask for number/currency (Wrongly created Group separator)
- Masked-control : bug fixed in Backspace/del for number/currency when not have decimal. 
- Number-Up/Down-control : Added larger-step
- Method Syntax Adjustment (need to be refactored to new syntax):
    Addvalidator -> AddValidator (Input-Control/List-Control/MaskEditList-Control)
    Offvalue -> OffValue, Onvalue -> OnValue (SliderSwitch-Control)

**Sample Usage**
----------------

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
