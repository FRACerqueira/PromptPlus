<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Table&lt;T&gt; — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiTable Control →](../multitable/index.md)

---

`Table<T>` paints its output in named regions. Each region is a `TableStyles` value you can recolor
per control instance.

> ℹ️ Styling is **per control** — there is no global "style all tables" API. The only style-related
> global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `TableStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The confirmed answer shown after `.Run()` |
| `Description` | The description line under the prompt |
| `HeaderText` | The column header row |
| `BorderLines` | The outer borders, column separators, and header separator |
| `SelectedCell` | The focused / selected row |
| `UnselectedCell` | The non-focused rows |
| `DisabledRow` | Rows marked non-selectable |
| `Pagination` | The page indicator |
| `Error` | The validation error line |
| `Tooltips` | The keyboard-hint line |
| `TaggedInfo` | The tagged-information region (e.g. the filter label) |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Table<Product>("Select a product")
    .AddColumn("Name",     x => x.Name)
    .AddColumn("Category", x => x.Category)
    .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Styles(TableStyles.BorderLines,    new Style(Color.Silver, Color.Black))
    .Styles(TableStyles.HeaderText,     new Style(Color.Cyan,   Color.Black))
    .Styles(TableStyles.SelectedCell,   new Style(Color.Black,  Color.Cyan))
    .Styles(TableStyles.UnselectedCell, new Style(Color.White,  Color.Black))
    .Styles(TableStyles.DisabledRow,    new Style(Color.Silver, Color.Black))
    .Run();
```

To reuse a theme across tables, wrap the styling in a helper you call for each control — the library
does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the pattern.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with `PromptPlus.Config.ContrastRatio`.

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Methods → Styles](methods.md#styles)
- [MultiTable → Styles](../multitable/styles.md) — the `MultiTableStyles` regions
