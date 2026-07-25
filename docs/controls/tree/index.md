<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Tree&lt;T&gt;**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Tree — Methods →](methods.md)

---

> An expandable/collapsible hierarchy where the user browses nodes and picks **one** item, confirming with **Enter**.

`Tree<T>` renders an arbitrary hierarchy of items of type `T` as a navigable tree. You build the
structure explicitly — a required root, first-level nodes, and nested children — and the control
decides at runtime whether each node is a *container* (has children) or a *leaf*. It expands and
collapses branches lazily, filters against the full path, can restrict confirmation to leaves, and
validates the choice before returning it.

> ☑️ Need to check **several** nodes at once? Use the [**MultiTree**](../multitree/index.md)
> control — same tree model, with tri-state checkboxes.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Building the model, keyboard, expand/collapse, filtering, leaf-only, validation |
| [Styles](styles.md) | The `TreeStyles` regions and how to recolor them |

---

## When to use it

| Use `Tree<T>` when… | Consider instead… |
|---|---|
| The data is hierarchical and the user picks one node | — |
| The user may check several nodes | [MultiTree](../multitree/index.md) |
| The data is a flat list | [Select](../select/index.md) |
| The data is tabular (multiple columns) | [Table](../table/index.md) |
| It is a yes/no or single-key answer | [KeyPress / Confirm](../keypress/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Tree<string>("Pick a folder")
    .Root("Company")                       // required
    .TextSelector(n => n)                  // required
    .DefaultMatchBy((a, b) => a == b)      // required
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"You chose {result.Content}");
```

- `Tree<string>("Pick a folder")` creates a tree of strings. The type argument `T` is the node type.
- `Root(...)`, `TextSelector(...)`, and `DefaultMatchBy(...)` are **required** — see
  [Operations → Building the tree model](operations.md#building-the-tree-model).
- `.Run()` renders the tree and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<T?>`](../../architecture.md#resultpromptt) whose `Content` is **nullable**.

---

## Building the tree

You add nodes explicitly. First-level nodes are children of the root; deeper nodes are added on the
node objects returned to you:

```csharp
using PromptPlusLibrary;

var tree = PromptPlus.Controls
    .Tree<string>("Pick an item")
    .Root("Company")
    .TextSelector(n => n)
    .DefaultMatchBy((a, b) => a == b);

var eng     = tree.AddLast("Engineering");   // first-level → returns ITreeNode<string>
var backend = eng.AddLast("Backend");        // child of Engineering
backend.AddLast("API");                      // leaf
backend.AddLast("Database");                 // leaf
tree.AddLast("Sales");
tree.AddLast("HR");

var result = tree.Run();
```

- [`AddLast`](methods.md#addlast) / [`AddFirst`](methods.md#addfirst) add a first-level node and
  return an [`ITreeNode<T>`](methods.md#itreenodet) so you can attach children.
- Children are added the same way on the node: `node.AddLast(...)` / `node.AddFirst(...)`.
- [`AddAfter`](methods.md#addafter) / [`AddBefore`](methods.md#addbefore) insert a sibling relative
  to an existing node — handy for controlling order.

A node with children renders as a container (expandable); a node with none renders as a leaf.

---

## A richer example

```csharp
using PromptPlusLibrary;

var tree = PromptPlus.Controls
    .Tree<Node>("Pick a service", "Type to filter the full path")
    .Root(company)
    .TextSelector(n => n.Name)
    .ExtraInfo(n => n.Info)              // secondary text next to each node
    .PathSeparator('.')                 // Company.Engineering.Backend.API
    .ShowFullPath(true)                 // answer shows the full path
    .SelectLeafOnly()                   // only leaves can be confirmed
    .Filter(FilterMode.Contains)        // live filtering as the user types
    .DefaultMatchBy((a, b) => a.Id == b.Id);

BuildTree(tree);
var result = tree.Run();
```

This combines **extra info**, **full-path display**, **leaf-only** confirmation, and **filtering** —
see [Operations](operations.md) for how they behave together.

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
| Confirmation rules | `SelectLeafOnly`, `PredicateSelected`, `PredicateSelectedAsync` |
| Initial value | `Default`, `DefaultMatchBy` |
| Read-only display | `ViewOnly` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| History | `EnableHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Tree<T>` returns `ResultPrompt<T?>` — the single chosen node value (nullable).

| Member | Meaning |
|---|---|
| `.Content` | The selected `T` (or `null`/`default` if aborted or nothing was confirmed) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var (node, aborted) = PromptPlus.Controls
    .Tree<string>("Folder")
    .Root("Company").TextSelector(n => n).DefaultMatchBy((a, b) => a == b)
    .Run();
if (!aborted) PromptPlus.Console.WriteLine(node);
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — building the model, keyboard, filtering, leaf-only, validation
- [Styles](styles.md) — recolor the tree regions
- [MultiTree](../multitree/index.md) — multiple-choice sibling
- [Select](../select/index.md) · [Table](../table/index.md) — flat and tabular pickers
