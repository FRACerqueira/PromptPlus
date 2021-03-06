# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Colors
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) 

## Documentation
Command Color. Commands to write text easily with color.

![](./images/Color.gif)

### Syntax 1
[**Top**](#promptplus--colors)
Using simple commands for all text.

```csharp
PromptPlus.WriteLine(Exception value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null)
````

```csharp
PromptPlus.WriteLine(string value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null, bool underline = false)
````

```csharp
PromptPlus.Write(string value, ConsoleColor? forecolor = null, ConsoleColor? backcolor = null, bool underline = false)
````

### Syntax 2
[**Top**](#promptplus--colors)
Using color extensions to colorize parts of a text.

```csharp
PromptPlus.WriteLine("This is a simples ","line".White().OnBlue().Underline(), " with ", "color".Red());
````

### Syntax 3
[**Top**](#promptplus--colors)
Using markup style to colorize parts of a text with stacked colors ("LIFO").

```csharp
PromptPlus.WriteLine("This [cyan]is [red]a [white:blue]simples[/] line with [yellow!u]color[/]. End [/]line.");
````
### Syntax 4
Using all together to colorize parts of a text.

```csharp
PromptPlus.WriteLine("[cyan]This[/] is another [white:blue]simples[/] line using [red!u]Mask[/].".Mask(ConsoleColor.Yellow));
````

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis) 


