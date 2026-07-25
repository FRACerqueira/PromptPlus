<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Table&lt;T&gt;**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Table — Methods →](methods.md)

---

> A paginated, filterable grid with named columns where the user picks **one row** and confirms with **Enter**.

`Table<T>` renders any collection as a bordered table. You declare the columns (header, value selector,
optional formatter, width, and alignment), feed it rows of your own type, and the user navigates rows —
and, on wide tables, columns — with the keyboard. It can filter as the user types, scroll horizontally
when the columns overflow, validate the highlighted row before returning it, and persist the last choice.

> ☑️ Need to pick **several** rows at once? Use the [**MultiTable**](../multitable/index.md)
> control — same grid model, with a checkbox per row.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, columns, filtering, scrolling, validation, history, view-only |
| [Styles](styles.md) | The `TableStyles` regions and how to recolor them |

---

## When to use it

| Use `Table<T>` when… | Consider instead… |
|---|---|
| The data is tabular and the user picks one row | — |
| The user may check several rows | [MultiTable](../multitable/index.md) |
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
    .Table<Product>("Select a product")
    .AddColumn("Id",       x => x.Id, width: 4, alignment: ColumnAlignment.Right)
    .AddColumn("Name",     x => x.Name)
    .AddColumn("Category", x => x.Category)
    .AddColumn("Price",    x => x.Price, v => $"$ {v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"You chose {result.Content.Value.Name}");
```

- `Table<Product>("Select a product")` creates the grid. The type argument `T` is the **row** type.
- `.AddColumn(...)` is **header-first**: header text, then a `Func<T, object>` that extracts the cell value,
  then optional formatter / width / alignment / filterable flag.
- `.AddItems([...])` fills the rows; `.AddItem(x)` adds one at a time.
- `.Run()` renders the table and blocks until **Enter** (confirm) or **Esc** (abort), returning
  [`ResultPrompt<TableResult<T>>`](../../architecture.md#resultpromptt).

> At least one column **and** one row must be configured before `Run`, otherwise a `ValidationException` is thrown.

---

## A richer example

```csharp
using PromptPlusLibrary;

var product = PromptPlus.Controls
    .Table<Product>("Select a product", "All column features in one table")
    .AddColumn("Id",       x => x.Id,       width: 4, alignment: ColumnAlignment.Right)
    .AddColumn("Name",     x => x.Name,     isFilterable: true)
    .AddColumn("Category", x => x.Category, alignment: ColumnAlignment.Center)
    .AddColumn("Price",    x => x.Price,    v => $"$ {v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Filter(FilterMode.Contains, FilterTableMode.Answer)   // live filtering as the user types
    .ChangeDescription(item => $"Category: {item.Category}")
    .PageSize(8)                                            // 8 rows visible at a time
    .Run();
```

This combines **columns**, **filtering**, a **dynamic description**, and **paging** — see
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
| Initial row | `Default`, `UseDefaultHistory`, `DefaultMatchBy` |
| Validate on confirm | `PredicateSelected`, `PredicateSelectedAsync` |
| Read-only display | `ViewOnly` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Table<T>` returns `ResultPrompt<TableResult<T>>`. The `TableResult<T>` carries the confirmed row and
its table coordinates.

| Member | Meaning |
|---|---|
| `.Content` | The [`TableResult<T>`](methods.md#run) (default when aborted) |
| `.Content.Value` | The selected row `T` |
| `.Content.RowIndex` | Zero-based index of the selected row |
| `.Content.ColumnIndex` | Zero-based index of the focused column |
| `.IsAborted` | `true` when the user pressed Esc |

`TableResult<T>` also **deconstructs** into `(value, row, column)`:

```csharp
var result = PromptPlus.Controls.Table<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();

if (!result.IsAborted)
{
    var (row, rowIndex, columnIndex) = result.Content;
    PromptPlus.Console.WriteLine($"{row.Name} at row {rowIndex}, column {columnIndex}");
}
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, columns, filtering, scrolling, validation
- [Styles](styles.md) — recolor the table regions
- [MultiTable](../multitable/index.md) — multiple-row sibling
- [Select](../select/index.md) · [Tree](../tree/index.md) — single-column and hierarchical pickers
