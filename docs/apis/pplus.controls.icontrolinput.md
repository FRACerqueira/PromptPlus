# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlInput 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlInput

Namespace: PPlus.Controls

Represents the interface with all Methods of the Input control

```csharp
public interface IControlInput : IPromptControls<String>
```

Implements [IPromptControls&lt;String&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-acceptinput"/>**AcceptInput(Func&lt;Char, Boolean&gt;)**

Execute a function to accept char input.
 <br>If result true accept char input; otherwise, ignore char input.

```csharp
IControlInput AcceptInput(Func<Char, Boolean> value)
```

#### Parameters

`value` [Func&lt;Char, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to accept

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-addvalidators"/>**AddValidators(params Func&lt;Object, ValidationResult&gt;[])**

Add a validator to accept sucessfull finish of control.
 <br>Tip: see  to validators embedding

```csharp
IControlInput AddValidators(params Func<Object, ValidationResult>[] validators)
```

#### Parameters

`validators` [Func&lt;Object, ValidationResult&gt;[]](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the function validator.

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;String, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlInput ChangeDescription(Func<String, String> value)
```

#### Parameters

`value` [Func&lt;String, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply change

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlInput Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-default"/>**Default(String)**

Default value when stated.

```csharp
IControlInput Default(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Value default

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-defaultifempty"/>**DefaultIfEmpty(String)**

Default value when finished value is empty.

```csharp
IControlInput DefaultIfEmpty(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Finished value default

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-enabledviewsecret"/>**EnabledViewSecret(Nullable&lt;HotKey&gt;)**

Enable user to view the input without mask.

```csharp
IControlInput EnabledViewSecret(Nullable<HotKey> hotkeypress)
```

#### Parameters

`hotkeypress` [Nullable&lt;HotKey&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Overwrite a [HotKey](./pplus.controls.hotkey.md) to toggle view. Default value is 'F2'

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-filtertype"/>**FilterType(FilterMode)**

Filter strategy for filter items in History colletion
 <br>Default value is FilterMode.StartsWith<br>When  is set to Disabled, the HistoryMinimumPrefixLength value is automatically set to zero

```csharp
IControlInput FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-historyenabled"/>**HistoryEnabled(String)**

Enabled saved history inputs.
 <br>The history file is saved in  in the 'PromptPlus.History' folder.

```csharp
IControlInput HistoryEnabled(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to saved history

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-historymaxitems"/>**HistoryMaxItems(Byte)**

Set maximum items saved on history.After maximum the items are rotates.

```csharp
IControlInput HistoryMaxItems(byte value)
```

#### Parameters

`value` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
maximum items saved

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-historyminimumprefixlength"/>**HistoryMinimumPrefixLength(Int32)**

Minimum chars to enabled history feature. Default value is 0.
 <br>History items are filtered by the starts with entry.<br>When command FilterType set to  Disabled History items the value must be zero

```csharp
IControlInput HistoryMinimumPrefixLength(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Minimum chars number

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-historypagesize"/>**HistoryPageSize(Int32)**

Set max.item view per page on history.Default value for this control is 10.

```csharp
IControlInput HistoryPageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-historytimeout"/>**HistoryTimeout(TimeSpan)**

Set timeout to valid items saved on history. Default value is 365 days.

```csharp
IControlInput HistoryTimeout(TimeSpan value)
```

#### Parameters

`value` [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>
timeout value

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-inputtocase"/>**InputToCase(CaseOptions)**

Transform char input using [CaseOptions](./pplus.controls.caseoptions.md).

```csharp
IControlInput InputToCase(CaseOptions value)
```

#### Parameters

`value` [CaseOptions](./pplus.controls.caseoptions.md)<br>
Transform option

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-issecret"/>**IsSecret(Nullable&lt;Char&gt;)**

The input is a secret. the input text is masked to '#' (default value)

```csharp
IControlInput IsSecret(Nullable<Char> value)
```

#### Parameters

`value` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
char secret

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-maxlength"/>**MaxLength(UInt16)**

MaxLength of input text.The value must be greater than or equal to 1
 <br>Default value is 0 (no limit)

```csharp
IControlInput MaxLength(ushort value)
```

#### Parameters

`value` [UInt16](https://docs.microsoft.com/en-us/dotnet/api/system.uint16)<br>
Length

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite default start value with last result saved on history.

```csharp
IControlInput OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-suggestionhandler"/>**SuggestionHandler(Func&lt;SuggestionInput, SuggestionOutput&gt;)**

Add Suggestion Handler feature

```csharp
IControlInput SuggestionHandler(Func<SuggestionInput, SuggestionOutput> value)
```

#### Parameters

`value` [Func&lt;SuggestionInput, SuggestionOutput&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply suggestions. [SuggestionInput](./pplus.controls.suggestioninput.md) and [SuggestionOutput](./pplus.controls.suggestionoutput.md)

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)

### <a id="methods-validateondemand"/>**ValidateOnDemand(Boolean)**

Execute validators foreach input

```csharp
IControlInput ValidateOnDemand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true execute validators foreach input; otherwise, only at finish.

#### Returns

[IControlInput](./pplus.controls.icontrolinput.md)


- - -
[**Back to List Api**](./apis.md)
