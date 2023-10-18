# **Welcome to PromptPlus**

**Interactive command-line toolkit for .NET Core with powerful controls and commands to create professional console applications.**

All controls input/filter (except Masked input) using [**GNU Readline**](https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts.  

PromptPlus **Supports 4/8/24-bit colors** in the terminal with auto-detection of the current terminal's capabilities and automatic color conversion.

#### [Visit the official page for complete documentation of PromptPlus](https://fracerqueira.github.io/PromptPlus)

**PromptPlus** was developed in C# with the **netstandard2.1**, **.NET 6** and **.NET 7** target frameworks.

## What's new in V4.1.1

- Split feature Control Table:
    - PromptPlus.TableSelect\<T> to Select item in table : Select row, column and data in a grid/table 
        - Samples in project [Table Select Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/TableSelectSamples)
    - PromptPlus.Table\<T> to write table in console : Show data in a grid/table 
        - Samples in project [Table Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/TableBasicSamples)
- New Control : TableMultSelect\<T> :  Select multi-data in a grid/table 
    - Samples in project [Table MultiSelect Samples](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/TableMultiSelectSamples)
    - Main features :
        - More than 80 layout combinations
        - Navigation by row and columns
        - Scroll the table when it is larger than the screen
        - Split text when it is larger than the column size
        - Automatic header and column completion
        - Color customization of each element
        - Search for data filtered by columns
        - Formatting by column or by data type definition
- New feature: 
    - MinimalRender the prompt and control description are not rendered, showing only the minimum necessary without using resources.
        - Global property : MinimalRender
        - Instance control(By config command): MinimalRender(bool value = true)
- New feature: 
    - Pagination Template to customize pagination information
        - Global property : PaginationTemplate
        - Instance control(By config command) : PaginationTemplate(Func<int, int, int, string>? value)
- New feature: 
    - PromptPlus.Join() 
    - Fluent-Interface to write text (less code typed) 
- Changed feature:
    - Moved tooltips and validation message to the end of render to all control
- Improvement : 
    - Optimized the Calendar control to have symbols when selecting elements
- Improvement :
    - Optimize Render of ProgressBar (less lines)
- Improvement : 
    - Optimize Render of SliderNumber (less lines)
- Improvement : 
    - Optimize resource usage in rendering (less cultural dependency)
- Improvement : 
    - Reinforce the validation of invalid or optional parameters in all controls
- Renamed command: 
    - 'DescriptionWithInputType' to 'ShowTipInputType'.
    - Now extra-line to tip InputType
- Renamed command: 
    - 'AppendGroupOnDescription' to 'ShowTipGroup'.
    - Now extra-line to tip group
- Fixed bug : 
    - The Slide Switch Control does not show on/off values ​​when they are not customized
- Fixed bug : 
    - Alternate screen doesn't update background style when changing color
- Fixed bug : 
    - Exception when try delete[F3] in empty colletion in AddTolist/AddtoMaskEditList control
- Fixed bug : 
    - Edit[F2] Immutable item in AddTolist/AddtoMaskEditList control
- Fixed bug : 
    - CTRL-V (paste data) does not show input in some controls

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

PromptPlus **includes code** from other software released under the **MIT license**:

- [Spectre.Console](https://spectreconsole.net/), Copyright (c) 2020 Patrik Svensson, Phil Scott, Nils Andresen.
- [Sharprompt](https://github.com/shibayan/Sharprompt), Copyright (c) 2019 shibayan.
- [xmldoc2md](https://github.com/FRACerqueira/xmldoc2md), Copyright (c) 2022 Charles de Vandière.
- [FIGlet](https://github.com/auriou/FIGlet), Copyright (c) 2014 Philippe AURIOU
