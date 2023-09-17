# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:EventPipe<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# EventPipe&lt;T&gt;

Namespace: PPlus.Controls

Represents the event to pipe with with all Methods to change sequence

```csharp
public class EventPipe<T>
```

#### Type Parameters

`T`<br>
Typeof Input

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [EventPipe&lt;T&gt;](./pplus.controls.eventpipe-1.md)

## Properties

### <a id="properties-currentpipe"/>**CurrentPipe**

From Pipe

```csharp
public string CurrentPipe { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-frompipe"/>**FromPipe**

From Pipe

```csharp
public string FromPipe { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-input"/>**Input**

Input value

```csharp
public T Input { get; set; }
```

#### Property Value

T<br>

### <a id="properties-pipes"/>**Pipes**

List Pipes

```csharp
public ReadOnlyCollection<String> Pipes { get; }
```

#### Property Value

[ReadOnlyCollection&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.objectmodel.readonlycollection-1)<br>

### <a id="properties-topipe"/>**ToPipe**

Next Pipe

```csharp
public string ToPipe { get; private set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Methods

### <a id="methods-abortpipeline"/>**AbortPipeline()**

Abort Pipeline.

```csharp
public void AbortPipeline()
```

### <a id="methods-endpipeline"/>**EndPipeline()**

End Pipeline.

```csharp
public void EndPipeline()
```

### <a id="methods-nextpipe"/>**NextPipe(String)**

Set Next Pipe.

```csharp
public void NextPipe(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="methods-nextpipe"/>**NextPipe&lt;TID&gt;(TID)**

Set Next Pipe.

```csharp
public void NextPipe<TID>(TID value)
```

#### Type Parameters

`TID`<br>

#### Parameters

`value` TID<br>


- - -
[**Back to List Api**](./apis.md)
