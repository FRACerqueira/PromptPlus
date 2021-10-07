# **Welcome to PromptPlus**

Interactive command-line  toolkit for **C#** with powerful controls.
PromptPlus was developed in c# with the **netstandard2.1** target framework, with compatibility for:
- .NET Core 3.1, 5.X, 6.X
- NET Framework 4.8

[**visit the official page for complete documentation**](https://fracerqueira.github.io/PromptPlus/)

## **Sample Usage**

```csharp
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
```

## **List Controls**

| controls | Details |
| --- | --- |
| [Any key](https://fracerqueira.github.io/PromptPlus/anykey) | Simple any key press |
| [Key Press](https://fracerqueira.github.io/PromptPlus/keypress) | Simple specific key |
| [Confirm](https://fracerqueira.github.io/PromptPlus/confirm) | Simple confirm with  with tool tips and language detection |
| [Input](https://fracerqueira.github.io/PromptPlus/input) | Input text with input validator with tooltips |
| [Password](https://fracerqueira.github.io/PromptPlus/password) | Input password with input validator and show/hide(optional) input value |
| [MaskEdit Generic](https://fracerqueira.github.io/PromptPlus/maskeditgeneric) | Input with masked input , tooltips and input validator |
| [MaskEdit Date](https://fracerqueira.github.io/PromptPlus/maskeditdate) | Date input with language parameter, tooltips and input validator |
| [MaskEdit Time](https://fracerqueira.github.io/PromptPlus/maskeditdate) | Time input with language parameter, tooltips and input validator |
| [MaskEdit Date and Time](https://fracerqueira.github.io/PromptPlus/maskeditdate) | Date and time input with language parameter, tooltips and input validator |
| [MaskEdit Number](https://fracerqueira.github.io/PromptPlus/maskeditnumber) | Numeric input with language parameter, tooltips and input validator |
| [MaskEdit Currency](https://fracerqueira.github.io/PromptPlus/maskeditnumber) | Currency input with language parameter, tooltips and input validator |
| [Select](https://fracerqueira.github.io/PromptPlus/select)| Generic select input IEnumerable/Enum with auto-paginator and tooltips and more |
| [MultiSelect](https://fracerqueira.github.io/PromptPlus/multiselect) | Generic multi select input IEnumerable/Enum with auto-paginator , tooltips and more |
| [List](https://fracerqueira.github.io/PromptPlus/list) | Create Generic IEnumerable with auto-paginator, tooptip , input validator, message error by type/format and more |
| [ListMasked](https://fracerqueira.github.io/PromptPlus/listmasked) | Create generic IEnumerable with masked input, auto-paginator, tooptip , input validator |
| [Browser](https://fracerqueira.github.io/PromptPlus/browser) | Browser files/folder with auto-paginator and tooltips |
| [Slider Number](https://fracerqueira.github.io/PromptPlus/slidernumber) | Numeric ranger with short/large step and tooltips |
| [Number Up/Down](https://fracerqueira.github.io/PromptPlus/numberupdown) | Numeric ranger with step and tooltips |
| [Slider Switche](https://fracerqueira.github.io/PromptPlus/sliderswitche) | Generic choice with customization and tooltips |
| [Progress Bar](https://fracerqueira.github.io/PromptPlus/progressbar) | Progress Bar with interation customization |
| [WaitProcess](https://fracerqueira.github.io/PromptPlus/waitprocess) | Wait process with animation |
| [PipeLine](https://fracerqueira.github.io/PromptPlus/pipeline) | Pipeline sequence to all prompts with condition by pipe and status summary |

## **Supported platforms**

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

## **License**

This project is licensed under the [MIT License](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
