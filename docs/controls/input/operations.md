<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Input — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [Input — Styles ?](styles.md)

---

How the `Input` control behaves while it is running: keyboard, the order in which restrictions and
validation apply, history, autocomplete, and the small details that matter in real apps.

---

## Anatomy of the control

```
Name: John_                              ? prompt + typed text + cursor
First and last name                      ? description (optional / dynamic)
Current length: 4                        ? ChangeDescription output (optional)
Value is invalid                         ? error line, only when validation fails
Enter: confirm  Esc: cancel  F3: history ? tooltip (toggle with F1 / Ctrl+F1)
```

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

### Editing

| Key | Action |
|---|---|
| Any printable character | Insert at the cursor (subject to [`AcceptInput`](methods.md#acceptinput) and [`MaxLength`](methods.md#maxlength)) |
| `?` / `?` | Move one character |
| `Ctrl+?` / `Ctrl+?` | Move one word |
| `Home` / `End` | Start / end of line |
| `Backspace` | Delete character before the cursor |
| `Delete` | Delete character at the cursor |
| `Ctrl+Backspace` | Delete the word to the left |
| `Ctrl+Delete` | Delete the word to the right |

### Actions

| Key | Action |
|---|---|
| `Enter` | Confirm — runs validation, then returns the value |
| `Esc` | Abort (when the abort key is enabled) ? `IsAborted == true` |
| `Tab` / `Shift+Tab` | Apply / cycle autocomplete suggestions (when a [suggestion handler](methods.md#suggestionhandler) is set) |
| `F3` | Open history navigation (when [history](methods.md#enabledhistory) is enabled) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

> When **Emacs key bindings** are enabled (`PromptPlus.Config.EmacsKeyBindings = true`), the text
> also responds to `Ctrl+A/E/B/F/D/K/U/W/Y/T` and friends. See
> [Keyboard Bindings](../../keyboard-bindings.md) for the full table.

---

## How a keystroke is processed

The restrictions apply in a fixed order, so it helps to picture the pipeline:

```
key pressed
   ¦
   +- is it a control/navigation key? --? handled (move, delete, confirm, …)
   ¦
   +- printable character
          ¦
          +- AcceptInput(c) == false ? --? ignored
          +- length already == MaxLength ? --? ignored
          +- InputToCase applied (upper/lower)
          +- inserted at cursor
```

Consequently:

- `AcceptInput` and `MaxLength` are **preventive** — they stop bad input from ever appearing.
- `InputToCase` transforms accepted characters on the way in, so `.Content` is already cased.

---

## Confirmation & validation flow

Pressing **Enter** runs this sequence:

1. If the field is empty and [`DefaultIfEmpty`](methods.md#defaultifempty) was set, its value is substituted.
2. Validation runs — [`PredicateValid`](methods.md#predicatevalid) /
   [`PredicateValidAsync`](methods.md#predicatevalidasync), if configured.
3. **Valid** ? the control closes and returns `ResultPrompt<string>` with `IsAborted == false`.
   **Invalid** ? the control stays open, shows the error line, and waits for more input.

> ?? Validation only runs on confirm, never per keystroke. Use `AcceptInput` for per-keystroke rules
> and `PredicateValid` for whole-value rules (length, format, uniqueness).

---

## History

When [`EnabledHistory(filename, …)`](methods.md#enabledhistory) is set:

- Each confirmed value is appended to the on-disk store named `filename`.
- Pressing **F3** opens a paged list of past entries filtered by what is currently typed
  (`FilterType`, `MinPrefixLength`, `PageSize` control this).
- With `Default(string.Empty, useDefaultHistory: true)`, the most recent entry is pre-loaded as the
  starting value.
- Entries expire per `ExpirationTime`; the store keeps at most `MaxItems`.

You can also manage a history store directly with `PromptPlus.Controls.History(filename)` — add,
save, or remove entries programmatically (used in the samples to seed reproducible data).

> ?? Do not enable history on secret fields — confirmed values are written to disk in the store.

---

## Autocomplete

When a [suggestion handler](methods.md#suggestionhandler) is set:

- The provider is called once the typed length reaches
  [`MinimumSuggestionLength`](methods.md#minimumsuggestionlength).
- **`autocomplete: true`** (default) — if the provider returns exactly one match, **Tab** applies it
  immediately.
- **`autocomplete: false`** — matches are presented as a list; **Tab / Shift+Tab** cycle through them
  and the highlighted one is inserted.

History (F3) and suggestions (Tab) are independent features and can be used together.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Input` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the field after confirm; only the answer line remains |
| `HideOnAbort(true)` | Erases the field after Esc |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

---

## Edge cases & gotchas

- **Single-line rendering.** The value is displayed on one line. If it is wider than the terminal, a
  sliding window with `…` is shown, but `.Content` always holds the full untruncated text.
- **Newlines** are not accepted (Enter confirms). For multi-line data collect line by line.
- **Aborted results** carry `.Content == ""`, not the seeded `Default`. Always branch on `IsAborted`.
- **Async callbacks block the UI thread** — see the warning under
  [`PredicateValidAsync`](methods.md#predicatevalidasync).

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key and Emacs reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
