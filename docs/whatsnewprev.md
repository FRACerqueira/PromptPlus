![Logo](../icon.png)

# PromptPlus Previous versions

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

----
### V.4.2.0
[**Main**](../README.md) | [**Top**](#promptplus-previous-versions)

- Added .NET8 target frameworks.
- Split of feature:
    - PromptPlus.TableSelect\<T> to Select item in table : Select row, column and data in a grid/table 
    - PromptPlus.Table\<T> to write table in console : Show data in a grid/table 
- New Control : TableMultSelect\<T> :  Select multi-data in a grid/table 
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
    - Color Token now accepts ':' to separate foreground color from background color
    - eg: [RED:BLUE] = [RED ON BLUE]
- Improvement : 
    - Optimized the Calendar control to have symbols when selecting elements
- Improvement :
    - Optimize Render of ProgressBar (less lines)
- Improvement : 
    - Optimize Render of SliderNumber (less lines)
- Improvement : 
    - Added Styles command for custom colors on all controls
        - Removed the ApplyStyle command from the Config interface (now use the Styles command)
        - Added ToStyle() extension for Color Class (less code typed)
- Improvement : 
    - Added command HideRange to not show range (Min/Max values) in the SliderNumber control    
- Improvement : 
    - Optimize resource usage in rendering (less cultural dependency)
- Improvement : 
    - Reinforce the validation of invalid or optional parameters in all controls
- Improvement : 
    - Remove code copy (MIT license) from other project and applied package (for lower maintenance)
- Improvement : 
    - Optimized the WaitControl control (for cancel correctly tasks)
    - Removed property Context (EventWaitProcess) 
    - Added Method ChangeContext(Action<T> action) in EventWaitProcess (for change context over thread safe)
    - Renamed command 'CancelAllNextTasks' to 'CancelAllTasks' (WaitControl)
- Documentation: 
    - Examples of snapshot controls updated to reflect layout changes and reduced image size (faster page loading)
    - Reviewed credit references and licenses
- Renamed command: 
    - 'DescriptionWithInputType' to 'ShowTipInputType'.
    - Now extra-line to tip InputType
- Renamed command: 
    - 'AppendGroupOnDescription' to 'ShowTipGroup'.
    - Now extra-line to tip group
- Fixed bug : 
    - Table control does not render correctly when it does not support Unicode
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
- Fixed bug : 
    - Refinement of Unicode symbol rendering in all controls (Corret render)
- Removed Control Pipeline

----
### V4.1.0
[**Main**](../README.md) | [**Top**](#promptplus-previous-versions)

- New Control : Table<T> Display/Select data in a grid/table
    - Main features :
        - More than 100 layout combinations
        - Navigation by row and columns
        - Scroll the table when it is larger than the screen
        - Split text when it is larger than the column size
        - Automatic header and column completion
        - Color customization of each element
        - Search for data filtered by columns
        - Formatting by column or by data type definition

- Improvement commands with default values ​​(all controls)
- Bug fixed: grouped item ordering. The sort option will be ignored
    - Affeted Controls : Select/MultiSelect
- Bug fixed: 'AcceptInput' method causes failure by not allowing navigation keys to be selected.
    - Affeted Controls : AddtoList/Input
- Improvement : Direct writes to standard error output stream
    - New Commands : OutputError()
- New feature: Escaping format characters color 
    - Global property : IgnoreColorTokens
    - New Commands : EscapeColorTokens()/AcceptColorTokens()
- New feature: Group items in the select control
- New feature: Add separator line in the select control

**Special thanks to [ividyon](https://github.com/ividyon) for suggesting improvements and actively participating in this release**


----
### V4.0.5
[**Main**](../README.md) | [**Top**](#promptplus-previous-versions)

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

----
### V4.0.4
[**Main**](../README.md) | [**Top**](#promptplus-previous-versions)

- Fixed bug PromptPlus not restore terminal original setting when shutdown application
- Fixed bug Autocomplete does not change result when backspace is pressed during search
- Added Property CurrentBuffer in console drive to return Current Buffer running (Primary/Secondary)
- Added SwapBuffer command to switch Primary/Secondary buffer (Valid only When console 'ansi'  supported)
- Renamed 'AlternateScreen' to 'RunOnBuffer'. Now executes a custom action on TargetBuffer and returns to CurrentBuffer
- Refactored console drivers initialization, control options initialization
- Added auto create Environment 'PromptPlusConvertCodePage' to custom automate convert codepage to unicode-codepage
    - Default value is = '850;65001'

----
### V4.0.3
[**Main**](../README.md) | [**Top**](#promptplus-previous-versions)

- New control to switch Alternate screen 
- Fixed bug Console does not change foreground/background color correctly
- Fixed bug Control ProgressBar
    - Not show gradient when set ProgressBarType.Fill
- Improve testability of result classes/struct (Internal to public)

----
### V4.0.2
[**Main**](../README.md) | [**Top**](#promptplus-previous-versions)

- New Control Pipeline
    - PromptPlus.Pipeline(T startvalue)
- Changed WaitControl to take context value in tasks and return context in result
    - There are small break-changes  
- Add Answer key check equals "Yes"/"No" using config values
    - IsYesResponseKey(this ConsoleKeyInfo keyinfo)
    - IsNoResponseKey(this ConsoleKeyInfo keyinfo)

----
### V4.0.0
[**Main**](../README.md) | [**Top**](#promptplus-previous-versions)

- Newest controls and color improvement and layout
    - Calendar
    - Chartbar
    - Browser multi-select
    - Treeview
    - WaitTask
    - Progressbar
