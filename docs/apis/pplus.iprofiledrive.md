# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IProfileDrive 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IProfileDrive

Namespace: PPlus

Represents the interface for profile setup for console.

```csharp
public interface IProfileDrive
```

## Properties

### <a id="properties-bufferheight"/>**BufferHeight**

Gets the height of the buffer area.

```csharp
public abstract int BufferHeight { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-bufferwidth"/>**BufferWidth**

Gets the width of the buffer area.

```csharp
public abstract int BufferWidth { get; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-colordepth"/>**ColorDepth**

Get Color capacity.[ColorSystem](./pplus.colorsystem.md)

```csharp
public abstract ColorSystem ColorDepth { get; }
```

#### Property Value

[ColorSystem](./pplus.colorsystem.md)<br>

### <a id="properties-islegacy"/>**IsLegacy**

Get Terminal is legacy.

```csharp
public abstract bool IsLegacy { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isterminal"/>**IsTerminal**

Get Terminal mode.

```csharp
public abstract bool IsTerminal { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isunicodesupported"/>**IsUnicodeSupported**

Get Unicode Supported.

```csharp
public abstract bool IsUnicodeSupported { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-overflowstrategy"/>**OverflowStrategy**

Get write Overflow Strategy.

```csharp
public abstract Overflow OverflowStrategy { get; }
```

#### Property Value

[Overflow](./pplus.overflow.md)<br>

### <a id="properties-padleft"/>**PadLeft**

Get screen margin left

```csharp
public abstract byte PadLeft { get; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### <a id="properties-padright"/>**PadRight**

Get screen margin right

```csharp
public abstract byte PadRight { get; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### <a id="properties-provider"/>**Provider**

Get provider mode.

```csharp
public abstract string Provider { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-supportsansi"/>**SupportsAnsi**

Get SupportsAnsi mode.

```csharp
public abstract bool SupportsAnsi { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>


- - -
[**Back to List Api**](./apis.md)
