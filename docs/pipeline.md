# PromptPlus # PipeLine
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
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
Pipeline(IList<IFormPPlusBase> steps, CancellationToken? cancellationToken = null)
```

**_Note1: All controls inherit from IPromptbase_**

**_Note2: All controls that inherit from IFormPPlusBase are in the namespece PPlus.Pipe_**

**_Note3: It is mandatory to tell the control to use the Step extension to define a pipe for a contro´s Pipeline. See examples below!_**

### Return
[**Top**](#promptplus--input)

```csharp
ResultPPlus<IEnumerable<ResultPipe>>
```

### Sample
[**Top**](#promptplus--input)

```csharp
var steps = new List<IFormPPlusBase>
{
    PPlus.Pipe.Input<string>(new InputOptions { Message = "Your first name (empty = skip lastname)" })
    .Step("First Name"),

    PPlus.Pipe.Input<string>(new InputOptions { Message = "Your last name" })
    .Step("Last Name",(res,context) =>
    {
        return !string.IsNullOrEmpty( ((ResultPPlus<string>)res[0].ValuePipe).Value);
    }),

    PPlus.Pipe.MaskEdit(PPlus.MaskTypeDateOnly, "Your birth date",cancellationToken: _stopApp)
    .Step("birth date"),

    PPlus.Pipe.Progressbar("Processing Tasks ",  UpdateSampleHandlerAsync, 30)
    .Step("Update")
};
var pipiline = PPlus.Pipeline(steps, _stopApp);
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**ResultPipe**](resultpipe) |
[**Step Extension**](pipelinestep) 

