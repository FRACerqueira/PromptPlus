# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:StyleSchemaExtensions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
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
 <br>Foreground : 'ConsoleColor.Cyan'<br>Background : same Console Background when not set

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

### <a id="methods-browserfile"/>**BrowserFile(StyleSchema)**

Get [Style](./pplus.style.md) text Browser File.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style BrowserFile(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-browserfolder"/>**BrowserFolder(StyleSchema)**

Get [Style](./pplus.style.md) text Browser Folder.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style BrowserFolder(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-browsersize"/>**BrowserSize(StyleSchema)**

Get [Style](./pplus.style.md) text Browser Size.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style BrowserSize(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-calendarday"/>**CalendarDay(StyleSchema)**

Get [Style](./pplus.style.md) CalendarDay.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style CalendarDay(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-calendarhighlight"/>**CalendarHighlight(StyleSchema)**

Get [Style](./pplus.style.md) CalendarHighlight.
 <br>Foreground : 'ConsoleColor.DarkYellow'<br>Background : same Console Background when not set

```csharp
public static Style CalendarHighlight(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-calendarmonth"/>**CalendarMonth(StyleSchema)**

Get [Style](./pplus.style.md) CalendarMonth.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style CalendarMonth(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-calendarweekday"/>**CalendarWeekDay(StyleSchema)**

Get [Style](./pplus.style.md) CalendarWeekDay.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style CalendarWeekDay(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-calendaryear"/>**CalendarYear(StyleSchema)**

Get [Style](./pplus.style.md) CalendarYear.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style CalendarYear(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-chartlabel"/>**ChartLabel(StyleSchema)**

Get [Style](./pplus.style.md) ChartLabel.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style ChartLabel(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-chartorder"/>**ChartOrder(StyleSchema)**

Get [Style](./pplus.style.md) ChartOrder.
 <br>Foreground : 'ConsoleColor.DarkGray'<br>Background : same Console Background when not set

```csharp
public static Style ChartOrder(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-chartpercent"/>**ChartPercent(StyleSchema)**

Get [Style](./pplus.style.md) ChartPercent.
 <br>Foreground : 'ConsoleColor.DarkYellow'<br>Background : same Console Background when not set

```csharp
public static Style ChartPercent(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-charttitle"/>**ChartTitle(StyleSchema)**

Get [Style](./pplus.style.md) ChartTitle.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style ChartTitle(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-chartvalue"/>**ChartValue(StyleSchema)**

Get [Style](./pplus.style.md) ChartValue.
 <br>Foreground : 'ConsoleColor.Cyan'<br>Background : same Console Background when not set

```csharp
public static Style ChartValue(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-description"/>**Description(StyleSchema)**

Get [Style](./pplus.style.md) text Description.
 <br>Foreground : 'ConsoleColor.DarkYellow'<br>Background : same Console Background when not set

```csharp
public static Style Description(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-disabled"/>**Disabled(StyleSchema)**

Get [Style](./pplus.style.md) text Disabled.
 <br>Foreground : 'ConsoleColor.DarkGray'<br>Background : same Console Background when not set

```csharp
public static Style Disabled(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-error"/>**Error(StyleSchema)**

Get [Style](./pplus.style.md) text Error.
 <br>Foreground : 'ConsoleColor.Red'<br>Background : same Console Background when not set

```csharp
public static Style Error(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-filtermatch"/>**FilterMatch(StyleSchema)**

Get [Style](./pplus.style.md) FilterMatch.
 <br>Foreground : 'ConsoleColor.Yellow'<br>Background : same Console Background when not set

```csharp
public static Style FilterMatch(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-filterunmatch"/>**FilterUnMatch(StyleSchema)**

Get [Style](./pplus.style.md) FilterUnMatch.
 <br>Foreground : 'ConsoleColor.Cyan'<br>Background : same Console Background when not set

```csharp
public static Style FilterUnMatch(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-grouptip"/>**GroupTip(StyleSchema)**

Get [Style](./pplus.style.md) GroupTip.
 <br>Foreground : 'ConsoleColor.DarkGray'<br>Background : same Console Background when not set

```csharp
public static Style GroupTip(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-lines"/>**Lines(StyleSchema)**

Get [Style](./pplus.style.md) Lines.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style Lines(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-masknegative"/>**MaskNegative(StyleSchema)**

Get [Style](./pplus.style.md) MaskNegative.
 <br>Foreground : 'ConsoleColor.Cyan'<br>Background : same Console Background when not set

```csharp
public static Style MaskNegative(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-maskpositive"/>**MaskPositive(StyleSchema)**

Get [Style](./pplus.style.md) MaskPositive.
 <br>Foreground : 'ConsoleColor.Cyan'<br>Background : same Console Background when not set

```csharp
public static Style MaskPositive(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-masktypetip"/>**MaskTypeTip(StyleSchema)**

Get [Style](./pplus.style.md) MaskTypeTip.
 <br>Foreground : 'ConsoleColor.Yellow'<br>Background : same Console Background when not set

```csharp
public static Style MaskTypeTip(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-onoff"/>**OnOff(StyleSchema)**

Get [Style](./pplus.style.md) GroupTip.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style OnOff(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-pagination"/>**Pagination(StyleSchema)**

Get [Style](./pplus.style.md) text Pagination.
 <br>Foreground : 'ConsoleColor.DarkGray'<br>Background : same Console Background when not set

```csharp
public static Style Pagination(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-prompt"/>**Prompt(StyleSchema)**

Get [Style](./pplus.style.md) text Prompt.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style Prompt(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-ranger"/>**Ranger(StyleSchema)**

Get [Style](./pplus.style.md) Range.
 <br>Foreground : 'ConsoleColor.DarkYellow'<br>Background : same Console Background when not set

```csharp
public static Style Ranger(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-selected"/>**Selected(StyleSchema)**

Get [Style](./pplus.style.md) text Selected.
 <br>Foreground : 'ConsoleColor.Green'<br>Background : same Console Background when not set

```csharp
public static Style Selected(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-slider"/>**Slider(StyleSchema)**

Get [Style](./pplus.style.md) text Slider-On(Foreground)/Slider-Off(Background).
 <br>Foreground : 'ConsoleColor.Cyan'<br>Background : 'ConsoleColor.DarkGray'

```csharp
public static Style Slider(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-spinner"/>**Spinner(StyleSchema)**

Get [Style](./pplus.style.md) Spinner.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style Spinner(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-suggestion"/>**Suggestion(StyleSchema)**

Get [Style](./pplus.style.md) text Suggestion.
 <br>Foreground : 'ConsoleColor.Yellow'<br>Background : same Console Background when not set

```csharp
public static Style Suggestion(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-tablecontent"/>**TableContent(StyleSchema)**

Get [Style](./pplus.style.md) TableContent.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TableContent(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-tableheader"/>**TableHeader(StyleSchema)**

Get [Style](./pplus.style.md) TableHeader.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TableHeader(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-tabletitle"/>**TableTitle(StyleSchema)**

Get [Style](./pplus.style.md) TableTitle.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TableTitle(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-taggedinfo"/>**TaggedInfo(StyleSchema)**

Get [Style](./pplus.style.md) text Tagged info.
 <br>Foreground : 'ConsoleColor.DarkYellow'<br>Background : same Console Background when not set

```csharp
public static Style TaggedInfo(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-taskelapsedtime"/>**TaskElapsedTime(StyleSchema)**

Get [Style](./pplus.style.md) text TaskElapsedTime.
 <br>Foreground : 'ConsoleColor.DarkYellow'<br>Background : same Console Background when not set

```csharp
public static Style TaskElapsedTime(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-tasktitle"/>**TaskTitle(StyleSchema)**

Get [Style](./pplus.style.md) text TaskTitle.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TaskTitle(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-tooltips"/>**Tooltips(StyleSchema)**

Get [Style](./pplus.style.md) text Tooltips.
 <br>Foreground : 'ConsoleColor.DarkGray'<br>Background : same Console Background when not set

```csharp
public static Style Tooltips(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-treeviewchild"/>**TreeViewChild(StyleSchema)**

Get [Style](./pplus.style.md) text TreeView Child.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TreeViewChild(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-treeviewexpand"/>**TreeViewExpand(StyleSchema)**

Get [Style](./pplus.style.md) text TreeView Expand.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TreeViewExpand(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-treeviewparent"/>**TreeViewParent(StyleSchema)**

Get [Style](./pplus.style.md) text TreeView Parent.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TreeViewParent(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-treeviewroot"/>**TreeViewRoot(StyleSchema)**

Get [Style](./pplus.style.md) text TreeView Root.
 <br>Foreground : 'ConsoleColor.White'<br>Background : same Console Background when not set

```csharp
public static Style TreeViewRoot(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)

### <a id="methods-unselected"/>**UnSelected(StyleSchema)**

Get [Style](./pplus.style.md) text UnSelected.
 <br>Foreground : 'ConsoleColor.Gray'<br>Background : same Console Background when not set

```csharp
public static Style UnSelected(StyleSchema schema)
```

#### Parameters

`schema` [StyleSchema](./pplus.controls.styleschema.md)<br>

#### Returns

[Style](./pplus.style.md)


- - -
[**Back to List Api**](./apis.md)
