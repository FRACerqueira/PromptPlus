<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **PromptPlus — Documentation Index**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Getting Started →](getting-started.md)

---

Everything you need to use PromptPlus, organized by topic.

---

## Reference Pages

| Page | What you will find |
|---|---|
| [Getting Started](getting-started.md) | Prerequisites, install, first complete app, config round-trip |
| [Core Concepts](concepts.md) | The mental model — entry points, lifecycle, `ResultPrompt<T>`, config layers, styling |
| [Architecture](architecture.md) | Entry points, control lifecycle, `ResultPrompt<T>`, two config layers |
| [Global Behaviors](global-behaviors.md) | Full `IPromptPlusConfig` property table, `HotKey`, `ToFile()`, per-control overrides |
| [Keyboard Bindings](keyboard-bindings.md) | Emacs shortcuts, physical key reference by category |
| [Visual Symbols](visual-symbols.md) | Symbol catalog — characters, where shown, configurability |
| [Spinners](spinners.md) | The `SpinnersType` catalog, visual samples, and Unicode/ASCII portability |
| [Global Styles](global-styles.md) | The `Style` API, per-control `.Styles()`, and `ContrastRatio` |
| [Widgets](widgets.md) | Output-only widgets: Slider, Calendar, Switch, Banner, Dash, ChartBar |
| [Demo Mode](demo-mode.md) | Scripted keyboard input for recording GIFs/videos of console apps, and the redirected-input guard exception |

---

## Control Pages

Every control has its own folder with four pages: an overview (`index`), a full `methods` reference,
an `operations` guide (keyboard, validation, history), and a `styles` page.

| Control | Page |
|---|---|
| `Input` | [controls/input/](controls/input/index.md) |
| `Secret` | [controls/secret/](controls/secret/index.md) |
| `KeyPress` | [controls/keypress/](controls/keypress/index.md) |
| `Confirm` | [controls/confirm/](controls/confirm/index.md) |
| `Select<T>` | [controls/select/](controls/select/index.md) |
| `MultiSelect<T>` | [controls/multiselect/](controls/multiselect/index.md) |
| MaskEdit family (`MaskEdit`, `MaskInteger`, `MaskLong`, `MaskDecimal`, `MaskDecimalCurrency`, `MaskDouble`, `MaskDoubleCurrency`, `MaskDateTime`, `MaskDate`, `MaskDateOnly`, `MaskTime`, `MaskTimeOnly`) | [controls/maskedit/](controls/maskedit/index.md) |
| `Calendar` | [controls/calendar/](controls/calendar/index.md) |
| `Slider` | [controls/slider/](controls/slider/index.md) |
| `Switch` | [controls/switch/](controls/switch/index.md) |
| `ProgressBar` | [controls/progressbar/](controls/progressbar/index.md) |
| `Task` | [controls/task/](controls/task/index.md) |
| `MultiTasks` | [controls/multitasks/](controls/multitasks/index.md) |
| `Timer` | [controls/timer/](controls/timer/index.md) |
| `TableSelect<T>` | [controls/tableselect/](controls/tableselect/index.md) |
| `TableMultiSelect<T>` | [controls/tablemultiselect/](controls/tablemultiselect/index.md) |
| `TreeSelect<T>` | [controls/treeselect/](controls/treeselect/index.md) |
| `TreeMultiSelect<T>` | [controls/treemultiselect/](controls/treemultiselect/index.md) |
| `File` | [controls/file/](controls/file/index.md) |
| `MultiFile` | [controls/multifile/](controls/multifile/index.md) |
| `ChartBar` | [controls/chartbar/](controls/chartbar/index.md) |

---

## API Reference

| Page | Description |
|---|---|
| [API Documentation](api/PromptPlusLibrary.md) | Full auto-generated reference for all types |

---

## Migration

| Guide | What you will find |
|---|---|
| [Migration Guide v5.x → v6.x](migration-v5-to-v6.md) | Breaking changes, renamed controls and methods, removed APIs, and new features — overview for all controls |
| [Input / AutoComplete](migration/controls/input.md) | `AutoComplete()` (non-generic) → `Input().SuggestionHandler(...)`, async additions |
| [Select / MultiSelect](migration/controls/select.md) | Renamed methods, `HideCountSelected` removed, async additions |
| [KeyPress / Confirm](migration/controls/keypress.md) | `AddKeyValid` → `AddValidKey`, timeout/countdown removed |
| [TableSelect / TableMultiSelect](migration/controls/tableselect.md) | `AddColumn` reformulated, `Filter` signature, new features |
| [TreeSelect / TreeMultiSelect](migration/controls/treeselect.md) | Factory renames, tree-building API reworked (`Root`/`AddLast`), remote-control removal |
| [File / MultiFile](migration/controls/file.md) | Factory renames, attribute methods, new MultiFile features |
| [MultiTasks / Time / Task](migration/controls/tasks.md) | Factory renames, `AddTask` reformulated, return types |
| [MaskEdit (all types)](migration/controls/maskedit.md) | No breaking changes; `PredicateSelectedAsync` added (factories unchanged) |
| [Slider / Calendar / Switch / ProgressBar / ChartBar / History](migration/controls/nochanges.md) | Mostly additions; breaking changes in `ProgressBar` and `ChartBar` |

---

## See also

- [README](../README.md) — project overview and quick start
- [Getting Started](getting-started.md) — recommended first read
- [Architecture](architecture.md) — understand the design before diving into controls
- [Migration Guide v5.x → v6.x](migration-v5-to-v6.md) — upgrading from the previous major version
