# PromptPlus # SliderNumber
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**SliderNumber Options**](slidernumberoptions) |
[**BaseOptions**](baseoptions)

## Documentation
Control SliderNumber. Numeric ranger with short/large step and tooltips.

![](./images/SliderNumber.gif)

### Options

[**SliderNumber Options**](slidernumberoptions)

### Syntax
[**Top**](#promptplus--slidernumber)

```csharp
SliderNumberForm<T>(SliderNumberOptions<T> options)
SliderNumber<T>(Action<SliderNumberOptions<T>> configure, CancellationToken? cancellationToken = null)
```

```csharp
SliderNumber<T>(string message, T value, T min, T max, T shortstep, T? largestep = null, int fracionalDig = 0, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- value = initial value 
- min = minimum value
- max = maximum value
- shortstep = shortstep increment/decrement of value
- largestep = largestep increment/decrement of value. If null value, largestep equal maximum value divide by 10
- fracionalDig = Amount decimals of value

### Return
[**Top**](#promptplus--slidernumber)

```csharp
ResultPromptPlus<T>
```

### Sample
[**Top**](#promptplus--slidernumber)

```csharp
var number = PromptPlus.SliderNumber("Select a number", 5.5, 0, 10, 0.1, fracionalDig: 1, cancellationToken: _stopApp);
if (number.IsAborted)
{
   Return;
}
Console.WriteLine($"Your answer is: {number.Value}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**SliderNumber Options**](slidernumberoptions) |
[**BaseOptions**](baseoptions)
