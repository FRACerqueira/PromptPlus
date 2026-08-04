<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Visual Symbols**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Spinners →](spinners.md)

---

PromptPlus draws state and structure with a small set of glyphs. Each glyph has **two forms** — a
Unicode form and an ASCII fallback — and PromptPlus picks the right one automatically based on what
your terminal can render.

> The catalog below lists the real defaults from the library's internal symbol table. The glyphs
> themselves are internal (not individually configurable). The two characters you *can* change are the
> Secret mask and the MaskEdit placeholder — see [Configurable characters](#configurable-characters).

---

## How the ASCII fallback works

For every glyph, PromptPlus stores a pair `(ascii, unicode)`. At render time it returns the Unicode
form only when **both** conditions hold:

1. the terminal reports Unicode support (`Console.SupportsUnicode`), **and**
2. the active output encoding can actually encode that specific glyph.

Otherwise it returns the ASCII form. Detection is automatic and transparent — you do not configure
anything.

---

## List & selection

| Meaning | ASCII | Unicode | Where shown |
|---|---|---|---|
| Focus cursor | `>` | `►` | Focused item — used far more broadly than just Select/MultiSelect/TreeSelect: also TableSelect, TableMultiSelect, MultiFile, ChartBar, Calendar, Input (suggestions), MultiTasks, and other list-like controls |
| Checkbox — checked | `[x]` | `[■]` | Selected item in any multi-select-style control (MultiSelect, TableMultiSelect, TreeMultiSelect, MultiFile) |
| Checkbox — unchecked | `[ ]` | `[ ]` | Unselected item, same set of controls |
| Checkbox — partial (tri-state) | `[?]` | `[▪]` | Indeterminate node in TreeMultiSelect |
| Checkbox — busy/loading | `[*]` | `[◔]` | Item whose children are loading |

> There is **no** `●` / `○` / `›` / `✓` used for list selection — those glyphs belong to task status
> (below) or are not used at all.

---

## Tree & expand / collapse

| Meaning | ASCII | Unicode |
|---|---|---|
| Expanded node | `[-]` | `[▼]` |
| Collapsed node | `[+]` | `[►]` |
| Tree branch (crossing) | ` \|-` | ` ├─` |
| Tree branch (corner) | ` \|_` | ` └─` |
| Tree vertical line | ` \| ` | ` │ ` |
| Tree indent (no line) | `   ` | `   ` |

Blank 3-column indent, used where no vertical line continues at that depth.

---

## Bars (Progress, Slider, Chart)

By **default**, the progress bar, slider, and chart bar render their filled portion as a **colored
space** (the color carries the meaning). The glyphs below are used only when you pick an alternate
bar style (e.g. `SliderBarType.Square`, `ProgressBarType.Square`) — and **not every style exists on
every control**:

| Bar style | ASCII | Unicode | ProgressBar | Slider | ChartBar |
|---|---|---|:---:|:---:|:---:|
| Fill (colored space, default) | ` ` | ` ` | ✅ | ✅ | ✅ |
| Light | `-` | `─` | ✅ | ✅ | ✅ |
| Square (solid block) | `#` | `█` | ✅ | ✅ | ✅ |
| Double-light | `=` | `═` | ✅ | ✅ | ❌ |
| Dot | `.` | `∙` | ✅ | ✅ | ❌ |
| Bar (half bar) | `\|` | `▌` | ✅ | ❌ | ❌ |

`ChartBarType`, `SliderBarType`, and `ProgressBarType` are three separate enums — the styles they
have in common (`Fill`/`Light`/`Square`) render with the identical glyphs shown above, but
`ChartBarType` doesn't have `DoubleLight`/`Dot` members at all, and `SliderBarType` doesn't have
`Bar`. Only `ProgressBarType` has all of them.

The chart's item-label swatch is a separate glyph from any of these bar styles: `#` (ASCII) /
`■` (Unicode).

> There is **no** `░` "empty" glyph — empty bar space is just an un-colored space.

---

## Calendar

| Meaning | ASCII | Unicode |
|---|---|---|
| Today / current date (wraps the day) | `<dd>` | `◄dd►` |
| Highlighted date | `!` | `*` |
| Date-with-note marker | `*` | `∙` |
| Highlighted note marker | `#` | `♦` |

---

## Task status

Used by the Task and MultiTasks controls.

| Status | ASCII | Unicode |
|---|---|---|
| Waiting | (space) | `○` |
| Running | `>` | `◐` |
| Success | `v` | `●` |
| Failed | `x` | `✗` |
| Done | `V` | `√` |
| Error | `!` | `‼` |
| Canceled | `x` | `×` |

---

## Borders & grid

Tables and framed output use box-drawing glyphs that fall back to `+ - = | `. For example the single
border corner is `+`/`┌…┐` and the double border is `=`/`╔…╗`. These are chosen automatically per the
[fallback rule](#how-the-ascii-fallback-works).

---

## Overflow indicator

When a single-line value is wider than the terminal, PromptPlus appends an overflow indicator:

| ASCII | Unicode |
|---|---|
| `_` | `…` |

The truncation is display-only — `result.Content` always holds the full, untruncated value.

---

## Configurable characters

Two characters are exposed on [`PromptPlus.Config`](global-behaviors.md):

| Character | Default | Config property | Where shown |
|---|---|---|---|
| Secret mask | `#` | `SecretChar` | Each character typed into a Secret control |
| MaskEdit placeholder | `_` | `PromptMaskEdit` | Empty positions in MaskEdit controls |

```csharp
PromptPlus.Config.SecretChar = '*';
PromptPlus.Config.PromptMaskEdit = '.';
```

---

## Relationship to styles

A symbol's **color** comes from the [style](global-styles.md) of the region it appears in (focus,
selected, disabled, and so on). You change the color by styling that region — the glyph character
itself does not change.

---

## See also

- [Global Styles](global-styles.md) — color the regions these glyphs appear in
- [Global Behaviors](global-behaviors.md) — `SecretChar`, `PromptMaskEdit`
- [Spinners](spinners.md) — animated progress indicators (they also fall back to ASCII on non-Unicode terminals)
