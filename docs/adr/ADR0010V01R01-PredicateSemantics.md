<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0010V01R01 |
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

  ## **ADR0010V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0009V01R01](ADR0009V01R01-AsciiFallbackForSymbols.md) • [ADR Index](README.md) • **Next:** [ADR0011V01R01 →](ADR0011V01R01-PerControlCultureIsolation.md)

---

# ADR0010V01R01 — Predicate semantics for validation and filtering

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

Controls accept user predicates for validation and item filtering. Inconsistent
boolean meaning (does `true` mean "valid" or "reject"?) is a frequent source of
bugs.

## Decision

Standardize predicate meaning across all controls:

- **Validation** predicate returns `true` when the input **is valid**.
- **Filter** predicate returns `true` when the item **should be shown**.

`true` always means "keep / accept". No control inverts this.

## Consequences

- **Positive:** one rule to remember for every predicate; fewer inverted-logic
  bugs.
- **Negative / trade-off:** users migrating from libraries with the opposite
  convention must adapt; the rule is documented per control to prevent surprises.
