<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IChartBarControl Interface

Provides a fluent API for configuring and running an interactive horizontal chart bar control\.

```csharp
public interface IChartBarControl
```

### Remarks
The chart displays a set of labeled data items as horizontal bars with optional percentage
and legend sections\. The user can navigate items with the arrow keys, optionally switch
between [Standard](ChartBarLayout.md#PromptPlusLibrary.ChartBarLayout.Standard 'PromptPlusLibrary\.ChartBarLayout\.Standard') and [Stacked](ChartBarLayout.md#PromptPlusLibrary.ChartBarLayout.Stacked 'PromptPlusLibrary\.ChartBarLayout\.Stacked') layouts,
and cycle through sort orders\. Pressing Enter returns the currently highlighted
[ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem')\. Call [Run\(CancellationToken\)](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IChartBarControl\.Run\(System\.Threading\.CancellationToken\)') last\.
### Methods

<a name='PromptPlusLibrary.IChartBarControl.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string)'></a>

## IChartBarControl\.AddItem\(string, double, Nullable\<Color\>, string\) Method

Adds a data item to be displayed in the chart bar visualization\.

```csharp
PromptPlusLibrary.IChartBarControl AddItem(string label, double value, System.Nullable<ConsolePlusLibrary.Color> colorBar=null, string? id=null);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).label'></a>

`label` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The display label for the chart item\. Cannot be null or empty\.

<a name='PromptPlusLibrary.IChartBarControl.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The numeric value associated with the item\.

<a name='PromptPlusLibrary.IChartBarControl.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).colorBar'></a>

`colorBar` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional color for the bar\. If not specified, colors are automatically assigned in a rotating sequence\.

<a name='PromptPlusLibrary.IChartBarControl.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string).id'></a>

`id` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional unique identifier for the item\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when label is null or empty\.

### Remarks
Colors are automatically assigned in descending sequence from 15 to 0 and then back to 15 if not explicitly specified\.

<a name='PromptPlusLibrary.IChartBarControl.BarType(PromptPlusLibrary.ChartBarType)'></a>

## IChartBarControl\.BarType\(ChartBarType\) Method

Defines the type of bar to use in the chart\.
Default value is [Fill](ChartBarType.md#PromptPlusLibrary.ChartBarType.Fill 'PromptPlusLibrary\.ChartBarType\.Fill')\.

```csharp
PromptPlusLibrary.IChartBarControl BarType(PromptPlusLibrary.ChartBarType type=PromptPlusLibrary.ChartBarType.Fill);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.BarType(PromptPlusLibrary.ChartBarType).type'></a>

`type` [ChartBarType](ChartBarType.md 'PromptPlusLibrary\.ChartBarType')

The [ChartBarType](ChartBarType.md 'PromptPlusLibrary\.ChartBarType') to set\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

<a name='PromptPlusLibrary.IChartBarControl.ChangeDescription(System.Func_PromptPlusLibrary.ChartItem,string_)'></a>

## IChartBarControl\.ChangeDescription\(Func\<ChartItem,string\>\) Method

Configures dynamic description generation for chart items\.

```csharp
PromptPlusLibrary.IChartBarControl ChangeDescription(System.Func<PromptPlusLibrary.ChartItem,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.ChangeDescription(System.Func_PromptPlusLibrary.ChartItem,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that takes the current description and returns the updated description\. Cannot be `null`\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.ChangeDescription(System.Func_PromptPlusLibrary.ChartItem,string_).value 'PromptPlusLibrary\.IChartBarControl\.ChangeDescription\(System\.Func\<PromptPlusLibrary\.ChartItem,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IChartBarControl.ChangeDescriptionAsync(System.Func_PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task_string__)'></a>

## IChartBarControl\.ChangeDescriptionAsync\(Func\<ChartItem,Task\<string\>\>\) Method

Asynchronous counterpart of [ChangeDescription\(Func&lt;ChartItem,string&gt;\)](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.ChangeDescription(System.Func_PromptPlusLibrary.ChartItem,string_) 'PromptPlusLibrary\.IChartBarControl\.ChangeDescription\(System\.Func\<PromptPlusLibrary\.ChartItem,string\>\)')\. The task is
awaited synchronously \(blocking\) each frame\.

```csharp
PromptPlusLibrary.IChartBarControl ChangeDescriptionAsync(System.Func<PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.ChangeDescriptionAsync(System.Func_PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous function that takes a chart item and returns the updated description\. Cannot be `null`\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [value](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.ChangeDescriptionAsync(System.Func_PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.IChartBarControl\.ChangeDescriptionAsync\(System\.Func\<PromptPlusLibrary\.ChartItem,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.IChartBarControl.Culture(string)'></a>

## IChartBarControl\.Culture\(string\) Method

Sets the [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use for displaying values by name\. Default value is current PromptPlus culture\.

```csharp
PromptPlusLibrary.IChartBarControl Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [cultureName](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Culture(string).cultureName 'PromptPlusLibrary\.IChartBarControl\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.IChartBarControl.Culture(System.Globalization.CultureInfo)'></a>

## IChartBarControl\.Culture\(CultureInfo\) Method

Sets the [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use for displaying values\. Default value is current PromptPlus culture\.

```csharp
PromptPlusLibrary.IChartBarControl Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [culture](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.IChartBarControl\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.IChartBarControl.EnableLayoutSwitcher(bool)'></a>

## IChartBarControl\.EnableLayoutSwitcher\(bool\) Method

Enables or disables the layout switcher functionality that allows users to toggle between
[Standard](ChartBarLayout.md#PromptPlusLibrary.ChartBarLayout.Standard 'PromptPlusLibrary\.ChartBarLayout\.Standard') and [Stacked](ChartBarLayout.md#PromptPlusLibrary.ChartBarLayout.Stacked 'PromptPlusLibrary\.ChartBarLayout\.Stacked') layouts\.
Default is enabled \([true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\)\.

```csharp
PromptPlusLibrary.IChartBarControl EnableLayoutSwitcher(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.EnableLayoutSwitcher(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to enable layout switching; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to disable it\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

### Remarks
When enabled, users can press the configured hotkey to switch between layouts\.
When switching to stacked layout, the control will validate console width availability\.

<a name='PromptPlusLibrary.IChartBarControl.EnableOrderingSwitcher(bool)'></a>

## IChartBarControl\.EnableOrderingSwitcher\(bool\) Method

Enables or disables the ordering switcher functionality that allows users to change
the sort order of chart items \(None, Ascending, Descending\)\.
Default is enabled \([true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\)\.

```csharp
PromptPlusLibrary.IChartBarControl EnableOrderingSwitcher(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.EnableOrderingSwitcher(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to enable ordering switching; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to disable it\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

### Remarks
When enabled, users can press the configured hotkey to cycle through sort orders\.

<a name='PromptPlusLibrary.IChartBarControl.FractionalDigits(byte)'></a>

## IChartBarControl\.FractionalDigits\(byte\) Method

Defines the fractional digits of values to display\. Default is 2\.

```csharp
PromptPlusLibrary.IChartBarControl FractionalDigits(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.FractionalDigits(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of fractional digits\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown if [value](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.FractionalDigits(byte).value 'PromptPlusLibrary\.IChartBarControl\.FractionalDigits\(byte\)\.value') is greater than 5\.

<a name='PromptPlusLibrary.IChartBarControl.HideElements(PromptPlusLibrary.HideChart)'></a>

## IChartBarControl\.HideElements\(HideChart\) Method

Hides specific elements of the chart bar\. Default is to show all elements\.

```csharp
PromptPlusLibrary.IChartBarControl HideElements(PromptPlusLibrary.HideChart value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.HideElements(PromptPlusLibrary.HideChart).value'></a>

`value` [HideChart](HideChart.md 'PromptPlusLibrary\.HideChart')

The elements to hide\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

### Remarks
By default, all chart elements are visible\. Use this method to selectively hide specific components
of the visualization for a cleaner or more focused display\.

<a name='PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_)'></a>

## IChartBarControl\.Interaction\<T\>\(IEnumerable\<T\>, Action\<T,IChartBarControl\>\) Method

Iterates [items](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).items 'PromptPlusLibrary\.IChartBarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarControl\>\)\.items') and invokes [interactionaction](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).interactionaction 'PromptPlusLibrary\.IChartBarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarControl\>\)\.interactionaction') for each
element, giving the caller a chance to add chart items programmatically\.
Equivalent to calling [AddItem\(string, double, Nullable&lt;Color&gt;, string\)](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.AddItem(string,double,System.Nullable_ConsolePlusLibrary.Color_,string) 'PromptPlusLibrary\.IChartBarControl\.AddItem\(string, double, System\.Nullable\<ConsolePlusLibrary\.Color\>, string\)') inside the loop\.

```csharp
PromptPlusLibrary.IChartBarControl Interaction<T>(System.Collections.Generic.IEnumerable<T> items, System.Action<T,PromptPlusLibrary.IChartBarControl> interactionaction);
```
#### Type parameters

<a name='PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).T'></a>

`T`

The type of elements in the input sequence\.
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).T 'PromptPlusLibrary\.IChartBarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarControl\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The input sequence to iterate\. Cannot be `null`\.

<a name='PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).interactionaction'></a>

`interactionaction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).T 'PromptPlusLibrary\.IChartBarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarControl\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

The action invoked for each element, receiving the element and the current
            [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\. Cannot be `null`\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [items](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).items 'PromptPlusLibrary\.IChartBarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarControl\>\)\.items') or [interactionaction](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IChartBarControl_).interactionaction 'PromptPlusLibrary\.IChartBarControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IChartBarControl\>\)\.interactionaction') is `null`\.

<a name='PromptPlusLibrary.IChartBarControl.Layout(PromptPlusLibrary.ChartBarLayout)'></a>

## IChartBarControl\.Layout\(ChartBarLayout\) Method

Sets the layout of the chart bar\.
Default value is [Standard](ChartBarLayout.md#PromptPlusLibrary.ChartBarLayout.Standard 'PromptPlusLibrary\.ChartBarLayout\.Standard')\.

```csharp
PromptPlusLibrary.IChartBarControl Layout(PromptPlusLibrary.ChartBarLayout layout=PromptPlusLibrary.ChartBarLayout.Standard);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Layout(PromptPlusLibrary.ChartBarLayout).layout'></a>

`layout` [ChartBarLayout](ChartBarLayout.md 'PromptPlusLibrary\.ChartBarLayout')

The [ChartBarLayout](ChartBarLayout.md 'PromptPlusLibrary\.ChartBarLayout') to set\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

### Remarks
When attempting to switch to [Stacked](ChartBarLayout.md#PromptPlusLibrary.ChartBarLayout.Stacked 'PromptPlusLibrary\.ChartBarLayout\.Stacked') during runtime,
the control will validate if the console has sufficient width to render all items\.
The minimum required width is calculated as the maximum value between the chart width
and the number of items, plus a margin of 2 characters\. If the console width is insufficient,
the layout switch will be silently prevented to avoid rendering issues\.

<a name='PromptPlusLibrary.IChartBarControl.MaxLengthLabel(byte)'></a>

## IChartBarControl\.MaxLengthLabel\(byte\) Method

Sets the maximum length for the label displayed on the chart bar control\.
Default is 0 \(no truncation \- labels are shown in full\)\.

```csharp
PromptPlusLibrary.IChartBarControl MaxLengthLabel(byte value=0);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.MaxLengthLabel(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The maximum number of characters allowed for the label\. Use 0 to disable truncation and show full labels\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

<a name='PromptPlusLibrary.IChartBarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IChartBarControl\.Options\(Action\<IControlOptions\>\) Method

Applies custom options to the control\.

```csharp
PromptPlusLibrary.IChartBarControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An action to configure [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [options](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IChartBarControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IChartBarControl.OrderBy(PromptPlusLibrary.ChartBarOrder)'></a>

## IChartBarControl\.OrderBy\(ChartBarOrder\) Method

Defines the display order of chart items based on specified criteria\.

```csharp
PromptPlusLibrary.IChartBarControl OrderBy(PromptPlusLibrary.ChartBarOrder order);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.OrderBy(PromptPlusLibrary.ChartBarOrder).order'></a>

`order` [ChartBarOrder](ChartBarOrder.md 'PromptPlusLibrary\.ChartBarOrder')

The [ChartBarOrder](ChartBarOrder.md 'PromptPlusLibrary\.ChartBarOrder') criteria for sorting items\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

<a name='PromptPlusLibrary.IChartBarControl.PageSize(byte)'></a>

## IChartBarControl\.PageSize\(byte\) Method

Sets the maximum number of items to display per page in the chart visualization\.
Default value is 0 \(no pagination\)\.

```csharp
PromptPlusLibrary.IChartBarControl PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

Maximum number of items to show per page\. Use 0 to disable pagination\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

<a name='PromptPlusLibrary.IChartBarControl.PredicateSelected(System.Func_PromptPlusLibrary.ChartItem,bool_)'></a>

## IChartBarControl\.PredicateSelected\(Func\<ChartItem,bool\>\) Method

Sets a validation rule for determining which items can be selected\.

```csharp
PromptPlusLibrary.IChartBarControl PredicateSelected(System.Func<PromptPlusLibrary.ChartItem,bool> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.PredicateSelected(System.Func_PromptPlusLibrary.ChartItem,bool_).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that evaluates whether a chart item should be selectable\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [validselect](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.PredicateSelected(System.Func_PromptPlusLibrary.ChartItem,bool_).validselect 'PromptPlusLibrary\.IChartBarControl\.PredicateSelected\(System\.Func\<PromptPlusLibrary\.ChartItem,bool\>\)\.validselect') is `null`\.

<a name='PromptPlusLibrary.IChartBarControl.PredicateSelectedAsync(System.Func_PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task_bool__)'></a>

## IChartBarControl\.PredicateSelectedAsync\(Func\<ChartItem,Task\<bool\>\>\) Method

Sets an asynchronous validation rule for determining which items can be selected\.

```csharp
PromptPlusLibrary.IChartBarControl PredicateSelectedAsync(System.Func<PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task<bool>> validselect);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.PredicateSelectedAsync(System.Func_PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task_bool__).validselect'></a>

`validselect` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

An asynchronous function that evaluates whether a chart item should be selectable\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown if [validselect](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.PredicateSelectedAsync(System.Func_PromptPlusLibrary.ChartItem,System.Threading.Tasks.Task_bool__).validselect 'PromptPlusLibrary\.IChartBarControl\.PredicateSelectedAsync\(System\.Func\<PromptPlusLibrary\.ChartItem,System\.Threading\.Tasks\.Task\<bool\>\>\)\.validselect') is `null`\.

### Remarks
The asynchronous predicate is evaluated synchronously \(blocking\) on the UI thread; it does not run in parallel\.

<a name='PromptPlusLibrary.IChartBarControl.Run(System.Threading.CancellationToken)'></a>

## IChartBarControl\.Run\(CancellationToken\) Method

Displays the chart bar control and blocks until the user confirms or cancels,
returning the highlighted [ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem') at confirmation time\.

```csharp
PromptPlusLibrary.ResultPrompt<PromptPlusLibrary.ChartItem?> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the operation\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the selected [ChartItem](ChartItem.md 'PromptPlusLibrary\.ChartItem'), or an aborted result if cancelled\.

<a name='PromptPlusLibrary.IChartBarControl.ShowLegends(bool)'></a>

## IChartBarControl\.ShowLegends\(bool\) Method

Shows legends after the chart bar\. Default is false\.

```csharp
PromptPlusLibrary.IChartBarControl ShowLegends(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.ShowLegends(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether to show legends with value and percentage\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

<a name='PromptPlusLibrary.IChartBarControl.Styles(PromptPlusLibrary.ChartBarStyles,ConsolePlusLibrary.Style)'></a>

## IChartBarControl\.Styles\(ChartBarStyles, Style\) Method

Overwrites styles for the chart bar\.

```csharp
PromptPlusLibrary.IChartBarControl Styles(PromptPlusLibrary.ChartBarStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Styles(PromptPlusLibrary.ChartBarStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [ChartBarStyles](ChartBarStyles.md 'PromptPlusLibrary\.ChartBarStyles')

The [ChartBarStyles](ChartBarStyles.md 'PromptPlusLibrary\.ChartBarStyles') of the content\.

<a name='PromptPlusLibrary.IChartBarControl.Styles(PromptPlusLibrary.ChartBarStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

<a name='PromptPlusLibrary.IChartBarControl.Title(string,PromptPlusLibrary.TextAlignment)'></a>

## IChartBarControl\.Title\(string, TextAlignment\) Method

Sets the title of the chart bar\.

```csharp
PromptPlusLibrary.IChartBarControl Title(string title, PromptPlusLibrary.TextAlignment alignment=PromptPlusLibrary.TextAlignment.Center);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Title(string,PromptPlusLibrary.TextAlignment).title'></a>

`title` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to display as the chart title\.

<a name='PromptPlusLibrary.IChartBarControl.Title(string,PromptPlusLibrary.TextAlignment).alignment'></a>

`alignment` [TextAlignment](TextAlignment.md 'PromptPlusLibrary\.TextAlignment')

The [TextAlignment](TextAlignment.md 'PromptPlusLibrary\.TextAlignment') for positioning the title text\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown if [title](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Title(string,PromptPlusLibrary.TextAlignment).title 'PromptPlusLibrary\.IChartBarControl\.Title\(string, PromptPlusLibrary\.TextAlignment\)\.title') is `null` or empty\.

<a name='PromptPlusLibrary.IChartBarControl.Width(byte)'></a>

## IChartBarControl\.Width\(byte\) Method

Sets the width of the chart bar\.
Default value is 50\. The value must be greater than or equal to 10\.

```csharp
PromptPlusLibrary.IChartBarControl Width(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IChartBarControl.Width(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The width to set\.

#### Returns
[IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl')  
The current [IChartBarControl](IChartBarControl.md 'PromptPlusLibrary\.IChartBarControl') instance\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown if [value](IChartBarControl.md#PromptPlusLibrary.IChartBarControl.Width(byte).value 'PromptPlusLibrary\.IChartBarControl\.Width\(byte\)\.value') is less than 10\.