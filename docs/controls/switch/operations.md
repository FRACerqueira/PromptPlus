<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Switch — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Switch — Styles →](styles.md)

---

How the `Switch` control behaves while it is running: keyboard, the two labeled states, history, and
the details that matter in real apps.

---

## Anatomy of the control

```
Enable feature?                          ← prompt
Notifications are enabled                 ← description (optional / dynamic)
 Yes   No                                 ← the two states; the active one is highlighted
Left/Right / Space: toggle  Enter  Esc    ← tooltip (toggle with F1 / Ctrl+F1)
```

- The two states show the labels from [`OnValue`](methods.md#onvalue) /
  [`OffValue`](methods.md#offvalue), or the localized Yes/No text when you do not override them.
- The active state is highlighted; the on and off states are painted by the
  [`SwitchOn`](styles.md) / [`SwitchOff`](styles.md) style regions.

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

| Key | Action |
|---|---|
| `←` / `→` | Flip between the off and on states |
| `Space` | Toggle the current state |
| `Tab` / `Shift+Tab` | Move between the states |
| `Enter` | Confirm — returns the current state |
| `Esc` | Abort (when the abort key is enabled) → `IsAborted == true`, `Content == null` |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

There are only two states, so any of the toggle keys moves between on and off; **Enter** commits the
state that is currently highlighted.

---

## States & labels

- A switch is always in one of two states — **on** (`true`) or **off** (`false`).
- Set the starting state with [`Default(bool)`](methods.md#default); it starts **off** if you do
  not.
- Override the shown text with [`OnValue`](methods.md#onvalue) / [`OffValue`](methods.md#offvalue).
  The emoji overloads render an emoji with a plain-text fallback for terminals that cannot display
  it. Without overrides, the labels are the localized Yes/No text (not literal `ON`/`OFF`).
- [`ChangeDescription`](methods.md#changedescription) updates the description line as the state
  flips — handy to spell out the consequence of each state.

---

## History

When [`EnabledHistory(filename, …)`](methods.md#enabledhistory) is set:

- Each confirmed state is persisted to the on-disk store named `filename`.
- With [`Default(value, useDefaultHistory: true)`](methods.md#default), the last saved state is
  pre-loaded as the starting state instead of `value`.
- `IHistoryOptions` (expiration, max items, and so on) governs what is retained.

You can also manage a history store directly with `PromptPlus.Controls.History(filename)` — add,
save, or remove entries programmatically (used in the samples to seed reproducible data).

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Switch` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the toggle after confirm; only the answer line remains |
| `HideOnAbort(true)` | Erases the toggle after Esc |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Description(...)` | Overrides the description text |

> The global `PromptPlus.Config.SwitchWidth` influences how the toggle is laid out.

---

## Edge cases & gotchas

- **Aborted results carry `Content == null`**, not the seeded `Default`. Always branch on
  `IsAborted` before reading the value; because `Content` is `bool?`, compare with `== true` to be
  explicit.
- **Emoji fallback** — when the terminal cannot render the emoji you pass to
  [`OnValue`](methods.md#onvalue) / [`OffValue`](methods.md#offvalue), the plain-text fallback is
  shown instead.
- **Async description callbacks** run while the prompt is open; keep them fast so toggling stays
  responsive.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`, `SwitchWidth`
