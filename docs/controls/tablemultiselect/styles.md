<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **TableMultiSelect&lt;T&gt; — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [TreeSelect Control →](../treeselect/index.md)

---

`MultiTable<T>` paints its output in named regions. Each region is a `TableMultiSelectStyles` value you can
recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all multi-tables" API. The only style-related
> global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `TableMultiSelectStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The answer region (shows the selected-items count) |
| `Description` | The description line under the prompt |
| `HeaderText` | The column header row |
| `BorderLines` | The outer borders, column separators, and header separator |
| `SelectedCell` | The focused (cursor) row — the checkbox mark and all cell text |
| `UnselectedCell` | The non-focused rows — the checkbox mark and all cell text |
| `DisabledRow` | Rows that cannot be toggled — the checkbox mark and all cell text |
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
    .TableMultiSelect<Product>("Select products")
    .AddColumn("Name",     x => x.Name)
    .AddColumn("Category", x => x.Category)
    .AddColumn("Price",    x => x.Price, v => $"${v:N2}", alignment: ColumnAlignment.Right)
    .AddItems(products)
    .Styles(TableMultiSelectStyles.BorderLines,    new Style(Color.Silver, Color.Black))
    .Styles(TableMultiSelectStyles.HeaderText,     new Style(Color.Cyan,   Color.Black))
    .Styles(TableMultiSelectStyles.SelectedCell,   new Style(Color.Black,  Color.Cyan))
    .Styles(TableMultiSelectStyles.UnselectedCell, new Style(Color.White,  Color.Black))
    .Styles(TableMultiSelectStyles.DisabledRow,    new Style(Color.Silver, Color.Black))
    .Run();
```

To reuse a theme across multi-tables, wrap the styling in a helper you call for each control — the
library does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the pattern.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with `PromptPlus.Config.ContrastRatio`.

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Methods → Styles](methods.md#styles)
- [Table → Styles](../tableselect/styles.md) — the `TableSelectStyles` regions
