# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:EventWaitProcess<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# EventWaitProcess&lt;T&gt;

Namespace: PPlus.Controls

Represents the event to task process with with conex value

```csharp
public class EventWaitProcess<T> : System.IDisposable
```

#### Type Parameters

`T`<br>
Typeof Input

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventWaitProcess&lt;T&gt;](./pplus.controls.eventwaitprocess-1.md)<br>
Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable)

## Properties

### <a id="properties-cancelallnexttasks"/>**CancelAllNextTasks**

Get/Set Cancel all next tasks.

```csharp
public bool CancelAllNextTasks { get; set; }
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

### <a id="methods-dispose"/>**Dispose()**

Dispose

```csharp
public void Dispose()
```

### <a id="methods-dispose"/>**Dispose(Boolean)**

Dispose

```csharp
protected internal void Dispose(bool disposing)
```

#### Parameters

`disposing` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
if disposing


- - -
[**Back to List Api**](./apis.md)
