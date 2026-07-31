<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Keyboard Bindings**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Visual Symbols →](visual-symbols.md)

---

PromptPlus supports two sets of keyboard shortcuts: **standard** keys (always active) and optional **GNU Emacs** shortcuts for text-editing actions.

---

## Enabling Emacs Key Bindings

Emacs bindings are inherited by ConsolePlus. Enable them globally:

```csharp
using PromptPlusLibrary;

// Global — applies to all controls
PromptPlus.Console.EnabledEmacs = true;

```
---

## Emacs Shortcuts

| Action | Emacs Key | Standard Key |
|---|---|---|
| Move to start of line | `Ctrl+A` | `Home` |
| Move to end of line | `Ctrl+E` | `End` |
| Move one character left | `Ctrl+B` | `←` |
| Move one character right | `Ctrl+F` | `→` |
| Delete character under cursor | `Ctrl+D` | `Delete` |
| Delete to end of line | `Ctrl+K` | — |
| Delete to start of line | `Ctrl+U` | — |
| Delete word left | `Ctrl+W` / `Alt+Backspace` | `Ctrl+Backspace` |
| Delete word right | `Alt+D` | `Ctrl+Delete` |
| Move one word left | `Alt+B` | `Ctrl+←` |
| Move one word right | `Alt+F` | `Ctrl+→` |
| Yank (paste kill buffer) | `Ctrl+Y` | — |
| Transpose characters | `Ctrl+T` | — |

---

## Standard Key Reference

### Text Input (`Input`, `Secret`, `MaskEdit` family)

| Key | Action |
|---|---|
| `←` / `→` | Move cursor one character |
| `Ctrl+←` / `Ctrl+→` | Move cursor one word |
| `Home` / `End` | Jump to start / end of line |
| `Backspace` | Delete character before cursor |
| `Delete` | Delete character at cursor |
| `Ctrl+Backspace` | Delete word left |
| `Ctrl+Delete` | Delete word right |
| `Tab` / `Shift+Tab` | Apply / cycle autocomplete suggestions (when a suggestion handler is set) |
| `F3` | Open history (when history is enabled) |
| `↑` / `↓` | Navigate the open history / suggestion list |
| `F2` | Reveal / hide the typed text (`Secret` only) |
| `Enter` | Confirm |
| `Esc` | Abort (if `EnabledAbortKey = true`) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Toggle tooltip visibility |

### List & Selection (`Select<T>`, `MultiSelect<T>`)

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus |
| `Page Up` / `Page Down` | Jump one page |
| `Home` / `End` | Jump to first / last item |
| `Enter` | Confirm selection |
| `Space` | Toggle item (MultiSelect) |
| `F2` | Select all / deselect all (MultiSelect) |
| `F3` | Filter to show selected only (MultiSelect) |
| Any printable char | Type to filter (when `Filter` is enabled) |
| `Esc` | Abort |

> When history is enabled on a `Select<T>`, press **F3** to open its history list.

### Calendar

| Key | Action |
|---|---|
| `←` / `→` | Previous / next day |
| `↑` / `↓` | Previous / next week |
| `Page Up` / `Page Down` | Previous / next month |
| `Ctrl+Page Up` / `Ctrl+Page Down` | Previous / next year |
| `Home` | Go to today |
| `F2` | Toggle notes display |
| `Enter` | Confirm date |
| `Esc` | Abort |

### File & Tree Browser (`File`, `MultiFile`, `Tree<T>`, `MultiTree<T>`)

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus |
| `→` / `Enter` on folder | Expand / enter folder |
| `←` | Collapse / go up |
| `Space` | Toggle selection (Multi variants) |
| `F2` | Select all / deselect all (Multi variants) |
| `F4` | Select by wildcard pattern (Multi variants) |
| `Shift+F3` | Toggle full path display |
| `Any printable char` | Filter |
| `Esc` | Abort |

### Slider & Switch

| Key | Action |
|---|---|
| `←` / `↓` | Decrease value |
| `→` / `↑` | Increase value |
| `Home` | Minimum value |
| `End` | Maximum value |
| `Space` | Toggle (Switch only) |
| `Enter` | Confirm |
| `Esc` | Abort |

### Chart Bar

| Key | Action |
|---|---|
| `F2` | Switch chart layout |
| `F3` | Toggle legend |
| `F4` | Cycle sort order |
| `Enter` | Confirm |
| `Esc` | Abort |

### Universal (all controls)

| Key | Action |
|---|---|
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Toggle tooltip visibility |
| `Esc` | Abort (when `EnabledAbortKey = true`) |
| `Ctrl+C` | Abort (default); passes to OS when `RemoveHandlerCtrlC = true` |

---

## See also

- [Global Behaviors](global-behaviors.md) — enable/disable Emacs bindings globally, configure abort key
- [Architecture](architecture.md) — `IControlOptions` per-control settings
