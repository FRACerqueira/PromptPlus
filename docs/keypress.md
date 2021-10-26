# PromptPlus # KeyPress
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**Base Methods**](basemethods) |
[**Pipe Methods**](pipemethods)

## Documentation
Control KeyPress. Simple specific key.

![](./images/KeyPress.gif)

### Syntax
[**Top**](#promptplus--keypress)

```csharp
KeyPress(char? Keypress = null, ConsoleModifiers? keymodifiers = null)
````

### Methods
[**Top**](#promptplus--keypress)

- ```csharp
  Prompt(string value)
  ``` 
  - set prompt message 

### Return
[**Top**](#promptplus--keypress)

```csharp
IControlKeyPress           //for Control Methods
IPromptControls<bool>      //for others Base Methods
ResultPromptPlus<bool>     //for Base Method Run, when execution is direct 
IPromptPipe                //for Pipe condition 
IFormPlusBase              //for only definition of pipe to Pipeline Control
````

### Sample
[**Top**](#promptplus--keypress)

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
