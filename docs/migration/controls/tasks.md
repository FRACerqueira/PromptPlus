# Migration v5.x → v6.x: MultiTasks, Timer and Task

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## Renamed controls

| v5.x | v6.x |
|---|---|
| `WaitProcess()` | `MultiTasks()` |
| `WaitTimer()` | `Time()` |
| `WaitCommand()` | `Task()` |

---

## WaitProcess → MultiTasks

### 1. `AddTask` — full rework

The `AddTask` signature was completely redesigned: `ExtraInfoProcess` is gone and a context dictionary replaces the loose parameter.

**Before (v5.x):**
```csharp
using PromptPlusLibrary;

PromptPlus.Controls.WaitProcess("Processing:")
    .AddTask(
        TaskMode.Sequential,
        id: "import",
        process: (param, extra, ct) =>
        {
            extra.Update("Importing records...");
            ImportData((string)param!, ct);
        },
        label: "Import data",
        parameter: "file.csv")
    .Run();
```

**After (v6.x):**
```csharp
using PromptPlusLibrary;

PromptPlus.Controls.MultiTasks("Processing:")
    .AddTask(
        title: "Import data",
        handler: (ctx, ct) =>
        {
            var file = ctx["file"]?.ToString();
            ImportData(file!, ct);
            return null;   // optional output context
        },
        context: new Dictionary<string, object?> { ["file"] = "file.csv" },
        mode: MultiTasksMode.Sequential)
    .Run();
```

There is also a simplified overload and an async form:
```csharp
.AddTask("Quick step", ct => DoWork(ct), MultiTasksMode.Parallel)
.AddTaskAsync("Async step", async ct => await DoWorkAsync(ct))
```

### 2. `MaxDegreeProcess(byte)` → `MaxDegreeOfParallelism(int)`

```csharp
// v5.x
.MaxDegreeProcess(4)

// v6.x
.MaxDegreeOfParallelism(4)
```

### 3. `TaskMode` → `MultiTasksMode`

```csharp
// v5.x: .AddTask(TaskMode.Sequential, ...)
// v6.x: .AddTask(..., mode: MultiTasksMode.Sequential)
```

### 4. Return type: `StateProcess[]` → `StateMultiTasks`

**Before (v5.x):**
```csharp
ResultPrompt<StateProcess[]> result = PromptPlus.Controls.WaitProcess(":")
    .AddTask(/* ... */)
    .Run();

foreach (StateProcess state in result.Content ?? [])
    Console.WriteLine($"{state.Id}: {state.Status}");
```

**After (v6.x):**
```csharp
ResultPrompt<StateMultiTasks> result = PromptPlus.Controls.MultiTasks(":")
    .AddTask(/* ... */)
    .Run();

// StateMultiTasks aggregates the run
Console.WriteLine($"All succeeded: {result.Content.AllSucceeded}");
Console.WriteLine($"Any failed:    {result.Content.AnyFailed}");
foreach (var r in result.Content.Results) { /* per-task result */ }
```

> `StateMultiTasks` members: `ElapsedTime`, `Results` (`IReadOnlyList<MultiTaskResult>`), `Aborted`, `AllSucceeded`, `AnyFailed`.

### 5. Removed: `Finish(Func<...>)`, `ChangeDescription(Func<...>)`, `IntervalUpdate(int)`

```csharp
// v5.x only — dynamic finish/description and update interval are gone
.Finish(states => $"...")
.ChangeDescription(states => $"...")
.IntervalUpdate(200)
```

Use `StopOnError(bool)` and per-task titles to communicate state:
```csharp
PromptPlus.Controls.MultiTasks("Processing:")
    .AddTask("Task 1", ct => { /* ... */ })
    .StopOnError(true)
    .Run();
```

---

## WaitTimer → Time

### 1. Duration moved from the factory to a fluent method

In v5.x the duration was a **constructor argument** (`WaitTimer(int ms, ...)` or `WaitTimer(TimeSpan, ...)`). In v6.x it is a fluent `Duration(...)` call.

**Before (v5.x):**
```csharp
PromptPlus.Controls.WaitTimer(TimeSpan.FromSeconds(30), "Waiting:")
    .IsCountDown(true)
    .Run();
```

**After (v6.x):**
```csharp
PromptPlus.Controls.Timer("Waiting:")
    .Duration(TimeSpan.FromSeconds(30))          // or Duration(30)
    .DisplayMode(TimerDisplayMode.Countdown)      // enum: Countdown | Elapsed
    .Run();
```

### 2. `IsCountDown(bool)` → `DisplayMode(TimerDisplayMode)`

```csharp
// v5.x
.IsCountDown(true)

// v6.x — note the enum value is "Countdown" (lower-case d)
.DisplayMode(TimerDisplayMode.Countdown)
```

### 3. `ShowElapsedTime(int, bool)` — removed

```csharp
// v5.x only — time is displayed automatically; customize with Format(...)
.ShowElapsedTime(500, true)
```
```csharp
// v6.x
PromptPlus.Controls.Timer("Waiting:")
    .Duration(30)
    .Format(@"mm\:ss")
    .Run();
```

### 4. Return type: `TimeSpan?` → `TimeSpan`

```csharp
// v5.x — Run() → ResultPrompt<TimeSpan?>
// v6.x — Run() → ResultPrompt<TimeSpan>
TimeSpan elapsed = PromptPlus.Controls.Timer("...").Duration(30).Run().Content;
```

### 5. New in `Timer`
```csharp
PromptPlus.Controls.Timer("Processing:")
    .Duration(TimeSpan.FromMinutes(2))
    .DisplayMode(TimerDisplayMode.Countdown)
    .Format(@"mm\:ss")
    .Culture(new CultureInfo("pt-BR"))
    .Finish("Done!")
    .ChangeDescription(remaining => $"Time left: {remaining:mm\\:ss}")
    .Spinner(SpinnersType.Dots)
    .Run();
```

---

## WaitCommand → Task

### 1. `CommandHandler(Action)` → `Action(Action<CancellationToken>)`

**Before (v5.x):**
```csharp
PromptPlus.Controls.WaitCommand("Processing:")
    .CommandHandler(() => RunTask())
    .Run();
```

**After (v6.x):**
```csharp
PromptPlus.Controls.Task("Processing:")
    .Action(ct => RunTask(ct))
    .Run();
```

### 2. `ShowElapsedTime(int, bool)` → `ShowElapsedTime(bool, string?)`

```csharp
// v5.x
.ShowElapsedTime(500, true)

// v6.x — interval removed, optional display format added
.ShowElapsedTime(true, @"mm\:ss")
```

### 3. `Finish(string)` → `Finish(string, string? errortext)`

```csharp
// v5.x
.Finish("Done!")

// v6.x — optional error text
.Finish("Done!", "Execution failed")
```

### 4. Return type: `Exception?` → `StateTask`

**Before (v5.x):**
```csharp
var result = PromptPlus.Controls.WaitCommand(":")
    .CommandHandler(() => Run())
    .Run();

if (result.Content is not null)                 // Content was the Exception?
    Console.WriteLine($"Error: {result.Content.Message}");
```

**After (v6.x):**
```csharp
var result = PromptPlus.Controls.Task(":")
    .Action(ct => Run(ct))
    .Run();

if (result.Content.Exception is not null)       // StateTask.Exception
    Console.WriteLine($"Error: {result.Content.Exception.Message}");
```

> `StateTask` members: `ElapsedTime`, `OutputContext`, `Exception`, and `GetOutput<T>(key, out found)`. There is **no** `IsFaulted` — check `Exception is not null`.

### 5. New in `Task`
```csharp
var context = new Dictionary<string, object?> { ["data"] = payload };

PromptPlus.Controls.Task("Processing:")
    .Context(context)
    .Culture(new CultureInfo("pt-BR"))
    .ActionAsync(async (ctx, ct) =>
    {
        await ProcessAsync(ctx["data"], ct);
        return new Dictionary<string, object?> { ["result"] = "ok" };
    })
    .ChangeDescriptionAsync(async elapsed => $"Running for {elapsed:mm\\:ss} — {await StatusAsync()}")
    .Finish("Finished!", "An error occurred")
    .Run();
```

---

## Full API reference

### WaitProcess → MultiTasks

| Method | v5.x | v6.x | Change |
|---|---|---|---|
| Factory | `WaitProcess()` | `MultiTasks()` | Renamed |
| `AddTask(TaskMode, id, Action<obj,ExtraInfoProcess,CT>, label, param)` | ✅ | ❌ | Reworked |
| `AddTask(title, Func<IReadOnlyDict,CT,IDict?>, context, MultiTasksMode?)` | ❌ | ✅ | New form |
| `AddTask(title, Action<CT>, MultiTasksMode?)` | ❌ | ✅ | New overload |
| `AddTaskAsync(...)` (x2) | ❌ | ✅ | New |
| `MaxDegreeProcess(byte)` | ✅ | ❌ | → `MaxDegreeOfParallelism(int)` |
| `Finish(Func<...>)` · `ChangeDescription(Func<...>)` · `IntervalUpdate(int)` | ✅ | ❌ | Removed |
| `ShowElapsedTime(bool)` | ✅ | ✅ | v6.x adds optional `string? format` |
| `Spinner(SpinnersType)` | ✅ | ✅ | Unchanged |
| `StopOnError(bool)` · `Mode(MultiTasksMode)` · `Interaction<T>` · `Culture` · `PageSize(byte)` | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<StateProcess[]>` | `ResultPrompt<StateMultiTasks>` | Return type changed |

### WaitTimer → Time

| Method | v5.x | v6.x | Change |
|---|---|---|---|
| Factory | `WaitTimer(int/TimeSpan, ...)` | `Time()` | Renamed; duration moved to `Duration(...)` |
| `IsCountDown(bool)` | ✅ | ❌ | → `DisplayMode(TimerDisplayMode)` |
| `ShowElapsedTime(int, bool)` | ✅ | ❌ | Removed |
| `Finish(string)` · `Spinner(SpinnersType)` | ✅ | ✅ | Unchanged |
| `Duration` · `Format` · `Culture` · `DisplayMode` · `ChangeDescription` · `ChangeDescriptionAsync` | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<TimeSpan?>` | `ResultPrompt<TimeSpan>` | No longer nullable |

### WaitCommand → Task

| Method | v5.x | v6.x | Change |
|---|---|---|---|
| Factory | `WaitCommand()` | `Task()` | Renamed |
| `CommandHandler(Action)` | ✅ | ❌ | → `Action(Action<CT>)` |
| `ShowElapsedTime(int, bool)` | ✅ | ❌ | → `ShowElapsedTime(bool, string?)` |
| `Finish(string)` | ✅ | ❌ | → `Finish(string, string?)` |
| `Spinner(SpinnersType)` | ✅ | ✅ | Unchanged |
| `Action` (x3) · `ActionAsync` (x3) · `Context` · `Culture` · `ChangeDescription` · `ChangeDescriptionAsync` | ❌ | ✅ | New |
| `Run()` | `ResultPrompt<Exception?>` | `ResultPrompt<StateTask>` | Return type changed |
