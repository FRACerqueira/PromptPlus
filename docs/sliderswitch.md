# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # SliderSwitch
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control SliderSwitch. Generic choice with customization and tooltips.

![](./images/SliderSwitch.gif)

### Syntax
[**Top**](#promptplus--sliderswitch)

```csharp
SliderSwitche(string prompt, string description = null)
```

### Methods
[**Top**](#promptplus--sliderswitch)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  Default(double value)
  ``` 
  - initial value

- ```csharp
  OffValue(string value)
``` 
  - value to off state. If ommited, value = OffValue in resx.

- ```csharp
  OnValue(string value)
  ``` 
  - Value to on state. If ommited, value = OnValue in resx.

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
  ResultPromptPlus<bool> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#promptplus--sliderswitch)

```csharp
IControlSliderSwitch       //for Control Methods
ResultPromptPlus<bool>     //After execute Run method
IPromptPipe                //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase              //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--sliderswitch)

```csharp
var slider = PromptPlus.SliderSwitch("Turn on/off")
    .Run(_stopApp);

if (slider.IsAborted)
{
    return;
}
PromptPlus.WriteLine($"Your answer is: {slider.Value}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
