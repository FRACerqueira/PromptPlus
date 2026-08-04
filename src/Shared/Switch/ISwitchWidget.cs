// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and rendering a read-only Switch widget that
    /// displays a boolean on/off state as a visual toggle, without waiting for user interaction.
    /// </summary>
    /// <remarks>
    /// A widget is for display only: unlike <see cref="ISwitchControl"/>, it does not read input.
    /// Call <see cref="Show"/> last to render the switch on the console.
    /// </remarks>
    public interface ISwitchWidget
    {
        /// <summary>
        /// Overrides the visual style applied to a specific region of the Switch widget.
        /// </summary>
        /// <param name="styleType">The <see cref="SwitchStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="ISwitchWidget"/> instance for chaining.</returns>
        ISwitchWidget Styles(SwitchStyles styleType, Style style);

        /// <summary>
        /// Sets the label displayed for the <c>off</c> (false) state, replacing the default localized text.
        /// </summary>
        /// <param name="value">The text to show when the switch is off.</param>
        /// <returns>The current <see cref="ISwitchWidget"/> instance for chaining.</returns>
        ISwitchWidget OffValue(string value);

        /// <summary>
        /// Sets the label for the <c>off</c> (false) state using an emoji, with a plain-text fallback
        /// for terminals that do not support emoji rendering.
        /// </summary>
        /// <param name="emojiName">The emoji to display for the off state.</param>
        /// <param name="fallbacktext">The plain-text label used when the emoji cannot be rendered.</param>
        /// <returns>The current <see cref="ISwitchWidget"/> instance for chaining.</returns>
        ISwitchWidget OffValue(EmojiName emojiName, string fallbacktext);

        /// <summary>
        /// Sets the label displayed for the <c>on</c> (true) state, replacing the default localized text.
        /// </summary>
        /// <param name="value">The text to show when the switch is on.</param>
        /// <returns>The current <see cref="ISwitchWidget"/> instance for chaining.</returns>
        ISwitchWidget OnValue(string value);

        /// <summary>
        /// Sets the label for the <c>on</c> (true) state using an emoji, with a plain-text fallback
        /// for terminals that do not support emoji rendering.
        /// </summary>
        /// <param name="emojiName">The emoji to display for the on state.</param>
        /// <param name="fallbacktext">The plain-text label used when the emoji cannot be rendered.</param>
        /// <returns>The current <see cref="ISwitchWidget"/> instance for chaining.</returns>
        ISwitchWidget OnValue(EmojiName emojiName, string fallbacktext);

        /// <summary>
        /// Renders the Switch widget on the console using the current configuration. Call this method last.
        /// </summary>
        void Show();
    }
}
