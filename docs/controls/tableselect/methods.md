<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **TableSelect&lt;T&gt; — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [TableSelect — Operations →](operations.md)

---

Every fluent method on `ITableSelectControl<T>`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.TableSelect<T>(string prompt = "", string? description = null)`,
> which returns `ITableSelectControl<T>`.

**Quick jump:**
[AddColumn](#addcolumn) ·
[AddItem](#additem) ·
[AddItems](#additems) ·
[Interaction](#interaction) ·
[InteractionAsync](#interactionasync) ·
[TextSelector](#textselector) ·
[TextSelectorAsync](#textselectorasync) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Filter](#filter) ·
[PageSize](#pagesize) ·
[LayoutMode](#layoutmode) ·
[HideElements](#hideelements) ·
[HorizontalScroll](#horizontalscroll) ·
[Default](#default) ·
[UseDefaultHistory](#usedefaulthistory) ·
[DefaultMatchBy](#defaultmatchby) ·
[PredicateSelected](#predicateselected) ·
[PredicateSelectedAsync](#predicateselectedasync) ·
[ViewOnly](#viewonly) ·
[EnableHistory](#enablehistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Defining columns

### `AddColumn`

```csharp
ITableSelectControl<T> AddColumn(
    string header,
    Func<T, object> selector,
    Func<object, string>? formatter = null,
    int? width = null,
    ColumnAlignment alignment = ColumnAlignment.Left,
    bool isFilterable = false)
```

Adds a column. Signature is **header-first**: the header text comes before the value selector.
At least one column must be added before [`Run`](#run).

| Parameter | Meaning |
|---|---|
| `header` | Column title. Cannot be `null`, empty, or whitespace. |
| `selector` | Extracts the cell value from a row. |
| `formatter` | Optional — converts the raw cell value to its display string. `null` uses `ToString()`. |
| `width` | Fixed column width in characters. `null` (default) auto-sizes from the header and cell values at `Run` time. Must be greater than zero when specified. |
| `alignment` | Cell content alignment — [`ColumnAlignment`](operations.md#column-alignment). Default `Left`. |
| `isFilterable` | When `true`, this column's cells participate in filter matching (see [`Filter`](#filter)). Default `false`. |

```csharp
PromptPlus.Controls.TableSelect<Product>("Select a product")
    .AddColumn("Id",       x => x.Id,        width: 4, alignment: ColumnAlignment.Right)
    .AddColumn("Name",     x => x.Name,      isFilterable: true)
    .AddColumn("Category", x => x.Category,  alignment: ColumnAlignment.Center)
    .AddColumn("Price",    x => x.Price,     v => $"$ {v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Run();
```

> Throws `ArgumentNullException` if `header` or `selector` is `null`, `ArgumentException` if `header`
> is empty/whitespace, and `ArgumentOutOfRangeException` if `width` is specified and not greater than zero.

---

## Adding rows

### `AddItem`

```csharp
ITableSelectControl<T> AddItem(T value, bool disable = false)
```

Adds a single row. Set `disable: true` to show it but make it non-selectable. At least one row must be
added before [`Run`](#run).

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItem(products[0])
    .AddItem(products[1], disable: true)   // visible but not selectable
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `AddItems`

```csharp
ITableSelectControl<T> AddItems(IEnumerable<T> values, bool disable = false)
```

Adds many rows at once. `disable: true` disables all of them.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();
```

> Throws `ArgumentNullException` if `values` is `null`.

---

## Loading from a source

### `Interaction`

```csharp
ITableSelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ITableSelectControl<T>> interactionAction)
```

Iterates a source collection and lets you add rows programmatically — useful for per-item logic such as
disabling rows conditionally.

```csharp
PromptPlus.Controls.TableSelect<Product>("Select an available product")
    .AddColumn("Name",      x => x.Name)
    .AddColumn("Available", x => x.Available ? "Yes" : "No", alignment: ColumnAlignment.Center, width: 10)
    .Interaction(products, (p, ctrl) => ctrl.AddItem(p, disable: !p.Available))
    .Run();
```

---

### `InteractionAsync`

```csharp
ITableSelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ITableSelectControl<T>, Task> interactionAction)
```

Asynchronous version of [`Interaction`](#interaction); each task is awaited synchronously.

---

## Answer text & description

### `TextSelector`

```csharp
ITableSelectControl<T> TextSelector(Func<T, string> value)
```

Sets the **answer text** shown after the control completes. Without it (and without
[`TextSelectorAsync`](#textselectorasync)), the confirmed row's `ToString()` is used.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name",  x => x.Name)
    .AddColumn("Price", x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .TextSelector(item => $"{item.Name} (${item.Price:N2})")
    .Run();
```

---

### `TextSelectorAsync`

```csharp
ITableSelectControl<T> TextSelectorAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`TextSelector`](#textselector).

---

### `ChangeDescription`

```csharp
ITableSelectControl<T> ChangeDescription(Func<T, string> value)
```

Recomputes the description line from the **currently highlighted row** as the user navigates.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .ChangeDescription(item => $"Category: {item.Category} | Origin: {item.Origin}")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
ITableSelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

---

## Filtering & paging

### `Filter`

```csharp
ITableSelectControl<T> Filter(FilterMode value, FilterTableMode filterby = FilterTableMode.Answer)
```

Enables live filtering as the user types. Default is `FilterMode.Disabled` with `FilterTableMode.Answer`.

| `FilterMode` | Behavior |
|---|---|
| `Disabled` | No filtering (default) |
| `Contains` | Match rows containing the typed text |
| `StartsWith` | Match rows starting with the typed text |

| `FilterTableMode` | What the filter matches against |
|---|---|
| `Answer` | The answer text (result of [`TextSelector`](#textselector)) |
| `ColumnFilters` | The concatenated text of every column declared with `isFilterable: true` |

```csharp
PromptPlus.Controls.TableSelect<Product>("Search product")
    .AddColumn("Name",     x => x.Name,     isFilterable: true)
    .AddColumn("Category", x => x.Category, isFilterable: true)
    .AddColumn("Origin",   x => x.Origin,   isFilterable: true)
    .AddItems(products)
    .Filter(FilterMode.Contains, FilterTableMode.ColumnFilters)
    .Run();
```

---

### `PageSize`

```csharp
ITableSelectControl<T> PageSize(byte value)
```

Maximum rows per page (valid range 0–255). `0` (default) auto-computes from the terminal height.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product").AddColumn("Name", x => x.Name).AddItems(products).PageSize(8).Run();
```

---

## Layout & borders

### `LayoutMode`

```csharp
ITableSelectControl<T> LayoutMode(TableLayoutMode mode)
```

Sets the box-drawing character set for borders. Default `SingleBox`.

| `TableLayoutMode` | Renders |
|---|---|
| `SingleBox` | Single Unicode box-drawing lines (default) |
| `DoubleBox` | Double Unicode box-drawing lines |
| `SingleASCII` | Single lines using plain ASCII characters |
| `DoubleASCII` | Double lines using plain ASCII characters |
| `None` | No border characters at all |

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .LayoutMode(TableLayoutMode.DoubleBox)
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();
```

---

### `HideElements`

```csharp
ITableSelectControl<T> HideElements(HideTable borders)
```

Hides one or more border regions. `HideTable` is a `[Flags]` enum — combine with `|`. Default
`HideTable.None` (everything visible).

| `HideTable` | Hides |
|---|---|
| `None` | Nothing — show all elements (default) |
| `RowSeparator` | Horizontal separators between data rows |
| `Header` | The entire header row **and** the header/data separator line |
| `ColumnSeparator` | Vertical separators between columns |
| `OuterBorder` | The outer frame (top, bottom, left, right edges) |

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .HideElements(HideTable.OuterBorder | HideTable.RowSeparator)
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();
```

---

### `HorizontalScroll`

```csharp
ITableSelectControl<T> HorizontalScroll(HorizontalScrollMode mode)
```

Controls how columns scroll when they do not all fit on screen. Default `Full`. When every column fits
within the console width, horizontal scrolling is inactive and the column keys (Tab / Shift+Tab) are ignored.

| `HorizontalScrollMode` | Behavior |
|---|---|
| `Full` | Moves the visible viewport as a full column window |
| `Column` | Scrolls by focusing columns one at a time |

```csharp
PromptPlus.Controls.TableSelect<Employee>("Employee")
    .HorizontalScroll(HorizontalScrollMode.Column)
    // ... 12 columns ...
    .AddItems(employees)
    .Run();
```

---

## Initial row & equality

### `Default`

```csharp
ITableSelectControl<T> Default(T value, bool useDefaultHistory = true)
```

Pre-selects `value` as the initial cursor position, matched with [`DefaultMatchBy`](#defaultmatchby)
(default: `EqualityComparer<T>.Default`). Disabled rows and rows rejected by a selection predicate are
not pre-selected. When `useDefaultHistory` is `true` and [history](#enablehistory) is enabled, the most
recent history entry overrides this value.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default(products[0])
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `UseDefaultHistory`

```csharp
ITableSelectControl<T> UseDefaultHistory()
```

Sets the initial cursor to the most recent history entry, clearing any value set by [`Default`](#default).
Has no effect unless [`EnableHistory`](#enablehistory) is set.

---

### `DefaultMatchBy`

```csharp
ITableSelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
```

Custom equality used to locate the [`Default`](#default) row and match history values — essential for
records/classes where reference equality is not meaningful.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default(new Product(4, "New York", "any", 0m))
    .Run();
```

> Throws `ArgumentNullException` if `comparer` is `null`.

---

## Validating the confirmed row

Validation runs on **Enter**. On failure the table stays open and shows an error line.

### `PredicateSelected`

```csharp
ITableSelectControl<T> PredicateSelected(Func<T, bool> validselect)
ITableSelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<T, bool>` | `true` = valid | Generic error on failure |
| `Func<T, (bool, string?)>` | `(isValid, message)` | Custom `message` on failure |

Setting a synchronous predicate replaces any previously registered asynchronous one.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name",     x => x.Name)
    .AddColumn("Category", x => x.Category)
    .AddItems(products)
    .PredicateSelected(p => p.Category is "Electronics" or "Peripherals"
        ? (true, null)
        : (false, $"Category '{p.Category}' is not allowed."))
    .Run();
```

> Throws `ArgumentNullException` if `validselect` is `null`.

---

### `PredicateSelectedAsync`

```csharp
ITableSelectControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect)
ITableSelectControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts; setting one replaces any previously registered synchronous predicate.

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Read-only display

### `ViewOnly`

```csharp
ITableSelectControl<T> ViewOnly(bool value = true)
```

Renders the table for browsing only — the user can navigate freely but cannot change the selection. On
**Enter** the control always returns the item highlighted at startup (set via [`Default`](#default) or the
first row), regardless of where the user browsed. Selection predicates and disabled-row restrictions are
not enforced in this mode. Default `false`.

```csharp
PromptPlus.Controls.TableSelect<Product>("Product catalogue (view only)", "Press Esc or Enter to exit")
    .AddColumn("Name",  x => x.Name)
    .AddColumn("Price", x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .PageSize(4)
    .ViewOnly()
    .Run();
```

---

## History

### `EnableHistory`

```csharp
ITableSelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists confirmed selections to `filename` and can pre-select the last one (via [`Default`](#default) or
[`UseDefaultHistory`](#usedefaulthistory)). The `IHistoryOptions` builder is identical to the one
documented for [Input → EnableHistory](../input/methods.md#enablehistory) (`MinPrefixLength`, `MaxItems`,
`ExpirationTime`, `FilterType`, `PageSize`).

```csharp
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Id",   x => x.Id, width: 4, alignment: ColumnAlignment.Right)
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .EnableHistory("table-product-history")
    .UseDefaultHistory()
    .Run();
```

> Throws `ArgumentNullException` if `filename` is `null`, `ArgumentException` if it is empty/whitespace.

---

## Appearance & behavior

### `Styles`

```csharp
ITableSelectControl<T> Styles(TableSelectStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Styles(TableSelectStyles.SelectedCell, new Style(Color.Black, Color.Cyan))
    .Run();
```

---

### `Options`

```csharp
ITableSelectControl<T> Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<TableSelectResult<T>> Run(CancellationToken token = default)
```

Renders the table and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<TableSelectResult<T>>`](../../architecture.md#resultpromptt); the `TableSelectResult<T>` exposes
`.Value`, `.RowIndex`, `.ColumnIndex`, and deconstructs into `(value, row, column)`.

```csharp
var result = PromptPlus.Controls.TableSelect<Product>("Product")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine(result.Content.Value.Name);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `TableSelectStyles` regions
- [Index](index.md) — overview and method map
- [TableMultiSelect → Methods](../tablemultiselect/methods.md) — the multiple-row sibling
