<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Guard interactive controls against redirected console input in `Run()`|
|Version|01|
|Revision|02|
|Scope||
|Domain||
|Created|Proposed (2026-07-28)|
|Changed|Accepted (2026-07-31)|
|Superseded||
<!-- Do not remove this comment, lines and table (1-12) -->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0023V01R02**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0023V01R02 — Guard interactive controls against redirected console input in `Run()`

## Context

`BaseControlPrompt<T>.WaitKeypress` — the loop every interactive control blocks on while waiting for
a key — is:

```csharp
while (!console.KeyAvailable && !token.IsCancellationRequested)
{
    ...
    token.WaitHandle.WaitOne(16);
}
```

`Show()` and the default `Run()` overload use `CancellationToken.None`, which never cancels. Per
ConsolePlus's [ADR0015 — Redirected/headless console I/O contract](https://github.com/FRACerqueira/ConsolePlus/blob/develop/docs/adr/ADR0015V01R02-RedirectedConsoleIoContract.md),
`KeyAvailable` **fails safe** under redirected input — it returns `false` forever instead of
throwing. Combined, this means any interactive control run against a redirected console with no
caller-supplied `CancellationToken` — the common case, since `Show()` and plain `.Run()` are the
documented default usage pattern throughout `docs/` — **hangs forever**, with no exception, no log,
and no way to tell it apart from a control just waiting for real user input.

This was previously masked by an inconsistency on the ConsolePlus side: before its own redirected-I/O
contract (ADR0015) was in place, `KeyAvailable`/`ReadKey` threw a raw, undocumented exception under
redirection, which at least crashed loudly (if unpredictably). Fixing that inconsistency in
ConsolePlus (correctly, per its own contract) removed the accidental crash and exposed the hang
underneath — confirmed empirically:
`PromptPlus.Controls.Input("Name").Run()` under redirected stdin did not return within 3 seconds.

Not every control is affected equally. `ProgressBar`, `Task`, `MultiTasks`, and `Timer` override
`WaitKeypress` but complete on their own signal (progress reaching 100%, the wrapped task finishing,
the countdown elapsing) — they never actually depend on a real key becoming available, and were
confirmed empirically to complete normally under redirected input. Only controls that have **no**
completion path except a real keystroke are at risk.

## Decision

Guard at the top of `BaseControlPrompt<T>.Run()` — the single method every control and widget
reaches through the `IControls`/`IWidgets` factory interfaces — rather than in the constructor or in
each control individually:

```csharp
if (!isWidget && !IsLiveAutoRenderControl && console.IsInputRedirected && !console.DemoModeActive)
{
    throw new InvalidOperationException(
        "Cannot run an interactive control: console input is redirected and no key presses can be read.");
}
```

- **`Run()`, not the constructor:** a control is constructed at fluent-chain time
  (`Input("x").Default("y")`), before any real intent to interact with it — throwing there would fire
  even when `.Run()` is never called. `BaseControlPrompt<T>` is also a primary constructor with no
  body, so a guard there needs contrived plumbing. `Run()` already owns the `isWidget` gate this
  guard has to respect, and `console.IsInputRedirected` (the real `Console.IsInputRedirected`, not
  the CI-provider heuristic `Profile.Interactive`) was already exposed on `IConsole` and reachable
  right there — no new plumbing needed.
- **Widgets excluded** (`isWidget`) — they never enter the key-reading loop; unaffected either way.
- **`IsLiveAutoRenderControl` controls excluded** (`ProgressBar`/`Task`/`MultiTasks`/`Timer`) — the
  same flag the base class already uses to distinguish "renders automatically, doesn't need a real
  key" controls for resize handling. Guarding them too would have newly broken an already-working,
  legitimate use case: running a spinner/progress/countdown control in a script or CI pipeline with
  redirected input.
- **`DemoModeActive` excluded (R02)** — it means scripted keys are queued and consumed by
  `KeyAvailable`/`ReadKeyAsync` regardless of redirection (see ConsolePlus's
  [Demo Mode](https://github.com/FRACerqueira/ConsolePlus/blob/develop/docs/demo-mode.md)), so the "no
  key presses can be read" premise this guard protects against does not hold for that read. This
  check is per-call: it only excludes reads actually serviced by a queued scripted key, not the whole
  run — see [Consequences](#consequences).

## Consequences

- **Positive:** a redirected/piped/CI run of an interactive control now fails immediately with a
  clear, documented exception instead of hanging indefinitely with no diagnostic. Single chokepoint —
  covers all current and future interactive controls automatically via inheritance; no per-control
  changes needed.
- **Negative / trade-off:** this is a public behavior change. A caller that previously relied on an
  interactive control silently hanging under redirected input (unlikely, but possible if it supplied
  its own timeout `CancellationToken` expecting a graceful abort) now gets an immediate exception
  instead. Callers that need to run under redirected/non-interactive input must use a `Live` control
  (`ProgressBar`/`Task`/`MultiTasks`/`Timer`) instead of an interactive one, **or** drive the control
  under [Demo Mode](../demo-mode.md) with the scripted-key queue kept non-empty for the control's
  entire run — there is no other opt-out.
- **Dependency:** requires a ConsolePlus version that implements the ADR0015 redirected-I/O contract —
  a reliable `IConsole.IsInputRedirected` and `KeyAvailable` failing safe rather than throwing — for
  this guard to be meaningful. As of R02, it also requires a ConsolePlus version that implements
  `DemoModeActive` (any version shipping Demo Mode) for the exception clause to compile/apply.
- **Verified:** `PromptPlus.Tests` 690/690 green (net10.0) — the `VirtualTerminal` test driver
  hardcodes `IsInputRedirected => false`, so no test seam was needed. Empirical check against the real
  `ConsolePlus.net` package: `Input(...).Run()` under redirected stdin now throws immediately with the
  message above; `Task(...).Run()` under the same conditions still completes normally.

