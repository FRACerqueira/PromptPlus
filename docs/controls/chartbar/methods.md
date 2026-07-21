<div align="center">
  <img src="../../../icon.png" alt="PromptPlus" width="120" height="120" />

  # PromptPlus

  ## **ChartBar — Methods**

  [![NuGet](https://img.shields.io/badge/NuGet-PromptPlus-blue)](https://www.nuget.org/packages/PromptPlus)
  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
  [![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)

</div>

[← Back to Home](../../../README.md) • **Next:** [ChartBar — Operations →](operations.md)

---

Every fluent method on `IChartBarControl`. Each returns the same control instance, so calls chain in
any order. Call [`Run`](#run) last.

> The factory is `PromptPlus.Controls.ChartBar(string prompt = "", string? description = null)`,
> which returns `IChartBarControl`.

**Quick jump:**
[AddItem](#additem) ·
[Interaction](#interaction) ·
[Layout](#layout) ·
[BarType](#bartype) ·
[Width](#width) ·
[Title](#title) ·
[Culture](#culture) ·
[FractionalDigits](#fractionaldigits) ·
[MaxLengthLabel](#maxlengthlabel) ·
[OrderBy](#orderby) ·
[ShowLegends](#showlegends) ·
[HideElements](#hideelements) ·
[PageSize](#pagesize) ·
[EnableLayoutSwitcher](#enablelayoutswitcher) ·
[EnableOrderingSwitcher](#enableorderingswitcher) ·
[PredicateSelected](#predicateselected) ·
[PredicateSelectedAsync](#predicateselectedasync) ·
[ChangeDescription](#changedescription) ·
[ChangeDescriptionAsync](#changedescriptionasync) ·
[Styles](#styles) ·
[Options](#options) ·
[Run](#run)

---

## Adding items

### `AddItem`

```csharp
IChartBarControl AddItem(string label, double value, Color? colorBar = null, string? id = null)
```

Adds one bar. `label` is required; `value` drives the bar length and percentage. `colorBar` sets the
bar color — omit it and colors are auto-assigned in a rotating sequence. `id` is an optional
identifier carried through to the returned [`ChartItem`](index.md#the-chartitem-type).

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color lives here

PromptPlus.Controls.ChartBar("Select item")
    .AddItem("North", 120, Color.Green, id: "n")
    .AddItem("South", 80)   // auto color
    .Run();
```

> Throws `ArgumentException` if `label` is `null` or empty.

There is no `AddItems` — add many bars with a loop or with [`Interaction`](#interaction).

---

### `Interaction`

```csharp
IChartBarControl Interaction<T>(IEnumerable<T> items, Action<T, IChartBarControl> interactionaction)
```

Iterates a source collection and lets you add bars programmatically — the equivalent of calling
`AddItem` inside a loop.

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .Interaction(regions, (row, ctrl) => ctrl.AddItem(row.Name, row.Total))
    .Run();
```

> Throws `ArgumentNullException` if `items` or `interactionaction` is `null`.

---

## Chart layout & bars

### `Layout`

```csharp
IChartBarControl Layout(ChartBarLayout layout = ChartBarLayout.Standard)
```

Sets the initial layout.

| `ChartBarLayout` | Renders |
|---|---|
| `Standard` | One horizontal bar per item, with its own label (default) |
| `Stacked` | All items in a single continuous bar |

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("CPU", 45).AddItem("Memory", 70).AddItem("Disk", 30)
    .Layout(ChartBarLayout.Stacked)
    .Run();
```

> Switching to `Stacked` needs enough console width to render every item. If the console is too
> narrow the switch is silently prevented. Users can also toggle layout at runtime with **F2** unless
> disabled — see [`EnableLayoutSwitcher`](#enablelayoutswitcher).

---

### `BarType`

```csharp
IChartBarControl BarType(ChartBarType type = ChartBarType.Fill)
```

Chooses the glyph used to draw the bars.

| `ChartBarType` | Renders |
|---|---|
| `Fill` | Solid filled bar using background color (default) |
| `Light` | Light shade character (`░`) |
| `Square` | Medium shade character (`▪`) |

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .BarType(ChartBarType.Light)
    .AddItem("Product 1", 55).AddItem("Product 2", 90)
    .Run();
```

---

### `Width`

```csharp
IChartBarControl Width(byte value)
```

Sets the drawing width of the chart in characters. Default is `50`.

```csharp
PromptPlus.Controls.ChartBar("Select item").Width(70).AddItem("A", 45).Run();
```

> Throws `ArgumentOutOfRangeException` if `value` is less than `10`.

---

### `Title`

```csharp
IChartBarControl Title(string title, TextAlignment alignment = TextAlignment.Center)
```

Adds a title line above the chart, aligned `Left`, `Center` (default), or `Right`.

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .Title("Sales by Region", TextAlignment.Center)
    .AddItem("North", 120).AddItem("South", 80)
    .Run();
```

> Throws `ArgumentException` if `title` is `null` or empty. Hide an already-set title at runtime with
> [`HideElements(HideChart.Title)`](#hideelements).

---

## Values & labels

### `Culture`

```csharp
IChartBarControl Culture(CultureInfo culture)
IChartBarControl Culture(string cultureName)
```

Sets the culture used to format numeric values. Defaults to the current PromptPlus culture.

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .Culture("en-US")
    .AddItem("A", 45.678)
    .Run();
```

> The `CultureInfo` overload throws `ArgumentNullException` if `culture` is `null`; the string
> overload throws `ArgumentException` if `cultureName` is `null` or empty.

---

### `FractionalDigits`

```csharp
IChartBarControl FractionalDigits(byte value)
```

Number of fractional digits shown for values. Default is `2`.

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .FractionalDigits(2)
    .AddItem("Value A", 45.678).AddItem("Value B", 82.123)
    .Run();
```

> Throws `ArgumentOutOfRangeException` if `value` is greater than `5`.

---

### `MaxLengthLabel`

```csharp
IChartBarControl MaxLengthLabel(byte value = 0)
```

Maximum number of characters shown for each label. `0` (default) shows labels in full with no
truncation.

```csharp
PromptPlus.Controls.ChartBar("Select item").MaxLengthLabel(12).AddItem("A really long label", 40).Run();
```

---

## Ordering & legend

### `OrderBy`

```csharp
IChartBarControl OrderBy(ChartBarOrder order)
```

Sets the initial sort order of the bars.

| `ChartBarOrder` | Sorts by |
|---|---|
| `None` | Insertion order (default) |
| `Highest` | Value, descending (highest first) |
| `Smallest` | Value, ascending (smallest first) |
| `LabelAsc` | Label, A → Z |
| `LabelDesc` | Label, Z → A |

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("Low", 20).AddItem("High", 90).AddItem("Medium", 50)
    .OrderBy(ChartBarOrder.Highest)
    .Run();
```

> Users can cycle the order at runtime with **F4** unless disabled — see
> [`EnableOrderingSwitcher`](#enableorderingswitcher). The method is `OrderBy` (not `Order`).

---

### `ShowLegends`

```csharp
IChartBarControl ShowLegends(bool value = true)
```

Shows a legend section listing each item with its value and percentage after the chart. Default is
`false` (hidden).

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("Q1", 100).AddItem("Q2", 120)
    .ShowLegends()
    .Run();
```

> Users can toggle the legend at runtime with **F3**. The method is `ShowLegends` (not `ShowLegend`).

---

## Hiding & paging

### `HideElements`

```csharp
IChartBarControl HideElements(HideChart value)
```

Hides one or more chart elements. `HideChart` is a `[Flags]` enum, so combine values with `|`.

| `HideChart` | Hides |
|---|---|
| `None` | Nothing (default — all elements shown) |
| `Title` | The chart title |
| `Values` | The numeric values on bars |
| `Percentage` | The percentage values on bars |

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("Data 1", 40).AddItem("Data 2", 75)
    .HideElements(HideChart.Title | HideChart.Percentage)
    .Run();
```

---

### `PageSize`

```csharp
IChartBarControl PageSize(byte value)
```

Maximum number of bars shown per page. Default `0` disables pagination (all items on one view).

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .Interaction(manyRows, (r, c) => c.AddItem(r.Name, r.Value))
    .PageSize(10)
    .Run();
```

---

## Runtime switchers

### `EnableLayoutSwitcher`

```csharp
IChartBarControl EnableLayoutSwitcher(bool value = true)
```

Enables or disables the **F2** hotkey that toggles between `Standard` and `Stacked` layouts at
runtime. Default enabled.

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("A", 40).AddItem("B", 85)
    .EnableLayoutSwitcher(false)   // F2 no longer switches layout
    .Run();
```

---

### `EnableOrderingSwitcher`

```csharp
IChartBarControl EnableOrderingSwitcher(bool value = true)
```

Enables or disables the **F4** hotkey that cycles through sort orders at runtime. Default enabled.

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("Low", 20).AddItem("High", 90)
    .EnableOrderingSwitcher(false)   // F4 no longer cycles order
    .Run();
```

---

## Validating the confirmed item

Validation runs on **Enter**. On failure the chart stays open and shows an error.

### `PredicateSelected`

```csharp
IChartBarControl PredicateSelected(Func<ChartItem, bool> validselect)
IChartBarControl PredicateSelected(Func<ChartItem, (bool, string?)> validselect)
```

| Overload | Return | Behavior |
|---|---|---|
| `Func<ChartItem, bool>` | `true` = valid | Generic error on failure |
| `Func<ChartItem, (bool, string?)>` | `(isValid, message)` | Custom `message` on failure |

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("Valid", 80).AddItem("Invalid", 30).AddItem("Valid", 60)
    .PredicateSelected(item => item.Value < 50
        ? (false, "Value must be >= 50")
        : (true, null))
    .Run();
```

> Throws `ArgumentNullException` if `validselect` is `null`.

---

### `PredicateSelectedAsync`

```csharp
IChartBarControl PredicateSelectedAsync(Func<ChartItem, Task<bool>> validselect)
IChartBarControl PredicateSelectedAsync(Func<ChartItem, Task<(bool, string?)>> validselect)
```

Asynchronous counterparts.

> ⚠️ The async predicate is awaited **synchronously (blocking) on the UI thread** — keep it fast.

---

## Dynamic description

### `ChangeDescription`

```csharp
IChartBarControl ChangeDescription(Func<ChartItem, string> value)
```

Recomputes the description from the **currently highlighted item** as the user navigates.

```csharp
PromptPlus.Controls.ChartBar("Select item")
    .AddItem("Option 1", 55).AddItem("Option 2", 75)
    .ChangeDescription(item => $"Selected: {item.Label} ({item.Percent:F1}%)")
    .Run();
```

> Throws `ArgumentNullException` if `value` is `null`.

---

### `ChangeDescriptionAsync`

```csharp
IChartBarControl ChangeDescriptionAsync(Func<ChartItem, Task<string>> value)
```

Asynchronous version of [`ChangeDescription`](#changedescription), awaited synchronously each frame.

---

## Appearance & behavior

### `Styles`

```csharp
IChartBarControl Styles(ChartBarStyles styleType, Style style)
```

Recolors one visual region of this control. See the region list and examples on the
[Styles](styles.md) page.

```csharp
using PromptPlusLibrary;
using ConsolePlusLibrary;   // Color, Style live here
PromptPlus.Controls.ChartBar("Select item").AddItem("A", 40)
    .Styles(ChartBarStyles.ChartTitle, new Style(Color.Yellow, Color.Default))
    .Run();
```

---

### `Options`

```csharp
IChartBarControl Options(Action<IControlOptions> options)
```

Overrides global behaviors for this one control (prompt/description text, abort key, tooltip,
hide-after-finish). See
[Global Behaviors → Per-Control Override](../../global-behaviors.md#per-control-override--icontroloptions).

> Throws `ArgumentNullException` if `options` is `null`.

---

## Running the control

### `Run`

```csharp
ResultPrompt<ChartItem?> Run(CancellationToken token = default)
```

Renders the chart and blocks until the user confirms (**Enter**) or aborts (**Esc**). Returns
[`ResultPrompt<ChartItem?>`](../../architecture.md#resultpromptt) carrying the highlighted
[`ChartItem`](index.md#the-chartitem-type).

```csharp
var result = PromptPlus.Controls.ChartBar("Select item").AddItem("A", 40).Run();
```

---

## See also

- [Operations](operations.md) — how these methods behave at runtime
- [Styles](styles.md) — the `ChartBarStyles` regions
- [Index](index.md) — overview and method map
