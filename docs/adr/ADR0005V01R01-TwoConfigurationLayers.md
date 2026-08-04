<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Two configuration layers (global vs per-control)|
|Version|01|
|Revision|01|
|Scope||
|Domain||
|Created|Proposed (2026-07-24)|
|Changed|Accepted (2026-07-24)|
|Superseded||
<!-- Do not remove this comment, lines and table (1-12) -->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0005V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0005V01R01 — Two configuration layers (global vs per-control)

## Context

Applications want app-wide defaults (page size, hide-after-finish, culture) but
also need to override them for a single prompt without mutating global state.

## Decision

Provide two layers:

- **Layer 1 — Global** (`PromptPlus.Config`): defaults for every control, set
  once at startup.
- **Layer 2 — Per-control** (`.Options(o => ...)`): overrides for a single
  control instance.

Per-control always wins over global. Global is never mutated by a per-control
override.

## Consequences

- **Positive:** predictable precedence; global stays a clean baseline; local
  overrides are explicit and scoped.
- **Negative / trade-off:** two places to look when reasoning about a control's
  effective settings — mitigated by the strict "local wins" rule.

