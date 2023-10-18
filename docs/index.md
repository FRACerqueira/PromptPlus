# <img align="left" width="100" height="100" src="./images/icon.png">Welcome to PromptPlus

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

**PromptPlus** was developed in C# with the **netstandard2.1**, **.NET 6** and **.NET 7** target frameworks.


## Table of Contents

- [What's new - previous versions](https://github.com/FRACerqueira/PromptPlus/blob/main/whatsnewprev.md)
- [Features](#features)
- [Migrate Version V3.3 to V4.0](migrateversion.md)
- [Installing](#installing)
- [Examples](#examples)
- [Controls Snapshot](snapshot.md)
- [Console Engine](#console-engine)
- [Culture](#culture)
- [Colors](#colors)
- [Hotkeys](#hotkeys)
- [Keypress Extensions Emacs](#keypress-extensions-emacs)
- [Validators](#validators)
- [Global Settings](globalsettings.md)
- [Supported Platforms](#supported-platforms)
- [License](#license)
- [Credits](#credits)
- [API Reference](./apis/apis.md)

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

## Features
[**Top**](#table-of-contents)
 
**_All features have IntelliSense_. PromptPlus has more than 25 controls with many features like: filters, validators, history, suggestions, spinner(19 embedding type and plus custom yours!), colors and styles for control-elements** :
- Banner Ascii
- Input text / Secret / AutoComplete with spinner
- MaskEdit Generic / Only Date / Only Time / DateTime / Number /  Currency
- Calendar with multiple layouts
- Select and Multi-Select(with group select!) 
- AddTo(Add/Remove) items for text and masked text
- Wait Keypress with animate spinner
- Slider numeric ranger with gradient colors
- Up-Down numeric ranger 
- Switch (style on/off)
- Wait Process (Run background tasks Sequential/Parallel) with elapsedtime and spinner 
- Wait Time with countdown and spinner
- ChartBar with enabled Interaction to switch layout, Legend and order when browse the charts / Legends.
- Progress bar with 8 types , gradient colors and spinner
- Browser File and Folder with multi-select, colors and spinner
- Treeview hierarchical structures with multi-select and colors
- Switch Alternate screen
- Execution pipeline with conditions
- Table, TableSelect and TableMultSelct with multiple layouts
 
**All controls** have the same organization (see in action: [**Controls Snapshot**](snapshot.md)):
- input/filter (except Masked input) using **[GNU Readline](https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts**.  
- Prompt, description and data entry (ever)
- Extra actions per stage : OnStartControl/OnInputRender/OnTryAcceptInput/OnFinishControl (ever)
- Tooltips (ever and configurable) 
- Filter by Contains / StartsWith (configurable) (depends on the control)
- Collection subset items and iterations (depends on the control)
- Page information and page-size(depends on the control)
- Spinner animation (depends on the control)
- Error message (depends on the control and validators)

All controls use **fluent interface**; an object-oriented API whose design relies extensively on method chaining. Its goal is to increase code legibility. The term was coined in 2005 by Eric Evans and Martin Fowler.

```csharp
//MaskEdit Generic
var mask = PromptPlus.MaskEdit("input", "MaskEdit Generic input")
    .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
    .DescriptionWithInputType(FormatWeek.Short)
    .Run();
```

PromptPlus driver console  **Supports 4/8/24-bit colors** in the terminal with **auto-detection** of the current terminal's capabilities.

## Migrate Version
[**Top**](#table-of-contents)

Until version 3 the console engine was based on a model from another project that has several serious problems that cause exceptions during execution in addition to increasing the complexity of the code for correct rendering...
**PromptPlus v4** has been **completely rebuilt** for a better experience, with significant improvements with new controls and more developer power. The console driver now supports better rendering, with the ability to detect terminal capabilities and allow for 24-bit color, text overflow strategies based on terminal size, and left and right margins for a nicer layout.
**The Controls have been revised to be more responsive, allow color styles in many of their elements**, and adapt to the terminal size even with resizing.

For migrate V3 to V4 [**see this link**](migrateversion.md).

## Installing
[**Top**](#table-of-contents)

PromptPlus was developed in C# with the **netstandard2.1**, **.NET 6** AND **.NET 7** target frameworks.

```
Install-Package PromptPlus [-pre]
```

```
dotnet add package PromptPlus [--prerelease]
```

**_Note:  [-pre]/[--prerelease] usage for pre-release versions_**

## Examples
[**Top**](#table-of-contents)

The folder at github [**Samples**](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples) contains more **30** samples!.

```
dotnet run --project [name of sample]
```

## Console Engine
[**Top**](#table-of-contents)

The console driver has the ability to detect terminal capabilities and allow for **24-bit color and text overflow strategies**  based on terminal size, and left and right margins for a nicer layout.
The new engine detects support ansi commands and adjust output for this functionality respecting OS differences , terminal mode and Windows console mode. The Colors are automatically adjusted to the capacity of the terminal. This automatic adjustment may slightly modify the final color when converting to a lower bit resolution.

### Wrap to Console - Code sample 

PromptPlus has **cross-platform** wrap to console for key features.

```csharp
//Console.CursorLeft = 1;
PromptPlus.CursorLeft = 1;
//Console.ReadKey();
PromptPlus.ReadKey();
```

### Extra commands to Console
- Clear(Color? backcolor = null)
  - Clear console with color and set BackgroundColor with color.
- ClearLine(int? row = null, Style? style = null) 
  - Clear row line with style.
- ClearRestOfLine(Style? style = null)
  - Clear rest of line.
- WriteLines(int steps = 1)
  - Write many new lines.
- SingleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null)
  - Writes a line with a single dash line after.
- DoubleDash(string value, DashOptions dashOptions = DashOptions.AsciiSingleBorder, int extralines = 0, Style? style = null)
  - Writes a line between a pair of dash lines.
- MoveCursor(CursorDirection direction, int steps)
  - Move cursor by direction
- WaitKeypress(bool intercept, CancellationToken? cancellationToken)
  - Wait a keypress with cancellation token
- ReadLineWithEmacs(uint? maxlength = uint.MaxValue,Action<string,int> afteraccept = null, CaseOptions caseOptions = CaseOptions.Any)
  - Read line from stream using Emacs keyboard shortcuts with maxlength, case options and user action after each accepted keystroke

### Extend Write / Writeline
- Write(string value, Style? style = null, bool clearrestofline = false)
    -  Write a text to output console with options for style and clear rest of line  
- Write(Exception value, Style? style = null, bool clearrestofline = false)
    -  Write a Exception to output console with options for style and clear rest of line  
- WriteLine(string? value = null, Style? style = null, bool clearrestofline = true)
    -  Write a text to output console with line terminator, option for style and clear rest of line (default)
- WriteLine(Exception value, Style? style = null, bool clearrestofline = true)
    -  Write a exception to output console with line terminator, option for style and clear rest of line (default)

### Setup and auto detect - Code sample 

```csharp
PromptPlus.Setup((cfg) =>
{
    cfg.PadLeft = 2;
    cfg.PadRight = 2;
    cfg.Culture = new CultureInfo("en-us");
    cfg.BackgroundColor = ConsoleColor.Blue;
});
PromptPlus.SingleDash($"[yellow]Console Information[/]", DashOptions.DoubleBorder, 1 /*extra lines*/);
PromptPlus.WriteLine($"IsTerminal: {PromptPlus.IsTerminal}");
PromptPlus.WriteLine($"IsUnicodeSupported: {PromptPlus.IsUnicodeSupported}");
PromptPlus.WriteLine($"OutputEncoding: {PromptPlus.OutputEncoding.EncodingName}");
PromptPlus.WriteLine($"ColorDepth: {PromptPlus.ColorDepth}");
PromptPlus.WriteLine($"BackgroundColor: {PromptPlus.BackgroundColor}");
PromptPlus.WriteLine($"ForegroundColor: {PromptPlus.ForegroundColor}");
PromptPlus.WriteLine($"SupportsAnsi: {PromptPlus.SupportsAnsi}");
PromptPlus.WriteLine($"Buffers(Width/Height): {PromptPlus.BufferWidth}/{PromptPlus.BufferHeight}");
PromptPlus.WriteLine($"PadScreen(Left/Right): {PromptPlus.PadLeft}/{PromptPlus.PadRight}\n");

PromptPlus
    .KeyPress()
    .Config(cfg =>
    {
        cfg.HideAfterFinish(true)
          .ShowTooltip(false) 
          .ApplyStyle(StyleControls.Tooltips,Style.Plain.Foreground(Color.Grey100));
    }) 
    .Spinner(SpinnersType.Balloon)
    .Run();
    
```

### Output detect
![](./images/consoleinfo.gif)


### Overflow Capacity - Code sample 

```csharp
PromptPlus.Clear();
PromptPlus.DoubleDash($"PromptPlus Style.OverflowEllipsis");
PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + 
"asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", style: Style.OverflowEllipsis);

PromptPlus.DoubleDash($"PromptPlus Style.OverflowCrop");
PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + 
"asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", style: Style.OverflowCrop);

PromptPlus.DoubleDash($"PromptPlus default");
PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + 
"asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj");

PromptPlus
    .KeyPress()
    .Config(cfg => cfg.HideAfterFinish(true))
    .Spinner(SpinnersType.DotsScrolling)
    .Run();
```

### Output Overflow Capacity

![](./images/consoleoverflowcapacity.gif)


#### Sample color capacity ([Project sample](https://github.com/FRACerqueira/PromptPlus/tree/main/Samples/ConsoleFeaturesSamples))


**_Note: This layout and code was inspired by the excellent project:spectreconsole, having the same color palette_**

![](./images/consolecolorcapacity.gif)


## Culture
[**Top**](#table-of-contents)

PromptPlus applies the language/culture **only when running controls**. The language/culture of the application is **not affected**. If language/culture is not informed, the application's language/culture will be used with fallback to en-US.

All messages are affected when changed language/culture. PromptPlus has languages embedded:
- en-US (Default)
- pt-BR
 
```csharp
//sample global set for messages and validate
PromptPlus.DefaultCulture = new CultureInfo("en-US");
```

```csharp
//sample only control
PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
    .Mask(MaskedType.DateOnly)
    .DescriptionWithInputType(FormatWeek.Short)
    .Culture(new CultureInfo("en-us")) //overwrite culture
    .AcceptEmptyValue()
    .Run();

PromptPlus.MaskEdit("input", "MaskEdit DateOnly input")
    .Mask(MaskedType.DateOnly)
    .DescriptionWithInputType(FormatWeek.Short)
    .Culture("pt-br") //overwrite culture
    .AcceptEmptyValue()
    .Run();
```

To use a non-embedded language/culture:

- Copy the **PromptPlusResources.resx** file in folder PromptPlus/Resources
- Translate messages with same format to your language/culture
- Convert .resx files to binary .resources files ([**reference link here**](https://docs.microsoft.com/en-us/dotnet/core/extensions/work-with-resx-files-programmatically))
- Publish the compiled file (**PromptPlus.[Language].resources**) in the same folder as the binaries.

## Colors
[**Top**](#table-of-contents)

PromptPlus is in accordance with informal standard [**NO COLOR**](https://no-color.org/). when there is the environment variable "no_color" the colors are disabled.

Prompt Plus also has commands for coloring parts of the text.

#### Direct console
```csharp
PromptPlus.WriteLine("[RGB(255,0,0) ON WHITE]Test[YELLOW] COLOR [/] BACK COLOR [/] other text");
PromptPlus.WriteLine("[#ff0000 ON WHITE]Test [YELLOW] COLOR [/] BACK COLOR [/] other text");
PromptPlus.WriteLine("[RED ON WHITE]Test[YELLOW] COLOR [/] BACK COLOR [/] other text");
````

### Using Style

```csharp
PromptPlus.WriteLine("Test", new Style(Color.White, Color.Red, Overflow.None));
PromptPlus.WriteLine("Test", new Style(new Color(255, 255, 255), Color.Red, Overflow.None));
PromptPlus.WriteLine("Test", new Style(Color.FromConsoleColor(ConsoleColor.White), Color.Red, Overflow.None));
PromptPlus.WriteLine("Test", new Style(Color.FromInt32(255), Color.Red, Overflow.None));
````

### Over controls
```csharp
PromptPlus
    .Input("Input [blue]sample2[/]", "with [yellow]description[/]")
    .Run();
````

### Escaping format characters
To output a [ you use [[, and to output a ] you use ]].
```csharp
PromptPlus.WriteLine("[[Test]]");
````

Promptplus uses the **same default colors and engine(softly modified)** as the third party project: spectreconsole.

[**Default color for controls and console**](colors.md)

## Hotkeys
[**Top**](#table-of-contents)

Hotkeys (global and control-specific) are configurable.

 ```csharp
//sample global
PromptPlus.Config.SelectAllPress = new HotKey(UserHotKey.F7);
```

[**Default Hotkeys for controls**](hotkeys.md)

## Keypress Extensions Emacs
[**Top**](#table-of-contents)

PromptPlus have a lot extensions to check Key-press with GNU Readline Emacs keyboard shortcuts.
For more details [visit the **official API page**](./apis/pplus.promptpluskeyinfoextensions.md)

## Validators
[**Top**](#table-of-contents)

PromptPlus have a lot extensions to **commons validator** and **validator import**(No duplicate code!) 

For more details see [**List validators embedding**](./apis/pplus.controls.promptvalidators.md)

```csharp
private class MylCass
{
    [Required(ErrorMessage = "{0} is required!")]
    [MinLength(3, ErrorMessage = "Min. Length = 3.")]
    [MaxLength(5, ErrorMessage = "Max. Length = 5.")]
    [Display(Prompt ="My Input")]
    public string MyInput { get; set; }
}
```
```csharp
var inst = new MylCass();

PromptPlus
    .Input("Input sample2", "import validator from decorate")
    .Default(inst.Text)
    .AddValidators(PromptValidators.ImportValidators(inst,x => x!.Text!))
    .Run();

if (name.IsAborted)
{
   return;
}
PromptPlus.WriteLine($"Your input: {name.Value}!");
```

```csharp
//sample
PromptPlus.EnabledAbortKey = false;
```
## Supported platforms
[**Top**](#table-of-contents)

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

## License
[**Top**](#table-of-contents)

Copyright 2021 @ Fernando Cerqueira

PromptPlus is licensed under the MIT license. For more information see [LICENSE](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE).

## Credits
[**Top**](#table-of-contents)

PromptPlus **includes code** from other software released under the **MIT license**:

- [Spectre.Console](https://spectreconsole.net/), Copyright (c) 2020 Patrik Svensson, Phil Scott, Nils Andresen.
- [Sharprompt](https://github.com/shibayan/Sharprompt), Copyright (c) 2019 shibayan.
- [xmldoc2md](https://github.com/FRACerqueira/xmldoc2md), Copyright (c) 2022 Charles de Vandière.
- [FIGlet](https://github.com/auriou/FIGlet), Copyright (c) 2014 Philippe AURIOU

