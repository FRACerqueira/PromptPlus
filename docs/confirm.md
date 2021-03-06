# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Confirm
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control Confirm. Simple confirm with with tool tips and language detection.

![](./images/Confirm.gif)

### Syntax
[**Top**](#promptplus--confirm)

```csharp
Confirm(string prompt, string description = null)
````

### Methods
[**Top**](#promptplus--confirm)

- ```csharp
  Prompt(string value, string description = null)
  ``` 
  - set prompt message and optional description

- ```csharp
  Default(bool value)
  ``` 
    - Default value. True for positive confirm or False for negative confirm.

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
[**Top**](#promptplus--confirm)

```csharp
IControlConfirm            //for Control Methods
ResultPromptPlus<bool>     //After execute Run method
IPromptPipe                //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase              //for only definition of pipe to Pipeline Control
```

### Sample
[**Top**](#promptplus--confirm)

```csharp
PromptPlus.DefaultCulture = new CultureInfo("en-US");
var answer = PromptPlus.Confirm("Are you ready?")
    .Default(true)
    .Run(_stopApp);
if (answer.IsAborted)
{
    return;
}
if (answer.Value)
{
    Console.WriteLine($"Sua resposta é Yes");
}
else
{
    Console.WriteLine($"Sua resposta é No");
}
````

```csharp
PromptPlus.DefaultCulture = new CultureInfo("pt-BR");
var answer = PromptPlus.Confirm("Você esta pronto?")
    .Default(true)
    .Run(_stopApp);

if (answer.IsAborted)
{
    return;
}
if (answer.Value)
{
    Console.WriteLine($"Sua resposta é Sim");
}
else
{
    Console.WriteLine($"Sua resposta é Não");
}
````

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
