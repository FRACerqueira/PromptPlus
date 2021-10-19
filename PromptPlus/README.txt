**Welcome to PromptPlus**

Interactive command-line  toolkit for **C#** with powerful controls.
PromptPlus was developed in c# with the **netstandard2.1** target framework, with compatibility for:

- .NET Core 3.1, 5.X, 6.X
- NET Framework 4.8

**visit the official page for complete documentation** :
https://fracerqueira.github.io/PromptPlus

**Relase Notes (This Version)**

Enhancements / **Break changes and behavior**
-----------------------------------------

- Refactored all controls to fluent-interface syntax to improve extensibility points.
- Adjusted type return to optimize SliderNumber control.
- Removed pipeline namespace. Now the pipeline syntax is the same as for controls.
- Removed access to options classes, unified by fluent-interfaces model
- Changed Type return: ResultMasked for ListMasked Control.
- Expanded ListMasked control to support all types of MaskEdit.
- Simplified Select and Multiselect controls for native support of enum types.
- Changed pipeline extension "Step" to fuent-interface method "AddPipe".
- Revised all documentation for new changes

**Sample Usage**

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
    .Addvalidator(PromptValidators.Required())
    .Addvalidator(PromptValidators.MinLength(3))
    .Run(_stopApp);

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
