<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Test framework: xUnit + Verify|
|Version|01|
|Revision|01|
|Scope||
|Domain||
|Created|Proposed (2026-07-22)|
|Changed|Accepted (2026-07-22)|
|Superseded||
<!-- Do not remove this comment, lines and table (1-12) -->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0015V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0015V01R01 — Test framework: xUnit + Verify

## Context

The test suite needs a test runner and a snapshot/approval mechanism to assert
rendered console output. The main candidates were **xUnit** (with the
**Verify** snapshot library) versus **TUnit**.

## Decision

Adopt **xUnit + Verify** as the test framework and snapshot mechanism. The
existing test skeletons are already written for xUnit + Verify and are kept as
they are.

## Consequences

- **Positive:** mature, widely known stack; Verify handles the rendered-output
  snapshots cleanly; the existing test skeletons require no rewrite.
- **Negative / trade-off:** TUnit's newer features are not adopted; the team
  standardizes on xUnit conventions.

