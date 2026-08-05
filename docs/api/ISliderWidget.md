<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ISliderWidget Interface

Provides a fluent API for configuring and displaying a read\-only slider widget that draws a
numeric value as a horizontal bar, without waiting for user interaction\.

```csharp
public interface ISliderWidget
```

### Remarks
A widget is meant for display only: unlike [ISliderControl](ISliderControl.md 'PromptPlusLibrary\.ISliderControl'), it does not read input from the user\.
Every configuration method returns the same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so the calls can be
chained together \(fluent style\)\. Call [Show\(\)](ISliderWidget.md#PromptPlusLibrary.ISliderWidget.Show() 'PromptPlusLibrary\.ISliderWidget\.Show\(\)') last to render the bar on the console\.
### Methods

<a name='PromptPlusLibrary.ISliderWidget.BarType(PromptPlusLibrary.SliderBarType)'></a>

## ISliderWidget\.BarType\(SliderBarType\) Method

Selects the character set used to draw the slider bar\. Default is [Fill](SliderBarType.md#PromptPlusLibrary.SliderBarType.Fill 'PromptPlusLibrary\.SliderBarType\.Fill')\.

```csharp
PromptPlusLibrary.ISliderWidget BarType(PromptPlusLibrary.SliderBarType type);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.BarType(PromptPlusLibrary.SliderBarType).type'></a>

`type` [SliderBarType](SliderBarType.md 'PromptPlusLibrary\.SliderBarType')

The visual style of the bar, one of the [SliderBarType](SliderBarType.md 'PromptPlusLibrary\.SliderBarType') values\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

<a name='PromptPlusLibrary.ISliderWidget.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_)'></a>

## ISliderWidget\.ChangeColor\(Func\<double,Style\>\) Method

Changes the color of the bar dynamically according to the current value \(for example green when high, red when low\)\.

```csharp
PromptPlusLibrary.ISliderWidget ChangeColor(System.Func<double,ConsolePlusLibrary.Style> value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the current value and returns the [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ISliderWidget.md#PromptPlusLibrary.ISliderWidget.ChangeColor(System.Func_double,ConsolePlusLibrary.Style_).value 'PromptPlusLibrary\.ISliderWidget\.ChangeColor\(System\.Func\<double,ConsolePlusLibrary\.Style\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ISliderWidget.ChangeGradient(ConsolePlusLibrary.Color[])'></a>

## ISliderWidget\.ChangeGradient\(Color\[\]\) Method

Paints the bar with a gradient that transitions across the supplied colors as the value grows\.

```csharp
PromptPlusLibrary.ISliderWidget ChangeGradient(params ConsolePlusLibrary.Color[] colors);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.ChangeGradient(ConsolePlusLibrary.Color[]).colors'></a>

`colors` [ConsolePlusLibrary\.Color](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.color 'ConsolePlusLibrary\.Color')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The ordered colors used to build the gradient\. Cannot be `null` or empty\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [colors](ISliderWidget.md#PromptPlusLibrary.ISliderWidget.ChangeGradient(ConsolePlusLibrary.Color[]).colors 'PromptPlusLibrary\.ISliderWidget\.ChangeGradient\(ConsolePlusLibrary\.Color\[\]\)\.colors') is `null` or empty\.

<a name='PromptPlusLibrary.ISliderWidget.Culture(string)'></a>

## ISliderWidget\.Culture\(string\) Method

Sets the culture used to format the numeric value from a culture name \(for example `"en-US"` or `"pt-BR"`\)\.
Defaults to the current PromptPlus culture\.

```csharp
PromptPlusLibrary.ISliderWidget Culture(string cultureName);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.Culture(string).cultureName'></a>

`cultureName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The culture name, for example `"en-US"`\. Cannot be `null` or empty\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [cultureName](ISliderWidget.md#PromptPlusLibrary.ISliderWidget.Culture(string).cultureName 'PromptPlusLibrary\.ISliderWidget\.Culture\(string\)\.cultureName') is `null` or empty\.

<a name='PromptPlusLibrary.ISliderWidget.Culture(System.Globalization.CultureInfo)'></a>

## ISliderWidget\.Culture\(CultureInfo\) Method

Sets the culture used to format the numeric value \(decimal separator, digit grouping, and so on\)\.
Defaults to the current PromptPlus culture\.

```csharp
PromptPlusLibrary.ISliderWidget Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null`\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](ISliderWidget.md#PromptPlusLibrary.ISliderWidget.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.ISliderWidget\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.ISliderWidget.HideElements(PromptPlusLibrary.HideSlider)'></a>

## ISliderWidget\.HideElements\(HideSlider\) Method

Hides one or more visual elements of the slider \(such as delimiters or the range display\)\. By default every element is shown\.

```csharp
PromptPlusLibrary.ISliderWidget HideElements(PromptPlusLibrary.HideSlider value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.HideElements(PromptPlusLibrary.HideSlider).value'></a>

`value` [HideSlider](HideSlider.md 'PromptPlusLibrary\.HideSlider')

The elements to hide\. Combine [HideSlider](HideSlider.md 'PromptPlusLibrary\.HideSlider') values with a bitwise OR\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

<a name='PromptPlusLibrary.ISliderWidget.Show()'></a>

## ISliderWidget\.Show\(\) Method

Renders the slider bar on the console using the current configuration\. Call this method last\.

```csharp
void Show();
```

<a name='PromptPlusLibrary.ISliderWidget.Styles(PromptPlusLibrary.SliderStyles,ConsolePlusLibrary.Style)'></a>

## ISliderWidget\.Styles\(SliderStyles, Style\) Method

Overrides the colors of a specific region of the slider, such as the answer text or the bar itself\.

```csharp
PromptPlusLibrary.ISliderWidget Styles(PromptPlusLibrary.SliderStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.Styles(PromptPlusLibrary.SliderStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [SliderStyles](SliderStyles.md 'PromptPlusLibrary\.SliderStyles')

The region to restyle, one of the [SliderStyles](SliderStyles.md 'PromptPlusLibrary\.SliderStyles') values\.

<a name='PromptPlusLibrary.ISliderWidget.Styles(PromptPlusLibrary.SliderStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') \(colors\) to apply to that region\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

<a name='PromptPlusLibrary.ISliderWidget.Width(byte)'></a>

## ISliderWidget\.Width\(byte\) Method

Sets the width of the slider bar, measured in console characters\. Default is `40` and the value must be at least `10`\.

```csharp
PromptPlusLibrary.ISliderWidget Width(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.ISliderWidget.Width(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The width of the bar, in characters\.

#### Returns
[ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget')  
The same [ISliderWidget](ISliderWidget.md 'PromptPlusLibrary\.ISliderWidget') instance, so additional settings can be chained\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [value](ISliderWidget.md#PromptPlusLibrary.ISliderWidget.Width(byte).value 'PromptPlusLibrary\.ISliderWidget\.Width\(byte\)\.value') is less than `10`\.