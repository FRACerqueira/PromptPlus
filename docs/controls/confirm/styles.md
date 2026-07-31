<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Confirm — Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Input Control →](../input/index.md)

---

`Confirm` paints its output through the same `KeyPressStyles` regions as
[KeyPress](../keypress/index.md), because it *is* a KeyPress control. The full region list and
recoloring guidance live on the [KeyPress → Styles](../keypress/styles.md) page; this page is a short
pointer.

> ℹ️ Styling is **per control** — there is no global "style all Confirm" API. The only style-related
> global setting is `PromptPlus.Config.ContrastRatio`.

---

## The `KeyPressStyles` regions

| Region | What it paints |
|---|---|
| `Prompt` | The prompt text |
| `Answer` | The pressed-key answer line |
| `Description` | The description line under the prompt |
| `TaggedInfo` | Extra-info text (affixed with the prefix/suffix from config) |
| `Tooltips` | The Yes/No key-hint line |
| `Error` | The invalid-key message line |

---

## Recoloring a region

Use the fluent [`Styles`](../keypress/methods.md#styles) method exactly as on KeyPress. A `Style` is a
foreground color and a background color — **there is no bold/italic/underline**. A bare `Color` is
accepted as shorthand for a foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Confirm("Apply changes?")
    .Styles(KeyPressStyles.Prompt, Color.Yellow)                       // Color shorthand
    .Styles(KeyPressStyles.Error, new Style(Color.Red, Color.Default)) // explicit fg + bg
    .Run();
```

---

## See also

- [KeyPress → Styles](../keypress/styles.md) — the full region list, examples, and contrast notes
- [Global Styles](../../global-styles.md) — the `Style` type, per-control styling, contrast
- [Index](index.md) — the yes/no pattern
- [Methods](methods.md) — the shared API
