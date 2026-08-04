<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **TreeMultiSelect<T> — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [TreeMultiSelect — Styles →](styles.md)

---

How the `TreeMultiSelect<T>` control is built and how it behaves while running: the tree model, keyboard,
checking and cascade, filtering, leaf-only rules, validation, ranges, history, and view-only mode.

---

## Anatomy of the control

```
Check items and press Enter: API  (service) ← prompt + live answer (follows the cursor) + ExtraInfo
Space=check  Enter=confirm  ESC=abort     ← description (optional / dynamic)
▼ [~] Company                            ← root, indeterminate (some descendants checked)
  ▼ [x] Engineering        (dept)        ← checked container + ExtraInfo
    ▶ [x] Backend          (team)        ← collapsed, fully checked
  › [ ] API                (service)     ← focused unchecked leaf
  ▶ [ ] Sales              (dept)
Filter: ap_                              ← live filter text (when filtering)
Checked: 3                               ← tagged count
Page 1/2                                 ← pagination
Enter: confirm  Esc: cancel              ← tooltip
```

Checkbox states: `[ ]` unchecked · `[x]` checked · `[~]` indeterminate (some but not all descendants
checked). The answer line updates as you navigate and includes `ExtraInfo`/`ExtraInfoAsync` when set
(same two-space format as the list row), scrollable via `Home`/`End`/`←`/`→` when it overflows the
width. Once confirmed (**Enter**), the final answer shown is the checked-values summary — no
`ExtraInfo`. Every region can be recolored — see [Styles](styles.md).

---

## Building the tree model

The hierarchy is built explicitly, in code, before `Run` — identical to [`TreeSelect<T>`](../treeselect/operations.md#building-the-tree-model).
Three calls are **required**: [`Root`](methods.md#root), [`TextSelector`](methods.md#textselector),
and [`DefaultMatchBy`](methods.md#defaultmatchby).

1. **Set the root.** `Root(value)` defines the single top-level node. Call it first.
2. **Add first-level nodes.** [`AddLast`](methods.md#addlast) / [`AddFirst`](methods.md#addfirst)
   attach a node under the root and **return an [`ITreeNode<T>`](methods.md#itreenodet)**.
3. **Add children.** Call `AddLast` / `AddFirst` on the returned node to nest deeper.
4. **Order siblings** with [`AddAfter`](methods.md#addafter) / [`AddBefore`](methods.md#addbefore).

```csharp
var tree = PromptPlus.Controls.TreeMultiSelect<Node>("Check items")
    .Root(company)
    .TextSelector(n => n.Name)
    .DefaultMatchBy((a, b) => a.Id == b.Id);

var eng     = tree.AddLast(engineering);   // first-level
var backend = eng.AddLast(backendTeam);    // child of Engineering
backend.AddLast(api);                      // leaf
backend.AddLast(database);                 // leaf
tree.AddLast(sales);
```

- **Container vs leaf is inferred:** a node with children is a container (with a tri-state checkbox);
  a node with none is a leaf.
- **[`Interaction` / `InteractionAsync`](methods.md#interaction)** build the same structure from an
  external source.
- **Lazy rendering:** rows are materialized on expand and released on collapse, so large trees stay
  cheap.

---

## Keyboard

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down |
| `→` / `+` | Expand the focused container |
| `←` / `-` | Collapse the focused container |
| `Space` | Check / uncheck the focused node (cascades per [`CascadeCheck`](methods.md#cascadecheck)) |
| `Ctrl+Space` | Recursive check of a container + descendants (when [`RecursiveMarkWithCtrlSpace`](methods.md#recursivemarkwithctrlspace) is on) |
| `F2` | Toggle-all (check / uncheck every node) |
| `Page Up` / `Page Down` | Jump one page |
| `Ctrl+Home` / `Ctrl+End` | First / last visible row |
| `Shift+F3` | Toggle short name ↔ full path display |
| `Enter` | Confirm the checked set (runs the range + validation gates) |
| `Esc` | Abort → `IsAborted == true` |
| Any printable character | Type to filter (when [`Filter`](methods.md#filter) is not `Disabled`) |
| `Backspace` | Edit / clear the filter text |
| `Home` / `End` / `←` / `→` | Scroll the answer line horizontally (when it overflows the width) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

---

## Checking & cascade

The check key (**Space**) toggles the focused node. What happens next depends on two settings:

- **[`CascadeCheck`](methods.md#cascadecheck)** (default `true`): checking/unchecking a container
  propagates the state to all its descendants. With `false`, only the container itself toggles.
- **[`RecursiveMarkWithCtrlSpace`](methods.md#recursivemarkwithctrlspace)** (default `false`):
  - `false` → plain **Space** performs the recursive mark on containers (when `CascadeCheck` is on).
  - `true` → plain **Space** toggles only the focused node; **Ctrl+Space** does the recursive mark.

Containers show a **tri-state** checkbox: unchecked when no descendant is checked, checked when all
are, and indeterminate (`[~]`) when only some are. `F2` toggles every node at once.

---

## Leaf-only checking

[`CheckLeafOnly()`](methods.md#checkleafonly) blocks checking of container nodes — only leaves can be
checked. Attempting to check a container is rejected. Use it when only concrete items (not folders)
are valid selections.

---

## Filtering

When [`Filter`](methods.md#filter) is `Contains` or `StartsWith`, typing a printable character
switches the tree into filter mode: nodes are matched against their **full path** (parent chain joined
by [`PathSeparator`](methods.md#pathseparator)), case-insensitively. Checking still works on matched
nodes. **Backspace** edits the filter; clearing it restores the tree view. `Disabled` (the default)
turns typing off entirely.

---

## Node text & extra info

- [`TextSelector`](methods.md#textselector) decides each node's label (required).
- [`ExtraInfo` / `ExtraInfoAsync`](methods.md#extrainfo) render a secondary column next to the label.
- [`ShowFullPath`](methods.md#showfullpath) makes the answer line show the full parent chain for each
  checked item; `Shift+F3` toggles the same short/long display while navigating.

---

## Disabled nodes

Any node added with `disable: true` (on [`Root`](methods.md#root), [`AddLast`](methods.md#addlast),
[`AddFirst`](methods.md#addfirst), [`AddAfter`](methods.md#addafter), [`AddBefore`](methods.md#addbefore),
or the chained [`ITreeMultiSelectNode<T>`](methods.md#imultitreenodet) overloads) is rendered with
[`TreeMultiSelectStyles.Disabled`](styles.md) and follows a distinct tri-state / cascade semantic:

1. **Interactive checks are blocked.** `Space` / `Ctrl+Space` on a disabled node do nothing
   (`SelectionDisabled`), just like the single-selection `TreeSelect`.
2. **Cascade passes through, it does not mark.** A cascade (`Ctrl+Space` on an ancestor, with
   `CascadeCheck` on) crosses a disabled container to reach its *enabled* descendants without touching the
   disabled container's own flag.
3. **`Default(...)` can force it.** A disabled node can still be force-checked through
   [`Default`](methods.md#default) (or the construction-time `check: true`), bypassing the interactive block.
4. **Forced marks survive clear-all.** `F2` (toggle-all) skips disabled nodes in *both* directions: it
   neither checks them on select-all nor unchecks a force-checked disabled node on clear-all.

> Because a container's checkbox under `CascadeCheck` is always derived from its enabled leaf descendants,
> a disabled container whose descendants are fully checked reports **Indeterminate** (never claims a
> confirmation of its own) unless its own flag was explicitly forced via `Default`/`check`.

---

## Confirmation flow: range & validation

Pressing **Enter** confirms the checked set after two gates:

1. **Range gate** — if [`Range(min, max)`](methods.md#range) is set, confirmation is blocked until the
   number of checked items is within `[min, max]`.
2. **Per-node validation** — [`PredicateChecked`](methods.md#predicatechecked) /
   [`PredicateCheckedAsync`](methods.md#predicatecheckedasync) run when the user tries to *check* a
   node; failing nodes show an error and cannot be checked.

When both pass, the control closes and returns the checked values as `T[]`.

---

## Initial values & history

- [`Default(values)`](methods.md#default) pre-checks nodes and expands the tree to reveal each one;
  provide [`DefaultMatchBy`](methods.md#defaultmatchby) so the right nodes are located (required).
- With [`EnableHistory`](methods.md#enablehistory), the checked set is persisted to disk and
  restored on the next run; the restored values override `Default` when
  `useDefaultHistory: true` is in effect.

---

## View-only mode

[`ViewOnly()`](methods.md#viewonly) renders the tree for display only:

- Arrow keys navigate and containers still expand/collapse, but Space is disabled — nothing can be
  checked or unchecked.
- Enter returns the pre-checked [`Default`](methods.md#default) values.
- Useful for showing a read-only snapshot of a checked hierarchy inline with other prompts.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `TreeMultiSelect<T>` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the tree after confirm — the whole control is erased, not just the interactive part |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally
(`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **The result is an array.** `Run` returns `ResultPrompt<T[]>`; `.Content` is an empty array when
  nothing is checked. Always branch on `IsAborted` first.
- **Custom types need equality.** [`DefaultMatchBy`](methods.md#defaultmatchby) is required and drives
  `Default`, history, and cascade matching.
- **Range blocks Enter, predicate blocks Space.** `Range` gates the final confirm; `PredicateChecked`
  gates individual checks — they are separate stages.
- **Root must come first.** Adding nodes before `Root` throws `InvalidOperationException`.
- **Async callbacks block the UI thread** — keep validators, extra-info, and description callbacks fast.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
- [TreeSelect → Operations](../treeselect/operations.md) — the single-choice sibling
