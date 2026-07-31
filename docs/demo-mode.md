<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Demo Mode**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Global Behaviors](global-behaviors.md) • [Back to Home](../README.md) • [Docs Index](index.md)

---

**Demo Mode** is a ConsolePlus feature — surfaced automatically through `PromptPlus.Console`, since
that property *is* the underlying `IConsole` — that lets you script keyboard input ahead of time
instead of typing it live. It exists to make **recording** interactive console apps (GIFs, videos,
screenshots for a README) reliable and repeatable. It is exactly how the demo video in
[this project's own README](../README.md) was produced.

> 📖 For the full member-by-member API reference (`DemoModeEnabled`, `EnqueueText`, `ScriptedDelayMs`,
> etc.), see ConsolePlus's [Demo Mode guide](https://github.com/FRACerqueira/ConsolePlus/blob/develop/docs/demo-mode.md).
> This page covers what's specific to **PromptPlus controls**.

## Table of contents
- [Quick example](#quick-example)
- [The redirected-input guard exception](#the-redirected-input-guard-exception)
- [Live controls need no scripted keys](#live-controls-need-no-scripted-keys)
- [Full runnable sample](#full-runnable-sample)

---

## Quick example

```csharp
using PromptPlusLibrary;

PromptPlus.Console.DemoModeEnabled = true;
PromptPlus.Console.ScriptedDelayMs = 180; // typing-effect pacing between keys

// Enqueue immediately before .Run() — Run() only returns after consuming its own Enter,
// so ordering across multiple controls stays correct with no extra synchronization.
PromptPlus.Console.EnqueueText("Fulano", delayMs: 500);
PromptPlus.Console.EnqueueKey(ConsoleKey.Enter, delayMs: 500);
var name = PromptPlus.Controls.Input("Name").Run();

PromptPlus.Console.DemoModeEnabled = false; // back to real keyboard input
```

Every control consumes scripted keys exactly as if they came from a real keyboard — no control-level
opt-in is needed beyond enabling Demo Mode on `PromptPlus.Console` and queuing the keys that control
expects.

---

## The redirected-input guard exception

By default, `.Run()`/`.Show()` on an interactive control throws `InvalidOperationException`
immediately when console input is redirected, instead of hanging forever waiting for a key that can
never arrive — see [Global Behaviors](global-behaviors.md#running-under-redirectednon-interactive-input)
and [ADR0023](adr/ADR0023V01R02-GuardInteractiveControlsAgainstRedirectedInput.md) for the full
rationale. That guard now has one exception: it does not fire while `DemoModeActive` is `true`
(Demo Mode enabled **and** a scripted key currently queued), since a scripted key is available
regardless of redirection.

This is what lets `AutoDemoSamples` (below) drive real interactive controls — `Input`, `Select`,
`MultiSelect`, `MaskDate`, and more — from a recording pipeline where stdio may well be redirected.

> ⚠️ **The exception is evaluated per key read, not for the whole run.** `DemoModeEnabled = true`
> alone does not make a redirected run safe. If the scripted queue runs dry while a control still
> needs another key, the guard's normal redirected-input behavior resumes for that read. A script
> driving a redirected/headless run must queue every key each control needs before letting that
> control's `.Run()` return. See ConsolePlus's
> [Demo Mode and redirected/headless input](https://github.com/FRACerqueira/ConsolePlus/blob/develop/docs/demo-mode.md#demo-mode-and-redirectedheadless-input).

---

## Live controls need no scripted keys

`ProgressBar`, `Task`, `MultiTasks`, and `Time` complete on their own signal (progress reaching 100%,
the wrapped task finishing, the countdown elapsing) — they never wait on a real keystroke, so they run
identically under Demo Mode, redirected input, both, or neither. Nothing needs to be scripted for
them; just run them as usual inside a Demo Mode script.

---

## Full runnable sample

[`samples/AutoDemoSamples`](../samples/AutoDemoSamples/Program.cs) is a complete, runnable console app
that scripts a walkthrough of ten different controls and widgets back to back — `Input`, `Select`,
`MultiSelect`, `MaskDate`, `Input` with suggestions (both auto-complete and manual), `MultiTasks`,
`ProgressBar`, `Slider`, and `ChartBar`. It is the actual source used to record this project's README
demo video: run it, select the terminal window as your recording area during the initial
`Console.ReadKey()` pause, and it plays itself out at a natural typing pace.

---

[← Global Behaviors](global-behaviors.md) • [Back to Home](../README.md) • [Docs Index](index.md)
