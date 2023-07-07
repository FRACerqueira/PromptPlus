# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ColorExtensions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
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

### **GetInvertedColor(Color)**

Get Inverted color by Luminance for best contrast

```csharp
public static Color GetInvertedColor(Color value)
```

#### Parameters

`value` [Color](./pplus.color.md)<br>
The value to write.

#### Returns

[Color](./pplus.color.md)<br>
[Color](./pplus.color.md) White or Black


- - -
[**Back to List Api**](./apis.md)
