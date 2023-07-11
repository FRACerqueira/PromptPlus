# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlKeyPress 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlKeyPress

Namespace: PPlus.Controls

```csharp
public interface IControlKeyPress : IPromptControls<ConsoleKeyInfo>
```

Implements [IPromptControls&lt;ConsoleKeyInfo&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-addkeyvalid"/>**AddKeyValid(ConsoleKey, Nullable&lt;ConsoleModifiers&gt;)**

Add Key and Modifiers valids for keypress

```csharp
IControlKeyPress AddKeyValid(ConsoleKey key, Nullable<ConsoleModifiers> modifiers)
```

#### Parameters

`key` ConsoleKey<br>
Key

`modifiers` [Nullable&lt;ConsoleModifiers&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Modifiers

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlKeyPress Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### <a id="methods-spinner"/>**Spinner(SpinnersType, Nullable&lt;Style&gt;, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). ValueResult value is SpinnersType.Ascii
 <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected

```csharp
IControlKeyPress Spinner(SpinnersType spinnersType, Nullable<Style> SpinnerStyle, Nullable<Int32> speedAnimation, IEnumerable<String> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
Spinners Type

`SpinnerStyle` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of spinner. [Style](./pplus.style.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable value for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)

### <a id="methods-textkeyvalid"/>**TextKeyValid(Func&lt;ConsoleKeyInfo, String&gt;)**

Overwrite default ConsoleKey string to custom string.
 <br>When return null value the control use defaut string

```csharp
IControlKeyPress TextKeyValid(Func<ConsoleKeyInfo, String> value)
```

#### Parameters

`value` [Func&lt;ConsoleKeyInfo, String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
Transform function. When return null value the control use defaut string

#### Returns

[IControlKeyPress](./pplus.controls.icontrolkeypress.md)


- - -
[**Back to List Api**](./apis.md)
