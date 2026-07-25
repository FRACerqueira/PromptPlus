<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Secret — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [Secret — Styles ?](styles.md)

---

How the `Secret` control behaves while it is running: keyboard, the mask character, the F2 reveal,
the order in which restrictions and validation apply, and the security rules that matter when you
handle passwords and keys.

---

## Anatomy of the control

```
Password: ####_                          ? prompt + masked text + cursor
Min 8 chars with upper/lower/digit       ? description (optional / dynamic)
Length: 4                                ? ChangeDescription output (optional)
Password must have at least 8 chars      ? error line, only when validation fails
Enter: confirm  Esc: cancel  F2: reveal  ? tooltip (toggle with F1 / Ctrl+F1)
```

Every region can be recolored — see [Styles](styles.md). What the user typed appears as mask
symbols, not the real characters, unless they reveal it with F2 (see below).

---

## Keyboard

### Editing

| Key | Action |
|---|---|
| Any printable character | Insert at the cursor, shown as the mask symbol (subject to [`AcceptInput`](methods.md#acceptinput) and [`MaxLength`](methods.md#maxlength)) |
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
| `F2` | Reveal / hide the plain text (when [`MaskSecret`](methods.md#masksecret) was left with `enabledView: true`) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

> When **Emacs key bindings** are enabled (`PromptPlus.Config.EmacsKeyBindings = true`), the text
> also responds to `Ctrl+A/E/B/F/D/K/U/W/Y/T` and friends. See
> [Keyboard Bindings](../../keyboard-bindings.md) for the full table.

`Secret` has **no** Tab autocomplete and **no** F3 history — those `Input` features are omitted by
design (a secret must not be suggested or persisted).

---

## The mask character

Every accepted character is drawn as a single mask symbol. The symbol is resolved in this order:

1. The `value` passed to [`MaskSecret(char?, bool)`](methods.md#masksecret), when not `null`.
2. Otherwise `PromptPlus.Config.SecretChar` — the global default, `'#'`.

```csharp
PromptPlus.Config.SecretChar = '•';   // change the default for every Secret control
```

Calling `MaskSecret('*')` overrides the config for that one control. If you never call
`MaskSecret`, the control still masks input using `Config.SecretChar` and still allows F2 reveal.

---

## Revealing the value (F2)

- When `MaskSecret(..., enabledView: true)` (the default), pressing **F2** toggles between the
  masked view and the plain text so the user can verify what they typed. Pressing F2 again re-masks it.
- When `enabledView: false`, F2 does nothing and the value can never be shown on screen.

The reveal hotkey is configurable via `PromptPlus.Config.HotKeyInputPasswordView` (default `F2`).

> ?? Disable reveal (`enabledView: false`) when someone might be looking over the user's shoulder or
> the session is being recorded — a PIN entry is a good candidate.

---

## How a keystroke is processed

The restrictions apply in a fixed order, so it helps to picture the pipeline:

```
key pressed
   ¦
   +- is it a control/navigation key? --? handled (move, delete, F2, confirm, …)
   ¦
   +- printable character
          ¦
          +- AcceptInput(c) == false ? --? ignored
          +- length already == MaxLength ? --? ignored
          +- InputToCase applied (upper/lower)
          +- inserted at cursor, drawn as the mask symbol
```

Consequently:

- `AcceptInput` and `MaxLength` are **preventive** — they stop bad input from ever appearing.
- `InputToCase` transforms accepted characters on the way in, so `.Content` is already cased.
- The mask affects only the **display**; `.Content` always holds the real characters.

---

## Confirmation & validation flow

Pressing **Enter** runs this sequence:

1. Validation runs — [`PredicateValid`](methods.md#predicatevalid) /
   [`PredicateValidAsync`](methods.md#predicatevalidasync), if configured.
2. **Valid** ? the control closes and returns `ResultPrompt<string>` with `IsAborted == false`.
   **Invalid** ? the control stays open, shows the error line, and waits for more input.

> ?? Validation only runs on confirm, never per keystroke. Use `AcceptInput` for per-keystroke rules
> and `PredicateValid` for whole-value rules (length, complexity, match against a policy).

Because `Secret` has no `DefaultIfEmpty`, an empty field confirms as an empty string — add a
`PredicateValid` if an empty secret is not acceptable.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Secret` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the field after confirm; only the answer line remains |
| `HideOnAbort(true)` | Erases the field after Esc |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

---

## Security note

`Secret` hides the value on screen, but it cannot secure what your application does with it after
`Run()` returns. Follow these rules:

- **Never log or print `.Content`.** Do not write it to the console, a log file, or telemetry.
- **Do not enable history or any persistence.** `Secret` has no `EnabledHistory` on purpose;
  do not route a secret through `Input` (which can) or through your own on-disk cache.
- **Do not offer secrets as suggestions.** `Secret` has no autocomplete for the same reason.
- **Consume, then discard.** Use the value immediately (authenticate, build the connection) and
  avoid holding it in long-lived variables.
- **Prefer `enabledView: false`** for shoulder-surfing-prone contexts (recorded sessions, shared
  screens).

---

## Edge cases & gotchas

- **Single-line rendering.** The masked value is displayed on one line. If it is wider than the
  terminal, a sliding window is shown, but `.Content` always holds the full untruncated text.
- **Newlines** are not accepted (Enter confirms). Secrets are single-line values.
- **Aborted results** carry `.Content == ""`. Always branch on `IsAborted` before using the value.
- **Async callbacks block the UI thread** — see the warning under
  [`PredicateValidAsync`](methods.md#predicatevalidasync).

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key and Emacs reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
- [Input ? Operations](../input/operations.md) — the un-masked sibling, including history behavior
