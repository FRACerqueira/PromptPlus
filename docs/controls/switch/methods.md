<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Switch — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Switch — Operations →](operations.md)

---

Every fluent method on `ISwitchControl`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Switch(string prompt = "", string? description = null)`,
> which returns `ISwitchControl`.

**Quick jump:**
[Default](#default) ·
[OnValue](#onvalue) ·
[OffValue](#offvalue) ·
[EnabledHistory](#enabledhistory) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Starting value

### `Default`

```csharp
ISwitchControl Default(bool value, bool useDefaultHistory = true)
```

Sets the initial state shown when the control opens. Default is `false` (off).

| Parameter | Meaning |
|---|---|
| `value` | The initial state: `true` for on, `false` for off. |
| `useDefaultHistory` | When `true` (default) **and** history is enabled via [`EnabledHistory`](#enabledhistory), the last confirmed value in history is used instead of `value`. |

```csharp
PromptPlus.Controls.Switch("Enable feature?")
    .Default(true)
    .Run();
```

---

## State labels

By default the two states show the localized Yes/No text. Override either state with plain text or
with an emoji plus a fallback.

### `OnValue`

Two overloads set the label for the **on** (true) state.

```csharp
ISwitchControl OnValue(string value)
ISwitchControl OnValue(EmojiName emojiName, string fallbacktext)
```

| Overload | Meaning |
|---|---|
| `OnValue(string)` | Plain-text label for the on state. |
| `OnValue(EmojiName, string)` | Emoji label, with `fallbacktext` used when the terminal cannot render emoji. |

```csharp
// Plain text
PromptPlus.Controls.Switch("Environment")
    .OnValue("Production")
    .Run();

// Emoji with fallback
using PromptPlusLibrary;

PromptPlus.Controls.Switch("Power")
    .OnValue(EmojiName.GreenCircle, "ON")
    .Run();
```

---

### `OffValue`

Two overloads set the label for the **off** (false) state.

```csharp
ISwitchControl OffValue(string value)
ISwitchControl OffValue(EmojiName emojiName, string fallbacktext)
```

| Overload | Meaning |
|---|---|
| `OffValue(string)` | Plain-text label for the off state. |
| `OffValue(EmojiName, string)` | Emoji label, with `fallbacktext` used when the terminal cannot render emoji. |

```csharp
PromptPlus.Controls.Switch("Power")
    .OnValue(EmojiName.GreenCircle, "ON")
    .OffValue(EmojiName.RedCircle, "OFF")
    .Run();
```

---

## History

### `EnabledHistory`

```csharp
ISwitchControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists the confirmed boolean to a file under `filename` so it can be reloaded as the default on
the next run.

| Parameter | Meaning |
|---|---|
| `filename` | A stable, unique key for this switch's history store. Cannot be `null`. |
| `options` | Optional `IHistoryOptions` configuration (expiration, max items, and so on). |

```csharp
PromptPlus.Controls.Switch("Use default from history?")
    .Default(false, true)             // fall back to the last history value
    .EnabledHistory("switch-history")
    .Run();
```

> 💡 Pair `Default(value, useDefaultHistory: true)` with `EnabledHistory(...)` to pre-load the last
> state the user confirmed. See [Operations → History](operations.md#history) for runtime behavior.

---

## Dynamic description

### `ChangeDescription`

```csharp
ISwitchControl ChangeDescription(Func<bool, string> value)
```

Recomputes the description line as the state changes. The callback receives the current boolean and
returns the description to display.

```csharp
PromptPlus.Controls.Switch("Notifications")
    .ChangeDescription(current => current
        ? "Notifications are enabled"
        : "Notifications are disabled")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
ISwitchControl ChangeDescriptionAsync(Func<bool, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription), for a description sourced
asynchronously.

```csharp
PromptPlus.Controls.Switch("Telemetry")
    .ChangeDescriptionAsync(async current =>
    {
        await Task.Delay(1).ConfigureAwait(false);
        return current ? "Telemetry will be sent" : "Telemetry will stay local";
    })
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

## Appearance & behavior

### `Styles`

```csharp
ISwitchControl Styles(SwitchStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region list and
examples on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here

PromptPlus.Controls.Switch("Use cache")
    .Styles(SwitchStyles.SwitchOn, new Style(Color.Black, Color.Darkgreen))
    .Styles(SwitchStyles.SwitchOff, new Style(Color.Black, Color.Darkred))
    .Run();
```

> Throws `ArgumentNullException` if `style` is `null`.

---

### `Options`

```csharp
ISwitchControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, abort key, tooltip, hide-after-finish, and the extra-info affixes.

```csharp
PromptPlus.Controls.Switch("Verbose mode")
    .Options(opt =>
    {
        opt.Description("Toggle using Left/Right arrows, Tab/Shift+Tab, or Space");
        opt.ShowTooltip(false);
        opt.EnabledAbortKey(true);
        opt.HideAfterFinish(false);
    })
    .Run();
```

See [Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list.

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<bool?> Run(CancellationToken token = default)
```

Renders the toggle and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns a
[`ResultPrompt<bool?>`](../../architecture.md#resultpromptt) whose `.Content` is `null` when the
prompt is cancelled.

| Parameter | Meaning |
|---|---|
| `token` | A `CancellationToken` that cancels the prompt while it waits for input. |

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
var result = PromptPlus.Controls.Switch("Cancelable switch").Run(cts.Token);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `SwitchStyles` regions
- [Index](index.md) — overview and method map
