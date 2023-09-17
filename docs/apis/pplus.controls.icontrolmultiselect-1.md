# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlMultiSelect<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlMultiSelect&lt;T&gt;

Namespace: PPlus.Controls

Represents the interface with all Methods of the MultiSelect control

```csharp
public interface IControlMultiSelect<T> : IPromptControls<IEnumerable<T>>
```

#### Type Parameters

`T`<br>
typeof return

Implements IPromptControls&lt;IEnumerable&lt;T&gt;&gt;

## Methods

### <a id="methods-adddefault"/>**AddDefault(params T[])**

Add default value selected to initial list.

```csharp
IControlMultiSelect<T> AddDefault(params T[] values)
```

#### Parameters

`values` T[]<br>
Value default

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-additem"/>**AddItem(T, Boolean, Boolean)**

Add item to list

```csharp
IControlMultiSelect<T> AddItem(T value, bool disable, bool selected)
```

#### Parameters

`value` T<br>
Item to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

`selected` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item selected, otherwise no

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-additemgrouped"/>**AddItemGrouped(String, T, Boolean, Boolean)**

Add Item in a group to list

```csharp
IControlMultiSelect<T> AddItemGrouped(string group, T value, bool disable, bool selected)
```

#### Parameters

`group` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Group name

`value` T<br>
Item to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

`selected` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item selected, otherwise no

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-additems"/>**AddItems(IEnumerable&lt;T&gt;, Boolean, Boolean)**

Add items colletion to list

```csharp
IControlMultiSelect<T> AddItems(IEnumerable<T> values, bool disable, bool selected)
```

#### Parameters

`values` IEnumerable&lt;T&gt;<br>
items colletion to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

`selected` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item selected, otherwise no

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-additemsgrouped"/>**AddItemsGrouped(String, IEnumerable&lt;T&gt;, Boolean, Boolean)**

Add Items colletion in a group to List

```csharp
IControlMultiSelect<T> AddItemsGrouped(string group, IEnumerable<T> value, bool disable, bool selected)
```

#### Parameters

`group` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Group name

`value` IEnumerable&lt;T&gt;<br>
items colletion to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

`selected` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item selected, otherwise no

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-additemsto"/>**AddItemsTo(AdderScope, params T[])**

Add Items colletion to scope Disable/Remove [AdderScope](./pplus.controls.adderscope.md)<br>At startup the list items will be compared and will be removed or disabled <br>Tip: Use  for custom comparer

```csharp
IControlMultiSelect<T> AddItemsTo(AdderScope scope, params T[] values)
```

#### Parameters

`scope` [AdderScope](./pplus.controls.adderscope.md)<br>
scope Disable/Remove

`values` T[]<br>
items colletion

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-appendgroupondescription"/>**AppendGroupOnDescription()**

Append group text on description

```csharp
IControlMultiSelect<T> AppendGroupOnDescription()
```

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;T, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlMultiSelect<T> ChangeDescription(Func<T, String> value)
```

#### Parameters

`value` Func&lt;T, String&gt;<br>
function to apply change

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlMultiSelect<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-equalitems"/>**EqualItems(Func&lt;T, T, Boolean&gt;)**

Custom item comparator

```csharp
IControlMultiSelect<T> EqualItems(Func<T, T, Boolean> comparer)
```

#### Parameters

`comparer` Func&lt;T, T, Boolean&gt;<br>
function comparator

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-filtertype"/>**FilterType(FilterMode)**

Filter strategy for filter items in colletion
 <br>Default value is FilterMode.Contains

```csharp
IControlMultiSelect<T> FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-hotkeyinvertselected"/>**HotKeyInvertSelected(HotKey)**

Overwrite a HotKey to Invert Selected item. Default value is 'F3'

```csharp
IControlMultiSelect<T> HotKeyInvertSelected(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to Invert Selected item

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-hotkeyselectall"/>**HotKeySelectAll(HotKey)**

Overwrite a HotKey to Select All item. Default value is 'F2'

```csharp
IControlMultiSelect<T> HotKeySelectAll(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to Select All item

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-interaction"/>**Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlMultiSelect&lt;T&gt;, T1&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlMultiSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlMultiSelect<T>, T1> action)
```

#### Type Parameters

`T1`<br>
Type external colletion

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlMultiSelect&lt;T&gt;, T1&gt;<br>
Action to execute

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-orderby"/>**OrderBy(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort list by expression

```csharp
IControlMultiSelect<T> OrderBy(Expression<Func<T, Object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the colletion

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-orderbydescending"/>**OrderByDescending(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort Descending list by expression

```csharp
IControlMultiSelect<T> OrderByDescending(Expression<Func<T, Object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the colletion

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-overflowanswer"/>**OverflowAnswer(Overflow)**

Overwrite Overflow strategy answer
 <br>Default value is Overflow.Ellipsis

```csharp
IControlMultiSelect<T> OverflowAnswer(Overflow value)
```

#### Parameters

`value` [Overflow](./pplus.overflow.md)<br>
Overflow strategy

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite defaults start selected value with last result saved on history.

```csharp
IControlMultiSelect<T> OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlMultiSelect<T> PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-range"/>**Range(Int32, Nullable&lt;Int32&gt;)**

Defines a minimum and maximum (optional) range of items selected in the list

```csharp
IControlMultiSelect<T> Range(int minvalue, Nullable<Int32> maxvalue)
```

#### Parameters

`minvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Minimum number of items

`maxvalue` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Maximum number of items

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-textselector"/>**TextSelector(Func&lt;T, String&gt;)**

Function to show text Item in list.Default value is Item.ToString()

```csharp
IControlMultiSelect<T> TextSelector(Func<T, String> value)
```

#### Parameters

`value` Func&lt;T, String&gt;<br>
Function to show text Item in list

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)


- - -
[**Back to List Api**](./apis.md)
