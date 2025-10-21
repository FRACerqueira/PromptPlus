# <img align="left" width="100" height="100" src="../icon.png">PromptPlus What's new
[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

## V.5.0.0

**The version 5 has been completely redesigned** and optimized for better stability, consistency, and performance. 
All controls and behaviors have been revisited and improved to ensure sustainable evolution. 
Due to the significant modifications, version 5 introduced **significant changes and is incompatible with versions 4.x**, although the concepts and components are very similar, requiring a small learning curve and minor methodological adjustments.

- Support for .Net10, .Net9 and .Net8
- The tooltip mechanism shows all keys and hotkeys for each control by switching the view ('F1').
- All interative controls start at : **PromptPlus.Controls**.\<name of control\>.
- All no interative controls start at : **PromptPlus.Widgets**.\<name of Widget\>.
- All commands for console start at : **PromptPlus.Console**.\<command\>.
- All general config start at : **PromptPlus.Config**.\<config\>.

### Table of Contents

- [General changes](#general-changes)
- [PromptPlus Config](#config)
- [Console Commands](#console-commands)
- [Options(all Controls)](#options)
- **AddtoList Control Discontinued!**
- [AutoComplete Control](#autocomplete-control)
- [Banner Widget](#banner-widget)
- [Calendar Control](#calendar-control)
- [Calendar Widget](#calendar-widget-new) **NEW!** 
- [Chart Bar Control](#chart-bar-control)
- [Chart Bar Widget](#chart-bar-widget-new) **NEW!** 
- [File MultiSelect Control](#file-multiselect-control)
- [File Select Control](#file-select-control)
- [Input Control](#input-control)
- [Keypress Control](#keypress-control)
- **MaskEdit (Old Version) Control Discontinued!**
- [MaskEdit Control](#maskedit-control) **NEW!**  
- [MaskDateTime Control]() **NEW!** 
- [MaskDate Control]() **NEW!** 
- [MaskDateOnly Control]() **NEW!** 
- [MaskTime Control]() **NEW!** 
- [MaskTimeOnly Control]() **NEW!** 
- [MaskDecimalCurrency  Control]() **NEW!** 
- [MaskDoubleCurrency  Control]() **NEW!** 
- [MaskDecimal Control]() **NEW!** 
- [MaskDouble Control]() **NEW!** 
- [MaskInteger Control]() **NEW!** 
- [MaskLong Control]() **NEW!** 
- [MultiSelect Control]()
- [NodeTree MultiSelect Control]()
- [NodeTree Select Control]()
- [Progress Bar Control]()
- [ReadLine Emacs Control]() **NEW!** 
- [Select Control]()
- [Slider Control]()
- [Slider Widget]()  **NEW!** 
- [Switch Control]()
- [Switch Widget]()  **NEW!** 
- [Table MultiSelect Control]()
- [Table Select Control]()
- [Table Widget]() **NEW!** 
- [Wait Process Control]()
- [Wait Timer Control]()

### General changes
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Support for .Net10, .Net9 and .Net8
- External references were reviewed and only those necessary for size treatment for East Asian characters were used.
- New control rendering engine adjusts more fluidly to the screen size and avoids flickering by redrawing only the changed lines.
- Revised control of hotkeys and special characters ensuring consistency according to the console's capabilities.
- Created the separation of interactive controls and added several non-interactive controls (widgets).
- Introduced **NEW multi-threaded operation** for controls and commands. Now each command and control block the main thread during printing/interaction execution.
- Renaming several control methods for better clarity and reduced scope, aiming at the unique responsibility that each component intends to perform, allowing for sustainable evolution.
- Created the concept of an editing window for controls that require a significant input/response size, ensuring visual consistency and adequate navigability.
- A slice architecture was adopted for each component, allowing individual evolution of each one with low interference to the others.
- The **NEW tooltip mechanism** now shows all keys and hotkeys for each control by switching the view ('F1').
- All interative controls start at : **PromptPlus.Controls**.\<name of control\>.
    - All initialization contracts have been standardized: PromptPlus.Controls.\<name of control\>(string prompt = "", string? description = null).
- All no interative controls start at : **PromptPlus.Widgets**.\<name of Widget\>.
    - For each non-interactive control the initialization contract was customized.
- All commands for console start at : **PromptPlus.Console**.\<command\>.
- All general config start at : **PromptPlus.Config**.\<config\>.

### Config
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

### Console Commands
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

### Options
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)


### AutoComplete Control

[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlAutoComplete -> IAutoCompleteControl.
- Command initialization: PromptPlus.AutoComplete -> PromptPlus.Controls.AutoComplete.
    - Fixed Standard: AutoComplete(string prompt = "", string? description = null). 
    - Removed : AutoComplete(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : AutoComplete(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - MaxWidth(byte maxWidth).
    - PredicateSelected(Func<string, (bool,string?)> validselect).
    - PredicateSelected(Func<string, bool)> validselect).
    - EnabledHistory(string filename, Action\<IHistoryOptions\>? options = null).
    - Options(Action\<IControlOptions\> options).
    - TextSelector(Func<string, string> value).
- Changed: 
    - PageSize(int value) -> PageSize(byte value).
    - MinimumPrefixLength(int value) -> MinimumPrefixLength(byte value).
    - CompletionAsyncService(Func<string, int, CancellationToken, Task<string[]>> value) -> CompletionAsyncService(Func<string, CancellationToken, Task<string[]>> value).
    - Default(string value) -> Default(string value, bool usedefaultHistory = true).
    - Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable\<string\>? customspinner = null) -> Spinner(SpinnersType spinnersType).
    - MaxLength(ushort value) -> MaxLength(int maxLength, byte? maxWidth = null).
- Removed: 
    - OverwriteDefaultFrom(string value, TimeSpan? timeout = null).
    - AddValidators(params Func<object, ValidationResult>[] validators).
    - Config(Action\<IPromptConfig\> context).
    - ValidateOnDemand(bool value = true).

###  Banner Widget
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IBannerControl -> IBanner.
  - Command initialization: PromptPlus.Banner -> PromptPlus.Widgets.Banner.
    - Standard : Banner(string value, Style? style = null). 
    - Removed : Banner(string value).
- Added:
    - Border(BannerDashOptions dashOptions).
    - Show()
- Changed: 
    - LoadFont(string value) -> FromFont(string filepathFont).
    - LoadFont(Stream value) -> FromFont(Stream streamFont).
- Removed: 
    - Run(Color? color = null,BannerDashOptions bannerDash = BannerDashOptions.None).

### Calendar Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlCalendar -> ICalendarControl.
- Command initialization: PromptPlus.Calendar -> PromptPlus.Controls.Calendar.
    - Fixed Standard: Calendar(string prompt = "", string? description = null).
    - Removed : Calendar(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : Calendar(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - Default(DateTime value).
    - DisableDates(params DateTime[] dates).
    - AddNote(DateTime value, string? note = null).
    - Highlights(params DateTime[] dates)
    - PredicateSelected(Func<ChartItem, (bool,string?)> validselect).
    - PredicateSelected(Func<ChartItem, bool)> validselect).
    - Options(Action\<IControlOptions\> options).
- Changed: 
    - Layout(CalendarLayout value) -> Layout(CalendarLayout layout = CalendarLayout.SingleGrid)
    - Interaction\<T1\>(IEnumerable\<T1\> values, Action\<IControlCalendar, T1\> action) -> Interaction\<T\>(IEnumerable\<T\> items, Action\<T, ICalendarControl\> interactionaction)
    - PageSize(int value) -> PageSize(byte value).
- Removed: 
    - Default(DateTime value, PolicyInvalidDate policy = PolicyInvalidDate.NextDate)
    - AddItems(CalendarScope scope, params ItemCalendar[] values)
    - HotKeySwitchNotes(HotKey value)
    - OverwriteDefaultFrom(string value, TimeSpan? timeout = null)
    - AddValidators(params Func<object, ValidationResult>[] validators)
    - Config(Action\<IPromptConfig\> context).

### Calendar Widget (NEW)
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Command initialization: PromptPlus.Widgets.Calendar.
    - Standard : Calendar(DateTime dateref). 
- Commands: 
    - Layout(CalendarLayout layout = CalendarLayout.SingleGrid).
    - Culture(CultureInfo culture).
    - Culture(string cultureName).
    - FirstDayOfWeek(DayOfWeek firstDayOfWeek).
    - Styles(CalendarStyles styleType, Style style).
    - Show().

### Chart Bar Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlChartBar -> IChartBarControl.
- Command initialization: PromptPlus.ChartBar -> PromptPlus.Controls.ChartBar.
    - Fixed Standard: ChartBar(string prompt = "", string? description = null).
    - Removed : ChartBar(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : ChartBar(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - Title(string title, TextAlignment alignment = TextAlignment.Center).
    - ChangeDescription(Func<ChartItem, string> value).
    - HideElements(HideChart value).
    - Options(Action\<IControlOptions\> options).
- Changed: 
    - Layout(LayoutChart value) ->  Layout(ChartBarLayout layout = ChartBarLayout.Standard).
    - BarType(ChartBarType value) -> BarType(ChartBarType type = ChartBarType.Fill).
    - Width(int value) -> Width(byte value).
    - Interaction\<T1\>(IEnumerable\<T1\> values, Action\<IControlChartBar, T1\> action) -> Interaction\<T\>(IEnumerable\<T\> items, Action\<T, IChartBarControl\> interactionaction).
    - AddItem(string label, double value, Color? colorbar = null) -> AddItem(string label, double value, Color? colorBar = null, string? id = null).
    - ShowLegends(bool withvalue = true, bool withPercent = true) -> ShowLegends(bool value = true).
    - PageSize(int value) -> PageSize(byte value).
- Removed: 
    - TitleAlignment(Alignment value = Alignment.Left).
    - HidePercent(bool value = true).
    - HideValue(bool value = true).
    - HideOrdination(bool value = true).
    - EnabledInteractionUser(bool switchType = true, bool switchLegend = true, bool switchorder = true).
    - Config(Action\<IPromptConfig\> context).
    - HotKeySwitchType(HotKey value).
    - HotKeySwitchLegend(HotKey value).
    - HotKeySwitchOrder(HotKey value).

### Chart Bar Widget (NEW)
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Command initialization: PromptPlus.Widgets.ChartBar.
    - Standard : ChartBar(string title, TextAlignment titleAlignment = TextAlignment.Center, bool showlegends = false)
- Commands: 
    - Layout(ChartBarLayout layout = ChartBarLayout.Standard).
    - Culture(CultureInfo culture).
    - Culture(string cultureName).
    - BarType(ChartBarType type = ChartBarType.Fill).
    - Width(byte value).
    - Styles(ChartBarStyles styleType, Style style).
    - AddItem(string label, double value, Color? colorBar = null, string? id = null).
    - Interaction\<T\>(IEnumerable\<T\> items, Action\<T, IChartBarWidget\> interactionaction).
    - FractionalDigits(byte value).
    - OrderBy(ChartBarOrder order).
    - HideElements(HideChart value).
    - Show().

### File MultiSelect Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlBrowserMultiSelect -> IFileMultiSelectControl.
    - Changed : ResultPrompt\<ItemBrowser[]\> Run(CancellationToken? token = null)  -> ResultPrompt\<ItemFile[]\> Run(CancellationToken token = default).
- Command initialization: PromptPlus.BrowserMultiSelect -> PromptPlus.Controls.FileMultiSelect.
    - Fixed Standard: FileMultiSelect(string prompt = "", string? description = null). 
    - Removed : BrowserMultiSelect(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : BrowserMultiSelect(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - HideZeroEntries(bool value = true).
    - HideFilesBySize(long minvalue, long maxvalue = long.MaxValue).
    - MaxWidth(byte maxWidth).
    - EnabledSearchFilter(FilterMode filter = FilterMode.Contains)
    - SearchPattern(string value)
    - PredicateSelected(Func<string, (bool,string?)> validselect).
    - PredicateSelected(Func<string, bool)> validselect).
    - PredicateDisabled(Func<ItemFile, bool> validdisabled)
    - Options(Action\<IControlOptions\> options).
    - HideCountSelected(bool value = true)
- Changed: 
    - PageSize(int value) -> PageSize(byte value).
    - ShowSize(bool value = true) -> HideSizeInfo(bool value = true).
    - Root(string value,bool expandall,  Func\<ItemBrowser, bool\>? validselect = null, Func\<ItemBrowser, bool\>? setdisabled = null) -> Root(string value).
- Removed: 
    - OverwriteDefaultFrom(string value, TimeSpan? timeout = null).
    - Default(string value).
    - NoSpinner(bool value = true).
    - DisabledRecursiveExpand(bool value = true).
    - Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable\<string\>? customspinner = null).
    - ShowLines(bool value = true).
    - ShowExpand(bool value = true).
    - ShowCurrentFolder(bool value = true).
    - SearchFolderPattern(string value).
    - SearchFilePattern(string value).
    - FilterType(FilterMode value).
    - SelectAll(Func<ItemBrowser, bool>? validselect = null).
    - AddFixedSelect(params string[] values).
    - HotKeyFullPath(HotKey value).
    - HotKeyToggleExpand(HotKey value).
    - HotKeyToggleExpandAll(HotKey value).
    - AfterExpanded(Action\<ItemBrowser\> value).
    - AfterCollapsed(Action\<ItemBrowser\> value).
    - BeforeExpanded(Action\<ItemBrowser\> value).
    - BeforeCollapsed(Action\<ItemBrowser\> value).
    - Config(Action\<IPromptConfig\> context).

### File Select Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlBrowserSelect -> IFileSelectControl.
    - Changed : ResultPrompt\<ItemBrowser\> Run(CancellationToken? token = null)  -> ResultPrompt\<ItemFile\> Run(CancellationToken token = default).
- Command initialization: PromptPlus.Browser -> PromptPlus.Controls.FileSelect.
    - Fixed Standard: FileSelect(string prompt = "", string? description = null). 
    - Removed : Browser(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : Browser(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - HideZeroEntries(bool value = true).
    - HideFilesBySize(long minvalue, long maxvalue = long.MaxValue).
    - EnabledSearchFilter(FilterMode filter = FilterMode.Contains)
    - SearchPattern(string value)
    - PredicateSelected(Func<string, (bool,string?)> validselect).
    - PredicateSelected(Func<string, bool)> validselect).
    - PredicateDisabled(Func<ItemFile, bool> validdisabled)
    - Options(Action\<IControlOptions\> options).
- Changed: 
    - PageSize(int value) -> PageSize(byte value).
    - ShowSize(bool value = true) -> HideSizeInfo(bool value = true).
    - Root(string value,bool expandall,  Func\<ItemBrowser, bool\>? validselect = null, Func\<ItemBrowser, bool\>? setdisabled = null) -> Root(string value).
- Removed: 
    - OverwriteDefaultFrom(string value, TimeSpan? timeout = null).
    - Default(string value).
    - NoSpinner(bool value = true).
    - DisabledRecursiveExpand(bool value = true).
    - Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable\<string\>? customspinner = null).
    - ShowLines(bool value = true).
    - ShowExpand(bool value = true).
    - ShowCurrentFolder(bool value = true).
    - SearchFolderPattern(string value).
    - SearchFilePattern(string value).
    - FilterType(FilterMode value).
    - SelectAll(Func<ItemBrowser, bool>? validselect = null).
    - AddFixedSelect(params string[] values).
    - HotKeyFullPath(HotKey value).
    - HotKeyToggleExpand(HotKey value).
    - HotKeyToggleExpandAll(HotKey value).
    - AfterExpanded(Action\<ItemBrowser\> value).
    - AfterCollapsed(Action\<ItemBrowser\> value).
    - BeforeExpanded(Action\<ItemBrowser\> value).
    - BeforeCollapsed(Action\<ItemBrowser\> value).
    - Config(Action\<IPromptConfig\> context).

### Input Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlInput -> IInputControl.
- Command initialization: PromptPlus.Input -> PromptPlus.Controls.Input.
    - Fixed Standard: Input(string prompt = "", string? description = null). 
    - Removed : Input(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : Input(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - MaxWidth(byte maxWidth).
    - PageSize(byte value).
    - EnabledHistory(string filename, Action\<IHistoryOptions\>? options = null).
    - PredicateSelected(Func<string, (bool,string?)> validselect).
    - PredicateSelected(Func<string, bool)> validselect).
    - Options(Action\<IControlOptions\> options).
- Changed: 
    - Default(string value) -> Default(string value, bool usedefaultHistory = true).
    - MaxLength(ushort value) -> MaxLength(int maxLength, byte? maxWidth = null).
    - IsSecret(char? value = '#') -> IsSecret(char? value = null, bool enabledView = true).
    - SuggestionHandler(Func\<SuggestionInput, SuggestionOutput\> value) -> SuggestionHandler(Func\<string, string[]\> value).
- Removed: 
    - OverwriteDefaultFrom(string value, TimeSpan? timeout = null).
    - EnabledViewSecret(HotKey? hotkeypress = null).
    - AddValidators(params Func<object, ValidationResult>[] validators).
    - Config(Action\<IPromptConfig\> context).
    - ValidateOnDemand(bool value = true).
    - HistoryMinimumPrefixLength(int value).
    - HistoryEnabled(string value).
    - HistoryTimeout(TimeSpan value).
    - HistoryMaxItems(byte value).
    - HistoryPageSize(int value).

### Keypress Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlKeyPress -> IKeyPressControl.
- Command initialization: PromptPlus.KeyPress -> PromptPlus.Controls.KeyPress.
    - Fixed Standard: KeyPress(string prompt = "", string? description = null). 
    - Removed : KeyPress(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : KeyPress(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - Options(Action\<IControlOptions\> options).
    - ShowInvalidKey(bool value = true).
- Changed: 
    - Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable\<string\>? customspinner = null) -> Spinner(SpinnersType spinnersType).
    - AddKeyValid(ConsoleKey key,ConsoleModifiers? modifiers = null) -> AddKeyValid(ConsoleKey key, ConsoleModifiers? modifiers = null, string? showtext = null).
- Removed: 
    - Config(Action\<IPromptConfig\> context).
    - TextKeyValid(Func<ConsoleKeyInfo, string?> value).
 