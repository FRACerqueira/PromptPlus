<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **TreeMultiSelect&lt;T&gt; — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ChartBar Control →](../chartbar/index.md)

---

`TreeMultiSelect<T>` paints its output in named regions. Each region is a `TreeMultiSelectStyles` value you can
recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all trees" API. The only style-related
> global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `TreeMultiSelectStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The confirmed answer shown after `.Run()` |
| `Description` | The description line under the prompt |
| `Selected` | The focused node (cursor) |
| `UnSelected` | The non-focused nodes |
| `Disabled` | Nodes that cannot be checked (e.g., containers under `CheckLeafOnly`, or nodes added with `disable: true`) |
| `Error` | The validation / range error line |
| `Pagination` | The page indicator |
| `TaggedInfo` | The tagged-information region (e.g. the filter label) |
| `Tooltips` | The keyboard-hint line |
| `Lines` | The tree connector lines |
| `ExpandSymbol` | The expand / collapse indicator on containers |
| `Root` | The root node |
| `Node` | Container / leaf entries |
| `ChildsCount` | The extra-info column rendered next to each node ([`ExtraInfo`](methods.md#extrainfo)) |

> This is the same set as [`TreeSelectStyles`](../treeselect/styles.md#the-treestyles-regions) plus `TaggedInfo`,
> which colors the filter-label tag unique to the multi-selection control (shown while
> [`Filter`](methods.md#filter) mode is active), matching `TaggedInfo`'s role on
> [`MultiSelect`](../multiselect/styles.md#the-multiselectstyles-regions) and
> [`TableMultiSelect`](../tablemultiselect/styles.md#the-tablemultiselectstyles-regions).

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .TreeMultiSelect<Node>("Nodes")
    .Root(company)
    .TextSelector(n => n.Name)
    .ExtraInfo(n => n.Info)
    .DefaultMatchBy((a, b) => a.Id == b.Id)
    .Styles(TreeMultiSelectStyles.Prompt,      new Style(Color.Yellow,  Color.Black))
    .Styles(TreeMultiSelectStyles.Root,        new Style(Color.Magenta, Color.Black))
    .Styles(TreeMultiSelectStyles.Node,        new Style(Color.Cyan,    Color.Black))
    .Styles(TreeMultiSelectStyles.ChildsCount, new Style(Color.Gray,    Color.Black))
    .Styles(TreeMultiSelectStyles.Selected,    new Style(Color.Black,   Color.Gray))
    .Run();
```

To reuse a theme across trees, wrap the styling in a helper you call for each control — the library
does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the pattern.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with `PromptPlus.Config.ContrastRatio`.

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Visual Symbols](../../visual-symbols.md) — the focus arrow, checkboxes, and expand markers these styles color
- [Methods → Styles](methods.md#styles)
- [TreeSelect → Styles](../treeselect/styles.md) — the single-choice sibling
