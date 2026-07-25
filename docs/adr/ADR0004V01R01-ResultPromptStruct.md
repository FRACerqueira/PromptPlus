<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0004V01R01 |
| Version | 01 |
| Revision | 01 |
| Status | Accepted |
| Created | 2026-07-24 |
| Changed | 2026-07-24 |
| Superseded |  |
-->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0004V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0003V01R01](ADR0003V01R01-AutoInitializationOnFirstAccess.md) • [ADR Index](README.md) • **Next:** [ADR0005V01R01 →](ADR0005V01R01-TwoConfigurationLayers.md)

---

# ADR0004V01R01 — Result model: `ResultPrompt<T>` readonly struct

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

Every interactive control must return both a value and whether the user aborted
(Esc). Options considered: throw on abort, return `null`, or return a rich
result object.

## Decision

Every `.Run()` returns a **readonly struct** `ResultPrompt<T>` with:

- `.Content` (`T`) — the confirmed value (`default(T)` when aborted).
- `.IsAborted` (`bool`) — `true` when the user pressed Esc or another abort key.
- `.Deconstruct(...)` — enables tuple-style unpacking `var (value, aborted) = ...`.

Abort is a normal, non-exceptional outcome; the property is named `Content`
(not `Value`).

## Consequences

- **Positive:** no exceptions on the common cancel path; value-type avoids heap
  allocation; deconstruction reads cleanly.
- **Negative / trade-off:** callers must check `IsAborted` before trusting
  `Content`; the `Content` vs `Value` naming must be documented because it is a
  common expectation mismatch.
