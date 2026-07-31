# Migration v5.x → v6.x: Slider, Calendar, Switch, ProgressBar, ChartBar

> Back to [Migration Overview](../../migration-v5-to-v6.md)

This page covers controls that are **mostly** unchanged. Two of them (**ProgressBar** and **ChartBar**) do have breaking changes — see the notes below.

---

## Slider — three renames only

The v5.x and v6.x `Slider` APIs are otherwise the same (`Range`, `Step`, `LargeStep`, `Default`, `Layout`, `Width`, `Culture`, `ChangeColor`, `ChangeGradient`, `ChangeDescription`, `HideElements`), with these breaking renames:

### Renamed public methods

| v5.x member | v6.x member |
|---|---|
| `Fill(SliderBarType)` | `BarType(SliderBarType)` |
| `FracionalDig(int)` | `FractionalDigits(int)` |
| `EnabledHistory(string, Action<IHistoryOptions>?)` | `EnableHistory(string, Action<IHistoryOptions>?)` |

```csharp
// v5.x
.Fill(SliderBarType.Fill)
.FracionalDig(2)

// v6.x — method renamed; the SliderBarType.Fill enum member is unchanged
.BarType(SliderBarType.Fill)
.FractionalDigits(2)   // spelling fixed (matches ChartBar.FractionalDigits)
```

> Only the **method** `Fill` was renamed to `BarType`. The `SliderBarType.Fill` enum member keeps its name.

> The `Default(double, bool)` parameter was also renamed `usedefaultHistory` → `useDefaultHistory` (only affects named-argument callers).

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;

// v5.x and v6.x — same API
var result = PromptPlus.Controls.Slider("Volume:")
    .Range(0, 100)
    .Default(50)
    .Step(5)
    .ChangeGradient(Color.Green, Color.Yellow, Color.Red)
    .ChangeColor(value => value > 80 ? Style.Plain(Color.Red) : Style.Plain(Color.Green))
    .ChangeDescription(value => $"{value:F0}%")
    .Run();

double? volume = result.Content;   // ResultPrompt<double?>
```

> ⚠️ `ChangeGradient`, `ChangeColor` and `ChangeDescription` already existed on the v5.x `Slider` — they are **not** v6.x novelties.

---

## Calendar — no breaking changes

No method was renamed or removed. v6.x adds async variants and a few helpers.

```csharp
// v5.x — synchronous only
PromptPlus.Controls.Calendar("Date:")
    .PredicateSelected(date => date != null && date.Value.DayOfWeek != DayOfWeek.Sunday)
    .Run();

// v6.x — async is now available
PromptPlus.Controls.Calendar("Date:")
    .PredicateSelectedAsync(async date =>
    {
        if (date is null) return (false, "Invalid date");
        bool available = await CheckAvailabilityAsync(date.Value);
        return (available, available ? null : "Unavailable date");
    })
    .Run();
```

New in v6.x: `PredicateSelectedAsync` (x2) · `ChangeDescriptionAsync` · `InteractionAsync` · `AddNotes(...)` · `EnableHistory(...)`. `Default(DateTime)` gained an optional `bool useDefaultHistory` parameter (source-compatible).

---

## Switch — style-enum + EnableHistory renames only

The `Switch` control methods are otherwise source-compatible, but two style slots were renamed,
one parameter was recased, and `EnabledHistory` was renamed:

```csharp
// v5.x                         // v6.x
SwitchStyles.SliderOn      →    SwitchStyles.SwitchOn
SwitchStyles.SliderOff     →    SwitchStyles.SwitchOff
.Default(true, usedefaultHistory: false)  →  .Default(true, useDefaultHistory: false)
.EnabledHistory("switch_history")  →  .EnableHistory("switch_history")
```

New in v6.x: the `EmojiName` overloads of `OnValue`/`OffValue`, and `ChangeDescriptionAsync`.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;

// v5.x — string overloads and synchronous description
PromptPlus.Controls.Switch("Notifications:")
    .OnValue("On")
    .OffValue("Off")
    .ChangeDescription(value => value ? "Notifications on" : "Notifications off")
    .Run();

// v6.x — new emoji overloads + async description
PromptPlus.Controls.Switch("Dark mode:")
    .OnValue(EmojiName.NewMoon, "Dark")
    .OffValue(EmojiName.Sun, "Light")
    .ChangeDescriptionAsync(async value => (await GetStatusAsync(value)).Description)
    .Run();
```

---

## ProgressBar — ⚠️ has breaking changes

### Breaking change 1: `UpdateHandler` event type changed

The event argument type changed from `HandlerProgressBar` (v5.x) to `ProgressBarEvent` (v6.x), and the context parameter changed from `KeyValuePair<string,object?>[]?` to `IDictionary<string,object?>?`.

```csharp
// v5.x
.UpdateHandler((HandlerProgressBar evt, CancellationToken ct) => { /* ... */ })

// v6.x — ProgressBarEvent exposes Value, Minvalue, Maxvalue, Update(...), Finish, ...
.UpdateHandler((ProgressBarEvent evt, CancellationToken ct) =>
{
    // e.g. evt.Update(newValue);
})
```

### Breaking change 2: `IntervalUpdate(int)` removed

```csharp
// v5.x only — the UI update interval is no longer configurable
.IntervalUpdate(200)
```

### Breaking change 3: `FracionalDig` renamed

```csharp
// v5.x
.FracionalDig(2)

// v6.x — spelling fixed
.FractionalDigits(2)
```

### What's new in v6.x

`UpdateHandlerAsync` and `ChangeDescriptionAsync` are new. `ChangeGradient`, `ChangeColor` and `ChangeDescription` already existed in v5.x.

```csharp
PromptPlus.Controls.ProgressBar("Upload:")
    .Range(0, 100)
    .UpdateHandlerAsync(async (evt, ct) =>
    {
        await SendChunkAsync(evt.Value, ct);
        evt.Update(evt.Value + 10);
    })
    .ChangeDescriptionAsync(async progress => $"{progress:F0}% — ETA {await EstimateEtaAsync(progress)}")
    .Run();
```

`Run()` returns `ResultPrompt<StateProgress>` (unchanged).

---

## ChartBar — ⚠️ has breaking changes

### Breaking change 1: `MaxWidth(byte)` removed

```csharp
// v5.x only — width is automatic in v6.x
.MaxWidth(60)
```

### Breaking change 2: `ChartBarOrder.LabelDec` renamed

```csharp
// v5.x
.OrderBy(ChartBarOrder.LabelDec)

// v6.x — spelling fixed (descending)
.OrderBy(ChartBarOrder.LabelDesc)
```

### Already present in v5.x (not new)

`ChartBar` was **already selectable in v5.x**: it returned `ResultPrompt<ChartItem?>` and already had `PredicateSelected` (x2), `OrderBy`, `ShowLegends`, `FractionalDigits`, `BarType`, `Interaction`, `ChangeDescription` and the `AddItem(string label, double value, Color? colorBar = null, string? id = null)` signature.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;

// v5.x and v6.x — same selection API
var selected = PromptPlus.Controls.ChartBar("Sales by region:")
    .AddItem("North", 120, Color.Blue)
    .AddItem("South", 85, Color.Green)
    .PredicateSelected(item => item.Value > 0)
    .OrderBy(ChartBarOrder.Highest)
    .ShowLegends()
    .Run();

Console.WriteLine($"Selected: {selected.Content?.Label}");   // ChartItem.Label
```

### What's new in v6.x

`PredicateSelectedAsync` (x2), `ChangeDescriptionAsync`, plus interactive switchers `EnableLayoutSwitcher(bool)` / `EnableOrderingSwitcher(bool)`.

```csharp
PromptPlus.Controls.ChartBar("Metrics:")
    .Interaction(metrics, (m, ctrl) => ctrl.AddItem(m.Name, m.Value, m.Color))
    .ChangeDescriptionAsync(async item => await GetMetricDetailAsync(item.Label))
    .Run();
```

---

## History — new in v6.x

v6.x lets you manage a persisted history **directly**, without going through an input control.

```csharp
using PromptPlusLibrary;

IHistory hist = PromptPlus.Controls.History("my_history");

hist.AddHistory("typed value", timeout: TimeSpan.FromDays(30));
hist.Save();

IList<string> entries = hist.ReadHistory<string>();

hist.Remove();   // delete the history file
```

`IHistory`: `AddHistory(string value, TimeSpan? timeout = null)` · `ReadHistory<T>()` · `Save()` · `Remove()`.

---

## Summary

| Control | Breaking changes | New in v6.x |
|---|---|---|
| `Slider` | ⚠️ `FracionalDig` → `FractionalDigits`; `Default` param recased | None |
| `Calendar` | ❌ None | `PredicateSelectedAsync`, `ChangeDescriptionAsync`, `InteractionAsync`, `AddNotes`, `EnableHistory` |
| `Switch` | ⚠️ `SwitchStyles.SliderOn/SliderOff` → `SwitchOn/SwitchOff`; `Default` param recased; `EnabledHistory` → `EnableHistory` | `OnValue/OffValue(EmojiName, string)`, `ChangeDescriptionAsync` |
| `ProgressBar` | ⚠️ `UpdateHandler` event type; `IntervalUpdate` removed; `FracionalDig` → `FractionalDigits` | `UpdateHandlerAsync`, `ChangeDescriptionAsync` |
| `ChartBar` | ⚠️ `MaxWidth` removed; `ChartBarOrder.LabelDec` → `LabelDesc` | `PredicateSelectedAsync` (x2), `ChangeDescriptionAsync`, layout/ordering switchers |
| `History` | — | **New helper** — direct history management |
