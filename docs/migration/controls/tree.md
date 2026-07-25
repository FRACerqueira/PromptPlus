# Migration v5.x → v6.x: Tree and MultiTree

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## Renamed controls

| v5.x | v6.x |
|---|---|
| `NodeTreeSelect<T>()` | `Tree<T>()` |
| `NodeTreeMultiSelect<T>()` | `MultiTree<T>()` |

## Renamed public methods

Beyond the factory renames above, `MultiTree<T>` **renamed** its confirmation validator. `Tree<T>`
(single-selection) keeps `PredicateSelected`.

| Control | v5.x member | v6.x member |
|---|---|---|
| MultiTree | `PredicateSelected(Func<T, bool>)` | `PredicateChecked(Func<T, bool>)` |
| MultiTree | `PredicateSelected(Func<T, (bool, string?)>)` | `PredicateChecked(Func<T, (bool, string?)>)` |

> The async overloads follow the same name: `MultiTree` uses `PredicateCheckedAsync`; `Tree` uses
> `PredicateSelectedAsync`.

The type constraint is unchanged: `where T : class, new()`.

---

## Breaking Changes

### 1. Tree-building API fully reworked ⚠️

This is the most impactful change. The v5.x `AddRootNode` / `AddChildNode` methods **no longer exist**. In v6.x you set the root with `Root(T)` and attach nodes with `AddLast` / `AddFirst`, which **return the created node** so you can attach children to it.

**Before (v5.x):**
```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls.NodeTreeSelect<Department>("Department:")
    .AddRootNode(company, nodeseparator: " > ")
    .AddChildNode(company, division)
    .AddChildNode(division, team)
    .TextSelector(d => d.Name)
    .Run();
```

**After (v6.x):**
```csharp
using PromptPlusLibrary;

var control = PromptPlus.Controls.Tree<Department>("Department:")
    .TextSelector(d => d.Name)
    .PathSeparator('>');       // was the per-call `nodeseparator` string

control.Root(company);         // required
var divisionNode = control.AddLast(division);   // first-level node (child of root)
divisionNode.AddLast(team);                      // child of the division node

var result = control.Run();
Department? picked = result.Content;             // ResultPrompt<T?> → read via .Content
```

> **What changed:**
> - `AddRootNode(T, string nodeseparator)` → `Root(T)` + `PathSeparator(char)`
> - `AddChildNode(parent, value)` → `control.AddLast/AddFirst(value)` for first-level nodes, then `node.AddLast/AddFirst(value)` for deeper children (plus `AddAfter`/`AddBefore` for sibling insertion)
> - `AddLast`/`AddFirst` return the new `ITreeNode<T>`

### 2. `DisableRecursiveCount` — removed with no equivalent

```csharp
// v5.x only — remove during migration
.DisableRecursiveCount(true)
```

### 3. `HideCount` / `HideCountSelected` — removed with no equivalent

v5.x `NodeTreeSelect` had `HideCount(bool)`; `NodeTreeMultiSelect` had both `HideCount(bool)` and `HideCountSelected(bool)`. **None exist in v6.x.**

```csharp
// v5.x only — remove during migration
.HideCount(true)
.HideCountSelected(true)
```

### 4. `PredicateDisabled` — removed with no equivalent

```csharp
// v5.x only — per-node disable predicate is not available in v6.x
.PredicateDisabled(node => !node.Enabled)
```

### 5. `MaxWidth(byte)` — removed

```csharp
// v5.x only — remove during migration
.MaxWidth(60)
```

### 6. Styles enum renamed

```csharp
// v5.x
.Styles(NodeTreeStyles.Selected, style)

// v6.x
.Styles(TreeStyles.Selected, style)        // MultiTreeStyles for MultiTree
```

### 7. Remote tree controls — removed with no equivalent

`NodeTreeRemoteSelect<T1,T2>` and `NodeTreeRemoteMultiSelect<T1,T2>` were removed.

**Before (v5.x):**
```csharp
PromptPlus.Controls.NodeTreeRemoteSelect<Category, int>()
    .RemoteSource(async (parentId, ct) => await LoadChildrenAsync(parentId, ct))
    .Run();
```

**After (v6.x):**
```csharp
// No direct equivalent — load the whole tree first, then build it.
var data = await LoadWholeTreeAsync();

var control = PromptPlus.Controls.Tree<Category>("Category:")
    .TextSelector(c => c.Name);
control.Root(data.Root);
// attach nodes from your pre-loaded data...
var result = control.Run();
```

> ⚠️ Applications that relied on lazy (on-demand) child loading must pre-load the whole structure or implement paging externally.

---

## What's new in v6.x

New on `Tree`/`MultiTree`: `ViewOnly` · `Filter` · `ShowFullPath` · `DefaultMatchBy` · `Default` / `EnabledHistory` · `AddAfter` / `AddBefore` · `ChangeDescriptionAsync` · `ExtraInfoAsync` · `InteractionAsync`. `Tree` adds `SelectLeafOnly` and `PredicateSelectedAsync` (x2); `MultiTree` adds `CheckLeafOnly`, `CascadeCheck`, `RecursiveMarkWithCtrlSpace`, `PredicateCheckedAsync` (x2), plus the node-level `disable` / `check` parameters on `Root`/`Add*`.

```csharp
PromptPlus.Controls.MultiTree<Category>("Categories:")
    .TextSelector(c => c.Name)
    .CascadeCheck(true)                 // checking a node checks its descendants
    .RecursiveMarkWithCtrlSpace(true)   // Ctrl+Space marks recursively
    .Range(1, 5)
    .Run();
```

---

## Full API reference

### Tree\<T\> — v5.x vs v6.x

| Method | v5.x (NodeTreeSelect) | v6.x (Tree) | Change |
|---|---|---|---|
| Factory | `NodeTreeSelect<T>()` | `Tree<T>()` | Renamed |
| `AddRootNode(T, string nodeseparator)` | ✅ | ❌ | → `Root(T)` + `PathSeparator(char)` |
| `AddChildNode(T parent, T value)` | ✅ | ❌ | → `AddLast`/`AddFirst` (control + node) |
| `TextSelector(Func<T,string>)` · `ExtraInfo(Func<T,string?>)` · `PageSize(byte)` · `ChangeDescription` · `Interaction` · `PredicateSelected` (x2) | ✅ | ✅ | Unchanged |
| `DisableRecursiveCount(bool)` | ✅ | ❌ | Removed |
| `HideCount(bool)` | ✅ | ❌ | Removed |
| `PredicateDisabled(Func<T,bool>)` | ✅ | ❌ | Removed |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `Styles(NodeTreeStyles, Style)` | ✅ | ❌ | → `Styles(TreeStyles, Style)` |
| `Root` · `AddLast` · `AddFirst` · `AddAfter` · `AddBefore` · `PathSeparator` · `ViewOnly` · `Filter` · `SelectLeafOnly` · `ShowFullPath` · `DefaultMatchBy` · `Default` · `EnabledHistory` · async variants | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<T>` | `ResultPrompt<T?>` | Read via `.Content` |

### MultiTree\<T\> — v5.x vs v6.x

| Method | v5.x (NodeTreeMultiSelect) | v6.x (MultiTree) | Change |
|---|---|---|---|
| Factory | `NodeTreeMultiSelect<T>()` | `MultiTree<T>()` | Renamed |
| `AddRootNode` / `AddChildNode` | ✅ | ❌ | → `Root` + `AddLast`/`AddFirst` |
| `DisableRecursiveCount(bool)` | ✅ | ❌ | Removed |
| `HideCount(bool)` / `HideCountSelected(bool)` | ✅ | ❌ | Removed |
| `PredicateDisabled(Func<T,bool>)` | ✅ | ❌ | Removed |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `Range(int, int?)` | ✅ | ✅ | Unchanged |
| `PredicateSelected` (x2) | ✅ | ❌ | Renamed to `PredicateChecked` (x2) |
| `CascadeCheck` · `RecursiveMarkWithCtrlSpace` · `CheckLeafOnly` · `ShowFullPath` · `ViewOnly` · `Filter` · `DefaultMatchBy` · `PredicateChecked` (x2) · async variants | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<T[]>` | `ResultPrompt<T[]>` | Unchanged |
