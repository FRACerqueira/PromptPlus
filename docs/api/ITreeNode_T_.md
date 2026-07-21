<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ITreeNode\<T\> Interface

Represents a node of the tree exposed by the [ITreeControl&lt;T&gt;](ITreeControl_T_.md 'PromptPlusLibrary\.ITreeControl\<T\>') when it is being
constructed\. A node carries a user value and can have any number of children added lazily
through [AddLast\(T\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddLast(T) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddLast\(T\)') and [AddFirst\(T\)](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.AddFirst(T) 'PromptPlusLibrary\.ITreeNode\<T\>\.AddFirst\(T\)')\.

```csharp
public interface ITreeNode<T>
```
#### Type parameters

<a name='PromptPlusLibrary.ITreeNode_T_.T'></a>

`T`

The user item type held by the node\.
### Properties

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

<a name='PromptPlusLibrary.ITreeNode_T_.AddFirst(T)'></a>

## ITreeNode\<T\>\.AddFirst\(T\) Method

Inserts a child at the beginning of this node's children collection\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddFirst(T value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeNode_T_.AddFirst(T).value'></a>

`value` [T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')

The value of the new child\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')  
The newly created child node\.

<a name='PromptPlusLibrary.ITreeNode_T_.AddLast(T)'></a>

## ITreeNode\<T\>\.AddLast\(T\) Method

Appends a child at the end of this node's children collection\.

```csharp
PromptPlusLibrary.ITreeNode<T> AddLast(T value);
```
#### Parameters

<a name='PromptPlusLibrary.ITreeNode_T_.AddLast(T).value'></a>

`value` [T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')

The value of the new child\. Cannot be `null`\.

#### Returns
[PromptPlusLibrary\.ITreeNode&lt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')[T](ITreeNode_T_.md#PromptPlusLibrary.ITreeNode_T_.T 'PromptPlusLibrary\.ITreeNode\<T\>\.T')[&gt;](ITreeNode_T_.md 'PromptPlusLibrary\.ITreeNode\<T\>')  
The newly created child node\.