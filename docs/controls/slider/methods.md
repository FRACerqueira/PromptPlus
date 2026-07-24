<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Slider — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Slider — Operations →](operations.md)

---

Every fluent method on `ISliderControl`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Slider(string prompt = "", string? description = null)`,
> which returns `ISliderControl`.

**Quick jump:**
[Range](#range) ·
[Width](#width) ·
[FractionalDigits](#fractionalDigits) ·
[Culture](#culture) ·
[Default](#default) ·
[Step](#step) ·
[LargeStep](#largestep) ·
[BarType](#bartype) ·
[Layout](#layout) ·
[ChangeColor](#changecolor) ·
[ChangeGradient](#changegradient) ·
[HideElements](#hideelements) ·
[EnabledHistory](#enabledhistory) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Range & precision

### `Range`

```csharp
ISliderControl Range(double minvalue, double maxvalue)
```

Defines the lower and upper limits the slider can reach. Defaults to `0` and `100`.

| Parameter | Meaning |
|---|---|
| `minvalue` | The smallest value the user can select. |
| `maxvalue` | The largest value the user can select. |

```csharp
PromptPlus.Controls.Slider("Score")
    .Range(0, 10)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `minvalue` is greater than or equal to `maxvalue`.

---

### `Width`

```csharp
ISliderControl Width(byte value)
```

Sets the width of the bar, measured in console characters. Default is `30`; the value must be
between `10` and `100`.

```csharp
PromptPlus.Controls.Slider("Value", "Bar drawn with 60 characters")
    .Width(60)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `value` is less than `10` or greater than `100`.

---

### `FractionalDigits`

```csharp
ISliderControl FractionalDigits(byte value)
```

Sets how many decimal places are shown for the value. Default is `0` (whole numbers); the maximum
is `5`.

```csharp
PromptPlus.Controls.Slider("Ratio")
    .Range(0, 1)
    .FractionalDigits(2)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `value` is greater than `5`.

---

### `Culture`

Two overloads control how the number is formatted (decimal separator, digit grouping). Both
default to the current PromptPlus culture.

```csharp
ISliderControl Culture(CultureInfo culture)
ISliderControl Culture(string cultureName)
```

| Overload | Meaning |
|---|---|
| `Culture(CultureInfo)` | Pass a `CultureInfo` directly. Cannot be `null`. |
| `Culture(string)` | Pass a culture name such as `"en-US"` or `"pt-BR"`. Cannot be `null` or empty. |

```csharp
PromptPlus.Controls.Slider("Preço", "Value formatted with pt-BR culture")
    .Culture("pt-BR")            // comma decimal separator
    .Range(0, 10)
    .FractionalDigits(2)
    .Step(0.25)
    .Run();
```

---

## Starting value

### `Default`

```csharp
ISliderControl Default(double value, bool useDefaultHistory = true)
```

Sets the value that is pre-selected when the slider is first shown. Default is `0`.

| Parameter | Meaning |
|---|---|
| `value` | The initial value. Must be inside the range from [`Range`](#range). |
| `useDefaultHistory` | When `true` **and** history is enabled via [`EnabledHistory`](#enabledhistory), the last saved value is used instead of `value`. Default `true`. |

```csharp
PromptPlus.Controls.Slider("Value")
    .Range(-50, 50)
    .Default(0)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `value` is outside the minimum/maximum range.

---

## Step increments

### `Step`

```csharp
ISliderControl Step(double value)
```

Sets the amount added or removed on each **small** change (the arrow keys). Default is **1/100 of
the range** (so `1` for the default 0..100 range).

```csharp
PromptPlus.Controls.Slider("Value")
    .Step(0.5)
    .Run();
```

---

### `LargeStep`

```csharp
ISliderControl LargeStep(double value)
```

Sets the amount added or removed on each **large** change (Page Up / Page Down). Default is
**1/10 of the range** (so `10` for the default 0..100 range).

```csharp
PromptPlus.Controls.Slider("Value")
    .Step(0.5)
    .LargeStep(5)
    .Run();
```

---

## Bar appearance & layout

### `BarType`

```csharp
ISliderControl BarType(SliderBarType type)
```

Selects the character set used to draw the bar. Default is `SliderBarType.Fill`.

| `SliderBarType` | Bar drawn with |
|---|---|
| `Fill` | Solid fill blocks (default) |
| `Light` | Light line characters |
| `DoubleLight` | Double light line characters |
| `Square` | Square characters |
| `Dot` | Dotted characters |

```csharp
PromptPlus.Controls.Slider("Value", "Bar style: Square")
    .BarType(SliderBarType.Square)
    .Run();
```

---

### `Layout`

```csharp
ISliderControl Layout(SliderLayout value)
```

Chooses how the user changes the value and how the control is drawn. Default is
`SliderLayout.LeftRight`.

| `SliderLayout` | Behavior |
|---|---|
| `LeftRight` | Left / Right arrows change the value and the horizontal bar is shown (default) |
| `UpDown` | Up / Down arrows change the value; the bar is hidden and the delimiter/range widgets are not shown |

```csharp
PromptPlus.Controls.Slider("Value")
    .Layout(SliderLayout.UpDown)
    .Run();
```

---

### `ChangeColor`

```csharp
ISliderControl ChangeColor(Func<double, Style> value)
```

Colors the bar dynamically according to the current value — for example red when low and gold when
high. The callback receives the current value and returns the `Style` to apply.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here

PromptPlus.Controls.Slider("Value", "Red <= 30, Blue <= 70, Gold > 70")
    .ChangeColor(value =>
    {
        if (value <= 30) return new Style(Color.Red, Color.Red);
        if (value <= 70) return new Style(Color.Blue, Color.Blue);
        return new Style(Color.Darkgoldenrod, Color.Darkgoldenrod);
    })
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeGradient`

```csharp
ISliderControl ChangeGradient(params Color[] colors)
```

Paints the bar with a gradient that transitions across the supplied colors as the value grows.
Pass two or more colors in order.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;

PromptPlus.Controls.Slider("Value")
    .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
    .Run();
```

> Throws `ArgumentNullException` if `colors` is `null` or empty. Use `ChangeColor` for threshold
> logic and `ChangeGradient` for a smooth blend — pick one approach per control.

---

### `HideElements`

```csharp
ISliderControl HideElements(HideSlider value)
```

Hides one or more decorative elements. By default every element is shown. `HideSlider` is a
`[Flags]` enum — combine values with a bitwise OR.

| `HideSlider` | Effect |
|---|---|
| `None` | Nothing hidden (default) |
| `Delimit` | Hides the delimiters |
| `Range` | Hides the min/max range display |

```csharp
PromptPlus.Controls.Slider("Value")
    .HideElements(HideSlider.Delimit | HideSlider.Range)
    .Run();
```

---

## History

### `EnabledHistory`

```csharp
ISliderControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists the confirmed value to a file under `filename` so it can be reused as the default on the
next run.

| Parameter | Meaning |
|---|---|
| `filename` | A stable, unique key for this slider's history store. Cannot be `null`. |
| `options` | Optional `IHistoryOptions` configuration (expiration, max items, and so on). |

```csharp
PromptPlus.Controls.Slider("Value")
    .Default(0, true)                 // fall back to the last history value
    .FractionalDigits(2)
    .Step(0.5)
    .LargeStep(5)
    .EnabledHistory("slider-history")
    .Run();
```

> 💡 Pair `Default(value, useDefaultHistory: true)` with `EnabledHistory(...)` to pre-load the last
> value the user confirmed. See [Operations → History](operations.md#history) for runtime behavior.

---

## Dynamic description

### `ChangeDescription`

```csharp
ISliderControl ChangeDescription(Func<double, string> value)
```

Recomputes the description line as the value changes. The callback receives the current value and
returns the description to display — handy for live readouts.

```csharp
PromptPlus.Controls.Slider("Value")
    .ChangeDescription(value => $"Current selection: {value:0} %")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
ISliderControl ChangeDescriptionAsync(Func<double, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription), for a description sourced
asynchronously.

```csharp
PromptPlus.Controls.Slider("Value")
    .ChangeDescriptionAsync(async value =>
    {
        await Task.Delay(1).ConfigureAwait(false);
        return $"Async description for value {value:0}";
    })
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

## Appearance & behavior

### `Styles`

```csharp
ISliderControl Styles(SliderStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region list and
examples on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;

PromptPlus.Controls.Slider("Value")
    .Styles(SliderStyles.Prompt, new Style(Color.Aqua, Color.Black))
    .Styles(SliderStyles.Answer, new Style(Color.Green, Color.Black))
    .Styles(SliderStyles.Slider, new Style(Color.Blue, Color.Black))
    .Run();
```

> Throws `ArgumentNullException` if `style` is `null`.

---

### `Options`

```csharp
ISliderControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, abort key, tooltip, hide-after-finish, and the extra-info affixes.

```csharp
PromptPlus.Controls.Slider("Value")
    .Options(opt =>
    {
        opt.Description("Custom options sample");
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
ResultPrompt<double?> Run(CancellationToken token = default)
```

Renders the slider and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns a
[`ResultPrompt<double?>`](../../architecture.md#resultpromptt) whose `.Content` is `null` when the
prompt is cancelled.

| Parameter | Meaning |
|---|---|
| `token` | A `CancellationToken` that cancels the prompt while it waits for input. |

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
var result = PromptPlus.Controls.Slider("Value").Run(cts.Token);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `SliderStyles` regions
- [Index](index.md) — overview and method map
