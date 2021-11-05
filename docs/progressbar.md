# PromptPlus # ProgressBar
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
Progressbar(string prompt = null)
```

### Methods
[**Top**](#promptplus--progressbar)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message 
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

### Return
[**Top**](#promptplus--progressbar)

```csharp
IControlProgressbar                 //for Control Methods
IPromptControls<ProgressBarInfo>    //for others Base Methods
ResultPromptPlus<ProgressBarInfo>   //for Base Method Run, when execution is direct 
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
