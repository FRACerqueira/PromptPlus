# Migration v5.x → v6.x: Select and MultiSelect

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## Renamed public methods

The v5.x members below were **renamed** in v6.x (same behavior, new name). This is the fast list to
search-and-replace before reading the detailed sections.

| Control | v5.x member | v6.x member |
|---|---|---|
| Select / MultiSelect | `EqualItems(Func<T,T,bool>)` | `DefaultMatchBy(Func<T,T,bool>)` |
| Select / MultiSelect | `OnlyView(bool)` | `ViewOnly(bool)` |
| Select / MultiSelect | `DefaultHistory(bool)` | `UseDefaultHistory()` *(no parameter)* |
| MultiSelect | `PredicateSelected` / `PredicateSelectedAsync` | `PredicateChecked` / `PredicateCheckedAsync` |

> `Select<T>` keeps `PredicateSelected` / `PredicateSelectedAsync`. Only the multi-choice
> `MultiSelect<T>` renamed them to `PredicateChecked` / `PredicateCheckedAsync`.

## Select\<T\>

### Breaking Changes

#### 1. `EqualItems` → `DefaultMatchBy`

**Before (v5.x):**
```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Select<Product>("Product:")
    .AddItems(products)
    .EqualItems((a, b) => a.Id == b.Id)
    .Run();
```

**After (v6.x):**
```csharp
PromptPlus.Controls.Select<Product>("Product:")
    .AddItems(products)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Run();
```

#### 2. `OnlyView` → `ViewOnly`

```csharp
// v5.x
.OnlyView(true)

// v6.x
.ViewOnly(true)
```

#### 3. `DefaultHistory(bool)` → `UseDefaultHistory()`

**Before (v5.x):**
```csharp
PromptPlus.Controls.Select<string>("Option:")
    .AddItems(options)
    .EnabledHistory("myhistory")
    .DefaultHistory(true)
    .Run();
```

**After (v6.x):**
```csharp
PromptPlus.Controls.Select<string>("Option:")
    .AddItems(options)
    .EnabledHistory("myhistory")
    .UseDefaultHistory()   // no parameter — always enables it
    .Run();
```

#### 4. `Filter(FilterMode, bool caseinsensitive)` → `Filter(FilterMode)`

```csharp
// v5.x
.Filter(FilterMode.Contains, false)

// v6.x — the caseinsensitive parameter was removed
.Filter(FilterMode.Contains)
```

#### 5. `MaxWidth(byte)` — removed

```csharp
// v5.x — remove this line during migration
.MaxWidth(60)
```

---

### What's new in v6.x (Select)

`DefaultMatchBy`, `ViewOnly`, `UseDefaultHistory`, and the following async variants are new (v5.x had only the synchronous forms):

```csharp
PromptPlus.Controls.Select<Product>("Product:")
    .AddItems(products)
    .ChangeDescriptionAsync(async item =>
    {
        var details = await LoadDetailsAsync(item.Id);
        return details.Description;
    })
    .PredicateSelectedAsync(async item =>
    {
        bool active = await CheckStatusAsync(item.Id);
        return (active, active ? null : "Inactive item");
    })
    .Run();
```

New: `ChangeDescriptionAsync` · `InteractionAsync` · `TextSelectorAsync` · `ExtraInfoAsync` · `PredicateSelectedAsync` (x2).

---

## MultiSelect\<T\>

### Breaking Changes

#### 1. `EqualItems` → `DefaultMatchBy`

```csharp
// v5.x
.EqualItems((a, b) => a.Id == b.Id)

// v6.x
.DefaultMatchBy((a, b) => a.Id == b.Id)
```

#### 2. `OnlyView` → `ViewOnly`

```csharp
// v5.x
.OnlyView(true)

// v6.x
.ViewOnly(true)
```

#### 3. `DefaultHistory(bool)` → `UseDefaultHistory()`

```csharp
// v5.x
.DefaultHistory(true)

// v6.x
.UseDefaultHistory()
```

#### 4. `MaxWidth(byte)` — removed

```csharp
// v5.x — remove this line during migration
.MaxWidth(60)
```

#### 5. `HideCountSelected(bool)` — removed with no equivalent

**Before (v5.x):**
```csharp
PromptPlus.Controls.MultiSelect<string>("Items:")
    .AddItems(items)
    .HideCountSelected(true)
    .Run();
```

**After (v6.x):**
```csharp
// HideCountSelected was removed — the selected count is always shown
PromptPlus.Controls.MultiSelect<string>("Items:")
    .AddItems(items)
    .Run();
```

---

### What's new in v6.x (MultiSelect)

New: `DefaultMatchBy` · `ViewOnly` · `UseDefaultHistory` · `TextSelectorAsync` · `ExtraInfoAsync` · `ChangeDescriptionAsync` · `InteractionAsync` · `PredicateCheckedAsync` (x2).

> In `MultiSelect<T>`, the v5.x `PredicateSelected` / `PredicateSelectedAsync` were renamed to
> `PredicateChecked` / `PredicateCheckedAsync`.

```csharp
PromptPlus.Controls.MultiSelect<Product>("Products:")
    .AddItems(products)
    .TextSelectorAsync(async p => await FormatNameAsync(p))
    .ExtraInfoAsync(async p => await FormatPriceAsync(p))
    .PredicateCheckedAsync(async item =>
    {
        bool available = await CheckStockAsync(item.Id);
        return (available, available ? null : "Out of stock");
    })
    .Run();
```

---

## Full API reference

### Select\<T\> — v5.x vs v6.x

| Method | v5.x | v6.x | Change |
|---|---|---|---|
| `EqualItems(Func<T,T,bool>)` | ✅ | ❌ | Renamed to `DefaultMatchBy` |
| `DefaultMatchBy(Func<T,T,bool>)` | ❌ | ✅ | New name |
| `OnlyView(bool)` | ✅ | ❌ | Renamed to `ViewOnly` |
| `ViewOnly(bool)` | ❌ | ✅ | New name |
| `DefaultHistory(bool)` | ✅ | ❌ | Replaced by `UseDefaultHistory()` |
| `UseDefaultHistory()` | ❌ | ✅ | New (no parameter) |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `Filter(FilterMode, bool)` | ✅ | ❌ | `bool` parameter removed |
| `Filter(FilterMode)` | ❌ | ✅ | New |
| `ChangeDescription` · `Interaction` · `TextSelector` · `ExtraInfo` · `PredicateSelected` (x2) | ✅ | ✅ | Unchanged |
| `ChangeDescriptionAsync` · `InteractionAsync` · `TextSelectorAsync` · `ExtraInfoAsync` · `PredicateSelectedAsync` (x2) | ❌ | ✅ | New |

### MultiSelect\<T\> — v5.x vs v6.x

| Method | v5.x | v6.x | Change |
|---|---|---|---|
| `EqualItems(Func<T,T,bool>)` | ✅ | ❌ | Renamed to `DefaultMatchBy` |
| `OnlyView(bool)` | ✅ | ❌ | Renamed to `ViewOnly` |
| `DefaultHistory(bool)` | ✅ | ❌ | Replaced by `UseDefaultHistory()` |
| `MaxWidth(byte)` | ✅ | ❌ | Removed |
| `HideCountSelected(bool)` | ✅ | ❌ | Removed with no equivalent |
| `Range(int, int?)` | ✅ | ✅ | Unchanged |
| `Filter(FilterMode, bool)` | ✅ | ❌ | → `Filter(FilterMode)` |
| `PredicateSelected` / `PredicateSelectedAsync` (x2) | ✅ | ❌ | Renamed to `PredicateChecked` / `PredicateCheckedAsync` |
| `ChangeDescription` · `Interaction` · `TextSelector` · `ExtraInfo` | ✅ | ✅ | Unchanged |
| `DefaultMatchBy` · `ViewOnly` · `UseDefaultHistory` · `PredicateChecked` · async variants | ❌ | ✅ | New |
