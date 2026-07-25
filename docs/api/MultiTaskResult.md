<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## MultiTaskResult Struct

Represents the final result of a single task within the MultiTasks control\.

```csharp
public readonly struct MultiTaskResult
```
### Constructors

<a name='PromptPlusLibrary.MultiTaskResult.MultiTaskResult(string,PromptPlusLibrary.MultiTaskState,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception)'></a>

## MultiTaskResult\(string, MultiTaskState, TimeSpan, IReadOnlyDictionary\<string,object\>, Exception\) Constructor

Represents the final result of a single task within the MultiTasks control\.

```csharp
public MultiTaskResult(string title, PromptPlusLibrary.MultiTaskState state, System.TimeSpan elapsedtime, System.Collections.Generic.IReadOnlyDictionary<string,object?>? outputcontext=null, System.Exception? error=null);
```
#### Parameters

<a name='PromptPlusLibrary.MultiTaskResult.MultiTaskResult(string,PromptPlusLibrary.MultiTaskState,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).title'></a>

`title` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The task title\.

<a name='PromptPlusLibrary.MultiTaskResult.MultiTaskResult(string,PromptPlusLibrary.MultiTaskState,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).state'></a>

`state` [MultiTaskState](MultiTaskState.md 'PromptPlusLibrary\.MultiTaskState')

The final [MultiTaskState](MultiTaskState.md 'PromptPlusLibrary\.MultiTaskState')\.

<a name='PromptPlusLibrary.MultiTaskResult.MultiTaskResult(string,PromptPlusLibrary.MultiTaskState,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).elapsedtime'></a>

`elapsedtime` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The task elapsed execution time\.

<a name='PromptPlusLibrary.MultiTaskResult.MultiTaskResult(string,PromptPlusLibrary.MultiTaskState,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).outputcontext'></a>

`outputcontext` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

The isolated output context produced by the task\.

<a name='PromptPlusLibrary.MultiTaskResult.MultiTaskResult(string,PromptPlusLibrary.MultiTaskState,System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).error'></a>

`error` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

The captured exception, if any\.
### Properties

<a name='PromptPlusLibrary.MultiTaskResult.ElapsedTime'></a>

## MultiTaskResult\.ElapsedTime Property

Gets the task elapsed execution time\.

```csharp
public System.TimeSpan ElapsedTime { get; }
```

#### Property Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='PromptPlusLibrary.MultiTaskResult.Exception'></a>

## MultiTaskResult\.Exception Property

Gets the captured exception, if one occurred during execution\.

```csharp
public System.Exception? Exception { get; }
```

#### Property Value
[System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

<a name='PromptPlusLibrary.MultiTaskResult.OutputContext'></a>

## MultiTaskResult\.OutputContext Property

Gets the isolated output context produced by the task\.

```csharp
public System.Collections.Generic.IReadOnlyDictionary<string,object?>? OutputContext { get; }
```

#### Property Value
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

<a name='PromptPlusLibrary.MultiTaskResult.State'></a>

## MultiTaskResult\.State Property

Gets the final task state\.

```csharp
public PromptPlusLibrary.MultiTaskState State { get; }
```

#### Property Value
[MultiTaskState](MultiTaskState.md 'PromptPlusLibrary\.MultiTaskState')

<a name='PromptPlusLibrary.MultiTaskResult.Title'></a>

## MultiTaskResult\.Title Property

Gets the task title\.

```csharp
public string Title { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='PromptPlusLibrary.MultiTaskResult.GetOutput_T_(string,bool)'></a>

## MultiTaskResult\.GetOutput\<T\>\(string, bool\) Method

Tries to read an output context value and cast it to [T](MultiTaskResult.md#PromptPlusLibrary.MultiTaskResult.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.MultiTaskResult\.GetOutput\<T\>\(string, bool\)\.T')\.

```csharp
public T? GetOutput<T>(string key, out bool found);
```
#### Type parameters

<a name='PromptPlusLibrary.MultiTaskResult.GetOutput_T_(string,bool).T'></a>

`T`

Expected value type\.
#### Parameters

<a name='PromptPlusLibrary.MultiTaskResult.GetOutput_T_(string,bool).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Output context key\.

<a name='PromptPlusLibrary.MultiTaskResult.GetOutput_T_(string,bool).found'></a>

`found` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` when the key exists and the value matches [T](MultiTaskResult.md#PromptPlusLibrary.MultiTaskResult.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.MultiTaskResult\.GetOutput\<T\>\(string, bool\)\.T'); otherwise, `false`\.

#### Returns
[T](MultiTaskResult.md#PromptPlusLibrary.MultiTaskResult.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.MultiTaskResult\.GetOutput\<T\>\(string, bool\)\.T')  
The typed value when found; otherwise, `default`\.