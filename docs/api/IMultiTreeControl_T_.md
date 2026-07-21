<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMultiTreeControl\<T\> Interface

Provides a fluent API for configuring and running a generic multi\-selection tree control
that browses an arbitrary hierarchy of items of type [T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T') as an
expandable/collapsible tree with tri\-state checkboxes \(unchecked / checked / indeterminate\)\.

```csharp
public interface IMultiTreeControl<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.T'></a>

`T`

The type of items in the tree\.

### Remarks
The tree structure is built the same way as [ITreeControl&lt;T&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>'): via
[Root\(T\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Root(T) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Root\(T\)'), [AddLast\(T\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddLast(T) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddLast\(T\)')/[AddFirst\(T\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddFirst(T) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddFirst\(T\)'),
[AddAfter\(ITreeNode&lt;T&gt;, T\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T\)')/[AddBefore\(ITreeNode&lt;T&gt;, T\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T\)') and the [ITreeNode&lt;T&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>') children
helpers\. Container nodes display a tri\-state checkbox that reflects the aggregate check
state of their descendants\. Pressing the check key \(Space\) on a container cycles through
Unchecked → Checked \(all descendants\) → Unchecked\. Pressing Enter confirms the selection
and returns all checked leaf \(or all checked\) values\.
### Methods

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T)'></a>

## IMultiTreeControl\<T\>\.AddAfter\(ITreeNode\<T\>, T\) Method

Inserts a new sibling immediately after [node](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node')\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddAfter(PromptPlusLibrary.ITreeNode<T> node, T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).node'></a>

`node` [PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).value'></a>

`value` [T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
Thrown if [node](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddAfter(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddAfter\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node') does not belong to this tree or is the root\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T)'></a>

## IMultiTreeControl\<T\>\.AddBefore\(ITreeNode\<T\>, T\) Method

Inserts a new sibling immediately before [node](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node')\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddBefore(PromptPlusLibrary.ITreeNode<T> node, T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).node'></a>

`node` [PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).value'></a>

`value` [T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

#### Exceptions

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
Thrown if [node](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.AddBefore(PromptPlusLibrary.ITreeNode_T_,T).node 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.AddBefore\(PromptPlusLibrary\.ITreeNode\<T\>, T\)\.node') does not belong to this tree or is the root\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddFirst(T)'></a>

## IMultiTreeControl\<T\>\.AddFirst\(T\) Method

Adds a new node as the first child of the root and returns it\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddFirst(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddFirst(T).value'></a>

`value` [T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddLast(T)'></a>

## IMultiTreeControl\<T\>\.AddLast\(T\) Method

Adds a new node as the last child of the root and returns it so children can be
appended to it\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddLast(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.AddLast(T).value'></a>

`value` [T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.CascadeCheck(bool)'></a>

## IMultiTreeControl\<T\>\.CascadeCheck\(bool\) Method

When `true` \(default\), checking/unchecking a container propagates the new state
to all its descendants\. When `false`, only the container itself is toggled\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> CascadeCheck(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.CascadeCheck(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ChangeDescription(System.Func_T,string_)'></a>

## IMultiTreeControl\<T\>\.ChangeDescription\(Func\<T,string\>\) Method

Dynamically updates the description area based on the node currently under the cursor\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> ChangeDescription(System.Func<T,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ChangeDescription(System.Func_T,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## IMultiTreeControl\<T\>\.ChangeDescriptionAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous variant of [ChangeDescription\(Func&lt;T,string&gt;\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.ChangeDescription(System.Func_T,string_) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.ChangeDescription\(System\.Func\<T,string\>\)')\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> ChangeDescriptionAsync(System.Func<T,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ChangeDescriptionAsync(System.Func_T,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.CheckLeafOnly(bool)'></a>

## IMultiTreeControl\<T\>\.CheckLeafOnly\(bool\) Method

When `true`, only leaf nodes \(nodes without children\) can be checked\.
Checking a container is blocked\. Default is `false`\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> CheckLeafOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.CheckLeafOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool)'></a>

## IMultiTreeControl\<T\>\.Default\(IEnumerable\<T\>, bool\) Method

Pre\-checks one or more items\. The tree auto\-expands to each pre\-checked node\.
When [useDefaultHistory](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).useDefaultHistory 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.useDefaultHistory') is `true` and history is enabled,
the history values override [values](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).values 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)\.values')\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> Default(System.Collections.Generic.IEnumerable<T> values, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.DefaultMatchBy(System.Func_T,T,bool_)'></a>

## IMultiTreeControl\<T\>\.DefaultMatchBy\(Func\<T,T,bool\>\) Method

Sets the equality comparer used to match items \(e\.g\. for [Default\(IEnumerable&lt;T&gt;, bool\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)') lookup\)\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> DefaultMatchBy(System.Func<T,T,bool> comparer);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.DefaultMatchBy(System.Func_T,T,bool_).comparer'></a>

`comparer` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## IMultiTreeControl\<T\>\.EnabledHistory\(string, Action\<IHistoryOptions\>\) Method

Enables history persistence\. Previously checked items are restored on next run\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> EnabledHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ExtraInfo(System.Func_T,string_)'></a>

## IMultiTreeControl\<T\>\.ExtraInfo\(Func\<T,string\>\) Method

Sets a function that returns optional extra information rendered next to each node label\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> ExtraInfo(System.Func<T,string?> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [extraInfoNode](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.ExtraInfo(System.Func_T,string_).extraInfoNode 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.ExtraInfo\(System\.Func\<T,string\>\)\.extraInfoNode') is `null`\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__)'></a>

## IMultiTreeControl\<T\>\.ExtraInfoAsync\(Func\<T,Task\<string\>\>\) Method

Asynchronous counterpart of [ExtraInfo\(Func&lt;T,string&gt;\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.ExtraInfo(System.Func_T,string_) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.ExtraInfo\(System\.Func\<T,string\>\)')\.
The task is awaited synchronously \(blocking\) once per node, per render frame\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> ExtraInfoAsync(System.Func<T,System.Threading.Tasks.Task<string?>> extraInfoNode);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode'></a>

`extraInfoNode` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [extraInfoNode](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.ExtraInfoAsync(System.Func_T,System.Threading.Tasks.Task_string__).extraInfoNode 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.ExtraInfoAsync\(System\.Func\<T,System\.Threading\.Tasks\.Task\<string\>\>\)\.extraInfoNode') is `null`\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Filter(PromptPlusLibrary.FilterMode)'></a>

## IMultiTreeControl\<T\>\.Filter\(FilterMode\) Method

Sets the filter strategy for the filter mode\. Default is [Disabled](FilterMode.md#PromptPlusLibrary.FilterMode.Disabled 'PromptPlusLibrary\.FilterMode\.Disabled')\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> Filter(PromptPlusLibrary.FilterMode value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Filter(PromptPlusLibrary.FilterMode).value'></a>

`value` [FilterMode](FilterMode.md 'PromptPlusLibrary\.FilterMode')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__)'></a>

## IMultiTreeControl\<T\>\.Interaction\<T1\>\(IEnumerable\<T1\>, Action\<T1,IMultiTreeControl\<T\>\>\) Method

Iterates over [items](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__).items 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTreeControl\<T\>\>\)\.items') and invokes [interactionAction](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__).interactionAction 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTreeControl\<T\>\>\)\.interactionAction')
for each element, allowing bulk population of the tree\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> Interaction<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Action<T1,PromptPlusLibrary.IMultiTreeControl<T>> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__).T1'></a>

`T1`
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__).T1 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTreeControl\<T\>\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T1](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__).T1 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTreeControl\<T\>\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTreeControl_T_,System.Threading.Tasks.Task_)'></a>

## IMultiTreeControl\<T\>\.InteractionAsync\<T1\>\(IEnumerable\<T1\>, Func\<T1,IMultiTreeControl\<T\>,Task\>\) Method

Asynchronous variant of [Interaction&lt;T1&gt;\(IEnumerable&lt;T1&gt;, Action&lt;T1,IMultiTreeControl&lt;T&gt;&gt;\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Interaction_T1_(System.Collections.Generic.IEnumerable_T1_,System.Action_T1,PromptPlusLibrary.IMultiTreeControl_T__) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Interaction\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Action\<T1,PromptPlusLibrary\.IMultiTreeControl\<T\>\>\)')\. Each callback is awaited
synchronously so the tree is fully populated before `Run` is called\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> InteractionAsync<T1>(System.Collections.Generic.IEnumerable<T1> items, System.Func<T1,PromptPlusLibrary.IMultiTreeControl<T>,System.Threading.Tasks.Task> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTreeControl_T_,System.Threading.Tasks.Task_).T1'></a>

`T1`
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTreeControl_T_,System.Threading.Tasks.Task_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T1](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTreeControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiTreeControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTreeControl_T_,System.Threading.Tasks.Task_).interactionAction'></a>

`interactionAction` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[T1](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.InteractionAsync_T1_(System.Collections.Generic.IEnumerable_T1_,System.Func_T1,PromptPlusLibrary.IMultiTreeControl_T_,System.Threading.Tasks.Task_).T1 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.InteractionAsync\<T1\>\(System\.Collections\.Generic\.IEnumerable\<T1\>, System\.Func\<T1,PromptPlusLibrary\.IMultiTreeControl\<T\>,System\.Threading\.Tasks\.Task\>\)\.T1')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMultiTreeControl\<T\>\.Options\(Action\<IControlOptions\>\) Method

Applies custom options to the control\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PageSize(byte)'></a>

## IMultiTreeControl\<T\>\.PageSize\(byte\) Method

Sets the maximum number of visible rows per page\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PathSeparator(char)'></a>

## IMultiTreeControl\<T\>\.PathSeparator\(char\) Method

Sets the path separator character used when showing full paths\. Default is `'/'`\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> PathSeparator(char value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PathSeparator(char).value'></a>

`value` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PredicateSelected(System.Func_T,bool_)'></a>

## IMultiTreeControl\<T\>\.PredicateSelected\(Func\<T,bool\>\) Method

Sets a predicate that decides whether a node can be checked\.
Nodes that fail the predicate show an error when the user tries to check them\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> PredicateSelected(System.Func<T,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PredicateSelected(System.Func_T,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__)'></a>

## IMultiTreeControl\<T\>\.PredicateSelectedAsync\(Func\<T,Task\<bool\>\>\) Method

Asynchronous variant of [PredicateSelected\(Func&lt;T,bool&gt;\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.PredicateSelected(System.Func_T,bool_) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.PredicateSelected\(System\.Func\<T,bool\>\)')\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> PredicateSelectedAsync(System.Func<T,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.PredicateSelectedAsync(System.Func_T,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Range(int,System.Nullable_int_)'></a>

## IMultiTreeControl\<T\>\.Range\(int, Nullable\<int\>\) Method

Defines the valid range for the number of checked items\.
Confirmation is blocked until the count falls within `[minvalue, maxvalue]`\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> Range(int minvalue, System.Nullable<int> maxvalue=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Range(int,System.Nullable_int_).minvalue'></a>

`minvalue` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Minimum number of checked items \(≥ 0\)\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Range(int,System.Nullable_int_).maxvalue'></a>

`maxvalue` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional maximum\. When `null` there is no upper bound\.

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.RecursiveMarkWithCtrlSpace(bool)'></a>

## IMultiTreeControl\<T\>\.RecursiveMarkWithCtrlSpace\(bool\) Method

Enables using `Ctrl+Space` for the recursive container selection \(check/uncheck
the container and all descendants\)\. When enabled, plain `Space` only toggles the
checked state of the selected node itself, and the recursive action is moved to
`Ctrl+Space`\. When disabled \(default\), plain `Space` performs the recursive
selection on containers \(if [CascadeCheck\(bool\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.CascadeCheck(bool) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.CascadeCheck\(bool\)') is `true`\)\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> RecursiveMarkWithCtrlSpace(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.RecursiveMarkWithCtrlSpace(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to use `Ctrl+Space` for recursive marking; otherwise, `false`\.

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')  
The same [IMultiTreeControl&lt;T&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Root(T)'></a>

## IMultiTreeControl\<T\>\.Root\(T\) Method

Sets the root value of the tree\. Must be called before adding any children\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> Root(T value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Root(T).value'></a>

`value` [T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Root(T).value 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Root\(T\)\.value') is `null`\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Run(System.Threading.CancellationToken)'></a>

## IMultiTreeControl\<T\>\.Run\(CancellationToken\) Method

Runs the MultiTree control and returns the result\.

```csharp
PromptPlusLibrary.ResultPrompt<T[]> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Cancellation token\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') whose `Content` is the array of checked values,
or an aborted result if the user cancelled\.

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ShowFullPath(bool)'></a>

## IMultiTreeControl\<T\>\.ShowFullPath\(bool\) Method

When `true`, the answer line shows the full ancestor path for each checked item
instead of just its own name\. Default is `false`\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> ShowFullPath(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ShowFullPath(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Styles(PromptPlusLibrary.MultiTreeStyles,ConsolePlusLibrary.Style)'></a>

## IMultiTreeControl\<T\>\.Styles\(MultiTreeStyles, Style\) Method

Overrides a style region for the MultiTree control\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> Styles(PromptPlusLibrary.MultiTreeStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Styles(PromptPlusLibrary.MultiTreeStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MultiTreeStyles](MultiTreeStyles.md 'PromptPlusLibrary\.MultiTreeStyles')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.Styles(PromptPlusLibrary.MultiTreeStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.TextSelector(System.Func_T,string_)'></a>

## IMultiTreeControl\<T\>\.TextSelector\(Func\<T,string\>\) Method

Sets the function used to obtain the display text for each node\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> TextSelector(System.Func<T,string> selector);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.TextSelector(System.Func_T,string_).selector'></a>

`selector` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ViewOnly(bool)'></a>

## IMultiTreeControl\<T\>\.ViewOnly\(bool\) Method

Puts the control into view\-only mode\. The user can navigate and expand/collapse the
tree but cannot check items\. Enter returns the pre\-checked defaults\.

```csharp
PromptPlusLibrary.IMultiTreeControl<T> ViewOnly(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeControl_T_.ViewOnly(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[PromptPlusLibrary\.IMultiTreeControl&lt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')[T](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.T 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.T')[&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>')