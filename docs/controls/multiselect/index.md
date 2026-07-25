<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiSelect&lt;T&gt;**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [MultiSelect — Methods ?](methods.md)

---

> A scrollable, filterable list of checkboxes where the user checks **several** items with **Space**
> and confirms the whole set with **Enter**.

`MultiSelect<T>` is the multiple-choice sibling of [`Select<T>`](../select/index.md): same list model,
same grouping, filtering, and paging — but each row carries a checkbox and `.Run()` returns the whole
**array** of checked items. It works with strings, enums, records, or your own types; it can group
items under headers, filter as the user types, enforce a minimum/maximum number of picks, show extra
info per row, and validate each check before it is accepted.

> ?? Need to pick exactly **one** item? Use the [**Select**](../select/index.md) control — same list
> model, single answer.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, checking, filtering, grouping, range, validation, history, view-only |
| [Styles](styles.md) | The `MultiSelectStyles` regions and how to recolor them |

---

## When to use it

| Use `MultiSelect<T>` when… | Consider instead… |
|---|---|
| The user checks several options from a known set | — |
| The user picks exactly one option | [Select](../select/index.md) |
| The data is tabular (multiple columns) | [Table](../table/index.md) |
| The data is hierarchical | [Tree](../tree/index.md) |
| It is a yes/no or single-key answer | [KeyPress / Confirm](../keypress/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .MultiSelect<string>("Pick your toppings")
    .AddItems(["Cheese", "Mushrooms", "Onions", "Peppers"])
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"You chose: {string.Join(", ", result.Content)}");
```

- `MultiSelect<string>("Pick your toppings")` creates a checkbox list of strings. The type argument
  `T` is the item type.
- `.AddItems([...])` fills the list; `.AddItem(x)` adds one at a time (and can pre-check it).
- `.Run()` renders the list and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<T[]>`](../../architecture.md#resultpromptt) — the array of checked items.

---

## Selecting from an enum

`MultiSelect<T>` reads enum members automatically — including `[Display(Name = ..., Order = ...)]`
attributes for the label and ordering. No `AddItems` call needed:

```csharp
enum Feature { Logging, Caching, Metrics, Tracing }

var features = PromptPlus.Controls.MultiSelect<Feature>("Enable features").Run();
```

---

## A richer example

```csharp
using PromptPlusLibrary;

var cities = PromptPlus.Controls
    .MultiSelect<string>("Which cities?", "Type to filter")
    .AddGroupedItems("North America", ["Seattle", "Boston", "New York"])
    .AddGroupedItems("Asia",          ["Tokyo", "Singapore", "Shanghai"])
    .Filter(FilterMode.Contains)     // live filtering as the user types
    .Range(1, 3)                     // at least 1, at most 3
    .PageSize(5)                     // 5 rows visible at a time
    .Run();
```

This combines **grouping** (headers), **filtering**, **paging**, and a **selection range** — see
[Operations](operations.md) for how they behave together.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Add items | `AddItem`, `AddItems`, `AddGroupedItem`, `AddGroupedItems`, `AddSeparator` |
| Load from a source | `Interaction`, `InteractionAsync` |
| Item text & info | `TextSelector`, `TextSelectorAsync`, `ExtraInfo`, `ExtraInfoAsync`, `HideTipGroup` |
| Filtering & paging | `Filter`, `PageSize` |
| Initial value | `Default`, `UseDefaultHistory`, `DefaultMatchBy` |
| Selection range | `Range` |
| Validate each check | `PredicateChecked`, `PredicateCheckedAsync` |
| Read-only display | `ViewOnly` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| History | `EnableHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`MultiSelect<T>` returns `ResultPrompt<T[]>` — the array of checked items.

| Member | Meaning |
|---|---|
| `.Content` | The checked items as a `T[]` (an **empty array** if aborted or nothing checked) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var (picked, aborted) = PromptPlus.Controls
    .MultiSelect<string>("Toppings").AddItems(["Cheese", "Onions"]).Run();
if (!aborted)
    PromptPlus.Console.WriteLine($"{picked.Length} selected");
```

> Unlike [`Select<T>`](../select/index.md) (which returns a single `T`), `MultiSelect<T>` always
> returns an array — never `null`. Check `.Content.Length` to tell "confirmed nothing" from a real
> pick.

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, checking, filtering, grouping, range, validation
- [Styles](styles.md) — recolor the list regions
- [Select](../select/index.md) — single-choice sibling
- [Table](../table/index.md) · [Tree](../tree/index.md) — tabular and hierarchical pickers
