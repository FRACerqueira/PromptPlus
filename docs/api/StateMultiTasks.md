<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## StateMultiTasks Struct

Represents the final state of a MultiTasks control execution\.

```csharp
public readonly struct StateMultiTasks
```
### Constructors

<a name='PromptPlusLibrary.StateMultiTasks.StateMultiTasks(System.TimeSpan,System.Collections.Generic.IReadOnlyList_PromptPlusLibrary.MultiTaskResult_,bool)'></a>

## StateMultiTasks\(TimeSpan, IReadOnlyList\<MultiTaskResult\>, bool\) Constructor

Represents the final state of a MultiTasks control execution\.

```csharp
public StateMultiTasks(System.TimeSpan elapsedtime, System.Collections.Generic.IReadOnlyList<PromptPlusLibrary.MultiTaskResult> results, bool aborted);
```
#### Parameters

<a name='PromptPlusLibrary.StateMultiTasks.StateMultiTasks(System.TimeSpan,System.Collections.Generic.IReadOnlyList_PromptPlusLibrary.MultiTaskResult_,bool).elapsedtime'></a>

`elapsedtime` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

Total elapsed execution time of the whole run\.

<a name='PromptPlusLibrary.StateMultiTasks.StateMultiTasks(System.TimeSpan,System.Collections.Generic.IReadOnlyList_PromptPlusLibrary.MultiTaskResult_,bool).results'></a>

`results` [System\.Collections\.Generic\.IReadOnlyList&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1 'System\.Collections\.Generic\.IReadOnlyList\`1')[MultiTaskResult](MultiTaskResult.md 'PromptPlusLibrary\.MultiTaskResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1 'System\.Collections\.Generic\.IReadOnlyList\`1')

The per\-task results\.

<a name='PromptPlusLibrary.StateMultiTasks.StateMultiTasks(System.TimeSpan,System.Collections.Generic.IReadOnlyList_PromptPlusLibrary.MultiTaskResult_,bool).aborted'></a>

`aborted` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the run was aborted\.
### Properties

<a name='PromptPlusLibrary.StateMultiTasks.Aborted'></a>

## StateMultiTasks\.Aborted Property

Gets whether the run was aborted before all tasks finished\.

```csharp
public bool Aborted { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.StateMultiTasks.AllSucceeded'></a>

## StateMultiTasks\.AllSucceeded Property

Gets whether every task finished successfully\.

```csharp
public bool AllSucceeded { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.StateMultiTasks.AnyFailed'></a>

## StateMultiTasks\.AnyFailed Property

Gets whether at least one task finished with a failure\.

```csharp
public bool AnyFailed { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='PromptPlusLibrary.StateMultiTasks.ElapsedTime'></a>

## StateMultiTasks\.ElapsedTime Property

Gets the total elapsed execution time of the whole run\.

```csharp
public System.TimeSpan ElapsedTime { get; }
```

#### Property Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='PromptPlusLibrary.StateMultiTasks.Results'></a>

## StateMultiTasks\.Results Property

Gets the per\-task results\. Never `null`, even for a `default` instance\.

```csharp
public System.Collections.Generic.IReadOnlyList<PromptPlusLibrary.MultiTaskResult> Results { get; }
```

#### Property Value
[System\.Collections\.Generic\.IReadOnlyList&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1 'System\.Collections\.Generic\.IReadOnlyList\`1')[MultiTaskResult](MultiTaskResult.md 'PromptPlusLibrary\.MultiTaskResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlylist-1 'System\.Collections\.Generic\.IReadOnlyList\`1')