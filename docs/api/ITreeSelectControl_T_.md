<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ITreeSelectControl\<T\> Interface

Provides a fluent API for configuring and running the TreeSelect control, which browses an
arbitrary hierarchy of items of type [T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T') as an expandable/collapsible tree\.

```csharp
public interface ITreeSelectControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.T'></a>

`T`

The type of items in the tree\.

### Remarks
The tree structure is built explicitly by the caller through [Root\(T, bool\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Root(T,bool) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Root\(T, bool\)'),
[AddLast\(T, bool\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddLast(T,bool) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddLast\(T, bool\)')/[AddFirst\(T, bool\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddFirst(T,bool) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddFirst\(T, bool\)') \(first\-level nodes\),
[AddAfter\(ITreeNode&lt;T&gt;, T, bool\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)')/[AddBefore\(ITreeNode&lt;T&gt;, T, bool\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)')
\(sibling insertion\) and [AddLast\(T, bool\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddLast(T,bool) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddLast\(T, bool\)')/
[AddFirst\(T, bool\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddFirst(T,bool) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddFirst\(T, bool\)') \(nested children\)\. Whether a node is a
container or a leaf is inferred from whether it has children\. The rendered tree
materializes visible rows lazily on expand and releases them on collapse, keeping memory
proportional to what is visible\. Nodes can be marked `disable` at creation time so
they are shown and navigable but cannot be confirmed\.
### Methods

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool)'></a>

## ITreeSelectControl\<T\>\.AddAfter\(ITreeNode\<T\>, T, bool\) Method

Inserts a sibling immediately after [node](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool).node 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)\.node')\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddAfter(PromptPlusLibrary.ITreeNode<T> node, T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool).node'></a>

`node` [PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

The reference sibling\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool).value'></a>

`value` [T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')

The value of the new node\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new node cannot be confirmed\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [node](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool).node 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)\.node') is `null`\.

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When [node](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T,bool).node 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)\.node') does not belong to this tree\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool)'></a>

## ITreeSelectControl\<T\>\.AddBefore\(ITreeNode\<T\>, T, bool\) Method

Inserts a sibling immediately before [node](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool).node 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)\.node')\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddBefore(PromptPlusLibrary.ITreeNode<T> node, T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool).node'></a>

`node` [PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

The reference sibling\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool).value'></a>

`value` [T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')

The value of the new node\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new node cannot be confirmed\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [node](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool).node 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)\.node') is `null`\.

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When [node](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T,bool).node 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T, bool\)\.node') does not belong to this tree\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddFirst(T,bool)'></a>

## ITreeSelectControl\<T\>\.AddFirst\(T, bool\) Method

Adds a first\-level node \(child of the root\) at the beginning\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddFirst(T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddFirst(T,bool).value'></a>

`value` [T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')

The value of the new node\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddFirst(T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new node cannot be confirmed\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When the root has not been set yet\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddLast(T,bool)'></a>

## ITreeSelectControl\<T\>\.AddLast\(T, bool\) Method

Adds a first\-level node \(child of the root\) at the end\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddLast(T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddLast(T,bool).value'></a>

`value` [T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')

The value of the new node\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.AddLast(T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new node cannot be confirmed\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')  
The newly created node so children can be attached to it\.

#### Exceptions

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When the root has not been set yet\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ChangeDescription(System.Func_T,string_)'></a>

## ITreeSelectControl\<T\>\.ChangeDescription\(Func\<T,string\>\) Method

Dynamically updates the prompt description based on the currently selected node\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> ChangeDescription(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ChangeDescription(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current item and returns the description\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.ChangeDescription(System.Func_T,string_).value 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ITreeSelectControl\<T\>\.ChangeDescriptionAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [ChangeDescription\(Func&lt;T,string&gt;\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.ChangeDescription(System.Func_T,string_) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)')\. The task is
awaited synchronously \(blocking\) each frame\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> ChangeDescriptionAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.ChangeDescriptionAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Default(T,bool)'></a>

## ITreeSelectControl\<T\>\.Default\(T, bool\) Method

Pre\-selects an item, expanding the tree down to it when reachable from the root\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> Default(T value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Default(T,bool).value'></a>

`value` [T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Default(T,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Default(T,bool).value 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Default\(T, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_)'></a>

## ITreeSelectControl\<T\>\.DefaultMatchBy\(Func\<T,T,bool\>\) Method

Sets the item comparator used to locate the default value and the value restored from
history within the tree\. Required\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> DefaultMatchBy(System.Func<T,T,bool> comparer);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When comparer    is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## ITreeSelectControl\<T\>\.EnableHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history: the selected value is serialized as JSON and stored, and on the next run
the tree is searched \(using [DefaultMatchBy\(Func&lt;T,T,bool&gt;\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.DefaultMatchBy(System.Func_T,T,bool_) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.DefaultMatchBy\(System\.Func\<T,T,bool\>\)')\) for an item that equals the restored
value so that it can be pre\-selected\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> EnableHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [filename](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.EnableHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.EnableHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ExtraInfo(System.Func_T,string_)'></a>

## ITreeSelectControl\<T\>\.ExtraInfo\(Func\<T,string\>\) Method

Sets an optional extra info selector rendered next to the node text\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> ExtraInfo(System.Func<T,string?> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [extraInfoNode](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.ExtraInfo\(System\.Func\<T,string\>\)\.extraInfoNode') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ITreeSelectControl\<T\>\.ExtraInfoAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [ExtraInfo\(Func&lt;T,string&gt;\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.ExtraInfo(System.Func_T,string_) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.ExtraInfo\(System\.Func\<T,string\>\)')\.
The task is awaited synchronously \(blocking\) once per node, per render frame\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> ExtraInfoAsync(System.Func<T,System.Threading.Tasks.Task<string?>> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [extraInfoNode](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.ExtraInfoAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.extraInfoNode') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Filter(PromptPlusLibrary.FilterMode)'></a>

## ITreeSelectControl\<T\>\.Filter\(FilterMode\) Method

Enables interactive filtering\. When the user types a printable character while the tree
is in select mode the control switches to filter mode, flattens the whole tree once and
applies the requested [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') against the node full path \(parent chain
joined by [PathSeparator\(char\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.PathSeparator(char) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.PathSeparator\(char\)')\)\. Clearing the filter restores the lazy tree
view preserving the previous expand/collapse state\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> Filter(PromptPlusLibrary.FilterMode value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Filter(PromptPlusLibrary.FilterMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

The [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') to apply\.

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__)'></a>

## ITreeSelectControl\<T\>\.Interaction\<T1\>\(IEnumerable\<T1\>, Action\<T1,ITreeSelectControl\<T\>\>\) Method

Iterates [items](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).items 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>\>\)\.items') and invokes [interactionAction](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).interactionAction 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>\>\)\.interactionAction') for each
element, giving the caller a chance to add first\-level nodes \(and further descendants\)
programmatically\. Equivalent to calling [AddLast\(T, bool\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.AddLast(T,bool) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.AddLast\(T, bool\)') inside the loop\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> Interaction<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Action<T1,PromptPlusLibrary.ITreeSelectControl<T>> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).T1'></a>

`T1`
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).T1 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T1](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).T1 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [items](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).items 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>\>\)\.items') or [interactionAction](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__).interactionAction 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_)'></a>

## ITreeSelectControl\<T\>\.InteractionAsync\<T1\>\(IEnumerable\<T1\>, Func\<T1,ITreeSelectControl\<T\>,Task\>\) Method

Asynchronous counterpart of [Interaction&lt;T1&gt;\(IEnumerable&lt;T1&gt;, Action&lt;T1,ITreeSelectControl&lt;T&gt;&gt;\)](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeSelectControl_T__) 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>\>\)')\.
The tasks are awaited sequentially \(blocking\) so tree construction remains deterministic\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> InteractionAsync<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Func<T1,PromptPlusLibrary.ITreeSelectControl<T>,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_).T1'></a>

`T1`
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T1](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [items](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_).items 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.items') or [interactionAction](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeSelectControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeSelectControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ITreeSelectControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(prompt, tooltips, abort behavior\)\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [options](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PageSize(byte)'></a>

## ITreeSelectControl\<T\>\.PageSize\(byte\) Method

Sets the maximum number of visible rows per page \(0 = auto\-fit\)\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PathSeparator(char)'></a>

## ITreeSelectControl\<T\>\.PathSeparator\(char\) Method

Sets the character used to compose the full path in the answer line\. Default is `'/'`\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> PathSeparator(char value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PathSeparator(char).value'></a>

`value` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## ITreeSelectControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a validation predicate evaluated when the user presses Enter\. When it returns
`false`, the selection is rejected and a generic error is shown\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## ITreeSelectControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate evaluated \(blocking\) when the user presses Enter\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Root(T,bool)'></a>

## ITreeSelectControl\<T\>\.Root\(T, bool\) Method

Sets the root value shown as the top\-level node\. Required\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> Root(T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Root(T,bool).value'></a>

`value` [T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')

The root value\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Root(T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the root cannot be confirmed\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.Root(T,bool).value 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.Root\(T, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Run(System.Threading.CancellationToken)'></a>

## ITreeSelectControl\<T\>\.Run\(CancellationToken\) Method

Displays the TreeSelect control and blocks until the user confirms or cancels\.

```csharp
PromptPlusLibrary.ResultPrompt<T?> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.SelectLeafOnly(bool)'></a>

## ITreeSelectControl\<T\>\.SelectLeafOnly\(bool\) Method

When enabled, blocks selection of container nodes \(only leaves can be confirmed\)\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> SelectLeafOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.SelectLeafOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ShowFullPath(bool)'></a>

## ITreeSelectControl\<T\>\.ShowFullPath\(bool\) Method

Shows the full path \(parent chain\) instead of only the entry name in the answer\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> ShowFullPath(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ShowFullPath(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Styles(PromptPlusLibrary.TreeSelectStyles,ConsolePlusLibrary.Style)'></a>

## ITreeSelectControl\<T\>\.Styles\(TreeSelectStyles, Style\) Method

Overrides visual styles for a specific region of the TreeSelect control\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> Styles(PromptPlusLibrary.TreeSelectStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Styles(PromptPlusLibrary.TreeSelectStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [TreeSelectStyles](TreeSelectStyles.md 'PromptPlusLibrary\.TreeSelectStyles')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.Styles(PromptPlusLibrary.TreeSelectStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

<a name='PromptPlusLibrary.ITreeSelectControl_T_.TextSelector(System.Func_T,string_)'></a>

## ITreeSelectControl\<T\>\.TextSelector\(Func\<T,string\>\) Method

Sets the display text selector\. Required\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> TextSelector(System.Func<T,string> selector);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.TextSelector(System.Func_T,string_).selector'></a>

`selector` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [selector](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.TextSelector(System.Func_T,string_).selector 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.TextSelector\(System\.Func\<T,string\>\)\.selector') is `null`\.

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ViewOnly(bool)'></a>

## ITreeSelectControl\<T\>\.ViewOnly\(bool\) Method

Configures the control for view\-only mode, where nodes can be navigated but not selected\.

```csharp
PromptPlusLibrary.ITreeSelectControl<T> ViewOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeSelectControl_T_.ViewOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, enables view\-only mode; otherwise, item selection is enabled\.

#### Returns
[PromptPlusLibrary\.ITreeSelectControl&lt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')[T](ITreeSelectControl_T_.md#PromptPlusLibrary.ITreeSelectControl_T_.T 'PromptPlusLibrary\.ITreeSelectControl\<T\>\.T')[&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>')