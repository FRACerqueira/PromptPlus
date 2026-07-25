<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Confirm — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Confirm — Operations →](operations.md)

---

`Confirm` returns the same `IKeyPressControl` as [KeyPress](../keypress/index.md), so it exposes the
**identical fluent API**. Rather than duplicate it here, this page summarises the methods and links
to the full reference on the [KeyPress → Methods](../keypress/methods.md) page.

> The factory is
> `PromptPlus.Controls.Confirm(string prompt = "", string? description = null, bool showresult = false)`,
> which returns `IKeyPressControl`. It differs from `KeyPress(...)` only in that the culture Yes/No
> keys are pre-registered as valid keys.

---

## The factory parameters

```csharp
IKeyPressControl Confirm(string prompt = "", string? description = null, bool showresult = false)
```

| Parameter | Meaning |
|---|---|
| `prompt` | The prompt text shown to the user. Default empty. |
| `description` | An optional description line shown under the prompt. Default `null`. |
| `showresult` | When `true`, the answer line stays on screen after the control finishes; when `false` (default) it is hidden after finish. |

```csharp
PromptPlus.Controls.Confirm("Apply changes", "Press Y or N").Run();
```

---

## The shared API

| Method | Purpose | Full reference |
|---|---|---|
| `AddValidKey` | Register an additional valid key (accumulates on top of the pre-set Yes/No keys) | [KeyPress → AddValidKey](../keypress/methods.md#addvalidkey) |
| `ShowMessage` | Message shown for a rejected key | [KeyPress → ShowMessage](../keypress/methods.md#showmessage) |
| `ShowMessageAsync` | Async message for a rejected key | [KeyPress → ShowMessageAsync](../keypress/methods.md#showmessageasync) |
| `Styles` | Recolor a `KeyPressStyles` region | [KeyPress → Styles](../keypress/methods.md#styles) |
| `Options` | Per-control behavior overrides | [KeyPress → Options](../keypress/methods.md#options) |
| `Run` | Display and return `ResultPrompt<ConsoleKeyInfo?>` | [KeyPress → Run](../keypress/methods.md#run) |

> Because Confirm already registers the Yes/No keys, calling `AddValidKey` **adds more** accepted
> keys — it does not replace the yes/no pair. For a plain yes/no question you normally call none of
> these except optionally `Options`, `Styles`, and `Run`.

---

## Typical Confirm chain

```csharp
using PromptPlusLibrary;

var confirm = PromptPlus.Controls
    .Confirm("Delete file?")
    .Options(o => o.ShowTooltip(true))
    .Run();
```

Reading the result (yes/no) is covered on the [Index](index.md#the-yesno-pattern) page.

---

## See also

- [KeyPress → Methods](../keypress/methods.md) — the complete signatures and examples
- [Index](index.md) — the yes/no pattern and return-value interpretation
- [Operations](operations.md) — runtime behavior
- [Styles](styles.md) — the `KeyPressStyles` regions
