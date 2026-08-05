<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Controls vs Widgets separation|
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

  ## **ADR0007V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0007V01R01 — Controls vs Widgets separation

## Context

The library offers both interactive prompts (that read the keyboard and return a
value) and output-only visual elements (banners, tables, progress). Mixing them
under one factory blurs their very different lifecycles.

## Decision

Split the surface into two factories:

- **`PromptPlus.Controls`** — interactive elements that block, read input, and
  return a `ResultPrompt<T>` via `.Run()`.
- **`PromptPlus.Widgets`** — output/visual elements that render to the console
  and do not return a user value.

A type is a control if and only if it produces a `ResultPrompt<T>`.

## Consequences

- **Positive:** the return contract makes the category unambiguous; discovery is
  guided by intent (interact vs display).
- **Negative / trade-off:** some elements (e.g. progress) could be argued either
  way; the "returns a result?" test resolves the ambiguity deterministically.

