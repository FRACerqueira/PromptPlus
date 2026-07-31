<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## MultiTaskState Enum

Represents the execution state of a single task in the MultiTasks control\.

```csharp
public enum MultiTaskState
```
### Fields

<a name='PromptPlusLibrary.MultiTaskState.Waiting'></a>

`Waiting` 0

The task is waiting to be executed\.

<a name='PromptPlusLibrary.MultiTaskState.Running'></a>

`Running` 1

The task is currently running\.

<a name='PromptPlusLibrary.MultiTaskState.Success'></a>

`Success` 2

The task finished successfully\.

<a name='PromptPlusLibrary.MultiTaskState.Failed'></a>

`Failed` 3

The task finished with an error\.