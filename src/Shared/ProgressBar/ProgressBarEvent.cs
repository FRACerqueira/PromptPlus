// ***************************************************************************************
// MIT LICENCE
// The maintenance and evolution is maintained by the PromptPlus project under MIT license
// ***************************************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace PromptPlusLibrary
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Represents the mutable state used by ProgressBar update callbacks.
    /// </summary>
    public sealed class ProgressBarEvent
    {
        private double? _lastvalue;
        private double _currentvalue;
        private readonly double _min;
        private readonly double _max;
        private Exception? _error;
        private bool _aborted;
        private readonly IDictionary<string, object?>? _paramcontext;
        private readonly Dictionary<string, object?> _outputcontext = [];
            

        /// <summary>
        /// Initializes a new <see cref="ProgressBarEvent"/> instance.
        /// </summary>
        /// <param name="value">Initial progress value.</param>
        /// <param name="min">Minimum allowed progress value.</param>
        /// <param name="max">Maximum allowed progress value.</param>
        /// <param name="paramcontext">Optional input context available to the callback.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="min"/> is greater than or equal to <paramref name="max"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is outside the <paramref name="min"/>/<paramref name="max"/> range.</exception>
        public ProgressBarEvent(double value, double min, double max, IDictionary<string, object?>? paramcontext = null)
        {
            if (min >= max)
            {
                throw new ArgumentException("The minimum value must be less than the maximum value.", nameof(min));
            }

            if (value < min || value > max)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The initial value must be within the min/max range.");
            }

            _lastvalue = null;
            _currentvalue = value;
            _min = min;
            _max = max;
            _paramcontext = paramcontext;
        }

        /// <summary>
        /// Gets the maximum progress value.
        /// </summary>
        public double Maxvalue => _max;

        /// <summary>
        /// Gets the minimum progress value.
        /// </summary>
        public double Minvalue => _min;

        /// <summary>
        /// Gets the current progress value.
        /// </summary>
        public double Value => _currentvalue;

        /// <summary>
        /// Gets the output context produced during handler execution.
        /// </summary>
        public ReadOnlyDictionary<string, object?> OutputContext => new(_outputcontext);

        /// <summary>
        /// Tries to read an input context value and cast it to <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Expected value type.</typeparam>
        /// <param name="key">Input context key.</param>
        /// <param name="found"><c>true</c> when the key exists and the value matches <typeparamref name="T"/>; otherwise, <c>false</c>.</param>
        /// <returns>The typed value when found; otherwise, <c>default</c>.</returns>
        public T InputParam<T>(string key, out bool found)
        {
            found = false;
            if (_paramcontext == null || !_paramcontext.TryGetValue(key, out var rawValue))
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

        /// <summary>
        /// Adds or updates an output context entry.
        /// </summary>
        /// <param name="key">Output context key.</param>
        /// <param name="value">Value associated with <paramref name="key"/>.</param>
        public void AddOutputContext<T>(string key, T value)
        {
            _outputcontext[key] = value;
        }

        /// <summary>
        /// Removes an output context entry by key.
        /// </summary>
        /// <param name="key">Output context key.</param>
        public void RemoveOutputContext(string key)
        {
            _outputcontext.Remove(key);
        }

        /// <summary>
        /// Gets whether processing has completed (aborted or reached max value).
        /// </summary>
        public bool Finish => _aborted || _currentvalue >= Maxvalue;

        /// <summary>
        /// Gets the error that caused an abort, if any.
        /// </summary>
        public Exception? Error => _error;

        /// <summary>
        /// Stores an error and aborts further processing.
        /// </summary>
        /// <param name="error">Associated error. Can be <c>null</c>.</param>
        public void ErrorAndAbort(Exception? error)
        {
            _error = error;
            _aborted = true;
        }

        /// <summary>
        /// Updates the current value, clamped to the configured range.
        /// </summary>
        /// <param name="value">New progress value.</param>
        public void Update(double value)
        {
            _currentvalue = Math.Clamp(value, Minvalue, Maxvalue);
        }

        /// <summary>
        /// Indicates whether state changed since the previous check.
        /// </summary>
        /// <returns><c>true</c> when aborted or when the value changed; otherwise, <c>false</c>.</returns>
        public bool HasChange()
        {
            if (_aborted)
            {
                return true;
            }

            if (_lastvalue != _currentvalue)
            {
                _lastvalue = _currentvalue;
                return true;
            }

            return false;
        }

        internal void Abort()
        {
            _aborted = true;
        }
    }
}
