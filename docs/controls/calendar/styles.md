<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Calendar — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Timer Control →](../timer/index.md)

---

`Calendar` paints its output in named regions. Each region is a `CalendarStyles` value you can
recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all calendars" API. The only style-related
> global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `CalendarStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The confirmed answer shown after `.Run()` |
| `Description` | The description line under the prompt |
| `Selected` | The highlighted (cursor) day |
| `UnSelected` | The non-focused days |
| `Disabled` | Days blocked by [`DisableDates`](methods.md#disabledates) / [`DisabledWeekend`](methods.md#disabledweekend) or outside [`Range`](methods.md#range) |
| `Error` | The validation error line |
| `Pagination` | The notes page indicator |
| `TaggedInfo` | **Dead style — never referenced by the control.** Notes text actually renders with `Selected`/`UnSelected`, matching the day's own highlight state, not a dedicated region |
| `Tooltips` | The keyboard-hint line |
| `Lines` | The grid lines |
| `CalendarHighlight` | Days marked by [`Highlights`](methods.md#highlights) |
| `CalendarDay` | The day numbers in the grid |
| `CalendarMonth` | The month name in the header |
| `CalendarYear` | The year in the header |
| `CalendarWeekDay` | The week-day header row |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Calendar("Date")
    .Styles(CalendarStyles.Lines,             Color.Blue)
    .Styles(CalendarStyles.Selected,          Color.Green)
    .Styles(CalendarStyles.CalendarDay,       Color.Yellow)
    .Styles(CalendarStyles.CalendarHighlight, Color.Blue)
    .Styles(CalendarStyles.CalendarMonth,     Color.Green)
    .Styles(CalendarStyles.CalendarWeekDay,   Color.Aqua)
    .Styles(CalendarStyles.CalendarYear,      Color.Violet)
    .Styles(CalendarStyles.Selected,          new Style(Color.Green, Color.Default))
    .Run();
```

To reuse a theme across calendars, wrap the styling in a helper you call for each control — the
library does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the
pattern.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with `PromptPlus.Config.ContrastRatio`.

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Visual Symbols](../../visual-symbols.md) — the markers these styles color
- [Methods → Styles](methods.md#styles)
