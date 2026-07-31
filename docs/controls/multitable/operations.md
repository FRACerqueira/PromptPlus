<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTable&lt;T&gt; — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiTable — Styles →](styles.md)

---

How the `MultiTable<T>` control behaves while it is running: keyboard, checkboxes, ranges, filtering,
horizontal scrolling, paging, history, and view-only mode.

---

## Anatomy of the control

```
Select products: Notebook Pro            ← prompt + live answer (follows the cursor + column)
Category: Electronics                     ← description (optional / dynamic)
+---------------------------------------+
|   |  Id  | Name          |   Price    |  ← header row
+---+------+---------------+------------+
| x |    1 | Notebook Pro  |  $ 1299.99 |  ← focused, checked row
|   |    2 | Wireless Mouse|  $   29.90 |  ← unchecked row
+---------------------------------------+
Filter: note                              ← live filter text (when filtering)
Page 1/2                                  ← pagination
Space: check  Enter: confirm  Esc: cancel ← tooltip
```

The answer line updates as you navigate: without a [`TextSelector`](methods.md#textselector) it
shows the value of the row under the cursor in whichever column is currently Tab-focused (`Tab`/
`Shift+Tab` change it); with a `TextSelector`, that fixed text is shown instead, regardless of
column. Once confirmed (**Enter**), the final answer shown is the checked-rows summary
(comma-joined), resolved the same way per row.

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down (rows) |
| `Page Up` / `Page Down` | Jump one page |
| `Space` | Toggle the checkbox on the focused row |
| `Tab` / `Shift+Tab` | Move the focused **column** (only when the table scrolls horizontally) |
| `Enter` | Confirm the checked set (enforces [`Range`](methods.md#range)) |
| `Esc` | Abort → `IsAborted == true` |
| Any printable character | Type to filter (when [`Filter`](methods.md#filter) is not `Disabled`) |
| `Backspace` | Edit the filter text |
| `F2` | Toggle all rows on / off (respects the check predicate) |
| `F3` | Show only the checked rows / restore the full list |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

Disabled rows are skipped and cannot be toggled.

> The exact physical keys for toggle-all / filter-selected are configurable — the tooltip line always
> shows the active bindings. See [Keyboard Bindings](../../keyboard-bindings.md).

---

## Checking rows

- **Space** toggles the focused row. A row can only be checked if the
  [check predicate](methods.md#predicatechecked) allows it; otherwise the row stays unchecked and an
  error line is shown.
- [`AddItem`](methods.md#additem) / [`AddItems`](methods.md#additems) can start rows checked with
  `ischecked: true`.
- [`Default(values)`](methods.md#default) pre-checks matching rows; its values take precedence over the
  per-item `ischecked` flag.
- **F2** toggles every eligible row on or off at once; **F3** narrows the view to the checked rows and
  back.

---

## Range constraints

[`Range(min, max?)`](methods.md#range) is enforced on **Enter**:

- If fewer than `min` rows are checked, the control stays open with a "minimum" error.
- If more than `max` rows are checked, it stays open with a "maximum" error.
- `max` of `null` means unlimited.

---

## Columns

- [`AddColumn`](methods.md#addcolumn) is **header-first**: header text, then a `Func<T, object>` selector,
  then optional formatter, width, alignment, and filterable flag.
- **Width** is either fixed (`width: n`) or auto-sized from the header and all cell values at `Run` time.
- Content that exceeds the column width is truncated with an ellipsis.

### Column alignment

[`ColumnAlignment`](methods.md#addcolumn) sets per-column horizontal alignment:

| Value | Effect |
|---|---|
| `Left` | Align cell content to the left (default) |
| `Right` | Align cell content to the right (typical for numbers) |
| `Center` | Center cell content |

---

## Horizontal scrolling

When the columns do not all fit within the console width, the table scrolls horizontally and
`Tab` / `Shift+Tab` move the focused column. [`HorizontalScroll`](methods.md#horizontalscroll) chooses how:

- **`Full`** (default) — shifts the visible viewport as a full column window.
- **`Column`** — focuses one column at a time.

When all columns fit, scrolling is inactive and the column keys are ignored.

---

## Layout & border regions

- [`LayoutMode`](methods.md#layoutmode) picks the box-drawing character set: `SingleBox` (default),
  `DoubleBox`, `SingleASCII`, `DoubleASCII`, or `None`.
- [`HideElements`](methods.md#hideelements) hides individual regions — `RowSeparator`, `Header`,
  `ColumnSeparator`, `OuterBorder` — and combines with `|`. `HideTable.None` (default) shows everything.

---

## Filtering

When [`Filter`](methods.md#filter) is `Contains` or `StartsWith`, typing narrows the visible rows in real time:

- The match is case-insensitive.
- **`FilterTableMode.Answer`** matches against the answer text ([`TextSelector`](methods.md#textselector)).
- **`FilterTableMode.ColumnFilters`** matches against the concatenated text of every column declared with
  `isFilterable: true`.
- Rows already checked keep their checked state while filtered in or out; the returned array reflects the
  full checked set regardless of the current filter.
- Clearing the filter (Backspace) restores the full list.

`Disabled` (the default) turns typing off entirely — arrow keys only.

---

## Initial checked set & history

- [`Default(values)`](methods.md#default) pre-checks rows; provide
  [`DefaultMatchBy`](methods.md#defaultmatchby) for records/classes so the right rows are located.
- With [`EnableHistory`](methods.md#enablehistory), the checked array is stored on disk as JSON;
  [`UseDefaultHistory`](methods.md#usedefaulthistory) restores it on the next run.
- History options (`MinPrefixLength`, `MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`) match the
  [Input history options](../input/methods.md#enablehistory).

---

## View-only mode

[`ViewOnly()`](methods.md#viewonly) renders the table for display only:

- Arrow and column keys still navigate, but checkboxes cannot be toggled.
- Rows marked via [`Default`](methods.md#default) remain shown as checked (read-only).
- On **Enter** the pre-checked set is returned unchanged.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `MultiTable<T>` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the table after confirm; only the answer remains |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally (`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **Configuration is required.** `Run` throws `ValidationException` unless at least one column **and** one
  row have been added.
- **Nothing checked** returns an **empty array** (`.Content.Length == 0`), not `null` — this is distinct
  from an aborted result. Branch on `IsAborted` first.
- **No `.Value` wrapper.** Iterating `.Content` yields each `T` directly, unlike
  [`Table<T>`](../table/index.md) which returns a single `TableResult<T>`.
- **Custom types need equality.** Without [`DefaultMatchBy`](methods.md#defaultmatchby), `Default` on a
  record/class may not match the intended rows.
- **Async callbacks block the UI thread** — keep predicates, selectors, and description callbacks fast.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
- [Table → Operations](../table/operations.md) — the single-row sibling
