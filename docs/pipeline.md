# PromptPlus # PipeLine
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultPipe**](resultpipe) |
[**Step Extension**](pipelinestep) 

## Documentation
Control PipeLine. Pipeline sequence to **all prompts** with **condition by pipe** and status summary.

![](./images/PipeLine.gif)

### Options

Not have options

### Syntax
[**Top**](#promptplus--pipeline)

```csharp
Pipeline(IList<IFormPlusBase> steps, CancellationToken? cancellationToken = null)
```

**_Note1: All controls inherit from IFormPlusBase. All controls that inherit from IFormPlusBase are in the namespece PromptPlus.Pipe_**

**_Note2: It is mandatory to tell the control to use the Step extension to define a pipe for a contro´s Pipeline. See examples below!_**

### Return
[**Top**](#promptplus--input)

```csharp
ResultPromptPlus<IEnumerable<ResultPipe>>
```

### Sample
[**Top**](#promptplus--input)

```csharp
var steps = new List<IFormPlusBase>
{
    PromptPlus.Pipe.Input(new InputOptions { Message = "Your first name (empty = skip lastname)" })
    .Step("First Name"),

    PromptPlus.Pipe.Input(new InputOptions { Message = "Your last name" })
    .Step("Last Name",(res,context) =>
    {
        return !string.IsNullOrEmpty( ((ResultPromptPlus<string>)res[0].ValuePipe).Value);
    }),

    PromptPlus.Pipe.MaskEdit(PromptPlus.MaskTypeDateOnly, "Your birth date",cancellationToken: _stopApp)
    .Step("birth date"),

    PromptPlus.Pipe.WaitProcess("phase 1", new SingleProcess{ ProcessToRun = (_stopApp) =>
    {
        _stopApp.WaitHandle.WaitOne(4000);
        if (_stopApp.IsCancellationRequested)
        {
            return Task.FromResult<object>("canceled");
        }
        return Task.FromResult<object>("Done");
    } }).Step("Update phase 1"),

    PromptPlus.Pipe.Progressbar("Processing Tasks ",  UpdateSampleHandlerAsync)
    .Step("Update phase 2")
};
var pipeline = PromptPlus.Pipeline(steps, _stopApp);
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**ResultPipe**](resultpipe) |
[**Step Extension**](pipelinestep) 

