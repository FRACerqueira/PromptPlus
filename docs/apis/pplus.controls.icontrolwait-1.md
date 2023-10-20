# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlWait<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlWait&lt;T&gt;

Namespace: PPlus.Controls

Represents the interface with all Methods of the WaitTimer/WaitProcess control

```csharp
public interface IControlWait<T> : IPromptControls<ResultWaitProcess<T>>
```

#### Type Parameters

`T`<br>
typeof return

Implements IPromptControls&lt;ResultWaitProcess&lt;T&gt;&gt;

## Methods

### <a id="methods-addstep"/>**AddStep(StepMode, params Action&lt;EventWaitProcess&lt;T&gt;, CancellationToken&gt;[])**

Add list of tasks to execute.

```csharp
IControlWait<T> AddStep(StepMode stepMode, params Action<EventWaitProcess<T>, CancellationToken>[] process)
```

#### Parameters

`stepMode` [StepMode](./pplus.controls.stepmode.md)<br>
Sequential or parallel execution

`process` Action&lt;EventWaitProcess&lt;T&gt;, CancellationToken&gt;[]<br>
list of tasks

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-addstep"/>**AddStep(StepMode, String, String, params Action&lt;EventWaitProcess&lt;T&gt;, CancellationToken&gt;[])**

Add list of tasks to execute with title and description

```csharp
IControlWait<T> AddStep(StepMode stepMode, string id, string label, params Action<EventWaitProcess<T>, CancellationToken>[] process)
```

#### Parameters

`stepMode` [StepMode](./pplus.controls.stepmode.md)<br>
Sequential or parallel execution

`id` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Id of tasks

`label` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Label of tasks

`process` Action&lt;EventWaitProcess&lt;T&gt;, CancellationToken&gt;[]<br>
list of tasks

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlWait<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
Action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-context"/>**Context(T)**

Set Contex value for all tasks

```csharp
IControlWait<T> Context(T value)
```

#### Parameters

`value` T<br>
Context value

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-finish"/>**Finish(String)**

Finish answer to show when Wait process is completed.

```csharp
IControlWait<T> Finish(string text)
```

#### Parameters

`text` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
Text Finish answer

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-interaction"/>**Interaction&lt;T1&gt;(IEnumerable&lt;T1&gt;, Action&lt;IControlWait&lt;T&gt;, T1&gt;)**

Execute a action foreach item of colletion passed as a parameter

```csharp
IControlWait<T> Interaction<T1>(IEnumerable<T1> values, Action<IControlWait<T>, T1> action)
```

#### Type Parameters

`T1`<br>

#### Parameters

`values` IEnumerable&lt;T1&gt;<br>
Colletion for interaction

`action` Action&lt;IControlWait&lt;T&gt;, T1&gt;<br>
Action to execute

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-maxdegreeprocess"/>**MaxDegreeProcess(Int32)**

Maximum number of concurrent tasks enable. Default vaue is number of processors.

```csharp
IControlWait<T> MaxDegreeProcess(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Number of concurrent tasks

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-showelapsedtime"/>**ShowElapsedTime(Boolean)**

Define if show Elapsed Time for each task.Default false.

```csharp
IControlWait<T> ShowElapsedTime(bool value)
```

#### Parameters

`value` [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
show Elapsed Time

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-spinner"/>**Spinner(SpinnersType, Nullable&lt;Style&gt;, Nullable&lt;Int32&gt;, IEnumerable&lt;String&gt;)**

Overwrite [SpinnersType](./pplus.controls.spinnerstype.md). Default value is SpinnersType.Ascii
 <br>When use custom spinner, if has unicode values console does not support it, the rendering may not be as expected

```csharp
IControlWait<T> Spinner(SpinnersType spinnersType, Nullable<Style> SpinnerStyle, Nullable<Int32> speedAnimation, IEnumerable<String> customspinner)
```

#### Parameters

`spinnersType` [SpinnersType](./pplus.controls.spinnerstype.md)<br>
The [SpinnersType](./pplus.controls.spinnerstype.md)

`SpinnerStyle` [Nullable&lt;Style&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Style of spinner. [Style](./pplus.style.md)

`speedAnimation` [Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
Number of mileseconds foreach iteration of spinner. Valid only to SpinnersType.custom, otherwise will be ignored

`customspinner` [IEnumerable&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1)<br>
IEnumerable values for custom spinner. Valid only to SpinnersType.custom, otherwise will be ignored

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-styles"/>**Styles(StyleWait, Style)**

Overwrite Styles Wait. [StyleWait](./pplus.controls.stylewait.md)

```csharp
IControlWait<T> Styles(StyleWait styletype, Style value)
```

#### Parameters

`styletype` [StyleWait](./pplus.controls.stylewait.md)<br>
Styles Wait

`value` [Style](./pplus.style.md)<br>
[Style](./pplus.style.md)

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)

### <a id="methods-tasktitle"/>**TaskTitle(String)**

Overwrite Task Title . Default task title comes from the embedded resource.

```csharp
IControlWait<T> TaskTitle(string value)
```

#### Parameters

`value` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
TaskTitle Task

#### Returns

[IControlWait&lt;T&gt;](./pplus.controls.icontrolwait-1.md)


- - -
[**Back to List Api**](./apis.md)
