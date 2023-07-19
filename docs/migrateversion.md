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
- Removed: string.\[color\]() 
	- See new Color Syntax
- Changed:
  - PromptPlus.DefaultCulture 
  - TO: PromptPlus.Config.DefaultCulture
- Changed:
  - PromptPlus.CursorPosition 
  - TO: PromptPlus.Config.SetCursorPosition
- Changed:
  - ClearRestOfLine(Consolecolor)
  - TO: ClearRestOfLine(Style)
- Changed:
  - WriteLine(string, forecolor,backcolor)
  - TO: WriteLine(string, style)

### Validators
- Changed:
  - Namespace : PromptPlusValidators
  - TO: PromptValidators  
- Changed:
  - inst.ImportValidators(x => x.MyInput)
  - TO : ImportValidators(inst, x => x.MyInput)

### Controls

- Removed: HideSymbolPromptAndResult
- Removed: EnabledAbortAllPipes
- Removed: EnabledPromptTooltip
- Removed: SaveConfigToFile
- Removed: LoadConfigFromFile
- Removed: Pipeline Contol
- Removed: Readline Contol. 
  - Now, see new console command PromptPlus.ReadLineWithEmacs
- Changed: 
  - AddValidator(Func<object, ValidationResult> validator)
  -  AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
  - TO: AddValidators(params Func<object, ValidationResult>[] validators)
- Changed: 
  - DescriptionSelector(Func<T, string> value);
  - TO: ChangeDescription(Func<string, string> value);
- Changed: 
  - SuggestionHandler(Func<SugestionInput, SugestionOutput> value, bool EnterTryFininsh = false)
  - TO: SuggestionHandler(Func<SugestionInput, SugestionOutput> value);
  
### Autocomplete

- Removed: CompletionInterval(value) 
- Removed: AcceptWithoutMatch()
- Changegd:
  - SpeedAnimation(int value)
  - TO: Spinner(SpinnersType spinnersType, Style? SpinnerStyle = null, int? speedAnimation = null, IEnumerable<string>? customspinner = null)
- Changed: 
  - CaseInsensitive(value)
  - TO: InputToCase(CaseOptions)
- Changed: 
  - CompletionAsyncService(Func<string, int, CancellationToken, Task<string[]>> value);
  - CompletionWithDescriptionAsyncService(Func<string, int, CancellationToken, Task<ValueDescription<string>[]>> value);
  - TO: CompletionAsyncService(Func<string, int, CancellationToken, Task<string[]>> value);

### Select
 
- Changed:
  - HideItem(T value)
  - TO: AddItemTo(AdderScopee, T value)
- Changed:
  - HideItems(IEnumerable<T> value)
  - TO:AddItemsTo(AdderScope scope, IEnumerable<T> values)
- Changed:
  - DisableItem(T value)
  - TO: AddItemTo(AdderScopee, T value)
- Changed:
  - DisableItems(IEnumerable<T> value)
  - TO:AddItemsTo(AdderScope scope, IEnumerable<T> values)
- Changed:
  - Default(T value, Func<T,T,bool> funcfound=null);
  - TO: Default(T value)
- AutoSelectIfOne() 
  - TO: AutoSelect()

### Multiselect

- Changed:
  - AddGroup(IEnumerable<T> value, string group)
  - TO :AddItemGrouped(string group, T value, bool disable = false, bool selected = false)
- Changed:
  - ShowGroupOnDescription(string)
  - TO: AppendGroupOnDescription()
- Changed:
  - HideItem(T value)
  - TO: AddItemTo(AdderScopee, T value)
- Changed:
  - HideItems(IEnumerable<T> value)
  - TO:AddItemsTo(AdderScope scope, IEnumerable<T> values)
- Changed:
  - DisableItem(T value)
  - TO: AddItemTo(AdderScopee, T value)
- Changed:
  - DisableItems(IEnumerable<T> value)
  - TO:AddItemsTo(AdderScope scope, IEnumerable<T> values)
- Changed:
  - AddDefault(T value, Func<T, T, bool> funcfound = null)
  - AddItem(T value, bool disable = false, bool selected = false)
- Changed:
  - AddDefaults(IEnumerable<T> value, Func<T, T, bool> funcfound = null)
  - AddItems(IEnumerable<T> values, bool disable = false, bool selected = false);

### MaskEdit    

- Changed:
  - PromptPlus.MaskEdit(MaskedType, string)
  - TO: PromptPlus.MaskEdit(string) when Generic Type
- Changed:
  - PromptPlus.MaskEdit(MaskedType, string)
  - TO: PromptPlus.MaskEdit(string).Mask(MaskedType) when NOT Generic Type
- Changed:
  - ResultMasked.Value.ObjectValue
  - TO: ResultMasked.Value.Masked
- Changed:
  - Default(object value)
  - TO: Default(string value)
- Changed:
  - Removed AcceptSignal(bool)
  - AmmoutPositions(int, int)   
  - TO: AmmoutPositions(int, int, bool) with AcceptSignal
- Changed:
  - FormatYear.Y4 : FormatYear.Y2 
  - TO: FormatYear.Long, FormatYear.Short
- Changed:
  - ShowDayWeek(FormatWeek) 
  - TO: DescriptionWithInputType(FormatWeek)
- Changed:
  - FillZeros(bool) 
  - TO: FillZeros()
- Changed:
  - UpperCase(bool)
  - TO: InputToCase(CaseOptions)

### Input

- Changed:
  - IsPassword(bool value)
  - TO: IsSecret()
- Changed:
  - InitialValue(string value)
  - TO: Default(string value)
- Changed:
  - Default(string value)
  - TO: DefaultIfEmpty(string value)
	
### Keypress  

- Many Changes, see new Keypress  Control

### Slider (Number/Swith)

- Many Changes, see new Sliders Control

### List 

- Many Changes, see new AddtoList Control
 
### ListMasked 

- Many Changes, see new AddtoMaskEditList Control

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

