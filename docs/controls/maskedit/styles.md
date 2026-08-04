<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MaskEdit — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Select Control →](../select/index.md)

---

Every MaskEdit control paints its output in named regions. Each region is a `MaskEditStyles` value you
can recolor per control instance with the fluent [`Styles`](methods.md#styles) method. All four
interfaces (string, number, currency, date/time) share the same enum.

> ℹ️ Styling is **per control** — there is no global "style all MaskEdits" API. Set styles on each
> control (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `MaskEditStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The value the user is typing / the confirmed answer |
| `Description` | The description line under the prompt |
| `Error` | The validation error line |
| `TaggedInfo` | Extra-info text (affixed with the prefix/suffix from config) |
| `Tooltips` | The keyboard-hint line |
| `NegativeValue` | The answer when it is a **negative** number |
| `PositiveValue` | The answer when it is a **positive** number |

---

## Positive / negative value coloring

`PositiveValue` and `NegativeValue` matter on the **number** and **currency** controls when a sign is
allowed (`NumberFormat(..., withsignal: true)`). The value is painted with `PositiveValue` while it is
zero or positive and with `NegativeValue` once it goes negative — a natural fit for balances and
accounting fields. When the value is unsigned, `Answer` applies.

```csharp
using ConsolePlusLibrary;   // Color, Style live here
using PromptPlusLibrary;

PromptPlus.Controls.MaskDecimal("Balance")
    .NumberFormat(6, 2, withsignal: true)
    .Styles(MaskEditStyles.PositiveValue, new Style(Color.Green, Color.Black))
    .Styles(MaskEditStyles.NegativeValue, new Style(Color.Red, Color.Black))
    .Run();
```

> For the string and date/time controls there is no signed value, so the answer uses the `Answer`
> region; `PositiveValue` / `NegativeValue` are effectively unused there.

---

## Recoloring a region

A `Style` is a foreground color, a background color, **and an `Overflow` strategy** — **there is no
bold/italic/underline `Decoration`**. Construct one with `new Style(foreground, background)`; a bare
`Color` is accepted as a foreground-only shorthand.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls.MaskEdit("Styled")
    .Mask("AAAA-9999")
    .Styles(MaskEditStyles.Prompt,     new Style(Color.Yellow, Color.Black))
    .Styles(MaskEditStyles.Answer,     new Style(Color.Green, Color.Black))
    .Styles(MaskEditStyles.TaggedInfo, new Style(Color.Cyan, Color.Black))
    .Run();
```

To reuse a theme across controls, wrap the styling in a helper you call for each control — the library
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
