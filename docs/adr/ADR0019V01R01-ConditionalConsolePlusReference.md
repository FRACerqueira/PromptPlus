<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0019V01R01 |
| Version | 01 |
| Revision | 01 |
| Status | Accepted |
| Created | 2026-07-22 |
| Changed | 2026-07-22 |
| Superseded |  |
-->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0019V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0018V01R01](ADR0018V01R01-PilotControlsAndPublicApiSurface.md) • [ADR Index](README.md) • **Next:** [ADR0020V01R01 →](ADR0020V01R01-DisplayWidthOverCharCountForLayout.md)

---

# ADR0019V01R01 — Conditional ConsolePlus reference for tests

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-22

## Context

`PromptPlus/src/PromptPlus.csproj:69` references ConsolePlus via a
**`PackageReference`** (published NuGet), not a `ProjectReference`. This would
break the planned `InternalsVisibleTo` for `PromptPlus.Tests`, because the
resolved assembly would be the **published package**, not the local build of
`ConsolePlus/src`.

## Decision

Use a **`Configuration`-conditional reference** in
`PromptPlus/src/PromptPlus.csproj`:

- **Release** (normal packaging): `PackageReference` to the published
  `ConsolePlus.net`.
- **Debug** (tests/dev): `ProjectReference` to
  `../../ConsolePlus/src/ConsolePlus.csproj`.

This follows a pattern the `.csproj` **already uses** (the
`ItemGroup Condition="'$(Configuration)' == 'Release' ..."` at line 62 for
`DefaultDocumentation`), so it introduces no new mechanism.

```xml
<ItemGroup Condition="'$(Configuration)' != 'Debug'">
  <PackageReference Include="ConsolePlus.net" Version="0.5.2-Beta" />
</ItemGroup>
<ItemGroup Condition="'$(Configuration)' == 'Debug'">
  <ProjectReference Include="..\..\ConsolePlus\src\ConsolePlus.csproj" />
</ItemGroup>
```

## Consequences

- **Positive:** `dotnet test` runs in `Debug` by default, so `PromptPlus.Tests`
  automatically pulls the local ConsolePlus build, keeping `InternalsVisibleTo`
  working — no explicit `-c` needed. Release packaging is unchanged.
- **Negative / risk:** the `PackageReference` version must be kept current for
  Release; a mismatch between the local Debug build and the published package
  could mask integration issues — mitigated by CI building/testing against the
  local ProjectReference.
