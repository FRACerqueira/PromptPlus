// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the interface for profile setup for console.
    /// </summary>
    public interface IProfileSetup
    {
        /// <summary>
        /// Get/Set the default console foreground color.
        /// </summary>
        Color DefaultConsoleForegroundColor { get; set; }

        /// <summary>
        /// Get/Set the default console background color.
        /// </summary>
        Color DefaultConsoleBackgroundColor { get; set; }

        /// <summary>
        /// Get/Set the screen margin left.
        /// </summary>
        byte PadLeft { get; set; }

        /// <summary>
        /// Get/Set the screen margin right.
        /// </summary>
        byte PadRight { get; set; }

        /// <summary>
        /// Get/Set the display text overflow strategy.
        /// </summary>
        Overflow OverflowStrategy { get; set; }

    }
}
