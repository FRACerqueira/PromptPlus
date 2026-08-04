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
    /// Provides a fluent API for configuring and running a secret (masked) text input control.
    /// </summary>
    /// <remarks>
    /// Each typed character is replaced on screen by a mask symbol so the actual value is never
    /// visible while typing. The user can optionally toggle plain-text visibility with F2 if
    /// <see cref="MaskSecret(char?, bool)"/> is called with <c>enabledView = true</c> (the default).
    /// Every configuration method returns the same <see cref="IInputSecretControl"/> instance so
    /// the calls can be chained (fluent style). Call <see cref="Run(CancellationToken)"/> last to
    /// display the control and read the submitted value.
    /// </remarks>
    public interface IInputSecretControl
    {
        /// <summary>
        /// Forces entered text to follow the specified casing rule.
        /// Default is <see cref="CaseOptions.Any"/> (no transformation).
        /// </summary>
        /// <param name="value">The case transformation option to apply.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        IInputSecretControl InputToCase(CaseOptions value);

        /// <summary>
        /// Sets a filter that validates each typed character before it is added to the input.
        /// </summary>
        /// <remarks>If the callback returns <c>true</c>, the character is accepted; otherwise, it is ignored.</remarks>
        /// <param name="value">A function that receives a character and returns <c>true</c> to accept it, or <c>false</c> to ignore it.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputSecretControl AcceptInput(Func<char, bool> value);

        /// <summary>
        /// Sets the masking character used to hide each typed character on screen.
        /// </summary>
        /// <remarks>
        /// While the user types, every character is replaced by the mask symbol. When
        /// <paramref name="enabledView"/> is <c>true</c>, pressing F2 toggles between masked
        /// and plain-text views so the user can verify what was typed.
        /// </remarks>
        /// <param name="value">The character used as the mask symbol. Defaults to <c>'#'</c> when <c>null</c>.</param>
        /// <param name="enabledView">If <c>true</c> (default), the user can press F2 to reveal or hide the typed text.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        IInputSecretControl MaskSecret(char? value = null, bool enabledView = true);

        /// <summary>
        /// Limits the number of characters that can be entered.
        /// Default is zero (no limit).
        /// </summary>
        /// <param name="maxLength">The maximum number of characters allowed for the input.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        IInputSecretControl MaxLength(int maxLength);

        /// <summary>
        /// Sets a validation function executed when the user confirms the input.
        /// </summary>
        /// <param name="value">A predicate that returns whether the submitted value is valid.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        IInputSecretControl PredicateValid(Func<string, bool> value);

        /// <summary>
        /// Sets an asynchronous validation function executed when the user confirms the input.
        /// </summary>
        /// <param name="value">An asynchronous predicate that returns whether the submitted value is valid.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IInputSecretControl PredicateValidAsync(Func<string, Task<bool>> value);

        /// <summary>
        /// Sets a validation function that also returns a custom validation message.
        /// </summary>
        /// <param name="value">A predicate that returns a tuple: the first value indicates validity, and the second is an optional error message.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        IInputSecretControl PredicateValid(Func<string, (bool, string?)> value);

        /// <summary>
        /// Sets an asynchronous validation function that also returns a custom validation message.
        /// </summary>
        /// <param name="value">An asynchronous predicate that returns a tuple: the first value indicates validity, and the second is an optional error message.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IInputSecretControl PredicateValidAsync(Func<string, Task<(bool, string?)>> value);

        /// <summary>
        /// Updates the control description dynamically using a synchronous callback.
        /// </summary>
        /// <param name="value">A function that receives the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputSecretControl ChangeDescription(Func<string, string> value);

        /// <summary>
        /// Updates the control description dynamically using an asynchronous callback.
        /// </summary>
        /// <param name="value">A function that asynchronously receives the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputSecretControl ChangeDescriptionAsync(Func<string, Task<string>> value);

        /// <summary>
        /// Overrides one visual style used by the input control.
        /// </summary>
        /// <param name="styleType">The <see cref="InputStyles"/> to override.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        IInputSecretControl Styles(InputStyles styleType, Style style);

        /// <summary>
        /// Applies additional control options using a synchronous configuration callback.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputSecretControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IInputSecretControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Displays the secret input control and blocks until the user confirms or cancels, returning the submitted text.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the prompt while it is waiting for input. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the submitted <see cref="string"/> value, or an aborted result if the user cancels.</returns>
        ResultPrompt<string> Run(CancellationToken token = default);
    }
}
