# <img align="left" width="100" height="100" src="../images/icon.png">PromptPlus API:IControlPipeline<T> 

[![Build](https://github.com/FRACerqueira/PromptPlus/workflows/Build/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/build.yml)
[![Publish](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml/badge.svg)](https://github.com/FRACerqueira/PromptPlus/actions/workflows/publish.yml)
[![License](https://img.shields.io/github/license/FRACerqueira/PromptPlus)](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)
[![Downloads](https://img.shields.io/nuget/dt/PromptPlus)](https://www.nuget.org/packages/PromptPlus/)

[**Back to List Api**](./apis.md)

# IControlPipeline&lt;T&gt;

Namespace: PPlus.Controls

Represents the interface with all Methods of the Pipeline control

```csharp
public interface IControlPipeline<T> : IPromptControls<ResultPipeline<T>>
```

#### Type Parameters

`T`<br>
typeof return

Implements IPromptControls&lt;ResultPipeline&lt;T&gt;&gt;

## Methods

### <a id="methods-addpipe"/>**AddPipe(String, Action&lt;EventPipe&lt;T&gt;, CancellationToken&gt;, Func&lt;EventPipe&lt;T&gt;, CancellationToken, Boolean&gt;)**

Add the pipe

```csharp
IControlPipeline<T> AddPipe(string idpipe, Action<EventPipe<T>, CancellationToken> command, Func<EventPipe<T>, CancellationToken, Boolean> condition)
```

#### Parameters

`idpipe` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The unique id to pipe

`command` Action&lt;EventPipe&lt;T&gt;, CancellationToken&gt;<br>
The handler to execute. See [EventPipe&lt;T&gt;](./pplus.controls.eventpipe-1.md) to modified sequence

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Boolean&gt;<br>
The condition to start pipe. If true execute pipe, otherwise goto next pipe. See [EventPipe&lt;T&gt;](./pplus.controls.eventpipe-1.md) to modified sequence

#### Returns

[IControlPipeline&lt;T&gt;](./pplus.controls.icontrolpipeline-1.md)

### <a id="methods-addpipe"/>**AddPipe&lt;TID&gt;(TID, Action&lt;EventPipe&lt;T&gt;, CancellationToken&gt;, Func&lt;EventPipe&lt;T&gt;, CancellationToken, Boolean&gt;)**

Add the pipe by enum ID

```csharp
IControlPipeline<T> AddPipe<TID>(TID idpipe, Action<EventPipe<T>, CancellationToken> command, Func<EventPipe<T>, CancellationToken, Boolean> condition)
```

#### Type Parameters

`TID`<br>

#### Parameters

`idpipe` TID<br>
The unique id to pipe

`command` Action&lt;EventPipe&lt;T&gt;, CancellationToken&gt;<br>
The handler to execute. See [EventPipe&lt;T&gt;](./pplus.controls.eventpipe-1.md) to modified sequence

`condition` Func&lt;EventPipe&lt;T&gt;, CancellationToken, Boolean&gt;<br>
The condition to start pipe. If true execute pipe, otherwise goto next pipe. See [EventPipe&lt;T&gt;](./pplus.controls.eventpipe-1.md) to modified sequence

#### Returns

[IControlPipeline&lt;T&gt;](./pplus.controls.icontrolpipeline-1.md)

### <a id="methods-config"/>**Config(Action&lt;IPromptConfig&gt;)**

Custom config the control.

```csharp
IControlPipeline<T> Config(Action<IPromptConfig> context)
```

#### Parameters

`context` [Action&lt;IPromptConfig&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.action-1)<br>
Action to apply changes. [IPromptConfig](./pplus.controls.ipromptconfig.md)

#### Returns

[IControlPipeline&lt;T&gt;](./pplus.controls.icontrolpipeline-1.md)


- - -
[**Back to List Api**](./apis.md)
