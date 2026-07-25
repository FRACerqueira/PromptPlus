<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ProgressBar — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ProgressBar — Operations →](operations.md)

---

Every fluent method on `IProgressBarControl`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.ProgressBar(string prompt = "", string? description = null)`,
> which returns `IProgressBarControl`.

**Quick jump:**
[UpdateHandler](#updatehandler) ·
[UpdateHandlerAsync](#updatehandlerasync) ·
[Range](#range) ·
[Default](#default) ·
[FractionalDigits](#fractionalDigits) ·
[Width](#width) ·
[Fill](#fill) ·
[Spinner](#spinner) ·
[ChangeColor](#changecolor) ·
[ChangeGradient](#changegradient) ·
[HideElements](#hideelements) ·
[Finish](#finish) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Culture](#culture) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## The work loop

The progress value is **not** something you set once — it is driven by a handler that loops while the
work runs. Register exactly one handler (sync *or* async). Inside it you receive a
[`ProgressBarEvent`](operations.md#the-progressbarevent) whose `Update(value)` moves the bar. See
[Operations](operations.md) for the full event surface.

### `UpdateHandler`

```csharp
IProgressBarControl UpdateHandler(
    Action<ProgressBarEvent, CancellationToken> value,
    IDictionary<string, object?>? context = null)
```

Registers a **synchronous** work loop.

| Parameter | Meaning |
|---|---|
| `value` | The loop. Receives the `ProgressBarEvent` and the run's `CancellationToken`. Cannot be `null`. |
| `context` | Optional input key/value data made available to the handler via `evt.InputParam<T>(...)`. |

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .UpdateHandler((bar, token) =>
    {
        while (!token.IsCancellationRequested && !bar.Finish)
        {
            token.WaitHandle.WaitOne(80);
            bar.Update(bar.Value + 2);
        }
    })
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `UpdateHandlerAsync`

```csharp
IProgressBarControl UpdateHandlerAsync(
    Func<ProgressBarEvent, CancellationToken, Task> value,
    IDictionary<string, object?>? context = null)
```

Asynchronous counterpart of [`UpdateHandler`](#updatehandler), for loops that `await` I/O.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .UpdateHandlerAsync(async (bar, token) =>
    {
        while (!token.IsCancellationRequested && !bar.Finish)
        {
            await Task.Delay(80, token).ConfigureAwait(false);
            bar.Update(bar.Value + 2);
        }
    })
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

## Range & value

### `Range`

```csharp
IProgressBarControl Range(double minvalue, double maxvalue)
```

Sets the numeric bounds of the bar. The default range is `0` to `100`. `bar.Finish` becomes `true`
once the value reaches `maxvalue`.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Range(-30, 30)
    .Default(-30)
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `minvalue` is greater than or equal to `maxvalue`.

---

### `Default`

```csharp
IProgressBarControl Default(double value)
```

Sets the **initial** value the bar starts at. Default `0`. Must be inside the configured range.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Range(-30, 30)
    .Default(-30)
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `value` is outside the configured range.

---

### `FractionalDigits`

```csharp
IProgressBarControl FractionalDigits(byte value)
```

Sets how many fractional digits are shown for the value. Default `0` (whole numbers); maximum `5`.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .FractionalDigits(2)
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `value` is greater than `5`.

---

### `Width`

```csharp
IProgressBarControl Width(byte value)
```

Sets the rendered width of the bar track, in characters. Default `40`; minimum `10`.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Width(30)
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `value` is less than `10`.

---

## Appearance

### `Fill`

```csharp
IProgressBarControl Fill(ProgressBarType type)
```

Chooses the visual fill style of the track. Default `ProgressBarType.Fill`.

| `ProgressBarType` | Look |
|---|---|
| `Fill` | Solid filled bar (default) |
| `Bar` | A simple bar |
| `Square` | Square blocks |
| `Light` | Light-weight blocks |
| `DoubleLight` | Double light blocks |
| `Dot` | Dots |

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Fill(ProgressBarType.Square)
    .UpdateHandler(Work)
    .Run();
```

---

### `Spinner`

```csharp
IProgressBarControl Spinner(SpinnersType spinnersType)
```

Shows an animated spinner alongside the bar while the operation is running. `SpinnersType` offers many
styles across several families (common ones: `Default`, `Dots`, `Line`, `Star`, `Arc`); on non-Unicode
terminals it automatically falls back to the `Ascii` spinner. See [Spinners](../../spinners.md) and the
[Spinner catalog](../../spinners-catalog.md) for the full list and frames.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Spinner(SpinnersType.Dots)
    .UpdateHandler(Work)
    .Run();
```

---

### `ChangeColor`

```csharp
IProgressBarControl ChangeColor(Func<double, Style> value)
```

Recomputes the bar color from the current value, so it changes as the bar advances. The callback
receives the value and returns the [`Style`](../../global-styles.md) to paint.

```csharp
using ConsolePlusLibrary;

PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .ChangeColor(value =>
    {
        if (value <= 30) return new Style(Color.Red, Color.Red);
        if (value <= 70) return new Style(Color.Blue, Color.Blue);
        return new Style(Color.Darkgoldenrod, Color.Darkgoldenrod);
    })
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeGradient`

```csharp
IProgressBarControl ChangeGradient(params Color[] colors)
```

Applies a gradient across the filled portion, interpolated over the configured range as the value
advances. Pass two or more colors.

```csharp
using ConsolePlusLibrary;

PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentNullException` if `colors` is `null` or empty.

---

### `HideElements`

```csharp
IProgressBarControl HideElements(HideProgressBar value)
```

Hides one or more visual elements. `HideProgressBar` is a `[Flags]` enum — combine with `|`.

| `HideProgressBar` | Hides |
|---|---|
| `None` | Nothing (default) |
| `Delimit` | The bar delimiters |
| `Range` | The min/max range text |
| `PromptAnswer` | The prompt + answer line |
| `ElapsedTime` | The elapsed-time display |
| `ProgressbarAtFinish` | The whole bar once it finishes |

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .HideElements(HideProgressBar.PromptAnswer | HideProgressBar.Range | HideProgressBar.Delimit)
    .UpdateHandler(Work)
    .Run();
```

---

## Text & culture

### `Finish`

```csharp
IProgressBarControl Finish(string finishtext)
```

Sets the text shown when the bar completes, exposed afterwards as `StateProgress.FinishedText`.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Finish("End progress")
    .UpdateHandler(Work)
    .Run();
```

---

### `ChangeDescription`

```csharp
IProgressBarControl ChangeDescription(Func<double, string> value)
```

Refreshes the description line every time the value changes. The callback receives the current value
and returns the text to display.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .ChangeDescription(value => $"Processed: {value:0}%")
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
IProgressBarControl ChangeDescriptionAsync(Func<double, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription). The task is awaited synchronously
each time the description refreshes — keep it fast.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .ChangeDescriptionAsync(async value =>
    {
        await Task.Delay(10);
        return $"Processed (async): {value:0}%";
    })
    .UpdateHandler(Work)
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `Culture`

```csharp
IProgressBarControl Culture(CultureInfo culture)
IProgressBarControl Culture(string cultureName)
```

Sets the culture used to format numeric values. Pass a `CultureInfo` or a culture name such as
`"pt-BR"`.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ", "Culture: pt-BR")
    .Culture("pt-BR")
    .FractionalDigits(2)
    .UpdateHandler(Work)
    .Run();
```

> The string overload throws `ArgumentException` for a null/empty name and `CultureNotFoundException`
> for an unknown one.

---

## Styling & behavior

### `Styles`

```csharp
IProgressBarControl Styles(ProgressBarStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region list and
examples on the [Styles](styles.md) page.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Styles(ProgressBarStyles.Slider, new Style(Color.Green, Color.Default))
    .UpdateHandler(Work)
    .Run();
```

---

### `Options`

```csharp
IProgressBarControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, tooltip, hide-after-finish, and the abort key.

```csharp
PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .Options(o => o
        .ShowTooltip(true)
        .HideAfterFinish(true))
    .UpdateHandler(Work)
    .Run();
```

See [Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list.

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<StateProgress> Run(CancellationToken token = default)
```

Renders the bar and blocks until the update handler returns (work finished) or `token` is cancelled.
Returns a [`ResultPrompt<StateProgress>`](../../architecture.md#resultpromptt) — see
[Index → Return value](index.md#return-value).

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var result = PromptPlus.Controls.ProgressBar("Work").UpdateHandler(Work).Run(cts.Token);
```

---

## See also

- [Operations](operations.md) — the `ProgressBarEvent`, context, cancellation, and errors
- [Styles](styles.md) — the `ProgressBarStyles` regions
- [Index](index.md) — overview and method map
