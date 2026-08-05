<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MaskEdit — Operations**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MaskEdit — Styles →](styles.md)

---

How the MaskEdit controls behave while running: the mask grammar, the placeholder, cursor modes, the
keyboard, confirmation and validation, and how culture shapes numbers and dates.

---

## Anatomy of the control

```
Phone: (12_) ___-____                    ← prompt + mask with typed digits + placeholder
Format: 3 letters, dash, 4 digits        ← description (optional)
Numeric input                            ← input-type hint (hidden by HideTipInputType)
must not start with '0'                  ← error line, only when validation fails
Enter: confirm  Esc: cancel              ← tooltip
```

Every region can be recolored — see [Styles](styles.md).

---

## Mask tokens

These apply to the **string** control ([`Mask`](methods.md#mask)). Any character that is not a token
is drawn as a literal that the user cannot edit.

| Token | Accepts |
|---|---|
| `9` | A numeric digit (0–9) |
| `L` | A lowercase letter |
| `U` | An uppercase letter |
| `A` | Any letter (upper or lower) — **not** digits |
| `X` | A letter or a digit |
| `C` | A custom character (only what a `[ ]` list allows) |
| `\` | Escape — the **next** character is treated as a literal |
| `{ }` | Group delimiters — apply a custom list or constant to a single mask type inside |
| `[ ]` | Delimiters for a custom character list |
| `( )` | Delimiters for a constant value inside a group |
| anything else | A literal character, shown but not editable |

```csharp
// Escaped literal '#', a digit restricted to odd digits, then a constant 'A'
PromptPlus.Controls.MaskEdit("Code")
    .Mask(@"\#C[13579]\AT")
    .Run();
```

Common patterns:

| Goal | Mask |
|---|---|
| US phone `(123) 456-7890` | `\(999\)\ 999\-9999` |
| License plate `ABC-1234` | `UUU-9999` |
| ZIP+4 `99999-999` | `99999-999` |
| Serial `AAAA-AAAA` | `AAAA-AAAA` |

> The **number**, **currency**, and **date/time** controls do **not** take a mask string. Their masks
> are generated: [`NumberFormat`](methods.md#numberformat-number) for numbers/currency, and the active
> [culture](#culture) for dates.

---

## Placeholder

Every control shows a placeholder character in unfilled positions, set with
[`PromptMask`](methods.md#promptmask) (default `'_'`).

```csharp
PromptPlus.Controls.MaskInteger("Counter")
    .NumberFormat(4, withseparatorgroup: false)
    .PromptMask('0')          // empty positions read as 0000
    .Run();
```

---

## Input modes

[`InputMode`](methods.md#inputmode) (string and date/time controls) sets how the cursor travels:

| `InputBehavior` | Behavior |
|---|---|
| `EditSkipToInput` (default) | The cursor auto-skips literals/separators and lands on the next editable position, so typing flows straight through the pattern. |
| `EditCursorFreely` | The cursor advances one position at a time and can rest on literals — useful when you want to edit a single segment in place. |

---

## Keyboard

### Editing

| Key | Action |
|---|---|
| Digit / letter (per token) | Fill the current editable position, then advance |
| `←` / `→` | Move between positions (skipping literals in `EditSkipToInput`) |
| `Tab` / `Shift+Tab` | Jump to the next / previous mask **delimiter** (date/time masks only) |
| `Home` / `End` | Jump to the first / last position |
| `Backspace` | Clear the previous editable position |
| `Delete` | Clear the position at the cursor |
| `Insert` | Toggle insert / overwrite mode |

For the **date/time** control, `←`/`→` move **one digit position at a time** within the current
field (day/month/year/hour/minute/second) — they only cross into the next/previous field once the
current one is exhausted. To jump directly between whole fields, use `Tab`/`Shift+Tab` instead.

> If `PromptPlus.Console.EnabledEmacs` is `true`, `MaskEdit` also accepts the position/delete subset
> of Emacs keys — `Ctrl+A/E/B/F/D/H/L` — but not word motion, word deletion, yank, transpose, or
> case toggling. See [Keyboard Bindings](../../keyboard-bindings.md) for the full table.

### Actions

| Key | Action |
|---|---|
| `Enter` | Confirm — runs validation, then returns the parsed value |
| `Esc` | Abort (when the abort key is enabled) → `IsAborted == true` |
| `+` / `-` | Set the sign, when `withsignal: true` on a number/currency mask |
| `F1` | Cycle tooltip content |
| `Ctrl+F1` | Show / hide the tooltip |

> The abort key and tooltip visibility are configurable via [`Options`](methods.md#options) or globally
> on [`PromptPlus.Config`](../../global-behaviors.md).

---

## Confirmation & validation flow

Pressing **Enter** runs this sequence:

1. If the field is empty and [`DefaultIfEmpty`](methods.md#defaultifempty) was set, its value is substituted.
2. The typed characters are parsed into the target type (`int`, `decimal`, `DateTime`, …). A value that
   does not satisfy the mask cannot be confirmed.
3. Validation runs — [`PredicateSelected`](methods.md#predicateselected) /
   [`PredicateSelectedAsync`](methods.md#predicateselectedasync), if configured.
4. **Valid** → the control closes and returns `ResultPrompt<T>` with `IsAborted == false`.
   **Invalid** → the control stays open, shows the error line, and waits for more input.

> ⚠️ Validation only runs on confirm, never per keystroke. The mask already prevents malformed input;
> use `PredicateSelected` for whole-value rules such as ranges, business logic, or uniqueness. There is
> no `MinValue` / `MaxValue` property — a range check is a predicate.

```csharp
PromptPlus.Controls.MaskInteger("Percentage")
    .NumberFormat(3, withseparatorgroup: false)
    .PredicateSelected(v => v is >= 1 and <= 100
        ? (true, null)
        : (false, "Enter a value between 1 and 100."))
    .Run();
```

---

## Culture

Culture drives locale-dependent formatting. Set it per control with
[`Culture(CultureInfo)` / `Culture(string)`](methods.md#culture); otherwise the control uses
`PromptPlus.Config.DefaultCulture`.

| Control | What culture affects |
|---|---|
| Number | The thousands (digit-group) separator |
| Currency | The decimal and thousands separators, plus the currency symbol (`*Currency` factories) |
| Date/Time | The order of the parts and the separators (e.g. `MM/dd/yyyy` vs `dd/MM/yyyy`) |

```csharp
// Grouping and currency symbol follow the culture
PromptPlus.Controls.MaskDecimalCurrency("Preço")
    .NumberFormat(6, 2)
    .Culture(new CultureInfo("pt-BR"))   // "R$ 1.234,56"
    .Run();

// Date part order follows the culture
PromptPlus.Controls.MaskDate("Data")
    .Culture("pt-BR")                    // dd/MM/yyyy
    .Run();
```

---

## Date/time specifics

- **Fixed parts.** [`FixedValues(part, value)`](methods.md#fixedvalues) locks a segment to a constant;
  `-1` locks it to the current (now) value. Locked parts are skipped during editing.
- **Weekday display.** [`WeekTypeMode`](methods.md#weektypemode) appends the weekday (`WeekShort` /
  `WeekLong`) next to the value once the date is complete.
- **`MaskDate` / `MaskTime` return `DateTime`.** `MaskDate` leaves the time part unused and `MaskTime`
  leaves the date part unused. Use `MaskDateOnly` / `MaskTimeOnly` for the dedicated `DateOnly` /
  `TimeOnly` types.

---

## Edge cases & gotchas

- **`Mask` is required** for the string control; calling `Run` without it has no pattern to edit.
- **`Default` must fit the mask.** For the string control, provide the value without literals unless the
  mask contains none (e.g. `Default("1234567890")` for `(999) 999-9999`).
- **`returnWithMask`** decides whether string literals are in `.Content` — `false` returns only typed
  characters, `true` keeps parentheses/dashes/spaces.
- **Aborted results** carry the target type's default (`""`, `0`, `default(DateTime)`), not the seeded
  `Default`. Always branch on `IsAborted`.
- **Async callbacks block the UI thread** — see the warning under
  [`PredicateSelectedAsync`](methods.md#predicateselectedasync).

---

## See also

- [Methods](methods.md) — the API these behaviors come from
- [Styles](styles.md) — the `MaskEditStyles` regions
- [Global Behaviors](../../global-behaviors.md) — the config layer behind `Options`
