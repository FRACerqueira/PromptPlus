# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus StyleSchema 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# StyleSchema

Namespace: PPlus.Controls

Represents The Styles Schema of current instance of control

```csharp
public class StyleSchema
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [StyleSchema](./pplus.controls.styleschema.md)

## Methods

### **ApplyStyle(StyleControls, Style)**

Apply style current instance of control

```csharp
public Style ApplyStyle(StyleControls styleControl, Style value)
```

#### Parameters

`styleControl` [StyleControls](./pplus.controls.stylecontrols.md)<br>
[StyleControls](./pplus.controls.stylecontrols.md) to apply

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md) value to apply

#### Returns

[Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

### **GetStyle(StyleControls)**

```csharp
internal Style GetStyle(StyleControls styleControls)
```

#### Parameters

`styleControls` [StyleControls](./pplus.controls.stylecontrols.md)<br>

#### Returns

[Style](./pplus.style.md)<br>

### **UpdateBackgoundColor(Color)**

```csharp
internal void UpdateBackgoundColor(Color backgoundcolor)
```

#### Parameters

`backgoundcolor` [Color](./pplus.color.md)<br>


- - -
[**Back to List Api**](./apis.md)
