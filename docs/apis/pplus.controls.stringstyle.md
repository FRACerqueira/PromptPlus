# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:StringStyle 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# StringStyle

Namespace: PPlus.Controls

Represents the text string with style

```csharp
public struct StringStyle
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [StringStyle](./pplus.controls.stringstyle.md)<br>
Implements [IEquatable&lt;StringStyle&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Properties

### <a id="properties-style"/>**Style**

Get/Set Style/&gt;

```csharp
public Style Style { get; set; }
```

#### Property Value

[Style](./pplus.style.md)<br>

### <a id="properties-text"/>**Text**

Get/Set Text

```csharp
public string Text { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Constructors

### <a id="constructors-.ctor"/>**StringStyle()**

Create empty StringStyle

```csharp
StringStyle()
```

### <a id="constructors-.ctor"/>**StringStyle(String)**

Create StringStyle with text

```csharp
StringStyle(string text)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The text

### <a id="constructors-.ctor"/>**StringStyle(String, Style)**

Create a new instance of String-Style

```csharp
StringStyle(string text, Style style)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text

`style` [Style](./pplus.style.md)<br>
[StringStyle.Style](./pplus.controls.stringstyle.md#style) text

## Methods

### <a id="methods-equals"/>**Equals(StringStyle)**

Checks if two [StringStyle](./pplus.controls.stringstyle.md) instances are equal.

```csharp
bool Equals(StringStyle other)
```

#### Parameters

`other` [StringStyle](./pplus.controls.stringstyle.md)<br>
The StringStyle to compare.

#### Returns

`true` if the two StringStyle are equal, otherwise `false`.

### <a id="methods-equals"/>**Equals(Object)**

```csharp
bool Equals(object obj)
```

#### Parameters

`obj` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)

### <a id="methods-gethashcode"/>**GetHashCode()**

```csharp
int GetHashCode()
```

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)


- - -
[**Back to List Api**](./apis.md)
