**Welcome to PromptPlus**

Interactive command-line toolkit for **C#** with powerful controls.
PromptPlus was developed in c# with the **netstandard2.1** target framework, with compatibility for:

- .NET Core 3.1, 5.X, 6.X
- NET Framework 4.8

**visit the official page for complete documentation** :
https://fracerqueira.github.io/PromptPlus

**Relase Notes (This Version)**

**Break changes and behavior (Previous versions need to be refactored to new syntax) ** 
---------------------------------------------------------------------------------------

- Refactored all controls to fluent-interface syntax to improve extensibility points.
- Removed pipeline namespace. Now the pipeline syntax is the same as for controls.
- Removed access to options classes, unified by fluent-interfaces model
- Changed pipeline extension "Step" to fuent-interface method "ToPipe".
- Refactored ConsoleDriver
- Added ansi color for terminal compatibility 
- Refactored render text to use ansi-color and underline

**New controls/Command**
------------------------
- Banner-control                : Show ASCII text banner. 
- screen-control                : Switch screen (principal/alternate Screen)
- StatusBar-control             : Frezeen bottom line(s) with template/columns over alternate Screen
- Write-Command                 : Colored-text forecolor/backcolor and undeline
- WriteLine-Command             : Colored-text forecolor/backcolor, undeline and multiples lines
- Clear-Command                 : Clear screeen with set backcolor
- ClearRestOfLine-Command       : Clear rest of line
- ConsoleDefaultColor-Command   : Set forecolor/backcolor screen

**Enhancements**
----------------
- Select-control        : Added disable and hide items options. 
- Select-control        : Native support of enum types (removed type  EnumValue<T>). 
- MultSelect-control    : Native support of enum types (removed type  EnumValue<T>). 
- MultSelect-control    : Added disable and hide items options.
- MultSelect-control    : Added group of items for quick multiple selection.
- MultSelect-control    : Revised the look to keep the selection symbols.
- ListMasked-control    : Expanded to support all types of MaskEdit-control.
- ListMasked-Control    : Adjusted type return to ResultMasked.
- ListMasked-Control    : Added method to run the validators on each interaction.
- SliderNumber-control  : Adjusted type return to double.
- Input-Control         : Added method to run the validators on each interaction.
- List-Control          : Added method to run the validators on each interaction.
- MaskedInput-Control   : Added method to run the validators on each interaction.
- MaskedInput-Control   : Added method to show day week for mask-type date/datetime.

- Revised documentation for new changes (In progress)

**Fixed bugs**
--------------
- ListMasked-control: bug fixed when deleting selection (not deleting).
- Masked-control : bug fixed in delete/backspace key behaviors

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
    .Addvalidator(PromptValidators.Required())
    .Addvalidator(PromptValidators.MinLength(3))
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
