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
| Focus cursor | `>` | `►` | Focused item in Select / MultiSelect / Tree |
| Checkbox — checked | `[x]` | `[■]` | Selected item in MultiSelect |
| Checkbox — unchecked | `[ ]` | `[ ]` | Unselected item in MultiSelect |
| Checkbox — partial (tri-state) | `[?]` | `[▪]` | Indeterminate node in MultiTree |
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

---

## Bars (Progress, Slider, Chart)

By **default**, the progress bar, slider, and chart bar render their filled portion as a **colored
space** (the color carries the meaning). The glyphs below are used only when you pick an alternate
bar style (e.g. `SliderBarType.Square`, `ProgressBarType.Square`).

| Bar style | ASCII | Unicode |
|---|---|---|
| Square (solid block) | `#` | `█` |
| Light | `-` | `─` |
| Double-light | `=` | `═` |
| Dot | `.` | `∙` |
| Half bar | `\|` | `▌` |
| Chart label swatch | `#` | `■` |

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
