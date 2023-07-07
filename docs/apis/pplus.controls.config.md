# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus Config 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# Config

Namespace: PPlus.Controls

Represents the common config properties for all controls.

```csharp
public class Config
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Config](./pplus.controls.config.md)

## Properties

### **DefaultCulture**

Get/Set default Culture([CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)) for all controls.

```csharp
public CultureInfo DefaultCulture { get; set; }
```

#### Property Value

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>

### **PageSize**

```csharp
public int PageSize { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **CompletionMinimumPrefixLength**

```csharp
public int CompletionMinimumPrefixLength { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **CompletionWaitToStart**

Get/Set Interval in mileseconds to wait start Completion funcion.

<br>

```csharp
public int CompletionWaitToStart { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **CompletionMaxCount**

Get/Set Completion Max Items to return.

<br>

```csharp
public int CompletionMaxCount { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **HistoryTimeout**

Get/Set History Timeout.

<br>

```csharp
public TimeSpan HistoryTimeout { get; set; }
```

#### Property Value

[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

### **ShowTooltip**

Get/Set enabled show Tooltip for all controls.

<br>

```csharp
public bool ShowTooltip { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **EnabledAbortKey**

Get/Set enabled abortKey(ESC) for all controls.

<br>

```csharp
public bool EnabledAbortKey { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **HideAfterFinish**

Get/Set hide controls after finish for all controls.

<br>

```csharp
public bool HideAfterFinish { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **HideOnAbort**

Get/Set hide controls On Abort for all controls.

<br>

```csharp
public bool HideOnAbort { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **SecretChar**

Get/Set value char for secret input

<br>

```csharp
public Nullable<char> SecretChar { get; set; }
```

#### Property Value

[Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### **YesChar**

Get/Set value for YES answer

<br>

```csharp
public Nullable<char> YesChar { get; set; }
```

#### Property Value

[Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### **NoChar**

Get/Set value for NO answer

<br>

```csharp
public Nullable<char> NoChar { get; set; }
```

#### Property Value

[Nullable&lt;Char&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

### **TooltipKeyPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to show/hide Tooltip.

<br>

```csharp
public HotKey TooltipKeyPress { get; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **PasswordViewPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to toggle password view.

<br>

```csharp
public HotKey PasswordViewPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **SelectAllPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Select all item.

<br>

```csharp
public HotKey SelectAllPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **InvertSelectedPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Invert Selected item.

<br>

```csharp
public HotKey InvertSelectedPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **EditItemPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Edit item.

<br>

```csharp
public HotKey EditItemPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **RemoveItemPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) to Remove item.

<br>

```csharp
public HotKey RemoveItemPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **FullPathPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) toggle current path to full path.

<br>

```csharp
public HotKey FullPathPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **ToggleExpandPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) Toggle Expand/Collapse node.

<br>

```csharp
public HotKey ToggleExpandPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

### **ToggleExpandAllPress**

Get/Set [HotKey](./pplus.controls.hotkey.md) Toggle Expand /Collapse All node.

<br>

```csharp
public HotKey ToggleExpandAllPress { get; set; }
```

#### Property Value

[HotKey](./pplus.controls.hotkey.md)<br>

## Methods

### **Symbols(SymbolType, String, String)**

Get/Set the Symbols for all controls.If empty params return current set.

```csharp
public ValueTuple<string, string> Symbols(SymbolType schema, string value, string unicode)
```

#### Parameters

`schema` [SymbolType](./pplus.controls.symboltype.md)<br>
[SymbolType](./pplus.controls.symboltype.md) to set

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text symbol when not unicode support

`unicode` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text symbol when has unicode support

#### Returns

[ValueTuple&lt;String, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.valuetuple-2)<br>
(string value, string unicode) value


- - -
[**Back to List Api**](./apis.md)
