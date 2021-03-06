# <img align="left" width="100" height="100" src="./images/icon.png"> Welcome to PromptPlus

Interactive command-line based application framework for **C# with powerful controls**.

**PromptPlus** was developed in c# with the **netstandard2.1**, **.Net5** and **.Net6** target frameworks.

[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)


![](./images/PipeLine.gif)

All controls input/filter using **[GNU Readline](https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts**.  

![](./images/Readline.gif)
![](./images/InputHistory.gif)
![](./images/InputSugestion.gif)

**PromptPlus** has separate pakage integrate command line parse **CommandDotNet(4.3.0/6.0.0)**: 

<p align="left">
    <img valign="middle" width="80" height="80" src="./images/iconCmdNet.png">
    <a href="https://fracerqueira.github.io/PromptPlus/ppluscmddotnet.html"><b>PromptPlus.CommandDotNet!!</b></a>
</p>

[![Downloads](https://img.shields.io/nuget/dt/PromptPlusCommandDotNet)](https://www.nuget.org/packages/PromptPlusCommandDotNet/)
[![NuGet](https://img.shields.io/nuget/v/PromptPlusCommandDotNet)](https://www.nuget.org/packages/PromptPlusCommandDotNet/)

**Innovative middleware policy for CommandDotNet with PromptPlus.CommandDotNet**:

- Interative session with readline prompt, Sugestions and History.
    - Now you can help to discover arguments (Sugestions) and history actions in interactive sessions.

![](./images/PplusCmddotnetRepl.gif)

- Wizard to find all the commands/options and arguments with prompt and run.
    - Now you can discover and learn the existing commands, options and arguments.

![](./images/PplusCmddotnetWizard.gif)

**[CommandDotNet is third party applications. Visit official page for complete documentation](https://commanddotnet.bilal-fazlani.com)**

An open-source guide to help you write better command-line programs, taking traditional UNIX principles and updating them for the modern day.

**[Command Line Interface Guidelines](https://clig.dev/)**

## Help
- [Install](#install)
- [Organization](#organization)
    - [Pagination Behavior](#pagination-behavior)
- [Global Settings](#global-settings)
    - [Culture](#culture)
    - [Colors](#colors)
    - [Symbols](#symbols)
    - [Hotkeys](#hotkeys)
- [Load and Save Settings](#load-and-save-settings)
- [**Api Controls**](#apis)
- [**PromptPlus.CommandDotNet**](ppluscmddotnet.md)
- [**Extensions**](#extensions)
- [Snapshot of All Controls](#snapshot)
- [Supported Platforms](#supported-platforms)

# Documentation

## Install
[**Top**](#help)

PromptPlus was developed in c# with the **netstandard2.1, .NET 5 AND .NET6 ** target frameworks.

```
Install-Package PromptPlus [-pre]
```

```
dotnet add package PromptPlus [--prerelease]
```

**_Note:  [-pre]/[--prerelease] usage for pre-release versions_**

## Organization
[**Top**](#help)

All controls have the same lines organization:
- Message and data entry (ever)
- Filter (depends on the control)
- Description (configurable/optional)
- Tooltips (configurable)
- Collection subset items (depends on the control, page size and size of console/terminal)
- Page information (depends on size colletion, page size and size of console/terminal)
- Error message (depends on the control and validators)

tooltips can be global (hotkey always active - default F1) and control specific. All controls have the properties to show hide tooltips.

### Pagination Behavior
[**Top**](#help)

When a control has a collection it can be paged with a limit of items per page. When the item per page limit is not entered, the number of items per page is set to the maximum allowed by the console/terminal size. If the console/terminal is resized, an adjustment will be made to a number of items per page and a message will be issued on the console (only when it is a terminal)

## Global Settings
[**Top**](#help)
- DisabledAllTooltips 
    - Override options to EnabledStandardTooltip and EnabledPromptTooltip . Default = false
- EnabledBeep 
    - Enabled/disabled beep. Default = false
- EnabledStandardTooltip 
    - Show/Hide global tooltip. Default = true. When false then control??s tooltip not show.
- EnabledPromptTooltip
    - Enabled/disabled control??s tootip for default value. Default = true
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

### Culture
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

- Use the **PromptPlusResources.resx** file in folder PromptPlus/Resources
- Translate messages with same format to your language/culture
- Convert .resx files to binary .resources files ([**reference link here**](https://docs.microsoft.com/en-us/dotnet/core/extensions/work-with-resx-files-programmatically))
- Publish the compiled file (**PromptPlus.{Language}.resources**) in the same folder as the binaries.

### Colors
[**Top**](#help)

PromptPlus is in accordance with informal standard [**NO COLOR**](https://no-color.org/). when there is the environment variable "no_color" the colors are disabled.
PromptPlus has a configurable color(16 color) schema.

- ForeColorSchema 
    - Screen foreground color. Default = Console.ForegroundColor
- BackColorSchema
    - Screen background color. Default = Console.BackgroundColor
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
 
 ```csharp
//sample
PromptPlus.ColorSchema.Answer = ConsoleColor.Cyan;
```

PromptPlus also has commands **for coloring parts of text and underlining**.

```csharp
//sample
PromptPlus.WriteLine("This [cyan]is [red]a [white:blue]simples[/] line with [yellow!u]color[/]. End [/]line.");
````

```csharp
//sample
PromptPlus.WriteLine("This is a simples ","line".White().OnBlue().Underline(), " with ", "color".Red());
````

### Symbols
[**Top**](#help)

PromptPlus has a configurable symbols with Unicode support and Fallback.

- MaskEmpty
    - Symbol : ???
    - Fallback : double space
    - Apply to : Empty character for input masked
- File
    - Symbol : ???
    - Fallback : "- "
    - Apply to : Symbol for file in browser control
 - Folder
    - Symbol : ???
    - Fallback : "> "
    - Apply to : Symbol for folder in browser control
 - Prompt
    - Symbol : ???
    - Fallback : "->"
    - Apply to : Symbol for prompt control
 - Done
    - Symbol : ???
    - Fallback : "V "
    - Apply to : Symbol for prompt done
 - Error
    - Symbol : ??
    - Fallback : "\>\>"
    - Apply to : Symbol for message error
 - Selector
    - Symbol : ???
    - Fallback : "> "
    - Apply to : Symbol for seletor item.
 - Selected
    - Symbol : ???
    - Fallback : "* "
    - Apply to : Symbol for Selected item.
 - NotSelect
    - Symbol : ???
    - Fallback : double space
    - Apply to : Symbol for not Selected item.
 - TaskRun
    - Symbol : ???
    - Fallback : "* "
    - Apply to : Symbol for backgound process running.
 - Skiped
    - Symbol : ??
    - Fallback : "x "
    - Apply to : Symbol for  backgound process canceled.

 ```csharp
//sample
PromptPlus.Symbols.Done = new Symbol("???", "V ");
```
**_Note: new Symbol() return : Symbol = single space and Fallback = double space._**

### Hotkeys
[**Top**](#help)

Hotkeys (global and control-specific) are configurable. Some hotkeys are internal and reserved.

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

```csharp
//sample
PromptPlus.AbortAllPipesKeyPress = new HotKey(UserHotKey.F7, alt: true, ctrl: false, shift: false);
```

## Load and Save Settings
[**Top**](#help)

PromptPlus allows saving and loading a previous configuration of culture, behavior, hotkeys, colors and symbols.A file with the default configuration is available in the package in the Resources folder named **PromptPlus.config.json** . To load automatically the file must be placed in your project and published in the **same folder** as the binaries.

```csharp
//sample save
PromptPlus.SaveConfigToFile(folderfile: "YourFolder");
//sample load
PromptPlus.LoadConfigFromFile(folderfile: "YourFolder");
```

**_Note: if the folderfile parameter is omitted, it will be saved/loaded from the default application folder_**

## Apis
[**Top**](#help)

Controls/Commands | Details
--- | ---
[Commands](commands.md) |  Command set for PromptPlus console
[Color](colorcmd.md) | Easy to add some color-text and underline
[ASCII-Banner](banner.md) |  ASCII text banner 
[Any-key](anykey.md) |  Simple any key press
[Key-Press](keypress.md) | Simple specific key
[Confirm](confirm.md) | Simple confirm with  with tool tips and language detection 
[AutoComplete](autocomplete.md) | Input text with sugestions, validator, and tooltips
[Readline](readline.md) | Input text with GNU Readline Emacs keyboard shortcuts, sugestions and historic
[Input](input.md) | Input text with input validator with tooltips
[Extensions points](basemethods.md#sample-use-of-extraaction) | Input text with history/suguestions using extensions points
[Password](input.md) | Input password with input validator and show/hide(optional) input value
[MaskEdit-Generic](maskeditgeneric.md) | Input with masked input , tooltips and input validator
[MaskEdit-Date](maskeditdate.md) | Date input with language parameter, tooltips and input validator
[MaskEdit-Time](maskeditdate.md) | Time input with language parameter, tooltips and input validator
[MaskEdit-Date/Time](maskeditdate.md) | Date and time input with language parameter, tooltips and input validator
[MaskEdit-Number](maskeditnumber.md) | Numeric input with language parameter, tooltips and input validator
[MaskEdit-Currency](maskeditnumber.md) | Currency input with language parameter, tooltips and input validator
[Select](select.md)| Generic select input IEnumerable/Enum with auto-paginator and tooltips and more
[MultiSelect](multiselect.md) | Generic multi select input IEnumerable/Enum with group, auto-paginator , tooltips and more
[List](list.md) | Create Generic IEnumerable with auto-paginator, tooptip , input validator, message error by type/format and more
[ListMasked](listmasked.md) | Create generic IEnumerable with maskededit, auto-paginator, tooptip , input validator
[Browser](browser.md) | Browser files/folder with auto-paginator and tooltips
[Slider-Number](slidernumber.md) | Numeric range with short/large step and tooltips
[Number-Up/Down](slidernumber.md) | Numeric range with step and tooltips
[Slider-Switch](sliderswitch.md) | Generic choice with customization and tooltips
[Progress-Bar](progressbar.md) | Progress Bar with interation customization
[Wait-Process](waitprocess.md) | Wait process with animation
[PipeLine](pipeline.md) | Pipeline sequence to all prompts with condition by pipe and status summary

## Extensions
[**Top**](#help)

PromptPlus have a extension to **import validators**. No duplicate code! 

```csharp
private class MylCass
{
    [Required(ErrorMessage = "{0} is required!")]
    [MinLength(3, ErrorMessage = "Min. Length = 3.")]
    [MaxLength(5, ErrorMessage = "Min. Length = 5.")]
    [Display(Prompt ="My Input")]
    public string MyInput { get; set; }
}
```
```csharp
var inst = new MylCass();
var name = PromptPlus.Input("Input Value for MyInput")
    .Addvalidators(inst.ImportValidators(x => x.MyInput))
    .Run(_stopApp);

if (name.IsAborted)
{
   return;
}
Console.WriteLine($"Your input: {name.Value}!");
```

## Snapshot

### Input
[**Top**](#help) | [AutoComplete](autocomplete.md) | [Input](input.md) | [Password](input.md)

![](./images/AutoComplete.gif)
![](./images/Input.gif)
![](./images/Password.gif)

### Readline 
[**Top**](#help) | [ReadLine](readline.md) | [Input & History /sugestion](basemethods.md#sample-use-of-extraaction)

![](./images/Readline.gif)
![](./images/InputHistory.gif)
![](./images/InputSugestion.gif)

### MaskEdit
[**Top**](#help) | [MaskEdit Generic](maskeditgeneric.md) | [MaskEdit Date/Time](maskeditdate.md) | [MaskEdit Number/Currency](maskeditnumber.md) 

![](./images/MaskEditGeneric.gif)
![](./images/MaskEditDateTime.gif)
![](./images/MaskEditNumber.gif)
![](./images/MaskEditCurrency.gif)

### KeyPress
[**Top**](#help) | [Any key](anykey.md) | [Key Press](keypress.md)

![](./images/Anykey.gif)
![](./images/KeyPress.gif)

### Selectors
[**Top**](#help) | [Select](select.md) | [MultiSelect](multiselect.md)

![](./images/Select.gif)
![](./images/MultSelect.gif)

### Confirm
[**Top**](#help) | [Confirm](confirm.md) | [Slider Switch](sliderswitch.md)

![](./images/Confirm.gif)
![](./images/SliderSwitch.gif)

### WaitProcess
[**Top**](#help) | [WaitProcess](waitprocess.md) 

![](./images/WaitProcess.gif)

### ProgressBar
[**Top**](#help) | [Progress Bar](progressbar.md)

![](./images/ProgressBar.gif)

### Slider Number
[**Top**](#help) | [Slider Number](slidernumber.md) | [Number Up/Down](slidernumber.md)

![](./images/SliderNumber.gif)
![](./images/NumberUpDown.gif)

### List
[**Top**](#help) | [List / List & Sugestion](list.md) | [ListMasked](listmasked.md)

![](./images/List.gif)
![](./images/ListSugestion.gif)
![](./images/MaskedList.gif)

### Browser
[**Top**](#help) | [Browser](browser.md)

![](./images/Browser.gif)

### PipeLine
[**Top**](#help) | [PipeLine](pipeline.md)

![](./images/PipeLine.gif)

### ASCII-Banner
[**Top**](#help) | [ASCII-Banner](banner.md)

![](./images/Banner.gif)


### Colors
[**Top**](#help) | [Colors](colorcmd.md)

![](./images/Colors.gif)

### Commands
[**Top**](#help) | [Commands](commands.md)

![](./images/Commands.gif)


## Supported platforms
[**Top**](#help)

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app
