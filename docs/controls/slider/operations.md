<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Slider — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Slider — Styles →](styles.md)

---

How the `Slider` control behaves while it is running: keyboard, how the value steps, ranges and
precision, colors and gradients, layout, history, and the details that matter in real apps.

---

## Anatomy of the control

```
Select value                             ← prompt
Current selection: 65 %                  ← description (optional / dynamic)
0 [██████████████░░░░░░░░] 100   65      ← range · bar · answer
Left/Right: change  Enter: confirm  Esc  ← tooltip (toggle with F1 / Ctrl+F1)
```

- The **range** delimiters (`0` … `100`) and the **min/max display** can be hidden with
  [`HideElements`](methods.md#hideelements).
- The **bar** is drawn with the character set from [`BarType`](methods.md#bartype) and colored by
  [`ChangeColor`](methods.md#changecolor) / [`ChangeGradient`](methods.md#changegradient).
- The **answer** shows the current value formatted with [`FractionalDigits`](methods.md#fractionalDigits)
  and [`Culture`](methods.md#culture).

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

The keys that change the value depend on the [`Layout`](methods.md#layout).

### `LeftRight` layout (default — horizontal bar)

| Key | Action |
|---|---|
| `←` / `→` | Decrease / increase by one [`Step`](methods.md#step) |
| `Page Down` / `Page Up` | Decrease / increase by one [`LargeStep`](methods.md#largestep) |
| `Home` | Jump to the minimum value |
| `End` | Jump to the maximum value |

### `UpDown` layout (vertical, no bar)

| Key | Action |
|---|---|
| `↓` / `↑` | Decrease / increase by one [`Step`](methods.md#step) |
| `Page Down` / `Page Up` | Decrease / increase by one [`LargeStep`](methods.md#largestep) |
| `Home` | Jump to the minimum value |
| `End` | Jump to the maximum value |

### Actions (both layouts)

| Key | Action |
|---|---|
| `Enter` | Confirm — returns the current value |
| `Esc` | Abort (when the abort key is enabled) → `IsAborted == true`, `Content == null` |
| `F3` | Open history navigation (when [history](methods.md#enabledhistory) is enabled) |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

---

## How the value steps

- Every change is clamped to the [`Range`](methods.md#range). You can never move below the minimum
  or above the maximum, so `Home`/`End` are shortcuts to the two ends.
- The arrow keys apply the small [`Step`](methods.md#step); Page Up / Page Down apply the larger
  [`LargeStep`](methods.md#largestep).
- If you never set them, `Step` defaults to **1/100 of the range** and `LargeStep` to **1/10 of the
  range**. For the default 0..100 range that is `1` and `10`.
- The displayed number is rounded to [`FractionalDigits`](methods.md#fractionalDigits) decimals and
  formatted with the active [`Culture`](methods.md#culture) (so `pt-BR` shows a comma separator).

> 💡 Choose a `Step` that is a meaningful increment for your domain — for example `5` for a
> percentage picked in fives — so keyboard navigation feels natural.

---

## Colors & gradients

Two independent ways to color the bar:

- [`ChangeColor(Func<double, Style>)`](methods.md#changecolor) recomputes the bar `Style` from the
  current value — ideal for thresholds (red below 30, gold above 70).
- [`ChangeGradient(params Color[])`](methods.md#changegradient) blends the supplied colors smoothly
  across the bar as the value grows.

Both react as the value changes. Use one or the other per control.

---

## Layout differences

`Layout(SliderLayout.UpDown)` is more than a rotation:

- Navigation moves to the **Up / Down** arrows.
- The **horizontal bar is hidden**, and the delimiter/range widgets are not drawn.
- It is a compact form for tight screens or when the bar itself is not important.

The default `LeftRight` layout shows the full bar and uses the Left / Right arrows.

---

## History

When [`EnabledHistory(filename, …)`](methods.md#enabledhistory) is set:

- Each confirmed value is persisted to the on-disk store named `filename`.
- With [`Default(value, useDefaultHistory: true)`](methods.md#default), the last saved value is
  pre-loaded as the starting position instead of `value`.
- Pressing **F3** opens history navigation; the [`Pagination`](styles.md), [`Selected`](styles.md),
  and [`UnSelected`](styles.md) style regions paint that list.
- `IHistoryOptions` (expiration, max items, and so on) governs what is retained.

You can also manage a history store directly with `PromptPlus.Controls.History(filename)` — add,
save, or remove entries programmatically (used in the samples to seed reproducible data).

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Slider` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the bar after confirm; only the answer line remains |
| `HideOnAbort(true)` | Erases the bar after Esc |
| `ShowTooltip(false)` | Hides the keyboard hint line |
| `Description(...)` | Overrides the description text |

> The global `PromptPlus.Config.SliderWidth` sets the default bar width; override it per control with
> [`Width`](methods.md#width).

---

## Edge cases & gotchas

- **Aborted results carry `Content == null`**, not the seeded `Default`. Always branch on
  `IsAborted` before reading the value.
- **`Default` must sit inside the range** — a value outside min/max throws
  `ArgumentOutOfRangeException` when the control is built.
- **Precision is display + rounding** — the returned value honors `FractionalDigits`; more decimals are
  not preserved beyond it.
- **Async description callbacks** run while the prompt is open; keep them fast so stepping stays
  responsive.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`, `SliderWidth`
