<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Input — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[? Back to Home](../../../README.md) • **Next:** [Input — Operations ?](operations.md)

---

Every fluent method on `IInputControl`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Input(string prompt = "", string? description = null)`,
> which returns `IInputControl`.

**Quick jump:**
[Default](#default) ·
[DefaultIfEmpty](#defaultifempty) ·
[InputToCase](#inputtocase) ·
[AcceptInput](#acceptinput) ·
[MaxLength](#maxlength) ·
[PredicateValid](#predicatevalid) ·
[PredicateValidAsync](#predicatevalidasync) ·
[SuggestionHandler](#suggestionhandler) ·
[SuggestionHandlerAsync](#suggestionhandlerasync) ·
[MinimumSuggestionLength](#minimumsuggestionlength) ·
[EnabledHistory](#enabledhistory) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Seeding a value

### `Default`

```csharp
IInputControl Default(string value, bool useDefaultHistory = true)
```

Pre-fills the field with `value` before the user starts typing.

| Parameter | Meaning |
|---|---|
| `value` | The initial text shown in the field. Cannot be `null`. |
| `useDefaultHistory` | When `true` **and** history is enabled via [`EnabledHistory`](#enabledhistory), the most recent history entry is preferred over `value`. Default `true`. |

```csharp
PromptPlus.Controls.Input("Profile")
    .Default("John Doe")
    .Run();
```

> ?? Combine `Default(string.Empty, true)` with `EnabledHistory(...)` to pre-load the last value
> the user confirmed on a previous run.

---

### `DefaultIfEmpty`

```csharp
IInputControl DefaultIfEmpty(string value)
```

Sets the value returned when the user confirms **without typing anything**. Unlike
[`Default`](#default), this text is *not* shown in the field — it is only substituted at confirm time.

```csharp
PromptPlus.Controls.Input("Display Name", "Press Enter to accept the fallback")
    .DefaultIfEmpty("Anonymous")
    .Run();
// User presses Enter on an empty field ? result.Content == "Anonymous"
```

---

## Restricting what can be typed

### `InputToCase`

```csharp
IInputControl InputToCase(CaseOptions value)
```

Coerces every typed character to a casing rule as it is entered.

| `CaseOptions` | Effect |
|---|---|
| `Any` | No transformation (default) |
| `Uppercase` | Letters become upper case |
| `Lowercase` | Letters become lower case |

```csharp
PromptPlus.Controls.Input("Code", "Letters are upper-cased as you type")
    .InputToCase(CaseOptions.Uppercase)
    .Run();
```

---

### `AcceptInput`

```csharp
IInputControl AcceptInput(Func<char, bool> value)
```

A per-keystroke filter. The callback receives each character the moment it is typed; return
`true` to accept it or `false` to silently ignore it. Rejected characters never enter the field.

```csharp
// Digits only
PromptPlus.Controls.Input("PIN")
    .AcceptInput(char.IsDigit)
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `MaxLength`

```csharp
IInputControl MaxLength(int maxLength)
```

Caps the number of characters. Once the limit is reached, further keystrokes are ignored.
A value of `0` or less means **no limit** (the default).

```csharp
PromptPlus.Controls.Input("Name", "Max 20 characters")
    .MaxLength(20)
    .Run();
```

---

## Validating the confirmed value

Validation runs **when the user presses Enter**. If it fails, the control stays open and shows an
error (styled with [`InputStyles.Error`](styles.md)); the value is only returned when validation passes.

### `PredicateValid`

Two overloads — pick the tuple form when you want to show a custom message.

```csharp
IInputControl PredicateValid(Func<string, bool> validselect)
IInputControl PredicateValid(Func<string, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<string, bool>` | `true` = valid | On failure, a generic error is shown |
| `Func<string, (bool, string?)>` | `(isValid, message)` | On failure, `message` is shown (or a generic one if `null`) |

```csharp
// Boolean form
PromptPlus.Controls.Input("Code")
    .PredicateValid(v => v.Length >= 2)
    .Run();

// Message form
PromptPlus.Controls.Input("Code")
    .PredicateValid(v => v.Length >= 2
        ? (true, null)
        : (false, "Length must be at least 2"))
    .Run();
```

---

### `PredicateValidAsync`

Asynchronous counterparts of [`PredicateValid`](#predicatevalid), for validation that awaits
I/O (a database, an HTTP call).

```csharp
IInputControl PredicateValidAsync(Func<string, Task<bool>> validselect)
IInputControl PredicateValidAsync(Func<string, Task<(bool, string?)>> validselect)
```

```csharp
PromptPlus.Controls.Input("Username")
    .PredicateValidAsync(async name =>
    {
        var taken = await api.IsUsernameTakenAsync(name);
        return taken ? (false, "That username is already taken") : (true, null);
    })
    .Run();
```

> ?? The async predicate is awaited **synchronously (blocking) on the UI thread** — it does not run
> in parallel with the render loop. Keep it fast; long calls freeze the prompt until they return.

---

## Autocomplete suggestions

### `SuggestionHandler`

```csharp
IInputControl SuggestionHandler(Func<string, string[]> value, bool autocomplete = true)
```

Supplies Tab / Shift+Tab completion. The callback receives the current text and returns candidate
strings.

| Parameter | Meaning |
|---|---|
| `value` | Returns the suggestion array for the current input. Cannot be `null`. |
| `autocomplete` | `true` (default): when a single match exists, Tab applies it directly. `false`: matches are shown in a list to cycle through with Tab / Shift+Tab. |

```csharp
PromptPlus.Controls.Input("Environment", "TAB / Shift+TAB to rotate")
    .SuggestionHandler(input =>
    {
        var all = new[] { "dev", "test", "staging", "prod" };
        return string.IsNullOrWhiteSpace(input)
            ? all
            : all.Where(x => x.StartsWith(input, StringComparison.OrdinalIgnoreCase)).ToArray();
    })
    .Run();
```

---

### `SuggestionHandlerAsync`

```csharp
IInputControl SuggestionHandlerAsync(Func<string, Task<string[]>> value, bool autocomplete = true)
```

Asynchronous version of [`SuggestionHandler`](#suggestionhandler) for suggestions fetched from a
service. Same `autocomplete` semantics.

```csharp
PromptPlus.Controls.Input("Country")
    .SuggestionHandlerAsync(async input =>
    {
        var matches = await api.SearchCountriesAsync(input);
        return matches.ToArray();
    })
    .Run();
```

---

### `MinimumSuggestionLength`

```csharp
IInputControl MinimumSuggestionLength(byte value)
```

Waits until at least `value` characters have been typed before invoking the suggestion provider.
Default `0` (suggestions from the first character). Useful to avoid firing an expensive async
lookup on every keystroke.

```csharp
PromptPlus.Controls.Input("Country")
    .MinimumSuggestionLength(2)
    .SuggestionHandlerAsync(api.SearchCountriesAsync)
    .Run();
```

---

## History

### `EnabledHistory`

```csharp
IInputControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists confirmed values to disk under `filename` and lets the user recall them with **F3**.

| Parameter | Meaning |
|---|---|
| `filename` | A stable, unique key for this field's history store. Cannot be `null`. |
| `options` | Optional `IHistoryOptions` configuration (see below). |

`IHistoryOptions`:

| Method | Default | Effect |
|---|---|---|
| `MinPrefixLength(byte)` | `0` | Minimum characters typed before history suggestions appear |
| `MaxItems(byte)` | `255` | Maximum entries retained |
| `ExpirationTime(TimeSpan)` | `365` days | How long new entries live |
| `FilterType(FilterMode)` | `Contains` | How the typed prefix matches history entries |
| `PageSize(byte)` | `5` | Entries shown per page during history navigation |

```csharp
PromptPlus.Controls.Input("Profile", "Press F3 to browse history")
    .EnabledHistory("profile-history", opt => opt
        .MinPrefixLength(2)
        .FilterType(FilterMode.StartsWith)
        .MaxItems(10)
        .ExpirationTime(TimeSpan.FromDays(30)))
    .Run();
```

> ?? Use a distinct `filename` per field so unrelated histories don't collide. See
> [Operations ? History](operations.md#history) for the runtime behavior.

---

## Dynamic description

### `ChangeDescription`

```csharp
IInputControl ChangeDescription(Func<string, string> value)
```

Recomputes the description line on every keystroke. The callback receives the current text and
returns the description to display — handy for live counters or hints.

```csharp
PromptPlus.Controls.Input("Bio")
    .ChangeDescription(text => $"Current length: {text.Length}")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
IInputControl ChangeDescriptionAsync(Func<string, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

---

## Appearance & behavior

### `Styles`

```csharp
IInputControl Styles(InputStyles styleType, Style style)
```

Overrides the color/decoration of one visual region of this control instance. See the full region
list and examples on the [Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here

PromptPlus.Controls.Input("Name")
    .Styles(InputStyles.Answer, new Style(Color.Green, Color.Default))
    .Run();
```

---

### `Options`

```csharp
IInputControl Options(Action<IControlOptions> options)
```

Overrides global behaviors ([`PromptPlus.Config`](../../global-behaviors.md)) for this one control —
prompt/description text, abort key, tooltip, hide-after-finish, and the extra-info affixes.

```csharp
PromptPlus.Controls.Input("Name")
    .Options(o => o
        .EnabledAbortKey(false)   // no Esc for this field
        .ShowTooltip(true)
        .HideAfterFinish(true))   // erase the UI once confirmed
    .Run();
```

See [Global Behaviors ? Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions)
for the complete `IControlOptions` list.

---

## Running the control

### `Run`

```csharp
ResultPrompt<string> Run(CancellationToken token = default)
```

Renders the field and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns a
[`ResultPrompt<string>`](../../architecture.md#resultpromptt).

| Parameter | Meaning |
|---|---|
| `token` | A `CancellationToken` that cancels the prompt while it waits for input. |

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var result = PromptPlus.Controls.Input("Name").Run(cts.Token);
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `InputStyles` regions
- [Index](index.md) — overview and method map
