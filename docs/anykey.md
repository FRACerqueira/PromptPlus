# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # AnyKey
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Simple any key press.

![](./images/Anykey.gif)

### Syntax
[**Top**](#-promptplus--anykey)

```csharp
KeyPress() 
```

### Methods
[**Top**](#-promptplus--anykey)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message. If omitted, default value is text "Press any key"

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
  - It is mandatory to use this method to use with the Pipeline control. See examples in [**PipeLine Control**](pipeline)

- ```csharp
  ResultPromptPlus<bool> Run(CancellationToken? value = null)
  ``` 
	- Control execution

### Returns Types
[**Top**](#-promptplus--anykey)

```csharp
IControlKeyPress        //for Control Methods
ResultPromptPlus<bool>  //After execute method Run
IPromptPipe             //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase           //for only definition of pipe to Pipeline Control
```
### Sample
[**Top**](#-promptplus--anykey)

```csharp
var key = PromptPlus.KeyPress()
        .Run(_stopApp);

if (key.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, key pressed");
```

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
