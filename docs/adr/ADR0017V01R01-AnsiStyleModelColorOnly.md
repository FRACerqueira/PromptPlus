<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0017V01R01 |
| Version | 01 |
| Revision | 01 |
| Status | Accepted |
| Created | 2026-07-22 |
| Changed | 2026-07-22 |
| Superseded |  |
-->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0017V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0016V01R01](ADR0016V01R01-TargetFrameworksMirrorProduction.md) • [ADR Index](README.md) • **Next:** [ADR0018V01R01 →](ADR0018V01R01-PilotControlsAndPublicApiSurface.md)

---

# ADR0017V01R01 — ANSI style model: color only (no attributes)

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-22

## Context

The ANSI interpreter used by the test driver must model the styling that
production code actually emits, no more and no less. The question was whether to
model text attributes (bold, underline, etc.) in the style grid.

## Decision

Model **color only** (foreground / background), **no text attributes**. Verified
in code:

- `Style` (`ConsolePlus/src/Shared/Style.cs:23-38`) exposes only `Foreground`,
  `Background` and `OverflowStrategy`.
- `ConsoleWriter.ApplyStyle` (`ConsoleAbstractions/ConsoleWriter.cs:257-268`)
  emits only SGR sequences from `AnsiColorBuilder` (color).

Production does not emit bold/underline/etc today, so there is nothing to model
in the grid. The `// TODO Fase 2: 4-bit / 256` comments in the source are
confirmed as **"not applicable"**, not as pending work.

## Consequences

- **Positive:** the interpreter stays small and matches real output exactly;
  snapshots are simpler.
- **Negative / risk:** if production later emits attributes, the interpreter and
  this decision must be revisited (a new version of this ADR).
