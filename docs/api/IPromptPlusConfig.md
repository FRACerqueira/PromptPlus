<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IPromptPlusConfig Interface

Defines global configuration settings applied across all PromptPlus controls \(defaults, culture, hotkeys, symbols and layout\)\.

```csharp
public interface IPromptPlusConfig
```
### Fields

<a name='PromptPlusLibrary.IPromptPlusConfig.NameResourcePromptPlusConfigFile'></a>

## IPromptPlusConfig\.NameResourcePromptPlusConfigFile Field

Gets the name of the configuration file used for PromptPlus\.

```csharp
const string NameResourcePromptPlusConfigFile = "PromptPlus.config";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Properties

<a name='PromptPlusLibrary.IPromptPlusConfig.ChartWidth'></a>

## IPromptPlusConfig\.ChartWidth Property

Gets or sets the width of the chart bar\.
Default value is 80\. 
Valid range is 10–255; values outside the range are coerced to the nearest boundary\.

```csharp
byte ChartWidth { get; set; }
```

#### Property Value
[System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

<a name='PromptPlusLibrary.IPromptPlusConfig.ContrastRatio'></a>

## IPromptPlusConfig\.ContrastRatio Property

Gets or sets the contrast ratio used for foreground colour selection in controls\.
Default: `2.7`\.

```csharp
double ContrastRatio { get; set; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

### Remarks
The best contrast ratio for readability is 4\.5 or higher, but this may not be achievable with all colour combinations\.
The zero value disables contrast ratio checking, allowing any colour combination to be used\.

<a name='PromptPlusLibrary.IPromptPlusConfig.DefaultCulture'></a>

## IPromptPlusConfig\.DefaultCulture Property

Gets or sets the default [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') used for formatting and localisation\.

```csharp
System.Globalization.CultureInfo DefaultCulture { get; set; }
```

#### Property Value
[System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

### Remarks
Default: [System\.Globalization\.CultureInfo\.CurrentCulture](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.currentculture 'System\.Globalization\.CultureInfo\.CurrentCulture') at the time the configuration is created\.

<a name='PromptPlusLibrary.IPromptPlusConfig.EnabledAbortKey'></a>

## IPromptPlusConfig\.EnabledAbortKey Property

Gets or sets whether the abort \(Esc\) hotkey is enabled globally\. 
Default value is true\.
If `true`, Esc can abort controls\.

```csharp
bool EnabledAbortKey { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.IPromptPlusConfig.FirstDayOfWeek'></a>

## IPromptPlusConfig\.FirstDayOfWeek Property

Gets or sets the first day of the week used by calendar\-based controls\.
Default: [System\.DayOfWeek\.Sunday](https://learn.microsoft.com/en-us/dotnet/api/system.dayofweek.sunday 'System\.DayOfWeek\.Sunday')\.

```csharp
System.DayOfWeek FirstDayOfWeek { get; set; }
```

#### Property Value
[System\.DayOfWeek](https://learn.microsoft.com/en-us/dotnet/api/system.dayofweek 'System\.DayOfWeek')

<a name='PromptPlusLibrary.IPromptPlusConfig.HideAfterFinish'></a>

## IPromptPlusConfig\.HideAfterFinish Property

Gets or sets whether a control’s render area is cleared after successful completion\. 
Default value is false\.
If `true`, the area is cleared\.

```csharp
bool HideAfterFinish { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.IPromptPlusConfig.HideOnAbort'></a>

## IPromptPlusConfig\.HideOnAbort Property

Gets or sets whether a control’s render area is cleared after being aborted\. 
Default value is false\.         
If `true`, the area is cleared\.

```csharp
bool HideOnAbort { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyAbortKeyPress'></a>

## IPromptPlusConfig\.HotKeyAbortKeyPress Property

Gets the global abort hotkey \(default: Esc\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyAbortKeyPress { get; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyCalendarSwitchNotes'></a>

## IPromptPlusConfig\.HotKeyCalendarSwitchNotes Property

Gets or sets the hotkey for toggling calendar notes display \(default: F2\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyCalendarSwitchNotes { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyChartBarSwitchLayout'></a>

## IPromptPlusConfig\.HotKeyChartBarSwitchLayout Property

Gets or sets the hotkey for chart bar layout switching \(default: F2\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyChartBarSwitchLayout { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyChartBarSwitchLegend'></a>

## IPromptPlusConfig\.HotKeyChartBarSwitchLegend Property

Gets or sets the hotkey for chart bar legend visibility switching \(default: F3\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyChartBarSwitchLegend { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyChartBarSwitchOrder'></a>

## IPromptPlusConfig\.HotKeyChartBarSwitchOrder Property

Gets or sets the hotkey for chart bar ordering switching \(default: F4\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyChartBarSwitchOrder { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyFilterAllSelected'></a>

## IPromptPlusConfig\.HotKeyFilterAllSelected Property

Gets or sets the hotkey for filtering all selected items \(default: F3\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyFilterAllSelected { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyInputHistoryView'></a>

## IPromptPlusConfig\.HotKeyInputHistoryView Property

Gets or sets the hotkey for showing input history entries \(default: F3\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyInputHistoryView { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyInputPasswordView'></a>

## IPromptPlusConfig\.HotKeyInputPasswordView Property

Gets or sets the hotkey for toggling password visibility \(default: F2\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyInputPasswordView { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyToggleAll'></a>

## IPromptPlusConfig\.HotKeyToggleAll Property

Gets or sets the hotkey for toggling selection of all items \(default: F2\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyToggleAll { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyToggleFullPath'></a>

## IPromptPlusConfig\.HotKeyToggleFullPath Property

Gets or sets the hotkey for toggling full path display of files \(default: Shift\+F3\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyToggleFullPath { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyTooltip'></a>

## IPromptPlusConfig\.HotKeyTooltip Property

Gets or sets the hotkey that toggles tooltip cycling \(default: F1\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyTooltip { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.HotKeyTooltipShowHide'></a>

## IPromptPlusConfig\.HotKeyTooltipShowHide Property

Gets or sets the hotkey that shows/hides tooltips \(default: Ctrl\+F1\)\.

```csharp
PromptPlusLibrary.HotKey HotKeyTooltipShowHide { get; set; }
```

#### Property Value
[HotKey](HotKey.md 'PromptPlusLibrary\.HotKey')

<a name='PromptPlusLibrary.IPromptPlusConfig.MaxLenghtFilterText'></a>

## IPromptPlusConfig\.MaxLenghtFilterText Property

Gets or sets the maximum length used when filtering text in controls\.
Default value is 25\. 
Valid range is 5–50; values outside the range are coerced to the nearest boundary\.

```csharp
byte MaxLenghtFilterText { get; set; }
```

#### Property Value
[System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

<a name='PromptPlusLibrary.IPromptPlusConfig.NoChar'></a>

## IPromptPlusConfig\.NoChar Property

Gets or sets the character representing a logical "No" response\.

```csharp
char NoChar { get; set; }
```

#### Property Value
[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

### Remarks
Default: `'n'` \(culture\-dependent; taken from localised resources when not set\)\.

<a name='PromptPlusLibrary.IPromptPlusConfig.PageSize'></a>

## IPromptPlusConfig\.PageSize Property

Gets or sets the maximum number of items displayed per page\.
Default value is 0\.
Valid range is 0\-255\. A value of 0 automatically calculates page size based on screen height, reserving lines for header, footer, and pagination\.
If the value is greater than the available height \(minus reserved lines\), it is coerced to the maximum allowed value\.

```csharp
byte PageSize { get; set; }
```

#### Property Value
[System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

<a name='PromptPlusLibrary.IPromptPlusConfig.PrefixExtraInfo'></a>

## IPromptPlusConfig\.PrefixExtraInfo Property

Gets or sets the prefix string appended before extra info text\.

```csharp
string PrefixExtraInfo { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.IPromptPlusConfig.ProgressBarWidth'></a>

## IPromptPlusConfig\.ProgressBarWidth Property

Gets or sets the width of the progress bar\.
Default value is 40\. 
Valid range is 10–255; values outside the range are coerced to the nearest boundary\.

```csharp
byte ProgressBarWidth { get; set; }
```

#### Property Value
[System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

<a name='PromptPlusLibrary.IPromptPlusConfig.PromptMaskEdit'></a>

## IPromptPlusConfig\.PromptMaskEdit Property

Gets or sets the character to use as the prompt mask input\.
Default is '\_'\.

```csharp
char PromptMaskEdit { get; set; }
```

#### Property Value
[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

<a name='PromptPlusLibrary.IPromptPlusConfig.RemoveHandlerCtrlC'></a>

## IPromptPlusConfig\.RemoveHandlerCtrlC Property

Gets or sets whether the library should handle Ctrl\+C key presses to abort operations\.

```csharp
bool RemoveHandlerCtrlC { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

### Remarks
Default: `false`\.

<a name='PromptPlusLibrary.IPromptPlusConfig.SecretChar'></a>

## IPromptPlusConfig\.SecretChar Property

Gets or sets the character to use as the secret mask input\. 
Default is '\#'\.

```csharp
char SecretChar { get; set; }
```

#### Property Value
[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

<a name='PromptPlusLibrary.IPromptPlusConfig.ShowMessageAbortKey'></a>

## IPromptPlusConfig\.ShowMessageAbortKey Property

Gets or sets whether an abort message is shown after an abort occurs\. 
Default value is true\.
If `true`, a localized message is displayed\.

```csharp
bool ShowMessageAbortKey { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.IPromptPlusConfig.ShowTooltip'></a>

## IPromptPlusConfig\.ShowTooltip Property

Gets or sets whether tooltips are shown by default for controls\. 
Default value is true\.
If `true`, tooltip rendering is enabled\.

```csharp
bool ShowTooltip { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.IPromptPlusConfig.SliderWidth'></a>

## IPromptPlusConfig\.SliderWidth Property

Gets or sets the width of the slider bar\.
Default value is 30\. 
Valid range is 10–100; values outside the range are coerced to the nearest boundary\.

```csharp
byte SliderWidth { get; set; }
```

#### Property Value
[System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

<a name='PromptPlusLibrary.IPromptPlusConfig.SuffixExtraInfo'></a>

## IPromptPlusConfig\.SuffixExtraInfo Property

Gets or sets the suffix string appended after extra info text\.

```csharp
string SuffixExtraInfo { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='PromptPlusLibrary.IPromptPlusConfig.SufixAfterPrompt'></a>

## IPromptPlusConfig\.SufixAfterPrompt Property

Gets or sets the suffix string to append after the prompt text\.

```csharp
string SufixAfterPrompt { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Remarks
Default: ': ' \(colon \+ space\)

<a name='PromptPlusLibrary.IPromptPlusConfig.SwitchWidth'></a>

## IPromptPlusConfig\.SwitchWidth Property

Gets or sets the width of the switch bar\.
Default value is 4\.  
Valid range is 4–10; values outside the range are coerced to the nearest boundary\.

```csharp
byte SwitchWidth { get; set; }
```

#### Property Value
[System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

<a name='PromptPlusLibrary.IPromptPlusConfig.YesChar'></a>

## IPromptPlusConfig\.YesChar Property

Gets or sets the character representing a logical "Yes" response\.

```csharp
char YesChar { get; set; }
```

#### Property Value
[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

### Remarks
Default: `'y'` \(culture\-dependent; taken from localised resources when not set\)\.
### Methods

<a name='PromptPlusLibrary.IPromptPlusConfig.ToFile(string)'></a>

## IPromptPlusConfig\.ToFile\(string\) Method

Creates a configuration file for PromptPlus using the name [NameResourcePromptPlusConfigFile](IPromptPlusConfig.md#PromptPlusLibrary.IPromptPlusConfig.NameResourcePromptPlusConfigFile 'PromptPlusLibrary\.IPromptPlusConfig\.NameResourcePromptPlusConfigFile')\.

```csharp
void ToFile(string foldername);
```
#### Parameters

<a name='PromptPlusLibrary.IPromptPlusConfig.ToFile(string).foldername'></a>

`foldername` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The folder path where [NameResourcePromptPlusConfigFile](IPromptPlusConfig.md#PromptPlusLibrary.IPromptPlusConfig.NameResourcePromptPlusConfigFile 'PromptPlusLibrary\.IPromptPlusConfig\.NameResourcePromptPlusConfigFile') will be created\. Cannot be `null` or empty\.