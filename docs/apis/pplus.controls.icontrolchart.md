# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlChart 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlChart

Namespace: PPlus.Controls

Represents the Breakdown ChartBar Control interface

```csharp
public interface IControlChart
```

## Methods

### <a id="methods-additem"/>**AddItem(String, Double, Nullable&lt;Color&gt;)**

Add item to ChartBar

```csharp
IControlChart AddItem(string label, double value, Nullable<Color> colorbar)
```

#### Parameters

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label Item to add

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
Value to Item

`colorbar` [Nullable&lt;Color&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Color](./pplus.color.md) bar. If not informed, the colorbar will be chosen in sequence starting at zero.

#### Returns

[IControlMultiSelect&lt;T&gt;](./pplus.controls.icontrolmultiselect-1.md)

### <a id="methods-charbar"/>**CharBar(Char)**

Set [Char](https://docs.microsoft.com/en-us/dotnet/api/system.char) to show progress.Default value '#'
 <br>Valid on ProgressBarType.Char, otherwise is ignored

```csharp
IControlChart CharBar(char value)
```

#### Parameters

`value` [Char](https://docs.microsoft.com/en-us/dotnet/api/system.char)<br>
Char to show

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-chartpadleft"/>**ChartPadLeft(Int32)**

PadLeft bar chart with spaces

```csharp
IControlChart ChartPadLeft(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of spaces

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-culture"/>**Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to on show value format.

```csharp
IControlChart Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-culture"/>**Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to show value format.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlChart Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-fracionaldig"/>**FracionalDig(Int32)**

Define the Fracional Digits of value to show. Default is 0.

```csharp
IControlChart FracionalDig(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Fracional Digits

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-hidepercent"/>**HidePercent()**

Show Percent in ChartBar bar

```csharp
IControlChart HidePercent()
```

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-hidevalue"/>**HideValue()**

Hide value in ChartBar bar

```csharp
IControlChart HideValue()
```

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-interaction"/>**Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlChart, T1&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlChart Interaction<T1>(IEnumerable<T1> values, Action<IControlChart, T1> action)
```

#### Type Parameters

`T1`<br>
Type external colletion

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlChart, T1&gt;<br>
Action to execute

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-orderby"/>**OrderBy(ChartOrder)**

Sort Item by Highest Value

```csharp
IControlChart OrderBy(ChartOrder chartOrder)
```

#### Parameters

`chartOrder` [ChartOrder](./pplus.controls.chartorder.md)<br>
The sort value chart items

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-pagesize"/>**PageSize(Int32)**

Set max.item view per page.Default value for this control is 10.

```csharp
IControlChart PageSize(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of Max.items

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-run"/>**Run(Nullable&lt;ChartBarType&gt;, BannerDashOptions, Nullable&lt;Color&gt;)**

Execute this control and show ChartBar.

```csharp
void Run(Nullable<ChartBarType> barType, BannerDashOptions dashOptions, Nullable<Color> colorDash)
```

#### Parameters

`barType` [Nullable&lt;ChartBarType&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The type chart Bar. [ChartBarType](./pplus.controls.chartbartype.md)

`dashOptions` [BannerDashOptions](./pplus.controls.bannerdashoptions.md)<br>
The type of [BannerDashOptions](./pplus.controls.bannerdashoptions.md)

`colorDash` [Nullable&lt;Color&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Color](./pplus.color.md) Dash

### <a id="methods-showlegends"/>**ShowLegends(Boolean, Boolean)**

Show Legends after ChartBar

```csharp
IControlChart ShowLegends(bool withvalue, bool withPercent)
```

#### Parameters

`withvalue` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Show value in legend

`withPercent` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Show Percent in legend

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-styles"/>**Styles(StyleChart, Style)**

Styles for content chart/&gt;

```csharp
IControlChart Styles(StyleChart styletype, Style value)
```

#### Parameters

`styletype` [StyleChart](./pplus.controls.stylechart.md)<br>
[StyleChart](./pplus.controls.stylechart.md) of content chart

`value` [Style](./pplus.style.md)<br>
The [Style](./pplus.style.md)

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-title"/>**Title(String, TitleAligment, Nullable&lt;Style&gt;)**

Define Tille to Widgets.

```csharp
IControlChart Title(string value, TitleAligment titlealigment, Nullable<Style> style)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Title

`titlealigment` [TitleAligment](./pplus.controls.titlealigment.md)<br>
Title Aligment.If the title is greater than the size of the chart Align will be left.

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
[Style](./pplus.style.md) of Title

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)

### <a id="methods-width"/>**Width(Int32)**

Define Width to Widgets. Default value is 80.

```csharp
IControlChart Width(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Width

#### Returns

[IControlChart](./pplus.controls.icontrolchart.md)


- - -
[**Back to List Api**](./apis.md)
