<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0009V01R01 |
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

  ## **ADR0009V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0008V01R01](ADR0008V01R01-ConfirmKeyPressResultModel.md) • [ADR Index](README.md) • **Next:** [ADR0010V01R01 →](ADR0010V01R01-PredicateSemantics.md)

---

# ADR0009V01R01 — ASCII fallback for visual symbols

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

PromptPlus uses Unicode glyphs for selection markers, prompts, and status icons.
Some terminals, code pages, or redirected outputs cannot render these glyphs and
would show mojibake or boxes.

## Decision

Every visual symbol has an ASCII fallback. When the runtime capability detection
(from ConsolePlus) reports that Unicode is unavailable, PromptPlus renders the
ASCII variant automatically. Symbols are defined as Unicode/ASCII pairs, never as
a bare Unicode literal.

## Consequences

- **Positive:** output stays legible on limited terminals and CI logs; no manual
  configuration needed.
- **Negative / trade-off:** ASCII variants look plainer; a symbol added without
  its fallback pair is a defect, so the paired definition is a maintenance rule.
