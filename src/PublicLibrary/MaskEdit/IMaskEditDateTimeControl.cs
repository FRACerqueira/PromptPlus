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
    /// </summary>
    /// <typeparam name="T">The type of input.</typeparam>
    /// <remarks>Valid only types:
    /// <list type="bullet">
    /// <item><description><see cref="DateTime"/></description></item>
    /// </list>
    /// </remarks>
    public interface IMaskEditDateTimeControl<T>
    {
        /// <summary>
        /// Prompt mask character.
        /// </summary>
        /// <param name="value">Prompt mask character. Default is '_'.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> PromptMask(char value = '_');

        /// <summary>
        /// Sets a fixed date value.
        /// </summary>
        /// <param name="partdetetime">The <see cref="DateTimePart"/>representing the fixed datetime part to use.</param>
        /// <param name="value">The value to fixed.If the value is equal to -1 it would be set to the current value of the part</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> FixedValues(DateTimePart partdetetime, int value);


        /// <summary>
        /// The input behavior. Defaul value is <see cref="InputBehavior.EditSkipToInput"/>.
        /// </summary>
        /// <param name="inputBehavior">The input behavior</param>
        /// <returns></returns>
        IMaskEditDateTimeControl<T> InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput);

        /// <summary>
        /// Hide the input type tip. Default <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hide type input.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> HideTipInputType(bool value = true);

        /// <summary>
        /// Show the week for date.
        /// </summary>
        /// <param name="value">The week format for date types. Default is <see cref="WeekType.None"/>.</param>
        /// /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> WeekTypeMode(WeekType value = WeekType.WeekShort);

        /// <summary>
        /// Sets the default value for the input.
        /// </summary>
        /// <param name="value">The default value.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> Default(T value);

        /// <summary>
        /// Sets the default value to use when the input is empty.
        /// </summary>
        /// <param name="value">The default value for empty input.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> DefaultIfEmpty(T value);

        /// <summary>
        /// Sets the culture for format validation. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use for validation and format date.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture for format validation. Default value is current PromptPlus culture.
        /// </summary>
        /// <param name="cultureName">The culture name to use for validation and format date. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        IMaskEditDateTimeControl<T> Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        IMaskEditDateTimeControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Overrides the styles for the input control.
        /// </summary>
        /// <param name="styleType">The <see cref="InputStyles"/> to override.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Applies custom options to the MaskEdit input control.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditDateTimeControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditDateTimeControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Runs the MaskEdit input control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result of the input control execution.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);

    }
}
