# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ResultProgessBar<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ResultProgessBar&lt;T&gt;

Namespace: PPlus.Controls

Represents The Result to ProgessBar Controls

```csharp
public struct ResultProgessBar<T>
```

#### Type Parameters

`T`<br>
Typeof return

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ResultProgessBar&lt;T&gt;](./pplus.controls.resultprogessbar-1.md)

## Properties

### <a id="properties-context"/>**Context**

Get conext Result

```csharp
public T Context { get; }
```

#### Property Value

T<br>

### <a id="properties-lastvalue"/>**Lastvalue**

Get last value progress

```csharp
public double Lastvalue { get; }
```

#### Property Value

[Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultProgessBar()**

Create a ResultProgessBar

```csharp
ResultProgessBar()
```

**Remarks:**

Do not use this constructor!


- - -
[**Back to List Api**](./apis.md)
