<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ITreeNode\<T\> Interface

Represents a node of the tree exposed by the [ITreeSelectControl&lt;T&gt;](ITreeSelectControl_T_.md 'PromptPlusLibrary\.ITreeSelectControl\<T\>') when it is being
constructed\. A node carries a user value and can have any number of children added lazily
through [AddLast\(T, bool\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddLast(T,bool) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddLast\(T, bool\)') and [AddFirst\(T, bool\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddFirst(T,bool) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddFirst\(T, bool\)')\.

```csharp
public interface ITreeNode<T>
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeNode_T_.T'></a>

`T`

The user item type held by the node\.

Derived  
↳ [ITreeMultiSelectNode&lt;T&gt;](ITreeMultiSelectNode_T_.md 'PromptPlusLibrary\.ITreeMultiSelectNode\<T\>')
### Properties

<a name='PromptPlusLibrary.ITreeNode_T_.Disabled'></a>

## ITreeNode\<T\>\.Disabled Property

Whether this node can be confirmed\. Disabled nodes are still shown and can
            still be navigated to and expanded/collapsed; only confirming them \(`Enter`\) is
            blocked, and view\-only mode ignores this entirely, same as `PredicateSelected`\.

```csharp
bool Disabled { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.ITreeNode_T_.Parent'></a>

## ITreeNode\<T\>\.Parent Property

The parent node, or `null` when this node is the root of the tree\.

```csharp
PromptPlusLibrary.ITreeNode<T>? Parent { get; }
```

#### Property Value
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')

<a name='PromptPlusLibrary.ITreeNode_T_.Value'></a>

## ITreeNode\<T\>\.Value Property

The user value associated with this node\.

```csharp
T Value { get; }
```

#### Property Value
[T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')
### Methods

<a name='PromptPlusLibrary.ITreeNode_T_.AddFirst(T,bool)'></a>

## ITreeNode\<T\>\.AddFirst\(T, bool\) Method

Inserts a child at the beginning of this node's children collection\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddFirst(T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeNode_T_.AddFirst(T,bool).value'></a>

`value` [T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')

The value of the new child\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeNode_T_.AddFirst(T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new child cannot be confirmed\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')  
The newly created child node\.

<a name='PromptPlusLibrary.ITreeNode_T_.AddLast(T,bool)'></a>

## ITreeNode\<T\>\.AddLast\(T, bool\) Method

Appends a child at the end of this node's children collection\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddLast(T value, bool disable=false);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeNode_T_.AddLast(T,bool).value'></a>

`value` [T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')

The value of the new child\. Cannot be `null`\.

<a name='PromptPlusLibrary.ITreeNode_T_.AddLast(T,bool).disable'></a>

`disable` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true`, the new child cannot be confirmed\. Default is `false`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')  
The newly created child node\.