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
    /// Represents the final state of a Task control execution.
    /// </summary>
    /// <param name="elapsedtime">Total elapsed execution time.</param>
    /// <param name="outputcontext">Isolated output context produced by the task.</param>
    /// <param name="error">Captured exception, if any.</param>
    public readonly struct StateTask(TimeSpan elapsedtime, IReadOnlyDictionary<string, object?>? outputcontext = null, Exception? error = null)
    {
        /// <summary>
        /// Gets the total elapsed time.
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
