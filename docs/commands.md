# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Commands
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus)

## Documentation
Command set for PromptPlus console

![](./images/Commands.gif)

### Syntax
[**Top**](#promptplus--commands)

Using simple command to change forecolor and backcolor.

```csharp
PromptPlus.ConsoleDefaultColor(ConsoleColor? forecolor = null, ConsoleColor? backcolor = null)
````

Using simple command to clear screen and change backcolor.

```csharp
PromptPlus.Clear(ConsoleColor? backcolor = null)
````

Using simple command to more one writeline.

```csharp
PromptPlus.WriteLines(int value = 1)
````
Using simple command to clear rest of line.

```csharp
PromptPlus.ClearRestOfLine(ConsoleColor? color = null)
````

Using simple command to clear a line.

```csharp
 PromptPlus.ClearLine(int top)
````

Using simple command to wait a ConsoleKeyInfo with CancellationToken.

```csharp
ResultPromptPlus<ConsoleKeyInfo> keyinfo = PromptPlus.WaitKeypress(CancellationToken cancellationToken)
````

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) |
[**ResultPromptPlus**](resultpromptplus)

