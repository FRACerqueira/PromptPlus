<!-- Do not remove this comment, lines and table (1-12) -->
|Adr-Plus Fields|Values Migrated <!-- Migrated -->|
|--|--|
|ADR|Live answer line follows the cursor, not a checked summary|
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

  ## **ADR0024V01R01**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[↑ ADR Index](indexadrs.md)

---

# ADR0024V01R01 — Live answer line follows the cursor, not a checked summary

## Context

Every control with a live/final answer distinction has two independent rendering paths: `WriteAnswer`
(the live answer, redrawn every frame while the control is running) and `FinishTemplate` /
`BuildCheckedItemsText` / equivalent (the final answer, written once after Enter/confirm). Before this
decision, what the live path showed was inconsistent across controls:

- `TableControl` (single-choice) already showed the row/column currently under the cursor, resolved
  through a `GetAnswerText(item)` helper that reads `TextSelector` when configured, otherwise the
  currently Tab-focused column's cell value. `MultiFileControl` and `MultiTreeControl` also already
  followed the cursor since their inception.
- `MultiSelectControl` and `MultiTableControl`, by contrast, rebuilt the live answer as a
  comma-joined summary of every currently **checked** item on each check/uncheck, independent of
  where the cursor was.
- Separately, `ExtraInfo`/`ExtraInfoAsync` (`SelectControl`, `MultiSelectControl`, `TreeControl`,
  `MultiTreeControl`) and the byte-size suffix (`FileControl`, `MultiFileControl`) were rendered only
  in the list row, never in the answer. The list row has no horizontal scroll: when it overflows the
  console width, the cut-off text is unrecoverable. The answer line, in every one of these controls,
  already supports horizontal scrolling for exactly this kind of overflow (see
  [ADR0025](ADR0025V01R01-ReadOnlyAnswerViewportSharedBufferAndResizePosition.md) for how that
  scrolling itself behaves).

This decision was implemented across two rounds: `MultiSelectControl`'s live answer plus `ExtraInfo`
on `SelectControl`/`MultiSelectControl` first (commit `2016a06`), then `MultiTableControl`'s live
answer plus `ExtraInfo` on `TreeControl`/`MultiTreeControl` and the size suffix on
`FileControl`/`MultiFileControl` in a follow-up round covering every other control with the same
shape.

## Decision

1. **The live answer always reflects the item/row currently under the navigation cursor**, never an
   aggregated summary of checked/selected items. This applies uniformly to `Select`, `MultiSelect`,
   `Table`, `MultiTable`, `File`, `MultiFile`, `Tree`, and `MultiTree`. `Table` is the reference
   implementation this decision generalizes, not a new pattern.
2. **The final answer keeps its own existing semantics and is unaffected by (1).** For Multi*
   controls it stays a plain, comma-joined summary of checked items; for single-choice controls it
   stays the plain selected value. It never includes `ExtraInfo`, the size suffix, or the live
   cursor's per-column value.
3. **`ExtraInfo`/`ExtraInfoAsync` and the file-size suffix are appended to the live answer only**,
   never to the final answer — because the live answer line can recover overflowed text via
   horizontal scroll, and the final answer is meant to stay a clean, plain summary.
4. **Formatting of the appended text follows each control's own existing convention**, not a unified
   one: `Select`/`MultiSelect` use the configurable `PrefixExtraInfoValue`/`SuffixExtraInfoValue`
   (default `" ("`/`")"`); `Tree`/`MultiTree` use a fixed two-space separator with no parens;
   `File`/`MultiFile` use a fixed two-space separator before the formatted byte size. Unifying these
   formats was explicitly out of scope.
5. **`MultiTable`'s per-column complexity (Tab/Shift+Tab changes which column the live answer
   reflects) needed no new logic** — the existing `GetAnswerText(item)` helper, already proven by
   `Table` in production, was reused as-is by pointing it at the single item under the cursor instead
   of joining every checked row.

## Consequences

- **Positive:** one consistent mental model across every control with a live/final answer split — the
  answer shows what you are looking at right now; only Enter commits a summary.
- **Positive:** users navigating a long list with `ExtraInfo` or a file size can always recover
  overflowed text via the answer line's horizontal scroll, where before it was only visible if the
  list row happened to fit the console width.
- **Negative / trade-off:** `MultiSelect`/`MultiTable`'s live answer no longer shows a running count
  of checked items while navigating; that information is carried by the existing pagination/footer
  count (`Checked: N`), not duplicated here.
- **Negative / trade-off (documented, not fixed):** `Tree`/`MultiTree`'s `GetExtraInfoText` has no
  per-node cache, unlike `Select`/`MultiSelect`'s `ItemSelect<T>.ExtraText`. This decision adds one
  extra delegate call per frame for the focused node, on top of the already-uncached per-frame cost
  the list row already paid for every visible node. A caching improvement is a separate, unrequested
  change and was not made here.
- **Resolved:** a `MultiSelect` group header under the cursor shows the group name in the live
  answer — this is not a regression of the group-header case, it is the same "follow the cursor" rule
  applied to a header row instead of an item row.

