# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlChartBar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlChartBar

Namespace: PPlus.Controls

Represents the interface with all Methods of the ChartBar control

```csharp
public interface IControlChartBar : IPromptControls<Boolean>
```

Implements [IPromptControls&lt;Boolean&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-additem"/>**AddItem(String, Double, Nullable&lt;Color&gt;)**

Add item to ChartBar

```csharp
IControlChartBar AddItem(string label, double value, Nullable<Color> colorbar)
```

#### Parameters

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label Item to add

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
Value to Item

`colorbar` [Nullable&lt;Color&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [Color](./pplus.color.md) bar. 
 <br>If not informed, the color bar will be chosen in descending sequence from 15 to 0 and then back to 15.

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-bartype"/>**BarType(ChartBarType)**

Define type Bar to ChartBar.

```csharp
IControlChartBar BarType(ChartBarType value)
```

#### Parameters

`value` [ChartBarType](./pplus.controls.chartbartype.md)<br>
The [ChartBarType](./pplus.controls.chartbartype.md). Default value 'ChartType.Fill'

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-chartpadleft"/>**ChartPadLeft(Byte)**

Pad-Left to write ChartBar

```csharp
IControlChartBar ChartPadLeft(byte value)
```

#### Parameters

`value` [Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>
Number of spaces. Default value is 0.

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-culture"/>**Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to on show value format.

```csharp
IControlChartBar Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-culture"/>**Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to show value format.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlChartBar Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-enabledinteractionuser"/>**EnabledInteractionUser(Boolean, Boolean, Boolean, Nullable&lt;Int32&gt;)**

Enabled Interaction to switch Type , Legend and order when browse the charts / Legends.

```csharp
IControlChartBar EnabledInteractionUser(bool switchType, bool switchLegend, bool switchorder, Nullable<Int32> pagesize)
```

#### Parameters

`switchType` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Enabled switch Type

`switchLegend` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Enabled switch legend

`switchorder` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Enabled switch Ordination

`pagesize` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Set max.item view per page.Default value for this control is 10.

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-fracionaldig"/>**FracionalDig(Int32)**

Define the Fracional Digits of value to show. Default is 0.

```csharp
IControlChartBar FracionalDig(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Fracional Digits

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-hideordination"/>**HideOrdination()**

Hide info of ordination labels

```csharp
IControlChartBar HideOrdination()
```

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-hidepercent"/>**HidePercent()**

Hide Percent in bar

```csharp
IControlChartBar HidePercent()
```

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-hidevalue"/>**HideValue()**

Hide value in bar

```csharp
IControlChartBar HideValue()
```

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-hotkeyswitchlegend"/>**HotKeySwitchLegend(HotKey)**

Overwrite a HotKey to Switch Legend Chart. Default value is 'F3'

```csharp
IControlChartBar HotKeySwitchLegend(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to Switch Legend Chart

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-hotkeyswitchorder"/>**HotKeySwitchOrder(HotKey)**

Overwrite a HotKey to Switch ordination bar and label. Default value is 'F4'

```csharp
IControlChartBar HotKeySwitchOrder(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to Switch ordination bar and label

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-hotkeyswitchtype"/>**HotKeySwitchType(HotKey)**

Overwrite a HotKey to Switch Type Chart. Default value is 'F2'

```csharp
IControlChartBar HotKeySwitchType(HotKey value)
```

#### Parameters

`value` [HotKey](./pplus.controls.hotkey.md)<br>
The [HotKey](./pplus.controls.hotkey.md) to Switch Type Chart

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-interaction"/>**Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlChartBar, T1&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlChartBar Interaction<T1>(IEnumerable<T1> values, Action<IControlChartBar, T1> action)
```

#### Type Parameters

`T1`<br>
Type external colletion

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlChartBar, T1&gt;<br>
Action to execute

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-orderby"/>**OrderBy(ChartOrder)**

Sort bars and labels

```csharp
IControlChartBar OrderBy(ChartOrder chartOrder)
```

#### Parameters

`chartOrder` [ChartOrder](./pplus.controls.chartorder.md)<br>
The sort type

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-showlegends"/>**ShowLegends(Boolean, Boolean)**

Show Legends after ChartBar

```csharp
IControlChartBar ShowLegends(bool withvalue, bool withPercent)
```

#### Parameters

`withvalue` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Show value in legend

`withPercent` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Show Percent in legend

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-styles"/>**Styles(StyleChart, Style)**

Styles for ChartBar content

```csharp
IControlChartBar Styles(StyleChart styletype, Style value)
```

#### Parameters

`styletype` [StyleChart](./pplus.controls.stylechart.md)<br>
[StyleChart](./pplus.controls.stylechart.md) of content

`value` [Style](./pplus.style.md)<br>
The [Style](./pplus.style.md)

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-titlealignment"/>**TitleAlignment(Alignment)**

Define Tille Alignment.

```csharp
IControlChartBar TitleAlignment(Alignment value)
```

#### Parameters

`value` [Alignment](./pplus.controls.alignment.md)<br>
The [Alignment](./pplus.controls.alignment.md) title

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-type"/>**Type(ChartType)**

Define type to ChartBar.

```csharp
IControlChartBar Type(ChartType value)
```

#### Parameters

`value` [ChartType](./pplus.controls.charttype.md)<br>
The [ChartType](./pplus.controls.charttype.md). Default value 'ChartType.StandBar'

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)

### <a id="methods-width"/>**Width(Int32)**

Define Width to ChartBar. Default value is 80.

```csharp
IControlChartBar Width(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Width

#### Returns

[IControlChartBar](./pplus.controls.icontrolchartbar.md)


- - -
[**Back to List Api**](./apis.md)
