# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlSelect<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlSelect&lt;T&gt;

Namespace: PPlus.Controls

Represents the interface with all Methods of the Select control

```csharp
public interface IControlSelect<T> : IPromptControls<T>
```

#### Type Parameters

`T`<br>
typeof return

Implements IPromptControls&lt;T&gt;

## Methods

### <a id="methods-additem"/>**AddItem(T, Boolean)**

Add item to list

```csharp
IControlSelect<T> AddItem(T value, bool disable)
```

#### Parameters

`value` T<br>
Item to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-additemgrouped"/>**AddItemGrouped(String, T, Boolean)**

Add Item in a group to list
 <br>AddItemGrouped cannot be used with OrderBy/OrderByDescending

```csharp
IControlSelect<T> AddItemGrouped(string group, T value, bool disable)
```

#### Parameters

`group` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Group name

`value` T<br>
Item to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-additems"/>**AddItems(IEnumerable&lt;T&gt;, Boolean)**

Add items colletion to list

```csharp
IControlSelect<T> AddItems(IEnumerable<T> values, bool disable)
```

#### Parameters

`values` IEnumerable&lt;T&gt;<br>
items colletion to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-additemsgrouped"/>**AddItemsGrouped(String, IEnumerable&lt;T&gt;, Boolean)**

Add Items colletion in a group to List
 <br>AddItemsGrouped cannot be used with OrderBy/OrderByDescending

```csharp
IControlSelect<T> AddItemsGrouped(string group, IEnumerable<T> value, bool disable)
```

#### Parameters

`group` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Group name

`value` IEnumerable&lt;T&gt;<br>
items colletion to add

`disable` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true item disabled, otherwise no

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-additemsto"/>**AddItemsTo(AdderScope, params T[])**

Add Items colletion to scope Disable/Remove [AdderScope](./pplus.controls.adderscope.md)<br>At startup the list items will be compared and will be removed or disabled <br>Tip: Use  for custom comparer

```csharp
IControlSelect<T> AddItemsTo(AdderScope scope, params T[] values)
```

#### Parameters

`scope` [AdderScope](./pplus.controls.adderscope.md)<br>
scope Disable/Remove

`values` T[]<br>
items colletion

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-additemsto"/>**AddItemsTo(AdderScope, IEnumerable&lt;T&gt;)**

Add Items colletion to scope Disable/Remove [AdderScope](./pplus.controls.adderscope.md)<br>At startup the list items will be compared and will be removed or disabled <br>Tip: Use  for custom comparer

```csharp
IControlSelect<T> AddItemsTo(AdderScope scope, IEnumerable<T> values)
```

#### Parameters

`scope` [AdderScope](./pplus.controls.adderscope.md)<br>
scope Disable/Remove

`values` IEnumerable&lt;T&gt;<br>
items colletion

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-autoselect"/>**AutoSelect(Boolean)**

Automatically select and finalize item when only one item is in the list . Default false.

```csharp
IControlSelect<T> AutoSelect(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Automatically select

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;T, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlSelect<T> ChangeDescription(Func<T, String> value)
```

#### Parameters

`value` Func&lt;T, String&gt;<br>
function to apply change

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlSelect<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-default"/>**Default(T)**

Default value selected.

```csharp
IControlSelect<T> Default(T value)
```

#### Parameters

`value` T<br>
Value default

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-equalitems"/>**EqualItems(Func&lt;T, T, Boolean&gt;)**

Custom item comparator

```csharp
IControlSelect<T> EqualItems(Func<T, T, Boolean> comparer)
```

#### Parameters

`comparer` Func&lt;T, T, Boolean&gt;<br>
function comparator

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-filtertype"/>**FilterType(FilterMode)**

Filter strategy for filter items in colletion
 <br>Default value is FilterMode.Contains

```csharp
IControlSelect<T> FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-interaction"/>**Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlSelect&lt;T&gt;, T1&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlSelect<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlSelect<T>, T1> action)
```

#### Type Parameters

`T1`<br>
Type external colletion

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlSelect&lt;T&gt;, T1&gt;<br>
Action to execute

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-orderby"/>**OrderBy(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort list by expression
 <br>OrderBy cannot be used Separator or Grouped item

```csharp
IControlSelect<T> OrderBy(Expression<Func<T, Object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the colletion

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-orderbydescending"/>**OrderByDescending(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort Descending list by expression
 <br>OrderByDescending cannot be used Separator or Grouped item

```csharp
IControlSelect<T> OrderByDescending(Expression<Func<T, Object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the colletion

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite defaults start selected value with last result saved on history.

```csharp
IControlSelect<T> OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.
 <br>Default value : 10.The value must be greater than or equal to 1

```csharp
IControlSelect<T> PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-separator"/>**Separator(SeparatorLine, Nullable&lt;Char&gt;)**

Add Separation line
 <br>Separatorcannot be used with OrderBy/OrderByDescending

```csharp
IControlSelect<T> Separator(SeparatorLine separatorLine, Nullable<Char> value)
```

#### Parameters

`separatorLine` [SeparatorLine](./pplus.controls.separatorline.md)<br>
Type Separation line.Default value is SeparatorLine.SingleLine [SeparatorLine](./pplus.controls.separatorline.md)

`value` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Char Separation line. Valid only SeparatorLine is SeparatorLine.Char

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-showtipgroup"/>**ShowTipGroup(Boolean)**

Show tip with text of group. Default false

```csharp
IControlSelect<T> ShowTipGroup(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
If True, it shows the tip with the group text, otherwise nothing.

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-styles"/>**Styles(SelectStyles, Style)**

Overwrite Styles

```csharp
IControlSelect<T> Styles(SelectStyles styletype, Style value)
```

#### Parameters

`styletype` [SelectStyles](./pplus.controls.selectstyles.md)<br>
[SelectStyles](./pplus.controls.selectstyles.md) of content

`value` [Style](./pplus.style.md)<br>
The [Style](./pplus.style.md)

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### <a id="methods-textselector"/>**TextSelector(Func&lt;T, String&gt;)**

Function to show text Item in list.Default value is Item.ToString()

```csharp
IControlSelect<T> TextSelector(Func<T, String> value)
```

#### Parameters

`value` Func&lt;T, String&gt;<br>
Function to show text Item in list

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)


- - -
[**Back to List Api**](./apis.md)
