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
    /// Represents the final state of a Progress Bar control execution.
    /// </summary>
    /// <param name="value">Final numeric value.</param>
    /// <param name="valuetext">Final display text.</param>
    /// <param name="minvalue">Configured minimum value.</param>
    /// <param name="maxvalue">Configured maximum value.</param>
    /// <param name="elapsedtime">Total elapsed execution time.</param>
    /// <param name="resultcontext">Optional output context values.</param>
    /// <param name="error">Captured exception, if any.</param>
    public readonly struct StateProgress(double value, string? valuetext, double minvalue, double maxvalue, TimeSpan elapsedtime, IReadOnlyDictionary<string, object?>? resultcontext = null, Exception? error = null)
    {
        /// <summary>
        /// Gets the final numeric value.
        /// </summary>
        public double? FinishedValue { get; } = value;

        /// <summary>
        /// Gets the final display text.
        /// </summary>
        public string? FinishedText { get; } = valuetext;

        /// <summary>
        /// Gets the configured minimum value.
        /// </summary>
        public double MinValue { get; } = minvalue;

        /// <summary>
        /// Gets the configured maximum value.
        /// </summary>
        public double MaxValue { get; } = maxvalue;

        /// <summary>
        /// Gets the captured exception, if one occurred.
        /// </summary>
        public Exception? ExceptionProgress { get; } = error;

        /// <summary>
        /// Gets the total elapsed time.
        /// </summary>
        public TimeSpan ElapsedTime { get; } = elapsedtime;

        /// <summary>
        /// Gets optional output context values.
        /// </summary>
        public IReadOnlyDictionary<string, object?>? OutputContext { get; } = resultcontext;

        /// <summary>
        /// Tries to read an output context value and cast it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected value type.</typeparam>
        /// <param name="key">Output context key.</param>
        /// <param name="found"><c>true</c> when the key exists and the value matches <typeparamref name="T"/>; otherwise, <c>false</c>.</param>
        /// <returns>The typed value when found; otherwise, <c>default</c>.</returns>
        public T GetOutput<T>(string key, out bool found)
        {
            found = false;
            if (OutputContext == null || !OutputContext.TryGetValue(key, out var rawValue))
            {
                return default!;
            }

            if (rawValue is T value)
            {
                found = true;
                return value;
            }

            return default!;
        }
    }
}
