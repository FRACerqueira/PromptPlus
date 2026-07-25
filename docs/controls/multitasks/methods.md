<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiTasks — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiTasks — Operations →](operations.md)

---

Every fluent method on `IMultiTasksControl`. Each returns the same control instance, so calls chain in
any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.MultiTasks(string prompt = "", string? description = null)`,
> which returns `IMultiTasksControl`.

**Quick jump:**
[AddTask](#addtask) ·
[AddTaskAsync](#addtaskasync) ·
[Interaction](#interaction) ·
[Mode](#mode) ·
[MaxDegreeOfParallelism](#maxdegreeofparallelism) ·
[StopOnError](#stoponerror) ·
[ShowElapsedTime](#showelapsedtime) ·
[Spinner](#spinner) ·
[PageSize](#pagesize) ·
[Culture](#culture) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Adding tasks

Each task has a **title** and a **work delegate**. Both sync and async come in two shapes: **no
context** and **input + output context**. Every add method also takes an optional per-task
[`mode`](#mode) that overrides the control default.

### `AddTask`

```csharp
IMultiTasksControl AddTask(string title, Action<CancellationToken> handler, MultiTasksMode? mode = null)
IMultiTasksControl AddTask(string title,
    Func<IReadOnlyDictionary<string, object?>, CancellationToken, IDictionary<string, object?>?> handler,
    IDictionary<string, object?>? context = null,
    MultiTasksMode? mode = null)
```

Adds a **synchronous** task.

| Parameter | Meaning |
|---|---|
| `title` | The row title shown in the list. Cannot be `null`. |
| `handler` | The work. The context overload receives an input dictionary and returns an output dictionary (or `null`). Cannot be `null`. |
| `context` | Optional isolated input context for this task. |
| `mode` | Optional per-task execution mode. When `null`, the control default from [`Mode`](#mode) applies. |

```csharp
PromptPlus.Controls.MultiTasks("Setup")
    .AddTask("Clean temp", token => CleanTemp(token))
    .Run();
```

> Throws `ArgumentNullException` if `title` or `handler` is `null`.

---

### `AddTaskAsync`

```csharp
IMultiTasksControl AddTaskAsync(string title, Func<CancellationToken, Task> handler, MultiTasksMode? mode = null)
IMultiTasksControl AddTaskAsync(string title,
    Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler,
    IDictionary<string, object?>? context = null,
    MultiTasksMode? mode = null)
```

Adds an **asynchronous** task — the async mirror of [`AddTask`](#addtask).

```csharp
// Simple form
PromptPlus.Controls.MultiTasks("Downloading")
    .AddTaskAsync("file-1.zip", async token => await Task.Delay(2000, token))
    .Run();

// With input + output context
var ctx = new Dictionary<string, object?> { ["factor"] = 3 };
PromptPlus.Controls.MultiTasks("Computing values")
    .AddTaskAsync("compute A", async (input, token) =>
    {
        await Task.Delay(1200, token);
        int f = input.TryGetValue("factor", out var v) && v is int n ? n : 1;
        return new Dictionary<string, object?> { ["value"] = f * 10 };
    }, ctx)
    .Run();
```

> Throws `ArgumentNullException` if `title` or `handler` is `null`.

---

### `Interaction`

```csharp
IMultiTasksControl Interaction<T>(IEnumerable<T> items, Action<T, IMultiTasksControl> interactionAction)
```

Iterates a collection and lets you register one or more tasks per item — a compact way to build the
list from data.

```csharp
var services = new[] { "auth", "billing", "notifications" };

PromptPlus.Controls.MultiTasks("Bootstrapping services")
    .Mode(MultiTasksMode.Parallel)
    .Interaction(services, (svc, ctrl) =>
        ctrl.AddTaskAsync($"start {svc}", async t => await Task.Delay(800, t), mode: MultiTasksMode.Sequential))
    .Run();
```

> Throws `ArgumentNullException` if `items` or `interactionAction` is `null`.

---

## Execution

### `Mode`

```csharp
IMultiTasksControl Mode(MultiTasksMode mode)
```

Sets the **default** execution mode for tasks that don't specify their own. Default
`MultiTasksMode.Sequential`.

| `MultiTasksMode` | Behavior |
|---|---|
| `Sequential` | Tasks run one after another, in the order added |
| `Parallel` | Tasks run concurrently |

Tasks always keep their added order. Consecutive `Parallel` tasks form a sub-set that runs together;
the run only advances once every item of the current sub-set has finished. See
[Operations → Execution modes](operations.md#execution-modes--ordering).

```csharp
PromptPlus.Controls.MultiTasks("Deploy")
    .Mode(MultiTasksMode.Parallel)
    .AddTaskAsync("api", async t => await Task.Delay(1500, t))
    .Run();
```

---

### `MaxDegreeOfParallelism`

```csharp
IMultiTasksControl MaxDegreeOfParallelism(int value)
```

Caps how many tasks run at once in `Parallel` mode. The value is clamped to a sensible range based on
CPU cores. Use `0` to auto-select from `Environment.ProcessorCount`.

```csharp
PromptPlus.Controls.MultiTasks("Downloading files in parallel")
    .Mode(MultiTasksMode.Parallel)
    .MaxDegreeOfParallelism(2)
    .AddTaskAsync("file-1.zip", async t => await Task.Delay(2000, t))
    .Run();
```

---

### `StopOnError`

```csharp
IMultiTasksControl StopOnError(bool value = true)
```

In **sequential** mode, stops the remaining tasks when one fails. Ignored in parallel mode.

```csharp
PromptPlus.Controls.MultiTasks("Deploy pipeline")
    .Mode(MultiTasksMode.Sequential)
    .StopOnError()
    .AddTaskAsync("Build", async t => await Task.Delay(1200, t))
    .AddTaskAsync("Test", async t => { await Task.Delay(1000, t); throw new InvalidOperationException("2 tests failed"); })
    .AddTaskAsync("Publish", async t => await Task.Delay(1000, t))   // skipped after Test fails
    .Run();
```

---

## Display

### `ShowElapsedTime`

```csharp
IMultiTasksControl ShowElapsedTime(bool value = true, string? format = null)
```

Shows the elapsed time next to each task. **Enabled by default.**

| Parameter | Meaning |
|---|---|
| `value` | `true` to display per-task elapsed time. Default `true`. |
| `format` | Optional `TimeSpan` format string. Default `hh\:mm\:ss`. |

```csharp
PromptPlus.Controls.MultiTasks("Setup")
    .ShowElapsedTime(true, @"mm\:ss")
    .AddTaskAsync("Step", async t => await Task.Delay(1000, t))
    .Run();
```

---

### `Spinner`

```csharp
IMultiTasksControl Spinner(SpinnersType spinnersType)
```

Shows an animated spinner in the summary line while at least one task is running. `SpinnersType` offers
many styles across several families (common ones: `Default`, `Dots`, `Line`, `Star`); on non-Unicode
terminals it automatically falls back to the `Ascii` spinner. See [Spinners](../../spinners.md) and the
[Spinner catalog](../../spinners-catalog.md) for the full list and frames.

```csharp
PromptPlus.Controls.MultiTasks("Setup")
    .Spinner(SpinnersType.Dots)
    .AddTaskAsync("Step", async t => await Task.Delay(1000, t))
    .Run();
```

---

### `PageSize`

```csharp
IMultiTasksControl PageSize(byte value)
```

Sets the maximum number of visible task rows per page. `0` auto-fits to the console height. When there
are more tasks than rows, the list scrolls (Up/Down, PageUp/PageDown).

```csharp
PromptPlus.Controls.MultiTasks("Processing batch")
    .PageSize(6)
    .Run();
```

---

## Formatting

### `Culture`

```csharp
IMultiTasksControl Culture(CultureInfo culture)
```

Sets the culture used to format elapsed-time values.

```csharp
using System.Globalization;

PromptPlus.Controls.MultiTasks("Setup")
    .Culture(new CultureInfo("pt-BR"))
    .Run();
```

> Throws `ArgumentNullException` if `culture` is `null`.

---

## Styling & behavior

### `Styles`

```csharp
IMultiTasksControl Styles(MultiTasksStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. See the full region list and
examples on the [Styles](styles.md) page.

```csharp
using ConsolePlusLibrary;
using PromptPlusLibrary;

PromptPlus.Controls.MultiTasks("Setup")
    .Styles(MultiTasksStyles.SuccessTask, new Style(Color.Green, Color.Default))
    .Styles(MultiTasksStyles.FailedTask,  new Style(Color.Red,   Color.Default))
    .Run();
```

---

### `Options`

```csharp
IMultiTasksControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, tooltip, abort key, and hide-after-finish.

```csharp
PromptPlus.Controls.MultiTasks("Setup")
    .Options(o => o
        .ShowTooltip(false)
        .HideAfterFinish(true))
    .Run();
```

See [Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list.

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<StateMultiTasks> Run(CancellationToken token = default)
```

Displays the list, runs the tasks, and blocks until they all finish or the run is cancelled. Returns a
[`ResultPrompt<StateMultiTasks>`](../../architecture.md#resultpromptt) — see
[Index → Return value](index.md#return-value).

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

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `MultiTasksStyles` regions
- [Index](index.md) — overview and method map
