# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlSliderSwitch 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlSliderSwitch

Namespace: PPlus.Controls

Represents the interface with all Methods of the SliderSwitch control

```csharp
public interface IControlSliderSwitch : IPromptControls<Boolean>
```

Implements [IPromptControls&lt;Boolean&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-changecoloroff"/>**ChangeColorOff(Style)**

Change Color when state 'Off'. 
 <br>state-Off(Foreground)/Background<br>Default Foreground : 'ConsoleColor.Cyan'<br>Default Background : 'ConsoleColor.DarkGray'

```csharp
IControlSliderSwitch ChangeColorOff(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-changecoloron"/>**ChangeColorOn(Style)**

Change Color when state 'On'. 
 <br>state-On(Foreground)/Background<br>Default Foreground : 'ConsoleColor.Cyan'<br>Default Background : 'ConsoleColor.DarkGray'

```csharp
IControlSliderSwitch ChangeColorOn(Style value)
```

#### Parameters

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-changedescription"/>**ChangeDescription(Func&lt;Boolean, String&gt;)**

Dynamically change the description using a user role

```csharp
IControlSliderSwitch ChangeDescription(Func<Boolean, String> value)
```

#### Parameters

`value` [Func&lt;Boolean, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
function to apply change

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlSliderSwitch Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-default"/>**Default(Boolean)**

Default value for switch

```csharp
IControlSliderSwitch Default(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true is 'on', otherwise 'off'

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-offvalue"/>**OffValue(String)**

Text to 'off' value. Default value comes from resource.

```csharp
IControlSliderSwitch OffValue(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text off

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-onvalue"/>**OnValue(String)**

Text to 'on' value. Default value comes from resource.

```csharp
IControlSliderSwitch OnValue(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text on

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-overwritedefaultfrom"/>**OverwriteDefaultFrom(String, Nullable&lt;TimeSpan&gt;)**

Overwrite default start value with last result saved on history.

```csharp
IControlSliderSwitch OverwriteDefaultFrom(string value, Nullable<TimeSpan> timeout)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
name of file to save history

`timeout` [Nullable&lt;TimeSpan&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The timeout for valid items saved. Default value is 365 days

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-styles"/>**Styles(SliderSwitchStyles, Style)**

Overwrite Styles

```csharp
IControlSliderSwitch Styles(SliderSwitchStyles styletype, Style value)
```

#### Parameters

`styletype` [SliderSwitchStyles](./pplus.controls.sliderswitchstyles.md)<br>
[SliderSwitchStyles](./pplus.controls.sliderswitchstyles.md) of content

`value` [Style](./pplus.style.md)<br>
The [Style](./pplus.style.md)

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)

### <a id="methods-width"/>**Width(Int32)**

Define Width to Widgets. Default value is 6.The value must be greater than or equal to 6.

```csharp
IControlSliderSwitch Width(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Width

#### Returns

[IControlSliderSwitch](./pplus.controls.icontrolsliderswitch.md)


- - -
[**Back to List Api**](./apis.md)
