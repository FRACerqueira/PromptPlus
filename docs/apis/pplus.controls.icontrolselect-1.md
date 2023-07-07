# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus IControlSelect 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlSelect&lt;T&gt;

Namespace: PPlus.Controls

```csharp
public interface IControlSelect<T> : IPromptControls`1
```

#### Type Parameters

`T`<br>

Implements IPromptControls&lt;T&gt;

## Methods

### **Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlSelect&lt;T&gt;, T1&gt;)**

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

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlSelect<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **Default(T)**

Default value seleted.

```csharp
IControlSelect<T> Default(T value)
```

#### Parameters

`value` T<br>
Value default

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite defaults start seleted value with last result saved on history.

```csharp
IControlSelect<T> OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlSelect<T> PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **FilterType(FilterMode)**

Filter strategy for filter items in colletion

<br>

```csharp
IControlSelect<T> FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **OrderBy(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort list by expression

```csharp
IControlSelect<T> OrderBy(Expression<Func<T, object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the colletion

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### **OrderByDescending(Expression&lt;Func&lt;T, Object&gt;&gt;)**

Sort Descending list by expression

```csharp
IControlSelect<T> OrderByDescending(Expression<Func<T, object>> value)
```

#### Parameters

`value` Expression&lt;Func&lt;T, Object&gt;&gt;<br>
expresion to sort the colletion

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### **TextSelector(Func&lt;T, String&gt;)**

Function to show text Item in list.Default value is Item.ToString()

```csharp
IControlSelect<T> TextSelector(Func<T, string> value)
```

#### Parameters

`value` Func&lt;T, String&gt;<br>
Function to show text Item in list

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **ChangeDescription(Func&lt;T, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlSelect<T> ChangeDescription(Func<T, string> value)
```

#### Parameters

`value` Func&lt;T, String&gt;<br>
function to apply change

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **AddItem(T, Boolean)**

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

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **AddItems(IEnumerable&lt;T&gt;, Boolean)**

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

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **AddItemTo(AdderScope, T)**

Add item to scope Disable/Remove [AdderScope](./pplus.controls.adderscope.md)

<br><br>

```csharp
IControlSelect<T> AddItemTo(AdderScope scope, T value)
```

#### Parameters

`scope` [AdderScope](./pplus.controls.adderscope.md)<br>
scope Disable/Remove

`value` T<br>
item

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **AddItemsTo(AdderScope, IEnumerable&lt;T&gt;)**

Add Items colletion to scope Disable/Remove [AdderScope](./pplus.controls.adderscope.md)

<br><br>

```csharp
IControlSelect<T> AddItemsTo(AdderScope scope, IEnumerable<T> values)
```

#### Parameters

`scope` [AdderScope](./pplus.controls.adderscope.md)<br>
scope Disable/Remove

`values` IEnumerable&lt;T&gt;<br>
items colletion

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **EqualItems(Func&lt;T, T, Boolean&gt;)**

Custom item comparator

```csharp
IControlSelect<T> EqualItems(Func<T, T, bool> comparer)
```

#### Parameters

`comparer` Func&lt;T, T, Boolean&gt;<br>
function comparator

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)

### **AutoSelect()**

Automatically select item when only one item is in the list

```csharp
IControlSelect<T> AutoSelect()
```

#### Returns

[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)<br>
[IControlSelect&lt;T&gt;](./pplus.controls.icontrolselect-1.md)


- - -
[**Back to List Api**](./apis.md)
