<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MaskEdit — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MaskEdit — Operations →](operations.md)

---

The MaskEdit family exposes four fluent interfaces. They share a common core — `PromptMask`,
`HideTipInputType`, `Default`, `DefaultIfEmpty`, `PredicateSelected(Async)`, `Styles`, `Options`,
`Run` — and each adds a few interface-specific methods for shaping the mask. Every method returns the
same control instance, so calls chain in any order. Call [`Run`](#run) last.

**Jump to an interface:**
[String](#string--imaskeditstringcontrolstring) ·
[Number](#number--imaskeditnumbercontrolt) ·
[Currency](#currency--imaskeditcurrencycontrolt) ·
[Date/Time](#datetime--imaskeditdatetimecontrolt) ·
[Shared surface](#shared-surface)

---

## String — `IMaskEditStringControl<string>`

Factory: `PromptPlus.Controls.MaskEdit(string prompt = "", string? description = null)`.

**Methods:**
[Mask](#mask) ·
[PromptMask](#promptmask) ·
[InputMode](#inputmode) ·
[HideTipInputType](#hidetipinputtype) ·
plus the [shared surface](#shared-surface).

### `Mask`

```csharp
IMaskEditStringControl<string> Mask(string mask, bool returnWithMask = false)
```

**Required.** Defines the editable pattern. Each token position accepts one class of character;
anything not a token is rendered as an un-editable literal. See the full
[mask-token table](operations.md#mask-tokens) for the complete grammar.

| Parameter | Meaning |
|---|---|
| `mask` | The pattern string (e.g. `UUU-9999`). Tokens: `9` digit, `L` lower letter, `U` upper letter, `A` any letter, `X` letter or digit, `C` custom, `\` escape next char as literal, `{}` `[]` `()` groups. |
| `returnWithMask` | `false` (default): `.Content` contains only the typed characters. `true`: `.Content` keeps the mask literals. |

```csharp
// Result WITHOUT literals: "1234567890"
PromptPlus.Controls.MaskEdit("Phone")
    .Mask(@"\(999\)\ 999\-9999")
    .Run();

// Result WITH literals: "(123) 456-7890"
PromptPlus.Controls.MaskEdit("Phone")
    .Mask("(999) 999-9999", returnWithMask: true)
    .Run();
```

### `InputMode`

```csharp
IMaskEditStringControl<string> InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput)
```

Sets how the cursor moves through the mask.

| `InputBehavior` | Effect |
|---|---|
| `EditSkipToInput` | Default. The cursor jumps straight to the next editable position, skipping literals. |
| `EditCursorFreely` | The cursor moves one position at a time, over literals too. |

```csharp
PromptPlus.Controls.MaskEdit("CPF")
    .Mask("999.999.999-99")
    .InputMode(InputBehavior.EditCursorFreely)
    .Run();
```

---

## Number — `IMaskEditNumberControl<T>`

Factories: `MaskInteger` → `int`, `MaskLong` → `long`. There is **no** mask string — the mask is
built from `NumberFormat`.

**Methods:**
[NumberFormat](#numberformat-number) ·
[Culture](#culture) ·
[PromptMask](#promptmask) ·
[HideTipInputType](#hidetipinputtype) ·
plus the [shared surface](#shared-surface).

### `NumberFormat` (number)

```csharp
IMaskEditNumberControl<T> NumberFormat(byte integerpart, bool withsignal = false, bool withseparatorgroup = true)
```

Builds the whole-number mask.

| Parameter | Meaning |
|---|---|
| `integerpart` | Maximum number of digits allowed. |
| `withsignal` | `true` allows a leading sign (`+` / `-`). Default `false`. |
| `withseparatorgroup` | `true` (default) shows the culture's thousands separator. |

```csharp
PromptPlus.Controls.MaskInteger("Temperature")
    .NumberFormat(3, withsignal: true)            // e.g. -40
    .Run();

PromptPlus.Controls.MaskLong("Card number")
    .NumberFormat(15)                             // 15 grouped digits
    .Run();
```

### `Culture`

```csharp
IMaskEditNumberControl<T> Culture(CultureInfo culture)
IMaskEditNumberControl<T> Culture(string cultureName)
```

Sets the culture that supplies the digit-group separator. The string overload throws
`ArgumentException` for a null/empty name.

```csharp
PromptPlus.Controls.MaskInteger("Menge")
    .NumberFormat(7)
    .Culture("de-DE")                             // "1.234.567"
    .Run();
```

---

## Currency — `IMaskEditCurrencyControl<T>`

Factories: `MaskDecimal` / `MaskDecimalCurrency` → `decimal`, `MaskDouble` / `MaskDoubleCurrency`
→ `double`. The `*Currency` variants prepend the culture's currency symbol; the plain variants do
not. All four share this one interface.

**Methods:**
[NumberFormat](#numberformat-currency) ·
[Culture](#culture) ·
[PromptMask](#promptmask) ·
[HideTipInputType](#hidetipinputtype) ·
plus the [shared surface](#shared-surface).

### `NumberFormat` (currency)

```csharp
IMaskEditCurrencyControl<T> NumberFormat(byte integerpart, byte decimalpart = 2, bool withsignal = false, bool withseparatorgroup = true)
```

Builds the fixed-decimal mask. Same as the number overload, plus a decimal-digit count.

| Parameter | Meaning |
|---|---|
| `integerpart` | Maximum number of integer digits. |
| `decimalpart` | Number of digits after the decimal point. Default `2`. |
| `withsignal` | `true` allows a leading sign (`+` / `-`). Default `false`. |
| `withseparatorgroup` | `true` (default) shows the culture's thousands separator. |

```csharp
// decimal, 6 integer digits + 2 decimals, no symbol
PromptPlus.Controls.MaskDecimal("Amount")
    .NumberFormat(6, 2)
    .Run();

// decimal with the culture currency symbol
PromptPlus.Controls.MaskDecimalCurrency("Price")
    .NumberFormat(6, 2)
    .Culture("pt-BR")                             // "R$ 1.234,56"
    .Run();

// double, signed, 3 decimals, no grouping
PromptPlus.Controls.MaskDouble("Rate")
    .NumberFormat(3, 4, withseparatorgroup: false)
    .Run();
```

> The `Culture` overloads are identical in shape to the [number ones](#culture); for the `*Currency`
> factories, culture also drives the currency symbol.

---

## Date/Time — `IMaskEditDateTimeControl<T>`

Factories: `MaskDateTime` / `MaskDate` / `MaskTime` → `DateTime`, `MaskDateOnly` → `DateOnly`,
`MaskTimeOnly` → `TimeOnly`. There is **no** mask string — the part order and separators come from
the culture, and each part (day, month, year, hour, minute, second) is a separate editable field.

**Methods:**
[FixedValues](#fixedvalues) ·
[WeekTypeMode](#weektypemode) ·
[InputMode](#inputmode-datetime) ·
[Culture](#culture) ·
[PromptMask](#promptmask) ·
[HideTipInputType](#hidetipinputtype) ·
plus the [shared surface](#shared-surface).

### `FixedValues`

```csharp
IMaskEditDateTimeControl<T> FixedValues(DateTimePart dateTimePart, int value)
```

Locks one date/time part to a constant the user cannot edit.

| Parameter | Meaning |
|---|---|
| `dateTimePart` | The `DateTimePart` to lock: `Day`, `Month`, `Year`, `Hour`, `Minute`, `Second`. |
| `value` | The constant to set. Use `-1` to lock the part to its current (now) value. |

```csharp
PromptPlus.Controls.MaskDate("Day only")
    .FixedValues(DateTimePart.Year, -1)           // current year, locked
    .FixedValues(DateTimePart.Month, 12)          // December, locked
    .Run();
```

### `WeekTypeMode`

```csharp
IMaskEditDateTimeControl<T> WeekTypeMode(WeekType value = WeekType.WeekShort)
```

Shows the weekday next to the value once the date is complete.

| `WeekType` | Effect |
|---|---|
| `None` | Do not show the weekday |
| `WeekShort` | Abbreviated (e.g. "Mon") |
| `WeekLong` | Full name (e.g. "Monday") |

```csharp
PromptPlus.Controls.MaskDate("Pick a date")
    .WeekTypeMode(WeekType.WeekLong)
    .Run();
```

### `InputMode` (date/time)

```csharp
IMaskEditDateTimeControl<T> InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput)
```

Same `InputBehavior` semantics as the [string control](#inputmode): `EditSkipToInput` (default)
jumps across separators; `EditCursorFreely` moves one position at a time.

```csharp
PromptPlus.Controls.MaskDateTime("Timestamp")
    .InputMode(InputBehavior.EditCursorFreely)
    .Run();
```

---

## Shared surface

Every MaskEdit interface exposes the following members with identical shapes (the return type is the
control's own interface). Examples below use `MaskEdit`; they apply to all four.

**Members:**
[PromptMask](#promptmask) ·
[HideTipInputType](#hidetipinputtype) ·
[Default](#default) ·
[DefaultIfEmpty](#defaultifempty) ·
[PredicateSelected](#predicateselected) ·
[PredicateSelectedAsync](#predicateselectedasync) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

### `PromptMask`

```csharp
PromptMask(char value = '_')
```

Sets the character shown in empty (unfilled) mask positions. Default `'_'`.

```csharp
PromptPlus.Controls.MaskEdit("Serial")
    .Mask("AAAA-AAAA")
    .PromptMask('#')
    .Run();
```

### `HideTipInputType`

```csharp
HideTipInputType(bool value = true)
```

Hides the input-type hint shown below the field. Default behavior is `false` (hint visible); calling
it with no argument hides the hint.

```csharp
PromptPlus.Controls.MaskEdit("ZIP")
    .Mask("99999-999")
    .HideTipInputType()
    .Run();
```

### `Default`

```csharp
Default(T value)
```

Pre-fills the field with `value` before the user starts typing. For the string control the value must
fit the mask (supply it without the literals unless the mask has none).

```csharp
PromptPlus.Controls.MaskEdit("Phone")
    .Mask("(999) 999-9999")
    .Default("1234567890")
    .Run();

PromptPlus.Controls.MaskDate("Date")
    .Default(DateTime.Today)
    .Run();
```

### `DefaultIfEmpty`

```csharp
DefaultIfEmpty(T value)
```

Sets the value returned when the user confirms an empty field. Unlike [`Default`](#default), it is not
shown in the field — it is only substituted at confirm time.

```csharp
PromptPlus.Controls.MaskInteger("Quantity")
    .NumberFormat(6)
    .DefaultIfEmpty(100)          // Enter on empty → 100
    .Run();
```

### `PredicateSelected`

Validation that runs **when the user presses Enter**. Two overloads — pick the tuple form to show a
custom message.

```csharp
PredicateSelected(Func<T, bool> validselect)
PredicateSelected(Func<T, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<T, bool>` | `true` = valid | On failure, a generic error is shown |
| `Func<T, (bool, string?)>` | `(isValid, message)` | On failure, `message` is shown (or a generic one if `null`) |

```csharp
// Boolean form (number)
PromptPlus.Controls.MaskInteger("Even number")
    .NumberFormat(4, withseparatorgroup: false)
    .PredicateSelected(v => v % 2 == 0)
    .Run();

// Message form (currency) — this is how you enforce a range
PromptPlus.Controls.MaskDecimalCurrency("Price")
    .NumberFormat(6, 2)
    .PredicateSelected(v => v <= 1000m
        ? (true, null)
        : (false, "The price cannot exceed 1000."))
    .Run();
```

> There is no `MinValue` / `MaxValue` — express range and business rules through this predicate.

### `PredicateSelectedAsync`

Asynchronous counterparts, for validation that awaits I/O.

```csharp
PredicateSelectedAsync(Func<T, Task<bool>> validselect)
PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect)
```

```csharp
PromptPlus.Controls.MaskEdit("Order code")
    .Mask("UUU-9999")
    .PredicateSelectedAsync(async code =>
    {
        var ok = await api.OrderExistsAsync(code);
        return ok ? (true, null) : (false, "No such order");
    })
    .Run();
```

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — it does not run in
> parallel with the render loop. Keep it fast; long calls freeze the prompt until they return.

### `Styles`

```csharp
Styles(MaskEditStyles styleType, Style style)
```

Overrides the color of one visual region of this control instance. Throws `ArgumentNullException` if
`style` is `null`. See the full region list on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here

PromptPlus.Controls.MaskEdit("Styled")
    .Mask("AAAA-9999")
    .Styles(MaskEditStyles.Prompt, new Style(Color.Yellow, Color.Black))
    .Styles(MaskEditStyles.Answer, new Style(Color.Green, Color.Black))
    .Run();
```

### `Options`

```csharp
Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
description text, abort key, tooltip, hide-after-finish, and the extra-info affixes. Throws
`ArgumentNullException` if `options` is `null`.

```csharp
PromptPlus.Controls.MaskEdit("Product code")
    .Mask("UUU-9999")
    .Options(o => o
        .Description("Format: 3 letters, dash, 4 digits")
        .EnabledAbortKey(true)
        .ShowMessageAbortKey(true)
        .HideAfterFinish(false))
    .Run();
```

See [Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list.

### `Run`

```csharp
ResultPrompt<T> Run(CancellationToken token = default)
```

Renders the field and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns a
[`ResultPrompt<T>`](../../architecture.md#resultpromptt) with the parsed value.

| Parameter | Meaning |
|---|---|
| `token` | A `CancellationToken` that cancels the prompt while it waits for input. |

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
var result = PromptPlus.Controls
    .MaskEdit("Type before timeout")
    .Mask("999-999")
    .Run(cts.Token);
```

---

## See also

- [Operations](operations.md) — mask tokens, input modes, keyboard, validation, culture
- [Styles](styles.md) — the `MaskEditStyles` regions
- [Index](index.md) — overview and the eleven factories
