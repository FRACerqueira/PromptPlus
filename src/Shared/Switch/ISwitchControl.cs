// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running a Switch control that lets the user
    /// toggle a boolean value between <c>on</c> and <c>false</c> (off) states.
    /// </summary>
    /// <remarks>
    /// The user moves between the two states by pressing the Left/Right arrow keys or the
    /// Space bar. Each state is displayed as a configurable label or emoji. When history is
    /// enabled, the last confirmed value is persisted and can be pre-loaded on the next run.
    /// Call <see cref="Run(CancellationToken)"/> last to display the control and read the
    /// chosen value.
    /// </remarks>
    public interface ISwitchControl
    {
        /// <summary>
        /// Applies shared control options (such as prompt text, tooltip visibility, and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        ISwitchControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the Switch control.
        /// </summary>
        /// <param name="styleType">The <see cref="SwitchStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl Styles(SwitchStyles styleType, Style style);

        /// <summary>
        /// Sets the initial value displayed when the control opens. Default is <c>false</c> (off).
        /// </summary>
        /// <param name="value">The initial boolean value: <c>true</c> for on, <c>false</c> for off.</param>
        /// <param name="useDefaultHistory">When <c>true</c> (default) and history is enabled via <see cref="EnableHistory(string, Action{IHistoryOptions}?)"/>, the last confirmed value stored in history is used instead of <paramref name="value"/>.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        ISwitchControl Default(bool value, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the label displayed for the <c>off</c> (false) state, replacing the default localized text.
        /// </summary>
        /// <param name="value">The text to show when the switch is off.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl OffValue(string value);

        /// <summary>
        /// Sets the label for the <c>off</c> (false) state using an emoji, with a plain-text fallback
        /// for terminals that do not support emoji rendering.
        /// </summary>
        /// <param name="emojiName">The emoji to display for the off state.</param>
        /// <param name="fallbacktext">The plain-text label used when the emoji cannot be rendered.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl OffValue(EmojiName emojiName, string fallbacktext);

        /// <summary>
        /// Sets the label displayed for the <c>on</c> (true) state, replacing the default localized text.
        /// </summary>
        /// <param name="value">The text to show when the switch is on.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl OnValue(string value);

        /// <summary>
        /// Sets the label for the <c>on</c> (true) state using an emoji, with a plain-text fallback
        /// for terminals that do not support emoji rendering.
        /// </summary>
        /// <param name="emojiName">The emoji to display for the on state.</param>
        /// <param name="fallbacktext">The plain-text label used when the emoji cannot be rendered.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        ISwitchControl OnValue(EmojiName emojiName, string fallbacktext);

        /// <summary>
        /// Enables value history, persisting the confirmed boolean to a file so it can be
        /// reloaded as the default on the next run.
        /// </summary>
        /// <param name="filename">The name of the file used to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An optional callback to configure the <see cref="IHistoryOptions"/> (expiration, max items, etc.).</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="filename"/> is <c>null</c>.</exception>
        ISwitchControl EnableHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Updates the description text dynamically based on the current switch state.
        /// </summary>
        /// <param name="value">A function that receives the current boolean value and returns the description to display. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="ISwitchControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ISwitchControl ChangeDescription(Func<bool, string> value);

        /// <summary>
        /// Asynchronous version of <see cref="ChangeDescription(Func{bool, string})"/> that updates the description
        /// text according to the current value (useful when the text comes from an asynchronous source).
        /// </summary>
        /// <param name="value">A function that receives the current value and asynchronously returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ISwitchControl"/> instance, so additional settings can be chained.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ISwitchControl ChangeDescriptionAsync(Func<bool, Task<string>> value);

        /// <summary>
        /// Displays the Switch control and blocks until the user confirms or cancels, returning the chosen state.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the prompt while it is waiting for input. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the confirmed <see cref="bool"/> value (<c>true</c> = on, <c>false</c> = off), or an aborted result if the user cancels.</returns>
        ResultPrompt<bool?> Run(CancellationToken token = default);
    }
}
