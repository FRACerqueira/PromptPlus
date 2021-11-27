# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Input
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)


## Documentation
Control Input. Generic input with input validator with tooltips.

![](./images/Input.gif)

### Syntax
[**Top**](#-promptplus--input)

```csharp
Input(string prompt, string description = null)
```

### Methods
[**Top**](#-promptplus--input)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  InitialValue(string value)
  ``` 
  -  Initial value for input.If IsPassword = true , the InitValue method will be ignored.

- ```csharp
  Default(string value)
  ``` 
  - Default value for input. If the input is empty and there is a DefaultValue and the all condition from Validators is true, the return will be DefaultValue
  - When there is a default value, the InitValue method will be ignored and replaced by the default value  
- ```csharp
  IsPassword(bool swithVisible)
  ``` 
    - Input is password type.Default Value = false

- ```csharp
  ValidateOnDemand()
  ``` 
    - Run the validators on each interaction

- ```csharp
  DescriptionSelector(Func<string, string> value)
  ``` 
    - Run the fucntion on each interaction and show result in description line.

- ```csharp
  AddValidator(Func<object, ValidationResult> validator);
  ``` 
    - item of input validator.

- ```csharp
  AddValidators(IEnumerable<Func<object, ValidationResult>> validators)
  ``` 
    - List of input validator

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
  ResultPromptPlus<string> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Return
[**Top**](#-promptplus--input)

```csharp
IControlInput                //for Control Methods
ResultPromptPlus<string>     //After execute Run method
IPromptPipe                  //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase                //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#-promptplus--input)

```csharp
var name = PromptPlus.Input("What's your name?")
    .Default("Peter Parker")
    .Addvalidator(PromptPlusValidators.Required())
    .Addvalidator(PromptPlusValidators.MinLength(3))
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
