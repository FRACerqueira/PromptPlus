# Migration v5.x → v6.x: Input and AutoComplete

> Back to [Migration Overview](../../migration-v5-to-v6.md)

## Renamed public methods

The v5.x members below were **renamed** in v6.x (same behavior, new name). Apply to both `Input` and
`Secret`.

| Control | v5.x member | v6.x member |
|---|---|---|
| Input / Secret | `PredicateSelected(Func<string, bool>)` | `PredicateValid(Func<string, bool>)` |
| Input / Secret | `PredicateSelected(Func<string, (bool, string?)>)` | `PredicateValid(Func<string, (bool, string?)>)` |
| Input only (`Secret` has no history support) | `EnabledHistory(string, Action<IHistoryOptions>?)` | `EnableHistory(string, Action<IHistoryOptions>?)` |

> The async overloads follow the same name: `PredicateValidAsync` (new in v6.x).

## AutoComplete → Input

In v5.x, `AutoComplete` was a dedicated, **non-generic** control (`PromptPlus.Controls.AutoComplete(...)`) built around an async completion service. In v6.x the dedicated control is **removed**; suggestion/completion is provided by `Input` via `SuggestionHandler` / `SuggestionHandlerAsync`.

| Behavior | v5.x | v6.x |
|---|---|---|
| Async completion source | `AutoComplete().CompletionAsyncService(func)` | `Input().SuggestionHandlerAsync(func)` |
| Auto-apply the suggestion when a single match exists | — | `Input().SuggestionHandler(func, autocomplete: true)` *(default)* |
| Only list suggestions for manual selection | — | `Input().SuggestionHandler(func, autocomplete: false)` |

---

## Breaking Changes

### 1. `AutoComplete()` removed → use `Input().SuggestionHandler(...)`

**Before (v5.x):**
```csharp
using PromptPlusLibrary;

// AutoComplete is non-generic and uses CompletionAsyncService
var result = PromptPlus.Controls.AutoComplete("City:")
    .CompletionAsyncService(async (input, ct) => await GetSuggestionsAsync(input, ct))
    .MinimumPrefixLength(2)
    .Run();
```

**After (v6.x):**
```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls.Input("City:")
    .SuggestionHandlerAsync(async input => await GetSuggestionsAsync(input))
    .MinimumSuggestionLength(2)
    .Run();
```

> The v5.x `CompletionAsyncService(Func<string, CancellationToken, Task<string[]>>)` maps to the v6.x `SuggestionHandlerAsync(Func<string, Task<string[]>>)`. Suggestions are triggered with **Tab / Shift+Tab**.

---

### 2. `MaxWidth(byte)` — removed

Both v5.x `Input` and `AutoComplete` had `MaxWidth(byte)`. It is removed in v6.x; width is automatic.

```csharp
// v5.x — remove this line during migration
.MaxWidth(40)
```

---

## What's new in v6.x

The methods below are **new on `Input` in v6.x** (they did not exist on the v5.x `Input`):

### `SuggestionHandlerAsync` — async suggestions

```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Input("City:")
    .SuggestionHandlerAsync(async input =>
    {
        var results = await FetchSuggestionsAsync(input);
        return results.ToArray();
    })
    .Run();
```

### `autocomplete` parameter on `SuggestionHandler`

The v5.x `Input` had `SuggestionHandler(Func<string,string[]>)` with no options. In v6.x an `autocomplete` flag (default `true`) controls whether a single match is applied automatically (`true`) or shown as a list for manual selection (`false`).

```csharp
PromptPlus.Controls.Input("City:")
    .SuggestionHandler(input => GetCities(input), autocomplete: false)
    .Run();
```

### `MinimumSuggestionLength(byte)`

```csharp
// suggestions only appear after 3 characters are typed
PromptPlus.Controls.Input("City:")
    .SuggestionHandler(input => GetCities(input))
    .MinimumSuggestionLength(3)
    .Run();
```

### `PredicateValidAsync`

The v5.x `Input` had only the synchronous validator, named `PredicateSelected`. v6.x **renames** it to
`PredicateValid` and adds async overloads (`PredicateValidAsync`, useful for remote validation).

```csharp
using PromptPlusLibrary;

PromptPlus.Controls.Input("E-mail:")
    .PredicateValidAsync(async value =>
    {
        bool valid = await CheckEmailExistsAsync(value);
        return (valid, valid ? null : "E-mail not found");
    })
    .Run();
```

### `ChangeDescriptionAsync`

The v5.x `Input` had only the synchronous `ChangeDescription`. v6.x adds the async overload.

```csharp
PromptPlus.Controls.Input("Tax ID:")
    .ChangeDescriptionAsync(async value =>
    {
        if (value.Length < 11) return "Type the 11 digits";
        var name = await LookupNameAsync(value);
        return $"Taxpayer: {name}";
    })
    .Run();
```

---

## Full API reference

### Preserved (unchanged)

| Method | Note |
|---|---|
| `Default(string value, bool useDefaultHistory = true)` | Parameter recased `usedefaultHistory` → `useDefaultHistory` (affects named-argument callers only) |
| `DefaultIfEmpty(string value)` | Unchanged |
| `InputToCase(CaseOptions value)` | **Already existed in v5.x** (not new) |
| `AcceptInput(Func<char, bool>)` | Unchanged |
| `ChangeDescription(Func<string, string>)` | Unchanged |
| `Styles(InputStyles, Style)` | Unchanged |
| `Options(Action<IControlOptions>)` | Unchanged |

### Changed

| v5.x | v6.x | Change |
|---|---|---|
| `MaxLength(int, byte?)` | `MaxLength(int)` | Optional second parameter dropped |
| `SuggestionHandler(Func<string,string[]>)` | `SuggestionHandler(Func<string,string[]>, bool autocomplete = true)` | `autocomplete` parameter added |
| `PredicateSelected(Func<string, bool>)` | `PredicateValid(Func<string, bool>)` | Renamed |
| `PredicateSelected(Func<string, (bool, string?)>)` | `PredicateValid(Func<string, (bool, string?)>)` | Renamed |
| `EnabledHistory(string, Action<IHistoryOptions>?)` | `EnableHistory(string, Action<IHistoryOptions>?)` | Renamed |

### Removed

| v5.x method | Reason |
|---|---|
| `MaxWidth(byte)` | Width is automatic in v6.x |
| `IsSecret(char?, bool)` | Use the dedicated `Secret(...)` control |

### New in v6.x

| Method | Description |
|---|---|
| `SuggestionHandlerAsync(Func<string, Task<string[]>>, bool autocomplete = true)` | Async suggestion provider |
| `MinimumSuggestionLength(byte)` | Minimum characters before suggestions appear |
| `PredicateValidAsync(Func<string, Task<bool>>)` | Async validation |
| `PredicateValidAsync(Func<string, Task<(bool, string?)>>)` | Async validation with error message |
| `ChangeDescriptionAsync(Func<string, Task<string>>)` | Async dynamic description |

> ⚠️ **Correction vs. earlier drafts:** `SuggestionHandlerAsync` is **new in v6.x** — it did **not** exist on the v5.x `Input`, and it is **not** removed. Likewise, `InputToCase` already existed in v5.x and is **not** a v6.x novelty.
