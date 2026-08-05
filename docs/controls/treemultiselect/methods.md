<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **TreeMultiSelect&lt;T&gt; — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [TreeMultiSelect — Operations →](operations.md)

---

Every fluent method on `ITreeMultiSelectControl<T>`. Each returns the same control instance, so calls chain
in any order — **except** [`AddLast`](#addlast) / [`AddFirst`](#addfirst) / [`AddAfter`](#addafter) /
[`AddBefore`](#addbefore), which return the new [`ITreeMultiSelectNode<T>`](#imultitreenodet) so you can attach
children to it. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.TreeMultiSelect<T>(string prompt = "", string? description = null)`,
> which returns `ITreeMultiSelectControl<T>`.

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
[CheckLeafOnly](#checkleafonly) ·
[CascadeCheck](#cascadecheck) ·
[RecursiveMarkWithCtrlSpace](#recursivemarkwithctrlspace) ·
[Range](#range) ·
[Default](#default) ·
[DefaultMatchBy](#defaultmatchby) ·
[PredicateChecked](#predicatechecked) ·
[PredicateCheckedAsync](#predicatecheckedasync) ·
[ViewOnly](#viewonly) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[EnableHistory](#enablehistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Building the tree

### `Root`

```csharp
ITreeMultiSelectControl<T> Root(T value, bool disable = false, bool check = false)
```

| Parameter | Meaning |
|---|---|
| `disable` | When `true`, the node cannot be checked interactively (`Space`/`Ctrl+Space` are blocked) and it renders with [`TreeMultiSelectStyles.Disabled`](styles.md). A cascade *passes through* a disabled container to reach enabled descendants without touching the disabled node's own flag. `Default(...)` can still force-check a disabled node, and that forced mark survives `F2` clear-all. Default `false`. |
| `check` | When `true`, the node starts pre-checked. Additive with [`Default`](#default) / history (neither clears the other); it is applied *before* `Default`/history is resolved. Default `false`. |

Sets the top-level node shown at the top of the tree. **Required** — call it before adding any children.

```csharp
PromptPlus.Controls.TreeMultiSelect<string>("Folders")
    .Root("Company")
    .TextSelector(n => n)
    .DefaultMatchBy((a, b) => a == b)
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `AddLast`

```csharp
ITreeMultiSelectNode<T> AddLast(T value, bool disable = false, bool check = false)
```

Adds a first-level node (child of the root) at the **end** and returns it so children can be attached.
The `disable` and `check` parameters have the same meaning as on [`Root`](#root).

```csharp
var eng = tree.AddLast("Engineering");
eng.AddLast("Backend");   // nested child
```

---

### `AddFirst`

```csharp
ITreeMultiSelectNode<T> AddFirst(T value, bool disable = false, bool check = false)
```

Adds a first-level node at the **beginning** so it appears at the very top of the child list.
The `disable` and `check` parameters have the same meaning as on [`Root`](#root).

---

### `AddAfter`

```csharp
ITreeMultiSelectNode<T> AddAfter(ITreeNode<T> node, T value, bool disable = false, bool check = false)
```

Inserts a sibling immediately **after** `node` and returns the new node.
The `disable` and `check` parameters have the same meaning as on [`Root`](#root).

> Throws `InvalidOperationException` if `node` does not belong to this tree or is the root.

---

### `AddBefore`

```csharp
ITreeMultiSelectNode<T> AddBefore(ITreeNode<T> node, T value, bool disable = false, bool check = false)
```

Inserts a sibling immediately **before** `node` and returns the new node.
The `disable` and `check` parameters have the same meaning as on [`Root`](#root).

```csharp
var sales = tree.AddLast("Sales");
tree.AddBefore(sales, "HR");   // → [HR, Sales]
```

> Throws `InvalidOperationException` if `node` does not belong to this tree or is the root.

---

### `ITreeMultiSelectNode<T>`

The object returned by the `Add*` methods. It extends [`ITreeNode<T>`](#itreenodet) with TreeMultiSelect-specific
overloads so that nodes built by *chaining* (deep subtrees) can also be pre-checked or disabled. Use it to
read the node and attach children.

```csharp
public interface ITreeMultiSelectNode<T> : ITreeNode<T>
{
    // adds a child, optionally disabled and/or pre-checked
    ITreeMultiSelectNode<T> AddLast(T value, bool disable = false, bool check = false);
    ITreeMultiSelectNode<T> AddFirst(T value, bool disable = false, bool check = false);
}

public interface ITreeNode<T>
{
    T Value { get; }                 // the user value on this node
    ITreeNode<T>? Parent { get; }    // parent node, or null for the root
    ITreeNode<T> AddLast(T value);   // append a child (check/disable default to false)
    ITreeNode<T> AddFirst(T value);  // prepend a child (check/disable default to false)
}
```

> The base `ITreeNode<T>` (shared with the single-selection `TreeSelect` control) is left untouched; its
> `AddLast`/`AddFirst` behave as `check: false, disable: false`. The `disable` and `check` parameters have
> the same semantics as on [`Root`](#root).

A node with at least one child renders as a **container** (with a tri-state checkbox); a node with
none renders as a **leaf**.

---

## Populating from a source

### `Interaction`

```csharp
ITreeMultiSelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITreeMultiSelectControl<T>> interactionAction)
```

Iterates a source collection and lets you add first-level nodes (and their descendants)
programmatically — the callback receives each item and the control.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Departments")
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

---

### `InteractionAsync`

```csharp
ITreeMultiSelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITreeMultiSelectControl<T>, Task> interactionAction)
```

Asynchronous version of [`Interaction`](#interaction). Each callback is awaited synchronously so the
tree is fully populated before `Run` is called.

---

## Node text & info

### `TextSelector`

```csharp
ITreeMultiSelectControl<T> TextSelector(Func<T, string> selector)
```

Sets how each node is rendered as text. **Required.**

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company)
    .TextSelector(n => n.Name)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Run();
```

---

### `ExtraInfo`

```csharp
ITreeMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
```

Shows a secondary piece of text next to each node label (return `null` to show nothing for that node).
The focused node's `ExtraInfo` also appears in the live answer line while navigating (not in the
final checked-values summary shown after **Enter**) — see
[Operations](operations.md#anatomy-of-the-control).

> Throws `ArgumentNullException` if `extraInfoNode` is `null`.

---

### `ExtraInfoAsync`

```csharp
ITreeMultiSelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
```

Asynchronous version of [`ExtraInfo`](#extrainfo).

> ⚠️ The task is awaited **synchronously (blocking)** once per node, per render frame — keep it fast.

---

## Paths & paging

### `PathSeparator`

```csharp
ITreeMultiSelectControl<T> PathSeparator(char value)
```

Sets the character that joins the parent chain when a full path is shown. Default is `'/'`.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .PathSeparator('.')       // Company.Engineering.Backend.API
    .ShowFullPath()
    .Run();
```

---

### `ShowFullPath`

```csharp
ITreeMultiSelectControl<T> ShowFullPath(bool value = true)
```

When `true`, the answer line shows the full ancestor path for each checked item instead of just its
own name. Default `false`.

---

### `PageSize`

```csharp
ITreeMultiSelectControl<T> PageSize(byte value)
```

Rows visible at once. `0` (default) auto-fits from terminal height. Only the visible slice is
materialized, so large trees stay cheap.

---

## Filtering

### `Filter`

```csharp
ITreeMultiSelectControl<T> Filter(FilterMode value)
```

Enables interactive filtering. Typing a printable character switches the tree into filter mode and
applies the chosen `FilterMode` against each node's full path. Checking still works on matched nodes;
clearing the filter restores the tree view. Default `FilterMode.Disabled`.

| `FilterMode` | Behavior |
|---|---|
| `Disabled` | No filtering (default) |
| `Contains` | Match nodes whose path contains the typed text |
| `StartsWith` | Match nodes whose path starts with the typed text |

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Filter(FilterMode.Contains)
    .Run();
```

---

## Checking behavior

### `CheckLeafOnly`

```csharp
ITreeMultiSelectControl<T> CheckLeafOnly(bool value = true)
```

When `true`, only leaf nodes (nodes without children) can be checked — checking a container is
blocked. Default `false`.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Check leaves")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .CheckLeafOnly()
    .Run();
```

---

### `CascadeCheck`

```csharp
ITreeMultiSelectControl<T> CascadeCheck(bool value = true)
```

When `true` (**default**), checking/unchecking a container propagates the new state to all its
descendants. When `false`, only the container itself is toggled.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .CascadeCheck(false)   // container check does not touch children
    .Run();
```

---

### `RecursiveMarkWithCtrlSpace`

```csharp
ITreeMultiSelectControl<T> RecursiveMarkWithCtrlSpace(bool value = true)
```

Controls which key performs the recursive (container + all descendants) check:

| Setting | Plain `Space` | `Ctrl+Space` |
|---|---|---|
| `false` (default) | Recursive on containers (when [`CascadeCheck`](#cascadecheck) is `true`) | — |
| `true` | Toggles only the focused node itself | Recursive check on the container and its descendants |

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .CascadeCheck(true)
    .RecursiveMarkWithCtrlSpace(true)   // Space = single, Ctrl+Space = recursive
    .Run();
```

---

### `Range`

```csharp
ITreeMultiSelectControl<T> Range(int minvalue, int? maxvalue = null)
```

Defines the valid range for the number of checked items. Confirmation (Enter) is blocked until the
count falls within `[minvalue, maxvalue]`. When `maxvalue` is `null` there is no upper bound.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Pick 2 to 4")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Range(2, 4)
    .Run();
```

> Throws `ArgumentOutOfRangeException` if `minvalue` is negative, or if `maxvalue` is specified and
> less than `minvalue`.

---

## Check rules

### `PredicateChecked`

```csharp
ITreeMultiSelectControl<T> PredicateChecked(Func<T, bool> validselect)
ITreeMultiSelectControl<T> PredicateChecked(Func<T, (bool, string?)> validselect)
```

Decides whether a node can be checked. Nodes that fail the predicate show an error when the user tries
to check them.

| Overload | Return | Behavior |
|---|---|---|
| `Func<T, bool>` | `true` = checkable | Generic error on failure |
| `Func<T, (bool, string?)>` | `(isValid, message)` | Custom `message` on failure |

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Check services")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .ExtraInfo(n => n.Info)
    .PredicateChecked(n => n.Info == "service"
        ? (true, null)
        : (false, $"'{n.Name}' is a {n.Info}, not a service."))
    .Run();
```

---

### `PredicateCheckedAsync`

```csharp
ITreeMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect)
ITreeMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts.

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Initial values & equality

### `Default`

```csharp
ITreeMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true)
```

Pre-checks one or more items; the tree auto-expands to reveal each of them. When `useDefaultHistory`
is `true` and [history](#enablehistory) is enabled, the restored history values override `values`.
Matching uses [`DefaultMatchBy`](#defaultmatchby).

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default([api, mobile])
    .Run();
```

---

### `DefaultMatchBy`

```csharp
ITreeMultiSelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
```

Custom equality used to locate the [`Default`](#default) values and any values restored from history.
**Required** — essential for records/classes where reference equality is not meaningful.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Run();
```

---

## Read-only display

### `ViewOnly`

```csharp
ITreeMultiSelectControl<T> ViewOnly(bool value = true)
```

Puts the control into view-only mode: the user can navigate and expand/collapse the tree but cannot
check items. Enter returns the pre-checked [`Default`](#default) values.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Read-only")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default([api, emea])
    .ViewOnly()
    .Run();
```

---

## Dynamic description

### `ChangeDescription`

```csharp
ITreeMultiSelectControl<T> ChangeDescription(Func<T, string> value)
```

Recomputes the description from the node **currently under the cursor** as the user navigates.

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .ChangeDescription(n => $"[Id={n.Id}] {n.Name}")
    .Run();
```

---

### `ChangeDescriptionAsync`

```csharp
ITreeMultiSelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

---

## History

### `EnableHistory`

```csharp
ITreeMultiSelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists the checked values to `filename`; previously checked items are restored on the next run. The
`IHistoryOptions` builder is the same one documented for
[Input → EnableHistory](../input/methods.md#enablehistory).

```csharp
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default([api, database])
    .EnableHistory("multi-tree-history")
    .Run();
```

> Throws `ArgumentNullException` if `filename` is `null`, `ArgumentException` if it is empty/whitespace.

---

## Appearance & behavior

### `Styles`

```csharp
ITreeMultiSelectControl<T> Styles(TreeMultiSelectStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Styles(TreeMultiSelectStyles.Selected, new Style(Color.Black, Color.Gray))
    .Run();
```

---

### `Options`

```csharp
ITreeMultiSelectControl<T> Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish, extra-info affixes). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

---

## Running the control

### `Run`

```csharp
ResultPrompt<T[]> Run(CancellationToken token = default)
```

Renders the tree and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<T[]>`](../../architecture.md#resultpromptt) — the `Content` is the **array** of checked
values.

```csharp
var result = PromptPlus.Controls.TreeMultiSelect<Node>("Nodes")
    .Root(company).TextSelector(n => n.Name).DefaultMatchBy((a, b) => a.Id == b.Id)
    .Run();
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `TreeMultiSelectStyles` regions
- [Index](index.md) — overview and method map
- [TreeSelect → Methods](../treeselect/methods.md) — the single-choice sibling
