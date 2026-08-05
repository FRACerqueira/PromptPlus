<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Test driver sharing mechanism|
|Version|01|
|Revision|01|
|Scope||
|Domain||
|Created|Proposed (2026-07-22)|
|Changed|Accepted (2026-07-22)|
|Superseded|Superseded (2026-07-23) : 0026|
<!-- Do not remove this comment, lines and table (1-12) -->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0014V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0014V01R01 — Test driver sharing mechanism

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
- **Negative / trade-off:** the mechanism assumes a single repository. If the
  libraries live in **separate repositories**, a cross-repo relative
  `<Compile Include>` link becomes fragile or impossible.

## Superseded

This record was superseded on 2026-07-23 when it was confirmed that ConsolePlus
and PromptPlus are **two distinct GitHub repositories**, invalidating the
single-repo assumption. See
[ADR0026V01R01](ADR0026V01R01-TestDriverSharingMechanism--0014.md).

