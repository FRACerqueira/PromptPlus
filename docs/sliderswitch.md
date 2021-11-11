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
SliderSwitche(string prompt = null)
```

### Methods
[**Top**](#promptplus--sliderswitch)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message 
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

### Return
[**Top**](#promptplus--sliderswitch)

```csharp
IControlSliderSwitch       //for Control Methods
IPromptControls<bool>      //for others Base Methods
ResultPromptPlus<bool>     //for Base Method Run, when execution is direct 
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
