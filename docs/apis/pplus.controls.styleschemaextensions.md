# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:StyleSchemaExtensions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# StyleSchemaExtensions

Namespace: PPlus.Controls

Represents The Style funtions Extensions

```csharp
public static class StyleSchemaExtensions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [StyleSchemaExtensions](./pplus.controls.styleschemaextensions.md)

## Methods

### <a id="methods-answer"/>**Answer(StyleSchema)**

Get [Style](./pplus.style.md) text Answer.
 <br>ValueResult Foreground : 'ConsoleColor.Cyan'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Answer(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-applybackground"/>**ApplyBackground(StyleSchema, StyleControls, Color)**

Apply global Background to [StyleControls](./pplus.controls.stylecontrols.md)

```csharp
public static Style ApplyBackground(StyleSchema schema, StyleControls styleControl, Color background)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>
The [StyleSchema](./pplus.controls.styleschema.md)

`styleControl` [StyleControls](./pplus.controls.stylecontrols.md)<br>
[StyleControls](./pplus.controls.stylecontrols.md) to apply

`background` [Color](./pplus.color.md)<br>
Background [Color](./pplus.color.md)

#### Returns

[Style](./pplus.style.md)

### <a id="methods-applyforeground"/>**ApplyForeground(StyleSchema, StyleControls, Color)**

Apply Foreground to [StyleControls](./pplus.controls.stylecontrols.md)

```csharp
public static Style ApplyForeground(StyleSchema schema, StyleControls styleControl, Color foreground)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>
The [StyleSchema](./pplus.controls.styleschema.md)

`styleControl` [StyleControls](./pplus.controls.stylecontrols.md)<br>
[StyleControls](./pplus.controls.stylecontrols.md) to apply

`foreground` [Color](./pplus.color.md)<br>
Foreground [Color](./pplus.color.md)

#### Returns

[Style](./pplus.style.md)

### <a id="methods-chart"/>**Chart(StyleSchema)**

Get [Style](./pplus.style.md) text Chart.
 <br>ValueResult Foreground : 'ConsoleColor.White'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Chart(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-description"/>**Description(StyleSchema)**

Get [Style](./pplus.style.md) text Description.
 <br>ValueResult Foreground : 'ConsoleColor.DarkYellow'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Description(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-disabled"/>**Disabled(StyleSchema)**

Get [Style](./pplus.style.md) text Disabled.
 <br>ValueResult Foreground : 'ConsoleColor.DarkGray'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Disabled(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-error"/>**Error(StyleSchema)**

Get [Style](./pplus.style.md) text Error.
 <br>ValueResult Foreground : 'ConsoleColor.Red'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Error(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-pagination"/>**Pagination(StyleSchema)**

Get [Style](./pplus.style.md) text Pagination.
 <br>ValueResult Foreground : 'ConsoleColor.DarkGray'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Pagination(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-prompt"/>**Prompt(StyleSchema)**

Get [Style](./pplus.style.md) text Prompt.
 <br>ValueResult Foreground : 'ConsoleColor.White'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Prompt(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-selected"/>**Selected(StyleSchema)**

Get [Style](./pplus.style.md) text Selected.
 <br>ValueResult Foreground : 'ConsoleColor.Green'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Selected(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-slider"/>**Slider(StyleSchema)**

Get [Style](./pplus.style.md) text Slider-On(Foreground)/Slider-Off(Background).
 <br>ValueResult Foreground : 'ConsoleColor.Cyan'<br>ValueResult Background : 'ConsoleColor.DarkGray'

```csharp
public static Style Slider(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-suggestion"/>**Suggestion(StyleSchema)**

Get [Style](./pplus.style.md) text Suggestion.
 <br>ValueResult Foreground : 'ConsoleColor.Yellow'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Suggestion(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-taggedinfo"/>**TaggedInfo(StyleSchema)**

Get [Style](./pplus.style.md) text Tagged info.
 <br>ValueResult Foreground : 'ConsoleColor.DarkYellow'<br>ValueResult Background : same Console Background when set

```csharp
public static Style TaggedInfo(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-tooltips"/>**Tooltips(StyleSchema)**

Get [Style](./pplus.style.md) text Tooltips.
 <br>ValueResult Foreground : 'ConsoleColor.DarkGray'<br>ValueResult Background : same Console Background when set

```csharp
public static Style Tooltips(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-unselected"/>**UnSelected(StyleSchema)**

Get [Style](./pplus.style.md) text UnSelected.
 <br>ValueResult Foreground : 'ConsoleColor.Gray'<br>ValueResult Background : same Console Background when set

```csharp
public static Style UnSelected(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)


- - -
[**Back to List Api**](./apis.md)
