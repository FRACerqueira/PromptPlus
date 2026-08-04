<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ChartBar — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ChartBar — Styles →](styles.md)

---

How the `ChartBar` control behaves while it is running: keyboard, adding items, ordering, legends,
layouts, paging, and validation.

---

## Anatomy of the control

```
Select item                              ← prompt
Selected: North (34.3%)                  ← description (optional / dynamic)
Sales by Region                          ← title (ChartTitle)
› North  ████████████████████  120  34%  ← highlighted bar + value + percent
  South  █████████████          80  23%
  East   ███████████████        95  27%
  West   ██████████████████    110  31%
North 120 (34%)  South 80 (23%) …        ← legend (when ShowLegends)
Page 1/2                                 ← pagination (when PageSize > 0)
Enter: confirm  Esc: cancel  F2/F3/F4    ← tooltip
```

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

| Key | Action |
|---|---|
| `↑` / `↓` | Move the highlight between bars (in `Stacked` layout, `←`/`→` do the same) |
| `Page Up` / `Page Down` | Jump one page |
| `Ctrl+Home` / `Ctrl+End` | First / last bar (plain `Home`/`End` do **not** do this) |
| `F2` | Switch layout `Standard` ⇄ `Stacked` (unless disabled) |
| `F3` | Toggle the legend on / off — only works once [`ShowLegends()`](methods.md#showlegends) has been called at least once; inert otherwise |
| `F4` | Cycle the sort order |
| `Enter` | Confirm the highlighted bar (runs validation) |
| `Esc` | Abort → `IsAborted == true` |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

---

## Adding items

- [`AddItem(label, value)`](methods.md#additem) adds one bar. There is no `AddItems`; add many with a
  loop or with [`Interaction`](methods.md#interaction).
- Bar colors are optional — pass a `Color` to `AddItem`, or omit it and the control assigns colors in
  a rotating sequence.
- Percentages are computed for you: each bar's `Percent` is its share of the total of all values, and
  is exposed on the returned [`ChartItem`](index.md#the-chartitem-type).
- Values are formatted using [`Culture`](methods.md#culture) and
  [`FractionalDigits`](methods.md#fractionaldigits) (default 2 digits).

---

## Layouts

- [`Layout`](methods.md#layout) sets the starting layout: `Standard` (one bar per item) or `Stacked`
  (all items in one continuous bar).
- Press **F2** at runtime to switch, unless [`EnableLayoutSwitcher(false)`](methods.md#enablelayoutswitcher)
  is set.
- Switching to `Stacked` requires enough console width to render every item; if the console is too
  narrow the switch is **silently prevented** to avoid broken rendering.

---

## Ordering

- [`OrderBy`](methods.md#orderby) sets the initial order: `None`, `Highest`, `Smallest`, `LabelAsc`,
  or `LabelDesc`.
- `ChartBarOrder.None` is a **no-op** — it keeps the original insertion order (the sequential
  auto-id order); it never reshuffles the bars.
- Press **F4** at runtime to cycle through the orders, unless
  [`EnableOrderingSwitcher(false)`](methods.md#enableorderingswitcher) is set.
- The active order is shown by the ordering indicator (styled with
  [`ChartOrder`](styles.md#the-chartbarstyles-regions)).

---

## Legends

- [`ShowLegends()`](methods.md#showlegends) adds a legend after the chart listing each item with its
  value and percentage. It is **off by default**.
- Press **F3** at runtime to toggle the legend on or off — but only if `ShowLegends()` was called at
  least once before `Run()`. If you never call it, **F3 does nothing**; there's no way to turn
  legends on for the first time from the keyboard.

---

## Hiding elements & paging

- [`HideElements`](methods.md#hideelements) removes the title, values, and/or percentages from the
  chart. `HideChart` is a `[Flags]` enum, so combine with `|`.
- [`PageSize`](methods.md#pagesize) limits how many bars are shown at once; `0` (default) does **not**
  disable paging — it auto-computes a page size that fits the terminal height. Page Up / Page Down
  move between pages and the page indicator appears whenever there's more than one page.

---

## Confirmation & validation flow

Pressing **Enter** on the highlighted bar:

1. Validation runs — [`PredicateSelected`](methods.md#predicateselected) /
   [`PredicateSelectedAsync`](methods.md#predicateselectedasync), if configured.
2. **Valid** → the control closes and returns the highlighted
   [`ChartItem`](index.md#the-chartitem-type).
   **Invalid** → the chart stays open and shows the error line.

The predicate receives the `ChartItem`, so you can validate on `Value`, `Label`, `Percent`, or `Id`:

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("Valid", 80).AddItem("Invalid", 30)
    .PredicateSelected(item => item.Value < 50
        ? (false, "Value must be >= 50")
        : (true, null))
    .Run();
```

---

## Reading the result

`Run()` returns `ResultPrompt<ChartItem?>` — the item highlighted **at the moment Enter was pressed**,
not a sum or aggregate.

```csharp
var result = PromptPlus.Controls.ChartBar("Select item")
    .AddItem("A", 40).AddItem("B", 85)
    .Run();

if (result.IsAborted)
    PromptPlus.Console.WriteLine("Canceled.");
else if (result.Content is not null)
    PromptPlus.Console.WriteLine($"{result.Content.Label} = {result.Content.Value} ({result.Content.Percent:F2}%)");
```

---

## Dynamic description

[`ChangeDescription`](methods.md#changedescription) recomputes the description from the currently
highlighted item as the user navigates — handy for showing the live label, value, or percentage.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `ChartBar` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm a bar |
| `HideAfterFinish(true)` | Erases the chart after confirm; only the answer remains |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

---

## Edge cases & gotchas

- **Aborted results** carry `.Content == null`. Always branch on `IsAborted` (and null-check
  `.Content`) before use.
- **Stacked layout may not switch** on narrow terminals — the F2 toggle is silently ignored when
  width is insufficient.
- **Async callbacks block the UI thread** — keep validators and description callbacks fast.
- **Width has a floor** — [`Width`](methods.md#width) below 10 throws; keep the chart at least 10
  characters wide.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
- [ChartBar widget](../../widgets.md#chartbar) — the read-only display sibling
