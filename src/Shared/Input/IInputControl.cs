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
    /// Provides a fluent API for configuring and running an interactive text input control.
    /// </summary>
    /// <remarks>
    /// The user types free text that is shown in plain view (for hidden/secret input use
    /// <see cref="IInputSecretControl"/> instead). Features include optional character filtering,
    /// case coercion, max-length enforcement, Tab/Shift+Tab autocomplete suggestions, F3 history
    /// navigation, and confirmation-time validation. Call <see cref="Run(CancellationToken)"/>
    /// last to display the control and read the submitted value.
    /// </remarks>
    public interface IInputControl
    {

        /// <summary>
        /// Sets the initial text displayed before the user starts typing.
        /// </summary>
        /// <param name="value">The initial value to display. Cannot be <c>null</c>.</param>
        /// <param name="useDefaultHistory">If <c>true</c> and history is enabled with <see cref="EnabledHistory(string, Action{IHistoryOptions}?)"/>, the most recent history value is preferred; otherwise, <paramref name="value"/> is used.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IInputControl Default(string value, bool useDefaultHistory = true);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the fallback value returned when the user submits without typing any text.
        /// </summary>
        /// <param name="value">The default value to use when the input is empty.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl DefaultIfEmpty(string value);

        /// <summary>
        /// Forces entered text to follow the specified casing rule.
        /// Default is <see cref="CaseOptions.Any"/> (no transformation).
        /// </summary>
        /// <param name="value">The case transformation option to apply.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl InputToCase(CaseOptions value);

        /// <summary>
        /// Sets a filter that validates each typed character before it is added to the input.
        /// </summary>
        /// <remarks>If the callback returns <c>true</c>, the character is accepted; otherwise, it is ignored.</remarks>
        /// <param name="value">A function that receives a character and returns <c>true</c> to accept it, or <c>false</c> to ignore it.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl AcceptInput(Func<char, bool> value);

        /// <summary>
        /// Limits the number of characters that can be entered.
        /// Default is zero or less (no limit).
        /// </summary>
        /// <param name="maxLength">The maximum number of characters allowed for the input.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl MaxLength(int maxLength);

        /// <summary>
        /// Sets a validation function executed when the user confirms the input.
        /// </summary>
        /// <param name="validselect">A predicate that returns whether the submitted value is valid.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl PredicateSelected(Func<string, bool> validselect);

        /// <summary>
        /// Sets an asynchronous validation function executed when the user confirms the input.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns whether the submitted value is valid.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IInputControl PredicateSelectedAsync(Func<string, Task<bool>> validselect);

        /// <summary>
        /// Sets a validation function that also returns a custom validation message.
        /// </summary>
        /// <param name="validselect">A predicate that returns a tuple: the first value indicates validity, and the second is an optional error message.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl PredicateSelected(Func<string, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous validation function that also returns a custom validation message.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns a tuple: the first value indicates validity, and the second is an optional error message.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IInputControl PredicateSelectedAsync(Func<string, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Updates the control description dynamically using a synchronous callback.
        /// </summary>
        /// <param name="value">A function that receives the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl ChangeDescription(Func<string, string> value);

        /// <summary>
        /// Updates the control description dynamically using an asynchronous callback.
        /// </summary>
        /// <param name="value">A function that asynchronously receives the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl ChangeDescriptionAsync(Func<string, Task<string>> value);

        /// <summary>
        /// Overrides one visual style used by the input control.
        /// </summary>
        /// <param name="styleType">The <see cref="InputStyles"/> to override.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IInputControl Styles(InputStyles styleType, Style style);

        /// <summary>
        /// Applies additional control options using a synchronous configuration callback.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IInputControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Enables input history (F3) and optionally customizes how history is stored and loaded.
        /// </summary>
        /// <param name="filename">The name of the file to store history. Cannot be <c>null</c>.</param>
        /// <param name="options">An action to configure <see cref="IHistoryOptions"/>. Optional.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        IInputControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Adds a synchronous suggestion provider for Tab and Shift+Tab completion.
        /// </summary>
        /// <param name="value">A function that receives the current input and returns an array of suggestions. Cannot be <c>null</c>.</param>
        /// <param name="autocomplete">If <c>true</c> (default), pressing Tab/Shift+Tab automatically applies the suggestion when only one match exists; if <c>false</c>, suggestions are shown in a list for manual selection.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl SuggestionHandler(Func<string, string[]> value, bool autocomplete = true);

        /// <summary>
        /// Adds an asynchronous suggestion provider for Tab and Shift+Tab completion.
        /// </summary>
        /// <param name="value">A function that asynchronously receives the current input and returns an array of suggestions. Cannot be <c>null</c>.</param>
        /// <param name="autocomplete">If <c>true</c> (default), pressing Tab/Shift+Tab automatically applies the suggestion when only one match exists; if <c>false</c>, suggestions are shown in a list for manual selection.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl SuggestionHandlerAsync(Func<string, Task<string[]>> value, bool autocomplete = true);

        /// <summary>
        /// Sets the minimum number of characters that must be typed before the suggestion
        /// provider is invoked. Default is <c>0</c> (suggestions appear from the first character).
        /// </summary>
        /// <param name="value">The minimum number of characters. Must be greater than or equal to 0.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl MinimumSuggestionLength(byte value);

        /// <summary>
        /// Displays the input control and blocks until the user submits or cancels, returning the final value.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the control while it is waiting for input. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the submitted <see cref="string"/> value, or an aborted result if the user cancelled.</returns>
        ResultPrompt<string> Run(CancellationToken token = default);
    }
}
