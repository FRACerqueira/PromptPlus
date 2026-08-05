<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiSelect&lt;T&gt; — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiSelect — Styles →](styles.md)

---

How the `MultiSelect<T>` control behaves while it is running: keyboard, checking, filtering, grouping,
paging, the selection range, validation, history, and view-only mode.

---

## Anatomy of the control

```
Which cities? Seattle (Length: 7)        ← prompt + live answer (follows the cursor) + ExtraInfo
Type to filter                           ← description (optional / dynamic)
North America                            ← group header (grouped lists)
› [x] Seattle          (Length: 7)       ← focused item (checked) + ExtraInfo
  [ ] New York         (Length: 8)
Asia
  [x] Tokyo            (Length: 5)
Filter: to_                              ← live filter text (when filtering)
Page 1/2                                 ← pagination
Space: check  F2: all  Enter: confirm    ← tooltip
```

> The answer line updates as you navigate and includes `ExtraInfo`/`ExtraInfoAsync` when set — useful
> when a row's own text is too wide for the console and would otherwise be cut off, since this line
> scrolls horizontally (`Home`/`End`/`←`/`→`). Once confirmed (**Enter**), the final answer
> shown is the checked-items summary (`BuildCheckedItemsText`) — no `ExtraInfo`.

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down |
| `Page Up` / `Page Down` | Jump one page |
| `Ctrl+Home` / `Ctrl+End` | First / last item |
| `Space` | Toggle the checkbox of the focused item (runs the check predicate) |
| `Space` (on a group header) | Toggle every item in that group |
| `F2` | **Toggle all** — check all items, or uncheck them if all are checked |
| `F3` | **Filter only selected** — show only the checked items; press again to leave the view |
| `Enter` | Confirm the checked set (runs range validation) |
| `Esc` | Abort → `IsAborted == true`, `Content` is an empty array |
| Any printable character | Type to filter, or **jump** to the next match when filtering is `Disabled` |
| `Backspace` | Edit the filter text |
| `Home` / `End` / `←` / `→` | Scroll the answer line horizontally (when it overflows the width) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

Disabled items and separators are skipped automatically as you move. The `F2` toggle-all and `F3`
filter-selected hotkeys are the multi-select additions over [`Select<T>`](../select/operations.md).

> There is no `F4` wildcard-selection hotkey — that was dead config (`HotKeySelectWildcard`) that
> has since been removed entirely from the codebase, not a feature that belongs to some other
> control instead.
>
> The physical keys for `F2`/`F3` come from `PromptPlus.Config.HotKeyToggleAll` and
> `HotKeyFilterAllSelected`; both are configurable. See [Keyboard Bindings](../../keyboard-bindings.md).

---

## Checking items

- **Space** toggles the focused item's checkbox. Each check first runs the
  [`PredicateChecked`](methods.md#predicatechecked) validator (if configured); a rejected check is
  refused and an error line appears.
- **Space on a group header** toggles every (enabled, non-separator) item in that group at once. If
  they are not all checked, they all become checked; otherwise they all clear.
- **F2 (toggle all)** does the same across the whole list — or across the *currently filtered* items
  when a filter is active.
- Mass operations (group-header Space and F2) **silently skip** items the predicate rejects instead
  of showing an error — only a single manual check surfaces the validation message.
- Disabled items can never be checked, by Space or by a mass operation.

---

## Filtering

When [`Filter`](methods.md#filter) is `Contains` or `StartsWith`, typing narrows the visible list in
real time:

- The match is case-insensitive.
- Matching resets paging to the first page of results.
- Checked state is preserved — filtering only changes what is *visible*, never what is checked.
- Clearing the filter (Backspace) restores the full list.

When [`Filter`](methods.md#filter) is `Disabled` (the default), typing a printable character instead
**jumps** focus to the next item whose text starts with that character (wrapping around) — arrow keys
still work as normal.

---

## Filter only selected (F3)

`F3` toggles a view that shows **only the checked items**, so the user can review or trim a large
selection:

- It is available once at least one item is checked (or to leave the view once inside it).
- Unchecking the last remaining item automatically drops back to the full list.
- It clears any typed text filter while active.

---

## Grouping & separators

- [`AddGroupedItem` / `AddGroupedItems`](methods.md#addgroupeditem) render a header above their items;
  the header repeats appropriately as the list scrolls.
- A hint shows the group of the focused item — hide it with [`HideTipGroup`](methods.md#hidetipgroup).
- Pressing **Space** on a group header bulk-toggles that group (see [Checking items](#checking-items)).
- [`AddSeparator`](methods.md#addseparator) draws a divider line (single, double, or a custom char).
  Separators are purely visual and are skipped during navigation.

Grouped and ungrouped items can be mixed in one list.

---

## Item text & extra info

- [`TextSelector`](methods.md#textselector) decides the label; without it, `ToString()` is used and
  enum members honor their `[Display(Name = ...)]` attribute.
- Enum ordering follows `[Display(Order = ...)]` when present.
- [`ExtraInfo`](methods.md#extrainfo) adds a secondary label for the focused row, wrapped by the
  extra-info prefix/suffix (default `(` `)`, configurable).

---

## Selection range & confirmation flow

Pressing **Enter**:

1. If [`Range`](methods.md#range) is set, the count of checked items is validated —
   - fewer than the minimum → the list stays open with a "minimum" error;
   - more than the maximum → the list stays open with a "maximum" error.
2. Otherwise the control closes and returns the checked items as a `T[]`.

The running error line also updates live as you check/uncheck against the range, so the user sees when
the selection becomes valid. In [`ViewOnly`](#view-only-mode) mode, Enter always confirms (there is
nothing to validate).

---

## Initial selection & history

- [`Default(values)`](methods.md#default) pre-checks every matching item; provide
  [`DefaultMatchBy`](methods.md#defaultmatchby) for records/classes so the right rows are located.
- With [`EnableHistory`](methods.md#enablehistory), confirmed selections are stored on disk;
  [`UseDefaultHistory`](methods.md#usedefaulthistory) (or `Default(..., useDefaultHistory: true)`)
  restores the last set on the next run.
- History options (`MinPrefixLength`, `MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`) match
  the [Input history options](../input/methods.md#enablehistory).

---

## View-only mode

[`ViewOnly()`](methods.md#viewonly) renders the list for display only:

- Arrow keys still scroll, but checkboxes cannot be changed and F2/F3 are inactive.
- Combine with `Default` or `AddItems(..., ischecked: true)` to show a fixed checked set.
- Useful for showing a read-only snapshot inline with other prompts.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `MultiSelect<T>` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the list after confirm — the whole control is erased, not just the interactive part |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally
(`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **Aborted / empty results** carry `.Content` as an **empty array** — never `null`. Branch on
  `IsAborted`, and check `.Content.Length` to distinguish "confirmed nothing" from a real selection.
- **Custom types need equality.** Without [`DefaultMatchBy`](methods.md#defaultmatchby), `Default` on
  records/classes may not check the intended rows.
- **Async callbacks block the UI thread** — keep validators, selectors, and description callbacks fast.
- **Long labels** are truncated to a single line with `…`; the full item is still returned in
  `.Content`.
- **Range vs. predicate** are independent: `Range` limits *how many* are checked (at Enter), while
  `PredicateChecked` limits *which* items may be checked (at Space).

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
- [Select → Operations](../select/operations.md) — the single-choice sibling's runtime behavior
