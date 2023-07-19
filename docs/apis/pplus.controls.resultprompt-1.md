# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ResultPrompt<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ResultPrompt&lt;T&gt;

Namespace: PPlus.Controls

Represents The Result  to Controls

```csharp
public struct ResultPrompt<T>
```

#### Type Parameters

`T`<br>

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ResultPrompt&lt;T&gt;](./pplus.controls.resultprompt-1.md)

## Properties

### <a id="properties-isaborted"/>**IsAborted**

Control is Aborted. True to aborted; otherwise, false.

```csharp
public bool IsAborted { get; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-value"/>**Value**

Value result

```csharp
public T Value { get; }
```

#### Property Value

T<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultPrompt()**

Create a ResultPrompt

```csharp
ResultPrompt()
```

**Remarks:**

Do not use this constructor!


- - -
[**Back to List Api**](./apis.md)
