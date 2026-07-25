<img src="https://raw.githubusercontent.com/FRACerqueira/PromptPlus/main/icon.png" width="120" alt="PromptPlus" />

#### [PromptPlus](PromptPlus.md 'PromptPlus')
### [PromptPlusLibrary](PromptPlusLibrary.md 'PromptPlusLibrary')

## MultiTasksMode Enum

Defines how a set of MultiTasks are executed\.

```csharp
public enum MultiTasksMode
```
### Fields

<a name='PromptPlusLibrary.MultiTasksMode.Sequential'></a>

`Sequential` 0

Tasks are executed one after another, in the order they were added\.

<a name='PromptPlusLibrary.MultiTasksMode.Parallel'></a>

`Parallel` 1

Tasks are executed concurrently \(in parallel\)\.