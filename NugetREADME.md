# **Welcome to PromptPlus**

Interactive command-line  toolkit for **C#** with powerful controls and commands.
PromptPlus was developed in c# with the **netstandard2.1, .NET 5 AND .NET6 ** target frameworks.

All controls input/filter using **[GNU Readline](https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts**.  

**PromptPlus** has separate pakage integrate command line parse **CommandDotNet(4.3.0/5.0.1)**:
- PromptPlus.CommandDotNet

## **Official pages** :

#### **[Visit the PromptPlus official page for complete documentation](https://fracerqueira.github.io/PromptPlus)**

#### **[Visit the CommandDotNet official page for complete documentation](https://commanddotnet.bilal-fazlani.com)**

## **PromptPlus.CommandDotNet - Sample Usage**

```csharp
public class Program
{
    static int Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

        PromptPlus.ConsoleDefaultColor(ConsoleColor.White, ConsoleColor.Black);
        PromptPlus.Clear();

        return new AppRunner<Examples>()
            .UseDefaultMiddleware()
            .UsePrompter()
            .UseNameCasing(Case.KebabCase)
            .UsePromptPlusAnsiConsole()
            .UsePromptPlusArgumentPrompter()
            .UsePromptPlusWizard()
            .UsePromptPlusRepl(colorizeSessionInitMessage: (msg) => msg.Yellow().Underline())
            .Run(args);
    }
}
//for usage AppRunner see https://commanddotnet.bilal-fazlani.com/
```

## **PromptPlus Controls - Sample Usage**

```csharp
//ASCII text banners
PromptPlus.Banner("PromptPlus")
    .Run(ConsoleColor.Green);

//MaskEdit Generic
var mask = PromptPlus.MaskEdit(MaskedType.Generic, "Inventory Number")
    .Mask(@"\XYZ 9{3}-L{3}-C[ABC]N{1}[XYZ]-A{3}")
    .Run(_stopApp);

if (mask.IsAborted)
{
    return;
}
if (string.IsNullOrEmpty(mask.Result.Value))
{
    Console.WriteLine($"your input was empty!");
}
else
{
    Console.WriteLine($"your input was {mask.Result.ObjectValue}!");
}

//AnyKey
var key = PromptPlus.KeyPress()
        .Run(_stopApp);

if (key.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, key pressed");
```

## **Supported platforms**

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

## **License**

This project is licensed under the [MIT License](https://github.com/FRACerqueira/PromptPlus/blob/master/LICENSE)

