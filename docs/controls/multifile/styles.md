<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiFile — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Slider Control →](../slider/index.md)

---

`MultiFile` paints its output in named regions. Each region is a `MultiFileStyles` value you can
recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all file browsers" API. The only
> style-related global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `MultiFileStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The confirmed answer shown after `.Run()` |
| `Description` | The description line under the prompt |
| `Selected` | The focused / selected entry |
| `UnSelected` | The non-focused entries |
| `Error` | The error line |
| `Pagination` | The page indicator |
| `TaggedInfo` | The checked-count / tagged summary |
| `Tooltips` | The keyboard-hint line |
| `Lines` | The tree connector lines |
| `ExpandSymbol` | The expand / collapse symbol on folders |
| `FileRoot` | The root folder header |
| `FileTypeFolder` | Folder entries |
| `FileTypeFile` | File entries |
| `FileSize` | The file-size column |

> Compared with [`FileStyles`](../file/styles.md#the-filestyles-regions), `MultiFileStyles` adds the
> `TaggedInfo` region for the checked-count summary.

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .MultiFile("Styled browser")
    .Root(@"C:\Projects")
    .Styles(MultiFileStyles.Prompt,         new Style(Color.Yellow, Color.Black))
    .Styles(MultiFileStyles.FileTypeFolder, new Style(Color.Cyan, Color.Black))
    .Styles(MultiFileStyles.FileTypeFile,   new Style(Color.White, Color.Black))
    .Styles(MultiFileStyles.FileSize,       new Style(Color.Gray, Color.Black))
    .Styles(MultiFileStyles.Selected,       new Style(Color.Black, Color.Gray))
    .Run();
```

To reuse a theme across browsers, wrap the styling in a helper you call for each control — the
library does not broadcast styles for you. See [Global Styles](../../global-styles.md) for the
pattern.

---

## Contrast enforcement

PromptPlus nudges foreground colors that fall below the configured contrast ratio so text stays
readable on any terminal theme. Tune or disable with `PromptPlus.Config.ContrastRatio`.

---

## See also

- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [ConsolePlus → Styles & Overflow](../../../../ConsolePlus/docs/styles.md) — full `Style` API and `Overflow`
- [Visual Symbols](../../visual-symbols.md) — the focus arrow, checkboxes, and tree symbols these styles color
- [File → Styles](../file/styles.md) — the single-selection sibling (no `TaggedInfo` region)
- [Methods → Styles](methods.md#styles)
