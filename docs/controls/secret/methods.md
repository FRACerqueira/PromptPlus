<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Secret — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Secret — Operations →](operations.md)

---

Every fluent method on `IInputSecretControl`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Secret(string prompt = "", string? description = null)`,
> which returns `IInputSecretControl`.

**Quick jump:**
[MaskSecret](#masksecret) ·
[InputToCase](#inputtocase) ·
[AcceptInput](#acceptinput) ·
[MaxLength](#maxlength) ·
[PredicateSelected](#predicateselected) ·
[PredicateSelectedAsync](#predicateselectedasync) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Masking & reveal

### `MaskSecret`

```csharp
IInputSecretControl MaskSecret(char? value = null, bool enabledView = true)
```

Sets the character that hides each typed character on screen, and whether the user may reveal the
plain text with **F2**.

| Parameter | Meaning |
|---|---|
| `value` | The mask symbol shown in place of each character. When `null`, falls back to `PromptPlus.Config.SecretChar` (default `'#'`). |
| `enabledView` | When `true` (default), the user can press **F2** to toggle between the masked view and the plain text. When `false`, the value can never be revealed on screen. |

```csharp
using PromptPlusLibrary;

// Hide with '*', and forbid reveal
PromptPlus.Controls.Secret("PIN")
    .MaskSecret('*', enabledView: false)
    .Run();
```

> If you never call `MaskSecret`, the control still masks input using
> `PromptPlus.Config.SecretChar` and allows F2 reveal (`enabledView` defaults to on). Call it
> only to change the symbol or to disable reveal. See
> [Operations → The mask character](operations.md#the-mask-character).

---

## Restricting what can be typed

### `InputToCase`

```csharp
IInputSecretControl InputToCase(CaseOptions value)
```

Coerces every typed character to a casing rule as it is entered.

| `CaseOptions` | Effect |
|---|---|
| `Any` | No transformation (default) |
| `Uppercase` | Letters become upper case |
| `Lowercase` | Letters become lower case |

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // CaseOptions

PromptPlus.Controls.Secret("ApiKey", "Input is transformed to lowercase")
    .InputToCase(CaseOptions.Lowercase)
    .Run();
```

---

### `AcceptInput`

```csharp
IInputSecretControl AcceptInput(Func<char, bool> value)
```

A per-keystroke filter. The callback receives each character the moment it is typed; return
`true` to accept it or `false` to silently ignore it. Rejected characters never enter the field.

```csharp
using PromptPlusLibrary;

// Digits only
PromptPlus.Controls.Secret("PIN")
    .AcceptInput(char.IsDigit)
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `MaxLength`

```csharp
IInputSecretControl MaxLength(int maxLength)
```

Caps the number of characters. Once the limit is reached, further keystrokes are ignored.
A value of `0` or less means **no limit** (the default).

```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Secret("PIN", "Max 4 digits")
    .MaxLength(4)
    .Run();
```

---

## Validating the confirmed value

Validation runs **when the user presses Enter**. If it fails, the control stays open and shows an
error (styled with [`InputStyles.Error`](styles.md)); the value is only returned when validation passes.

### `PredicateSelected`

Two overloads — pick the tuple form when you want to show a custom message.

```csharp
IInputSecretControl PredicateSelected(Func<string, bool> value)
IInputSecretControl PredicateSelected(Func<string, (bool, string?)> value)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<string, bool>` | `true` = valid | On failure, a generic error is shown |
| `Func<string, (bool, string?)>` | `(isValid, message)` | On failure, `message` is shown (or a generic one if `null`) |

```csharp
using PromptPlusLibrary;
using System.Text.RegularExpressions;

// Boolean form — complexity rule
PromptPlus.Controls.Secret("Password", "Min 8 chars with upper/lower/digit/special")
    .PredicateSelected(x =>
    {
        var rule = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        return rule.IsMatch(x);
    })
    .Run();

// Message form
PromptPlus.Controls.Secret("PIN")
    .PredicateSelected(v => v.Length == 4
        ? (true, null)
        : (false, "PIN must be exactly 4 digits"))
    .Run();
```

---

### `PredicateSelectedAsync`

Asynchronous counterparts of [`PredicateSelected`](#predicateselected), for validation that awaits
I/O (a credential store, an HTTP call).

```csharp
IInputSecretControl PredicateSelectedAsync(Func<string, Task<bool>> value)
IInputSecretControl PredicateSelectedAsync(Func<string, Task<(bool, string?)>> value)
```

```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Secret("Password", "Minimum 8 chars (async)")
    .PredicateSelectedAsync(async x =>
    {
        await Task.Delay(1);
        return x.Length < 8
            ? (false, "Password must have at least 8 chars")
            : (true, (string?)null);
    })
    .Run();
```

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — it does not run
> in parallel with the render loop. Keep it fast; long calls freeze the prompt until they return.

---

## Dynamic description

### `ChangeDescription`

```csharp
IInputSecretControl ChangeDescription(Func<string, string> value)
```

Recomputes the description line on every keystroke. The callback receives the current text and
returns the description to display — handy for a live length counter.

```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Secret("Password", "Minimum 8 chars")
    .ChangeDescription(input => $"Length: {input.Length}")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.
>
> 💡 The callback receives the raw text — show a derived value like `input.Length`, never the
> secret itself.

---

### `ChangeDescriptionAsync`

```csharp
IInputSecretControl ChangeDescriptionAsync(Func<string, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Secret("Password", "Minimum 8 chars (async)")
    .ChangeDescriptionAsync(async input =>
    {
        await Task.Delay(1);
        return $"Length: {input.Length}";
    })
    .Run();
```

---

## Appearance & behavior

### `Styles`

```csharp
IInputSecretControl Styles(InputStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region
list and examples on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here

PromptPlus.Controls.Secret("PIN")
    .Styles(InputStyles.Answer, Color.Green)
    .Run();
```

> Throws `ArgumentNullException` if `style` is `null`.

---

### `Options`

```csharp
IInputSecretControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, abort key, tooltip, hide-after-finish, and the extra-info affixes.

```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Secret("Password")
    .Options(o => o
        .EnabledAbortKey(false)   // no Esc for this field
        .ShowTooltip(true)
        .HideAfterFinish(true))   // erase the UI once confirmed
    .Run();
```

See [Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list.

---

## Running the control

### `Run`

```csharp
ResultPrompt<string> Run(CancellationToken token = default)
```

Renders the masked field and blocks until the user confirms (**Enter**) or aborts (**Esc**).
Returns a [`ResultPrompt<string>`](../../architecture.md#resultpromptt).

| Parameter | Meaning |
|---|---|
| `token` | A `CancellationToken` that cancels the prompt while it waits for input. |

```csharp
using PromptPlusLibrary;

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var result = PromptPlus.Controls.Secret("Password").Run(cts.Token);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `InputStyles` regions
- [Index](index.md) — overview and method map
- [Input → Methods](../input/methods.md) — the full un-masked API for comparison
