// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Globalization;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a MaskEdit number control.
    /// </summary>
    /// <typeparam name="T">The numeric type for the input value, restricted to integer types.</typeparam>
    /// <remarks>Valid only types:
    /// <list type="bullet">
    /// <item><description><see cref="int"/></description></item>
    /// <item><description><see cref="long"/></description></item>
    /// </list>
    /// </remarks>
    public interface IMaskEditNumberControl<T>
    {
        /// <summary>
        /// Sets the prompt mask character used to indicate input positions.
        /// </summary>
        /// <param name="value">Prompt mask character. Default is '_'.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> PromptMask(char value = '_');

        /// <summary>
        /// Configures the number format for the input with specified formatting options.
        /// </summary>
        /// <param name="integerpart">The maximum number of digits allowed in the integer part.</param>
        /// <param name="withsignal">If <c>true</c>, allows a sign (+/-) in the input. Default is <c>false</c>.</param>
        /// <param name="withseparatorgroup">If <c>true</c>, allows group separators (e.g., thousands separator). Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> NumberFormat(byte integerpart, bool withsignal = false, bool withseparatorgroup = true);

        /// <summary>
        /// Controls the visibility of the input type tip in the interface.
        /// </summary>
        /// <param name="value">If <c>true</c>, hides the input type tip. Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> HideTipInputType(bool value = true);

        /// <summary>
        /// Sets the initial default value for the input control.
        /// </summary>
        /// <param name="value">The default value to initialize the control with.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> Default(T value);

        /// <summary>
        /// Sets the fallback value to use when the input is empty.
        /// </summary>
        /// <param name="value">The value to use when no input is provided.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> DefaultIfEmpty(T value);

        /// <summary>
        /// Sets the culture for number formatting and validation.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use for validation and number formatting.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for number formatting and validation using a culture name.
        /// </summary>
        /// <param name="cultureName">The culture name to use for validation and number formatting. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        IMaskEditNumberControl<T> Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets a validation predicate to determine if a selected value is valid.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether a value is valid and selectable.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate with custom error messaging.
        /// </summary>
        /// <param name="validselect">A predicate function that returns a tuple of (isValid, errorMessage) for validation.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Customizes the visual appearance of specific interface elements.
        /// </summary>
        /// <param name="styleType">The interface element to style.</param>
        /// <param name="style">The style configuration to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IMaskEditNumberControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Configures general control behavior options.
        /// </summary>
        /// <param name="options">An action to configure the control options. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditNumberControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Executes the input control and returns the result.
        /// </summary>
        /// <param name="token">Optional cancellation token to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the the input control execution.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
