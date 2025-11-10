![PromptPlus Logo](https://raw.githubusercontent.com/FRACerqueira/PromptPlus/refs/heads/main/icon.png)

### IConsole interface
</br>


#### Defines console interaction and rendering capabilities combined with a profile ([`IProfileDrive`](./IProfileDrive.md)).

```csharp
public interface IConsole : IProfileDrive
```

### Members

| name | description |
| --- | --- |
| [BackgroundColor](IConsole/BackgroundColor.md) { get; set; } | Gets or sets the current background [`Color`](./Color.md). |
| [BehaviorAfterCancelKeyPress](IConsole/BehaviorAfterCancelKeyPress.md) { get; } | Gets the behavior to be applied after a cancel key (Ctrl+C/Ctrl+Break) is pressed. This setting is ignored if no handler is configured via [`CancelKeyPress`](./IConsole/CancelKeyPress.md) or returns `false` from [`CancelKeyPress`](./IConsole/CancelKeyPress.md). |
| [CurrentBuffer](IConsole/CurrentBuffer.md) { get; } | Gets the currently active screen buffer. |
| [CursorLeft](IConsole/CursorLeft.md) { get; } | Gets the column (left) position of the cursor within the buffer. |
| [CursorTop](IConsole/CursorTop.md) { get; } | Gets the row (top) position of the cursor within the buffer. |
| [CursorVisible](IConsole/CursorVisible.md) { get; set; } | Gets or sets a value indicating whether the cursor is visible. |
| [EnabledExclusiveContext](IConsole/EnabledExclusiveContext.md) { get; set; } | Gets a value indicating Enabled Exclusive ontext for controls/wdgets and commands console. Default value is `false`. |
| [Error](IConsole/Error.md) { get; } | Gets the standard error writer. |
| [ForegroundColor](IConsole/ForegroundColor.md) { get; set; } | Gets or sets the current foreground [`Color`](./Color.md). |
| [In](IConsole/In.md) { get; } | Gets the standard input reader. |
| [InputEncoding](IConsole/InputEncoding.md) { get; set; } | Gets or sets the encoding for standard input. |
| [IsEnabledSwapScreen](IConsole/IsEnabledSwapScreen.md) { get; } | Gets a value indicating whether screen swapping is supported. |
| [IsErrorRedirected](IConsole/IsErrorRedirected.md) { get; } | Gets a value indicating whether standard error is redirected. |
| [IsInputRedirected](IConsole/IsInputRedirected.md) { get; } | Gets a value indicating whether standard input is redirected. |
| [IsOutputRedirected](IConsole/IsOutputRedirected.md) { get; } | Gets a value indicating whether standard output is redirected. |
| [KeyAvailable](IConsole/KeyAvailable.md) { get; } | Gets a value indicating whether a key press is available. |
| [Out](IConsole/Out.md) { get; } | Gets the standard output writer. |
| [OutputEncoding](IConsole/OutputEncoding.md) { get; set; } | Gets or sets the encoding for standard output. |
| [UserPressKeyAborted](IConsole/UserPressKeyAborted.md) { get; } | Gets a value indicating whether the operation was aborted by the user (Ctrl+C / Ctrl+Break). |
| [Beep](IConsole/Beep.md)() | Emits an audible beep if supported. |
| [CancelKeyPress](IConsole/CancelKeyPress.md)(…) | Sets a handler for console cancel events (Ctrl+C/Break). |
| [Clear](IConsole/Clear.md)() | Clears the buffer (and visible window) and resets cursor to (0,0). |
| [DefaultColors](IConsole/DefaultColors.md)(…) | Sets the default foreground and background console colors used when resetting. |
| [GetCursorPosition](IConsole/GetCursorPosition.md)() | Gets the current cursor position. |
| [HideCursor](IConsole/HideCursor.md)() | Hides the cursor. |
| [OnBuffer](IConsole/OnBuffer.md)(…) | Executes an action on a target buffer and then restores the original buffer. |
| [ReadKey](IConsole/ReadKey.md)(…) | Reads the next key press. |
| [ReadLine](IConsole/ReadLine.md)() | Reads a line of text from the input stream. |
| [RemoveCancelKeyPress](IConsole/RemoveCancelKeyPress.md)() | Removes the current cancel key press (Ctrl+C/Break) handler and restores default behavior. |
| [ResetColor](IConsole/ResetColor.md)() | Resets the current colors to the configured defaults. |
| [SetCursorPosition](IConsole/SetCursorPosition.md)(…) | Sets the cursor position. |
| [SetError](IConsole/SetError.md)(…) | Sets the standard error writer. |
| [SetIn](IConsole/SetIn.md)(…) | Sets the standard input source. |
| [SetOut](IConsole/SetOut.md)(…) | Sets the standard output writer. |
| [ShowCursor](IConsole/ShowCursor.md)() | Shows the cursor. |
| [SwapBuffer](IConsole/SwapBuffer.md)(…) | Attempts to switch to a target screen buffer. |
| [Write](IConsole/Write.md)(…) | Writes a character array. (3 methods) |
| [WriteLine](IConsole/WriteLine.md)(…) | Writes a character array followed by a line terminator. (3 methods) |

### Remarks

This interface abstracts terminal features (cursor control, colors, encoding, buffering, input/output streams, and multi‑screen support). Implementations should adapt behavior based on the underlying environment (ANSI support, Unicode capability, color depth, etc.).

### See Also

* interface [IProfileDrive](./IProfileDrive.md)
* namespace [PromptPlusLibrary](../PromptPlus.md)

<!-- DO NOT EDIT: generated by xmldocmd for PromptPlus.dll -->
