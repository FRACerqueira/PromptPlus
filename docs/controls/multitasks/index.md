<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTasks**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiTasks — Methods →](methods.md)

---

> Runs several operations — sequentially or in parallel — and shows a paginated list with a live
> waiting / running / success / failure status for each one.

The `MultiTasks` control is the multi-item sibling of [`Task`](../task/index.md). You add named tasks
(sync or async), pick an execution mode, and `Run()` renders the list with per-task spinners, elapsed
times, and status icons. It returns a [`StateMultiTasks`](#return-value) with the per-task results and
overall success/failure flags.

> 🧩 Running a **single** operation? Use [**Task**](../task/index.md). Reporting a **measurable**
> numeric value? Use [**ProgressBar**](../progressbar/index.md). Just waiting a fixed duration? Use
> [**Time**](../time/index.md).

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What it is, when to use it, a first working example, the method map |
| [Methods](methods.md) | Every fluent method — signature, parameters, defaults, and a snippet |
| [Operations](operations.md) | Execution modes, ordering, contexts, StopOnError, pagination, cancellation |
| [Styles](styles.md) | The `MultiTasksStyles` regions and how to recolor them |

---

## When to use it

| Use `MultiTasks` when… | Consider instead… |
|---|---|
| You run several operations and want per-item status | — |
| You run exactly one operation | [Task](../task/index.md) |
| You can report a numeric progress value | [ProgressBar](../progressbar/index.md) |
| You just need to wait a fixed duration | [Time](../time/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;
using System.Threading;
using System.Threading.Tasks;

var result = PromptPlus.Controls
    .MultiTasks("Running setup steps")
    .Mode(MultiTasksMode.Sequential)
    .ShowElapsedTime()
    .Spinner(SpinnersType.Dots)
    .AddTaskAsync("Load configuration", async token => await Task.Delay(1200, token))
    .AddTaskAsync("Connect to database", async token => await Task.Delay(1500, token))
    .AddTaskAsync("Warm up cache", async token => await Task.Delay(1000, token))
    .Run();

PromptPlus.Console.WriteLine($"AllSucceeded={result.Content.AllSucceeded}");
```

- `MultiTasks("Running setup steps")` creates the control. The first argument is the **prompt**; an
  optional second argument is a **description** line.
- `AddTask` / `AddTaskAsync` register each task with a title and a work delegate.
- `Mode(...)` sets the default execution mode (`Sequential` by default).
- `.Run()` renders the list and blocks until every task finishes (or the run is cancelled).
- The call returns a [`ResultPrompt<StateMultiTasks>`](../../architecture.md#resultpromptt): read
  `.Content` for the [`StateMultiTasks`](#return-value).

---

## A more complete example

```csharp
using PromptPlusLibrary;
using System.Threading;
using System.Threading.Tasks;

var result = PromptPlus.Controls
    .MultiTasks("Downloading files in parallel")
    .Mode(MultiTasksMode.Parallel)
    .MaxDegreeOfParallelism(2)         // at most 2 at a time
    .ShowElapsedTime()
    .Spinner(SpinnersType.Dots)
    .PageSize(6)                        // scrollable list of 6 rows
    .AddTaskAsync("file-1.zip", async token => await Task.Delay(2000, token))
    .AddTaskAsync("file-2.zip", async token => await Task.Delay(1200, token))
    .AddTaskAsync("file-3.zip", async token => await Task.Delay(2600, token))
    .AddTaskAsync("file-4.zip", async token => await Task.Delay(800, token))
    .Run();

foreach (var r in result.Content.Results)
    PromptPlus.Console.WriteLine($"{r.Title}: {r.State} ({r.ElapsedTime})");
```

This shows parallel execution with a **capped degree of parallelism**, a **paginated** list, and
reading back **per-task results**. See [Operations](operations.md) for mode ordering and error
handling.

---

## Method map

Grouped by purpose. Full signatures and examples are on the [Methods](methods.md) page.

| Purpose | Methods |
|---|---|
| Add tasks | `AddTask`, `AddTaskAsync`, `Interaction` |
| Execution | `Mode`, `MaxDegreeOfParallelism`, `StopOnError` |
| Display | `ShowElapsedTime`, `Spinner`, `PageSize` |
| Formatting | `Culture` |
| Styling & behavior | `Styles`, `Options` |
| Run | `Run` |

---

## Return value

`MultiTasks` returns `ResultPrompt<StateMultiTasks>`.

| Member | Meaning |
|---|---|
| `.IsAborted` | `true` when the run was cancelled (token / Esc) |
| `.Content` | The final [`StateMultiTasks`](#statemultitasks-members) struct |

### `StateMultiTasks` members

| Member | Meaning |
|---|---|
| `ElapsedTime` | Total time of the whole run (`TimeSpan`) |
| `Results` | The per-task results — `IReadOnlyList<MultiTaskResult>` (never `null`) |
| `Aborted` | `true` if the run was aborted before all tasks finished |
| `AllSucceeded` | `true` when there is at least one task and every one succeeded |
| `AnyFailed` | `true` when at least one task failed |

### `MultiTaskResult` members (one per task)

| Member | Meaning |
|---|---|
| `Title` | The task title |
| `State` | The final `MultiTaskState` — `Waiting`, `Running`, `Success`, or `Failed` |
| `ElapsedTime` | How long this task ran |
| `Exception` | The exception it threw, if any |
| `OutputContext` | The output dictionary it returned |
| `GetOutput<T>(key, out found)` | Typed read of an `OutputContext` entry |

```csharp
var s = result.Content;
PromptPlus.Console.WriteLine($"Elapsed={s.ElapsedTime}, AllSucceeded={s.AllSucceeded}, AnyFailed={s.AnyFailed}");
```

---

## See also

- [Methods](methods.md) — the full fluent API
- [Operations](operations.md) — modes, ordering, contexts, StopOnError, pagination, cancellation
- [Styles](styles.md) — recolor the per-state task rows, spinner, and pagination regions
- [Task](../task/index.md) — a single operation
- [ProgressBar](../progressbar/index.md) — measurable progress
- [Time](../time/index.md) — a fixed-duration countdown
