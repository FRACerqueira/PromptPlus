// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using ConsolePlusLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Provides a fluent API for configuring and running a ProgressBar control that drives a
    /// visual progress indicator from an external update-handler callback, displaying the
    /// current value, an optional spinner, and an optional description that all update in real time.
    /// </summary>
    /// <remarks>
    /// The progress value is updated by the callback registered via
    /// <see cref="UpdateHandler(Action{ProgressBarEvent, CancellationToken}, IDictionary{string, object?}?)"/>
    /// or its async variant. When the callback reports completion (or the cancellation token is
    /// signalled) the control returns the final <see cref="StateProgress"/>. Every configuration
    /// method returns the same <see cref="IProgressBarControl"/> instance so the calls can be
    /// chained (fluent style). Call <see cref="Run(CancellationToken)"/> last.
    /// </remarks>
    public interface IProgressBarControl
    {
        /// <summary>
        /// Applies shared control options (such as prompt text, tooltip visibility, and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        IProgressBarControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Sets the visual fill style of the progress bar track. Default is <see cref="ProgressBarType.Fill"/>.
        /// </summary>
        /// <param name="type">The fill style to use.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl Fill(ProgressBarType type);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the ProgressBar control.
        /// </summary>
        /// <param name="styleType">The <see cref="ProgressBarStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl Styles(ProgressBarStyles styleType, Style style);

        /// <summary>
        /// Displays an animated spinner alongside the progress bar while the operation is running.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Sets the culture used to format numeric values.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        IProgressBarControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the culture used to format numeric values by culture name.
        /// </summary>
        /// <param name="cultureName">The name of the <see cref="CultureInfo"/> to use. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="cultureName"/> is <c>null</c> or empty.</exception>
        /// <exception cref="CultureNotFoundException">Thrown when the specified culture name is not valid.</exception>
        IProgressBarControl Culture(string cultureName) => Culture(new CultureInfo(cultureName));

        /// <summary>
        /// Sets the valid numeric range for the ProgressBar.
        /// </summary>
        /// <param name="minvalue">Minimum allowed value.</param>
        /// <param name="maxvalue">Maximum allowed value.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="minvalue"/> is greater than or equal to <paramref name="maxvalue"/>.</exception>
        IProgressBarControl Range(double minvalue, double maxvalue);

        /// <summary>
        /// Sets the ProgressBar width. Default is 40 and minimum is 10.
        /// </summary>
        /// <param name="value">The width of the ProgressBar.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is less than 10.</exception>
        IProgressBarControl Width(byte value);

        /// <summary>
        /// Sets the initial ProgressBar value. Default is 0.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is outside the configured range.</exception>
#pragma warning disable CA1716 // Identifiers should not match keywords
        IProgressBarControl Default(double value);
#pragma warning restore CA1716 // Identifiers should not match keywords

        /// <summary>
        /// Sets the number of fractional digits shown for values. Default is 0.
        /// </summary>
        /// <param name="value">The number of fractional digits.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is greater than 5.</exception>
        IProgressBarControl FractionalDigits(byte value);

        /// <summary>
        /// Registers a callback that returns a <see cref="Style"/> based on the current progress value,
        /// so the bar color changes dynamically as the value advances.
        /// </summary>
        /// <param name="value">A function that receives the current numeric value and returns the style to apply. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl ChangeColor(Func<double, Style> value);

        /// <summary>
        /// Applies a gradient color sequence to the filled portion of the bar.
        /// The gradient is interpolated across the configured range as the value advances.
        /// </summary>
        /// <param name="colors">Two or more <see cref="Color"/> values that define the gradient. Cannot be <c>null</c> or empty.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="colors"/> is <c>null</c> or empty.</exception>
        IProgressBarControl ChangeGradient(params Color[] colors);

        /// <summary>
        /// Registers a callback that provides a dynamic description text based on the current
        /// progress value; the description is refreshed every time the value changes.
        /// </summary>
        /// <param name="value">A function that receives the current numeric value and returns the description to display. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl ChangeDescription(Func<double, string> value);

        /// <summary>
        /// Asynchronous variant of <see cref="ChangeDescription(Func{double, string})"/>.
        /// The task is awaited synchronously each time the description is refreshed.
        /// </summary>
        /// <param name="value">An async callback that receives the current numeric value and returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl ChangeDescriptionAsync(Func<double, Task<string>> value);

        /// <summary>
        /// Sets the text displayed when the ProgressBar completes.
        /// </summary>
        /// <param name="finishtext">Completion text.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl Finish(string finishtext);

        /// <summary>
        /// Sets a synchronous callback to update ProgressBar values during execution.
        /// </summary>
        /// <param name="value">Callback that receives <see cref="ProgressBarEvent"/> and <see cref="CancellationToken"/>. Cannot be <c>null</c>.</param>
        /// <param name="context">Optional key/value context data passed to the callback.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl UpdateHandler(Action<ProgressBarEvent, CancellationToken> value, IDictionary<string, object?>? context = null);

        /// <summary>
        /// Sets an asynchronous callback to update ProgressBar values during execution.
        /// </summary>
        /// <param name="value">Async callback that receives <see cref="ProgressBarEvent"/> and <see cref="CancellationToken"/>. Cannot be <c>null</c>.</param>
        /// <param name="context">Optional key/value context data passed to the callback.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        IProgressBarControl UpdateHandlerAsync(Func<ProgressBarEvent, CancellationToken, Task> value, IDictionary<string, object?>? context = null);

        /// <summary>
        /// Hides one or more visual elements of the ProgressBar (e.g. the value label or the percentage).
        /// </summary>
        /// <param name="value">A <see cref="HideProgressBar"/> flags value identifying the elements to hide.</param>
        /// <returns>The current <see cref="IProgressBarControl"/> instance for chaining.</returns>
        IProgressBarControl HideElements(HideProgressBar value);

        /// <summary>
        /// Displays the ProgressBar control and blocks until the update-handler signals completion
        /// or the cancellation token is triggered, returning the final state.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the operation. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the final <see cref="StateProgress"/>.</returns>
        ResultPrompt<StateProgress> Run(CancellationToken token = default);
    }
}
