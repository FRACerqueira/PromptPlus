<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTasks — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiTasks — Styles →](styles.md)

---

How the `MultiTasks` control behaves while it runs: execution modes and ordering, per-task contexts,
StopOnError, parallelism, pagination, cancellation, and reading results.

---

## Anatomy of the control

```
Running setup steps                       ← prompt (spinner only if you called .Spinner(...))
1 ok, 0 failed, 2 wait                    ← always-rendered counts summary
  ● Load configuration     00:00:01        ← Success row (● / v ASCII) + elapsed time
  ◐ Connect to database    00:00:00        ← Running row (◐ / > ASCII)
  ○ Warm up cache                          ← Waiting row (○ / space ASCII); ✗ / x for Failed
Qty:3 items. 1 of 1 pages.                ← pagination — rendered even for a single page
Esc: cancel                               ← tooltip
```

Each task row is painted per state (`Waiting`, `Running`, `Success`, `Failed`) and can be recolored —
see [Styles](styles.md). The list scrolls when there are more tasks than [`PageSize`](methods.md#pagesize)
rows, but the page indicator itself renders for any non-empty list, single page or not.

> ⚠️ **No spinner renders unless you explicitly call [`Spinner(...)`](methods.md#spinner).** It is
> opt-in, not automatic just because a task is running.

---

## Execution modes & ordering

The default mode comes from [`Mode(...)`](methods.md#mode) (`Sequential` unless changed); each task
may override it with a per-task `mode` argument. **The list order is always preserved** — tasks are
never globally reordered or grouped by mode. The run walks the list top to bottom:

- A `Sequential` task runs alone; the run waits for it before moving on.
- **Consecutive** `Parallel` tasks form a sub-set that runs concurrently; the run advances only once
  every task in that sub-set has finished.

```csharp
PromptPlus.Controls.MultiTasks("Mixed pipeline", "Order preserved; parallel block runs together")
    .Mode(MultiTasksMode.Sequential)                 // default
    .AddTaskAsync("Prepare", async t => await Task.Delay(1000, t))                                  // 1) sequential
    .AddTaskAsync("Download A", async t => await Task.Delay(1800, t), mode: MultiTasksMode.Parallel) // 2)┐
    .AddTaskAsync("Download B", async t => await Task.Delay(1200, t), mode: MultiTasksMode.Parallel) // 3)├ run together
    .AddTaskAsync("Download C", async t => await Task.Delay(2200, t), mode: MultiTasksMode.Parallel) // 4)┘
    .AddTaskAsync("Finalize", async t => await Task.Delay(1000, t))                                  // 5) after the block
    .Run();
```

---

## Parallelism

In parallel mode, [`MaxDegreeOfParallelism(n)`](methods.md#maxdegreeofparallelism) caps concurrency;
`0` auto-selects from the CPU core count. The value is clamped to a sensible range, so you can request
more than the machine can usefully run and it will be reduced.

```csharp
PromptPlus.Controls.MultiTasks("Processing batch", "Use Up/Down and PageUp/PageDown to scroll")
    .Mode(MultiTasksMode.Parallel)
    .MaxDegreeOfParallelism(0)   // auto from CPU cores
    .PageSize(6)
    .Run();
```

---

## StopOnError

[`StopOnError(true)`](methods.md#stoponerror) applies to **sequential** execution: as soon as a task
fails, the remaining ones are skipped (they end in the `Waiting` state). It is ignored in parallel
mode, where all started tasks run to completion regardless.

```csharp
var result = PromptPlus.Controls.MultiTasks("Deploy pipeline")
    .Mode(MultiTasksMode.Sequential)
    .StopOnError()
    .AddTaskAsync("Build", async t => await Task.Delay(1200, t))
    .AddTaskAsync("Test", async t => { await Task.Delay(1000, t); throw new InvalidOperationException("2 tests failed"); })
    .AddTaskAsync("Publish", async t => await Task.Delay(1000, t))
    .Run();
// result.Content.AnyFailed == true; "Publish" never ran.
```

---

## Per-task contexts

The context overloads of [`AddTask`/`AddTaskAsync`](methods.md#addtask) give each task an isolated
input dictionary and let it return an isolated output dictionary. Read outputs afterwards from each
`MultiTaskResult`:

```csharp
var ctxA = new Dictionary<string, object?> { ["factor"] = 3 };
var ctxB = new Dictionary<string, object?> { ["factor"] = 5 };

var result = PromptPlus.Controls.MultiTasks("Computing values")
    .Mode(MultiTasksMode.Parallel)
    .AddTaskAsync("compute A", async (input, token) =>
    {
        await Task.Delay(1200, token);
        int f = input.TryGetValue("factor", out var v) && v is int n ? n : 1;
        return new Dictionary<string, object?> { ["value"] = f * 10 };
    }, ctxA)
    .AddTaskAsync("compute B", async (input, token) =>
    {
        await Task.Delay(1500, token);
        int f = input.TryGetValue("factor", out var v) && v is int n ? n : 1;
        return new Dictionary<string, object?> { ["value"] = f * 10 };
    }, ctxB)
    .Run();

foreach (var r in result.Content.Results)
{
    int value = r.GetOutput<int>("value", out bool found);
    PromptPlus.Console.WriteLine($"{r.Title} => state: {r.State}, value: {(found ? value : -1)}");
}
```

---

## Reading results

`StateMultiTasks` aggregates the run:

- `Results` — one `MultiTaskResult` per task (in the added order), each with `Title`, `State`,
  `ElapsedTime`, `Exception`, `OutputContext`, and `GetOutput<T>`.
- `ElapsedTime` — total wall-clock time of the run.
- `AllSucceeded` — `true` only if there is at least one task and every one is `Success`. In
  practice this is never checked against zero tasks — `Run()` throws `InvalidOperationException`
  immediately if you call it without adding at least one task via `AddTask`/`AddTaskAsync`.
- `AnyFailed` — `true` if any task is `Failed`. A cancelled/aborted run also puts any in-flight
  tasks into `Failed`, so `AnyFailed` can be `true` on an aborted run too — check `Aborted` first if
  you need to distinguish "cancelled" from "a task genuinely threw."
- `Aborted` — `true` if the run was aborted before all tasks finished.

`MultiTaskState` values: `Waiting`, `Running`, `Success`, `Failed`.

---

## Errors

A task that throws ends in the `Failed` state with its exception on `MultiTaskResult.Exception`; the
control captures it rather than propagating. The exception's `.Message` is also rendered inline,
right next to that task's row (styled with `MultiTasksStyles.FailedTask`), not just available
programmatically after `Run()` returns. In parallel mode other tasks continue; in sequential mode
[`StopOnError`](#stoponerror) decides whether the rest run.

```csharp
var result = PromptPlus.Controls.MultiTasks("Health checks")
    .Mode(MultiTasksMode.Parallel)
    .AddTaskAsync("api", async t => await Task.Delay(1500, t))
    .AddTaskAsync("worker", async t => { await Task.Delay(1000, t); throw new TimeoutException("worker not responding"); })
    .AddTaskAsync("storage", async t => await Task.Delay(2000, t))
    .Run();
// result.Content.AnyFailed == true; "api" and "storage" still finish.
```

---

## Cancellation

Pass a `CancellationToken` to [`Run(token)`](methods.md#run), or let the user press **Esc**. The token
is forwarded to each task delegate; forward it to your awaits so in-flight work unwinds. A cancelled
run reports `IsAborted == true` and `StateMultiTasks.Aborted == true`.

```csharp
using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
{
    var result = PromptPlus.Controls.MultiTasks("Long running batch")
        .Mode(MultiTasksMode.Parallel)
        .AddTaskAsync("task-1", async token => await Task.Delay(TimeSpan.FromSeconds(10), token))
        .AddTaskAsync("task-2", async token => await Task.Delay(TimeSpan.FromSeconds(10), token))
        .Run(sw.Token);
}
```

---

## Pagination

The page indicator (`Qty:{n} items. {p} of {n} pages.`) renders for **any non-empty task list**,
even a single page that fits entirely on screen — it isn't conditional on actually needing to
scroll. When the number of tasks exceeds [`PageSize`](methods.md#pagesize) (or the auto-fit height
with `PageSize(0)`), the list becomes scrollable: **Up/Down** move one row, **PageUp/PageDown** move
a page. The `Pagination` style region paints the page indicator.

---

## Spinners

[`Spinner(SpinnersType)`](methods.md#spinner) shows a looping animation in the summary line while any
task is running. `SpinnersType` spans several families — braille/dots, lines/bars, shapes, toggles,
arrows/motion, and emoji. On a terminal without Unicode support it **automatically falls back to the
`Ascii` spinner** (`- \ | /`).

See [Spinners](../../spinners.md) for usage and the fallback rules, and the
[Spinner catalog](../../spinners-catalog.md) for every spinner's frames.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `MultiTasks` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the run can only end by finishing |
| `HideAfterFinish(true)` | Erases the list after completion |
| `ShowTooltip(false)` | Hides the keyboard-hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

---

## Edge cases & gotchas

- **Order is preserved.** Modes group *consecutive* runs; they never reorder the list.
- **`StopOnError` is sequential-only.** In parallel mode, started tasks always finish.
- **`AllSucceeded` needs at least one task.** An empty run reports `false`.
- **`Results` is never null**, even for a `default` `StateMultiTasks`.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Styles](styles.md) — the `MultiTasksStyles` regions
- [Task](../task/index.md) — a single operation with the same context model
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
