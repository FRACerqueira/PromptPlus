# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlMaskEditList 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlMaskEditList

Namespace: PPlus.Controls

Represents the interface with all Methods of the AddtoMaskEditList control

```csharp
public interface IControlMaskEditList : IPromptControls<IEnumerable<ResultMasked>>
```

Implements [IPromptControls&lt;IEnumerable&lt;ResultMasked&gt;&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-acceptemptyvalue"/>**AcceptEmptyValue()**

Accept empty value
 <br>Valid only for type not equal MaskedType.Generic, otherwise this set will be ignored.

```csharp
IControlMaskEditList AcceptEmptyValue()
```

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-additem"/>**AddItem(String, Boolean)**

Add item to initial list

```csharp
IControlMaskEditList AddItem(string value, bool immutable)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text item to add

`immutable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true the item cannot be removed; otherwise yes.

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-additems"/>**AddItems(IEnumerable&lt;String&gt;, Boolean)**

Add items colletion to initial list

```csharp
IControlMaskEditList AddItems(IEnumerable<String> values, bool immutable)
```

#### Parameters

`values` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
items colletion to add

`immutable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true the item cannot be removed; otherwise yes.

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-addvalidators"/>**AddValidators(Func&lt;Object, ValidationResult&gt;[])**

Add a validator to accept sucessfull finish of control.
 <br>Tip: see  to validators embeding

```csharp
IControlMaskEditList AddValidators(Func<Object, ValidationResult>[] validators)
```

#### Parameters

`validators` [Func&lt;Object, ValidationResult&gt;[]](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the function validator.

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-allowduplicate"/>**AllowDuplicate()**

Allow duplicate items.Default value for this control is false.

```csharp
IControlMaskEditList AllowDuplicate()
```

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-ammoutpositions"/>**AmmoutPositions(Int32, Int32, Boolean)**

Defines integer lenght, decimal lenght and accept signl.
 <br>Valid only for type MaskedType.Number or Currency, otherwise this set will be ignored.<br>This set is Requeried for these types.

```csharp
IControlMaskEditList AmmoutPositions(int intvalue, int decimalvalue, bool acceptSignal)
```

#### Parameters

`intvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
integer lenght

`decimalvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
decimal lenght

`acceptSignal` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
True accept signal; otherwise, no.

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;String, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlMaskEditList ChangeDescription(Func<String, String> value)
```

#### Parameters

`value` [Func&lt;String, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply change

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlMaskEditList Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-culture"/>**Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to validate input when the type is not generic.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlMaskEditList Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use on validate

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-culture"/>**Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to validate input when the type is not generic.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlMaskEditList Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use on validate

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-default"/>**Default(String)**

Default initial value when when stated.

```csharp
IControlMaskEditList Default(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
initial value

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-descriptionwithinputtype"/>**DescriptionWithInputType(FormatWeek)**

Append to desription the tip of type input.

```csharp
IControlMaskEditList DescriptionWithInputType(FormatWeek week)
```

#### Parameters

`week` [FormatWeek](./pplus.controls.formatweek.md)<br>
show name of week for type date. [FormatWeek](./pplus.controls.formatweek.md)

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-fillzeros"/>**FillZeros()**

Fill zeros mask.
 <br>Not valid for type MaskedType.Generic (this set will be ignored).<br>When used this feature the AcceptEmptyValue feature will be ignored.<br>When MaskedType.Number or MaskedType.Currency this feature is always on.

```csharp
IControlMaskEditList FillZeros()
```

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-formattime"/>**FormatTime(FormatTime)**

Defines time parts input.
 <br>Valid only for type MaskedType.TimeOnly or DateTime, otherwise this set will be ignored.

```csharp
IControlMaskEditList FormatTime(FormatTime value)
```

#### Parameters

`value` [FormatTime](./pplus.controls.formattime.md)<br>
[FormatTime](./pplus.controls.formattime.md)

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-formatyear"/>**FormatYear(FormatYear)**

Defines if year is long or short.
 <br>Valid only for type MaskedType.DateOnly or DateTime, otherwise this set will be ignored.

```csharp
IControlMaskEditList FormatYear(FormatYear value)
```

#### Parameters

`value` [FormatYear](./pplus.controls.formatyear.md)<br>
[FormatYear](./pplus.controls.formatyear.md)

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-hotkeyedititem"/>**HotKeyEditItem(HotKey)**

Overwrite a HotKey to edit item. Default value is 'F2'

```csharp
IControlMaskEditList HotKeyEditItem(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to edit item

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-hotkeyremoveitem"/>**HotKeyRemoveItem(HotKey)**

Overwrite a HotKey to remove item. Default value is 'F3'

```csharp
IControlMaskEditList HotKeyRemoveItem(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to remove item

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-inputtocase"/>**InputToCase(CaseOptions)**

Transform char input using [CaseOptions](./pplus.controls.caseoptions.md).

```csharp
IControlMaskEditList InputToCase(CaseOptions value)
```

#### Parameters

`value` [CaseOptions](./pplus.controls.caseoptions.md)<br>
Transform option

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-interaction"/>**Interaction&lt;T&gt;(IEnumerable&lt;T&gt;, Action&lt;IControlMaskEditList, T&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlMaskEditList Interaction<T>(IEnumerable<T> values, Action<IControlMaskEditList, T> action)
```

#### Type Parameters

`T`<br>
Typeof item

#### Parameters

`values` IEnumerable&lt;T&gt;<br>
Colletion for interaction

`action` Action&lt;IControlMaskEditList, T&gt;<br>
Action to execute

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-mask"/>**Mask(String, Nullable&lt;Char&gt;)**

Defines mask input. Rules for Generic type:
 <br>9 - Only a numeric character<br>L - Only a letter<br>C - OnlyCustom character<br>A - Any character<br>N - OnlyCustom character +  Only a numeric character<br>X - OnlyCustom character +  Only a letter<br>\ - Escape character<br>{ - Initial delimiter for repetition of masks<br>} - Final delimiter for repetition of masks<br>[-Initial delimiter for list of Custom character<br>] - Final delimiter for list of Custom character

```csharp
IControlMaskEditList Mask(string value, Nullable<Char> promptmask)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text of masked when type is Generic. otherwise must be null.

`promptmask` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Prompt mask overwriter. Default value is '■'/'_'

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-mask"/>**Mask(MaskedType, Nullable&lt;Char&gt;)**

Defines type of mask control.

```csharp
IControlMaskEditList Mask(MaskedType maskedType, Nullable<Char> promptmask)
```

#### Parameters

`maskedType` [MaskedType](./pplus.controls.maskedtype.md)<br>
Type masked

`promptmask` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Prompt mask overwriter. Default value is '■'/'_'

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-negativestyle"/>**NegativeStyle(Style)**

Overwrite [Style](./pplus.style.md) to region neggative input.
 <br>Default Foreground : 'StyleControls.Answer'<br>Default Background : same Console Background when setted

```csharp
IControlMaskEditList NegativeStyle(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
Style

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlMaskEditList PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-positivestyle"/>**PositiveStyle(Style)**

Overwrite [Style](./pplus.style.md) to region positive input.
 <br>Default Foreground : 'StyleControls.Answer'<br>Default Background : Same Console Background when setted

```csharp
IControlMaskEditList PositiveStyle(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
Style

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-range"/>**Range(Int32, Nullable&lt;Int32&gt;)**

Defines a minimum and maximum (optional) range of items in the list

```csharp
IControlMaskEditList Range(int minvalue, Nullable<Int32> maxvalue)
```

#### Parameters

`minvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Minimum number of items

`maxvalue` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Maximum number of items

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-suggestionhandler"/>**SuggestionHandler(Func&lt;SugestionInput, SugestionOutput&gt;)**

Add Suggestion Handler feature

```csharp
IControlMaskEditList SuggestionHandler(Func<SugestionInput, SugestionOutput> value)
```

#### Parameters

`value` [Func&lt;SugestionInput, SugestionOutput&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply suggestions. [SugestionInput](./pplus.controls.sugestioninput.md) and [SugestionOutput](./pplus.controls.sugestionoutput.md)

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)

### <a id="methods-typetipstyle"/>**TypeTipStyle(Style)**

Overwrite [Style](./pplus.style.md) to region tip type input.
 <br>Default Foreground : 'ConsoleColor.Yellow'<br>Default Background : same Console Background when setted

```csharp
IControlMaskEditList TypeTipStyle(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
Style

#### Returns

[IControlMaskEditList](./pplus.controls.icontrolmaskeditlist.md)


- - -
[**Back to List Api**](./apis.md)
