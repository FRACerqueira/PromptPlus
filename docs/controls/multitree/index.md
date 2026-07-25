<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTree&lt;T&gt;**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [MultiTree — Methods ?](methods.md)

---

> An expandable/collapsible hierarchy where the user **checks several** nodes with tri-state
> checkboxes and confirms with **Enter**.

`MultiTree<T>` renders an arbitrary hierarchy of items of type `T` as a navigable tree with
checkboxes. Containers show a tri-state box (unchecked / checked / indeterminate) that reflects the
aggregate state of their descendants. You build the structure exactly like [`Tree<T>`](../tree/index.md)
— a required root, first-level nodes, and nested children — then the user checks nodes and confirms.
Checking can cascade to descendants, be limited to leaves, or be constrained by a count range.

> ?? Only need to pick **one** node? Use the [**Tree**](../tree/index.md) control — same tree model,
> single selection.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Building the model, keyboard, cascade check, filtering, leaf-only, validation |
| [Styles](styles.md) | The `MultiTreeStyles` regions and how to recolor them |

---

## When to use it

| Use `MultiTree<T>` when… | Consider instead… |
|---|---|
| The data is hierarchical and the user checks several nodes | — |
| The user picks a single node | [Tree](../tree/index.md) |
| The data is a flat multi-select list | [MultiSelect](../multiselect/index.md) |
| The data is tabular (multiple columns) | [Table](../table/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .MultiTree<string>("Check folders")
    .Root("Company")                       // required
    .TextSelector(n => n)                  // required
    .DefaultMatchBy((a, b) => a == b)      // required
    .Run();

if (!result.IsAborted)
    foreach (var item in result.Content)
        PromptPlus.Console.WriteLine(item);
```

- `MultiTree<string>("Check folders")` creates a tree of strings. The type argument `T` is the node type.
- `Root(...)`, `TextSelector(...)`, and `DefaultMatchBy(...)` are **required** — see
  [Operations ? Building the tree model](operations.md#building-the-tree-model).
- `.Run()` renders the tree and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<T[]>`](../../architecture.md#resultpromptt) whose `Content` is the **array** of
  checked values.

---

## Building the tree

You add nodes explicitly, exactly as in [`Tree<T>`](../tree/index.md). First-level nodes are children
of the root; deeper nodes are added on the node objects returned to you:

```csharp
using PromptPlusLibrary;

var tree = PromptPlus.Controls
    .MultiTree<string>("Check items")
    .Root("Company")
    .TextSelector(n => n)
    .DefaultMatchBy((a, b) => a == b);

var eng     = tree.AddLast("Engineering");   // first-level ? returns ITreeNode<string>
var backend = eng.AddLast("Backend");        // child of Engineering
backend.AddLast("API");                      // leaf
backend.AddLast("Database");                 // leaf
tree.AddLast("Sales");

var result = tree.Run();
```

- [`AddLast`](methods.md#addlast) / [`AddFirst`](methods.md#addfirst) add a first-level node and
  return an [`ITreeNode<T>`](methods.md#itreenodet) so you can attach children.
- Children are added the same way on the node: `node.AddLast(...)` / `node.AddFirst(...)`.
- [`AddAfter`](methods.md#addafter) / [`AddBefore`](methods.md#addbefore) control sibling order.

A node with children renders as a container (with a tri-state checkbox); a node with none is a leaf.

---

## A richer example

```csharp
using PromptPlusLibrary;

var tree = PromptPlus.Controls
    .MultiTree<Node>("Check services", "Space checks; Enter confirms")
    .Root(company)
    .TextSelector(n => n.Name)
    .ExtraInfo(n => n.Info)
    .CascadeCheck(true)                 // checking a container marks its descendants
    .CheckLeafOnly(false)              // containers can be checked too
    .Range(2, 4)                       // require between 2 and 4 checked items
    .Default([api, mobile])            // pre-check two nodes
    .DefaultMatchBy((a, b) => a.Id == b.Id);

BuildTree(tree);
var result = tree.Run();
```

This combines **cascade checking**, a **count range**, and **pre-checked defaults** — see
[Operations](operations.md) for how they behave together.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Build the tree | `Root`, `AddLast`, `AddFirst`, `AddAfter`, `AddBefore` |
| Populate from a source | `Interaction`, `InteractionAsync` |
| Node text & info | `TextSelector`, `ExtraInfo`, `ExtraInfoAsync` |
| Paths & paging | `PathSeparator`, `ShowFullPath`, `PageSize` |
| Filtering | `Filter` |
| Checking behavior | `CheckLeafOnly`, `CascadeCheck`, `RecursiveMarkWithCtrlSpace`, `Range` |
| Check rules | `PredicateChecked`, `PredicateCheckedAsync` |
| Initial values | `Default`, `DefaultMatchBy` |
| Read-only display | `ViewOnly` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`MultiTree<T>` returns `ResultPrompt<T[]>` — the array of checked node values.

| Member | Meaning |
|---|---|
| `.Content` | The `T[]` of checked values (empty array if none) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var (nodes, aborted) = PromptPlus.Controls
    .MultiTree<string>("Folders")
    .Root("Company").TextSelector(n => n).DefaultMatchBy((a, b) => a == b)
    .Run();
if (!aborted) PromptPlus.Console.WriteLine($"{nodes.Length} checked");
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — building the model, keyboard, cascade check, filtering, validation
- [Styles](styles.md) — recolor the tree regions
- [Tree](../tree/index.md) — single-choice sibling
- [MultiSelect](../multiselect/index.md) · [Table](../table/index.md) — flat and tabular pickers
