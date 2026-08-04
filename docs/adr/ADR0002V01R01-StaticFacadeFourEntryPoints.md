<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Static facade with four entry points|
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

  ## **ADR0002V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0001V01R01](ADR0001V01R01-SplitConsolePlusAndPromptPlus.md) • [ADR Index](README.md) • **Next:** [ADR0003V01R01 →](ADR0003V01R01-AutoInitializationOnFirstAccess.md)

---

# ADR0002V01R01 — Static facade with four entry points

## Context

PromptPlus needs a discoverable, low-ceremony surface. Users should not have to
wire up services, instantiate renderers, or manage lifetimes to show a prompt.

## Decision

Expose everything through a single static facade `PromptPlus` with exactly four
entry points:

- `.Config` → `IPromptPlusConfig` — global settings for all controls.
- `.Controls` → `IControls` — factory for interactive controls.
- `.Widgets` → `IWidgets` — factory for output-only widgets.
- `.Console` → `IConsole` — the ConsolePlus driver (same instance as `ConsolePlus.Driver`).

Objects are never instantiated manually; all access flows through these four
properties.

## Consequences

- **Positive:** trivial onboarding, one import, no DI required; a single mental
  model shared with ConsolePlus (which uses the same facade pattern).
- **Negative / trade-off:** global static state; testability depends on the
  facade being process-wide. Accepted for a console UX library.

