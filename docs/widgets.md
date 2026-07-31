<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Widgets**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [API Documentation Guide →](api-documentation-guide.md)

---

Widgets are **output-only**. They display information — a slider bar, a calendar, a switch, a banner,
a separator, a bar chart — without reading input and without returning a `ResultPrompt`. Use them
alongside your interactive controls to present data.

There are two shapes:

- **Fluent widgets** (`Slider`, `Calendar`, `Switch`, `ChartBar`) return a configuration object.
  You chain settings and then call **`.Show()`** to render. Nothing appears until `.Show()` is called.
- **Immediate widgets** (`Banner`, `Dash`) render the moment you call the method — they return `void`.

> ⚠️ Fluent widgets render with **`.Show()`**, not `.Run()`. (`.Run()` belongs to interactive
> *controls*.)

---

## Controls vs Widgets

| | Controls (`IControls`) | Widgets (`IWidgets`) |
|---|---|---|
| Reads user input | ✅ Yes | ❌ No |
| Blocks execution | ✅ Until Enter/Esc | ❌ No |
| Returns a value | ✅ `ResultPrompt<T>` | ❌ No |
| How to render | `.Run()` | `.Show()` (fluent) or immediate (`Banner`, `Dash`) |

---

## Slider

Renders a read-only slider bar for a numeric value within a range.

```csharp
using PromptPlusLibrary;

// value 65, range 0–100, 0 fractional digits
PromptPlus.Widgets.Slider(65, 0, 100, 0).Show();
```

Signature: `Slider(double value, double minvalue = 0, double maxvalue = 100, byte fractionalDigits = 2)`.
The returned `ISliderWidget` is fluent — chain `Fill`, `Width`, `Styles`, `Culture`, `ChangeColor`,
`ChangeGradient`, or `HideElements` before `.Show()`:

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color lives here

PromptPlus.Widgets.Slider(65)
    .Width(40)
    .ChangeGradient(Color.Red, Color.Yellow, Color.Green)
    .Show();
```

---

## Calendar

Renders a read-only calendar for the month/year of a reference date (the day component is ignored).

```csharp
using PromptPlusLibrary;
using System;

PromptPlus.Widgets.Calendar(DateTime.Today).Show();
PromptPlus.Widgets.Calendar(new DateTime(2025, 12, 25)).Show();
```

The rendered calendar respects `PromptPlus.Config.FirstDayOfWeek`.

---

## Switch

Renders a read-only on/off toggle.

```csharp
using PromptPlusLibrary;

PromptPlus.Widgets.Switch(true).Show();
PromptPlus.Widgets.Switch(false).Show();
```

By default the toggle shows the localized `YesChar` / `NoChar` labels (e.g. `Y` / `N`). Override them
with `.OnValue(...)` / `.OffValue(...)` (text or emoji) before `.Show()`.

---

## Banner

Renders large decorative text using a FIGlet font. `Banner` returns `void` — it renders immediately.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

// Default font, current style
PromptPlus.Widgets.Banner("Welcome!");

// Custom color (Style = foreground + background; no bold/italic concept)
PromptPlus.Widgets.Banner("PromptPlus", new Style(Color.Cyan, Color.Default));
```

Overloads let you supply a FIGlet font by file path or stream, and a `DashOptions` border.

---

## Dash

Renders a styled text line followed by a separator rule. Returns `void` — renders immediately.

```csharp
using PromptPlusLibrary;

// Simple separator with a label
PromptPlus.Widgets.Dash("Section title");

// With extra blank lines after
PromptPlus.Widgets.Dash("Results", extralines: 1);
```

Signature: `Dash(string? value, Style? style = null, DashOptions dashOptions = DashOptions.SingleBorder, int extralines = 0, bool applycolorbackground = false)`.

> The older `SingleDash` / `DoubleDash` methods are **obsolete** — use `Dash`.

---

## ChartBar

Renders a read-only horizontal bar chart. `ChartBar()` returns a fluent `IChartBarWidget`; add items
and call `.Show()`.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Widgets.ChartBar()
    .Title("Sales")
    .AddItem("Alpha", 42, Color.Cyan)
    .AddItem("Beta",  28, Color.Green)
    .AddItem("Gamma", 15, Color.Yellow)
    .ShowLegends()
    .Show();
```

`AddItem(string label, double value, Color? colorBar = null, string? id = null)` — omit the color to
let PromptPlus assign one automatically. Set the width with `.Width(byte)` (default 50, minimum 10),
the sort with `.OrderBy(...)`, and the layout with `.Layout(...)`.

> The **widget** is display-only. If you need the interactive chart with F2/F3/F4 hot keys (switch
> layout, toggle legend, cycle order), use the [ChartBar **control**](controls/chartbar/index.md) instead.

---

## Writing styled text between widgets

For styled text output, use `PromptPlus.Console` (the same `IConsole` driver as `ConsolePlus.Driver`):

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Widgets.Dash("Report");
PromptPlus.Console.WriteLine("[bold cyan]Sales Summary[/]");   // markup handled by ConsolePlus
PromptPlus.Widgets.ChartBar()
    .AddItem("Q1", 120, Color.Cyan)
    .AddItem("Q2", 98,  Color.Blue)
    .Show();
PromptPlus.Console.WriteLine("[green]All data loaded.[/]");
```

---

## See also

- [Architecture](architecture.md) — Controls vs Widgets distinction
- [Global Behaviors](global-behaviors.md) — `ChartWidth`, `SliderWidth`, `SwitchWidth`, `FirstDayOfWeek`
- [Global Styles](global-styles.md) — the `Style` API used by widget `.Styles(...)`
- [ChartBar control](controls/chartbar/index.md) — the interactive chart (with user input)
