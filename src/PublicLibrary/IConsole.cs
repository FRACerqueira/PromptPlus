// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the interface for console.
    /// </summary>
    public interface IConsole : IProfileDrive
    {
        /// <summary>
        /// Get/set console ForegroundColor
        /// </summary>
        Color ForegroundColor { get; set; }

        /// <summary>
        /// Get/set console BackgroundColor
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Set the default foreground and background console colors.
        /// </summary>
        /// <param name="foreground">The default console ForegroundColor</param>
        /// <param name="background">The default console BackgroundColor</param>
        void DefaultColors(Color foreground, Color background);

        /// <summary>
        /// Reset colors to default values.
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets the column position of the cursor within the buffer area.
        /// </summary>
        int CursorLeft { get; }

        /// <summary>
        /// Gets the row position of the cursor within the buffer area.
        /// </summary>
        int CursorTop { get; }

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="left"/> or  <paramref name="top"/> is out of range screen.</exception>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Gets the current position of the cursor in the buffer area.
        /// </summary>
        /// <returns>The column and row of the cursor.</returns>
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
        /// Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window. true to not display the pressed key; otherwise, false.</param>
        /// <returns>
        /// An object that describes the System.ConsoleKey constant and Unicode character,
        /// if any, that correspond to the pressed console key. The System.ConsoleKeyInfo
        /// also describes, in a bitwise combination of System.ConsoleModifiers values,
        /// whether one or more Shift, Alt, or Ctrl modifier keys were pressed simultaneously
        /// with the console key.
        /// </returns>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        bool KeyAvailable { get; }

        /// <summary>
        /// Reads a line from the input stream. A line is defined as a sequence of characters followed by
        /// a carriage return ('\r'), a line feed ('\n'), or a carriage return
        /// immediately followed by a line feed. The resulting string does not
        /// contain the terminating carriage return and/or line feed.
        /// </summary>
        /// <returns>
        /// The returned value is null if the end of the input stream has been reached.
        /// </returns>
        string? ReadLine();

        /// <summary>
        /// Gets a value that indicates whether input has been redirected from the standard input stream.
        /// </summary>
        bool IsInputRedirected { get; }

        /// <summary>
        /// Gets or sets the encoding for the standard input stream.
        /// </summary>
        Encoding InputEncoding { get; set; }

        /// <summary>
        /// Gets the standard input stream.
        /// </summary>
        TextReader In { get; }

        /// <summary>
        /// Sets the standard input stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard input.</param>
        void SetIn(TextReader value);

        /// <summary>
        /// Gets the output CodePage.
        /// </summary>
        int CodePage { get; }

        /// <summary>
        /// Gets a value that indicates whether output has been redirected from the standard output stream.
        /// </summary>
        bool IsOutputRedirected { get; }

        /// <summary>
        /// Gets a value that indicates whether error has been redirected from the standard error stream.
        /// </summary>
        bool IsErrorRedirected { get; }

        /// <summary>
        /// Gets or sets an encoding for the standard output stream.
        /// </summary>
        Encoding OutputEncoding { get; set; }

        /// <summary>
        /// Gets the standard output stream.
        /// </summary>
        TextWriter Out { get; }

        /// <summary>
        /// Gets the standard error stream.
        /// </summary>
        TextWriter Error { get; }

        /// <summary>
        /// Sets the standard output stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard output.</param>
        void SetOut(TextWriter value);

        /// <summary>
        /// Sets the standard error stream.
        /// </summary>
        /// <param name="value">A stream that is the new standard error.</param>
        void SetError(TextWriter value);

        /// <summary>
        /// Clears the console buffer and corresponding console window of display information.
        /// Moves cursor to the top of the console.
        /// </summary>
        void Clear();

        /// <summary>
        /// Plays the sound of a beep through the console speaker.
        /// </summary>
        void Beep();

        /// <summary>
        /// Writes the text representation of a character array to the standard output stream.
        /// </summary>
        /// <param name="buffer">A character array to write.</param>
        /// <param name="style">The <see cref="Style"/> overwrite style current output</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) Write(char[] buffer, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Writes the text representation of a character to the standard output stream.
        /// </summary>
        /// <param name="buffer">A character array to write.</param>
        /// <param name="style">The <see cref="Style"/> overwrite style current output</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) Write(char buffer, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Writes the text representation of a string value to the standard output stream.
        /// </summary>
        /// <param name="value">A string value to write.</param>
        /// <param name="style">The <see cref="Style"/> overwrite style current output</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) Write(string value, Style? style = null, bool clearrestofline = false);

        /// <summary>
        /// Writes the text representation of a string value with token Colors to the standard output stream.
        /// </summary>
        /// <param name="value">A string value with token Colors to write.</param>
        /// <param name="overflow">The <see cref="Overflow"/> Strategy</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) WriteColor(string value, Overflow overflow = Overflow.Crop, bool clearrestofline = false);

        /// <summary>
        /// Writes the text representation of a character array to the standard output stream with line terminator.
        /// </summary>
        /// <param name="buffer">A character array to write.</param>
        /// <param name="style">The <see cref="Style"/> overwrite style current output</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) WriteLine(char[] buffer, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Writes the text representation of a character to the standard output stream with line terminator.
        /// </summary>
        /// <param name="buffer">A character array to write.</param>
        /// <param name="style">The <see cref="Style"/> overwrite style current output</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) WriteLine(char buffer, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Writes the text representation of a string value to the standard output stream with line terminator.
        /// </summary>
        /// <param name="value">A string value to write.</param>
        /// <param name="style">The <see cref="Style"/> overwrite style current output</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) WriteLine(string value, Style? style = null, bool clearrestofline = true);

        /// <summary>
        /// Writes the text representation of a string value with token Colors to the standard output stream with line terminator.
        /// </summary>
        /// <param name="value">A string value with token Colors to write.</param>
        /// <param name="overflow">The <see cref="Overflow"/> Strategy</param>
        /// <param name="clearrestofline">Indicates whether to clear the rest of the line.</param>
        /// <returns>The column and row position of the cursor.</returns>
        (int Left, int Top) WriteLineColor(string value, Overflow overflow = Overflow.Crop, bool clearrestofline = true);

        /// <summary>
        /// Get Current Screen Buffer
        /// </summary>
        TargetScreen CurrentBuffer { get; }

        /// <summary>
        /// Swap Screen Buffer
        /// </summary>
        /// <param name="value">The target buffer</param>
        /// <returns>True when console has capacity to swap to target buffer, otherwise false</returns>
        bool SwapBuffer(TargetScreen value);

        /// <summary>
        /// Run an action on target screen buffer and return to original screen buffer
        /// </summary>
        /// <param name="target">The target buffer</param>
        /// <param name="value">The action</param>        
        /// <param name="defaultforecolor">The default fore color</param>        
        /// <param name="defaultbackcolor">The default back color</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/></param> 
        /// <returns>True when console has capacity to run on target buffer, otherwise false</returns>
        bool OnBuffer(TargetScreen target, Action<CancellationToken> value, ConsoleColor? defaultforecolor = null, ConsoleColor? defaultbackcolor = null, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets a value indicating whether the console supports swapping screens.
        /// </summary>
        bool IsEnabledSwapScreen { get; }
    }
}
