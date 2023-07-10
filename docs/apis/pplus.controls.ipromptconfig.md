# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IPromptConfig 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IPromptConfig

Namespace: PPlus.Controls

Interface for config controls and overwrite default values

```csharp
public interface IPromptConfig
```

## Methods

### **EnabledAbortKey(Boolean)**

Overwrite default Enabled/Disabled AbortKey press of control

```csharp
IPromptConfig EnabledAbortKey(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **ShowTooltip(Boolean)**

Overwrite default Show/Hide Tooltip of control

```csharp
IPromptConfig ShowTooltip(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **HideAfterFinish(Boolean)**

Overwrite default Clear render area of control after finished

```csharp
IPromptConfig HideAfterFinish(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **HideOnAbort(Boolean)**

Overwrite default Clear render area of control after AbortKey press

```csharp
IPromptConfig HideOnAbort(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **AddExtraAction(StageControl, Action&lt;Object, Object&gt;)**

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

### **SetContext(Object)**

Set generic context for then control to pass in stage ExtraAction parameter

```csharp
IPromptConfig SetContext(object value)
```

#### Parameters

`value` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **ApplyStyle(StyleControls, Style)**

Overwrite default style for [StyleControls](./pplus.controls.stylecontrols.md) of control

```csharp
IPromptConfig ApplyStyle(StyleControls styleControl, Style value)
```

#### Parameters

`styleControl` [StyleControls](./pplus.controls.stylecontrols.md)<br>
Style overwriter

`value` [Style](./pplus.style.md)<br>
value

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **Description(String)**

Set description for the control

```csharp
IPromptConfig Description(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text description. Accept markup color

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **Description(StringStyle)**

Set description for the control

```csharp
IPromptConfig Description(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>
Value description with style

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **Prompt(String)**

Set prompt for the control

```csharp
IPromptConfig Prompt(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text prompt. Accept markup color

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **Prompt(StringStyle)**

Set prompt for the control

```csharp
IPromptConfig Prompt(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>
Value prompt with style

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **Tooltips(String)**

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

### **Tooltips(StringStyle)**

Set Tooltips for the control. This value overwrite default tooltips control.

```csharp
IPromptConfig Tooltips(StringStyle value)
```

#### Parameters

`value` [StringStyle](./pplus.controls.stringstyle.md)<br>
Value tooltip with style

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)

### **Symbols(SymbolType, String, String)**

Overwrite default Symbols for [SymbolType](./pplus.controls.symboltype.md) of control

```csharp
IPromptConfig Symbols(SymbolType schema, string value, string unicode)
```

#### Parameters

`schema` [SymbolType](./pplus.controls.symboltype.md)<br>
Symbol overwriter

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text when **not** is-unicode supported

`unicode` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text when has is-unicode supported

#### Returns

[IPromptConfig](./pplus.controls.ipromptconfig.md)


- - -
[**Back to List Api**](./apis.md)
