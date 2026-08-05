<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MaskEdit**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MaskEdit — Methods →](methods.md)

---

> Structured, pattern-constrained entry. The user types into a fixed template — a phone number,
> a price, a date — and the value is parsed into the right .NET type on confirm.

The **MaskEdit** family is a set of twelve factories that share one idea: the field is shaped by a
mask so the user can only produce well-formed input. Literal characters (dashes, parentheses,
separators) are painted for you and skipped over; only the editable positions accept keystrokes.
Each factory returns a strongly typed value — `string`, `int`, `decimal`, `DateTime`, `TimeOnly`,
and so on — through a [`ResultPrompt<T>`](../../architecture.md#resultpromptt).

> 💬 Free-form text with no fixed shape? Use [**Input**](../input/index.md) instead. MaskEdit is for
> values that must match a pattern.

---

## On this page

| Sub-page | What you will find |
|---|---|
| **Index** (this page) | What the family is, the twelve factories, when to use each, a first example |
| [Methods](methods.md) | Every fluent method, grouped by the four control interfaces, with signatures and snippets |
| [Operations](operations.md) | Mask tokens, placeholder, input modes, keyboard, validation, and culture |
| [Styles](styles.md) | The `MaskEditStyles` regions and positive/negative value coloring |

---

## The four control interfaces

Although there are twelve factories, they resolve to just four fluent interfaces. Everything on the
[Methods](methods.md) page is organized this way.

| Interface | Backs | Shape of the mask |
|---|---|---|
| `IMaskEditStringControl<string>` | `MaskEdit` | A free-form token mask you write yourself via `Mask(...)` |
| `IMaskEditNumberControl<T>` | `MaskInteger`, `MaskLong` | A whole-number mask built by `NumberFormat(...)` |
| `IMaskEditCurrencyControl<T>` | `MaskDecimal`, `MaskDecimalCurrency`, `MaskDouble`, `MaskDoubleCurrency` | A fixed-decimal mask built by `NumberFormat(...)` |
| `IMaskEditDateTimeControl<T>` | `MaskDateTime`, `MaskDate`, `MaskDateOnly`, `MaskTime`, `MaskTimeOnly` | A culture-ordered date/time mask (no mask string) |

---

## The twelve factories

All live on `PromptPlus.Controls`. Each takes `(string prompt = "", string? description = null)`.

| Factory | Returns `ResultPrompt<T>` where `T` = | Interface | Use it for |
|---|---|---|---|
| `MaskEdit` | `string` | `IMaskEditStringControl<string>` | Any fixed-shape text: phone, plate, SSN, product code |
| `MaskInteger` | `int` | `IMaskEditNumberControl<int>` | Whole numbers within `int` range |
| `MaskLong` | `long` | `IMaskEditNumberControl<long>` | Large whole numbers (card numbers, IDs) |
| `MaskDecimal` | `decimal` | `IMaskEditCurrencyControl<decimal>` | Fixed-decimal `decimal`, no currency symbol |
| `MaskDecimalCurrency` | `decimal` | `IMaskEditCurrencyControl<decimal>` | Money as `decimal`, with the culture's currency symbol |
| `MaskDouble` | `double` | `IMaskEditCurrencyControl<double>` | Fixed-decimal `double`, no currency symbol |
| `MaskDoubleCurrency` | `double` | `IMaskEditCurrencyControl<double>` | Money as `double`, with the culture's currency symbol |
| `MaskDateTime` | `DateTime` | `IMaskEditDateTimeControl<DateTime>` | Date **and** time in one field |
| `MaskDate` | `DateTime` | `IMaskEditDateTimeControl<DateTime>` | Date only (time part unused) |
| `MaskDateOnly` | `DateOnly` | `IMaskEditDateTimeControl<DateOnly>` | Date only, as a `DateOnly` |
| `MaskTime` | `DateTime` | `IMaskEditDateTimeControl<DateTime>` | Time only (date part unused) |
| `MaskTimeOnly` | `TimeOnly` | `IMaskEditDateTimeControl<TimeOnly>` | Time only, as a `TimeOnly` |

> 💡 Prefer `MaskDateOnly` / `MaskTimeOnly` when you want the modern `DateOnly` / `TimeOnly` types;
> use `MaskDate` / `MaskTime` when the rest of your code works in `DateTime`.

---

## When to use it

| Use MaskEdit when… | Consider instead… |
|---|---|
| Input must follow a fixed pattern (date, phone, currency) | — |
| You want the value already parsed to `int` / `decimal` / `DateTime` | [Input](../input/index.md) (returns a raw string) |
| The value is any free-form string | [Input](../input/index.md) |
| The value is a secret to hide as typed | [Secret](../secret/index.md) |
| The user should pick from a known list | [Select](../select/index.md) |

---

## Minimal example

```csharp
using PromptPlusLibrary;

var phone = PromptPlus.Controls
    .MaskEdit("Phone")
    .Mask(@"\(999\)\ 999\-9999")   // (999) 999-9999
    .Run();

if (!phone.IsAborted)
    PromptPlus.Console.WriteLine($"Phone: {phone.Content}");
```

- `MaskEdit("Phone")` creates the string control. The first argument is the **prompt**; an optional
  second argument is a **description** line shown under it.
- `.Mask(...)` is **required** for the string control — it defines the editable pattern. `9` accepts a
  digit; `\` escapes the following character so parentheses, spaces, and dashes render as literals.
- `.Run()` renders the field and blocks until the user presses **Enter** (confirm) or **Esc** (abort).
- The call returns a [`ResultPrompt<string>`](../../architecture.md#resultpromptt): read `.Content`
  for the value and `.IsAborted` to detect Esc.

> 💡 Always check `IsAborted` before using `.Content`. On abort, `.Content` holds the type's default.

---

## A typed example

The number, currency, and date/time factories parse straight into their target type — no
`int.Parse` on your side.

```csharp
using PromptPlusLibrary;

var price = PromptPlus.Controls
    .MaskDecimalCurrency("Price")
    .NumberFormat(integerpart: 6, decimalpart: 2)   // up to 6 integer digits, 2 decimals
    .PredicateSelected(v => v > 0m
        ? (true, null)
        : (false, "Price must be greater than zero"))
    .Run();

if (!price.IsAborted)
    PromptPlus.Console.WriteLine($"Price: {price.Content:C}");   // price.Content is a decimal
```

This shows the number/currency pattern: **shape the mask with `NumberFormat`** and **validate the
parsed value with `PredicateSelected`** (there is no min/max property — range checks live in the
predicate). See [Methods](methods.md) for the full surface.

---

## Return value

Every factory returns `ResultPrompt<T>` for its `T` from the table above.

| Member | Meaning |
|---|---|
| `.Content` | The confirmed, parsed value (the type's default if aborted) |
| `.IsAborted` | `true` when the user pressed Esc / the abort key |

```csharp
var (year, aborted) = PromptPlus.Controls
    .MaskInteger("Year")
    .NumberFormat(4, withseparatorgroup: false)
    .Run();
if (!aborted) PromptPlus.Console.WriteLine(year);
```

---

## See also

- [Methods](methods.md) — the fluent API of all four interfaces
- [Operations](operations.md) — mask tokens, input modes, keyboard, validation, culture
- [Styles](styles.md) — recolor the regions and the positive/negative value colors
- [Input](../input/index.md) — free-form text sibling
- [Secret](../secret/index.md) — masked (hidden) input
