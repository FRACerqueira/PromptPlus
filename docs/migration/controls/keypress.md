# Migration v5.x → v6.x: KeyPress and Confirm

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## KeyPress

Both v5.x and v6.x `KeyPress` return `ResultPrompt<ConsoleKeyInfo?>`. Read the key with `result.Content` (not `result.Value`).

### Breaking Changes

#### 1. `AddKeyValid` → `AddValidKey` (parameter `showtext` → `displayText`)

**Before (v5.x):**
```csharp
using PromptPlusLibrary;

PromptPlus.Controls.KeyPress("Press a key:")
    .AddKeyValid(ConsoleKey.Enter, null, "Enter to confirm")
    .AddKeyValid(ConsoleKey.Escape, null, "Esc to cancel")
    .Run();
```

**After (v6.x):**
```csharp
PromptPlus.Controls.KeyPress("Press a key:")
    .AddValidKey(ConsoleKey.Enter, null, "Enter to confirm")
    .AddValidKey(ConsoleKey.Escape, null, "Esc to cancel")
    .Run();
```

> Renamed only — the signature is identical.

#### 2. `Timeout` — removed with no equivalent

**Before (v5.x):**
```csharp
PromptPlus.Controls.KeyPress("Continue? (waiting 10s...)")
    .AddKeyValid(ConsoleKey.Y, null, "Yes")
    .AddKeyValid(ConsoleKey.N, null, "No")
    .Timeout(TimeSpan.FromSeconds(10), ConsoleKey.N)
    .Run();
```

**After (v6.x):**
```csharp
// Timeout is not available in v6.x.
// Alternative: use a CancellationToken with an external timeout.
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
var result = PromptPlus.Controls.KeyPress("Continue?")
    .AddValidKey(ConsoleKey.Y, null, "Yes")
    .AddValidKey(ConsoleKey.N, null, "No")
    .Run(cts.Token);

if (result.IsAborted)
{
    // timed out — treat as your default answer
}
```

> ⚠️ With a `CancellationToken` the control is aborted (it does **not** return a default key). If you need a default key on expiry, implement that logic externally.

#### 3. `ShowCountDown` — removed with no equivalent

```csharp
// v5.x only — remove this line during migration
.ShowCountDown(true)
```

#### 4. `ShowInvalidKey` — removed with no equivalent

```csharp
// v5.x only — remove this line during migration
.ShowInvalidKey(false)
```

#### 5. `Spinner` — removed with no equivalent

```csharp
// v5.x only — Spinner is not available on KeyPress in v6.x
.Spinner(SpinnersType.Dots)
```

---

### What's new in v6.x

#### `ShowMessage` — display a dynamic message after the key is pressed

```csharp
var result = PromptPlus.Controls.KeyPress("Press Y or N:")
    .AddValidKey(ConsoleKey.Y, null, "Yes")
    .AddValidKey(ConsoleKey.N, null, "No")
    .ShowMessage(key => key.Key == ConsoleKey.Y ? "Confirmed!" : "Cancelled.")
    .Run();
```

#### `ShowMessageAsync`

```csharp
var result = PromptPlus.Controls.KeyPress("Press a key:")
    .AddValidKey(ConsoleKey.Enter, null, "Continue")
    .ShowMessageAsync(async (key, ct) => await GetExtraInfoAsync(key, ct))
    .Run();
```

---

## Confirm

`Confirm` is a `KeyPress` preconfigured with Yes/No keys derived from the culture (or `YesChar`/`NoChar`). It returns `ResultPrompt<ConsoleKeyInfo?>` — the same as `KeyPress`.

**Before (v5.x):**
```csharp
var confirmed = PromptPlus.Controls.Confirm("Continue?").Run();

if (confirmed.Content?.Key == ConsoleKey.Y)
{
    // confirmed
}
```

**After (v6.x):**
```csharp
// Same shape — read the key from .Content (not .Value)
var confirmed = PromptPlus.Controls.Confirm("Continue?").Run();

if (confirmed.Content?.Key == ConsoleKey.Y)
{
    // confirmed
}
```

---

## Full API reference — KeyPress v5.x vs v6.x

| Method | v5.x | v6.x | Change |
|---|---|---|---|
| `AddKeyValid(key, modifiers, showtext)` | ✅ | ❌ | Renamed to `AddValidKey` |
| `AddValidKey(key, modifiers, displayText)` | ❌ | ✅ | New name |
| `Timeout(TimeSpan/int, ConsoleKey, ConsoleModifiers?)` | ✅ | ❌ | Removed with no equivalent |
| `ShowCountDown(bool)` | ✅ | ❌ | Removed with no equivalent |
| `ShowInvalidKey(bool)` | ✅ | ❌ | Removed with no equivalent |
| `Spinner(SpinnersType)` | ✅ | ❌ | Removed with no equivalent |
| `ShowMessage(Func<ConsoleKeyInfo, string>)` | ❌ | ✅ | New |
| `ShowMessageAsync(...)` | ❌ | ✅ | New |
| `Options(Action<IControlOptions>)` | ✅ | ✅ | Unchanged |
| `Styles(KeyPressStyles, Style)` | ✅ | ✅ | Unchanged |
| `Run(CancellationToken)` → `ResultPrompt<ConsoleKeyInfo?>` | ✅ | ✅ | Unchanged |
