**Welcome to PromptPlus**

Interactive command-line  toolkit for **C#** with powerful controls.
PromptPlus was developed in c# with the **netstandard2.1** target framework, with compatibility for:

- .NET Core 3.1, 5.X, 6.X
- NET Framework 4.8

**visit the official page for complete documentation** :
https://fracerqueira.github.io/PromptPlus

**Relase Notes (This Version)**

Enhancements
------------
Select control : Enhancement UI to simlify select control when only one item on filter
Readme.txt : Included in package (this file)

**Sample Usage**

//MaskEdit Generic
var mask = PromptPlus.MaskEdit(PromptPlus.MaskTypeGeneric, 
    "Inventory Number", 
    @"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}", 
    cancellationToken: _stopApp);

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
var key = PromptPlus.AnyKey(_stopApp);

if (key.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, key pressed");


//input
var name = PromptPlus.Input(
    "What's your name?", 
    validators: new[] { Validators.Required(), Validators.MinLength(3) });

if (name.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, {name.Result}!");

**Supported platforms**

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app
