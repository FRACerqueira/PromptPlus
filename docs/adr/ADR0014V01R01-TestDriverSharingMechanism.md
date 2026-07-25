<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0014V01R01 |
| Version | 01 |
| Revision | 01 |
| Status | Superseded |
| Created | 2026-07-22 |
| Changed | 2026-07-22 |
| Superseded | ADR0014V02R01-TestDriverSharingMechanism.md |
-->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0014V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0013V01R01](ADR0013V01R01-MigrationGuideRules.md) • [ADR Index](README.md) • **Next:** [ADR0014V02R01 →](ADR0014V02R01-TestDriverSharingMechanism.md)

---

# ADR0014V01R01 — Test driver sharing mechanism

- **Status:** Superseded by [ADR0014V02R01](ADR0014V02R01-TestDriverSharingMechanism.md)
- **Version:** V01 / Revision R01
- **Created:** 2026-07-22

## Context

The test architecture needs a shared "driver" (an ANSI-consuming console test
harness, `tests/_driver-src/*.cs`) usable by the test projects of both
ConsolePlus and PromptPlus. At the time this decision was first closed
(2026-07-22), the working assumption was a **single repository** hosting both
libraries, and no test project or link-source pattern existed yet in the repo
(clean ground, no conflict).

## Decision

Adopt a **single linked-source** mechanism: keep one physical copy of the driver
under `tests/_driver-src/*.cs` and reference it from each `*.Tests.csproj` with
`<Compile Include="..." LinkBase="..." />`, so both test projects compile the
same source files without duplication.

## Consequences

- **Positive:** one source of truth for the driver; no drift between the two
  test projects; simple to reason about.
- **Negative / risk:** the mechanism assumes a single repository. If the
  libraries live in **separate repositories**, a cross-repo relative
  `<Compile Include>` link becomes fragile or impossible.

## Superseded

This record was superseded on 2026-07-23 when it was confirmed that ConsolePlus
and PromptPlus are **two distinct GitHub repositories**, invalidating the
single-repo assumption. See
[ADR0014V02R01](ADR0014V02R01-TestDriverSharingMechanism.md).
