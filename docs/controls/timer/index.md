<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Timer**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Timer — Methods →](methods.md)

---

> A countdown timer. It suspends execution for a fixed duration while showing a live count — down to
> zero (default) or up from zero — and returns the elapsed time.

The `Timer` control is **not** a time picker. It is a visible, cancellable wait: a "starting in 3…"
delay, a cool-down between retries, a scripted pause in a demo. You set a required
[`Duration`](#minimal-example), `Run()` renders the countdown, and the call returns the elapsed
`TimeSpan` when it completes or is cancelled.

> ⏱ Need to *do* work during the wait? Use [**Task**](../task/index.md) (one operation) or
> [**MultiTasks**](../multitasks/index.md) (several). Need a *measurable* progress value instead of a
> clock? Use [**ProgressBar**](../progressbar/index.md).

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Duration, display mode, format, cancellation, and edge cases |
| [Styles](styles.md) | The `TimerStyles` regions and how to recolor them |

---

## When to use it

| Use `Timer` when… | Consider instead… |
|---|---|
| You need a visible, fixed-length wait/countdown | — |
| You run one operation while showing progress | [Task](../task/index.md) |
| You run several operations at once | [MultiTasks](../multitasks/index.md) |
| You can report a measurable numeric value | [ProgressBar](../progressbar/index.md) |
| You want the user to *enter* a time value | [MaskTimeOnly](../../index.md#control-pages) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Timer("Starting in")
    .Duration(3)                 // required: 3 seconds, must be > 0
    .Run();

PromptPlus.Console.WriteLine($"Waited {result.Content}");
```

- `Timer("Starting in")` creates the control. The first argument is the **prompt**; an optional second
  argument is a **description** line.
- `Duration(3)` is **required** — set it with a whole number of seconds or a `TimeSpan`. It must be
  greater than zero.
- `.Run()` renders the live countdown and blocks for the duration (or until Esc / the token cancels).
- The call returns a [`ResultPrompt<TimeSpan>`](../../architecture.md#resultpromptt): `.Content` is the
  **elapsed** `TimeSpan` and `.IsAborted` tells you whether it was cancelled early.

> 💡 The result is always the *elapsed* time, regardless of whether you display a countdown or a
> count-up (see [`DisplayMode`](methods.md#displaymode)).

---

## A more complete example

```csharp
using ConsolePlusLibrary;   // Color, Style
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Timer("Please wait")
    .Duration(TimeSpan.FromSeconds(10))
    .Format(@"mm\:ss\:fff")                         // minutes:seconds:millis
    .DisplayMode(TimerDisplayMode.Countdown)         // count down (default)
    .Finish("Done!")                                // text shown at the end
    .ChangeDescription(remaining => $"Remaining: {remaining.TotalSeconds:0} second(s)")
    .Styles(TimerStyles.Answer, new Style(Color.Green, Color.Black))
    .Run();
```

This combines a `TimeSpan` **duration**, a custom **format**, an explicit **display mode**, a
**finish** message, a **live description**, and a **styled** answer. See
[Operations](operations.md) for how the modes and format interact.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Length (required) | `Duration` |
| Display | `DisplayMode`, `Format`, `Spinner`, `Finish` |
| Text & culture | `ChangeDescription`, `ChangeDescriptionAsync`, `Culture` |
| Styling & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Timer` returns `ResultPrompt<TimeSpan>`.

| Member | Meaning |
|---|---|
| `.Content` | The **elapsed** time (`TimeSpan`) — the full duration on normal completion, or less on abort |
| `.IsAborted` | `true` when the countdown was cancelled (Esc / token) before completing |

```csharp
var result = PromptPlus.Controls.Timer("Starting in").Duration(3).Run();
PromptPlus.Console.WriteLine($"IsAborted: {result.IsAborted}, Elapsed: {result.Content}");
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — duration, display mode, format, cancellation
- [Styles](styles.md) — recolor the prompt, answer, and spinner regions
- [Task](../task/index.md) — run one operation while showing feedback
- [MultiTasks](../multitasks/index.md) — run several operations at once
- [ProgressBar](../progressbar/index.md) — measurable progress
