// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

namespace PromptPlusLibrary
{
    /// <summary>
    /// Defines mutable setup values used to configure a console profile before it is materialized.
    /// </summary>
    /// <remarks>
    /// An implementation collects environment and user preferences (colors, margins and overflow behavior)
    /// that will later be applied to create an immutable runtime profile (<see cref="IProfileDrive"/>).
    /// </remarks>
    /// <seealso cref="IProfileDrive"/>
    public interface IProfileSetup
    {
        /// <summary>
        /// Gets or sets the default foreground <see cref="Color"/> applied when no explicit color is specified.
        /// </summary>
        /// <value>The default foreground color.</value>
        Color DefaultConsoleForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the default background <see cref="Color"/> applied when no explicit color is specified.
        /// </summary>
        /// <value>The default background color.</value>
        Color DefaultConsoleBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the left screen margin (number of leading spaces reserved).
        /// </summary>
        /// <value>The left padding in character cells.</value>
        byte PadLeft { get; set; }

        /// <summary>
        /// Gets or sets the right screen margin (number of trailing spaces reserved).
        /// </summary>
        /// <value>The right padding in character cells.</value>
        byte PadRight { get; set; }

        /// <summary>
        /// Gets or sets the strategy to apply when rendered text exceeds the available width.
        /// </summary>
        /// <value>The overflow handling strategy.</value>
        Overflow OverflowStrategy { get; set; }
    }
}
