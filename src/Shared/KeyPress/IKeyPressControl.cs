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
    /// Provides a fluent API for configuring and running a KeyPress control that waits for the
    /// user to press a single key, optionally restricting which keys (and modifier combinations)
    /// are accepted.
    /// </summary>
    /// <remarks>
    /// When no valid keys are registered via <see cref="AddValidKey"/>, any key is accepted.
    /// When one or more valid keys are registered, the control keeps waiting until the user
    /// presses an accepted key combination; pressing any other key triggers the invalid-key
    /// message (if configured). Call <see cref="Run(CancellationToken)"/> last to display the
    /// control and read the result.
    /// </remarks>
    public interface IKeyPressControl
    {
        /// <summary>
        /// Registers a key (with optional modifier requirement) as a valid input for this control.
        /// </summary>
        /// <remarks>
        /// Multiple calls to <see cref="AddValidKey"/> accumulate accepted combinations. If no valid
        /// keys are registered, any key is accepted. The optional <paramref name="displayText"/> overrides
        /// the key name shown in the tooltip; it is useful when the key has a friendlier alias.
        /// </remarks>
        /// <param name="key">The <see cref="ConsoleKey"/> to accept.</param>
        /// <param name="requiredModifiers">Optional <see cref="ConsoleModifiers"/> that must be held simultaneously. Use <c>null</c> (default) to accept the key without any modifier.</param>
        /// <param name="displayText">Optional label shown to the user in the tooltip instead of the key name.</param>
        /// <returns>The same <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl AddValidKey(ConsoleKey key, ConsoleModifiers? requiredModifiers = null, string? displayText = null);

        /// <summary>
        /// Sets a synchronous callback that builds the error message displayed when the user
        /// presses a key that is not in the accepted set.
        /// </summary>
        /// <remarks>
        /// The callback receives the <see cref="ConsoleKeyInfo"/> of the rejected key and returns
        /// the text to display. Pass <c>null</c> to suppress the error message.
        /// </remarks>
        /// <param name="message">A function that receives the rejected key info and returns the error text, or <c>null</c> to disable the message.</param>
        /// <returns>The same <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl ShowMessage(Func<ConsoleKeyInfo, string>? message);

        /// <summary>
        /// Sets an asynchronous callback that builds the error message displayed when the user
        /// presses a key that is not in the accepted set.
        /// </summary>
        /// <remarks>
        /// The asynchronous callback is evaluated synchronously (blocking) on the UI thread.
        /// It receives the rejected <see cref="ConsoleKeyInfo"/> and a <see cref="CancellationToken"/> tied
        /// to the control's lifetime. Pass <c>null</c> to suppress the error message.
        /// Replaces any previously registered synchronous message callback set via <see cref="ShowMessage"/>.
        /// </remarks>
        /// <param name="message">An async function that receives the rejected key info and a cancellation token, and returns the error text, or <c>null</c> to disable the message.</param>
        /// <returns>The same <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl ShowMessageAsync(Func<ConsoleKeyInfo, CancellationToken, Task<string>>? message = null);

        /// <summary>
        /// Applies shared control options (such as prompt text, tooltip visibility, and abort behavior).
        /// </summary>
        /// <param name="configureOptions">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IKeyPressControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="configureOptions"/> is <c>null</c>.</exception>
        IKeyPressControl Options(Action<IControlOptions> configureOptions);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the KeyPress control.
        /// </summary>
        /// <param name="styleType">The <see cref="KeyPressStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The same <see cref="IKeyPressControl"/> instance for chaining.</returns>
        IKeyPressControl Styles(KeyPressStyles styleType, Style style);


        /// <summary>
        /// Displays the KeyPress control and blocks until the user presses an accepted key or cancels.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the wait. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the <see cref="ConsoleKeyInfo"/> of the accepted key, or an aborted result if the user cancels.</returns>
        ResultPrompt<ConsoleKeyInfo?> Run(CancellationToken cancellationToken = default);
    }
}
