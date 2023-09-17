# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IConsoleExtendDrive 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IConsoleExtendDrive

Namespace: PPlus

Represents the interface for extend console.

```csharp
public interface IConsoleExtendDrive
```

## Properties

### <a id="properties-currentbuffer"/>**CurrentBuffer**

Get Current Screen Buffer

```csharp
public abstract TargetBuffer CurrentBuffer { get; }
```

#### Property Value

[TargetBuffer](./pplus.targetbuffer.md)<br>

### <a id="properties-enabledextend"/>**EnabledExtend**

The extend capacity is enabled

```csharp
public abstract bool EnabledExtend { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

## Methods

### <a id="methods-onbuffer"/>**OnBuffer(TargetBuffer, Action&lt;CancellationToken&gt;, Nullable&lt;ConsoleColor&gt;, Nullable&lt;ConsoleColor&gt;, Nullable&lt;CancellationToken&gt;)**

Run a action on target screen buffer and return to original screen buffer

```csharp
bool OnBuffer(TargetBuffer target, Action<CancellationToken> value, Nullable<ConsoleColor> defaultforecolor, Nullable<ConsoleColor> defaultbackcolor, Nullable<CancellationToken> cancellationToken)
```

#### Parameters

`target` [TargetBuffer](./pplus.targetbuffer.md)<br>
The target buffer

`value` [Action&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
The action

`defaultforecolor` [Nullable&lt;ConsoleColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The default fore color

`defaultbackcolor` [Nullable&lt;ConsoleColor&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The default back color

`cancellationToken` [Nullable&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The [CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken)

#### Returns

True when console has capacity to run on target buffer, otherwhise false

### <a id="methods-swapbuffer"/>**SwapBuffer(TargetBuffer)**

Swap Screen Buffer

```csharp
bool SwapBuffer(TargetBuffer value)
```

#### Parameters

`value` [TargetBuffer](./pplus.targetbuffer.md)<br>
The target buffer

#### Returns

True when console has capacity to swap to target buffer, otherwhise false


- - -
[**Back to List Api**](./apis.md)
