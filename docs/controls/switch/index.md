<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Switch**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Switch — Methods →](methods.md)

---

> Toggle a single boolean between its **on** and **off** states. The user flips it with the arrow
> keys or **Space** and confirms with **Enter**.

The `Switch` control is the fastest way to collect one yes/no, enable/disable, or active/inactive
value. It shows two labeled states and lets the user flip between them; the labels can be plain text
or emoji, and the confirmed value can be persisted as history. Everything is configured through a
single fluent chain.

> 🔘 Just need to **display** an on/off indicator without asking for input? Use the read-only
> [**Switch widget**](../../widgets.md#switch) instead — it renders the same toggle but does not
> wait for the user.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, labels, history, and edge cases |
| [Styles](styles.md) | The `SwitchStyles` regions and how to recolor them |

---

## When to use it

| Use `Switch` when… | Consider instead… |
|---|---|
| You need a single on/off boolean | — |
| You want a yes/no key-press confirmation | [Confirm](../confirm/index.md) |
| The user should pick one of several options | [Select](../select/index.md) |
| You only need to display a state, not collect one | [Switch widget](../../widgets.md#switch) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Switch("Enable feature?")
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"Enabled: {result.Content}");
```

- `Switch("Enable feature?")` creates the control. The first argument is the **prompt**; an optional
  second argument is a **description** line shown under it.
- With no configuration the switch starts **off** (`false`) and shows the localized Yes/No labels.
- `.Run()` renders the toggle and blocks until the user presses **Enter** (confirm) or **Esc** (abort).
- The call returns a [`ResultPrompt<bool?>`](../../architecture.md#resultpromptt): read `.Content`
  for the state and `.IsAborted` to detect Esc.

> 💡 The value is nullable. On abort `.Content` is `null` — always check `IsAborted` first.

---

## A more complete example

```csharp
using PromptPlusLibrary;

var env = PromptPlus.Controls
    .Switch("Environment")
    .OnValue("Production")     // label for the on state
    .OffValue("Development")   // label for the off state
    .Default(false)            // start on Development
    .Run();

if (!env.IsAborted)
    PromptPlus.Console.WriteLine($"Environment: {(env.Content == true ? "Production" : "Development")}");
```

This shows the two things you most often customize: the **state labels** (`OnValue` / `OffValue`)
and the **starting value** (`Default`). See [Methods](methods.md) for the emoji label overloads and
[Operations](operations.md) for how flipping behaves at runtime.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Starting value | `Default` |
| State labels | `OnValue`, `OffValue` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Switch` returns `ResultPrompt<bool?>`.

| Member | Meaning |
|---|---|
| `.Content` | The confirmed state (`true` = on, `false` = off; `null` if aborted) |
| `.IsAborted` | `true` when the user pressed Esc / the abort key |

```csharp
var (state, aborted) = PromptPlus.Controls.Switch("Enable?").Run();
if (!aborted) PromptPlus.Console.WriteLine($"{state}");
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, labels, history
- [Styles](styles.md) — recolor the prompt, on/off, and error regions
- [Switch widget](../../widgets.md#switch) — the read-only, display-only sibling
- [Slider](../slider/index.md) — the numeric-range cousin
