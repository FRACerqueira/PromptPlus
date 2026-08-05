<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Task — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiTasks Control →](../multitasks/index.md)

---

The `Task` control paints its output in named regions. Each region is a `TaskStyles` value you can
recolor per control instance.

> ℹ️ Styling is **per control** — there is no global "style all tasks" API. Set styles on each control
> (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `TaskStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The answer / result line |
| `Description` | The description line (including [`ChangeDescription`](methods.md#changedescription) output) |
| `Tooltips` | The keyboard-hint line |
| `Spinner` | The animated spinner |
| `ElapsedTime` | The elapsed-time display |
| `Error` | The error line (shown when the task throws) |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color, a background
color, and an `Overflow` strategy — **there is no bold/italic/underline**. A bare `Color` is accepted
as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Task("Please wait")
    .ShowElapsedTime()
    .Spinner(SpinnersType.Default)
    .Styles(TaskStyles.Prompt,      new Style(Color.Yellow, Color.Black))
    .Styles(TaskStyles.ElapsedTime, new Style(Color.Cyan,   Color.Black))
    .Styles(TaskStyles.Spinner,     new Style(Color.Green,  Color.Black))
    .ActionAsync(async t => await Task.Delay(2000, t))
    .Run();
```

To reuse a theme across tasks, wrap the styling in a helper you call for each control — the library
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
