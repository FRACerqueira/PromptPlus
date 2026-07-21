<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ITreeControl\<T\> Interface

Provides a fluent API for configuring and running a generic tree control that browses an
arbitrary hierarchy of items of type [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T') as an expandable/collapsible tree\.

```csharp
public interface ITreeControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeControl_T_.T'></a>

`T`

The type of items in the tree\.

### Remarks
The tree structure is built explicitly by the caller through [Root\(T\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Root(T) 'PromptPlusLibrary\.ITreeControl\<T\>\.Root\(T\)'),
[AddLast\(T\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddLast(T) 'PromptPlusLibrary\.ITreeControl\<T\>\.AddLast\(T\)')/[AddFirst\(T\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddFirst(T) 'PromptPlusLibrary\.ITreeControl\<T\>\.AddFirst\(T\)') \(first\-level nodes\),
[AddAfter\(ITreeNode&lt;T&gt;, T\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T) 'PromptPlusLibrary\.ITreeControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T\)')/[AddBefore\(ITreeNode&lt;T&gt;, T\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T) 'PromptPlusLibrary\.ITreeControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T\)') \(sibling
insertion\) and [AddLast\(T\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddLast(T) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddLast\(T\)')/[AddFirst\(T\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddFirst(T) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddFirst\(T\)')
\(nested children\)\. Whether a node is a container or a leaf is inferred from whether it has
children\. The rendered tree materializes visible rows lazily on expand and releases them on
collapse, keeping memory proportional to what is visible\.
### Methods

<a name='PromptPlusLibrary.ITreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T)'></a>

## ITreeControl\<T\>\.AddAfter\(ITreeNode\<T\>, T\) Method

Inserts a sibling immediately after [node](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.ITreeControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node')\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddAfter(PromptPlusLibrary.ITreeNode<T> node, T value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).node'></a>

`node` [PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).value'></a>

`value` [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [node](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.ITreeControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node') is `null`\.

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When [node](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.ITreeControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node') does not belong to this tree\.

<a name='PromptPlusLibrary.ITreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T)'></a>

## ITreeControl\<T\>\.AddBefore\(ITreeNode\<T\>, T\) Method

Inserts a sibling immediately before [node](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.ITreeControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node')\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddBefore(PromptPlusLibrary.ITreeNode<T> node, T value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).node'></a>

`node` [PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).value'></a>

`value` [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [node](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.ITreeControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node') is `null`\.

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When [node](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.ITreeControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node') does not belong to this tree\.

<a name='PromptPlusLibrary.ITreeControl_T_.AddFirst(T)'></a>

## ITreeControl\<T\>\.AddFirst\(T\) Method

Adds a first\-level node \(child of the root\) at the beginning\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddFirst(T value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.AddFirst(T).value'></a>

`value` [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When the root has not been set yet\.

<a name='PromptPlusLibrary.ITreeControl_T_.AddLast(T)'></a>

## ITreeControl\<T\>\.AddLast\(T\) Method

Adds a first\-level node \(child of the root\) at the end\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddLast(T value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.AddLast(T).value'></a>

`value` [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')  
The newly created node so children can be attached to it\.

#### Exceptions

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When the root has not been set yet\.

<a name='PromptPlusLibrary.ITreeControl_T_.ChangeDescription(System.Func_T,string_)'></a>

## ITreeControl\<T\>\.ChangeDescription\(Func\<T,string\>\) Method

Dynamically updates the prompt description based on the currently selected node\.

```csharp
PromptPlusLibrary.ITreeControl<T> ChangeDescription(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.ChangeDescription(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current item and returns the description\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.ChangeDescription(System.Func_T,string_).value 'PromptPlusLibrary\.ITreeControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ITreeControl\<T\>\.ChangeDescriptionAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [ChangeDescription\(Func&lt;T,string&gt;\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.ChangeDescription(System.Func_T,string_) 'PromptPlusLibrary\.ITreeControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)')\. The task is
awaited synchronously \(blocking\) each frame\.

```csharp
PromptPlusLibrary.ITreeControl<T> ChangeDescriptionAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ITreeControl\<T\>\.ChangeDescriptionAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.Default(T,bool)'></a>

## ITreeControl\<T\>\.Default\(T, bool\) Method

Pre\-selects an item, expanding the tree down to it when reachable from the root\.

```csharp
PromptPlusLibrary.ITreeControl<T> Default(T value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Default(T,bool).value'></a>

`value` [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')

<a name='PromptPlusLibrary.ITreeControl_T_.Default(T,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Default(T,bool).value 'PromptPlusLibrary\.ITreeControl\<T\>\.Default\(T, bool\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.DefaultMatchBy(System.Func_T,T,bool_)'></a>

## ITreeControl\<T\>\.DefaultMatchBy\(Func\<T,T,bool\>\) Method

Sets the item comparator used to locate the default value and the value restored from
history within the tree\. Required\.

```csharp
PromptPlusLibrary.ITreeControl<T> DefaultMatchBy(System.Func<T,T,bool> comparer);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When comparer    is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## ITreeControl\<T\>\.EnabledHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history: the selected value is serialized as JSON and stored, and on the next run
the tree is searched \(using [DefaultMatchBy\(Func&lt;T,T,bool&gt;\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.DefaultMatchBy(System.Func_T,T,bool_) 'PromptPlusLibrary\.ITreeControl\<T\>\.DefaultMatchBy\(System\.Func\<T,T,bool\>\)')\) for an item that equals the restored
value so that it can be pre\-selected\.

```csharp
PromptPlusLibrary.ITreeControl<T> EnabledHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.ITreeControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [filename](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ITreeControl\<T\>\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.ExtraInfo(System.Func_T,string_)'></a>

## ITreeControl\<T\>\.ExtraInfo\(Func\<T,string\>\) Method

Sets an optional extra info selector rendered next to the node text\.

```csharp
PromptPlusLibrary.ITreeControl<T> ExtraInfo(System.Func<T,string?> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [extraInfoNode](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode 'PromptPlusLibrary\.ITreeControl\<T\>\.ExtraInfo\(System\.Func\<T,string\>\)\.extraInfoNode') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## ITreeControl\<T\>\.ExtraInfoAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [ExtraInfo\(Func&lt;T,string&gt;\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.ExtraInfo(System.Func_T,string_) 'PromptPlusLibrary\.ITreeControl\<T\>\.ExtraInfo\(System\.Func\<T,string\>\)')\.
The task is awaited synchronously \(blocking\) once per node, per render frame\.

```csharp
PromptPlusLibrary.ITreeControl<T> ExtraInfoAsync(System.Func<T,System.Threading.Tasks.Task<string?>> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [extraInfoNode](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode 'PromptPlusLibrary\.ITreeControl\<T\>\.ExtraInfoAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.extraInfoNode') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.Filter(PromptPlusLibrary.FilterMode)'></a>

## ITreeControl\<T\>\.Filter\(FilterMode\) Method

Enables interactive filtering\. When the user types a printable character while the tree
is in select mode the control switches to filter mode, flattens the whole tree once and
applies the requested [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') against the node full path \(parent chain
joined by [PathSeparator\(char\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.PathSeparator(char) 'PromptPlusLibrary\.ITreeControl\<T\>\.PathSeparator\(char\)')\)\. Clearing the filter restores the lazy tree
view preserving the previous expand/collapse state\.

```csharp
PromptPlusLibrary.ITreeControl<T> Filter(PromptPlusLibrary.FilterMode value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Filter(PromptPlusLibrary.FilterMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

The [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode') to apply\.

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__)'></a>

## ITreeControl\<T\>\.Interaction\<T1\>\(IEnumerable\<T1\>, Action\<T1,ITreeControl\<T\>\>\) Method

Iterates [items](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).items 'PromptPlusLibrary\.ITreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeControl\<T\>\>\)\.items') and invokes [interactionAction](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).interactionAction 'PromptPlusLibrary\.ITreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeControl\<T\>\>\)\.interactionAction') for each
element, giving the caller a chance to add first\-level nodes \(and further descendants\)
programmatically\. Equivalent to calling [AddLast\(T\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.AddLast(T) 'PromptPlusLibrary\.ITreeControl\<T\>\.AddLast\(T\)') inside the loop\.

```csharp
PromptPlusLibrary.ITreeControl<T> Interaction<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Action<T1,PromptPlusLibrary.ITreeControl<T>> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).T1'></a>

`T1`
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).T1 'PromptPlusLibrary\.ITreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeControl\<T\>\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T1](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).T1 'PromptPlusLibrary\.ITreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeControl\<T\>\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [items](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).items 'PromptPlusLibrary\.ITreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeControl\<T\>\>\)\.items') or [interactionAction](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__).interactionAction 'PromptPlusLibrary\.ITreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeControl\<T\>\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_)'></a>

## ITreeControl\<T\>\.InteractionAsync\<T1\>\(IEnumerable\<T1\>, Func\<T1,ITreeControl\<T\>,Task\>\) Method

Asynchronous counterpart of [Interaction&lt;T1&gt;\(IEnumerable&lt;T1&gt;, Action&lt;T1,ITreeControl&lt;T&gt;&gt;\)](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.ITreeControl_T__) 'PromptPlusLibrary\.ITreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.ITreeControl\<T\>\>\)')\.
The tasks are awaited sequentially \(blocking\) so tree construction remains deterministic\.

```csharp
PromptPlusLibrary.ITreeControl<T> InteractionAsync<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Func<T1,PromptPlusLibrary.ITreeControl<T>,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_).T1'></a>

`T1`
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ITreeControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T1](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.ITreeControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [items](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_).items 'PromptPlusLibrary\.ITreeControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.items') or [interactionAction](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.ITreeControl_T_,System.Threading.Tasks.Task_).interactionAction 'PromptPlusLibrary\.ITreeControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.ITreeControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ITreeControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(prompt, tooltips, abort behavior\)\.

```csharp
PromptPlusLibrary.ITreeControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [options](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ITreeControl\<T\>\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.PageSize(byte)'></a>

## ITreeControl\<T\>\.PageSize\(byte\) Method

Sets the maximum number of visible rows per page \(0 = auto\-fit\)\.

```csharp
PromptPlusLibrary.ITreeControl<T> PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.PathSeparator(char)'></a>

## ITreeControl\<T\>\.PathSeparator\(char\) Method

Sets the character used to compose the full path in the answer line\. Default is `'/'`\.

```csharp
PromptPlusLibrary.ITreeControl<T> PathSeparator(char value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.PathSeparator(char).value'></a>

`value` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## ITreeControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a validation predicate evaluated when the user presses Enter\. When it returns
`false`, the selection is rejected and a generic error is shown\.

```csharp
PromptPlusLibrary.ITreeControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## ITreeControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Sets an asynchronous validation predicate evaluated \(blocking\) when the user presses Enter\.

```csharp
PromptPlusLibrary.ITreeControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread\.

<a name='PromptPlusLibrary.ITreeControl_T_.Root(T)'></a>

## ITreeControl\<T\>\.Root\(T\) Method

Sets the root value shown as the top\-level node\. Required\.

```csharp
PromptPlusLibrary.ITreeControl<T> Root(T value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Root(T).value'></a>

`value` [T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [value](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.Root(T).value 'PromptPlusLibrary\.ITreeControl\<T\>\.Root\(T\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.Run(System.Threading.CancellationToken)'></a>

## ITreeControl\<T\>\.Run\(CancellationToken\) Method

Displays the Tree control and blocks until the user confirms or cancels\.

```csharp
PromptPlusLibrary.ResultPrompt<T?> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.SelectLeafOnly(bool)'></a>

## ITreeControl\<T\>\.SelectLeafOnly\(bool\) Method

When enabled, blocks selection of container nodes \(only leaves can be confirmed\)\.

```csharp
PromptPlusLibrary.ITreeControl<T> SelectLeafOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.SelectLeafOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.ShowFullPath(bool)'></a>

## ITreeControl\<T\>\.ShowFullPath\(bool\) Method

Shows the full path \(parent chain\) instead of only the entry name in the answer\.

```csharp
PromptPlusLibrary.ITreeControl<T> ShowFullPath(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.ShowFullPath(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.Styles(PromptPlusLibrary.TreeStyles,ConsolePlusLibrary.Style)'></a>

## ITreeControl\<T\>\.Styles\(TreeStyles, Style\) Method

Overrides visual styles for a specific region of the Tree control\.

```csharp
PromptPlusLibrary.ITreeControl<T> Styles(PromptPlusLibrary.TreeStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.Styles(PromptPlusLibrary.TreeStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [TreeStyles](TreeStyles.md 'PromptPlusLibrary\.TreeStyles')

<a name='PromptPlusLibrary.ITreeControl_T_.Styles(PromptPlusLibrary.TreeStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

<a name='PromptPlusLibrary.ITreeControl_T_.TextSelector(System.Func_T,string_)'></a>

## ITreeControl\<T\>\.TextSelector\(Func\<T,string\>\) Method

Sets the display text selector\. Required\.

```csharp
PromptPlusLibrary.ITreeControl<T> TextSelector(System.Func<T,string> selector);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.TextSelector(System.Func_T,string_).selector'></a>

`selector` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [selector](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.TextSelector(System.Func_T,string_).selector 'PromptPlusLibrary\.ITreeControl\<T\>\.TextSelector\(System\.Func\<T,string\>\)\.selector') is `null`\.

<a name='PromptPlusLibrary.ITreeControl_T_.ViewOnly(bool)'></a>

## ITreeControl\<T\>\.ViewOnly\(bool\) Method

Configures the control for view\-only mode, where nodes can be navigated but not selected\.

```csharp
PromptPlusLibrary.ITreeControl<T> ViewOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeControl_T_.ViewOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If `true`, enables view\-only mode; otherwise, item selection is enabled\.

#### Returns
[PromptPlusLibrary\.ITreeControl&lt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')[T](ITreeControl_T_.md#PromptPlusLibrary.ITreeControl_T_.T 'PromptPlusLibrary\.ITreeControl\<T\>\.T')[&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>')