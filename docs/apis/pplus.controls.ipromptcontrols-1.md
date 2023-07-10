# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IPromptControls<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IPromptControls&lt;T&gt;

Namespace: PPlus.Controls

Represents the interface with all Methods/Properties of the control

```csharp
public interface IPromptControls<T>
```

#### Type Parameters

`T`<br>

## Methods

### **Run(Nullable&lt;CancellationToken&gt;)**

Execute this control and return ResultPrompt with type .

```csharp
ResultPrompt<T> Run(Nullable<CancellationToken> value)
```

#### Parameters

`value` [Nullable&lt;CancellationToken&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
[CancellationToken](https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken) for control

#### Returns

[ResultPrompt&lt;T&gt;](./pplus.controls.resultprompt-1.md)


- - -
[**Back to List Api**](./apis.md)
