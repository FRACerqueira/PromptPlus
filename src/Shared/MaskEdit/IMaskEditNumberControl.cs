// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running a masked integer input control.
    /// </summary>
    /// <typeparam name="T">The integer type for the input value. Supported types: <see cref="int"/>, <see cref="long"/>.</typeparam>
    /// <remarks>
    /// The number format (digit count, sign, and thousands separator) is defined by
    /// <see cref="NumberFormat(byte, bool, bool)"/>. The culture controls digit group and
    /// decimal separator characters. Call <see cref="Run(CancellationToken)"/> last to display
    /// the control and read the submitted integer value.
    /// </remarks>
    public interface IMaskEditNumberControl<T>
    {
        /// <summary>
        /// Sets the placeholder character shown in empty input positions. Default is <c>'_'</c>.
        /// </summary>
        /// <param name="value">The placeholder character displayed in unfilled positions.</param>
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
        /// Hides the input-type hint shown below the numeric field. Default is <c>false</c> (hint visible).
        /// </summary>
        /// <param name="value">If <c>true</c>, the input-type hint is hidden; otherwise, it is shown.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> HideTipInputType(bool value = true);

        /// <summary>
        /// Sets the value pre-filled when the control is first displayed.
        /// </summary>
        /// <param name="value">The initial value shown in the input field.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IMaskEditNumberControl<T> Default(T value);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the value returned when the user submits without typing any digits.
        /// </summary>
        /// <param name="value">The fallback value used when the input field is left empty.</param>
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
        /// Sets a synchronous validation predicate executed when the user confirms the value.
        /// </summary>
        /// <param name="validselect">A predicate that returns <c>true</c> when the submitted value is acceptable.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a synchronous validation predicate that also returns a custom error message when invalid.
        /// </summary>
        /// <param name="validselect">A predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when invalid.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns <c>true</c> when an item is valid and can be selected.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMaskEditNumberControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid and returns an optional error message.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns a tuple: the first value indicates whether the item is valid, and the second is an optional error message.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMaskEditNumberControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the integer input control.
        /// </summary>
        /// <param name="styleType">The <see cref="MaskEditStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        IMaskEditNumberControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Applies shared control options (such as prompt text, tooltip visibility, and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditNumberControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditNumberControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Displays the masked integer input control and blocks until the user confirms or cancels,
        /// returning the submitted integer value.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the submitted integer value.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
