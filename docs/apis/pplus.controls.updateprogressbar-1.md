# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:UpdateProgressBar 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# UpdateProgressBar&lt;T&gt;

Namespace: PPlus.Controls

Represents the commands to update values of Progress Bar

```csharp
public class UpdateProgressBar<T>
```

#### Type Parameters

`T`<br>
typeof instance result

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [UpdateProgressBar&lt;T&gt;](./pplus.controls.updateprogressbar-1.md)

## Properties

### **Maxvalue**

Maximum value of Progress Bar

```csharp
public double Maxvalue { get; private set; }
```

#### Property Value

[Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>

### **Minvalue**

Minimal value of Progress Bar

```csharp
public double Minvalue { get; private set; }
```

#### Property Value

[Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>

### **Value**

Current value of Progress Bar

```csharp
public double Value { get; private set; }
```

#### Property Value

[Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>

### **Finish**

Get/Set Finish Progress Bar

```csharp
public bool Finish { get; set; }
```

#### Property Value

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **Context**

Get/Set instance result value for general purpose

```csharp
public T Context { get; set; }
```

#### Property Value

T<br>

### **Description**

Current Description

```csharp
public string Description { get; private set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Methods

### **Update(Double)**

Update current value

```csharp
public void Update(double value)
```

#### Parameters

`value` [Double](https://docs.microsoft.com/en-us/dotnet/api/system.double)<br>
new current value

### **ChangeDescription(String)**

Change curent Description

```csharp
public void ChangeDescription(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
new description


- - -
[**Back to List Api**](./apis.md)
