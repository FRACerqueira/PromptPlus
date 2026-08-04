<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Generated API docs are off-limits for manual edits|
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

  ## **ADR0012V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0012V01R01 — Generated API docs are off-limits for manual edits

## Context

The `docs/api/` folder is generated from XML doc comments by the documentation
tooling. Manual edits there are silently overwritten on the next build and give
a false impression of being authoritative.

## Decision

`docs/api/` is **generated output** and must never be edited by hand. All API
documentation changes are made in the source XML doc comments. Narrative and
conceptual documentation lives in the hand-written `docs/*.md` files, which are
the only Markdown docs that may be edited directly.

## Consequences

- **Positive:** single source of truth for API docs (the code); no lost edits.
- **Negative / trade-off:** correcting API text requires a code change and a
  regeneration step rather than a quick Markdown edit.

