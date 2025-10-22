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
- [MaskEdit Control](#maskedit-control-new) **NEW!**  
- [MaskDateTime Control](#maskdatetime-control-new) **NEW!** 
- [MaskCurrency Control](#maskcurrency-control-new) **NEW!** 
- [MaskNumber Control](#masknumber-control-new) **NEW!** 
- [MultiSelect Control](#multiselect-control)
- [NodeTree MultiSelect Control](#nodetree-multiselect-control)
- [NodeTree Select Control](#nodetree-select-control)
- [Progress Bar Control](#progress-bar-control)
- [ReadLine Emacs Control](#readline-emacs-control-new) **NEW!** 
- [Select Control](#select-control)
- [Slider Control](#slider-control)
- [Slider Widget](#slider-widget-new)  **NEW!** 
- [Switch Control](#switch-control)
- [Switch Widget](#switch-widget-new)  **NEW!** 
- [Table MultiSelect Control](#table-multiselect-control)
- [Table Select Control](#table-select-control)
- [Table Widget](#table-widget-new)  **NEW!** 
- [Wait Process Control](#wait-process-control)
- [Wait Timer Control](#Wait-timer-control)

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
    - PredicateSelected(Func\<string, (bool,string?)\> validselect).
    - PredicateSelected(Func\<string, bool)\> validselect).
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
    - PredicateSelected(Func\<DateTime?, (bool,string?)\> validselect).
    - PredicateSelected(Func\<DateTime?, bool)\> validselect).
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
    - PredicateSelected(Func\<ChartItem, (bool,string?)\> validselect).
    - PredicateSelected(Func\<ChartItem, bool)\> validselect).
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
    - Changed : ResultPrompt\<ItemBrowser\[ \]\> Run(CancellationToken? token = null)  -> ResultPrompt\<ItemFile\[ \]\> Run(CancellationToken token = default).
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
    - PredicateSelected(Func<ItemFile, (bool,string?)\> validselect).
    - PredicateSelected(Func\<ItemFile, bool)\> validselect).
    - PredicateDisabled(Func\<ItemFile, bool\> validdisabled)
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
    - PredicateSelected(Func\<ItemFile, (bool,string?)\> validselect).
    - PredicateSelected(Func\<ItemFile, bool)\> validselect).
    - PredicateDisabled(Func\<ItemFile, bool\> validdisabled)
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
    - PredicateSelected(Func\<string, (bool,string?)\> validselect).
    - PredicateSelected(Func\<string, bool)\> validselect).
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
 
### MaskEdit Control (NEW)
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

The MaskEdit control has been completely redesigned and now returns and handles the type passed as an argument with special attention. 
Navigation and comands has been optimized for each supported type: string, date/time, numeric, and currency (string, DateTime, DateOnly, TimeOnly, integer, long, double, and decimal).

- Command initialization:
    - Interface : IMaskEditStringControl\<T\>. 'T' It will always be of type string
    - Fixed Standard: MaskEdit(string prompt = "", string? description = null). 
    - Return type : string.
    - Mask Pattern:
        - 9 - Numeric character accepts delimiters for constant or custom.
        - L - Lower Letter character accepts delimiters for constant or custom.
        - U - Upper Letter character accepts delimiters for constant or custom.
        - A - Lower and Upper Letter character accepts delimiters for constant or custom.
        - X - Numeric, Lower and Upper Letter character accepts delimiters for constant or custom.
        - C - Custom character accepts only delimiters for custom.
        - \ - Escape character to use the next char as constant.
        - { } - Delimiters group to apply custom list or constant value valid only a single mask type insede the group.
        - \[ \] - Delimiters for custom value.
        - ( ) - Delimiters for constant value inside the group.
- Commands: 
    - Mask(string mask, bool returnWithMask = false). **Required!**
    - PromptMask(char value = '_').
    - InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput).
    - HideTipInputType(bool value = true).
    - Default(T value).
    - DefaultIfEmpty(T value).
    - PredicateSelected(Func\<T, bool\> validselect).
    - PredicateSelected(Func\<T, (bool, string?)\> validselect).
    - Styles(MaskEditStyles styleType, Style style).
    - Options(Action\<IControlOptions\> options).
    - ResultPrompt\<T\> Run(CancellationToken token = default).

### MaskDateTime Control (NEW)
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

The MaskEdit control has been completely redesigned and now returns and handles the type passed as an argument with special attention. 
Navigation and comands has been optimized for each supported type: string, date/time, numeric, and currency (string, DateTime, DateOnly, TimeOnly, integer, long, double, and decimal).

- Command initialization: 
    - Interface : IMaskEditDateTimeControl\<T\>. 'T' It will always be of the types: DateTime, DateOnly or TimeOnly.
    - Fixed Standard: 
        - MaskDateTime(string prompt = "", string? description = null). 
            - Return type : DateTime.
        - MaskDate(string prompt = "", string? description = null). 
            - Return type : DateTime.
        - MaskDateOnly(string prompt = "", string? description = null). 
            - Return type : DateOnly.
        - MaskTime(string prompt = "", string? description = null). 
            - Return type : DateTime.
        - MaskTimeOnly(string prompt = "", string? description = null). 
            - Return type : TimeOnly.
- Commands: 
    - PromptMask(char value = '_').
    - InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput).
    - FixedValues(DateTimePart partdetetime, int value).
    - HideTipInputType(bool value = true).
    - WeekTypeMode(WeekType value = WeekType.WeekShort).
    - Default(T value).
    - DefaultIfEmpty(T value).
    - Culture(CultureInfo culture).
    - Culture(string cultureName).
    - PredicateSelected(Func\<T, bool\> validselect).
    - PredicateSelected(Func\<T, (bool, string?)\> validselect).
    - Styles(MaskEditStyles styleType, Style style).
    - Options(Action\<IControlOptions\> options).
    - ResultPrompt\<T\> Run(CancellationToken token = default).

### MaskCurrency Control (NEW)
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

The MaskEdit control has been completely redesigned and now returns and handles the type passed as an argument with special attention. 
Navigation and comands has been optimized for each supported type: string, date/time, numeric, and currency (string, DateTime, DateOnly, TimeOnly, integer, long, double, and decimal).

- Command initialization: PromptPlus.Controls.MaskEdit.
    - Interface : IMaskEditCurrencyControl\<T\>. 'T' It will always be of the types: double or decimal.
    - Fixed Standard: 
        - MaskDecimalCurrency(string prompt = "", string? description = null). 
            - Return type : decimal.
        - MaskDoubleCurrency(string prompt = "", string? description = null). 
            - Return type : double.
- Commands: 
    - PromptMask(char value = '_').
    - NumberFormat(byte integerpart, byte decimalpart = 2, bool withsignal = false, bool withseparatorgroup = true).
    - HideTipInputType(bool value = true).
    - Default(T value).
    - DefaultIfEmpty(T value).
    - Culture(CultureInfo culture).
    - Culture(string cultureName).
    - PredicateSelected(Func\<T, bool\> validselect).
    - PredicateSelected(Func\<T, (bool, string?)\> validselect).
    - Styles(MaskEditStyles styleType, Style style).
    - Options(Action\<IControlOptions\> options).
    - ResultPrompt\<T\> Run(CancellationToken token = default).

### MaskNumber Control (NEW)
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

The MaskEdit control has been completely redesigned and now returns and handles the type passed as an argument with special attention. 
Navigation and comands has been optimized for each supported type: string, date/time, numeric, and currency (string, DateTime, DateOnly, TimeOnly, integer, long, double, and decimal).

- Command initialization: PromptPlus.Controls.MaskEdit.
    - Interface : IMaskEditNumberControl\<T\>. 'T' It will always be of the types: integer, long, double or decimal.
    - Fixed Standard: 
        - MaskDecimal(string prompt = "", string? description = null). 
            - Return type : decimal.
        - MaskDouble(string prompt = "", string? description = null). 
            - Return type : double.
        - MaskDouble(string prompt = "", string? description = null). 
            - Return type : double.
        - MaskInteger(string prompt = "", string? description = null). 
            - Return type : integer.
        - MaskLong(string prompt = "", string? description = null). 
            - Return type : long.
- Commands: 
    - PromptMask(char value = '_').
    - NumberFormat(byte integerpart, bool withsignal = false, bool withseparatorgroup = true).
    - HideTipInputType(bool value = true).
    - Default(T value).
    - DefaultIfEmpty(T value).
    - Culture(CultureInfo culture).
    - Culture(string cultureName).
    - PredicateSelected(Func\<T, bool\> validselect).
    - PredicateSelected(Func\<T, (bool, string?)\> validselect).
    - Styles(MaskEditStyles styleType, Style style).
    - Options(Action\<IControlOptions\> options).
    - ResultPrompt\<T\> Run(CancellationToken token = default).

### MultiSelect Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlMultiSelect\<T\[ \]\> -> IMultiSelectControl\<T \[ \]\>.
- Command initialization: PromptPlus.MultiSelect -> PromptPlus.Controls.MultiSelect.
    - Fixed Standard: MultiSelect\<T\>(string prompt = "", string? description = null). 
    - Removed : MultiSelect\<T\>(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : MultiSelect\<T\>(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - MaxWidth(byte maxWidth).
    - PredicateSelected(Func\<T, (bool,string?)\> validselect).
    - PredicateSelected(Func\<T, bool)\> validselect).
    - EnabledHistory(string filename, Action\<IHistoryOptions\>? options = null).
    - Options(Action\<IControlOptions\> options).
    - AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null).
    - HideCountSelected(bool value = true).
    - ShowAllSelected(bool value).
    - Default(IEnumerable\<T\> values, bool usedefaultHistory = true).
- Changed: 
    - PageSize(int value) -> PageSize(byte value).
    - Interaction\<T1\>(IEnumerable\<T1\> values, Action\<IControlMultiSelect\<T\>, T1\> action) -> Interaction(IEnumerable\<T\> items, Action\<T, IMultiSelectControl\<T\>\> interactionAction)
    - ShowTipGroup(bool value = true) -> HideTipGroup(bool value = true).
    - FilterType(FilterMode value) -> Filter(FilterMode value, bool caseinsensitive = true).
    - AddItem(T value, bool disable = false, bool selected = false) -> AddItem(T value, bool valuechecked = false, bool disable = false).
    - AddItems(IEnumerable\<T\> values, bool disable = false, bool selected = false) -> AddItems(IEnumerable\<T\> values, bool valuechecked = false, bool disable = false).
    - AddItemGrouped(string group, T value, bool disable = false, bool selected = false) -> AddGroupedItem(string group, T value, bool valuechecked = false, bool disable = false).
    - AddItemsGrouped(string group, IEnumerable\<T\> value, bool disable = false, bool selected = false) -> AddGroupedItems(string group, IEnumerable\<T\> values, bool valuechecked = false, bool disable = false).
- Removed: 
    - OverflowAnswer(Overflow value).
    - AddDefault(params T[] values).
    - AddDefault(IEnumerable\<T\> values).
    - OrderBy(Expression\<Func\<T, object\>\> value).
    - OrderByDescending(Expression\<Func\<T, object\>\> value).
    - OverwriteDefaultFrom(string value, TimeSpan? timeout = null).
    - HotKeySelectAll(HotKey value).
    - HotKeyInvertSelected(HotKey value).
    - AddItemsTo(AdderScope scope, params T[] values).
    - AddItemsTo(AdderScope scope, IEnumerable\<T\> values).
    - Config(Action\<IPromptConfig\> context).

### NodeTree MultiSelect Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlTreeViewMultiSelect\<T\[ \]\> -> INodeTreeMultiSelectControl\<T\[ \]\>.
- Command initialization: PromptPlus.TreeViewMultiSelect -> PromptPlus.Controls.NodeTreeMultiSelect.
    - Fixed Standard: NodeTreeMultiSelect\<T\>(string prompt = "", string? description = null). 
    - Removed : TreeViewMultiSelect\<T\>(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : TreeViewMultiSelect\<T\>(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - MaxWidth(byte maxWidth).
    - Options(Action\<IControlOptions\> options).
    - TextSelector(Func<T, string> value).
    - PredicateSelected(Func\<T, (bool,string?)\> validselect).
    - PredicateSelected(Func\<T, bool)\> validselect).
    - PredicateDisabled(Func<T, bool> validdisabled).
    - AddRootNode(T value, bool valuechecked = false, string nodeseparator = "|").
    - AddChildNode(T parent, T value, bool valuechecked = false).
    - HideCountSelected(bool value = true).
    - HideSize(bool value = true).
- Changed: 
    - PageSize(int value) -> PageSize(byte value).
    - Interaction\<T1\>(IEnumerable\<T1\> values, Action\<IControlTreeViewMultiSelect\<T\>, T1\> action) -> Interaction(IEnumerable\<T\> items, Action\<T, INodeTreeMultiSelectControl\<T\>\> interactionAction)
    - Styles(TreeViewStyles content, Style value) -> Styles(NodeTreeStyles styleType, Style style).
    - FilterType(FilterMode value) -> Filter(FilterMode value, bool caseinsensitive = true).
- Removed: 
    - Config(Action\<IPromptConfig\> context).
    - ShowLines(bool value = true).
    - ShowExpand(bool value = true).
    - ExpandAll(bool value = true).
    - AddFixedSelect(params T[] values).
    - SelectAll(Func\<T, bool\>? validselect = null).
    - Default(T value).
    - ShowCurrentNode(bool value = true).
    - HotKeyFullPath(HotKey value).
    - HotKeyToggleExpand(HotKey value).
    - HotKeyToggleExpandAll(HotKey value).
    - AfterExpanded(Action\<T\> value).
    - AfterCollapsed(Action\<T\> value).
    - BeforeExpanded(Action\<T\> value).
    - BeforeCollapsed(Action\<T\> value).
    - RootNode(T value, Func\<T, string\> textnode, Func\<T, bool\>? validselect = null, Func\<T, bool\>? setdisabled = null, char? separatePath = null, Func\<T, string\> uniquenode = null).
    - AddNode(T value).
    - AddNode(T Parent, T value).

### NodeTree Select Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlTreeViewSelect\<T\> -> INodeTreeSelectControl\<T\>.
- Command initialization: PromptPlus.TreeView -> PromptPlus.Controls.NodeTreeSelect.
    - Fixed Standard: NodeTreeSelect\<T\>(string prompt = "", string? description = null). 
    - Removed : TreeView\<T\>(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : TreeView\<T\>(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - Options(Action\<IControlOptions\> options).
    - TextSelector(Func<T, string> value).
    - PredicateSelected(Func\<T, (bool,string?)\> validselect).
    - PredicateSelected(Func\<T, bool)\> validselect).
    - PredicateDisabled(Func<T, bool> validdisabled).
    - AddRootNode(T value, bool valuechecked = false, string nodeseparator = "|").
    - AddChildNode(T parent, T value, bool valuechecked = false).
    - HideSize(bool value = true).
- Changed: 
    - PageSize(int value) -> PageSize(byte value).
    - Interaction\<T1\>(IEnumerable\<T1\> values, Action\<IControlTreeViewSelect\<T\>, T1\> action) -> Interaction(IEnumerable\<T\> items, Action\<T, INodeTreeSelectControl\<T\>\> interactionAction)
    - Styles(TreeViewStyles content, Style value) -> Styles(NodeTreeStyles styleType, Style style).
    - FilterType(FilterMode value) -> Filter(FilterMode value, bool caseinsensitive = true).
- Removed: 
    - Config(Action\<IPromptConfig\> context).
    - ShowLines(bool value = true).
    - ShowExpand(bool value = true).
    - ExpandAll(bool value = true).
    - Default(T value).
    - ShowCurrentNode(bool value = true).
    - HotKeyFullPath(HotKey value).
    - HotKeyToggleExpand(HotKey value).
    - HotKeyToggleExpandAll(HotKey value).
    - AfterExpanded(Action\<T\> value).
    - AfterCollapsed(Action\<T\> value).
    - BeforeExpanded(Action\<T\> value).
    - BeforeCollapsed(Action\<T\> value).
    - RootNode(T value, Func\<T, string\> textnode, Func\<T, bool\>? validselect = null, Func\<T, bool\>? setdisabled = null, char? separatePath = null, Func\<T, string\> uniquenode = null).
    - AddNode(T value).
    - AddNode(T Parent, T value).

### Progress Bar Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlProgressBar\<T\> -> IProgressBarControl.
- Command initialization: PromptPlus.ProgressBar -> PromptPlus.Controls.ProgressBar.
    - Fixed Standard: ProgressBar(string prompt = "", string? description = null). 
    - Removed : ProgressBar\<T\>(ProgressBarType barType, string prompt, T defaultresult, string description = null);
    - Removed : ProgressBar\<T\>(ProgressBarType barType, string prompt, T defaultresult, string description, Action\<IPromptConfig\> config = null).
- Added:
    - Options(Action\<IControlOptions\> options).
    - Fill(ProgressBarType type).
    - Range(double minvalue, double maxvalue).
    - IntervalUpdate(int mileseconds = 100).
- Changed: 
    - ResultPrompt\<ResultProgessBar\<T\>\> Run(CancellationToken? value = null) -> ResultPrompt\<StateProgress\> Run(CancellationToken token = default).
    - PageSize(int value) -> PageSize(byte value).
    - Spinner(SpinnersType spinnersType, int? speedAnimation = null, IEnumerable\<string\>? customspinner = null) -> Spinner(SpinnersType spinnersType).
    - Width(int value) -> Width(byte value).
    - FracionalDig(int value) -> FracionalDig(byte value).
    - UpdateHandler(Action<UpdateProgressBar\<T\>, CancellationToken> value) -> UpdateHandler(Action<HandlerProgressBar, CancellationToken> value).
- Removed: 
    - Config(Action\<IPromptConfig\> context).
    - CharBar(char value).

### ReadLine Emacs Control (NEW)
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Command initialization: PromptPlus.Control.InputEmacs.
    - Standard : InputEmacs(string initialvalue = "")
- Commands: 
    - ValidateKey(Func<char, bool> validateKeyFunc).
    - MaxLength(int maxLength).
    - ReadOnly(bool value = true).
    - MaxWidth(int maxWidth).
    - CaseOptions(CaseOptions caseOptions).
    - EscAbort(bool escAbort = true).
    - string? ReadLine().

### Select Control
[**Main**](../README.md) | [**Top**](#promptplus-whats-new)

- Renamed interface : IControlSelect\<T\> -> ISelectControl\<T\>.
- Command initialization: PromptPlus.Select -> PromptPlus.Controls.Select.
    - Fixed Standard: Select\<T\>(string prompt = "", string? description = null). 
    - Removed : Select\<T\>(string prompt, Action\<IPromptConfig\> config = null).
    - Removed : Select\<T\>(string prompt, string? description, Action\<IPromptConfig\> config = null).
- Added:
    - PredicateSelected(Func\<T, (bool,string?)\> validselect).
    - PredicateSelected(Func\<T, bool)\> validselect).
    - EnabledHistory(string filename, Action\<IHistoryOptions\>? options = null).
    - Options(Action\<IControlOptions\> options).
- Changed: 
    - PageSize(int value) -> PageSize(byte value).
    - FilterType(FilterMode value) -> Filter(FilterMode value, bool caseinsensitive = true).
    - Interaction\<T1\>(IEnumerable\<T1\> values, Action\<IControlSelect\<T\>, T1\> action) -> Interaction(IEnumerable\<T\> items, Action\<T, ISelectControl\<T\>\> interactionAction)
    - Default(T value) -> Default(T value, bool usedefaultHistory = true).
    - ShowTipGroup(bool value = true) -> HideTipGroup(bool value = true).
    - AddItemGrouped(string group, T value, bool disable = false) -> AddGroupedItem(string group, T value, bool disable = false)
    - AddItemsGrouped(string group, IEnumerable\<T\> value, bool disable = false) -> AddGroupedItems(string group, IEnumerable\<T\> values, bool disable = false).
    - Separator(SeparatorLine separatorLine  = SeparatorLine.SingleLine, char? value = null) -> AddSeparator(SeparatorLine separatorLine = SeparatorLine.SingleLine, char? value = null).
- Removed: 
    - OrderBy(Expression\<Func\<T, object\>\> value).
    - OrderByDescending(Expression\<Func\<T, object\>\> value).
    - OverwriteDefaultFrom(string value, TimeSpan? timeout = null).
    - Config(Action\<IPromptConfig\> context).
    - AddItemsTo(AdderScope scope, params T[] values).
    - AddItemsTo(AdderScope scope, IEnumerable<T> values).
