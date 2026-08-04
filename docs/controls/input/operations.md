<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Input — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Input — Styles →](styles.md)

---

How the `Input` control behaves while it is running: keyboard, the order in which restrictions and
validation apply, history, autocomplete, and the small details that matter in real apps.

---

## Anatomy of the control

```
Name: John_                              ← prompt + typed text + cursor
First and last name                      ← description (optional / dynamic)
Current length: 4                        ← ChangeDescription output (optional)
Value is invalid                         ← error line, only when validation fails
Enter: confirm  Esc: cancel  F3: history ← tooltip (toggle with F1 / Ctrl+F1)
```

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

### Editing

| Key | Action |
|---|---|
| Any printable character | Insert at the cursor (subject to [`AcceptInput`](methods.md#acceptinput) and [`MaxLength`](methods.md#maxlength)) |
| `←` / `→` | Move one character |
| `Home` / `End` | Start / end of line |
| `Backspace` | Delete character before the cursor |
| `Delete` | Delete character at the cursor |
| `Insert` | Toggle insert / overwrite mode |

There is no plain (non-Emacs) word-motion or word-deletion binding: `Ctrl+←`/`Ctrl+→`,
`Ctrl+Backspace`, and `Ctrl+Delete` do **not** move/delete by word. Word motion/deletion is
Emacs-only — `Alt+F`/`Alt+B` to move, `Ctrl+W`/`Alt+D` to delete — see the warning below.

### Actions

| Key | Action |
|---|---|
| `Enter` | Confirm — runs validation, then returns the value |
| `Esc` | Abort (when the abort key is enabled) → `IsAborted == true` |
| `Tab` / `Shift+Tab` | Autocomplete — behavior differs by mode, see [Autocomplete](#autocomplete) below |
| `F3` | Open history navigation (when [history](methods.md#enablehistory) is enabled) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

> When **Emacs key bindings** are enabled (`PromptPlus.Console.EnabledEmacs = true`), the text
> also responds to `Ctrl+A/E/B/F/D/K/U/W/T` and friends (no `Ctrl+Y` — there's no yank/kill-buffer
> support here). See [Keyboard Bindings](../../keyboard-bindings.md) for the full table.

---

## How a keystroke is processed

The restrictions apply in a fixed order, so it helps to picture the pipeline:

```
key pressed
   │
   ├─ is it a control/navigation key? --→ handled (move, delete, confirm, …)
   │
   └─ printable character
          │
          ├─ InputToCase applied FIRST (upper/lower) — before AcceptInput ever sees the char
          ├─ AcceptInput(transformedChar) == false? --→ ignored
          ├─ length already == MaxLength? --→ ignored
          └─ inserted at cursor
```

Consequently:

- **`InputToCase` runs BEFORE `AcceptInput`, not after.** `AcceptInput` receives the
  *already-cased* character, never the raw one the user typed. A predicate that only accepts one
  case (e.g. `InputToCase: CaseOptions.Uppercase` combined with `AcceptInput: char.IsLower`) will
  silently reject every character, since it never sees a lowercase char to accept. Write
  `AcceptInput` predicates that expect the target case (or are case-insensitive) when combining the
  two.
- `AcceptInput` and `MaxLength` are still **preventive** — they stop bad input from ever appearing;
  they just see it after the case transform, not before.

---

## Confirmation & validation flow

Pressing **Enter** runs this sequence:

1. Validation runs — [`PredicateValid`](methods.md#predicatevalid) /
   [`PredicateValidAsync`](methods.md#predicatevalidasync), if configured — against the **raw
   typed value**, even if it's empty.
2. **Invalid** → the control stays open, shows the error line, and waits for more input.
   **Valid** → only now, if the field was empty, [`DefaultIfEmpty`](methods.md#defaultifempty)'s
   value is substituted; the control closes and returns `ResultPrompt<string>` with
   `IsAborted == false`.

> ⚠️ **`DefaultIfEmpty` runs AFTER validation, not before.** A `PredicateValid` that rejects empty
> input will reject an empty field before `DefaultIfEmpty` ever gets a chance to substitute its
> value — the substitution only happens once the raw (possibly empty) value has already passed
> validation. If you want an empty field to be valid, your predicate must accept empty strings.

> ⚠️ Validation only runs on confirm, never per keystroke. Use `AcceptInput` for per-keystroke rules
> and `PredicateValid` for whole-value rules (length, format, uniqueness).

---

## History

When [`EnableHistory(filename, …)`](methods.md#enablehistory) is set:

- Each confirmed value is appended to the on-disk store named `filename`.
- Pressing **F3** opens a paged list of past entries filtered by what is currently typed
  (`FilterType`, `MinPrefixLength`, `PageSize` control this). While that list is open: `↑`/`↓` move
  between entries, `Page Up`/`Page Down` move between pages, `Ctrl+Home`/`Ctrl+End` jump to the
  first/last entry, and `F3` again closes the list back to normal editing.
- With `Default(string.Empty, useDefaultHistory: true)`, the most recent entry is pre-loaded as the
  starting value.
- Entries expire per `ExpirationTime`; the store keeps at most `MaxItems`.

> ⚠️ **`Ctrl+Delete`, while the history list is open, deletes the entire on-disk history file** —
> not a single entry, the whole store. This is a real, destructive, and easy-to-trigger-by-accident
> action (it's the same physical key combo a user might reach for expecting "delete word").

You can also manage a history store directly with `PromptPlus.Controls.History(filename)` — add,
save, or remove entries programmatically (used in the samples to seed reproducible data).

> ⚠️ Do not enable history on secret fields — confirmed values are written to disk in the store.

---

## Autocomplete

When a [suggestion handler](methods.md#suggestionhandler) is set, the provider is called once the
typed length reaches [`MinimumSuggestionLength`](methods.md#minimumsuggestionlength). What happens
next depends on `autocomplete`, and the two modes behave quite differently — this isn't just "same
feature, different key":

- **`autocomplete: true`** (default) — there is **no separate suggestion list**. Each **Tab** press
  replaces the buffer directly with the next match (wrapping around at the end); **Shift+Tab has no
  effect** in this mode. This applies regardless of how many matches the provider returned — "apply
  immediately for a single match" is not the real rule; every match, one or many, is cycled the same
  way.
- **`autocomplete: false`** — pressing **Tab or Shift+Tab** switches into a separate suggestion-list
  mode. Once there: **↑ / ↓** move the highlight, **Tab** accepts the highlighted suggestion and
  returns to normal editing, **Shift+Tab** cancels and restores the text you had before entering the
  list.

History (F3) and suggestions (Tab) are independent features and can be used together.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Input` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the field after confirm — the whole control is erased, not just the interactive part |
| `HideOnAbort(true)` | Erases the field after Esc |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

---

## Edge cases & gotchas

- **Single-line rendering.** The value is displayed on one line. If it is wider than the terminal, a
  sliding window with `…` is shown, but `.Content` always holds the full untruncated text.
- **Newlines** are not accepted (Enter confirms). For multi-line data collect line by line.
- **Aborted results do NOT reliably carry `.Content == ""`.** `.Content` holds whatever is currently
  in the text buffer at the moment of abort — a seeded `Default` the user never typed over, or
  partially-typed text, not necessarily empty. Always branch on `IsAborted` before using the value.
- **Async callbacks block the UI thread** — see the warning under
  [`PredicateValidAsync`](methods.md#predicatevalidasync).

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key and Emacs reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
