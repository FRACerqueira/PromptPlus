# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:DashOptions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# DashOptions

Namespace: PPlus

Represents a boder when write line with SingleDash/DoubleDash.

```csharp
public enum DashOptions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Enum](https://docs.microsoft.com/en-us/dotnet/api/system.enum) → [DashOptions](./pplus.dashoptions.md)<br>
Implements [IComparable](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable), [IFormattable](https://docs.microsoft.com/en-us/dotnet/api/system.iformattable), [IConvertible](https://docs.microsoft.com/en-us/dotnet/api/system.iconvertible)

## Fields

| Name | Value | Description |
| --- | --: | --- |
| AsciiSingleBorder | 0 | Ascii Single Border '-' |
| AsciiDoubleBorder | 1 | Ascii Single Border '=' |
| SingleBorder | 2 | Single Border unicode '─' <br>When not supported unicode : '-' |
| DoubleBorder | 3 | Double Border unicode '═' <br>When not supported unicode : '=' |
| HeavyBorder | 4 | Heavy Border unicode '━' <br>When not supported unicode : '*' |


- - -
[**Back to List Api**](./apis.md)
