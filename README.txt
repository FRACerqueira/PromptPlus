================================================================================
PromptPlus
================================================================================

A modern .NET library that delivers polished, interactive console experiences -
text input, searchable lists, masked fields, date/time pickers, file browsers,
progress bars, charts and more - all through one sleek fluent API.

NuGet: https://www.nuget.org/packages/PromptPlus
License: MIT
Target Frameworks: .NET 8, .NET 9, .NET 10

================================================================================
OVERVIEW
================================================================================

PromptPlus transforms your console apps with 20+ interactive controls and
6 output-only widgets, configured through a readable fluent API. It supports
two-layer configuration (global defaults + per-control overrides),
abort-anywhere with IsAborted flag, history persistence, terminal resize
detection, and 11 built-in locales.

PromptPlus is built on top of ConsolePlus and shares the same IConsole driver
for styled output.

================================================================================
KEY FEATURES
================================================================================

* 20+ interactive controls
  - From key-press to multi-column tables and tree browsers

* 6 output-only widgets
  - Sliders, calendars, banners, charts without blocking

* Fluent API
  - Every control configured with readable method chains

* Two-layer config
  - Set defaults with PromptPlus.Config, override per control with .Options()

* Abort anywhere
  - Esc aborts any control; result carries an IsAborted flag

* History persistence
  - Last confirmed value saved and pre-loaded automatically

* Terminal-safe
  - Auto-detects size, re-renders on resize, enforces 80x10 minimum

* Localization
  - 11 built-in locales selected from current culture

* Cross-platform
  - Windows, Linux, macOS; .NET 8, 9 and 10

================================================================================
INSTALLATION
================================================================================

    dotnet add package PromptPlus

Or via the Package Manager Console:

    Install-Package PromptPlus

================================================================================
QUICK START
================================================================================

    using PromptPlusLibrary;

    // Ask for a name
    var nameResult = PromptPlus.Controls.Input("Your name").Run();
    if (nameResult.IsAborted) return;

    // Choose a color
    var colorResult = PromptPlus.Controls
        .Select<string>("Favorite color")
        .AddItems(["Red", "Green", "Blue"])
        .Run();

    // Deconstruct result
    var (color, aborted) = colorResult;
    if (!aborted)
        PromptPlus.Console.WriteLine($"Hello {nameResult.Content}, you chose {color}!");

Tip: Every control returns ResultPrompt<T>. Use .Content for the value,
.IsAborted to detect Esc, or deconstruct with var (value, aborted) = result.

"================================================================================`nTWO-LAYER CONFIGURATION`n================================================================================`n`n    PromptPlus.Config.PageSize = 8;`n    PromptPlus.Config.HideAfterFinish = true;`n`n    // Per-control override always wins`n    PromptPlus.Controls.Input(""Notes"").Options(o => o.HideAfterFinish(false)).Run();`n`n================================================================================`nCONTROLS`n================================================================================`n`nInput, Secret, KeyPress, Confirm, Select/MultiSelect, Table/MultiTable,`nTree/MultiTree, File/MultiFile, Calendar, Slider, Switch, Time, ProgressBar,`nTask/MultiTasks, ChartBar, Mask editors (string, int, long, decimal, double,`ncurrency, date, time, DateOnly, TimeOnly).`n`n================================================================================`nWIDGETS (output-only)`n================================================================================`n`nSlider, Calendar, Switch, Banner, Dash separator, ChartBar.`n`n================================================================================`nLOCALIZATION`n================================================================================`n`n11 built-in locales: English, pt-BR, de-DE, es-ES, fr-FR, it-IT, ja-JP,`nko-KR, nl-BE, ru-RU, zh-CN.`n`n================================================================================`nDOCUMENTATION`n================================================================================`n`nFull docs: https://github.com/FRACerqueira/PromptPlus`n`n================================================================================`nLICENSE`n================================================================================`n`nMIT (c) PromptPlus contributors" | Add-Content -Path "PromptPlus\README.txt" -Enco==========================================================================ding UTF8======
TWO-LAYER CONFIGURATION
================================================================================

    // Layer 1 - global defaults applied to all controls
    PromptPlus.Config.PageSize = 8;
    PromptPlus.Config.HideAfterFinish = true;

    // Layer 2 - per-control override always wins
    PromptPlus.Controls
        .Input("Notes")
        .Options(o => o.HideAfterFinish(false).ShowTooltip(false))
        .Run();

================================================================================
CONTROLS
================================================================================

Input, Secret, KeyPress, Confirm, Select / MultiSelect, Table / MultiTable,
Tree / MultiTree, File / MultiFile, Calendar, Slider, Switch, Time,
ProgressBar, Task / MultiTasks, ChartBar and a full family of Mask editors
(string, integer, long, decimal, double, currency, date, time, DateOnly,
TimeOnly).

================================================================================
WIDGETS (output-only)
================================================================================

Slider, Calendar, Switch, Banner, Dash separator, ChartBar.

================================================================================
LOCALIZATION
================================================================================

11 built-in locales: English (default), pt-BR, de-DE, es-ES, fr-FR, it-IT,
ja-JP, ko-KR, nl-BE, ru-RU, zh-CN. Custom locales supported via satellite
resource files.

================================================================================
DOCUMENTATION & SAMPLES
================================================================================

The full documentation, control reference, keyboard bindings, styling guide
and runnable samples are available in the project repository.

Full README and docs on GitHub: https://github.com/FRACerqueira/PromptPlus

================================================================================
LICENSE
================================================================================

PromptPlus is licensed under the MIT License : https://opensource.org/licenses/MIT).

Maintained by the PromptPlus project • © 2026 Fernando Cerqueira</sub>

