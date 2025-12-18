// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides methods to configure and control input behavior in the PromptPlus library.
    /// </summary>
    public interface IInputControl
    {

        /// <summary>
        /// Sets the default value for the input when specified.
        /// <remarks>Default cannot be used with <see cref="IsSecret"/>.</remarks>
        /// </summary>
        /// <param name="value">The default value to use.</param>
        /// <param name="usedefaultHistory">Indicates whether to use the default value from history (if enabled <see cref="EnabledHistory(string, Action{IHistoryOptions}?)"/>).</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl Default(string value, bool usedefaultHistory = true);

        /// <summary>
        /// Sets the default value to use when the input is empty.
        /// </summary>
        /// <param name="value">The default value to use when the input is empty.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl DefaultIfEmpty(string value);

        /// <summary>
        /// Transforms the input characters using the specified <see cref="CaseOptions"/>.
        /// </summary>
        /// <param name="value">The case transformation option to apply.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl InputToCase(CaseOptions value);

        /// <summary>
        /// Executes a function to determine whether to accept a character input.
        /// <remarks>If the function returns <c>true</c>, the character is accepted; otherwise, it is ignored.</remarks>
        /// </summary>
        /// <param name="value">A function that takes a character and returns a boolean indicating whether to accept the input.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl AcceptInput(Func<char, bool> value);

        /// <summary>
        /// Sets the maximum length for the input and optionally the maximum width. The default vaue for <paramref name="maxLength"/> is <see cref="int.MaxValue"/> characters.
        /// </summary>
        /// <remarks>
        /// The value of <paramref name="maxWidth"/> must be less than <paramref name="maxLength"/> otherwise, it will be set to equal.
        /// </remarks>
        /// <param name="maxLength">The maximum number of characters allowed for the input.</param>
        /// <param name="maxWidth">The maximum width of the input in characters. The value must be less than <paramref name="maxLength"/> otherwise, it will be equal.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 1.</exception>
        IInputControl MaxLength(int maxLength, byte? maxWidth = null);

        /// <summary>
        /// Sets the maximum width for the input. Default value is <see cref="IPromptPlusConfig.MaxWidth"/>.
        /// <remarks>
        /// The value of <paramref name="maxWidth"/> must be less than <see cref="MaxLength(int,byte?)"/> otherwise,  it will be set to equal.
        /// </remarks>
        /// </summary>
        /// <param name="maxWidth">The maximum width of the input in characters.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 1.</exception>
        IInputControl MaxWidth(byte maxWidth);

        /// <summary>
        /// Masks the input as a secret, replacing characters with a specified mask character.
        /// <remarks>
        /// <para>The following features cannot be used with <see cref="IsSecret"/>:</para>
        /// <list type="bullet">
        /// <item><description>SuggestionHandler</description></item>
        /// <item><description>DefaultEmpty</description></item>
        /// <item><description>Default</description></item>
        /// <item><description>HistoryEnabled</description></item>
        /// </list>
        /// </remarks>
        /// </summary>
        /// <param name="value">The character to use as the mask. Defaults is '#'.</param>
        /// <param name="enabledView">Enables the user to toggle the visibility of the masked input. Default value is true.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl IsSecret(char? value = null, bool enabledView = true);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl PredicateSelected(Func<string, bool> validselect);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable with custom message.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        IInputControl PredicateSelected(Func<string, (bool, string?)> validselect);

        /// <summary>
        /// Dynamically changes the description using a user-defined function.
        /// </summary>
        /// <param name="value">A function that takes the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl ChangeDescription(Func<string, string> value);

        /// <summary>
        /// Overrides the styles for the input control.
        /// </summary>
        /// <param name="styleType">The <see cref="InputStyles"/> to override.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IInputControl Styles(InputStyles styleType, Style style);

        /// <summary>
        /// Applies custom options to the input control.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IInputControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Enabled History and applies custom options to History feature. 
        /// </summary>
        /// <param name="filename">The name of the file to store history.</param>
        /// <param name="options">An action to configure the <see cref="IHistoryOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        IInputControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Adds a suggestion handler to provide input suggestions.
        /// <remarks>Cannot be used with <see cref="IsSecret"/>.</remarks>
        /// </summary>
        /// <param name="value">A function that takes the current input and returns a string array of sugestions.</param>
        /// <returns>The current <see cref="IInputControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IInputControl SuggestionHandler(Func<string, string[]> value);

        /// <summary>
        /// Runs the input control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the input control execution. </returns>
        ResultPrompt<string> Run(CancellationToken token = default);
    }
}
