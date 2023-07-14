# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlSliderNumber 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlSliderNumber

Namespace: PPlus.Controls

Represents the interface with all Methods of the SliderNumber control

```csharp
public interface IControlSliderNumber : IPromptControls<Double>
```

Implements [IPromptControls&lt;Double&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-changecolor"/>**ChangeColor(Func&lt;Double, Color&gt;)**

Dynamically change color Widgets
 <br>Valid only When MoveKeyPress equal left/right mode, otherwise its is ignored

```csharp
IControlSliderNumber ChangeColor(Func<Double, Color> value)
```

#### Parameters

`value` [Func&lt;Double, Color&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to change color

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;Double, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlSliderNumber ChangeDescription(Func<Double, String> value)
```

#### Parameters

`value` [Func&lt;Double, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply change

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-changegradient"/>**ChangeGradient(Color[])**

Dynamically Change Gradient color Widgets
 <br>Valid only When MoveKeyPress equal left/right mode, otherwise its is ignored

```csharp
IControlSliderNumber ChangeGradient(Color[] colors)
```

#### Parameters

`colors` [Color[]](./pplus.color.md)<br>
list of colors Gradient

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlSliderNumber Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-culture"/>**Culture(CultureInfo)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to validate input value format.

```csharp
IControlSliderNumber Culture(CultureInfo value)
```

#### Parameters

`value` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
CultureInfo to use on validate

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-culture"/>**Culture(String)**

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) to validate input when the type is not generic.
 <br>Default value is global Promptplus Cultureinfo

```csharp
IControlSliderNumber Culture(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Name of CultureInfo to use on validate

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-default"/>**Default(Double)**

Initial value

```csharp
IControlSliderNumber Default(double value)
```

#### Parameters

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
value

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-fracionaldig"/>**FracionalDig(Int32)**

Define the Fracional Digits of value. Default is 0.

```csharp
IControlSliderNumber FracionalDig(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Fracional Digits

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-largestep"/>**LargeStep(Double)**

Define the large step to change. Default value is 1/10 of range

```csharp
IControlSliderNumber LargeStep(double value)
```

#### Parameters

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
short step to change

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-movekeypress"/>**MoveKeyPress(SliderNumberType)**

Define the KeyPress to change value. Default value is Left or Right.
 <br>When MoveKeyPress equal Up or Down , slider control not show Widgets

```csharp
IControlSliderNumber MoveKeyPress(SliderNumberType value)
```

#### Parameters

`value` [SliderNumberType](./pplus.controls.slidernumbertype.md)<br>
Left/Right or Up/Down. [SliderNumberType](./pplus.controls.slidernumbertype.md)

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite default start value with last result saved on history.

```csharp
IControlSliderNumber OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-range"/>**Range(Double, Double)**

Defines a minimum and maximum range values

```csharp
IControlSliderNumber Range(double minvalue, double maxvalue)
```

#### Parameters

`minvalue` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
Minimum number

`maxvalue` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
Maximum number

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-step"/>**Step(Double)**

Define the short step to change. Default value is 1/100 of range

```csharp
IControlSliderNumber Step(double value)
```

#### Parameters

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
short step to change

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)

### <a id="methods-width"/>**Width(Int32)**

Define Width to Widgets. Default value is 40.

```csharp
IControlSliderNumber Width(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Width

#### Returns

[IControlSliderNumber](./pplus.controls.icontrolslidernumber.md)


- - -
[**Back to List Api**](./apis.md)
