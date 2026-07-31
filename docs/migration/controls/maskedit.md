# Migration v5.x → v6.x: MaskEdit

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## Summary of changes

The MaskEdit family has **no breaking changes**. Every factory method and every builder method that existed in v5.x still exists in v6.x with the same signature.

The only change is an **addition**:

- **`PredicateSelectedAsync` (x2)** is new on every MaskEdit type in v6.x. v5.x had only the synchronous `PredicateSelected` overloads.

> ⚠️ **Correction vs. earlier drafts:** the concrete factory methods (`MaskInteger`, `MaskLong`, `MaskDecimal`, `MaskDecimalCurrency`, `MaskDouble`, `MaskDoubleCurrency`, `MaskTime`, `MaskTimeOnly`, `MaskEdit`, `MaskDate`, `MaskDateTime`, `MaskDateOnly`) **all already existed in v5.x** — they are *not* new in v6.x. There were never any generic `MaskNumber<T>()` / `MaskCurrency<T>()` factories, and `MaxWidth(byte)` never existed on any MaskEdit control.

## Factory methods (identical in v5.x and v6.x)

| Factory | Return type | Value |
|---|---|---|
| `MaskEdit()` | `IMaskEditStringControl<string>` | masked string |
| `MaskDate()` | `IMaskEditDateTimeControl<DateTime>` | date |
| `MaskDateTime()` | `IMaskEditDateTimeControl<DateTime>` | date + time |
| `MaskDateOnly()` | `IMaskEditDateTimeControl<DateOnly>` | date |
| `MaskTime()` | `IMaskEditDateTimeControl<DateTime>` | time as `DateTime` |
| `MaskTimeOnly()` | `IMaskEditDateTimeControl<TimeOnly>` | time as `TimeOnly` |
| `MaskInteger()` | `IMaskEditNumberControl<int>` | integer |
| `MaskLong()` | `IMaskEditNumberControl<long>` | long |
| `MaskDecimal()` | `IMaskEditCurrencyControl<decimal>` | decimal (no currency symbol) |
| `MaskDecimalCurrency()` | `IMaskEditCurrencyControl<decimal>` | decimal (currency) |
| `MaskDouble()` | `IMaskEditCurrencyControl<double>` | double (no currency symbol) |
| `MaskDoubleCurrency()` | `IMaskEditCurrencyControl<double>` | double (currency) |

---

## MaskEdit — String

`PredicateSelected` receives the value directly (a `string`) — there is no wrapper object.

```csharp
using PromptPlusLibrary;

// v5.x and v6.x — same API
var result = PromptPlus.Controls.MaskEdit("Tax ID:")
    .Mask("999.999.999-99")
    .PromptMask('_')
    .Default("000.000.000-00")
    .PredicateSelected(value => IsValidTaxId(value))   // value is the string
    .Run();

string masked = result.Content;
```

### New in v6.x: `PredicateSelectedAsync`
```csharp
var result = PromptPlus.Controls.MaskEdit("Tax ID:")
    .Mask("999.999.999-99")
    .PredicateSelectedAsync(async value =>
    {
        bool valid = await ValidateTaxIdRemotelyAsync(value);
        return (valid, valid ? null : "Invalid tax ID");
    })
    .Run();
```

---

## MaskEdit — Date and Time

`PredicateSelected` receives the typed value directly (`DateTime`, `DateOnly` or `TimeOnly`).

```csharp
using PromptPlusLibrary;

// v5.x and v6.x — same API
var result = PromptPlus.Controls.MaskDate("Birth date:")
    .Default(DateTime.Today)
    .Culture(new CultureInfo("pt-BR"))
    .WeekTypeMode(WeekType.WeekShort)
    .InputMode(InputBehavior.EditSkipToInput)
    .PredicateSelected(date => date <= DateTime.Today)   // date is a DateTime
    .Run();
```

`MaskTime` / `MaskTimeOnly` also existed in v5.x:
```csharp
var t1 = PromptPlus.Controls.MaskTime("Time:").Default(DateTime.Now).Run();          // DateTime
var t2 = PromptPlus.Controls.MaskTimeOnly("Time:").Default(TimeOnly.FromDateTime(DateTime.Now)).Run();  // TimeOnly
```

---

## MaskEdit — Integers

```csharp
// v5.x and v6.x — same API
var qty = PromptPlus.Controls.MaskInteger("Quantity:")
    .NumberFormat(integerpart: 6, withsignal: false, withseparatorgroup: true)
    .Default(0)
    .Culture(new CultureInfo("pt-BR"))
    .Run();

var big = PromptPlus.Controls.MaskLong("Big number:")
    .NumberFormat(integerpart: 12)
    .Default(0L)
    .Run();
```

---

## MaskEdit — Decimal / Double (currency)

`NumberFormat` on the currency/decimal controls takes `decimalpart` (default `2`):

```csharp
// v5.x and v6.x — same API
var price = PromptPlus.Controls.MaskDecimalCurrency("Amount:")
    .NumberFormat(integerpart: 10, decimalpart: 2, withsignal: false)
    .Default(0m)
    .Culture(new CultureInfo("pt-BR"))
    .Run();

var rate = PromptPlus.Controls.MaskDouble("Rate:")
    .NumberFormat(integerpart: 6, decimalpart: 6)
    .Default(0d)
    .Run();
```

---

## Reference: API present on all MaskEdit types

| Method | v5.x | v6.x |
|---|---|---|
| `PromptMask(char)` | ✅ | ✅ |
| `Default(T)` · `DefaultIfEmpty(T)` | ✅ | ✅ |
| `HideTipInputType(bool)` | ✅ | ✅ |
| `PredicateSelected(Func<T,bool>)` | ✅ | ✅ |
| `PredicateSelected(Func<T,(bool,string?)>)` | ✅ | ✅ |
| `Styles(MaskEditStyles, Style)` · `Options(...)` | ✅ | ✅ |
| `PredicateSelectedAsync(Func<T,Task<bool>>)` | ❌ | ✅ **New** |
| `PredicateSelectedAsync(Func<T,Task<(bool,string?)>>)` | ❌ | ✅ **New** |

Type-specific methods (unchanged v5.x → v6.x): String adds `Mask(string, bool)` + `InputMode`; DateTime adds `FixedValues`, `InputMode`, `WeekTypeMode`, `Culture`; Number adds `NumberFormat(byte, bool, bool)` + `Culture`; Currency adds `NumberFormat(byte, byte, bool, bool)` + `Culture`.
