# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ResultMasked 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ResultMasked

Namespace: PPlus.Controls

Represents The Result to masked Controls

```csharp
public struct ResultMasked
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ResultMasked](./pplus.controls.resultmasked.md)

## Properties

### <a id="properties-input"/>**Input**

Get Text without mask

```csharp
public string Input { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-masked"/>**Masked**

Get Text with mask

```csharp
public string Masked { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultMasked()**

Create a ResultMasked

```csharp
ResultMasked()
```

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**ResultMasked(String, String)**

Create ResultMasked instance.

```csharp
ResultMasked(string value, string valueMask)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`valueMask` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>


- - -
[**Back to List Api**](./apis.md)
