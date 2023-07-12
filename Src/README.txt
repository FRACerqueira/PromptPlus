=========================================================
 ____                             _   ____  _
|  _ \ _ __  ___  _ __ ___  _ __ | |_|  _ \| |_   _ ___
| |_) | '__|/ _ \| '_ ` _ \| '_ \| __| |_) | | | | | __|
|  __/| |  | (_) | | | | | | |_) | |_|  __/| | |_| |__ \
|_|   |_|   \___/|_| |_| |_| .__/ \__|_|   |_|\__,_|___/
                           |_|

Welcome to PromptPlus v4.0.0  
=========================================================

Interactive command-line toolkit for .Net core with powerful controls and commands
to create professional console applications.

All controls input/filter  (except Masked input) using **GNU Readline**
(https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts.  

PromptPlus Supports 4/8/24-bit colors in the terminal with auto-detection 
of the current terminal's capabilities.

Visit the official page for complete documentation of PromptPlus:
https://fracerqueira.github.io/PromptPlus

PromptPlus was developed in c# with target frameworks:

- netstandard2.1
- .NET 6
- .NET 7

*** What news in PromptPlus V4.0.0 ***
---------------------------------------

- New console engine
    - Supports 4/8/24-bit colors
    - Auto-detection of the current terminal's capabilities
    - New commands
    - New support feature colors
- All controls have been improved to accept color customization, new features and new design.
- Added new filter for colletion by "Contains" or "StartsWith"
- Added powerful new controls (eg: Treeview, wait tasks Parallel/Sequential, 
  8 progress bar types, wait timer, 19 spinners types, Gradient colors and more)

**PromptPlus Controls - Sample Usage**
--------------------------------------

//ASCII text banners
PromptPlus
    .Banner("PromptPlus v4.0")
    .Run(Color.Yellow,BannerDashOptions.DoubleBorderUpDown);

//MaskEdit DateTime
var mask = PromptPlus.MaskEdit("input", "MaskEdit DateTime input")
    .Mask(MaskedType.DateTime)
    .DescriptionWithInputType(FormatWeek.Short)
    .Culture("en-us")
    .AcceptEmptyValue()
    .Run();

if (!mask.IsAborted)
{
    PromptPlus.WriteLine($"You input with mask is {mask.Value.Masked}");
    PromptPlus.WriteLine($"You input without mask is {mask.Value.Input}");
}    

//INPUT
var input = PromptPlus
    .Input("Input sample")
    .Default("foo")
    .Run();

if (!input.IsAborted)
{
    PromptPlus.WriteLine($"You input is {input.Value}");
}

//AnyKey
var kp = PromptPlus
    .KeyPress()
    .Config(cfg => cfg.HideAfterFinish(true))
    .Spinner(SpinnersType.DotsScrolling)
    .Run();

**Supported platforms**
-----------------------

- Windows
    - Command Prompt, PowerShell, Windows Terminal
- Linux (Ubuntu, etc)
    - Windows Terminal (WSL 2)
- macOS
    - Terminal.app

**License**
-----------

Copyright 2021 @ Fernando Cerqueira
PromptPlus project is licensed under the  the MIT license.