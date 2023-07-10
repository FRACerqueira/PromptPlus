# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:SugestionOutput 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# SugestionOutput

Namespace: PPlus.Controls

Represents The Sugestion output struct.

```csharp
public struct SugestionOutput
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [SugestionOutput](./pplus.controls.sugestionoutput.md)

## Constructors

### **SugestionOutput()**

Create a empty SugestionOutput

```csharp
SugestionOutput()
```

## Methods

### **Add(String)**

Add sugestion

```csharp
void Add(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text sugestion

### **AddRange(IEnumerable&lt;String&gt;)**

Add Enumerable sugestions

```csharp
void AddRange(IEnumerable<String> items)
```

#### Parameters

`items` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
Enumerable text sugestions


- - -
[**Back to List Api**](./apis.md)
