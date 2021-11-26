# <img align="left" width="100" height="100" src="./images/icon.png"> PromptPlus # Banner
[**Main**](index.md#help) | 
[**Controls**](index.md#apis)

## Documentation
Control Banner. Simple ASCII text banner.

![](./images/Banner.gif)

### Syntax
[**Top**](#-promptplus--banner)

```csharp
Banner(string value)
````

### Methods
[**Top**](#-promptplus--banner)

- ```csharp
LoadFont(string value)
``` 
  -  path file of type "flf". see [figlet](http://www.figlet.org/)

- ```csharp
LoadFont(Stream value)
``` 
  - stream file of type "flf". see [figlet](http://www.figlet.org/)

- ```csharp
FIGletWidth(CharacterWidth value)
``` 
  - Width FIGlet-type (Smush,Fitted,Full)

- ```csharp
Run(ConsoleColor? color = null)
``` 
  - Show Banner with forecolor paramater.

### Return
[**Top**](#-promptplus--banner)

```csharp
IFIGlet            //for Control Methods
```

### Sample
[**Top**](#-promptplus--banner)

```csharp
var colorsel = PromptPlus.Select<ConsoleColor>("Select a color")
    .HideItem(PromptPlus.BackgroundColor)
    .Run(_stopApp);

if (colorsel.IsAborted)
{
    return;
}

var widthsel = PromptPlus.Select<CharacterWidth>("Select a Character Width")
    .Run(_stopApp);
if (widthsel.IsAborted)
{
    return;
}

var fontsel = PromptPlus.Select<string>("Select a font")
        .AddItem("Default")
        .AddItem("Starwars")
        .Run(_stopApp);
if (fontsel.IsAborted)
{
    return;
}

if (fontsel.Value[0] == 'D')
{
    PromptPlus.Banner("PromptPlus")
        .FIGletWidth(widthsel.Value)
        .Run(colorsel.Value);
}
else
{
    PromptPlus.Banner("PromptPlus")
        .FIGletWidth(widthsel.Value)
        .LoadFont("starwars.flf")
        .Run(colorsel.Value);
}
````

### Links
[**Main**](index.md#help) | 
[**Controls**](index.md#apis)
