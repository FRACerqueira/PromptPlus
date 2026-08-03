<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTasks — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [TableSelect Control →](../tableselect/index.md)

---

The `MultiTasks` control paints its output in named regions. Each region is a `MultiTasksStyles` value
you can recolor per control instance. Notably, the four task states each have their own region, so the
list reads at a glance.

> ℹ️ Styling is **per control** — there is no global "style all multi-tasks" API. Set styles on each
> control (or through a small helper you reuse). The only style-related global setting is
> `PromptPlus.Config.ContrastRatio`.

---

## The `MultiTasksStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The answer / summary line |
| `Description` | The description line under the prompt |
| `Tooltips` | The keyboard-hint line |
| `Pagination` | The page indicator when the list scrolls |
| `Spinner` | The summary spinner (shown while any task runs) |
| `ElapsedTime` | The per-task elapsed-time display |
| `WaitingTask` | A task row waiting to run |
| `RunningTask` | A task row currently running |
| `SuccessTask` | A task row that finished successfully |
| `FailedTask` | A task row that finished with a failure |
| `Error` | The error line |

---

## Recoloring a region

Use the fluent [`Styles`](methods.md#styles) method. A `Style` is a foreground color and a background
color — **there is no bold/italic/underline**. A bare `Color` is accepted as shorthand for a
foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .MultiTasks("Running setup steps")
    .Styles(MultiTasksStyles.RunningTask, Color.Cyan)                            // Color shorthand
    .Styles(MultiTasksStyles.SuccessTask, new Style(Color.Green, Color.Default)) // explicit fg + bg
    .Styles(MultiTasksStyles.FailedTask,  new Style(Color.Red,   Color.Default))
    .Run();
```

To reuse a theme across runs, wrap the styling in a helper you call for each control — the library
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
