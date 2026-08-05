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
Select value 65                          ← prompt + live answer (same line)
Current selection: 65 %                  ← description (optional / dynamic)
0 [██████████████░░░░░░░░] 100           ← range · bar (LeftRight layout)
Left/Right: change  Enter: confirm  Esc  ← tooltip (toggle with F1 / Ctrl+F1)
```

- The **answer** is written right after the prompt, on the same line — not next to the bar. It
  shows the current value formatted with [`FractionalDigits`](methods.md#fractionaldigits) and
  [`Culture`](methods.md#culture).
- The **range** delimiters (`0` … `100`) and the **min/max display** can be hidden with
  [`HideElements`](methods.md#hideelements).
- The **bar** is drawn on its own line, with the character set from [`BarType`](methods.md#bartype)
  and colored by [`ChangeColor`](methods.md#changecolor) / [`ChangeGradient`](methods.md#changegradient).

Every region can be recolored — see [Styles](styles.md).

---

## Keyboard

The keys that change the value depend on the [`Layout`](methods.md#layout).

### `LeftRight` layout (default — horizontal bar)

| Key | Action |
|---|---|
| `←` / `→` | Decrease / increase by one [`Step`](methods.md#step) |
| `Shift+Tab` / `Tab` | Decrease / increase by one [`LargeStep`](methods.md#largestep) |

### `UpDown` layout (vertical, no bar)

| Key | Action |
|---|---|
| `↓` / `↑` | Decrease / increase by one [`Step`](methods.md#step) |
| `Shift+Tab` / `Tab` | Decrease / increase by one [`LargeStep`](methods.md#largestep) |

### Actions (both layouts)

| Key | Action |
|---|---|
| `Enter` | Confirm — returns the current value |
| `Esc` | Abort (when the abort key is enabled) → `IsAborted == true`, `Content == null` |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

> ⚠️ There is **no** `Home`/`End`-to-min/max binding, and `Page Up`/`Page Down` do **not** trigger
> the large step — `Tab`/`Shift+Tab` do. There is also **no F3 interactive history list** like
> Input/Secret have; `EnableHistory` on Slider is passive — it only pre-loads the last confirmed
> value as the next run's `Default`, nothing more.

---

## How the value steps

- Every change is clamped to the [`Range`](methods.md#range) — you can never move below the minimum
  or above the maximum, but there's no dedicated jump-to-min/max key; repeatedly pressing `Tab`/
  `Shift+Tab` (or the arrow keys) is the only way there.
- The arrow keys apply the small [`Step`](methods.md#step); `Tab`/`Shift+Tab` apply the larger
  [`LargeStep`](methods.md#largestep).
- If you never set them, `Step` defaults to **1/100 of the range** and `LargeStep` to **1/10 of the
  range**. For the default 0..100 range that is `1` and `10`.
- The displayed number is rounded to [`FractionalDigits`](methods.md#fractionaldigits) decimals and
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
- The **horizontal bar and its delimiters are hidden**.
- The `[min,max]` range display is still shown next to the answer by default — hide it too with
  [`HideElements(HideSlider.Range)`](methods.md#hideelements) if you want a bare value.
- It is a compact form for tight screens or when the bar itself is not important.

The default `LeftRight` layout shows the full bar and uses the Left / Right arrows.

---

## History

When [`EnableHistory(filename, …)`](methods.md#enablehistory) is set:

- Each confirmed value is persisted to the on-disk store named `filename`.
- With [`Default(value, useDefaultHistory: true)`](methods.md#default), the last saved value is
  pre-loaded as the starting position instead of `value`.
- `IHistoryOptions` (expiration, max items, and so on) governs what is retained.

Unlike Input/Secret, Slider has **no interactive history list** — there's no F3 hotkey and no
`Pagination`/`Selected`/`UnSelected` history browsing UI. History here is purely passive: it just
remembers the last confirmed value for next time.

You can also manage a history store directly with `PromptPlus.Controls.History(filename)` — add,
save, or remove entries programmatically (used in the samples to seed reproducible data).

---

## Options that change behavior

Set per instance via [`Options(...)`](methods.md#options), or globally on
[`PromptPlus.Config`](../../global-behaviors.md):

| Option | Effect on `Slider` |
|---|---|
| `EnabledAbortKey(false)` | Removes Esc — the user must confirm |
| `HideAfterFinish(true)` | Erases the bar after confirm — the whole control is erased, not just the interactive part |
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
  `InvalidOperationException` when the control runs, not `ArgumentOutOfRangeException`.
- **Precision is display + rounding** — the returned value honors `FractionalDigits`; more decimals are
  not preserved beyond it.
- **Async description callbacks** run while the prompt is open; keep them fast so stepping stays
  responsive.

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Keyboard Bindings](../../keyboard-bindings.md) — full physical-key reference
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`, `SliderWidth`
