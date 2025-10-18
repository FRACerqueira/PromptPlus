# <img align="left" width="100" height="100" src="../icon.png">PromptPlus What's new
[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

## V.5.0.0

**The version 5 has been completely redesigned** and optimized for better stability, consistency, and performance. 
All controls and behaviors have been revisited and improved to ensure sustainable evolution. 
Due to the significant modifications, version 5 introduced **significant changes and is incompatible with versions 4.x**, although the concepts and components are very similar, requiring a small learning curve and minor methodological adjustments.

### Table of Contents

- [General changes](#general-changes)
- [PromptPlus Config](#config)
- [Console Commands](#console-commands)
- [Options(all Controls)](#options)
- [AutoComplete Control](#autocomplete-control)
- [Banner Widget](#banner-widget)
- [Calendar Control](#calendar-control)
- [Calendar Widget](#calendar-widget-new) **NEW!** 
- [Chart Bar Control](#chart-bar-control)
- [Chart Bar Widget](#chart-bar-widget-new) **NEW!** 

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
- All no interative controls start at : **PromptPlus.Widgets**.\<name of control\>.
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
    - PredicateSelected(Func<string, bool> validselect).
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
    - PredicateSelected(Func<string, bool> validselect).
    - Options(Action\<IControlOptions\> options).
- Changed: 
    - Layout(CalendarLayout value) -> Layout(CalendarLayout layout = CalendarLayout.SingleGrid)
    - Interaction\<T1\>(IEnumerable\<T1\> values, Action\<IControlCalendar, T1\> action) -> Interaction\<T\>(IEnumerable\<T\> items, Action\<T, ICalendarControl\> interactionaction)
    - PageSize(int value) -> PageSize(byte value).
    - 
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
