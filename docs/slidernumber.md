# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # SliderNumber
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control SliderNumber. Numeric ranger with short/large step and tooltips.

![](./images/SliderNumber.gif)


### Syntax
[**Top**](#promptplus--slidernumber)

```csharp
SliderNumber( SliderNumberType.UpDown,string prompt, string description = null)
SliderNumber( SliderNumberType.LeftRightstring prompt, string description = null)
```

### Methods
[**Top**](#promptplus--slidernumber)
- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  Default(double value)
  ``` 
  - initial value

- ```csharp
  Range(double minvalue, double maxvalue)
  ``` 
  - Range values

- ```csharp
  Step(double value)
  ``` 
  - shortstep increment/decrement of value

- ```csharp
  LargeStep(double value)
  ``` 
  - largestep increment/decrement of value. If ommited, largestep equal maximum value divide by 10.

- ```csharp
  FracionalDig(int value)
  ``` 
  - precision increment/decrement . If ommited, precison = 0.

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
  ResultPromptPlus<double> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#promptplus--slidernumber)

```csharp
IControlSliderNumber         //for Control Methods
ResultPromptPlus<double>     //After execute Run method
IPromptPipe                  //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--slidernumber)

```csharp
var number = PromptPlus.SliderNumber(SliderNumberType.LeftRight,"Select a number")
    .Default(5.5)
    .Range(0, 10)
    .Step(0.1)
    .FracionalDig(1)
    .Run(_stopApp);

if (number.IsAborted)
{
    return;
}
PromptPlus.WriteLine($"Your answer is: {number.Value}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
