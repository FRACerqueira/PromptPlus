# Migration Guide v5.x → v6.x (PromptPlus)

[← Docs Index](index.md) • [Getting Started](getting-started.md)

> **Direction**: v5.x (source) → v6.x (target)
> Scope: only changes visible to the **end user** (public API).

## How controls are created

In **both** v5.x and v6.x, controls are created through the `PromptPlus.Controls` accessor:

```csharp
using PromptPlusLibrary;

var result = PromptPlus.Controls.Input("What is your name?").Run();
```

> ⚠️ Every code example in this guide uses `PromptPlus.Controls.<Control>(...)`. There is **no** direct `PromptPlus.<Control>(...)` shortcut.

## Reading a result

`Run()` returns `ResultPrompt<T>`, which exposes **`Content`** (the value) and **`IsAborted`**. It can also be deconstructed:

```csharp
var result = PromptPlus.Controls.Input("Name").Run();
if (!result.IsAborted)
    Console.WriteLine(result.Content);

// or, by deconstruction:
var (name, aborted) = PromptPlus.Controls.Input("Name").Run();
```

> ⚠️ `ResultPrompt<T>` has **no** `.Value` member — use `.Content`. (The `Table` controls wrap their payload in `TableResult<T>`, which *does* have `.Value`; see [Table / MultiTable](migration/controls/table.md).)

## Per-control sub-pages

| Control | Link |
|---|---|
| Input / AutoComplete | [migration/controls/input.md](migration/controls/input.md) |
| Select / MultiSelect | [migration/controls/select.md](migration/controls/select.md) |
| KeyPress / Confirm | [migration/controls/keypress.md](migration/controls/keypress.md) |
| Table / MultiTable | [migration/controls/table.md](migration/controls/table.md) |
| Tree / MultiTree | [migration/controls/tree.md](migration/controls/tree.md) |
| File / MultiFile | [migration/controls/file.md](migration/controls/file.md) |
| MultiTasks / Time / Task | [migration/controls/tasks.md](migration/controls/tasks.md) |
| MaskEdit (all types) | [migration/controls/maskedit.md](migration/controls/maskedit.md) |
| Slider / Calendar / Switch / ProgressBar / ChartBar | [migration/controls/nochanges.md](migration/controls/nochanges.md) |

---

## 🔄 Renamed factory methods

| v5.x | v6.x | Notes |
|---|---|---|
| `TableSelect<T>()` | `Table<T>()` | Renamed; `AddColumn` reworked |
| `TableMultiSelect<T>()` | `MultiTable<T>()` | Renamed; `AddColumn` reworked |
| `NodeTreeSelect<T>()` | `Tree<T>()` | Renamed; tree-building API reworked (`AddRootNode`/`AddChildNode` → `Root`/`AddLast`/`AddFirst`) |
| `NodeTreeMultiSelect<T>()` | `MultiTree<T>()` | Renamed; same tree-building rework |
| `WaitProcess()` | `MultiTasks()` | Renamed; `AddTask` fully reworked |
| `WaitTimer()` | `Time()` | Renamed; API fully reworked |
| `WaitCommand()` | `Task()` | Renamed; `CommandHandler` replaced by a cancellable `Action` |
| `FileSelect()` | `File()` | Renamed; visibility methods renamed |
| `FileMultiSelect()` | `MultiFile()` | Renamed; new navigation methods added |
| `AutoComplete()` | `Input().SuggestionHandler(func, autocomplete:false)` | Dedicated control removed; folded into `Input` |
| `InputEmacs()` → `IEmacs` | `ConsolePlus.ReadLineEmacs(...)` (ConsolePlus library) | Emacs-style line editing moved out of PromptPlus into ConsolePlus |

> `AutoComplete` in v5.x is a **non-generic** control (`AutoComplete()`), not `AutoComplete<T>()`. See [Input / AutoComplete](migration/controls/input.md).

> **Emacs / ReadLine.** v5.x had `PromptPlus.Controls.InputEmacs(...)` (returning `IEmacs`). It was **removed** from PromptPlus in v6.x; equivalent Emacs-style line editing now lives in the sibling **ConsolePlus** library — `ConsolePlus.ReadLineEmacs(...)` / `ReadInlineEmacs(...)`, the `EnabledEmacsKeyBindings()` / `DisabledEmacsKeyBindings()` toggles, and plain `ConsolePlus.ReadLine()`.

---

## 🔴 Global breaking change — `MaxWidth(byte)` removed

`MaxWidth(byte)` was **removed from the public API** in v6.x. In v5.x it existed on:

`Input` · `AutoComplete` · `Select<T>` · `MultiSelect<T>` · `NodeTreeSelect<T>` · `NodeTreeMultiSelect<T>` · `FileSelect` · `FileMultiSelect` · `TableSelect<T>` · `TableMultiSelect<T>` · `ChartBar`

> The width is now managed **automatically** by the prompt's answer area.

---

## 🔴 Breaking changes — renamed methods

| Control v5.x | Method v5.x | Method v6.x | Detail |
|---|---|---|---|
| `Select<T>` | `EqualItems(Func<T,T,bool>)` | `DefaultMatchBy(Func<T,T,bool>)` | Renamed, same signature |
| `Select<T>` / `MultiSelect<T>` | `OnlyView(bool)` | `ViewOnly(bool)` | Renamed, same semantics |
| `Select<T>` / `MultiSelect<T>` | `DefaultHistory(bool)` | `UseDefaultHistory()` | Parameter dropped; always enables the default history |
| `MultiSelect<T>` | `EqualItems(Func<T,T,bool>)` | `DefaultMatchBy(Func<T,T,bool>)` | Renamed, same signature |
| `KeyPress` | `AddKeyValid(key, modifiers, showtext)` | `AddValidKey(key, modifiers, displayText)` | Method and parameter renamed |
| `WaitProcess` → `MultiTasks` | `MaxDegreeProcess(byte)` | `MaxDegreeOfParallelism(int)` | Renamed and type widened `byte` → `int` |
| `TableSelect` → `Table` | `Layout(TableLayout)` | `LayoutMode(TableLayoutMode)` | Renamed; enum renamed too |
| `TableSelect` → `Table` | `HideHeaders(bool)` | `HideElements(HideTable)` | Replaced by a flags enum that hides several regions |
| `FileSelect` → `File` | `AcceptHiddenAttributes(bool)` | `ShowHidden(bool)` | Renamed (same effect: `true` makes hidden entries visible) |
| `FileSelect` → `File` | `AcceptSystemAttributes(bool)` | `ShowSystem(bool)` | Renamed (same effect as above) |
| `File` / `MultiFile` | `HideSizeInfo(bool)` | `HideSize(bool)` | Renamed, same semantics |
| `WaitTimer` → `Time` | `IsCountDown(bool)` | `DisplayMode(TimeDisplayMode)` | Replaced by an enum (`Countdown`/`Elapsed`) |
| `Slider` / `ProgressBar` | `FracionalDig(byte)` | `FractionalDigits(byte)` | Renamed (spelling fix); matches `ChartBar.FractionalDigits` |

### Renamed enum values

| Enum | v5.x | v6.x | Notes |
|---|---|---|---|
| `SwitchStyles` | `SliderOn` / `SliderOff` | `SwitchOn` / `SwitchOff` | Style slots renamed to match the control |
| `ChartBarOrder` | `LabelDec` | `LabelDesc` | Spelling fix (descending) |

### Renamed parameter (named-argument callers only)

| Method | v5.x parameter | v6.x parameter |
|---|---|---|
| `Input.Default` · `Slider.Default` · `Switch.Default` | `usedefaultHistory` | `useDefaultHistory` |

---

## 🔴 Breaking changes — changed method signatures

| Control v5.x | Method v5.x | Method v6.x | Detail |
|---|---|---|---|
| `Select<T>` | `Filter(FilterMode, bool caseinsensitive)` | `Filter(FilterMode)` | `caseinsensitive` parameter removed |
| `Table` / `MultiTable` | `Filter(FilterMode, bool caseinsensitive)` | `Filter(FilterMode, FilterTableMode)` | Second parameter changed from `bool` to enum `FilterTableMode` |
| `Table` / `MultiTable` | `AddColumn(string title, int width, Func<T,string> rowvalue, TextAlignment rowAlignment, TextAlignment titleAlignment, bool titlereplaceswidth, int maxslidinglines)` | `AddColumn(string header, Func<T,object> selector, Func<object,string>? formatter, int? width, ColumnAlignment alignment, bool isFilterable)` | Full rework — `object` selector, optional formatter, `TextAlignment` → `ColumnAlignment`, new `isFilterable` |
| `Table` / `MultiTable` | `ChangeDescription(Func<T,int,int,string>)` | `ChangeDescription(Func<T,string>)` | Row/column indices dropped |
| `WaitProcess` → `MultiTasks` | `AddTask(TaskMode mode, string id, Action<object?,ExtraInfoProcess,CancellationToken> process, string? label, object? parameter)` | `AddTask(string title, Func<IReadOnlyDictionary<string,object?>, CancellationToken, IDictionary<string,object?>?> handler, IDictionary<string,object?>? context, MultiTasksMode? mode)` | Full rework — `ExtraInfoProcess` removed, context as a dictionary, `TaskMode` → `MultiTasksMode` |
| `WaitProcess` → `MultiTasks` | `Run()` → `ResultPrompt<StateProcess[]>` | `Run()` → `ResultPrompt<StateMultiTasks>` | Return type changed |
| `WaitTimer` → `Time` | duration passed to the factory: `WaitTimer(int ms, …)` / `WaitTimer(TimeSpan, …)` | `Time().Duration(TimeSpan)` / `Duration(int seconds)` | Duration is now a fluent method |
| `WaitTimer` → `Time` | `ShowElapsedTime(int milliseconds, bool value)` | Removed | Time is displayed automatically; use `Format(...)` |
| `WaitTimer` → `Time` | `Run()` → `ResultPrompt<TimeSpan?>` | `Run()` → `ResultPrompt<TimeSpan>` | No longer nullable |
| `WaitCommand` → `Task` | `CommandHandler(Action commandaction)` | `Action(Action<CancellationToken> handler)` | Renamed; handler now receives a `CancellationToken` |
| `WaitCommand` → `Task` | `ShowElapsedTime(int milliseconds, bool value)` | `ShowElapsedTime(bool value, string? format)` | Interval removed; optional display format added |
| `WaitCommand` → `Task` | `Finish(string text)` | `Finish(string finishtext, string? errortext)` | Optional error text added |
| `WaitCommand` → `Task` | `Run()` → `ResultPrompt<Exception?>` | `Run()` → `ResultPrompt<StateTask>` | Return type changed |
| `ProgressBar` | `UpdateHandler(Action<HandlerProgressBar, CancellationToken>, KeyValuePair<string,object?>[]?)` | `UpdateHandler(Action<ProgressBarEvent, CancellationToken>, IDictionary<string,object?>?)` | Event type `HandlerProgressBar` → `ProgressBarEvent`; context type changed |

---

## 🔴 Breaking changes — methods removed with no v6.x equivalent

| Control v5.x | Removed method | Impact |
|---|---|---|
| `Input` / `AutoComplete` | `MaxWidth(byte)` | Width is now automatic |
| `KeyPress` | `Timeout(TimeSpan/int, ConsoleKey, ConsoleModifiers?)` | No built-in timeout with a default key |
| `KeyPress` | `ShowCountDown(bool)` | Countdown display not available |
| `KeyPress` | `ShowInvalidKey(bool)` | Invalid-key display toggle not available |
| `KeyPress` | `Spinner(SpinnersType)` | Spinner on KeyPress not available |
| `MultiSelect<T>` | `HideCountSelected(bool)` | Selected-count is always shown |
| `WaitProcess` → `MultiTasks` | `Finish(Func<IEnumerable<StateProcess>,string>)` | Dynamic finish text removed; see `StopOnError(bool)` |
| `WaitProcess` → `MultiTasks` | `ChangeDescription(Func<IEnumerable<StateProcess>,string>)` | Dynamic aggregate description not available |
| `WaitProcess` → `MultiTasks` | `IntervalUpdate(int)` | UI update interval not configurable |
| `WaitTimer` → `Time` | `ShowElapsedTime(int, bool)` | Elapsed display managed automatically |
| `ProgressBar` | `IntervalUpdate(int)` | UI update interval not configurable |
| `FileSelect` → `File` | `HideZeroEntries(bool)` | Empty folders are always shown |
| `FileSelect` → `File` | `HideFilesBySize(long, long)` | Size filter not available (use `PredicateSelected` on `MultiFile`) |
| `FileSelect` / `FileMultiSelect` | `EnabledSearchFilter(FilterMode)` | Built-in search filter not available (use `SearchPattern`) |
| `FileMultiSelect` → `MultiFile` | `PredicateDisabled(Func<ItemFile,bool>)` | Per-item disable predicate not available |
| `NodeTree*` | `DisableRecursiveCount(bool)` | Recursive child-count toggle not available |
| `NodeTree*` | `HideCount(bool)` / `HideCountSelected(bool)` | Node-count display toggles not available |
| `NodeTree*` | `PredicateDisabled(Func<T,bool>)` | Per-node disable predicate not available |
| `Table` / `MultiTable` | `SeparatorRows(bool)` | Row separators not available |
| `ChartBar` | `MaxWidth(byte)` | Width is now automatic |

---

## 🔴 v5.x controls with no v6.x equivalent

| Control v5.x | Description | Status |
|---|---|---|
| `RemoteSelect<T1,T2>()` | Select with remote/paged loading | ❌ Removed — no replacement |
| `RemoteMultiSelect<T1,T2>()` | MultiSelect with remote/paged loading | ❌ Removed — no replacement |
| `NodeTreeRemoteSelect<T1,T2>()` | Tree with remote node loading | ❌ Removed — no replacement |
| `NodeTreeRemoteMultiSelect<T1,T2>()` | MultiTree with remote node loading | ❌ Removed — no replacement |

> ⚠️ Applications that relied on remote/paged loading must load the data themselves and feed the `Select`, `MultiSelect`, `Tree` or `MultiTree` controls.

---

## 🟢 What's new in v6.x

### New controls / helpers

| Item | Description |
|---|---|
| `History(string filename)` | **Direct** management of persisted history — read, write and remove entries without needing an input control |

> `Time()` is not a *new* control — it is the renamed `WaitTimer()` with a reworked API.

### New methods on existing controls

| Control | New v6.x methods |
|---|---|
| `KeyPress` | `ShowMessage(Func<ConsoleKeyInfo,string>)` · `ShowMessageAsync(...)` |
| `Select<T>` | `DefaultMatchBy` · `ViewOnly` · `UseDefaultHistory` · `ChangeDescriptionAsync` · `InteractionAsync` · `TextSelectorAsync` · `ExtraInfoAsync` · `PredicateSelectedAsync` (x2) |
| `MultiSelect<T>` | `DefaultMatchBy` · `ViewOnly` · `UseDefaultHistory` · `TextSelectorAsync` · `ExtraInfoAsync` · `ChangeDescriptionAsync` · `InteractionAsync` · `PredicateSelectedAsync` (x2) |
| `Input` | `SuggestionHandler(..., bool autocomplete)` · `SuggestionHandlerAsync(..., bool autocomplete)` · `MinimumSuggestionLength(byte)` · `PredicateSelectedAsync` (x2) · `ChangeDescriptionAsync` |
| `Table` / `MultiTable` | `DefaultMatchBy` · `ViewOnly` · `HorizontalScroll` · `ChangeDescriptionAsync` · `TextSelectorAsync` · `InteractionAsync` · `PredicateSelectedAsync` (x2) · (`MultiTable`) `EnabledHistory` · `UseDefaultHistory` |
| `Tree` / `MultiTree` | `ViewOnly` · `Filter` · `SelectLeafOnly`/`CheckLeafOnly` · `ShowFullPath` · `CascadeCheck` (MultiTree) · `ChangeDescriptionAsync` · `ExtraInfoAsync` · `InteractionAsync` · `PredicateSelectedAsync` (x2) |
| `MultiTasks` | `AddTaskAsync` · `Interaction<T>` · `StopOnError(bool)` · `Mode(MultiTasksMode)` |
| `Task` | `ChangeDescription` · `ChangeDescriptionAsync` · `Context(IDictionary)` · `Culture(CultureInfo)` · multiple `Action`/`ActionAsync` overloads |
| `Time` | `Duration(TimeSpan/int)` · `Format(string)` · `Culture(CultureInfo)` · `ChangeDescription` · `ChangeDescriptionAsync` · `DisplayMode(TimeDisplayMode)` |
| `File` / `MultiFile` | `SelectFilesOnly(bool)` · `ShowFullPath(bool)` · (`MultiFile`) `CascadeCheck` · `RecursiveMarkWithCtrlSpace` · `PredicateSelectedAsync` (x2) |
| `Switch` | `ChangeDescriptionAsync` · `OffValue(EmojiName, string)` · `OnValue(EmojiName, string)` |
| `ProgressBar` | `ChangeDescriptionAsync` · `UpdateHandlerAsync` |
| `ChartBar` | `PredicateSelectedAsync` (x2) · `ChangeDescriptionAsync` |
| `Calendar` | `PredicateSelectedAsync` (x2) · `ChangeDescriptionAsync` · `InteractionAsync` · `AddNotes` |
| MaskEdit family | `PredicateSelectedAsync` (x2) on every type |

> The MaskEdit factory methods (`MaskInteger`, `MaskLong`, `MaskDecimal`, `MaskDecimalCurrency`, `MaskDouble`, `MaskDoubleCurrency`, `MaskTime`, `MaskTimeOnly`, `MaskDate`, `MaskDateTime`, `MaskDateOnly`, `MaskEdit`) already existed in v5.x — they are **not** new. See [MaskEdit](migration/controls/maskedit.md).
