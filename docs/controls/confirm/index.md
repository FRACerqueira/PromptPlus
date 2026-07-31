<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Confirm**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Confirm — Methods →](methods.md)

---

> A yes/no question. The user presses the culture-specific **Yes** or **No** key and the control returns immediately.

`Confirm` is a preset of the [**KeyPress**](../keypress/index.md) control. It returns the very same
`IKeyPressControl` interface and behaves identically — the only difference is that the Yes and No
keys for the current culture are **already registered as valid keys** for you, so the user can only
answer with Yes or No.

> 🔑 Because Confirm *is* a KeyPress, its full API reference lives on the KeyPress pages. This page
> focuses on the yes/no pattern and how to read the result. See
> [Methods](methods.md), [Operations](operations.md), and [Styles](styles.md) here for the short
> version with links to the shared reference.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | The yes/no pattern and how to interpret the return value |
| [Methods](methods.md) | The shared `IKeyPressControl` API (links to the KeyPress reference) |
| [Operations](operations.md) | Runtime behavior (links to the KeyPress reference) |
| [Styles](styles.md) | The `KeyPressStyles` regions (links to the KeyPress reference) |

---

## When to use it

| Use `Confirm` when… | Consider instead… |
|---|---|
| You need a plain yes/no answer | — |
| You need a different single-key choice (A / B / C) | [KeyPress](../keypress/index.md) |
| You need to pick from a labelled list | [Select](../select/index.md) |

---

## The yes/no pattern

```csharp
using PromptPlusLibrary;

var confirm = PromptPlus.Controls
    .Confirm("Apply changes?")
    .Run();

if (confirm.IsAborted)
{
    PromptPlus.Console.WriteLine("Cancelled.");
}
else if (confirm.Content is { } k &&
         char.ToUpperInvariant(k.KeyChar) == char.ToUpperInvariant(PromptPlus.Config.YesChar))
{
    PromptPlus.Console.WriteLine("Yes — applying changes.");
}
else
{
    PromptPlus.Console.WriteLine("No — nothing changed.");
}
```

- `Confirm("Apply changes?")` creates the control with the culture Yes/No keys pre-registered.
- `.Run()` returns a [`ResultPrompt<ConsoleKeyInfo?>`](../../architecture.md#resultpromptt) — exactly
  like KeyPress, **not a `bool`**.
- To decide yes vs. no, compare the pressed `KeyChar` to
  [`PromptPlus.Config.YesChar`](../../global-behaviors.md). Anything else that reached `.Content`
  (the No key) is a "no".

> 💡 The Yes/No keys follow the current culture. In `en-US` they are **Y**/**N**; setting
> `PromptPlus.Config.DefaultCulture` to another culture (e.g. `pt-BR`) switches them to that
> culture's letters, and `PromptPlus.Config.YesChar` / `NoChar` reflect the active pair.

---

## Return value

`Confirm` returns `ResultPrompt<ConsoleKeyInfo?>` — identical to [KeyPress](../keypress/index.md).

| Member | Meaning |
|---|---|
| `.Content` | The pressed key as a nullable `ConsoleKeyInfo?` (no value when aborted) |
| `.IsAborted` | `true` when the user pressed Esc / the abort key |

Interpret it with the Yes-char comparison shown above:

```csharp
static bool IsYes(ResultPrompt<ConsoleKeyInfo?> r) =>
    !r.IsAborted &&
    r.Content is { } k &&
    char.ToUpperInvariant(k.KeyChar) == char.ToUpperInvariant(PromptPlus.Config.YesChar);
```

> Always branch on `.IsAborted` first — an aborted confirm has no `.Content` value and is neither
> "yes" nor "no".

---

## Prompt and description

Like KeyPress, `Confirm` takes an optional prompt, description, and `showresult` flag:

```csharp
PromptPlus.Controls
    .Confirm("Apply changes", "Press the culture-specific Yes/No key")
    .Run();
```

```csharp
IKeyPressControl Confirm(string prompt = "", string? description = null, bool showresult = false)
```

---

## See also

- [KeyPress](../keypress/index.md) — the general control Confirm is built on
- [Methods](methods.md) — the shared API (linked to the KeyPress reference)
- [Operations](operations.md) — runtime behavior
- [Styles](styles.md) — the `KeyPressStyles` regions
- [Global Behaviors](../../global-behaviors.md) — `YesChar`, `NoChar`, `DefaultCulture`
