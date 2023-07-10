# <img align="left" width="100" height="100" src="./images/icon.png">PromptPlus Migrate

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Main**](index.md#table-of-contents)  

**PromptPlus v4** has been **completely rebuilt** for a better experience, with significant improvements with new controls and more developer power. The console driver now supports better rendering, with the ability to detect terminal capabilities and allow for 24-bit color, text overflow strategies based on terminal size, and left and right margins for a nicer layout.
**The Controls have been revised to be more responsive, allow color styles in many of their elements**, and adapt to the terminal size even with resizing.

### Console

- Removed: string.Underline()
- Removed: string.Strikeout()
- Removed: string.\[color\](), see new Color Syntax
- PromptPlus.DefaultCulture 
  - TO: PromptPlus.Config.DefaultCulture
- PromptPlus.CursorPosition 
  - TO: PromptPlus.Config.SetCursorPosition
- ClearRestOfLine(Consolecolor)
  - TO: ClearRestOfLine(Style)
- WriteLine(string, forecolor,backcolor)
  - TO: WriteLine(string, style)

### Validators

- Renamed: Namespace : PromptPlusValidators
  - TO: PromptValidators  
- inst.ImportValidators(x => x.MyInput)
  - TO : PromptValidators.ImportValidators(inst, x => x.MyInput)

### Controls

- Removed: SaveConfigToFile
- Removed: LoadConfigFromFile
- Removed: Pipeline Contol
- Removed: Readline Contol, Now, see PromptPlus.ReadLineWithEmacs
- AddValidator(...)
    - TO: AddValidators(...)
- DescriptionSelector(string) 
  - TO: ChangeDescription(string)
- UpperCase(bool)
  - TO: InputToCase(CaseOptions)
- LowerCase(bool)
  - TO: InputToCase(CaseOptions)
  
### Autocomplete

- Removed: CompletionInterval(value) - AutoComplete Control
- Removed: AcceptWithoutMatch() - AutoComplete Control
- Task<ValueDescription<string>[] ServiceCompleteAsync(string prefixText, int count, CancellationToken cancellationToken)
  - TO: Task<string[]> ServiceCompleteAsync(string prefixText, int count, CancellationToken cancellationToken)

### Select
 
- HideItem(PromptPlus.BackgroundColor) 
  - TO: AddItemTo(AdderScope.Remove, PromptPlus.BackgroundColor)
- AutoSelectIfOne() 
  - TO: AutoSelect()

### MaskEdit    

- PromptPlus.MaskEdit(MaskedType, string)
  - TO: PromptPlus.MaskEdit(string) when Generic Type
- PromptPlus.MaskEdit(MaskedType, string)
  - TO: PromptPlus.MaskEdit(string).Mask(MaskedType) when NOT Generic Type
- ResultMasked.Value.ObjectValue
  - TO: ResultMasked.Value.Masked
- Default(object)
  - TO: Default(string)
- DescriptionSelector(ResultMasked) 
  - TO: ChangeDescription(string)
- AmmoutPositions(int, int)   
  - TO: AmmoutPositions(int, int, bool - AcceptSignal)
- FormatYear.Y4 : FormatYear.Y2 
    - TO: FormatYear.Long, FormatYear.Short
- ShowDayWeek(FormatWeek) 
  - TO: DescriptionWithInputType(FormatWeek)
- FillZeros(bool) 
  - TO: FillZeros()
- Removed: AcceptSignal(bool)
 
### Keypress  

- PromptPlus.KeyPress(string, ConsoleModifiers)
  - TO: AddKeyValid(ConsoleKey, ConsoleModifiers)

### Slider

- SliderNumber(SliderNumberType, string)
  - TO: MoveKeyPress(SliderNumberType)

### Input

- IsPassword(true)
  - TO: IsSecret()

### List 

- Renamed to AddToList
- Removed: List<T>,now T is string
- Removed: InitialValue(value)
- Removed: ValidateOnDemand()
- AllowDuplicate(bool) 
  - TO: AllowDuplicate()
 
### ListMasked 

- Renamed to AddtoMaskEditList
- See changes to MasKedit

### Multiselect

- AddGroup(IEnumerable<T>,string)
  - TO :AddItemsGrouped(string,IEnumerable<T>)
- ShowGroupOnDescription(string)
  - TO: AppendGroupOnDescription()

### Confirm

- Many Changes, see new Confirm Control

### Browser

- Many Changes, see new Browse Control

### Tasks

- Many Changes, see new WaitProcess Control

### Progressbar

- Many Changes, see new Progressbar Control

- - - 
[**Top**](#promptplus-migrate)

