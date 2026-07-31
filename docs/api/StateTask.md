<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## StateTask Struct

Represents the final state of a Task control execution\.

```csharp
public readonly struct StateTask
```
### Constructors

<a name='PromptPlusLibrary.StateTask.StateTask(System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception)'></a>

## StateTask\(TimeSpan, IReadOnlyDictionary\<string,object\>, Exception\) Constructor

Represents the final state of a Task control execution\.

```csharp
public StateTask(System.TimeSpan elapsedtime, System.Collections.Generic.IReadOnlyDictionary<string,object?>? outputcontext=null, System.Exception? error=null);
```
#### Parameters

<a name='PromptPlusLibrary.StateTask.StateTask(System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).elapsedtime'></a>

`elapsedtime` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

Total elapsed execution time\.

<a name='PromptPlusLibrary.StateTask.StateTask(System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).outputcontext'></a>

`outputcontext` [System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')

Isolated output context produced by the task\.

<a name='PromptPlusLibrary.StateTask.StateTask(System.TimeSpan,System.Collections.Generic.IReadOnlyDictionary_string,object_,System.Exception).error'></a>

`error` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

Captured exception, if any\.
### Properties

<a name='PromptPlusLibrary.StateTask.ElapsedTime'></a>

## StateTask\.ElapsedTime Property

Gets the total elapsed time\.

```csharp
public System.TimeSpan ElapsedTime { get; }
```

#### Property Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='PromptPlusLibrary.StateTask.Exception'></a>

## StateTask\.Exception Property

Gets the captured exception, if one occurred during execution\.

```csharp
public System.Exception? Exception { get; }
```

#### Property Value
[System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

<a name='PromptPlusLibrary.StateTask.OutputContext'></a>

## StateTask\.OutputContext Property

Gets the isolated output context produced by the task\.

```csharp
public System.Collections.Generic.IReadOnlyDictionary<string,object?>? OutputContext { get; }
```

#### Property Value
[System\.Collections\.Generic\.IReadOnlyDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlydictionary-2 'System\.Collections\.Generic\.IReadOnlyDictionary\`2')
### Methods

<a name='PromptPlusLibrary.StateTask.GetOutput_T_(string,bool)'></a>

## StateTask\.GetOutput\<T\>\(string, bool\) Method

Tries to read an output context value and cast it to [T](StateTask.md#PromptPlusLibrary.StateTask.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.StateTask\.GetOutput\<T\>\(string, bool\)\.T')\.

```csharp
public T? GetOutput<T>(string key, out bool found);
```
#### Type parameters

<a name='PromptPlusLibrary.StateTask.GetOutput_T_(string,bool).T'></a>

`T`

Expected value type\.
#### Parameters

<a name='PromptPlusLibrary.StateTask.GetOutput_T_(string,bool).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Output context key\.

<a name='PromptPlusLibrary.StateTask.GetOutput_T_(string,bool).found'></a>

`found` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` when the key exists and the value matches [T](StateTask.md#PromptPlusLibrary.StateTask.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.StateTask\.GetOutput\<T\>\(string, bool\)\.T'); otherwise, `false`\.

#### Returns
[T](StateTask.md#PromptPlusLibrary.StateTask.GetOutput_T_(string,bool).T 'PromptPlusLibrary\.StateTask\.GetOutput\<T\>\(string, bool\)\.T')  
The typed value when found; otherwise, `default`\.