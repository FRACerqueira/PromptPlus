<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0008V01R01 |
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

  ## **ADR0008V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0007V01R01](ADR0007V01R01-ControlsVsWidgetsSeparation.md) • [ADR Index](README.md) • **Next:** [ADR0009V01R01 →](ADR0009V01R01-AsciiFallbackForSymbols.md)

---

# ADR0008V01R01 — Distinct result model for Confirm and KeyPress

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

Not every interactive control returns arbitrary content. Confirm is a yes/no
decision; KeyPress captures a single keystroke. Forcing them into the generic
`ResultPrompt<T>` value shape would hide their real semantics.

## Decision

- **Confirm** returns a result whose meaningful state is a boolean answer plus
  `IsAborted`; callers read the yes/no outcome, not a free-form `Content`.
- **KeyPress** returns the captured key information plus `IsAborted`, exposing
  the key and its modifiers rather than a text value.

Both still follow the abort-is-not-an-exception rule from ADR0004.

## Consequences

- **Positive:** each control's result reads naturally for its purpose; abort
  semantics remain consistent across all controls.
- **Negative / trade-off:** slightly more result shapes to learn; justified by
  the clarity of intent per control.
