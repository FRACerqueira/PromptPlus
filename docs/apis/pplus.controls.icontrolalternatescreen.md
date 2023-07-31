# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlAlternateScreen 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlAlternateScreen

Namespace: PPlus.Controls

Represents the interface with all Methods of the AlternateScreen control

```csharp
public interface IControlAlternateScreen : IPromptControls<Boolean>
```

Implements [IPromptControls&lt;Boolean&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### <a id="methods-backgroundcolor"/>**BackgroundColor(ConsoleColor)**

Set Background Color to AlternateScreen buffer.

```csharp
IControlAlternateScreen BackgroundColor(ConsoleColor value)
```

#### Parameters

`value` ConsoleColor<br>
The Background

#### Returns

[IControlAlternateScreen](./pplus.controls.icontrolalternatescreen.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlAlternateScreen Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlAlternateScreen](./pplus.controls.icontrolalternatescreen.md)

### <a id="methods-customaction"/>**CustomAction(Action&lt;CancellationToken&gt;)**

Action when console/terminal has Capability to swith to AlternateScreen buffer.
 <br>If not has capability to swith AlternateScreen, the action not run and the control return false, otherwise the control return true

```csharp
IControlAlternateScreen CustomAction(Action<CancellationToken> value)
```

#### Parameters

`value` [Action&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

#### Returns

[IControlAlternateScreen](./pplus.controls.icontrolalternatescreen.md)

### <a id="methods-foregroundcolor"/>**ForegroundColor(ConsoleColor)**

Set Foreground Color to AlternateScreen buffer.

```csharp
IControlAlternateScreen ForegroundColor(ConsoleColor value)
```

#### Parameters

`value` ConsoleColor<br>
The Foreground

#### Returns

[IControlAlternateScreen](./pplus.controls.icontrolalternatescreen.md)


- - -
[**Back to List Api**](./apis.md)
