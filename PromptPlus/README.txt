 ____                             _   ____  _
|  _ \ _ __  ___  _ __ ___  _ __ | |_|  _ \| |_   _ ___
| |_) | '__|/ _ \| '_ ` _ \| '_ \| __| |_) | | | | | __|
|  __/| |  | (_) | | | | | | |_) | |_|  __/| | |_| |__ \
|_|   |_|   \___/|_| |_| |_| .__/ \__|_|   |_|\__,_|___/
                           |_|
**Welcome to PromptPlus**

Interactive command-line toolkit for **C#** with powerful controls.

PromptPlus was developed in c# with target frameworks:

- netstandard2.1
- .NET 5
- .NET 6

All controls input/filter using **GNU Readline ** https://en.wikipedia.org/wiki/GNU_Readline Emacs keyboard shortcuts.

PromptPlus has **separate pakage** integrate command line parse CommandDotNet(4.3.0/6.0.0):

- PromptPlus.CommandDotNet (V1.0.0.300)!!!

**visit the official pages for complete documentation** :

https://fracerqueira.github.io/PromptPlus
For PromptPlus controls

https://commanddotnet.bilal-fazlani.com/
CommandDotNet is third party applications

**Relase Notes PromptPlus.CommandDotNet (V1.0.0.300)**
-----------------------------------------------------------

- Added Middleware ** UsePromptPlusWizard **
    - Directive to wizard find all the commands/options and arguments with prompt and then run.
    - Now you can discover and learn the existing commands, options and arguments.
- Added Middleware UsePromptPlusAnsiConsole
    - Makes the IConsoleDriver available as a command parameter and will forward IConsole.Out to the PromptPLus IConsoleDriver.
- Added Middleware UsePromptPlusArgumentPrompter
    - Adds support for prompting arguments.By default, prompts for arguments missing a required value. Missing is determined by IArgumentArity, not by any validation frameworks.
- Added Middleware UsePromptPlusConfig
    - Load custom config(Colors/hotkeys/and so on) for PromptPlus.Remark: This method is only necessary when the file is in a custom folder. Prompt Plus automatically loads the file if the file is placed in the same folder as the binaries.
- Added PromptValidatorUri attribute
    - Attribute to Bind the property or parameters with validate Uri format using PromptPlus-Control
- Added PromptPlusTypeNumber attribute
    - Attribute to Bind the property or parameters with validate number format using PromptPlus-MaskEdit-Control
- Added PromptPlusTypeCurrency attribute
    - Attribute to Bind the property or parameters with validate Currency format using PromptPlus-MaskEdit-Control
- Added PromptPlusTypeDate attribute
    - Attribute to Bind the property or parameters with validate date format using PromptPlus-MaskEdit-Control
- Added PromptPlusTypeTime attribute
    - Attribute to Bind the property or parameters with validate Time format using PromptPlus-MaskEdit-Control
- Added PromptPlusTypeBrowser attribute
    - Attribute to Bind the property or parameters with validate input using PromptPlus-Browser-Control
- Added PromptPlusTypeMasked attribute
    - Attribute to Bind the property or parameters with validate generic format using PromptPlus-MaskEdit-Control

**PromptPlus.CommandDotNet - Sample Usage**
-------------------------------------------
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
            .Run(args);
    }
}

//for usage AppRunner see https://commanddotnet.bilal-fazlani.com/

**Relase Notes PromptPlus (V.3.0.0)**
-------------------------------------

- Refactoring - Changed HotKeys to only F1~F12 keys (maybe requires refactoring)
- Refactoring - Renamed root namespace to PPlus (requires refactoring)
- Refactoring - Moved EnabledAbortKey/EnabledAbortAllPipes/EnabledPromptTooltip/HideAfterFinish to IPromptConfig (maybe requires refactoring)
- Refactoring - Added Config(Action<IPromptConfig> context) method to config and return to interface control for better usability.
- Refactoring - Method Syntax Adjustment to Input-Control (need to be refactored to new syntax):
    - AddValidators(Func<object, ValidationResult> validator) -> AddValidator(Func<object, ValidationResult> validator) 
- Improvement -  Accordance with informal standard [NO COLOR]https://no-color.org/.
    - When there is the environment variable "no_color"" the colors are disabled.
- Improvement - Apply GNU Readline - Emacs keyboard shortcuts for all controls
- Improvement - Added Extensions points to all controls (AddExtraAction)
    - OnStartControl
    - OnInputRender
    - OnInputAccept
    - OnFinishControl
- Improvement - Add New Control ***ReadLine** with sugestion and history capacity
- Improvement - AddSugestion (Tab/ShiftTab) to input control (with Extensions points)
- Improvement - AddHistory (Down/ShiftDown) to input control (with Extensions points)
- Improvement - Check Nullabled to AddDefault(s)/AddItem(s)/AddGroup/HideItem(s)/DisableItem(s)/Default methods
- Improvement - New Embededs Validator :
    - IsUriScheme(UriKind uriKind = UriKind.Absolute,string allowedUriSchemes = null, string errorMessage = null)
    - IsTypeBoolean(string errorMessage = null)
    - IsTypeByte(string errorMessage = null)
    - IsTypeChar(string errorMessage = null)
    - IsTypeDecimal(string errorMessage = null)
    - IsTypeDouble(string errorMessage = null)
    - IsTypeSByte(string errorMessage = null)
    - IsTypeDateTime(string errorMessage = null)
    - IsTypeShort(string errorMessage = null) / IsTypeInt16(string errorMessage = null)
    - IsTypeInt(string errorMessage = null) / IsTypeInt32(string errorMessage = null)
    - IsTypeLong(string errorMessage = null) / IsTypeInt64(string errorMessage = null)
    - IsTypeFloat(string errorMessage = null) / IsTypeSingle(string errorMessage = null)
    - IsTypeUshort(string errorMessage = null) /IsTypeUInt16(string errorMessage = null)
    - IsTypeUInt(string errorMessage = null) / IsTypeUInt32(string errorMessage = null)
    - IsTypeULong(string errorMessage = null) / IsTypeUInt64(string errorMessage = null)
- Improvement - Added Description parameter to all controls
- Improvement - Added InitialValue(string value) method  to Input-Control
- Improvement - Added InitialValue(T value, bool ever = false) method to List-Control
- Improvement - Added InitialValue(string value, bool ever = false) method to Listmasked-Control
- Improvement - Added AddItem(T value)/AddItems(IEnumerable<T> value) method to List-Controls
    - Now the List-Control and Listmasked-Control can start with values
- Improvement - Added TransformItems(Func<string, string> value) method to Listmasked-Control
    - Now the added items can be transformed during startup.
- Improvement - Added global hotkey (default value = F3) show/hide Description
- Improvement - Added color Schema Description (default value = ConsoleColor.Cyan)
- Improvement - Added color Schema CurrentTokenForeColor (for PromptPlus.CommandDotNet, default value = ConsoleColor.Yellow)
- Improvement - Added DescriptionSelector method for Description change on each interaction
    - Input-Control
    - AutoComplete-Control
    - Listmasked-Control
    - List-Control
    - MaskEdit-Control
    - Select-Control
    - MultSelect-Control
- Improvement - Reviewed Defaut value to input with masked (maybe requires refactoring)
    - MaskEdit-Control/Listmasked-Control
- Improvement - Added Ctrl-Enter to Finish widthout added last input
    - List-Control/Listmasked-Control
- Improvement - Color the text with stacked colors ("LIFO") and simplified the final color token to "[/]"

- Fixed bug - Defaut value to Browser-Control
- Fixed bug - Added masked item in list widthout Leading Zeros for MaskedType.Number/MaskedType.NumberCurrency (Listmasked-Control)
- Fixed bug - Embededs Validator for many kind-input types
- Fixed bug - Isolate thread Culture for all controls
- Fixed bug - Color when item disabled (Select-Control)
- Fixed bug - Added missing ShowDayWeek method to Listmasked-Control

**PromptPlus Controls - Sample Usage**
--------------------------------------

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


//input
var name = PromptPlus.Input("What's your name?")
    .Default("Peter Parker")
    .Addvalidator(PromptPlusValidators.Required())
    .Addvalidator(PromptPlusValidators.MinLength(3))
    .Run(_stopApp);

if (name.IsAborted)
{
    return;
}
Console.WriteLine($"Hello, {name.Result}!");

**Supported platforms**
-----------------------

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app
