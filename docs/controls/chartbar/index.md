<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ChartBar**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ChartBar — Methods →](methods.md)

---

> An interactive horizontal bar chart the user can navigate, re-sort, and re-layout — picking **one**
> bar and confirming with **Enter**.

`ChartBar` renders a set of labeled numeric items as horizontal bars with values, percentages, and an
optional legend. Unlike a static chart, it is a live control: arrow keys move a highlight between
bars, hotkeys switch the layout and sort order, and pressing **Enter** returns the highlighted
[`ChartItem`](#return-value).

> ☑️ Just need to **display** a bar chart with no interaction? Use the read-only
> [**ChartBar widget**](../../widgets.md#chartbar) — same visuals, rendered with `.Show()` and no
> selection.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, ordering, legends, layouts, validation, paging |
| [Styles](styles.md) | The `ChartBarStyles` regions and how to recolor them |

---

## When to use it

| Use `ChartBar` when… | Consider instead… |
|---|---|
| The user compares numeric values and picks one | — |
| You only want to display a chart, no picking | [ChartBar widget](../../widgets.md#chartbar) |
| The choices are plain text with no magnitude | [Select](../select/index.md) |
| The data is tabular (multiple columns) | [TableSelect](../tableselect/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .ChartBar("Select item")
    .AddItem("Item A", 40)
    .AddItem("Item B", 85)
    .AddItem("Item C", 60)
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"You chose {result.Content?.Label}");
```

- `ChartBar("Select item")` creates the control; the prompt is shown above the chart.
- `.AddItem(label, value)` adds one bar; percentages are computed for you from the totals.
- `.Run()` renders the chart and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<ChartItem?>`](../../architecture.md#resultpromptt).

---

## A richer example

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color lives here

var result = PromptPlus.Controls
    .ChartBar("Select item")
    .Title("Sales by Region", TextAlignment.Center)
    .AddItem("North", 120, Color.Green)
    .AddItem("South", 80)
    .AddItem("East",  95)
    .AddItem("West",  110)
    .OrderBy(ChartBarOrder.Highest)   // sort bars by value, highest first
    .ShowLegends()                    // add the value + percentage legend
    .Run();
```

This combines a **title**, a **custom bar color**, an initial **sort order**, and the **legend** —
see [Operations](operations.md) for how these behave together at runtime.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Add items | `AddItem`, `Interaction` |
| Chart layout & bars | `Layout`, `BarType`, `Width`, `Title` |
| Values & labels | `Culture`, `FractionalDigits`, `MaxLengthLabel` |
| Ordering & legend | `OrderBy`, `ShowLegends` |
| Hide / page elements | `HideElements`, `PageSize` |
| Runtime switchers | `EnableLayoutSwitcher`, `EnableOrderingSwitcher` |
| Validate on confirm | `PredicateSelected`, `PredicateSelectedAsync` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`ChartBar` returns `ResultPrompt<ChartItem?>` — the **highlighted item at the moment Enter was
pressed**. It is a single item, not a sum or an aggregate value.

| Member | Meaning |
|---|---|
| `.Content` | The highlighted [`ChartItem`](#the-chartitem-type) (`null` if aborted) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var result = PromptPlus.Controls
    .ChartBar("Select item")
    .AddItem("A", 40).AddItem("B", 85)
    .Run();

if (!result.IsAborted && result.Content is not null)
    PromptPlus.Console.WriteLine($"{result.Content.Label} = {result.Content.Value}");
```

### The `ChartItem` type

`ChartItem` is a sealed class describing one bar:

| Member | Type | Meaning |
|---|---|---|
| `Id` | `string` | The id you passed to `AddItem` (or an auto id) |
| `Label` | `string` | The display label |
| `Value` | `double` | The numeric value |
| `Color` | `Color?` | The bar color (auto-assigned if you did not set one) |
| `Percent` | `double` | The item's share of the total, computed by the control |
| `StyleBar` | `Style?` | The style used to paint this bar |

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, ordering, legends, layouts, validation
- [Styles](styles.md) — recolor the chart regions
- [ChartBar widget](../../widgets.md#chartbar) — the read-only `.Show()` sibling
- [Select](../select/index.md) · [TableSelect](../tableselect/index.md) — text and tabular pickers
