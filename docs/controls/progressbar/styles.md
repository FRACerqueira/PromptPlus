<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ProgressBar — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Task Control →](../task/index.md)

---

The `ProgressBar` control paints its output in named regions. Each region is a `ProgressBarStyles`
value you can recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all progress bars" API. Set styles on each
> control (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `ProgressBarStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The current value shown next to the prompt |
| `Description` | The description line (including [`ChangeDescription`](methods.md#changedescription) output) |
| `TaggedInfo` | Extra-info text (affixed with the prefix/suffix from config) |
| `Tooltips` | The keyboard-hint line |
| `Spinner` | The animated spinner |
| `Slider` | The bar track / filled portion |
| `Ranger` | The min/max range text |
| `Error` | The error line |

> The `Slider` color is what [`ChangeColor`](methods.md#changecolor) and
> [`ChangeGradient`](methods.md#changegradient) override dynamically per value. A static
> `Styles(ProgressBarStyles.Slider, …)` sets the base color when no dynamic color is configured.

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color and a background
color — **there is no bold/italic/underline**. A bare `Color` is accepted as shorthand for a
foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .ProgressBar("Wait Progress: ")
    .Styles(ProgressBarStyles.Slider, new Style(Color.Green, Color.Default))   // bar color
    .Styles(ProgressBarStyles.Ranger, Color.Grey)                             // range text
    .Styles(ProgressBarStyles.Error,  new Style(Color.Red, Color.Default))    // error line
    .UpdateHandler(Work)
    .Run();
```

To reuse a theme across bars, wrap the styling in a helper you call for each control — the library
does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the pattern.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with:

```csharp
PromptPlus.Config.ContrastRatio = 0;     // disable
PromptPlus.Config.ContrastRatio = 4.5;   // WCAG AA target
```

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Methods → Styles](methods.md#styles)
- [Methods → ChangeColor / ChangeGradient](methods.md#changecolor) — dynamic slider coloring
