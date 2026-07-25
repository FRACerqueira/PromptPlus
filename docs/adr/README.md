<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Architecture Decision Records (ADR)**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Docs Index](../index.md) • [Back to Home](../../README.md) • **First:** [ADR0001 →](ADR0001V01R01-SplitConsolePlusAndPromptPlus.md)

---

This folder holds the **Architecture Decision Records** for PromptPlus,
covering both product/design decisions and the test-and-build architecture. Each
record captures one architectural decision, including its context, the decision
itself, alternatives, and consequences.

The records follow the [AdrPlus](https://github.com/FRACerqueira/AdrPlus)
convention, **profile 3 — "Product team with frequent revisions"**, which keeps
revision metadata visible and standardized:

```json
{
  "lenseq": 4,
  "lenversion": 2,
  "lenrevision": 2
}
```

## Naming convention

```
ADR{seq:0000}V{version:00}R{revision:00}-DecisionTitle.md
```

- `ADR0001V01R01-...` — created
- `ADR0001V02R01-...` — after a version bump (new decision that supersedes the prior version)
- `ADR0001V03R02-...` — after a revision within a version
- `ADR0002V01R01-DecisionTitle--0001.md` — after a supersede bump

Status labels: **Proposed** → **Accepted** / **Rejected**, and **Superseded**
when a successor version replaces it.

Numbering is **per project**. ADR0001 records the foundational decision to split
PromptPlus 5.x into two projects (ConsolePlus + PromptPlus). The product/design
ADRs (0002–0013) follow from the PromptPlus architecture documentation; the
test/build ADRs (0014–0019) predate that survey and were renumbered to follow.

## Index

### Foundation

| ADR | Title | Version | Status |
| --- | --- | --- | --- |
| [ADR0001V01R01](ADR0001V01R01-SplitConsolePlusAndPromptPlus.md) | Split PromptPlus 5.x into two projects (ConsolePlus + PromptPlus) | V01 | Accepted |

### Product & design decisions

| ADR | Title | Version | Status |
| --- | --- | --- | --- |
| [ADR0002V01R01](ADR0002V01R01-StaticFacadeFourEntryPoints.md) | Static facade with four entry points | V01 | Accepted |
| [ADR0003V01R01](ADR0003V01R01-AutoInitializationOnFirstAccess.md) | Auto-initialization on first access | V01 | Accepted |
| [ADR0004V01R01](ADR0004V01R01-ResultPromptStruct.md) | Result model: `ResultPrompt<T>` readonly struct | V01 | Accepted |
| [ADR0005V01R01](ADR0005V01R01-TwoConfigurationLayers.md) | Two configuration layers (global vs per-control) | V01 | Accepted |
| [ADR0006V01R01](ADR0006V01R01-NoGlobalStyleApi.md) | No global style API; per-control styling | V01 | Accepted |
| [ADR0007V01R01](ADR0007V01R01-ControlsVsWidgetsSeparation.md) | Controls vs Widgets separation | V01 | Accepted |
| [ADR0008V01R01](ADR0008V01R01-ConfirmKeyPressResultModel.md) | Distinct result model for Confirm and KeyPress | V01 | Accepted |
| [ADR0009V01R01](ADR0009V01R01-AsciiFallbackForSymbols.md) | ASCII fallback for visual symbols | V01 | Accepted |
| [ADR0010V01R01](ADR0010V01R01-PredicateSemantics.md) | Predicate semantics for validation and filtering | V01 | Accepted |
| [ADR0011V01R01](ADR0011V01R01-PerControlCultureIsolation.md) | Per-control culture isolation | V01 | Accepted |
| [ADR0012V01R01](ADR0012V01R01-GeneratedApiDocsOffLimits.md) | Generated API docs are off-limits for manual edits | V01 | Accepted |
| [ADR0013V01R01](ADR0013V01R01-MigrationGuideRules.md) | Migration guide maintenance rules (v5 → v6) | V01 | Accepted |
| [ADR0020V01R01](ADR0020V01R01-DisplayWidthOverCharCountForLayout.md) | Display width (columns), not character count, for text layout | V01 | Accepted |

### Test & build architecture

| ADR | Title | Version | Status |
| --- | --- | --- | --- |
| [ADR0014V01R01](ADR0014V01R01-TestDriverSharingMechanism.md) | Test driver sharing mechanism | V01 | Superseded (by V02) |
| [ADR0014V02R01](ADR0014V02R01-TestDriverSharingMechanism.md) | Test driver sharing mechanism | V02 | Accepted |
| [ADR0015V01R01](ADR0015V01R01-TestFrameworkXUnitVerify.md) | Test framework: xUnit + Verify | V01 | Accepted |
| [ADR0016V01R01](ADR0016V01R01-TargetFrameworksMirrorProduction.md) | Target frameworks mirror production (net10/net9/net8) | V01 | Accepted |
| [ADR0017V01R01](ADR0017V01R01-AnsiStyleModelColorOnly.md) | ANSI style model: color only (no attributes) | V01 | Accepted |
| [ADR0018V01R01](ADR0018V01R01-PilotControlsAndPublicApiSurface.md) | Pilot controls and public API surface | V01 | Accepted |
| [ADR0019V01R01](ADR0019V01R01-ConditionalConsolePlusReference.md) | Conditional ConsolePlus reference for tests | V01 | Accepted |
| [ADR0021V01R01](ADR0021V01R01-CiTestScopeNet10OnlyPublishTrustsCi.md) | CI test scope is net10.0-only; release workflow trusts CI and does not re-run tests | V01 | Accepted |
| [ADR0022V01R01](ADR0022V01R01-CiSkipsTestsForDocOnlyChanges.md) | CI skips the test run (build-only) for documentation-only changes | V01 | Accepted |
