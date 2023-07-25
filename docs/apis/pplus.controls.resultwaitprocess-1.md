# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:ResultWaitProcess<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# ResultWaitProcess&lt;T&gt;

Namespace: PPlus.Controls

Represents The Result to WaitProcess Controls

```csharp
public struct ResultWaitProcess<T>
```

#### Type Parameters

`T`<br>
Typeof return

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [ResultWaitProcess&lt;T&gt;](./pplus.controls.resultwaitprocess-1.md)

## Properties

### <a id="properties-context"/>**Context**

Get conext value

```csharp
public T Context { get; }
```

#### Property Value

T<br>

### <a id="properties-states"/>**States**

Get State of process

```csharp
public StateProcess[] States { get; }
```

#### Property Value

[StateProcess[]](./pplus.controls.stateprocess.md)<br>

## Constructors

### <a id="constructors-.ctor"/>**ResultWaitProcess()**

Create a ResultPipeline

```csharp
ResultWaitProcess()
```

**Remarks:**

Do not use this constructor!


- - -
[**Back to List Api**](./apis.md)
