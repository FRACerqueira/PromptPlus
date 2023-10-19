# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:BannerDashOptions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# BannerDashOptions

Namespace: PPlus.Controls

Represents a boder when write Banner.

```csharp
public enum BannerDashOptions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Enum](https://docs.microsoft.com/en-us/dotnet/api/system.enum) → [BannerDashOptions](./pplus.controls.bannerdashoptions.md)<br>
Implements [IComparable](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable), [IFormattable](https://docs.microsoft.com/en-us/dotnet/api/system.iformattable), [IConvertible](https://docs.microsoft.com/en-us/dotnet/api/system.iconvertible)

## Fields

| Name | Value | Description |
| --- | --: | --- |
| None | 0 | Nome, No border. |
| AsciiSingleBorderDown | 1 | Ascii Single Border '-' after banner |
| AsciiDoubleBorderDown | 2 | Ascii Double Border '=' after banner. If not unicode supported write '=' |
| SingleBorderDown | 3 | Single Border '─' after banner. If not unicode supported write '-' |
| DoubleBorderDown | 4 | Single Border '═' after banner. If not unicode supported write '=' |
| HeavyBorderDown | 5 | Single Border '━' after banner. If not unicode supported write '*' |
| AsciiSingleBorderUpDown | 6 | Ascii single Border '=' before and after banner |
| AsciiDoubleBorderUpDown | 7 | Ascii Double Border '=' before and after banner |
| SingleBorderUpDown | 8 | Single Border '─' after banner and after banner. If not unicode supported write '-' |
| DoubleBorderUpDown | 9 | Single Border '═' after banner and after banner. If not unicode supported write '=' |
| HeavyBorderUpDown | 10 | Single Border '━' after banner. If not unicode supported write '*' |


- - -
[**Back to List Api**](./apis.md)
