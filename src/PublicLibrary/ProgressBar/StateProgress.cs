// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents the state of a Progress Bar.
    /// </summary>
    /// <param name="value">Last value of Progress Bar.</param>
    /// <param name="valuetext">Finished text of Progress Bar.</param>
    /// <param name="minvalue">Min value of Progress Bar.</param>
    /// <param name="maxvalue">Max value of Progress Bar.</param>
    /// <param name="elapsedtime">Elapsed Time of Progress Bar.</param>
    /// <param name="resultcontext">Result context of Progress Bar.</param>
    /// <param name="error">Exception of Progress Bar.</param>
    public readonly struct StateProgress(double value, string? valuetext, double minvalue, double maxvalue, TimeSpan elapsedtime, KeyValuePair<string, object?>[]? resultcontext = null, Exception? error = null)
    {
        /// <summary>
        /// Last value of Progress Bar
        /// </summary>
        public double? FinishedValue { get; } = value;

        /// <summary>
        /// Finished text of Progress Bar
        /// </summary>
        public string? FinishedText { get; } = valuetext;

        /// <summary>
        /// Min value of Progress Bar
        /// </summary>
        public double MinValue { get; } = minvalue;

        /// <summary>
        /// Max value of Progress Bar
        /// </summary>
        public double MaxValue { get; } = maxvalue;

        /// <summary>
        /// Exception of Progress Bar
        /// </summary>
        public Exception? ExceptionProgress { get; } = error;

        /// <summary>
        /// Elapsed Time of Progress Bar
        /// </summary>
        public TimeSpan ElapsedTime { get; } = elapsedtime;

        /// <summary>
        /// Result values for context.
        /// </summary>
        public KeyValuePair<string, object?>[]? OutputContext { get; } = resultcontext;

        /// <summary>
        /// Retrieves the value of a context parameter identified by the specified key and casts it to the specified
        /// type.
        /// </summary>
        /// <remarks>Use this method to retrieve strongly typed context parameters. If the parameter
        /// exists but is not of the requested type, a <see cref="KeyNotFoundException"/> is thrown.</remarks>
        /// <typeparam name="T">
        /// The type to which the parameter value will be cast. Must match the actual type of the stored parameter value.
        /// </typeparam>
        /// <param name="key">The key that identifies the context parameter to retrieve. Cannot be null.</param>
        /// <returns>The value of the context parameter associated with the specified key, cast to type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the context parameter collection is not available.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if a parameter with the specified key does not exist or cannot be cast to type <typeparamref name="T"/>.
        /// </exception>
        public T GetOutput<T>(string? key = null)
        {
            if (OutputContext == null)
            {
                throw new InvalidOperationException("No context parameters available.");
            }

            foreach (var param in OutputContext)
            {
                if ((string.IsNullOrEmpty(key) || param.Key == key) && param.Value is T value)
                {
                    return value;
                }
            }
            throw new KeyNotFoundException($"Parameter '{key??string.Empty}' not found or is not of type {typeof(T)}.");
        }

    }
}
