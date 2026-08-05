<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **MultiSelect&lt;T&gt; — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [MultiSelect — Operations →](operations.md)

---

Every fluent method on `IMultiSelectControl<T>`. Each returns the same control instance, so calls
chain in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.MultiSelect<T>(string prompt = "", string? description = null)`,
> which returns `IMultiSelectControl<T>`.

**Quick jump:**
[AddItem](#additem) ·
[AddItems](#additems) ·
[AddGroupedItem](#addgroupeditem) ·
[AddGroupedItems](#addgroupeditems) ·
[AddSeparator](#addseparator) ·
[Interaction](#interaction) ·
[InteractionAsync](#interactionasync) ·
[TextSelector](#textselector) ·
[TextSelectorAsync](#textselectorasync) ·
[ExtraInfo](#extrainfo) ·
[ExtraInfoAsync](#extrainfoasync) ·
[HideTipGroup](#hidetipgroup) ·
[Filter](#filter) ·
[PageSize](#pagesize) ·
[Default](#default) ·
[UseDefaultHistory](#usedefaulthistory) ·
[DefaultMatchBy](#defaultmatchby) ·
[Range](#range) ·
[PredicateChecked](#predicatechecked) ·
[PredicateCheckedAsync](#predicatecheckedasync) ·
[ViewOnly](#viewonly) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[EnableHistory](#enablehistory) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Adding items

### `AddItem`

```csharp
IMultiSelectControl<T> AddItem(T value, bool ischecked = false, bool disable = false)
```

Adds a single item. Set `ischecked: true` to pre-check it; set `disable: true` to show it grayed out
and non-selectable.

```csharp
PromptPlus.Controls.MultiSelect<string>("Toppings")
    .AddItem("Cheese", ischecked: true)   // starts checked
    .AddItem("Onions")
    .AddItem("Anchovies", disable: true)  // visible but not selectable
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `AddItems`

```csharp
IMultiSelectControl<T> AddItems(IEnumerable<T> values, bool ischecked = false, bool disable = false)
```

Adds many items at once. `ischecked: true` pre-checks all of them; `disable: true` disables all of
them.

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItems(["Seattle", "London", "Tokyo"])
    .Run();
```

---

### `AddGroupedItem`

```csharp
IMultiSelectControl<T> AddGroupedItem(string group, T value, bool ischecked = false, bool disable = false)
```

Adds one item under a named group header. Pre-check it with `ischecked: true`.

---

### `AddGroupedItems`

```csharp
IMultiSelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool ischecked = false, bool disable = false)
```

Adds many items under a named group header. Items keep their group as they scroll.

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddGroupedItems("North America", ["Seattle", "New York"])
    .AddGroupedItems("Asia",          ["Tokyo", "Singapore"])
    .Run();
```

> A hint shows the group of the focused item; hide it with [`HideTipGroup`](#hidetipgroup).
> Pressing **Space** on a group header toggles every item in that group — see
> [Operations](operations.md#checking-items).

---

### `AddSeparator`

```csharp
IMultiSelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null)
```

Inserts a visual divider between items.

| `SeparatorLine` | Renders |
|---|---|
| `SingleLine` | A single-line rule (default) |
| `DoubleLine` | A double-line rule |
| `UserChar` | A row of the character passed in `value` |

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItem("Seattle")
    .AddSeparator()                          // single line
    .AddItem("Tokyo")
    .AddSeparator(SeparatorLine.DoubleLine)  // double line
    .AddItem("London")
    .AddSeparator(SeparatorLine.UserChar, '*')
    .AddItem("Other")
    .Run();
```

---

## Loading from a source

### `Interaction`

```csharp
IMultiSelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, IMultiSelectControl<T>> interactionAction)
```

Iterates a source collection and lets you add items programmatically — useful when mapping from a
different shape or applying per-item logic.

```csharp
PromptPlus.Controls.MultiSelect<(int id, string City, string other)>("Cities")
    .Interaction(MyCities(), (row, ctrl) => ctrl.AddItem(row))
    .TextSelector(row => row.City)
    .Run();
```

---

### `InteractionAsync`

```csharp
IMultiSelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, IMultiSelectControl<T>, Task> interactionAction)
```

Asynchronous version of [`Interaction`](#interaction), for sources that need awaiting per item. The
returned task is awaited **synchronously (blocking)**.

---

## Item text & info

### `TextSelector`

```csharp
IMultiSelectControl<T> TextSelector(Func<T, string> value)
```

Sets how each item is rendered as text. By default `ToString()` is used (and `[Display]` names for
enums). Provide a selector for custom types.

```csharp
PromptPlus.Controls.MultiSelect<User>("Users")
    .AddItems(users)
    .TextSelector(u => $"{u.Name} <{u.Email}>")
    .Run();
```

---

### `TextSelectorAsync`

```csharp
IMultiSelectControl<T> TextSelectorAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`TextSelector`](#textselector).

---

### `ExtraInfo`

```csharp
IMultiSelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
```

Shows a secondary piece of text for the focused item (return `null` to show nothing). It is wrapped
with the prefix/suffix from config (default `(` `)`). It also appears in the live answer line while
navigating (not in the final checked-items summary shown after **Enter**) — see
[Operations](operations.md#anatomy-of-the-control).

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItems(["Seattle", "Tokyo"])
    .ExtraInfo(city => $"Length: {city.Length}")
    .Run();
```

---

### `ExtraInfoAsync`

```csharp
IMultiSelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
```

Asynchronous version of [`ExtraInfo`](#extrainfo). The task is awaited synchronously each time the
cursor moves.

---

### `HideTipGroup`

```csharp
IMultiSelectControl<T> HideTipGroup(bool value = true)
```

Hides the "current group" hint shown for grouped lists. Default `false` (hint visible).

---

## Filtering & paging

### `Filter`

```csharp
IMultiSelectControl<T> Filter(FilterMode value)
```

Controls live filtering as the user types.

| `FilterMode` | Behavior |
|---|---|
| `Disabled` | No filtering (default) |
| `Contains` | Match items containing the typed text |
| `StartsWith` | Match items starting with the typed text |

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItems(cities)
    .Filter(FilterMode.StartsWith)
    .Run();
```

> When filtering is `Disabled`, a printable key instead **jumps** to the next item starting with that
> character. See [Operations](operations.md#filtering).

---

### `PageSize`

```csharp
IMultiSelectControl<T> PageSize(byte value)
```

Rows visible at once (valid range 0–255). `0` (default) auto-computes from terminal height, reserving
lines for the header, footer, and pagination. Values above the available height are clamped.

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities").AddItems(cities).PageSize(8).Run();
```

---

## Initial value & equality

### `Default`

```csharp
IMultiSelectControl<T> Default(IEnumerable<T> values, bool useDefaultHistory = true)
```

Pre-checks every item that matches a value in `values`, and moves focus to the first match. When
`useDefaultHistory` is `true` and [history](#enablehistory) is enabled, the last history entry is
preferred over `values`. Matching uses [`DefaultMatchBy`](#defaultmatchby) if provided, otherwise
default equality.

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItems(cities)
    .Default(["Tokyo", "Seattle"])
    .Run();
```

> Note the difference from [`Select<T>.Default`](../select/methods.md#default): this takes a
> **collection** of values, since multiple items can start checked.
>
> Throws `ArgumentNullException` if `values` is `null`.

---

### `UseDefaultHistory`

```csharp
IMultiSelectControl<T> UseDefaultHistory()
```

Initializes the checked set from the most recent history entry (when
[`EnableHistory`](#enablehistory) is set), overriding any values supplied by [`Default`](#default).
Has no effect when history is not enabled.

---

### `DefaultMatchBy`

```csharp
IMultiSelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
```

Custom equality used to locate the [`Default`](#default) items (and to compare items in general) —
essential for records/classes where reference equality is not meaningful.

```csharp
PromptPlus.Controls.MultiSelect<(int id, string City, string other)>("Cities")
    .Interaction(MyCities(), (r, c) => c.AddItem(r))
    .TextSelector(r => r.City)
    .DefaultMatchBy((a, b) => a.id == b.id)
    .Default([new(4, "New York", "any4")])
    .Run();
```

> Throws `ArgumentNullException` if `comparer` is `null`.

---

## Selection range

### `Range`

```csharp
IMultiSelectControl<T> Range(int minvalue, int? maxvalue = null)
```

Constrains how many items may be checked. `minvalue` is the minimum required at confirm time;
`maxvalue` (optional) caps the maximum. The range is enforced on **Enter**: too few or too many
checked keeps the list open and shows an error.

| Argument | Meaning |
|---|---|
| `minvalue` | Minimum items that must be checked (0 = optional) |
| `maxvalue` | Maximum items allowed; `null` (default) = no upper bound |

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities", "Min. 2, Max. 3")
    .AddItems(["Seattle", "London", "Tokyo", "New York", "Singapore"])
    .Range(2, 3)
    .Run();
```

> Throws `ArgumentOutOfRangeException` when `minvalue < 0`, or when `maxvalue` is specified and is
> less than `minvalue`.

---

## Validating each check

Validation runs when the user **checks an item** (Space). On failure the check is rejected and the
list shows an error.

### `PredicateChecked`

```csharp
IMultiSelectControl<T> PredicateChecked(Func<T, bool> validselect)
IMultiSelectControl<T> PredicateChecked(Func<T, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<T, bool>` | `true` = valid | Generic error on failure |
| `Func<T, (bool, string?)>` | `(isValid, message)` | Custom `message` on failure |

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItems(["Seattle", "London", "Tokyo"])
    .PredicateChecked(city => city == "Tokyo"
        ? (true, null)
        : (false, "Only Tokyo can be selected"))
    .Run();
```

> Mass operations — [toggle-all (F2)](operations.md#keyboard) and group-header Space — **silently
> skip** items the predicate rejects rather than showing an error.

---

### `PredicateCheckedAsync`

```csharp
IMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<bool>> validselect)
IMultiSelectControl<T> PredicateCheckedAsync(Func<T, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts of the two [`PredicateChecked`](#predicatechecked) overloads.

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Read-only display

### `ViewOnly`

```csharp
IMultiSelectControl<T> ViewOnly(bool value = true)
```

Renders the list for viewing only — checks cannot be changed. Combine with [`Default`](#default) (or
`AddItems(..., ischecked: true)`) to show a fixed set of checked entries.

```csharp
PromptPlus.Controls.MultiSelect<string>("Enabled features (read-only)")
    .AddItems(["Logging", "Caching", "Metrics"], ischecked: true)
    .ViewOnly()
    .Run();
```

---

## Dynamic description

### `ChangeDescription`

```csharp
IMultiSelectControl<T> ChangeDescription(Func<T, string> value)
```

Recomputes the description from the **currently focused item** as the user navigates.

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItems(cities)
    .ChangeDescription(city => $"You are on: {city}")
    .Run();
```

---

### `ChangeDescriptionAsync`

```csharp
IMultiSelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

---

## History

### `EnableHistory`

```csharp
IMultiSelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists confirmed selections to `filename` and can restore them (via [`Default`](#default) or
[`UseDefaultHistory`](#usedefaulthistory)). The `IHistoryOptions` builder is identical to the one
documented for [Input → EnableHistory](../input/methods.md#enablehistory) (`MinPrefixLength`,
`MaxItems`, `ExpirationTime`, `FilterType`, `PageSize`).

```csharp
PromptPlus.Controls.MultiSelect<string>("Cities")
    .AddItems(cities)
    .EnableHistory("city-history", opt => opt.MaxItems(8).FilterType(FilterMode.StartsWith))
    .UseDefaultHistory()
    .Run();
```

> Throws `ArgumentNullException` if `filename` is `null`.

---

## Appearance & behavior

### `Styles`

```csharp
IMultiSelectControl<T> Styles(MultiSelectStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.MultiSelect<string>("Cities").AddItems(cities)
    .Styles(MultiSelectStyles.Selected, new Style(Color.Blue, Color.Default))
    .Run();
```

---

### `Options`

```csharp
IMultiSelectControl<T> Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish, extra-info affixes). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<T[]> Run(CancellationToken token = default)
```

Renders the list and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<T[]>`](../../architecture.md#resultpromptt) — the array of checked items (an empty
array when aborted or nothing was checked).

```csharp
var result = PromptPlus.Controls.MultiSelect<string>("Cities").AddItems(cities).Run();
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `MultiSelectStyles` regions
- [Index](index.md) — overview and method map
- [Select → Methods](../select/methods.md) — the single-choice sibling's API
