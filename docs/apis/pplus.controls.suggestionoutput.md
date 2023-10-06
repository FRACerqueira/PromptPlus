# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:SuggestionOutput 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
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
SuggestionOutput Add(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text suggestion

#### Returns

[SuggestionOutput](./pplus.controls.suggestionoutput.md)

### <a id="methods-addrange"/>**AddRange(IEnumerable&lt;String&gt;)**

Add Enumerable suggestions

```csharp
SuggestionOutput AddRange(IEnumerable<String> items)
```

#### Parameters

`items` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
Enumerable text suggestions

#### Returns

[SuggestionOutput](./pplus.controls.suggestionoutput.md)

### <a id="methods-create"/>**Create()**

Create a new instance of SuggestionOutput

```csharp
SuggestionOutput Create()
```

#### Returns

[SuggestionOutput](./pplus.controls.suggestionoutput.md)


- - -
[**Back to List Api**](./apis.md)
