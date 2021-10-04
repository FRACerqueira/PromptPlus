# PromptPlus # Key Press
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus) |
[**KeyPress Options**](keypressoptions) |
[**BaseOptions**](baseoptions)

## Documentation
Control KeyPress. Simple specific key.

![](./images/KeyPress.gif)

### Options

[**KeyPress Options**](keypressoptions)

### Syntax
[**Top**](#promptplus--key-press)

```csharp
//Note: If KeyPressOptions is default create eg.: new KeyPressOptions(), will be the same as the AnyKey control.
KeyPress(KeyPressOptions options, CancellationToken? cancellationToken = null)
KeyPress(Action<KeyPressOptions> configure, CancellationToken? cancellationToken = null)
````

```csharp
//Note: The HideAfterFinish property in KeyPress Options will always be True.
KeyPress(string message, char? Keypress, ConsoleModifiers? keymodifiers = null, CancellationToken? cancellationToken = null)
````

### Return
[**Top**](#promptplus--key-press)

```csharp
ResultPromptPlus<bool>
````

### Sample
[**Top**](#promptplus--key-press)

```csharp
var key = PromptPlus.KeyPress("Press Ctrl-B to continue", 'B', 
   ConsoleModifiers.Control, 
   cancellationToken:_stopApp);
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
[**KeyPress Options**](keypressoptions) |
[**BaseOptions**](baseoptions)

