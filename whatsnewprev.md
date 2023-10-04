# <img align="left" width="100" height="100" src="./docs/images/icon.png">PromptPlus What's new

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

## What's new in V4.0.5
[**Main**](https://github.com/FRACerqueira/PromptPlus/#table-of-contents) | [**Top**](#promptplus-whats-new)

- Added new global propety 'ExtraExceptionInfo' to write extra console exception info
- Added new global propety 'DisableToggleTooltip' to disable toggle Tooltip
- Added new global propety 'ShowOnlyExistingPagination' to disable Page information when pagination not exists 
- Added new Method DisableToggleTooltip(bool value) to overwrite default DisableToggleTooltip in control 
- Added new Method ShowOnlyExistingPagination(bool value) to overwrite default Overwrite default Show pagination only if exists in control
- Added new item to Enum FilterMode : 'Disabled'. This item disable filter feature in coletions
- Improved to not show text prompt when text value is null or empty
- Improved terminal mode detection (for Windows 11 Versions)
- Rebuilt FIGlet to MIT License 
- Fixed credits (MIT License Copyright) 
- Fixed bug PromptPlus not restore StyleSchema when ResetColor
- Fixed Spell checking (Breaking Changes)
    - SugestionInput to SuggestionInput
        - Controls: MaskEdit/AddTolist/Input
    - SugestionOutput to SuggestionOutput
        - Controls: MaskEdit/AddTolist/Input
    - MaxLenght to MaxLength 
        - Controls: AutoComplete/AddTolist/Input
    - StyleControls.Sugestion to StyleControls.Suggestion
    - StyleSchemaExtensions.Sugestion to StyleSchemaExtensions.Suggestion
    - PromptPlusException.Plataform to PromptPlusException.Platform

**Special thanks to [ividyon](https://github.com/ividyon) for spell checking corrections, all documentation, fixed credits (MIT License Copyright) and wrong method/property names**

## What's new in V4.0.4
[**Main**](https://github.com/FRACerqueira/PromptPlus/#table-of-contents) | [**Top**](#promptplus-whats-new)

- Fixed bug PromptPlus not restore terminal original setting when shutdown application
- Fixed bug Autocomplete does not change result when backspace is pressed during search
- Added Property CurrentBuffer in console drive to return Current Buffer running (Primary/Secondary)
- Added SwapBuffer command to switch Primary/Secondary buffer (Valid only When console 'ansi'  supported)
- Renamed 'AlternateScreen' to 'RunOnBuffer'. Now executes a custom action on TargetBuffer and returns to CurrentBuffer
- Refactored console drivers initialization, control options initialization
- Added auto create Environment 'PromptPlusConvertCodePage' to custom automate convert codepage to unicode-codepage
    - Default value is = '850;65001'

## What's new in V4.0.3
[**Main**](https://github.com/FRACerqueira/PromptPlus/#table-of-contents) | [**Top**](#promptplus-whats-new)

- New control to switch Alternate screen 
- Fixed bug Console does not change foreground/background color correctly
- Fixed bug Control ProgressBar
    - Not show gradient when set ProgressBarType.Fill
- Improve testability of result classes/struct (Internal to public)

## What's new in V4.0.2
[**Main**](https://github.com/FRACerqueira/PromptPlus/#table-of-contents) | [**Top**](#promptplus-whats-new)

- New Control Pipeline
    - PromptPlus.Pipeline(T startvalue)
- Changed WaitControl to take context value in tasks and return context in result
    - There are small break-changes  
- Add Answer key check equals "Yes"/"No" using config values
    - IsYesResponseKey(this ConsoleKeyInfo keyinfo)
    - IsNoResponseKey(this ConsoleKeyInfo keyinfo)

## What's new in V4.0.0
[**Main**](https://github.com/FRACerqueira/PromptPlus/#table-of-contents) | [**Top**](#promptplus-whats-new)

### Newest controls and color improvement and layout

![](./Docs/images/calendar1.gif)
![](./Docs/images/chartbar1.gif)
![](./Docs/images/multiselectbrowser1.gif)
![](./Docs/images/treeview1.gif)
![](./Docs/images/treeview2.gif)
![](./Docs/images/waittask1.gif)
![](./Docs/images/progressbar1.gif) 
