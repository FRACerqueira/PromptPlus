// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a keypress control.
    /// </summary>
    public interface IKeyPressControl
    {
        /// <summary>
        /// Adds a valid key and optional modifiers for keypress.
        /// </summary>
        /// <param name="key">The key, <see cref="ConsoleKey"/>.</param>
        /// <param name="modifiers">The optional modifiers, <see cref="ConsoleModifiers"/>.</param>
        /// <param name="showtext">The text to overwrite the default <see cref="ConsoleKey"/> string.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl AddKeyValid(ConsoleKey key, ConsoleModifiers? modifiers = null, string? showtext = null);


        /// <summary>
        /// Creates a key press control that waits for user input for the specified duration, returning a default key
        /// and modifiers if no input is received within the timeout period.
        /// </summary>
        /// <param name="time">The maximum amount of time to wait for a key press before returning the default key and modifiers. Must be a
        /// non-negative duration.</param>
        /// <param name="defaultkey">The key to return if the timeout elapses without user input.</param>
        /// <param name="defaultmodifiers">The modifier keys (such as Shift, Alt, or Control) to associate with the default key if the timeout elapses.
        /// If null, no modifiers are applied.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>

        IKeyPressControl Timeout(TimeSpan time, ConsoleKey defaultkey, ConsoleModifiers? defaultmodifiers = null);

        /// <summary>
        /// Creates a key press control that waits for user input for the specified duration in milliseconds, returning a default key
        /// and modifiers if no input is received within the timeout period.
        /// </summary>
        /// <param name="milliseconds">The maximum amount of time to wait for a key press before returning the default key and modifiers. Must be a
        /// non-negative duration.</param>
        /// <param name="defaultkey">The key to return if the timeout elapses without user input.</param>
        /// <param name="defaultmodifiers">The modifier keys (such as Shift, Alt, or Control) to associate with the default key if the timeout elapses.
        /// If null, no modifiers are applied.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl Timeout(int milliseconds, ConsoleKey defaultkey, ConsoleModifiers? defaultmodifiers = null) => Timeout(TimeSpan.FromMilliseconds(milliseconds), defaultkey, defaultmodifiers);

        /// <summary>
        /// Defines whether to show countdown elapsed time. Default is true.
        /// </summary>
        /// <param name="value">
        /// If true, shows countdown elapsed time.
        /// The interval in milliseconds for updating the countdown display. Default is 500 milliseconds.
        /// </param>
        /// <returns>The current <see cref="IWaitTimerControl"/> instance for chaining.</returns>
        IKeyPressControl ShowCountDown(bool value = true);

        /// <summary>
        /// Configures whether invalid keypresses should be displayed to the user.
        /// </summary>
        /// <param name="value">If <c>true</c>, invalid keypresses will be shown; otherwise, they will be hidden.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl ShowInvalidKey(bool value = true);

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IKeyPressControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the keypress.
        /// </summary>
        /// <param name="styleType">The <see cref="KeyPressStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IKeyPressControl Styles(KeyPressStyles styleType, Style style);

        /// <summary>
        /// Shows a <see cref="SpinnersType"/> animation at the end of the prompt.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The current <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Runs the keypress control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the keypress control execution.</returns>
        ResultPrompt<ConsoleKeyInfo?> Run(CancellationToken token = default);
    }
}
