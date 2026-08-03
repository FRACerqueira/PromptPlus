<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Styles**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Widgets →](widgets.md)

---

PromptPlus colors every control through the `Style` type from ConsolePlus. This page explains what a
`Style` is, how to apply one to a control, and the one setting that affects styling globally
(contrast).

> ℹ️ **There is no global "set this style for all controls" API.** Styling is applied **per control
> instance** via each control's `.Styles(...)` method. The only style-related global setting is
> `ContrastRatio`. (Earlier drafts referenced a `PromptPlus.Config.Styles(...)` method and a
> `PromptPlusStyles` enum — neither exists in the library.)

---

## What is a `Style`?

A `Style` is a small immutable value with three parts:

```csharp
public readonly struct Style(Color foreground, Color background, Overflow overflowStrategy = Overflow.None)
```

| Component | Type | Meaning |
|---|---|---|
| `Foreground` | `Color` | Text color |
| `Background` | `Color` | Color behind the text |
| `OverflowStrategy` | `Overflow` | What to do when content is wider than the line (`None`, `Crop`, `Ellipsis`) |

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style, Overflow live here

// Foreground + background
var s1 = new Style(Color.White, Color.Navy);

// A Color converts implicitly to a foreground-only Style (background = terminal default)
Style s2 = Color.Lime;
```

> ⚠️ There is **no text-decoration** concept (no bold / italic / underline) — a `Style` is colors
> plus an overflow strategy. See [ConsolePlus → Styles & Overflow](../../ConsolePlus/docs/styles.md)
> for the full `Style` API, builder helpers (`ForeGround`, `Background`, `Overflow`, `Colors`), and
> the `Overflow` strategies.

---

## Styling a control

Every control exposes a fluent `.Styles(<ControlEnum>, style)` method. Each control has its own enum
of regions, and each region is styled independently. A bare `Color` is accepted as shorthand for a
foreground-only style.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls
    .Select<string>("Pick one")
    .AddItems(["Alpha", "Beta", "Gamma"])
    .Styles(SelectStyles.Selected, Color.Yellow)                  // Color shorthand
    .Styles(SelectStyles.Prompt,   new Style(Color.White, Color.Default))
    .Run();
```

Apply the same style to every control by setting it in a small helper (or a config-loading routine)
that you call for each control — the library does not broadcast a style for you.

---

## Per-control style enums

Each control defines its own regions. The exact list lives on that control's **Styles** page; common
region names include `Prompt`, `Answer`, `Description`, `Selected`, `UnSelected`, `Disabled`,
`Error`, `Pagination`, `Tooltips`.

| Control | Style enum |
|---|---|
| Input / Secret | `InputStyles` |
| KeyPress / Confirm | `KeyPressStyles` |
| Select | `SelectStyles` |
| MultiSelect | `MultiSelectStyles` |
| MaskEdit family | `MaskEditStyles` |
| Calendar | `CalendarStyles` |
| Slider | `SliderStyles` |
| Switch | `SwitchStyles` |
| ProgressBar | `ProgressBarStyles` |
| Task | `TaskStyles` |
| MultiTasks | `MultiTasksStyles` |
| Timer | `TimerStyles` |
| TableSelect | `TableSelectStyles` |
| TableMultiSelect | `TableMultiSelectStyles` |
| TreeSelect | `TreeSelectStyles` |
| TreeMultiSelect | `TreeMultiSelectStyles` |
| File | `FileStyles` |
| MultiFile | `MultiFileStyles` |
| ChartBar | `ChartBarStyles` |

---

## Contrast enforcement (the one global style setting)

PromptPlus enforces a minimum contrast ratio between foreground and background so text stays readable
on any terminal theme. When a chosen foreground falls below the ratio against its background, it is
adjusted automatically.

```csharp
// Default is 2.7
PromptPlus.Config.ContrastRatio = 0;     // disable enforcement (use any colors as-is)
PromptPlus.Config.ContrastRatio = 4.5;   // WCAG AA target
```

> A ratio of `0` disables the check entirely. Higher values improve readability but may not be
> achievable with every color pair, in which case PromptPlus picks the closest compliant color.

---

## Practical example — a consistent theme

Because there is no global style broadcast, centralize your theme in one place and apply it per
control:

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

static ISelectControl<T> Themed<T>(ISelectControl<T> c) => c
    .Styles(SelectStyles.Prompt,   Color.Cyan)
    .Styles(SelectStyles.Answer,   Color.Green)
    .Styles(SelectStyles.Tooltips, Color.Grey);

var result = Themed(PromptPlus.Controls.Select<string>("Color").AddItems(colors)).Run();
```

---

## See also

- [ConsolePlus → Styles & Overflow](../../ConsolePlus/docs/styles.md) — the full `Style` API and `Overflow` strategies
- [Visual Symbols](visual-symbols.md) — the glyphs these styles color
- [Global Behaviors](global-behaviors.md) — `ContrastRatio` and the rest of `PromptPlus.Config`
