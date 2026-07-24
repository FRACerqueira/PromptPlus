# Test driver maintenance (VirtualTerminal) — duplicated-source strategy

> Audience: contributors working on the unit test suites of **ConsolePlus** or **PromptPlus**. Not part
> of the public API docs.

## Context

ConsolePlus and PromptPlus are two independent, separately versioned open-source repositories on
GitHub, each with its own `.git`. They may sit side by side on a contributor's disk for convenience,
but that local layout is not part of either published repo — anything outside `ConsolePlus/` or
`PromptPlus/` (e.g. a shared top-level folder) does not belong to either project and must not be
required to build or test either one in isolation.

Both test suites need an in-memory fake of `IConsole`/`IConsolePlus` — the **VirtualTerminal driver** —
to drive controls and rendering without a real terminal. The driver reaches into ConsolePlus internals
(`ConsoleWriter`) via `InternalsVisibleTo`, so it can't be a plain cross-repo file reference. Each repo
therefore keeps its **own physical copy** of the driver, committed to its own git history.

Decided 2026-07-23, after weighing three options (shared NuGet test-kit package, independent
from-scratch drivers, duplicated source): duplicated source wins for now because the driver is
stable and changes rarely, and it avoids adding a release/versioning cycle for something this small.
Revisit if driver churn increases — see "Rejected alternatives" below.

## Where the driver lives

- `ConsolePlus/tests/_driver-src/*.cs`
- `PromptPlus/tests/_driver-src/*.cs`

These are two **independent copies** of the same files. Nothing links them at build time.

## When you must touch the driver

- Fixing a bug in the ANSI interpreter, the screen grid, or the input queue.
- ConsolePlus adds or changes a method on `IConsole`/`IConsolePlus` and the driver needs a matching
  overload.
- ConsolePlus starts emitting a new SGR/CSI sequence the interpreter doesn't understand yet (rare —
  ConsolePlus does not emit bold/underline SGR today, only foreground/background truecolor).

## Procedure — apply a driver change to BOTH repos

1. Make the fix in one repo's `_driver-src` first; get that repo's own suite green.
2. Copy the changed file(s) verbatim into the other repo's `_driver-src`.
3. Re-run the other repo's suite too. The driver is compiled independently there (different
   `InternalsVisibleTo` grant, different local API surface reachable), so "it compiled on one side"
   does not guarantee it compiles on the other.
4. Commit to both repos. They can't share a commit, so cross-reference by text, e.g. `mirrors
   ConsolePlus@<short-sha>` in the PromptPlus commit message (or vice versa).
5. Never fix only one side "for now." A silent drift between the two copies means the two suites stop
   testing against an equivalent fake, defeating the point of keeping them identical.

## Detecting drift

There is no automated link between the two copies, so drift is only caught by discipline:

- The PR checklist below, for every PR touching `_driver-src` in either repo.
- An occasional manual diff between the two folders (works from a side-by-side checkout like this
  one) — expect zero output. If not empty, agree with the other repo's maintainer(s) on which version
  is authoritative and re-sync both.

## Rejected alternatives (context for future maintainers)

Recorded so nobody re-proposes these without re-deriving the same tradeoff:

- **Shared source across repos (relative path / symlink into a folder outside both repos).** This was
  the original approach and the mistake being corrected by this document: it breaks the instant either
  repo is cloned on its own, since the referenced folder isn't part of that repo.
- **NuGet package (e.g. `ConsolePlus.net.TestKit`) published from the ConsolePlus repo.**
  Architecturally the cleanest option — single source of truth, and better internals encapsulation
  (`InternalsVisibleTo` would target only the TestKit assembly, not `PromptPlus.Tests` directly, unlike
  today). Rejected for now: it adds a release/versioning cycle to every driver fix (tag → publish →
  wait for propagation → bump package reference) and a new public package surface to support. Worth
  revisiting if the driver starts changing frequently.
- **Independent from-scratch drivers per repo.** Avoids duplication bookkeeping, but risks the two
  fakes silently diverging in behavior over time (different bugs, different coverage) — worse than the
  explicit copy-paste discipline described above.

## PR checklist for changes touching the driver

- [ ] Change applied to both `ConsolePlus/tests/_driver-src` and `PromptPlus/tests/_driver-src`
- [ ] Both test suites pass locally
- [ ] Diff between the two `_driver-src` folders is empty
- [ ] If the change reflects a ConsolePlus API change, confirm PromptPlus's reference to ConsolePlus
      (`PromptPlus.csproj`, `ProjectReference` in Debug vs `PackageReference` in Release) resolves a
      version that actually has the new API

---

This file exists identically in both repos (`ConsolePlus/docs/testing-driver-maintenance.md` and
`PromptPlus/docs/testing-driver-maintenance.md`). Keep the two copies of *this document* in sync the
same way you keep the driver itself in sync.
