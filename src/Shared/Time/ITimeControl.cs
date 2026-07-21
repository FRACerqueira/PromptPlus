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
    /// Provides a fluent API for configuring and displaying a Time control that suspends
    /// execution for a fixed duration while presenting a live countdown to the user.
    /// </summary>
    /// <remarks>
    /// Every configuration method returns the same <see cref="ITimeControl"/> instance, so the
    /// calls can be chained together (fluent style). Call <see cref="Run(CancellationToken)"/>
    /// last to display the control and block for the configured duration.
    /// </remarks>
    public interface ITimeControl
    {
        /// <summary>
        /// Applies the shared control options (such as prompt message, tooltips and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        ITimeControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the Time control.
        /// </summary>
        /// <param name="styleType">The <see cref="TimeStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="style"/> is <c>null</c>.</exception>
        ITimeControl Styles(TimeStyles styleType, Style style);

        /// <summary>
        /// Sets the total duration to wait while displaying the countdown.
        /// </summary>
        /// <param name="duration">The duration to suspend execution. Must be greater than zero.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="duration"/> is less than or equal to zero.</exception>
        ITimeControl Duration(TimeSpan duration);

        /// <summary>
        /// Sets the total duration, in seconds, to wait while displaying the countdown.
        /// </summary>
        /// <param name="seconds">The number of seconds to suspend execution. Must be greater than zero.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="seconds"/> is less than or equal to zero.</exception>
        ITimeControl Duration(int seconds);

        /// <summary>
        /// Sets the format string used to render the remaining time. Default is <c>hh\:mm\:ss</c>.
        /// </summary>
        /// <param name="format">A <see cref="TimeSpan"/> format string.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        ITimeControl Format(string format);

        /// <summary>
        /// Sets the culture used to format the countdown value.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        ITimeControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the text displayed when the countdown finishes. When not set, the elapsed time is shown.
        /// </summary>
        /// <param name="finishtext">The text to display at the end.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        ITimeControl Finish(string finishtext);

        /// <summary>
        /// Dynamically changes the description of the control based on the remaining time.
        /// </summary>
        /// <param name="value">A function that receives the remaining time and returns the description to display. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ITimeControl ChangeDescription(Func<TimeSpan, string> value);

        /// <summary>
        /// Asynchronous version of <see cref="ChangeDescription(Func{TimeSpan, string})"/> that updates the
        /// description text according to the remaining time (useful when the text comes from an asynchronous source).
        /// </summary>
        /// <param name="value">A function that receives the remaining time and asynchronously returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ITimeControl ChangeDescriptionAsync(Func<TimeSpan, Task<string>> value);

        /// <summary>
        /// Sets whether the control displays the remaining time (countdown) or the elapsed time.
        /// The default is <see cref="TimeDisplayMode.Countdown"/>.
        /// </summary>
        /// <param name="mode">The <see cref="TimeDisplayMode"/> to use.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        ITimeControl DisplayMode(TimeDisplayMode mode);

        /// <summary>
        /// Displays an animated spinner next to the time value while the countdown is running.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The same <see cref="ITimeControl"/> instance for chaining.</returns>
        ITimeControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Displays the countdown and blocks until it completes or is aborted, returning the elapsed time.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the countdown while it is waiting.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the elapsed <see cref="TimeSpan"/>.</returns>
        ResultPrompt<TimeSpan> Run(CancellationToken token = default);
    }
}
