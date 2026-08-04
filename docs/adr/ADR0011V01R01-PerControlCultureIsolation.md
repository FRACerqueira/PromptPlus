<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Per-control culture isolation|
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

  ## **ADR0011V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0010V01R01](ADR0010V01R01-PredicateSemantics.md) • [ADR Index](README.md) • **Next:** [ADR0012V01R01 →](ADR0012V01R01-GeneratedApiDocsOffLimits.md)

---

# ADR0011V01R01 — Per-control culture isolation

## Context

Controls that parse or format numbers, dates, and masks must respect a culture.
Changing `Thread.CurrentThread.CurrentCulture` globally would leak into the host
application's own formatting.

## Decision

A control's culture is set via `.Culture(...)` and applied **only within that
control's execution scope**. The global/thread culture is never mutated. When no
culture is specified, the control uses the ambient culture read at run time.

## Consequences

- **Positive:** no side effects on the host app's culture; parallel prompts can
  use different cultures safely.
- **Negative / trade-off:** culture must be passed explicitly when it differs
  from the ambient one; accepted to guarantee isolation.

