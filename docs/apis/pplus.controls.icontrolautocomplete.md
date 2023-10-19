# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlAutoComplete 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlAutoComplete

Namespace: PPlus.Controls

Represents the interface with all Methods of the AutoComplete control

```csharp
public interface IControlAutoComplete : IPromptControls<String>
```

Implements [IPromptControls&lt;String&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-acceptinput"/>**AcceptInput(Func&lt;Char, Boolean&gt;)**

Execute a function to accept char input.
 <br>If result true accept char input; otherwise, ignore char input.

```csharp
IControlAutoComplete AcceptInput(Func<Char, Boolean> value)
```

#### Parameters

`value` [Func&lt;Char, Boolean&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to accept

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-addvalidators"/>**AddValidators(params Func&lt;Object, ValidationResult&gt;[])**

Add a validator to accept sucessfull finish of control.
 <br>Tip: see  to validators embedding

```csharp
IControlAutoComplete AddValidators(params Func<Object, ValidationResult>[] validators)
```

#### Parameters

`validators` [Func&lt;Object, ValidationResult&gt;[]](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the function validator.

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;String, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlAutoComplete ChangeDescription(Func<String, String> value)
```

#### Parameters

`value` [Func&lt;String, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply change

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-completionasyncservice"/>**CompletionAsyncService(Func&lt;String, Int32, CancellationToken, Task&lt;String[]&gt;&gt;)**

The function to execute autocomplete. This function is requeried to run!
 <br>First param is a current text input<br>Second param is current cursor postion at text input<br>third parameter is the control cancellation token

```csharp
IControlAutoComplete CompletionAsyncService(Func<String, Int32, CancellationToken, Task<String[]>> value)
```

#### Parameters

`value` [Func&lt;String, Int32, CancellationToken, Task&lt;String[]&gt;&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-4)<br>
function to autocomplete

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-completionmaxcount"/>**CompletionMaxCount(Int32)**

The max.items to return from function autocomplete.The value must be greater than or equal to 1

```csharp
IControlAutoComplete CompletionMaxCount(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of max.items

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-completionwaittostart"/>**CompletionWaitToStart(Int32)**

Number of mileseconds to wait before to start function autocomplete
 <br>Default value : 1000. The value must be greater than or equal to 100.

```csharp
IControlAutoComplete CompletionWaitToStart(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of mileseconds

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlAutoComplete Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-default"/>**Default(String)**

Default value when stated.

```csharp
IControlAutoComplete Default(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Value default

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-defaultifempty"/>**DefaultIfEmpty(String)**

Default value when finished value is empty.

```csharp
IControlAutoComplete DefaultIfEmpty(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Finished value default

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-inputtocase"/>**InputToCase(CaseOptions)**

Transform char input using [CaseOptions](./pplus.controls.caseoptions.md).

```csharp
IControlAutoComplete InputToCase(CaseOptions value)
```

#### Parameters

`value` [CaseOptions](./pplus.controls.caseoptions.md)<br>
Transform option

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-maxlength"/>**MaxLength(UInt16)**

MaxLength of input text.The value must be greater than or equal to 1
 <br>Default value is 0 (no limit)

```csharp
IControlAutoComplete MaxLength(ushort value)
```

#### Parameters

`value` [UInt16](https://docs.microsoft.com/en-us/dotnet/api/system.uint16)<br>
Length

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-minimumprefixlength"/>**MinimumPrefixLength(Int32)**

Number minimum of chars to accept autocomplete
 <br>Default value : 3.The value must be greater than or equal to 1

```csharp
IControlAutoComplete MinimumPrefixLength(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of chars

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite default start value with last result saved on history.

```csharp
IControlAutoComplete OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

IControlAutoComplete

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.
 <br>Default value : 10.The value must be greater than or equal to 1

```csharp
IControlAutoComplete PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-spinner"/>**Spinner(SpinnersType, Nullable&lt;Style&gt;, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). Default value is SpinnersType.Ascii
 <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected

```csharp
IControlAutoComplete Spinner(SpinnersType spinnersType, Nullable<Style> SpinnerStyle, Nullable<Int32> speedAnimation, IEnumerable<String> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
The [SpinnersType](./pplus.controls.spinnerstype.md)

`SpinnerStyle` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of spinner. [Style](./pplus.style.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach iteration of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)

### <a id="methods-validateondemand"/>**ValidateOnDemand(Boolean)**

Execute validators foreach input

```csharp
IControlAutoComplete ValidateOnDemand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true execute validators foreach input; otherwise, only at finish.

#### Returns

[IControlAutoComplete](./pplus.controls.icontrolautocomplete.md)


- - -
[**Back to List Api**](./apis.md)
