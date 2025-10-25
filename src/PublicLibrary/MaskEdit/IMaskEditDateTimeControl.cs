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
    /// Provides functionality for configuring and interacting with a MaskEdit Date Time/Date Only control.
    /// This control handles date and time input with mask-based editing capabilities.
    /// </summary>
    /// <typeparam name="T">The type of input value to be handled by the control.</typeparam>
    /// <remarks>Valid only types:
    /// <list type="bullet">
    /// <item><description><see cref="DateTime"/></description></item>
    /// </list>
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
        /// <param name="partdetetime">The datetime part to fix.</param>
        /// <param name="value">The value to set. Use -1 to set to the current value of the part.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> FixedValues(DateTimePart partdetetime, int value);

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
        IMaskEditDateTimeControl<T> Default(T value);

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
        /// Sets a validation predicate for determining valid selected values.
        /// </summary>
        /// <param name="validselect">The predicate function that validates selected values.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Sets a validation predicate that provides custom error messages for invalid selections.
        /// </summary>
        /// <param name="validselect">The predicate function that returns a tuple of (bool, string?) for validation result and message.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="validselect"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> PredicateSelected(Func<T, (bool, string?)> validselect);

        /// <summary>
        /// Customizes the visual style for specific elements of the control.
        /// </summary>
        /// <param name="styleType">The element type to style.</param>
        /// <param name="style">The style to apply.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Configures additional control options through an action delegate.
        /// </summary>
        /// <param name="options">The action to configure control options.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Executes the control and returns the input result.
        /// </summary>
        /// <param name="token">The cancellation token to observe. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the the input control execution.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);
    }
}
