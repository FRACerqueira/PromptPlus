<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **KeyPress — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Confirm Control →](../confirm/index.md)

---

The `KeyPress` (and [`Confirm`](../confirm/index.md)) control paints its output in named regions.
Each region is a `KeyPressStyles` value you can recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all KeyPress" API. Set styles on each
> control (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `KeyPressStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The pressed-key answer line |
| `Description` | The description line under the prompt |
| `TaggedInfo` | **Dead style — never referenced by the control.** Setting it has no visible effect |
| `Tooltips` | The key-hint line |
| `Error` | The invalid-key message line |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color and a
background color — **there is no bold/italic/underline**. A bare `Color` is accepted as shorthand
for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .KeyPress("Styled sample", "Press 1 or 2")
    .AddValidKey(ConsoleKey.D1, null, "Option 1")
    .AddValidKey(ConsoleKey.D2, null, "Option 2")
    .Styles(KeyPressStyles.Prompt, Color.Yellow)                       // Color shorthand (foreground)
    .Styles(KeyPressStyles.Description, Color.Cyan)
    .Styles(KeyPressStyles.Answer, Color.Green)
    .Styles(KeyPressStyles.TaggedInfo, Color.Blue)                      // no visible effect — dead style
    .Styles(KeyPressStyles.Tooltips, Color.Magenta)
    .Styles(KeyPressStyles.Error, new Style(Color.Red, Color.Default)) // explicit fg + bg
    .Run();
```

To reuse a theme across controls, wrap the styling in a helper you call for each control — the
library does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the
pattern.

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
- [Confirm → Styles](../confirm/styles.md) — the same regions on the yes/no preset
