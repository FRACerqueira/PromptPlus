<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Architecture**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Global Behaviors →](global-behaviors.md)

---

A quick mental model so you can use PromptPlus confidently and avoid surprises.

---

## Four Entry Points

Everything in PromptPlus is accessed through four static properties on the `PromptPlus` class:

```
PromptPlusLibrary.PromptPlus
│
├── .Config   → IPromptPlusConfig   (global settings for all controls)
├── .Controls → IControls           (factory for interactive controls)
├── .Widgets  → IWidgets            (factory for output-only widgets)
└── .Console  → IConsole            (ConsolePlus driver — same as ConsolePlus.Driver)
```

```csharp
using PromptPlusLibrary;

// Global config
PromptPlus.Config.PageSize = 10;

// Create a control
var result = PromptPlus.Controls.Input("Your name").Run();

// Render a widget
PromptPlus.Widgets.Banner("Welcome!");

// Write styled text
PromptPlus.Console.WriteLine("[green]Done![/]");
```

> 💡 You never instantiate PromptPlus objects manually. Always go through these four entry points.

---

## Controls vs Widgets

| | Controls (`IControls`) | Widgets (`IWidgets`) |
|---|---|---|
| User interaction | ✅ Yes — waits for input | ❌ No — renders immediately |
| Blocks execution | ✅ Yes — until Enter or Esc | ❌ No |
| Returns a value | ✅ `ResultPrompt<T>` from `.Run()` | ❌ No result |
| Example | `Input`, `Select<T>`, `Calendar` | `Banner`, `Dash`, `Slider(value,…)` |

---

## Control Lifecycle

Every interactive control follows the same four-step lifecycle:

```
1. Factory      PromptPlus.Controls.Input("Name")
       ↓
2. Fluent config  .DefaultValue("Alice")
                  .MaxLength(50)
                  .EnableHistory("name-history")
       ↓
3. Per-control    .Options(o => o
   overrides          .HideAfterFinish(true)
                      .ShowTooltip(false))
       ↓
4. Execute        .Run()   ←── blocks until Enter or Esc
       ↓
   ResultPrompt<T>
```

> ⚠️ Calling `.Run()` is always required. Without it, the control is configured but never shown.

---

## `ResultPrompt<T>`

`ResultPrompt<T>` is a **readonly struct** returned by every `.Run()` call.

| Member | Type | Description |
|---|---|---|
| `.Content` | `T` | The confirmed value (default of `T` when aborted) |
| `.IsAborted` | `bool` | `true` if the user pressed Esc (or another abort key) |
| `.Deconstruct(…)` | — | Enables tuple-style unpacking |

### Usage patterns

```csharp
using PromptPlusLibrary;

// Pattern 1 — property access
var result = PromptPlus.Controls.Input("Name").Run();
if (!result.IsAborted)
    Console.WriteLine(result.Content);

// Pattern 2 — deconstruct
var (name, aborted) = PromptPlus.Controls.Input("Name").Run();
if (!aborted)
    Console.WriteLine(name);

// Pattern 3 — inline guard
var result = PromptPlus.Controls.Select<string>("Color")
    .AddItems(["Red", "Green", "Blue"])
    .Run();
if (result.IsAborted) return;
Console.WriteLine($"Chosen: {result.Content}");
```

---

## Two Configuration Layers

### Layer 1 — Global (`PromptPlus.Config`)

Sets defaults for every control in the application. Set it once at startup:

```csharp
PromptPlus.Config.PageSize = 8;
PromptPlus.Config.HideAfterFinish = true;
PromptPlus.Config.DefaultCulture = new CultureInfo("en-US");
```

### Layer 2 — Per-control (`.Options()`)

Overrides global defaults for a single control instance. Always wins over Layer 1:

```csharp
PromptPlus.Controls
    .Input("Notes")
    .Options(o => o
        .HideAfterFinish(false)   // override global true
        .Description("Free text"))
    .Run();
```

See [global-behaviors.md](global-behaviors.md) for the complete property and override reference.

---

## Auto-Initialization

PromptPlus initializes automatically the first time any of its four entry points is accessed. The sequence is:

1. Detect terminal capabilities (color support, Unicode, window size)
2. Look for `PromptPlus.config` in the working directory — load it if found
3. Register an error-log hook: unhandled exceptions write to `%LocalAppData%/PromptPlus/PromptPlus.error.log`

You do not need to call any initialization method. The first access triggers it.

---

## Runtime Behaviors

| Behavior | What you observe |
|---|---|
| Terminal resize | The active control re-renders its own area. Surrounding output is untouched. |
| Minimum size (80×10) | If the terminal is too small, a resize prompt appears and execution waits. |
| Culture isolation | `DefaultCulture` is applied only during `.Run()`. The original thread culture is always restored afterward — even on error. |
| Single-line rendering | Newlines are stripped from displayed values. When the value is wider than the terminal, a sliding window with `…` is used. `result.Content` always holds the original full value. |
| Redirected console input | `.Run()`/`.Show()` throw `InvalidOperationException` immediately instead of hanging when console input is redirected (a file, a pipe, a CI runner). `ProgressBar`, `Task`, `MultiTasks`, and `Time` are unaffected — they complete on their own signal and run normally under redirected input. See [ADR0023](adr/ADR0023V01R01-GuardInteractiveControlsAgainstRedirectedInput.md). |

---

## Localization

PromptPlus reads culture-sensitive defaults (such as `YesChar` and `NoChar` for Confirm) from `CultureInfo.CurrentCulture` at startup. Override the culture with:

```csharp
PromptPlus.Config.DefaultCulture = new CultureInfo("fr-FR");
```

| Config property | Culture-sensitive default |
|---|---|
| `YesChar` | `'y'` (or locale equivalent) |
| `NoChar` | `'n'` (or locale equivalent) |
| `FirstDayOfWeek` | `DayOfWeek.Sunday` |
| `DefaultCulture` | `CultureInfo.CurrentCulture` |

---

## See also

- [Global Behaviors](global-behaviors.md) — full `IPromptPlusConfig` reference
- [Getting Started](getting-started.md) — first working app
- [Controls index](index.md) — all control pages
