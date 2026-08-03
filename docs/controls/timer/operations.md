<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Timer — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Timer — Styles →](styles.md)

---

How the `Timer` control behaves while it runs: the required duration, count-down vs. count-up display,
formatting, cancellation, and the details that matter in real apps.

---

## Anatomy of the control

```
Please wait  00:00:03  ⠹                   ← prompt + time value + spinner (optional)
Remaining: 3 second(s)                     ← ChangeDescription output (optional)
Done!                                       ← Finish text (at the end)
Esc: cancel                               ← tooltip
```

The `Timer` control is non-interactive: there is nothing to type or select. The only user action is
**Esc** to abort (when enabled). Each region can be recolored — see [Styles](styles.md).

---

## Duration is required

There is no default length — you must call [`Duration`](methods.md#duration) with a value greater than
zero, as seconds or a `TimeSpan`:

```csharp
PromptPlus.Controls.Timer("Starting in").Duration(3).Run();                     // 3 seconds
PromptPlus.Controls.Timer("Cooling down").Duration(TimeSpan.FromSeconds(10)).Run();
```

A zero or negative duration throws `ArgumentOutOfRangeException`.

---

## Count down or count up

[`DisplayMode`](methods.md#displaymode) controls only what the on-screen number shows:

- **`Countdown`** (default) — the number starts at the duration and ticks toward `00:00:00`.
- **`Elapsed`** — the number starts at zero and ticks up to the duration.

The value in the [`ChangeDescription`](methods.md#changedescription) callback is always the
**remaining** time. In `Elapsed` mode, derive the remainder if you need it:

```csharp
var totalDuration = TimeSpan.FromSeconds(5);

PromptPlus.Controls.Timer("Running")
    .Duration(totalDuration)
    .DisplayMode(TimerDisplayMode.Elapsed)
    .ChangeDescription(elapsed =>
    {
        var remaining = totalDuration - elapsed;
        if (remaining < TimeSpan.Zero) remaining = TimeSpan.Zero;
        return $"Remaining: {remaining.TotalSeconds:0} second(s)";
    })
    .Run();
```

> Regardless of display mode, `Run()` returns the **elapsed** `TimeSpan` in `.Content`.

---

## Formatting

[`Format`](methods.md#format) is a standard `TimeSpan` format string; the default is `hh\:mm\:ss`.
Combine it with [`Culture`](methods.md#culture) for locale-specific rendering.

```csharp
PromptPlus.Controls.Timer("Countdown")
    .Duration(5)
    .Format(@"mm\:ss\:fff")     // show milliseconds
    .Run();
```

---

## Cancellation

The countdown ends early — reporting `IsAborted == true` — when the user presses **Esc** (abort key
enabled) or the `Run(token)` token fires. On abort, `.Content` holds the time elapsed so far, not the
full duration.

```csharp
using (var sw = new CancellationTokenSource(TimeSpan.FromSeconds(2)))
{
    var result = PromptPlus.Controls.Timer("Cancelable countdown")
        .Duration(10)
        .Run(sw.Token);
    // result.IsAborted == true; result.Content ≈ 2 seconds
}
```

---

## Spinners

[`Spinner(SpinnersType)`](methods.md#spinner) shows a looping animation next to the countdown while it
runs. `SpinnersType` spans several families — braille/dots, lines/bars, shapes, toggles,
arrows/motion, and emoji. On a terminal without Unicode support it **automatically falls back to the
`Ascii` spinner** (`- \ | /`).

See [Spinners](../../spinners.md) for usage and the fallback rules, and the
[Spinner catalog](../../spinners-catalog.md) for every spinner's frames.

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Timer` |
|---|---|
| `EnabledAbortKey(true/false)` | Enables/removes Esc as a way to end the countdown early |
| `ShowMessageAbortKey(true)` | Shows the abort-key hint |
| `HideAfterFinish(true)` | Erases the control after the countdown ends |
| `HideOnAbort(true)` | Erases the control after Esc |
| `ShowTooltip(false)` | Hides the keyboard-hint line |
| `Prompt(...)` / `Description(...)` | Overrides the prompt / description text |

```csharp
PromptPlus.Controls.Timer("Please wait")
    .Duration(4)
    .Options(opt =>
    {
        opt.Description("Press ESC to abort the countdown");
        opt.ShowTooltip(false);
        opt.ShowMessageAbortKey(true);
        opt.EnabledAbortKey(true);
        opt.HideAfterFinish(false);
        opt.HideOnAbort(false);
    })
    .Run();
```

---

## Edge cases & gotchas

- **Not a picker.** `Timer` waits and displays a clock; it does not collect a time value. To *enter* a
  time, use a MaskEdit date/time control.
- **Duration is mandatory.** Forgetting it (or passing `0`) throws.
- **Description receives remaining time** even in `Elapsed` mode — compute the counterpart yourself.
- **Async description refresh is awaited synchronously** — keep
  [`ChangeDescriptionAsync`](methods.md#changedescriptionasync) fast.
- **A blank prompt is fine:** `PromptPlus.Controls.Timer().Duration(5)...` renders just the countdown.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Styles](styles.md) — the `TimerStyles` regions
- [Task](../task/index.md) / [MultiTasks](../multitasks/index.md) — do work during a wait
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
