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
    /// Provides a fluent API for configuring and running a masked string input control.
    /// </summary>
    /// <typeparam name="T">The type for the input value. Must be <see cref="string"/>.</typeparam>
    /// <remarks>
    /// The user can only type characters that are allowed by the mask pattern defined via
    /// <see cref="Mask(string, bool)"/>. Literal characters in the mask are displayed but cannot
    /// be edited. Call <see cref="Run(CancellationToken)"/> last to display the control and
    /// read the submitted value.
    /// </remarks>
    public interface IMaskEditStringControl<T>
    {
        /// <summary>
        /// Sets the input mask pattern, Required!.
        /// </summary>
        /// <param name="mask">The mask pattern. Mask rules:
        /// <list type="bullet">
        /// <item><description>Any character not defined in the rules will be treated as a literal character</description></item>
        /// <item><description>9 - Numeric character accepts delimiters for constant or custom.</description></item>
        /// <item><description>L - Lower Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>U - Upper Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>A - Lower and Upper Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>X - Numeric, Lower and Upper Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>C - Custom character accepts only delimiters for custom.</description></item>
        /// <item><description>\ - Escape character to use the next char as constant.</description></item>
        /// <item><description>{ } - Delimiters group to apply custom list or constant value, valid only a single mask type inside the group.</description></item>
        /// <item><description>[ ] - Delimiters for custom value.</description></item>
        /// <item><description>( ) - Delimiters for constant value inside the group.</description></item>
        /// </list>
        /// </param>
        /// <param name="returnWithMask">If <c>true</c>, the result includes the mask. Default value is <c>false</c>.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        /// <remarks>
        /// The mask can include literal characters and special pattern characters to define the input format.
        /// </remarks>
        IMaskEditStringControl<T> Mask(string mask, bool returnWithMask = false);

        /// <summary>
        /// Sets the placeholder character shown in empty input positions of the mask. Default is <c>'_'</c>.
        /// </summary>
        /// <param name="value">The placeholder character displayed in unfilled mask positions.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> PromptMask(char value = '_');

        /// <summary>
        /// Sets how the cursor behaves when the user starts typing inside the masked field.
        /// Default is <see cref="InputBehavior.EditSkipToInput"/>, which moves the cursor to the
        /// first editable position automatically.
        /// </summary>
        /// <param name="inputBehavior">The input behavior to apply.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput);

        /// <summary>
        /// Hides the input-type hint shown below the masked field. Default is <c>false</c> (hint visible).
        /// </summary>
        /// <param name="value">If <c>true</c>, the input-type hint is hidden; otherwise, it is shown.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> HideTipInputType(bool value = true);

        /// <summary>
        /// Sets the default value for the input.
        /// </summary>
        /// <param name="value">The default value.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IMaskEditStringControl<T> Default(T value);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the default value to use when the input is empty.
        /// </summary>
        /// <param name="value">The default value for empty input.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> DefaultIfEmpty(T value);

        /// <summary>
        /// Sets a synchronous validation predicate that determines whether the submitted value is valid.
        /// </summary>
        /// <param name="validselect">A predicate that returns <c>true</c> when the value is valid.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a synchronous validation predicate that determines whether the submitted value is valid and returns an optional error message.
        /// </summary>
        /// <param name="validselect">A predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when invalid.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns <c>true</c> when an item is valid and can be selected.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMaskEditStringControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid and returns an optional error message.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns a tuple: the first value indicates whether the item is valid, and the second is an optional error message.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMaskEditStringControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect);


        /// <summary>
        /// Overrides the visual style applied to a specific region of the masked input control.
        /// </summary>
        /// <param name="styleType">The <see cref="MaskEditStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Applies custom options to the MaskEdit input control.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditStringControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Displays the masked input control and blocks until the user confirms or cancels,
        /// returning the submitted (and optionally unmasked) value.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the submitted string value.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
