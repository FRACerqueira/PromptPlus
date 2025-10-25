    // ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Defines a console profile describing capabilities, dimensions, colors and display behavior
    /// for the current console/terminal session.
    /// </summary>
    /// <remarks>
    /// An implementation should provide immutable (snapshot) values representing the environment
    /// at the time it was created. These values can be used to adapt rendering (color depth,
    /// ANSI/Unicode support, margins, buffer size and overflow strategy).
    /// </remarks>
    public interface IProfileDrive
    {
        /// <summary>
        /// Gets the default foreground <see cref="Color"/> used when no explicit color is specified.
        /// </summary>
        /// <value>The default foreground color.</value>
        Color DefaultConsoleForegroundColor { get; }

        /// <summary>
        /// Gets the default background <see cref="Color"/> used when no explicit color is specified.
        /// </summary>
        /// <value>The default background color.</value>
        Color DefaultConsoleBackgroundColor { get; }

        /// <summary>
        /// Gets the profile name (e.g. an identifier for the terminal type or configuration).
        /// </summary>
        /// <value>The logical name of this profile.</value>
        string ProfileName { get; }

        /// <summary>
        /// Gets a value indicating whether the output device is a terminal (TTY) rather than redirected.
        /// </summary>
        /// <value><c>true</c> if running on an interactive terminal; otherwise <c>false</c>.</value>
        bool IsTerminal { get; }

        /// <summary>
        /// Gets a value indicating whether the console is a legacy console lacking modern features
        /// (e.g. full ANSI or extended color support).
        /// </summary>
        /// <value><c>true</c> if the console is considered legacy; otherwise <c>false</c>.</value>
        bool IsLegacy { get; }

        /// <summary>
        /// Gets a value indicating whether Unicode output is fully supported.
        /// </summary>
        /// <value><c>true</c> if Unicode is supported; otherwise <c>false</c>.</value>
        bool IsUnicodeSupported { get; }

        /// <summary>
        /// Gets a value indicating whether ANSI escape sequences are supported for styling/output.
        /// </summary>
        /// <value><c>true</c> if ANSI sequences are supported; otherwise <c>false</c>.</value>
        bool SupportsAnsi { get; }

        /// <summary>
        /// Gets the color depth (capability) of the console.
        /// </summary>
        /// <value>The supported <see cref="ColorSystem"/>.</value>
        /// <seealso cref="ColorSystem"/>
        ColorSystem ColorDepth { get; }

        /// <summary>
        /// Gets the left margin (number of spaces reserved at the left edge).
        /// </summary>
        /// <value>The left padding (column offset).</value>
        byte PadLeft { get; }

        /// <summary>
        /// Gets the right margin (number of spaces reserved at the right edge).
        /// </summary>
        /// <value>The right padding.</value>
        byte PadRight { get; }

        /// <summary>
        /// Gets the width of the console buffer (in character cells).
        /// </summary>
        /// <value>The buffer width.</value>
        int BufferWidth { get; }

        /// <summary>
        /// Gets the height of the console buffer (in rows).
        /// </summary>
        /// <value>The buffer height.</value>
        int BufferHeight { get; }

        /// <summary>
        /// Gets the strategy used when rendered text exceeds the available width.
        /// </summary>
        /// <value>The overflow handling strategy.</value>
        Overflow OverflowStrategy { get; }
    }
}
