# PromptPlus # PipeLine
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultPipe**](resultpipe) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control PipeLine. Pipeline sequence to **all prompts** with **condition by pipe** and status summary.

![](./images/PipeLine.gif)

### Syntax
[**Top**](#promptplus--pipeline)

```csharp
Pipeline()
```
### Methods
[**Top**](#promptplus--pipeline)

- ```csharp
  AddPipe(IFormPlusBase value)
  ``` 
  - Add PromptPlus-Control to pipeline. All controls inherit from IFormPlusBase.
- ```csharp
  AddPipes(IEnumerable<IFormPlusBase> value)
  ``` 
  - Add IEnumerable PromptPlus-Control to pipeline. All controls inherit from IFormPlusBase.  


### Return
[**Top**](#promptplus--pipeline)

```csharp
IControlPipeLine                            //for Control Methods
ResultPromptPlus<IEnumerable<ResultPipe>>   //for Base Method Run, when execution is direct 
IPromptPipe                                 //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                               //for only definition of pipe to Pipeline Control
```


### Sample
[**Top**](#promptplus--pipeline)

```csharp
var pipeline = PromptPlus.Pipeline()
    .AddPipe(PromptPlus.Input("Your first name (empty = skip lastname)")
            .ToPipe(null, "First Name"))
    .AddPipe(PromptPlus.Input("Your last name")
        .PipeCondition((res, context) =>
        {
            return !string.IsNullOrEmpty(((ResultPromptPlus<string>)res[0].ValuePipe).Value);
        })
        .ToPipe(null, "Last Name"))
    .AddPipe(PromptPlus.MaskEdit(MaskedType.DateOnly, "Your birth date")
        .ToPipe(null, "birth date"))
    .AddPipe(
        PromptPlus.WaitProcess("phase 1")
        .AddProcess(new SingleProcess((_stopApp) =>
            {
                _stopApp.WaitHandle.WaitOne(4000);
                if (_stopApp.IsCancellationRequested)
                {
                    return Task.FromResult<object>("canceled");
                }
                return Task.FromResult<object>("Done");
            }))
        .ToPipe(null, "Update phase 1"))
    .AddPipe(PromptPlus.Progressbar("Processing Tasks ")
        .UpdateHandler(UpdateSampleHandlerAsync)
        .ToPipe(null, "Update phase 2"))
    .Run(_stopApp);
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultPipe**](resultpipe) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)



