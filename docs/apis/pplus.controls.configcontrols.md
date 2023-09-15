# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ConfigControls 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ConfigControls

Namespace: PPlus.Controls

Represents the common config properties for all controls.

```csharp
public class ConfigControls
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ConfigControls](./pplus.controls.configcontrols.md)

## Properties

### <a id="properties-calendarswitchnotespress"/>**CalendarSwitchNotesPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to toggle view notes in calendar.
 <br>Default value : '[F2]'

```csharp
public HotKey CalendarSwitchNotesPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-chartbarswitchlegendpress"/>**ChartBarSwitchLegendPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to toggle ChartBar Legend view.
 <br>Default value : '[F3]'

```csharp
public HotKey ChartBarSwitchLegendPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-chartbarswitchorderpress"/>**ChartBarSwitchOrderPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to toggle ChartBar Order view.
 <br>Default value : '[F4]'

```csharp
public HotKey ChartBarSwitchOrderPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-chartbarswitchtypepress"/>**ChartBarSwitchTypePress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to toggle ChartBar Layout view.
 <br>Default value : '[F2]'

```csharp
public HotKey ChartBarSwitchTypePress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-completionmaxcount"/>**CompletionMaxCount**

Get/Set Completion Max Items to return.
 <br>Default value : 1000. If value  less than 1 internal set to 1.

```csharp
public int CompletionMaxCount { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-completionminimumprefixlength"/>**CompletionMinimumPrefixLength**

Get/Set Minimum Prefix Length.
 <br>Default value : 3.If value less than 0 internal set to 0.

```csharp
public int CompletionMinimumPrefixLength { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-completionwaittostart"/>**CompletionWaitToStart**

Get/Set Interval in mileseconds to wait start Completion funcion.
 <br>Default value : 1000. If value less than 10 internal set to 10.

```csharp
public int CompletionWaitToStart { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-defaultculture"/>**DefaultCulture**

Get/Set default Culture([CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)) for all controls.

```csharp
public CultureInfo DefaultCulture { get; set; }
```

#### Property Value

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>

### <a id="properties-disabletoggletooltip"/>**DisableToggleTooltip**

Get/Set Disable toggle Tooltip for all controls at startup.
 <br>Default value : false

```csharp
public bool DisableToggleTooltip { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-edititempress"/>**EditItemPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Edit item.
 <br>Default value : '[F2]'

```csharp
public HotKey EditItemPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-enabledabortkey"/>**EnabledAbortKey**

Get/Set enable abortKey(ESC) for all controls.
 <br>Default value : true

```csharp
public bool EnabledAbortKey { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-fullpathpress"/>**FullPathPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) toggle current path to full path.
 <br>Default value : '[F2]'

```csharp
public HotKey FullPathPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-hideafterfinish"/>**HideAfterFinish**

Get/Set hide controls after finish for all controls.
 <br>Default value : false

```csharp
public bool HideAfterFinish { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-hideonabort"/>**HideOnAbort**

Get/Set hide controls On Abort for all controls.
 <br>Default value : false

```csharp
public bool HideOnAbort { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-historytimeout"/>**HistoryTimeout**

Get/Set History Timeout.
 <br>Default value : 365 days

```csharp
public TimeSpan HistoryTimeout { get; set; }
```

#### Property Value

[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

### <a id="properties-invertselectedpress"/>**InvertSelectedPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Invert Selected item.
 <br>Default value : '[F3]'

```csharp
public HotKey InvertSelectedPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-nochar"/>**NoChar**

Get/Set value for NO answer
 <br>Default value : NoChar in built-in resources.  Fall-back when null : N

```csharp
public Nullable<Char> NoChar { get; set; }
```

#### Property Value

[Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### <a id="properties-pagesize"/>**PageSize**

Get/Set Page Size from colletions.
 <br>Default value : 10. If value less than 1 internal set to 1.

```csharp
public int PageSize { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-passwordviewpress"/>**PasswordViewPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to toggle password view.
 <br>Default value : '[F2]'

```csharp
public HotKey PasswordViewPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-removeitempress"/>**RemoveItemPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Remove item.
 <br>Default value : '[F3]'

```csharp
public HotKey RemoveItemPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-secretchar"/>**SecretChar**

Get/Set value char for secret input
 <br>Default value : '#'.  Fall-back when null : '#'

```csharp
public Nullable<Char> SecretChar { get; set; }
```

#### Property Value

[Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### <a id="properties-selectallpress"/>**SelectAllPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Select all item.
 <br>Default value : '[F2]'

```csharp
public HotKey SelectAllPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-showonlyexistingpagination"/>**ShowOnlyExistingPagination**

Get/Set Show pagination only if exists
 <br>Default value : false

```csharp
public bool ShowOnlyExistingPagination { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-showtooltip"/>**ShowTooltip**

Get/Set enable show Tooltip for all controls at startup.
 <br>Default value : true

```csharp
public bool ShowTooltip { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-toggleexpandallpress"/>**ToggleExpandAllPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) Toggle Expand /Collapse All node.
 <br>Default value : '[F4]'

```csharp
public HotKey ToggleExpandAllPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-toggleexpandpress"/>**ToggleExpandPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) Toggle Expand/Collapse node.
 <br>Default value : '[F3]'

```csharp
public HotKey ToggleExpandPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-tooltipkeypress"/>**TooltipKeyPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to show/hide Tooltip.
 <br>Default value : '[F1]'

```csharp
public HotKey TooltipKeyPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### <a id="properties-yeschar"/>**YesChar**

Get/Set value for YES answer
 <br>Default value : YesChar in built-in resources.  Fall-back when null : Y

```csharp
public Nullable<Char> YesChar { get; set; }
```

#### Property Value

[Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

## Methods

### <a id="methods-symbols"/>**Symbols(SymbolType, String, String)**

Get/Set the Symbols for all controls.If empty params return current set.

```csharp
public ValueTuple<String, String> Symbols(SymbolType schema, string value, string unicode)
```

#### Parameters

`schema` [SymbolType](./pplus.controls.symboltype.md)<br>
[SymbolType](./pplus.controls.symboltype.md) to set

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text symbol when not unicode support

`unicode` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text symbol when has unicode support

#### Returns

(string value, string unicode) value


- - -
[**Back to List Api**](./apis.md)
