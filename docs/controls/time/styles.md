<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Time — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Switch Control →](../switch/index.md)

---

The `Time` (countdown) control paints its output in named regions. Each region is a `TimeStyles` value
you can recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all timers" API. Set styles on each control
> (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `TimeStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The time value (the countdown / count-up number) |
| `Description` | The description line (including [`ChangeDescription`](methods.md#changedescription) output) |
| `Tooltips` | The keyboard-hint line |
| `Spinner` | The animated spinner |
| `Error` | The error line |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color and a background
color — **there is no bold/italic/underline**. A bare `Color` is accepted as shorthand for a
foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Time("Please wait")
    .Duration(5)
    .Styles(TimeStyles.Prompt, new Style(Color.Yellow, Color.Black))
    .Styles(TimeStyles.Answer, new Style(Color.Green,  Color.Black))
    .Run();
```

To reuse a theme across timers, wrap the styling in a helper you call for each control — the library
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
