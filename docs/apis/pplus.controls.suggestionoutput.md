# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:SuggestionOutput 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# SuggestionOutput

Namespace: PPlus.Controls

Represents The Suggestion output struct.

```csharp
public struct SuggestionOutput
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [SuggestionOutput](./pplus.controls.suggestionoutput.md)

## Constructors

### <a id="constructors-.ctor"/>**SuggestionOutput()**

Create a empty SuggestionOutput

```csharp
SuggestionOutput()
```

## Methods

### <a id="methods-add"/>**Add(String)**

Add suggestion

```csharp
void Add(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text suggestion

### <a id="methods-addrange"/>**AddRange(IEnumerable&lt;String&gt;)**

Add Enumerable suggestions

```csharp
void AddRange(IEnumerable<String> items)
```

#### Parameters

`items` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
Enumerable text suggestions


- - -
[**Back to List Api**](./apis.md)
