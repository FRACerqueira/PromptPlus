<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Time — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Time — Operations →](operations.md)

---

Every fluent method on `ITimeControl`. Each returns the same control instance, so calls chain in any
order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Time(string prompt = "", string? description = null)`, which
> returns `ITimeControl`. [`Duration`](#duration) is **required** — without it there is no length to
> count.

**Quick jump:**
[Duration](#duration) ·
[DisplayMode](#displaymode) ·
[Format](#format) ·
[Spinner](#spinner) ·
[Finish](#finish) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Culture](#culture) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Length

### `Duration`

```csharp
ITimeControl Duration(TimeSpan duration)
ITimeControl Duration(int seconds)
```

Sets how long the control waits while showing the count. **Required**; must be greater than zero.

| Overload | Use when |
|---|---|
| `Duration(int seconds)` | You have a whole number of seconds |
| `Duration(TimeSpan duration)` | You need sub-second or larger precision |

```csharp
// seconds
PromptPlus.Controls.Time("Starting in").Duration(3).Run();

// TimeSpan
PromptPlus.Controls.Time("Cooling down").Duration(TimeSpan.FromSeconds(10)).Run();
```

> Both overloads throw `ArgumentOutOfRangeException` when the value is less than or equal to zero.

---

## Display

### `DisplayMode`

```csharp
ITimeControl DisplayMode(TimeDisplayMode mode)
```

Chooses whether the on-screen number counts **down** or **up**. Default `TimeDisplayMode.Countdown`.

| `TimeDisplayMode` | Shows |
|---|---|
| `Countdown` | The remaining time, counting down to zero (default) |
| `Elapsed` | The elapsed time, counting up from zero to the duration |

```csharp
PromptPlus.Controls.Time("Running")
    .Duration(5)
    .DisplayMode(TimeDisplayMode.Elapsed)
    .Run();
```

> Either way, `Run()` still returns the **elapsed** time in `.Content`.

---

### `Format`

```csharp
ITimeControl Format(string format)
```

Sets the `TimeSpan` format string used to render the number. Default `hh\:mm\:ss`.

```csharp
PromptPlus.Controls.Time("Countdown")
    .Duration(5)
    .Format(@"mm\:ss\:fff")
    .Run();
```

> Use verbatim strings (`@"…"`) or escape the separators (`\:`) as `TimeSpan` formatting requires.

---

### `Spinner`

```csharp
ITimeControl Spinner(SpinnersType spinnersType)
```

Shows an animated spinner next to the time value while the countdown runs. `SpinnersType` offers many
styles across several families (common ones: `Default`, `Dots`, `Line`, `Star`); on non-Unicode
terminals it automatically falls back to the `Ascii` spinner. See [Spinners](../../spinners.md) and the
[Spinner catalog](../../spinners-catalog.md) for the full list and frames.

```csharp
PromptPlus.Controls.Time("Please wait")
    .Duration(5)
    .Spinner(SpinnersType.Default)
    .Run();
```

---

### `Finish`

```csharp
ITimeControl Finish(string finishtext)
```

Sets the text shown when the countdown finishes. When not set, the elapsed time is shown.

```csharp
PromptPlus.Controls.Time("Please wait")
    .Duration(3)
    .Finish("Done!")
    .Run();
```

---

## Text & culture

### `ChangeDescription`

```csharp
ITimeControl ChangeDescription(Func<TimeSpan, string> value)
```

Recomputes the description line from the remaining time as the countdown runs.

```csharp
PromptPlus.Controls.Time("Please wait")
    .Duration(5)
    .ChangeDescription(remaining => $"Remaining: {remaining.TotalSeconds:0} second(s)")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`. Note the callback receives the **remaining**
> time; in `Elapsed` display mode compute the remainder yourself (`duration - elapsed`).

---

### `ChangeDescriptionAsync`

```csharp
ITimeControl ChangeDescriptionAsync(Func<TimeSpan, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

```csharp
PromptPlus.Controls.Time("Please wait")
    .Duration(5)
    .ChangeDescriptionAsync(async remaining =>
    {
        await Task.Delay(1).ConfigureAwait(false);
        return $"Async remaining: {remaining:ss} s";
    })
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `Culture`

```csharp
ITimeControl Culture(CultureInfo culture)
```

Sets the culture used to format the countdown value.

```csharp
using System.Globalization;

PromptPlus.Controls.Time("Aguarde")
    .Duration(4)
    .Culture(new CultureInfo("pt-BR"))
    .Run();
```

> Throws `ArgumentNullException` if `culture` is `null`.

---

## Styling & behavior

### `Styles`

```csharp
ITimeControl Styles(TimeStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region list and
examples on the [Styles](styles.md) page.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls.Time("Please wait")
    .Duration(5)
    .Styles(TimeStyles.Prompt, new Style(Color.Yellow, Color.Black))
    .Styles(TimeStyles.Answer, new Style(Color.Green, Color.Black))
    .Run();
```

---

### `Options`

```csharp
ITimeControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, tooltip, abort key, and hide-after-finish.

```csharp
PromptPlus.Controls.Time("Please wait")
    .Duration(4)
    .Options(opt =>
    {
        opt.Description("Press ESC to abort the countdown");
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
ResultPrompt<TimeSpan> Run(CancellationToken token = default)
```

Displays the countdown and blocks until it completes or is cancelled. Returns a
[`ResultPrompt<TimeSpan>`](../../architecture.md#resultpromptt) whose `.Content` is the elapsed time.

| Parameter | Meaning |
|---|---|
| `token` | A `CancellationToken` that ends the countdown early. |

```csharp
using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
{
    var result = PromptPlus.Controls.Time("Cancelable countdown", "Runs with a CancellationToken")
        .Duration(10)
        .Run(sw.Token);
    // result.IsAborted == true; result.Content is the ~2s elapsed
}
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `TimeStyles` regions
- [Index](index.md) — overview and method map
