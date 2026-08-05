<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Task**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Task — Methods →](methods.md)

---

> Runs one synchronous or asynchronous operation and shows that it is working — an animated spinner,
> the elapsed time, and a completion (or error) message — until it finishes.

The `Task` control is for a single unit of background work whose progress you *cannot* measure: a
save, a download, an API call. You attach the work with an `Action(...)`/`ActionAsync(...)` method,
`Run()` displays the spinner and elapsed time, and the control returns a [`StateTask`](#return-value)
carrying the elapsed time, any output the work produced, and any exception it threw.

> 📊 If you *can* measure progress as a number, use the [**ProgressBar**](../progressbar/index.md)
> control instead. To run **several** operations (sequentially or in parallel) with a per-task status
> list, use [**MultiTasks**](../multitasks/index.md). For a fixed-length wait, use
> [**Timer**](../timer/index.md).

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Action overloads, contexts, elapsed time, cancellation, errors |
| [Styles](styles.md) | The `TaskStyles` regions and how to recolor them |

---

## When to use it

| Use `Task` when… | Consider instead… |
|---|---|
| You run one operation you can't measure | — |
| You can report a numeric progress value | [ProgressBar](../progressbar/index.md) |
| You run many operations at once | [MultiTasks](../multitasks/index.md) |
| You just need to wait a fixed duration | [Timer](../timer/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;
using System.Threading;

var result = PromptPlus.Controls
    .Task("Processing")
    .Action(token => Thread.Sleep(2000))   // the work
    .Run();

if (!result.IsAborted)
    PromptPlus.Console.WriteLine($"Done in {result.Content.ElapsedTime}");
```

- `Task("Processing")` creates the control. The first argument is the **prompt**; an optional second
  argument is a **description** line.
- `Action(...)` attaches the work. The work runs when you call `Run()` — there is **no work delegate
  on `Run`**; you set it here.
- `.Run()` shows the control and blocks until the work returns (or throws, or is cancelled).
- The call returns a [`ResultPrompt<StateTask>`](../../architecture.md#resultpromptt): read `.Content`
  for the [`StateTask`](#return-value) and `.IsAborted` for cancellation.

> 💡 Add [`ShowElapsedTime()`](methods.md#showelapsedtime) and [`Spinner(...)`](methods.md#spinner)
> to give the user visible feedback that something is happening.

---

## A more complete example

```csharp
using PromptPlusLibrary;
using System.Threading;
using System.Threading.Tasks;

var context = new Dictionary<string, object?> { ["name"] = "PromptPlus", ["count"] = 10 };

var result = PromptPlus.Controls
    .Task("Computing")
    .ShowElapsedTime()
    .Spinner(SpinnersType.Dots)
    .Context(context)                                  // isolated input context
    .Finish("Computed!", "Computation failed!")        // success / error text
    .ActionAsync(async (input, token) =>
    {
        await Task.Delay(1500, token).ConfigureAwait(false);
        int count = input.TryGetValue("count", out var raw) && raw is int c ? c : 0;
        return new Dictionary<string, object?> { ["result"] = count * 2 };   // output context
    })
    .Run();

int doubled = result.Content.GetOutput<int>("result", out bool found);
```

This shows the most useful pieces together: **elapsed time**, a **spinner**, an **input context** the
work reads, an **output context** it returns, and **finish text**. See [Operations](operations.md) for
how contexts and errors flow.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Attach the work | `Action`, `ActionAsync` |
| Data in/out | `Context` |
| Feedback | `ShowElapsedTime`, `Spinner`, `Finish`, `ChangeDescription`, `ChangeDescriptionAsync` |
| Formatting | `Culture` |
| Styling & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`Task` returns `ResultPrompt<StateTask>`.

| Member | Meaning |
|---|---|
| `.IsAborted` | `true` when the run was cancelled (token / Esc) |
| `.Content` | The final [`StateTask`](#statetask-members) struct |

### `StateTask` members

| Member | Meaning |
|---|---|
| `ElapsedTime` | How long the work ran (`TimeSpan`) |
| `Exception` | The exception the work threw, if any (the control captures it — it does not propagate) |
| `OutputContext` | The output dictionary the work returned (`IReadOnlyDictionary<string, object?>?`) |
| `GetOutput<T>(key, out found)` | Typed read of an `OutputContext` entry |

> There is **no `.Status`** member, and `Exception is null` alone is **not** enough to conclude
> success — check `.IsAborted` first. A run cancelled via the token (forwarded to your awaits, as
> recommended) throws `OperationCanceledException` inside the handler, which is caught silently:
> `Exception` stays `null` but `IsAborted` is `true`. See [Operations → Errors](operations.md#errors).

```csharp
var result = PromptPlus.Controls.Task("Save").Action(Save).Run();
PromptPlus.Console.WriteLine($"HasError={result.Content.Exception is not null}, Elapsed={result.Content.ElapsedTime}");
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — action overloads, contexts, elapsed time, cancellation, errors
- [Styles](styles.md) — recolor the prompt, spinner, elapsed-time, and error regions
- [ProgressBar](../progressbar/index.md) — measurable progress
- [MultiTasks](../multitasks/index.md) — many operations at once
- [Timer](../timer/index.md) — a fixed-duration countdown
