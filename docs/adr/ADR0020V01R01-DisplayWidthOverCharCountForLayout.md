<!-- Do not remove this comment, lines and table -->
<!--
| Fields | Values |
| --- | --- |
| ADR | ADR0020V01R01 |
| Version | 01 |
| Revision | 01 |
| Status | Accepted |
| Created | 2026-07-24 |
| Changed | 2026-07-24 |
| Superseded |  |
-->

<div align="center">
  <img src="../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ADR0020V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← ADR0019V01R01](ADR0019V01R01-ConditionalConsolePlusReference.md) • [ADR Index](README.md) • **Next:** [ADR0021V01R01 →](ADR0021V01R01-CiTestScopeNet10OnlyPublishTrustsCi.md)

---

# ADR0020V01R01 — Display width (columns), not character count, for text layout

- **Status:** Accepted
- **Version:** V01 / Revision R01
- **Created:** 2026-07-24

## Context

PromptPlus ships resources for 11 cultures (`en-us, pt-br, de-de, es-es, fr-fr, it-it, ja-jp, ko-kr,
nl-be, ru-ru, zh-cn`). In `ja-jp`, `ko-kr` and `zh-cn`, and in any user-supplied text that happens to
contain CJK characters regardless of the active culture, a single character can be a **wide rune**:
it occupies 2 columns on the terminal but counts as 1 in `string.Length`.

`ConsolePlusLibrary` (the layer PromptPlus is built on) already exposes `string.GetDisplayLength()`,
which measures text width in actual terminal columns, wide runes counted as 2. It did **not**,
before this decision, expose a matching truncation primitive — code that needed to cut text down to a
column budget had no correct primitive to call.

An audit (2026-07-24) found that several places across ConsolePlus and PromptPlus used
`string.Length`, `PadRight`/`PadLeft`, or character-index slicing (`text[..n]`) as a stand-in for
display width. The most severe instance was a genuine production bug, not just a cosmetic one:
`ConsoleWriter.WriteOutput` (ConsolePlus) computed an overflow budget in **columns** via
`GetDisplayLength()`, then applied that column count as a **character index** to `string.Substring`.
Reproduced with a test: an 8-character Korean string (16 display columns) written into a 10-column
terminal under `Overflow.Crop` or `Overflow.Ellipsis` threw `ArgumentOutOfRangeException`. This path
is reached by every control, because `BaseControlPrompt.WriteLineSegments` forces
`Overflow.Ellipsis` on every rendered line.

Beyond that shared root cause, several individual controls independently perform their own layout
math (column-width computation, padding, truncation) using `.Length` on text that can legitimately
contain wide runes: table/column headers and cell values (`TableControl`, `MultiTableControl`),
chart titles and item labels (`ChartBarControl`), calendar month/weekday names formatted through
`CultureInfo` (`CalendarControl`), separator-line width for grouped/separated list items
(`SelectControl`, `MultiSelectControl`), and secret-field masking after an already-correct
column-aware viewport slice (`InputControl`). Each was tracked item-by-item in a working rollout
plan (since completed and removed).

## Decision

1. **Display width (terminal columns), not `string.Length`, is the canonical unit for any layout
   decision that determines how many columns a piece of text occupies on screen** — column-width
   calculation, padding, truncation, alignment/centering, and cursor/scroll positioning. This applies
   whenever the text involved is user-supplied, localized, or otherwise not guaranteed to be
   ASCII-only (item labels, titles, table/tree/list cell values including a generic `T.ToString()`,
   culture-formatted names, tooltips). It does **not** apply to fixed ASCII content such as
   zero-padded numeric/date components (`PadLeft(2,'0')` for a day/hour) or to counts over
   non-string collections — those stay as they are.

2. **`ConsolePlusLibrary.StringExtensions` is the single source of truth for this**, exposing both
   halves of the primitive pair:
   - `GetDisplayLength()` — measure (already existed).
   - `TruncateToDisplayWidth(maxWidth)` — cut to a column budget without splitting a wide rune in
     half (added by this decision).

   Control code must use these instead of hand-rolling rune-width logic or reusing `.Length as if it
   were column count. `BaseControlPrompt`'s existing rune-trim helpers (used for the text-input
   viewport) remain as-is; they solve the same problem for a case `TruncateToDisplayWidth` doesn't
   cover (bidirectional trim around a cursor position) and are not being replaced.

3. **The shared rendering path (`ConsoleWriter.WriteOutput`'s `Crop`/`Ellipsis` handling) is fixed to
   use `TruncateToDisplayWidth`** instead of a raw character-index `Substring`, since it is reached
   by every control and the bug there was an outright crash, not just misalignment.

4. **Per-control fixes are tracked, not bundled into this ADR.** Each remaining instance (Table/
   MultiTable, ChartBar, Calendar, Select/MultiSelect, Input secret masking) is a separate, boundable
   change with its own regression test, tracked in a working rollout plan (since completed and
   removed). One of them (`ChartBar`'s
   `MaxLengthLabel`) has a public-API semantic question (character count vs. display columns) that
   needs an explicit answer before it's touched, precisely because it's observable API behavior, not
   an internal-only fix.

## Consequences

- **Positive:** CJK text (in any of the three CJK-scripted supported cultures, or simply typed by a
  user regardless of active culture) renders and truncates correctly wherever the fix is applied,
  starting with the crash-level shared bug. Future control code has one documented, tested pair of
  primitives to reach for instead of re-deriving rune-width logic per control.
- **Positive:** the fix is incremental and independently testable per control — no single large,
  risky change.
- **Negative / risk:** `TruncateToDisplayWidth`, when applied, can produce a string whose *character*
  length differs from what `.Length`-based logic previously assumed; anywhere a fix changes truncation
  behavior for content that was already CJK (rare in practice, since the prior behavior was broken),
  visual output for that content changes. Mitigated by keeping each control's fix scoped and covered
  by a regression test that pins the new (correct) behavior.
- **Resolved:** `ChartBarControl.MaxLengthLabel(byte)` keeps its documented character-count contract
  unchanged — it still caps how many symbols/runes of a label are retained, CJK or not. What was
  fixed is internal: retention (how much of the label to keep) and alignment (how many columns to pad
  to) are now two separate computations. The label is truncated to at most `MaxLengthLabel` **runes**
  (not raw UTF-16 units, so a supplementary-plane CJK surrogate pair is never split), then padding is
  computed from the *actual display width* of that truncated text across all items, not from the
  character cap itself. This keeps the public API's observable behavior identical while making
  cross-item column alignment correct regardless of script mix. The exact fields/methods touched
  are recorded in the fix's own commit and regression tests.
