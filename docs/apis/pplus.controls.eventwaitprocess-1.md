# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:EventWaitProcess<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# EventWaitProcess&lt;T&gt;

Namespace: PPlus.Controls

Represents the event to task process with with conex value

```csharp
public class EventWaitProcess<T>
```

#### Type Parameters

`T`<br>
Typeof Input

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventWaitProcess&lt;T&gt;](./pplus.controls.eventwaitprocess-1.md)

## Properties

### <a id="properties-cancelnextall"/>**CancelNextAll**

Context value

```csharp
public bool CancelNextAll { get; private set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-context"/>**Context**

Get/set Context value

```csharp
public T Context { get; set; }
```

#### Property Value

T<br>

## Methods

### <a id="methods-cancelallnexttasks"/>**CancelAllNextTasks()**

Set Cancel all next tasks.

```csharp
public void CancelAllNextTasks()
```


- - -
[**Back to List Api**](./apis.md)
