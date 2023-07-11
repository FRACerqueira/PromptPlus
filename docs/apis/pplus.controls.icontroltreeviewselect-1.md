# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlTreeViewSelect<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlTreeViewSelect&lt;T&gt;

Namespace: PPlus.Controls

```csharp
public interface IControlTreeViewSelect<T> : IPromptControls<T>
```

#### Type Parameters

`T`<br>

Implements IPromptControls&lt;T&gt;

## Methods

### <a id="methods-addnode"/>**AddNode(T)**

Add a node

```csharp
IControlTreeViewSelect<T> AddNode(T value)
```

#### Parameters

`value` T<br>
value node

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-addnode"/>**AddNode(T, T)**

Add a node in parent node

```csharp
IControlTreeViewSelect<T> AddNode(T Parent, T value)
```

#### Parameters

`Parent` T<br>
value parent

`value` T<br>
value node

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-aftercollapsed"/>**AfterCollapsed(Action&lt;T&gt;)**

Action to execute after Collapsed

```csharp
IControlTreeViewSelect<T> AfterCollapsed(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-afterexpanded"/>**AfterExpanded(Action&lt;T&gt;)**

Action to execute after Expanded

```csharp
IControlTreeViewSelect<T> AfterExpanded(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-beforecollapsed"/>**BeforeCollapsed(Action&lt;T&gt;)**

Action to execute before Collapsed

```csharp
IControlTreeViewSelect<T> BeforeCollapsed(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-beforeexpanded"/>**BeforeExpanded(Action&lt;T&gt;)**

Action to execute before Expanded

```csharp
IControlTreeViewSelect<T> BeforeExpanded(Action<T> value)
```

#### Parameters

`value` Action&lt;T&gt;<br>
The action

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlTreeViewSelect<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-default"/>**Default(T)**

Default item node seleted when started

```csharp
IControlTreeViewSelect<T> Default(T value)
```

#### Parameters

`value` T<br>
value node

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-hotkeyfullpath"/>**HotKeyFullPath(HotKey)**

Overwrite a HotKey toggle current name node parent to FullPath. Default value is 'F2'

```csharp
IControlTreeViewSelect<T> HotKeyFullPath(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to toggle current name node to FullPath

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-hotkeytoggleexpand"/>**HotKeyToggleExpand(HotKey)**

Overwrite a HotKey expand/Collap current node selected. Default value is 'F3'

```csharp
IControlTreeViewSelect<T> HotKeyToggleExpand(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collapse current node selected

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-hotkeytoggleexpandall"/>**HotKeyToggleExpandAll(HotKey)**

Overwrite a HotKey expand/Collap all nodes. Default value is 'F4'

```csharp
IControlTreeViewSelect<T> HotKeyToggleExpandAll(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to expand/Collap all nodes

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-interaction"/>**Interaction(IEnumerable&lt;T&gt;, Action&lt;IControlTreeViewSelect&lt;T&gt;, T&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlTreeViewSelect<T> Interaction(IEnumerable<T> values, Action<IControlTreeViewSelect<T>, T> action)
```

#### Parameters

`values` IEnumerable&lt;T&gt;<br>
Colletion for interaction

`action` Action&lt;IControlTreeViewSelect&lt;T&gt;, T&gt;<br>
Action to execute

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlTreeViewSelect<T> PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-rootnode"/>**RootNode(T, Func&lt;T, String&gt;, Boolean, Func&lt;T, Boolean&gt;, Func&lt;T, Boolean&gt;, Nullable&lt;Char&gt;, Func&lt;T, String&gt;)**

Set root node

```csharp
IControlTreeViewSelect<T> RootNode(T value, Func<T, String> textnode, bool expandall, Func<T, Boolean> validselect, Func<T, Boolean> setdisabled, Nullable<Char> separatePath, Func<T, String> uniquenode)
```

#### Parameters

`value` T<br>
value node

`textnode` Func&lt;T, String&gt;<br>
function to show text in node

`expandall` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true expand all nodes, otherwise 'no'

`validselect` Func&lt;T, Boolean&gt;<br>
Select all items that satisfy the selection function

`setdisabled` Func&lt;T, Boolean&gt;<br>
Disabled all items that satisfy the disabled function

`separatePath` [Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Separate path nodes. Default value is '/'

`uniquenode` Func&lt;T, String&gt;<br>
function to return unique identify node

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-showcurrentnode"/>**ShowCurrentNode(Boolean)**

Append name node parent on description

```csharp
IControlTreeViewSelect<T> ShowCurrentNode(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Append current name node parent on description, not append

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-showexpand"/>**ShowExpand(Boolean)**

Show expand SymbolType.Expanded. Default is true

```csharp
IControlTreeViewSelect<T> ShowExpand(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show Expanded SymbolType, otherwise 'no'

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-showlines"/>**ShowLines(Boolean)**

Show lines of level. Default is true

```csharp
IControlTreeViewSelect<T> ShowLines(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true Show lines, otherwise 'no'

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)

### <a id="methods-styles"/>**Styles(StyleTreeView, Style)**

Overwrite Styles treeview. [StyleTreeView](./pplus.controls.styletreeview.md)

```csharp
IControlTreeViewSelect<T> Styles(StyleTreeView styletype, Style value)
```

#### Parameters

`styletype` [StyleTreeView](./pplus.controls.styletreeview.md)<br>
Styles treeview

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlTreeViewSelect&lt;T&gt;](./pplus.controls.icontroltreeviewselect-1.md)


- - -
[**Back to List Api**](./apis.md)
