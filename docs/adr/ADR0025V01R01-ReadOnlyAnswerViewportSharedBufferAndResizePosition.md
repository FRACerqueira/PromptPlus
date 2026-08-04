<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Read-only answer viewport: shared buffer, resize preserves position|
|Version|01|
|Revision|01|
|Scope||
|Domain||
|Created|Proposed (2026-07-30)|
|Changed|Accepted (2026-07-30)|
|Superseded||
<!-- Do not remove this comment, lines and table (1-12) -->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0025V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0024V01R01](ADR0024V01R01-LiveAnswerLineFollowsTheCursorNotACheckedSummary.md) • [ADR Index](README.md) • **Next:** [ADR0026V01R01 →](ADR0026V01R01-TestDriverSharingMechanism--0014.md)

---

# ADR0025V01R01 — Read-only answer viewport: shared buffer, resize preserves position

## Context

Two independent implementations of the same guarantee — a read-only answer line that scrolls
horizontally when its text overflows the console width — coexisted in the codebase:

- **Manual pattern:** each control owns its own `EmacsConsoleBuffer` field, a `_updatePosAnswerBuffer`
  flag that the key loop threads through resize/cancel events so a resize does not clobber an
  in-progress scroll, and hand-rolled `ViewportSlice` rendering. Used by `Select`, `MultiSelect`,
  `Table`, `MultiTable`, `Input`.
- **Shared pattern:** `BaseControlPrompt.WriteAnswerViewport` / `TryAnswerViewportNavigation`, where
  the base class owns a single internal buffer and both rendering and key handling read/write that
  same instance. Used by `File`, `MultiFile`, `MultiTree`, and, after this decision, `Tree`.

A real bug was found in `TreeControl`: it rendered its live answer from its own manually-owned
`_answerBuffer` via `ViewportSlice`, but its key loop forwarded the answer-scroll keys
(`Home`/`End`/`←`/`→`) to `TryAnswerViewportNavigation`, which moves the base class's separate
`_answerViewportBuffer`. Because `TreeControl` never called `WriteAnswerViewport`, that buffer was
never initialized, so the navigation call always returned `false` and those keys did nothing — a
mismatched hybrid of both patterns, not a deliberate design, confirmed by tracing
`BaseControlPrompt.TryAnswerViewportNavigation`'s null-buffer guard.

Fixing that bug by moving `TreeControl` fully onto the shared `WriteAnswerViewport` pattern surfaced
a second, unrelated gap: `WriteAnswerViewport` re-anchored the scroll to Home on any terminal resize,
whereas the manual pattern (still used by `Select`/`MultiSelect`/`Table`/`MultiTable`/`Input`)
deliberately preserves the scroll position across a resize — a past, tested fix (confirmed present in
those five controls, and, per a pre-existing test comment, also present in `TreeControl` before this
session by direct analogy to `Select`). `TreeControl` would have silently lost that guarantee as a
side effect of the buffer-mismatch fix. `MultiTreeControl` never had it — it always used the shared
pattern, which offered no way to opt out of the re-anchor.

An opt-in `preservePositionOnResize` parameter on `WriteAnswerViewport` was implemented first, to
extend the guarantee to `Tree`/`MultiTree` without touching `File`/`MultiFile`. Once the follow-up
question — should `File`/`MultiFile` also preserve position on resize, for full consistency with the
other six controls — was answered yes, every caller of `WriteAnswerViewport` wanted the same
behavior, making the opt-in dead weight; it was removed in favor of making the guarantee
unconditional.

## Decision

1. **`BaseControlPrompt.WriteAnswerViewport` / `TryAnswerViewportNavigation` is the canonical
   mechanism for any control whose answer line is read-only and needs horizontal scrolling.** New
   controls with this need should use it rather than hand-rolling a manual buffer and wiring the key
   loop to it separately — the shared version couples rendering and key handling to the same buffer
   instance by construction, which rules out the class of bug found in `TreeControl`.
2. **A terminal resize never discards the user's horizontal scroll position on a read-only answer
   line.** `WriteAnswerViewport` guarantees this unconditionally for all of its callers (`File`,
   `MultiFile`, `Tree`, `MultiTree`), matching the resize behavior already present in the five
   controls that still use the manual pattern (`Select`, `MultiSelect`, `Table`, `MultiTable`,
   `Input`).
3. **Migrating `Select`/`MultiSelect`/`Table`/`MultiTable`/`Input` off the manual pattern is out of
   scope for this decision.** Both patterns deliver the same resize guarantee today, so there is no
   user-visible inconsistency — only an internal-implementation one. A future migration, if pursued,
   is a separate, boundable change per control, tracked individually rather than bundled here (same
   convention as [ADR0020](ADR0020V01R01-DisplayWidthOverCharCountForLayout.md)'s per-control
   follow-ups).

## Consequences

- **Positive:** the resize/scroll bug class found in `TreeControl` cannot recur in any control using
  the shared pattern, since rendering and key handling always agree on the same buffer instance.
- **Positive:** `File`/`MultiFile` gain resize-position preservation they never had, closing the only
  user-visible gap between the two patterns' resize behavior.
- **Negative / trade-off:** two implementations of the same guarantee still coexist in the codebase.
  A reader of `Select`'s or `Table`'s render code will not find `WriteAnswerViewport` there and needs
  the `_updatePosAnswerBuffer` dance explained separately from this ADR's mechanism.
- **Resolved:** `TreeControl`'s now-dead manual answer-rendering state (`_answerBuffer` used only for
  rendering, `_updatePosAnswerBuffer`) was removed as part of this decision. Its one remaining,
  unrelated use of `_answerBuffer` — checking whether a typed character should switch the control
  into filter mode — was repointed to `_filterBuffer`, since `EmacsConsoleBuffer.IsPrintable` does not
  depend on instance state (confirmed via its `CA1822`-suppressed, "ByDesign" attribute); no buffer
  was left dangling.

