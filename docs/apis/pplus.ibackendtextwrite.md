# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IBackendTextWrite 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IBackendTextWrite

Namespace: PPlus

Represents the interface for write text console.

```csharp
public interface IBackendTextWrite
```

## Methods

### <a id="methods-write"/>**Write(String, Nullable&lt;Style&gt;, Boolean)**

Write a text to output console.

```csharp
int Write(string value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

Number of lines write on console

### <a id="methods-writeline"/>**WriteLine(String, Nullable&lt;Style&gt;, Boolean)**

Write a text to output console with line terminator.

```csharp
int WriteLine(string value, Nullable<Style> style, bool clearrestofline)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
text to write

`style` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of text

`clearrestofline` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
Clear rest of line after write

#### Returns

Number of lines write on console


- - -
[**Back to List Api**](./apis.md)
