<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Split PromptPlus 5.x into two projects (ConsolePlus + PromptPlus)|
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

  ## **ADR0001V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0001V01R01 — Split PromptPlus 5.x into two projects (ConsolePlus + PromptPlus)

## Context

Up to version **5.x**, PromptPlus was a single library that bundled two very
different responsibilities:

- **Low-level console rendering** — writing styled output, colors, markup,
  cursor/screen control, terminal capability detection, and graceful degradation
  across terminals, SSH sessions, CI pipelines, and redirected output.
- **High-level interaction** — prompts and controls (input, select, confirm,
  etc.) that read the keyboard and return a user result.

Bundling both in one package created problems:

- Consumers who only needed **rendering** were forced to depend on the entire
  interaction stack.
- The rendering engine could not evolve or be versioned independently of the
  interactive controls.
- The internal boundary between "how you render" and "how you interact" was
  blurred, making the codebase harder to test and reason about.

## Decision

Starting after the 5.x line, **split PromptPlus into two separate projects, each
in its own repository**:

- **ConsolePlus** — the rendering foundation ("how you render"): capability
  profile, ANSI/non-ANSI drivers, colors, markup, styles, cursor/screen, emoji,
  and low-level ANSI access.
- **PromptPlus** — the interactive toolkit ("how you interact"): controls and
  widgets layered on top of ConsolePlus.

PromptPlus depends on ConsolePlus (one-directional); ConsolePlus has **no**
dependency on PromptPlus. Both reuse the same console driver instance
(`PromptPlus.Console` is the same as `ConsolePlus.Driver`).

## Consequences

- **Positive:** ConsolePlus can be consumed standalone for pure rendering; each
  project versions and releases independently; the rendering/interaction boundary
  is explicit and testable.
- **Negative / trade-off:** two repositories to coordinate when the shared driver
  contract changes; consumers upgrading from PromptPlus 5.x must add the
  ConsolePlus dependency and follow the migration guide.

## Related

- The migration-guide rules in [ADR0013](ADR0013V01R01-MigrationGuideRules.md).
- ConsolePlus ADR0001 records the same decision from the rendering side.

