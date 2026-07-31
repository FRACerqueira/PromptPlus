<div align="center">
  <img src="icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  **PromptPlus transforms your console apps with a modern .NET library that delivers polished, interactive experiences — from text input with history and searchable lists to masked fields, date/time pickers, file browsers, progress bars, charts, and more — all streamlined through one sleek fluent API.**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)
  [![NuGet](https://img.shields.io/nuget/vpre/PromptPlus.svg?label=beta)](https://www.nuget.org/packages/PromptPlus)
  [![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

</div>

---

## Highlights

- **20+ interactive controls** — from a simple key-press to multi-column tables and tree browsers
- **6 output-only widgets** — render sliders, calendars, banners, charts and more without blocking
- **Fluent API** — every control is configured with readable method chains
- **Two-layer config** — set defaults once with `PromptPlus.Config`, override per control with `.Options()`
- **Abort anywhere** — Esc aborts any control; result carries an `IsAborted` flag
- **History persistence** — last confirmed value saved and pre-loaded automatically
- **Terminal-safe** — auto-detects size, re-renders on resize, enforces 80×10 minimum gracefully
- **Cross-platform** — Windows, Linux, macOS; .NET 8, 9 and 10
- **Demo Mode** — script keyboard input to auto-record GIFs of your controls, no human needed ([`AutoDemoSamples`](samples/AutoDemoSamples/Program.cs))

<div align="center">
  <img src="media/PromptPlusDemo.gif" alt="PromptPlus demo" width="720" />
</div>

---

## What's new in the latest version

### 📢 Release Note – PromptPlus V.6.X Beta

### 🚀 Beta Phase Launch
- The **6.X** version officially enters the **Beta phase**.  
- Purpose: validate **new features, adjustments, and improvements** currently under development.

### 🛠️ Source Code
- Available in the **main** branch.  

### 📦 NuGet Package
- Latest update: **6.0.0-Beta[seq]**.  
- To install, you must **enable the pre-release option** in NuGet.

### 💬 Community Feedback
- This space is open for:  
  - Sharing **feedback**  
  - Reporting **issues**  
  - Suggesting **enhancements**

---

## Installation

PromptPlus 6.x is currently in **Beta** — you must enable pre-release packages to install it.

```shell
dotnet add package PromptPlus --prerelease
```

Or via the Package Manager Console:

```powershell
Install-Package PromptPlus -IncludePrerelease
```

---

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

> 💡 **Tip:** Every control returns `ResultPrompt<T>`. Use `.Content` for the value, `.IsAborted` to detect Esc, or deconstruct with `var (value, aborted) = result`.

---

## Two-Layer Configuration

### Layer 1 — Global defaults (applied to all controls)

```csharp
using PromptPlusLibrary;

PromptPlus.Config.PageSize = 8;
PromptPlus.Config.HideAfterFinish = true;
```

### Layer 2 — Per-control override (`.Options()` fluent method)

```csharp
PromptPlus.Controls
    .Input("Notes")
    .Options(o => o
        .HideAfterFinish(false)
        .ShowTooltip(false))
    .Run();
```

Per-control settings always win over global config. See [`docs/global-behaviors.md`](docs/global-behaviors.md) for the full property reference.

### Persist config to disk

```csharp
// Write PromptPlus.config to the current directory
PromptPlus.Config.ToFile(".");
```

On next run, PromptPlus automatically reads `PromptPlus.config` from the working directory.

---

## Global Behaviors

| Behavior | Observable effect |
|---|---|
| Terminal resize detection | Control re-renders its own area; surrounding output is untouched |
| Minimum terminal size (80×10) | Shows a resize prompt and waits — never crashes |
| Culture isolation | `DefaultCulture` applied only during `.Run()`; thread culture always restored |
| Single-line rendering | Newlines stripped; sliding window with `…` when value is too wide |
| History persistence | Last confirmed value saved to disk; pre-loaded on next run |
| HideAfterFinish | Control UI erased after confirmation; only the final answer line remains |
| HideOnAbort | Control UI erased when user presses Esc |
| Ctrl+C handling | Intercepted by default → triggers abort; set `RemoveHandlerCtrlC = true` to pass to OS |
| Tooltip visibility | `ShowTooltip = true` shows keyboard hints below the prompt |
| Abort key hint | `ShowMessageAbortKey = true` includes the abort-key name in the tooltip |
| Auto-initialization | PromptPlus initializes on first access: detects terminal, loads config, registers error log |

---

## Localization

PromptPlus ships **11 built-in locales** as embedded resources. The active locale is selected automatically from `CultureInfo.CurrentCulture`; override it at any time with:

```csharp
PromptPlus.Config.DefaultCulture = new CultureInfo("pt-BR");
```

| Culture code | Language |
|---|---|
| *(default)* | English |
| `pt-BR` | Portuguese (Brazil) |
| `de-DE` | German |
| `es-ES` | Spanish |
| `fr-FR` | French |
| `it-IT` | Italian |
| `ja-JP` | Japanese |
| `ko-KR` | Korean |
| `nl-BE` | Dutch (Belgium) |
| `ru-RU` | Russian |
| `zh-CN` | Chinese (Simplified) |

> If `DefaultCulture` is set to a culture that has no embedded resource, PromptPlus falls back to the default English strings.

### Adding a custom locale

If your target culture is not listed above, you can provide your own satellite resource:

1. Copy `PromptPlus/Resources/PromptPlusResources.resx` from the source tree (or extract it from the NuGet package).
2. Translate every message value to your language, keeping the existing key names and format placeholders unchanged.
3. Compile the `.resx` file into a binary `.resources` file — see [Compiling .resx files (Microsoft docs)](https://learn.microsoft.com/dotnet/core/extensions/work-with-resx-files-programmatically).
4. Place the compiled file, named `PromptPlus.<culture-code>.resources` (e.g. `PromptPlus.pl-PL.resources`), in the same directory as your application binaries.

PromptPlus will discover and load it automatically at runtime via the standard .NET resource fallback chain.

---

## Controls Reference

| Control | Factory method | Returns |
|---|---|---|
| [Text&nbsp;input](docs/controls/input/index.md) | `PromptPlus.Controls.Input(prompt)` | `ResultPrompt<string>` |
| [Secret&nbsp;/&nbsp;password](docs/controls/secret/index.md) | `PromptPlus.Controls.Secret(prompt)` | `ResultPrompt<string>` |
| [Key&nbsp;press](docs/controls/keypress/index.md) | `PromptPlus.Controls.KeyPress(prompt)` | `ResultPrompt<ConsoleKeyInfo?>` |
| [Confirm&nbsp;(yes/no)](docs/controls/confirm/index.md) | `PromptPlus.Controls.Confirm(prompt)` | `ResultPrompt<ConsoleKeyInfo?>` |
| [Single&nbsp;select](docs/controls/select/index.md) | `PromptPlus.Controls.Select<T>(prompt)` | `ResultPrompt<T>` |
| [Multi&nbsp;select](docs/controls/multiselect/index.md) | `PromptPlus.Controls.MultiSelect<T>(prompt)` | `ResultPrompt<IEnumerable<T>>` |
| [Table](docs/controls/table/index.md) | `PromptPlus.Controls.Table<T>(prompt)` | `ResultPrompt<TableResult<T>>` |
| [Multi-table](docs/controls/multitable/index.md) | `PromptPlus.Controls.MultiTable<T>(prompt)` | `ResultPrompt<IEnumerable<TableResult<T>>>` |
| [Tree](docs/controls/tree/index.md) | `PromptPlus.Controls.Tree<T>(prompt)` | `ResultPrompt<T>` |
| [Multi-tree](docs/controls/multitree/index.md) | `PromptPlus.Controls.MultiTree<T>(prompt)` | `ResultPrompt<IEnumerable<T>>` |
| [File&nbsp;browser](docs/controls/file/index.md) | `PromptPlus.Controls.File(prompt)` | `ResultPrompt<FileInfo>` |
| [Multi-file](docs/controls/multifile/index.md) | `PromptPlus.Controls.MultiFile(prompt)` | `ResultPrompt<IEnumerable<FileInfo>>` |
| [Calendar](docs/controls/calendar/index.md) | `PromptPlus.Controls.Calendar(prompt)` | `ResultPrompt<DateTime>` |
| [Progress&nbsp;bar](docs/controls/progressbar/index.md) | `PromptPlus.Controls.ProgressBar(prompt)` | `ResultPrompt<double>` |
| [Task](docs/controls/task/index.md) | `PromptPlus.Controls.Task(prompt)` | `ResultPrompt<StateTask>` |
| [Multi-tasks](docs/controls/multitasks/index.md) | `PromptPlus.Controls.MultiTasks(prompt)` | `ResultPrompt<IEnumerable<MultiTaskResult>>` |
| [Chart&nbsp;bar](docs/controls/chartbar/index.md) | `PromptPlus.Controls.ChartBar(prompt)` | `ResultPrompt<double>` |
| [Mask&nbsp;—&nbsp;string](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskEdit(prompt)` | `ResultPrompt<string>` |
| [Mask&nbsp;—&nbsp;integer](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskInteger(prompt)` | `ResultPrompt<int>` |
| [Mask&nbsp;—&nbsp;long](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskLong(prompt)` | `ResultPrompt<long>` |
| [Mask&nbsp;—&nbsp;decimal](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskDecimal(prompt)` | `ResultPrompt<decimal>` |
| [Mask&nbsp;—&nbsp;decimal&nbsp;currency](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskDecimalCurrency(prompt)` | `ResultPrompt<decimal>` |
| [Mask&nbsp;—&nbsp;double](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskDouble(prompt)` | `ResultPrompt<double>` |
| [Mask&nbsp;—&nbsp;double&nbsp;currency](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskDoubleCurrency(prompt)` | `ResultPrompt<double>` |
| [Mask&nbsp;—&nbsp;date&nbsp;&&nbsp;time](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskDateTime(prompt)` | `ResultPrompt<DateTime>` |
| [Mask&nbsp;—&nbsp;date&nbsp;only](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskDate(prompt)` | `ResultPrompt<DateTime>` |
| [Mask&nbsp;—&nbsp;DateOnly](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskDateOnly(prompt)` | `ResultPrompt<DateOnly>` |
| [Mask&nbsp;—&nbsp;time&nbsp;only](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskTime(prompt)` | `ResultPrompt<DateTime>` |
| [Mask&nbsp;—&nbsp;TimeOnly](docs/controls/maskedit/index.md) | `PromptPlus.Controls.MaskTimeOnly(prompt)` | `ResultPrompt<TimeOnly>` |

---

## Widgets Reference

Widgets are output-only — no user input, no `ResultPrompt`. `Banner` and `Dash` render immediately;
the fluent widgets (`Slider`, `Calendar`, `Switch`, `ChartBar`) render when you call `.Show()`.

| Widget | Factory method | Output |
|---|---|---|
| [Slider (display)](docs/widgets.md) | `PromptPlus.Widgets.Slider(value, min, max, fracionaldig)` | `ISliderWidget` |
| [Calendar (display)](docs/widgets.md) | `PromptPlus.Widgets.Calendar(dateref)` | `ICalendarWidget` |
| [Switch (display)](docs/widgets.md) | `PromptPlus.Widgets.Switch(value)` | `ISwitchWidget` |
| [Banner](docs/widgets.md) | `PromptPlus.Widgets.Banner(text)` | immediate render |
| [Dash separator](docs/widgets.md) | `PromptPlus.Widgets.Dash(text)` | immediate render |
| [Chart bar (display)](docs/widgets.md) | `PromptPlus.Widgets.ChartBar()` | `IChartBarWidget` |

---

## ConsolePlus Integration

`PromptPlus.Console` exposes the same `IConsole` driver as `ConsolePlus`. Use it to write styled text, manage cursor, and compose output alongside your controls:

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

// These two are the same object:
PromptPlus.Console.WriteLine("Hello, [bold]world[/]!");
ConsolePlus.WriteLine("Hello, [bold]world[/]!");
```

---

## Samples

The `samples/` folder contains runnable projects for every control and widget — one sample per
concept — plus [`AutoDemoSamples`](samples/AutoDemoSamples/Program.cs), which scripts a walkthrough of
several controls using [Demo Mode](docs/demo-mode.md) and is the actual source used to record the
demo GIF above.

---

## Documentation

| Page | Description |
|---|---|
| [Getting Started](docs/getting-started.md) | Install, first app, config walkthrough |
| [Architecture](docs/architecture.md) | Entry points, lifecycle, ResultPrompt |
| [Global Behaviors](docs/global-behaviors.md) | Full IPromptPlusConfig reference |
| [Keyboard Bindings](docs/keyboard-bindings.md) | Emacs shortcuts, physical key reference |
| [Visual Symbols](docs/visual-symbols.md) | Symbol catalog |
| [Global Styles](docs/global-styles.md) | Style override API |
| [Widgets](docs/widgets.md) | Output-only widgets guide |
| [Demo Mode](docs/demo-mode.md) | Scripted keyboard input for recording GIFs/videos of console apps |
| [Controls index](docs/index.md) | All pages in one place |
| [API Reference](docs/api/PromptPlusLibrary.md) | Auto-generated API docs |

---

## Architecture Decision Records (ADR)

PromptPlus documents its significant architectural and design decisions as
**Architecture Decision Records (ADR)**, following the
[AdrPlus](https://github.com/FRACerqueira/AdrPlus) convention. Each record
captures the context, the decision, the alternatives considered, and the
consequences — so the reasoning behind the library's design stays traceable over
time.

👉 See the **[ADR index](docs/adr/README.md)** for the full list of decisions.

---

## Code of Conduct
This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [Code of Conduct](CODE_OF_CONDUCT.md).

----
## Contributing

See the [Contributing guide](CONTRIBUTING.md) for developer documentation.

**Special thanks**

- [ividyon](https://github.com/ividyon) for their continued contributions to product improvement.

## License

PromptPlus is licensed under the **[MIT License](https://opensource.org/licenses/MIT)**.

---

<div align="center">
  <sub>Maintained by the ConsolePlus project • © 2026 Fernando Cerqueira</sub>
</div>
