# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:BaseOptions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# BaseOptions

Namespace: PPlus.Controls

```csharp
public abstract class BaseOptions : IPromptConfig
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [BaseOptions](./pplus.controls.baseoptions.md)<br>
Implements [IPromptConfig](./pplus.controls.ipromptconfig.md)

## Methods

### <a id="methods-addextraaction"/>**AddExtraAction(StageControl, Action&lt;Object, Object&gt;)**

```csharp
public IPromptConfig AddExtraAction(StageControl stage, Action<Object, Object> useraction)
```

#### Parameters

`stage` [StageControl](./pplus.controls.stagecontrol.md)<br>

`useraction` [Action&lt;Object, Object&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-2)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-applystyle"/>**ApplyStyle(StyleControls, Style)**

```csharp
public IPromptConfig ApplyStyle(StyleControls styleControl, Style value)
```

#### Parameters

`styleControl` [StyleControls](./pplus.controls.stylecontrols.md)<br>

`value` [Style](./pplus.style.md)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-description"/>**Description(StringStyle)**

```csharp
public IPromptConfig Description(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-description"/>**Description(String)**

```csharp
public IPromptConfig Description(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-disabletoggletooltip"/>**DisableToggleTooltip(Boolean)**

```csharp
public IPromptConfig DisableToggleTooltip(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-enabledabortkey"/>**EnabledAbortKey(Boolean)**

```csharp
public IPromptConfig EnabledAbortKey(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-hideafterfinish"/>**HideAfterFinish(Boolean)**

```csharp
public IPromptConfig HideAfterFinish(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-hideanswer"/>**HideAnswer(Boolean)**

```csharp
public IPromptConfig HideAnswer(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-hideonabort"/>**HideOnAbort(Boolean)**

```csharp
public IPromptConfig HideOnAbort(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-prompt"/>**Prompt(StringStyle)**

```csharp
public IPromptConfig Prompt(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-prompt"/>**Prompt(String)**

```csharp
public IPromptConfig Prompt(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-setcontext"/>**SetContext(Object)**

```csharp
public IPromptConfig SetContext(object value)
```

#### Parameters

`value` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-showonlyexistingpagination"/>**ShowOnlyExistingPagination(Boolean)**

```csharp
public IPromptConfig ShowOnlyExistingPagination(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-showtooltip"/>**ShowTooltip(Boolean)**

```csharp
public IPromptConfig ShowTooltip(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-symbols"/>**Symbols(SymbolType, String, String)**

```csharp
public IPromptConfig Symbols(SymbolType schema, string value, string unicode)
```

#### Parameters

`schema` [SymbolType](./pplus.controls.symboltype.md)<br>

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`unicode` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-tooltips"/>**Tooltips(StringStyle)**

```csharp
public IPromptConfig Tooltips(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-tooltips"/>**Tooltips(String)**

```csharp
public IPromptConfig Tooltips(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)


- - -
[**Back to List Api**](./apis.md)
