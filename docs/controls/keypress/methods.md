<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **KeyPress — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [KeyPress — Operations →](operations.md)

---

Every fluent method on `IKeyPressControl`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is
> `PromptPlus.Controls.KeyPress(string prompt = "", string? description = null, bool showresult = false)`,
> which returns `IKeyPressControl`. The [`Confirm`](../confirm/index.md) factory has the identical
> signature and returns the same interface — so every method below applies to Confirm too.

**Quick jump:**
[AddValidKey](#addvalidkey) ·
[ShowMessage](#showmessage) ·
[ShowMessageAsync](#showmessageasync) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## The factory parameters

```csharp
IKeyPressControl KeyPress(string prompt = "", string? description = null, bool showresult = false)
```

| Parameter | Meaning |
|---|---|
| `prompt` | The prompt text shown to the user. Default empty. |
| `description` | An optional description line shown under the prompt. Default `null`. |
| `showresult` | When `true`, the pressed-key answer line stays on screen after the control finishes; when `false` (default) it is hidden after finish. |

```csharp
// Keep the answer line visible after the key is pressed
PromptPlus.Controls.KeyPress("Pick one", "Press 1 or 2", showresult: true)
    .Run();
```

---

## Restricting which keys are accepted

### `AddValidKey`

```csharp
IKeyPressControl AddValidKey(ConsoleKey key, ConsoleModifiers? requiredModifiers = null, string? displayText = null)
```

Registers one key (optionally with a required modifier) as an accepted input. **Calls accumulate**:
each `AddValidKey` adds one more accepted combination. If you never call it, **any key is accepted**.

| Parameter | Meaning |
|---|---|
| `key` | The `ConsoleKey` to accept. |
| `requiredModifiers` | A `ConsoleModifiers` that must be held at the same time (e.g. `ConsoleModifiers.Control`). Use `null` (default) to accept the key with no modifier. |
| `displayText` | An optional friendlier label shown in the tooltip instead of the raw key name. |

```csharp
PromptPlus.Controls.KeyPress("Press a valid key")
    .AddValidKey(ConsoleKey.A)                              // A
    .AddValidKey(ConsoleKey.B, ConsoleModifiers.Control)   // Ctrl+B
    .AddValidKey(ConsoleKey.N, null, "Off")                // N, shown as "Off"
    .AddValidKey(ConsoleKey.Y, null, "On")                 // Y, shown as "On"
    .Run();
```

> Once one or more valid keys are registered, pressing any other key does **not** end the control —
> it triggers the invalid-key message (see below) and the control keeps waiting.

---

## Message for rejected keys

### `ShowMessage`

```csharp
IKeyPressControl ShowMessage(Func<ConsoleKeyInfo, string>? message)
```

Sets a synchronous callback that builds the error text shown when the user presses a key that is not
in the accepted set. The callback receives the rejected `ConsoleKeyInfo` and returns the text to
display. Pass `null` to suppress the message.

```csharp
PromptPlus.Controls.KeyPress("Press a valid key")
    .AddValidKey(ConsoleKey.A)
    .AddValidKey(ConsoleKey.Y, null, "On")
    .ShowMessage(key => $"Invalid key '{key.Key}'. Try A or Y.")
    .Run();
```

The message is painted with [`KeyPressStyles.Error`](styles.md) and clears when the next key is
pressed.

---

### `ShowMessageAsync`

```csharp
IKeyPressControl ShowMessageAsync(Func<ConsoleKeyInfo, CancellationToken, Task<string>>? message = null)
```

Asynchronous counterpart of [`ShowMessage`](#showmessage), for message text that awaits I/O. The
callback receives the rejected `ConsoleKeyInfo` and a `CancellationToken` tied to the control's
lifetime, and returns the text. Pass `null` to suppress the message. Registering an async callback
**replaces** any synchronous one set via [`ShowMessage`](#showmessage).

```csharp
PromptPlus.Controls.KeyPress("Choose mode", "Only D (Debug) or R (Release)")
    .AddValidKey(ConsoleKey.D, null, "Debug")
    .AddValidKey(ConsoleKey.R, null, "Release")
    .ShowMessageAsync(async (key, cancellationToken) =>
    {
        await Task.Delay(80, cancellationToken);
        return $"'{key.Key}' is not valid. Use D or R.";
    })
    .Run();
```

> ⚠️ The async callback is awaited **synchronously (blocking) on the UI thread** — it does not run in
> parallel with the render loop. Keep it fast; long calls freeze the prompt until they return.

---

## Appearance & behavior

### `Styles`

```csharp
IKeyPressControl Styles(KeyPressStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region list and
examples on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here

PromptPlus.Controls.KeyPress("Styled sample")
    .Styles(KeyPressStyles.Prompt, new Style(Color.Yellow, Color.Default))
    .Run();
```

> Throws `ArgumentNullException` if `style` is `null`.

---

### `Options`

```csharp
IKeyPressControl Options(Action<IControlOptions> configureOptions)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, abort key, tooltip visibility, hide-after-finish, and the extra-info
affixes.

```csharp
PromptPlus.Controls.KeyPress("Continue?")
    .Options(o => o
        .EnabledAbortKey(false)     // no Esc for this prompt
        .ShowTooltip(true)
        .HideAfterFinish(false))    // keep the UI after a key is pressed
    .Run();
```

See [Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list. Throws `ArgumentNullException` if `configureOptions` is
`null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<ConsoleKeyInfo?> Run(CancellationToken cancellationToken = default)
```

Renders the prompt and blocks until the user presses an accepted key or aborts (**Esc**). Returns a
[`ResultPrompt<ConsoleKeyInfo?>`](../../architecture.md#resultpromptt) — read `.Content.HasValue`
before `.Content.Value`.

| Parameter | Meaning |
|---|---|
| `cancellationToken` | A `CancellationToken` that cancels the wait while the control waits for input. |

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var result = PromptPlus.Controls.KeyPress("Press any key").Run(cts.Token);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `KeyPressStyles` regions
- [Index](index.md) — overview and method map
- [Confirm → Methods](../confirm/methods.md) — the same API, pre-set for yes/no
