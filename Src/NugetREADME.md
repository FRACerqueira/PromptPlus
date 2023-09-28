# **Welcome to PromptPlus**

**Interactive command-line toolkit for .NET Core with powerful controls and commands to create professional console applications.**

All controls input/filter (except Masked input) using [**GNU Readline**](https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts.  

PromptPlus **Supports 4/8/24-bit colors** in the terminal with auto-detection of the current terminal's capabilities and automatic color conversion.

#### [Visit the official page for complete documentation of PromptPlus](https://fracerqueira.github.io/PromptPlus)

**PromptPlus** was developed in C# with the **netstandard2.1**, **.NET 6** and **.NET 7** target frameworks.

## What's new in V4.1.0

- New Control : Table<T> , Display data in a grid-table   
    - Samples in project [Table Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/TableSamples)
    - Main features :
        - More than 100 layout combinations
        - Navigation by row and columns
        - Scroll the table when it is larger than the screen
        - Scroll text when it is larger than the column size
        - Automatic header and column completion
        - Color customization of each element
        - Search for data filtered by columns
        - Formatting by column or by data type definition
- Improvement commands with default values ​​(all controls)
- Bug fixed: grouped item ordering. The sort option will be ignored
- Bug fixed: 'AcceptInput' method causes failure by not allowing navigation keys to be selected.
    - Affeted Controls : AddtoList/Input
- Improvement : Direct writes to standard error output stream
    - New Commands : OutputError()
    - Sample with commemts in project [ConsoleFeatures Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/ConsoleFeaturesSamples)
- New feature: Escaping format characters color 
    - Global property : IgnoreColorTokens
    - New Commands : EscapeColorTokens()/AcceptColorTokens()
    - Sample with commemts in project [ConsoleFeatures Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/ConsoleFeaturesSamples)
- New feature: Group items in the select control
    - Sample with commemts in project [SelectBasic Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/SelectBasicSamples)
- New feature: Add separator line in the select control
    - Sample with commemts in project [SelectBasic Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/SelectBasicSamples)
 
**Special thanks to [ividyon](https://github.com/ividyon) for suggesting improvements and actively participating in this release**

## **PromptPlus Controls - Sample Usage**

```csharp
//ASCII text banners
PromptPlus
    .Banner("PromptPlus v4.0")
    .Run(Color.Yellow,BannerDashOptions.DoubleBorderUpDown);

//MaskEdit DateTime
var mask = PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
    .Mask(MaskedType.DateTime)
    .DescriptionWithInputType(FormatWeek.Short)
    .Culture("en-us")
    .AcceptEmptyValue()
    .Run();

if (!mask.IsAborted)
{
    PromptPlus.WriteLine($"You input with mask is {mask.Value.Masked}");
    PromptPlus.WriteLine($"You input without mask is {mask.Value.Input}");
}    

//INPUT
var input = PromptPlus
    .Input("Input sample")
    .Default("foo")
    .Run();

if (!input.IsAborted)
{
    PromptPlus.WriteLine($"You input is {input.Value}");
}

//AnyKey
var kp = PromptPlus
    .KeyPress()
    .Config(cfg => cfg.HideAfterFinish(true))
    .Spinner(SpinnersType.DotsScrolling)
    .Run();
```

## **Supported platforms**

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

## **License**

Copyright 2021 @ Fernando Cerqueira

This project is licensed under the [MIT License](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)


## **Credits**

PromptPlus includes code from other software released under the MIT license:

- [Spectre.Console](https://spectreconsole.net/), Copyright (c) 2020 Patrik Svensson, Phil Scott, Nils Andresen.
- [Sharprompt](https://github.com/shibayan/Sharprompt), Copyright (c) 2019 shibayan.
- [xmldoc2md](https://github.com/FRACerqueira/xmldoc2md), Copyright (c) 2022 Charles de Vandière.
- [FIGlet](https://github.com/auriou/FIGlet), Copyright (c) 2014 Philippe AURIOU
