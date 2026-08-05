<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Task — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Task — Styles →](styles.md)

---

How the `Task` control behaves while it runs: the action overloads, the isolated contexts, elapsed
time, cancellation, and error handling.

---

## Anatomy of the control

```
Processing  ⠹  00:00:03                   ← prompt + spinner + elapsed time
Running for 3 second(s)...                 ← ChangeDescription output (optional)
Saved!                                     ← Finish text (on completion)
Esc: cancel                               ← tooltip
```

The spinner appears with [`Spinner(...)`](methods.md#spinner); the elapsed time with
[`ShowElapsedTime()`](methods.md#showelapsedtime); the description with
[`ChangeDescription`](methods.md#changedescription). There are no editing keys — the only user action
is **Esc** to abort (when enabled). Each region can be recolored — see [Styles](styles.md).

---

## Choosing an action overload

Attach exactly one work delegate. There are six overloads across `Action` (sync) and `ActionAsync`
(async), differing only in the data they carry:

| Data shape | Sync | Async |
|---|---|---|
| No context | `Action(Action<CancellationToken>)` | `ActionAsync(Func<CancellationToken, Task>)` |
| Output only | `Action(Func<CancellationToken, IDictionary?>)` | `ActionAsync(Func<CancellationToken, Task<IDictionary?>>)` |
| Input + output | `Action(Func<IReadOnlyDictionary, CancellationToken, IDictionary?>)` | `ActionAsync(Func<IReadOnlyDictionary, CancellationToken, Task<IDictionary?>>)` |

Prefer `ActionAsync` for I/O-bound work so the run loop stays responsive.

---

## Isolated input & output contexts

The task works with two independent dictionaries:

- **Input context** — supplied via [`Context(...)`](methods.md#context) and passed to the two-argument
  overloads as an `IReadOnlyDictionary`.
- **Output context** — whatever your delegate *returns* (an `IDictionary<string, object?>?`, or
  `null`). It surfaces afterwards on `StateTask.OutputContext` and via `GetOutput<T>`.

They are isolated from each other: writing the output never mutates the input.

```csharp
var context = new Dictionary<string, object?> { ["name"] = "PromptPlus", ["count"] = 10 };

var result = PromptPlus.Controls.Task("Computing")
    .Context(context)
    .ActionAsync(async (input, token) =>
    {
        await Task.Delay(1500, token).ConfigureAwait(false);
        int count = input.TryGetValue("count", out var raw) && raw is int c ? c : 0;
        return new Dictionary<string, object?>
        {
            ["result"] = count * 2,
            ["message"] = $"Processed {input["name"]}"
        };
    })
    .Run();

int doubled = result.Content.GetOutput<int>("result", out bool found);       // 20, true
string message = result.Content.GetOutput<string>("message", out _) ?? "";
```

`GetOutput<T>` returns `default` and sets `found = false` when the key is missing or the stored value
is not a `T`.

---

## Elapsed time

`StateTask.ElapsedTime` always reports how long the action ran, whether or not you displayed it. Call
[`ShowElapsedTime(true, format)`](methods.md#showelapsedtime) to render it live; the format defaults
to `hh\:mm\:ss` and honors [`Culture(...)`](methods.md#culture).

---

## Cancellation

Pass a `CancellationToken` to [`Run(token)`](methods.md#run) to enforce a deadline, or let the user
press **Esc**. Either way, the token your delegate receives is signalled; forward it to your awaits so
the work unwinds:

```csharp
using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
{
    var result = PromptPlus.Controls.Task("Long task", "Runs with a CancellationToken")
        .ShowElapsedTime()
        .ActionAsync(async token => await Task.Delay(TimeSpan.FromSeconds(10), token).ConfigureAwait(false))
        .Run(sw.Token);
    // result.IsAborted == true
}
```

---

## Errors

If the action throws, the control **captures** the exception rather than letting it propagate. The
run ends, the [`Finish`](methods.md#finish) error text (or a default localized message) is shown, and
the exception lands on `StateTask.Exception`:

```csharp
var result = PromptPlus.Controls.Task("Fetching data")
    .Finish("Fetched!", "Fetch failed!")
    .ActionAsync(async token =>
    {
        await Task.Delay(1500, token).ConfigureAwait(false);
        throw new TimeoutException("Remote server did not respond");
    })
    .Run();

if (result.Content.Exception is not null)
    PromptPlus.Console.WriteLine($"{result.Content.Exception.GetType().Name}: {result.Content.Exception.Message}");
```

There is no separate status flag, but `Exception is null` does **not** by itself mean success:
forwarding the token to your awaits (as recommended above) means a cancelled run throws
`OperationCanceledException` inside the handler, which is caught silently — `Exception` stays `null`
but `IsAborted` is `true`. Always check `IsAborted` first, and only then treat `Exception is null` as
success. When an action throws before returning an output context, `OutputContext` is an **empty**
dictionary, not `null`.

---

## Spinners

[`Spinner(SpinnersType)`](methods.md#spinner) shows a looping animation while the task runs.
`SpinnersType` spans several families — braille/dots, lines/bars, shapes, toggles, arrows/motion, and
emoji. On a terminal without Unicode support it **automatically falls back to the `Ascii` spinner**
(`- \ | /`).

See [Spinners](../../spinners.md) for usage and the fallback rules, and the
[Spinner catalog](../../spinners-catalog.md) for every spinner's frames.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Task` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the task can only end by finishing/failing |
| `HideAfterFinish(true)` | Erases the control after completion |
| `ShowTooltip(false)` | Hides the keyboard-hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

---

## Edge cases & gotchas

- **Attach exactly one action.** Setting several replaces the previous one; only the last is run.
- **`Run` has no work parameter.** The delegate you passed to `Action`/`ActionAsync` is what runs.
- **The description callback receives elapsed time**, not progress — `Task` is unmeasured. For a
  numeric value, use [ProgressBar](../progressbar/index.md).
- **Async description refresh is awaited synchronously** — keep
  [`ChangeDescriptionAsync`](methods.md#changedescriptionasync) fast.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Styles](styles.md) — the `TaskStyles` regions
- [MultiTasks](../multitasks/index.md) — run many of these at once
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
