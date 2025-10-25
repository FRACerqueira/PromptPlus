// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary.Core
{
    /// <summary>
    /// Represents the profile setup for the console.
    /// </summary>
    internal sealed class ProfileSetup : IProfileSetup
    {
        /// <summary>
        /// Get/Set the default console foreground color.
        /// </summary>
        public Color DefaultConsoleForegroundColor { get; set; }
        /// <summary>
        /// Get/Set the default console background color.
        /// </summary>
        public Color DefaultConsoleBackgroundColor { get; set; }

        /// <summary>
        /// Get/Set the screen margin left.
        /// </summary>
        public byte PadLeft { get; set; }
        /// <summary>
        /// Get/Set the screen margin right.
        /// </summary>
        public byte PadRight { get; set; }
        /// <summary>
        /// Get/Set the display text overflow strategy.
        /// </summary>
        public Overflow OverflowStrategy { get; set; }
    }
}
