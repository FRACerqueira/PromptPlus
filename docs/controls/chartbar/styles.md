<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ChartBar — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** 

---

`ChartBar` paints its output in named regions. Each region is a `ChartBarStyles` value you can
recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all charts" API. The only style-related
> global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `ChartBarStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Error` | The validation error line |
| `Selected` | The highlighted / current chart item |
| `ChartLabel` | The item labels |
| `ChartValue` | The numeric values |
| `ChartPercent` | The percentage values |
| `ChartTitle` | The chart title ([`Title`](methods.md#title)) |
| `ChartOrder` | The sort-order indicator |
| `Answer` | The confirmed answer shown after `.Run()` |
| `Pagination` | The page indicator |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .ChartBar("Select item")
    .AddItem("North", 120).AddItem("South", 80).AddItem("East", 95)
    .Title("Sales by Region")
    .Styles(ChartBarStyles.ChartTitle,   Color.Yellow)
    .Styles(ChartBarStyles.ChartPercent, Color.Cyan)
    .Styles(ChartBarStyles.Selected,     new Style(Color.Blue, Color.Default))
    .Run();
```

To reuse a theme across charts, wrap the styling in a helper you call for each control — the library
does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the pattern.

> Bar colors themselves come from [`AddItem`](methods.md#additem)'s `colorBar` argument (or the
> automatic rotating sequence), not from `ChartBarStyles`. Use `Styles` for the labels, values,
> percentages, title, and other text regions.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with `PromptPlus.Config.ContrastRatio`.

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Visual Symbols](../../visual-symbols.md) — the highlight marker these styles color
- [Methods → Styles](methods.md#styles)
