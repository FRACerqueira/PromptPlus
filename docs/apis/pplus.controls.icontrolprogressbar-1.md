# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus IControlProgressBar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlProgressBar&lt;T&gt;

Namespace: PPlus.Controls

```csharp
public interface IControlProgressBar<T> : IPromptControls`1
```

#### Type Parameters

`T`<br>

Implements IPromptControls&lt;ResultProgessBar&lt;T&gt;&gt;

## Methods

### **HideElements(HideProgressBar)**

Hide elements progress bar Widgets. Default is Show all elements
 <br>

```csharp
IControlProgressBar<T> HideElements(HideProgressBar value)
```

#### Parameters

`value` [HideProgressBar](./pplus.controls.hideprogressbar.md)<br>
element to hide. [HideProgressBar](./pplus.controls.hideprogressbar.md)

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlProgressBar<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to on show value format.

```csharp
IControlProgressBar<T> Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **CharBar(Char)**

Set [Char](https://docs.microsoft.com/en-us/dotnet/api/system.char) to show progress.Default value '#'

<br>

```csharp
IControlProgressBar<T> CharBar(char value)
```

#### Parameters

`value` [Char](https://docs.microsoft.com/en-us/dotnet/api/system.char)<br>
Char to show

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to show value format.

<br>

```csharp
IControlProgressBar<T> Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **Finish(String)**

Finish answer to show when progressbar is completed.

```csharp
IControlProgressBar<T> Finish(string text)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text Finish answer

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **Spinner(SpinnersType, Nullable&lt;Style&gt;, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). Default value is SpinnersType.Ascii

<br>

```csharp
IControlProgressBar<T> Spinner(SpinnersType spinnersType, Nullable<Style> SpinnerStyle, Nullable<int> speedAnimation, IEnumerable<string> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
Spinners Type

`SpinnerStyle` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of spinner. [Style](./pplus.style.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **Width(Int32)**

Define Width to Widgets. Default value is 80.

```csharp
IControlProgressBar<T> Width(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Width

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **Default(Double)**

Initial value

```csharp
IControlProgressBar<T> Default(double value)
```

#### Parameters

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
value

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **FracionalDig(Int32)**

Define the Fracional Digits of value. Default is 0.

```csharp
IControlProgressBar<T> FracionalDig(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Fracional Digits

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **ChangeColor(Func&lt;Double, Style&gt;)**

Dynamically change Style Widgets

```csharp
IControlProgressBar<T> ChangeColor(Func<double, Style> value)
```

#### Parameters

`value` [Func&lt;Double, Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to change color

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **ChangeGradient(Color[])**

Dynamically Change Gradient color Widgets

```csharp
IControlProgressBar<T> ChangeGradient(Color[] colors)
```

#### Parameters

`colors` [Color[]](./pplus.color.md)<br>
list of colors Gradient

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)

### **UpdateHandler(Action&lt;UpdateProgressBar&lt;T&gt;, CancellationToken&gt;)**

Handler to execute Update ProgressBar values.

```csharp
IControlProgressBar<T> UpdateHandler(Action<UpdateProgressBar<T>, CancellationToken> value)
```

#### Parameters

`value` Action&lt;UpdateProgressBar&lt;T&gt;, CancellationToken&gt;<br>
Handler.See [UpdateProgressBar&lt;T&gt;](./pplus.controls.updateprogressbar-1.md) to change value

#### Returns

[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)<br>
[IControlProgressBar&lt;T&gt;](./pplus.controls.icontrolprogressbar-1.md)


- - -
[**Back to List Api**](./apis.md)
