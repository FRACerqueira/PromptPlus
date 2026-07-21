<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Slider**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Slider — Methods →](methods.md)

---

> Pick a numeric value by moving a bar between a minimum and a maximum. The user nudges the
> value with the arrow keys and confirms with **Enter**.

The `Slider` control is the natural way to collect a *bounded* number — volume, brightness, a
timeout in seconds, a priority, a percentage. Instead of typing digits, the user slides a visual
bar between the limits you define. It supports custom ranges, decimal precision, small and large
steps, per-value coloring, gradients, a vertical layout, and persistent history — all through a
single fluent chain.

> 📊 Just need to **display** a value without asking for input? Use the read-only
> [**Slider widget**](../../widgets.md#slider) instead — it renders the same bar but does not
> wait for the user.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Keyboard, stepping, ranges, gradients, history, and edge cases |
| [Styles](styles.md) | The `SliderStyles` regions and how to recolor them |

---

## When to use it

| Use `Slider` when… | Consider instead… |
|---|---|
| You need a bounded numeric value chosen by feel | — |
| The user should pick from a short, discrete list | [Select](../select/index.md) |
| You need free numeric entry, currency, or a fixed pattern | [MaskEdit family](../maskedit/index.md) |
| You only need to display a value, not collect one | [Slider widget](../../widgets.md#slider) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Slider("Select value")
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"Value: {result.Content}");
```

- `Slider("Select value")` creates the control. The first argument is the **prompt**; an optional
  second argument is a **description** line shown under it.
- With no configuration the slider spans the default range **0..100**, uses whole numbers, draws a
  30-character bar, and starts at `0`.
- `.Run()` renders the bar and blocks until the user presses **Enter** (confirm) or **Esc** (abort).
- The call returns a [`ResultPrompt<double?>`](../../architecture.md#resultpromptt): read `.Content`
  for the value and `.IsAborted` to detect Esc.

> 💡 The value is nullable. On abort `.Content` is `null` — always check `IsAborted` first.

---

## A more complete example

```csharp
using PromptPlusLibrary;

var temperature = PromptPlus.Controls
    .Slider("Target temperature", "Custom range -50..50")
    .Range(-50, 50)          // lower / upper limit
    .FractionalDigits(1)          // one decimal place
    .Step(0.5)                // arrow keys move by 0.5
    .LargeStep(5)             // Page Up / Page Down move by 5
    .Default(0)               // start centered
    .Run();

if (!temperature.IsAborted)
    PromptPlus.Console.WriteLine($"Temperature: {temperature.Content}");
```

This combines the four things you most often tune together: a **custom range** (`Range`),
**precision** (`FractionalDigits`), the **small and large step** sizes (`Step`, `LargeStep`), and a
**starting value** (`Default`). See [Operations](operations.md) for how stepping behaves at runtime.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Range & precision | `Range`, `Width`, `FractionalDigits`, `Culture` |
| Starting value | `Default` |
| Step increments | `Step`, `LargeStep` |
| Bar appearance & layout | `Fill`, `ChangeColor`, `ChangeGradient`, `HideElements`, `Layout` |
| Dynamic description | `ChangeDescription`, `ChangeDescriptionAsync` |
| History | `EnabledHistory` |
| Appearance & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Slider` returns `ResultPrompt<double?>`.

| Member | Meaning |
|---|---|
| `.Content` | The confirmed value (`null` if aborted) |
| `.IsAborted` | `true` when the user pressed Esc / the abort key |

```csharp
var (value, aborted) = PromptPlus.Controls.Slider("Value").Run();
if (!aborted) PromptPlus.Console.WriteLine($"{value}");
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — keyboard, stepping, gradients, history
- [Styles](styles.md) — recolor the prompt, bar, range, and error regions
- [Slider widget](../../widgets.md#slider) — the read-only, display-only sibling
- [Switch](../switch/index.md) — the boolean on/off cousin
