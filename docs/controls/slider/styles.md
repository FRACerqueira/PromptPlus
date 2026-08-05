<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Slider — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Calendar Control →](../calendar/index.md)

---

The `Slider` control paints its output in named regions. Each region is a `SliderStyles` value you
can recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all sliders" API. Set styles on each
> control (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `SliderStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The formatted numeric value |
| `Description` | The description line under the prompt |
| `Tooltips` | The keyboard-hint line |
| `Slider` | The slider bar itself |
| `Ranger` | The min/max range display |
| `Selected` | Unused — see note below |
| `UnSelected` | Unused — see note below |
| `Pagination` | Unused — see note below |
| `Error` | The error line |

> The bar color is usually driven dynamically by [`ChangeColor`](methods.md#changecolor) or
> [`ChangeGradient`](methods.md#changegradient); use the `Slider` region for a static bar color.

> ⚠️ `Selected`, `UnSelected`, and `Pagination` exist on the `SliderStyles` enum but are never
> painted — unlike Input/Secret/Select, Slider has **no interactive history list** (see
> [Operations → History](operations.md#history)), so setting these has no visible effect.

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is
accepted as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Slider("Select value")
    .Styles(SliderStyles.Prompt, new Style(Color.Aqua, Color.Black))
    .Styles(SliderStyles.Answer, new Style(Color.Green, Color.Black))
    .Styles(SliderStyles.Slider, new Style(Color.Blue, Color.Black))
    .Styles(SliderStyles.Ranger, Color.Cyan)                 // Color shorthand (foreground)
    .Run();
```

To reuse a theme across sliders, wrap the styling in a helper you call for each control — the
library does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the pattern.

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
