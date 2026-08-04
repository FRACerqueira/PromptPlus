# The ConsolePlus dependency

> Audience: contributors building or debugging PromptPlus locally. Not part of the public API docs.

## What

PromptPlus depends on ConsolePlus. `src/PromptPlus.csproj` resolves that dependency with a single,
unconditional `PackageReference` to the published `ConsolePlus.net` NuGet package — the same
reference is used for every configuration (`Debug`, `Release`, `ReleaseDoc`):

```xml
<PackageReference Include="ConsolePlus.net" Version="1.0.0-Beta8" />
```

There is **no** `ProjectReference` to a sibling ConsolePlus checkout, and no configuration-based
split between "packaged" and "local dev" resolution.

## Why this is enough for local development too

A `PackageReference` alone would normally block `tests/PromptPlus.Tests` from reaching ConsolePlus
internals (e.g. `ConsoleWriter`, used by the `VirtualTerminal` test driver — see
[Test Driver Maintenance](testing-driver-maintenance.md)). That access is granted a different way:
ConsolePlus's own published assembly carries

```csharp
[assembly: InternalsVisibleTo("PromptPlus.Tests")]
```

in its `AssemblyInfo.cs`, unconditionally. So the plain NuGet package already exposes what
`PromptPlus.Tests` needs — no `ProjectReference`, no sibling checkout, no build-configuration
gymnastics required.

## What this means for you as a contributor

- **You do not need a `ConsolePlus` sibling checkout to build, debug, or test PromptPlus.** A plain
  `dotnet build src/PromptPlus.csproj` (any configuration) or
  `dotnet test tests/PromptPlus.Tests/PromptPlus.Tests.csproj` resolves everything from the published
  `ConsolePlus.net` package.
- If you need to **step through ConsolePlus source** while debugging an issue that spans both
  libraries, that's a separate, deliberate local choice — not something the build requires. Point
  your debugger/IDE at a local ConsolePlus checkout's symbols, or temporarily swap in a local NuGet
  feed (see [Testing Driver Maintenance](testing-driver-maintenance.md) for the related note on
  keeping the two repos' driver copies in sync when you do cross-repo work).
- When ConsolePlus ships a new API that PromptPlus needs, bump the `Version` on the
  `PackageReference` above — there's no separate Debug-path version to keep in sync.
