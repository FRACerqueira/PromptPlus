# PromptPlus # Input
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Input Options**](inputoptions) |
[**BaseOptions**](baseoptions)

## Documentation
Control Input. Generic input with input validator with tooltips.

![](./images/Input.gif)

### Options

[**Input Options**](inputoptions)

### Syntax
[**Top**](#promptplus--input)

```csharp
Input<T>(InputOptions options, CancellationToken? cancellationToken = null)
Input<T>(Action<InputOptions> configure, CancellationToken? cancellationToken = null)
```

```csharp
//Note : The properties are fixed in InputOptions: SwithVisiblePassword = true, IsPassword = false
Input<T>(string message, object defaultValue = null, IList<Func<object, ValidationResult>> validators = null, CancellationToken? cancellationToken = null)
```

**_Note1: defaultValue will be displayed with the expression defaultValue.ToString()._**

**_Note2: If the input is empty and there is a DefaultValue and the all condition from Validators is true, the return will be DefaultValue._**

### Return
[**Top**](#promptplus--input)

```csharp
ResultPromptPlus<T>
```

### Sample
[**Top**](#promptplus--input)

```csharp
var name = PromptPlus.Input<string>("What's your name?", 
             validators: new[] { Validators.Required(), Validators.MinLength(3) }, 
             cancellationToken: _stopApp);
if (name.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, {name.Value}!");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Input Options**](inputoptions) |
[**BaseOptions**](baseoptions)
