<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0016V01R01 |
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

  ## **ADR0016V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0015V01R01](ADR0015V01R01-TestFrameworkXUnitVerify.md) • [ADR Index](README.md) • **Next:** [ADR0017V01R01 →](ADR0017V01R01-AnsiStyleModelColorOnly.md)

---

# ADR0016V01R01 — Target frameworks mirror production (net10/net9/net8)

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-22

## Context

The test projects must exercise the libraries on the same runtimes the libraries
ship for, to catch target-framework-specific behavior.

## Decision

Mirror the production target framework list **exactly** — same monikers and same
order — in every `*.Tests.csproj`:

```
net10.0;net9.0;net8.0
```

This was confirmed by direct reading of `ConsolePlus/src/ConsolePlus.csproj:4`
and `PromptPlus/src/PromptPlus.csproj:3`, which both use `net10.0;net9.0;net8.0`.

## Consequences

- **Positive:** tests run against all shipped runtimes; no gap between tested and
  released frameworks.
- **Negative / trade-off:** multi-target test builds are slower and multiply the
  matrix; the list must be kept in sync if production TFMs change.
