// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a MaskEdit string control.
    /// </summary>
    /// <typeparam name="T">The type of input.</typeparam>
    /// <remarks>Valid only types:
    /// <list type="bullet">
    /// <item><description><see cref="string"/></description></item>
    /// </list>
    /// </remarks>
    public interface IMaskEditStringControl<T>
    {
        /// <summary>
        /// Sets the input mask pattern, Required!.
        /// </summary>
        /// <param name="mask">The mask pattern
        /// The mask pattern. Mask rules:
        /// <list type="bullet">
        /// <item><description></description></item>
        /// <item><description>9 - Numeric character accepts delimiters for constant or custom.</description></item>
        /// <item><description>L - Lower Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>U - Upper Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>A - Lower and Upper Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>X - Numeric, Lower and Upper Letter character accepts delimiters for constant or custom.</description></item>
        /// <item><description>C - Custom characteraccepts only delimiters for custom.</description></item>
        /// <item><description>\ - Escape character to use the next char as constant.</description></item>
        /// <item><description>{ } - Delimiters group to apply custom list or constant value valid only a single mask type insede the group.</description></item>
        /// <item><description>[ ] - Delimiters for custom value.</description></item>
        /// <item><description>( ) - Delimiters for constant value insede the group.</description></item>
        /// </list>
        /// </param>
        /// <param name="returnWithMask">If <c>true</c>, the result includes the mask. Default value is <c>false</c>.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> Mask(string mask, bool returnWithMask = false);

        /// <summary>
        /// Prompt mask character.
        /// </summary>
        /// <param name="value">Prompt mask character. Default is '_'.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> PromptMask(char value = '_');

        /// <summary>
        /// The input behavior. Defaul value is <see cref="InputBehavior.EditSkipToInput"/>.
        /// </summary>
        /// <param name="inputBehavior">The input behavior</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> InputMode(InputBehavior inputBehavior = InputBehavior.EditSkipToInput);

        /// <summary>
        /// Hide the input type tip. Default <c>false</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, hide type input.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> HideTipInputType(bool value = true);

        /// <summary>
        /// Sets the default value for the input.
        /// </summary>
        /// <param name="value">The default value.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> Default(T value);

        /// <summary>
        /// Sets the default value to use when the input is empty.
        /// </summary>
        /// <param name="value">The default value for empty input.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> DefaultIfEmpty(T value);

        /// <summary>
        /// Set validation predicate for selected item.
        /// </summary>
        /// <param name="validselect">A predicate function that determines whether an Item is considered valid and should be selectable.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        IMaskEditStringControl<T> PredicateSelected(Func<T, bool> validselect);

        /// <summary>
        /// Overrides the styles for the input control.
        /// </summary>
        /// <param name="styleType">The <see cref="InputStyles"/> to override.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IMaskEditStringControl<T> Styles(MaskEditStyles styleType, Style style);

        /// <summary>
        /// Applies custom options to the MaskEdit input control.
        /// </summary>
        /// <param name="options">An action to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IMaskEditStringControl{T}"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IMaskEditStringControl<T> Options(Action<IControlOptions> options);

        /// <summary>
        /// Runs the MaskEdit input control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result of the input control execution.</returns>
        ResultPrompt<T> Run(CancellationToken token = default);

    }
}
