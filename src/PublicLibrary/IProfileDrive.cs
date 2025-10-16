// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the interface for profile for console.
    /// </summary>
    public interface IProfileDrive
    {
        /// <summary>
        /// Gets the default console foreground color.
        /// </summary>
        Color DefaultConsoleForegroundColor { get; }

        /// <summary>
        /// Gets the default console background color.
        /// </summary>
        Color DefaultConsoleBackgroundColor { get; }

        /// <summary>
        /// Gets the profile name.
        /// </summary>
        string ProfileName { get; }

        /// <summary>
        /// Gets a value indicating whether the console is a terminal.
        /// </summary>
        bool IsTerminal { get; }

        /// <summary>
        /// Gets a value indicating whether the console is a legacy console.
        /// </summary>
        bool IsLegacy { get; }

        /// <summary>
        /// Gets a value indicating whether the console supports Unicode.
        /// </summary>
        bool IsUnicodeSupported { get; }

        /// <summary>
        /// Gets a value indicating whether the console supports ANSI.
        /// </summary>
        bool SupportsAnsi { get; }

        /// <summary>
        /// Gets the color capacity.
        /// </summary>
        /// <seealso cref="ColorSystem"/>
        ColorSystem ColorDepth { get; }

        /// <summary>
        /// Gets the screen margin left.
        /// </summary>
        byte PadLeft { get; }

        /// <summary>
        /// Gets the screen margin right.
        /// </summary>
        byte PadRight { get; }

        /// <summary>
        /// Gets the width of the buffer area.
        /// </summary>
        int BufferWidth { get; }

        /// <summary>
        /// Gets the height of the buffer area.
        /// </summary>
        int BufferHeight { get; }

        /// <summary>
        /// Gets the display text overflow strategy.
        /// </summary>
        Overflow OverflowStrategy { get; }
    }
}
