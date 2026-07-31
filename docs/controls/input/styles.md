<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Input — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Secret Control →](../secret/index.md)

---

The `Input` (and [`Secret`](../secret/index.md)) control paints its output in named regions. Each
region is an `InputStyles` value you can recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all inputs" API. Set styles on each
> control (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `InputStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The value the user is typing / the confirmed answer |
| `Description` | The description line under the prompt |
| `Suggestion` | Autocomplete suggestion text |
| `Selected` | The highlighted suggestion / history entry |
| `UnSelected` | Non-highlighted suggestions / history entries |
| `Error` | The validation error line |
| `Pagination` | The page indicator during history / suggestion navigation |
| `TaggedInfo` | Extra-info text (affixed with the prefix/suffix from config) |
| `Tooltips` | The keyboard-hint line |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Input("Name")
    .Styles(InputStyles.Answer, Color.Green)                          // Color shorthand (foreground)
    .Styles(InputStyles.Error,  new Style(Color.Red, Color.Default))  // explicit fg + bg
    .Run();
```

To reuse a theme across inputs, wrap the styling in a helper you call for each control — the library
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
