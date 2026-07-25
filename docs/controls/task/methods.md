<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Task — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Task — Operations →](operations.md)

---

Every fluent method on `ITaskControl`. Each returns the same control instance, so calls chain in any
order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Task(string prompt = "", string? description = null)`, which
> returns `ITaskControl`.

**Quick jump:**
[Action](#action) ·
[ActionAsync](#actionasync) ·
[Context](#context) ·
[ShowElapsedTime](#showelapsedtime) ·
[Spinner](#spinner) ·
[Finish](#finish) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Culture](#culture) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Attaching the work

Set the work with one `Action`/`ActionAsync` overload. Each comes in three shapes: **no context**,
**output only**, and **input + output**. [`Run`](#run) takes *no* work delegate — it only runs the
one you attached here.

### `Action`

```csharp
ITaskControl Action(Action<CancellationToken> handler)
ITaskControl Action(Func<CancellationToken, IDictionary<string, object?>?> handler)
ITaskControl Action(Func<IReadOnlyDictionary<string, object?>, CancellationToken, IDictionary<string, object?>?> handler)
```

Attaches a **synchronous** work delegate.

| Overload | Receives | Returns |
|---|---|---|
| `Action<CancellationToken>` | token | nothing |
| `Func<CancellationToken, IDictionary?>` | token | output context (or `null`) |
| `Func<IReadOnlyDictionary, CancellationToken, IDictionary?>` | input context + token | output context (or `null`) |

```csharp
// Simplest form
PromptPlus.Controls.Task("Processing")
    .Action(token => Thread.Sleep(2000))
    .Run();
```

> Throws `ArgumentNullException` if `handler` is `null`.

---

### `ActionAsync`

```csharp
ITaskControl ActionAsync(Func<CancellationToken, Task> handler)
ITaskControl ActionAsync(Func<CancellationToken, Task<IDictionary<string, object?>?>> handler)
ITaskControl ActionAsync(Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler)
```

Attaches an **asynchronous** work delegate — the async mirror of [`Action`](#action).

```csharp
// Output-only form
PromptPlus.Controls.Task("Generating token")
    .ShowElapsedTime()
    .Spinner(SpinnersType.Default)
    .ActionAsync(async token =>
    {
        await Task.Delay(1500, token).ConfigureAwait(false);
        return new Dictionary<string, object?>
        {
            ["token"] = Guid.NewGuid().ToString("N"),
            ["expiresInSeconds"] = 3600
        };
    })
    .Run();
```

> Throws `ArgumentNullException` if `handler` is `null`.

---

## Passing data in

### `Context`

```csharp
ITaskControl Context(IDictionary<string, object?> context)
```

Provides the **isolated input context** handed to the two-argument `Action`/`ActionAsync` overloads.
The work reads it as a read-only dictionary; it is independent from the output context the work
returns.

```csharp
var inputOnly = new Dictionary<string, object?> { ["user"] = "Alice", ["retries"] = 3 };

PromptPlus.Controls.Task("Authenticating")
    .Context(inputOnly)
    .ActionAsync(async (input, token) =>
    {
        int retries = input.TryGetValue("retries", out var r) && r is int n ? n : 1;
        for (int i = 0; i < retries; i++)
            await Task.Delay(600, token).ConfigureAwait(false);
        return null;   // no output context
    })
    .Run();
```

> Throws `ArgumentNullException` if `context` is `null`.

---

## Feedback while running

### `ShowElapsedTime`

```csharp
ITaskControl ShowElapsedTime(bool value = true, string? format = null)
```

Shows the running elapsed time next to the control. **Hidden by default.**

| Parameter | Meaning |
|---|---|
| `value` | `true` to display the elapsed time. Default `true`. |
| `format` | Optional `TimeSpan` format string. Default `hh\:mm\:ss`. |

```csharp
PromptPlus.Controls.Task("Downloading")
    .ShowElapsedTime()                 // default hh:mm:ss
    .ActionAsync(async t => await Task.Delay(3000, t))
    .Run();
```

> This is `ShowElapsedTime`, not `ElapsedTime`. `StateTask.ElapsedTime` is the *result* property.

---

### `Spinner`

```csharp
ITaskControl Spinner(SpinnersType spinnersType)
```

Shows an animated spinner while the task runs. `SpinnersType` offers many styles across several
families (common ones: `Default`, `Dots`, `Line`, `Star`, `Arc`); on non-Unicode terminals it
automatically falls back to the `Ascii` spinner. See [Spinners](../../spinners.md) and the
[Spinner catalog](../../spinners-catalog.md) for the full list and frames.

```csharp
PromptPlus.Controls.Task("Downloading")
    .Spinner(SpinnersType.Dots)
    .ActionAsync(async t => await Task.Delay(3000, t))
    .Run();
```

---

### `Finish`

```csharp
ITaskControl Finish(string finishtext, string? errortext = null)
```

Sets the text shown when the task ends. When not set, the elapsed time is shown instead.

| Parameter | Meaning |
|---|---|
| `finishtext` | Text shown on successful completion |
| `errortext` | Text shown when the task threw. When `null`, a default localized error message is used. |

```csharp
PromptPlus.Controls.Task("Risky operation")
    .Finish("Done!", "Operation failed!")
    .Action(token =>
    {
        Thread.Sleep(1500);
        throw new InvalidOperationException("Something went wrong");
    })
    .Run();
```

---

### `ChangeDescription`

```csharp
ITaskControl ChangeDescription(Func<TimeSpan, string> value)
```

Recomputes the description line from the elapsed time while the task runs — handy for a live status.

```csharp
PromptPlus.Controls.Task("Working")
    .ShowElapsedTime()
    .ChangeDescription(elapsed => $"Running for {elapsed.TotalSeconds:0} second(s)...")
    .ActionAsync(async t => await Task.Delay(3000, t))
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
ITaskControl ChangeDescriptionAsync(Func<TimeSpan, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

```csharp
PromptPlus.Controls.Task("Working")
    .ShowElapsedTime()
    .ChangeDescriptionAsync(async elapsed =>
    {
        await Task.Delay(1).ConfigureAwait(false);
        return $"Async status at {elapsed:ss} s";
    })
    .ActionAsync(async t => await Task.Delay(3000, t))
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

## Formatting

### `Culture`

```csharp
ITaskControl Culture(CultureInfo culture)
```

Sets the culture used to format the elapsed-time value.

```csharp
using System.Globalization;

PromptPlus.Controls.Task("Processing")
    .ShowElapsedTime()
    .Culture(new CultureInfo("pt-BR"))
    .Action(token => Thread.Sleep(2000))
    .Run();
```

> Throws `ArgumentNullException` if `culture` is `null`.

---

## Styling & behavior

### `Styles`

```csharp
ITaskControl Styles(TaskStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region list and
examples on the [Styles](styles.md) page.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls.Task("Please wait")
    .ShowElapsedTime()
    .Spinner(SpinnersType.Default)
    .Styles(TaskStyles.Prompt, new Style(Color.Yellow, Color.Black))
    .Styles(TaskStyles.ElapsedTime, new Style(Color.Cyan, Color.Black))
    .Styles(TaskStyles.Spinner, new Style(Color.Green, Color.Black))
    .ActionAsync(async t => await Task.Delay(2000, t))
    .Run();
```

---

### `Options`

```csharp
ITaskControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, tooltip, abort key, and hide-after-finish.

```csharp
PromptPlus.Controls.Task("Please wait")
    .Options(o => o
        .ShowTooltip(false)
        .HideAfterFinish(true))
    .Action(token => Thread.Sleep(1000))
    .Run();
```

See [Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list.

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<StateTask> Run(CancellationToken token = default)
```

Displays the control, runs the attached action, and blocks until it completes, throws, or is
cancelled. Returns a [`ResultPrompt<StateTask>`](../../architecture.md#resultpromptt) — see
[Index → Return value](index.md#return-value).

| Parameter | Meaning |
|---|---|
| `token` | A `CancellationToken` that cancels the task while it runs. Also forwarded to the action's own token. |

```csharp
using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
{
    var result = PromptPlus.Controls.Task("Long task")
        .ActionAsync(async token => await Task.Delay(TimeSpan.FromSeconds(10), token))
        .Run(sw.Token);
    // result.IsAborted == true when the token fires first
}
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `TaskStyles` regions
- [Index](index.md) — overview and method map
