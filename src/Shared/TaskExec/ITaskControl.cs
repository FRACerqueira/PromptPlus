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
    /// Provides a fluent API for configuring and running a Task control that executes a
    /// synchronous or asynchronous action/function and waits for it to complete, optionally
    /// displaying the elapsed time and an animated spinner.
    /// </summary>
    /// <remarks>
    /// The task receives an isolated input context (<see cref="IDictionary{TKey, TValue}"/>) and
    /// produces a separate isolated output context. Both dictionaries are independent from each
    /// other. Every configuration method returns the same <see cref="ITaskControl"/> instance so
    /// the calls can be chained (fluent style). Call <see cref="Run(CancellationToken)"/> last to
    /// display the control and block until the task finishes.
    /// </remarks>
    public interface ITaskControl
    {
        /// <summary>
        /// Applies the shared control options (such as prompt message, tooltips and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        ITaskControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the Task control.
        /// </summary>
        /// <param name="styleType">The <see cref="TaskStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        ITaskControl Styles(TaskStyles styleType, Style style);

        /// <summary>
        /// Sets the culture used to format the elapsed time value.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        ITaskControl Culture(CultureInfo culture);

        /// <summary>
        /// Shows the elapsed time while the task is running. Hidden by default.
        /// </summary>
        /// <param name="value"><c>true</c> to display the elapsed time; otherwise, <c>false</c>.</param>
        /// <param name="format">Optional <see cref="TimeSpan"/> format string. Default is <c>hh\:mm\:ss</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        ITaskControl ShowElapsedTime(bool value = true, string? format = null);

        /// <summary>
        /// Displays an animated spinner while the task is running.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        ITaskControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Sets the text displayed when the task finishes. When not set, the elapsed time is shown.
        /// </summary>
        /// <param name="finishtext">The text to display when the task finishes successfully.</param>
        /// <param name="errortext">
        /// Optional text to display when the task finishes with an error. When <c>null</c>, a default
        /// localized error message is shown.
        /// </param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        ITaskControl Finish(string finishtext, string? errortext = null);

        /// <summary>
        /// Dynamically changes the description of the control based on the elapsed time while the task runs.
        /// </summary>
        /// <param name="value">A function that receives the elapsed time and returns the description to display. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ITaskControl ChangeDescription(Func<TimeSpan, string> value);

        /// <summary>
        /// Asynchronous version of <see cref="ChangeDescription(Func{TimeSpan, string})"/> that updates the
        /// description text according to the elapsed time (useful when the text comes from an asynchronous source).
        /// </summary>
        /// <param name="value">A function that receives the elapsed time and asynchronously returns the description. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <c>null</c>.</exception>
        ITaskControl ChangeDescriptionAsync(Func<TimeSpan, Task<string>> value);

        /// <summary>
        /// Provides the isolated input context passed to the task handler.
        /// </summary>
        /// <param name="context">The input context dictionary. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <c>null</c>.</exception>
        ITaskControl Context(IDictionary<string, object?> context);

        /// <summary>
        /// Sets the synchronous action to execute. The action receives an isolated read-only input
        /// context and returns an isolated output context (or <c>null</c>) that is exposed through
        /// <see cref="StateTask.OutputContext"/>.
        /// </summary>
        /// <param name="handler">
        /// A callback receiving the input context and a <see cref="CancellationToken"/> and returning
        /// the output context. Cannot be <c>null</c>.
        /// </param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        ITaskControl Action(Func<IReadOnlyDictionary<string, object?>, CancellationToken, IDictionary<string, object?>?> handler);

        /// <summary>
        /// Sets the synchronous action to execute without an input context. Returns an isolated
        /// output context (or <c>null</c>) that is exposed through <see cref="StateTask.OutputContext"/>.
        /// </summary>
        /// <param name="handler">
        /// A callback receiving a <see cref="CancellationToken"/> and returning the output context. Cannot be <c>null</c>.
        /// </param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        ITaskControl Action(Func<CancellationToken, IDictionary<string, object?>?> handler);

        /// <summary>
        /// Sets a simple synchronous action to execute without input or output context.
        /// </summary>
        /// <param name="handler">A callback receiving a <see cref="CancellationToken"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        ITaskControl Action(Action<CancellationToken> handler);

        /// <summary>
        /// Sets the asynchronous function to execute. The function receives an isolated read-only input
        /// context and returns an isolated output context (or <c>null</c>) that is exposed through
        /// <see cref="StateTask.OutputContext"/>.
        /// </summary>
        /// <param name="handler">
        /// A callback receiving the input context and a <see cref="CancellationToken"/> and returning a
        /// <see cref="Task{TResult}"/> with the output context. Cannot be <c>null</c>.
        /// </param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        ITaskControl ActionAsync(Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler);

        /// <summary>
        /// Sets the asynchronous function to execute without an input context. Returns an isolated
        /// output context (or <c>null</c>) that is exposed through <see cref="StateTask.OutputContext"/>.
        /// </summary>
        /// <param name="handler">
        /// A callback receiving a <see cref="CancellationToken"/> and returning a <see cref="Task{TResult}"/>
        /// with the output context. Cannot be <c>null</c>.
        /// </param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        ITaskControl ActionAsync(Func<CancellationToken, Task<IDictionary<string, object?>?>> handler);

        /// <summary>
        /// Sets a simple asynchronous function to execute without input or output context.
        /// </summary>
        /// <param name="handler">A callback receiving a <see cref="CancellationToken"/> and returning a <see cref="Task"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="ITaskControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        ITaskControl ActionAsync(Func<CancellationToken, Task> handler);

        /// <summary>
        /// Executes the task, blocks until it completes or is aborted, and returns its final state.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the task while it is running.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the final <see cref="StateTask"/>.</returns>
        ResultPrompt<StateTask> Run(CancellationToken token = default);
    }
}
