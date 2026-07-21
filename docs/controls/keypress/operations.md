<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **KeyPress — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [KeyPress — Styles →](styles.md)

---

How the `KeyPress` control behaves while it is running: the single-keystroke wait, how valid keys
are matched, the invalid-key message, the tooltip, and aborting.

---

## Anatomy of the control

```
Press a valid key                        ← prompt
A, Ctrl+B, N(Off), Y(On)                 ← description (optional)
Invalid key 'Z'. Try A, Ctrl+B, N or Y. ← error line, only after a rejected key
A  Ctrl+B  Off  On   Esc: cancel         ← tooltip: valid keys + hints
```

Every region can be recolored — see [Styles](styles.md).

---

## The single-keystroke wait

Unlike text controls, `KeyPress` does **not** wait for Enter. It returns the instant an accepted key
is pressed:

- With **no valid keys registered**, the very first key the user presses ends the wait and is
  returned in `.Content`.
- With **one or more valid keys registered**, only an accepted key ends the wait; every other key is
  rejected (see below) and the control keeps waiting.

```csharp
// "Press any key to continue" — any key ends the wait
PromptPlus.Controls.KeyPress("Press any key to continue").Run();
```

---

## Valid-key restriction

Each [`AddValidKey`](methods.md#addvalidkey) call **accumulates** one accepted combination. A key is
accepted only when both its `ConsoleKey` and the required modifiers match a registered entry:

```csharp
PromptPlus.Controls.KeyPress("Press a valid key", "A, Ctrl+B, N(Off), Y(On)")
    .AddValidKey(ConsoleKey.A)                              // A alone
    .AddValidKey(ConsoleKey.B, ConsoleModifiers.Control)   // requires Ctrl held
    .AddValidKey(ConsoleKey.N, null, "Off")                // N, labelled "Off"
    .AddValidKey(ConsoleKey.Y, null, "On")                 // Y, labelled "On"
    .Run();
```

- **No `AddValidKey` calls → any key is accepted.** This is the "press any key" mode.
- With a modifier requirement, the modifier must be held: `AddValidKey(ConsoleKey.B, ConsoleModifiers.Control)`
  accepts **Ctrl+B**, not a bare **B**.
- The optional `displayText` only changes the tooltip label — the returned `.Content.Value.Key` is
  still the real `ConsoleKey`.

> 💡 To branch on the result, compare `.Content.Value.Key` to the `ConsoleKey` you registered, e.g.
> `if (result.Content is { } k && k.Key == ConsoleKey.Y)`.

---

## Invalid-key message

When valid keys are registered and the user presses one that is not accepted:

1. The control stays open (the wait continues).
2. If a message callback is configured, its text is shown on the error line, styled with
   [`KeyPressStyles.Error`](styles.md).
3. The message clears when the next key is pressed.

Set the text synchronously with [`ShowMessage`](methods.md#showmessage) or asynchronously with
[`ShowMessageAsync`](methods.md#showmessageasync); both receive the rejected `ConsoleKeyInfo`:

```csharp
PromptPlus.Controls.KeyPress("Press a valid key")
    .AddValidKey(ConsoleKey.A)
    .AddValidKey(ConsoleKey.Y, null, "On")
    .ShowMessage(key => $"Invalid key '{key.Key}'. Try A or Y.")
    .Run();
```

> If no message callback is set, a rejected key is simply ignored and the control keeps waiting
> silently.

---

## Tooltip

The tooltip line lists the accepted keys (using each key's `displayText` when supplied) alongside
the standard hints such as the abort key. Toggle it per instance with
[`Options(o => o.ShowTooltip(...))`](methods.md#options) or globally via
[`PromptPlus.Config`](../../global-behaviors.md).

| Key | Action |
|---|---|
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

---

## Abort

When the abort key is enabled (the default), pressing **Esc** cancels the wait:

- `.IsAborted` is `true`.
- `.Content` has **no value** (`HasValue == false`).

```csharp
var result = PromptPlus.Controls.KeyPress("Press a key").Run();
if (result.IsAborted)
    PromptPlus.Console.WriteLine("Cancelled.");
else
    PromptPlus.Console.WriteLine($"Pressed {result.Content!.Value.Key}");
```

Disable Esc with [`Options(o => o.EnabledAbortKey(false))`](methods.md#options) to force the user to
press a valid key.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `KeyPress` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must press a valid key |
| `HideAfterFinish(true)` | Erases the prompt after a key is pressed |
| `HideOnAbort(true)` | Erases the prompt after Esc |
| `ShowTooltip(false)` | Hides the key-hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

> The `showresult` factory parameter is the sibling switch for whether the pressed-key answer line
> remains visible after the control finishes.

---

## Edge cases & gotchas

- **`.Content` is nullable.** Always check `.Content.HasValue` (or pattern-match `is { } k`) before
  reading `.Value` — an aborted result has no value.
- **Modifiers matter.** A registered `Ctrl+B` will not match a bare `B`, and vice versa.
- **Message callbacks block the UI thread** — see the warning under
  [`ShowMessageAsync`](methods.md#showmessageasync).

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Styles](styles.md) — the `KeyPressStyles` regions
- [Confirm → Operations](../confirm/operations.md) — the yes/no interpretation of the same flow
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
