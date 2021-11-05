# PromptPlus # SliderSwitche
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control SliderSwitche. Generic choice with customization and tooltips.

![](./images/SliderSwitche.gif)

### Syntax
[**Top**](#promptplus--sliderswitche)

```csharp
SliderSwitche(string prompt = null)
```

### Methods
[**Top**](#promptplus--sliderswitche)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message 
- ```csharp
  Default(double value)
  ``` 
  - initial value
- ```csharp
  Offvalue(string value)
``` 
  - value to off state. If ommited, value = OffValue in resx.
- ```csharp
  Onvalue(string value)
  ``` 
  - Value to on state. If ommited, value = OnValue in resx.

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
[**Top**](#promptplus--sliderswitche)

```csharp
var slider = PromptPlus.SliderSwitche("Turn on/off")
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
