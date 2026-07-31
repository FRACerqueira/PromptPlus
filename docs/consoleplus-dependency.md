# The ConsolePlus dependency — Debug vs Release

> Audience: contributors building or debugging PromptPlus locally. Not part of the public API docs.

## What

PromptPlus depends on ConsolePlus. `src/PromptPlus.csproj` resolves that dependency differently
depending on the build configuration (decision "D6", 2026-07-22):

- **Release** — the configuration that gets packaged and published to NuGet — uses a
  `PackageReference` to the published `ConsolePlus.net` package.
- **Debug** — the default configuration for local development — uses a `ProjectReference` to
  `../../ConsolePlus/src/ConsolePlus.csproj`, i.e. a **sibling checkout of the ConsolePlus repo**,
  cloned next to `PromptPlus` on disk.

```
some-folder/
├── ConsolePlus/     <- separate GitHub repo, own .git
└── PromptPlus/      <- separate GitHub repo, own .git — this is where you are
```

## Why

- What ships to users (Release) must build against a stable, versioned, published dependency —
  never against whatever happens to be on a contributor's disk.
- Local development (Debug) benefits from two things a NuGet reference can't give you:
  1. Step-through debugging into ConsolePlus source when tracking down an issue that spans both
     libraries.
  2. `InternalsVisibleTo` access — the `VirtualTerminal` test driver (see
     [Test Driver Maintenance](testing-driver-maintenance.md)) needs to reach ConsolePlus internals
     (e.g. `ConsoleWriter`), which the published package does not expose.

## What this means for you as a contributor

- **To build or debug PromptPlus in Debug config (the default when you run `dotnet build` with no
  `-c` flag), you need the `ConsolePlus` repo cloned as a sibling folder next to `PromptPlus`**, as
  shown above. Without it, `dotnet build src/PromptPlus.csproj` fails with
  `CS0246: The type or namespace name 'ConsolePlusLibrary' could not be found` — that's this, not a
  broken checkout.
- If you only have `PromptPlus` cloned on its own, build in **Release** instead:
  ```
  dotnet build src/PromptPlus.csproj --configuration Release
  ```
  That resolves against the published NuGet package and needs nothing else on disk.
- **Running the test suite always needs Debug** (`tests/PromptPlus.Tests`), regardless of which
  configuration you're packaging for — the driver's `InternalsVisibleTo` access only exists on that
  path. This is also why CI builds the library in Release and runs the tests in Debug as two separate
  steps (`.github/workflows/ci.yml`), instead of one blind build+test pass.
- If `dotnet restore`/`dotnet build`/`dotnet test` are run as separate commands, make sure the
  `Configuration` you pass is consistent across them (e.g. don't `restore` with no config and then
  `build --configuration Release --no-restore` — restore defaults to Debug and won't fetch the
  Release-only `ConsolePlus.net` package, causing the same `CS0246` error above for a different
  reason). Simplest: let `build`/`test` do their own implicit restore rather than splitting the steps.
- Don't "fix" this by making Debug use the NuGet package or Release use the `ProjectReference` — the
  split is deliberate, not an oversight.
