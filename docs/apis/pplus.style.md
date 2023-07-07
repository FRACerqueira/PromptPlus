# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:Style 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# Style

Namespace: PPlus

Represents the Style : Colors and overflow strategy.

```csharp
public struct Style
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [Style](./pplus.style.md)<br>
Implements [IEquatable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## Properties

### **Plain**

Gets a [Style](./pplus.style.md) with the default colors and and overflow None.

```csharp
public static Style Plain { get; internal set; }
```

#### Property Value

[Style](./pplus.style.md)<br>

### **OverflowCrop**

Gets a [Style](./pplus.style.md) with the default colors and overflow Crop.

```csharp
public static Style OverflowCrop { get; }
```

#### Property Value

[Style](./pplus.style.md)<br>

### **OverflowEllipsis**

Gets a [Style](./pplus.style.md) with the default colors and overflow Ellipsis.

```csharp
public static Style OverflowEllipsis { get; }
```

#### Property Value

[Style](./pplus.style.md)<br>

### **Foreground**

Gets the foreground color.

```csharp
public Color Foreground { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **Background**

Gets the background color.

```csharp
public Color Background { get; }
```

#### Property Value

[Color](./pplus.color.md)<br>

### **OverflowStrategy**

Gets the Overflow strategy.

```csharp
public Overflow OverflowStrategy { get; }
```

#### Property Value

[Overflow](./pplus.overflow.md)<br>

## Constructors

### **Style()**

create a new [Style](./pplus.style.md) with default foreground/background colors and none overflow strategy.

```csharp
Style()
```

### **Style(Color, Color, Overflow)**

Create a new instance of [Style](./pplus.style.md) with foreground/background colors and overflow strategy.

```csharp
Style(Color foreground, Color background, Overflow overflowStrategy)
```

#### Parameters

`foreground` [Color](./pplus.color.md)<br>
[Color](./pplus.color.md) foreground

`background` [Color](./pplus.color.md)<br>
[Color](./pplus.color.md) background

`overflowStrategy` [Overflow](./pplus.overflow.md)<br>
[Overflow](./pplus.overflow.md) Strategy

## Methods

### **Combine(Style)**

```csharp
Style Combine(Style other)
```

#### Parameters

`other` [Style](./pplus.style.md)<br>

#### Returns

[Style](./pplus.style.md)<br>

### **GetHashCode()**

```csharp
int GetHashCode()
```

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **Equals(Style)**

Checks if two [Style](./pplus.style.md) instances are not equal.

```csharp
bool Equals(Style other)
```

#### Parameters

`other` [Style](./pplus.style.md)<br>
The Style instance to compare.

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
`true`true if the two Style are not equal, otherwise `false`false.

### **Equals(Object)**

```csharp
bool Equals(object obj)
```

#### Parameters

`obj` [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>


- - -
[**Back to List Api**](./apis.md)
