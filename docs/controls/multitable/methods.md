<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTable&lt;T&gt; — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [MultiTable — Operations ?](operations.md)

---

Every fluent method on `IMultiTableControl<T>`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.MultiTable<T>(string prompt = "", string? description = null)`,
> which returns `IMultiTableControl<T>`.

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
[Range](#range) ·
[UseDefaultHistory](#usedefaulthistory) ·
[DefaultMatchBy](#defaultmatchby) ·
[PredicateChecked](#predicatechecked) ·
[PredicateCheckedAsync](#predicatecheckedasync) ·
[ViewOnly](#viewonly) ·
[EnabledHistory](#enabledhistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Defining columns

### `AddColumn`

```csharp
IMultiTableControl<T> AddColumn(
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
| `width` | Fixed column width in characters. `null` (default) auto-sizes from the header and cell values. |
| `alignment` | Cell content alignment — [`ColumnAlignment`](operations.md#column-alignment). Default `Left`. |
| `isFilterable` | When `true`, this column's cells participate in filter matching (see [`Filter`](#filter)). Default `false`. |

```csharp
PromptPlus.Controls.MultiTable<Product>("Select products")
    .AddColumn("Id",       x => x.Id,        width: 4, alignment: ColumnAlignment.Right)
    .AddColumn("Name",     x => x.Name,      isFilterable: true)
    .AddColumn("Category", x => x.Category,  alignment: ColumnAlignment.Center)
    .AddColumn("Price",    x => x.Price,     v => $"$ {v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Run();
```

---

## Adding rows

### `AddItem`

```csharp
IMultiTableControl<T> AddItem(T value, bool ischecked = false, bool disable = false)
```

Adds a single row. `ischecked: true` starts the row checked; `disable: true` shows it but prevents
toggling. At least one row must be added before [`Run`](#run).

```csharp
PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItem(products[0], ischecked: true)
    .AddItem(products[1], disable: true)     // visible but cannot be toggled
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `AddItems`

```csharp
IMultiTableControl<T> AddItems(IEnumerable<T> values, bool ischecked = false, bool disable = false)
```

Adds many rows at once. `ischecked: true` pre-checks all of them; `disable: true` disables all of them.

```csharp
PromptPlus.Controls.MultiTable<Product>("Deselect products to exclude")
    .AddColumn("Name", x => x.Name)
    .AddItems(products, ischecked: true)     // all rows start checked
    .Range(minvalue: 1)
    .Run();
```

> Throws `ArgumentNullException` if `values` is `null`.

---

## Loading from a source

### `Interaction`

```csharp
IMultiTableControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, IMultiTableControl<T>> interactionAction)
```

Iterates a source collection and lets you add rows programmatically — useful for per-item logic such as
pre-checking or disabling rows conditionally.

```csharp
PromptPlus.Controls.MultiTable<Product>("Select products")
    .AddColumn("Name",      x => x.Name)
    .AddColumn("Available", x => x.Available ? "Yes" : "No", alignment: ColumnAlignment.Center, width: 10)
    .Interaction(products, (p, ctrl) => ctrl.AddItem(p, ischecked: p.Available, disable: !p.Available))
    .Run();
```

---

### `InteractionAsync`

```csharp
IMultiTableControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, IMultiTableControl<T>, Task> interactionAction)
```

Asynchronous version of [`Interaction`](#interaction); each task is awaited synchronously.

---

## Answer text & description

### `TextSelector`

```csharp
IMultiTableControl<T> TextSelector(Func<T, string> value)
```

Sets how each row is rendered as answer text (used by [`FilterTableMode.Answer`](#filter) and the
selected-items summary). Without it (and without [`TextSelectorAsync`](#textselectorasync)), `ToString()`
is used.

```csharp
PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .TextSelector(item => item.Name)
    .Run();
```

---

### `TextSelectorAsync`

```csharp
IMultiTableControl<T> TextSelectorAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`TextSelector`](#textselector).

---

### `ChangeDescription`

```csharp
IMultiTableControl<T> ChangeDescription(Func<T, string> value)
```

Recomputes the description line from the **currently focused row** as the user navigates.

```csharp
PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .ChangeDescription(item => $"Category: {item.Category} | Origin: {item.Origin}")
    .Run();
```

---

### `ChangeDescriptionAsync`

```csharp
IMultiTableControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

---

## Filtering & paging

### `Filter`

```csharp
IMultiTableControl<T> Filter(FilterMode value, FilterTableMode filterby = FilterTableMode.Answer)
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
PromptPlus.Controls.MultiTable<Product>("Filter products")
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
IMultiTableControl<T> PageSize(byte value)
```

Maximum rows per page (valid range 0–255). `0` (default) auto-computes from the terminal height.

```csharp
PromptPlus.Controls.MultiTable<Product>("Products").AddColumn("Name", x => x.Name).AddItems(products).PageSize(8).Run();
```

---

## Layout & borders

### `LayoutMode`

```csharp
IMultiTableControl<T> LayoutMode(TableLayoutMode mode)
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
PromptPlus.Controls.MultiTable<Product>("Products")
    .LayoutMode(TableLayoutMode.DoubleBox)
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();
```

---

### `HideElements`

```csharp
IMultiTableControl<T> HideElements(HideTable borders)
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
PromptPlus.Controls.MultiTable<Product>("Products")
    .HideElements(HideTable.OuterBorder | HideTable.RowSeparator)
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();
```

---

### `HorizontalScroll`

```csharp
IMultiTableControl<T> HorizontalScroll(HorizontalScrollMode mode)
```

Controls how columns scroll when they do not all fit on screen. Default `Full`. When every column fits,
horizontal scrolling is inactive and the column keys (Tab / Shift+Tab) are ignored.

| `HorizontalScrollMode` | Behavior |
|---|---|
| `Full` | Moves the visible viewport as a full column window |
| `Column` | Scrolls by focusing columns one at a time |

```csharp
PromptPlus.Controls.MultiTable<Employee>("Employees")
    .HorizontalScroll(HorizontalScrollMode.Column)
    // ... 12 columns ...
    .AddItems(employees)
    .Run();
```

---

## Checked set & count

### `Default`

```csharp
IMultiTableControl<T> Default(IEnumerable<T> values)
```

Pre-checks every matching row and positions the cursor on the first match. Matching uses
[`DefaultMatchBy`](#defaultmatchby) (default: `EqualityComparer<T>.Default`). Values in this list take
precedence — items are marked checked regardless of `ischecked` at [`AddItem`](#additem) time — and
disabled rows matching the list are also marked checked (read-only visual). Has no effect when `values`
is empty.

```csharp
PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default([products[0], products[2]])
    .Run();
```

---

### `Range`

```csharp
IMultiTableControl<T> Range(int minvalue, int? maxvalue = null)
```

Constrains the number of checked rows at confirmation time. `minvalue` must be `>= 0`; `maxvalue`
`null` means unlimited. On **Enter**, if the checked count is outside the range the control stays open
and shows an error.

```csharp
PromptPlus.Controls.MultiTable<Product>("Select 2 to 4 products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Range(minvalue: 2, maxvalue: 4)
    .Run();
```

---

### `UseDefaultHistory`

```csharp
IMultiTableControl<T> UseDefaultHistory()
```

Loads the most recent history entry as the initial checked set, clearing any value set by
[`Default`](#default). Has no effect unless [`EnabledHistory`](#enabledhistory) is set.

---

### `DefaultMatchBy`

```csharp
IMultiTableControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
```

Custom equality used to match [`Default`](#default) and history values against the loaded rows —
essential for records/classes where reference equality is not meaningful.

```csharp
PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Default(products.Where(p => p.Available))
    .Run();
```

---

## Restricting what can be checked

A predicate decides whether a row can be **checked** (toggled on). On rejection the row stays unchecked
and, for the tuple overload, a custom message is shown.

### `PredicateChecked`

```csharp
IMultiTableControl<T> PredicateChecked(Func<T, bool> validselect)
IMultiTableControl<T> PredicateChecked(Func<T, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<T, bool>` | `true` = can be checked | Generic error on rejection |
| `Func<T, (bool, string?)>` | `(canCheck, message)` | Custom `message` on rejection |

Setting a synchronous predicate replaces any previously registered asynchronous one.

```csharp
PromptPlus.Controls.MultiTable<Product>("Products (stock > 50 only)")
    .AddColumn("Name",  x => x.Name)
    .AddColumn("Stock", x => x.Stock, alignment: ColumnAlignment.Right, width: 7)
    .AddItems(products)
    .PredicateChecked(p => p.Stock > 50
        ? (true, null)
        : (false, $"'{p.Name}' has only {p.Stock} units in stock."))
    .Run();
```

---

### `PredicateCheckedAsync`

```csharp
IMultiTableControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect)
IMultiTableControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts; setting one replaces any previously registered synchronous predicate.

> ?? The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Read-only display

### `ViewOnly`

```csharp
IMultiTableControl<T> ViewOnly(bool value = true)
```

Renders the table for browsing only — the user can navigate rows but cannot toggle checkboxes. Rows
marked via [`Default`](#default) are still shown pre-checked (read-only visual). Default `false`.

```csharp
PromptPlus.Controls.MultiTable<Product>("Product catalogue (view only)", "Press Esc or Enter to exit")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Default(products.Where(p => p.Available))
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .PageSize(4)
    .ViewOnly()
    .Run();
```

---

## History

### `EnabledHistory`

```csharp
IMultiTableControl<T> EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists the checked set to `filename` (serialized as JSON) and can restore it on the next run via
[`UseDefaultHistory`](#usedefaulthistory). The `IHistoryOptions` builder is identical to the one
documented for [Input ? EnabledHistory](../input/methods.md#enabledhistory) (`MinPrefixLength`, `MaxItems`,
`ExpirationTime`, `FilterType`, `PageSize`).

```csharp
PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Id",   x => x.Id, width: 4, alignment: ColumnAlignment.Right)
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .EnabledHistory("multitable-product-history")
    .UseDefaultHistory()
    .Run();
```

---

## Appearance & behavior

### `Styles`

```csharp
IMultiTableControl<T> Styles(MultiTableStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Styles(MultiTableStyles.SelectedCell, new Style(Color.Black, Color.Cyan))
    .Run();
```

---

### `Options`

```csharp
IMultiTableControl<T> Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish). See
[Global Behaviors ? Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<T[]> Run(CancellationToken token = default)
```

Renders the table and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<T[]>`](../../architecture.md#resultpromptt); `.Content` is the array of checked rows —
iterating it yields each `T` **directly** (no `.Value` wrapper).

```csharp
var result = PromptPlus.Controls.MultiTable<Product>("Products")
    .AddColumn("Name", x => x.Name)
    .AddItems(products)
    .Run();

if (!result.IsAborted)
    foreach (var p in result.Content)
        PromptPlus.Console.WriteLine(p.Name);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `MultiTableStyles` regions
- [Index](index.md) — overview and method map
- [Table ? Methods](../table/methods.md) — the single-row sibling
