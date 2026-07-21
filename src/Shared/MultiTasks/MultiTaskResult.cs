// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the final result of a single task within the MultiTasks control.
    /// </summary>
    /// <param name="title">The task title.</param>
    /// <param name="state">The final <see cref="MultiTaskState"/>.</param>
    /// <param name="elapsedtime">The task elapsed execution time.</param>
    /// <param name="outputcontext">The isolated output context produced by the task.</param>
    /// <param name="error">The captured exception, if any.</param>
    public readonly struct MultiTaskResult(string title, MultiTaskState state, TimeSpan elapsedtime, IReadOnlyDictionary<string, object?>? outputcontext = null, Exception? error = null)
    {
        /// <summary>
        /// Gets the task title.
        /// </summary>
        public string Title { get; } = title;

        /// <summary>
        /// Gets the final task state.
        /// </summary>
        public MultiTaskState State { get; } = state;

        /// <summary>
        /// Gets the task elapsed execution time.
        /// </summary>
        public TimeSpan ElapsedTime { get; } = elapsedtime;

        /// <summary>
        /// Gets the isolated output context produced by the task.
        /// </summary>
        public IReadOnlyDictionary<string, object?>? OutputContext { get; } = outputcontext;

        /// <summary>
        /// Gets the captured exception, if one occurred during execution.
        /// </summary>
        public Exception? Exception { get; } = error;

        /// <summary>
        /// Tries to read an output context value and cast it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected value type.</typeparam>
        /// <param name="key">Output context key.</param>
        /// <param name="found"><c>true</c> when the key exists and the value matches <typeparamref name="T"/>; otherwise, <c>false</c>.</param>
        /// <returns>The typed value when found; otherwise, <c>default</c>.</returns>
        public T? GetOutput<T>(string key, out bool found)
        {
            found = false;
            if (OutputContext == null || !OutputContext.TryGetValue(key, out var rawValue))
            {
                return default;
            }
            if (rawValue is T typed)
            {
                found = true;
                return typed;
            }
            return default;
        }
    }
}
