<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Select&lt;T&gt; — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Select — Styles →](styles.md)

---

How the `Select<T>` control behaves while it is running: keyboard, filtering, grouping, paging,
validation, history, and view-only mode.

---

## Anatomy of the control

```
Which city?                              ← prompt
Type to filter                           ← description (optional / dynamic)
North America                            ← group header (grouped lists)
› Seattle            (Length: 7)         ← focused item + ExtraInfo
  New York           (Length: 8)
Asia
  Tokyo              (Length: 5)
Filter: to_                              ← live filter text (when filtering)
Page 1/2                                 ← pagination
Enter: confirm  Esc: cancel              ← tooltip
```

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down |
| `Page Up` / `Page Down` | Jump one page |
| `Home` / `End` | First / last item |
| `Enter` | Confirm the focused item (runs validation) |
| `Esc` | Abort → `IsAborted == true` |
| Any printable character | Type to filter (when [`Filter`](methods.md#filter) is not `Disabled`) |
| `Backspace` | Edit the filter text |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

Disabled items and separators are skipped automatically as you move.

---

## Filtering

When [`Filter`](methods.md#filter) is `Contains` or `StartsWith`, typing narrows the visible list in
real time:

- The match is case-insensitive.
- Matching resets paging to the first page of results.
- With [`AutoSelect`](methods.md#autoselect), narrowing to a single selectable item confirms it
  immediately — no Enter required.
- Clearing the filter (Backspace) restores the full list.

`Disabled` (the default) turns typing off entirely — arrow keys only.

---

## Grouping & separators

- [`AddGroupedItem` / `AddGroupedItems`](methods.md#addgroupeditem) render a header above their items;
  the header repeats appropriately as the list scrolls.
- A hint shows the group of the focused item — hide it with [`HideTipGroup`](methods.md#hidetipgroup).
- [`AddSeparator`](methods.md#addseparator) draws a divider line (single, double, or a custom char).
  Separators are purely visual and are skipped during navigation.

Grouped and ungrouped items can be mixed in one list.

---

## Item text & extra info

- [`TextSelector`](methods.md#textselector) decides the label; without it, `ToString()` is used and
  enum members honor their `[Display(Name = ...)]` attribute.
- Enum ordering follows `[Display(Order = ...)]` when present.
- [`ExtraInfo`](methods.md#extrainfo) adds a secondary label per row, wrapped by the extra-info
  prefix/suffix (default `(` `)`, configurable).

---

## Confirmation & validation flow

Pressing **Enter** on the focused item:

1. Validation runs — [`PredicateSelected`](methods.md#predicateselected) /
   [`PredicateSelectedAsync`](methods.md#predicateselectedasync), if configured.
2. **Valid** → the control closes and returns the item.
   **Invalid** → the list stays open and shows the error line.

Disabled items cannot be focused for confirmation, so validation typically guards *business* rules
(e.g., "this option is not available for your plan") rather than availability.

---

## Initial selection & history

- [`Default(value)`](methods.md#default) pre-highlights an item; provide
  [`DefaultMatchBy`](methods.md#defaultmatchby) for records/classes so the right row is located.
- With [`EnableHistory`](methods.md#enablehistory), confirmed selections are stored on disk;
  [`UseDefaultHistory`](methods.md#usedefaulthistory) (or `Default(..., useDefaultHistory: true)`)
  restores the last one on the next run.
- History options (`MinPrefixLength`, `MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`) match
  the [Input history options](../input/methods.md#enablehistory).

---

## View-only mode

[`ViewOnly()`](methods.md#viewonly) renders the list for display only:

- Arrow keys still scroll, but items cannot be selected.
- Combine with `Default` to highlight one entry.
- Useful for showing a read-only snapshot inline with other prompts.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Select<T>` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must choose |
| `HideAfterFinish(true)` | Erases the list after confirm; only the answer remains |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally
(`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **Aborted results** carry `.Content == default(T)` (e.g., `null` for reference types). Always branch
  on `IsAborted`.
- **Custom types need equality.** Without [`DefaultMatchBy`](methods.md#defaultmatchby), `Default` on a
  record/class may not match the intended row.
- **Async callbacks block the UI thread** — keep validators, selectors, and description callbacks fast.
- **Long labels** are truncated to a single line with `…`; the full item is still returned in `.Content`.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
