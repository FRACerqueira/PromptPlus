<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Calendar — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Calendar — Operations →](operations.md)

---

Every fluent method on `ICalendarControl`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Calendar(string prompt = "", string? description = null)`,
> which returns `ICalendarControl`.

**Quick jump:**
[Layout](#layout) ·
[Culture](#culture) ·
[FirstDayOfWeek](#firstdayofweek) ·
[Range](#range) ·
[DisableDates](#disabledates) ·
[DisabledWeekend](#disabledweekend) ·
[AddNote](#addnote) ·
[AddNotes](#addnotes) ·
[PageSize](#pagesize) ·
[Highlights](#highlights) ·
[Interaction](#interaction) ·
[InteractionAsync](#interactionasync) ·
[Default](#default) ·
[PredicateSelected](#predicateselected) ·
[PredicateSelectedAsync](#predicateselectedasync) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[EnabledHistory](#enabledhistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Layout & culture

### `Layout`

```csharp
ICalendarControl Layout(CalendarLayout layout = CalendarLayout.SingleGrid)
```

Sets how the grid and its lines are drawn.

| `CalendarLayout` | Renders |
|---|---|
| `SingleGrid` | Single-line box drawing (default) |
| `DoubleGrid` | Double-line box drawing |
| `AsciiSingleGrid` | ASCII single-line grid (portable, no Unicode) |
| `AsciiDoubleGrid` | ASCII double-line grid |

```csharp
PromptPlus.Controls.Calendar("Date")
    .Layout(CalendarLayout.AsciiSingleGrid)
    .Run();
```

---

### `Culture`

```csharp
ICalendarControl Culture(CultureInfo culture)
ICalendarControl Culture(string cultureName)
```

Sets the culture used to display month names, day names, and date formatting — and to parse and
validate dates. The default is the current PromptPlus culture. The `string` overload is shorthand for
`Culture(new CultureInfo(cultureName))`.

```csharp
using PromptPlusLibrary;
using System.Globalization;

PromptPlus.Controls.Calendar("Data")
    .Culture("pt-BR")
    .Run();

PromptPlus.Controls.Calendar("Date")
    .Culture(new CultureInfo("en-US"))
    .Run();
```

> `Culture(CultureInfo)` throws `ArgumentNullException` if `culture` is `null`;
> `Culture(string)` throws `ArgumentException` if `cultureName` is `null` or empty.

---

### `FirstDayOfWeek`

```csharp
ICalendarControl FirstDayOfWeek(DayOfWeek firstDayOfWeek)
```

Sets which weekday appears in the first column. When not set, the culture's default is used.

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Controls.Calendar("Date")
    .FirstDayOfWeek(DayOfWeek.Monday)
    .Run();
```

---

## Restricting dates

### `Range`

```csharp
ICalendarControl Range(DateTime minValue, DateTime maxValue)
```

Defines an inclusive window of selectable dates. Days outside it are shown but cannot be confirmed.

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date")
    .Range(today.AddDays(-3), today.AddDays(3))
    .Run();
```

> Throws `ArgumentOutOfRangeException` if `minValue` is greater than `maxValue`.
> A [`Default`](#default) outside the range is ignored.

---

### `DisableDates`

```csharp
ICalendarControl DisableDates(params DateTime[] dates)
```

Marks specific dates as non-selectable (rendered with the `Disabled` style).

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date")
    .DisableDates(today.AddDays(1), today.AddDays(2))
    .Run();
```

> Throws `ArgumentNullException` if `dates` is `null`.

---

### `DisabledWeekend`

```csharp
ICalendarControl DisabledWeekend(bool value = true)
```

Blocks Saturday and Sunday from selection. Default `true` when called.

```csharp
PromptPlus.Controls.Calendar("Business date")
    .DisabledWeekend()
    .Run();
```

---

## Notes

### `AddNote`

```csharp
ICalendarControl AddNote(DateTime value, string? note = null)
```

Attaches a note to a single date. A `null` note becomes an empty string. Notes for the highlighted
day are shown when the user presses **F2** (see [Operations](operations.md#notes-f2)).

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Controls.Calendar("Date", "Press [F2] to read notes")
    .AddNote(DateTime.Now.Date, "Team standup at 09:00")
    .Run();
```

---

### `AddNotes`

```csharp
ICalendarControl AddNotes((DateTime, string?)[] notes)
```

Adds several notes at once as `(date, note)` tuples. A `null` note becomes an empty string.

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date")
    .AddNotes(
    [
        (today.AddDays(1), "Tomorrow note"),
        (today.AddDays(2), "Day+2 note")
    ])
    .Run();
```

---

### `PageSize`

```csharp
ICalendarControl PageSize(byte value)
```

Maximum number of **notes** shown per page (valid range 0–255). `0` (default) auto-computes from
terminal height, reserving lines for header, footer, and pagination. Values above the available
height are clamped.

```csharp
PromptPlus.Controls.Calendar("Date").PageSize(3).Run();
```

---

## Highlights

### `Highlights`

```csharp
ICalendarControl Highlights(params DateTime[] dates)
```

Marks one or more dates so they stand out (rendered with the `CalendarHighlight` style). Highlighted
dates remain selectable — the marking is purely visual.

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date")
    .Highlights(today, today.AddDays(3))
    .Run();
```

> Throws `ArgumentNullException` if `dates` is `null`.

---

## Loading from a source

### `Interaction`

```csharp
ICalendarControl Interaction<T>(IEnumerable<T> items, Action<T, ICalendarControl> interactionAction)
```

Iterates a source collection and lets you configure the calendar programmatically per item — useful
for generating notes from data.

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date")
    .Interaction(myEvents, (evt, ctrl) => ctrl.AddNote(evt.Date, evt.Title))
    .Run();
```

> Throws `ArgumentNullException` if `items` or `interactionAction` is `null`.

---

### `InteractionAsync`

```csharp
ICalendarControl InteractionAsync<T>(IEnumerable<T> items, Func<T, ICalendarControl, Task> interactionAction)
```

Asynchronous version of [`Interaction`](#interaction), for sources that need awaiting per item.

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Date")
    .InteractionAsync([8, 9], async (offset, ctrl) =>
    {
        await Task.Delay(1).ConfigureAwait(false);
        ctrl.AddNote(today.AddDays(offset), $"Async note {offset}");
    })
    .Run();
```

> Throws `ArgumentNullException` if `items` or `interactionAction` is `null`.

---

## Initial value

### `Default`

```csharp
ICalendarControl Default(DateTime value, bool useDefaultHistory = true)
```

Opens the grid on `value` (default is today). When `useDefaultHistory` is `true` and
[history](#enabledhistory) is enabled, the last history value is preferred. A `value` outside the
[`Range`](#range) is ignored.

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Controls.Calendar("Date")
    .Default(DateTime.Now)
    .Run();
```

---

## Validating the confirmed date

Validation runs on **Enter**. On failure the grid stays open and shows an error.

### `PredicateSelected`

```csharp
ICalendarControl PredicateSelected(Func<DateTime?, bool> isValidSelection)
ICalendarControl PredicateSelected(Func<DateTime?, (bool, string?)> validateSelection)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<DateTime?, bool>` | `true` = valid | Generic error on failure |
| `Func<DateTime?, (bool, string?)>` | `(isValid, message)` | Custom `message` on failure |

```csharp
using PromptPlusLibrary;
using System;

// bool overload — only odd days allowed
PromptPlus.Controls.Calendar("Select odd day")
    .PredicateSelected(date => date.HasValue && date.Value.Day % 2 == 1)
    .Run();

// message overload — custom error text
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

### `PredicateSelectedAsync`

```csharp
ICalendarControl PredicateSelectedAsync(Func<DateTime?, Task<bool>> isValidSelection)
ICalendarControl PredicateSelectedAsync(Func<DateTime?, Task<(bool, string?)>> validateSelection)
```

Asynchronous counterparts of [`PredicateSelected`](#predicateselected).

```csharp
using PromptPlusLibrary;
using System;

var today = DateTime.Now.Date;
PromptPlus.Controls.Calendar("Select future date")
    .PredicateSelectedAsync(async date =>
    {
        await Task.Delay(1).ConfigureAwait(false);
        return date.HasValue && date.Value.Date >= today;
    })
    .Run();
```

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Dynamic description

### `ChangeDescription`

```csharp
ICalendarControl ChangeDescription(Func<DateTime?, string> value)
```

Recomputes the description from the **currently highlighted date** as the user navigates.

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Controls.Calendar("Date")
    .ChangeDescription(date => $"Selected day: {date:yyyy-MM-dd}")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
ICalendarControl ChangeDescriptionAsync(Func<DateTime?, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Controls.Calendar("Date")
    .ChangeDescriptionAsync(date => Task.FromResult($"Async: {date:dddd, dd MMM yyyy}"))
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

## History

### `EnabledHistory`

```csharp
ICalendarControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists confirmed dates to `filename` and can pre-select the last one (via
[`Default(..., useDefaultHistory: true)`](#default)). The `IHistoryOptions` builder is identical to
the one documented for
[Input → EnabledHistory](../input/methods.md#enabledhistory) (`MinPrefixLength`, `MaxItems`,
`ExpirationTime`, `FilterType`, `PageSize`).

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Controls.Calendar("Date")
    .Default(DateTime.Now, useDefaultHistory: true)
    .EnabledHistory("calendar-history", opt => opt.MaxItems(5))
    .Run();
```

> Throws `ArgumentNullException` if `filename` is `null`.

---

## Appearance & behavior

### `Styles`

```csharp
ICalendarControl Styles(CalendarStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here

PromptPlus.Controls.Calendar("Date")
    .Styles(CalendarStyles.Selected, new Style(Color.Green, Color.Default))
    .Run();
```

> Throws `ArgumentNullException` if `style` is `null`.

---

### `Options`

```csharp
ICalendarControl Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

```csharp
PromptPlus.Controls.Calendar("Date")
    .Options(opt =>
    {
        opt.Description("Custom options sample");
        opt.ShowTooltip(true);
        opt.EnabledAbortKey(true);
    })
    .Run();
```

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<DateTime?> Run(CancellationToken token = default)
```

Renders the grid and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<DateTime?>`](../../architecture.md#resultpromptt) — the `.Content` is `null` when
aborted.

```csharp
var result = PromptPlus.Controls.Calendar("Date").Run();
if (!result.IsAborted && result.Content.HasValue)
    PromptPlus.Console.WriteLine(result.Content.Value.ToString("d"));
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `CalendarStyles` regions
- [Index](index.md) — overview and method map
