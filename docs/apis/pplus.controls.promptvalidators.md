# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:PromptValidators 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
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

### <a id="methods-importvalidators"/>**ImportValidators&lt;T&gt;(T, Expression&lt;Func&lt;T, Object&gt;&gt;)**

Import validation from object to control

```csharp
public static Func<Object, ValidationResult>[] ImportValidators<T>(T instance, Expression<Func<T, Object>> expression)
```

#### Type Parameters

`T`<br>

#### Parameters

`instance` T<br>

`expression` Expression&lt;Func&lt;T, Object&gt;&gt;<br>

#### Returns

the Validation function

### <a id="methods-iscurrency"/>**IsCurrency(CultureInfo, String)**

Validation function for Currency

```csharp
public static Func<Object, ValidationResult> IsCurrency(CultureInfo cultureinfo, string errorMessage)
```

#### Parameters

`cultureinfo` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
Culture to validate

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-isdatetime"/>**IsDateTime(CultureInfo, String)**

Validation function for DateTime

```csharp
public static Func<Object, ValidationResult> IsDateTime(CultureInfo cultureinfo, string errorMessage)
```

#### Parameters

`cultureinfo` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
Culture to validate

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-isnumber"/>**IsNumber(CultureInfo, String)**

Validation function for Number

```csharp
public static Func<Object, ValidationResult> IsNumber(CultureInfo cultureinfo, string errorMessage)
```

#### Parameters

`cultureinfo` [CultureInfo](https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)<br>
Culture to validate

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeboolean"/>**IsTypeBoolean(String)**

Validation function for Is type Boolean

```csharp
public static Func<Object, ValidationResult> IsTypeBoolean(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypebyte"/>**IsTypeByte(String)**

Validation function for Is type Byte

```csharp
public static Func<Object, ValidationResult> IsTypeByte(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypechar"/>**IsTypeChar(String)**

Validation function for Is type Char

```csharp
public static Func<Object, ValidationResult> IsTypeChar(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypedatetime"/>**IsTypeDateTime(String)**

Validation function for Is type DateTime

```csharp
public static Func<Object, ValidationResult> IsTypeDateTime(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypedecimal"/>**IsTypeDecimal(String)**

Validation function for Is type Decimal

```csharp
public static Func<Object, ValidationResult> IsTypeDecimal(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypedouble"/>**IsTypeDouble(String)**

Validation function for Is type Double

```csharp
public static Func<Object, ValidationResult> IsTypeDouble(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypefloat"/>**IsTypeFloat(String)**

Validation function for Is type Float

```csharp
public static Func<Object, ValidationResult> IsTypeFloat(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeint"/>**IsTypeInt(String)**

Validation function for Is type Int

```csharp
public static Func<Object, ValidationResult> IsTypeInt(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeint16"/>**IsTypeInt16(String)**

Validation function for Is type Int16

```csharp
public static Func<Object, ValidationResult> IsTypeInt16(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeint32"/>**IsTypeInt32(String)**

Validation function for Is type Int32

```csharp
public static Func<Object, ValidationResult> IsTypeInt32(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeint64"/>**IsTypeInt64(String)**

Validation function for Is type Int64

```csharp
public static Func<Object, ValidationResult> IsTypeInt64(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypelong"/>**IsTypeLong(String)**

Validation function for Is type Long

```csharp
public static Func<Object, ValidationResult> IsTypeLong(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypesbyte"/>**IsTypeSByte(String)**

Validation function for Is type SByte

```csharp
public static Func<Object, ValidationResult> IsTypeSByte(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeshort"/>**IsTypeShort(String)**

Validation function for Is type Short

```csharp
public static Func<Object, ValidationResult> IsTypeShort(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypesingle"/>**IsTypeSingle(String)**

Validation function for Is type Single

```csharp
public static Func<Object, ValidationResult> IsTypeSingle(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeuint"/>**IsTypeUInt(String)**

Validation function for Is type UInt

```csharp
public static Func<Object, ValidationResult> IsTypeUInt(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeuint16"/>**IsTypeUInt16(String)**

Validation function for Is type UInt16

```csharp
public static Func<Object, ValidationResult> IsTypeUInt16(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeuint32"/>**IsTypeUInt32(String)**

Validation function for Is type UInt32

```csharp
public static Func<Object, ValidationResult> IsTypeUInt32(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeuint64"/>**IsTypeUInt64(String)**

Validation function for Is type UInt64

```csharp
public static Func<Object, ValidationResult> IsTypeUInt64(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeulong"/>**IsTypeULong(String)**

Validation function for Is type ULong

```csharp
public static Func<Object, ValidationResult> IsTypeULong(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-istypeushort"/>**IsTypeUshort(String)**

Validation function for Is type Ushort

```csharp
public static Func<Object, ValidationResult> IsTypeUshort(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-isurischeme"/>**IsUriScheme(UriKind, String, String)**

Validation function for Uri Scheme

```csharp
public static Func<Object, ValidationResult> IsUriScheme(UriKind uriKind, string allowedUriSchemes, string errorMessage)
```

#### Parameters

`uriKind` UriKind<br>
Kind of uri

`allowedUriSchemes` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
list of allowed uri scheme. Schemes must be separated by a semicolon.

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-maxlength"/>**MaxLength(Int32, String)**

Validation function for MaxLength

```csharp
public static Func<Object, ValidationResult> MaxLength(int length, string errorMessage)
```

#### Parameters

`length` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
MaxLength value

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-minlength"/>**MinLength(Int32, String)**

Validation function for MinLength

```csharp
public static Func<Object, ValidationResult> MinLength(int length, string errorMessage)
```

#### Parameters

`length` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
MinLength value

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-regularexpression"/>**RegularExpression(String, String)**

Validation function for RegularExpression

```csharp
public static Func<Object, ValidationResult> RegularExpression(string pattern, string errorMessage)
```

#### Parameters

`pattern` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
RegularExpression value

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function

### <a id="methods-required"/>**Required(String)**

Validation function for Required

```csharp
public static Func<Object, ValidationResult> Required(string errorMessage)
```

#### Parameters

`errorMessage` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Custom error message to show

#### Returns

the Validation function


- - -
[**Back to List Api**](./apis.md)
