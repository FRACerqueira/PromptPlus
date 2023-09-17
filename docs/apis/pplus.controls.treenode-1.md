# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:TreeNode<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# TreeNode&lt;T&gt;

Namespace: PPlus.Controls

Represents the tree node

```csharp
public class TreeNode<T>
```

#### Type Parameters

`T`<br>
type of Node

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [TreeNode&lt;T&gt;](./pplus.controls.treenode-1.md)

## Properties

### <a id="properties-childrens"/>**Childrens**

List of Children's nodes of node

```csharp
public List<TreeNode<T>> Childrens { get; internal set; }
```

#### Property Value

List&lt;TreeNode&lt;T&gt;&gt;<br>

### <a id="properties-isdisabled"/>**IsDisabled**

Node disabled

```csharp
public bool IsDisabled { get; internal set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isexpanded"/>**IsExpanded**

Node expandend

```csharp
public bool IsExpanded { get; internal set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-ishaschild"/>**IsHasChild**

Node has Child

```csharp
public bool IsHasChild { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-ismarked"/>**IsMarked**

Node Marked

```csharp
public bool IsMarked { get; internal set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isroot"/>**IsRoot**

Node Is root. Top Level

```csharp
public bool IsRoot { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isselected"/>**IsSelected**

Node Current.

```csharp
public bool IsSelected { get; internal set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-level"/>**Level**

Node Level

```csharp
public int Level { get; internal set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-nextnode"/>**NextNode**

Next node

```csharp
public TreeNode<T> NextNode { get; internal set; }
```

#### Property Value

[TreeNode&lt;T&gt;](./pplus.controls.treenode-1.md)<br>

### <a id="properties-parent"/>**Parent**

Parent node

```csharp
public TreeNode<T> Parent { get; internal set; }
```

#### Property Value

[TreeNode&lt;T&gt;](./pplus.controls.treenode-1.md)<br>

### <a id="properties-prevnode"/>**PrevNode**

Previus node

```csharp
public TreeNode<T> PrevNode { get; internal set; }
```

#### Property Value

[TreeNode&lt;T&gt;](./pplus.controls.treenode-1.md)<br>

### <a id="properties-text"/>**Text**

Text Node

```csharp
public string Text { get; internal set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-value"/>**Value**

Value of node

```csharp
public T Value { get; internal set; }
```

#### Property Value

T<br>

## Constructors

### <a id="constructors-.ctor"/>**TreeNode()**

Create a tree node

```csharp
public TreeNode()
```


- - -
[**Back to List Api**](./apis.md)
