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
    /// Provides a fluent API for configuring and running a MultiTasks control that executes
    /// several synchronous or asynchronous tasks (sequentially or in parallel), presenting a
    /// paginated execution list with waiting / running / success / failure status indicators.
    /// </summary>
    /// <remarks>
    /// Each task receives an isolated read-only input context and returns a separate isolated
    /// output context. Every configuration method returns the same <see cref="IMultiTasksControl"/>
    /// instance so the calls can be chained (fluent style). Call <see cref="Run(CancellationToken)"/>
    /// last to display the control and block until all tasks finish.
    /// </remarks>
    public interface IMultiTasksControl
    {
        /// <summary>
        /// Applies the shared control options (such as prompt message, tooltips and abort behavior).
        /// </summary>
        /// <param name="options">A callback used to configure the <see cref="IControlOptions"/>. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is <c>null</c>.</exception>
        IMultiTasksControl Options(Action<IControlOptions> options);

        /// <summary>
        /// Overrides the visual style applied to a specific region of the MultiTasks control.
        /// </summary>
        /// <param name="styleType">The <see cref="MultiTasksStyles"/> region whose style is overridden.</param>
        /// <param name="style">The <see cref="Style"/> to apply.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        IMultiTasksControl Styles(MultiTasksStyles styleType, Style style);

        /// <summary>
        /// Sets the culture used to format elapsed time values.
        /// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="culture"/> is <c>null</c>.</exception>
        IMultiTasksControl Culture(CultureInfo culture);

        /// <summary>
        /// Sets the default execution mode used by tasks that do not specify their own mode.
        /// Default is <see cref="MultiTasksMode.Sequential"/>.
        /// </summary>
        /// <remarks>
        /// Tasks execute strictly in the order they were added. Consecutive tasks that resolve to
        /// <see cref="MultiTasksMode.Parallel"/> form a sub-set that runs concurrently; the run only
        /// advances to the next task/sub-set once every item of the current sub-set has finished.
        /// A <see cref="MultiTasksMode.Sequential"/> task runs on its own before the next one starts.
        /// The list order is always preserved (tasks are never reordered/grouped globally by mode).
        /// </remarks>
        /// <param name="mode">The default <see cref="MultiTasksMode"/> to use.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        IMultiTasksControl Mode(MultiTasksMode mode);

        /// <summary>
        /// Shows the elapsed time next to each task. Enabled by default.
        /// </summary>
        /// <param name="value"><c>true</c> to display per-task elapsed time; otherwise, <c>false</c>.</param>
        /// <param name="format">Optional <see cref="TimeSpan"/> format string. Default is <c>hh\:mm\:ss</c>.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        IMultiTasksControl ShowElapsedTime(bool value = true, string? format = null);

        /// <summary>
        /// Displays an animated spinner in the summary line while at least one task is running.
        /// </summary>
        /// <param name="spinnersType">The <see cref="SpinnersType"/> to display.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        IMultiTasksControl Spinner(SpinnersType spinnersType);

        /// <summary>
        /// In sequential mode, stops the remaining tasks when a task fails. Ignored in parallel mode.
        /// </summary>
        /// <param name="value"><c>true</c> to stop on the first failure; otherwise, <c>false</c>.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        IMultiTasksControl StopOnError(bool value = true);

        /// <summary>
        /// Sets the maximum number of tasks that can run concurrently in <see cref="MultiTasksMode.Parallel"/> mode.
        /// The value is clamped to a sensible range based on the available CPU cores. Use <c>0</c> to
        /// auto-select a value derived from <see cref="Environment.ProcessorCount"/>.
        /// </summary>
        /// <param name="value">The maximum degree of parallelism, or <c>0</c> to auto-select.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        IMultiTasksControl MaxDegreeOfParallelism(int value);

        /// <summary>
        /// Sets the maximum number of visible task rows per page. A value of <c>0</c> auto-fits to the console height.
        /// </summary>
        /// <param name="value">The desired page size.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        IMultiTasksControl PageSize(byte value);

        /// <summary>
        /// Adds a synchronous task with the given title, receiving an isolated read-only input context
        /// and returning an isolated output context (or <c>null</c>).
        /// </summary>
        /// <param name="title">The task title displayed in the list. Cannot be <c>null</c>.</param>
        /// <param name="handler">The task callback. Cannot be <c>null</c>.</param>
        /// <param name="context">Optional isolated input context for this task.</param>
        /// <param name="mode">Optional execution mode for this task. When <c>null</c>, the default from <see cref="Mode(MultiTasksMode)"/> is used.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> or <paramref name="handler"/> is <c>null</c>.</exception>
        IMultiTasksControl AddTask(string title, Func<IReadOnlyDictionary<string, object?>, CancellationToken, IDictionary<string, object?>?> handler, IDictionary<string, object?>? context = null, MultiTasksMode? mode = null);

        /// <summary>
        /// Adds a simple synchronous task with the given title, without input or output context.
        /// </summary>
        /// <param name="title">The task title displayed in the list. Cannot be <c>null</c>.</param>
        /// <param name="handler">The task callback. Cannot be <c>null</c>.</param>
        /// <param name="mode">Optional execution mode for this task. When <c>null</c>, the default from <see cref="Mode(MultiTasksMode)"/> is used.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> or <paramref name="handler"/> is <c>null</c>.</exception>
        IMultiTasksControl AddTask(string title, Action<CancellationToken> handler, MultiTasksMode? mode = null);

        /// <summary>
        /// Adds an asynchronous task with the given title, receiving an isolated read-only input context
        /// and returning an isolated output context (or <c>null</c>).
        /// </summary>
        /// <param name="title">The task title displayed in the list. Cannot be <c>null</c>.</param>
        /// <param name="handler">The asynchronous task callback. Cannot be <c>null</c>.</param>
        /// <param name="context">Optional isolated input context for this task.</param>
        /// <param name="mode">Optional execution mode for this task. When <c>null</c>, the default from <see cref="Mode(MultiTasksMode)"/> is used.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> or <paramref name="handler"/> is <c>null</c>.</exception>
        IMultiTasksControl AddTaskAsync(string title, Func<IReadOnlyDictionary<string, object?>, CancellationToken, Task<IDictionary<string, object?>?>> handler, IDictionary<string, object?>? context = null, MultiTasksMode? mode = null);

        /// <summary>
        /// Adds a simple asynchronous task with the given title, without input or output context.
        /// </summary>
        /// <param name="title">The task title displayed in the list. Cannot be <c>null</c>.</param>
        /// <param name="handler">The asynchronous task callback. Cannot be <c>null</c>.</param>
        /// <param name="mode">Optional execution mode for this task. When <c>null</c>, the default from <see cref="Mode(MultiTasksMode)"/> is used.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="title"/> or <paramref name="handler"/> is <c>null</c>.</exception>
        IMultiTasksControl AddTaskAsync(string title, Func<CancellationToken, Task> handler, MultiTasksMode? mode = null);

        /// <summary>
        /// Iterates a collection and lets the caller register one or more tasks per item, enabling more
        /// complex scenarios (mirrors the Interaction pattern used by other controls).
        /// </summary>
        /// <typeparam name="T">The item type.</typeparam>
        /// <param name="items">The items to iterate. Cannot be <c>null</c>.</param>
        /// <param name="interactionAction">A callback receiving each item and this control to register tasks. Cannot be <c>null</c>.</param>
        /// <returns>The same <see cref="IMultiTasksControl"/> instance for chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> or <paramref name="interactionAction"/> is <c>null</c>.</exception>
        IMultiTasksControl Interaction<T>(IEnumerable<T> items, Action<T, IMultiTasksControl> interactionAction);

        /// <summary>
        /// Executes all tasks, blocks until they complete or the run is aborted, and returns the final state.
        /// </summary>
        /// <param name="token">A <see cref="CancellationToken"/> used to cancel the run while it is executing.</param>
        /// <returns>A <see cref="ResultPrompt{T}"/> containing the final <see cref="StateMultiTasks"/>.</returns>
        ResultPrompt<StateMultiTasks> Run(CancellationToken token = default);
    }
}
