<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Confirm — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Confirm — Styles →](styles.md)

---

`Confirm` runs on the same engine as [KeyPress](../keypress/index.md), so its runtime behavior —
the single-keystroke wait, valid-key matching, invalid-key message, tooltip, and abort — is exactly
the same. This page notes only what is specific to the yes/no preset and links to the shared
reference on [KeyPress → Operations](../keypress/operations.md).

---

## Anatomy of the control

```
Apply changes? (Y/N)                     ← prompt — Confirm always appends " (Yes/No key)" itself
Press the culture-specific Yes/No key    ← description (optional)
Esc:Abort.Ctrl+F1:Show/hide tooltip      ← tooltip — does NOT list the valid Y/N keys, only hints
```

Every region can be recolored — see [Styles](styles.md).

---

## What Confirm pre-registers

Confirm calls the KeyPress engine with the **current culture's Yes and No keys already registered as
valid keys**. Consequences:

- Only the Yes key and the No key end the wait; any other key is rejected (and shows the invalid-key
  message if one is configured).
- The specific letters depend on the active culture. In `en-US` they are **Y**/**N**; switching
  `PromptPlus.Config.DefaultCulture` to, say, `pt-BR` switches them to that culture's letters.
- [`PromptPlus.Config.YesChar`](../../global-behaviors.md) / `NoChar` expose the active pair, which is
  how you interpret the result (see [Index](index.md#the-yesno-pattern)).
- `Confirm(...)` **automatically appends** `" ({yesKey}/{noKey})"` to whatever prompt text you pass —
  e.g. `Confirm("Apply changes?")` renders as `Apply changes? (Y/N)`. You don't need to (and
  shouldn't) add this hint yourself.

You can still register **extra** keys with [`AddValidKey`](../keypress/methods.md#addvalidkey); they
are accepted in addition to Yes/No.

---

## The shared behaviors

Everything below works identically to KeyPress — follow the links for the details:

| Behavior | Reference |
|---|---|
| Single-keystroke wait (returns on the first accepted key) | [KeyPress → Single-keystroke wait](../keypress/operations.md#the-single-keystroke-wait) |
| Valid-key restriction (accumulates; Yes/No are pre-set here) | [KeyPress → Valid-key restriction](../keypress/operations.md#valid-key-restriction) |
| Invalid-key message | [KeyPress → Invalid-key message](../keypress/operations.md#invalid-key-message) |
| Tooltip (F1 / Ctrl+F1) | [KeyPress → Tooltip](../keypress/operations.md#tooltip) |
| Abort (Esc → `.IsAborted`, no `.Content` value) | [KeyPress → Abort](../keypress/operations.md#abort) |
| `Options` overrides | [KeyPress → Options that change behavior](../keypress/operations.md#options-that-change-behavior) |

---

## Interpreting yes vs. no

The result is a `ConsoleKeyInfo?`, **not a bool**. Compare the pressed character to the Yes char:

```csharp
var confirm = PromptPlus.Controls.Confirm("Continue?").Run();

if (!confirm.IsAborted &&
    confirm.Content is { } k &&
    char.ToUpperInvariant(k.KeyChar) == char.ToUpperInvariant(PromptPlus.Config.YesChar))
{
    // Yes
}
```

Branch on `.IsAborted` first — an aborted confirm carries no `.Content` value and is neither yes nor
no.

---

## See also

- [KeyPress → Operations](../keypress/operations.md) — the complete runtime reference
- [Index](index.md) — the yes/no pattern and return-value interpretation
- [Methods](methods.md) — the shared API
- [Global Behaviors](../../global-behaviors.md) — `YesChar`, `NoChar`, `DefaultCulture`
