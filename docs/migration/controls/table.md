# Migration v5.x → v6.x: Table and MultiTable

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## Renamed controls

| v5.x | v6.x |
|---|---|
| `TableSelect<T>()` | `Table<T>()` |
| `TableMultiSelect<T>()` | `MultiTable<T>()` |

## Renamed public methods

`MultiTable<T>` **renamed** its confirmation validator. `Table<T>` (single-selection) keeps
`PredicateSelected`.

| Control | v5.x member | v6.x member |
|---|---|---|
| MultiTable | `PredicateSelected(Func<T, bool>)` | `PredicateChecked(Func<T, bool>)` |
| MultiTable | `PredicateSelected(Func<T, (bool, string?)>)` | `PredicateChecked(Func<T, (bool, string?)>)` |

> The async overloads follow the same name: `MultiTable` uses `PredicateCheckedAsync`; `Table` uses
> `PredicateSelectedAsync`.

---

## Breaking Changes

### 1. `AddColumn` — full rework

This is the most impactful change. The `AddColumn` signature was completely redesigned.

**Before (v5.x):**
```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;

PromptPlus.Controls.TableSelect<Product>("Products:")
    .AddColumn("Name",  width: 20, p => p.Name,
               rowAlignment: TextAlignment.Left,
               titleAlignment: TextAlignment.Center,
               titlereplaceswidth: true,
               maxslidinglines: 0)
    .AddColumn("Price", width: 10, p => p.Price.ToString("C"),
               rowAlignment: TextAlignment.Right)
    .AddItems(products)
    .Run();
```

**After (v6.x):**
```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;

PromptPlus.Controls.Table<Product>("Products:")
    .AddColumn("Name",
               selector: p => p.Name,
               width: 20,
               alignment: ColumnAlignment.Left,
               isFilterable: true)
    .AddColumn("Price",
               selector: p => p.Price,
               formatter: val => ((decimal)val).ToString("C"),
               width: 10,
               alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Run();
```

> **Key differences:**
> - selector is `Func<T, object>` (was `Func<T, string>`)
> - `formatter` is a separate optional `Func<object, string>?`
> - `TextAlignment` → `ColumnAlignment`
> - `titleAlignment`, `titlereplaceswidth`, `maxslidinglines` **removed**
> - new `isFilterable` controls whether the column participates in filtering

### 2. `Filter(FilterMode, bool caseinsensitive)` → `Filter(FilterMode, FilterTableMode)`

```csharp
// v5.x — second parameter was case sensitivity
.Filter(FilterMode.Contains, false)

// v6.x — second parameter is the filter scope (default FilterTableMode.Answer)
.Filter(FilterMode.Contains, FilterTableMode.Answer)
```

### 3. `Layout(TableLayout)` → `LayoutMode(TableLayoutMode)`

```csharp
// v5.x
.Layout(TableLayout.SingleGridFull)

// v6.x — method and enum renamed
.LayoutMode(TableLayoutMode.SingleBox)
```

### 4. `HideHeaders(bool)` → `HideElements(HideTable)`

The v5.x boolean toggle was replaced by a `[Flags]` enum that can hide several table regions at once.

```csharp
// v5.x
.HideHeaders(true)

// v6.x
.HideElements(HideTable.Header)
```

### 5. `ChangeDescription` signature changed

```csharp
// v5.x — received row/column indices
.ChangeDescription((item, row, col) => $"{item.Name}")

// v6.x
.ChangeDescription(item => $"{item.Name}")
```

### 6. `OnlyView(bool)` → `ViewOnly(bool)`

```csharp
// v5.x
.OnlyView(true)

// v6.x
.ViewOnly(true)
```

### 7. `SeparatorRows` — removed with no equivalent

```csharp
// v5.x only — remove during migration
.SeparatorRows(true)
```

### 8. `MaxWidth(byte)` — removed

```csharp
// v5.x only — remove during migration
.MaxWidth(80)
```

### 9. `Table<T>` return type changed (single-select)

```csharp
// v5.x — TableSelect<T>.Run() → ResultPrompt<T>
Product? picked = PromptPlus.Controls.TableSelect<Product>("Products:")
    .AddColumn(/* ... */).AddItems(products).Run().Content;

// v6.x — Table<T>.Run() → ResultPrompt<TableResult<T>>
var result = PromptPlus.Controls.Table<Product>("Products:")
    .AddColumn(/* ... */).AddItems(products).Run();

Product? picked = result.Content.Value;          // the selected row
int row  = result.Content.RowIndex;
int col  = result.Content.ColumnIndex;
```

> `MultiTable<T>.Run()` still returns `ResultPrompt<T[]>` (read via `.Content`).

---

## What's new in v6.x

### `HorizontalScroll` (Table and MultiTable)
```csharp
PromptPlus.Controls.Table<Product>("Products:")
    .AddColumn("Name", p => p.Name, width: 30)
    .AddColumn("Description", p => p.Description, width: 50)
    .AddItems(products)
    .HorizontalScroll(HorizontalScrollMode.Auto)
    .Run();
```

### MultiTable — history, view-only and async variants
```csharp
PromptPlus.Controls.MultiTable<Product>("Products:")
    .AddColumn("Name", p => p.Name)
    .AddItems(products)
    .EnableHistory("products_selection")
    .UseDefaultHistory()
    .TextSelectorAsync(async p => await FormatNameAsync(p))
    .ChangeDescriptionAsync(async p => (await LoadInfoAsync(p.Id)).Summary)
    .Run();
```

New on `Table`/`MultiTable`: `DefaultMatchBy` · `ViewOnly` · `HorizontalScroll` · `ChangeDescriptionAsync` · `TextSelectorAsync` · `InteractionAsync`; `Table` adds `PredicateSelectedAsync` (x2) and `MultiTable` adds `PredicateCheckedAsync` (x2); plus `EnableHistory` / `UseDefaultHistory` on `MultiTable`.

---

## Full API reference

### Table\<T\> — v5.x vs v6.x

| Method | v5.x (TableSelect) | v6.x (Table) | Change |
|---|---|---|---|
| Factory | `TableSelect<T>()` | `Table<T>()` | Renamed |
| `AddColumn(title, width, Func<T,string>, TextAlignment, TextAlignment, bool, int)` | ✅ | ❌ | Reworked |
| `AddColumn(header, Func<T,object>, Func<object,string>?, int?, ColumnAlignment, bool)` | ❌ | ✅ | New form |
| `Filter(FilterMode, bool)` | ✅ | ❌ | Parameter changed |
| `Filter(FilterMode, FilterTableMode)` | ❌ | ✅ | New form |
| `Layout(TableLayout)` | ✅ | ❌ | Renamed to `LayoutMode(TableLayoutMode)` |
| `LayoutMode(TableLayoutMode)` | ❌ | ✅ | New name |
| `HideHeaders(bool)` | ✅ | ❌ | Replaced by `HideElements(HideTable)` |
| `HideElements(HideTable)` | ❌ | ✅ | New |
| `OnlyView(bool)` | ✅ | ❌ | Renamed to `ViewOnly` |
| `ChangeDescription(Func<T,int,int,string>)` | ✅ | ❌ | → `ChangeDescription(Func<T,string>)` |
| `SeparatorRows(bool)` | ✅ | ❌ | Removed with no equivalent |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `EqualItems(Func<T,T,bool>)` | ✅ | ❌ | Renamed to `DefaultMatchBy` |
| `TextSelector(Func<T,string>)` · `PageSize(byte)` | ✅ | ✅ | Unchanged |
| `HorizontalScroll` · `ViewOnly` · `DefaultMatchBy` · async variants | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<T>` | `ResultPrompt<TableResult<T>>` | Return type changed |

### MultiTable\<T\> — v5.x vs v6.x

| Method | v5.x (TableMultiSelect) | v6.x (MultiTable) | Change |
|---|---|---|---|
| Factory | `TableMultiSelect<T>()` | `MultiTable<T>()` | Renamed |
| `AddColumn(...)` | ✅ | ✅ | Same rework as above |
| `Filter(FilterMode, bool)` | ✅ | ❌ | → `Filter(FilterMode, FilterTableMode)` |
| `Layout(TableLayout)` | ✅ | ❌ | → `LayoutMode(TableLayoutMode)` |
| `HideHeaders(bool)` | ✅ | ❌ | → `HideElements(HideTable)` |
| `OnlyView(bool)` | ✅ | ❌ | → `ViewOnly` |
| `SeparatorRows(bool)` | ✅ | ❌ | Removed with no equivalent |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `EqualItems(Func<T,T,bool>)` | ✅ | ❌ | Renamed to `DefaultMatchBy` |
| `Range(int, int?)` | ✅ | ✅ | Unchanged |
| `PredicateSelected` (x2) | ✅ | ❌ | Renamed to `PredicateChecked` (x2) |
| `EnableHistory` · `UseDefaultHistory` · `ViewOnly` · `DefaultMatchBy` · `HorizontalScroll` · `PredicateChecked` (x2) · async variants | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<T[]>` | `ResultPrompt<T[]>` | Unchanged |
