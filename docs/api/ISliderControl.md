<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ISliderControl Interface

Provides a fluent API for configuring and displaying a slider control that lets the user
pick a numeric value by moving a bar between a minimum and a maximum limit\.

```csharp
public interface ISliderControl
```

### Remarks
Every configuration method returns the same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so the calls can be
chained together \(fluent style\)\. Call [Run\(CancellationToken\)](ISliderControl.md#PromptPlusLibrary.ISliderControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ISliderControl\.Run\(System\.Threading\.CancellationToken\)') last to display the control
and read the value chosen by the user\.
### Methods

<a name='PromptPlusLibrary.ISliderControl.BarType(PromptPlusLibrary.SliderBarType)'></a>

## ISliderControl\.BarType\(SliderBarType\) Method

Selects the character set used to draw the slider bar\. Default is [Fill](SliderBarType.md#PromptPlusLibrary.SliderBarType.Fill 'PromptPlusLibrary\.SliderBarType\.Fill')\.

```csharp
PromptPlusLibrary.ISliderControl BarType(PromptPlusLibrary.SliderBarType type);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.BarType(PromptPlusLibrary.SliderBarType).type'></a>

`type` [SliderBarType](SliderBarType.md 'PromptPlusLibrary\.SliderBarType')

The visual style of the bar, one of the [SliderBarType](SliderBarType.md 'PromptPlusLibrary\.SliderBarType') values\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

<a name='PromptPlusLibrary.ISliderControl.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_)'></a>

## ISliderControl\.ChangeColor\(Func\<double,Style\>\) Method

Changes the color of the bar dynamically according to the current value \(for example green when high, red when low\)\.

```csharp
PromptPlusLibrary.ISliderControl ChangeColor(System.Func<double,ConsolePlusLibrary.Style> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current value and returns the [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ISliderControl.md#PromptPlusLibrary.ISliderControl.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_).value 'PromptPlusLibrary\.ISliderControl\.ChangeColor\(System\.Func\<double,ConsolePlusLibrary\.Style\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISliderControl.ChangeDescription(System.Func_double,string_)'></a>

## ISliderControl\.ChangeDescription\(Func\<double,string\>\) Method

Updates the description text shown with the slider according to the current value\.

```csharp
PromptPlusLibrary.ISliderControl ChangeDescription(System.Func<double,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.ChangeDescription(System.Func_double,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current value and returns the description to display\. Cannot be `null`\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ISliderControl.md#PromptPlusLibrary.ISliderControl.ChangeDescription(System.Func_double,string_).value 'PromptPlusLibrary\.ISliderControl\.ChangeDescription\(System\.Func\<double,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISliderControl.ChangeDescriptionAsync(System.Func_double,System.Threading.Tasks.Task_string__)'></a>

## ISliderControl\.ChangeDescriptionAsync\(Func\<double,Task\<string\>\>\) Method

Asynchronous version of [ChangeDescription\(Func&lt;double,string&gt;\)](ISliderControl.md#PromptPlusLibrary.ISliderControl.ChangeDescription(System.Func_double,string_) 'PromptPlusLibrary\.ISliderControl\.ChangeDescription\(System\.Func\<double,string\>\)') that updates the description
text according to the current value \(useful when the text comes from an asynchronous source\)\.

```csharp
PromptPlusLibrary.ISliderControl ChangeDescriptionAsync(System.Func<double,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.ChangeDescriptionAsync(System.Func_double,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current value and asynchronously returns the description\. Cannot be `null`\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ISliderControl.md#PromptPlusLibrary.ISliderControl.ChangeDescriptionAsync(System.Func_double,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ISliderControl\.ChangeDescriptionAsync\(System\.Func\<double,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISliderControl.ChangeGradient(ConsolePlusLibrary.Color[])'></a>

## ISliderControl\.ChangeGradient\(Color\[\]\) Method

Paints the bar with a gradient that transitions across the supplied colors as the value grows\.

```csharp
PromptPlusLibrary.ISliderControl ChangeGradient(params ConsolePlusLibrary.Color[] colors);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.ChangeGradient(ConsolePlusLibrary.Color[]).colors'></a>

`colors` [ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The ordered colors used to build the gradient\. Cannot be `null` or empty\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [colors](ISliderControl.md#PromptPlusLibrary.ISliderControl.ChangeGradient(ConsolePlusLibrary.Color[]).colors 'PromptPlusLibrary\.ISliderControl\.ChangeGradient\(ConsolePlusLibrary\.Color\[\]\)\.colors') is `null` or empty\.

<a name='PromptPlusLibrary.ISliderControl.Culture(string)'></a>

## ISliderControl\.Culture\(string\) Method

Sets the culture used to format the numeric value from a culture name \(for example `"en-US"` or `"pt-BR"`\)\.
Defaults to the current PromptPlus culture\.

```csharp
PromptPlusLibrary.ISliderControl Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The culture name, for example `"en-US"`\. Cannot be `null` or empty\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [cultureName](ISliderControl.md#PromptPlusLibrary.ISliderControl.Culture(string).cultureName 'PromptPlusLibrary\.ISliderControl\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.ISliderControl.Culture(System.Globalization.CultureInfo)'></a>

## ISliderControl\.Culture\(CultureInfo\) Method

Sets the culture used to format the numeric value \(decimal separator, digit grouping, and so on\)\.
Defaults to the current PromptPlus culture\.

```csharp
PromptPlusLibrary.ISliderControl Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null`\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](ISliderControl.md#PromptPlusLibrary.ISliderControl.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.ISliderControl\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.ISliderControl.Default(double,bool)'></a>

## ISliderControl\.Default\(double, bool\) Method

Sets the value that is pre\-selected when the slider is first shown\. Default is `0`\.

```csharp
PromptPlusLibrary.ISliderControl Default(double value, bool useDefaultHistory=true);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Default(double,bool).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The initial value\. It must be inside the range defined by [Range\(double, double\)](ISliderControl.md#PromptPlusLibrary.ISliderControl.Range(double,double) 'PromptPlusLibrary\.ISliderControl\.Range\(double, double\)')\.

<a name='PromptPlusLibrary.ISliderControl.Default(double,bool).useDefaultHistory'></a>

`useDefaultHistory` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When `true` and history is enabled via [EnabledHistory\(string, Action&lt;IHistoryOptions&gt;\)](ISliderControl.md#PromptPlusLibrary.ISliderControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_) 'PromptPlusLibrary\.ISliderControl\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)'), the last saved value is used instead of [value](ISliderControl.md#PromptPlusLibrary.ISliderControl.Default(double,bool).value 'PromptPlusLibrary\.ISliderControl\.Default\(double, bool\)\.value')\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](ISliderControl.md#PromptPlusLibrary.ISliderControl.Default(double,bool).value 'PromptPlusLibrary\.ISliderControl\.Default\(double, bool\)\.value') is outside the minimum/maximum range\.

<a name='PromptPlusLibrary.ISliderControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_)'></a>

## ISliderControl\.EnabledHistory\(string, Action\<IHistoryOptions\>\) Method

Enables value history, persisting the chosen value to a file so it can be reused as the default on the next run\.

```csharp
PromptPlusLibrary.ISliderControl EnabledHistory(string filename, System.Action<PromptPlusLibrary.IHistoryOptions>? options=null);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename'></a>

`filename` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file name used to store the history\. Cannot be `null`\.

<a name='PromptPlusLibrary.ISliderControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

An optional callback to configure the [IHistoryOptions](IHistoryOptions.md 'PromptPlusLibrary\.IHistoryOptions') \(such as expiration\)\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [filename](ISliderControl.md#PromptPlusLibrary.ISliderControl.EnabledHistory(string,System.Action_PromptPlusLibrary.IHistoryOptions_).filename 'PromptPlusLibrary\.ISliderControl\.EnabledHistory\(string, System\.Action\<PromptPlusLibrary\.IHistoryOptions\>\)\.filename') is `null`\.

<a name='PromptPlusLibrary.ISliderControl.FractionalDigits(byte)'></a>

## ISliderControl\.FractionalDigits\(byte\) Method

Sets how many decimal places are shown for the slider value\. Default is `0` \(whole numbers\)\.

```csharp
PromptPlusLibrary.ISliderControl FractionalDigits(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.FractionalDigits(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of fractional digits, from `0` to `5`\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](ISliderControl.md#PromptPlusLibrary.ISliderControl.FractionalDigits(byte).value 'PromptPlusLibrary\.ISliderControl\.FractionalDigits\(byte\)\.value') is greater than `5`\.

<a name='PromptPlusLibrary.ISliderControl.HideElements(PromptPlusLibrary.HideSlider)'></a>

## ISliderControl\.HideElements\(HideSlider\) Method

Hides one or more visual elements of the slider \(such as delimiters or the range display\)\. By default every element is shown\.

```csharp
PromptPlusLibrary.ISliderControl HideElements(PromptPlusLibrary.HideSlider value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.HideElements(PromptPlusLibrary.HideSlider).value'></a>

`value` [HideSlider](HideSlider.md 'PromptPlusLibrary\.HideSlider')

The elements to hide\. Combine [HideSlider](HideSlider.md 'PromptPlusLibrary\.HideSlider') values with a bitwise OR\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

<a name='PromptPlusLibrary.ISliderControl.LargeStep(double)'></a>

## ISliderControl\.LargeStep\(double\) Method

Sets the amount added or removed on each large change \(for example Page Up/Page Down\)\. Default is 1/10 of the range\.

```csharp
PromptPlusLibrary.ISliderControl LargeStep(double value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.LargeStep(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The increment applied on a large step\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

<a name='PromptPlusLibrary.ISliderControl.Layout(PromptPlusLibrary.SliderLayout)'></a>

## ISliderControl\.Layout\(SliderLayout\) Method

Chooses how the user changes the value and how the control is drawn\. Default is [LeftRight](SliderLayout.md#PromptPlusLibrary.SliderLayout.LeftRight 'PromptPlusLibrary\.SliderLayout\.LeftRight')\.

```csharp
PromptPlusLibrary.ISliderControl Layout(PromptPlusLibrary.SliderLayout value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Layout(PromptPlusLibrary.SliderLayout).value'></a>

`value` [SliderLayout](SliderLayout.md 'PromptPlusLibrary\.SliderLayout')

The layout to use, one of the [SliderLayout](SliderLayout.md 'PromptPlusLibrary\.SliderLayout') values\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

### Remarks
[LeftRight](SliderLayout.md#PromptPlusLibrary.SliderLayout.LeftRight 'PromptPlusLibrary\.SliderLayout\.LeftRight') uses the Left/Right arrows and shows the bar, while
            [UpDown](SliderLayout.md#PromptPlusLibrary.SliderLayout.UpDown 'PromptPlusLibrary\.SliderLayout\.UpDown') uses the Up/Down arrows, hides the bar and does not show widgets\.

<a name='PromptPlusLibrary.ISliderControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ISliderControl\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(such as prompt message, tooltips and validation\)\.

```csharp
PromptPlusLibrary.ISliderControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](ISliderControl.md#PromptPlusLibrary.ISliderControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ISliderControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ISliderControl.Range(double,double)'></a>

## ISliderControl\.Range\(double, double\) Method

Defines the lower and upper limits the slider can reach\. Defaults to `0` for [minvalue](ISliderControl.md#PromptPlusLibrary.ISliderControl.Range(double,double).minvalue 'PromptPlusLibrary\.ISliderControl\.Range\(double, double\)\.minvalue')
and `100` for [maxvalue](ISliderControl.md#PromptPlusLibrary.ISliderControl.Range(double,double).maxvalue 'PromptPlusLibrary\.ISliderControl\.Range\(double, double\)\.maxvalue')\.

```csharp
PromptPlusLibrary.ISliderControl Range(double minvalue, double maxvalue);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Range(double,double).minvalue'></a>

`minvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The smallest value the user can select\.

<a name='PromptPlusLibrary.ISliderControl.Range(double,double).maxvalue'></a>

`maxvalue` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The largest value the user can select\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [minvalue](ISliderControl.md#PromptPlusLibrary.ISliderControl.Range(double,double).minvalue 'PromptPlusLibrary\.ISliderControl\.Range\(double, double\)\.minvalue') is greater than or equal to [maxvalue](ISliderControl.md#PromptPlusLibrary.ISliderControl.Range(double,double).maxvalue 'PromptPlusLibrary\.ISliderControl\.Range\(double, double\)\.maxvalue')\.

<a name='PromptPlusLibrary.ISliderControl.Run(System.Threading.CancellationToken)'></a>

## ISliderControl\.Run\(CancellationToken\) Method

Displays the slider and blocks until the user confirms or cancels, returning the selected value\.

```csharp
PromptPlusLibrary.ResultPrompt<System.Nullable<double>> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the prompt while it is waiting for input\. Defaults to [System\.Threading\.CancellationToken\.None](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken.none 'System\.Threading\.CancellationToken\.None')\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') wrapping the chosen value as a [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double'), or a `null` value when the prompt is cancelled\.

<a name='PromptPlusLibrary.ISliderControl.Step(double)'></a>

## ISliderControl\.Step\(double\) Method

Sets the amount added or removed on each small change \(arrow keys\)\. Default is 1/100 of the range\.

```csharp
PromptPlusLibrary.ISliderControl Step(double value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Step(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The increment applied on a small step\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

<a name='PromptPlusLibrary.ISliderControl.Styles(PromptPlusLibrary.SliderStyles,ConsolePlusLibrary.Style)'></a>

## ISliderControl\.Styles\(SliderStyles, Style\) Method

Overrides the colors of a specific region of the slider, such as the prompt, the answer or the bar itself\.

```csharp
PromptPlusLibrary.ISliderControl Styles(PromptPlusLibrary.SliderStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Styles(PromptPlusLibrary.SliderStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [SliderStyles](SliderStyles.md 'PromptPlusLibrary\.SliderStyles')

The region to restyle, one of the [SliderStyles](SliderStyles.md 'PromptPlusLibrary\.SliderStyles') values\.

<a name='PromptPlusLibrary.ISliderControl.Styles(PromptPlusLibrary.SliderStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') \(colors\) to apply to that region\. Cannot be `null`\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [style](ISliderControl.md#PromptPlusLibrary.ISliderControl.Styles(PromptPlusLibrary.SliderStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.ISliderControl\.Styles\(PromptPlusLibrary\.SliderStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.

<a name='PromptPlusLibrary.ISliderControl.Width(byte)'></a>

## ISliderControl\.Width\(byte\) Method

Sets the width of the slider bar, measured in console characters\. Default is `30` and the value must be at bet `10` and `100`\.

```csharp
PromptPlusLibrary.ISliderControl Width(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderControl.Width(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The width of the bar, in characters\.

#### Returns
[ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl')  
The same [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](ISliderControl.md#PromptPlusLibrary.ISliderControl.Width(byte).value 'PromptPlusLibrary\.ISliderControl\.Width\(byte\)\.value') is less than `10` or greater than `100`\.