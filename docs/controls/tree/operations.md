<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Tree<T> — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Tree — Styles →](styles.md)

---

How the `Tree<T>` control is built and how it behaves while running: the tree model, keyboard,
expand/collapse, filtering, leaf-only rules, validation, history, and view-only mode.

---

## Anatomy of the control

```
Pick a service: API  (service)           ← prompt + live answer (follows the cursor) + ExtraInfo
Type to filter the full path             ← description (optional / dynamic)
▼ Company                                ← root (expanded container)
  ▼ Engineering            (dept)        ← container + ExtraInfo
    ▶ Backend              (team)        ← collapsed container
  › API                    (service)     ← focused leaf
  ▶ Sales                  (dept)
Filter: ap_                              ← live filter text (when filtering)
Page 1/2                                 ← pagination
Enter: confirm  Esc: cancel              ← tooltip
```

The answer line updates as you navigate and includes `ExtraInfo`/`ExtraInfoAsync` when set (same
two-space format as the list row), scrollable via `Home`/`End`/`←`/`→` when it overflows the width.
Once confirmed (**Enter**), the final answer shown is the plain node text/path — no `ExtraInfo`.

Every region can be recolored — see [Styles](styles.md).

---

## Building the tree model

The hierarchy is built explicitly, in code, before `Run`. Three calls are **required**:
[`Root`](methods.md#root), [`TextSelector`](methods.md#textselector), and
[`DefaultMatchBy`](methods.md#defaultmatchby).

1. **Set the root.** `Root(value)` defines the single top-level node. Call it first — adding children
   before the root throws `InvalidOperationException`.
2. **Add first-level nodes.** [`AddLast`](methods.md#addlast) / [`AddFirst`](methods.md#addfirst)
   attach a node under the root and **return an [`ITreeNode<T>`](methods.md#itreenodet)**.
3. **Add children.** Call `AddLast` / `AddFirst` on the returned node to nest deeper — repeat to any
   depth.
4. **Order siblings** with [`AddAfter`](methods.md#addafter) / [`AddBefore`](methods.md#addbefore).

```csharp
var tree = PromptPlus.Controls.Tree<Node>("Pick an item")
    .Root(company)
    .TextSelector(n => n.Name)
    .DefaultMatchBy((a, b) => a.Id == b.Id);

var eng     = tree.AddLast(engineering);   // first-level
var backend = eng.AddLast(backendTeam);    // child of Engineering
backend.AddLast(api);                      // leaf
backend.AddLast(database);                 // leaf
tree.AddLast(sales);
```

- **Container vs leaf is inferred:** a node with children is a container (expandable); a node with
  none is a leaf.
- **[`Interaction` / `InteractionAsync`](methods.md#interaction)** build the same structure from an
  external source — you receive the control in the callback and call `AddLast` on it per item.
- **Lazy rendering:** visible rows are materialized on expand and released on collapse, so memory
  stays proportional to what is on screen — even for trees with thousands of nodes.

---

## Keyboard

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down |
| `→` / `+` | Expand the focused container |
| `←` / `-` | Collapse the focused container |
| `Page Up` / `Page Down` | Jump one page |
| `Ctrl+Home` / `Ctrl+End` | First / last visible row |
| `Shift+F3` | Toggle short name ↔ full path display |
| `Enter` | Confirm the focused node (runs leaf-only + validation) |
| `Esc` | Abort → `IsAborted == true` |
| Any printable character | Type to filter (when [`Filter`](methods.md#filter) is not `Disabled`) |
| `Backspace` | Edit / clear the filter text |
| `Home` / `End` / `←` / `→` | Scroll the answer line horizontally (when it overflows the width) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

---

## Expand & collapse

- Containers start collapsed unless a [`Default`](methods.md#default) (or restored history value)
  lives inside them — the tree auto-expands the branch down to that node.
- Expanding materializes the children of that node only; collapsing releases them again.
- Leaves have no expand indicator and ignore the expand/collapse keys.

---

## Filtering

When [`Filter`](methods.md#filter) is `Contains` or `StartsWith`, typing a printable character
switches the tree into filter mode:

- The whole tree is flattened once and each node's **full path** (parent chain joined by
  [`PathSeparator`](methods.md#pathseparator)) is matched against the typed text.
- Matching is case-insensitive.
- **Backspace** edits the filter; clearing it entirely restores the lazy tree view with the previous
  expand/collapse state intact.

`Disabled` (the default) turns typing off entirely — navigation keys only.

---

## Node text & extra info

- [`TextSelector`](methods.md#textselector) decides each node's label (required).
- [`ExtraInfo` / `ExtraInfoAsync`](methods.md#extrainfo) render a secondary column next to the label
  (for example the node's kind or count).
- [`ShowFullPath`](methods.md#showfullpath) makes the answer line show the full parent chain instead
  of only the node name; `Shift+F3` toggles the same short/long display while navigating.

---

## Leaf-only & validation flow

Pressing **Enter** on the focused node:

1. **Leaf-only gate** — if [`SelectLeafOnly`](methods.md#selectleafonly) is on and the node is a
   container, confirmation is rejected and the tree stays open.
2. **Validation** — [`PredicateSelected`](methods.md#predicateselected) /
   [`PredicateSelectedAsync`](methods.md#predicateselectedasync), if configured.
3. **Valid** → the control closes and returns the node value.
   **Invalid** → the tree stays open and shows the error line.

Use `SelectLeafOnly` for *structural* rules ("pick an actual item, not a folder") and
`PredicateSelected` for *business* rules ("only service items can be chosen").

---

## Initial selection & history

- [`Default(value)`](methods.md#default) pre-selects a node and expands the tree to reveal it;
  provide [`DefaultMatchBy`](methods.md#defaultmatchby) so the right node is located (required).
- With [`EnableHistory`](methods.md#enablehistory), the confirmed value is serialized to disk; on
  the next run the tree is searched (via `DefaultMatchBy`) for the restored value and pre-selects it
  when `Default(..., useDefaultHistory: true)` is in effect.

---

## View-only mode

[`ViewOnly()`](methods.md#viewonly) renders the tree for display only:

- Arrow keys navigate and containers still expand/collapse, but nodes cannot be confirmed as a choice.
- Enter returns the [`Default`](methods.md#default) value (or `null` when none was set).
- Useful for showing a read-only snapshot of a hierarchy inline with other prompts.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Tree<T>` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must choose |
| `HideAfterFinish(true)` | Erases the tree after confirm; only the answer remains |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally
(`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **The result is nullable.** `Run` returns `ResultPrompt<T?>`; `.Content` is `null`/`default` when
  aborted or when nothing was confirmed (e.g., `ViewOnly` with no `Default`). Always branch on
  `IsAborted`.
- **Custom types need equality.** [`DefaultMatchBy`](methods.md#defaultmatchby) is required and drives
  both `Default` and history lookups — without correct equality the intended node may not be found.
- **Root must come first.** Adding nodes before `Root` throws `InvalidOperationException`.
- **Async callbacks block the UI thread** — keep validators, extra-info, and description callbacks fast.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
- [MultiTree → Operations](../multitree/operations.md) — the multiple-choice sibling
