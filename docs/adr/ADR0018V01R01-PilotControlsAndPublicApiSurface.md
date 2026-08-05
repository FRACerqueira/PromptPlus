<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Pilot controls and public API surface|
|Version|01|
|Revision|01|
|Scope||
|Domain||
|Created|Proposed (2026-07-22)|
|Changed|Accepted (2026-07-22)|
|Superseded||
<!-- Do not remove this comment, lines and table (1-12) -->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0018V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0018V01R01 — Pilot controls and public API surface

## Context

A small pilot is needed to validate the end-to-end control-testing approach
before rolling it out to all controls. This requires knowing the exact public
API surface used to build, run and read a control result.

## Decision

Pilot with **`Input` + `Select`**, driving them through the **public interface
surface** (no `dynamic`/cast to internal concrete classes):

- `PromptPlusControls.Input(...)` / `.Select<T>(...)` return public interfaces
  `IInputControl` / `ISelectControl<T>`
  (`PromptPlus/src/Core/PromptControls.cs:167-188`).
- Execution is `Run(CancellationToken token = default)` — not a parameterless
  `Run()` — (`IInputControl.cs:173`, `ISelectControl.cs:37`).
- The result is `ResultPrompt<T>` (`Shared/Common/ResultPrompt.cs:16-38`) whose
  properties are **`Content`** (not `Value`) and **`IsAborted`**.

## Consequences

- **Positive:** tests bind to the stable public contract, not internals;
  validates the pattern on one input-style and one selection-style control.
- **Negative / trade-off:** the pilot only exercises `Input` and `Select`'s
  shape; other control families (e.g. multi-select, confirm) may expose a
  different public surface not validated until their own rollout.
- **Operational rule:** every control test must
  (1) enqueue a terminal key (Enter to confirm, Escape to abort) as the last key,
  and (2) pass a short-timeout `CancellationToken` to `Run(token)` to avoid the
  `WaitKeypress` spin-wait hang.

