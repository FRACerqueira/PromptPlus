# PromptPlus # Input
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)


## Documentation
Control Input. Generic input with input validator with tooltips.

![](./images/Input.gif)

### Syntax
[**Top**](#promptplus--input)

```csharp
Input(string prompt = null)
```

### Methods
[**Top**](#promptplus--input)

- Prompt(string value)
    - set prompt message
- Default(string value)
    - Default value for input. If the input is empty and there is a DefaultValue and the all condition from Validators is true, the return will be DefaultValue
- IsPassword(bool swithVisible)
    - Input is password type.Default Value = false
- Addvalidator(Func<object, ValidationResult> validator);
    - item of input validator. See
- Addvalidators(IEnumerable<Func<object, ValidationResult>> validators)
    - List of input validator

### Return
[**Top**](#promptplus--input)

```csharp
IControlInput                //for Control Methods
IPromptControls<string>      //for others Base Methods
ResultPromptPlus<string>     //for Base Method Run, when execution is direct 
IPromptPipe                  //for Pipe condition 
IFormPlusBase                //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--input)

```csharp
var name = PromptPlus.Input("What's your name?")
    .Default("Peter Parker")
    .Addvalidator(PromptValidators.Required())
    .Addvalidator(PromptValidators.MinLength(3))
    .Run(_stopApp);
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
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
