![Logo](https://raw.githubusercontent.com/FRACerqueira/PromptPlus/refs/heads/main/icon.png)

# **Welcome to PromptPlus**

**The best tool to Interactive command-line toolkit for .NET Core with powerful controls and commands to create professional console applications.**
 **PromptPlus** was developed in C# with the **.NET 8**, **.NET 9**, **.NET 10** target frameworks.
All controls input/filter using [**GNU Readline**](https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts.  

PromptPlus **Supports 4/8/24-bit colors** in the terminal with auto-detection of the current terminal's capabilities and automatic color conversion.

#### [Visit the official page for complete documentation of PromptPlus](https://github.com/FRACerqueira/PromptPlus)

## What's new in V5.0.1

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

## What's new in V5.0.0 (version release)

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

## **Supported platforms**

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

## License

Copyright 2025 @ Fernando Cerqueira

PromptPlus is licensed under the MIT license. 

