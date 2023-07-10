# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlTreeViewMultiSelect<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlTreeViewMultiSelect&lt;T&gt;

Namespace: PPlus.Controls

```csharp
public interface IControlTreeViewMultiSelect<T> : IPromptControls<T[]>
```

#### Type Parameters

`T`<br>

Implements IPromptControls&lt;T[]&gt;

## Methods

### **Interaction(IEnumerable&lt;T&gt;, Action&lt;IControlTreeViewMultiSelect&lt;T&gt;, T&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlTreeViewMultiSelect<T> Interaction(IEnumerable<T> values, Action<IControlTreeViewMultiSelect<T>, T> action)
```

#### Parameters

`values` IEnumerable&lt;T&gt;<br>
Colletion for interaction

`action` Action&lt;IControlTreeViewMultiSelect&lt;T&gt;, T&gt;<br>
Action to execute

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **Range(Int32, Nullable&lt;Int32&gt;)**

Defines a minimum and maximum (optional) range of items selected in the tree

```csharp
IControlTreeViewMultiSelect<T> Range(int minvalue, Nullable<Int32> maxvalue)
```

#### Parameters

`minvalue` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Minimum number of items

`maxvalue` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Maximum number of items

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlTreeViewMultiSelect<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **Styles(StyleTreeView, Style)**

Overwrite Styles treeview. [StyleTreeView](./pplus.controls.styletreeview.md)

```csharp
IControlTreeViewMultiSelect<T> Styles(StyleTreeView styletype, Style value)
```

#### Parameters

`styletype` [StyleTreeView](./pplus.controls.styletreeview.md)<br>
Styles treeview

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **ShowLines(Boolean)**

Show lines of level. Default is true

```csharp
IControlTreeViewMultiSelect<T> ShowLines(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show lines, otherwise 'no'

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **ShowExpand(Boolean)**

Show expand SymbolType.Expanded. Default is true

```csharp
IControlTreeViewMultiSelect<T> ShowExpand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show Expanded SymbolType, otherwise 'no'

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlTreeViewMultiSelect<T> PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **FilterType(FilterMode)**

Filter strategy for filter items in colletion
 <br>Default value is FilterMode.Contains

```csharp
IControlTreeViewMultiSelect<T> FilterType(FilterMode value)
```

#### Parameters

`value` [FilterMode](./pplus.controls.filtermode.md)<br>
Filter Mode

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **ExpandAll()**

Start treeview with all childs Expanded

```csharp
IControlTreeViewMultiSelect<T> ExpandAll()
```

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **AddFixedSelect(T[])**

Fixed select (immutable) items in list

```csharp
IControlTreeViewMultiSelect<T> AddFixedSelect(T[] values)
```

#### Parameters

`values` T[]<br>
list with items selected

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **SelectAll(Func&lt;T, Boolean&gt;)**

Select all items that satisfy the selection function

```csharp
IControlTreeViewMultiSelect<T> SelectAll(Func<T, Boolean> validselect)
```

#### Parameters

`validselect` Func&lt;T, Boolean&gt;<br>
the function

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **RootNode(T, Func&lt;T, String&gt;, Func&lt;T, Boolean&gt;, Func&lt;T, Boolean&gt;, Nullable&lt;Char&gt;, Func&lt;T, String&gt;)**

Set root node

```csharp
IControlTreeViewMultiSelect<T> RootNode(T value, Func<T, String> textnode, Func<T, Boolean> validselect, Func<T, Boolean> setdisabled, Nullable<Char> separatePath, Func<T, String> uniquenode)
```

#### Parameters

`value` T<br>
value node

`textnode` Func&lt;T, String&gt;<br>
function to show text in node

`validselect` Func&lt;T, Boolean&gt;<br>
Select all items that satisfy the selection function

`setdisabled` Func&lt;T, Boolean&gt;<br>
Disabled all items that satisfy the disabled function

`separatePath` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Separate path nodes. Default value is '/'

`uniquenode` Func&lt;T, String&gt;<br>
function to return unique identify node

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **AddNode(T)**

Add a node

```csharp
IControlTreeViewMultiSelect<T> AddNode(T value)
```

#### Parameters

`value` T<br>
value node

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **AddNode(T, T)**

Add a node in parent node

```csharp
IControlTreeViewMultiSelect<T> AddNode(T Parent, T value)
```

#### Parameters

`Parent` T<br>
value parent

`value` T<br>
value node

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **Default(T)**

Default item node seleted when started

```csharp
IControlTreeViewMultiSelect<T> Default(T value)
```

#### Parameters

`value` T<br>
value node

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **ShowCurrentNode(Boolean)**

Append name node parent on description

```csharp
IControlTreeViewMultiSelect<T> ShowCurrentNode(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Append current name node parent on description, not append

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **HotKeyFullPath(HotKey)**

Overwrite a HotKey toggle current name node parent to FullPath. Default value is 'F2'

```csharp
IControlTreeViewMultiSelect<T> HotKeyFullPath(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to toggle current name node to FullPath

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **HotKeyToggleExpand(HotKey)**

Overwrite a HotKey expand/Collap current node selected. Default value is 'F3'

```csharp
IControlTreeViewMultiSelect<T> HotKeyToggleExpand(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collapse current node selected

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **HotKeyToggleExpandAll(HotKey)**

Overwrite a HotKey expand/Collap all nodes. Default value is 'F4'

```csharp
IControlTreeViewMultiSelect<T> HotKeyToggleExpandAll(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collap all nodes

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **AfterExpanded(Action&lt;T&gt;)**

Action to execute after Expanded

```csharp
IControlTreeViewMultiSelect<T> AfterExpanded(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **AfterCollapsed(Action&lt;T&gt;)**

Action to execute after Collapsed

```csharp
IControlTreeViewMultiSelect<T> AfterCollapsed(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **BeforeExpanded(Action&lt;T&gt;)**

Action to execute before Expanded

```csharp
IControlTreeViewMultiSelect<T> BeforeExpanded(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)

### **BeforeCollapsed(Action&lt;T&gt;)**

Action to execute before Collapsed

```csharp
IControlTreeViewMultiSelect<T> BeforeCollapsed(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewMultiSelect&lt;T&gt;](./pplus.controls.icontroltreeviewmultiselect-1.md)


- - -
[**Back to List Api**](./apis.md)
