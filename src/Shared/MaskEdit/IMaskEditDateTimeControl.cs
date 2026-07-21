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
    /// Provides a fluent API for configuring and running a masked date/time input control.
    /// </summary>
    /// <typeparam name="T">The date/time type. Supported types: <see cref="DateTime"/>, <see cref="DateOnly"/>, <see cref="TimeOnly"/>.</typeparam>
    /// <remarks>
    /// Each segment of the date/time value (day, month, year, hour, minute, etc.) is displayed
    /// as a separate editable field. The user navigates between fields with the arrow keys and
    /// types digits to fill them in. Individual fields can be locked to a constant value via
    /// <see cref="FixedValues(DateTimePart, int)"/>. Call <see cref="Run(CancellationToken)"/>
    /// last to display the control and read the submitted value.
    /// </remarks>
    public interface IMaskEditDateTimeControl<T>
    {
        /// <summary>
        /// Sets the prompt mask character for unfilled positions in the input.
        /// </summary>
        /// <param name="value">The character to use as the prompt mask. Default is '_'.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> PromptMask(char value = '_');

        /// <summary>
        /// Sets a fixed value for a specific date/time part that cannot be modified during input.
        /// </summary>
        /// <param name="dateTimePart">The datetime part to fix.</param>
        /// <param name="value">The value to set. Use -1 to set to the current value of the part.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> FixedValues(DateTimePart dateTimePart, int value);

        /// <summary>
        /// Sets the input behavior mode for the control.
        /// </summary>
        /// <param name="inputBehavior">The input behavior to use. Default is <see cref="InputBehavior.EditSkipToInput"/>.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput);

        /// <summary>
        /// Controls the visibility of the input type tip.
        /// </summary>
        /// <param name="value">When <c>true</c>, hides the input type tip. Default is <c>false</c>.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> HideTipInputType(bool value = true);

        /// <summary>
        /// Configures the display of week information for dates.
        /// </summary>
        /// <param name="value">The week format to display. Default is <see cref="WeekType.WeekShort"/>.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> WeekTypeMode(WeekType value = WeekType.WeekShort);

        /// <summary>
        /// Sets the initial default value for the input control.
        /// </summary>
        /// <param name="value">The default value to use.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IMaskEditDateTimeControl<T> Default(T value);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the fallback value to use when the input is empty.
        /// </summary>
        /// <param name="value">The value to use when input is empty.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> DefaultIfEmpty(T value);

        /// <summary>
        /// Sets the culture for date/time formatting and validation using a CultureInfo object.
        /// </summary>
        /// <param name="culture">The culture to use for validation and formatting.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for date/time formatting and validation using a culture name.
        /// </summary>
        /// <param name="cultureName">The name of the culture to use.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        /// <exception cref="CultureNotFoundException">Thrown when the specified culture is not found.</exception>
        IMaskEditDateTimeControl<T> Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets a synchronous validation predicate executed when the user confirms the value.
        /// Returns <c>false</c> to reject the input and show a generic error.
        /// </summary>
        /// <param name="validselect">A predicate that returns <c>true</c> when the submitted value is acceptable.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a synchronous validation predicate that also returns a custom error message when invalid.
        /// </summary>
        /// <param name="validselect">A predicate returning a tuple: <c>true</c> when valid, plus an optional error message shown when invalid.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns <c>true</c> when an item is valid and can be selected.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMaskEditDateTimeControl<T> PredicateSelectedAsync(Func<T, Task<bool>> validselect);

        /// <summary>
        /// Sets an asynchronous validation predicate that determines whether the selected item is valid and returns an optional error message.
        /// </summary>
        /// <param name="validselect">An asynchronous predicate that returns a tuple: the first value indicates whether the item is valid, and the second is an optional error message.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <remarks>The asynchronous predicate is evaluated synchronously (blocking) on the UI thread; it does not run in parallel.</remarks>
        IMaskEditDateTimeControl<T> PredicateSelectedAsync(Func<T, Task<(bool, string?)>> validselect);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the date/time input control.
        /// </summary>
        /// <param name="styleType">The <see cref="MaskEditStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Applies shared control options (such as prompt text, tooltip visibility, and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Displays the masked date/time input control and blocks until the user confirms or cancels,
        /// returning the submitted date/time value.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the submitted date/time value.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
