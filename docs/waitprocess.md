# PromptPlus # WaitProcess
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResulProcess**](resultprocess) |
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
WaitProcess(WaitProcessOptions options, CancellationToken? cancellationToken = null)
WaitProcess(Action<WaitProcessOptions> configure, CancellationToken? cancellationToken = null)
```

```csharp
//Note : for single process
WaitProcess(string title, SingleProcess process, CancellationToken? cancellationToken = null)
//Note : for many process
WaitProcess(string title, IEnumerable<SingleProcess> process, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- title = Title of process
- process(single process) = class SingleProcess with function that will perform the Task 
- process(many process) = IEnumerable of [**SingleProcess**](singleprocess) will perform the Tasks 

### Return
[**Top**](#promptplus--waitprocess)

```csharp
ResultPromptPlus<IEnumerable<ResultProcess>>
```

### Sample
[**Top**](#promptplus--waitprocess)

```csharp
var process = PromptPlus.WaitProcess("phase 1", new SingleProcess
{
    ProcessToRun = (_stopApp) =>
    {
        _stopApp.WaitHandle.WaitOne(4000);
        if (_stopApp.IsCancellationRequested)
        {
            return Task.FromResult<object>("canceled");
        }
        return Task.FromResult<object>("Done");
    },
}, cancellationToken: _stopApp);
var aux = process.Value.First();
Console.WriteLine($"Result task ({aux.ProcessId}) : {aux.TextResult}. Canceled = {aux.IsCanceled}");
```

```csharp
var Process = PromptPlus.WaitProcess("My Tasks(3) Async", new List<SingleProcess>
{
    new SingleProcess                {
            ProcessId = "Task1",
            ProcessToRun = (_stopApp) =>
            {
                _stopApp.WaitHandle.WaitOne(10000);
                if (_stopApp.IsCancellationRequested)
                {
                return Task.FromResult<object>("canceled");
                }
                return Task.FromResult<object>("Done");
            }
    },
    new SingleProcess
    {
            ProcessId = "Task2",
            ProcessToRun = (_stopApp) =>
            {
                _stopApp.WaitHandle.WaitOne(5000);
                if (_stopApp.IsCancellationRequested)
                {
                return Task.FromResult<object>(-1);
                }
                return Task.FromResult<object>(1);
            }
    },
    new SingleProcess
    {
            ProcessId = "Task3",
            ProcessToRun = (_stopApp) =>
            {
                _stopApp.WaitHandle.WaitOne(7000);
                if (_stopApp.IsCancellationRequested)
                {
                return Task.FromResult<object>("Canceled");
                }
                return Task.FromResult<object>("Done");
            }
    },
}
, cancellationToken: _stopApp);
foreach (var item in Process.Value)
{
    Console.WriteLine($"Result tasks ({item.ProcessId}) : {item.ValueProcess}");
}
```
### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResulProcess**](resultprocess) |
[**WaitProcess Options**](waitprocessoptions) |
[**SingleProcess**](singleprocess) |
[**BaseOptions**](baseoptions)
