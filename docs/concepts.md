<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Core Concepts**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Architecture →](architecture.md)

---

A short mental model of how PromptPlus is put together. Read this once and every control page will
feel familiar. Each idea links to the page that covers it in depth.

---

## 1. One entry point, four surfaces

Everything hangs off the static `PromptPlus` class (namespace `PromptPlusLibrary`):

| Surface | Type | Purpose |
|---|---|---|
| `PromptPlus.Config` | `IPromptPlusConfig` | Global defaults for every control |
| `PromptPlus.Controls` | `IControls` | Factory for **interactive** controls |
| `PromptPlus.Widgets` | `IWidgets` | Factory for **output-only** widgets |
| `PromptPlus.Console` | `IConsole` | The ConsolePlus driver (styled text, cursor, screen) |

You never `new` these — always go through the four surfaces. See
[Architecture](architecture.md).

---

## 2. Controls vs Widgets

| | Controls | Widgets |
|---|---|---|
| Reads user input | ✅ Yes | ❌ No |
| Blocks execution | ✅ Until Enter/Esc | ❌ No |
| Returns a value | ✅ `ResultPrompt<T>` | ❌ No |
| Runs with | `.Run()` | `.Show()` (or renders immediately for `Banner`/`Dash`) |

Controls **ask**; widgets **display**. See [Widgets](widgets.md).

---

## 3. The fluent lifecycle

Every control follows the same four steps:

```
1. Factory        PromptPlus.Controls.Input("Name")
2. Fluent config  .Default("Alice").MaxLength(50)
3. Per-control    .Options(o => o.HideAfterFinish(true))   // optional
4. Execute        .Run()   ← blocks until Enter or Esc
                   → ResultPrompt<T>
```

Configuration methods return the same control instance, so they chain in any order. Nothing renders
until you call `.Run()` (controls) or `.Show()` (fluent widgets).

---

## 4. `ResultPrompt<T>` — the return value

Every `.Run()` returns a readonly `ResultPrompt<T>`:

| Member | Meaning |
|---|---|
| `.Content` | The confirmed value (default of `T` when aborted) |
| `.IsAborted` | `true` if the user pressed Esc / the abort key |

```csharp
var (name, aborted) = PromptPlus.Controls.Input("Name").Run();  // deconstruct
if (!aborted) PromptPlus.Console.WriteLine(name);
```

> Always branch on `IsAborted` before using `.Content`. See
> [Architecture → ResultPrompt](architecture.md#resultpromptt).

---

## 5. Two configuration layers

| Layer | How | Scope |
|---|---|---|
| Global | `PromptPlus.Config.PageSize = 8;` | Every control |
| Per-control | `.Options(o => o.HideAfterFinish(true))` | One instance (wins over global) |

Global config can be persisted to `PromptPlus.config` with `PromptPlus.Config.ToFile(".")` and is
reloaded automatically on the next run. See [Global Behaviors](global-behaviors.md).

---

## 6. Styling is per-control

There is **no global "style everything" API**. Each control colors its own regions with
`.Styles(<ControlEnum>, style)` — e.g. `InputStyles.Answer`, `SelectStyles.Selected`. A `Style` is a
foreground color, a background color, and an `Overflow` strategy (no bold/italic concept). The only
global style setting is `PromptPlus.Config.ContrastRatio`, which keeps text readable. See
[Global Styles](global-styles.md) and [Visual Symbols](visual-symbols.md).

---

## 7. Input aids

Most text and list controls share the same helpers:

| Aid | How | Key |
|---|---|---|
| Validation on confirm | `PredicateValid(...)` (Input/Secret) · `PredicateSelected(...)` (Select/Table/Tree) · `PredicateChecked(...)` (Multi* checkables) | — |
| Persistent history | `EnableHistory("key")` | **F3** |
| Autocomplete suggestions | `SuggestionHandler(...)` | **Tab / Shift+Tab** |
| Abort | enabled by default | **Esc** |

See [Keyboard Bindings](keyboard-bindings.md).

---

## 8. It just starts

On first access PromptPlus auto-initializes: it detects terminal capabilities, loads `PromptPlus.config`
if present, and registers an error-log hook. It re-renders on resize, enforces an 80×10 minimum
gracefully, and restores the thread culture after each `.Run()`. No init call required. See
[Architecture → Auto-Initialization](architecture.md#auto-initialization).

---

## Where to next

- [Getting Started](getting-started.md) — build your first app
- [Architecture](architecture.md) — the design in depth
- [Controls index](index.md#control-pages) — every control, page by page
