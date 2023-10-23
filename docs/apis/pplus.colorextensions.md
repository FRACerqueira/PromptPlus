# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ColorExtensions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ColorExtensions

Namespace: PPlus

Contains extension methods for [Color](./pplus.color.md).

```csharp
public static class ColorExtensions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ColorExtensions](./pplus.colorextensions.md)

## Methods

### <a id="methods-getinvertedcolor"/>**GetInvertedColor(Color)**

Get Inverted color by Luminance for best contrast

```csharp
public static Color GetInvertedColor(Color value)
```

#### Parameters

`value` [Color](./pplus.color.md)<br>
The value to write.

#### Returns

[Color](./pplus.color.md) White or Black

### <a id="methods-tostyle"/>**ToStyle(Color, Overflow)**

Convert Color to Style with default background color

```csharp
public static Style ToStyle(Color color, Overflow overflow)
```

#### Parameters

`color` [Color](./pplus.color.md)<br>
The color

`overflow` [Overflow](./pplus.overflow.md)<br>
The [Overflow](./pplus.overflow.md)

#### Returns

[Style](./pplus.style.md)


- - -
[**Back to List Api**](./apis.md)
