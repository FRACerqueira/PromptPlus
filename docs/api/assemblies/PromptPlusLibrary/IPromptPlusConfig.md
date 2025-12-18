![PromptPlus Logo](https://raw.githubusercontent.com/FRACerqueira/PromptPlus/refs/heads/main/icon.png)

### IPromptPlusConfig interface
</br>


#### Defines global configuration settings applied across all PromptPlus controls (defaults, culture, hotkeys, symbols and layout).

```csharp
public interface IPromptPlusConfig
```

### Members

| name | description |
| --- | --- |
| [AfterError](IPromptPlusConfig/AfterError.md) { get; set; } | Gets or sets the action to invoke after an error occurs during processing. |
| [ChartWidth](IPromptPlusConfig/ChartWidth.md) { get; set; } | Gets Sets the width of the chart bar.Default value is 80. Valid range is 10–255; values outside the range are coerced to the nearest boundary. |
| [CompletionWaitToStart](IPromptPlusConfig/CompletionWaitToStart.md) { get; set; } | Number of milliseconds to wait before to start function autocomplete. Default value is 500 Valid range is &gt; 100; values outside the range are coerced to the nearest boundary. |
| [DefaultCulture](IPromptPlusConfig/DefaultCulture.md) { get; set; } | Gets or sets the default CultureInfo used for formatting and localization. |
| [EnabledAbortKey](IPromptPlusConfig/EnabledAbortKey.md) { get; set; } | Gets or sets whether the abort (Esc) hotkey is enabled globally. If `true`, Esc can abort controls. |
| [EnableMessageAbortCtrlC](IPromptPlusConfig/EnableMessageAbortCtrlC.md) { get; set; } | Gets or sets a value indicating whether a message is displayed when the operation is aborted by pressing Ctrl+C. Default value is true. |
| [FirstDayOfWeek](IPromptPlusConfig/FirstDayOfWeek.md) { get; set; } | Gets or sets the first day of the week for calendar-based controls (default: Sunday). |
| [FolderLog](IPromptPlusConfig/FolderLog.md) { get; set; } | Gets or sets the folder path where log files will be stored. |
| [HideAfterFinish](IPromptPlusConfig/HideAfterFinish.md) { get; set; } | Gets or sets whether a control’s render area is cleared after successful completion. If `true`, the area is cleared. |
| [HideOnAbort](IPromptPlusConfig/HideOnAbort.md) { get; set; } | Gets or sets whether a control’s render area is cleared after being aborted. If `true`, the area is cleared. |
| [HotKeyAbortKeyPress](IPromptPlusConfig/HotKeyAbortKeyPress.md) { get; } | Gets the global abort hotkey (default: Esc). |
| [HotKeyFilterMode](IPromptPlusConfig/HotKeyFilterMode.md) { get; set; } | Gets or sets the hotkey for toggling filter mode (default: F4). |
| [HotKeyPasswordView](IPromptPlusConfig/HotKeyPasswordView.md) { get; set; } | Gets or sets the hotkey for toggling password visibility (default: F2). |
| [HotKeyShowHistory](IPromptPlusConfig/HotKeyShowHistory.md) { get; set; } | Gets or sets the hotkey for showing input history entries (default: F3). |
| [HotKeySwitchNotes](IPromptPlusConfig/HotKeySwitchNotes.md) { get; set; } | Gets or sets the hotkey for toggling calendar notes display (default: F2). |
| [HotKeyToggleFullPath](IPromptPlusConfig/HotKeyToggleFullPath.md) { get; set; } | Gets or sets the hotkey for toggling full path display of files (default: F2). |
| [HotKeyTooltip](IPromptPlusConfig/HotKeyTooltip.md) { get; } | Gets the hotkey that toggles tooltip cycling (default: F1). |
| [HotKeyTooltipChartBarSwitchLayout](IPromptPlusConfig/HotKeyTooltipChartBarSwitchLayout.md) { get; set; } | Gets or sets the hotkey for chart bar layout switching (default: F2). |
| [HotKeyTooltipChartBarSwitchLegend](IPromptPlusConfig/HotKeyTooltipChartBarSwitchLegend.md) { get; set; } | Gets or sets the hotkey for chart bar legend visibility switching (default: F3). |
| [HotKeyTooltipChartBarSwitchOrder](IPromptPlusConfig/HotKeyTooltipChartBarSwitchOrder.md) { get; set; } | Gets or sets the hotkey for chart bar ordering switching (default: F4). |
| [HotKeyTooltipFilterAllSelected](IPromptPlusConfig/HotKeyTooltipFilterAllSelected.md) { get; set; } | Gets or sets the hotkey for Filter all selected items (default: F3). |
| [HotKeyTooltipShowHide](IPromptPlusConfig/HotKeyTooltipShowHide.md) { get; } | Gets the hotkey that shows/hides tooltips (default: Ctrl+F1). |
| [HotKeyTooltipToggleAll](IPromptPlusConfig/HotKeyTooltipToggleAll.md) { get; set; } | Gets or sets the hotkey for toggling selection of all items (default: F2). |
| [HotKeyTooltipToggleAllGroups](IPromptPlusConfig/HotKeyTooltipToggleAllGroups.md) { get; set; } | Gets or sets the hotkey used to toggle tooltips for all groups. |
| [MaxLenghtFilterText](IPromptPlusConfig/MaxLenghtFilterText.md) { get; set; } | Gets or sets the maximum length used when filtering text in controls. Default value is 15. Valid range is 5–30; values outside the range are coerced to the nearest boundary. |
| [MaxWidth](IPromptPlusConfig/MaxWidth.md) { get; set; } | Gets or sets the Sets the maximum display width for selected items text. Default is 30 characters. Valid range is 10–255; values outside the range are coerced to the nearest boundary. |
| [MinimumPrefixLength](IPromptPlusConfig/MinimumPrefixLength.md) { get; set; } | Number minimum of chars to accept autocomplete.Default value is 3. Valid range is 1–255; values outside the range are coerced to the nearest boundary. |
| [NoChar](IPromptPlusConfig/NoChar.md) { get; set; } | Gets or sets the character representing a logical “No” response (default: 'n'). |
| [PageSize](IPromptPlusConfig/PageSize.md) { get; set; } | Gets or sets the maximum number of items to display per page. Default value is 10. Valid range is 1–255; values outside the range are coerced to the nearest boundary. |
| [PaginationTemplate](IPromptPlusConfig/PaginationTemplate.md) { get; set; } | Gets or sets the pagination template function. Parameters: total items, current page (1-based), total pages. Returns a formatted string for display. |
| [ProgressBarWidth](IPromptPlusConfig/ProgressBarWidth.md) { get; set; } | Gets Sets the width of the Progress bar.Default value is 80. Valid range is 10–255; values outside the range are coerced to the nearest boundary. |
| [PromptMaskEdit](IPromptPlusConfig/PromptMaskEdit.md) { get; set; } | Gets Sets the character to use as the Prompt MaskEdit. Defaults is '_'. |
| [ResetBasicStateAfterExist](IPromptPlusConfig/ResetBasicStateAfterExist.md) { get; set; } | Gets or sets a value indicating whether the basic state should be reset after an exit operation. Default value is true. The Culture,cursor, foreground, and background colors are reset to their default values. |
| [SecretChar](IPromptPlusConfig/SecretChar.md) { get; set; } | Gets Sets the character to use as the secret mask input. Defaults is '#'. |
| [ShowMesssageAbortKey](IPromptPlusConfig/ShowMesssageAbortKey.md) { get; set; } | Gets or sets whether an abort message is shown after an abort occurs. If `true`, a localized message is displayed. |
| [ShowTooltip](IPromptPlusConfig/ShowTooltip.md) { get; set; } | Gets or sets whether tooltips are shown by default for controls. If `true`, tooltip rendering is enabled. |
| [SliderWidth](IPromptPlusConfig/SliderWidth.md) { get; set; } | Gets Sets the width of the Slider bar.Default value is 40. Valid range is 10–255; values outside the range are coerced to the nearest boundary. |
| [SwitchWidth](IPromptPlusConfig/SwitchWidth.md) { get; set; } | Gets Sets the width of the Progress bar.Default value is 6. Valid range is 6–255; values outside the range are coerced to the nearest boundary. |
| [YesChar](IPromptPlusConfig/YesChar.md) { get; set; } | Gets or sets the character representing a logical “Yes” response (default: 'y'). |
| [ChangeSymbol](IPromptPlusConfig/ChangeSymbol.md)(…) | Replaces a global symbol representation with ASCII and Unicode variants. |

### See Also

* namespace [PromptPlusLibrary](../PromptPlus.md)

<!-- DO NOT EDIT: generated by xmldocmd for PromptPlus.dll -->
