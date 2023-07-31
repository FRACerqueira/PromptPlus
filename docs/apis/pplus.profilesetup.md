# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ProfileSetup 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ProfileSetup

Namespace: PPlus

Represents Profile Setup for console.

```csharp
public class ProfileSetup
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ProfileSetup](./pplus.profilesetup.md)

## Properties

### <a id="properties-backgroundcolor"/>**BackgroundColor**

Get/Set BackgroundColor console with color.

```csharp
public ConsoleColor BackgroundColor { get; set; }
```

#### Property Value

ConsoleColor<br>

### <a id="properties-colordepth"/>**ColorDepth**

Get/Set Color capacity.[ColorSystem](./pplus.colorsystem.md)

```csharp
public ColorSystem ColorDepth { get; set; }
```

#### Property Value

[ColorSystem](./pplus.colorsystem.md)<br>

### <a id="properties-culture"/>**Culture**

Get/Set Default Culture for console
 <br>Culture is global set fro threads

```csharp
public CultureInfo Culture { get; set; }
```

#### Property Value

[CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>

### <a id="properties-foregroundcolor"/>**ForegroundColor**

Get/Set Foreground console with color.

```csharp
public ConsoleColor ForegroundColor { get; set; }
```

#### Property Value

ConsoleColor<br>

### <a id="properties-islegacy"/>**IsLegacy**

Get/Set Terminal is legacy.

```csharp
public bool IsLegacy { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isterminal"/>**IsTerminal**

Get/Set Terminal mode. if Running over Terminal mode or not.

```csharp
public bool IsTerminal { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-isunicodesupported"/>**IsUnicodeSupported**

Get/Set Unicode Supported.

```csharp
public bool IsUnicodeSupported { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-overflowstrategy"/>**OverflowStrategy**

Get/Set write Overflow Strategy.

```csharp
public Overflow OverflowStrategy { get; set; }
```

#### Property Value

[Overflow](./pplus.overflow.md)<br>

### <a id="properties-padleft"/>**PadLeft**

Get/Set screen margin left

```csharp
public byte PadLeft { get; set; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### <a id="properties-padright"/>**PadRight**

Get/Set screen margin right

```csharp
public byte PadRight { get; set; }
```

#### Property Value

[Byte](https://docs.microsoft.com/en-us/dotnet/api/system.byte)<br>

### <a id="properties-supportsansi"/>**SupportsAnsi**

Get/Set SupportsAnsi mode commands.

```csharp
public bool SupportsAnsi { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>


- - -
[**Back to List Api**](./apis.md)
