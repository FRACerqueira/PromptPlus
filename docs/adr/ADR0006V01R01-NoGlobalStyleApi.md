<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0006V01R01 |
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

  ## **ADR0006V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0005V01R01](ADR0005V01R01-TwoConfigurationLayers.md) • [ADR Index](README.md) • **Next:** [ADR0007V01R01 →](ADR0007V01R01-ControlsVsWidgetsSeparation.md)

---

# ADR0006V01R01 — No global style API; per-control styling

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

Theming needs to be flexible per control without a sprawling global theme object
that every control must know about.

## Decision

There is **no global style API**. `PromptPlusStyles` does not exist and
`IPromptPlusConfig` has no `Styles(...)` member. Styling is applied per control
via `.Styles(<ControlEnum>, style)` (e.g. `InputStyles`, `SelectStyles`). The
`Style` value carries foreground, background, and an overflow strategy only —
there is no text-decoration concept. The style-region enum backing the controls
(`ComponentStyles`) is internal.

## Consequences

- **Positive:** each control owns its own style regions; no global god-object;
  styling is local and explicit.
- **Negative / trade-off:** applying one theme across many controls requires
  repeating `.Styles(...)` per control (or a user-side helper). Accepted for
  clarity and decoupling.
