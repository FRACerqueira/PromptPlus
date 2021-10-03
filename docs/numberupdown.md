# PromptPlus # NumberUpDown
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**SliderNumber Options**](slidernumberoptions) |
[**BasePromptOptions**](basepromptoptions)

## Documentation
Control NumberUpDown. Numeric ranger with step and tooltips.

![](./images/NumberUpDown.gif)

### Options

[**SliderNumber Options**](slidernumberoptions)

### Syntax
[**Top**](#promptplus--numberupdown)

```csharp
NumberUpDown<T>(SliderNumberOptions<T> options, CancellationToken? cancellationToken = null)
NumberUpDown<T>(Action<SliderNumberOptions<T>> configure, CancellationToken? cancellationToken = null)
```

```csharp
NumberUpDown<T>(string message, T value, T min, T max, T step, int fracionalDig = 0, CancellationToken? cancellationToken = null)
```

**Highlighted parameters**
- value = initial value 
- min = minimum value
- max = maximum value
- step = step increment/decrement of value
- fracionalDig = Amount decimals of value

### Return
[**Top**](#promptplus--numberupdown)


```csharp
ResultPPlus<T>
```

### Sample
[**Top**](#promptplus--numberupdown)

```csharp
var number = PPlus.NumberUpDown("Select a number", 5.5, 0, 10, 0.1, fracionalDig: 1, cancellationToken: _stopApp);
if (number.IsAborted)
{
   return;
}
Console.WriteLine($"Your answer is: {number.Value}");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPPlus**](resultpplus) |
[**SliderNumber Options**](slidernumberoptions) |
[**BasePromptOptions**](basepromptoptions)

