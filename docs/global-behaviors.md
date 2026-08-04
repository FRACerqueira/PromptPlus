<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Global Behaviors**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Keyboard Bindings →](keyboard-bindings.md)

---

PromptPlus has two configuration layers. This page covers **Layer 1** — global defaults that apply to every control — and explains how to override them per control.

---

## `IPromptPlusConfig` — Full Property Reference

Set these on `PromptPlus.Config` before running any controls. Changes take effect for all subsequent `.Run()` calls.

### Text & Formatting

| Property | Type | Default | Effect |
|---|---|---|---|
| `SufixAfterPrompt` | `string` | `": "` | Appended after every prompt text |
| `PrefixExtraInfo` | `string` | `"("` | Prefix for extra-info display |
| `SuffixExtraInfo` | `string` | `")"` | Suffix for extra-info display |
| `SecretChar` | `char` | `'#'` | Mask character used by the Secret control |
| `PromptMaskEdit` | `char` | `'_'` | Placeholder character in MaskEdit controls |
| `DefaultCulture` | `CultureInfo` | `CultureInfo.CurrentCulture` | Culture for formatting, parsing, and localization |
| `YesChar` | `char` | `'y'` (culture) | Character accepted as "Yes" in Confirm |
| `NoChar` | `char` | `'n'` (culture) | Character accepted as "No" in Confirm |

### Sizes & Widths

| Property | Type | Default | Constraints | Effect |
|---|---|---|---|---|
| `PageSize` | `byte` | `0` (auto) | — | Items per page; `0` = auto from terminal height |
| `ChartWidth` | `byte` | `80` | 10–255 | Width of chart bar rendering |
| `ProgressBarWidth` | `byte` | `40` | 10–255 | Width of progress bar |
| `SliderWidth` | `byte` | `30` | 10–100 | Width of slider bar |
| `SwitchWidth` | `byte` | `4` | 4–10 | Width of switch toggle |
| `MaxLenghtFilterText` | `byte` | `25` | 5–50 | Max characters in filter input |

### Control & Abort Behavior

| Property | Type | Default | Effect |
|---|---|---|---|
| `EnabledAbortKey` | `bool` | `true` | Enables Esc to abort controls globally |
| `ShowMessageAbortKey` | `bool` | `true` | Includes the abort-key name in tooltips |
| `ShowTooltip` | `bool` | `true` | Shows keyboard hints below prompts |
| `HideAfterFinish` | `bool` | `false` | Erases control UI after the user confirms |
| `HideOnAbort` | `bool` | `false` | Erases control UI when the user presses Esc |
| `RemoveHandlerCtrlC` | `bool` | `false` | When `true`, Ctrl+C is passed to the OS instead of triggering abort |

### Calendar

| Property | Type | Default | Effect |
|---|---|---|---|
| `FirstDayOfWeek` | `DayOfWeek` | `DayOfWeek.Sunday` | First day of week in all calendar controls |

### Hot Keys

Every hot key is settable (`get; set;`) **except** `HotKeyAbortKeyPress`, which is **read-only**
(see the override section below).

| Property | Default key | Settable? | Applies to |
|---|---|---|---|
| `HotKeyAbortKeyPress` | `Esc` | ❌ read-only | All controls — abort |
| `HotKeyTooltip` | `F1` | ✅ | All controls — cycle tooltip content |
| `HotKeyTooltipShowHide` | `Ctrl+F1` | ✅ | All controls — toggle tooltip visibility |
| `HotKeyInputHistoryView` | `F3` | ✅ | Input, Secret, Select, MultiSelect — show history |
| `HotKeyInputPasswordView` | `F2` | ✅ | Secret — reveal / hide typed text |
| `HotKeyCalendarSwitchNotes` | `F2` | ✅ | Calendar — toggle notes display |
| `HotKeyChartBarSwitchLayout` | `F2` | ✅ | ChartBar — switch layout |
| `HotKeyChartBarSwitchLegend` | `F3` | ✅ | ChartBar — toggle legend |
| `HotKeyChartBarSwitchOrder` | `F4` | ✅ | ChartBar — cycle sort order |
| `HotKeyToggleAll` | `F2` | ✅ | MultiSelect, TreeMultiSelect, TableMultiSelect, MultiFile — toggle all |
| `HotKeyFilterAllSelected` | `F3` | ✅ | MultiSelect, TreeMultiSelect, TableMultiSelect — filter selected |
| `HotKeySelectWildcard` | `F4` | ✅ | MultiFile, TreeMultiSelect — select entries by wildcard pattern |
| `HotKeyToggleFullPath` | `Shift+F3` | ✅ | File, MultiFile, TreeSelect, TreeMultiSelect — toggle full path |

---

## Error logging

When an unhandled exception aborts a control, PromptPlus writes a diagnostic file to:

```
%LocalAppData%/PromptPlus/PromptPlus.error.log
```

This is the OS local-application-data folder and resolves correctly on Windows, Linux, and macOS.
Writing the log never throws — any failure is silently ignored so it cannot interfere with shutdown.

---

## `HotKey` — Overriding a Shortcut

Use the `HotKey` struct to reassign any **settable** hot key. Its constructor is
`HotKey(ConsoleKey key, bool alt = false, bool ctrl = false, bool shift = false)`:

```csharp
using PromptPlusLibrary;
using System;

// Move the history hotkey from F3 to F7
PromptPlus.Config.HotKeyInputHistoryView = new HotKey(ConsoleKey.F7);

// Require Ctrl+L to toggle the tooltip
PromptPlus.Config.HotKeyTooltip = new HotKey(ConsoleKey.L, ctrl: true);
```

> ⚠️ `HotKeyAbortKeyPress` is **read-only** and always `Esc` — it cannot be reassigned. To turn the
> abort key off, set `EnabledAbortKey = false` (globally) or use `.Options(o => o.EnabledAbortKey(false))`
> per control.

---

## `ToFile()` — Persisting Config to Disk

Write the current `PromptPlus.Config` state to a JSON file so it is loaded automatically on the next run:

```csharp
using PromptPlusLibrary;

// Configure
PromptPlus.Config.PageSize = 10;
PromptPlus.Config.HideAfterFinish = true;

// Save to current directory
PromptPlus.Config.ToFile(".");
```

This writes `PromptPlus.config` to the specified folder. On the next application startup, PromptPlus looks for this file in the working directory and loads it automatically.

Example `PromptPlus.config` (JSON):

```json
{
  "PageSize": 10,
  "HideAfterFinish": true,
  "DefaultCulture": "en-US"
}
```

> 💡 You only need to include properties you want to override. Missing properties use their built-in defaults.

---

## Per-Control Override — `IControlOptions`

Every control exposes `.Options(action)` to override global settings for that single instance:

```csharp
using PromptPlusLibrary;

PromptPlus.Controls
	.Input("Your name")
	.Options(o => o
		.Prompt("Enter your full name")
		.Description("First and last name, please")
		.SufixAfterPrompt(" → ")
		.HideAfterFinish(false)
		.ShowTooltip(true)
		.EnabledAbortKey(false))
	.Run();
```

### Available overrides via `IControlOptions`

| Method | What it overrides |
|---|---|
| `Prompt(text)` | The prompt text shown to the user |
| `Description(text)` | Optional description line below the prompt |
| `SufixAfterPrompt(text)` | Text appended after the prompt (overrides global `SufixAfterPrompt`) |
| `PrefixExtraInfo(text)` | Prefix for extra-info section |
| `SuffixExtraInfo(text)` | Suffix for extra-info section |
| `EnabledAbortKey(bool)` | Enable or disable Esc for this control |
| `ShowMessageAbortKey(bool)` | Show or hide abort-key hint in tooltip |
| `ShowTooltip(bool)` | Show or hide keyboard hints |
| `HideAfterFinish(bool)` | Erase control UI after confirmation |
| `HideOnAbort(bool)` | Erase control UI after Esc |

---

## Observable Behaviors — Reference

| Behavior | Observable effect |
|---|---|
| Terminal resize detection | Control re-renders its own area automatically; surrounding output is untouched |
| Minimum terminal size (80×10) | Control shows a resize prompt and waits — never crashes or corrupts output |
| Culture isolation | `DefaultCulture` applied only during `.Run()`; thread culture always restored after — even on error |
| Single-line rendering | Newlines stripped from display; sliding window with `…` when value is too wide; `result.Content` always holds the original full value |
| History persistence | Last confirmed value saved to disk; pre-loaded on next run; configurable via `IHistoryOptions` |
| HideAfterFinish | Control UI erased after confirmation; only the final answer line remains |
| HideOnAbort | Control UI erased when user presses Esc |
| Ctrl+C handling | Intercepted by default → triggers abort; `RemoveHandlerCtrlC = true` passes to OS |
| Tooltip visibility | `ShowTooltip = true` shows keyboard hints below the prompt |
| Abort key hint | `ShowMessageAbortKey = true` includes the abort-key name in the tooltip |
| Auto-initialization | PromptPlus initializes on first access: detects terminal, loads `PromptPlus.config` if present, registers error log hook |
| Redirected console input | Interactive controls throw `InvalidOperationException` immediately when console input is redirected, instead of hanging forever. `ProgressBar`, `Task`, `MultiTasks`, `Timer` are exempt — they complete on their own signal and run normally under redirected input. [Demo Mode](demo-mode.md) is also exempt while active |

### Running under redirected/non-interactive input

```csharp
using PromptPlusLibrary;

// Under a redirected console (a file, a pipe, most CI runners), this throws
// InvalidOperationException immediately instead of hanging:
var result = PromptPlus.Controls.Input("Name").Run();

// Live controls have no such restriction — they don't wait on a real keystroke:
var state = PromptPlus.Controls.Task("Working")
    .Action(_ => DoWork())
    .Run();
```

An interactive control fundamentally needs a real key press to produce a result, and a redirected
console has no keyboard input buffer to read one from — a `ProgressBar`, `Task`, `MultiTasks`, or
`Timer` control is the normal way to handle automated/CI scenarios instead. The one opt-out is
[Demo Mode](demo-mode.md): while `PromptPlus.Console.DemoModeActive` is `true` (enabled and a
scripted key is currently queued), this guard does not fire, since a scripted key is available
regardless of redirection — see the linked guide for the caveats. See
[ADR0023](adr/ADR0023V01R02-GuardInteractiveControlsAgainstRedirectedInput.md) for the full rationale.

---

## See also

- [Architecture](architecture.md) — two config layers explained
- [Keyboard Bindings](keyboard-bindings.md) — full hotkey and Emacs reference
- [Getting Started](getting-started.md) — config walkthrough in a working app
