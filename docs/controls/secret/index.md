<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Secret**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [Secret — Methods ?](methods.md)

---

> Single-line, **masked** text entry. The user types, each character is hidden behind a mask symbol, and they confirm with **Enter**.

The `Secret` control is the masked sibling of [`Input`](../input/index.md). Use it whenever the
value must not appear on screen — a password, an API key, a PIN, a connection string, a token. It
shares the same live filtering, case coercion, length cap, and confirmation-time validation as
`Input`, but replaces every typed character with a mask symbol and can optionally let the user peek
at the plain text with **F2**.

> ?? `Secret` deliberately omits the persistence features of `Input` (no history, no autocomplete,
> no seeded default). A secret should never be written to disk or offered as a suggestion — see the
> [security note](operations.md#security-note) in Operations.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, the mask character, F2 reveal, validation flow, and the security note |
| [Styles](styles.md) | The `InputStyles` regions and how to recolor them |

---

## When to use it

| Use `Secret` when… | Consider instead… |
|---|---|
| The value is a password, key, PIN, or token | — |
| The value is ordinary free-form text | [Input](../input/index.md) |
| The value must match a fixed pattern (date, phone, currency) | [MaskEdit](../maskedit/index.md) |
| The user should pick from a known list | [Select](../select/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Secret("Password")
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine("Password captured.");
```

- `Secret("Password")` creates the control. The first argument is the **prompt**; an optional
  second argument is a **description** line shown under it.
- `.Run()` renders the masked field and blocks until the user presses **Enter** (confirm) or
  **Esc** (abort).
- The call returns a [`ResultPrompt<string>`](../../architecture.md#resultpromptt): read `.Content`
  for the entered text and `.IsAborted` to detect Esc.

> ?? Always check `IsAborted` before using `.Content`. On abort, `.Content` is an empty string.
> Never echo `.Content` to the console — the example above prints a confirmation, not the value.

---

## A more complete example

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, InputStyles-agnostic types

var pin = PromptPlus.Controls
    .Secret("PIN", "Only 4 digits are accepted")
    .MaskSecret('*', enabledView: false)   // hide with '*', no F2 reveal
    .AcceptInput(char.IsDigit)             // reject any non-digit keystroke
    .MaxLength(4)                          // stop accepting after 4 characters
    .PredicateValid(v => v.Length == 4
        ? (true, null)
        : (false, "PIN must be exactly 4 digits"))   // validate on Enter
    .Run();

if (!pin.IsAborted)
    PromptPlus.Console.WriteLine("PIN accepted.");
```

This combines the four most common building blocks: a **custom mask** with reveal disabled
(`MaskSecret`), **per-keystroke filtering** (`AcceptInput`), a **hard length cap** (`MaxLength`),
and **confirmation-time validation with a message** (`PredicateValid`). See
[Operations](operations.md) for how they interact.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Masking & reveal | `MaskSecret` |
| Restrict typing | `AcceptInput`, `MaxLength`, `InputToCase` |
| Validate on confirm | `PredicateValid`, `PredicateValidAsync` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

> Unlike [`Input`](../input/index.md#method-map), `Secret` has **no** `Default`, `DefaultIfEmpty`,
> `EnabledHistory`, or suggestion methods — by design.

---

## Return value

`Secret` returns `ResultPrompt<string>`.

| Member | Meaning |
|---|---|
| `.Content` | The confirmed text (empty string if aborted) |
| `.IsAborted` | `true` when the user pressed Esc / the abort key |

```csharp
var (secret, aborted) = PromptPlus.Controls.Secret("Token").Run();
if (!aborted) UseToken(secret);   // consume it — do not print it
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, the mask character, F2 reveal, validation, security note
- [Styles](styles.md) — recolor the prompt, answer, and error regions
- [Input](../input/index.md) — the un-masked sibling with history and autocomplete
- [MaskEdit](../maskedit/index.md) — structured input with a fixed pattern
