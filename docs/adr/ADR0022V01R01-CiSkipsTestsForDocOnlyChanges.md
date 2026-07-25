<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0022V01R01 |
| Version | 01 |
| Revision | 01 |
| Status | Accepted |
| Created | 2026-07-25 |
| Changed | 2026-07-25 |
| Superseded |  |
-->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0022V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0021V01R01](ADR0021V01R01-CiTestScopeNet10OnlyPublishTrustsCi.md) • [ADR Index](README.md)

---

# ADR0022V01R01 — CI skips the test run (build-only) for documentation-only changes

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-25

## Context

`ci.yml` (see [ADR0021V01R01](ADR0021V01R01-CiTestScopeNet10OnlyPublishTrustsCi.md)) runs the same
build + net10.0 test pass for every push and PR, regardless of what changed. A large share of this
project's commits only touch `docs/**` or a `*.md` file (the per-control docs pass, the v5→v6
migration guide, ADRs) and never touch `src/`, `tests/`, `samples/`, or the workflow files
themselves. Running the full 3-OS test matrix for a documentation wording fix adds several minutes
of CI time and consumes runner minutes for zero additional signal — a doc-only change cannot alter
runtime behavior the test suite would catch.

## Decision

Add a `changes` job to `ci.yml` that uses `dorny/paths-filter` to classify the push/PR diff: a
`code` output is `true` if any changed file is **not** under `docs/**` and is **not** a `*.md` file
anywhere in the repo (this also means changes to `.github/workflows/*.yml` itself, `*.csproj`
files, `src/`, `tests/`, and `samples/` all count as `code` — only `docs/**` and stray `*.md` files
are treated as documentation).

The `build-and-test` job always still runs `Build library (Release)` regardless of `code`'s value —
every change gets at least a compile check. The `Test (net10.0 only)` step, and the two
`Upload ... results` steps that depend on it, are gated on `needs.changes.outputs.code == 'true'`
and are skipped entirely for doc-only changes.

`workflow_dispatch` runs (no diff to compare against) and any case `paths-filter` can't resolve a
base ref for fall back to `code: true` — a manual run is assumed to want full validation.

## Consequences

- **Positive:** documentation-only pushes/PRs finish CI in roughly the time of a `dotnet build`
  instead of a full `dotnet build` + `dotnet test` matrix, on all 3 OSes — meaningfully faster
  feedback for the doc-heavy parts of this project's workflow, with no loss of real test coverage
  (doc changes can't affect runtime behavior).
- **Negative / trade-off:** a change that is *miscategorized* as doc-only (e.g. a commit that
  touches `docs/**` and *also* silently depends on an untracked side effect from a code change made
  in an earlier, separate commit) would not be re-verified by that specific CI run — this is an
  accepted risk given how independent doc commits are from the codebase in practice. The `changes`
  job itself adds one small `ubuntu-latest` checkout+filter step to every run's critical path
  (typically a few seconds), which is negligible next to the multi-minute test job it can now skip.
