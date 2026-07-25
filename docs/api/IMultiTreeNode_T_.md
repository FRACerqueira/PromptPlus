<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMultiTreeNode\<T\> Interface

Represents a node of the tree exposed by [IMultiTreeControl&lt;T&gt;](IMultiTreeControl_T_.md 'PromptPlusLibrary\.IMultiTreeControl\<T\>') while it is
being constructed\. Extends [ITreeNode&lt;T&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>') so that chaining off a node returned
by [AddLast\(T, bool, bool\)](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.AddLast(T,bool,bool) 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.AddLast\(T, bool, bool\)')/[AddFirst\(T, bool, bool\)](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.AddFirst(T,bool,bool) 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.AddFirst\(T, bool, bool\)') keeps access to the MultiTree\-specific
`check` parameter, the same way the base `disable` parameter already works on
[ITreeNode&lt;T&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')\.

```csharp
public interface IMultiTreeNode<T> : PromptPlusLibrary.ITreeNode<T>
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTreeNode_T_.T'></a>

`T`

The user item type held by the node\.

Implements [PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.T 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')
### Methods

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddFirst(T,bool,bool)'></a>

## IMultiTreeNode\<T\>\.AddFirst\(T, bool, bool\) Method

Inserts a child at the beginning of this node's children collection\.

```csharp
PromptPlusLibrary.IMultiTreeNode<T> AddFirst(T value, bool disable=false, bool check=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddFirst(T,bool,bool).value'></a>

`value` [T](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.T 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.T')

The value of the new child\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddFirst(T,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new child cannot be checked\. Default is `false`\.

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddFirst(T,bool,bool).check'></a>

`check` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Same semantics as in [AddLast\(T, bool, bool\)](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.AddLast(T,bool,bool) 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.AddLast\(T, bool, bool\)')\.

#### Returns
[PromptPlusLibrary\.IMultiTreeNode&lt;](IMultiTreeNode_T_.md 'PromptPlusLibrary\.IMultiTreeNode\<T\>')[T](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.T 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.T')[&gt;](IMultiTreeNode_T_.md 'PromptPlusLibrary\.IMultiTreeNode\<T\>')  
The newly created child node\.

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddLast(T,bool,bool)'></a>

## IMultiTreeNode\<T\>\.AddLast\(T, bool, bool\) Method

Appends a child at the end of this node's children collection\.

```csharp
PromptPlusLibrary.IMultiTreeNode<T> AddLast(T value, bool disable=false, bool check=false);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddLast(T,bool,bool).value'></a>

`value` [T](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.T 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.T')

The value of the new child\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddLast(T,bool,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new child cannot be checked\. Default is `false`\.

<a name='PromptPlusLibrary.IMultiTreeNode_T_.AddLast(T,bool,bool).check'></a>

`check` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new child starts pre\-checked\. Additive with
[Default\(IEnumerable&lt;T&gt;, bool\)](IMultiTreeControl_T_.md#PromptPlusLibrary.IMultiTreeControl_T_.Default(System.Collections.Generic.IEnumerable_T_,bool) 'PromptPlusLibrary\.IMultiTreeControl\<T\>\.Default\(System\.Collections\.Generic\.IEnumerable\<T\>, bool\)')
and history — whichever mechanism marks a node checked, it stays checked, neither one
clears the other\. Subject to cascade the same way an interactive check would be \(a
checked container cascades to its descendants when `CascadeCheck` is on\)\. Unlike
`Default`, does not auto\-expand the tree to reveal the node\.

#### Returns
[PromptPlusLibrary\.IMultiTreeNode&lt;](IMultiTreeNode_T_.md 'PromptPlusLibrary\.IMultiTreeNode\<T\>')[T](IMultiTreeNode_T_.md#PromptPlusLibrary.IMultiTreeNode_T_.T 'PromptPlusLibrary\.IMultiTreeNode\<T\>\.T')[&gt;](IMultiTreeNode_T_.md 'PromptPlusLibrary\.IMultiTreeNode\<T\>')  
The newly created child node\.