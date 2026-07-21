<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Tree&lt;T&gt; — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Tree — Operations →](operations.md)

---

Every fluent method on `ITreeControl<T>`. Each returns the same control instance, so calls chain
in any order — **except** [`AddLast`](#addlast) / [`AddFirst`](#addfirst) / [`AddAfter`](#addafter) /
[`AddBefore`](#addbefore), which return the new [`ITreeNode<T>`](#itreenodet) so you can attach
children to it. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Tree<T>(string prompt = "", string? description = null)`,
> which returns `ITreeControl<T>`.

> **Required before `Run`:** [`Root`](#root), [`TextSelector`](#textselector), and
> [`DefaultMatchBy`](#defaultmatchby).

**Quick jump:**
[Root](#root) ·
[AddLast](#addlast) ·
[AddFirst](#addfirst) ·
[AddAfter](#addafter) ·
[AddBefore](#addbefore) ·
[ITreeNode&lt;T&gt;](#itreenodet) ·
[Interaction](#interaction) ·
[InteractionAsync](#interactionasync) ·
[TextSelector](#textselector) ·
[ExtraInfo](#extrainfo) ·
[ExtraInfoAsync](#extrainfoasync) ·
[PathSeparator](#pathseparator) ·
[ShowFullPath](#showfullpath) ·
[PageSize](#pagesize) ·
[Filter](#filter) ·
[SelectLeafOnly](#selectleafonly) ·
[Default](#default) ·
[DefaultMatchBy](#defaultmatchby) ·
[PredicateSelected](#predicateselected) ·
[PredicateSelectedAsync](#predicateselectedasync) ·
[ViewOnly](#viewonly) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[EnabledHistory](#enabledhistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Building the tree

### `Root`

```csharp
ITreeControl<T> Root(T value)
```

Sets the top-level node shown at the top of the tree. **Required** — call it before adding any
children.

```csharp
PromptPlus.Controls.Tree<string>("Folders")
    .Root("Company")
    .TextSelector(n => n)
    .DefaultMatchBy((a, b) => a == b)
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `AddLast`

```csharp
ITreeNode<T> AddLast(T value)
```

Adds a first-level node (child of the root) at the **end** and returns it so children can be attached.

```csharp
var eng = tree.AddLast("Engineering");
eng.AddLast("Backend");   // nested child
```

> Throws `InvalidOperationException` if the root has not been set yet.

---

### `AddFirst`

```csharp
ITreeNode<T> AddFirst(T value)
```

Adds a first-level node at the **beginning** so it appears at the very top of the child list.

> Throws `InvalidOperationException` if the root has not been set yet.

---

### `AddAfter`

```csharp
ITreeNode<T> AddAfter(ITreeNode<T> node, T value)
```

Inserts a sibling immediately **after** `node` and returns the new node.

```csharp
var eng = tree.AddLast("Engineering");
tree.AddAfter(eng, "Sales");   // sibling right after Engineering
```

> Throws `ArgumentNullException` if `node` is `null`, or `InvalidOperationException` if `node` does
> not belong to this tree.

---

### `AddBefore`

```csharp
ITreeNode<T> AddBefore(ITreeNode<T> node, T value)
```

Inserts a sibling immediately **before** `node` and returns the new node.

```csharp
var sales = tree.AddLast("Sales");
tree.AddBefore(sales, "HR");   // → [HR, Sales]
```

> Throws `ArgumentNullException` if `node` is `null`, or `InvalidOperationException` if `node` does
> not belong to this tree.

---

### `ITreeNode<T>`

The object returned by the `Add*` methods. Use it to read the node and attach children.

```csharp
public interface ITreeNode<T>
{
    T Value { get; }                 // the user value on this node
    ITreeNode<T>? Parent { get; }    // parent node, or null for the root
    ITreeNode<T> AddLast(T value);   // append a child
    ITreeNode<T> AddFirst(T value);  // prepend a child
}
```

```csharp
var backend = eng.AddLast("Backend");
backend.AddLast("API");
backend.AddFirst("Database");   // Database appears before API
```

A node with at least one child renders as a **container**; a node with none renders as a **leaf**.

---

## Populating from a source

### `Interaction`

```csharp
ITreeControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITreeControl<T>> interactionAction)
```

Iterates a source collection and lets you add first-level nodes (and their descendants)
programmatically — equivalent to calling [`AddLast`](#addlast) inside the loop.

```csharp
PromptPlus.Controls.Tree<Node>("Departments")
    .Root(new Node { Id = 0, Name = "Company" })
    .TextSelector(n => n.Name)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Interaction(flatDepts, (dept, ctrl) =>
    {
        var deptNode = ctrl.AddLast(new Node { Name = dept.Dept });
        foreach (var team in dept.Teams)
            deptNode.AddLast(new Node { Name = team });
    })
    .Run();
```

> Throws `ArgumentNullException` if `items` or `interactionAction` is `null`.

---

### `InteractionAsync`

```csharp
ITreeControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITreeControl<T>, Task> interactionAction)
```

Asynchronous version of [`Interaction`](#interaction). Callbacks are awaited **sequentially
(blocking)** so tree construction stays deterministic.

---

## Node text & info

### `TextSelector`

```csharp
ITreeControl<T> TextSelector(Func<T, string> selector)
```

Sets how each node is rendered as text. **Required.**

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company)
    .TextSelector(n => n.Name)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Run();
```

> Throws `ArgumentNullException` if `selector` is `null`.

---

### `ExtraInfo`

```csharp
ITreeControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
```

Shows a secondary piece of text next to each node label (return `null` to show nothing for that node).

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .ExtraInfo(n => n.Info)
    .Run();
```

> Throws `ArgumentNullException` if `extraInfoNode` is `null`.

---

### `ExtraInfoAsync`

```csharp
ITreeControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
```

Asynchronous version of [`ExtraInfo`](#extrainfo).

> ⚠️ The task is awaited **synchronously (blocking)** once per node, per render frame — keep it fast.

---

## Paths & paging

### `PathSeparator`

```csharp
ITreeControl<T> PathSeparator(char value)
```

Sets the character that joins the parent chain when a full path is shown. Default is `'/'`.

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .PathSeparator('.')       // Company.Engineering.Backend.API
    .ShowFullPath()
    .Run();
```

The separator is also used by [`Filter`](#filter), which matches against the joined full path.

---

### `ShowFullPath`

```csharp
ITreeControl<T> ShowFullPath(bool value = true)
```

Shows the full path (parent chain) instead of only the entry name in the answer line. Default `false`.

---

### `PageSize`

```csharp
ITreeControl<T> PageSize(byte value)
```

Rows visible at once (valid range 0–255). `0` (default) auto-fits from terminal height. Only the
visible slice is materialized, so large trees stay cheap.

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .PageSize(15)
    .Run();
```

---

## Filtering

### `Filter`

```csharp
ITreeControl<T> Filter(FilterMode value)
```

Enables interactive filtering. Typing a printable character switches the tree to filter mode: the
whole tree is flattened once and the chosen `FilterMode` is applied against each node's **full path**
(the parent chain joined by [`PathSeparator`](#pathseparator)). Clearing the filter restores the lazy
tree view, preserving the previous expand/collapse state.

| `FilterMode` | Behavior |
|---|---|
| `Disabled` | No filtering (default) |
| `Contains` | Match nodes whose full path contains the typed text |
| `StartsWith` | Match nodes whose full path starts with the typed text |

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Filter(FilterMode.Contains)
    .Run();
```

---

## Confirmation rules

### `SelectLeafOnly`

```csharp
ITreeControl<T> SelectLeafOnly(bool value = true)
```

When enabled, blocks confirmation of container nodes — only leaves (nodes without children) can be
confirmed with Enter. Default `false`.

```csharp
PromptPlus.Controls.Tree<Node>("Pick a leaf")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .SelectLeafOnly()
    .Run();
```

---

### `PredicateSelected`

```csharp
ITreeControl<T> PredicateSelected(Func<T, bool> validselect)
ITreeControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
```

Validation evaluated when the user presses **Enter**. On failure the tree stays open and shows an error.

| Overload | Return | Behavior |
|---|---|---|
| `Func<T, bool>` | `true` = valid | Generic error on failure |
| `Func<T, (bool, string?)>` | `(isValid, message)` | Custom `message` on failure |

```csharp
PromptPlus.Controls.Tree<Node>("Pick a service")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .ExtraInfo(n => n.Info)
    .PredicateSelected(n => n.Info == "service"
        ? (true, null)
        : (false, $"'{n.Name}' is a {n.Info}, not a service."))
    .Run();
```

---

### `PredicateSelectedAsync`

```csharp
ITreeControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect)
ITreeControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts.

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Initial value & equality

### `Default`

```csharp
ITreeControl<T> Default(T value, bool useDefaultHistory = true)
```

Pre-selects `value`, expanding the tree down to it when it is reachable from the root. When
`useDefaultHistory` is `true` and [history](#enabledhistory) is enabled, the restored history value
is preferred. Matching uses [`DefaultMatchBy`](#defaultmatchby).

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default(database)   // tree auto-expands to reveal it
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `DefaultMatchBy`

```csharp
ITreeControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
```

Custom equality used to locate the [`Default`](#default) value and any value restored from history.
**Required** — essential for records/classes where reference equality is not meaningful.

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Run();
```

> Throws `ArgumentNullException` if `comparer` is `null`.

---

## Read-only display

### `ViewOnly`

```csharp
ITreeControl<T> ViewOnly(bool value = true)
```

Renders the tree for navigation only — nodes can be expanded/collapsed but not selected. Enter
returns the [`Default`](#default) value (or `null` if none was set).

```csharp
PromptPlus.Controls.Tree<Node>("Read-only tree")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .ViewOnly()
    .Run();
```

---

## Dynamic description

### `ChangeDescription`

```csharp
ITreeControl<T> ChangeDescription(Func<T, string> value)
```

Recomputes the description from the **currently focused node** as the user navigates.

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .ChangeDescription(n => $"[Id={n.Id}] {n.Name}")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
ITreeControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription), awaited synchronously (blocking)
each frame.

> Throws `ArgumentNullException` if `value` is `null`.

---

## History

### `EnabledHistory`

```csharp
ITreeControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists the confirmed value (serialized as JSON) to `filename`. On the next run the tree is searched
— using [`DefaultMatchBy`](#defaultmatchby) — for the restored value so it can be pre-selected. The
`IHistoryOptions` builder is the same one documented for
[Input → EnabledHistory](../input/methods.md#enabledhistory).

```csharp
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .EnabledHistory("tree-history")
    .Run();
```

> Throws `ArgumentNullException` if `filename` is `null`.

---

## Appearance & behavior

### `Styles`

```csharp
ITreeControl<T> Styles(TreeStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Styles(TreeStyles.Selected, new Style(Color.Black, Color.Gray))
    .Run();
```

---

### `Options`

```csharp
ITreeControl<T> Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish, extra-info affixes). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<T?> Run(CancellationToken token = default)
```

Renders the tree and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<T?>`](../../architecture.md#resultpromptt) — the `Content` is **nullable**.

```csharp
var result = PromptPlus.Controls.Tree<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Run();
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `TreeStyles` regions
- [Index](index.md) — overview and method map
- [MultiTree → Methods](../multitree/methods.md) — the multiple-choice sibling
