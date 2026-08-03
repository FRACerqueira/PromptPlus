<div align="center">
  <img src="../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Spinner Catalog**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../README.md) • **Next:** [Global Styles →](global-styles.md)

---

Every value of `SpinnersType`, with a sample of its animation frames (shown in cycle order). For how
to use spinners and the automatic ASCII fallback, see [Spinners](spinners.md).

> Notes on this page:
> - Long animations are trimmed to a representative slice, ending with `…`.
> - **Bold** names are pure ASCII; the rest need a Unicode-capable terminal (emoji spinners also need
>   an emoji-capable font).
> - On terminals without Unicode support, **any** spinner is automatically replaced by `Ascii`
>   (`-` `\` `|` `/`) — see [Automatic ASCII fallback](spinners.md#automatic-ascii-fallback).
> - How wide glyphs render (emoji, box-drawing) depends on your terminal font; slight misalignment
>   there is a font issue, not a library one.

---

## Dots & braille

- `Default` — `⣷ ⣯ ⣟ ⡿ ⢿ ⣻ ⣽ ⣾`
- `Dots` — `⠋ ⠙ ⠹ ⠸ ⠼ ⠴ ⠦ ⠧ ⠇ ⠏`
- `Dots2` — `⣾ ⣽ ⣻ ⢿ ⡿ ⣟ ⣯ ⣷`
- `Dots3` — `⠋ ⠙ ⠚ ⠞ ⠖ ⠦ ⠴ ⠲ ⠳ ⠓`
- `Dots4` — `⠄ ⠆ ⠇ ⠋ ⠙ ⠸ ⠰ ⠠ …` (grows and shrinks)
- `Dots5` — `⠋ ⠙ ⠚ ⠒ ⠂ ⠂ ⠒ ⠲ ⠴ …`
- `Dots6` — `⠁ ⠉ ⠙ ⠚ ⠒ ⠂ ⠂ ⠒ ⠲ …`
- `Dots7` — `⠈ ⠉ ⠋ ⠓ ⠒ ⠐ ⠐ ⠒ ⠖ …`
- `Dots8` — `⠁ ⠁ ⠉ ⠙ ⠚ ⠒ ⠂ ⠂ …`
- `Dots9` — `⢹ ⢺ ⢼ ⣸ ⣇ ⡧ ⡗ ⡏`
- `Dots10` — `⢄ ⢂ ⢁ ⡁ ⡈ ⡐ ⡠`
- `Dots11` — `⠁ ⠂ ⠄ ⡀ ⢀ ⠠ ⠐ ⠈`
- `Dots12` — `⢀⠀ ⡀⠀ ⠄⠀ ⢂⠀ ⡂⠀ ⠅⠀ …` (two-cell)
- `Dots13` — `⣼ ⣹ ⢻ ⠿ ⡟ ⣏ ⣧ ⣶`
- `Dots14` — `⠉⠉ ⠈⠙ ⠀⠹ ⠀⢸ ⠀⣰ ⢀⣠ …` (two-cell)
- `Dots8Bit` — `⠀ ⠁ ⠂ ⠃ ⠄ ⠅ ⠆ ⠇ …` (all 256 braille cells)
- `DotsCircle` — `⢎  ⠎⠁ ⠊⠑ ⠈⠱  ⡱ ⢀⡰ ⢄⡠ ⢆⡀`
- `Sand` — `⠁ ⠂ ⠄ ⡀ ⡈ ⡐ ⡠ ⣀ ⣁ …` (fills then drains)

---

## Lines & bars

- **`Ascii`** — `-` `\` `|` `/`  *(also the automatic fallback)*
- **`Line`** — `-` `\` `|` `/`
- `Line2` — `⠂ - – — – -`
- `Pipe` — `┤ ┘ ┴ └ ├ ┌ ┬ ┐`
- **`SimpleDots`** — `.` `..` `...` `(blank)`
- **`SimpleDotsScrolling`** — `.` `..` `...` `..` `.` `(blank)`
- `Flip` — `_ _ _ - ` `` ` ` `` ` ' ´ - _`
- **`Binary`** — `010010 001100 100101 111010 …`
- **`Dqpb`** — `d q p b`
- **`BouncingBar`** — `[    ] [=   ] [==  ] [=== ] [====] [ ===] …`
- `Material` — `█▁▁… ██▁… ███▁… …` (wide 20-cell sweep)
- `BetaWave` — `ρββββββ βρβββββ ββρββββ …`
- `Aesthetic` — `▰▱▱▱▱▱▱ ▰▰▱▱▱▱▱ ▰▰▰▱▱▱▱ …`
- `Layer` — `- = ≡`
- **`Toggle13`** — `= * -`

---

## Shapes

- `Star` — `✶ ✸ ✹ ✺ ✹ ✷`
- **`Star2`** — `+ x *`
- `Hamburger` — `☱ ☲ ☴`
- `GrowVertical` — `▁ ▃ ▄ ▅ ▆ ▇ ▆ ▅ ▄ ▃`
- `GrowHorizontal` — `▏ ▎ ▍ ▌ ▋ ▊ ▉ ▊ ▋ ▌ ▍ ▎`
- **`Balloon`** — `(space) . o O @ *`
- `Balloon2` — `. o O ° O o .`
- `Noise` — `▓ ▒ ░`
- `Bounce` — `⠁ ⠂ ⠄ ⠂`
- `BoxBounce` — `▖ ▘ ▝ ▗`
- `BoxBounce2` — `▌ ▀ ▐ ▄`
- `Triangle` — `◢ ◣ ◤ ◥`
- `Arc` — `◜ ◠ ◝ ◞ ◡ ◟`
- `Circle` — `◡ ⊙ ◠`
- `SquareCorners` — `◰ ◳ ◲ ◱`
- `CircleQuarters` — `◴ ◷ ◶ ◵`
- `CircleHalves` — `◐ ◓ ◑ ◒`
- `Squish` — `╫ ╪`
- `Point` — `∙∙∙ ●∙∙ ∙●∙ ∙∙● ∙∙∙`

---

## Toggle

- `Toggle` — `⊶ ⊷`
- `Toggle2` — `▫ ▪`
- `Toggle3` — `□ ■`
- `Toggle4` — `■ □ ▪ ▫`
- `Toggle5` — `▮ ▯`
- `Toggle6` — `ဝ ၀`
- `Toggle7` — `⦾ ⦿`
- `Toggle8` — `◍ ◌`
- `Toggle9` — `◉ ◎`
- `Toggle10` — `㊂ ㊀ ㊁`
- `Toggle11` — `⧇ ⧆`
- `Toggle12` — `☗ ☖`

---

## Arrows & motion

- `Arrow` — `← ↖ ↑ ↗ → ↘ ↓ ↙`
- `Arrow2` — `⬆️ ↗️ ➡️ ↘️ ⬇️ ↙️ ⬅️ ↖️`
- `Arrow3` — `▹▹▹▹▹ ▸▹▹▹▹ ▹▸▹▹▹ ▹▹▸▹▹ …`
- `BouncingBall` — `( ●    ) (  ●   ) (   ●  ) (    ● ) (     ●) …`
- `Pong` — `▐⠂       ▌ ▐⠈       ▌ ▐ ⠂      ▌ …` (wide, ball bounces across)
- `Shark` — `▐|\____________▌ ▐_|\___________▌ …` (wide, fin swims across)

---

## Emoji

Emoji spinners need a modern terminal with an emoji font.

- `Smiley` — `😄 😝`
- `Monkey` — `🙈 🙈 🙉 🙊`
- `Hearts` — `💛 💙 💜 💚 ❤️`
- `Clock` — `🕛 🕐 🕑 🕒 🕓 🕔 … 🕚`
- `Earth` — `🌍 🌎 🌏`
- `Moon` — `🌑 🌒 🌓 🌔 🌕 🌖 🌗 🌘`
- `Runner` — `🚶 🏃`
- `Weather` — `☀️ 🌤 ⛅️ 🌥 ☁️ 🌧 🌨 ⛈ …`
- `Christmas` — `🌲 🎄`
- `Grenade` — `، ′ ´ ‾ ⸌ ⸊ ⁎ ⁕ ⁓ …`
- `FingerDance` — `🤘 🤟 🖖 ✋ 🤚 👆`
- `FistBump` — `🤜　　　　🤛 … 　🤜🤛　　 … 🤜✨🤛` (wide)
- `SoccerHeader` — `🧑⚽️…🧑` (wide, ball headed back and forth)
- `Mindblown` — `😐 😮 😦 😧 🤯 💥 ✨`
- `Speaker` — `🔈 🔉 🔊 🔉`
- `OrangePulse` — `🔸 🔶 🟠 🟠 🔶`
- `BluePulse` — `🔹 🔷 🔵 🔵 🔷`
- `OrangeBluePulse` — `🔸 🔶 🟠 🔹 🔷 🔵 …`
- `TimeTravel` — `🕛 🕚 🕙 🕘 🕗 🕖 … 🕐` (clock running backward)
- `DwarfFortress` — `☺██████£££ …` (wide, a classic marching animation)

---

## See also

- [Spinners](spinners.md) — usage and the automatic ASCII fallback
- [ProgressBar](controls/progressbar/index.md) · [Task](controls/task/index.md) · [MultiTasks](controls/multitasks/index.md) · [Timer](controls/timer/index.md)
