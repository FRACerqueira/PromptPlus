# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:UpdateProgressBar<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# UpdateProgressBar&lt;T&gt;

Namespace: PPlus.Controls

Represents the commands to update values of Progress Bar

```csharp
public class UpdateProgressBar<T> : System.IDisposable
```

#### Type Parameters

`T`<br>
typeof instance result

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [UpdateProgressBar&lt;T&gt;](./pplus.controls.updateprogressbar-1.md)<br>
Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable)

## Properties

### <a id="properties-context"/>**Context**

Get/Set instance result value for general purpose

```csharp
public T Context { get; set; }
```

#### Property Value

T<br>

### <a id="properties-description"/>**Description**

Current Description

```csharp
public string Description { get; private set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### <a id="properties-finish"/>**Finish**

Get/Set Finish Progress Bar

```csharp
public bool Finish { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### <a id="properties-maxvalue"/>**Maxvalue**

Maximum value of Progress Bar

```csharp
public double Maxvalue { get; private set; }
```

#### Property Value

[Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>

### <a id="properties-minvalue"/>**Minvalue**

Minimal value of Progress Bar

```csharp
public double Minvalue { get; private set; }
```

#### Property Value

[Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>

### <a id="properties-value"/>**Value**

Current value of Progress Bar

```csharp
public double Value { get; private set; }
```

#### Property Value

[Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>

## Methods

### <a id="methods-changedescription"/>**ChangeDescription(String)**

Change curent Description

```csharp
public void ChangeDescription(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
new description

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

### <a id="methods-update"/>**Update(Double)**

Update current value

```csharp
public void Update(double value)
```

#### Parameters

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
new current value


- - -
[**Back to List Api**](./apis.md)
