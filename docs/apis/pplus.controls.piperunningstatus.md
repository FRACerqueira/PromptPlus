# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:PipeRunningStatus 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# PipeRunningStatus

Namespace: PPlus.Controls

Represents the running status of the tube

```csharp
public struct PipeRunningStatus
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://docs.microsoft.com/en-us/dotnet/api/system.valuetype) → [PipeRunningStatus](./pplus.controls.piperunningstatus.md)

## Properties

### <a id="properties-elapsedtime"/>**Elapsedtime**

Get Elapsedtime pipe

```csharp
public TimeSpan Elapsedtime { get; }
```

#### Property Value

[TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan)<br>

### <a id="properties-pipe"/>**Pipe**

Get pipes id

```csharp
public string Pipe { get; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-status"/>**Status**

Get status pipes

```csharp
public PipeStatus Status { get; }
```

#### Property Value

[PipeStatus](./pplus.controls.pipestatus.md)<br>

## Constructors

### <a id="constructors-.ctor"/>**PipeRunningStatus()**

Create a PipeRunningStatus

```csharp
PipeRunningStatus()
```

**Remarks:**

Do not use this constructor!


- - -
[**Back to List Api**](./apis.md)
