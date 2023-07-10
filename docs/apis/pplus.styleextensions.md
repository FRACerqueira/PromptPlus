# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:StyleExtensions 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# StyleExtensions

Namespace: PPlus

Contains extension methods for [Style](./pplus.style.md).

```csharp
public static class StyleExtensions
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [StyleExtensions](./pplus.styleextensions.md)

## Methods

### **Foreground(Style, Color)**

Create a new style from the specified one with the specified foreground color.

```csharp
public static Style Foreground(Style style, Color color)
```

#### Parameters

`style` [Style](./pplus.style.md)<br>
The style.

`color` [Color](./pplus.color.md)<br>
The foreground color.

#### Returns

The new [Style](./pplus.style.md)

### **Overflow(Style, Overflow)**

Create a new style from the specified one with the specified overfow strategy

```csharp
public static Style Overflow(Style style, Overflow overflow)
```

#### Parameters

`style` [Style](./pplus.style.md)<br>
The style.

`overflow` [Overflow](./pplus.overflow.md)<br>
The [Overflow](./pplus.overflow.md) overflow strategy

#### Returns

The new [Style](./pplus.style.md)

### **Background(Style, Color)**

Create a new style from the specified one with
 the specified background color.

```csharp
public static Style Background(Style style, Color color)
```

#### Parameters

`style` [Style](./pplus.style.md)<br>
The style.

`color` [Color](./pplus.color.md)<br>
The background color.

#### Returns

The new [Style](./pplus.style.md)


- - -
[**Back to List Api**](./apis.md)
