# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # ProgressBar
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ProgressBarInfo**](progressbarinfo) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control ProgressBar. Progress Bar with interation customization.

![](./images/ProgressBar.gif)

### Syntax
[**Top**](#promptplus--progressbar)

```csharp
Progressbar(string prompt, string description = null)
```

### Methods
[**Top**](#promptplus--progressbar)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  Width(int value)
  ``` 
  - Width bar. If the ommited Width = 100. If Width < 30, Width = 30.  If Width > 200, Width = 200

- ```csharp
  UpdateHandler(Func<ProgressBarInfo, CancellationToken, Task<ProgressBarInfo>> value)
  ``` 
    - function that will be performed for each interaction

- ```csharp
  StartInterationId(object value)
  ``` 
    - Identification start interaction. If ommited , value = 0 (int)

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
  - It is mandatory to use with the Pipeline control. See examples in [**PipeLine Control**](pipeline)

- ```csharp
  ResultPromptPlus<ProgressBarInfo> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#promptplus--progressbar)

```csharp
IControlProgressbar                 //for Control Methods
ResultPromptPlus<ProgressBarInfo>   //After execute Run method
IPromptPipe                         //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                       //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--progressbar)

```csharp
private async Task<ProgressBarInfo> UpdateSampleHandlerAsync(ProgressBarInfo status, CancellationToken cancellationToken)
{
    await Task.Delay(10);
    var aux = (int)status.InterationId + 1;
    var endupdate = true;
    if (aux < 100)
    {
        endupdate = false;
    }
    return new ProgressBarInfo(aux, endupdate, $"Interation {aux}", aux);
}
```

```csharp
var progress = PromptPlus.Progressbar("Processing Tasks")
    .UpdateHandler(UpdateSampleHandlerAsync)
    .Run(_stopApp);

if (progress.IsAborted)
{
    PromptPlus.WriteLine($"Your result is: {progress.Value.Message} Canceled!");
    return;
}
PromptPlus.WriteLine($"Your result is: {progress.Value.Message}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ProgressBarInfo**](progressbarinfo) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
