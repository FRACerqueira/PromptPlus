<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Calendar — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Calendar — Styles →](styles.md)

---

How the `Calendar` control behaves while it is running: keyboard navigation, notes, highlights,
disabled dates and weekends, range limits, culture and first-day-of-week, and validation.

---

## Anatomy of the control

```
Select a business date                    ← prompt
Weekends and holidays are blocked         ← description (optional / dynamic)
        July 2026                         ← month + year header
 Mo Tu We Th Fr Sa Su                     ← week-day header (respects FirstDayOfWeek)
        1  2  3  4  5                      ← day grid
  6  7  8  9 10 11 12
 13 14 15 16 17 18 19
 20[21]22 23 24 25 26                      ← [21] highlighted cursor day
 27 28 29 30 31
Notes for 21 Jul 2026                      ← notes panel (toggled with F2)
Page 1/2                                   ← notes pagination
Enter: confirm  Esc: cancel  F2: notes     ← tooltip
```

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

| Key | Action |
|---|---|
| `←` / `→` | Move one day back / forward |
| `↑` / `↓` | Move one week back / forward |
| `Page Up` / `Page Down` | Previous / next month |
| `Ctrl+Page Up` / `Ctrl+Page Down` | Previous / next year |
| `Home` | Jump to today |
| `F2` | Toggle the date-notes display |
| `Enter` | Confirm the highlighted date (runs validation) |
| `Esc` | Abort → `IsAborted == true` |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

Disabled days and days outside the [range](#range-limits) cannot be confirmed.

---

## Notes (F2)

Notes annotate individual dates and are read on demand:

- Add them with [`AddNote`](methods.md#addnote) (one date) or [`AddNotes`](methods.md#addnotes)
  (a `(date, note)[]` batch); a `null` note is stored as an empty string.
- Press **F2** while a day with notes is highlighted to toggle the notes panel open and closed.
- When a date has many notes, the panel paginates them. [`PageSize`](methods.md#pagesize) sets how
  many notes appear per page (`0` auto-computes from terminal height).
- Notes can be generated programmatically per data item with
  [`Interaction`](methods.md#interaction) / [`InteractionAsync`](methods.md#interactionasync).

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date", "Press [F2] to read notes")
    .AddNote(today, "Team standup at 09:00")
    .AddNotes([(today.AddDays(1), "Release freeze"), (today.AddDays(2), "Retro")])
    .PageSize(3)
    .Run();
```

---

## Highlights

[`Highlights`](methods.md#highlights) marks special days so they stand out in the grid using the
`CalendarHighlight` style. Highlighted days are still fully selectable — the marking is decorative,
not a restriction (contrast with [disabled dates](#disabled-dates--weekends)).

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date")
    .Highlights(today, today.AddDays(3))
    .Run();
```

---

## Disabled dates & weekends

Two ways to make days non-selectable, both rendered with the `Disabled` style:

- [`DisableDates(params DateTime[])`](methods.md#disabledates) blocks individual days.
- [`DisabledWeekend()`](methods.md#disabledweekend) blocks every Saturday and Sunday.

The cursor can still land on a disabled day, but pressing **Enter** there does not confirm it.

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Business date")
    .DisabledWeekend()
    .DisableDates(today.AddDays(1), today.AddDays(2))
    .Run();
```

---

## Range limits

[`Range(minValue, maxValue)`](methods.md#range) defines an inclusive selectable window. Days outside
it are shown but cannot be confirmed, and a [`Default`](methods.md#default) that falls outside the
range is ignored.

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date in range")
    .Range(today.AddDays(-3), today.AddDays(3))
    .Run();
```

> `Range` throws `ArgumentOutOfRangeException` if `minValue` is greater than `maxValue`.

---

## Culture & first day of week

- [`Culture`](methods.md#culture) drives month names, weekday names, and date formatting — and how
  dates are parsed and validated. Pass a `CultureInfo` or a culture name string (e.g. `"pt-BR"`).
- [`FirstDayOfWeek`](methods.md#firstdayofweek) sets which weekday occupies the first column,
  independent of the culture default.

```csharp
using PromptPlusLibrary;
using System;
using System.Globalization;

PromptPlus.Controls.Calendar("Data")
    .Culture(new CultureInfo("pt-BR"))
    .FirstDayOfWeek(DayOfWeek.Monday)
    .Run();
```

[`Layout`](methods.md#layout) is orthogonal to culture — pick `AsciiSingleGrid` / `AsciiDoubleGrid`
when the terminal cannot render Unicode box-drawing characters.

---

## Confirmation & validation flow

Pressing **Enter** on the highlighted date:

1. Validation runs — [`PredicateSelected`](methods.md#predicateselected) /
   [`PredicateSelectedAsync`](methods.md#predicateselectedasync), if configured. The predicate
   receives a `DateTime?`, so it can also reject a "no date" case.
2. **Valid** → the control closes and returns the date.
   **Invalid** → the grid stays open and shows the error line.

Disabled and out-of-range days cannot be confirmed, so predicates typically guard *business* rules
(e.g., "must be a future date") rather than availability.

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Controls.Calendar("Select day <= 28")
    .PredicateSelected(date =>
    {
        if (!date.HasValue)
            return (false, "Date is required");
        return date.Value.Day <= 28
            ? (true, (string?)null)
            : (false, "Only days up to 28 are allowed");
    })
    .Run();
```

---

## Initial date & history

- [`Default(value)`](methods.md#default) opens the grid on `value` (today when omitted); a value
  outside [`Range`](#range-limits) is ignored.
- With [`EnabledHistory`](methods.md#enabledhistory), confirmed dates are stored on disk;
  `Default(..., useDefaultHistory: true)` restores the last one on the next run.
- History options (`MinPrefixLength`, `MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`) match
  the [Input history options](../input/methods.md#enabledhistory).

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Calendar` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm a date |
| `HideAfterFinish(true)` | Erases the grid after confirm; only the answer remains |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

`PageSize` (notes per page) can be set per control ([`PageSize`](methods.md#pagesize)) or globally
(`PromptPlus.Config.PageSize`).

---

## Edge cases & gotchas

- **Aborted results** carry `.Content == null`. Always branch on `IsAborted` or `Content.HasValue`
  before reading the date.
- **Default outside range** is silently ignored — the grid opens on today instead.
- **Async callbacks block the UI thread** — keep validators and description callbacks fast.
- **Disabled vs. highlighted** — disabled days cannot be confirmed; highlighted days can. Use the
  right one for your intent.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
