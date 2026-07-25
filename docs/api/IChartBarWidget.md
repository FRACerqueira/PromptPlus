<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IChartBarWidget Interface

Provides a fluent API for configuring and displaying a read\-only chart bar widget that visualizes
data as horizontal bars, without waiting for user interaction\.

```csharp
public interface IChartBarWidget
```

### Remarks
A widget is meant for display only: unlike [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl'), it does not read input from the user\.
Every configuration method returns the same [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance, so the calls can be
chained together \(fluent style\)\. Call [Show\(\)](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Show() 'PromptPlusLibrary\.IChartBarWidget\.Show\(\)') last to render the chart on the console\.
### Methods

<a name='PromptPlusLibrary.IChartBarWidget.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string)'></a>

## IChartBarWidget\.AddItem\(string, double, Nullable\<Color\>, string\) Method

Adds a data item to be displayed in the chart bar visualization\.

```csharp
PromptPlusLibrary.IChartBarWidget AddItem(string label, double value, System.Nullable<ConsolePlusLibrary.Color> colorBar=null, string? id=null);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).label'></a>

`label` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The display label for the chart item\. Cannot be null or empty\.

<a name='PromptPlusLibrary.IChartBarWidget.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The numeric value associated with the item\.

<a name='PromptPlusLibrary.IChartBarWidget.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).colorBar'></a>

`colorBar` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional color for the bar\. If not specified, colors are automatically assigned in a rotating sequence\.

<a name='PromptPlusLibrary.IChartBarWidget.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).id'></a>

`id` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional unique identifier for the item\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when label is null or empty\.

### Remarks
Colors are automatically assigned in descending sequence from 15 to 0 and then back to 15 if not explicitly specified\.

<a name='PromptPlusLibrary.IChartBarWidget.BarType(PromptPlusLibrary.ChartBarType)'></a>

## IChartBarWidget\.BarType\(ChartBarType\) Method

Defines the type of bar to use in the chart\.
Default value is [Fill](ChartBarType.md#PromptPlusLibrary.ChartBarType.Fill 'PromptPlusLibrary\.ChartBarType\.Fill')\.

```csharp
PromptPlusLibrary.IChartBarWidget BarType(PromptPlusLibrary.ChartBarType type=PromptPlusLibrary.ChartBarType.Fill);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.BarType(PromptPlusLibrary.ChartBarType).type'></a>

`type` [ChartBarType](ChartBarType.md 'PromptPlusLibrary\.ChartBarType')

The [ChartBarType](ChartBarType.md 'PromptPlusLibrary\.ChartBarType') to set\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

<a name='PromptPlusLibrary.IChartBarWidget.Culture(string)'></a>

## IChartBarWidget\.Culture\(string\) Method

Sets the [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use for displaying values by name\. Default value is current PromptPlus culture\.

```csharp
PromptPlusLibrary.IChartBarWidget Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [cultureName](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Culture(string).cultureName 'PromptPlusLibrary\.IChartBarWidget\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.IChartBarWidget.Culture(System.Globalization.CultureInfo)'></a>

## IChartBarWidget\.Culture\(CultureInfo\) Method

Sets the [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use for displaying values\. Default value is current PromptPlus culture\.

```csharp
PromptPlusLibrary.IChartBarWidget Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [culture](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.IChartBarWidget\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.IChartBarWidget.FractionalDigits(byte)'></a>

## IChartBarWidget\.FractionalDigits\(byte\) Method

Defines the fractional digits of values to display\. Default is 2\.

```csharp
PromptPlusLibrary.IChartBarWidget FractionalDigits(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.FractionalDigits(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of fractional digits\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown if [value](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.FractionalDigits(byte).value 'PromptPlusLibrary\.IChartBarWidget\.FractionalDigits\(byte\)\.value') is greater than 5\.

<a name='PromptPlusLibrary.IChartBarWidget.HideElements(PromptPlusLibrary.HideChart)'></a>

## IChartBarWidget\.HideElements\(HideChart\) Method

Hides specific elements of the chart bar\. Default is to show all elements\.

```csharp
PromptPlusLibrary.IChartBarWidget HideElements(PromptPlusLibrary.HideChart value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.HideElements(PromptPlusLibrary.HideChart).value'></a>

`value` [HideChart](HideChart.md 'PromptPlusLibrary\.HideChart')

The elements to hide\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance for chaining\.

### Remarks
By default, all chart elements are visible\. Use this method to selectively hide specific components
of the visualization for a cleaner or more focused display\.

<a name='PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_)'></a>

## IChartBarWidget\.Interaction\<T\>\(IEnumerable\<T\>, Action\<T,IChartBarWidget\>\) Method

Iterates [items](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_).items 'PromptPlusLibrary\.IChartBarWidget\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarWidget\>\)\.items') and invokes [interactionaction](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_).interactionaction 'PromptPlusLibrary\.IChartBarWidget\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarWidget\>\)\.interactionaction') for each
element, giving the caller a chance to call [AddItem\(string, double, Nullable&lt;Color&gt;, string\)](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string) 'PromptPlusLibrary\.IChartBarWidget\.AddItem\(string, double, System\.Nullable\<ConsolePlusLibrary\.Color\>, string\)') programmatically\.

```csharp
PromptPlusLibrary.IChartBarWidget Interaction<T>(System.Collections.Generic.IEnumerable<T> items, System.Action<T,PromptPlusLibrary.IChartBarWidget> interactionaction);
```
#### Type parameters

<a name='PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_).T'></a>

`T`

The type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_).T 'PromptPlusLibrary\.IChartBarWidget\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarWidget\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be `null`\.

<a name='PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_).interactionaction'></a>

`interactionaction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarWidget_).T 'PromptPlusLibrary\.IChartBarWidget\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarWidget\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

The action invoked for each element\. Cannot be `null`\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance for chaining\.

<a name='PromptPlusLibrary.IChartBarWidget.Layout(PromptPlusLibrary.ChartBarLayout)'></a>

## IChartBarWidget\.Layout\(ChartBarLayout\) Method

Sets the layout of the chart bar\.
Default value is [Standard](ChartBarLayout.md#PromptPlusLibrary.ChartBarLayout.Standard 'PromptPlusLibrary\.ChartBarLayout\.Standard')\.

```csharp
PromptPlusLibrary.IChartBarWidget Layout(PromptPlusLibrary.ChartBarLayout layout=PromptPlusLibrary.ChartBarLayout.Standard);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.Layout(PromptPlusLibrary.ChartBarLayout).layout'></a>

`layout` [ChartBarLayout](ChartBarLayout.md 'PromptPlusLibrary\.ChartBarLayout')

The [ChartBarLayout](ChartBarLayout.md 'PromptPlusLibrary\.ChartBarLayout') to set\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

<a name='PromptPlusLibrary.IChartBarWidget.MaxLengthLabel(byte)'></a>

## IChartBarWidget\.MaxLengthLabel\(byte\) Method

Sets the maximum length for the label displayed on the chart bar widget\.
Default is 0 \(no truncation \- labels are shown in full\)\.

```csharp
PromptPlusLibrary.IChartBarWidget MaxLengthLabel(byte value=0);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.MaxLengthLabel(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of characters allowed for the label\. Use 0 to disable truncation and show full labels\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

<a name='PromptPlusLibrary.IChartBarWidget.OrderBy(PromptPlusLibrary.ChartBarOrder)'></a>

## IChartBarWidget\.OrderBy\(ChartBarOrder\) Method

Defines the display order of chart items based on specified criteria\.

```csharp
PromptPlusLibrary.IChartBarWidget OrderBy(PromptPlusLibrary.ChartBarOrder order);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.OrderBy(PromptPlusLibrary.ChartBarOrder).order'></a>

`order` [ChartBarOrder](ChartBarOrder.md 'PromptPlusLibrary\.ChartBarOrder')

The [ChartBarOrder](ChartBarOrder.md 'PromptPlusLibrary\.ChartBarOrder') criteria for sorting items\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

<a name='PromptPlusLibrary.IChartBarWidget.Show()'></a>

## IChartBarWidget\.Show\(\) Method

Renders the chart bar on the console using the current configuration\. Call this method last\.

```csharp
void Show();
```

<a name='PromptPlusLibrary.IChartBarWidget.ShowLegends(bool)'></a>

## IChartBarWidget\.ShowLegends\(bool\) Method

Shows legends after the chart bar\. Default is false\.

```csharp
PromptPlusLibrary.IChartBarWidget ShowLegends(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.ShowLegends(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether to show legends with value and percentage\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

<a name='PromptPlusLibrary.IChartBarWidget.Styles(PromptPlusLibrary.ChartBarStyles,ConsolePlusLibrary.Style)'></a>

## IChartBarWidget\.Styles\(ChartBarStyles, Style\) Method

Overrides the visual style applied to a specific region of the chart bar widget\.

```csharp
PromptPlusLibrary.IChartBarWidget Styles(PromptPlusLibrary.ChartBarStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.Styles(PromptPlusLibrary.ChartBarStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [ChartBarStyles](ChartBarStyles.md 'PromptPlusLibrary\.ChartBarStyles')

The [ChartBarStyles](ChartBarStyles.md 'PromptPlusLibrary\.ChartBarStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IChartBarWidget.Styles(PromptPlusLibrary.ChartBarStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

<a name='PromptPlusLibrary.IChartBarWidget.Title(string,PromptPlusLibrary.TextAlignment)'></a>

## IChartBarWidget\.Title\(string, TextAlignment\) Method

Sets the title of the chart bar\.

```csharp
PromptPlusLibrary.IChartBarWidget Title(string title, PromptPlusLibrary.TextAlignment alignment=PromptPlusLibrary.TextAlignment.Center);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.Title(string,PromptPlusLibrary.TextAlignment).title'></a>

`title` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to display as the chart title\.

<a name='PromptPlusLibrary.IChartBarWidget.Title(string,PromptPlusLibrary.TextAlignment).alignment'></a>

`alignment` [TextAlignment](TextAlignment.md 'PromptPlusLibrary\.TextAlignment')

The [TextAlignment](TextAlignment.md 'PromptPlusLibrary\.TextAlignment') for positioning the title text\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [title](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Title(string,PromptPlusLibrary.TextAlignment).title 'PromptPlusLibrary\.IChartBarWidget\.Title\(string, PromptPlusLibrary\.TextAlignment\)\.title') is `null` or empty\.

<a name='PromptPlusLibrary.IChartBarWidget.Width(byte)'></a>

## IChartBarWidget\.Width\(byte\) Method

Sets the width of the chart bar\.
Default value is 50\. The value must be greater than or equal to 10\.

```csharp
PromptPlusLibrary.IChartBarWidget Width(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarWidget.Width(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The width to set\.

#### Returns
[IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget')  
The current [IChartBarWidget](IChartBarWidget.md 'PromptPlusLibrary\.IChartBarWidget') instance\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown if [value](IChartBarWidget.md#PromptPlusLibrary.IChartBarWidget.Width(byte).value 'PromptPlusLibrary\.IChartBarWidget\.Width\(byte\)\.value') is less than 10\.