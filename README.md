![Logo](icon.png)

# Welcome to PromptPlus

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)


**The best tool to Interactive command-line toolkit for .NET Core with powerful controls and commands to create professional console applications.**
 **PromptPlus** was developed in C# with the **.NET 8**, **.NET 9**, **.NET 10** target frameworks.

----
## Table of Contents

- [What's new in the latest version](#whats-new-in-the-latest-version)
- [Features](#features)
- [Installing](#installing)
- [Console Engine](#console-engine)
- [Culture](#culture)
- [Keys navigation, hotkeys and Extensions Emacs](#keys-navigation)
- [Colors](#colors)
- [Examples](#examples)
- [Documentation](#documentation)
- [Supported Platforms](#supported-platforms)
- [Code of Conduct](#code-of-conduct)
- [Contributing](#contributing)
- [Credits](#credits)
- [License](#license)
- [Previous versions](./docs/whatsnewprev.md)

----
## What's new in the latest version

### V5.0.1
[**Top**](#table-of-contents)

- Removed: Feature multithreading (Incompatible when another process uses the same output stream).
- Fixed: Read properties to global config to all controls.
- Fixed: MaxWidths to global config to all controls.
- Fixed: ProgressBar Issue display % when has custom range.
- Fixed resources for en-US and pt-BR languages.
- Improved FileSelect control with History and DefaultHistory command.
- Improved KeyPress control with Timeout and ShowCountDown command.
- Improved ProgressBar control with context parameters and result context.
- Improved MultiSelect control with DefaultHistory command.
- Improved Select control with DefaultHistory command.
- Improved WaitProcess control with ExtraInfoProcess for provides functionality to update the extra information associated with a state process.
- Improved show only seleted items for controls with multi selected items.
- Improved History control with ReadHistory command.
- Improved EnableMessageAbortCtrlC property to indicate whether a message is displayed when the operation is aborted by pressing Ctrl+C .
- Improved Chartbar control with MaxLengthLabel command and options to show only legends.
- improved code quality and refactoring.
- Updated dependencies to latest versions.
- Updated samples to use latest version.
- Updated documentation.

### V5.0.0 (version release)

[**Top**](#table-of-contents)

We're very excited about the **big release** of this new version. **Version 5 has been completely redesigned** and optimized for better stability, consistency, and performance. 
All controls and behaviors have been revisited and improved to ensure sustainable evolution. 
Due to the significant modifications, version 5 introduced **significant changes and is incompatible with versions 4.x**, although the concepts and components are very similar, requiring a small learning curve and minor methodological adjustments.

- PromptPlus has **more than 37! controls/widgets**
- Support for **.Net10,.Net9 and .Net8**
- External references were reviewed and only those necessary for size treatment for East Asian characters were used.
- **New control rendering engine** adjusts more fluidly to the screen size and avoids flickering by redrawing only the changed lines.
- Revised control of hotkeys and special characters ensuring consistency according to the console's capabilities.
- Created the separation of interactive controls and **added several non-interactive controls (widgets)**
- Renaming several control methods for better clarity and reduced scope, aiming at the unique responsibility that each component intends to perform, allowing for sustainable evolution.
- Revised concept of an editing window for controls that require a significant input/response size , ensuring visual consistency and adequate navigability.
- A slice architecture was adopted for each component, allowing individual evolution of each one with low interference to the others.
- All interative controls start at : **PromptPlus.Controls**.\<name of control\>.
    - All initialization contracts have been standardized: PromptPlus.Controls.\<name of control\>(string prompt = "", string? description = null).
- All no interative controls start at : **PromptPlus.Widgets**.\<name of control\>.
    - For each non-interactive control the initialization contract was customized.
- All commands for console start at : **PromptPlus.Console**.\<command\>.
- All general config start at : **PromptPlus.Config**.\<config\>.
- **NEW tooltip mechanism**: Now shows all keys and hotkeys for each control by switching the view ('F1').
- **NEW controls group**: Remote sources.This control group is in the **experimental phase** and addresses the scenario of resolving the domain set of collections from an external source (e.g., database, cloud, API with pagination).
    - Remote Select for any type
    - Remote Multi Select for any type
    - Remote Nodetree select for any type
    - Remote Nodetree Multi select for any type
- **New console state preservation with capability to manipulate the console Ctrl+C / Ctrl+Break**.
- **NEW external file config 'PromptPlus.config'** to customize global behaviors of PromptPlus without code changes.

A more detailed list of changes and basic Concepts **[for each of the controls can be seen here](./docs/whatsnewcontrols.md)**. 

----
## Features
[**Top**](#table-of-contents)

**All features have IntelliSense. PromptPlus has more than 37! controls/widgets with many features like: filters, validators, history, suggestions, spinner(20 embedding type), colors(Supports 4/8/24-bit colors) and customizable element styles for each control** :

- AutoComplete with spinner
- Banner Ascii
- Calendar with multiple layouts
- Calendar Widget **NEW!** 
- ChartBar with switch layout, Legend and order
- ChartBar Widget **NEW!** 
- File and Folder multi-select
- File and Folder select
- Hisytory **NEW!** 
- Input text
- Input Secret 
- KeyPress
- MaskEdit Generic (string) **NEW!** 
- MaskEdit Date (DateTime/DateOnly) **NEW!** 
- MaskEdit Time (DateTime/TimeOnly) **NEW!** 
- MaskEdit DateTime **NEW!** 
- MaskEdit Number (integer,long,double,decimal) **NEW!** 
- MaskEdit Currency (double,decimal) **NEW!** 
- Multi-Select for any type (with group!) 
- Nodes hierarchical structures multi-select for any type
- Nodes hierarchical structures select for any type
- Progress bar with 6 types , gradient colors and spinner
- ReadLine Emacs **NEW!** 
- Remote Select for any type **NEW! EXPERIMENTAL!** 
- Remote Multi Select for any type **NEW! EXPERIMENTAL!** 
- Remote NodeTree Select for any type **NEW! EXPERIMENTAL!** 
- Remote NodeTree Multi Select for any type **NEW! EXPERIMENTAL!** 
- Select for any type (with group!) 
- Slider numeric ranger with gradient colors
- Slider  Widget **NEW!** 
- Switch (style on/off)
- Switch  Widget **NEW!** 
- Table structures multi-select for any type and multiple layouts
- Table structures select for any type and multiple layouts
- Table Widget **NEW!**
- Wait Process (Run background tasks Sequential/Parallel) with elapsedtime and spinner 
- Wait Time with countdown and spinner

A **complete list of all controls and widgets [snapshots can be seen here](./docs/snapshots.md)**

### snapshots (small sample)
![](./docs/images/fileselect.jpg)

![](./docs/images/progressbar.jpg)

![](./docs/images/chartbar.jpg)

![](./docs/images/consolecolorcapacity.jpg)

----
## Installing
[**Top**](#table-of-contents)

```
Install-Package PromptPlus [-pre]
```

```
dotnet add package PromptPlus [--prerelease]
```

**_Note:  [-pre]/[--prerelease] usage for pre-release versions_**

----
## Console Engine
[**Top**](#table-of-contents)

The console driver has the ability to detect terminal capabilities and allow for 24-bit color and text overflow strategies based on terminal size, and left and right margins for a nicer layout and automatic color conversion. 

The new engine detects support ansi commands and adjust output for this functionality respecting OS differences , terminal mode and Windows console mode. The Colors are automatically adjusted to the capacity of the terminal. 

This automatic adjustment may slightly modify the final color when converting to a lower bit resolution, in addition to detecting color tokenization in texts.

_Note: If the color tokenization is invalid, the value will be written without applying the color tokens, keeping the tokens as text._

```csharp
PromptPlus.Console.WriteLine("Test", new Style(ConsoleColor.Red, ConsoleColor.White, Overflow.None));
PromptPlus.Console.WriteLine("Test", new Style(Color.White, Color.Red, Overflow.None));
PromptPlus.Console.WriteLine("Test", new Style(new Color(255, 255, 255), Color.Red, Overflow.None));
PromptPlus.Console.WriteLine("Test", new Style(Color.FromConsoleColor(ConsoleColor.White), Color.Red, Overflow.None));
PromptPlus.Console.WriteLine("Test", new Style(Color.FromInt32(255), Color.Red, Overflow.None));
PromptPlus.Console.WriteLine("Test", new Style(Color.FromHtml("#ffffff"), Color.Red, Overflow.None));

PromptPlus.Console.WriteLine("[RGB(255,0,0):WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text");
PromptPlus.Console.WriteLine("[RED:WHITE]Test[bLUE] COLOR[/] BACK COLOR[/] other text");

PromptPlus.Widgets.SingleDash("Test SingleDash", DashOptions.DoubleBorder, 1, Style.Default().ForeGround(ConsoleColor.Red).Background(ConsoleColor.Yellow));
PromptPlus.Widgets.SingleDash("[RGB(255,0,0) ON WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text", DashOptions.AsciiSingleBorder, 1);
PromptPlus.Widgets.DoubleDash("Test SingleDash", DashOptions.DoubleBorder, 1, Style.Default().ForeGround(ConsoleColor.Red).Background(ConsoleColor.Yellow));
PromptPlus.Widgets.DoubleDash("[RGB(255,0,0) ON WHITE]Test[GREEN] COLOR[/] BACK COLOR [/] other text", DashOptions.AsciiSingleBorder, 1);

```
### About console state preservation

The Prompt Plus library aims to ensure that your logic executes without errors or failures as much as possible.

In this sense, when using the library, it maintains the initial state of the console colors and the initial culture of the execution thread. 

When the application terminates normally or when a critical error occurs, these properties are restored. All other properties must be guaranteed by the application.

The **ResetBasicStateAfterExist property** allows you to change this characteristic when its value is false.

Whenever a **critical error occurs, a log file is generated** before completion in the folder located at - \{SpecialFolder.UserProfile\}/PromptPlus.Log. This file is generated daily and kept for a maximum of 7 days.

### About console Ctrl+C / Ctrl+Break

The Prompt Plus library monitors the Ctrl+C / Ctrl+Break keys. 

When it is not handled by the 'CancelKeyPress' command, every time the application is closed it restores the initial state of colors, cursor, Culture and returns to the primary console buffer.

The Prompt Plus library provides the ability to manipulate the behavior of Ctrl+C/Ctrl+Break using the command:
- CancelKeyPress(AfterCancelKeyPress behaviorcontrols, Action\<object?, ConsoleCancelEventArgs\> actionhandle)

  The behavior after a cancel event can be configured using the `AfterCancelKeyPress` property:
  - AfterCancelKeyPress options:
    - Default: Follows the default action after a cancellation.
    - AbortCurrentControl: Abort the current control's operation when cancel key is pressed and continue with next operation.
    - AbortAllControl: Abort the All control's operation when cancel key is pressed and continue with next operation.

To remove the current cancel key press handler:
- RemoveCancelKeyPress() - Restores default console Ctrl+C/Break handling behavior.

To check if a cancel event has occurred:
- UserPressKeyAborted (read-only) - Returns true if the operation was aborted by Ctrl+C/Break.

----
## Culture
[**Top**](#table-of-contents)

PromptPlus applies the language/culture **only when running controls**. The language/culture of the application is **not affected**. If language/culture is not informed, the application's language/culture will be used with fallback to en-US.

All messages are affected when changed language/culture. PromptPlus has languages embedded:
- en-US (Default)
- pt-BR

To use a non-embedded language/culture:

- Copy the **PromptPlusResources.resx** file in folder PromptPlus/Resources
- Translate messages with same format to your language/culture
- Convert .resx files to binary .resources files ([**reference link here**](https://docs.microsoft.com/en-us/dotnet/core/extensions/work-with-resx-files-programmatically))
- Publish the compiled file (**PromptPlus.[Language].resources**) in the same folder as the binaries.

----
## Keys navigation
[**Top**](#table-of-contents)

### For text navigation

PromptPlus adopts the keyboard **left/right arrows, Home, End** keys for navigation, extending its functionality with the emacs combinations below:

- CTRL+T : To transpose the previous two characters.
- CTRL+L : To clears the content.
- CTRL+H : To deletes the previous character (equivalent to the backspace key).
- CTRL+E : To moves the cursor to the line end (equivalent to the end key).
- CTRL+A : To moves the cursor to the line start (equivalent to the home key).
- CTRL+B : To moves the cursor back one character (equivalent to the left arrow key).
- CTRL+F : To Moves the cursor forward one character (equivalent to the right arrow key).
- CTRL+D : To delete the current character (equivalent to the delete key).
- CTRL+U : To clears the line content before the cursor.
- CTRL+K : To clears the line content after the cursor.
- CTRL+W : To clear the word before the cursor.
- ALT+L : To lowers the case of every character from the cursor's position to the end of the current word.
- ALT+U : To upper the case of every character from the cursor's position to the end of the current word.
- ALT+C : To capitalizes the character under the cursor and moves to the end of the word.
- ALT+D : To clear the word after the cursor.
- ALT+F : To moves the cursor forward one word.
- ALT+B : To moves the cursor backward one word.
- INSER : To toggle input replacement mode (default/started in insert mode).
- ESC : (feature optional) to abort input and return default value and flag aborted.

### Console KeyInfo Extensions

- IsPressSpecialKey(this ConsoleKeyInfo keyinfo, ConsoleKey key, ConsoleModifiers modifier)
- IsPressEnterKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsAbortKeyPress(this ConsoleKeyInfo keyinfo)
- IsLowersCurrentWord(this ConsoleKeyInfo keyinfo)
- IsClearBeforeCursor(this ConsoleKeyInfo keyinfo)
- IsClearAfterCursor(this ConsoleKeyInfo keyinfo)
- IsClearWordBeforeCursor(this ConsoleKeyInfo keyinfo)
- IsClearWordAfterCursor(this ConsoleKeyInfo keyinfo)
- IsCapitalizeOverCursor(this ConsoleKeyInfo keyinfo)
- IsForwardWord(this ConsoleKeyInfo keyinfo)
- IsBackwardWord(this ConsoleKeyInfo keyinfo)
- IsUppersCurrentWord(this ConsoleKeyInfo keyinfo)
- IsTransposePrevious(this ConsoleKeyInfo keyinfo)
- IsClearContent(this ConsoleKeyInfo keyinfo)
- IsPressTabKey(this ConsoleKeyInfo keyinfo)
- IsPressShiftTabKey(this ConsoleKeyInfo keyinfo)
- IsPressEndKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressHomeKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressCtrlHomeKey(this ConsoleKeyInfo keyinfo)
- IsPressCtrlEndKey(this ConsoleKeyInfo keyinfo)
- IsPressBackspaceKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressDeleteKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressLeftArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressSpaceKey(this ConsoleKeyInfo keyinfo)
- IsPressCtrlSpaceKey(this ConsoleKeyInfo keyinfo)
- IsPressRightArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressUpArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressDownArrowKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressPageUpKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressPageDownKey(this ConsoleKeyInfo keyinfo, bool emacskeys = true)
- IsPressEscKey(this ConsoleKeyInfo keyinfo)
- IsYesResponseKey(this ConsoleKeyInfo keyinfo)
    - Check if ConsoleKeyInfo is equal to PromptPlus.Config.YesChar
- IsNoResponseKey(this ConsoleKeyInfo keyinfo)
    - Check if ConsoleKeyInfo is equal to PromptPlus.Config.NoChar 

### For list/colletion

PromptPlus adopts the keyboard **Up/Down arrows, Ctrl+Home, Ctrl+End, PageUp and PageDown** keys items navigation.

For multiple-selection lists, a **space** is used to indicate a check mark. The **Shift+space** combination is used in text input to differentiate it from a check mark, when applicable.

### For expand/collapse

PromptPlus adopts the keyboard **'+', Alt '+'** to expand/expand all and **'-'** to collapse.

----
## Colors
[**Top**](#table-of-contents)

The color class has implicit conversion to ConsoleColor. There are conversion methods to facilitate compatibility with other common color representations:

- Html
	-	Converts string color Html format (#RRGGBB).
- Int32
	-	Converts a standard color number (0 ~ 255).
- Console Color
	-	Converts a System.Console.ConsoleColor
- RGB
	-	Converts a RGB format (R,G,B)
- Name
	-	Converts a name standard color
      
The standard color table (0 - 255) can be [**viewed here**](./docs/colors.md).

    
----
## Examples
[**Top**](#table-of-contents)

The folder [**Samples**](https://github.com/FRACerqueira/PromptPlus/tree/main/samples) contains more **30!** projects samples.

```
dotnet run --project [name of sample]
```
----
## Documentation
[**Top**](#table-of-contents)

The library is well documented. The documentation is available in the [Docs directory](./docs/api/docindex.md).

----
## Supported platforms
[**Top**](#table-of-contents)

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

----
## Code of Conduct
[**Top**](#table-of-contents)

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [Code of Conduct](CODE_OF_CONDUCT.md).

----
## Contributing

See the [Contributing guide](CONTRIBUTING.md) for developer documentation.

----
## Credits
[**Top**](#table-of-contents)

Prompt Plus may **include pieces of code (copy)** from other software released under the MIT License:

- Color/Engine Console - [Spectre.Console](https://spectreconsole.net/),  Copyright (c) 2020 Patrik Svensson, Phil Scott, Nils Andresen. See [LICENSE](Licenses/LICENSE-SpectreConsole.md).

- Banner Ascii - [FIGlet](https://github.com/auriou/FIGlet), Copyright (c) 2014 Philippe AURIOU. See [LICENSE](Licenses/LICENSE-FIGlet.md).  


**API documentation generated by**

- [XmlDocMarkdown](https://github.com/ejball/XmlDocMarkdown), Copyright (c) 2024 [Ed Ball](https://github.com/ejball)
    - See an unrefined customization to contain header and other adjustments in project [XmlDocMarkdownGenerator](https://github.com/FRACerqueira/PromptPlus/tree/main/src/XmlDocMarkdownGenerator)  

**Special thanks**

- [ividyon](https://github.com/ividyon) for their continued contributions to product improvement.

----
## License
[**Top**](#table-of-contents)

Copyright 2025 @ Fernando Cerqueira

PromptPlus is licensed under the MIT license. See [LICENSE](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE).

