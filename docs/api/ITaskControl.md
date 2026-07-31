<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## ITaskControl Interface

Provides a fluent API for configuring and running a Task control that executes a
synchronous or asynchronous action/function and waits for it to complete, optionally
displaying the elapsed time and an animated spinner\.

```csharp
public interface ITaskControl
```

### Remarks
The task receives an isolated input context \([System\.Collections\.Generic\.IDictionary&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')\) and
produces a separate isolated output context\. Both dictionaries are independent from each
other\. Every configuration method returns the same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance so
the calls can be chained \(fluent style\)\. Call [Run\(CancellationToken\)](ITaskControl.md#PromptPlusLibrary.ITaskControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.ITaskControl\.Run\(System\.Threading\.CancellationToken\)') last to
display the control and block until the task finishes\.
### Methods

<a name='PromptPlusLibrary.ITaskControl.Action(System.Action_System.Threading.CancellationToken_)'></a>

## ITaskControl\.Action\(Action\<CancellationToken\>\) Method

Sets a simple synchronous action to execute without input or output context\.

```csharp
PromptPlusLibrary.ITaskControl Action(System.Action<System.Threading.CancellationToken> handler);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Action(System.Action_System.Threading.CancellationToken_).handler'></a>

`handler` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback receiving a [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [handler](ITaskControl.md#PromptPlusLibrary.ITaskControl.Action(System.Action_System.Threading.CancellationToken_).handler 'PromptPlusLibrary\.ITaskControl\.Action\(System\.Action\<System\.Threading\.CancellationToken\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.Action(System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__)'></a>

## ITaskControl\.Action\(Func\<IReadOnlyDictionary\<string,object\>,CancellationToken,IDictionary\<string,object\>\>\) Method

Sets the synchronous action to execute\. The action receives an isolated read\-only input
context and returns an isolated output context \(or `null`\) that is exposed through
[OutputContext](StateTask.md#PromptPlusLibrary.StateTask.OutputContext 'PromptPlusLibrary\.StateTask\.OutputContext')\.

```csharp
PromptPlusLibrary.ITaskControl Action(System.Func<System.Collections.Generic.IReadOnlyDictionary<string,object?>,System.Threading.CancellationToken,System.Collections.Generic.IDictionary<string,object?>?> handler);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Action(System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

A callback receiving the input context and a [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') and returning
the output context\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [handler](ITaskControl.md#PromptPlusLibrary.ITaskControl.Action(System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__).handler 'PromptPlusLibrary\.ITaskControl\.Action\(System\.Func\<System\.Collections\.Generic\.IReadOnlyDictionary\<string,object\>,System\.Threading\.CancellationToken,System\.Collections\.Generic\.IDictionary\<string,object\>\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.Action(System.Func_System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__)'></a>

## ITaskControl\.Action\(Func\<CancellationToken,IDictionary\<string,object\>\>\) Method

Sets the synchronous action to execute without an input context\. Returns an isolated
output context \(or `null`\) that is exposed through [OutputContext](StateTask.md#PromptPlusLibrary.StateTask.OutputContext 'PromptPlusLibrary\.StateTask\.OutputContext')\.

```csharp
PromptPlusLibrary.ITaskControl Action(System.Func<System.Threading.CancellationToken,System.Collections.Generic.IDictionary<string,object?>?> handler);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Action(System.Func_System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A callback receiving a [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') and returning the output context\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [handler](ITaskControl.md#PromptPlusLibrary.ITaskControl.Action(System.Func_System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__).handler 'PromptPlusLibrary\.ITaskControl\.Action\(System\.Func\<System\.Threading\.CancellationToken,System\.Collections\.Generic\.IDictionary\<string,object\>\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___)'></a>

## ITaskControl\.ActionAsync\(Func\<IReadOnlyDictionary\<string,object\>,CancellationToken,Task\<IDictionary\<string,object\>\>\>\) Method

Sets the asynchronous function to execute\. The function receives an isolated read\-only input
context and returns an isolated output context \(or `null`\) that is exposed through
[OutputContext](StateTask.md#PromptPlusLibrary.StateTask.OutputContext 'PromptPlusLibrary\.StateTask\.OutputContext')\.

```csharp
PromptPlusLibrary.ITaskControl ActionAsync(System.Func<System.Collections.Generic.IReadOnlyDictionary<string,object?>,System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string,object?>?>> handler);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

A callback receiving the input context and a [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') and returning a
[System\.Threading\.Tasks\.Task&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1') with the output context\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [handler](ITaskControl.md#PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___).handler 'PromptPlusLibrary\.ITaskControl\.ActionAsync\(System\.Func\<System\.Collections\.Generic\.IReadOnlyDictionary\<string,object\>,System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\<System\.Collections\.Generic\.IDictionary\<string,object\>\>\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___)'></a>

## ITaskControl\.ActionAsync\(Func\<CancellationToken,Task\<IDictionary\<string,object\>\>\>\) Method

Sets the asynchronous function to execute without an input context\. Returns an isolated
output context \(or `null`\) that is exposed through [OutputContext](StateTask.md#PromptPlusLibrary.StateTask.OutputContext 'PromptPlusLibrary\.StateTask\.OutputContext')\.

```csharp
PromptPlusLibrary.ITaskControl ActionAsync(System.Func<System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string,object?>?>> handler);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A callback receiving a [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') and returning a [System\.Threading\.Tasks\.Task&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')
with the output context\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [handler](ITaskControl.md#PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___).handler 'PromptPlusLibrary\.ITaskControl\.ActionAsync\(System\.Func\<System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\<System\.Collections\.Generic\.IDictionary\<string,object\>\>\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_)'></a>

## ITaskControl\.ActionAsync\(Func\<CancellationToken,Task\>\) Method

Sets a simple asynchronous function to execute without input or output context\.

```csharp
PromptPlusLibrary.ITaskControl ActionAsync(System.Func<System.Threading.CancellationToken,System.Threading.Tasks.Task> handler);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A callback receiving a [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') and returning a [System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [handler](ITaskControl.md#PromptPlusLibrary.ITaskControl.ActionAsync(System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_).handler 'PromptPlusLibrary\.ITaskControl\.ActionAsync\(System\.Func\<System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.ChangeDescription(System.Func_System.TimeSpan,string_)'></a>

## ITaskControl\.ChangeDescription\(Func\<TimeSpan,string\>\) Method

Dynamically changes the description of the control based on the elapsed time while the task runs\.

```csharp
PromptPlusLibrary.ITaskControl ChangeDescription(System.Func<System.TimeSpan,string> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.ChangeDescription(System.Func_System.TimeSpan,string_).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the elapsed time and returns the description to display\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITaskControl.md#PromptPlusLibrary.ITaskControl.ChangeDescription(System.Func_System.TimeSpan,string_).value 'PromptPlusLibrary\.ITaskControl\.ChangeDescription\(System\.Func\<System\.TimeSpan,string\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.ChangeDescriptionAsync(System.Func_System.TimeSpan,System.Threading.Tasks.Task_string__)'></a>

## ITaskControl\.ChangeDescriptionAsync\(Func\<TimeSpan,Task\<string\>\>\) Method

Asynchronous version of [ChangeDescription\(Func&lt;TimeSpan,string&gt;\)](ITaskControl.md#PromptPlusLibrary.ITaskControl.ChangeDescription(System.Func_System.TimeSpan,string_) 'PromptPlusLibrary\.ITaskControl\.ChangeDescription\(System\.Func\<System\.TimeSpan,string\>\)') that updates the
description text according to the elapsed time \(useful when the text comes from an asynchronous source\)\.

```csharp
PromptPlusLibrary.ITaskControl ChangeDescriptionAsync(System.Func<System.TimeSpan,System.Threading.Tasks.Task<string>> value);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.ChangeDescriptionAsync(System.Func_System.TimeSpan,System.Threading.Tasks.Task_string__).value'></a>

`value` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that receives the elapsed time and asynchronously returns the description\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [value](ITaskControl.md#PromptPlusLibrary.ITaskControl.ChangeDescriptionAsync(System.Func_System.TimeSpan,System.Threading.Tasks.Task_string__).value 'PromptPlusLibrary\.ITaskControl\.ChangeDescriptionAsync\(System\.Func\<System\.TimeSpan,System\.Threading\.Tasks\.Task\<string\>\>\)\.value') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.Context(System.Collections.Generic.IDictionary_string,object_)'></a>

## ITaskControl\.Context\(IDictionary\<string,object\>\) Method

Provides the isolated input context passed to the task handler\.

```csharp
PromptPlusLibrary.ITaskControl Context(System.Collections.Generic.IDictionary<string,object?> context);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Context(System.Collections.Generic.IDictionary_string,object_).context'></a>

`context` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

The input context dictionary\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [context](ITaskControl.md#PromptPlusLibrary.ITaskControl.Context(System.Collections.Generic.IDictionary_string,object_).context 'PromptPlusLibrary\.ITaskControl\.Context\(System\.Collections\.Generic\.IDictionary\<string,object\>\)\.context') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.Culture(System.Globalization.CultureInfo)'></a>

## ITaskControl\.Culture\(CultureInfo\) Method

Sets the culture used to format the elapsed time value\.

```csharp
PromptPlusLibrary.ITaskControl Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](ITaskControl.md#PromptPlusLibrary.ITaskControl.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.ITaskControl\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.Finish(string,string)'></a>

## ITaskControl\.Finish\(string, string\) Method

Sets the text displayed when the task finishes\. When not set, the elapsed time is shown\.

```csharp
PromptPlusLibrary.ITaskControl Finish(string finishtext, string? errortext=null);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Finish(string,string).finishtext'></a>

`finishtext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to display when the task finishes successfully\.

<a name='PromptPlusLibrary.ITaskControl.Finish(string,string).errortext'></a>

`errortext` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional text to display when the task finishes with an error\. When `null`, a default
localized error message is shown\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

<a name='PromptPlusLibrary.ITaskControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## ITaskControl\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(such as prompt message, tooltips and abort behavior\)\.

```csharp
PromptPlusLibrary.ITaskControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](ITaskControl.md#PromptPlusLibrary.ITaskControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.ITaskControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.ITaskControl.Run(System.Threading.CancellationToken)'></a>

## ITaskControl\.Run\(CancellationToken\) Method

Executes the task, blocks until it completes or is aborted, and returns its final state\.

```csharp
PromptPlusLibrary.ResultPrompt<PromptPlusLibrary.StateTask> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the task while it is running\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[StateTask](StateTask.md 'PromptPlusLibrary\.StateTask')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the final [StateTask](StateTask.md 'PromptPlusLibrary\.StateTask')\.

<a name='PromptPlusLibrary.ITaskControl.ShowElapsedTime(bool,string)'></a>

## ITaskControl\.ShowElapsedTime\(bool, string\) Method

Shows the elapsed time while the task is running\. Hidden by default\.

```csharp
PromptPlusLibrary.ITaskControl ShowElapsedTime(bool value=true, string? format=null);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.ShowElapsedTime(bool,string).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to display the elapsed time; otherwise, `false`\.

<a name='PromptPlusLibrary.ITaskControl.ShowElapsedTime(bool,string).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') format string\. Default is `hh\:mm\:ss`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

<a name='PromptPlusLibrary.ITaskControl.Spinner(PromptPlusLibrary.SpinnersType)'></a>

## ITaskControl\.Spinner\(SpinnersType\) Method

Displays an animated spinner while the task is running\.

```csharp
PromptPlusLibrary.ITaskControl Spinner(PromptPlusLibrary.SpinnersType spinnersType);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Spinner(PromptPlusLibrary.SpinnersType).spinnersType'></a>

`spinnersType` [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType')

The [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType') to display\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

<a name='PromptPlusLibrary.ITaskControl.Styles(PromptPlusLibrary.TaskStyles,ConsolePlusLibrary.Style)'></a>

## ITaskControl\.Styles\(TaskStyles, Style\) Method

Overrides the visual style applied to a specific region of the Task control\.

```csharp
PromptPlusLibrary.ITaskControl Styles(PromptPlusLibrary.TaskStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.ITaskControl.Styles(PromptPlusLibrary.TaskStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [TaskStyles](TaskStyles.md 'PromptPlusLibrary\.TaskStyles')

The [TaskStyles](TaskStyles.md 'PromptPlusLibrary\.TaskStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.ITaskControl.Styles(PromptPlusLibrary.TaskStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\. Cannot be `null`\.

#### Returns
[ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl')  
The same [ITaskControl](ITaskControl.md 'PromptPlusLibrary\.ITaskControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [style](ITaskControl.md#PromptPlusLibrary.ITaskControl.Styles(PromptPlusLibrary.TaskStyles,ConsolePlusLibrary.Style).style 'PromptPlusLibrary\.ITaskControl\.Styles\(PromptPlusLibrary\.TaskStyles, ConsolePlusLibrary\.Style\)\.style') is `null`\.