<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **Select&lt;T&gt; — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [Select — Operations →](operations.md)

---

Every fluent method on `ISelectControl<T>`. Each returns the same control instance, so calls chain
in any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.Select<T>(string prompt = "", string? description = null)`,
> which returns `ISelectControl<T>`.

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
[AutoSelect](#autoselect) ·
[PageSize](#pagesize) ·
[Default](#default) ·
[UseDefaultHistory](#usedefaulthistory) ·
[DefaultMatchBy](#defaultmatchby) ·
[PredicateSelected](#predicateselected) ·
[PredicateSelectedAsync](#predicateselectedasync) ·
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
ISelectControl<T> AddItem(T value, bool disable = false)
```

Adds a single item. Set `disable: true` to show it grayed out and non-selectable.

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItem("Tokyo")
    .AddItem("London", disable: true)   // visible but not selectable
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `AddItems`

```csharp
ISelectControl<T> AddItems(IEnumerable<T> values, bool disable = false)
```

Adds many items at once. `disable: true` disables all of them.

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(["Seattle", "London", "Tokyo"])
    .Run();
```

---

### `AddGroupedItem`

```csharp
ISelectControl<T> AddGroupedItem(string group, T value, bool disable = false)
```

Adds one item under a named group header.

---

### `AddGroupedItems`

```csharp
ISelectControl<T> AddGroupedItems(string group, IEnumerable<T> values, bool disable = false)
```

Adds many items under a named group header. Items keep their group as they scroll.

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddGroupedItems("North America", ["Seattle", "New York"])
    .AddGroupedItems("Asia",          ["Tokyo", "Singapore"])
    .Run();
```

> A hint shows the group of the focused item; hide it with [`HideTipGroup`](#hidetipgroup).

---

### `AddSeparator`

```csharp
ISelectControl<T> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null)
```

Inserts a visual divider between items.

| `SeparatorLine` | Renders |
|---|---|
| `SingleLine` | A single-line rule (default) |
| `DoubleLine` | A double-line rule |
| `UserChar` | A row of the character passed in `value` |

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItem("Seattle")
    .AddSeparator()                          // single line
    .AddItem("Tokyo")
    .AddSeparator(SeparatorLine.DoubleLine)  // double line
    .AddItem("London")
    .AddSeparator(SeparatorLine.UserChar, '*')
    .AddItem("Other")
    .Run();
```

> Throws `ArgumentNullException` if `separatorLine` is `SeparatorLine.UserChar` and `value` is `null`.

---

## Loading from a source

### `Interaction`

```csharp
ISelectControl<T> Interaction<T1>(IEnumerable<T1> items, Action<T1, ISelectControl<T>> interactionAction)
```

Iterates a source collection and lets you add items programmatically — useful when mapping from a
different shape or applying per-item logic.

```csharp
PromptPlus.Controls.Select<(int id, string City, string other)>("City")
    .Interaction(MyCities(), (row, ctrl) => ctrl.AddItem(row))
    .TextSelector(row => row.City)
    .Run();
```

---

### `InteractionAsync`

```csharp
ISelectControl<T> InteractionAsync<T1>(IEnumerable<T1> items, Func<T1, ISelectControl<T>, Task> interactionAction)
```

Asynchronous version of [`Interaction`](#interaction), for sources that need awaiting per item.

---

## Item text & info

### `TextSelector`

```csharp
ISelectControl<T> TextSelector(Func<T, string> value)
```

Sets how each item is rendered as text. By default `ToString()` is used (and `[Display]` names for
enums). Provide a selector for custom types.

```csharp
PromptPlus.Controls.Select<User>("User")
    .AddItems(users)
    .TextSelector(u => $"{u.Name} <{u.Email}>")
    .Run();
```

---

### `TextSelectorAsync`

```csharp
ISelectControl<T> TextSelectorAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`TextSelector`](#textselector).

---

### `ExtraInfo`

```csharp
ISelectControl<T> ExtraInfo(Func<T, string?> extraInfoNode)
```

Shows a secondary piece of text for each item (return `null` to show nothing for that item). It is
wrapped with the prefix/suffix from config (default `" ("` — with a leading space — and `")"`). The
focused item's `ExtraInfo` also
appears in the live answer line while navigating (not in the final answer shown after **Enter**) —
see [Operations](operations.md#anatomy-of-the-control).

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(["Seattle", "Tokyo"])
    .ExtraInfo(city => $"Length: {city.Length}")
    .Run();
```

---

### `ExtraInfoAsync`

```csharp
ISelectControl<T> ExtraInfoAsync(Func<T, Task<string?>> extraInfoNode)
```

Asynchronous version of [`ExtraInfo`](#extrainfo).

---

### `HideTipGroup`

```csharp
ISelectControl<T> HideTipGroup(bool value = true)
```

Hides the "current group" hint shown for grouped lists. Default `false` (hint visible).

---

## Filtering & paging

### `Filter`

```csharp
ISelectControl<T> Filter(FilterMode value)
```

Controls live filtering as the user types.

| `FilterMode` | Behavior |
|---|---|
| `Disabled` | No filtering (default) |
| `Contains` | Match items containing the typed text |
| `StartsWith` | Match items starting with the typed text |

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(cities)
    .Filter(FilterMode.Contains)
    .Run();
```

---

### `AutoSelect`

```csharp
ISelectControl<T> AutoSelect(bool value = true)
```

When filtering narrows the list to a **single** selectable item, that item is selected and confirmed
automatically — no Enter needed.

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(["Seattle", "London", "Tokyo"])
    .Filter(FilterMode.StartsWith)
    .AutoSelect()   // typing "T" auto-picks Tokyo
    .Run();
```

---

### `PageSize`

```csharp
ISelectControl<T> PageSize(byte value)
```

Rows visible at once (valid range 0–255). `0` (default) auto-computes from terminal height,
reserving lines for the header, footer, and pagination. Values above the available height are
clamped.

```csharp
PromptPlus.Controls.Select<string>("City").AddItems(cities).PageSize(8).Run();
```

---

## Initial value & equality

### `Default`

```csharp
ISelectControl<T> Default(T value, bool useDefaultHistory = true)
```

Pre-highlights `value`. When `useDefaultHistory` is `true` and [history](#enablehistory) is enabled,
the last history value is preferred. Matching uses [`DefaultMatchBy`](#defaultmatchby) if provided,
otherwise default equality.

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(cities)
    .Default("Tokyo")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `UseDefaultHistory`

```csharp
ISelectControl<T> UseDefaultHistory()
```

Sets the initial selection from the history store (when [`EnableHistory`](#enablehistory) is set),
without also passing an explicit `Default`.

---

### `DefaultMatchBy`

```csharp
ISelectControl<T> DefaultMatchBy(Func<T, T, bool> comparer)
```

Custom equality used to locate the `Default` item (and to compare items in general) — essential for
records/classes where reference equality is not meaningful.

```csharp
PromptPlus.Controls.Select<(int id, string City, string other)>("City")
    .Interaction(MyCities(), (r, c) => c.AddItem(r))
    .TextSelector(r => r.City)
    .DefaultMatchBy((a, b) => a.id == b.id)
    .Default(new(4, "New York", "any4"))
    .Run();
```

> Throws `ArgumentNullException` if `comparer` is `null`.

---

## Validating the confirmed item

Validation runs on **Enter**. On failure the list stays open and shows an error.

### `PredicateSelected`

```csharp
ISelectControl<T> PredicateSelected(Func<T, bool> validselect)
ISelectControl<T> PredicateSelected(Func<T, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<T, bool>` | `true` = valid | Generic error on failure |
| `Func<T, (bool, string?)>` | `(isValid, message)` | Custom `message` on failure |

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(["Seattle", "London", "Tokyo"])
    .PredicateSelected(c => c == "Tokyo"
        ? (true, null)
        : (false, "Only Tokyo can be selected"))
    .Run();
```

---

### `PredicateSelectedAsync`

```csharp
ISelectControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect)
ISelectControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts.

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Read-only display

### `ViewOnly`

```csharp
ISelectControl<T> ViewOnly(bool value = true)
```

Renders the list for viewing only — items cannot be selected. Combine with [`Default`](#default) to
highlight one entry.

```csharp
PromptPlus.Controls.Select<string>("Servers (read-only)")
    .AddItems(servers)
    .Default("web-01")
    .ViewOnly()
    .Run();
```

---

## Dynamic description

### `ChangeDescription`

```csharp
ISelectControl<T> ChangeDescription(Func<T, string> value)
```

Recomputes the description from the **currently focused item** as the user navigates.

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(cities)
    .ChangeDescription(city => $"You are on: {city}")
    .Run();
```

---

### `ChangeDescriptionAsync`

```csharp
ISelectControl<T> ChangeDescriptionAsync(Func<T, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription).

---

## History

### `EnableHistory`

```csharp
ISelectControl<T> EnableHistory(string filename, Action<IHistoryOptions>? options = null)
```

Persists confirmed selections to `filename` and can pre-select the last one (via
[`Default`](#default) or [`UseDefaultHistory`](#usedefaulthistory)). The `IHistoryOptions` builder is
identical to the one documented for
[Input → EnableHistory](../input/methods.md#enablehistory) (`MinPrefixLength`, `MaxItems`,
`ExpirationTime`, `FilterType`, `PageSize`).

```csharp
PromptPlus.Controls.Select<string>("City")
    .AddItems(cities)
    .EnableHistory("city-history", opt => opt.MaxItems(8).FilterType(FilterMode.StartsWith))
    .UseDefaultHistory()
    .Run();
```

---

## Appearance & behavior

### `Styles`

```csharp
ISelectControl<T> Styles(SelectStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.Select<string>("City").AddItems(cities)
    .Styles(SelectStyles.Selected, new Style(Color.Blue, Color.Default))
    .Run();
```

---

### `Options`

```csharp
ISelectControl<T> Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish, extra-info affixes). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

---

## Running the control

### `Run`

```csharp
ResultPrompt<T> Run(CancellationToken token = default)
```

Renders the list and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<T>`](../../architecture.md#resultpromptt).

```csharp
var result = PromptPlus.Controls.Select<string>("City").AddItems(cities).Run();
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `SelectStyles` regions
- [Index](index.md) — overview and method map
