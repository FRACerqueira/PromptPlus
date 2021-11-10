# **Welcome to PromptPlus**

Interactive command-line  toolkit for **C#** with powerful controls and commands.
PromptPlus was developed in c# with the **netstandard2.1, .NET 5 AND .NET6 ** target frameworks, with compatibility for:
- .NET Core 3.1, 5.X, 6.X

[**visit the official page for complete documentation**](https://fracerqueira.github.io/PromptPlus/)

## **Sample Usage**

```csharp
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
```

## **Supported platforms**

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

## **License**

This project is licensed under the [MIT License](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
