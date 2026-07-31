<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **KeyPress**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [KeyPress — Methods →](methods.md)

---

> Wait for a single keystroke. The user presses **one key** and the control returns immediately — no Enter required.

The `KeyPress` control is the primitive for "press any key to continue" pauses and for
single-key menus (press **A**, **B**, or **C**). By default any key satisfies it; register one or
more valid keys and it keeps waiting until the user presses an accepted key (or combination),
showing an optional message for every rejected key. Everything is configured through a single
fluent chain.

> ✅ Need a plain yes/no question? Use the [**Confirm**](../confirm/index.md) control — it is the
> same `IKeyPressControl` with the culture-specific Yes/No keys already registered.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, valid-key matching, invalid-key message, tooltip, abort |
| [Styles](styles.md) | The `KeyPressStyles` regions and how to recolor them |

---

## When to use it

| Use `KeyPress` when… | Consider instead… |
|---|---|
| You want a "press any key to continue" pause | — |
| You want a single-key choice (A / B / C, 1 / 2) | — |
| You need a yes/no answer | [Confirm](../confirm/index.md) |
| The user should pick from a labelled list | [Select](../select/index.md) |
| You need free-form typed text | [Input](../input/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .KeyPress("Press any key to continue")
    .Run();

if (!result.IsAborted && result.Content.HasValue)
    PromptPlus.Console.WriteLine($"You pressed {result.Content.Value.Key}");
```

- `KeyPress("Press any key to continue")` creates the control. The first argument is the
  **prompt**; an optional second argument is a **description** line shown under it.
- With no valid keys registered, **any key** ends the wait.
- `.Run()` renders the prompt and blocks until the user presses a key (or **Esc** to abort).
- The call returns a [`ResultPrompt<ConsoleKeyInfo?>`](../../architecture.md#resultpromptt): read
  `.Content` for the key that was pressed and `.IsAborted` to detect Esc.

> 💡 `.Content` is a **nullable** `ConsoleKeyInfo?`. Guard with `.Content.HasValue` before reading
> `.Content.Value.Key` or `.Content.Value.KeyChar`. On abort, `.Content` has no value.

---

## A more complete example

```csharp
using PromptPlusLibrary;

var choice = PromptPlus.Controls
    .KeyPress("Press a valid key", "A, Ctrl+B, N(Off), Y(On)")
    .AddValidKey(ConsoleKey.A)
    .AddValidKey(ConsoleKey.B, ConsoleModifiers.Control)
    .AddValidKey(ConsoleKey.N, null, "Off")
    .AddValidKey(ConsoleKey.Y, null, "On")
    .ShowMessage(key => $"Invalid key '{key.Key}'. Try A, Ctrl+B, N or Y.")
    .Run();

if (!choice.IsAborted && choice.Content is { } key)
    PromptPlus.Console.WriteLine($"Accepted {key.Key}");
```

This shows the three building blocks together: **restricting input** (each
[`AddValidKey`](methods.md#addvalidkey) accumulates one accepted combination), an optional
**display label** for the tooltip, and a **message for rejected keys**
([`ShowMessage`](methods.md#showmessage)). See [Operations](operations.md) for how they interact.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Restrict which keys are accepted | `AddValidKey` |
| Message for rejected keys | `ShowMessage`, `ShowMessageAsync` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`KeyPress` returns `ResultPrompt<ConsoleKeyInfo?>`.

| Member | Meaning |
|---|---|
| `.Content` | The pressed key as a nullable `ConsoleKeyInfo?` (no value when aborted) |
| `.IsAborted` | `true` when the user pressed Esc / the abort key |

```csharp
var result = PromptPlus.Controls.KeyPress("Continue?").Run();
if (!result.IsAborted && result.Content.HasValue)
{
    var info = result.Content.Value;
    PromptPlus.Console.WriteLine($"Key={info.Key}, Char={info.KeyChar}");
}
```

> The result is a `ConsoleKeyInfo?`, **not a bool** — it carries the physical key
> (`.Key`), the character (`.KeyChar`), and any held modifiers (`.Modifiers`).

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — valid-key matching, invalid-key message, tooltip, abort
- [Styles](styles.md) — recolor the prompt, answer, error, and tooltip regions
- [Confirm](../confirm/index.md) — the yes/no preset built on this same control
