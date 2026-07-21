<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Select&lt;T&gt;**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Select — Methods →](methods.md)

---

> A scrollable, filterable list where the user picks **one** item and confirms with **Enter**.

`Select<T>` turns any collection into an interactive menu. It works with strings, enums, records,
or your own types; it can group items under headers, filter as the user types, auto-select the last
match, show extra info per row, and validate the choice before returning it.

> ☑️ Need to pick **several** items at once? Use the [**MultiSelect**](../multiselect/index.md)
> control — same list model, with checkboxes.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, filtering, grouping, validation, history, view-only |
| [Styles](styles.md) | The `SelectStyles` regions and how to recolor them |

---

## When to use it

| Use `Select<T>` when… | Consider instead… |
|---|---|
| The user picks one option from a known set | — |
| The user may pick several | [MultiSelect](../multiselect/index.md) |
| The data is tabular (multiple columns) | [Table](../table/index.md) |
| The data is hierarchical | [Tree](../tree/index.md) |
| It is a yes/no or single-key answer | [KeyPress / Confirm](../keypress/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Select<string>("Favorite color")
    .AddItems(["Red", "Green", "Blue"])
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"You chose {result.Content}");
```

- `Select<string>("Favorite color")` creates a list of strings. The type argument `T` is the item type.
- `.AddItems([...])` fills the list; `.AddItem(x)` adds one at a time.
- `.Run()` renders the list and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<T>`](../../architecture.md#resultpromptt).

---

## Selecting from an enum

`Select<T>` reads enum members automatically — including `[Display(Name = ..., Order = ...)]`
attributes for the label and ordering. No `AddItems` call needed:

```csharp
enum Environment { Dev, Test, Staging, Prod }

var env = PromptPlus.Controls.Select<Environment>("Target environment").Run();
```

---

## A richer example

```csharp
using PromptPlusLibrary;

var city = PromptPlus.Controls
    .Select<string>("Which city?", "Type to filter")
    .AddGroupedItems("North America", ["Seattle", "Boston", "New York"])
    .AddGroupedItems("Asia",          ["Tokyo", "Singapore", "Shanghai"])
    .Filter(FilterMode.Contains)     // live filtering as the user types
    .PageSize(5)                     // 5 rows visible at a time
    .Run();
```

This combines **grouping** (headers), **filtering**, and **paging** — see
[Operations](operations.md) for how they behave together.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Add items | `AddItem`, `AddItems`, `AddGroupedItem`, `AddGroupedItems`, `AddSeparator` |
| Load from a source | `Interaction`, `InteractionAsync` |
| Item text & info | `TextSelector`, `TextSelectorAsync`, `ExtraInfo`, `ExtraInfoAsync`, `HideTipGroup` |
| Filtering & paging | `Filter`, `AutoSelect`, `PageSize` |
| Initial value | `Default`, `UseDefaultHistory`, `DefaultMatchBy` |
| Validate on confirm | `PredicateSelected`, `PredicateSelectedAsync` |
| Read-only display | `ViewOnly` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Select<T>` returns `ResultPrompt<T>` — the single chosen item.

| Member | Meaning |
|---|---|
| `.Content` | The selected `T` (default of `T` if aborted) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var (color, aborted) = PromptPlus.Controls
    .Select<string>("Color").AddItems(["Red", "Green"]).Run();
if (!aborted) PromptPlus.Console.WriteLine(color);
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, filtering, grouping, validation
- [Styles](styles.md) — recolor the list regions
- [MultiSelect](../multiselect/index.md) — multiple-choice sibling
- [Table](../table/index.md) · [Tree](../tree/index.md) — tabular and hierarchical pickers
