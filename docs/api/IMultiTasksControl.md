<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## IMultiTasksControl Interface

Provides a fluent API for configuring and running a MultiTasks control that executes
several synchronous or asynchronous tasks \(sequentially or in parallel\), presenting a
paginated execution list with waiting / running / success / failure status indicators\.

```csharp
public interface IMultiTasksControl
```

### Remarks
Each task receives an isolated read\-only input context and returns a separate isolated
output context\. Every configuration method returns the same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')
instance so the calls can be chained \(fluent style\)\. Call [Run\(CancellationToken\)](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Run(System.Threading.CancellationToken) 'PromptPlusLibrary\.IMultiTasksControl\.Run\(System\.Threading\.CancellationToken\)')
last to display the control and block until all tasks finish\.
### Methods

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Action_System.Threading.CancellationToken_,System.Nullable_PromptPlusLibrary.MultiTasksMode_)'></a>

## IMultiTasksControl\.AddTask\(string, Action\<CancellationToken\>, Nullable\<MultiTasksMode\>\) Method

Adds a simple synchronous task with the given title, without input or output context\.

```csharp
PromptPlusLibrary.IMultiTasksControl AddTask(string title, System.Action<System.Threading.CancellationToken> handler, System.Nullable<PromptPlusLibrary.MultiTasksMode> mode=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Action_System.Threading.CancellationToken_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title'></a>

`title` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The task title displayed in the list\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Action_System.Threading.CancellationToken_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler'></a>

`handler` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

The task callback\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Action_System.Threading.CancellationToken_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).mode'></a>

`mode` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[MultiTasksMode](MultiTasksMode.md 'PromptPlusLibrary\.MultiTasksMode')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional execution mode for this task\. When `null`, the default from [Mode\(MultiTasksMode\)](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Mode(PromptPlusLibrary.MultiTasksMode) 'PromptPlusLibrary\.IMultiTasksControl\.Mode\(PromptPlusLibrary\.MultiTasksMode\)') is used\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [title](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Action_System.Threading.CancellationToken_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title 'PromptPlusLibrary\.IMultiTasksControl\.AddTask\(string, System\.Action\<System\.Threading\.CancellationToken\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.title') or [handler](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Action_System.Threading.CancellationToken_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler 'PromptPlusLibrary\.IMultiTasksControl\.AddTask\(string, System\.Action\<System\.Threading\.CancellationToken\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_)'></a>

## IMultiTasksControl\.AddTask\(string, Func\<IReadOnlyDictionary\<string,object\>,CancellationToken,IDictionary\<string,object\>\>, IDictionary\<string,object\>, Nullable\<MultiTasksMode\>\) Method

Adds a synchronous task with the given title, receiving an isolated read\-only input context
and returning an isolated output context \(or `null`\)\.

```csharp
PromptPlusLibrary.IMultiTasksControl AddTask(string title, System.Func<System.Collections.Generic.IReadOnlyDictionary<string,object?>,System.Threading.CancellationToken,System.Collections.Generic.IDictionary<string,object?>?> handler, System.Collections.Generic.IDictionary<string,object?>? context=null, System.Nullable<PromptPlusLibrary.MultiTasksMode> mode=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title'></a>

`title` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The task title displayed in the list\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

The task callback\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).context'></a>

`context` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

Optional isolated input context for this task\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).mode'></a>

`mode` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[MultiTasksMode](MultiTasksMode.md 'PromptPlusLibrary\.MultiTasksMode')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional execution mode for this task\. When `null`, the default from [Mode\(MultiTasksMode\)](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Mode(PromptPlusLibrary.MultiTasksMode) 'PromptPlusLibrary\.IMultiTasksControl\.Mode\(PromptPlusLibrary\.MultiTasksMode\)') is used\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [title](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title 'PromptPlusLibrary\.IMultiTasksControl\.AddTask\(string, System\.Func\<System\.Collections\.Generic\.IReadOnlyDictionary\<string,object\>,System\.Threading\.CancellationToken,System\.Collections\.Generic\.IDictionary\<string,object\>\>, System\.Collections\.Generic\.IDictionary\<string,object\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.title') or [handler](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTask(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Collections.Generic.IDictionary_string,object__,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler 'PromptPlusLibrary\.IMultiTasksControl\.AddTask\(string, System\.Func\<System\.Collections\.Generic\.IReadOnlyDictionary\<string,object\>,System\.Threading\.CancellationToken,System\.Collections\.Generic\.IDictionary\<string,object\>\>, System\.Collections\.Generic\.IDictionary\<string,object\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_)'></a>

## IMultiTasksControl\.AddTaskAsync\(string, Func\<IReadOnlyDictionary\<string,object\>,CancellationToken,Task\<IDictionary\<string,object\>\>\>, IDictionary\<string,object\>, Nullable\<MultiTasksMode\>\) Method

Adds an asynchronous task with the given title, receiving an isolated read\-only input context
and returning an isolated output context \(or `null`\)\.

```csharp
PromptPlusLibrary.IMultiTasksControl AddTaskAsync(string title, System.Func<System.Collections.Generic.IReadOnlyDictionary<string,object?>,System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.IDictionary<string,object?>?>> handler, System.Collections.Generic.IDictionary<string,object?>? context=null, System.Nullable<PromptPlusLibrary.MultiTasksMode> mode=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title'></a>

`title` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The task title displayed in the list\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

The asynchronous task callback\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).context'></a>

`context` [System\.Collections\.Generic\.IDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2 'System\.Collections\.Generic\.IDictionary\`2')

Optional isolated input context for this task\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).mode'></a>

`mode` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[MultiTasksMode](MultiTasksMode.md 'PromptPlusLibrary\.MultiTasksMode')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional execution mode for this task\. When `null`, the default from [Mode\(MultiTasksMode\)](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Mode(PromptPlusLibrary.MultiTasksMode) 'PromptPlusLibrary\.IMultiTasksControl\.Mode\(PromptPlusLibrary\.MultiTasksMode\)') is used\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [title](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title 'PromptPlusLibrary\.IMultiTasksControl\.AddTaskAsync\(string, System\.Func\<System\.Collections\.Generic\.IReadOnlyDictionary\<string,object\>,System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\<System\.Collections\.Generic\.IDictionary\<string,object\>\>\>, System\.Collections\.Generic\.IDictionary\<string,object\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.title') or [handler](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.IDictionary_string,object___,System.Collections.Generic.IDictionary_string,object_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler 'PromptPlusLibrary\.IMultiTasksControl\.AddTaskAsync\(string, System\.Func\<System\.Collections\.Generic\.IReadOnlyDictionary\<string,object\>,System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\<System\.Collections\.Generic\.IDictionary\<string,object\>\>\>, System\.Collections\.Generic\.IDictionary\<string,object\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Nullable_PromptPlusLibrary.MultiTasksMode_)'></a>

## IMultiTasksControl\.AddTaskAsync\(string, Func\<CancellationToken,Task\>, Nullable\<MultiTasksMode\>\) Method

Adds a simple asynchronous task with the given title, without input or output context\.

```csharp
PromptPlusLibrary.IMultiTasksControl AddTaskAsync(string title, System.Func<System.Threading.CancellationToken,System.Threading.Tasks.Task> handler, System.Nullable<PromptPlusLibrary.MultiTasksMode> mode=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title'></a>

`title` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The task title displayed in the list\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler'></a>

`handler` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The asynchronous task callback\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).mode'></a>

`mode` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[MultiTasksMode](MultiTasksMode.md 'PromptPlusLibrary\.MultiTasksMode')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Optional execution mode for this task\. When `null`, the default from [Mode\(MultiTasksMode\)](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Mode(PromptPlusLibrary.MultiTasksMode) 'PromptPlusLibrary\.IMultiTasksControl\.Mode\(PromptPlusLibrary\.MultiTasksMode\)') is used\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [title](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).title 'PromptPlusLibrary\.IMultiTasksControl\.AddTaskAsync\(string, System\.Func\<System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.title') or [handler](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.AddTaskAsync(string,System.Func_System.Threading.CancellationToken,System.Threading.Tasks.Task_,System.Nullable_PromptPlusLibrary.MultiTasksMode_).handler 'PromptPlusLibrary\.IMultiTasksControl\.AddTaskAsync\(string, System\.Func\<System\.Threading\.CancellationToken,System\.Threading\.Tasks\.Task\>, System\.Nullable\<PromptPlusLibrary\.MultiTasksMode\>\)\.handler') is `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.Culture(System.Globalization.CultureInfo)'></a>

## IMultiTasksControl\.Culture\(CultureInfo\) Method

Sets the culture used to format elapsed time values\.

```csharp
PromptPlusLibrary.IMultiTasksControl Culture(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Culture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo') to use\. Cannot be `null`\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Culture(System.Globalization.CultureInfo).culture 'PromptPlusLibrary\.IMultiTasksControl\.Culture\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_)'></a>

## IMultiTasksControl\.Interaction\<T\>\(IEnumerable\<T\>, Action\<T,IMultiTasksControl\>\) Method

Iterates a collection and lets the caller register one or more tasks per item, enabling more
complex scenarios \(mirrors the Interaction pattern used by other controls\)\.

```csharp
PromptPlusLibrary.IMultiTasksControl Interaction<T>(System.Collections.Generic.IEnumerable<T> items, System.Action<T,PromptPlusLibrary.IMultiTasksControl> interactionAction);
```
#### Type parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_).T'></a>

`T`

The item type\.
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_).items'></a>

`items` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_).T 'PromptPlusLibrary\.IMultiTasksControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IMultiTasksControl\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The items to iterate\. Cannot be `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_).interactionAction'></a>

`interactionAction` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[T](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_).T 'PromptPlusLibrary\.IMultiTasksControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IMultiTasksControl\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

A callback receiving each item and this control to register tasks\. Cannot be `null`\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [items](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_).items 'PromptPlusLibrary\.IMultiTasksControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IMultiTasksControl\>\)\.items') or [interactionAction](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Interaction_T_(System.Collections.Generic.IEnumerable_T_,System.Action_T,PromptPlusLibrary.IMultiTasksControl_).interactionAction 'PromptPlusLibrary\.IMultiTasksControl\.Interaction\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Action\<T,PromptPlusLibrary\.IMultiTasksControl\>\)\.interactionAction') is `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.MaxDegreeOfParallelism(int)'></a>

## IMultiTasksControl\.MaxDegreeOfParallelism\(int\) Method

Sets the maximum number of tasks that can run concurrently in [Parallel](MultiTasksMode.md#PromptPlusLibrary.MultiTasksMode.Parallel 'PromptPlusLibrary\.MultiTasksMode\.Parallel') mode\.
The value is clamped to a sensible range based on the available CPU cores\. Use `0` to
auto\-select a value derived from [System\.Environment\.ProcessorCount](https://learn.microsoft.com/en-us/dotnet/api/system.environment.processorcount 'System\.Environment\.ProcessorCount')\.

```csharp
PromptPlusLibrary.IMultiTasksControl MaxDegreeOfParallelism(int value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.MaxDegreeOfParallelism(int).value'></a>

`value` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum degree of parallelism, or `0` to auto\-select\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTasksControl.Mode(PromptPlusLibrary.MultiTasksMode)'></a>

## IMultiTasksControl\.Mode\(MultiTasksMode\) Method

Sets the default execution mode used by tasks that do not specify their own mode\.
Default is [Sequential](MultiTasksMode.md#PromptPlusLibrary.MultiTasksMode.Sequential 'PromptPlusLibrary\.MultiTasksMode\.Sequential')\.

```csharp
PromptPlusLibrary.IMultiTasksControl Mode(PromptPlusLibrary.MultiTasksMode mode);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Mode(PromptPlusLibrary.MultiTasksMode).mode'></a>

`mode` [MultiTasksMode](MultiTasksMode.md 'PromptPlusLibrary\.MultiTasksMode')

The default [MultiTasksMode](MultiTasksMode.md 'PromptPlusLibrary\.MultiTasksMode') to use\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

### Remarks
Tasks execute strictly in the order they were added\. Consecutive tasks that resolve to
[Parallel](MultiTasksMode.md#PromptPlusLibrary.MultiTasksMode.Parallel 'PromptPlusLibrary\.MultiTasksMode\.Parallel') form a sub\-set that runs concurrently; the run only
advances to the next task/sub\-set once every item of the current sub\-set has finished\.
A [Sequential](MultiTasksMode.md#PromptPlusLibrary.MultiTasksMode.Sequential 'PromptPlusLibrary\.MultiTasksMode\.Sequential') task runs on its own before the next one starts\.
The list order is always preserved \(tasks are never reordered/grouped globally by mode\)\.

<a name='PromptPlusLibrary.IMultiTasksControl.Options(System.Action_PromptPlusLibrary.IControlOptions_)'></a>

## IMultiTasksControl\.Options\(Action\<IControlOptions\>\) Method

Applies the shared control options \(such as prompt message, tooltips and abort behavior\)\.

```csharp
PromptPlusLibrary.IMultiTasksControl Options(System.Action<PromptPlusLibrary.IControlOptions> options);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options'></a>

`options` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

A callback used to configure the [IControlOptions](IControlOptions.md 'PromptPlusLibrary\.IControlOptions')\. Cannot be `null`\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [options](IMultiTasksControl.md#PromptPlusLibrary.IMultiTasksControl.Options(System.Action_PromptPlusLibrary.IControlOptions_).options 'PromptPlusLibrary\.IMultiTasksControl\.Options\(System\.Action\<PromptPlusLibrary\.IControlOptions\>\)\.options') is `null`\.

<a name='PromptPlusLibrary.IMultiTasksControl.PageSize(byte)'></a>

## IMultiTasksControl\.PageSize\(byte\) Method

Sets the maximum number of visible task rows per page\. A value of `0` auto\-fits to the console height\.

```csharp
PromptPlusLibrary.IMultiTasksControl PageSize(byte value);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.PageSize(byte).value'></a>

`value` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The desired page size\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTasksControl.Run(System.Threading.CancellationToken)'></a>

## IMultiTasksControl\.Run\(CancellationToken\) Method

Executes all tasks, blocks until they complete or the run is aborted, and returns the final state\.

```csharp
PromptPlusLibrary.ResultPrompt<PromptPlusLibrary.StateMultiTasks> Run(System.Threading.CancellationToken token=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Run(System.Threading.CancellationToken).token'></a>

`token` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') used to cancel the run while it is executing\.

#### Returns
[PromptPlusLibrary\.ResultPrompt&lt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')[StateMultiTasks](StateMultiTasks.md 'PromptPlusLibrary\.StateMultiTasks')[&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>')  
A [ResultPrompt&lt;T&gt;](ResultPrompt_T_.md 'PromptPlusLibrary\.ResultPrompt\<T\>') containing the final [StateMultiTasks](StateMultiTasks.md 'PromptPlusLibrary\.StateMultiTasks')\.

<a name='PromptPlusLibrary.IMultiTasksControl.ShowElapsedTime(bool,string)'></a>

## IMultiTasksControl\.ShowElapsedTime\(bool, string\) Method

Shows the elapsed time next to each task\. Enabled by default\.

```csharp
PromptPlusLibrary.IMultiTasksControl ShowElapsedTime(bool value=true, string? format=null);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.ShowElapsedTime(bool,string).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to display per\-task elapsed time; otherwise, `false`\.

<a name='PromptPlusLibrary.IMultiTasksControl.ShowElapsedTime(bool,string).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Optional [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') format string\. Default is `hh\:mm\:ss`\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTasksControl.Spinner(PromptPlusLibrary.SpinnersType)'></a>

## IMultiTasksControl\.Spinner\(SpinnersType\) Method

Displays an animated spinner in the summary line while at least one task is running\.

```csharp
PromptPlusLibrary.IMultiTasksControl Spinner(PromptPlusLibrary.SpinnersType spinnersType);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Spinner(PromptPlusLibrary.SpinnersType).spinnersType'></a>

`spinnersType` [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType')

The [SpinnersType](SpinnersType.md 'PromptPlusLibrary\.SpinnersType') to display\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTasksControl.StopOnError(bool)'></a>

## IMultiTasksControl\.StopOnError\(bool\) Method

In sequential mode, stops the remaining tasks when a task fails\. Ignored in parallel mode\.

```csharp
PromptPlusLibrary.IMultiTasksControl StopOnError(bool value=true);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.StopOnError(bool).value'></a>

`value` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to stop on the first failure; otherwise, `false`\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.

<a name='PromptPlusLibrary.IMultiTasksControl.Styles(PromptPlusLibrary.MultiTasksStyles,ConsolePlusLibrary.Style)'></a>

## IMultiTasksControl\.Styles\(MultiTasksStyles, Style\) Method

Overrides the visual style applied to a specific region of the MultiTasks control\.

```csharp
PromptPlusLibrary.IMultiTasksControl Styles(PromptPlusLibrary.MultiTasksStyles styleType, ConsolePlusLibrary.Style style);
```
#### Parameters

<a name='PromptPlusLibrary.IMultiTasksControl.Styles(PromptPlusLibrary.MultiTasksStyles,ConsolePlusLibrary.Style).styleType'></a>

`styleType` [MultiTasksStyles](MultiTasksStyles.md 'PromptPlusLibrary\.MultiTasksStyles')

The [MultiTasksStyles](MultiTasksStyles.md 'PromptPlusLibrary\.MultiTasksStyles') region whose style is overridden\.

<a name='PromptPlusLibrary.IMultiTasksControl.Styles(PromptPlusLibrary.MultiTasksStyles,ConsolePlusLibrary.Style).style'></a>

`style` [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style')

The [ConsolePlusLibrary\.Style](https://learn.microsoft.com/en-us/dotnet/api/consolepluslibrary.style 'ConsolePlusLibrary\.Style') to apply\.

#### Returns
[IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl')  
The same [IMultiTasksControl](IMultiTasksControl.md 'PromptPlusLibrary\.IMultiTasksControl') instance for chaining\.