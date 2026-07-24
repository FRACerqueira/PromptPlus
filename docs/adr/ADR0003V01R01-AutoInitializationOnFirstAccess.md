<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0003V01R01 |
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

  ## **ADR0003V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0002V01R01](ADR0002V01R01-StaticFacadeFourEntryPoints.md) • [ADR Index](README.md) • **Next:** [ADR0004V01R01 →](ADR0004V01R01-ResultPromptStruct.md)

---

# ADR0003V01R01 — Auto-initialization on first access

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

A console UI library must know terminal capabilities before rendering, but
forcing users to call an explicit `Init()` is error-prone and easy to forget.

## Decision

Initialize automatically the first time any of the four entry points is
accessed. The sequence is:

1. Detect terminal capabilities (color support, Unicode, window size).
2. Look for `PromptPlus.config` in the working directory and load it if present.
3. Register an error-log hook that writes unhandled exceptions to
   `%LocalAppData%/PromptPlus/PromptPlus.error.log`.

No initialization method is exposed or required.

## Consequences

- **Positive:** zero-setup usage; capabilities are always resolved before the
  first render.
- **Negative / trade-off:** first-access cost is hidden; the error-log path is a
  fixed convention (not the working directory) that must be documented so users
  know where to look.
