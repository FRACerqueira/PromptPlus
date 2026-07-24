<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTable&lt;T&gt;**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [MultiTable — Methods ?](methods.md)

---

> A paginated, filterable grid with named columns and a checkbox per row — the user checks **any number**
> of rows and confirms with **Enter**.

`MultiTable<T>` is the multiple-selection sibling of [`Table<T>`](../table/index.md). It renders any
collection as a bordered table, but each row carries a checkbox: the user toggles rows with **Space**,
can be constrained to a minimum/maximum count, and gets back an array of the checked rows. Everything the
single-row table offers — columns, filtering, horizontal scrolling, history, view-only — applies here too.

> ?? Only need **one** row? Use the [**Table**](../table/index.md) control — same grid model, single choice.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, checkboxes, filtering, ranges, scrolling, history, view-only |
| [Styles](styles.md) | The `MultiTableStyles` regions and how to recolor them |

---

## When to use it

| Use `MultiTable<T>` when… | Consider instead… |
|---|---|
| The data is tabular and the user checks several rows | — |
| The user picks exactly one row | [Table](../table/index.md) |
| Each item is a single label (no columns) | [Select](../select/index.md) |
| The data is hierarchical | [Tree](../tree/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

record Product(int Id, string Name, string Category, decimal Price);

var products = new[]
{
    new Product(1, "Notebook Pro",   "Electronics", 1299.99m),
    new Product(2, "Wireless Mouse", "Peripherals",   29.90m),
    new Product(3, "4K Monitor",     "Electronics",  599.00m),
};

var result = PromptPlus.Controls
    .MultiTable<Product>("Select products", "Check all desired items")
    .AddColumn("Id",       x => x.Id, width: 4, alignment: ColumnAlignment.Right)
    .AddColumn("Name",     x => x.Name)
    .AddColumn("Category", x => x.Category)
    .AddColumn("Price",    x => x.Price, v => $"$ {v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Run();

if (!result.IsAborted)
    foreach (var product in result.Content)          // iterate yields T directly
        PromptPlus.Console.WriteLine(product.Name);
```

- `MultiTable<Product>(...)` creates the grid. The type argument `T` is the **row** type.
- `.AddColumn(...)` is **header-first**: header text, then a `Func<T, object>` that extracts the cell
  value, then optional formatter / width / alignment / filterable flag.
- `.AddItems([...])` fills the rows; `.AddItem(x, ischecked, disable)` adds one at a time.
- `.Run()` renders the table and blocks until **Enter** (confirm) or **Esc** (abort), returning
  [`ResultPrompt<T[]>`](../../architecture.md#resultpromptt) — `.Content` is an **array of the checked rows**.

> At least one column **and** one row must be configured before `Run`, otherwise a `ValidationException` is thrown.

---

## A richer example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .MultiTable<Product>("Select 2 to 4 products", "Available items pre-checked")
    .AddColumn("Name",      x => x.Name)
    .AddColumn("Category",  x => x.Category)
    .AddColumn("Available", x => x.Available ? "Yes" : "No", alignment: ColumnAlignment.Center, width: 10)
    .Interaction(products, (p, ctrl) => ctrl.AddItem(p, ischecked: p.Available, disable: !p.Available))
    .Range(minvalue: 2, maxvalue: 4)                 // must end with 2–4 checked
    .TextSelector(item => item.Name)
    .Run();
```

This combines **pre-checked rows**, a **disabled row**, and a **range constraint** — see
[Operations](operations.md) for how they behave together.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Define columns | `AddColumn` |
| Add rows | `AddItem`, `AddItems` |
| Load from a source | `Interaction`, `InteractionAsync` |
| Answer text & description | `TextSelector`, `TextSelectorAsync`, `ChangeDescription`, `ChangeDescriptionAsync` |
| Filtering & paging | `Filter`, `PageSize` |
| Layout & borders | `LayoutMode`, `HideElements`, `HorizontalScroll` |
| Checked set & count | `Default`, `Range`, `UseDefaultHistory`, `DefaultMatchBy` |
| Restrict what can be checked | `PredicateChecked`, `PredicateCheckedAsync` |
| Read-only display | `ViewOnly` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`MultiTable<T>` returns `ResultPrompt<T[]>` — an array of the checked rows.

| Member | Meaning |
|---|---|
| `.Content` | The `T[]` of checked rows (empty array when nothing is checked) |
| `.IsAborted` | `true` when the user pressed Esc |

Iterating `.Content` yields each `T` **directly** — there is no `.Value` wrapper (unlike
[`Table<T>`](../table/index.md), which returns a single `TableResult<T>`).

```csharp
var result = PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"Checked {result.Content.Length}: " +
        string.Join(", ", result.Content.Select(p => p.Name)));
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, checkboxes, ranges, filtering, scrolling
- [Styles](styles.md) — recolor the table regions
- [Table](../table/index.md) — single-row sibling
- [Select](../select/index.md) · [Tree](../tree/index.md) — single-column and hierarchical pickers
