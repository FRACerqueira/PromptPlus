<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0013V01R01 |
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

  ## **ADR0013V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0012V01R01](ADR0012V01R01-GeneratedApiDocsOffLimits.md) • [ADR Index](README.md) • **Next:** [ADR0014V01R01 →](ADR0014V01R01-TestDriverSharingMechanism.md)

---

# ADR0013V01R01 — Migration guide maintenance rules (v5 → v6)

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

The v5→v6 release changed the public surface significantly (static facade, new
result model, per-control styling). Users upgrading need an accurate, curated
mapping of old-to-new APIs.

## Decision

Maintain a dedicated `migration-v5-to-v6.md` guide that documents each breaking
change as an explicit old→new mapping. The guide only describes APIs that exist
in v6; removed APIs are shown solely for their replacement. Every breaking change
introduced going forward must be reflected in this guide as part of the change.

## Consequences

- **Positive:** upgraders have a single, trustworthy reference; the guide stays
  accurate because updating it is part of the change definition.
- **Negative / trade-off:** additional maintenance burden per breaking change;
  accepted as the cost of a reliable migration path.
