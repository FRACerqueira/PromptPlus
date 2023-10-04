# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlMaskEdit 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlMaskEdit

Namespace: PPlus.Controls

Represents the interface with all Methods of the MaskEdit control

```csharp
public interface IControlMaskEdit : IPromptControls<ResultMasked>
```

Implements [IPromptControls&lt;ResultMasked&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-acceptemptyvalue"/>**AcceptEmptyValue(Boolean)**

Accept empty value
 <br>Valid only for type not equal MaskedType.Generic, otherwise this set will be ignored.

```csharp
IControlMaskEdit AcceptEmptyValue(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Accept empty value

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-addvalidators"/>**AddValidators(params Func&lt;Object, ValidationResult&gt;[])**

Add a validator to accept sucessfull finish of control.
 <br>Tip: see  to validators embedding

```csharp
IControlMaskEdit AddValidators(params Func<Object, ValidationResult>[] validators)
```

#### Parameters

`validators` [Func&lt;Object, ValidationResult&gt;[]](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the function validator.

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-ammoutpositions"/>**AmmoutPositions(Int32, Int32, Boolean)**

Defines integer length, decimal length and accept signl.
 <br>Valid only for type MaskedType.Number or Currency, otherwise this set will be ignored.<br>This set is Requeried for these types.

```csharp
IControlMaskEdit AmmoutPositions(int intvalue, int decimalvalue, bool acceptSignal)
```

#### Parameters

`intvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
integer length

`decimalvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
decimal length

`acceptSignal` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
True accept signal; otherwise, no.

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;String, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlMaskEdit ChangeDescription(Func<String, String> value)
```

#### Parameters

`value` [Func&lt;String, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply change

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlMaskEdit Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-culture"/>**Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to validate input when the type is not generic.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlMaskEdit Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use on validate

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-culture"/>**Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to validate input when the type is not generic.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlMaskEdit Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use on validate

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-default"/>**Default(String)**

Default value (with mask!) when stated.

```csharp
IControlMaskEdit Default(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Value default

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-defaultifempty"/>**DefaultIfEmpty(String, Boolean)**

Default value (with mask!) when finished value is empty.

```csharp
IControlMaskEdit DefaultIfEmpty(string value, bool zeroIsEmpty)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Finished value default

`zeroIsEmpty` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Valid only for type MaskedType.Number or MaskedType.Currency, otherwise this set will be ignored.

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-descriptionwithinputtype"/>**DescriptionWithInputType(FormatWeek)**

Append to description the tip of type input.

```csharp
IControlMaskEdit DescriptionWithInputType(FormatWeek week)
```

#### Parameters

`week` [FormatWeek](./pplus.controls.formatweek.md)<br>
show name of week for type date. [FormatWeek](./pplus.controls.formatweek.md)

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-fillzeros"/>**FillZeros(Boolean)**

Fill zeros mask.Default false.
 <br>Not valid for type MaskedType.Generic (this set will be ignored).<br>When used this feature the AcceptEmptyValue feature will be ignored.<br>When MaskedType.Number or MaskedType.Currency this feature is always on.

```csharp
IControlMaskEdit FillZeros(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Fill zeros mask

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-filtertype"/>**FilterType(FilterMode)**

Filter strategy for filter items in History colletion
 <br>Default value is FilterMode.StartsWith

```csharp
IControlMaskEdit FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-formattime"/>**FormatTime(FormatTime)**

Defines time parts input.
 <br>Valid only for type MaskedType.TimeOnly or DateTime, otherwise this set will be ignored.

```csharp
IControlMaskEdit FormatTime(FormatTime value)
```

#### Parameters

`value` [FormatTime](./pplus.controls.formattime.md)<br>
[FormatTime](./pplus.controls.formattime.md)

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-formatyear"/>**FormatYear(FormatYear)**

Defines if year is long or short.
 <br>Valid only for type MaskedType.DateOnly or DateTime, otherwise this set will be ignored.

```csharp
IControlMaskEdit FormatYear(FormatYear value)
```

#### Parameters

`value` [FormatYear](./pplus.controls.formatyear.md)<br>
[FormatYear](./pplus.controls.formatyear.md)

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-historyenabled"/>**HistoryEnabled(String)**

Enabled saved history inputs.
 <br>The history file is saved in  in the 'PromptPlus.History' folder.

```csharp
IControlMaskEdit HistoryEnabled(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to saved history

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-historymaxitems"/>**HistoryMaxItems(Byte)**

Set maximum items saved on history.After maximum the items are rotates.

```csharp
IControlMaskEdit HistoryMaxItems(byte value)
```

#### Parameters

`value` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
maximum items saved

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-historyminimumprefixlength"/>**HistoryMinimumPrefixLength(Int32)**

Minimum chars (without mask!) to enabled history feature.
 <br>History items are filtered by the starts with entry.

```csharp
IControlMaskEdit HistoryMinimumPrefixLength(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Minimum chars number

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-historypagesize"/>**HistoryPageSize(Int32)**

Set max.item view per page on history.Default value for this control is 10.

```csharp
IControlMaskEdit HistoryPageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-historytimeout"/>**HistoryTimeout(TimeSpan)**

Set timeout to valid items saved on history. Default value is 365 days.

```csharp
IControlMaskEdit HistoryTimeout(TimeSpan value)
```

#### Parameters

`value` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>
timeout value

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-inputtocase"/>**InputToCase(CaseOptions)**

Transform char input using [CaseOptions](./pplus.controls.caseoptions.md).

```csharp
IControlMaskEdit InputToCase(CaseOptions value)
```

#### Parameters

`value` [CaseOptions](./pplus.controls.caseoptions.md)<br>
Transform option

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-mask"/>**Mask(String, Nullable&lt;Char&gt;)**

Defines mask input. Rules for Generic type:
 <br>9 - Only a numeric character<br>L - Only a letter<br>C - OnlyCustom character<br>A - Any character<br>N - OnlyCustom character +  Only a numeric character<br>X - OnlyCustom character +  Only a letter<br>\ - Escape character<br>{ - Initial delimiter for repetition of masks<br>} - Final delimiter for repetition of masks<br>[-Initial delimiter for list of Custom character<br>] - Final delimiter for list of Custom character

```csharp
IControlMaskEdit Mask(string value, Nullable<Char> promptmask)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text of masked.

`promptmask` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Prompt mask overwriter. Default value is '■'/'_'

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-mask"/>**Mask(MaskedType, Nullable&lt;Char&gt;)**

Defines type of mask control.

```csharp
IControlMaskEdit Mask(MaskedType maskedType, Nullable<Char> promptmask)
```

#### Parameters

`maskedType` [MaskedType](./pplus.controls.maskedtype.md)<br>
[MaskedType](./pplus.controls.maskedtype.md)

`promptmask` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Prompt mask overwriter. Default value is '■'/'_'

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-negativestyle"/>**NegativeStyle(Style)**

Overwrite [Style](./pplus.style.md) to region neggative input.
 <br>Default Foreground : 'StyleControls.Answer'<br>Default Background : Same Console Background when set

```csharp
IControlMaskEdit NegativeStyle(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
Style

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite default start value with last result saved on history.

```csharp
IControlMaskEdit OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-positivestyle"/>**PositiveStyle(Style)**

Overwrite [Style](./pplus.style.md) to region positive input.
 <br>Default Foreground : 'StyleControls.Answer'<br>Default Background : Same Console Background when set

```csharp
IControlMaskEdit PositiveStyle(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
Style

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-suggestionhandler"/>**SuggestionHandler(Func&lt;SuggestionInput, SuggestionOutput&gt;)**

Add Suggestion (with mask!) Handler feature

```csharp
IControlMaskEdit SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value)
```

#### Parameters

`value` [Func&lt;SuggestionInput, SuggestionOutput&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply suggestions. [SuggestionInput](./pplus.controls.suggestioninput.md) and [SuggestionOutput](./pplus.controls.suggestionoutput.md)

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-typetipstyle"/>**TypeTipStyle(Style)**

Overwrite [Style](./pplus.style.md) to region tip type input.
 <br>Default Foreground : 'ConsoleColor.Yellow'<br>Default Background : same Console Background when set

```csharp
IControlMaskEdit TypeTipStyle(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
Style

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)

### <a id="methods-validateondemand"/>**ValidateOnDemand(Boolean)**

Execute validators foreach input

```csharp
IControlMaskEdit ValidateOnDemand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true execute validators foreach input; otherwise, only at finish.

#### Returns

[IControlMaskEdit](./pplus.controls.icontrolmaskedit.md)


- - -
[**Back to List Api**](./apis.md)
