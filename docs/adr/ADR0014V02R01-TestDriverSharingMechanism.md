<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0014V02R01 |
| Version | 02 |
| Revision | 01 |
| Status | Accepted |
| Created | 2026-07-23 |
| Changed | 2026-07-23 |
| Superseded |  |
-->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0014V02R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0014V01R01](ADR0014V01R01-TestDriverSharingMechanism.md) • [ADR Index](README.md) • **Next:** [ADR0015V01R01 →](ADR0015V01R01-TestFrameworkXUnitVerify.md)

---

# ADR0014V02R01 — Test driver sharing mechanism

- **Status:** Accepted
- **Version:** V02 / Revision R01
- **Created:** 2026-07-23
- **Supersedes:** [ADR0014V01R01](ADR0014V01R01-TestDriverSharingMechanism.md)

## Context

The previous version ([V01](ADR0014V01R01-TestDriverSharingMechanism.md))
assumed a single repository and chose a linked-source mechanism. Verification on
2026-07-23 established that **ConsolePlus and PromptPlus are two independent
GitHub repositories** (`origin` remotes differ), so a single physical
link-source shared across repositories is not viable.

## Decision

Use a **physical copy** of the driver in each repository:
`ConsolePlus/tests/_driver-src` and `PromptPlus/tests/_driver-src`, as
independent copies, each versioned in its own repository.

## Alternatives considered (rejected)

- **Dedicated NuGet package for the driver** — rejected: too much packaging /
  release overhead for a test-only utility.
- **Independent drivers written from scratch per repo** — rejected: guarantees
  divergence and double the maintenance of the ANSI-consuming logic.

## Consequences

- **Positive:** each repository is self-contained and builds without cross-repo
  path assumptions.
- **Negative / risk:** the two copies can drift over time. Mitigation: treat the
  driver as a synchronized artifact per the maintenance doc, and keep the ANSI
  interpreter independent of the writer so unit-level width/overflow tests do not
  depend on the driver itself.
