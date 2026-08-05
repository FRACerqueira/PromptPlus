# ADR Index

This document lists all Architecture Decision Records tracked in this repository.
It is generated automatically by the `AdrIndexer` AdrPlus plugin — do not edit by hand.

## Index

| ADR | Title | Version | Status |
| --- | --- | --- | --- |
| [ADR0001V01R01-SplitConsolePlusAndPromptPlus](ADR0001V01R01-SplitConsolePlusAndPromptPlus.md) | Split PromptPlus 5.x into two projects (ConsolePlus + PromptPlus) | V01 (R01) | Accepted (2026-07-24) |
| [ADR0002V01R01-StaticFacadeFourEntryPoints](ADR0002V01R01-StaticFacadeFourEntryPoints.md) | Static facade with four entry points | V01 (R01) | Accepted (2026-07-24) |
| [ADR0003V01R01-AutoInitializationOnFirstAccess](ADR0003V01R01-AutoInitializationOnFirstAccess.md) | Auto-initialization on first access | V01 (R01) | Accepted (2026-07-24) |
| [ADR0004V01R01-ResultPromptStruct](ADR0004V01R01-ResultPromptStruct.md) | Result model: `ResultPrompt<T>` readonly struct | V01 (R01) | Accepted (2026-07-24) |
| [ADR0005V01R01-TwoConfigurationLayers](ADR0005V01R01-TwoConfigurationLayers.md) | Two configuration layers (global vs per-control) | V01 (R01) | Accepted (2026-07-24) |
| [ADR0006V01R01-NoGlobalStyleApi](ADR0006V01R01-NoGlobalStyleApi.md) | No global style API; per-control styling | V01 (R01) | Accepted (2026-07-24) |
| [ADR0007V01R01-ControlsVsWidgetsSeparation](ADR0007V01R01-ControlsVsWidgetsSeparation.md) | Controls vs Widgets separation | V01 (R01) | Accepted (2026-07-24) |
| [ADR0008V01R01-ConfirmKeyPressResultModel](ADR0008V01R01-ConfirmKeyPressResultModel.md) | Distinct result model for Confirm and KeyPress | V01 (R01) | Accepted (2026-07-24) |
| [ADR0009V01R01-AsciiFallbackForSymbols](ADR0009V01R01-AsciiFallbackForSymbols.md) | ASCII fallback for visual symbols | V01 (R01) | Accepted (2026-07-24) |
| [ADR0010V01R01-PredicateSemantics](ADR0010V01R01-PredicateSemantics.md) | Predicate semantics for validation and filtering | V01 (R01) | Accepted (2026-07-24) |
| [ADR0011V01R01-PerControlCultureIsolation](ADR0011V01R01-PerControlCultureIsolation.md) | Per-control culture isolation | V01 (R01) | Accepted (2026-07-24) |
| [ADR0012V01R01-GeneratedApiDocsOffLimits](ADR0012V01R01-GeneratedApiDocsOffLimits.md) | Generated API docs are off-limits for manual edits | V01 (R01) | Accepted (2026-07-24) |
| [ADR0013V01R01-MigrationGuideRules](ADR0013V01R01-MigrationGuideRules.md) | Migration guide maintenance rules (v5 → v6) | V01 (R01) | Accepted (2026-07-24) |
| [ADR0014V01R01-TestDriverSharingMechanism](ADR0014V01R01-TestDriverSharingMechanism.md) | Test driver sharing mechanism | V01 (R01) | Accepted (2026-07-22) |
| [ADR0015V01R01-TestFrameworkXUnitVerify](ADR0015V01R01-TestFrameworkXUnitVerify.md) | Test framework: xUnit + Verify | V01 (R01) | Accepted (2026-07-22) |
| [ADR0016V01R01-TargetFrameworksMirrorProduction](ADR0016V01R01-TargetFrameworksMirrorProduction.md) | Target frameworks mirror production (net10/net9/net8) | V01 (R01) | Accepted (2026-07-22) |
| [ADR0017V01R01-AnsiStyleModelColorOnly](ADR0017V01R01-AnsiStyleModelColorOnly.md) | ANSI style model: color only (no attributes) | V01 (R01) | Accepted (2026-07-22) |
| [ADR0018V01R01-PilotControlsAndPublicApiSurface](ADR0018V01R01-PilotControlsAndPublicApiSurface.md) | Pilot controls and public API surface | V01 (R01) | Accepted (2026-07-22) |
| [ADR0019V01R01-ConditionalConsolePlusReference](ADR0019V01R01-ConditionalConsolePlusReference.md) | Conditional ConsolePlus reference for tests | V01 (R01) | Accepted (2026-07-22) |
| [ADR0020V01R01-DisplayWidthOverCharCountForLayout](ADR0020V01R01-DisplayWidthOverCharCountForLayout.md) | Display width (columns), not character count, for text layout | V01 (R01) | Accepted (2026-07-24) |
| [ADR0021V01R01-CiTestScopeNet10OnlyPublishTrustsCi](ADR0021V01R01-CiTestScopeNet10OnlyPublishTrustsCi.md) | CI test scope is net10.0-only; the release workflow trusts CI and does not re-run tests | V01 (R01) | Accepted (2026-07-25) |
| [ADR0022V01R01-CiSkipsTestsForDocOnlyChanges](ADR0022V01R01-CiSkipsTestsForDocOnlyChanges.md) | CI skips the test run (build-only) for documentation-only changes | V01 (R01) | Accepted (2026-07-25) |
| [ADR0023V01R02-GuardInteractiveControlsAgainstRedirectedInput](ADR0023V01R02-GuardInteractiveControlsAgainstRedirectedInput.md) | Guard interactive controls against redirected console input in `Run()` | V01 (R02) | Accepted (2026-07-31) |
| [ADR0024V01R01-LiveAnswerLineFollowsTheCursorNotACheckedSummary](ADR0024V01R01-LiveAnswerLineFollowsTheCursorNotACheckedSummary.md) | Live answer line follows the cursor, not a checked summary | V01 (R01) | Accepted (2026-07-30) |
| [ADR0025V01R01-ReadOnlyAnswerViewportSharedBufferAndResizePosition](ADR0025V01R01-ReadOnlyAnswerViewportSharedBufferAndResizePosition.md) | Read-only answer viewport: shared buffer, resize preserves position | V01 (R01) | Accepted (2026-07-30) |
| [ADR0026V01R01-TestDriverSharingMechanism--0014](ADR0026V01R01-TestDriverSharingMechanism--0014.md) | Test driver sharing mechanism | V01 (R01) | Accepted (2026-07-23) |
