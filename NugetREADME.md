# PromptPlus

**A modern .NET library that delivers polished, interactive console experiences - text input, searchable lists, masked fields, date/time pickers, file browsers, progress bars, charts and more - all through one sleek fluent API.**

[![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)


---

PromptPlus transforms your console apps with **20+ interactive controls** and **6 output-only widgets**, configured through a readable fluent API. It supports two-layer configuration (global defaults + per-control overrides), abort-anywhere with `IsAborted` flag, history persistence, terminal resize detection, and 11 built-in locales.

> PromptPlus is built on top of [ConsolePlus](https://www.nuget.org/packages/ConsolePlus) and shares the same `IConsole` driver for styled output.


## Key Features

- **20+ interactive controls** - from key-press to multi-column tables and tree browsers
- **6 output-only widgets** - sliders, calendars, banners, charts without blocking
- **Fluent API** - every control configured with readable method chains
- **Two-layer config** - set defaults with `PromptPlus.Config`, override per control with `.Options()`
- **Abort anywhere** - Esc aborts any control; result carries an `IsAborted` flag
- **History persistence** - last confirmed value saved and pre-loaded automatically
- **Terminal-safe** - auto-detects size, re-renders on resize, enforces 80x10 minimum
- **Localization** - 11 built-in locales selected from current culture
- **Cross-platform** - Windows, Linux, macOS; .NET 8, 9 and 10

## Installation

```bash
dotnet add package PromptPlus
```

Or via the Package Manager Console:

```powershell
Install-Package PromptPlus
```

**Supported frameworks:** `.NET 8`, `.NET 9`, `.NET 10`

## Quick Start

```csharp
using PromptPlusLibrary;

// Ask for a name
var nameResult = PromptPlus.Controls.Input("Your name").Run();
if (nameResult.IsAborted) return;

// Choose a color
var colorResult = PromptPlus.Controls
    .Select<string>("Favorite color")
    .AddItems(["Red", "Green", "Blue"])
    .Run();

// Deconstruct result
var (color, aborted) = colorResult;
if (!aborted)
    PromptPlus.Console.WriteLine($"Hello {nameResult.Content}, you chose {color}!");
```

> **Tip:** Every control returns `ResultPrompt<T>`. Use `.Content` for the value, `.IsAborted` to detect Esc, or deconstruct with `var (value, aborted) = result`.

## Two-Layer Configuration

```csharp
// Layer 1 - global defaults applied to all controls
PromptPlus.Config.PageSize = 8;
PromptPlus.Config.HideAfterFinish = true;

// Layer 2 - per-control override always wins
PromptPlus.Controls
    .Input("Notes")
    .Options(o => o.HideAfterFinish(false).ShowTooltip(false))
    .Run();
```

## Controls

Input, Secret, KeyPress, Confirm, Select / MultiSelect, Table / MultiTable, Tree / MultiTree, File / MultiFile, Calendar, Slider, Switch, Time, ProgressBar, Task / MultiTasks, ChartBar and a full family of Mask editors (string, integer, long, decimal, double, currency, date, time, DateOnly, TimeOnly).

## Widgets (output-only)

Slider, Calendar, Switch, Banner, Dash separator, ChartBar.

## Localization

11 built-in locales: English (default), pt-BR, de-DE, es-ES, fr-FR, it-IT, ja-JP, ko-KR, nl-BE, ru-RU, zh-CN. Custom locales supported via satellite resource files.

---

## Documentation & Samples

The full documentation, control reference, keyboard bindings, styling guide and runnable samples are available in the project repository.

**[Full README and docs on GitHub](https://github.com/FRACerqueira/PromptPlus)**

---

## License

MIT (c) PromptPlus contributors
