<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ProgressBar**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ProgressBar — Methods →](methods.md)

---

> A determinate progress indicator driven by your own background work. You report the value; the
> control renders the bar, the percentage, an optional spinner, and the elapsed time in real time.

The `ProgressBar` control is for operations whose progress you can measure — copying files, importing
rows, processing a queue. You supply an **update handler** (sync or async) that loops while the work
runs and pushes the current value into the bar; the control paints the bar, applies color and
gradients, and returns the final [`StateProgress`](#return-value) when the handler signals completion
or the token is cancelled.

> ⏳ Don't have a measurable value? If you only need to show that *something* is happening while a
> single operation runs, use the [**Task**](../task/index.md) control (spinner + elapsed time). For
> several operations at once, use [**MultiTasks**](../multitasks/index.md). For a fixed-length
> wait/countdown, use [**Time**](../time/index.md).

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | The update handler, `ProgressBarEvent`, context, cancellation, errors |
| [Styles](styles.md) | The `ProgressBarStyles` regions and how to recolor them |

---

## When to use it

| Use `ProgressBar` when… | Consider instead… |
|---|---|
| You can measure progress as a number (0→100, 0→N) | — |
| The work is a single unmeasurable operation | [Task](../task/index.md) |
| You run several operations (sequential / parallel) | [MultiTasks](../multitasks/index.md) |
| You just need to wait a fixed duration | [Time](../time/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;
using System.Threading;

var result = PromptPlus.Controls
    .ProgressBar("Wait Progress: ")
    .UpdateHandler((bar, token) =>
    {
        while (!token.IsCancellationRequested && !bar.Finish)
        {
            token.WaitHandle.WaitOne(80);   // simulate work
            bar.Update(bar.Value + 2);      // report new value
        }
    })
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"Done at {result.Content.FinishedValue}");
```

- `ProgressBar("Wait Progress: ")` creates the control. The first argument is the **prompt**; an
  optional second argument is a **description** line shown under it.
- `UpdateHandler(...)` registers the work loop. Inside it you call `bar.Update(value)` to move the
  bar and read `bar.Finish` to know when the maximum was reached.
- `.Run()` renders the bar and blocks until the handler returns (work finished) or the token cancels.
- The call returns a [`ResultPrompt<StateProgress>`](../../architecture.md#resultpromptt): read
  `.Content` for the final [`StateProgress`](#return-value) and `.IsAborted` to detect cancellation.

> 💡 The handler owns the loop. Keep checking both `token.IsCancellationRequested` **and**
> `bar.Finish` so it exits promptly on completion *and* on cancellation.

---

## A more complete example

```csharp
using ConsolePlusLibrary;   // Color, Style
using PromptPlusLibrary;
using System.Threading;

var result = PromptPlus.Controls
    .ProgressBar("Importing", "Rows processed")
    .Range(0, 100)                                   // measurable range
    .FractionalDigits(1)                                 // show one decimal place
    .Spinner(SpinnersType.Dots)                      // animate while running
    .ChangeGradient(Color.Green, Color.Yellow, Color.Red)   // color across the range
    .ChangeDescription(value => $"Processed: {value:0}%")   // live description
    .Finish("Import complete")
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

This combines a **fixed range**, **fractional digits**, a **spinner**, a **gradient fill**, a **live
description**, and a **completion message** — all common building blocks. See
[Operations](operations.md) for how the handler and `ProgressBarEvent` drive the render.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| The work loop | `UpdateHandler`, `UpdateHandlerAsync` |
| Range & value | `Range`, `Default`, `FractionalDigits`, `Width` |
| Appearance | `Fill`, `Spinner`, `ChangeColor`, `ChangeGradient`, `HideElements` |
| Text & culture | `Finish`, `ChangeDescription`, `ChangeDescriptionAsync`, `Culture` |
| Styling & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`ProgressBar` returns `ResultPrompt<StateProgress>`.

| Member | Meaning |
|---|---|
| `.IsAborted` | `true` when the run was cancelled (token) or the handler called `ErrorAndAbort` |
| `.Content` | The final [`StateProgress`](#stateprogress-members) struct |

### `StateProgress` members

| Member | Meaning |
|---|---|
| `FinishedValue` | The final numeric value (`double?`) |
| `FinishedText` | The final display text |
| `MinValue` / `MaxValue` | The configured range bounds |
| `ElapsedTime` | Total time the bar ran (`TimeSpan`) |
| `ExceptionProgress` | The exception passed to `ErrorAndAbort`, if any |
| `OutputContext` | Optional output values the handler produced (`IReadOnlyDictionary<string, object?>?`) |
| `GetOutput<T>(key, out found)` | Typed read of an `OutputContext` entry |

```csharp
var result = PromptPlus.Controls.ProgressBar("Work").UpdateHandler(Work).Run();
PromptPlus.Console.WriteLine($"Aborted={result.IsAborted}, Value={result.Content.FinishedValue}, Elapsed={result.Content.ElapsedTime}");
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — the update handler, `ProgressBarEvent`, context, cancellation
- [Styles](styles.md) — recolor the slider, range, spinner, and error regions
- [Task](../task/index.md) — single unmeasurable operation (spinner + elapsed)
- [MultiTasks](../multitasks/index.md) — many operations, sequential or parallel
- [Time](../time/index.md) — a fixed-duration countdown
