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
    /// Provides functionality for configuring and interacting with a MaskEdit currency control.
    /// </summary>
    /// <typeparam name="T">The type of input. Valid only for <see cref="decimal"/> type.</typeparam>
    /// <remarks>Valid only types:
    /// <list type="bullet">
    /// <item><description><see cref="decimal"/></description></item>
    /// </list>
    /// </remarks>
    public interface IMaskEditCurrencyControl<T>
    {
        /// <summary>
        /// Prompt mask character.
        /// </summary>
        /// <param name="value">Prompt mask character. Default is '_'.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        IMaskEditCurrencyControl<T> PromptMask(char value = '_');

        /// <summary>
        /// Configures the number format for the input.
        /// </summary>
        /// <param name="integerpart">The maximum number of digits allowed in the integer part.</param>
        /// <param name="decimalpart">The number of decimal digits allowed after the decimal point. Default value is 2.</param>
        /// <param name="withsignal">If <c>true</c>, allows a sign (+/-) in the input. Default is <c>false</c>.</param>
        /// <param name="withseparatorgroup">If <c>true</c>, allows group separators (e.g., thousands separator). Default is <c>true</c>.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        IMaskEditCurrencyControl<T> NumberFormat(byte integerpart, byte decimalpart = 2, bool withsignal = false, bool withseparatorgroup = true);

        /// <summary>
        /// Hide the input type tip. Default <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hide type input.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        IMaskEditCurrencyControl<T> HideTipInputType(bool value = true);

        /// <summary>
        /// Sets the default value for the input.
        /// </summary>
        /// <param name="value">The default value.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        IMaskEditCurrencyControl<T> Default(T value);

        /// <summary>
        /// Sets the default value to use when the input is empty.
        /// </summary>
        /// <param name="value">The default value for empty input.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        /// <remarks>This value is used only when the input field is left empty by the user.</remarks>
        IMaskEditCurrencyControl<T> DefaultIfEmpty(T value);

        /// <summary>
        /// Sets the culture for format validation. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use for validation and format date.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        IMaskEditCurrencyControl<T> Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for format validation. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name to use for validation and format date. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        IMaskEditCurrencyControl<T> Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Validates input using a custom predicate.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an input value is valid.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        IMaskEditCurrencyControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Validates input using a custom predicate with custom error message support.
        /// </summary>
        /// <param name="validselect">A predicate function that returns a tuple containing a boolean validation result and an optional error message.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        IMaskEditCurrencyControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Overrides the styles for the input control.
        /// </summary>
        /// <param name="styleType">The <see cref="InputStyles"/> to override.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IMaskEditCurrencyControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Applies custom options to the MaskEdit input control.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditCurrencyControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditCurrencyControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Runs the MaskEdit input control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the the input control execution.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);

    }
}
