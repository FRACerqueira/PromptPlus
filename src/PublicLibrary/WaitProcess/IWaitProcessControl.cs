// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Threading;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Provides functionality for configuring and interacting with a Wait Process control.
    /// </summary>
    public interface IWaitProcessControl
    {

        /// <summary>
        /// Applies custom options to the control.
        /// </summary>
        /// <param name="options">An action to configure <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="options"/> is <c>null</c>.</exception>
        IWaitProcessControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overwrites styles for the Wait Process.
        /// </summary>
        /// <param name="styleType">The <see cref="WaitProcessStyles"/> to apply.</param>
        /// <param name="style">The <see cref="Style"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="style"/> is <c>null</c>.</exception>
        IWaitProcessControl Styles(WaitProcessStyles styleType, Style style);

        /// <summary>
        /// Shows a <see cref="SpinnersType"/> animation at the end of the prompt.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        IWaitProcessControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// Defines the finish answer to display when the Wait process is completed.
        /// </summary>
        /// <param name="finishtext">A function that takes the process states and returns the finish answer text. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="finishtext"/> is <c>null</c>.</exception>
        IWaitProcessControl Finish(Func<IEnumerable<StateProcess>, string> finishtext);

        /// <summary>
        /// Defines whether to show the elapsed time. Default is <c>true</c>.
        /// </summary>
        /// <param name="value">If <c>true</c>, shows the elapsed time; otherwise, hides it.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        IWaitProcessControl ShowElapsedTime(bool value = true);

        /// <summary>
        /// Defines the interval to update task status and spinner. Default is 100ms.
        /// </summary>
        /// <param name="mileseconds">The interval in milliseconds. Must be between 100 and 1000.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="mileseconds"/> is less than 100 or greater than 1000.</exception>
        IWaitProcessControl IntervalUpdate(int mileseconds = 100);

        /// <summary>
        /// Defines the maximum number of concurrent tasks enabled. The default value is the smaller number between the number of processors and 5.
        /// </summary>
        /// <param name="value">The number of concurrent tasks. Must be between 1 and 5.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is less than 1 or greater than 5.</exception>
        IWaitProcessControl MaxDegreeProcess(byte value);

        /// <summary>
        /// Performs an interaction with each item in the collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="items">The collection of items. Cannot be <c>null</c>.</param>
        /// <param name="interactionaction">The action to perform for each item. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="items"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="interactionaction"/> is <c>null</c>.</exception>
        IWaitProcessControl Interaction<T>(IEnumerable<T> items, Action<T, IWaitProcessControl> interactionaction);

        /// <summary>
        /// Dynamically changes the description using a user-defined function.
        /// </summary>
        /// <param name="value">A function that takes the current process states and returns the updated description. Cannot be <c>null</c>.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        IWaitProcessControl ChangeDescription(Func<IEnumerable<StateProcess>, string> value);

        /// <summary>
        /// Adds a task with a parameter to execute.
        /// </summary>
        /// <param name="mode">The task execution mode.</param>
        /// <param name="id">The unique identifier of the task. Cannot be <c>null</c>.</param>
        /// <param name="process">The action to execute for the task. Cannot be <c>null</c>.</param>
        /// <param name="label">The optional label for the task.</param>
        /// <param name="parameter">The optional parameter to pass to the task.</param>
        /// <returns>The current <see cref="IWaitProcessControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="id"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="process"/> is <c>null</c>.</exception>
        IWaitProcessControl AddTask(TaskMode mode, string id, Action<object?, CancellationToken> process, string? label = null, object? parameter = null);

        /// <summary>
        /// Runs the Wait process control and returns the result.
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/> to observe while waiting for the task to complete. Defaults to <see cref="CancellationToken.None"/>.</param>
        /// <returns>The result with type <see cref="ResultPrompt{T}"/> of the <see cref="StateProcess"/> execution.</returns>
        ResultPrompt<StateProcess[]> Run(CancellationToken token = default);
    }
}