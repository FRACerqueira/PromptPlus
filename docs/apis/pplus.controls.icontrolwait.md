# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus IControlWait 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlWait

Namespace: PPlus.Controls

```csharp
public interface IControlWait : IPromptControls`1
```

Implements [IPromptControls&lt;IEnumerable&lt;StateProcess&gt;&gt;](./pplus.controls.ipromptcontrols-1.md)

## Methods

### **Interaction&lt;T&gt;(IEnumerable&lt;T&gt;, Action&lt;IControlWait, T&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlWait Interaction<T>(IEnumerable<T> values, Action<IControlWait, T> action)
```

#### Type Parameters

`T`<br>
typeof item

#### Parameters

`values` IEnumerable&lt;T&gt;<br>
Colletion for interaction

`action` Action&lt;IControlWait, T&gt;<br>
Action to execute

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **MaxDegreeProcess(Int32)**

Maximum number of concurrent tasks enable. Default vaue is number of processors.

```csharp
IControlWait MaxDegreeProcess(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of concurrent tasks

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **TaskTitle(String)**

Overwrite Task Title . Default task title comes from the embedded resource.

```csharp
IControlWait TaskTitle(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
TaskTitle Task

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **ShowElapsedTime()**

Define if show Elapsed Time for each task and the format of Elapsed Time.

```csharp
IControlWait ShowElapsedTime()
```

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlWait Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
Action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **Finish(String)**

Finish answer to show when Wait process is completed.

```csharp
IControlWait Finish(string text)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text Finish answer

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **Spinner(SpinnersType, Nullable&lt;Style&gt;, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). Default value is SpinnersType.Ascii

<br>

```csharp
IControlWait Spinner(SpinnersType spinnersType, Nullable<Style> SpinnerStyle, Nullable<int> speedAnimation, IEnumerable<string> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
Spinners Type

`SpinnerStyle` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of spinner. [Style](./pplus.style.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach interation of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **AddStep(StepMode, Action`1[])**

Add list of tasks to execute.

```csharp
IControlWait AddStep(StepMode stepMode, Action`1[] process)
```

#### Parameters

`stepMode` [StepMode](./pplus.controls.stepmode.md)<br>
Sequential or parallel execution

`process` [Action`1[]](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
list of tasks

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)

### **AddStep(StepMode, String, String, Action`1[])**

Add list of tasks to execute with title and description

```csharp
IControlWait AddStep(StepMode stepMode, string id, string description, Action`1[] process)
```

#### Parameters

`stepMode` [StepMode](./pplus.controls.stepmode.md)<br>
Sequential or parallel execution

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
TaskTitle of tasks

`description` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Description of tasks

`process` [Action`1[]](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
list of tasks

#### Returns

[IControlWait](./pplus.controls.icontrolwait.md)<br>
[IControlWait](./pplus.controls.icontrolwait.md)


- - -
[**Back to List Api**](./apis.md)
