<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **TableSelect&lt;T&gt; — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [TableSelect — Styles →](styles.md)

---

How the `TableSelect<T>` control behaves while it is running: keyboard, columns, filtering, horizontal
scrolling, paging, validation, history, and view-only mode.

---

## Anatomy of the control

```
Select a product                         ← prompt
Category: Electronics                     ← description (optional / dynamic)
┌──────┬───────────────┬────────────┐
│  Id  │ Name          │   Price    │    ← header row
├──────┼───────────────┼────────────┤
│    1 │ Notebook Pro  │  $ 1299.99 │    ← focused row (SelectedCell)
│    2 │ Wireless Mouse│  $   29.90 │
└──────┴───────────────┴────────────┘
Filter: note                              ← live filter text (when filtering)
Page 1/2                                  ← pagination
Enter: confirm  Esc: cancel               ← tooltip
```

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down (rows) |
| `Page Up` / `Page Down` | Jump one page |
| `Home` / `End` | First / last row |
| `Tab` / `Shift+Tab` | Move the focused **column** (only when the table scrolls horizontally) |
| `Enter` | Confirm the focused row (runs validation) |
| `Esc` | Abort → `IsAborted == true` |
| Any printable character | Type to filter (when [`Filter`](methods.md#filter) is not `Disabled`) |
| `Backspace` | Edit the filter text |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

Disabled rows are skipped automatically as you move.

---

## Columns

- [`AddColumn`](methods.md#addcolumn) is **header-first**: header text, then a `Func<T, object>` selector,
  then optional formatter, width, alignment, and filterable flag.
- **Width** is either fixed (`width: n`) or auto-sized from the header and all cell values at `Run` time.
- Content that exceeds the column width is truncated with an ellipsis; the full row is still returned in
  `.Content.Value`.

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

When all columns fit, scrolling is inactive and the column keys are ignored. The focused column's index
is returned as `.Content.ColumnIndex`.

---

## Layout & border regions

- [`LayoutMode`](methods.md#layoutmode) picks the box-drawing character set: `SingleBox` (default),
  `DoubleBox`, `SingleASCII`, `DoubleASCII`, or `None` (no borders).
- [`HideElements`](methods.md#hideelements) hides individual regions — `RowSeparator`, `Header`,
  `ColumnSeparator`, `OuterBorder` — and combines with `|` to strip several at once. `HideTable.None`
  (default) shows everything.

---

## Filtering

When [`Filter`](methods.md#filter) is `Contains` or `StartsWith`, typing narrows the visible rows in real time:

- The match is case-insensitive.
- **`FilterTableMode.Answer`** matches against the answer text ([`TextSelector`](methods.md#textselector)).
- **`FilterTableMode.ColumnFilters`** matches against the concatenated text of every column declared with
  `isFilterable: true`.
- Matching resets paging to the first page of results.
- Clearing the filter (Backspace) restores the full list.

`Disabled` (the default) turns typing off entirely — arrow keys only.

---

## Confirmation & validation flow

Pressing **Enter** on the focused row:

1. Validation runs — [`PredicateSelected`](methods.md#predicateselected) /
   [`PredicateSelectedAsync`](methods.md#predicateselectedasync), if configured.
2. **Valid** → the control closes and returns the row inside a
   [`TableSelectResult<T>`](methods.md#run).
   **Invalid** → the table stays open and shows the error line.

Disabled rows cannot be focused for confirmation, so validation typically guards *business* rules rather
than availability.

---

## Initial selection & history

- [`Default(value)`](methods.md#default) pre-selects a row; provide
  [`DefaultMatchBy`](methods.md#defaultmatchby) for records/classes so the right row is located.
- With [`EnableHistory`](methods.md#enablehistory), confirmed selections are stored on disk;
  [`UseDefaultHistory`](methods.md#usedefaulthistory) (or `Default(..., useDefaultHistory: true)`)
  restores the last one on the next run.
- History options (`MinPrefixLength`, `MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`) match the
  [Input history options](../input/methods.md#enablehistory).

---

## View-only mode

[`ViewOnly()`](methods.md#viewonly) renders the table for display only:

- Arrow and column keys still navigate, but the selection cannot change.
- On **Enter** the control returns the item highlighted at startup (from `Default` or the first row),
  regardless of where the user browsed.
- Selection predicates and disabled-row restrictions are not enforced.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `TableSelect<T>` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must choose |
| `HideAfterFinish(true)` | Erases the table after confirm; only the answer remains |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` can be set per control ([`PageSize`](methods.md#pagesize)) or globally (`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **Configuration is required.** `Run` throws `ValidationException` unless at least one column **and** one
  row have been added.
- **Aborted results** carry `.Content == default` (a `TableSelectResult<T>` with `default(T)`). Always branch on
  `IsAborted`.
- **Custom types need equality.** Without [`DefaultMatchBy`](methods.md#defaultmatchby), `Default` on a
  record/class may not match the intended row.
- **Async callbacks block the UI thread** — keep validators, selectors, and description callbacks fast.
- **Long cells** are truncated with `…`; the full row is still returned in `.Content.Value`.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
- [TableMultiSelect → Operations](../tablemultiselect/operations.md) — checkbox behavior for the multiple-row sibling
