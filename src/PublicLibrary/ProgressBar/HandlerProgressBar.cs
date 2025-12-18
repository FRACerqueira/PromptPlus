// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;

namespace PromptPlusLibrary
{
    /// <summary>
    /// Represents a progress bar handler.
    /// </summary>
    public sealed class HandlerProgressBar
    {
        private double? _lastvalue;
        private double _currentvalue;
        private readonly double _min;
        private readonly double _max;
        private Exception? _error;
        private bool _aborted;
        private KeyValuePair<string, object?>[]? _paramcontext;
        private List<KeyValuePair<string, object?>> _outputcontext = [];


#pragma warning disable IDE0290 // Use primary constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerProgressBar"/> class with the specified value, minimum, and maximum values.
        /// </summary>
        /// <param name="value">The initial value.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="paramcontext">The context parameters to pass to the handler.</param>
        /// <exception cref="ArgumentException">Thrown when min is greater than or equal to max.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when value is outside the range [min, max].</exception>
        public HandlerProgressBar(double value, double min, double max, KeyValuePair<string, object?>[]? paramcontext)
        {
            // Initialize to a value outside the range to ensure HasChange works correctly.
            _lastvalue = value * -1;
            _currentvalue = value;
            _min = min;
            _max = max;
            _paramcontext = paramcontext;
        }
#pragma warning restore IDE0290 // Use primary constructor

        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        public double Maxvalue => _max;

        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        public double Minvalue => _min;

        /// <summary>
        /// Gets the maximum value of the progress bar.
        /// </summary>
        public double Value => _currentvalue;

        /// <summary>
        /// Gets a read-only collection of key-value pairs representing the output context for the current operation.
        /// </summary>
        public KeyValuePair<string, object?>[] OutputContext => [.. _outputcontext];

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
        public T GetParam<T>(string? key = null)
        {
            if (_paramcontext == null)
            {
                throw new InvalidOperationException("No context parameters available.");
            }

            foreach (var param in _paramcontext)
            {
                if ((string.IsNullOrEmpty(key) || param.Key == key) && param.Value is T value)
                {
                    return value;
                }
            }
            throw new KeyNotFoundException($"Parameter '{key ?? string.Empty}' not found or is not of type {typeof(T)}.");
        }

        /// <summary>
        /// Adds/update a key and associated value to the output context collection.
        /// </summary>
        /// <param name="key">The key that identifies the output context entry. Cannot be null.</param>
        /// <param name="value">The value to associate with the specified key. May be null.</param>
        public void SetOutputContext(string key, object? value)
        {
            var index = _outputcontext.FindIndex(kv => kv.Key == key);
            if (index >= 0)
            {
                _outputcontext[index] = new KeyValuePair<string, object?>(key, value);
            }
            else
            {
                _outputcontext.Add(new KeyValuePair<string, object?>(key, value));
            }
        }

        /// <summary>
        /// Removes the output context associated with the specified key, if it exists.
        /// </summary>
        /// <param name="key">The key of the output context to remove. Cannot be null.</param>
        public void RemoveOutputContext(string key)
        {
            var index = _outputcontext.FindIndex(kv => kv.Key == key);
            if (index >= 0)
            {
                _outputcontext.RemoveAt(index);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the progress bar has finished.
        /// </summary>
        public bool Finish => _aborted || _currentvalue >= Maxvalue;

        /// <summary>
        /// Gets or sets the error associated with the progress bar.
        /// </summary>
        public Exception? Error => _error;

        /// <summary>
        /// Sets the error associated with the progress bar.
        /// </summary>
        /// <param name="error"></param>
        public void ErrorAndAbort(Exception? error)
        {
            _error = error;
            _aborted = true;
        }

        /// <summary>
        /// Updates the current value of the progress bar.
        /// </summary>
        /// <param name="value">The new current value.</param>
        public void Update(double value)
        {
            if (value > Maxvalue)
            {
                value = Maxvalue;
            }
            if (value < Minvalue)
            {
                value = Minvalue;
            }
            _currentvalue = value;
        }

        /// <summary>
        /// Checks if the value has changed since the last update.
        /// </summary>
        /// <returns><c>true</c> if the value has changed; otherwise, <c>false</c>.</returns>
        public bool HasChange()
        {
            bool result = _lastvalue != Value;
            if (result)
            {
                _lastvalue = Value;
            }
            if (_aborted)
            {
                result = true;
            }
            return result;
        }

        internal void Abort()
        {
            _aborted = true;
        }
    }
}
