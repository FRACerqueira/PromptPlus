# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IPromptConfig 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IPromptConfig

Namespace: PPlus.Controls

Interface for config controls to overwrite default values

```csharp
public interface IPromptConfig
```

## Methods

### <a id="methods-addextraaction"/>**AddExtraAction(StageControl, Action&lt;Object, Object&gt;)**

Add generic action for the control when change [StageControl](./pplus.controls.stagecontrol.md) of control

```csharp
IPromptConfig AddExtraAction(StageControl stage, Action<Object, Object> useraction)
```

#### Parameters

`stage` [StageControl](./pplus.controls.stagecontrol.md)<br>
Stage control

`useraction` [Action&lt;Object, Object&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-2)<br>
Action to execute.
 First param is generic conext([IPromptConfig.SetContext(Object)](./pplus.controls.ipromptconfig.md#setcontextobject)).
 Second param is curent input value of control.

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-description"/>**Description(String)**

Set description for the control

```csharp
IPromptConfig Description(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text description. Accept markup color

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-description"/>**Description(StringStyle)**

Set description for the control

```csharp
IPromptConfig Description(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>
Value description with style

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-disabletoggletooltip"/>**DisableToggleTooltip(Boolean)**

Overwrite default DisableToggleTooltip of control

```csharp
IPromptConfig DisableToggleTooltip(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-enabledabortkey"/>**EnabledAbortKey(Boolean)**

Overwrite default Enabled/Disabled AbortKey press of control

```csharp
IPromptConfig EnabledAbortKey(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-hideafterfinish"/>**HideAfterFinish(Boolean)**

Overwrite default Clear render area of control after finished

```csharp
IPromptConfig HideAfterFinish(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-hideonabort"/>**HideOnAbort(Boolean)**

Overwrite default Clear render area of control after AbortKey press

```csharp
IPromptConfig HideOnAbort(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-minimalrender"/>**MinimalRender(Boolean)**

Overwrite default Hide Answer
 <br>When true, the prompt and control description are not rendered, showing only the minimum necessary without using resources (except the default tooltips when used)

```csharp
IPromptConfig MinimalRender(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-paginationtemplate"/>**PaginationTemplate(Func&lt;Int32, Int32, Int32, String&gt;)**

Overwrite PaginationTemplate

```csharp
IPromptConfig PaginationTemplate(Func<Int32, Int32, Int32, String> value)
```

#### Parameters

`value` [Func&lt;Int32, Int32, Int32, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-4)<br>
The function
 <br>string to show = Func(Total items,Current Page,Total pages)

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-prompt"/>**Prompt(String)**

Set prompt for the control

```csharp
IPromptConfig Prompt(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text prompt. Accept markup color

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-prompt"/>**Prompt(StringStyle)**

Set prompt for the control

```csharp
IPromptConfig Prompt(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>
Value prompt with style

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-setcontext"/>**SetContext(Object)**

Set generic context for then control to pass in stage ExtraAction parameter

```csharp
IPromptConfig SetContext(object value)
```

#### Parameters

`value` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-showonlyexistingpagination"/>**ShowOnlyExistingPagination(Boolean)**

Overwrite default Show pagination only if exists

```csharp
IPromptConfig ShowOnlyExistingPagination(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-showtooltip"/>**ShowTooltip(Boolean)**

Overwrite default Show/Hide Tooltip of control

```csharp
IPromptConfig ShowTooltip(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-tooltips"/>**Tooltips(String)**

Set prompt for the control

```csharp
IPromptConfig Tooltips(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text Tooltips. Accept markup color.
 This text overwrite default tooltips control.

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### <a id="methods-tooltips"/>**Tooltips(StringStyle)**

Set Tooltips for the control. This value overwrite default tooltips control.

```csharp
IPromptConfig Tooltips(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>
Value tooltip with style

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)


- - -
[**Back to List Api**](./apis.md)
