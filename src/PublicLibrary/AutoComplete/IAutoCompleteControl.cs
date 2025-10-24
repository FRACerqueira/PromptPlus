// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;
using System.Threading.Tasks;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides methods to configure and control AutoComplete input behavior in the PromptPlus library.
    /// </summary>
    public interface IAutoCompleteControl
    {
        /// <summary>
        /// Sets the maximum number of items to view per page. Default value is 5.
        /// </summary>
        /// <param name="value">Number of maximum items.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IAutoCompleteControl PageSize(byte value);

        /// <summary>
        /// Number minimum of chars to accept autocomplete.Default value is 3.
        /// </summary>
        /// <param name="value">Number of chars.The value must be greater than or equal to 1.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IAutoCompleteControl MinimumPrefixLength(byte value);

        /// <summary>
        /// Number of milliseconds to wait before to start function autocomplete. Default value is 500
        /// </summary>
        /// <param name="value">Number of milliseconds.The value must be greater than or equal to 100.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 100.</exception>
        IAutoCompleteControl CompletionWaitToStart(int value);

        /// <summary>
        /// The max.items to return from function autocomplete.Default value is <seealso cref="int.MaxValue"/>.
        /// </summary>
        /// <param name="value">Number of max.items. The value must be greater than or equal to 1.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1.</exception>
        IAutoCompleteControl CompletionMaxCount(int value);

        /// <summary>
        /// The function to execute AutoComplete. This function is required for operation.
        /// </summary>
        /// <param name="value">The async function that performs the autocomplete operation. 
        /// It takes a string input and returns an array of completion suggestions.
        /// </param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IAutoCompleteControl CompletionAsyncService(Func<string, CancellationToken, Task<string[]>> value);

        /// <summary>
        /// Sets the function to display text for items in the list. Default is input param from function.
        /// </summary>
        /// <param name="value">Function to extract text to input from item.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IAutoCompleteControl TextSelector(Func<string, string> value);

        /// <summary>
        /// Shows a <see cref="SpinnersType"/> animation at the end of the prompt. Default is <see cref="SpinnersType.Ascii"/>.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        IAutoCompleteControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Sets the default value for the input when specified.
        /// </summary>
        /// <param name="value">The default value to use.</param>
        /// <param name="usedefaultHistory">Indicates whether to use the default value from history (if enabled <see cref="EnabledHistory(string, Action{IHistoryOptions}?)"/>).</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        IAutoCompleteControl Default(string value, bool usedefaultHistory = true);

        /// <summary>
        /// Sets the default value to use when the AutoComplete input is empty.
        /// </summary>
        /// <param name="value">The default value to use when the input is empty.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        IAutoCompleteControl DefaultIfEmpty(string value);

        /// <summary>
        /// Transforms the input characters using the specified <see cref="CaseOptions"/>.
        /// </summary>
        /// <param name="value">The case transformation option to apply.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        IAutoCompleteControl InputToCase(CaseOptions value);

        /// <summary>
        /// Executes a function to determine whether to accept a character input.
        /// <remarks>If the function returns <c>true</c>, the character is accepted; otherwise, it is ignored.</remarks>
        /// </summary>
        /// <param name="value">A function that takes a character and returns a boolean indicating whether to accept the input.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IAutoCompleteControl AcceptInput(Func<char, bool> value);

        /// <summary>
        /// Sets the maximum length for the input and optionally the maximum width. The default vaue for <paramref name="maxLength"/> is <see cref="int.MaxValue"/> characters.
        /// </summary>
        /// <remarks>
        /// The value of <paramref name="maxWidth"/> must be less than <paramref name="maxLength"/> otherwise, it will be set to equal.
        /// </remarks>
        /// <param name="maxLength">The maximum number of characters allowed for the input.</param>
        /// <param name="maxWidth">>The maximum width of the input in characters.The value must be less than <paramref name="maxLength"/> otherwise, it will be equal.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxLength"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 1.</exception>
        IAutoCompleteControl MaxLength(int maxLength, byte? maxWidth = null);

        /// <summary>
        /// Sets the maximum width for the input.
        /// <remarks>
        /// The value of <paramref name="maxWidth"/> must be less than <see cref="MaxLength(int,byte?)"/> otherwise,  it will be set to equal.
        /// </remarks>
        /// </summary>
        /// <param name="maxWidth">The maximum width of the input in characters.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxWidth"/> is less than 1.</exception>
        IAutoCompleteControl MaxWidth(byte maxWidth);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable and custom error message.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        IAutoCompleteControl PredicateSelected(Func<string, (bool, string?)> validselect);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        IAutoCompleteControl PredicateSelected(Func<string, bool> validselect);

        /// <summary>
        /// Dynamically changes the description using a user-defined function.
        /// </summary>
        /// <param name="value">A function that takes the current description and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IAutoCompleteControl ChangeDescription(Func<string, string> value);

        /// <summary>
        /// Overrides the styles for the AutoComplete input control.
        /// </summary>
        /// <param name="styleType">The <see cref="AutoComleteStyles"/> to override.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IAutoCompleteControl Styles(AutoComleteStyles styleType, Style style);

        /// <summary>
        /// Applies custom options to the AutoComplete input control.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IAutoCompleteControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Enabled History and applies custom options to History feature. 
        /// </summary>
        /// <remarks>
        ///  The Defaults hotkey to Hisyory is <see cref="PromptConfig.HotKeyShowHistory"/>.
        /// </remarks>
        /// <param name="filename">The name of the file to store history.</param>
        /// <param name="options">An action to configure the <see cref="IHistoryOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IAutoCompleteControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is <c>null</c>.</exception>
        IAutoCompleteControl EnabledHistory(string filename, Action<IHistoryOptions>? options = null);

        /// <summary>
        /// Runs the AutoComplete input control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the input control execution. </returns>
        ResultPrompt<string> Run(CancellationToken token = default);
    }
}
