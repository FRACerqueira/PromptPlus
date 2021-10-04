# PromptPlus # WaitProcess
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**WaitProcess Options**](waitprocessoptions) |
[**SingleProcess**](singleprocess) |
[**BaseOptions**](baseoptions)

## Documentation
Control WaitProcess. Wait process with animation.

![](./images/WaitProcess.gif)

### Options

[**WaitProcess Options**](waitprocess)

### Syntax
[**Top**](#promptplus--waitprocess)

```csharp
WaitProcess<T>(WaitProcessOptions<T> options, CancellationToken? cancellationToken = null)
WaitProcess<T>(Action<WaitProcessOptions<T>> configure, CancellationToken? cancellationToken = null)
```

```csharp
//Note : for single process
WaitProcess<T>(string title, Func<Task<T>> process, Func<T, string> processTextResult = null, CancellationToken? cancellationToken = null)
//Note : for many process
WaitProcess<T>(string title, IEnumerable<SingleProcess<T>> process, Func<T, string> processTextResult = null, CancellationToken? cancellationToken = null)
```

**_Note: for many process all process must by same T´s type._** 


**Highlighted parameters**
- title = Title of process
- process(single process) = function that will perform the Task 
- process(many process) = IEnumerable of [**SingleProcess**](singleprocess) will perform the Task 
- processTextResult = Function that returns the string that will be displayed.Tf the value is null,the value will be item => item.ToString()

### Return
[**Top**](#promptplus--waitprocess)

```csharp
ResultPromptPlus<IEnumerable<T>>
```

### Sample
[**Top**](#promptplus--waitprocess)

```csharp
var progress = PromptPlus.WaitProcess("My Task", async () =>
    {
        await Task.Delay(10000);
        return "Done";
    }, cancellationToken: _stopApp);
if (progress.IsAborted)
{
    Console.WriteLine($"Your task aborted.");
}
```

```csharp
var progress = PromptPlus.WaitProcess("My Tasks(3) Async", new List<SingleProcess<string>>
{
    new SingleProcess<string>
    {
         ProcessId = "Task1",
         ProcessToRun = async () =>
         {
             await Task.Delay(10000);
             return "Done";
         }
    },
    new SingleProcess<string>
    {
         ProcessId = "Task2",
         ProcessToRun = async () =>
         {
             await Task.Delay(5000);
             return "Done";
         }
    },
    new SingleProcess<string>
    {
         ProcessId = "Task3",
         ProcessToRun = async () =>
         {
             await Task.Delay(7000);
             return "Done";
         }
    },
}
, cancellationToken: _stopApp);
if (progress.IsAborted)
{
    return;
}
```
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**WaitProcess Options**](waitprocessoptions) |
[**SingleProcess**](singleprocess) |
[**BaseOptions**](baseoptions)
