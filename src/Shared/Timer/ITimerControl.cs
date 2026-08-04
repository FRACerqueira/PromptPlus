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
    /// Provides a fluent API for configuring and displaying a Timer control that suspends
    /// execution for a fixed duration while presenting a live countdown or elapsed-time display to the user.
    /// </summary>
    /// <remarks>
    /// Every configuration method returns the same <see cref="ITimerControl"/> instance, so the
    /// calls can be chained together (fluent style). Call <see cref="Run(CancellationToken)"/>
    /// last to display the control and block for the configured duration.
    /// </remarks>
    public interface ITimerControl
    {
        /// <summary>
        /// Applies the shared control options (such as prompt message, tooltips and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        ITimerControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the Timer control.
        /// </summary>
        /// <param name="styleType">The <see cref="TimerStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        ITimerControl Styles(TimerStyles styleType, Style style);

        /// <summary>
        /// Sets the total duration to wait while displaying the countdown.
        /// </summary>
        /// <param name="duration">The duration to suspend execution. Must be greater than zero.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="duration"/> is less than or equal to zero.</exception>
        ITimerControl Duration(TimeSpan duration);

        /// <summary>
        /// Sets the total duration, in seconds, to wait while displaying the countdown.
        /// </summary>
        /// <param name="seconds">The number of seconds to suspend execution. Must be greater than zero.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="seconds"/> is less than or equal to zero.</exception>
        ITimerControl Duration(int seconds);

        /// <summary>
        /// Sets the format string used to render the remaining time. Default is <c>hh\:mm\:ss</c>.
        /// </summary>
        /// <param name="format">A <see cref="TimeSpan"/> format string.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        ITimerControl Format(string format);

        /// <summary>
        /// Sets the culture used to format the countdown value.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        ITimerControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the text displayed when the countdown finishes. When not set, the elapsed time is shown.
        /// </summary>
        /// <param name="finishtext">The text to display at the end.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        ITimerControl Finish(string finishtext);

        /// <summary>
        /// Dynamically changes the description of the control based on the remaining time.
        /// </summary>
        /// <param name="value">A function that receives the remaining time and returns the description to display. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ITimerControl ChangeDescription(Func<TimeSpan, string> value);

        /// <summary>
        /// Asynchronous version of <see cref="ChangeDescription(Func{TimeSpan, string})"/> that updates the
        /// description text according to the remaining time (useful when the text comes from an asynchronous source).
        /// </summary>
        /// <param name="value">A function that receives the remaining time and asynchronously returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ITimerControl ChangeDescriptionAsync(Func<TimeSpan, Task<string>> value);

        /// <summary>
        /// Sets whether the control displays the remaining time (countdown) or the elapsed time.
        /// The default is <see cref="TimerDisplayMode.Countdown"/>.
        /// </summary>
        /// <param name="mode">The <see cref="TimerDisplayMode"/> to use.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        ITimerControl DisplayMode(TimerDisplayMode mode);

        /// <summary>
        /// Displays an animated spinner next to the time value while the countdown is running.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The same <see cref="ITimerControl"/> instance for chaining.</returns>
        ITimerControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Displays the countdown and blocks until it completes or is aborted, returning the elapsed time.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the countdown while it is waiting.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> wrapping the elapsed <see cref="TimeSpan"/>.</returns>
        ResultPrompt<TimeSpan> Run(CancellationToken token = default);
    }
}
