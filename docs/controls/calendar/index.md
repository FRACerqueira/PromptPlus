<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Calendar**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Calendar — Methods →](methods.md)

---

> An interactive monthly calendar grid where the user navigates day-by-day and confirms a single **date** with **Enter**.

`Calendar` renders a full month grid. The user moves the highlight with the arrow keys, jumps
between months and years, and confirms the highlighted day. It can limit the selectable window to a
min/max range, disable individual days or whole weekends, attach notes to dates, highlight special
days, follow any culture and first-day-of-week, and validate the choice before returning it.

> ☑️ The result is a **nullable** `DateTime?` — an aborted run has no value. Always check
> `result.IsAborted` (or `result.Content.HasValue`) before using the date.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, first examples, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, notes, highlights, disabled dates, range, culture, validation |
| [Styles](styles.md) | The `CalendarStyles` regions and how to recolor them |

---

## When to use it

| Use `Calendar` when… | Consider instead… |
|---|---|
| The user picks a single calendar date | — |
| The user picks from a fixed list of options | [Select](../select/index.md) |
| The user types free-form text | [Input](../input/index.md) |
| It is a yes/no or single-key answer | [KeyPress / Confirm](../keypress/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;
using System;

var result = PromptPlus.Controls
    .Calendar("Select a date")
    .Run();

if (!result.IsAborted && result.Content.HasValue)
    PromptPlus.Console.WriteLine($"You chose {result.Content.Value:yyyy-MM-dd}");
```

- `Calendar("Select a date")` creates the control; the first argument is the prompt.
- `.Run()` renders the grid and blocks until **Enter** (confirm) or **Esc** (abort), returning a
  [`ResultPrompt<DateTime?>`](../../architecture.md#resultpromptt).
- `.Content` is a `DateTime?` — `null` when aborted, so guard with `HasValue`.

---

## Starting on a specific date

Use [`Default`](methods.md#default) to open the grid on a chosen day instead of today:

```csharp
using PromptPlusLibrary;
using System;

var date = PromptPlus.Controls
    .Calendar("Appointment date")
    .Default(new DateTime(2026, 7, 21))
    .Run();
```

---

## A richer example

```csharp
using PromptPlusLibrary;
using System;

var when = PromptPlus.Controls
    .Calendar("Select a business date", "Weekends and holidays are blocked")
    .Default(DateTime.Now)
    .FirstDayOfWeek(DayOfWeek.Monday)   // week starts Monday
    .DisabledWeekend()                  // Saturday / Sunday not selectable
    .DisableDates(new DateTime(2026, 7, 24))
    .Highlights(new DateTime(2026, 7, 21))
    .Range(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(30))
    .Run();
```

This combines **first-day-of-week**, **weekend blocking**, **disabled dates**, **highlights**, and
a **range limit** — see [Operations](operations.md) for how they behave together.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Layout & culture | `Layout`, `Culture`, `FirstDayOfWeek` |
| Restrict dates | `Range`, `DisableDates`, `DisabledWeekend` |
| Notes | `AddNote`, `AddNotes`, `PageSize` |
| Highlights | `Highlights` |
| Load from a source | `Interaction`, `InteractionAsync` |
| Initial value | `Default` |
| Validate on confirm | `PredicateSelected`, `PredicateSelectedAsync` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Calendar` returns `ResultPrompt<DateTime?>` — the confirmed date.

| Member | Meaning |
|---|---|
| `.Content` | The selected `DateTime?` (`null` if aborted) |
| `.IsAborted` | `true` when the user pressed Esc |

```csharp
var (date, aborted) = PromptPlus.Controls.Calendar("Date").Run();
if (!aborted && date.HasValue)
    PromptPlus.Console.WriteLine(date.Value.ToShortDateString());
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, notes, highlights, disabled dates, range, validation
- [Styles](styles.md) — recolor the calendar regions
- [Select](../select/index.md) — pick from a fixed list
- [Input](../input/index.md) — free-form text entry
