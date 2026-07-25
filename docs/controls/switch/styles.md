<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Switch — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ProgressBar Control →](../progressbar/index.md)

---

The `Switch` control paints its output in named regions. Each region is a `SwitchStyles` value you
can recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all switches" API. Set styles on each
> control (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `SwitchStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The confirmed state |
| `Description` | The description line under the prompt |
| `Tooltips` | The keyboard-hint line |
| `Slider` | The toggle track |
| `Ranger` | The state delimiters |
| `SwitchOn` | The label/region when the state is **on** |
| `SwitchOff` | The label/region when the state is **off** |
| `Error` | The error line |

> ⚠️ The on/off region names are spelled `SwitchOn` and `SwitchOff` in the enum (no second `c`) — use
> them exactly as shown.

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color and a
background color — **there is no bold/italic/underline**. A bare `Color` is accepted as shorthand
for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Switch("Use cache")
    .Styles(SwitchStyles.SwitchOn,  new Style(Color.Black, Color.Darkgreen))
    .Styles(SwitchStyles.SwitchOff, new Style(Color.Black, Color.Darkred))
    .Styles(SwitchStyles.Prompt,   Color.Yellow)              // Color shorthand (foreground)
    .Run();
```

To reuse a theme across switches, wrap the styling in a helper you call for each control — the
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
