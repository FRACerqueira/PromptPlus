# PromptPlus # WaitProcess
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**SingleProcess**](singleprocess) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control WaitProcess. Wait process with animation.

![](./images/WaitProcess.gif)

### Syntax
[**Top**](#promptplus--waitprocess)

```csharp
WaitProcess(string prompt = null)
```

### Methods
[**Top**](#promptplus--sliderswitche)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message
- ```csharp
  AddProcess(SingleProcess process)
``` 
  - Add struct-process will perform the Task. See [**SingleProcess**](singleprocess)
- ```csharp
  SpeedAnimation(int value)
``` 
  - Animation speed.If value < 10, value = 10. If value > 1000, value = 1000.

### Return
[**Top**](#promptplus--sliderswitche)

```csharp
IControlSliderSwitche      //for Control Methods
IPromptControls<bool>      //for others Base Methods
ResultPromptPlus<bool>     //for Base Method Run, when execution is direct 
IPromptPipe                //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase              //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--waitprocess)

```csharp
var Process = PromptPlus.WaitProcess("My Tasks(3) Async")
        .AddProcess(new SingleProcess((_stopApp) =>
            {
                _stopApp.WaitHandle.WaitOne(10000);
                if (_stopApp.IsCancellationRequested)
                {
                    return Task.FromResult<object>("canceled");
                }
                return Task.FromResult<object>("Done");
            }, "Task1"))
        .AddProcess(new SingleProcess((_stopApp) =>
            {
                _stopApp.WaitHandle.WaitOne(5000);
                if (_stopApp.IsCancellationRequested)
                {
                    return Task.FromResult<object>(-1);
                }
                return Task.FromResult<object>(1);
            }, "Task2"))
        .AddProcess(new SingleProcess((_stopApp) =>
            {
                _stopApp.WaitHandle.WaitOne(7000);
                if (_stopApp.IsCancellationRequested)
                {
                    return Task.FromResult<object>("Canceled");
                }
                return Task.FromResult<object>("Done");
            }, "Task3"))
        .Run(_stopApp);

foreach (var item in Process.Value)
{
    PromptPlus.WriteLine($"Result tasks ({item.ProcessId}) : {item.ValueProcess}");
}
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**SingleProcess**](singleprocess) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
