<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Select&lt;T&gt; — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiSelect Control →](../multiselect/index.md)

---

`Select<T>` paints its output in named regions. Each region is a `SelectStyles` value you can recolor
per control instance.

> ℹ️ Styling is **per control** — there is no global "style all selects" API. The only style-related
> global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `SelectStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The confirmed answer shown after `.Run()` |
| `Description` | The description line under the prompt |
| `Selected` | The focused / selected item |
| `UnSelected` | The non-focused items |
| `Disabled` | Items marked non-selectable |
| `Error` | The validation error line |
| `Pagination` | The page indicator |
| `TaggedInfo` | The extra-info text ([`ExtraInfo`](methods.md#extrainfo)) |
| `Tooltips` | The keyboard-hint line |
| `Lines` | Separator lines ([`AddSeparator`](methods.md#addseparator)) |
| `GroupTip` | The current-group hint for grouped lists |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Select<string>("City")
    .AddItems(["Seattle", "London", "Tokyo"])
    .Styles(SelectStyles.Prompt,      Color.Yellow)
    .Styles(SelectStyles.Description,  Color.Red)
    .Styles(SelectStyles.Selected,     new Style(Color.Blue, Color.Default))
    .Run();
```

To reuse a theme across selects, wrap the styling in a helper you call for each control — the library
does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the pattern.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with `PromptPlus.Config.ContrastRatio`.

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Visual Symbols](../../visual-symbols.md) — the focus arrow and item markers these styles color
- [Methods → Styles](methods.md#styles)
