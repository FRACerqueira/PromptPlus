![PromptPlus Logo](https://raw.githubusercontent.com/FRACerqueira/PromptPlus/refs/heads/main/icon.png)

### PromptPlus class
</br>


#### Provides extension methods for ConsoleKeyInfo to evaluate specific key combinations, including standard keys and Emacs-style shortcuts.

Provides the global entry point for all PromptPlus controls, widgets, configuration access and console services.

```csharp
public static class PromptPlus
```

### Public Members

| name | description |
| --- | --- |
| static [Config](PromptPlus/Config.md) { get; } | Gets the global configuration instance applied to newly created controls and widgets. |
| static [Console](PromptPlus/Console.md) { get; } | Gets the current console driver abstraction providing low-level I/O, color and buffer operations. |
| static [Controls](PromptPlus/Controls.md) { get; } | Gets a factory for interactive controls (input, select, file select, progress, masking, etc.). Each method returns a fluent configuration object. |
| static [NameResourceConfigFile](PromptPlus/NameResourceConfigFile.md) { get; } | Gets the default file name for the PromptPlus resource configuration file. |
| static [Widgets](PromptPlus/Widgets.md) { get; } | Gets a factory for creating and emitting visual widgets (banner, dash lines, chart bar, slider, etc.). |
| static [Clear](PromptPlus/Clear.md)(…) | Clears the console buffer with [`Color`](./Color.md) and set BackgroundColor with [`Color`](./Color.md) |
| static [ClearLine](PromptPlus/ClearLine.md)(…) | Clear line |
| static [CreatePromptPlusConfigFile](PromptPlus/CreatePromptPlusConfigFile.md)(…) | Creates a new configuration file for PromptPlus using the name [`NameResourceConfigFile`](./PromptPlus/NameResourceConfigFile.md) |
| static [ExclusiveContext](PromptPlus/ExclusiveContext.md)(…) | Create Exclusive context to write on standard output stream for any output included until the 'dispose' is done. |
| static [IsAbortKeyPress](PromptPlus/IsAbortKeyPress.md)(…) | Determines whether the configured global abort hotkey was pressed. |
| static [IsBackwardWord](PromptPlus/IsBackwardWord.md)(…) | Determines whether the Alt+B (backward word) Emacs shortcut was pressed. |
| static [IsCapitalizeOverCursor](PromptPlus/IsCapitalizeOverCursor.md)(…) | Determines whether the Alt+C (capitalize over cursor) Emacs shortcut was pressed. |
| static [IsClearAfterCursor](PromptPlus/IsClearAfterCursor.md)(…) | Determines whether the Control+K (clear after cursor) Emacs shortcut was pressed. |
| static [IsClearBeforeCursor](PromptPlus/IsClearBeforeCursor.md)(…) | Determines whether the Control+U (clear before cursor) Emacs shortcut was pressed. |
| static [IsClearContent](PromptPlus/IsClearContent.md)(…) | Determines whether the Control+L (clear content) Emacs shortcut was pressed. |
| static [IsClearWordAfterCursor](PromptPlus/IsClearWordAfterCursor.md)(…) | Determines whether the Control+D (clear word after cursor) Emacs shortcut was pressed. |
| static [IsClearWordBeforeCursor](PromptPlus/IsClearWordBeforeCursor.md)(…) | Determines whether the Control+W (clear word before cursor) Emacs shortcut was pressed. |
| static [IsForwardWord](PromptPlus/IsForwardWord.md)(…) | Determines whether the Alt+F (forward word) Emacs shortcut was pressed. |
| static [IsLowersCurrentWord](PromptPlus/IsLowersCurrentWord.md)(…) | Determines whether the Alt+L (lower-case word) Emacs shortcut was pressed. |
| static [IsNoResponseKey](PromptPlus/IsNoResponseKey.md)(…) | Check ConsoleKeyInfo is Yes key |
| static [IsPressBackspaceKey](PromptPlus/IsPressBackspaceKey.md)(…) | Determines whether Backspace or (optionally) Control+H (Emacs delete previous character) was pressed. |
| static [IsPressCtrlEndKey](PromptPlus/IsPressCtrlEndKey.md)(…) | Determines whether Control+End was pressed. |
| static [IsPressCtrlHomeKey](PromptPlus/IsPressCtrlHomeKey.md)(…) | Determines whether Control+Home was pressed. |
| static [IsPressCtrlSpaceKey](PromptPlus/IsPressCtrlSpaceKey.md)(…) | Determines whether Control+Spacebar was pressed. |
| static [IsPressDeleteKey](PromptPlus/IsPressDeleteKey.md)(…) | Determines whether Delete or (optionally) Control+D (Emacs delete forward) was pressed. |
| static [IsPressDownArrowKey](PromptPlus/IsPressDownArrowKey.md)(…) | Determines whether DownArrow or (optionally) Control+N (Emacs next line/history) was pressed. |
| static [IsPressEndKey](PromptPlus/IsPressEndKey.md)(…) | Determines whether End or (optionally) Control+E (Emacs end-of-line) was pressed. |
| static [IsPressEnterKey](PromptPlus/IsPressEnterKey.md)(…) | Determines whether the pressed key represents an Enter action. |
| static [IsPressEscKey](PromptPlus/IsPressEscKey.md)(…) | Determines whether Escape (without modifiers) was pressed. |
| static [IsPressHomeKey](PromptPlus/IsPressHomeKey.md)(…) | Determines whether Home or (optionally) Control+A (Emacs beginning-of-line) was pressed. |
| static [IsPressLeftArrowKey](PromptPlus/IsPressLeftArrowKey.md)(…) | Determines whether LeftArrow or (optionally) Control+B (Emacs move backward) was pressed. |
| static [IsPressPageDownKey](PromptPlus/IsPressPageDownKey.md)(…) | Determines whether PageDown or (optionally) Alt+N (Emacs next extended navigation) was pressed. |
| static [IsPressPageUpKey](PromptPlus/IsPressPageUpKey.md)(…) | Determines whether PageUp or (optionally) Alt+P (Emacs previous extended navigation) was pressed. |
| static [IsPressRightArrowKey](PromptPlus/IsPressRightArrowKey.md)(…) | Determines whether RightArrow or (optionally) Control+F (Emacs move forward) was pressed. |
| static [IsPressShiftTabKey](PromptPlus/IsPressShiftTabKey.md)(…) | Determines whether Shift+Tab was pressed. |
| static [IsPressSpaceKey](PromptPlus/IsPressSpaceKey.md)(…) | Determines whether Spacebar (without modifiers) was pressed. |
| static [IsPressSpecialKey](PromptPlus/IsPressSpecialKey.md)(…) | Determines whether the pressed key matches the specified *key* and *modifier*. |
| static [IsPressTabKey](PromptPlus/IsPressTabKey.md)(…) | Determines whether the Tab key (without modifiers) was pressed. |
| static [IsPressUpArrowKey](PromptPlus/IsPressUpArrowKey.md)(…) | Determines whether UpArrow or (optionally) Control+P (Emacs previous line/history) was pressed. |
| static [IsTransposePrevious](PromptPlus/IsTransposePrevious.md)(…) | Determines whether the Control+T (transpose previous characters) Emacs shortcut was pressed. |
| static [IsUppersCurrentWord](PromptPlus/IsUppersCurrentWord.md)(…) | Determines whether the Alt+U (upper-case word) Emacs shortcut was pressed. |
| static [IsYesResponseKey](PromptPlus/IsYesResponseKey.md)(…) | Check ConsoleKeyInfo is Yes key |
| static [Join](PromptPlus/Join.md)(…) | Wait all output using exclusive buffer to console |
| static [OutputError](PromptPlus/OutputError.md)(…) | Create context to write on standard error output stream for any output included until the 'dispose' is done. |
| static [ProfileConfig](PromptPlus/ProfileConfig.md)(…) | Reconfigures the active console profile (colors, padding, overflow). Thread-safe. |
| static [WriteLines](PromptPlus/WriteLines.md)(…) | Write lines with line terminator |

### Remarks

The static initialization sequence detects terminal capabilities (ANSI, Unicode, color depth, legacy mode), captures the original console state (culture, encoding, colors) and prepares an internal profile. Resources are restored automatically on process exit.

### See Also

* namespace [PromptPlusLibrary](../PromptPlus.md)

<!-- DO NOT EDIT: generated by xmldocmd for PromptPlus.dll -->
