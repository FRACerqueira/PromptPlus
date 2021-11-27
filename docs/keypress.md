# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # KeyPress
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control KeyPress. Simple specific key.

![](./images/KeyPress.gif)

### Syntax
[**Top**](#-promptplus--keypress)

```csharp
KeyPress(char? Keypress = null, ConsoleModifiers? keymodifiers = null)
````

### Methods
[**Top**](#-promptplus--keypress)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message

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
[**Top**](#-promptplus--keypress)

```csharp
IControlKeyPress           //for Control Methods
ResultPromptPlus<bool>     //After execute Run method
IPromptPipe                //for Pipe condition and transform to IFormPlusBase 
IFormPlusBase              //for only definition of pipe to Pipeline Control
````

### Sample
[**Top**](#-promptplus--keypress)

```csharp
var key = PromptPlus.KeyPress('B', ConsoleModifiers.Control)
    .Prompt("Press Ctrl-B to continue")
    .Run(_stopApp);
if (key.IsAborted)
{
    return;
}
Console.WriteLine($"Hello,  key Ctrl-B pressed");
````

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)
