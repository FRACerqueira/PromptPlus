// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Defines console interaction and rendering capabilities combined with a profile (<see cref="IProfileDrive"/>).
    /// </summary>
    /// <remarks>
    /// This interface abstracts terminal features (cursor control, colors, encoding, buffering, input/output streams,
    /// and multi‑screen support). Implementations should adapt behavior based on the underlying environment
    /// (ANSI support, Unicode capability, color depth, etc.).
    /// </remarks>
    /// <seealso cref="IProfileDrive"/>
    public interface IConsole : IProfileDrive
    {

        /// <summary>
        /// Sets a handler for console cancel events (Ctrl+C/Break).
        /// </summary>
        /// <param name="behaviorcontrols">
        /// A value from <see cref="AfterCancelKeyPress"/> enum determining how the console should handle cancel events:
        /// <list type="bullet">
        /// <item><description>Ignore - Continue running without aborting</description></item>
        /// <item><description>AbortCurrentControl - Abort only the current control's operation</description></item>
        /// <item><description>AbortAllControl - Abort all control's operation</description></item>
        /// </list>
        /// </param>
        /// <param name="actionhandle">A delegate that handles cancel events. The handler receives:
        /// <list type="bullet">
        /// <item><description>sender - The source of the event</description></item>
        /// <item><description><see cref="ConsoleCancelEventArgs"/> - Event data including cancel type and whether the event was handled</description></item>
        /// </list>
        /// Set to <c>null</c> to remove the current handler.</param>
        /// <remarks>
        /// <para>The handler's behavior is affected by the <see cref="BehaviorAfterCancelKeyPress"/> property setting.</para>
        /// <para>This method is not supported on the following platforms:</para>
        /// <list type="bullet">
        /// <item><description>Android</description></item>
        /// <item><description>Browser</description></item>
        /// <item><description>iOS</description></item>
        /// <item><description>tvOS</description></item>
        /// </list>
        /// </remarks>
        /// <seealso cref="BehaviorAfterCancelKeyPress"/>
        /// <seealso cref="UserPressKeyAborted"/>

        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("tvos")]
        void CancelKeyPress(AfterCancelKeyPress behaviorcontrols, Action<object?, ConsoleCancelEventArgs> actionhandle);

        /// <summary>
        /// Removes the current cancel key press (Ctrl+C/Break) handler and restores default behavior.
        /// </summary>
        /// <remarks>
        /// <para>This method remove handler previously set via <see cref="CancelKeyPress"/>.</para>
        /// <para>After calling this method:</para>
        /// <list type="bullet">
        /// <item><description>The <see cref="BehaviorAfterCancelKeyPress"/> setting will have no effect</description></item>
        /// <item><description>The <see cref="UserPressKeyAborted"/> setting to <c>false</c>.</description></item>
        /// </list>
        /// </remarks>
        /// <seealso cref="CancelKeyPress"/>
        /// <seealso cref="BehaviorAfterCancelKeyPress"/>
        /// <seealso cref="UserPressKeyAborted"/>
        void RemoveCancelKeyPress();

        /// <summary>
        /// Gets the behavior to be applied after a cancel key (Ctrl+C/Ctrl+Break) is pressed.
        /// This setting is ignored if no handler is configured via <see cref="CancelKeyPress"/> or returns <c>false</c> from <see cref="CancelKeyPress"/>.
        /// </summary>
        /// <value>
        /// A value from <see cref="AfterCancelKeyPress"/> enum determining how the console should 
        /// handle cancel events:
        /// <list type="bullet">
        /// <item><description>Ignore - Continue running without aborting</description></item>
        /// <item><description>AbortCurrentControl - Abort only the current control's operation</description></item>
        /// </list>
        /// </value>
        /// <seealso cref="CancelKeyPress"/>
        /// <seealso cref="UserPressKeyAborted"/>
        AfterCancelKeyPress BehaviorAfterCancelKeyPress { get; }

        /// <summary>
        /// Gets a value indicating Enabled Exclusive ontext for controls/wdgets and commands console. Default value is <c>false</c>.
        /// </summary>
        bool EnabledExclusiveContext { get; set; }


        /// <summary>
        /// Gets a value indicating whether the operation was aborted by the user (Ctrl+C / Ctrl+Break).
        /// </summary>
        bool UserPressKeyAborted { get; }

        /// <summary>
        /// Gets or sets the current foreground <see cref="Color"/>.
        /// </summary>
        /// <value>The active foreground color.</value>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the current background <see cref="Color"/>.
        /// </summary>
        /// <value>The active background color.</value>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Sets the default foreground and background console colors used when resetting.
        /// </summary>
        /// <param name="foreground">The default foreground <see cref="Color"/>.</param>
        /// <param name="background">The default background <see cref="Color"/>.</param>
        void DefaultColors(Color foreground, Color background);

        /// <summary>
        /// Resets the current colors to the configured defaults.
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        /// <value><c>true</c> if the cursor is visible; otherwise <c>false</c>.</value>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets the column (left) position of the cursor within the buffer.
        /// </summary>
        /// <value>The zero-based cursor column.</value>
        int CursorLeft { get; }

        /// <summary>
        /// Gets the row (top) position of the cursor within the buffer.
        /// </summary>
        /// <value>The zero-based cursor row.</value>
        int CursorTop { get; }

        /// <summary>
        /// Sets the cursor position.
        /// </summary>
        /// <param name="left">Zero-based column (0 is the leftmost).</param>
        /// <param name="top">Zero-based row (0 is the topmost).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="left"/> or <paramref name="top"/> is outside the valid buffer range.
        /// </exception>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Gets the current cursor position.
        /// </summary>
        /// <returns>A tuple with <c>Left</c> (column) and <c>Top</c> (row).</returns>
        (int Left, int Top) GetCursorPosition();

        /// <summary>
        /// Hides the cursor.
        /// </summary>
        void HideCursor();

        /// <summary>
        /// Shows the cursor.
        /// </summary>
        void ShowCursor();

        /// <summary>
        /// Reads the next key press.
        /// </summary>
        /// <param name="intercept">
        /// <c>true</c> to suppress echoing the key; <c>false</c> to display it.
        /// </param>
        /// <returns>
        /// A <see cref="ConsoleKeyInfo"/> containing the key, character (if any), and modifier state.
        /// </returns>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// Gets a value indicating whether a key press is available.
        /// </summary>
        /// <value><c>true</c> if a key is pending; otherwise <c>false</c>.</value>
        bool KeyAvailable { get; }

        /// <summary>
        /// Reads a line of text from the input stream.
        /// </summary>
        /// <returns>
        /// The line without the line terminator; <c>null</c> if end of stream is reached.
        /// </returns>
        string? ReadLine();

        /// <summary>
        /// Gets a value indicating whether standard input is redirected.
        /// </summary>
        /// <value><c>true</c> if input is redirected; otherwise <c>false</c>.</value>
        bool IsInputRedirected { get; }

        /// <summary>
        /// Gets or sets the encoding for standard input.
        /// </summary>
        /// <value>The input <see cref="Encoding"/>.</value>
        Encoding InputEncoding { get; set; }

        /// <summary>
        /// Gets the standard input reader.
        /// </summary>
        /// <value>The <see cref="TextReader"/> for input.</value>
        TextReader In { get; }

        /// <summary>
        /// Sets the standard input source.
        /// </summary>
        /// <param name="value">The new input <see cref="TextReader"/>.</param>
        void SetIn(TextReader value);

        /// <summary>
        /// Gets a value indicating whether standard output is redirected.
        /// </summary>
        /// <value><c>true</c> if output is redirected; otherwise <c>false</c>.</value>
        bool IsOutputRedirected { get; }

        /// <summary>
        /// Gets a value indicating whether standard error is redirected.
        /// </summary>
        /// <value><c>true</c> if error is redirected; otherwise <c>false</c>.</value>
        bool IsErrorRedirected { get; }

        /// <summary>
        /// Gets or sets the encoding for standard output.
        /// </summary>
        /// <value>The output <see cref="Encoding"/>.</value>
        Encoding OutputEncoding { get; set; }

        /// <summary>
        /// Gets the standard output writer.
        /// </summary>
        /// <value>The <see cref="TextWriter"/> for output.</value>
        TextWriter Out { get; }

        /// <summary>
        /// Gets the standard error writer.
        /// </summary>
        /// <value>The <see cref="TextWriter"/> for error output.</value>
        TextWriter Error { get; }

        /// <summary>
        /// Sets the standard output writer.
        /// </summary>
        /// <param name="value">The new output <see cref="TextWriter"/>.</param>
        void SetOut(TextWriter value);

        /// <summary>
        /// Sets the standard error writer.
        /// </summary>
        /// <param name="value">The new error <see cref="TextWriter"/>.</param>
        void SetError(TextWriter value);

        /// <summary>
        /// Clears the buffer (and visible window) and resets cursor to (0,0).
        /// </summary>
        void Clear();

        /// <summary>
        /// Emits an audible beep if supported.
        /// </summary>
        void Beep();

        /// <summary>
        /// Writes a character array.
        /// </summary>
        /// <param name="buffer">Characters to write.</param>
        /// <param name="style">Optional <see cref="Style"/> overriding current output style.</param>
        /// <param name="clearrestofline"><c>true</c> to clear remaining characters on the line.</param>
        void Write(char[] buffer, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Writes a single character.
        /// </summary>
        /// <param name="buffer">Character to write.</param>
        /// <param name="style">Optional <see cref="Style"/> overriding current output style.</param>
        /// <param name="clearrestofline"><c>true</c> to clear remaining characters on the line.</param>
        void Write(char buffer, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Writes a string.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <param name="style">Optional <see cref="Style"/> overriding current output style.</param>
        /// <param name="clearrestofline"><c>true</c> to clear remaining characters on the line.</param>
        void Write(string value, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Writes a character array followed by a line terminator.
        /// </summary>
        /// <param name="buffer">Characters to write.</param>
        /// <param name="style">Optional <see cref="Style"/> overriding current output style.</param>
        /// <param name="clearrestofline"><c>true</c> to clear remaining characters on the line before newline.</param>
        void WriteLine(char[] buffer, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Writes a single character followed by a line terminator.
        /// </summary>
        /// <param name="buffer">Character to write.</param>
        /// <param name="style">Optional <see cref="Style"/> overriding current output style.</param>
        /// <param name="clearrestofline"><c>true</c> to clear remaining characters on the line before newline.</param>
        void WriteLine(char buffer, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Writes a string followed by a line terminator.
        /// </summary>
        /// <param name="value">String to write.</param>
        /// <param name="style">Optional <see cref="Style"/> overriding current output style.</param>
        /// <param name="clearrestofline"><c>true</c> to clear remaining characters on the line before newline.</param>
        void WriteLine(string value, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Gets the currently active screen buffer.
        /// </summary>
        /// <value>The active <see cref="TargetScreen"/>.</value>
        TargetScreen CurrentBuffer { get; }

        /// <summary>
        /// Attempts to switch to a target screen buffer.
        /// </summary>
        /// <param name="value">Target buffer.</param>
        /// <returns><c>true</c> if the switch succeeded; otherwise <c>false</c>.</returns>
        bool SwapBuffer(TargetScreen value);

        /// <summary>
        /// Executes an action on a target buffer and then restores the original buffer.
        /// </summary>
        /// <param name="target">The target buffer.</param>
        /// <param name="value">Action to execute while the buffer is active.</param>
        /// <param name="defaultforecolor">Optional temporary default foreground <see cref="ConsoleColor"/>.</param>
        /// <param name="defaultbackcolor">Optional temporary default background <see cref="ConsoleColor"/>.</param>
        /// <param name="cancellationToken">Cancellation token for the action.</param>
        /// <returns><c>true</c> if executed on the requested buffer; otherwise <c>false</c>.</returns>
        bool OnBuffer(
            TargetScreen target,
            Action<CancellationToken> value,
            ConsoleColor? defaultforecolor = null,
            ConsoleColor? defaultbackcolor = null,
            CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a value indicating whether screen swapping is supported.
        /// </summary>
        /// <value><c>true</c> if buffer swapping is enabled; otherwise <c>false</c>.</value>
        bool IsEnabledSwapScreen { get; }
    }
}
