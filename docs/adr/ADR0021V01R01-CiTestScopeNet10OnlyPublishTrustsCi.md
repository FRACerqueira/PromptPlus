<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|CI test scope is net10.0-only; the release workflow trusts CI and does not re-run tests|
|Version|01|
|Revision|01|
|Scope||
|Domain||
|Created|Proposed (2026-07-25)|
|Changed|Accepted (2026-07-25)|
|Superseded||
<!-- Do not remove this comment, lines and table (1-12) -->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0021V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0020V01R01](ADR0020V01R01-DisplayWidthOverCharCountForLayout.md) • [ADR Index](README.md) • **Next:** [ADR0022V01R01 →](ADR0022V01R01-CiSkipsTestsForDocOnlyChanges.md)

---

# ADR0021V01R01 — CI test scope is net10.0-only; the release workflow trusts CI and does not re-run tests

## Context

`ci.yml` originally ran a `[ubuntu, windows, macos] x [net8, net9, net10]` matrix (9
build+test combinations) on every push and PR. This was slow and surfaced flaky failures from
timing-sensitive tests under CI's heavier parallel load. It was first split into a fast job
(ubuntu, net10.0 only, every push/PR) plus a full matrix job (gated to `main`/manual dispatch),
mirroring the "validate on net10.0 only, flag full matrix when needed" approach already used for
local dev validation in this project. That split still left the full-matrix job running the test
suite on all 3 TFMs, only reducing *how often* the 3x3 (later corrected to a straight 3-OS x
net10.0-only) combination ran, not the TFM axis itself. The fast/full split was subsequently
collapsed into a single job once it became clear a single always-on job covering just the OS axis
(net10.0 tests only) was sufficient signal for this project's needs.

Separately, `publish-nuget.yml` (triggered by pushing a `v*.*.*` tag) built the library, ran
`dotnet test` again (all TFMs, no `--framework` filter), then packed and published to NuGet. Since
a tag is only pushed after `ci.yml` has already validated the same commit via a normal push/PR, this
second test run was pure duplication of work already done, adding to release latency without
adding coverage.

## Decision

- `ci.yml`: a single `build-and-test` job, matrix over **3 OSes only** (`ubuntu-latest`,
  `windows-latest`, `macos-latest`), running on every push/PR/`workflow_dispatch`. Its `dotnet test`
  step passes `--framework net10.0`, so only net10.0 is exercised across all 3 OSes (3 combinations,
  not 9). The `Setup .NET`/`Build library (Release)` steps still install and build for all 3 TFMs
  (net8.0/net9.0/net10.0, matching [[ADR0016V01R01]]) — only *test execution* is narrowed, not the
  library's compile-time TFM coverage.
- `publish-nuget.yml`: no longer runs `dotnet test`. The pipeline is Checkout → Setup .NET → Build
  (Release) → Pack → Publish to NuGet.org → Create GitHub Release. It relies entirely on `ci.yml`
  having already validated the exact commit before it was tagged.

## Consequences

- **Positive:** CI wall-clock time and cost drop (3 test combinations instead of 9); the release
  pipeline no longer duplicates a test run that already happened on the same commit via `ci.yml`,
  shortening time-to-publish.
- **Negative / trade-off:** net8.0/net9.0-specific test regressions are no longer caught by CI —
  those TFMs are build-checked (compile must succeed) but never test-run, only net10.0 is. A release
  is only as trustworthy as `ci.yml` having actually run on the tagged commit — tagging a commit that
  bypassed `ci.yml` (e.g. pushed directly without a corresponding push/PR run, or a manually
  rewritten ref) would publish without any test verification at all. This is an accepted trade-off,
  not a gap the release workflow itself now guards against.

