# <img align="left" width="100" height="100" src="./images/icon.png">Welcome to PromptPlus
[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

**Interactive command-line toolkit for .Net core with powerful controls and commands to create professional console applications.**

**PromptPlus have more 20 controls with many features like: filters, validators, history, sugestions, spinner(19 embeding type and plus custom yours!), colors and styles for control-elements** :
- Banner Ascii
- Input text / Secret / AutoComplete with spinner
- MaskEdit Generic / Only Date / Only Time / DateTime / Number /  Currency
- Select and Multi-Select(with group select!) 
- AddTo(Add/Remove) items for text and masked text
- Wait Keypress with animate spinner
- Slider numeric ranger with gradient colors
- Up-Down numeric ranger 
- Switch (style on/off)
- Wait Tasks (Parallel/Sequential) with elapsedtime and spinner 
- Wait Time with countdown and spinner
- Progress bar with 8 types , gradient colors and spinner
- Browser File and Folder with multi-select, colors and spinner
- Treeview hierarchical structures with multi-select and colors

All controls have the same organization (see in action: [**Controls Snapshot**]()):

- controls input/filter using **[GNU Readline](https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts**. 
- Prompt, description and data entry (ever)
- User extra actions per stage : OnStartControl/OnInputRender/OnTryAcceptInput/OnFinishControl (ever)
- Tooltips (ever and configurable) 
- Filter by Contains / StartsWith (configurable) (depends on the control)
- Collection subset items and interations (depends on the control)
- Page information and page-size(depends on the control)
- Spinner animmation (depends on the control)
- Error message (depends on the control and validators)

PromptPlus driver console  **Supports 4/8/24-bit colors** in the terminal with auto-detection of the current terminal's capabilities.

**PromptPlus** was developed in c# with the **netstandard2.1**, **.Net 6** and **.Net 7** target frameworks.


## Migrate Version (V3 to V4)

Until version 3 the console engine was based on a model from another project that has several serious problems that cause exceptions during execution in addition to increasing the complexity of the code for correct rendering...
PromptPlus v4 has been completely rebuilt for a better experience, with significant improvements with new controls and more developer power. The console driver now supports better rendering, with the ability to detect terminal capabilities and allow for 24-bit color, text overflow strategies based on terminal size, and left and right margins for a nicer layout.
Controls have been revised to be more responsive, allow color styles in many of their elements, and adapt to the terminal size even with resizing.
All these improvements were only possible by generating some break-changes, but maintaining a high sixtax compatibility.

The differences and [**how to migrate can be seen at this link**]().



## Help
- [Install](#install)
- [Global Settings](#global-settings)
- [Console](#console-engine)
- [Culture](#culture)
- [Colors](#colors)
- [Hotkeys](#hotkeys)
- [Controls](#controls)
- [Supported Platforms](#supported-platforms)

## Install
[**Top**](#help)

PromptPlus was developed in c# with the **netstandard2.1, .NET 6 AND .NET7** target frameworks.

```
Install-Package PromptPlus [-pre]
```

```
dotnet add package PromptPlus [--prerelease]
```

**_Note:  [-pre]/[--prerelease] usage for pre-release versions_**


## Global Settings
[**Top**](#help)
- DisabledAllTooltips 
    - Override options to EnabledStandardTooltip and EnabledPromptTooltip . Default = false
- EnabledBeep 
    - Enabled/disabled beep. Default = false
- EnabledStandardTooltip 
    - Show/Hide global tooltip. Default = true. When false then control´s tooltip not show.
- EnabledPromptTooltip
    - Enabled/disabled control´s tootip for default value. Default = true
- EnabledAbortKey
    - Enabled/disabled hotkey to abort prompt/control executation. Default = true
- EnabledAbortAllPipes 
    - Enabled/disabled hotkey to abort all pipes. Default = true
- PasswordChar 
    - Character default to password input. Default = '#'
- DefaultCulture 
    - Set default language/culture for all controls. Default = Current Culture application
 

```csharp
//sample
PromptPlus.EnabledAbortKey = false;
```

## Console Engine
[**Top**](#help)

The console driver have the ability to detect terminal capabilities and allow for **24-bit color and text overflow strategies**  based on terminal size, and left and right margins for a nicer layout.
The new engine detects support ansi commands and adjust output for this functionality respecting OS differences , terminal mode and Windows console mode. The Colors are automatically adjusted to the capacity of the terminal. This automatic adjustment may slightly modify the final color when converting to a lower bit resolution.

### Setup and auto detect - Code sample 

```csharp
PromptPlus.Setup((cfg) =>
{
    cfg.PadLeft = 2;
    cfg.PadRight = 2;
    cfg.Culture = new CultureInfo("en-us");
    cfg.BackgroundColor = ConsoleColor.Blue;
});

PromptPlus.SingleDash($"[yellow]Console Information[/]", DashOptions.DoubleBorder, 1);
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
        cfg.HideAfterFinish(true);
        cfg.ShowTooltip(false); 
    }) 
    .Spinner(SpinnersType.Balloon)
    .Run();
    
```

### Output detect
![](./images/consoleinfo.gif)


### Overflow Capacity - Code sample 

```csharp
PromptPlus.Clear();
PromptPlus.DoubleDash($"PromptPlus.Console Style.OverflowEllipsis");
PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + 
"asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", style: Style.OverflowEllipsis);

PromptPlus.DoubleDash($"PromptPlus.Console Style.OverflowCrop");
PromptPlus.WriteLine("[RED ON WHITE]TESTE[YELLOW] COLOR [/] BACK COLOR [/]" + 
"asdajsdkldksdkasasdadasdadjashkjdahsdashdjkashdkashdkashdkashdakshdkashdkashdaskhdaskdhaskdhaskdhaskdhaskdhsakdhaskdhaskjdj", style: Style.OverflowCrop);

PromptPlus.DoubleDash($"PromptPlus.Console default");
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


#### Sample color capacity

**_Note: This layout and code was inspired by the excellent project:spectreconsole, having the same color palette_**

![](./images/consolecolorcapacity.gif)


## Culture
[**Top**](#help)

PromptPlus applies the language/culture **only when running controls**. The language/culture of the application is **not affected**. If language/culture is not informed, the application's language/culture will be used with fallback to en-US.

All messages are affected when changed language/culture. PromptPlus has languages embeded:
- en-US (Default)
- pt-BR
 
```csharp
//sample
PromptPlus.DefaultCulture = new CultureInfo("en-US");
```

To use a non-embedded language/culture:

- Copy the **PromptPlusResources.resx** file in folder PromptPlus/Resources
- Translate messages with same format to your language/culture
- Convert .resx files to binary .resources files ([**reference link here**](https://docs.microsoft.com/en-us/dotnet/core/extensions/work-with-resx-files-programmatically))
- Publish the compiled file (**PromptPlus.[Language].resources**) in the same folder as the binaries.

## Colors
[**Top**](#help)

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

**Default color for controls**

- Pagination
    - Page info foreground color. Default = Console.DarkGray
- Hint
    - Hit/tooltip info foreground color. Default = Console.DarkGray
- Answer
    - Answer foreground color. Default = Console.Cyan
- Select
    - Seleted item foreground color. Default = Console.Green
- Disabled
    - Disabled item foreground color. Default = Console.DarkGray
- Filter
    - Filter info foreground color. Default = Console.Yellow
- Error
    - Error info foreground color. Default = Console.Red
- DoneSymbol
    - Done symbol foreground color. Default = Console.Cyan
- PromptSymbol
    - Prompt symbol foreground color. Default = Console.Green
- SliderBackcolor
    - Sliders / Progress bar background color. Default = Console.DarkGray
- SliderForecolor
    - Sliders / Progress bar foreground color. Default = Console.Cyan
 

## Hotkeys
[**Top**](#help)

Hotkeys (global and control-specific) are configurable. Some hotkeys are internal and reserved.

 ```csharp
//sample global
PromptPlus.Config.SelectAllPress = new HotKey(UserHotKey.F7);
```

**Hotkeys Configurables:**

- AbortAllPipesKeyPress
    - Key for abort all pipes. Default = F7
- TooltipKeyPress
    - Key for Show/Hide tooltips. Default = F1
- ResumePipesKeyPress
    - Key for Show/Hide summary pipes. Default = F2
- UnSelectFilter
    - Key for Show input filter. Default = F4
- SwitchViewPassword
    - Key for Show/hide input password. Default = F5
- SelectAll
    - Key for toggle all items to selected. Default = F5
- InvertSelect
    - Key for invet selection to all items. Default = F6
- RemoveAll
    - Key for remove all items. Default = F4
- MarkSelect
    - Key for mark item in multi-select. Default = F8


## Supported platforms
[**Top**](#help)

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app
