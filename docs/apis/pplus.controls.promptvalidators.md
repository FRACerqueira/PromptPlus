# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus PromptValidators 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# PromptValidators

Namespace: PPlus.Controls

Represents validation functions for controls

```csharp
public static class PromptValidators
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [PromptValidators](./pplus.controls.promptvalidators.md)

## Methods

### **ImportValidators&lt;T&gt;(T, Expression&lt;Func&lt;T, Object&gt;&gt;)**

Import validation from object to control

```csharp
public static Func`2[] ImportValidators<T>(T instance, Expression<Func<T, object>> expression)
```

#### Type Parameters

`T`<br>

#### Parameters

`instance` T<br>

`expression` Expression&lt;Func&lt;T, Object&gt;&gt;<br>

#### Returns

[Func`2[]](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsUriScheme(UriKind, String, String)**

Validation function for Uri Scheme

```csharp
public static Func<object, ValidationResult> IsUriScheme(UriKind uriKind, string allowedUriSchemes, string errorMessage)
```

#### Parameters

`uriKind` UriKind<br>
Kind of uri

`allowedUriSchemes` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
list of allowed uri scheme. Schemes must be separated by a semicolon.

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsNumber(CultureInfo, String)**

Validation function for Number

```csharp
public static Func<object, ValidationResult> IsNumber(CultureInfo cultureinfo, string errorMessage)
```

#### Parameters

`cultureinfo` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
Culture to validate

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsCurrency(CultureInfo, String)**

Validation function for Currency

```csharp
public static Func<object, ValidationResult> IsCurrency(CultureInfo cultureinfo, string errorMessage)
```

#### Parameters

`cultureinfo` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
Culture to validate

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsDateTime(CultureInfo, String)**

Validation function for DateTime

```csharp
public static Func<object, ValidationResult> IsDateTime(CultureInfo cultureinfo, string errorMessage)
```

#### Parameters

`cultureinfo` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
Culture to validate

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **Required(String)**

Validation function for Required

```csharp
public static Func<object, ValidationResult> Required(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **MinLength(Int32, String)**

Validation function for MinLength

```csharp
public static Func<object, ValidationResult> MinLength(int length, string errorMessage)
```

#### Parameters

`length` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
MinLength value

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **MaxLength(Int32, String)**

Validation function for MaxLength

```csharp
public static Func<object, ValidationResult> MaxLength(int length, string errorMessage)
```

#### Parameters

`length` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
MaxLength value

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **RegularExpression(String, String)**

Validation function for RegularExpression

```csharp
public static Func<object, ValidationResult> RegularExpression(string pattern, string errorMessage)
```

#### Parameters

`pattern` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
RegularExpression value

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeBoolean(String)**

Validation function for Is Type Boolean

```csharp
public static Func<object, ValidationResult> IsTypeBoolean(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeByte(String)**

Validation function for Is Type Byte

```csharp
public static Func<object, ValidationResult> IsTypeByte(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeChar(String)**

Validation function for Is Type Char

```csharp
public static Func<object, ValidationResult> IsTypeChar(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeDecimal(String)**

Validation function for Is Type Decimal

```csharp
public static Func<object, ValidationResult> IsTypeDecimal(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeDouble(String)**

Validation function for Is Type Double

```csharp
public static Func<object, ValidationResult> IsTypeDouble(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeShort(String)**

Validation function for Is Type Short

```csharp
public static Func<object, ValidationResult> IsTypeShort(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeInt16(String)**

Validation function for Is Type Int16

```csharp
public static Func<object, ValidationResult> IsTypeInt16(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeInt(String)**

Validation function for Is Type Int

```csharp
public static Func<object, ValidationResult> IsTypeInt(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeInt32(String)**

Validation function for Is Type Int32

```csharp
public static Func<object, ValidationResult> IsTypeInt32(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeLong(String)**

Validation function for Is Type Long

```csharp
public static Func<object, ValidationResult> IsTypeLong(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeInt64(String)**

Validation function for Is Type Int64

```csharp
public static Func<object, ValidationResult> IsTypeInt64(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeSByte(String)**

Validation function for Is Type SByte

```csharp
public static Func<object, ValidationResult> IsTypeSByte(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeFloat(String)**

Validation function for Is Type Float

```csharp
public static Func<object, ValidationResult> IsTypeFloat(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeSingle(String)**

Validation function for Is Type Single

```csharp
public static Func<object, ValidationResult> IsTypeSingle(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeUshort(String)**

Validation function for Is Type Ushort

```csharp
public static Func<object, ValidationResult> IsTypeUshort(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeUInt16(String)**

Validation function for Is Type UInt16

```csharp
public static Func<object, ValidationResult> IsTypeUInt16(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeUInt(String)**

Validation function for Is Type UInt

```csharp
public static Func<object, ValidationResult> IsTypeUInt(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeUInt32(String)**

Validation function for Is Type UInt32

```csharp
public static Func<object, ValidationResult> IsTypeUInt32(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeULong(String)**

Validation function for Is Type ULong

```csharp
public static Func<object, ValidationResult> IsTypeULong(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeUInt64(String)**

Validation function for Is Type UInt64

```csharp
public static Func<object, ValidationResult> IsTypeUInt64(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function

### **IsTypeDateTime(String)**

Validation function for Is Type DateTime

```csharp
public static Func<object, ValidationResult> IsTypeDateTime(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

[Func&lt;Object, ValidationResult&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.func-2)<br>
the Validation function


- - -
[**Back to List Api**](./apis.md)
