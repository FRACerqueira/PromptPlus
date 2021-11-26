# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # WaitProcess
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
[**Top**](#-promptplus--waitprocess)

```csharp
WaitProcess(string prompt, string description = null)
```

### Methods
[**Top**](#-promptplus--waitprocess)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  AddProcess(SingleProcess process)
``` 
  - Add struct-process will perform the Task. See [**SingleProcess**](singleprocess)

- ```csharp
  SpeedAnimation(int value)
``` 
  - Animation speed.If value < 10, value = 10. If value > 1000, value = 1000.

- ```csharp
  Config(Action<IPromptConfig> context)
  ``` 
  - For access [**base methods**](basemethods) common to all controls.

- ```csharp
   PipeCondition(Func<ResultPipe[], object, bool> condition)
  ``` 
  - Set condition to run pipe.

- ```csharp
   ToPipe(string id, string title, object state = null)
  ``` 
  - Transform control to IFormPlusBase.
  - It is mandatory to use this method to use with the Pipeline control. See examples in [**PipeLine Control**](pipeline)

- ```csharp
  ResultPromptPlus<IEnumerable<ResultProcess>> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#-promptplus--waitprocess)

```csharp
IControlWaitProcess                           //for Control Methods
ResultPromptPlus<IEnumerable<ResultProcess>>  //After execute method Run
IPromptPipe                                   //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                                 //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#-promptplus--waitprocess)

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
