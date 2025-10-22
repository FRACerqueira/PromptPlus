=========================================================
 ____                             _   ____  _
|  _ \ _ __  ___  _ __ ___  _ __ | |_|  _ \| |_   _ ___
| |_) | '__|/ _ \| '_ ` _ \| '_ \| __| |_) | | | | | __|
|  __/| |  | (_) | | | | | | |_) | |_|  __/| | |_| |__ \
|_|   |_|   \___/|_| |_| |_| .__/ \__|_|   |_|\__,_|___/
                           |_|

Welcome to PromptPlus
=====================

Interactive command-line toolkit for .NET Core with powerful controls and commands
to create professional console and 'cli' applications.

All controls using **GNU Readline**
(https://en.wikipedia.org/wiki/GNU_Readline) Emacs keyboard shortcuts.  

PromptPlus Supports 4/8/24-bit colors in the terminal with auto-detection 
of the current terminal's capabilities  and automatic color conversion.

Visit the official page for complete documentation of PromptPlus:
https://github.com/FRACerqueira/PromptPlus

PromptPlus was developed in C# with target frameworks:

- .NET 10
- .NET 9
- .NET 8

*** What's new in V5.0.0 ***
----------------------------

We're very excited about the big release of this new version. **Version 5 has been completely redesigned** and optimized for better stability, consistency, and performance. 

All controls and behaviors have been revisited and improved to ensure sustainable evolution. 

Due to the significant modifications, version 5 introduced **significant changes and is incompatible with versions 4.x**, although the concepts and components are very similar, requiring a small learning curve and minor methodological adjustments.

- Support for .Net10,.Net9 and .Net8

- External references were reviewed and only those necessary for size treatment for East Asian characters were used.

- New control rendering engine adjusts more fluidly to the screen size and avoids flickering by redrawing only the changed lines.

- Revised control of hotkeys and special characters ensuring consistency according to the console's capabilities.

- Created the separation of interactive controls and added several non-interactive controls (widgets)

- Introduced **NEW multi-threaded operation** for controls and commands. Now each command and control block the main thread during printing/interaction execution.

- Renaming several control methods for better clarity and reduced scope, aiming at the unique responsibility that each component intends to perform, allowing for sustainable evolution.

- Created the concept of an editing window for controls that require a significant input/response size, ensuring visual consistency and adequate navigability.

- A slice architecture was adopted for each component, allowing individual evolution of each one with low interference to the others.

- The **NEW tooltip mechanism** now shows all keys and hotkeys for each control by switching the view ('F1').

- All interative controls start at : **PromptPlus.Controls**.<name of control>.

    - All initialization contracts have been standardized: PromptPlus.Controls.<name of control>(string prompt = "", string? description = null).

- All no interative controls start at : **PromptPlus.Widgets**.<name of control>.

    - For each non-interactive control the initialization contract was customized.

- All commands for console start at : **PromptPlus.Console**.<command>.

- All general config start at : **PromptPlus.Config**.<config>.

**License**
-----------

Copyright 2025 @ Fernando Cerqueira
PromptPlus project is licensed under the  the MIT license.
