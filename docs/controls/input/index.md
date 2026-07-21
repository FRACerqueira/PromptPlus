<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Input**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Input — Methods →](methods.md)

---

> Single-line, free-text entry. The user types plain text and confirms with **Enter**.

The `Input` control is the workhorse for collecting any unstructured string — a name, an
e‑mail address, a URL, a search term, a note. It supports live character filtering, case
coercion, length limits, confirmation-time validation, Tab autocomplete suggestions, and
persistent history — all through a single fluent chain.

> 🔒 Need to hide what the user types (passwords, API keys, PINs)? Use the
> [**Secret**](../secret/index.md) control instead — it shares this same API plus a mask character.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, validation flow, history, autocomplete, and edge cases |
| [Styles](styles.md) | The `InputStyles` regions and how to recolor them |

---

## When to use it

| Use `Input` when… | Consider instead… |
|---|---|
| You need any free-form string | — |
| The value is a password or secret | [Secret](../secret/index.md) |
| The value must match a fixed pattern (date, phone, currency) | [MaskEdit](../maskedit/index.md) |
| The user should pick from a known list | [Select](../select/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Input("Your name")
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"Hello, {result.Content}!");
```

- `Input("Your name")` creates the control. The first argument is the **prompt**; an optional
  second argument is a **description** line shown under it.
- `.Run()` renders the field and blocks until the user presses **Enter** (confirm) or **Esc** (abort).
- The call returns a [`ResultPrompt<string>`](../../architecture.md#resultpromptt): read `.Content`
  for the text and `.IsAborted` to detect Esc.

> 💡 Always check `IsAborted` before using `.Content`. On abort, `.Content` is an empty string.

---

## A more complete example

```csharp
using PromptPlusLibrary;

var pin = PromptPlus.Controls
    .Input("PIN", "Only digits are accepted (max 5)")
    .AcceptInput(char.IsDigit)          // reject any non-digit keystroke
    .MaxLength(5)                        // stop accepting after 5 characters
    .PredicateSelected(v => v.Length == 5
        ? (true, null)
        : (false, "PIN must be exactly 5 digits"))   // validate on Enter
    .Run();

if (!pin.IsAborted)
    PromptPlus.Console.WriteLine($"PIN accepted.");
```

This shows the three most common building blocks together: **per-keystroke filtering**
(`AcceptInput`), a **hard length cap** (`MaxLength`), and **confirmation-time validation with a
message** (`PredicateSelected`). See [Operations](operations.md) for how they interact.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Seed a value | `Default`, `DefaultIfEmpty` |
| Restrict typing | `AcceptInput`, `MaxLength`, `InputToCase` |
| Validate on confirm | `PredicateSelected`, `PredicateSelectedAsync` |
| Autocomplete | `SuggestionHandler`, `SuggestionHandlerAsync`, `MinimumSuggestionLength` |
| History | `EnabledHistory` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Input` returns `ResultPrompt<string>`.

| Member | Meaning |
|---|---|
| `.Content` | The confirmed text (empty string if aborted) |
| `.IsAborted` | `true` when the user pressed Esc / the abort key |

```csharp
var (text, aborted) = PromptPlus.Controls.Input("Name").Run();
if (!aborted) PromptPlus.Console.WriteLine(text);
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, validation, history, autocomplete
- [Styles](styles.md) — recolor the prompt, answer, error, and suggestion regions
- [Secret](../secret/index.md) — the masked-input sibling
- [MaskEdit](../maskedit/index.md) — structured input with a fixed pattern
