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
| Delete word left | `Ctrl+W` | — |
| Delete word right | `Alt+D` | — |
| Move one word left | `Alt+B` | — |
| Move one word right | `Alt+F` | — |
| Yank (paste kill buffer) | `Ctrl+Y` | — |
| Transpose characters | `Ctrl+T` | — |
| Toggle case, move to end of word | `Ctrl+C` | — |
| Uppercase to end of word | `Alt+U` | — |
| Lowercase to end of word | `Alt+L` | — |

> ⚠️ **`Ctrl+Backspace`, `Ctrl+Delete`, `Ctrl+←`, and `Ctrl+→` are NOT standard equivalents for
> word motion/deletion** — no such bindings exist for that purpose. Word motion/deletion is
> Emacs-only (the keys in the table above), and only when `PromptPlus.Console.EnabledEmacs` is
> `true`. Separately, **`Ctrl+Delete` performs a real, unrelated, destructive action**: on `Input`/
> `Secret` controls, once you've opened the history list with `F3`, `Ctrl+Delete` **clears the
> entire on-disk history file** — it has nothing to do with editing the current line.

> ⚠️ This Emacs subset applies to `Input` and `Secret`. **`MaskEdit`'s Emacs support only
> implements the position/delete-char rows** (`Ctrl+A/E/B/F/D/H`) plus `Ctrl+L` — word motion,
> word deletion, yank, transpose, and case toggling are **not** available on `MaskEdit`.

---

## Standard Key Reference

### Text Input — `Input`, `Secret`

| Key | Action |
|---|---|
| `←` / `→` | Move cursor one character |
| `Home` / `End` | Jump to start / end of line |
| `Backspace` | Delete character before cursor |
| `Delete` | Delete character at cursor |
| `Insert` | Toggle insert / overwrite mode |
| `Tab` | Accept the current suggestion and exit suggestion mode (when a suggestion handler is set) |
| `Shift+Tab` | Cancel suggestion mode without accepting (only when `autocomplete: false`) |
| `↑` / `↓` | Cycle suggestions, or navigate the open history list |
| `F3` | Open history (when history is enabled) |
| `Ctrl+Delete` | **While the history list is open (`F3`)**: clears the entire on-disk history file |
| `F2` | Reveal / hide the typed text (`Secret` only) |
| `Enter` | Confirm |
| `Esc` | Abort (if `EnabledAbortKey = true`) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Toggle tooltip visibility |

Word-level motion/deletion on `Input`/`Secret` is **Emacs-only** (`Alt+F`/`Alt+B` to move, `Ctrl+W`/
`Alt+D` to delete) — see [Emacs Shortcuts](#emacs-shortcuts) above; there is no non-Emacs equivalent.

### Text Input — `MaskEdit` (string / number / date-time / currency)

`MaskEdit` does **not** share Input/Secret's autocomplete or history behavior — it has neither.

| Key | Action |
|---|---|
| `←` / `→` | Move one editable position within the current field |
| `Tab` / `Shift+Tab` | Jump to the next / previous mask delimiter (date/time masks only) |
| `Backspace` / `Delete` | Delete the digit/character at the cursor |
| `Insert` | Toggle insert / overwrite mode |
| `Enter` | Confirm |
| `Esc` | Abort (if `EnabledAbortKey = true`) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Toggle tooltip visibility |

If `PromptPlus.Console.EnabledEmacs` is `true`, `MaskEdit` additionally accepts the position/delete
subset of Emacs keys — `Ctrl+A/E/B/F/D/H/L` — but none of the word-motion, word-deletion, yank,
transpose, or case-toggling shortcuts.

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

> `Select<T>`/`MultiSelect<T>` don't have a real history-view like `Input`/`Secret` do — enabling
> history on them just auto-loads the last confirmed value as the `Default` on the next run. There
> is no history list to open, and no `F3` history hotkey for these two controls.

### Table Selection (`TableSelect<T>`, `TableMultiSelect<T>`)

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus up / down (rows) |
| `Page Up` / `Page Down` | Jump one page |
| `Home` / `End` | First / last row |
| `Tab` / `Shift+Tab` | Move the focused **column** (only when the table scrolls horizontally) |
| `Space` | Toggle the checkbox on the focused row (`TableMultiSelect` only) |
| `F2` | Toggle all rows on / off (`TableMultiSelect` only) |
| `F3` | Show only the checked rows (`TableMultiSelect` only) |
| `Backspace` | Edit the filter text |
| `Enter` | Confirm |
| `Esc` | Abort |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

### Calendar

| Key | Action |
|---|---|
| `←` / `→` | Previous / next day |
| `↑` / `↓` | Previous / next week |
| `Tab` / `Shift+Tab` | Previous / next month |
| `Page Up` / `Page Down` | Previous / next year |
| `Home` | Go to today (only if today is within the configured `Range`) |
| `F2` | Toggle notes display (opens a separate keymap — see below) |
| `Enter` | Confirm date |
| `Esc` | Abort |

While the notes panel is open (`F2`): `↑`/`↓` move between notes, `Page Up`/`Page Down` move
between pages, `Ctrl+Home`/`Ctrl+End` jump to the first/last note, and typing a letter jumps to a
matching note.

### File & Tree Browser (`File`, `MultiFile`, `TreeSelect<T>`, `TreeMultiSelect<T>`)

| Key | Action |
|---|---|
| `↑` / `↓` | Move focus |
| `+` / `-` | Expand / collapse the focused folder (plain `←`/`→` do **not** expand/collapse) |
| `Tab` / `Shift+Tab` | Expand + descend into the first child / collapse the parent + move to it |
| `Enter` | Confirm the focused entry |
| `Space` | Toggle selection (Multi variants) |
| `F2` | Select all / deselect all (Multi variants) |
| `F3` | Filter to only checked items (`MultiFile` only — `TreeMultiSelect` doesn't have this) |
| `Shift+F3` | Toggle full path display |
| `Any printable char` | Filter |
| `Esc` | Abort |

### Slider

| Key | Action |
|---|---|
| `←` / `↓` | Decrease value by one step |
| `→` / `↑` | Increase value by one step |
| `Tab` / `Shift+Tab` | Decrease / increase by the large step |
| `Enter` | Confirm |
| `Esc` | Abort |

There is no `Home`/`End`-to-min/max binding — no such key handling exists for Slider.

### Switch

| Key | Action |
|---|---|
| `←` | Force the value to **off** (not a decrease — always sets off, regardless of the current state) |
| `→` | Force the value to **on** (always sets on, regardless of the current state) |
| `Space` | Toggle the current value |
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
