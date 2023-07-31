# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ResultPipeline<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ResultPipeline&lt;T&gt;

Namespace: PPlus.Controls

Represents The Result to Pipeline Controls

```csharp
public struct ResultPipeline<T>
```

#### Type Parameters

`T`<br>
Typeof return

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ResultPipeline&lt;T&gt;](./pplus.controls.resultpipeline-1.md)

## Properties

### <a id="properties-context"/>**Context**

Get conext value

```csharp
public T Context { get; }
```

#### Property Value

T<br>

### <a id="properties-pipes"/>**Pipes**

Get running status of pipeline

```csharp
public PipeRunningStatus[] Pipes { get; }
```

#### Property Value

[PipeRunningStatus[]](./pplus.controls.piperunningstatus.md)<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultPipeline()**

Create a ResultPipeline

```csharp
ResultPipeline()
```

**Remarks:**

Do not use this constructor!

### <a id="constructors-.ctor"/>**ResultPipeline(T, PipeRunningStatus[])**

Create a ResultPipeline.Purpose only for unit testing

```csharp
ResultPipeline(T conext, PipeRunningStatus[] pipes)
```

#### Parameters

`conext` T<br>
The value context

`pipes` [PipeRunningStatus[]](./pplus.controls.piperunningstatus.md)<br>
The status pipes


- - -
[**Back to List Api**](./apis.md)
