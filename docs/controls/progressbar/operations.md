<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ProgressBar — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ProgressBar — Styles →](styles.md)

---

How the `ProgressBar` control behaves while it runs: the update handler contract, the
`ProgressBarEvent` you drive it with, input/output context, cancellation, and errors.

---

## Anatomy of the control

```
Wait Progress: 42 %                       ← prompt + answer (value)
Processed: 42%                            ← ChangeDescription output (optional)
0 [██████████░░░░░░░░░░░░░░] 100  ⠹       ← range + slider + spinner
00:00:03                                  ← elapsed time
Esc: cancel                              ← tooltip
```

Each element can be hidden ([`HideElements`](methods.md#hideelements)) and recolored
([Styles](styles.md)). Unlike an interactive control, ProgressBar has no editing keys — but it does
still respond to **F1** (cycle tooltip content) and **Ctrl+F1** (show/hide the tooltip), same as
every other control. **Esc** aborts (when the abort key is enabled). Everything else is driven by
your update handler.

---

## The update handler

The heart of the control is a single loop you register with
[`UpdateHandler`](methods.md#updatehandler) or [`UpdateHandlerAsync`](methods.md#updatehandlerasync).
The control renders on its own thread; your handler runs the work and reports progress. A well-formed
loop looks like this:

```csharp
.UpdateHandler((bar, token) =>
{
    while (!token.IsCancellationRequested && !bar.Finish)
    {
        // do a slice of work…
        token.WaitHandle.WaitOne(80);
        // …then report the new value
        bar.Update(bar.Value + 2);
    }
})
```

The two loop conditions matter:

- **`!token.IsCancellationRequested`** — exit when the run is cancelled (Esc or the `Run(token)`
  token). This keeps the UI responsive.
- **`!bar.Finish`** — exit when the value has reached the maximum (or the handler aborted). `Finish`
  flips to `true` automatically once `bar.Value >= bar.Maxvalue`.

When the handler returns, the control closes and returns the final
[`StateProgress`](index.md#stateprogress-members).

> **Final frame is always rendered.** When the loop ends (completion or cancellation), the control
> paints one last frame before closing, so the finished bar, percentage and the
> [`ElapsedTime`](methods.md#hideelements) reflect the *actual* end state — the elapsed value shown
> matches [`StateProgress.ElapsedTime`](index.md#stateprogress-members) and is not frozen a frame
> early. If you set [`HideAfterFinish(true)`](methods.md#options) the frame is erased instead.

---

## The `ProgressBarEvent`

Your handler receives a `ProgressBarEvent` (`bar` above). It is the mutable bridge between your work
and the rendered bar.

| Member | Purpose |
|---|---|
| `Value` | The current value |
| `Minvalue` / `Maxvalue` | The configured range bounds |
| `Update(double value)` | Sets the current value, **clamped** to `[Minvalue, Maxvalue]` |
| `Finish` | `true` when aborted or `Value >= Maxvalue` |
| `HasChange()` | `true` if the value changed (or aborted) since the last check |
| `ErrorAndAbort(Exception?)` | Records an error and aborts the run |
| `Error` | The recorded exception, if any |
| `InputParam<T>(key, out found)` | Reads a value from the input **context** |
| `AddOutputContext<T>(key, value)` | Writes a value to the output context |
| `RemoveOutputContext(key)` | Removes an output-context entry |
| `OutputContext` | The accumulated output context (read-only) |

> 💡 `Update` clamps for you, so `bar.Update(bar.Value + step)` can safely overshoot the maximum on
> the last iteration — the value is capped at `Maxvalue` and `Finish` becomes `true`.

---

## Input & output context

Both handler overloads accept an optional `context` dictionary. Values you put there are readable
inside the loop via `InputParam<T>`; values you write with `AddOutputContext` are exposed afterwards
on `StateProgress.OutputContext` (and via `GetOutput<T>`).

```csharp
var result = PromptPlus.Controls.ProgressBar("Wait Progress: ")
    .UpdateHandler((bar, token) =>
    {
        int step = bar.InputParam<int>("step", out bool hasStep);
        if (!hasStep) step = 1;
        string tag = bar.InputParam<string>("tag", out _);

        while (!token.IsCancellationRequested && !bar.Finish)
        {
            token.WaitHandle.WaitOne(90);
            bar.Update(bar.Value + step);
            bar.AddOutputContext("LastTag", tag);
        }
        bar.AddOutputContext("FinishedAt", DateTimeOffset.UtcNow.ToString("O"));
    },
    new Dictionary<string, object?> { ["step"] = 4, ["tag"] = "context-sample" })
    .Run();

// Read outputs back:
var finishedAt = result.Content.GetOutput<string>("FinishedAt", out bool found);
```

The input and output dictionaries are independent — writing to the output never mutates the input.

---

## Cancellation

There are two ways the run ends early, both surfacing as `IsAborted == true`:

- **User presses Esc** (when the abort key is enabled). The token your handler observes is signalled.
- **The `Run(token)` token is cancelled.** Pass your own `CancellationToken` to enforce a deadline:

```csharp
using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(900)))
{
    var result = PromptPlus.Controls.ProgressBar("Wait Progress: ", "Token cancels before finish")
        .UpdateHandler(Work)
        .Run(cts.Token);
    // result.IsAborted == true if the token fired before the bar finished
}
```

Because your loop tests `token.IsCancellationRequested`, it exits cleanly; the control then returns
with whatever value it had reached.

---

## Errors

To fail the operation from inside the handler, call `ErrorAndAbort` with an exception. This stops the
run, sets `IsAborted`, and stores the exception on `StateProgress.ExceptionProgress`.

```csharp
.UpdateHandler((bar, token) =>
{
    while (!token.IsCancellationRequested && !bar.Finish)
    {
        token.WaitHandle.WaitOne(100);
        if (bar.Value >= 40)
        {
            bar.ErrorAndAbort(new InvalidOperationException("Simulated failure after 40%."));
            return;
        }
        bar.Update(bar.Value + 5);
    }
})
// …later:
if (result.Content.ExceptionProgress is not null)
    PromptPlus.Console.WriteLine($"Error: {result.Content.ExceptionProgress.Message}");
```

---

## Spinners

[`Spinner(SpinnersType)`](methods.md#spinner) shows a looping animation alongside the bar while it
runs. `SpinnersType` spans several families — braille/dots, lines/bars, shapes, toggles,
arrows/motion, and emoji. On a terminal without Unicode support it **automatically falls back to the
`Ascii` spinner** (`- \ | /`).

See [Spinners](../../spinners.md) for usage and the fallback rules, and the
[Spinner catalog](../../spinners-catalog.md) for every spinner's frames.

---

## Async description refresh

[`ChangeDescriptionAsync`](methods.md#changedescriptionasync) is awaited **synchronously** each time
the value changes. It does not run in parallel with the render loop, so keep the callback fast — a
slow await stalls the refresh.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `ProgressBar` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the run can only end by finishing or by the `Run` token |
| `HideAfterFinish(true)` | Erases the bar after it completes |
| `ShowTooltip(false)` | Hides the keyboard-hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

Note that [`HideElements(HideProgressBar.ProgressbarAtFinish)`](methods.md#hideelements) is the
element-level way to remove just the bar at the end, independent of `HideAfterFinish`.

---

## Edge cases & gotchas

- **Register exactly one handler.** Use `UpdateHandler` *or* `UpdateHandlerAsync`, not both.
- **The loop owns termination.** If it never sets/reaches `Finish` and the token is never cancelled,
  the control never returns. Always gate on both conditions.
- **`Update` clamps.** Values outside the range are pinned to the bounds; they never throw.
- **`Default` and initial value** must lie within the configured `Range`, or construction throws.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Styles](styles.md) — the `ProgressBarStyles` regions
- [Task](../task/index.md) / [MultiTasks](../multitasks/index.md) — for unmeasurable work
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
