<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Spinners**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Spinner Catalog →](spinners-catalog.md)

---

A **spinner** is a small looping animation that signals "work in progress". Four controls can show
one via a `.Spinner(SpinnersType)` method:

- [ProgressBar](controls/progressbar/index.md)
- [Task](controls/task/index.md)
- [MultiTasks](controls/multitasks/index.md)
- [Timer](controls/timer/index.md)

This page explains how to use spinners and how they behave on terminals without Unicode support.
For the **full visual list of every spinner and its frames**, see the
[**Spinner catalog**](spinners-catalog.md).

---

## Using a spinner

Pass a `SpinnersType` value. Each spinner animates at its own built-in speed — you pick the style,
the library drives the animation.

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls
    .Task("Loading")
    .Spinner(SpinnersType.Dots)     // choose any SpinnersType
    .Action(token => DoWork(token))
    .Run();
```

The same call works on ProgressBar, MultiTasks, and Timer. If you never call `.Spinner(...)`, the
control simply doesn't show one.

---

## Automatic ASCII fallback

> ✅ You can pick **any** spinner without worrying about portability. When the terminal does **not**
> report Unicode support (`PromptPlus.Console.SupportsUnicode == false`), calling `.Spinner(...)`
> automatically substitutes the **`Ascii`** spinner (`-` `\` `|` `/`). On a Unicode-capable terminal
> you get exactly the spinner you asked for.

Details worth knowing:

- The fallback is decided **when you call `.Spinner(...)`**, based on the detected terminal
  capability. It replaces the *whole* spinner (not individual frames).
- The switch is gated on `SupportsUnicode` **only** — it is not a per-glyph check. So on a terminal
  that reports Unicode support but is paired with a font that lacks a specific emoji, an emoji spinner
  (`Moon`, `Clock`, `Hearts`, …) may still render as boxes. If that scenario matters to you, prefer a
  braille/shape/ASCII spinner over an emoji one.

You normally don't need to branch yourself, but the capability is available if you want it:

```csharp
using PromptPlusLibrary;

if (!PromptPlus.Console.SupportsUnicode)
    PromptPlus.Console.WriteLine("[grey]Running in ASCII mode[/]");
```

### Pure-ASCII spinners

These use only plain ASCII characters, so they look identical everywhere (the automatic fallback
picks `Ascii`; the others are here if you want an ASCII look on a capable terminal too):

`Ascii` · `Line` · `SimpleDots` · `SimpleDotsScrolling` · `BouncingBar` · `Star2` · `Balloon` ·
`Binary` · `Dqpb` · `Toggle13`

---

## What to expect visually (quick sample)

A few representative spinners; the frames cycle in order. The **full catalog** with every spinner is
on the [Spinner catalog](spinners-catalog.md) page.

| `SpinnersType` | Frames (cycle) | Kind |
|---|---|---|
| `Ascii` / `Line` | `-` `\` `\|` `/` | ASCII (also the fallback) |
| `SimpleDots` | `.` → `..` → `...` → (blank) | ASCII |
| `BouncingBar` | `[= ]` `[== ]` `[=== ]` `[====]` … | ASCII |
| `Default` | ⣷ ⣯ ⣟ ⡿ ⢿ ⣻ ⣽ ⣾ | Unicode (braille) |
| `Dots` | ⠋ ⠙ ⠹ ⠸ ⠼ ⠴ ⠦ ⠧ ⠇ ⠏ | Unicode (braille) |
| `Arc` | ◜ ◠ ◝ ◞ ◡ ◟ | Unicode (shapes) |
| `Arrow` | ← ↖ ↑ ↗ → ↘ ↓ ↙ | Unicode (arrows) |
| `Moon` | 🌑 🌒 🌓 🌔 🌕 🌖 🌗 🌘 | Emoji |

> 💡 The animation speed is fixed per spinner (for example, `Dots` steps every ~80 ms, `Toggle`
> every ~250 ms). You choose the look; the pace comes with it.

---

## All spinner names

Grouped as in the `SpinnersType` enum. Names in **bold** are pure ASCII. See the frames for each on
the [Spinner catalog](spinners-catalog.md).

**Dots & braille** — `Default` · `Dots` · `Dots2` … `Dots14` · `Dots8Bit` · `DotsCircle` · `Sand`

**Lines & bars** — **`Ascii`** · **`Line`** · `Line2` · `Pipe` · **`SimpleDots`** ·
**`SimpleDotsScrolling`** · `Flip` · **`Binary`** · **`Dqpb`** · **`BouncingBar`** · `Material` ·
`BetaWave` · `Aesthetic` · `Layer` · **`Toggle13`**

**Shapes** — `Star` · **`Star2`** · `Hamburger` · `GrowVertical` · `GrowHorizontal` · **`Balloon`** ·
`Balloon2` · `Noise` · `Bounce` · `BoxBounce` · `BoxBounce2` · `Triangle` · `Arc` · `Circle` ·
`SquareCorners` · `CircleQuarters` · `CircleHalves` · `Squish` · `Point`

**Toggle** — `Toggle` · `Toggle2` … `Toggle12`

**Arrows & motion** — `Arrow` · `Arrow2` · `Arrow3` · `BouncingBall` · `Pong` · `Shark`

**Emoji** — `Smiley` · `Monkey` · `Hearts` · `Clock` · `Earth` · `Moon` · `Runner` · `Weather` ·
`Christmas` · `Grenade` · `FingerDance` · `FistBump` · `SoccerHeader` · `Mindblown` · `Speaker` ·
`OrangePulse` · `BluePulse` · `OrangeBluePulse` · `TimeTravel` · `DwarfFortress`

---

## See also

- [Spinner catalog](spinners-catalog.md) — every spinner with a visual sample of its frames
- [ProgressBar](controls/progressbar/index.md) · [Task](controls/task/index.md) · [MultiTasks](controls/multitasks/index.md) · [Timer](controls/timer/index.md) — the controls that accept a spinner
- [Visual Symbols](visual-symbols.md) — static glyphs and their ASCII fallbacks
- [Global Behaviors](global-behaviors.md) — terminal capability detection
